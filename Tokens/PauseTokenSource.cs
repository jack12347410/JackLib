using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JackLib
{
    public class PauseTokenSource
    {
        private AutoResetEvent _pauseSignal = new AutoResetEvent(true);

        public PauseToken Token
        {
            get { return new PauseToken(this); }
        }
        /// <summary>
        /// 嘗試請求'插單作業'暫停
        /// </summary>
        /// <returns>true, 已暫停;false, 目前無法暫停</returns>
        public bool TryEnterPause()
        {
            return _pauseSignal.WaitOne();
        }

        /// <summary>
        /// 請求'插單作業'暫停結束，繼續執行'插單作業'
        /// </summary>
        /// <returns>true, 請求成功;false, 否</returns>
        public bool ExitPause()
        {
            return _pauseSignal.Set();
        }

        /// <summary>
        /// 等待至允許可以繼續執行插單作業為止
        /// </summary>
        public void WaitToCanDo()
        {
            _pauseSignal.WaitOne();
        }

        /// <summary>
        /// 插單作業已完成
        /// </summary>
        public void Done()
        {
            while (!_pauseSignal.Set()) ;
        }
    }

    public struct PauseToken
    {
        private PauseTokenSource _source;

        internal PauseToken(PauseTokenSource source)
        {
            _source = source;
        }

        /// <summary>
        /// 嘗試請求'插單作業'暫停
        /// </summary>
        /// <returns>true, 已暫停;false, 目前無法暫停</returns>
        public bool TryEnterPause()
        {
            return _source.TryEnterPause();
        }

        /// <summary>
        /// 請求'插單作業'暫停結束，繼續執行'插單作業'
        /// </summary>
        /// <returns>true, 請求成功;false, 否</returns>
        public bool ExitPause()
        {
            return _source.ExitPause();
        }
    }
}
