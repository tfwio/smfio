/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using on.smfio.Common;
using CliEvent = System.EventArgs;

namespace on.smfio
{
  public partial class Reader : IMidiParser_Parser
  {
    /// <summary>
    /// Next Position (rse)
    /// 
    /// All that we want is the next buffer read position.
    /// 
    /// It just may be that we are probing for the end of a
    /// message chunk to the boundary of a delta-time (or RealTime).
    /// 
    /// FIXME: This is much bulkier than necessary.
    /// </summary>
    int Increment(int offset, int seek)
    {
      int status = CurrentStatus & 0xFF; // convert to byte
      // FF (append one)
      if (StatusQuery.IsNoteOff(status))           return offset + seek + 1; // 0xFF 0x8c 0xNN 0xNN
      if (StatusQuery.IsNoteOn(status))            return offset + seek + 1; // 0xFF 0x9c 0xNN 0xNN
      if (StatusQuery.IsKeyAftertouch(status))     return offset + seek + 1; // 0xFF 0xAc 0xNN 0xNN
      if (StatusQuery.IsControlChange(status))     return offset + seek + 1; // 0xFF 0xBc 0xNN 0xNN
      if (StatusQuery.IsProgramChange(status))     return offset + seek;     // 0xCc 0xNN 0xNN 0xNN
      if (StatusQuery.IsChannelAftertouch(status)) return offset + seek;     // 0xDc 0xNN 0xNN
      if (StatusQuery.IsPitchBend(status))         return offset + seek + 1; // 0xFF 0xEc 0xNN 0xNN
      if (StatusQuery.IsSequencerSpecific(status)) return offset + seek + FileHandle[ReaderIndex, offset + seek];
      if (StatusQuery.IsSystemExclusive(status))   return FileHandle[ReaderIndex].GetEndOfSystemExclusive(offset);
      //if (StatusQuery.IsSequencerSpecific(status)) return offset + seek; // 0xFF 0xF0
      // 
      if (StatusQuery.IsSystemCommon(status))      return offset + seek + FileHandle[ReaderIndex, offset + seek];
      if (StatusQuery.IsSystemRealtime(status))    return offset + seek; // 0xF0

      // this could be wrong.  We just checked for system-realtime, yes?
      // what if there are two realtime messages lined after the other?
      if (!StatusQuery.IsMidiBMessage(CurrentRunningStatus8)) return -1;
      
      // check this before the general common category

      return -1;
    }

    public string GetMbtString(long value) { return TimeUtil.GetMBT(value, Division); }

    // 
    // META STRING
    // ---------------

    /// <inheritdoc/>
    public string GetMetadataString(int offset)
    {
      long stringLength = 0;
      int stringStart = FileHandle[ReaderIndex].DeltaRead(offset + 2, out stringLength); // the message length byte starts at offset+2.
      var result = Strings.Encoding.GetString(FileHandle[ReaderIndex, stringStart, (int)stringLength]);
      return result;
    }

    /// <inheritdoc/>
    public byte[] GetMetaBString(int offset)
    {
      long result = 0;
      int nextOffset = FileHandle.ReadDelta(ReaderIndex, offset + 2, out result);
      return FileHandle[ReaderIndex, nextOffset, Convert.ToInt32(result)];
    }

