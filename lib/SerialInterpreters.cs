using HW;
using System;
using VRGlove;

// Implementations of SerialInterpreter interface
namespace SerialInterpreters
{

  /*
      Null Implementation
  */
  public class Null : HW.SerialInterpreter
  {
    public int[] ValuesFrom(string serialString)
    {
      // Set all joints to zero
      return new int[] {0};
    }
  }


  /*
      Delimited Implentation
      SerialInput which is list of integers separated by a delimiter
  */
  public class DelimitedInts : HW.SerialInterpreter
  {
    private string delimiter;

    public DelimitedInts(string delimiter = ".")
    {
      this.delimiter = delimiter;
    }

    public int[] ValuesFrom(string serialString)
    {
      // Get values from delimited string
      string[] tokens = serialString.Split(this.delimiter);
      int size = tokens.Length;
      int[] values = new int[size];

      // Convert to integers
      for (int idx = 0; idx < size; idx ++)
      {
        values[idx] = System.Convert.ToInt32( tokens[idx] );
      }
      return values;
    }
  }

  public class DelimitedUTFChars : HW.SerialInterpreter
  {
    private string delimiter;

    public DelimitedUTFChars(string delimiter = ".")
    {
      this.delimiter = delimiter;
    }

    public int[] ValuesFrom(string serialString)
    {
      // Get values from delimited string
      string[] tokens = serialString.Split(this.delimiter);
      int size = tokens.Length;
      int[] values = new int[size];

      // Convert to integers
      for (int idx = 0; idx < size; idx ++)
      {
        values[idx] = (int) tokens[idx][0];
      }
      return values;
    }
  }
}
