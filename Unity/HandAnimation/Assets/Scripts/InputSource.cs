using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRGlove;
using HardwareConnection;

/*
    Abstract form of input source which affects hand model
*/
public interface InputSource
{
  void Update(Hand target);
}
