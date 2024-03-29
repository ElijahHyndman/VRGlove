using VRGlove;
using HW;
using System;


public class MainClass {
  /*
      GloveObserver implementation

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


  public static void Main(string[] args) {
    int[] pattern = new int[] {JOINT.I1, JOINT.I2, JOINT.IM};
    HardwareConnection HW = new Connection(       connector : new Connectors.MacOS( baudRate : 9600, deviceName : "tty.DSDTECHHC-05"),
                                                  stringManager : new SerialStringManagers.SerialStringAccumulator(),
                                                  interpreter : new SerialInterpreters.DelimitedInts() );

    Glove glove = new Glove( hardwareConnection : HW , pattern : pattern);

    GloveObserver bug = new Debugger(pattern);
    glove.RegisterObserver(bug);

    while(true){
        glove.Update();
    }
  }
}
