using System.Diagnostics;
using System.Threading;

namespace IgfxHelper
{
	internal class TaskManager
	{
		private readonly Logger _logger;

		public TaskManager(Logger logger)
		{
			_logger = logger;
		}

		public void Run()
		{
			_logger.Log(GetType().Name, "Simple Task Manager (Press Ctrl+C to exit)");

			_logger.Log(GetType().Name, "Process ID  |  Process Name           |  CPU (%)  |  Memory (MB)");

			Process[] processes = Process.GetProcesses();

			foreach (Process process in processes)
			{
				string processName = process.ProcessName;
				int processId = process.Id;

				float cpuUsage = 0;
				float memoryUsageMB = 0;

				try
				{
					// CPU usage is not available for some system processes, so we need to handle exceptions.
					cpuUsage = GetCpuUsage(process);
					memoryUsageMB = process.WorkingSet64 / (1024.0f * 1024.0f);
				}
				catch
				{
					// Handle exceptions when reading CPU usage
				}

				_logger.Log(GetType().Name, $"{processId,-12} | {processName,-25} | {cpuUsage,8:F2}% | {memoryUsageMB,10:F2} MB");
			}

			Thread.Sleep(2000); // Wait for 2 seconds before updating the display.
		}

		// Function to get the CPU usage of a process
		private float GetCpuUsage(Process process)
		{
			PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
			cpuCounter.NextValue(); // Initial value is always zero
			Thread.Sleep(100); // Wait for 0.1 second to get an accurate reading
			float cpuUsage = cpuCounter.NextValue();
			cpuCounter.Dispose();
			return cpuUsage;
		}
	}
}
