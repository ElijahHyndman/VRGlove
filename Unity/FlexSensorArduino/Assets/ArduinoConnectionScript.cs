using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using TMPro;

public class ArduinoConnectionScript : MonoBehaviour
{
    // Related scripts
    [SerializeField]
    internal ArduinoNameScript NameFetchingScript;
    [SerializeField]
    internal TextMeshProUGUI ConnectionText;
    [SerializeField]
    internal TextMeshProUGUI ArduinoValueText;
    [SerializeField]
    internal SensorListener listener;

    // Constants
    string conn_start = "Connection: ";
    string conn_none = "No Connection";
    string val_start = "value: ";
    int ARDUINO_END_CHAR = 10;

    // Variables
    private string arduinoName;                             // string name of the arduino on computer
    private string arduinoLocation;                         // Full path to arduino on system
    private static string MAC_USB_LOCATION = "/dev/tty.";   // Location of usb files for Mac OS
    static int baud_rate = 9600;
    static int timeoutTime = 100;
    private string message = "X";

    SerialPort sp;                                          // Serial port for communicating to arduino


    //
    // Input Field Methods
    //
    public void newArduinoString(string arduinoString) {
      // Automatically called by textfield to update connection
      arduinoName = arduinoString;
      arduinoLocation = MAC_USB_LOCATION + arduinoName;
      attemptNewConnection();
    }

    //
    // Connection Establishment
    //
    private void attemptNewConnection() {
      // assert: arduinoLocation now holds location of new arduino connection
      print("Connecting to " + arduinoName);
      try {
        // Attempt to connect to arduino
        sp = new SerialPort(arduinoLocation, baud_rate);
        sp.Open();
        sp.ReadTimeout = timeoutTime;

        if( sp.IsOpen ) {
          print("Connected to: ["+arduinoLocation+"]");

          // Clear any existing data in Serial Buffer
          sp.DiscardInBuffer();
          sp.DiscardOutBuffer();
        }
      } catch {
        print("Failed to connect to: ["+arduinoLocation+"]" );
      }
    }

    //
    // Text Field Methods
    //
    private void updateConnectionText() {
      try {
        if ( sp.IsOpen ) {
          ConnectionText.text = conn_start + arduinoLocation;
        }
        else {
          ConnectionText.text = conn_start + conn_none;
        }
      } catch {
          ConnectionText.text = conn_start + conn_none;
      }
    }
    private void updateValueText(string msg) {
      ArduinoValueText.text = val_start + msg;
    }

    private string readArduinoString() {
      // methods from https://docs.microsoft.com/en-us/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-6.0

      // Read all bytes in serial port
      read = sp.ReadExisting();

      // Assert: Some serial messages are empty carriage returns
      carriageReturnMessage = (int)read[0] == ARDUINO_END_CHAR;
      if(carriageReturnMessage) {
        // skip
      } else {
        message = read;
      }

      return message;
    }

    //
    // Unity Methods
    //

    // Start is called before the first frame update
    void Start() {
      arduinoName = "NO_CONNECTION";
    }

    string read;
    bool carriageReturnMessage;
    void Update() {
        updateConnectionText();
        updateValueText(message);

        try { // Interact with connection
          if( sp.IsOpen ) {
            message = readArduinoString();
            listener.notify( System.Convert.ToInt32(message) );
          }
        }
        catch {} // If no connection, do Nothing
    }

}
