using System;
using System.Diagnostics;
using System.IO.Ports;
using VRGlove;
using HW;

public class Connection : HW.HardwareConnection
{

  private Connector _Connector;

  private SerialInterpreter _Interpreter;

  private SerialStringManager _StringManager;

  private SerialPort _SerialPort;


  public Connection(Connector connector, SerialStringManager stringManager, SerialInterpreter interpreter)
  {
    this._Connector = connector;
    this._Interpreter = interpreter;
    this._SerialPort = null;
    this._StringManager = stringManager;
  }


  /*
      Continually attempt to create connection with arduino hardware.
  */
  private void Connect()
  {
    // Infinite Loop
    while (true)
    {
      try
      {
        this._SerialPort = _Connector.Connect();
        Console.WriteLine("Connected to " + _SerialPort.PortName);
        return;
        /*
            SUCCESS
            exit.
        */
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
    // Infinite loop to continually attempt reconnection
    while(true)
    {
      try
      {
        // Read from Hardware Serial Connection (possibly a partial message)
        // accumulate onto gathered messages
        // If we received complete message, process it
        _StringManager.GetFrom(_SerialPort);

        String message = _StringManager.getMessage();

        if(message == null)
        {
          /*
            Nothing to process,
            EXIT
          */
        }
        else
        {
          return _Interpreter.ValuesFrom(message);
          /*
              Success!
              EXIT
          */
        }
      }

      // Many possible errors may arrive while in the infinite-loop.
      //  Continually retry if error occurs.
      catch (FormatException e)
      {
        // Console.WriteLine("FormatException.\n"+ e.Message);
        /*
          SerialBuffer overfilled or underfilled with values
          CONTINUE
        */
      }
      catch (TimeoutException e)
      {
        Console.WriteLine("Timeout Exception.\n"+ e.Message);
        this.Connect();
        /*
          Hardware Disconnected
          Wait for reconnection
          CONTINUE
        */
      }
      catch (System.IO.IOException e)
      {
        Console.WriteLine("IOException.\n"+ e.Message);
        this.Connect();
        /*
          Failure during SerialPort.ReadExisting (disconnected)
          Wait for reconnection
          CONTINUE
        */
      }
      catch (Exception e)
      {
        Console.WriteLine(e.GetType());
        Console.WriteLine("General Error.\n"+ e.Message);
        this.Connect();
        /*
          Console.WriteLine(e.ToString());
          CONTINUE
        */
      }
    }
  }
}


public class ConnectionTimingWrapper : HW.HardwareConnection
{
  // Nanoseconds per tick code from http://threadedminds.blogspot.com/2015/10/measure-time-milliseconds-micorseconds-nanoseconds-csharp.html
  HardwareConnection _Real;
  Stopwatch _Stopwatch;
  double frequency;
  double nanosecPerTick;

  public double NanoSecondsElapsed {
    get { return _Stopwatch.ElapsedTicks * nanosecPerTick;}
    set {}
  }

  public ConnectionTimingWrapper(HardwareConnection real)
  {
    this._Real = real;
    frequency = Stopwatch.Frequency;
    nanosecPerTick = (1000 * 1000 * 1000) / frequency;
  }

  public int[] GetValues()
  {
    _Stopwatch = Stopwatch.StartNew();

    int[] values = _Real.GetValues();

    _Stopwatch.Stop();

    return values;
  }
}



namespace SerialStringManagers
{
  /*
      Helper class for dealing with Serial Input Strings

      Accumulates partial strings received over serial into full messages.
      A full message will contain a newline character at the end.

      When the arduino sends a [message] of ["144.76.908\n"]
      If the computer polls the SerialPort quicker than the arduino can write, the computer may receive messages:
      - ["144.7"]
      - ["6.90"]
      - ["8\n"]
      which would be interpreted as values
      - 144.7.0
      - 6.90.0
      - 8.0.0
      respectively.

      SerialStringAccumulator will .accumulate() strings (1) ["144.7"], (2) ["6.90"], and (3) ["8\n"]
      After receiving (1) SerialStringAccumulator.getMessage() will return null
      After receiving (2) SerialStringAccumulator.getMessage() will return null
      After receiving (3) SerialStringAccumulator.getMessage() will return "144.76.908"

      if SerialStringAccumulator .accumulate()s the messages ["144.76.908\n722.89.40\n45.8"]
      - SerialStringAccumulator.getMessage() will return "144.76.908"
      - the value "722.89.40" will be thrown out (not processed quickly enough, avoids creating a queue)
      - further .accumulate()s will append onto ["45.8"]
      if the next message .accumulate()ed is ["21.6\n"]
      - .getMessage will then return "45.821.6"
  */
  public class SerialStringAccumulator : SerialStringManager
  {

    private string receivedString = "";

    char endOfMessageChar = '\n';

    // Accumulate new partial string
    private void accumulate(string newReceivedInput)
    {
      receivedString = receivedString + newReceivedInput;
    }


    public void GetFrom(SerialPort sp)
    {
      String newSerialInput = sp.ReadExisting();
      this.accumulate(newSerialInput);
    }


    // Return message if we have a completed message
    public string getMessage()
    {
      if(this.hasCompleteMessage())
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


    private bool hasCompleteMessage()
    {
      return receivedString.Contains(endOfMessageChar);
    }

  }


  /*
        Fake implementation for UUT testing
  */
  public class ConstantMessage : SerialStringManager
  {
    string _ConstantMessage;
    public ConstantMessage(string constantMessage)
    {
      this._ConstantMessage = constantMessage;
    }
    public void GetFrom(SerialPort sp)
    {
      // do nothing
    }
    public string getMessage()
    {
        return _ConstantMessage;
    }
  }
}


public class HardwareDisconnectException : Exception
{
  public HardwareDisconnectException()
  {
    // null exception
  }
}
