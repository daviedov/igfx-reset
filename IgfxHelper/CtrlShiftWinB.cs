using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IgfxHelper
{
	internal class CtrlShiftWinB
	{
		private readonly Logger _logger;

		public CtrlShiftWinB(Logger logger)
		{
			_logger = logger;
		}

		[DllImport("user32.dll")]
		private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
		private const int KEYEVENTF_EXTENDEDKEY = 1;
		private const int KEYEVENTF_KEYUP = 2;
		public void KeyDown(Keys vKey)
		{
			keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
		}
		public void KeyUp(Keys vKey)
		{
			keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
		}

		public void Run() {
			_logger.Log(GetType().Name, $"Start.");
			KeyDown(System.Windows.Forms.Keys.LWin);
			KeyDown(System.Windows.Forms.Keys.RControlKey);
			KeyDown(System.Windows.Forms.Keys.RShiftKey);
			KeyDown(System.Windows.Forms.Keys.B);
			KeyUp(System.Windows.Forms.Keys.LWin);
			KeyUp(System.Windows.Forms.Keys.RControlKey);
			KeyUp(System.Windows.Forms.Keys.RShiftKey);
			KeyUp(System.Windows.Forms.Keys.B);
			_logger.Log(GetType().Name, $"Finish.");
		}
	}
}
