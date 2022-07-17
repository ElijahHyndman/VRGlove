using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using VRGlove;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    private float gripTarget, gripCurrent;
    private float flexTarget, flexCurrent;
    public float speed;

    Animator animator;

    private Dictionary<int, float> _JointVals = new Dictionary<int, float>() {
      {JOINT.T1, 0.0f},
      {JOINT.T2, 0.0f},
      {JOINT.T3, 0.0f},
      {JOINT.TI, 0.0f},
      {JOINT.I1, 0.0f},
      {JOINT.I2, 0.0f},
      {JOINT.I3, 0.0f},
      {JOINT.IM, 0.0f},
      {JOINT.M1, 0.0f},
      {JOINT.M2, 0.0f},
      {JOINT.M3, 0.0f},
      {JOINT.MR, 0.0f},
      {JOINT.R1, 0.0f},
      {JOINT.R2, 0.0f},
      {JOINT.R3, 0.0f},
      {JOINT.RP, 0.0f},
      {JOINT.P1, 0.0f},
      {JOINT.P2, 0.0f},
      {JOINT.P3, 0.0f},
    };

    private Dictionary<int, string> _JointNames = new Dictionary<int, string>() {
      {JOINT.T1, "T1"},
      {JOINT.T2, "T2"},
      {JOINT.T3, "T3"},
      {JOINT.TI, "TI"},
      {JOINT.I1, "I1"},
      {JOINT.I2, "I2"},
      {JOINT.I3, "I3"},
      {JOINT.IM, "IM"},
      {JOINT.M1, "M1"},
      {JOINT.M2, "M2"},
      {JOINT.M3, "M3"},
      {JOINT.MR, "MR"},
      {JOINT.R1, "R1"},
      {JOINT.R2, "R2"},
      {JOINT.R3, "R3"},
      {JOINT.RP, "RP"},
      {JOINT.P1, "P1"},
      {JOINT.P2, "P2"},
      {JOINT.P3, "P3"},
    };

    // Start is called before the first frame update
    void Start()
    {
      animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      AnimateHand();
    }

    internal void SetJoint(int joint, float val) {
      _JointVals[joint] = val;
    }

    void AnimateHand() {
      // float inc = Time.deltaTime * speed ;
      //
      // if(gripCurrent!=gripTarget) {
      //   gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, inc);
      //   animator.SetFloat("I2", gripCurrent);
      // }
      // if(flexCurrent!=flexTarget) {
      //   flexCurrent = Mathf.MoveTowards(flexCurrent, flexTarget, inc);
      //   animator.SetFloat("I1", flexCurrent);
      // }
      foreach (int joint in _JointVals.Keys)
      {
        string paramName = _JointNames[joint];
        float value = _JointVals[joint];
        animator.SetFloat(paramName, value);
      }
    }
}
