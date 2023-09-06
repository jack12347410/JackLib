using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JackLib
{
    public sealed class TaskAbort
    {
        #region IsAborted

        private volatile bool _isAborted;

        /// <summary>
        /// 是否發生強制結束任務(Aborted)
        /// </summary>
        public bool IsAborted { get { return _isAborted; } }

        #endregion IsAborted

        #region IsCompleted

        private volatile bool _isCompleted;

        /// <summary>
        /// 是否已完成任務
        /// </summary>
        public bool IsCompleted { get { return _isCompleted; } }

        #endregion IsCompleted

        #region IsFaulted

        /// <summary>
        /// 是否發生錯誤(Faulted)
        /// </summary>
        public bool IsFaulted { get { return _error != null; } }

        private volatile Exception _error;
        public Exception Error { get { return _error; } }

        #endregion IsFaulted

        /// <summary>
        /// 建立一個已完成任務的新個體
        /// </summary>
        public static TaskAbort CompletedTask
        {
            get
            {
                var compTask = new TaskAbort();
                compTask._isCompleted = true;
                return compTask;
            }
        }

        /// <summary>
        /// 執行任務的'Worker'
        /// </summary>
        private volatile Thread _worker;

        /// <summary>
        /// 建立一個新個體
        /// </summary>
        private TaskAbort()
        { }

        /// <summary>
        /// 執行指定的任務
        /// </summary>
        /// <param name="action">指定的任務</param>
        /// <returns>任務</returns>
        public static TaskAbort StartNew(Action action)
        {
            if (action == null) new ArgumentNullException(nameof(action));

            var that = new TaskAbort();
            that.Start(action);
            return that;
        }

        /// <summary>
        /// 執行指定的任務
        /// </summary>
        /// <param name="action">指定的任務</param>
        private void Start(Action action)
        {
            _worker = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (ThreadAbortException)
                {
                    _isAborted = true;
                }
                catch (Exception ex)
                {
                    _error = ex;
                }
                finally
                {
                    _isCompleted = true;
                    Thread.MemoryBarrier();
                }
            });
            _worker.IsBackground = true;

            Thread.MemoryBarrier();
            _worker.Start();
        }

        /// <summary>
        /// 等待任務完成
        /// </summary>
        public void Wait()
        {
            if (_worker != null)
            {
                _worker.Join();
            }
        }

        #region WhenAll

        /// <summary>
        /// 等待所有任務完成
        /// </summary>
        /// <param name="tasks">任務</param>
        public static void WhenAll(IEnumerable<TaskAbort> tasks)
        {
            if (tasks == null) throw new ArgumentNullException(nameof(tasks));

            WhenAll(tasks.ToArray());
        }

        /// <summary>
        /// 等待所有任務完成
        /// </summary>
        /// <param name="tasks">任務</param>
        public static void WhenAll(params TaskAbort[] tasks)
        {
            if (tasks == null) throw new ArgumentNullException(nameof(tasks));
            if (tasks.Any(t => t == null)) throw new ArgumentException($"{nameof(tasks)} 集合包含 null 工作。", nameof(tasks));

            List<Exception> exs = new List<Exception>(tasks.Length);
            tasks.ForEach(t =>
            {
                try
                {
                    t.Wait();
                    if (t.IsFaulted)
                    {
                        exs.Add(t.Error);
                    }
                }
                catch (Exception ex)
                {
                    exs.Add(ex);
                }
            });

            if (exs.Any())
            {
                throw new AggregateException(exs);
            }
        }

        #endregion WhenAll
    }
}
