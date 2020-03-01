#region User/License
// oio * 7/17/2012 * 7:56 AM

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion
namespace System
{
  static public class MidiHelper
  {
    public static readonly string[] TwelveToneNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A#", "A", "B#" };

    static public string[] OctaveMacro()
    {
      string[] ax = new string [ 127 ];
      foreach (var i in KeyIndex) ax[i] = $"{TwelveToneNames[i % 12]}{Convert.ToInt32(i / 12) - 1,-3}";
      return ax;
    }
    public static Collections.Generic.IEnumerable<int> KeyIndex { get => GetIntRange(); }
    static Collections.Generic.IEnumerable<int> GetIntRange(int min=0, int max=127) { for (int i=min; i<=max; i++) yield return i; }
    static public bool IsEbony(int keyIndex) { foreach (var i in KeyIndex) if (i == keyIndex) return true; return false; }
    public static readonly int[] ebony_index = { 1, 3, 6, 8, 10 };
    public static readonly bool[] EbonyAndIvory =  { true, false, true, false, true, true, false, true, false, true, false, true };

  }
}


