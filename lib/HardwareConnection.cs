using System;
using System.IO.Ports;
using VRGlove;

namespace HW
{


  /*
        Connector Behavior

        Finds a SerialPort object to the arduino.

        Responsible for implementing behavior finding SerialPort connection to arduino on some specific platform (Mac, Windows, others)

        Connection is either found on first try or else exception is thrown.
        Repeated attempts to connect are handled by HardwareConnection Object.
  */
  public interface Connector
  {
    // Attempt connection once
    SerialPort Connect();
  }


  /*
      SerialInterpreter Behavior

      Interprets serial-input string "int.int..." as values for VR Finger joints.

      Responsible for knowing format of string received from hardware.
      Interpret string as a list of values, parse them, and return them as int[] list.
  */
  public interface SerialInterpreter
  {
    // For this given SerialInterpreter type/format interpretation, interpret the Serial Connection's string into useful values
    int[] ValuesFrom(string serialString);
  }

  /*
    SerialStringManager behavior

    manages strings coming from serial to compensate for Arduino v. Computer timing differences.

    Messages may be partially written or multiple messages may be written, accumulate and parse these into complete messages.
    Feed a complete message back, or feed Null to indicate that complete message is not prepared yet.

  */
  public interface SerialStringManager
  {
    void accumulate(string newReceivedInput);
    string getMessage();
  }


  /*
      HardwareConnection Entity

      - Provides abstracted .getValues() method for software->hardware connection.
      - Establishes connection and maintains connection.


      Responsible for abstracting away and keeping a connection consistent with Arduino Hardware.
      As connection is interrupted, process is halted until connection is resumed.

      Reesponsible for getting Serial string input from hardware, handing off to interpreter to interpret serial string, and passing interpreted values back to VRGlove.
  */
  public interface HardwareConnection
  {
    int[] GetValues();
  }


}
