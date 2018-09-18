/* Date: 11/12/2005 - Time: 4:19 PM */
// #define USEFLOAT
using System;
namespace on.smfio
{
  public class TimeUtil
  {
    const string DefaultMBTFormat = "{0:##,###,###,000}:{1:0#}:{2:00#}";
    public static string GetMBT(long pulse, int division, bool plusOne = true, string filter = DefaultMBTFormat)
    {
      int plusmode = plusOne ? 1 : 0;
      double value = pulse;
      var M = (int)(pulse / (division * 4.0)) + plusmode;
      var B = (int)(pulse / division) % 4 + plusmode;
      var T = (int)(pulse % division);
      return string.Format(filter, M, B, T);
    }
    /// <summary>
    /// Pulses from MBT.
    /// </summary>
    public static long GetMBT(int division, int M, int B, int T, int start=1)
    {
      return ((((M - start) * (4 - start)) + B) * division) + T;
    }
    public static double GetSeconds(int division, double tempo, long pulse, double sec = 0.0)
    {
      return ((60.0 / tempo) * (pulse / division)) + sec;
    }
    public static double GetSeconds(int division, uint muspqn, long pulse, double sec = 0.0)
    {
      return ((muspqn*0.000001) * (pulse / division)) + sec;
    }
    public static string GetSSeconds(double seconds, string filter="{0:00}:{1:00}:{2:00}.{3:000}")
    {
      var T = TimeSpan.FromSeconds(seconds);
      return string.Format(filter, T.Hours, T.Minutes, T.Seconds, T.Milliseconds);
    }

  }
}
