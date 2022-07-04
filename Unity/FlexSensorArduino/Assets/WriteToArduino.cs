using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.IO.Ports;

// Written using https://www.erikaagostinelli.com/post/using-arduino-to-control-unity-objects
// as a tutorial

public class WriteToArduino : MonoBehaviour
{
    static string arduino_usb = "/dev/tty.usbserial-1410";
    static int baud_rate = 9600;

    SerialPort sp = new SerialPort(arduino_usb, baud_rate);

    // Start is called before the first frame update
    void Start()
    {
      sp.Open();
      sp.ReadTimeout = 100;
      if(sp.IsOpen) {
        print("Connected");
      }
    }

    // Update is called once per frame
    int read = 0;
    void Update()
    {
      if(sp.IsOpen) {
          try {
            // read integer
            read = sp.ReadByte();
            print(read);

            // If read value is...
            if(read==1){
                   transform.Translate(0.0f,0.0f,0.1f);
            }

          // Serial Failure
          } catch (System.Exception) {
            // do nothing
          }
      }
    }
}
