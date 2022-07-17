using VRGlove;
using HardwareConnection;


public class MainClass {
  public static void Main(string[] args) {
    Connection HW = new Connection(       connector : new Connectors.MacOS(9600),
                                          interpreter : new SerialInterpreters.DelimitedInts() );
    int[] pattern = new int[] {JOINT.IM};

    Glove glove = new Glove( hardwareConnection : HW , pattern : pattern);

    GloveObserver bug = new GloveObservers.Debugger(JOINT.IM);
    glove.RegisterObserver(bug);

    while(true){
      glove.Update();
    }
  }
}
