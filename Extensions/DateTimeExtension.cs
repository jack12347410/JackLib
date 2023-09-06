using System;
using System.Globalization;

namespace JackLib
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 使用指定的格式和'與文化特性無關'的格式資訊，將目前 System.DateTime 物件的值，轉換為其相等的字串表示。
        /// </summary>
        /// <param name="format">標準或自訂的日期和時間格式字串。</param>
        /// <returns>前 System.DateTime 物件值的字串表示，如 format 及 provider所指定。</returns>
        public static string ToStringRaw(this DateTime source, string format)
        {
            return source.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}