using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRGlove;

public class KeyboardInputSource : InputSource
{
  public void Update(Hand target) {
    target.SetJoint(JOINT.T3, GetKey(KeyCode.G));
    target.SetJoint(JOINT.T2, GetKey(KeyCode.B));
    target.SetJoint(JOINT.T1, GetKey(KeyCode.Space));

    target.SetJoint(JOINT.TI, GetKey(KeyCode.N));
    target.SetJoint(JOINT.I3, GetKey(KeyCode.Alpha7));
    target.SetJoint(JOINT.I2, GetKey(KeyCode.U));
    target.SetJoint(JOINT.I1, GetKey(KeyCode.J));

    target.SetJoint(JOINT.MR, GetKey(KeyCode.M));
    target.SetJoint(JOINT.M3, GetKey(KeyCode.Alpha8));
    target.SetJoint(JOINT.M2, GetKey(KeyCode.I));
    target.SetJoint(JOINT.M1, GetKey(KeyCode.K));

    target.SetJoint(JOINT.MR, GetKey(KeyCode.Comma));
    target.SetJoint(JOINT.R3, GetKey(KeyCode.Alpha9));
    target.SetJoint(JOINT.R2, GetKey(KeyCode.O));
    target.SetJoint(JOINT.R1, GetKey(KeyCode.L));

    target.SetJoint(JOINT.RP, GetKey(KeyCode.Period));
    target.SetJoint(JOINT.P3, GetKey(KeyCode.Alpha0));
    target.SetJoint(JOINT.P2, GetKey(KeyCode.P));
    target.SetJoint(JOINT.P1, GetKey(KeyCode.Semicolon));
  }

  private float GetKey(UnityEngine.KeyCode key)
  {
    if(Input.GetKey(key))
    {
      return 1.0f;
    }
    else
    {
      return 0.0f;
    }
  }
}
