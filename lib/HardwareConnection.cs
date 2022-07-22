using System;
using System.IO.Ports;
using VRGlove;

namespace HardwareConnection
{


  /*
        Connector Behavior

        Responsible for implementing behavior for finding SerialPort connection to arduino on some specific platform (Mac, Windows, others)
        Connection is either found on first try or else exception is thrown.
        Repeated attempts to connect are handled by HardwareConnection Object.
  */
  public interface Connector
  {
    // Establish connection to arduino based on operating system procedures.
    // Tries once, throws exception on failure.
    SerialPort Connect();
  }


  /*
      SerialInterpreter Behavior

      Responsible for knowing format of string received from hardware.
      Interpret string as a list of values, parse them, and return them as int[] list.
  */
  public interface SerialInterpreter
  {
    // For this given SerialInterpreter type/format interpretation, interpret the Serial Connection's string into useful values
    int[] ValuesFrom(string serialString);
  }


  /*
      HardwareConnection Entity

      Responsible for abstracting away and keeping a connection consistent with Arduino Hardware.
      As connection is interrupted, process is halted until connection is resumed.

      Reesponsible for getting Serial string input from hardware, handing off to interpreter to interpret serial string, and passing interpreted values back to VRGlove.
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
      //Connect();
    }

    /*
        Continually attempt to create connection with arduino hardware. 
    */
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
        Attempt to establish connection again if connection severed. Halt processing until connection is resumed.

        return : list of values received from hardware
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
            /*
                Success!
                EXIT
            */
          }
        }
        catch (FormatException e)
        {
          /*
            SerialBuffer overfilled or underfilled with values
            CONTINUE
          */
        }
        catch (TimeoutException e)
        {
          this.Connect();
          /*
            Hardware Disconnected
            Wait for reconnection
            CONTINUE
          */
        }
        catch (System.IO.IOException e)
        {
          this.Connect();
          /*
            Failure during SerialPort.ReadExisting (disconnected)
            Wait for reconnection
            CONTINUE
          */
        }
        catch
        {
          this.Connect();
          /*
            Console.WriteLine(e.ToString());
            CONTINUE
          */
        }
      }
    }
  }
}
