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
using System.Collections.Generic;
using System.Drawing;

using on.smfio.chunk;
namespace on.smfio
{
	public static class MetaHelpers
	{
		static public string MetaName(StatusByte id)
		{
			switch (id)
			{
					case StatusByte.EndOfTrack:          return StatusString.EndTrack;
					case StatusByte.SequenceNumber:      return StatusString.SequenceNumber;
					case StatusByte.Text:                return StatusString.TextEvent;
					case StatusByte.Copyright:           return StatusString.Copyright;
					case StatusByte.SequenceName:        return StatusString.SequenceName;
					case StatusByte.InstrumentName:      return StatusString.InstrumentName;
					case StatusByte.Lyric:               return StatusString.LyricMarker;
					case StatusByte.Marker:              return StatusString.Marker;
          case StatusByte.Cue:                 return StatusString.CuePoint;
          case StatusByte.MetaStrFF08:         return StatusString.MetaStrFF08;
          case StatusByte.MetaStrFF09:         return StatusString.MetaStrFF09;
          case StatusByte.MetaStrFF0A:         return StatusString.MetaStrFF0A;
          case StatusByte.MetaStrFF0B:         return StatusString.MetaStrFF0B;
          case StatusByte.MetaStrFF0C:         return StatusString.MetaStrFF0C;
					case StatusByte.ChannelPrefix:       return StatusString.Channel;
					case StatusByte.PortMessage:         return StatusString.Port;
					case StatusByte.SetTempo:            return StatusString.Tempo;
					case StatusByte.SMPTEOffset:         return StatusString.SMPTE;
					case StatusByte.TimeSignature:       return StatusString.TimeSignature;
					case StatusByte.KeySignature:        return StatusString.KeySignature;
					case StatusByte.SystemExclusive:     return StatusString.SYSEX;
					case StatusByte.SequencerSpecific:   return StatusString.SYSSPF;
					//case MetaMsg32.SystemExclusive: return on.smfio.Common.ChannelType.SYSEX;
					default: return "UNKNOWN MESSAGE";
			}
		}
		/// <summary>
		/// Convert Int32 to a status byte for translation to StatusString.
		/// </summary>
		/// <param name="msg32"></param>
		static public string MetaNameFF(int msg32)
		{
			return MetaName((StatusByte)((byte)(msg32 & 0xFF)));
		}
		
		/// <summary>
		/// Several incoming messages such as Metadata Text contain 0xFF as a
		/// message prefix which is filtered out here.
		/// 
		/// Filters or erases LSB of 0xABCD such that yields 0xCD.
		/// </summary>
		/// <param name="id"></param>
		static public string MetaName(StatusWord id)
		{
			return MetaName((StatusByte)((int)id & 0x00FF));
		}
		

		#region Moved
		#region Meta helpers
		
		/// Seq No (0xFF00)
		static public string meta_FF00(byte one, byte two) { return string.Format( "Sequence Number: {0} or {0:X2}", Convert.ToUInt16(one << 8 | two)); }
		
		/// string format milliseconds per quarter
		static public string meta_FF51(int num) { return string.Format(StringRes.msg_time_ms_pqn, 60000000.0, num, 60000000.0/num ); }
		
		/// ?
		static public string meta_FF54(IReader reader, int offset) {
			reader.SMPTE_Offset.SetSMPTE(
				reader.FileHandle[reader.ReaderIndex].Data[offset+3],
				reader.FileHandle[reader.ReaderIndex].Data[offset+4],
				reader.FileHandle[reader.ReaderIndex].Data[offset+5],
				reader.FileHandle[reader.ReaderIndex].Data[offset+6],
				reader.FileHandle[reader.ReaderIndex].Data[offset+7]
				);
			return $"{reader.SMPTE_Offset}";
		}
		
		/// Midi Time Signature
		static public string meta_FF58(MTrk track, int offset, params int[] positions)
		{
			return string.Format(
				StringRes.STRING_META_FF58_FMT,
				track[offset+3],
				Math.Pow(-track[offset+4],2),
				track[offset+5],
				track[offset+6],
				track.ReadU32(offset)
			);
		}
		/// Key Signature
		static public string PrintKeysignature(MTrk track,int offset, params int[] positions) {
			return string.Format( "Key Signature: {0} {1}", (Common.KeySignatureType)track[offset+3], track[offset+4]==0?"Major":"Minor" );
		}
		
		/// <returns>End Of Track“FIN”</returns>
		static public string meta_FF2F() { return string.Format("FIN"); }
		
		#endregion

		#endregion
	}

}
