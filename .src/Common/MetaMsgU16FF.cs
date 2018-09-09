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
	/// Message (bits | 0xFF00)
	/// </summary>
	public enum MetaMsgU16FF : ushort
	{
		/// xFF00
		SequenceNo		= 0xFF00|ChannelType.sequence_number_byte,
		/// xFF01
		Text			= 0xFF00|ChannelType.text_byte,
		/// xFF02
		Copyright		= 0xFF00|ChannelType.copy_byte,
		/// xFF03
		SequenceName	= 0xFF00|ChannelType.sequence_name_byte,
		/// xFF04
		InstrumentName	= 0xFF00|ChannelType.instrument_name_byte,
		/// xFF05
		Lyric			= 0xFF00|ChannelType.lyric_byte,
		/// xFF06
		Marker			= 0xFF00|ChannelType.marker_byte,
		/// xFF07
		Cue				= 0xFF00|ChannelType.cue_byte,
		/// xFF20
		Chanel			= 0xFF00|ChannelType.channel_byte,
		/// xFF21
		Port			= 0xFF00|ChannelType.port_byte,
		/// xFF51
		Tempo			= 0xFF00|ChannelType.tempo_byte,
		/// xFF54
		SMPTE			= 0xFF00|ChannelType.smpte_byte,
		/// xFF58
		TimeSignature	= 0xFF00|ChannelType.time_signature_byte,
		/// xFF59
		KeySignature	= 0xFF00|ChannelType.key_signature_byte,
		/// xFF2F
		EndOfTrack		= 0xFF00|ChannelType.end_track_byte,
		/// xFFF0
		SystemExclusive = 0xFF00|ChannelType.sysex_byte,
		/// xFF7F
		SystemSpecific = 0xFF00|ChannelType.sspec_byte,
		
	}

}
