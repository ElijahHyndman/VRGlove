using HardwareConnection;
using System;
using VRGlove;

// Implementations of SerialInterpreter interface
namespace SerialInterpreters
{

  /*
      Null Implementation
  */
  public class Null : HardwareConnection.SerialInterpreter
  {
    public int[] ValuesFrom(string serialString)
    {
      // Set all joints to zero
      return new int[] {0};
    }
  }

  public class DelimitedInts : HardwareConnection.SerialInterpreter
  {
    private string delimiter;

    public DelimitedInts(string delimiter = ".")
    {
      this.delimiter = delimiter;
    }

    public int[] ValuesFrom(string serialString)
    {
      string[] tokens = serialString.Split(this.delimiter);
      int size = tokens.Length;
      int[] values = new int[size];
      for (int idx = 0; idx < size; idx ++)
      {
        values[idx] = System.Convert.ToInt32( tokens[idx] );
      }
      return values;
    }
  }
}
