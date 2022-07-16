using HardwareConnection;
using System;

// Implementations of SerialInterpreter interface
namespace SerialInterpreters
{

  /*
      Null Implementation
  */
  public class Null : HardwareConnection.SerialInterpreter
  {
    public int ValueFrom(string serialString)
    {
      // do nothing
      return 0;
    }
  }

  public class DelimitedInts : HardwareConnection.SerialInterpreter
  {
    public int ValueFrom(string serialString)
    {
      return System.Convert.ToInt32(serialString);
    }
  }
}
