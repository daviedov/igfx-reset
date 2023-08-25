using System;
using System.Management;

namespace IgfxHelper
{
	internal class PnpEnableDisable
	{
		private readonly Logger _logger;
		

		public PnpEnableDisable(Logger logger)
		{
			_logger = logger;
		}

		public void Run() {
			_logger.Log(GetType().Name, "Reset graphic driver using PnP diable/enable for display driver");
			// Set up a query to retrieve PnP device information
			string query = "SELECT * FROM Win32_PnPEntity WHERE PnPClass LIKE 'Display%'";


			// Create a ManagementObjectSearcher with the query
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

			// Execute the query and get a collection of results
			ManagementObjectCollection collection = searcher.Get();

			// Iterate through the results and print device information
			foreach (ManagementObject device in collection)
			{
				_logger.Log(GetType().Name, "Device Name: " + device["Name"]);
				_logger.Log(GetType().Name, "Device Description: " + device["Description"]);
				_logger.Log(GetType().Name, "Device Manufacturer: " + device["Manufacturer"]);
				_logger.Log(GetType().Name, "Device Status: " + device["Status"]);
				_logger.Log(GetType().Name, "Device Class: " + device["PNPClass"]);
				Console.WriteLine("Disable display driver");
				ChangeDeviceStatus(device["DeviceID"].ToString(), "Disable");
				_logger.Log(GetType().Name, "Enable display driver");
				ChangeDeviceStatus(device["DeviceID"].ToString(), "Enable");
				_logger.Log(GetType().Name );
			}
		}

		private  void ChangeDeviceStatus(string deviceId, string status)
		{

			// Create a ManagementObject with the path to the PnP device
			string devicePath = $"Win32_PnPEntity.DeviceID='{deviceId}'";
			ManagementObject device = new ManagementObject(new ManagementPath(devicePath));

			try
			{
				// Invoke the "Disable" method on the device
				ManagementBaseObject outParams = device.InvokeMethod(status, null, null);

				// Check the return code (0 indicates success)
				uint returnValue = (uint)(outParams.Properties["ReturnValue"].Value);
				if (returnValue == 0)
				{
					_logger.Log(GetType().Name, $"Device {status}d successfully.");
				}
				else
				{
					_logger.Log(GetType().Name, $"Failed to {status} the device. Return Code: " + returnValue);
				}
			}
			catch (Exception ex)
			{
				_logger.Log(GetType().Name, "An error occurred: " + ex.Message);
			}
		}
	}
}
