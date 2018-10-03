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
using System;
namespace on.smfio.Common
{
	/// <summary>
	/// 16-bit message type
	/// 
	/// These enum values are specifically used to detect number ranges.
	/// 
	/// I really have 
	/// 
	/// </summary>
	public enum ChMessageU16 : ushort
	{
		/// 0x8000
		NoteOff = Stat8.NoteOn << 8,
		/// 0x9000
		NoteOn = ChMessage8.NoteOn << 8,
		/// 0xA000
		KeyAftertouch = ChMessage8.KeyAftertouch << 8,
		/// 0xB000
		ControlChange = ChMessage8.ControlChange << 8,
		/// 0xC000
		ProgramChange = ChMessage8.ProgramChange << 8,
		/// 0xD000
		ChannelAftertouch = ChMessage8.ChannelAftertouch << 8,
		/// 0xE000
		PitchBend = ChMessage8.PitchBend << 8,
		/// 0xF000
		SystemMessage = ChMessage8.SystemMessage << 8,
		/// 0xF100
		SystemCommonLo = ChMessage8.SystemCommonLo << 8,
		/// 0xF700
		SystemCommonHi = ChMessage8.SystemCommonHi << 8,
		/// 0xF800
		SystemRealtimeLo = ChMessage8.SystemRealtimeLo << 8,
		/// 0xFF00
		SystemRealtimeHi = ChMessage8.SystemRealtimeHi << 8,
		/// 0xFF00
		SystemMessageMax = ChMessage8.SystemMessageMax << 8,
		// 0x10000
		// SystemMaximum = 0x10000, // 0x100 * 0x100
	}
}
