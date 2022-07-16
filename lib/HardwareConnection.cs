using System;
using System.IO.Ports;

namespace HardwareConnection
{

  //
  //
  // Interface
  //
  //
  public interface Connector
  {
    // Establish connection to arduino based on operating system procedures.
    // Tries once, throws exception on failure.
    SerialPort Connect();

    // Property
    bool Connected
    {
      get;
      set;
    }
  }

  //
  //
  //
  //
  //
  public interface SerialInterpreter
  {
    // For this given SerialInterpreter type/format interpretation, interpret the Serial Connection's string into useful values
    int ValueFrom(string serialString);
  }

  //
  //
  //
  //
  //
  public class Connection
  {
    private Connector connector;
    private SerialInterpreter interpreter;
    private SerialPort _serialPort;

    public Connection(Connector connector, SerialInterpreter interpreter)
    {
      this.connector = connector;
      this.interpreter = interpreter;
      this._serialPort = null;
      Connect();
    }

    private void Connect()
    {
      // repeatedly attempt to create connection
      while (!connector.Connected)
      {
        try
        {
          this._serialPort = connector.Connect();
          Console.WriteLine("Connected to " + _serialPort.PortName);
        } catch
        {
          // Continue until connection made
          //Console.WriteLine("Waiting for connection.");
        }
      }
    }

    public int GetValue()
    {
      try
      {
        // Assuming connection exists
        String serialString = _serialPort.ReadExisting();
        int value = interpreter.ValueFrom(serialString);
        Console.WriteLine("input: " + value);
        return value;
      } catch
      {
        // If not, wait however long to connect. then recurse
        this.Connect();
        return this.GetValue();
      }
    }
  }
}
