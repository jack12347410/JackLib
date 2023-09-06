using System;
using System.Collections.Generic;
using System.Linq;

namespace JackLib
{
    public static class LinqExtension
    {
        /// <summary>
        /// 陣列是否為 NULL 或 空陣列
        /// </summary>
        /// <returns>是否為 NULL 或 空陣列。true, 是;false, 否</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || (!source.Any());
        }

        /// <summary>
        /// ForEach
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// 提取指定index(含)前的陣列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="lastIndex"></param>
        /// <returns></returns>
        public static T[] Pop<T>(this Queue<T> source, int lastIndex)
        {
            T[] result = new T[lastIndex + 1];
            for (int i = 0; i <= lastIndex; i++)
            {
                result[i] = source.Dequeue();
            }

            return result;
        }

        /// <summary>
        /// 陣列全設定為 指定的值
        /// </summary>
        /// <param name="value">指定的值</param>
        public static void Populate<T>(this IList<T> source, T value)
        {
            if (source == null) throw new ArgumentNullException("source");

            for (int i = source.Count() - 1; i >= 0; --i)
            {
                source[i] = value;
            }
        }

        /// <summary>
        /// 是否皆符合指定的條件
        /// </summary>
        /// <param name="predicate">指定的條件</param>
        /// <returns>true, 皆符合;false, 則反之</returns>
        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (TSource current in source)
            {
                if (!predicate(current))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 將 IList 轉成 SortableBindingList
        /// </summary>
        /// <returns>傳回目前集合的 SortableBindingList</returns>
        //public static SortableBindingList<T> AsSortableBindingList<T>(this IList<T> source) where T : class
        //{
        //    if (source == null) throw new ArgumentNullException("source");

        //    return new SortableBindingList<T>(source);
        //}

        /// <summary>
        /// 將兩個 item 交換對調
        /// </summary>
        /// <param name="item1Index">Item 1 Index</param>
        /// <param name="item2Index">Item 2 Index</param>
        public static void Swap<T>(this IList<T> source, int item1Index, int item2Index)
        {
            T temp = source[item1Index];
            source[item1Index] = source[item2Index];
            source[item2Index] = temp;
        }

        /// <summary>
        /// 將連續數值分群並由小到大排列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<Queue<short>> Group(this short[] source)
        {
            if (source == null) throw new ArgumentNullException("source");

            Array.Sort(source);
            List<Queue<short>> result = new List<Queue<short>>();
            Queue<short> value = new Queue<short>();

            for (int i = 0; i < source.Count(); i++)
            {
                if(i < source.Count() -1 && source[i]+1 == source[i+1]) { value.Enqueue(source[i]); }
                else
                {
                    value.Enqueue(source[i]);
                    result.Add(value);
                    value = new Queue<short>();
                }
            }

            return result;
        }

        /// <summary>
        /// 取得資料中指定參數
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="dataName">欲取得參數名</param>
        /// <returns></returns>
        public static object GetDataValue<T>(this T source, string dataName)
        {
            return source.GetType().GetField(dataName).GetValue(source);
        }

        /// <summary>
        /// 取得資料中所有參數數量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int GetDataMaxCount<T>(this T source)
        {
            return source.GetType().GetFields().Count();
        }

        
    }
}