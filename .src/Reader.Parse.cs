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
    /// <summary>
    /// Get the next read position.
    /// 
    /// When calling this method, the reader is likely leaving off
    /// from the following 
    /// 
    /// 1. [**Delta-Time**] [**Status**] — following a status byte  
    ///    add one to offset
    /// 2. [**Delta-Time**] — following delta-time (running status message)  
    ///    offset increment not necessary.
    /// </summary>
    public int Increment(int offset)
    {
      int Op = offset, Op1 = offset + 1;
      int status = CurrentStatus & 0xFF; // convert to byte
      // FF (append one)
      if (StatusQuery.IsNoteOff(status))           return Op1; // 0x8c 0xNN 0xNN
      if (StatusQuery.IsNoteOn(status))            return Op1; // 0x9c 0xNN 0xNN
      if (StatusQuery.IsKeyAftertouch(status))     return Op1; // 0xAc 0xNN 0xNN
      if (StatusQuery.IsControlChange(status))     return Op1; // 0xBc 0xNN 0xNN
      if (StatusQuery.IsProgramChange(status))     return Op;  // 0xCc 0xNN
      if (StatusQuery.IsChannelAftertouch(status)) return Op;  // 0xDc 0xNN
      if (StatusQuery.IsPitchBend(status))         return Op1; // 0xEc 0xNN 0xNN
      if (StatusQuery.IsSequencerSpecific(status)) return Op + FileHandle[ReaderIndex, Op];
      if (StatusQuery.IsSystemExclusive(status))   return FileHandle[ReaderIndex].GetEndOfSystemExclusive(offset);
      // 
      if (StatusQuery.IsSystemCommon(status))      return Op + FileHandle[ReaderIndex, Op];
      if (StatusQuery.IsSystemRealtime(status))    return Op; // 0xF0

      if (!StatusQuery.IsMidiBMessage(CurrentRunningStatus8)) return -1;
      // what if there are two realtime messages lined after the other?

      return -1;
    }

    // 
    // META STRING
    // ---------------

    /// <inheritdoc/>
    public string GetMetadataString(int nTrackIndex, int nTrackOffset)
    {
      long stringLength = 0;
      int stringStart = FileHandle[nTrackIndex].DeltaRead(nTrackOffset + 2, out stringLength); // the message length byte starts at offset+2.
      var result = Strings.Encoding.GetString(FileHandle[nTrackIndex, stringStart, (int)stringLength]);
      return result;
    }

    /// <inheritdoc/>
    public byte[] GetMetadataBytes(int nTrackIndex, int nTrackOffset)
    {
      long result = 0;
      int nextOffset = FileHandle.ReadDelta(nTrackIndex, nTrackOffset + 2, out result);
      return FileHandle[nTrackIndex, nextOffset, Convert.ToInt32(result)];
    }

    /// <summary>
    /// if dealing with a running status message, we would simply tell nTrackOffset-1?
    /// </summary>
    public byte[] GetMessageBytes(int nTrackIndex, int nTrackOffset, ushort status)
    {
      if (status >= 0xFF00 && status <= 0xFF0C) return GetMetadataBytes(nTrackIndex, nTrackOffset);
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
        case Stat16.SequencerSpecific:
          return GetMetadataBytes(nTrackIndex, nTrackOffset);
        case Stat16.EndOfTrack:  return new byte[0];
      }
      switch (status & 0xF0)
      {
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
        // system exclusive message
        case Stat16.SystemExclusive:
          int nlength = FileHandle[nTrackIndex].GetEndOfSystemExclusive(nTrackOffset) - nTrackOffset;
          int noffset = nTrackOffset;
          return FileHandle[nTrackIndex, nTrackOffset, nlength];
       }
      return new byte[0];
    }
    /// <inheritdoc/>
    public string GetMessageString(int pTrackIndex, int pTrackOffset)
    {
      var msg32 = FileHandle.Get16Bit(pTrackIndex, pTrackOffset);
      switch ((StatusWord)msg32)
      {
        case StatusWord.SequenceNumber: /* 0xFF00 */ return MetaHelpers.meta_FF00(FileHandle[pTrackIndex, pTrackOffset + 3], FileHandle[pTrackIndex, pTrackOffset + 4]);
        case StatusWord.ChannelPrefix:  /* 0xFF20 */
        case StatusWord.PortMessage:    /* 0xFF21 */ return FileHandle[pTrackIndex, pTrackOffset + 3].ToString();
        case StatusWord.SetTempo:       /* 0xFF51 */ return MetaHelpers.meta_FF51(Convert.ToInt32(FileHandle[pTrackIndex].ReadU24(pTrackOffset + 3)));
        case StatusWord.SMPTEOffset:    /* 0xFF54 */ return MetaHelpers.meta_FF54(this, pTrackOffset);
        case StatusWord.TimeSignature:  /* 0xFF58 */ return MetaHelpers.meta_FF58(FileHandle[pTrackIndex], pTrackOffset);
        case StatusWord.KeySignature:   /* 0xFF59 */ return MetaHelpers.PrintKeysignature(FileHandle[pTrackIndex], pTrackOffset);
        case StatusWord.EndOfTrack:     /* 0xFF2F */ return MetaHelpers.meta_FF2F();
        case StatusWord.SequencerSpecific:  /* 0xFF7F */ return GetMetadataBytes(pTrackIndex, pTrackOffset).StringifyHex();
        case StatusWord.SystemExclusive:    /* 0xF0 */
          int nlength = FileHandle[pTrackIndex].GetEndOfSystemExclusive(pTrackOffset) - pTrackOffset;
          int noffset = pTrackOffset;
          string nresult = FileHandle[pTrackIndex, pTrackOffset, nlength].StringifyHex();
          return nresult;
        default: // check for a channel message
          if (CurrentRunningStatus8 == 0xF0)
          {
            long ro = 0;
            int no = FileHandle.ReadDelta(pTrackIndex, pTrackOffset + 1, out ro) - pTrackOffset;
            return FileHandle[pTrackIndex, pTrackOffset, Convert.ToInt32(ro) + 2].StringifyHex();
          }
          // string msg = string.Format(StringRes.String_Unknown_Message, CurrentRunningStatus8, FileHandle[pTrackIndex, pTrackOffset, 2].StringifyHex());
          return Strings.Encoding.GetString(FileHandle[pTrackIndex, pTrackOffset, FileHandle[pTrackIndex, pTrackOffset + 2] + 3]);
      }
    }

    // 
    // Event Insight (name, value) and specialized helpers
    // ---------------------------------

    /// <inheritdoc/>
    public string GetEventValueString(int nTrackIndex, int nTrackOffset)
    {
      int ExpandedRSE = CurrentRunningStatus8; // << 8;
      int delta = Increment(nTrackOffset);
      if (delta == -1) Debug.Assert(false, string.Format("warning… {0:X2}", ExpandedRSE));
      // channel voice
      else if (StatusQuery.IsNoteOn(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, 2]).StringifyHex();
      else if (StatusQuery.IsNoteOff(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, 2]).StringifyHex();
      // channel voice mode
      else if (StatusQuery.IsKeyAftertouch(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, 2]).StringifyHex();
      else if (StatusQuery.IsControlChange(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, 2]).StringifyHex();
      else if (StatusQuery.IsProgramChange(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, 2]).StringifyHex();
      else if (StatusQuery.IsChannelAftertouch(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, 2]).StringifyHex();
      else if (StatusQuery.IsPitchBend(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, 2]).StringifyHex();
      // sysex and seq-specific
      else if (StatusQuery.IsSequencerSpecific(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, FileHandle[nTrackIndex, nTrackOffset] + 1]).StringifyHex();
      else if (StatusQuery.IsSystemExclusive(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, FileHandle[nTrackIndex].GetEndOfSystemExclusive(nTrackOffset) - nTrackOffset]).StringifyHex();
      // any missed common messages
      else if (StatusQuery.IsSystemCommon(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, FileHandle[nTrackIndex, nTrackOffset] + 1]).StringifyHex();
      else if (StatusQuery.IsSystemRealtime(ExpandedRSE)) return (FileHandle[nTrackIndex, nTrackOffset, 4]).StringifyHex();
      return null;
    }

    public byte[] GetEventValue(int nTrackIndex, int nTrackOffset)
    {
      int offset0 = nTrackOffset, offset1 = nTrackOffset + 1;
      int length = Increment(offset0); // delta-byte-encoded 'size' integer.
      List<byte> returned = new List<byte> { (byte)(CurrentRunningStatus8 & 0xFF) };
      // channel/voice
      if (StatusQuery.IsNoteOn(CurrentRunningStatus8))                 returned.AddRange(FileHandle[nTrackIndex, offset0, 2]);
      else if (StatusQuery.IsNoteOff(CurrentRunningStatus8))           returned.AddRange(FileHandle[nTrackIndex, offset0, 2]);
      // channel/voice (mode)
      else if (StatusQuery.IsKeyAftertouch(CurrentRunningStatus8))     returned.AddRange(FileHandle[nTrackIndex, offset0, 2]);
      else if (StatusQuery.IsControlChange(CurrentRunningStatus8))     returned.AddRange(FileHandle[nTrackIndex, offset0, 2]);
      else if (StatusQuery.IsProgramChange(CurrentRunningStatus8))     returned.AddRange(FileHandle[nTrackIndex, offset0, 2]);
      else if (StatusQuery.IsChannelAftertouch(CurrentRunningStatus8)) returned.AddRange(FileHandle[nTrackIndex, offset0, 2]);
      else if (StatusQuery.IsPitchBend(CurrentRunningStatus8))         returned.AddRange(FileHandle[nTrackIndex, offset0, 2]);
      // metadata
      else if (StatusQuery.IsSequencerSpecific(CurrentRunningStatus8))
      {
        returned.Clear();
        returned.AddRange(FileHandle[nTrackIndex, nTrackOffset, 1]);
        returned.AddRange(FileHandle[nTrackIndex, offset0, FileHandle[nTrackIndex, offset0] + 1]);
      }
      // system
      else if (StatusQuery.IsSystemExclusive(CurrentRunningStatus8))
      {
        returned.Clear();
        returned.AddRange(FileHandle[selectedTrackNumber, nTrackOffset, 1]);
        returned.AddRange(FileHandle[nTrackIndex, nTrackOffset, FileHandle[nTrackIndex, nTrackOffset] + 1]);
      }
      else if (StatusQuery.IsSystemCommon(CurrentRunningStatus8))
      {
        returned.Clear();
        returned.AddRange(FileHandle[selectedTrackNumber, nTrackOffset, 2]);
        returned.AddRange(FileHandle[nTrackIndex, nTrackOffset, FileHandle[nTrackIndex, nTrackOffset] + 1]);
      }
      else if (StatusQuery.IsSystemRealtime(CurrentRunningStatus8))
        returned.AddRange(FileHandle[nTrackIndex, nTrackOffset, 2]);
      else if (length == -1) Debug.Assert(false, string.Format("warning… {0:X2}", CurrentRunningStatus8));
      byte[] bytes = returned.ToArray();
      return bytes;
    }

    public string GetEventString(int nTrackIndex, int nTrackOffset)
    {
      int offset0 = nTrackOffset, offset1 = nTrackOffset + 1;
      if (!StatusQuery.IsMidiMessage(CurrentRunningStatus8))
      {
        Debug.Assert(false, string.Format("warning… {0:X2}", CurrentRunningStatus8));
        return null;
      }
      // metadata
      if (StatusQuery.IsSequencerSpecific(CurrentRunningStatus8)) return StatusQuery.SystemExclusiveRange.Name;
      else if (StatusQuery.IsNoteOn(CurrentRunningStatus8)) return GetNoteMsg(offset0, StringRes.format_note_on);
      else if (StatusQuery.IsNoteOff(CurrentRunningStatus8)) return GetNoteMsg(offset0, StringRes.fotmat_note_off);
      // channel/voice
      else if (0x21==CurrentRunningStatus8) return FileHandle.Get8Bit(nTrackIndex, offset1).ToString();
      else if (StatusQuery.IsKeyAftertouch(CurrentRunningStatus8)) return string.Format(StringRes.mA, FileHandle[nTrackIndex, offset0], FileHandle[nTrackIndex, offset1]);
      else if (StatusQuery.IsControlChange(CurrentRunningStatus8))
      {
        var mapIndex = FileHandle.Get8Bit(nTrackIndex, offset0);
        var mapString1 = mapIndex > 127 ? $"{mapIndex}?" : EnumFile.CMAP[mapIndex].Trim();
        var mapString2 = FileHandle.Get8Bit(nTrackIndex, offset1);
        return string.Format(StringRes.mB, mapString1,mapString2 );
      }
      else if (StatusQuery.IsProgramChange(CurrentRunningStatus8)) return EnumFile.IMAP[FileHandle[nTrackIndex, offset0]].Replace((char)0xA, (char)0x20).Trim();
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
    string GetNoteMsg(int nTrackOffset, string format)
    {
      return string.Format(
        format,
        FileHandle[ReaderIndex, nTrackOffset],
        FileHandle[ReaderIndex, nTrackOffset + 1],
        SmfString.GetKeySharp(FileHandle[ReaderIndex, nTrackOffset]),
        SmfString.GetOctave(FileHandle[ReaderIndex, nTrackOffset])
        );
    }
  }
}
