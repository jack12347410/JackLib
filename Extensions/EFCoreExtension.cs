using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public static class EFCoreExtension
    {
        public static Task<IEnumerable<T>> DapperQueryAsync<T>(this DatabaseFacade database, string sql, object param, WarnRequest errorDoAgain = default)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));

            var conn = database.GetDbConnection();
            return RetryUtil.Retry(async () =>
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                return await conn.QueryAsync<T>(sql, param);

            }, errorDoAgain);
        }

        public static Task<object> DapperExecuteScalarAsync(this DatabaseFacade database, string sql, object param, WarnRequest errorDoAgain = default)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));

            var conn = database.GetDbConnection();
            return RetryUtil.Retry(async () =>
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                return await conn.ExecuteScalarAsync(sql, param);
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
        public static Task<int> DapperTransactionAsync(this DatabaseFacade database, string sql, object param, WarnRequest errorDoAgain = default)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));

            var conn = database.GetDbConnection();
            return RetryUtil.Retry(async () =>
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (var trans = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    var result = await conn.ExecuteAsync(sql, param, trans);
                    trans.Commit();

                    return result;
                }
            }, errorDoAgain);
        }

        /// <summary>
        /// 不同query多筆資料
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="errorDoAgain"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task DapperTransactionAsync(this DatabaseFacade database, IEnumerable<SqlCommandDto> queryParas, WarnRequest errorDoAgain = default)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));

            var conn = database.GetDbConnection();

            return RetryUtil.Retry(async () =>
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (var trans = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    foreach (var qp in queryParas)
                    {
                        await conn.ExecuteAsync(qp.Query, qp.Paras, trans);
                    }

                    trans.Commit();
                    return;
                }
            }, errorDoAgain);
        }
    }
}
