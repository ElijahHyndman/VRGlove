using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Connects some abstract form of InputSource (from arduino, from keyboard, etc) to the hand model for control
*/
public class HandController : MonoBehaviour
{
    public Hand targetHand;


    public InputSource controllerInput;

    void Start()
    {
      InputSource input = new ArduinoInputSource();
      // InputSource input = new ArduinoMemoryInputSource();
      // InputSource input = new KeyboardInputSource();
      controllerInput = input;
    }


    void Update()
    {
      controllerInput.Update( targetHand );
    }

}
