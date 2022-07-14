using HardwareConnection;

// Implementations of SerialInterpreter interface
namespace SerialInterpreters {

  /*
      Null Implementation
  */
  public class Null : HardwareConnection.SerialInterpreter {
    public float valueFrom(string serialString) {
      // do nothing
      return 0.0f;
    }
  }
}
