using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public static class RetryUtil
    {
        public static T? Retry<T>(Func<T> doFunc, WarnRequest errorDoAgain)
        {
            bool isTryAgain = false;

            do
            {
                try
                {
                    return doFunc();
                }
                catch (Exception ex)
                {
                    isTryAgain = errorDoAgain.IsAgainDo(ex.ToString());
                }
            } while (isTryAgain);

            return default(T);
        }

        public static void Retry(Action doFunc, WarnRequest errorDoAgain)
        {
            bool isTryAgain = false;

            do
            {
                try
                {
                    doFunc();
                }
                catch (Exception ex)
                {
                    isTryAgain = errorDoAgain.IsAgainDo(ex.ToString());
                }
            } while (isTryAgain);
        }
    }
}
