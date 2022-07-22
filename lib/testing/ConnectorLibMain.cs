using System;
using HardwareConnection;
using System.IO.Ports;

using System.Text; // Encoding

public class MainClass {
  public static void Main(string[] args) {
    Console.WriteLine("Hello World");
    Connector conn = new Connectors.MacOS(9600, USBGlob:"tty.DSDTECHHC-05");

    SerialPort sp = conn.Connect();
    while(true)
    {
        printHex(sp.ReadExisting());
        // Console.WriteLine("Expected: "); printHex("Message");
    }
  }

  public static void printHex(string str) {
    byte[] ba = Encoding.ASCII.GetBytes(str);
    var hexString = BitConverter.ToString(ba);
    Console.WriteLine(hexString);
  }
}
