using VRGlove;
using HardwareConnection;


public class MainClass {
  public static void Main(string[] args) {
    int[] pattern = new int[] {JOINT.I1, JOINT.I2, JOINT.IM};
    Connection HW = new Connection(       connector : new Connectors.MacOS(9600),
                                          interpreter : new SerialInterpreters.DelimitedInts() );

    Glove glove = new Glove( hardwareConnection : HW , pattern : pattern);

    GloveObserver bug = new GloveObservers.Debugger(pattern);
    glove.RegisterObserver(bug);

    while(true){
      glove.Update();
    }
  }
}
