using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public struct WarnRequest
    {
        private readonly Func<string, bool> _isAgainDoCallback;

        public WarnRequest(Func<string, bool> isAgainDoCallback)
        {
            _isAgainDoCallback = isAgainDoCallback;
        }

        public bool IsAgainDo(string errorMsg)
        {
            try
            {
                if (_isAgainDoCallback == null) return false;

                return _isAgainDoCallback(errorMsg);
            }
            catch { }

            return false;
        }
    }
}
