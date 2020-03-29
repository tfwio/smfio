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
    /// <summary>
    /// Converts 'RAW' integer pitch-wheel value where MSB and LSB
    /// values range from 0-127 and are stored in a two-byte integer (short).  
    /// [I.E.] 0x2001 in MSB/LSB = {MSB: [hex] 0x01 or [int] 1, LSB: [hex] 0x20 or [int] 32}.
    /// 
    /// The integer value is interpreted in Little Endian format.
    /// </summary>
    /// <param name="value">Two 7-bit byte values combined as a little-endian signed short integer.</param>
    /// <returns></returns>
    static public short ConvertPitchWheelFromRaw(short value)
    {
      int lsb = 0, msb = 0;
      lsb = (value & 0xF80) >> 7;
      msb = value & 0x7F;
      return Convert.ToInt16(msb + lsb);
    }
    // TODO: Check work
    /// <summary>
    /// Convert integer value to msb/lsb bytes.
    /// 
    /// Value may be in the range of 0 > 8192 > 16,383  
    /// where value = -8193 to 0 to 8192 or 
    /// </summary>
    /// <param name="value">integer input value.</param>
    /// <param name="msb">[out] MSB (lo byte)</param>
    /// <param name="lsb">[out] LSB (hi byte)</param>
    /// <returns>Returns a byte array with two bytes: byte[0]=MSB, byte[1]=LSB.</returns>
    /// <remarks>
    /// NOTE: this does not convert the RAW byte value of two combined
    /// bytes as found in the MIDI Message it self.
    /// 
    /// `LSB = (byte)(Convert.ToInt32(value &amp; 0xF80) >> 7)`  
    /// `MSB = Convert.ToByte(value &amp; 0x7F)`
    /// </remarks>
    static public byte[] GetPitchWheelBytes(int value, out byte msb, out byte lsb)
    {
      byte[] result = new byte[2] { 0, 0 };
      lsb = (byte)(Convert.ToInt32(value & 0xF80) >> 7);
      msb = Convert.ToByte(value & 0x7F);
      result[0] = msb;
      result[1] = lsb;
      return result;
    }
    // TODO: Check work
    static public short GetPitchWheelInt16(byte msb, byte lsb) { return Convert.ToInt16((lsb << 7) | msb); }
    // TODO: Check work
    static public int GetPitchWheelInt32(byte msb, byte lsb) { return GetPitchWheelInt16(msb, lsb); }

    static public string GetMetadataTitle(int status, bool eightBit = false)
    {
      if (eightBit == false)
      {
        switch (status)
        {
          case Stat16.Text:              return StatusWord.Text.GetEnumDescriptionAttribute();
          case Stat16.Copyright:         return StatusWord.Copyright.GetEnumDescriptionAttribute();
          case Stat16.SequenceName:      return StatusWord.SequenceName.GetEnumDescriptionAttribute();
          case Stat16.InstrumentName:    return StatusWord.InstrumentName.GetEnumDescriptionAttribute();
          case Stat16.Lyric:             return StatusWord.Lyric.GetEnumDescriptionAttribute();
          case Stat16.Marker:            return StatusWord.Marker.GetEnumDescriptionAttribute();
          case Stat16.Cue:               return StatusWord.Cue.GetEnumDescriptionAttribute();
          case Stat16.ChannelPrefix:     return StatusWord.ChannelPrefix.GetEnumDescriptionAttribute();
          case Stat16.SetTempo:          return StatusWord.SetTempo.GetEnumDescriptionAttribute();
          case Stat16.SMPTEOffset:       return StatusWord.SMPTEOffset.GetEnumDescriptionAttribute();
          case Stat16.TimeSignature:     return StatusWord.TimeSignature.GetEnumDescriptionAttribute();
          case Stat16.KeySignature:      return StatusWord.KeySignature.GetEnumDescriptionAttribute();
          case Stat16.SequenceNumber:    return StatusWord.SequenceNumber.GetEnumDescriptionAttribute();
          case Stat16.PortMessage:       return StatusWord.PortMessage.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_70: return StatusWord.SequencerSpecific_FF70.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_71: return StatusWord.SequencerSpecific_FF71.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_72: return StatusWord.SequencerSpecific_FF72.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_73: return StatusWord.SequencerSpecific_FF73.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_74: return StatusWord.SequencerSpecific_FF74.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_75: return StatusWord.SequencerSpecific_FF75.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_76: return StatusWord.SequencerSpecific_FF76.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_77: return StatusWord.SequencerSpecific_FF77.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_78: return StatusWord.SequencerSpecific_FF78.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_79: return StatusWord.SequencerSpecific_FF79.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_7A: return StatusWord.SequencerSpecific_FF7A.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_7B: return StatusWord.SequencerSpecific_FF7B.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_7C: return StatusWord.SequencerSpecific_FF7C.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_7D: return StatusWord.SequencerSpecific_FF7D.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_7E: return StatusWord.SequencerSpecific_FF7E.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SystemExclusive:   return StatusWord.SystemExclusive.GetEnumDescriptionAttribute();
          case Stat16.EndOfTrack:        return StatusWord.EndOfTrack.GetEnumDescriptionAttribute();
          case Stat16.MetaStrFF08:       return StatusWord.MetaStrFF08.GetEnumDescriptionAttribute();
          case Stat16.MetaStrFF09:       return StatusWord.MetaStrFF09.GetEnumDescriptionAttribute();
          case Stat16.MetaStrFF0A:       return StatusWord.MetaStrFF0A.GetEnumDescriptionAttribute();
          case Stat16.MetaStrFF0B:       return StatusWord.MetaStrFF0B.GetEnumDescriptionAttribute();
          case Stat16.MetaStrFF0C:       return StatusWord.MetaStrFF0C.GetEnumDescriptionAttribute();
        }
      }
      else
      {
        switch (status & 0xFF)
        {
          case Stat16.Text              & 0xFF: return StatusWord.Text.GetEnumDescriptionAttribute();
          case Stat16.Copyright         & 0xFF: return StatusWord.Copyright.GetEnumDescriptionAttribute();
          case Stat16.SequenceName      & 0xFF: return StatusWord.SequenceName.GetEnumDescriptionAttribute();
          case Stat16.InstrumentName    & 0xFF: return StatusWord.InstrumentName.GetEnumDescriptionAttribute();
          case Stat16.Lyric             & 0xFF: return StatusWord.Lyric.GetEnumDescriptionAttribute();
          case Stat16.Marker            & 0xFF: return StatusWord.Marker.GetEnumDescriptionAttribute();
          case Stat16.Cue               & 0xFF: return StatusWord.Cue.GetEnumDescriptionAttribute();
          case Stat16.ChannelPrefix     & 0xFF: return StatusWord.ChannelPrefix.GetEnumDescriptionAttribute();
          case Stat16.SetTempo          & 0xFF: return StatusWord.SetTempo.GetEnumDescriptionAttribute();
          case Stat16.SMPTEOffset       & 0xFF: return StatusWord.SMPTEOffset.GetEnumDescriptionAttribute();
          case Stat16.TimeSignature     & 0xFF: return StatusWord.TimeSignature.GetEnumDescriptionAttribute();
          case Stat16.KeySignature      & 0xFF: return StatusWord.KeySignature.GetEnumDescriptionAttribute();
          case Stat16.SequenceNumber    & 0xFF: return StatusWord.SequenceNumber.GetEnumDescriptionAttribute();
          case Stat16.PortMessage       & 0xFF: return StatusWord.PortMessage.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_70 & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_71 & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_72 & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_73 & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_74 & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_75 & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_76 & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_77 & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_78 & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_79 & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_7A & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_7B & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_7C & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_7D & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific_7E & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SequencerSpecific & 0xFF: return StatusWord.SequencerSpecific.GetEnumDescriptionAttribute();
          case Stat16.SystemExclusive   & 0xFF: return StatusWord.SystemExclusive.GetEnumDescriptionAttribute();
          case Stat16.EndOfTrack        & 0xFF: return StatusWord.EndOfTrack.GetEnumDescriptionAttribute();
          case Stat16.MetaStrFF08       & 0xFF: return StatusWord.MetaStrFF08.GetEnumDescriptionAttribute();
          case Stat16.MetaStrFF09       & 0xFF: return StatusWord.MetaStrFF09.GetEnumDescriptionAttribute();
          case Stat16.MetaStrFF0A       & 0xFF: return StatusWord.MetaStrFF0A.GetEnumDescriptionAttribute();
          case Stat16.MetaStrFF0B       & 0xFF: return StatusWord.MetaStrFF0B.GetEnumDescriptionAttribute();
          case Stat16.MetaStrFF0C       & 0xFF: return StatusWord.MetaStrFF0C.GetEnumDescriptionAttribute();
        }
      }
      return "UNKNOWN MESSAGE";
    }
		
		#region Meta helpers
		
		/// Seq No (0xFF00)
		static public string meta_FF00(byte one, byte two) { return string.Format( "Sequence Number: {0} or {0:X2}", Convert.ToUInt16(one << 8 | two)); }
		
		/// string format milliseconds per quarter
		static public string meta_FF51(int microseconds) { return string.Format(StringRes.msg_time_ms_pqn, 60000000.0, microseconds, 60000000.0/microseconds ); }
		
		/// ?
		static public string meta_FF54(IReader reader, int offset) {
			reader.SMPTE.SetSMPTE(
				reader.FileHandle[reader.ReaderIndex].Data[offset+3],
				reader.FileHandle[reader.ReaderIndex].Data[offset+4],
				reader.FileHandle[reader.ReaderIndex].Data[offset+5],
				reader.FileHandle[reader.ReaderIndex].Data[offset+6],
				reader.FileHandle[reader.ReaderIndex].Data[offset+7]
				);
			return $"{reader.SMPTE}";
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

	}

}
