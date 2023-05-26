using System;

namespace ProcessMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Config();
            config.LoadConfig();

            bool startupEnabled = config.StartupEnabled;
            string themeColor = config.ThemeColor;

            if (startupEnabled)
            {
                // 执行开机自启逻辑
                Console.WriteLine("开机自启已启用。");
            }
            else
            {
                Console.WriteLine("开机自启已禁用。");
            }
            var processMonitorMonitor = new ProcessMonitor();
            processMonitorMonitor.StartMonitoring();

            Console.ReadLine();
        }
    }
}