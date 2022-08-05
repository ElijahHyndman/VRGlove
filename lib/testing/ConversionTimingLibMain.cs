using VRGlove;
using HW;
using System;


public class MainClass {

  public static void Main(string[] args) {
    int N = 1000000; // total iterations

    int[] pattern = new int[] {JOINT.I1, JOINT.I2, JOINT.IM};

    string message = "";
    message += ((char)108);
    message += '.';
    message += ((char)0);
    message += '.';
    message += ((char)45) ;
    message += '\n';
    Console.WriteLine(message);

    SerialStringManager fauxManager = new SerialStringManagers.ConstantMessage(constantMessage : message);
    Connector fauxConnector = new Connectors.FauxConnector();

    ConnectionTimingWrapper HW = new ConnectionTimingWrapper (
                            new Connection(       connector : fauxConnector,
                                                  stringManager : fauxManager,
                                                  interpreter : new SerialInterpreters.DelimitedUTFChars() )
                            );

    VRGlove.VRGlove glove = new Glove( hardwareConnection : (HardwareConnection)HW , pattern : pattern);



    // The first few runs are outliers by a fw orders of magnitude. Run through them
    for(int i=0; i<10; i++) glove.Update();

    // Get Samples
    double[] samples = new double[N];
    double totalTime=0.0, mean=0.0;
    for(int iteration=0; iteration<N; iteration++)
    {
      // Update once
      glove.Update();

      samples[iteration] = HW.NanoSecondsElapsed;
      totalTime += samples[iteration];

      // Measure HardwareConnection.GetValues time elapsed
      // string elapsedTime = String.Format("{0:00}ns", samples[iteration] );
      // Console.WriteLine("RunTime " + elapsedTime);
    }

    // Calculate mean
    mean = totalTime / N;

    // Calculate variance
    double varianceNUM = 0.0;
    for(int iteration=0; iteration<N; iteration++)
    {
      double datapoint = samples[iteration];
      varianceNUM += Math.Pow( (datapoint-mean) , 2.0);
    }
    double variance = varianceNUM / N;
    double stdDeviation = Math.Pow(variance, 0.5);

    Console.WriteLine("Average Time: {0:00}ns" , mean);
    Console.WriteLine("Variance: {0:00}ns", variance);
    Console.WriteLine("Standard Deviation: {0:00}ns", stdDeviation);
  }
}
