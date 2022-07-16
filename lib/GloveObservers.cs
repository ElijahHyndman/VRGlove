using HardwareConnection;
using VRGlove;
using System;

namespace GloveObservers
{
  public class Debugger : VRGlove.GloveObserver
  {
    public override void Update()
    {
      int newVal = _Subject.Val;
      Console.WriteLine("Custom Usage: " + newVal);
    }
  }
}
