using HW;
using System.Collections.Generic;
using System;
using VRGlove;


public class Glove : VRGlove.VRGlove
{

  private HW.HardwareConnection Hardware;

  private List<GloveObserver> Observers;

  // The pattern in which joints will be interpreted
  // (from serial string received from arduino)
  // for input 466.88.2
  // {JOINT.T1, JOINT.T2, JOINT.T3} interprets values as flexions of the thumb
  // {JOINT.IM, JOINT.I1, JOINT.I2} interprets values as flexion of the first two index finger joints and the Index->middle finger joint
  private int[] JointPattern;

  // Current Values for joints
  // All joints will have a value, even if they do not have a corresponding sensor
  private Dictionary<int, int> _Joints = new Dictionary<int,int>() {
    {JOINT.T1, 0},
    {JOINT.T2, 0},
    {JOINT.T3, 0},
    {JOINT.TI, 0},
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

  public Glove(HW.HardwareConnection hardwareConnection, int[] pattern )
  {
    this.Hardware = hardwareConnection;
    this.JointPattern = pattern;
    Observers = new List<GloveObserver>();
  }


  public int Get(int joint)
  {
    return _Joints[joint];
  }

  /*
      New Listener to add
  */
  public void RegisterObserver(GloveObserver o)
  {
    o.VRGlove = this;
    Observers.Add(o);
  }


  /*
      Get values for joints.
      If connection severed, wait for reconnection.
  */
  public void Update()
  {
    try
    {
      // Get new values
      // Stalls if connection is severed
      int[] inputJointValues = Hardware.GetValues();


      // Store values
      int nJointsMeasured = inputJointValues.Length;
      int correspondingJointNumber;
      for(int idx=0; idx<nJointsMeasured; idx++)
      {
        // Use pattern to interpret
        correspondingJointNumber = this.JointPattern[idx];
        _Joints[ correspondingJointNumber ] = inputJointValues[idx];
      }

      // Done Storing
      // Notify observers of update
      foreach (GloveObserver o in Observers)
      {
        o.Notify();
      }
    }
    catch (IndexOutOfRangeException e)
    {
      /*
        Possible buffer error

        Arduino will continually write to serial buffer while in idle despite not being connected to program.
        Serial buffer may initialize with a few hundred characters in it, exceeding the expected amount.
        Skip these values.
      */
      Console.WriteLine("Index out of range avoided");
    }
    catch (HardwareDisconnectException e)
    {
      throw e;
    }
    catch
    {
      /*
        Failures may arise when picking up values.
        Ignore them, retry later.
      */
    }
  }
}
