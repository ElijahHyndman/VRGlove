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
      while (true)
      {
        try
        {
          this._serialPort = connector.Connect();
          Console.WriteLine("Connected to " + _serialPort.PortName);
          return;
        } catch
        {
          /*
            Continue until connection made
          */
        }
      }
    }

    /*
        Fetch list of integer values from hardware connection.
        Attempt to establish connection again if connection severed.
    */
    public int[] GetValues()
    {
      // Infinite loop with only one exit condition
      // Allows waiting for reconnection without recursion (creates buffer overflow)
      while(true)
      {
        try
        {
          // Assuming connection exists
          bool bufferFilled = _serialPort.BytesToRead>0;
          if(bufferFilled)
          {
            String serialInput = _serialPort.ReadExisting();
            return interpreter.ValuesFrom(serialInput);
            // EXIT
          }
        }
        catch (FormatException e)
        {
          // SerialBuffer overfilled or underfilled with values
          // CONTINUE
        }
        catch (TimeoutException e)
        {
          // Hardware Disconnected
          // Wait for reconnection
          this.Connect();
          // CONTINUE
        }
        catch (System.IO.IOException e)
        {
          // Failure during SerialPort.ReadExisting (disconnected)
          // Wait for reconnection
          this.Connect();
          // CONTINUE
        }
        catch (Exception e)
        {
          Console.WriteLine(e.ToString());
          // CONTINUE
        }
      }
    }
  }
}
