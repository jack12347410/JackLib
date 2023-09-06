using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public class DapperTypeHandlerExtension<T> : SqlMapper.TypeHandler<T>
    {
        //將 json string 從 db 取出時做轉型
        public override T Parse(object value)
        {
            //將 DB 的 json string 內容轉為目標的型別
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }
        //將值存回 db 時由 object 轉為 json
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = (value == null) ? (object)DBNull.Value : JsonConvert.SerializeObject(value);
            parameter.DbType = DbType.String;
        }
    }
}
