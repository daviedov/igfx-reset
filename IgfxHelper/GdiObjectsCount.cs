using System.Diagnostics;
using System;

namespace IgfxHelper
{
	internal class GdiObjectsCount
	{
		private readonly Logger _logger;

		public GdiObjectsCount(Logger logger)
		{
			_logger = logger;
		}

		public void Run()
		{
			// Get all running processes
			Process[] processes = Process.GetProcesses();

			// Loop through each process and print its GDI object count
			foreach (Process process in processes)
			{
				try
				{
					int gdiObjects = GetProcessGdiObjectCount(process);
					if (gdiObjects > 0)
					{
						_logger.Log(GetType().Name, $"Process: {process.ProcessName}, GDI Objects: {gdiObjects}");
					}
				}
				catch (Exception ex)
				{
					// Handle exceptions (e.g., access denied to a process)
					_logger.Log(GetType().Name, $"Error reading GDI objects for process {process.ProcessName}: {ex.Message}");
				}
			}
		}

		private int GetProcessGdiObjectCount(Process process)
		{
			// Open the process with appropriate permissions
			IntPtr hProcess = Process.GetProcessById(process.Id).Handle;

			// Use the GetGuiResources function to get the GDI object count
			int gdiObjects = (int)GetGuiResources(hProcess, 0);

			return gdiObjects;
		}

		// Import the GetGuiResources function from user32.dll
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		extern static IntPtr GetGuiResources(IntPtr hProcess, int uiFlags);
	}
}
