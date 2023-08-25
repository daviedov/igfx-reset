using System;

namespace IgfxHelper
{
	internal class Logger
	{
		private readonly string _filePath;

		public Logger(string filePath)
		{
			_filePath = filePath;
		}

		public void Log(string module, string message) {
			string text =$"{DateTime.Now}:{module}:{message}";
			Console.WriteLine(text);
			System.IO.File.AppendAllText(_filePath, $"{text}\r\n");
		}

		internal void Log(string module)
		{
			Log(module, string.Empty);
		}
	}
}
