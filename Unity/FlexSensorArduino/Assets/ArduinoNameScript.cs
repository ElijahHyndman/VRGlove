using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoNameScript : MonoBehaviour
{
    [SerializeField]
    internal ArduinoConnectionScript main;

    private string arduinoString;

    // Start is called before the first frame update
    void Start()
    {
      arduinoString = "DEFAULT";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void readStringInput(string s)
    {
      arduinoString = s; // get name from text field
      print("Text field new string: " + arduinoString);
      main.newArduinoString(arduinoString);
    }
}
