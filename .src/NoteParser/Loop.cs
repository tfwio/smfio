#region User/License
// oio * 7/31/2012 * 11:12 PM

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

using System;
using System.Drawing;

namespace on.smfio.util
{
  public class LoopPoint : Loop
  {
    public FloatPoint Mouse { get; set; }
    public int BarOffset { get; set; }
    public int BarStart { get; set; }
    public int BarLength { get; set; }
  }
  /// <summary>
  /// SAMPLE region
  /// </summary>
  public class Loop
  {

    public double Begin { get; set; }

    public double Length { get; set; }

    /// <summary>
    /// set length in stead.
    /// </summary>
    [System.Xml.Serialization.XmlIgnore]
    public double End { get { return Begin + Length; } set { Length = value - Begin; } }

    /// <summary>can not be modified.</summary>
    static public readonly Loop Empty = new Loop() { Begin = 0, End = 0 };
  }
}
