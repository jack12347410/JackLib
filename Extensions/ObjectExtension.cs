using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace JackLib
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 將物件內的 Field 輸出成文字串
        /// </summary>
        /// <param name="obj">物件</param>
        /// <returns>Field 輸出成文字串</returns>
        public static string FieldPrint(this object obj)
        {
            if (obj == null) return string.Empty;

            var fieldInfos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var sb = new StringBuilder("{");
            foreach (var field in fieldInfos)
            {
                if (field.IsPublic || field.IsAssembly)
                {
                    var value = field.GetValue(obj) ?? "NULL";
                    if (!(field.FieldType.IsArray || value is Array || (!(value is string) && value is IEnumerable)))
                    {
                        sb.Append($"[{field.Name}: {value.ToString()}] ");
                    }
                }
            }
            sb.Append("}");

            return sb.ToString();
        }
    }
}