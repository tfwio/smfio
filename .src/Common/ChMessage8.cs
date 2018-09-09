#region Info
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
#endregion
#region Using
using System;
#endregion

namespace on.smfio.Common
{
	public enum ChMessage8 : byte
	{
		/// 0x80
		NoteOff = 0x80,
		/// 0x90
		NoteOn = 0x90,
		/// 0xA0
		KeyAftertouch = 0xA0,
		/// 0xB0
		ControlChange = 0xB0,
		/// 0xC0
		ProgramChange = 0xC0,
		/// 0xD0
		ChannelAftertouch = 0xD0,
		/// 0xE0
		PitchBend = 0xE0,
		/// 0xF0
		SystemMessage = 0xF0,
		/// 0xF1
		SystemCommonLo = 0xF1,
		/// 0xF7
		SystemCommonHi = 0xF7,
		/// 0xF8
		SystemRealtimeLo = 0xF8,
		/// 0xFF
		SystemRealtimeHi = 0xFF,
		// 0xFF
		SystemMessageMax = SystemRealtimeHi,
	}
}
