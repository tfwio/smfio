using System;
namespace on.smfio
{
  public class Log
  {
    public static void ErrorMessage(string format, params object[] arg)
    {
      #if CONSOLE
      MessageBox.Show(string.Format(format, arg), "Press OK to continue", MessageBoxButtons.OK);
      #else
      Console.WriteLine("Unknown Message: {0}", arg);
      Console.WriteLine("Press a key to continue...");
      Console.ReadKey();
      #endif
    }
  }
}
