using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRGlove;
using HardwareConnection;



public class HandController : MonoBehaviour
{
    public Hand targetHand;
    float A, B;
    Glove glove;
    Flexer listener;

    static int JointA = JOINT.I1;
    static int JointB = JOINT.M1;



    /*
        Create VRGlove and establish connection
        - pattern specifies the order in which joint values are presented in the hardware input string from arduino

        Tie our custom GloveObserver to use values from glove
    */
    void Start()
    {
      Connection HW = new Connection(       connector : new Connectors.MacOS(9600),
                                        interpreter : new SerialInterpreters.DelimitedInts() );

      int[] pattern = new int[] {JointA, JointB};

      glove = new Glove( hardwareConnection : HW, pattern : pattern );

      listener = new Flexer();
      glove.RegisterObserver( listener );
    }


    /*
        Update glove values and relay to animation
    */
    void Update()
    {
      glove.Update(); // auto-notifies observers
      A = listener.val1;
      B = listener.val2;
      targetHand.SetJoint(JointA, A);
      targetHand.SetJoint(JointB, B);
    }


    /*
        Observer of a VR Glove
    */
    public class Flexer : VRGlove.GloveObserver {

      private float _val1=0.0f,
      _val2=0.0f,
      max1=0.0001f,
      max2=0.0001f;

      public float val1
      {
        get => _val1 / max1;
        set {}
      }
      public float val2
      {
        get => _val2 / max2;
        set {}
      }


      public override void Notify() {
        _val1 = (float)VRGlove.Get(JointA);
        _val2 = (float)VRGlove.Get(JointB);

        if(_val1>max1) max1 = _val1;
        if(_val2>max2) max2 = _val2;
      }
    }


    /*
      Optional keyboard inputs to avoid arduino sensors
    */
    private float GetFlex() {
      if(Input.GetKey("a")) {
        return 1.0f;
      } else {
        return 0.0f;
      }
    }
    private float GetGrip() {
      if(Input.GetKey("s")) {
        return 1.0f;
      } else {
        return 0.0f;
      }
    }
}
