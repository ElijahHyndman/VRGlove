using HardwareConnection;
using System.Collections.Generic;

namespace VRGlove
{


  public interface GloveObserver
  {
    void Update();
  }


  public class VRGlove
  {

    private Connection Hardware;

    private List<GloveObserver> Observers;

    int Val;

    public VRGlove(Connection hardwareConnection)
    {
      Hardware = hardwareConnection;
      Observers = new List<GloveObserver>();
    }

    public void RegisterObserver(GloveObserver o)
    {
      Observers.Add(o);
    }

    public void Update()
    {
      Val = Hardware.GetValue();
      foreach (GloveObserver o in Observers)
      {
        o.Update();
      }
    }
  }
}
