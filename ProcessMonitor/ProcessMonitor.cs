using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace ProcessMonitor
{

    /// <summary>
    /// 处理逻辑
    /// </summary>
    class ProcessMonitor
    {
        private string activeProcessName;
        private Stopwatch stopwatch;
        private List<Record> records;
        private bool isMonitoring;

        public ProcessMonitor()
        {
            activeProcessName = GetActiveProcessName(); // 初始化为当前活动的软件名称
            stopwatch = new Stopwatch();
            records = new List<Record>();
            isMonitoring = true;
        }

        public void StartMonitoring()
        {
            // 将 OnSessionSwitch 方法注册为 SessionSwitch 事件的处理程序
            SystemEvents.SessionSwitch += OnSessionSwitch;
            while (true)
            {
                if (isMonitoring)
                {
                    {
                        var currentProcessName = GetActiveProcessName();
                        // 排除无焦点的情况
                        if (currentProcessName != activeProcessName && currentProcessName != "Idle")
                        {
                            var elapsed = stopwatch.Elapsed;

                            var existingRecord = records.FirstOrDefault(r => r.ProcessName == activeProcessName);

                            if (existingRecord != null)
                            {
                                existingRecord.Duration += elapsed;
                            }
                            else
                            {
                                records.Add(new Record(activeProcessName, elapsed));
                            }

                            records = records.OrderByDescending(r => r.Duration).ToList();

                            Console.WriteLine("应用使用时长记录：");
                            foreach (var record in records)
                            {
                                var formattedDuration = DurationFormatter.FormatDuration(record.Duration);
                                Console.WriteLine($"应用: {record.ProcessName}, 使用时长: {formattedDuration}");
                                record.FormattedDuration = formattedDuration;
                            }

                            CsvWriter.WriteRecordsToCsv(records, "Record.csv");

                            stopwatch.Restart();
                            activeProcessName = currentProcessName;
                        }

                        Thread.Sleep(10);
                    }
                }
            }
            
        }
        
        /// <summary>
        /// 锁屏时暂停记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                isMonitoring = false;
                Console.WriteLine("记录已暂停!");
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                isMonitoring = true;
                Console.WriteLine("记录已恢复~");
            }
        }
        
        /// <summary>
        /// 获得当前处于Active的进程名称
        /// </summary>
        /// <returns></returns>
        private string GetActiveProcessName()
        {
            var activeProcess = GetActiveProcess();
            if (activeProcess != null)
            {
                return activeProcess.ProcessName;
            }
            return null;
        }

        /// <summary>
        /// 获得当前处于Active的进程ID
        /// </summary>
        /// <returns></returns>
        private Process GetActiveProcess()
        {
            var handle = GetForegroundWindow();
            GetWindowThreadProcessId(handle, out var processId);
            return Process.GetProcessById((int)processId);
        }

        // 获取当前前台窗口的句柄
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        // 获取该窗口所属的进程 ID
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    }
}
