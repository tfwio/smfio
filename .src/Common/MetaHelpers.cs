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

using on.smfio.chunk;

#endregion
namespace on.smfio.Common
{
  using StringRes = on.smfio.StringRes;

  public class MetaHelpers
	{
		static public string MetaNameFF(int id) { return MetaName((MetaMsgU16FF)id); }
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
		
		/// Seq No (0xFF00)
		static public string meta_FF00(byte one, byte two) { return string.Format( "Sequence Number: {0} or {0:X2}", Convert.ToUInt16(one << 8 | two)); }
		
		/// string format milliseconds per quarter
		static public string meta_FF51(int num) { return string.Format(StringRes.msg_time_ms_pqn, 60000000, num, 60000000/num ); }
		
		/// ?
		static public string meta_FF54() { return string.Format("SMPTE Offset"); }
		
		/// Midi Time Signature
		static public string meta_FF58(smf_mtrk track, int offset, params int[] positions)
		{
			return string.Format(
        StringRes.STRING_META_FF58_FMT,
				track.track[offset+3],
				Math.Pow(-track.track[offset+4],2),
				track.track[offset+5],
				track.track[offset+6],
				track.GetU32(offset)
			);
		}
		/// Key Signature
		static public string meta_FF59(smf_mtrk track,int offset, params int[] positions) {
			return string.Format( "Key Signature: {0} {1}", (KeySignatureType)track.track[offset+3], track.track[offset+4]==0?"Major":"Minor" );
		}
		
		/// <returns>End Of Track“FIN”</returns>
		static public string meta_FF2F() { return string.Format("FIN"); }
		
		#endregion

		#endregion
	}
}
