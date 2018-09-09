#region User/License
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
using System.Collections.Generic;

using System.Drawing;
using on.smfio.Common;
using on.smfio.chunk;
using MetaMsg32 = on.smfio.Common.MetaMsg32;

#endregion
namespace on.smfio.Common
{

	public class MetaHelpers
	{
		#region const

		#region constant Integers
//		/// x00
//		public const int	meta_seq_num	= 0x00;
//		/// x01
//		public const int	meta_text		= 0x01;
//		/// x02
//		public const int	meta_copy		= 0x02;
//		/// x03
//		public const int	meta_seq_nam	= 0x03;
//		/// x04
//		public const int	meta_inst_nam	= 0x04;
//		/// x05
//		public const int	meta_lyric		= 0x05;
//		/// x06
//		public const int	meta_marker		= 0x06;
//		/// x07
//		public const int	meta_cue		= 0x07;
//		/// x20
//		public const int	meta_chanel		= 0x20;
//		/// x21
//		public const int	meta_port		= 0x21;
//		/// x51
//		public const int	meta_tempo		= 0x51;
//		/// x54
//		public const int	meta_smpte		= 0x54;
//		/// x58
//		public const int	meta_tsig		= 0x58;
//		/// x59
//		public const int	meta_ksig		= 0x59;
//		/// x2F
//		public const int	meta_endtrack	= 0x2F;
//		/// x7F
//		public const int	meta_sysex		= 0x7F;
		#endregion
		
		#region Constant Strings
		public const string	str_seq_num		= "Sequence Number";
		public const string	str_text		= "Text Event";
		public const string	str_copy		= "Copyright";
		public const string	str_seq_nam		= "Sequence/Track Name";
		public const string	str_inst_nam	= "Instrument Name";
		public const string	str_lyric		= "Lyric Marker";
		public const string	str_marker		= "Marker";
		public const string	str_cue			= "Cue Point";
		public const string	str_chanel		= "Chanel";
		public const string	str_port		= "Port";
		public const string	str_tempo		= "Tempo";
		public const string	str_smpte		= "Smpte Offset";
		public const string	str_tsig		= "Time Signature";
		public const string	str_ksig		= "Key Signature";
		public const string	str_endtrack	= "End Track";
		public const string	str_sysex		= "System Message";
		#endregion
		
		#endregion
		
		/// our format parameter requires a tag: $(Meta) where any value parameter would be
		static public string MetaNameFormat(MetaMsg32 id, string format, params object[] values)
		{
			return string.Format(format,values)
				.Replace("$(Meta)",MetaName(id));
		}
		static public string MetaNameFormat(MetaMsgU16FF id, string format, params object[] values)
		{
			int bits = (int)id & 0x00FF; // id - 0xFF00;
			return string.Format(format,values).Replace("$(Meta)",MetaName((MetaMsg32)bits));
		}
		
		static public string MetaNameFF(int id) { return MetaName((MetaMsgU16FF)id); }
		static public string MetaName8(int id) { return MetaName((MetaMsg32)id); }
		static public string MetaName(MetaMsg32 id)
		{
			switch (id)
			{
					case MetaMsg32.EndOfTrack: return on.smfio.Common.ChannelType.EndTrack;
					case MetaMsg32.SequenceNo: return on.smfio.Common.ChannelType.SequenceNumber;
					case MetaMsg32.Text: return on.smfio.Common.ChannelType.TextEvent;
					case MetaMsg32.Copyright: return on.smfio.Common.ChannelType.Copyright;
					case MetaMsg32.SequenceName: return on.smfio.Common.ChannelType.SequenceName;
					case MetaMsg32.InstrumentName: return on.smfio.Common.ChannelType.InstrumentName;
					case MetaMsg32.Lyric: return on.smfio.Common.ChannelType.LyricMarker;
					case MetaMsg32.Marker: return on.smfio.Common.ChannelType.Marker;
					case MetaMsg32.Cue: return on.smfio.Common.ChannelType.CuePoint;
					case MetaMsg32.Chanel: return on.smfio.Common.ChannelType.Channel;
					case MetaMsg32.Port: return on.smfio.Common.ChannelType.Port;
					case MetaMsg32.Tempo: return on.smfio.Common.ChannelType.Tempo;
					case MetaMsg32.SMPTE: return on.smfio.Common.ChannelType.SMPTE;
					case MetaMsg32.TimeSignature: return on.smfio.Common.ChannelType.TimeSignature;
					case MetaMsg32.KeySignature: return on.smfio.Common.ChannelType.KeySignature;
					case MetaMsg32.SystemExclusive: return on.smfio.Common.ChannelType.SYSEX;
					case MetaMsg32.SystemSpecific: return on.smfio.Common.ChannelType.SYSSPF;
					//case MetaMsg32.SystemExclusive: return on.smfio.Common.ChannelType.SYSEX;
					default: return "UNKNOWN MESSAGE";
			}
		}
		
