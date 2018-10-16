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
  public partial class Reader : IReaderParser
  {

    // 
    // Event Insight
    // ---------------------------------

    /// <inheritdoc/>
    public int IncrementRun(int offset) { return Increment(offset, 0); }

    /// <inheritdoc/>
    public int GetNextPosition(int offset) { return Increment(offset, 1); }

    /// <summary>
    /// Next Position (rse)
    /// 
    /// All that we want is the next buffer read position.
    /// 
    /// It just may be that we are probing for the end of a
    /// message chunk to the boundary of a delta-time (or RealTime).
    /// </summary>
    int Increment(int offset, int seek)
    {
      int Op = offset + seek, Op1 = offset + seek + 1;
      int status = CurrentStatus & 0xFF; // convert to byte
      // FF (append one)
      if (StatusQuery.IsNoteOff(status))           return Op1; // 0xFF 0x8c 0xNN 0xNN
      if (StatusQuery.IsNoteOn(status))            return Op1; // 0xFF 0x9c 0xNN 0xNN
      if (StatusQuery.IsKeyAftertouch(status))     return Op1; // 0xFF 0xAc 0xNN 0xNN
      if (StatusQuery.IsControlChange(status))     return Op1; // 0xFF 0xBc 0xNN 0xNN
      if (StatusQuery.IsProgramChange(status))     return Op;  // 0xCc 0xNN 0xNN 0xNN
      if (StatusQuery.IsChannelAftertouch(status)) return Op;  // 0xDc 0xNN 0xNN
      if (StatusQuery.IsPitchBend(status))         return Op1; // 0xFF 0xEc 0xNN 0xNN
      if (StatusQuery.IsSequencerSpecific(status)) return Op + FileHandle[ReaderIndex, Op];
      if (StatusQuery.IsSystemExclusive(status))   return FileHandle[ReaderIndex].GetEndOfSystemExclusive(offset);
      //if (StatusQuery.IsSequencerSpecific(status)) return offset + seek; // 0xFF 0xF0
      // 
      if (StatusQuery.IsSystemCommon(status))      return Op + FileHandle[ReaderIndex, Op];
      if (StatusQuery.IsSystemRealtime(status))    return Op; // 0xF0

      // what if there are two realtime messages lined after the other?
      if (!StatusQuery.IsMidiBMessage(CurrentRunningStatus8)) return -1;

      return -1;
    }

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
    public byte[] GetMetaBString(int nTrackOffset) { return GetMetaBString(ReaderIndex, nTrackOffset); }

    /// <inheritdoc/>
    public byte[] GetMetaBString(int nTrackIndex, int nTrackOffset)
    {
      long result = 0;
      int nextOffset = FileHandle.ReadDelta(nTrackIndex, nTrackOffset + 2, out result);
      return FileHandle[nTrackIndex, nextOffset, Convert.ToInt32(result)];
    }

    /// <summary>
    ///  if dealing with a running status message, we would simply 
    /// tell nTrackOffset-1?
    /// </summary>
    byte[] GetMessageBytes(int nTrackIndex, int nTrackOffset, ushort status)
    {
      if (status >= 0xFF00 && status <= 0xFF0C) return GetMetaBString(nTrackIndex, nTrackOffset);
      ushort channelStatus = (ushort)(status & 0xFFF0);
      ushort channelValue = (ushort)(status & 0x000F);

      switch (status)
      {
        // metadata messages
        case Stat16.SequenceNumber:
        case Stat16.ChannelPrefix:
        case Stat16.PortMessage:
        case Stat16.SetTempo:
        case Stat16.SMPTEOffset:
        case Stat16.TimeSignature:
        case Stat16.KeySignature:
        case Stat16.SequencerSpecificMetaEvent:
          return GetMetaBString(nTrackIndex, nTrackOffset);
        case Stat16.EndOfTrack:  return new byte[0];
        // channel messages
        case Stat16.NoteOff:
        case Stat16.NoteOn:
        case Stat16.PolyphonicKeyPressure:
        case Stat16.ControlChange:
        case Stat16.PitchWheel:
          return FileHandle[nTrackIndex, nTrackOffset + 1, 2];
        case Stat16.ProgramChange:
        case Stat16.ChannelPressure:
          return FileHandle[nTrackIndex, nTrackOffset + 1, 1];
       }
      return new byte[0];
    }
    /// <inheritdoc/>
    public string GetMessageString(int pTrackOffset)
    {
      var msg32 = FileHandle.Get16Bit(ReaderIndex, pTrackOffset);
      switch ((StatusWord)msg32)
      {
        case StatusWord.SequenceNumber: /* 0xFF00 */ return MetaHelpers.meta_FF00(FileHandle[ReaderIndex, pTrackOffset + 3], FileHandle[ReaderIndex, pTrackOffset + 4]);
        case StatusWord.ChannelPrefix:  /* 0xFF20 */
        case StatusWord.PortMessage:    /* 0xFF21 */ return FileHandle[ReaderIndex, pTrackOffset + 3].ToString();
        case StatusWord.SetTempo:       /* 0xFF51 */ return MetaHelpers.meta_FF51(Convert.ToInt32(FileHandle[ReaderIndex].ReadU24(pTrackOffset + 3)));
        case StatusWord.SMPTEOffset:    /* 0xFF54 */ return MetaHelpers.meta_FF54(this, pTrackOffset);
        case StatusWord.TimeSignature:  /* 0xFF58 */ return MetaHelpers.meta_FF58(FileHandle[ReaderIndex], pTrackOffset);
        case StatusWord.KeySignature:   /* 0xFF59 */ return MetaHelpers.PrintKeysignature(FileHandle[ReaderIndex], pTrackOffset);
        case StatusWord.EndOfTrack:     /* 0xFF2F */ return MetaHelpers.meta_FF2F();
        case StatusWord.SequencerSpecific:  /* 0xFF7F */ return GetMetaBString(pTrackOffset).StringifyHex();
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

    // 
    // Event Insight (name, value) and specialized helpers
    // ---------------------------------

    /// <inheritdoc/>
    public string GetRseEventValueString(int offset) { return GetEventValueString(offset, 0); }

    /// <inheritdoc/>
    public string GetEventValueString(int offset) { return GetEventValueString(offset, 1); }

    /// <inheritdoc/>
    string GetEventValueString(int offset, int plus)
    {
      int ExpandedRSE = CurrentRunningStatus8; // << 8;
      int delta = IncrementRun(offset + plus);
      if (delta == -1) Debug.Assert(false, string.Format("warning… {0:X2}", ExpandedRSE));
      // channel voice
      else if (StatusQuery.IsNoteOn(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, 2]).StringifyHex();
      else if (StatusQuery.IsNoteOff(ExpandedRSE)) return (FileHandle[ReaderIndex, offset + plus, 2]).StringifyHex();
      // channel voice mode
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
      int Op = offset + plus, Op1 = offset + plus + 1;
      int length = IncrementRun(Op); // delta-byte-encoded 'size' integer. 
      List<byte> returned = new List<byte> { (byte)(this.CurrentRunningStatus8 & 0xff) };
      // channel/voice
      if (StatusQuery.IsNoteOn(CurrentRunningStatus8)) returned.AddRange(FileHandle[ReaderIndex, Op, 2]);
      else if (StatusQuery.IsNoteOff(CurrentRunningStatus8)) returned.AddRange(FileHandle[ReaderIndex, Op, 2]);
      // channel/voice (mode)
      else if (StatusQuery.IsKeyAftertouch(CurrentRunningStatus8)) returned.AddRange(FileHandle[ReaderIndex, Op, 2]);
      else if (StatusQuery.IsControlChange(CurrentRunningStatus8)) returned.AddRange(FileHandle[ReaderIndex, Op, 2]);
      else if (StatusQuery.IsProgramChange(CurrentRunningStatus8)) returned.AddRange(FileHandle[ReaderIndex, Op, 2]);
      else if (StatusQuery.IsChannelAftertouch(CurrentRunningStatus8)) returned.AddRange(FileHandle[ReaderIndex, Op, 2]);
      else if (StatusQuery.IsPitchBend(CurrentRunningStatus8)) returned.AddRange(FileHandle[ReaderIndex, Op, 2]);
      // metadata
      else if (StatusQuery.IsSequencerSpecific(CurrentRunningStatus8))
      {
        returned.Clear();
        returned.AddRange(FileHandle[selectedTrackNumber, offset, 1]);
        returned.AddRange(FileHandle[ReaderIndex, Op, FileHandle[ReaderIndex, Op] + 1]);
      }
      // system
      else if (StatusQuery.IsSystemExclusive(CurrentRunningStatus8))
      {
        returned.Clear();
        returned.AddRange(FileHandle[selectedTrackNumber, offset, 1]);
        returned.AddRange(FileHandle[ReaderIndex, offset + plus, FileHandle[ReaderIndex, offset + plus] + 1]);
      }
      else if (StatusQuery.IsSystemCommon(CurrentRunningStatus8))
      {
        returned.Clear();
        returned.AddRange(FileHandle[selectedTrackNumber, offset, 2]);
        returned.AddRange(FileHandle[ReaderIndex, offset + plus, FileHandle[ReaderIndex, offset + plus] + 1]);
      }
      else if (StatusQuery.IsSystemRealtime(CurrentRunningStatus8))
        returned.AddRange(FileHandle[ReaderIndex, offset + plus, 2]);
      else if (length == -1) Debug.Assert(false, string.Format("warning… {0:X2}", CurrentRunningStatus8));
      byte[] bytes = returned.ToArray();
      return bytes;
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
      // string bytestatus = $"{this.CurrentRunningStatus8:X4}";
      int Op = offset + plus, Op1 = offset + plus + 1;
      bool passFail = !StatusQuery.IsMidiMessage(this.CurrentRunningStatus8); // what are we checking for here?
      if (!StatusQuery.IsMidiMessage(this.CurrentRunningStatus8))
      {
        Debug.Assert(false, string.Format("warning… {0:X2}", CurrentRunningStatus8));
        return null;
      }
      // metadata
      if (StatusQuery.IsSequencerSpecific(CurrentRunningStatus8)) return StatusQuery.SystemExclusiveRange.Name;
      else if (StatusQuery.IsNoteOn(CurrentRunningStatus8)) return GetNoteMsg(0, Op, StringRes.m9);
      else if (StatusQuery.IsNoteOff(CurrentRunningStatus8)) return GetNoteMsg(0, Op, StringRes.m8);
      // channel/voice
      else if (0x21==CurrentRunningStatus8) return FileHandle.Get8Bit(ReaderIndex, Op1).ToString();
      else if (StatusQuery.IsKeyAftertouch(CurrentRunningStatus8)) return string.Format(StringRes.mA, FileHandle[ReaderIndex, Op], FileHandle[ReaderIndex, Op1]);
      else if (StatusQuery.IsControlChange(CurrentRunningStatus8))
      {
        // var shit = FileHandle[ReaderIndex].Data[offset+1];
        var mapIndex = FileHandle.Get8Bit(ReaderIndex, Op);
        var mapString1 = mapIndex > 127 ? $"{mapIndex}?" : EnumFile.CMAP[mapIndex].Trim();
        var mapString2 = FileHandle.Get8Bit(ReaderIndex, Op1);
        return string.Format(StringRes.mB, mapString1,mapString2 );
      }
      else if (StatusQuery.IsProgramChange(CurrentRunningStatus8)) return EnumFile.IMAP[FileHandle[ReaderIndex, Op]].Replace((char)0xA, (char)0x20).Trim();
      else if (StatusQuery.IsChannelAftertouch(CurrentRunningStatus8)) return StatusQuery.ChannelAftertouchRange.Name;
      else if (StatusQuery.IsPitchBend(CurrentRunningStatus8)) return StatusQuery.PitchBendRange.Name;
      // system
      else if (StatusQuery.IsSystemCommon(CurrentRunningStatus8)) return StatusQuery.SystemCommonMessageRange.Name;
      else if (StatusQuery.IsSystemRealtime(CurrentRunningStatus8)) return StatusQuery.SystemRealtimeRange.Name;
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
