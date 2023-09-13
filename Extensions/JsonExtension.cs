using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public static class JsonExtension
    {
        public static T Parse<T>(this string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }
}
