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

    private SerialStringAccumulator stringAccumulator;

    private SerialPort _serialPort;

    public Connection(Connector connector, SerialInterpreter interpreter)
    {
      this.connector = connector;
      this.interpreter = interpreter;
      this._serialPort = null;

      stringAccumulator = new SerialStringAccumulator();
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
      /* Infinite loop to continually attempt reconnection if communication is severed
      */
      while(true)
      {
        try
        {
          // Read from Hardware Serial Connection (possibly a partial message)
          // accumulate onto gathered messages
          // If we received complete message, process it
          String newSerialInput = _serialPort.ReadExisting();
          stringAccumulator.accumulate(newSerialInput);
          String message = stringAccumulator.getMessage();
          if(message == null)
          {
            /*skip*/
          }
          else
          {
            return interpreter.ValuesFrom(message);
            /*
                Success!
                EXIT
            */
          }
        }

        /* Many possible errors may arrive while in the infinite-loop.
          Continually retry if error occurs.
        */
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

  /*
      If the computer reads from the buffer quicker than the arduino can write, then partial messages may be processed.
      This object accumulates partial messages to allow only completed messages to be process.
  */
  public class SerialStringAccumulator
  {

    private string receivedString = "";

    char endOfMessageChar = '\n';

    public void accumulate(string newReceivedInput)
    {
      receivedString = receivedString + newReceivedInput;
    }


    private bool hasCompleteMessage()
    {
      return receivedString.Contains(endOfMessageChar);
    }


    public string getMessage()
    {
      if(hasCompleteMessage())
      {
        string[] messages = receivedString.Split(endOfMessageChar);
        string completedMessage = messages[0];
        string mostRecentMessage = messages[messages.Length - 1];
        /*
            If further, complete messages have been accumulated (i.e. indexes 2...infinity) exist, get rid of them.
        */
        receivedString = mostRecentMessage;
        return completedMessage;
      }
      else
      {
        return null;
      }
    }
  }
}
