using System;
using HardwareConnection;
using System.IO.Ports;

public class MainClass {

  public static void Main(string[] args) {
    Console.WriteLine("=======================================");
    // Connector con = new MacOSConn();
    // con.connect();

    Connection con = new Connection( new Connectors.MacOS(9600) , new SerialInterpreters.DelimitedInts() );

    while(true) {
      con.GetValues();
    }

    Console.WriteLine("End");
  }

}
