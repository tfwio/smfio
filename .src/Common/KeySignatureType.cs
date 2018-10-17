// oio * 2005-11-12 * 4:19 PM
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

namespace on.smfio.Common
{

  // https://unicode-table.com/en/blocks/musical-symbols/
  public enum KeySignatureType : byte
	{
    /// <summary>C&#x266D;</summary>
    [System.ComponentModel.Description("C\u266D")]
    CFlat = 0xF0, // -7
    /// <summary>G&#x266D;</summary>
    [System.ComponentModel.Description("C\u266D")]
    GFlat = 0xFA, // -6
    /// <summary>D&#x266D;</summary>
    [System.ComponentModel.Description("C\u266D")]
    DFlat = 0xFB, // -5
    /// <summary>A&#x266D;</summary>
    [System.ComponentModel.Description("C\u266D")]
    AFlat = 0xFC, // -4
    /// <summary>E&#x266D;</summary>
    [System.ComponentModel.Description("C\u266D")]
    EFlat = 0xFD, // -3
    /// <summary>B&#x266D;</summary>
    [System.ComponentModel.Description("C\u266D")]
    BFlat = 0xFE, // -2
    /// <summary>F&#x266D;</summary>
    [System.ComponentModel.Description("C\u266D")]
    FFlat = 0xFF, // -1
    [System.ComponentModel.Description("C\u266D")]
    C = 0,        // 0
    [System.ComponentModel.Description("C")]
    GSharp = 1,   // 1
    /// <summary>D#</summary>
    [System.ComponentModel.Description("D#")]
    DSharp = 2,   // 2
    /// <summary>A#</summary>
    [System.ComponentModel.Description("A#")]
    ASharp = 3,   // 3
    /// <summary>E#</summary>
    [System.ComponentModel.Description("E#")]
    ESharp = 4,   // 4
    /// <summary>B#</summary>
    [System.ComponentModel.Description("B#")]
    BSharp =5,    // 5
    /// <summary>F#</summary>
    [System.ComponentModel.Description("F#")]
    FSharp = 6,   // 6
    /// <summary>C#</summary>
    [System.ComponentModel.Description("C#")]
    CSharp = 7,   // 7
	}
}
