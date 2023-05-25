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
            SystemEvents.SessionSwitch += OnSessionSwitch;
            while (true)
            {
                if (isMonitoring)
                {
                    {
                        var currentSoftwareName = GetActiveProcessName();

                        if (currentSoftwareName != activeProcessName && currentSoftwareName != "Idle")
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
                            activeProcessName = currentSoftwareName;
                        }

                        Thread.Sleep(10);
                    }
                }
            }
            
        }
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

        private string GetActiveProcessName()
        {
            var activeProcess = GetActiveProcess();
            if (activeProcess != null)
            {
                return activeProcess.ProcessName;
            }
            return null;
        }

        private Process GetActiveProcess()
        {
            var handle = GetForegroundWindow();
            GetWindowThreadProcessId(handle, out var processId);
            return Process.GetProcessById((int)processId);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    }
}
