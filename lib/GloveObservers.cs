using HardwareConnection;
using VRGlove;
using System;

namespace GloveObservers
{
  /*
      Debug implementation
      Lists the value of desired joints to the console upon every update notification.
  */
  public class Debugger : VRGlove.GloveObserver
  {
    private int[] _joints;
    public Debugger(int[] desiredJoints) {
      _joints = desiredJoints;
    }
    public override void Notify()
    {
      Console.WriteLine("Reading");
      foreach (int joint in _joints)
      {
        Console.WriteLine("--" + VRGlove.Get(joint));
      }
    }
  }
}