    /// <inheritdoc/>
    public string GetMessageString(int pTrackOffset)
    {
      var msg32 = FileHandle.Get16Bit(ReaderIndex, pTrackOffset);
      switch ((StatusWord)msg32)
      {
        case StatusWord.SequenceNumber: /* 0xFF00 */ return MetaHelpers.meta_FF00(FileHandle[ReaderIndex, pTrackOffset + 3], FileHandle[ReaderIndex, pTrackOffset + 4]);
        case StatusWord.ChannelPrefix:  /* 0xFF20 */ return FileHandle[ReaderIndex, pTrackOffset + 3].ToString();
        case StatusWord.SetTempo:       /* 0xFF51 */ return MetaHelpers.meta_FF51(Convert.ToInt32(FileHandle[ReaderIndex].ReadU24(pTrackOffset + 3)));
        case StatusWord.SMPTEOffset:    /* 0xFF54 */ return MetaHelpers.meta_FF54();
        case StatusWord.TimeSignature:  /* 0xFF58 */ return MetaHelpers.meta_FF58(FileHandle[ReaderIndex], pTrackOffset);
        case StatusWord.KeySignature:   /* 0xFF59 */ return MetaHelpers.PrintKeysignature(FileHandle[ReaderIndex], pTrackOffset);
        case StatusWord.EndOfTrack:     /* 0xFF2F */ return MetaHelpers.meta_FF2F();
        // FIXME: This just is not right.
        case StatusWord.SequencerSpecific:  /* 0xFF7F */
          return GetMetaBString(pTrackOffset).StringifyHex();
        case StatusWord.SystemExclusive:    /* 0xF0 */
          int nlength = FileHandle[ReaderIndex].GetEndOfSystemExclusive(pTrackOffset) - pTrackOffset;
          int noffset = pTrackOffset;
          string nresult = FileHandle[ReaderIndex, pTrackOffset, nlength].StringifyHex();
          return nresult;
           
        default: // check for a channel message
          if (CurrentRunningStatus8 == 0xF0)
          {
            long ro = 0;
            int no = FileHandle.ReadDelta(ReaderIndex, pTrackOffset + 1, out ro) - pTrackOffset;
            return FileHandle[ReaderIndex, pTrackOffset, Convert.ToInt32(ro) + 2].StringifyHex();
          }
          string msg = string.Format(StringRes.String_Unknown_Message, CurrentRunningStatus8, FileHandle[ReaderIndex, pTrackOffset, 2].StringifyHex());
          return Strings.Encoding.GetString(FileHandle[ReaderIndex, pTrackOffset, FileHandle[ReaderIndex, pTrackOffset + 2] + 3]);
      }
    }

    /// <inheritdoc/>
    public int GetMetaLen(int offset, int plus) { return FileHandle[ReaderIndex, offset + 2]; }

    /// <inheritdoc/>
    public byte[] GetMetaValue(int offset)
    {
      switch ((StatusWord)FileHandle.Get16Bit(ReaderIndex, offset))
      {
        /* 0xff00 */
        case StatusWord.SequenceNumber: return FileHandle[ReaderIndex, offset, 5];
        /* 0xff20 */
        case StatusWord.ChannelPrefix: return FileHandle[ReaderIndex, offset, 4];
        /* 0xff51 */
        case StatusWord.SetTempo: return FileHandle[ReaderIndex, offset, 6];
        /* 0xff54 */
        case StatusWord.SMPTEOffset: return FileHandle[ReaderIndex, offset, 4];
        /* 0xff58 */
        case StatusWord.TimeSignature: return FileHandle[ReaderIndex, offset, 7];
        /* 0xff59 */
        case StatusWord.KeySignature: return FileHandle[ReaderIndex, offset, 5];
        /* 0xff2f */
        case StatusWord.EndOfTrack: return FileHandle[ReaderIndex, offset, 2];
        /* 0xff7f */
        case StatusWord.SequencerSpecific: return FileHandle[ReaderIndex, offset, 3];
        /* 0xff7f */
        case StatusWord.SystemExclusive: return FileHandle[ReaderIndex, offset, 4];
        default:
          Log.ErrorMessage(StringRes.String_Unknown_Message, CurrentRunningStatus8, FileHandle[ReaderIndex, offset + 1]);
          return FileHandle[ReaderIndex, offset, FileHandle[ReaderIndex, offset + 2] + 3];
      }
    }

    /// <inheritdoc/>
    public byte[] GetMetaData(int offset) {
      return FileHandle[ReaderIndex, offset, FileHandle[ReaderIndex, offset + 2] + 3];
    }

    // 
    // Event Insight
    // ---------------------------------

    /// <inheritdoc/>
    public int IncrementRun(int offset) { return Increment(offset, 0); }

    /// <inheritdoc/>
    public int GetNextPosition(int offset) { return Increment(offset, 1); }

    // 
    // Event Insight (name, value) and specialized helpers
    // ---------------------------------

    /// <inheritdoc/>
    public string GetRseEventValueString(int offset) {
      return GetEventValueString(offset, 0);
    }

    /// <inheritdoc/>
    public string GetEventValueString(int offset) {
      return GetEventValueString(offset, 1);
    }

