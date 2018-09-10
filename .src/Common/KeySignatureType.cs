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
		SevenFlats_C_Flat = 0xF0,
		SixFlats_G_Flat = 0xFA,
		FiveFlats_D_Flat = 0xFB,
		FourFlats_A_Flat = 0xFC,
		ThreeFlats_E_Flat = 0xFD,
		TwoFlats_B_Flat = 0xFE,
		OneFlats_F = 0xFF,
		C = 0,
		OneSharp_G = 1,
		TwoSharp_D = 2,
		ThreeSharp_A = 3,
		FourSharp_E = 4,
		FiveSharp_B =5,
		SixSharp_F = 6,
		SevenSharp_C = 7,
	}
}
