using System;
using System.IO.Ports;
using VRGlove;

namespace HardwareConnection
{

  /*
        Connector Behavior
  */
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

  /*
      SerialInterpreter Behavior
  */
  public interface SerialInterpreter
  {
    // For this given SerialInterpreter type/format interpretation, interpret the Serial Connection's string into useful values
    int[] ValuesFrom(string serialString);
  }

  /*
      HardwareConnection Entity
  */
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

    public int[] GetValues()
    {
      try
      {
        // Assuming connection exists
        String serialInput = _serialPort.ReadExisting();
        return interpreter.ValuesFrom(serialInput);
        // Console.WriteLine("input: " + value);
      } catch
      {
        // If not, wait however long to connect. then recurse
        this.Connect();
        return this.GetValues();
      }
    }
  }
}
