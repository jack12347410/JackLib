using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public class SqlCommandDto
    {
        public string Query { get; set; }
        public DynamicParameters Paras { get; set; }

        public SqlCommandDto(string query, DynamicParameters paras)
        {
            Query = query;
            Paras = paras;
        }
    }
}
