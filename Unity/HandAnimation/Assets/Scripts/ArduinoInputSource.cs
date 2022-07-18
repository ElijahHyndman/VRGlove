using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRGlove;
using HardwareConnection;

/*
    Arduino VRGlove board controlling finger movement 
*/
public class ArduinoInputSource : VRGlove.GloveObserver, InputSource
{
    Glove glove;

    static int _JointA = JOINT.IM;
    static int _JointB = JOINT.I2;

    private float _val1=0.0f;
    private float _val2=0.0f;
    private float max1=0.0001f;
    private float max2=0.0001f;

    /*
        Create and connect to VRGlove controller
    */
    public ArduinoInputSource()
    {
      Connection HW = new Connection(       connector : new Connectors.MacOS(9600),
                                            interpreter : new SerialInterpreters.DelimitedInts() );

      int[] pattern = new int[] {_JointA, _JointB};

      VRGlove = new Glove( hardwareConnection : HW, pattern : pattern );

      VRGlove.RegisterObserver( this );
    }


    // Automatically called by VRGlove when VRGlove.Update() called
    public override void Notify()
    {
      _val1 = (float)VRGlove.Get(_JointA);
      _val2 = (float)VRGlove.Get(_JointB);
      if(_val1>max1) max1 = _val1;
      if(_val2>max2) max2 = _val2;

      _val1 = 1.0f - _val1 / max1;
      _val2 = 1.0f - _val2 / max2;
    }

    // public void Update()
    // {
    //     VRGlove.Update();
    // }
    public void Update(Hand target)
    {
      VRGlove.Update();
      target.SetJoint(_JointA, _val1);
      target.SetJoint(_JointB, _val2);
    }
}
