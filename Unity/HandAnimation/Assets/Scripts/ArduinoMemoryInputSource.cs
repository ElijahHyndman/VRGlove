using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRGlove;
using HardwareConnection;

/*
  Assumes only three joints are being used

  memorizes max and min values of joints to normalize
*/
public class ArduinoMemoryInputSource : VRGlove.GloveObserver, InputSource
{
    int _JointA = JOINT.I2;
    int _JointB = JOINT.IM;
    int _JointC = JOINT.I1;

    float valA, valB, valC;
    float maxA=0.01f, maxB=0.01f, maxC=0.01f;
    float minA=100000.0f, minB=100000.0f, minC=100000.0f;

    public ArduinoMemoryInputSource()
    {
      Connection HW = new Connection(       connector : new Connectors.MacOS(9600, USBGlob : "tty.DSDTECHHC-05"),
                                            interpreter : new SerialInterpreters.DelimitedInts() );

      int[] pattern = new int[] {_JointA, _JointB, _JointC};

      VRGlove = new Glove( hardwareConnection : HW, pattern : pattern );

      VRGlove.RegisterObserver( this );
    }

    private float normalize(float val, float min, float max)
    {
      // float dist = val - min;
      // float range = max - min;
      // return dist / range;
      return 1.0f - val / max;
    }

    public override void Notify()
    {
      valA = (float)VRGlove.Get(_JointA);
      valB = (float)VRGlove.Get(_JointB);
      valC = (float)VRGlove.Get(_JointC);

      // Max
      if(valA>maxA) maxA=valA;
      if(valB>maxB) maxB=valB;
      if(valC>maxC) maxC=valC;
      // Min
      if(valA<minA) minA=valA;
      if(valB<minB) minB=valB;
      if(valC<minC) minC=valC;

      valA = normalize(valA, minA, maxA);
      valB = normalize(valB, minB, maxB);
      valC = normalize(valC, minC, maxC);
    }


    public void Update(Hand target)
    {
      VRGlove.Update();
      target.SetJoint(_JointA, valA);
      target.SetJoint(_JointB, valB);
      target.SetJoint(_JointC, valC);
    }
}
