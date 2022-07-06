using System;
using HardwareConnection;

// using Microsoft.Extensions.FileSystemGlobbing;
using System.IO;
using System.IO.Ports;

public class MacOSConn : HardwareConnection.Connector {
  private const string MAC_DEFAULT_USB_GLOB = "tty.usbserial-*";
  // variables
  private int baudRate;
  private string USBGlob;
  private SerialPort _serialPort;
  private bool _connected;

  // Public property
  public bool Connected {
    get { return _connected; }
    set { /*do nothing*/ }
  }

  public MacOSConn(int baudRate, string USBGlob=MAC_DEFAULT_USB_GLOB) {
    this.baudRate = baudRate;
    this.USBGlob = USBGlob;
    this._connected = false;
  }

  // Implementing Interface Contract
  public SerialPort connect() {
    SerialPort _serialPort;
    string searchDirectoryPath = "/dev/";

    // Get all USB devices
    string[] currentConnectedDevices = Directory.GetFiles(searchDirectoryPath, USBGlob); // Search for USB
    foreach(string devicePath in currentConnectedDevices ) {
        Console.WriteLine(devicePath);
        SerialPort _sp = new SerialPort(devicePath, this.baudRate);

        try {
          // Try to connect to device
          _sp.Open();
          _sp.ReadTimeout = 100;

          bool isConnection = _sp.IsOpen;
          if( isConnection ) {
            // Clean serial-port buffer
            _sp.DiscardInBuffer();
            _sp.DiscardOutBuffer();

            this._serialPort = _sp;
            this._connected = true;
            return this._serialPort;
          }
        } catch {
          // Move on if failed
        }
    }

    // No Device Found
    this._connected = false;
    throw new Exception();
  }

}
