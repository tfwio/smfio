/* Date: 11/12/2005 - Time: 4:19 PM */
// #define USEFLOAT
using System;
namespace on.smfio
{
  public static class TimeUtil
  {
    public const double s60 = 60.0;
    public const int    MicroSecond  = 1000000;
    public const double MicroMinute = 60000000.0;
    public const double MicroScale = 0.000001;

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
    public static long GetMBT(int division, int M, int B, int T, int start = 1)
    {
      return ((((M - start) * (4 - start)) + B) * division) + T;
    }
    public static double GetSeconds(int division, double tempo, long pulse, double sec = 0.0)
    {
      return ((s60 / tempo) * ((double)pulse / division)) + sec;
    }
    public static double GetSeconds(int division, uint muspqn, long pulse, double sec = 0.0)
    {
      return (((double)muspqn * MicroScale) * ((double)pulse / division)) + sec;
    }
    public static string GetSSeconds(double seconds, string filter = "{0:00}:{1:00}:{2:00}.{3:000}")
    {
      var T = TimeSpan.FromSeconds(seconds);
      return string.Format(filter, T.Hours, T.Minutes, T.Seconds, T.Milliseconds);
    }

    public static double GetSamples(double pulse, double tempo, int rate, int division)
    {
      return (s60 / tempo * rate) * ((double)pulse / division);
    }

    public static long GetPulses(long samples, int muspqn, int fs, int division)
    {
      return Convert.ToInt64((double)samples / (((muspqn * MicroScale) * fs) * division));
    }

    public static long GetPulses(long samples, double tempo, int fs, int division)
    {
      return Convert.ToInt64((double)samples / ((s60 / tempo * fs) * division));
    }

  }
}
