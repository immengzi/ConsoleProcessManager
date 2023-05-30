using System;
using Microsoft.Win32;

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

            // 用于 Debug
            Console.WriteLine(themeColor);
            if (config.StartupEnabled)
            {
                // 执行开机自启逻辑
                EnableStartup();
                Console.WriteLine("开机自启已启用。");
            }
            else
            {
                DisableStartup();
                Console.WriteLine("开机自启已禁用。");
            }

            if (config.NotificationEnabled)
            {
                Console.WriteLine("桌面通知已开启。");
            }
            else
            {
                Console.WriteLine("桌面通知未开启。");
            }

            var processMonitor = new ProcessMonitor();
            processMonitor.StartMonitoring();

            Console.ReadLine();
        }

        static void EnableStartup()
        {
            // 获取当前应用程序的可执行文件路径
            string appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            // 创建注册表项
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            // 设置开机自启
            rk.SetValue("ProcessMonitor", appPath);
        }

        static void DisableStartup()
        {
            // 创建注册表项
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            // 取消开机自启
            rk.DeleteValue("ProcessMonitor", false);
        }
    }
}