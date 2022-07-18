using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using VRGlove;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    public float speed;

    Animator animator;

    private const int nJoints = 19;
    private float[] _JointCurrent = new float[nJoints];
    private float[] _JointTarget = new float[nJoints];


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

    /*
        Set target position-value for a joint.
    */
    internal void SetJoint(int joint, float val) {
      _JointTarget[joint] = val;
    }

    /*
        Go through all Joints.
        Update their values and show new animation.
    */
    private void AnimateHand() {
      float inc = Time.deltaTime * speed ;

      // VRGlove.JOINT values must be integers 0 to Whatever-value
      for(int joint=0; joint<_JointCurrent.Length; joint++)
      {
        string paramName = JOINT.Names[joint];

        // Slowly move towards target value
        _JointCurrent[joint] = Mathf.MoveTowards(_JointCurrent[joint], _JointTarget[joint], inc);

        // Update animation to reflect current
        animator.SetFloat(paramName, _JointCurrent[joint]);
      }
    }
}
