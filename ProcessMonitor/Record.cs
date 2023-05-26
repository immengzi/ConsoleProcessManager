using System;

namespace ProcessMonitor
{

    /// <summary>
    /// 定义Record类
    /// </summary>
    class Record
    {
        public string ProcessName { get; set; }
        public TimeSpan Duration { get; set; }
        public string FormattedDuration { get; set; }
        public DateTime Date { get; set; }

        public Record(string processName, TimeSpan duration)
        {
            ProcessName = processName;
            Duration = duration;
            Date = DateTime.Now;
        }
    }
}