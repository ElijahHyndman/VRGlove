using System;
using HardwareConnection;
using System.IO.Ports;

public class MainClass {

  public static void Main(string[] args) {
    Console.WriteLine("=======================================");
    // Connector con = new MacOSConn();
    // con.connect();

    Console.WriteLine("Attempting to find connection.");
    Connection con = new Connection( new Connectors.MacOS(9600) , new SerialInterpreters.DelimitedInts() );

    Console.WriteLine("Polling values from connection.");
    while(true) {
      con.GetValues();
    }
  }

}
