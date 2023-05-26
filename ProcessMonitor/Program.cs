﻿using System;

namespace ProcessMonitor
{

    /// <summary>
    /// 主程序入口
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var config = Config.LoadConfig();
            string themeColor = config.ThemeColor;

            Console.WriteLine(themeColor);
            if (config.StartupEnabled)
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