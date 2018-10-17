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
	public enum KeySignatureType : byte
	{
		CFlat = 0xF0, // -7
		GFlat = 0xFA, // -6
		DFlat = 0xFB, // -5
		AFlat = 0xFC, // -4
		EFlat = 0xFD, // -3
		BFlat = 0xFE, // -2
		FFlat = 0xFF, // -1
		C = 0,        // 0
		GSharp = 1,   // 1
		DSharp = 2,   // 2
		ASharp = 3,   // 3
		ESharp = 4,   // 4
		BSharp =5,    // 5
		FSharp = 6,   // 6
		CSharp = 7,   // 7
	}
}
