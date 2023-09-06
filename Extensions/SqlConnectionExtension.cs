using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace JackLib
{
    public static class SqlConnectionExtension
    {
        public static void TryOpen(this SqlConnection sqlConnection, WarnRequest errorDoAgain)
        {
            if (sqlConnection == null) throw new ArgumentNullException(nameof(sqlConnection));
            if (sqlConnection.State == ConnectionState.Open) throw new InvalidOperationException($"The '{nameof(sqlConnection)}' is already open.");

            RetryUtil.Retry(() =>
            {
                sqlConnection.Open();
            }, errorDoAgain);
        }

        public static IEnumerable<T>? TryOpenAndQuery<T>(this SqlConnection sqlConnection, string sql, object param, WarnRequest errorDoAgain)
        {
            if (sqlConnection == null) throw new ArgumentNullException(nameof(sqlConnection));

            return RetryUtil.Retry(() =>
            {
                if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
                return sqlConnection.Query<T>(sql, param);

            }, errorDoAgain);
        }


        public static object? TryOpenAndExecuteScalar(this SqlConnection sqlConnection, string sql, object param, WarnRequest errorDoAgain)
        {
            if (sqlConnection == null) throw new ArgumentNullException(nameof(sqlConnection));

            return RetryUtil.Retry(() =>
            {
                if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
                return sqlConnection.ExecuteScalar(sql, param);
            }, errorDoAgain);

        }

        /// <summary>
        /// 同query多/單筆資料
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="errorDoAgain"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void TryTransaction(this SqlConnection sqlConnection, string sql, object param, WarnRequest errorDoAgain)
        {
            if (sqlConnection == null) throw new ArgumentNullException(nameof(sqlConnection));

            RetryUtil.Retry(() =>
            {
                if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
                using (var trans = sqlConnection.BeginTransaction(IsolationLevel.Serializable))
                {
                    sqlConnection.Execute(sql, param, trans);

                    trans.Commit();
                    return;
                }
            }, errorDoAgain);
        }

        /// <summary>
        /// 不同query多筆資料
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="queryParas"></param>
        /// <param name="errorDoAgain"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void TryTransaction(this SqlConnection sqlConnection, IEnumerable<SqlCommandDto> queryParas, WarnRequest errorDoAgain)
        {
            if (sqlConnection == null) throw new ArgumentNullException(nameof(sqlConnection));

            RetryUtil.Retry(() =>
            {
                if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
                using (var trans = sqlConnection.BeginTransaction(IsolationLevel.Serializable))
                {
                    foreach (var qp in queryParas)
                    {
                        sqlConnection.Execute(qp.Query, qp.Paras, trans);
                    }

                    trans.Commit();
                    return;
                }
            }, errorDoAgain);
        }
    }
}