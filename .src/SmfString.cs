/* Date: 11/12/2005 Time: 4:19 PM */
using System;

namespace on.smfio
{
  /// <summary>
  /// Contains several utility functions to convert binary data
  /// to a human readable format.
  /// </summary>
  static public class SmfString
  {
    public static EnumFile ControlMap, PatchMap, DrumMap;

    static public readonly string[] KeysFlat = new string[] { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };
    static public readonly string[] KeysSharp = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

    static public string GetKeySharp(int value) { return KeysSharp[value % 12]; }
    static public string GetKeyFlat(int value) { return KeysFlat[value % 12]; }

    // FIXME: -1
    static public int GetOctave(int value) { return (int)Math.Floor((double)value / 12)-1; }
  }
}
