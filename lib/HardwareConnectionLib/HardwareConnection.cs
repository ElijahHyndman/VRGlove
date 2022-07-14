using System;
using System.IO.Ports;

namespace HardwareConnection {

  //
  //
  // Interface
  //
  //
  public interface Connector {
    // Establish connection to arduino based on operating system procedures.
    // Tries once, throws exception on failure.
    SerialPort connect();

    // Property
    bool Connected {
      get;
      set;
    }
  }

  //
  //
  //
  //
  //
  public interface SerialInterpreter {
    // For this given SerialInterpreter type/format interpretation, interpret the Serial Connection's string into useful values
    float valueFrom(string serialString);
  }

  //
  //
  //
  //
  //
  public class Connection {
    private Connector connector;
    private SerialInterpreter interpreter;
    private SerialPort _serialPort;

    public Connection(Connector connector, SerialInterpreter interpreter) {
      this.connector = connector;
      this.interpreter = interpreter;
      this._serialPort = null;
      connect();
    }

    private void connect() {
      // repeatedly attempt to create connection
      while (!connector.Connected) {
        try {
          this._serialPort = connector.connect();
          Console.WriteLine("Connected to " + _serialPort.PortName);
        } catch {
          // Continue until connection made
          //Console.WriteLine("Waiting for connection.");
        }
      }
    }

    public float getValue() {
      try {
        // Assuming connection exists
        String serialString = _serialPort.ReadExisting();
        float value = interpreter.valueFrom(serialString);
        return value;
      } catch {
        // If not, wait to connect.
        // recurse
        connect();
        return this.getValue();
      }
    }
  }
}
