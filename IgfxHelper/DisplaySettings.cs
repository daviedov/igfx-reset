using System;
using System.Runtime.InteropServices;

namespace IgfxHelper
{
	internal class DisplaySettings
	{
		#region Non-Public Data Members

		[StructLayout(LayoutKind.Sequential)]
		private struct DeviceMode
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmDeviceName;
			public short dmSpecVersion;
			public short dmDriverVersion;
			public short dmSize;
			public short dmDriverExtra;
			public int dmFields;

			public short dmOrientation;
			public short dmPaperSize;
			public short dmPaperLength;
			public short dmPaperWidth;

			public short dmScale;
			public short dmCopies;
			public short dmDefaultSource;
			public short dmPrintQuality;
			public short dmColor;
			public short dmDuplex;
			public short dmYResolution;
			public short dmTTOption;
			public short dmCollate;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmFormName;
			public short dmLogPixels;
			public short dmBitsPerPel;
			public int dmPelsWidth;
			public int dmPelsHeight;

			public int dmDisplayFlags;
			public int dmDisplayFrequency;

			public int dmICMMethod;
			public int dmICMIntent;
			public int dmMediaType;
			public int dmDitherType;
			public int dmReserved1;
			public int dmReserved2;

			public int dmPanningWidth;
			public int dmPanningHeight;
		};

		private const int EnumCurrentSettings = -1;
		private const int CdsUpdateRegistry = 0x01;
		private const int CdsTest = 0x02;
		private const int CdsReset = 0x40000000;
		private const int DisplayChangeSuccessful = 0;
		private const int DisplayChangeRestart = 1;
		private const int DisplayChangeFailed = -1;
		private const int DeviceModeDeviceNameLength = 32;
		private const int DeviceModeDeviceFormName = 32;
		private const int DefaultWidth = 800;
		private const int DefaultHeight = 600;

		private readonly Logger _logger;

		#endregion

		#region Non-Public Properties/Methods

		[DllImport("user32.dll")]
		private static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DeviceMode devMode);
		[DllImport("user32.dll")]
		private static extern int ChangeDisplaySettings(ref DeviceMode devMode, int flags);

		private bool TryGetDeviceMode(out DeviceMode dm)
		{
			dm = new DeviceMode();
			dm.dmDeviceName = new string(new char[DeviceModeDeviceNameLength]);
			dm.dmFormName = new string(new char[DeviceModeDeviceFormName]);
			dm.dmSize = (short)Marshal.SizeOf(dm);

			int returnCode = EnumDisplaySettings(null, EnumCurrentSettings, ref dm);
			_logger.Log(GetType().Name, $"EnumDisplaySettings executed with return code {returnCode}.");
			return returnCode == 1;
		}

		private bool TryToChangeResolution(DeviceMode dm)
		{
			int returnCode = ChangeDisplaySettings(ref dm, CdsTest);

			if (returnCode == DisplayChangeFailed)
			{
				_logger.Log(GetType().Name, $"Unable to process display resolution change. Return code {returnCode}.");
			}
			else
			{
				returnCode = ChangeDisplaySettings(ref dm, CdsUpdateRegistry);
				switch (returnCode)
				{
					case DisplayChangeSuccessful:
						{
							_logger.Log(GetType().Name, $"TryToChangeResolution succeeded.");
							return true;
						}
					case DisplayChangeRestart:
						{
							_logger.Log(GetType().Name, $"A reboot is required for the display resolution change. Return code {returnCode}.");
							break;
						}
					default:
						{
							_logger.Log(GetType().Name, $"Failed to change the display resolution. Return code {returnCode}.");
							break;
						}
				}
			}
			return false;
		}


		#endregion

		#region Public Interface

		public DisplaySettings(Logger logger)
		{
			_logger = logger;
		}

		public void ChangeResolution()
		{
			_logger.Log(GetType().Name, $"TryToChangeResolution to {DefaultWidth}x{DefaultHeight} and return back to original.");

			try
			{
				if (TryGetDeviceMode(out DeviceMode dm))
				{
					int currentWidth = dm.dmPelsWidth;
					int currentHeight = dm.dmPelsHeight;
					dm.dmPelsWidth = DefaultWidth;
					dm.dmPelsHeight = DefaultHeight;
					if (TryToChangeResolution(dm))
					{
						if (TryGetDeviceMode(out dm))
						{
							dm.dmPelsWidth = currentWidth;
							dm.dmPelsHeight = currentHeight;
							TryToChangeResolution(dm);
						}
						else
						{
							_logger.Log(GetType().Name, "Unable to get display settings after change resolution.");
						}
					}
				}
				else
				{
					_logger.Log(GetType().Name, "Unable to get display settings before change resolution.");
				}
			}
			catch (Exception ex)
			{
				_logger.Log(GetType().Name, "Unable to get display settings before change resolution.");
				_logger.Log(GetType().Name, $"Exception {ex.Message}.");
			}
		}

		public void Reset()
		{
			_logger.Log(GetType().Name, $"TryToReset.");
			try
			{
				DeviceMode dm = new DeviceMode();
				dm.dmSize = (short)Marshal.SizeOf(dm);

				int returnCode = ChangeDisplaySettings(ref dm, CdsReset);
				switch (returnCode)
				{
					case DisplayChangeSuccessful:
						{
							_logger.Log(GetType().Name, $"TryToReset succeeded.");
							return;
						}
					case DisplayChangeRestart:
						{
							_logger.Log(GetType().Name, $"A reboot is required for the display resolution change. Return code {returnCode}.");
							break;
						}
					default:
						{
							_logger.Log(GetType().Name, $"Failed to change the display resolution. Return code {returnCode}.");
							break;
						}
				}


			}
			catch (Exception ex)
			{
				_logger.Log(GetType().Name, "Unable to reset display.");
				_logger.Log(GetType().Name, $"Exception {ex.Message}.");
			}
		}

		#endregion
	}
}
