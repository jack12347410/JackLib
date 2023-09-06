using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JackLib
{
    public class CustomCancellationTokenSource: CancellationTokenSource
    {
        private Exception _exception;
        public Exception ErrorException
        {
            get
            {
                return _exception;
            }
        }

        public void Cancel(Exception exception = null)
        {
            _exception = exception;
            base.Cancel();
        }

        public void ThrowIfMfgRunCancelIsRequest()
        {
            if (Token.IsCancellationRequested)
            {
                if (_exception == null) Token.ThrowIfCancellationRequested();
                else throw _exception;
            }
        }
    }
}
