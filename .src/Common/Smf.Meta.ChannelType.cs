#region Info
#endregion
#region Using
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
using System;

namespace on.smfio.Common
{
	/// <summary>Channel Helper Class</summary>
	static public class ChannelType
	{
		static public string GetString08(MetaMsg8 id)
		{
			switch (id)
			{
					case MetaMsg8.EndOfTrack: return on.smfio.Common.ChannelType.EndTrack;
					case MetaMsg8.SequenceNo: return on.smfio.Common.ChannelType.SequenceNumber;
					case MetaMsg8.Text: return on.smfio.Common.ChannelType.TextEvent;
					case MetaMsg8.Copyright: return on.smfio.Common.ChannelType.Copyright;
					case MetaMsg8.SequenceName: return on.smfio.Common.ChannelType.SequenceName;
					case MetaMsg8.InstrumentName: return on.smfio.Common.ChannelType.InstrumentName;
					case MetaMsg8.Lyric: return on.smfio.Common.ChannelType.LyricMarker;
					case MetaMsg8.Marker: return on.smfio.Common.ChannelType.Marker;
					case MetaMsg8.Cue: return on.smfio.Common.ChannelType.CuePoint;
					case MetaMsg8.Chanel: return on.smfio.Common.ChannelType.Channel;
					case MetaMsg8.Port: return on.smfio.Common.ChannelType.Port;
					case MetaMsg8.Tempo: return on.smfio.Common.ChannelType.Tempo;
					case MetaMsg8.SMPTE: return on.smfio.Common.ChannelType.SMPTE;
					case MetaMsg8.TimeSignature: return on.smfio.Common.ChannelType.TimeSignature;
					case MetaMsg8.KeySignature: return on.smfio.Common.ChannelType.KeySignature;
					case MetaMsg8.SystemExclusive: return on.smfio.Common.ChannelType.SYSEX;
					default: return "UNKNOWN MESSAGE";
			}
		}
		// 
		// -------------------
		#region Message Byte
		
		/// x00 (unsigned byte)
		public const byte	sequence_number_byte	= 0x00;
		/// x01
		public const byte	text_byte		= 0x01;
		/// x02
		public const byte	copy_byte		= 0x02;
		/// x03
		public const byte	sequence_name_byte	= 0x03;
		/// x04
		public const byte	instrument_name_byte	= 0x04;
		/// x05
		public const byte	lyric_byte		= 0x05;
		/// x06
		public const byte	marker_byte		= 0x06;
		/// x07
		public const byte	cue_byte		= 0x07;
		/// x20
		public const byte	channel_byte		= 0x20;
		/// x21
		public const byte	port_byte		= 0x21;
		/// x2F
		public const byte	end_track_byte	= 0x2F;
		/// x51
		public const byte	tempo_byte		= 0x51;
		/// x54
		public const byte	smpte_byte		= 0x54;
		/// x58
		public const byte	time_signature_byte		= 0x58;
		/// x59
		public const byte	key_signature_byte		= 0x59;
		/// xF0
		public const byte	sysex_byte		= 0xF0;
		/// x7F
		public const byte	sspec_byte		= 0x7F;
		/// xA0
		public const byte unkn_a0_byte		= 0xA0;
		
		#endregion
		// 
		// -------------------
		#region Message String
		
		/// Sequence Name
		public const string	SequenceNumber		= "Sequence Number";
		/// Text Event
		public const string	TextEvent		= "Text Event";
		/// Copyright
		public const string	Copyright		= "Copyright";
		/// Sequence Name
		public const string	SequenceName		= "Sequence/Track Name";
		/// Instrument Name
		public const string	InstrumentName	= "Instrument Name";
		/// Lyric Marker
		public const string	LyricMarker		= "Lyric Marker";
		/// Marker
		public const string	Marker		= "Marker";
		/// Cue
		public const string	CuePoint			= "Cue Point";
		/// Cue Point
		public const string	Channel		= "Chanel";
		/// Port
		public const string	Port		= "Port";
		/// Tempo
		public const string	Tempo		= "Tempo";
		/// SMPTE
		public const string	SMPTE		= "Smpte Offset";
		/// Time Signature
		public const string	TimeSignature		= "Time Signature";
		/// Key Signature
		public const string	KeySignature		= "Key Signature";
		/// End Of Track
		public const string	EndTrack	= "End Track";
		/// System Exclusive Message
		public const string	SYSEX	 	= "System Message";
		public const string	SYSSPF		= "System Specific";
		#endregion

	}
}
