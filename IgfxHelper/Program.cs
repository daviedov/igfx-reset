using System;

namespace IgfxHelper
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Logger logger = new Logger("igfx-helper.log");
			Console.WriteLine("IGFX helper");
			if (args.Length == 1)
			{
				Console.Beep(2000, 500);
				switch (args[0])
				{
					case "1":
					case "display-reset":
						{
							var displaySettings = new DisplaySettings(logger);
							displaySettings.Reset();
						}
						break;
					case "2":
					case "change-resolution":
						{
							var displaySettings = new DisplaySettings(logger);
							displaySettings.ChangeResolution();
						}
						break;
					case "3":
					case "ctrl-shift-win-b":
						var ctrlShiftWinB = new CtrlShiftWinB(logger);
						ctrlShiftWinB.Run();
						break;
					case "4":
					case "pnp":
						var pnpEnableDisable = new PnpEnableDisable(logger);
						pnpEnableDisable.Run();
						break;
					case "5":
					case "gdi-count":
						var gdiObjectsCount = new GdiObjectsCount(logger);
						gdiObjectsCount.Run();
						break;
					case "6":
					case "task-manager":
						var taskManager = new TaskManager(logger);
						taskManager.Run();
						break;
				}
				Console.Beep(1000, 500);
			}
			else
			{
				Console.WriteLine("Options:\r\n");
				Console.WriteLine("display-reset or 1\r\nInvoke ChangeDisplaySettings with Reset command.\r\n");
				Console.WriteLine("change-resolution or 2\r\nInvoke ChangeDisplaySettings with change resolution.\r\n");
				Console.WriteLine("ctrl-shift-win-b or 3\r\nInvoke Ctrl+Shift+Win+B shortcut.\r\n");
				Console.WriteLine("pnp or 4\r\nDisable and then enable graphic driver.\r\n");
				Console.WriteLine("gdi-count or 5\r\nLog all the proces with GDI count greater than 0.\r\n");
				Console.WriteLine("task-manager or 6\r\nLog all the process with CPU and Memory utilization.\r\n");

			}
		}
	}
}
