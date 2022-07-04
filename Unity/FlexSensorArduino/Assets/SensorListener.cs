using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SensorListener : MonoBehaviour
{

    private float rotateRate = 0.0f;

    public void notify(int sensorVal) {
      print(sensorVal);
      rotateRate = sensorVal / 50.0f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      transform.Rotate(0.0f, 0.0f, rotateRate, Space.Self);
    }
}
