using System;
using System.ComponentModel;
using System.Linq;

namespace JackLib
{
    public static class EnumExtension
    {
        /// <summary>
        /// 該值是否不存在於指定列舉內的常數。
        /// </summary>
        /// <returns>true, 不存在;false, 存在</returns>
        public static bool IsNotDefined<TEnum>(this TEnum enumValue) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            var type = typeof(TEnum);
            if (!type.IsEnum) throw new ArgumentException($"{nameof(TEnum)} 必須是 Enum Type。");

            return !Enum.IsDefined(type, enumValue);
        }

        /// <summary>
        /// 取得該enum所定義的Description
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="enumType">typeof(enum)</param>
        /// <returns></returns>
        public static string GetDescription<TEnum>(this TEnum value)
        {
            var type = typeof(TEnum);
            var name = Enum.GetNames(type)
                            .Where(f => f.Equals(value.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            .Select(d => d)
                            .FirstOrDefault();

            //// 找無相對應的列舉
            if (name == null)
            {
                return string.Empty;
            }

            //// 利用反射找出相對應的欄位
            var field = type.GetField(name);
            //// 取得欄位設定DescriptionAttribute的值
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            //// 無設定Description Attribute, 回傳Enum欄位名稱
            if (customAttribute == null || customAttribute.Length == 0)
            {
                return name;
            }

            //// 回傳Description Attribute的設定
            return ((DescriptionAttribute)customAttribute[0]).Description;
        }
    }
}