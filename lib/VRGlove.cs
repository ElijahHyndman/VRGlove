using HardwareConnection;
using System.Collections.Generic;
using System;

namespace VRGlove
{

  /*
      Observer Contract
      Subject-Observer design pattern.
      Allows multiple outputs and interpretations to exist for a single glove entity.
  */
  public abstract class GloveObserver
  {
    protected Glove _VRGlove;
    public Glove VRGlove
    {
      get => _VRGlove;
      set
      {
        this._VRGlove=value;
      }
    }

    // Contract for GloveObserver behavior
    abstract public void Notify();

  }

  /*
      Constants
      Treated like enums, independent of underlying datastructure.
      May change the datatype in the future.
  */
  public class JOINT {
    public static int T1 = 0; // Thumb1
    public static int T2 = 1; // Thumb2
    public static int T3 = 2; // Thumb3
    public static int TI = 3; // ThumbIndex
    public static int I1 = 4; // Index1
    public static int I2 = 5; // ifndex2
    public static int I3 = 6; // Index2
    public static int IM = 7; // IndexMiddle
    public static int M1 = 8; // Middle1
    public static int M2 = 9; // Middle2
    public static int M3 = 10; // Middle3
    public static int MR = 11; // MiddleRing
    public static int R1 = 12; // Ring1
    public static int R2 = 13; // Ring2
    public static int R3 = 14; // Ring3
    public static int RP = 15; // RingPinky
    public static int P1 = 16; // Pinky1
    public static int P2 = 17; // Pinky2
    public static int P3 = 18; // Pinky3
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

    // The pattern in which joints will be interpreted in the SerialInput string from arduino
    private int[] JointPattern;

    // All joints will have a value, even if they do not have a corresponding sensor
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
      this.JointPattern = pattern;
      Observers = new List<GloveObserver>();
    }

    public void RegisterObserver(GloveObserver o)
    {
      o.VRGlove = this;
      Observers.Add(o);
    }

    public void Update()
    {
      try
      {
        // Get new values
        int[] jointValues = Hardware.GetValues();
        int size = jointValues.Length;
        for(int idx=0; idx<size; idx++)
        {
          int joint = this.JointPattern[idx];
          this._Joints[ joint ] = jointValues[idx];
        }

        // Notify Observers
        foreach (GloveObserver o in Observers)
        {
          o.Notify();
        }
      }
      catch (IndexOutOfRangeException e)
      {
        /*
          Arduino will continually write to serial buffer while in idle despite not being connected to program.
          Serial buffer may begin with a few hundred characters in it, exceeding the expected amount.
          Skip these values.
        */
        Console.WriteLine("Index out of range avoided");
      }
    }

    public int Get(int joint)
    {
      return _Joints[joint];
    }
  }
}
