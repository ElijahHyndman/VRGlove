using VRGlove;
using HardwareConnection;


public class MainClass {
  public static void Main(string[] args) {
    Connection HW = new Connection(       connector : new Connectors.MacOS(9600),
                                          interpreter : new SerialInterpreters.DelimitedInts() );
    GloveObserver bug = new GloveObservers.Debugger();

    Glove glove = new Glove( hardwareConnection : HW );
    glove.RegisterObserver(bug);

    while(true){
      glove.Update();
    }
  }
}
