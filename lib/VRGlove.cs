using HardwareConnection;
using System.Collections.Generic;

namespace VRGlove
{

  /*

  */
  public abstract class GloveObserver
  {
    protected Glove _Subject;
    public Glove Subject
    {
      get => _Subject;
      set
      {
        this._Subject=value;
      }
    }

    // Contract for GloveObserver behavior
    abstract public void Update();

  }

  /*

  */
  public class JOINT {
    public static int I1 = 0; // Index1
    public static int I2 = 1; // ifndex2
    public static int I3 = 2; // Index2
    public static int IM = 3; // IndexMiddle
    public static int M1 = 4; // Middle1
    public static int M2 = 5; // Middle2
    public static int M3 = 6; // Middle3
    public static int MR = 7; // MiddleRing
    public static int R1 = 8; // Ring1
    public static int R2 = 9; // Ring2
    public static int R3 = 10; // Ring3
    public static int RP = 11; // RingPinky
    public static int P1 = 12; // Pinky1
    public static int P2 = 13; // Pinky2
    public static int P3 = 14; // Pinky3
  }

  /*
      VR Glove
      Software manifestation of glove.
      Glove has values for flexion of knuckles and joints.
      Multiple observers will listen to a single VR Glove to interpret and use its values.
      A VR Glove has a hardware manifestion which it derives its values from.
  */
  public class Glove
  {

    private Connection Hardware;

    private List<GloveObserver> Observers;

    private int[] Pattern;

    private Dictionary<int, int> _Joints = new Dictionary<int,int>() {
      {JOINT.I1, 0},
      {JOINT.I2, 0},
      {JOINT.I3, 0},
      {JOINT.IM, 0},
      {JOINT.M1, 0},
      {JOINT.M2, 0},
      {JOINT.M3, 0},
      {JOINT.MR, 0},
      {JOINT.R1, 0},
      {JOINT.R2, 0},
      {JOINT.R3, 0},
      {JOINT.RP, 0},
      {JOINT.P1, 0},
      {JOINT.P2, 0},
      {JOINT.P3, 0},
    };

    public Glove(Connection hardwareConnection, int[] pattern )
    {
      this.Hardware = hardwareConnection;
      this.Pattern = pattern;
      Observers = new List<GloveObserver>();
    }

    public void RegisterObserver(GloveObserver o)
    {
      o.Subject = this;
      Observers.Add(o);
    }

    public void Update()
    {
      int[] jointValues = Hardware.GetValues();
      int size = jointValues.Length;
      for(int idx=0; idx<size; idx++)
      {
        int joint = this.Pattern[idx];
        this._Joints[ joint ] = jointValues[idx];
      }

      foreach (GloveObserver o in Observers)
      {
        o.Update();
      }
    }

    public int get(int joint)
    {
      return _Joints[joint];
    }
  }
}
