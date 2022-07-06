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

  public MacOSConn(int baudRate, string USBGlob=MAC_DEFAULT_USB_GLOB) {
    this.baudRate = baudRate;
    this.USBGlob = USBGlob;
  }

  // Implementing Interface Contract
  public SerialPort connect() {
    SerialPort _serialPort;
    string searchDirectoryPath = "/dev/";

    // Get all USB devices
    // Try to create SerialPort to each
    string[] devices = Directory.GetFiles(searchDirectoryPath, USBGlob); // Search for USB
    foreach(string devicePath in devices ) {
        Console.WriteLine(devicePath);
        _serialPort = new SerialPort(devicePath, this.baudRate);

        try {
          _serialPort.Open();
          _serialPort.ReadTimeout = 100;

          bool isConnection = _serialPort.IsOpen;
          if( isConnection ) {
            // Clean serial-port buffer
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
            return _serialPort;
          }
        } catch {

        }
    }
    return null;
  }

}
