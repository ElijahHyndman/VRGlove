using System;
using HW;
using System.IO;
using System.IO.Ports;

// Implementations of Connector interface
namespace Connectors
{

  /*
      Mac OS implementation
  */
  public class MacOS : HW.Connector
  {
    private const string MAC_DEFAULT_USB_GLOB = "tty.usbserial-*";
    /*
        Variables
    */
    private int baudRate;
    private string _DeviceNameGlob;
    private SerialPort _serialPort;

    public MacOS(int baudRate, string deviceName=MAC_DEFAULT_USB_GLOB)
    {
      this.baudRate = baudRate;
      this._DeviceNameGlob = deviceName;
    }

    /*
        Implementing connection behavior for Mac OS
    */
    public SerialPort Connect()
    {
      string searchDirectoryPath = "/dev/";

      // Get all current USB devices
      string[] currentConnectedDevices = Directory.GetFiles(searchDirectoryPath, _DeviceNameGlob); // Search for USB
      foreach(string devicePath in currentConnectedDevices )
      {
          //Console.WriteLine(devicePath);
          SerialPort _sp = new SerialPort(devicePath, this.baudRate);

          // Try to connect to each device
          try
          {
            _sp.Open();
            //_sp.ReadTimeout = 100;

            bool isConnection = _sp.IsOpen;
            if( isConnection ) {
              // Clean serial-port buffer
              _sp.DiscardInBuffer();
              _sp.DiscardOutBuffer();

              // Success!!!
              this._serialPort = _sp;
              return this._serialPort;
              // Exit
            }

          // Move on if failed
          } catch
          { }
      }

      // No Device Found
      throw new Exception();
    }
  }
}
