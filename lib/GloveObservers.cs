using HardwareConnection;
using VRGlove;
using System;

namespace GloveObservers
{
  public class Debugger : VRGlove.GloveObserver
  {
    private int _joint;
    public Debugger(int joint) {
      _joint = joint;
    }
    public override void Update()
    {
      Console.WriteLine("Custom Usage: " + _Subject.get(_joint));
    }
  }
}
