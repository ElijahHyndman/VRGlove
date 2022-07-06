using System;
using System.IO.Ports;

namespace HardwareConnection {

  // Interface defining behavior for connection-finding objects
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

  public class Connection {
    private Connector connector;
    private SerialPort _serialPort;

    public Connection(Connector connector) {
      this.connector = connector;
      this._serialPort = null;
      connect();
    }

    private void connect() {
      while (!connector.Connected) {
        try {
          this._serialPort = connector.connect();
          Console.WriteLine("Connected to " + _serialPort.PortName);
        } catch {
          Console.WriteLine("Waiting for connection.");
        }
      }
    }
  }
}
