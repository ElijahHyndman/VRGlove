using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    private float gripTarget, gripCurrent;
    private float flexTarget, flexCurrent;
    public float speed;

    Animator animator;

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

    internal void SetGrip(float v) {
      gripTarget = v;
    }

    internal void SetFlexion(float v) {
      flexTarget = v;
    }

    void AnimateHand() {
      float inc = Time.deltaTime * speed ;

      if(gripCurrent!=gripTarget) {
        gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, inc);
        animator.SetFloat("Grip", gripCurrent);
      }
      if(flexCurrent!=flexTarget) {
        flexCurrent = Mathf.MoveTowards(flexCurrent, flexTarget, inc);
        animator.SetFloat("Flexion", flexCurrent);
      }
    }
}
