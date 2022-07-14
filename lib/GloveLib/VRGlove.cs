using HardwareConnection;

namespace VRGlove {
  public interface Observer {
    void Update();
  }

  public class VRGlove {
    private HardwareConnection hardware;
    private Observer[] observers;

    int val;

    public VRGlove(HardwareConnection hardwareConnection) {
      hardware = hardwareConnection;
    }

    public void RegisterObserver(Observer o) {

    }

    public void Update() {
      val = hardware.GetValue();
      observers.Update();
    }
  }
}