    /// <inheritdoc/>
    string GetEventValueString(int offset, int plus)
    {
      int ExpandedRSE = CurrentRunningStatus8; // << 8;
      int delta = IncrementRun(offset + plus);
      if (delta == -1) Debug.Assert(false, string.Format("warning… {0:X2}", ExpandedRSE));
      // 
      else if (StatusQuery.IsNoteOn(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, 2]).StringifyHex();
      else if (StatusQuery.IsNoteOff(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, 2]).StringifyHex();
      // voice mode
      else if (StatusQuery.IsKeyAftertouch(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, 2]).StringifyHex();
      else if (StatusQuery.IsControlChange(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, 2]).StringifyHex();
      else if (StatusQuery.IsProgramChange(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, 2]).StringifyHex();
      else if (StatusQuery.IsChannelAftertouch(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, 2]).StringifyHex();
      else if (StatusQuery.IsPitchBend(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, 2]).StringifyHex();
      // sysex and seq-specific
      else if (StatusQuery.IsSequencerSpecific(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, FileHandle[ReaderIndex, offset + plus] + 1]).StringifyHex();
      else if (StatusQuery.IsSystemExclusive(ExpandedRSE)) return (FileHandle[ReaderIndex, offset+plus, FileHandle[ReaderIndex].GetEndOfSystemExclusive(offset+plus) - offset]).StringifyHex();
      // any missed common messages
      else if (StatusQuery.IsSystemCommon(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, FileHandle[ReaderIndex, offset + plus] + 1]).StringifyHex();
      else if (StatusQuery.IsSystemRealtime(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, 4]).StringifyHex();
      return null;
    }

    /// <inheritdoc/>
    public byte[] GetRseEventValue(int offset) { return GetEventValue(offset, 0); }

    /// <inheritdoc/>
    public byte[] GetEventValue(int offset) { return GetEventValue(offset, 1); }

    byte[] GetEventValue(int offset, int plus)
    {
      List<byte> returned = new List<byte> { (byte)(CurrentRunningStatus8 & 0xff) };
      int ExpandedRSE = CurrentRunningStatus8;
      int length = IncrementRun(offset + plus); // delta-byte-encoded 'size' integer. 
      //
      if (StatusQuery.IsNoteOn(ExpandedRSE)) returned.AddRange(FileHandle[ReaderIndex, offset + plus, 2]);
      else if (StatusQuery.IsNoteOff(ExpandedRSE)) returned.AddRange(FileHandle[ReaderIndex, offset + plus, 2]);
      else if (StatusQuery.IsKeyAftertouch(ExpandedRSE)) returned.AddRange(FileHandle[ReaderIndex, offset + plus, 2]);
      else if (StatusQuery.IsControlChange(ExpandedRSE)) returned.AddRange(FileHandle[ReaderIndex, offset + plus, 2]);
      else if (StatusQuery.IsProgramChange(ExpandedRSE)) returned.AddRange(FileHandle[ReaderIndex, offset + plus, 2]);
      else if (StatusQuery.IsChannelAftertouch(ExpandedRSE)) returned.AddRange(FileHandle[ReaderIndex, offset + plus, 2]);
      else if (StatusQuery.IsPitchBend(ExpandedRSE)) returned.AddRange(FileHandle[ReaderIndex, offset + plus, 2]);
      else if (StatusQuery.IsSequencerSpecific(ExpandedRSE))
      {
        returned.Clear();
        returned.AddRange(FileHandle[selectedTrackNumber, offset, 1]);
        returned.AddRange(FileHandle[ReaderIndex, offset + plus, FileHandle[ReaderIndex, offset + plus] + 1]);
      }
      else if (StatusQuery.IsSystemExclusive(ExpandedRSE))
      {
        returned.Clear();
        returned.AddRange(FileHandle[selectedTrackNumber, offset, 1]);
        returned.AddRange(FileHandle[ReaderIndex, offset + plus, FileHandle[ReaderIndex, offset + plus] + 1]);
      }
      else if (StatusQuery.IsSystemCommon(ExpandedRSE))
      {
        returned.Clear();
        returned.AddRange(FileHandle[selectedTrackNumber, offset, 2]);
        returned.AddRange(FileHandle[ReaderIndex, offset + plus, FileHandle[ReaderIndex, offset + plus] + 1]);
      }
      else if (StatusQuery.IsSystemRealtime(ExpandedRSE))
        returned.AddRange(FileHandle[ReaderIndex, offset + plus, 2]);
      else if (length == -1) Debug.Assert(false, string.Format("warning… {0:X2}", ExpandedRSE));
      byte[] bytes = returned.ToArray();
      return bytes;
    }

    // 
    // Event Length
    // ---------------------------------

