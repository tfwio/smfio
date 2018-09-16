/* Date: 11/12/2005 - Time: 4:19 PM */
using System;
namespace on.smfio
{
  public class TimeUtil
  {
    const string DefaultMBTFormat = "{0:##,###,###,000}:{1:0#}:{2:00#}";
    public static string GetMBT(long pulse, int division, bool plusOne = true, string filter = DefaultMBTFormat)
    {
      int plusmode = plusOne ? 1 : 0;
      double value = (double)pulse;
      var M = Convert.ToInt32(Math.Floor(value / (division * 4.0)) + plusmode);
      var B = Convert.ToInt32((Math.Floor(value / division) % 4) + plusmode);
      var T = (int)(pulse % division);
      return string.Format(filter, M, B, T);
    }
    public static double GetSeconds(int division, double tempo, long pulse, double sec = 0.0)
    {
      return ((60.0 / tempo) * ((double)(pulse) / division)) + sec;
    }
    public static string GetSSeconds(double seconds)
    {
      var T = TimeSpan.FromSeconds(seconds);
      return string.Format("{0:00}:{1:00}:{2:00}.{3:00000}", T.Hours, T.Minutes, T.Seconds, T.Milliseconds);
    }

  }
}
