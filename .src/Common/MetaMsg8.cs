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
	/// <summary>
	/// 8-bit message type
	/// </summary>
	public enum MetaMsg8 : byte
	{
		/// x00
		SequenceNo		= ChannelType.sequence_number_byte,
		/// x01
		Text			= ChannelType.text_byte,
		/// x02
		Copyright		= ChannelType.copy_byte,
		/// x03
		SequenceName	= ChannelType.sequence_name_byte,
		/// x04
		InstrumentName	= ChannelType.instrument_name_byte,
		/// x05
		Lyric			= ChannelType.lyric_byte,
		/// x06
		Marker			= ChannelType.marker_byte,
		/// x07
		Cue				= ChannelType.cue_byte,
		/// x20
		Chanel			= ChannelType.channel_byte,
		/// x21
		Port			= ChannelType.port_byte,
		/// x51
		Tempo			= ChannelType.tempo_byte,
		/// x54
		SMPTE			= ChannelType.smpte_byte,
		/// x58
		TimeSignature	= ChannelType.time_signature_byte,
		/// x59
		KeySignature	= ChannelType.key_signature_byte,
		/// x2f
		EndOfTrack		= ChannelType.end_track_byte,
		/// x7f
		SystemExclusive = ChannelType.sysex_byte,
		/// 0xA0
		Unknown0xA0		= ChannelType.sysex_byte,
	}
}