    int GetEventLength(int offset, int plus)
    {
      int ExpandedRSE = CurrentRunningStatus8; // << 8;
      int delta = IncrementRun(offset + plus);
      if (delta == -1) Debug.Assert(false, string.Format("warning… {0:X2}", ExpandedRSE));
      else if (StatusQuery.IsNoteOn(ExpandedRSE)) return 2;
      else if (StatusQuery.IsNoteOff(ExpandedRSE)) return 2;
      else if (StatusQuery.IsKeyAftertouch(ExpandedRSE)) return 2;
      else if (StatusQuery.IsControlChange(ExpandedRSE)) return 2;
      else if (StatusQuery.IsProgramChange(ExpandedRSE)) return 2;
      else if (StatusQuery.IsChannelAftertouch(ExpandedRSE)) return 2;
      // this may never occur here and should in the Meta part
      else if (StatusQuery.IsPitchBend(ExpandedRSE)) return 2;
      else if (StatusQuery.IsSequencerSpecific(ExpandedRSE)) return GetMetaLen(offset, plus);
      else if (StatusQuery.IsSystemCommon(ExpandedRSE)) return GetMetaLen(offset, plus);
      else if (StatusQuery.IsSystemRealtime(ExpandedRSE)) return GetMetaLen(offset, plus);
      return -1;
    }

    // 
    // Event Names
    // ---------------------------------

    /// <inheritdoc/>
    public string GetRseEventString(int offset) { return GetEventString(offset, 0); }

    /// <inheritdoc/>
    public string GetEventString(int offset) { return GetEventString(offset, 1); }

    string GetEventString(int offset, int plus)
    {
      string bytestatus = $"{CurrentRunningStatus8:X4}";
      int ExpandedRSE = CurrentRunningStatus8; // << 8;
      bool passFail = !StatusQuery.IsMidiMessage(CurrentRunningStatus8); // what are we checking for here?
      if (!StatusQuery.IsMidiMessage(CurrentRunningStatus8))
      {
        Debug.Assert(false, string.Format("warning… {0:X2}", ExpandedRSE));
        return null;
      }
      else
      if (StatusQuery.IsNoteOn(ExpandedRSE)) return GetNoteMsg(0, offset + plus, StringRes.m9);
      else if (StatusQuery.IsNoteOff(ExpandedRSE)) return GetNoteMsg(0, offset + plus, StringRes.m8);
      else if (StatusQuery.IsKeyAftertouch(ExpandedRSE)) return string.Format(StringRes.mA, FileHandle[ReaderIndex, offset + plus], FileHandle[ReaderIndex, offset + plus + 1]);
      else if (StatusQuery.IsControlChange(ExpandedRSE)) return string.Format(StringRes.mB, SmfString.ControlMap[FileHandle.Get8Bit(ReaderIndex, offset + plus)].Replace((char)0xa, (char)0x20).Trim(), FileHandle.Get8Bit(ReaderIndex, offset + plus + 1));
      else if (StatusQuery.IsProgramChange(ExpandedRSE)) return SmfString.PatchMap[FileHandle[ReaderIndex, offset + plus]].Replace((char)0xa, (char)0x20).Trim();
      else if (StatusQuery.IsChannelAftertouch(ExpandedRSE)) return StatusQuery.ChannelAftertouchRange.Name;
      else if (StatusQuery.IsPitchBend(ExpandedRSE)) return StatusQuery.PitchBendRange.Name;
      else if (StatusQuery.IsSequencerSpecific(ExpandedRSE)) return StatusQuery.SystemExclusiveRange.Name;
      else if (StatusQuery.IsSystemCommon(ExpandedRSE)) return StatusQuery.SystemCommonMessageRange.Name;
      else if (StatusQuery.IsSystemRealtime(ExpandedRSE)) return StatusQuery.SystemRealtimeRange.Name;
      return null;
    }

    // 
    // String Formatters
    // ---------------------------------

    /// <inheritdoc/>
    string GetNoteMsg(int shift, int offset, string format) { return string.Format(format, FileHandle[ReaderIndex, offset + shift], FileHandle[ReaderIndex, offset + shift + 1], SmfString.GetKeySharp(FileHandle[ReaderIndex, offset + shift]), SmfString.GetOctave(FileHandle[ReaderIndex, offset + shift])); }

    /// <inheritdoc/>
    public string chV(int v) { return string.Format("{0} {1}", string.Format("{0:X2}", CurrentRunningStatus8), GetEventValueString(v)); }

    /// <inheritdoc/>
    public string chRseV(int v) { return string.Format("{0} {1}", string.Format("{0:X2}", CurrentRunningStatus8), GetRseEventValueString(v)); }
    
  }

}
