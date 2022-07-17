using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRGlove;
using HardwareConnection;



public class HandController : MonoBehaviour
{
    public Hand targetHand;
    float flex, grip;
    Glove glove;
    Flexer o;



    // Start is called before the first frame update
    void Start()
    {
      Connection HW = new Connection(       connector : new Connectors.MacOS(9600),
                                        interpreter : new SerialInterpreters.DelimitedInts() );
      o = new Flexer();
      int[] pattern = new int[] {JOINT.I1, JOINT.IM};

      glove = new Glove( hardwareConnection : HW, pattern : pattern );
      glove.RegisterObserver( o );
    }

    // Update is called once per frame
    void Update()
    {
      glove.Update();
      grip = o.val1;
      flex = o.val2;
      targetHand.SetGrip(grip);
      targetHand.SetFlexion(flex);
    }

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


    /*
        Observer of a VR Glove
    */
    public class Flexer : VRGlove.GloveObserver {
      private float _val1=0.0f, _val2=0.0f, max1=0.0001f, max2=0.0001f;
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
        _val1 = (float)VRGlove.Get(JOINT.I1);
        _val2 = (float)VRGlove.Get(JOINT.IM);

        if(_val1>max1) max1 = _val1;
        if(_val2>max2) max2 = _val2;
      }

    }
}