		static public string MetaName(MetaMsgU16FF id)
		{
			return MetaName((MetaMsg32)((int)id & 0x00FF));
		}
		
		static public Dictionary<string,Color> Colors
		{
			get
			{
				Dictionary<string,Color> dic = new Dictionary<string,Color>();
				dic.Add("red",Color.Red);
				dic.Add("meta",Color.Red);
				dic.Add("white",Color.White);
				dic.Add("chanel",Color.Red);
				dic.Add("225",Color.FromArgb(225,255,225));
				dic.Add("rse1",Color.FromArgb(225,255,225));
				dic.Add("rse2",Color.FromArgb(0,127,255));
				dic.Add("rse3",Color.FromArgb(125,225,125));
				dic.Add("tempo",Color.FromArgb(225,235,225));
				dic.Add("tsig",Color.FromArgb(225,235,225));
				dic.Add("ksig",Color.FromArgb(225,235,225));
				dic.Add("end",Color.FromArgb(125,135,125));
				dic.Add("ssx",Color.FromArgb(125,135,125));
				return dic;
			}
		}

		#region Moved
		#region Meta helpers
		
		/// Seq No
		static public string meta_FF00(byte one, byte two) { return string.Format("Sequence Number: {0} or {0:X2}",Convert.ToUInt16(one << 8 | two)); }
		
		/// ?
		static public string meta_FF20(smf_mtrk track, int pos1) { return string.Format("{0}",track.track[pos1]); }
		
		const string msg_time_ms_pqn = "mspqn: {0:###,###,###,##0}/{1:###,###,###,##0} = {2}";
		
		/// string format milliseconds per quarter
		static public string meta_FF51(int num) { return string.Format(msg_time_ms_pqn, 60000000, num, 60000000/num ); }
		
		/// ?
		static public string meta_FF54() { return string.Format("SMPTE Offset"); }
		
		/// Midi Time Signature
		static public string meta_FF58(smf_mtrk track, int offset, params int[] positions)
		{
			return string.Format(
				"{0}/{1}, clocks: {2}, 32nds:{3}",
				track.track[offset+3],
				Math.Pow(-track.track[offset+4],2),
				track.track[offset+5],
				track.track[offset+6],
				track.Get32Bit(offset)
			);
		}
		/// Key Signature
		static public string meta_FF59(smf_mtrk track,int offset, params int[] positions)
		{
			return string.Format(
				"Key Signature: {0} {1}",
				(KeySignatureType)track.track[offset+3],
				track.track[offset+4]==0?"Major":"Minor"
			);
		}
		
		/// <returns>End Of Track“FIN”</returns>
		static public string meta_FF2F() { return string.Format("FIN"); }
		
		/// sysex
		static public string meta_FF7F() { return string.Format("{0}",null); }
		
		/// ?
		static public string meta_FFA() { return string.Empty; }
		#endregion

		#endregion
	}
}
