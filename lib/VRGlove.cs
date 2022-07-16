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

    private int _Val;
    public int Val
    {
      get => _Val;
      set { /*Read Only*/ }
    }

    public Glove(Connection hardwareConnection)
    {
      Hardware = hardwareConnection;
      Observers = new List<GloveObserver>();
    }

    public void RegisterObserver(GloveObserver o)
    {
      o.Subject = this;
      Observers.Add(o);
    }

    public void Update()
    {
      _Val = Hardware.GetValue();
      foreach (GloveObserver o in Observers)
      {
        o.Update();
      }
    }
  }
}
