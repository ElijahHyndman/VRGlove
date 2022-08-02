using HardwareConnection;
using System.Collections.Generic;
using System;

namespace VRGlove
{

  /*
      GloveObserver

      Notified upon new values appearing on glove.
      Allows multiple processes to interact with VRGlove object.

      Observer Contract
      (Subject-Observer design pattern)

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
      Global Constants
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
    public static int NUMJOINT = 19;

    public static Dictionary<int, string> Names = new Dictionary<int, string>() {
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
  };


  /*
      VR Glove
      Software manifestation of glove.

      -Glove has values representing flexion of knuckles and joints.
      -Multiple observers will listen to a single VR Glove to use its values
      -A VR Glove has a hardware manifestion where it pulls values from
  */
  public interface VRGlove
  {
    void RegisterObserver(GloveObserver o);
    int Get(int joint);
    void Update();
  }

}
