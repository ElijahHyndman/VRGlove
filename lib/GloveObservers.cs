using HardwareConnection;
using VRGlove;

namespace GloveObservers
{
  /*
      Objects looking to be notified upon updates to the VRGlove to use its values
      - place in this class
      - implement VRGlove.GloveObserver interface behavior
  */

  class NullObserver : GloveObserver
  {
    public override void Notify()
    {
      // do nothing
    }
  }
}
