using System.Collections.Generic;
namespace System
{
  static class MidiHelperHelper
  {
    internal static IEnumerable<int> To(this int from, int to, int increment = 1) { for (int i = from; i <= to; i += increment) yield return i; }
    internal static IEnumerable<int> From(this int to, int from, int increment = 1) { for (int i = from; i <= to; i += increment) yield return i; }
  }
  public class MidiHelper
  {

    static string[] TwelveToneNames = new string[]{ "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A#", "A", "B#" };

    static public void GetOctaves(ref string[] notenames)
    {
      int i = 0, j = 0, k;
      string key = string.Empty;
      for (; i <= 127; i++)
      {
        try
        {
          j = i % 12; k = i / 12;
          key = $"{TwelveToneNames[j]}{Convert.ToInt32(k) - 1,-3}";
          notenames[i] = key;
        }
        catch { }
      }
    }

    static public string[] Byze = MidiHelper.OctaveMacro();
    static public string[] OctaveMacro()
    {
      string[] ax = new string [ 127 ];
      // foreach (var i in KeyIndex)
      int i = 0, j = 0, k;
      string key = string.Empty;
      for (; i < 127; i++)
      {
        j = i % 12;
        k = i / 12;
        key = $"{TwelveToneNames[j]}{Convert.ToInt32(k) - 1,-3}";
        ax[i] = key;
      }

      return ax;
    }
    public static IEnumerable<int> KeyIndex { get { for (int i = 0; i <= 127; i += 1) yield return i; } }

    static IEnumerable<int> GetIntRange(int min, int max) { for (int i = min; i <= max; i += 1) yield return i; }

    static public bool IsEbony(int keyIndex) { foreach (var i in ebony_index) { if (ebony_index[i] == (keyIndex % 12)) return true; } return false; }

    public static readonly int[] ebony_index = { 1, 3, 6, 8, 10 };
    public static readonly bool[] EbonyAndIvory =  { true, false, true, false, true, true, false, true, false, true, false, true };

  }
}


