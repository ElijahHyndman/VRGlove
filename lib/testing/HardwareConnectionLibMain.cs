using System;
using HardwareConnection;
using System.IO.Ports;

public class MainClass {

  public static void Main(string[] args) {
    Console.WriteLine("=======================================");
    // Connector con = new MacOSConn();
    // con.connect();
    //

    Connection con = new Connection( new MacOSConn(9600) );

    Console.WriteLine("End");
  }

}
