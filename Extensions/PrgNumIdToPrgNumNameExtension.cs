using System.Globalization;
using System.Text.RegularExpressions;

namespace JackLib
{
    public static class PrgNumIdToPrgNumNameExtension
    {
        /// <summary>
        /// 將指定的 PrgNumId 轉成對應 Oxxxx 格式
        /// </summary>
        /// <param name="prgNumId">將指定的 PrgNumId</param>
        /// <returns>Oxxxx 格式</returns>
        public static string ToPrgNumName(this int prgNumId)
        {
            return prgNumId.ToString(@"\O0000", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 將指定的 PrgNumId 轉成對應 Oxxxx 格式
        /// </summary>
        /// <param name="prgNumId">將指定的 PrgNumId</param>
        /// <returns>Oxxxx 格式</returns>
        public static string ToPrgNumName(this short prgNumId)
        {
            return prgNumId.ToString(@"\O0000", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 判斷是否為O加工程式碼
        /// </summary>
        /// <param name="prgName"></param>
        /// <returns></returns>
        public static bool IsProgramO(string source)
        {
            return Regex.IsMatch(source, @"[O][0-9][0-9][0-9][0-9]");
        }
    }
}