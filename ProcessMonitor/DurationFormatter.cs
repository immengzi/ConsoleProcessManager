using System;

namespace ProcessMonitor
{

    /// <summary>
    /// 格式化时间
    /// </summary>
    static class DurationFormatter
    {
        public static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalHours >= 1)
            {
                var hours = (int)duration.TotalHours;
                var minutes = duration.Minutes;
                return $"{hours}小时{minutes}分";
            }
            else
            {
                var minutes = (int)duration.TotalMinutes;
                var seconds = duration.Seconds;
                return $"{minutes}分钟{seconds}秒";
            }
        }
    }
}