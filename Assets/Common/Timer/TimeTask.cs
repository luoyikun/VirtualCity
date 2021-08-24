namespace Framework.Tools
{
    /// <summary>  
    /// 定时任务封装类  
    /// </summary>  
    public class TimeTask
    {
        /// <summary>  
        /// 延迟时间  
        /// </summary>  
        private float _timeDelay;
        /// <summary>  
        /// 延迟时间  
        /// </summary>  
        private float _timeDelayOnly;
        /// <summary>  
        /// 是否需要重复执行  
        /// </summary>  
        private bool _repeat;
        /// <summary>  
        /// 回调函数  
        /// </summary>  
        private TimeTaskDelegate _timeTaskCallBack;

        public float timeDelay
        {
            get { return _timeDelay; }
            set { _timeDelay = value; }
        }
        public float timeDelayOnly
        {
            get { return _timeDelayOnly; }
        }
        public bool repeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }
        public TimeTaskDelegate timeTaskCallBack
        {
            get { return _timeTaskCallBack; }
        }

        //构造函数  
        public TimeTask(float timeDelay, bool repeat, TimeTaskDelegate timeTaskCallBack)
        {
            _timeDelay = timeDelay;
            _timeDelayOnly = timeDelay;
            _repeat = repeat;
            _timeTaskCallBack = timeTaskCallBack;
        }

        public TimeTask(float timeDelay, TimeTaskDelegate timeTaskCallBack) : this(timeDelay, true, timeTaskCallBack) { }
    }
}