using System;

namespace ProcessMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var processMonitorMonitor = new ProcessMonitor();
            processMonitorMonitor.StartMonitoring();

            Console.ReadLine();
        }
    }
}