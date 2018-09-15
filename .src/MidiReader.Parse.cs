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
  public partial class MidiReader : IMidiParser_Parser
  {
    System.Text.Encoding Encoding = System.Text.Encoding.UTF8;

    /// <summary>Next Position (rse)</summary>
    int Increment(int offset, int plus)
    {
      int ExpandedRSE = RunningStatus32 << 8;
      if (MidiMessageInfo.IsNoteOff(ExpandedRSE))           return offset + plus + 1;
      if (MidiMessageInfo.IsNoteOn(ExpandedRSE))            return offset + plus + 1;
      if (MidiMessageInfo.IsKeyAftertouch(ExpandedRSE))     return offset + plus + 1;
      if (MidiMessageInfo.IsControlChange(ExpandedRSE))     return offset + plus + 1;
      if (MidiMessageInfo.IsProgramChange(ExpandedRSE))     return offset + plus;
      if (MidiMessageInfo.IsChannelAftertouch(ExpandedRSE)) return offset + plus;
      if (MidiMessageInfo.IsPitchBend(ExpandedRSE))         return offset + plus + 1;
      if (MidiMessageInfo.IsSystemMessage(ExpandedRSE))     return offset + plus + SmfFileHandle[SelectedTrackNumber, offset + plus];
      if (MidiMessageInfo.IsSystemCommon(ExpandedRSE))      return offset + plus + SmfFileHandle[SelectedTrackNumber, offset + plus];
      if (MidiMessageInfo.IsSystemRealtime(ExpandedRSE))    return offset + plus;
      if (!MidiMessageInfo.IsMidiBMessage(RunningStatus32)) return -1;
      return -1;
    }
    
    /// <inheritdoc/>
    public string GetMbtString(ulong value) { return MBT.GetString(value, FileDivision); }

    //
    // TIME
    // ---------------------------------
    
    /// <inheritdoc/>
    public int GetMetaNextPos(int offset)
    {
      long result = 0;
      return GetIntVar(offset + 2, out result) + Convert.ToInt32(result) - 1;
    }
    
    // 
    // META STRING
    // ---------------
    
    /// <inheritdoc/>
    public string GetMetaString(int offset)
    {
      long result = 0;
      int nextOffset = GetIntVar(offset + 2, out result);
      return System.Text.Encoding.UTF8.GetString(SmfFileHandle[SelectedTrackNumber, nextOffset, Convert.ToInt32(result)]);
    }
    
    /// <inheritdoc/>
    public string GetMetaSTR(int offset)
    {
      int flag = SmfFileHandle.Get16BitInt32(SelectedTrackNumber, offset);
      switch ((MetaMsgU16FF)flag) {
          
        case MetaMsgU16FF.SequenceNo:      /* 0xff00 */ return MetaHelpers.meta_FF00(SmfFileHandle[SelectedTrackNumber, offset + 3], SmfFileHandle[SelectedTrackNumber, offset + 4] );
        case MetaMsgU16FF.Chanel:          /* 0xff20 */ return SmfFileHandle[SelectedTrackNumber, offset + 3].ToString();
        case MetaMsgU16FF.Tempo:           /* 0xff51 */ return MetaHelpers.meta_FF51(Convert.ToInt32(SmfFileHandle[SelectedTrackNumber].Get24Bit(offset + 3)));
        case MetaMsgU16FF.SMPTE:           /* 0xff54 */ return MetaHelpers.meta_FF54();
        case MetaMsgU16FF.TimeSignature:   /* 0xff58 */ return MetaHelpers.meta_FF58(SmfFileHandle[SelectedTrackNumber], offset);
        case MetaMsgU16FF.KeySignature:    /* 0xff59 */ return MetaHelpers.meta_FF59(SmfFileHandle[SelectedTrackNumber], offset);
        case MetaMsgU16FF.EndOfTrack:      /* 0xff2f */ return MetaHelpers.meta_FF2F();
        case MetaMsgU16FF.SystemSpecific:  /* 0xff7f */ long result = 0;  int nextOffset  = GetIntVar(offset + 2, out result)  - offset; return SmfFileHandle[SelectedTrackNumber, offset, Convert.ToInt32(result) + 3].StringifyHex();
        case MetaMsgU16FF.SystemExclusive:              long result1 = 0; int nextOffset1 = GetIntVar(offset + 1, out result1) - offset; return SmfFileHandle[SelectedTrackNumber, offset, Convert.ToInt32(result1) + 4].StringifyHex();
        default: // check for a channel message
          if (RunningStatus32==0xF0)
          {
            long ro = 0;
            int no = GetIntVar(offset + 1, out ro) - offset;
            return SmfFileHandle[SelectedTrackNumber, offset, Convert.ToInt32(ro)+2].StringifyHex();
          }
          string msg = string.Format(StringRes.String_Unknown_Message, RunningStatus32, SmfFileHandle[SelectedTrackNumber, offset,2].StringifyHex() );
          

          return System.Text.Encoding.UTF8.GetString(SmfFileHandle[SelectedTrackNumber, offset, SmfFileHandle[SelectedTrackNumber, offset + 2] + 3]);
      }
    }

    /// <inheritdoc/>
    public byte[] GetMetaBString(int offset)
    {
      long result = 0;
      int nextOffset = GetIntVar(offset + 2, out result);
      return SmfFileHandle[SelectedTrackNumber, nextOffset, Convert.ToInt32(result)];
    }

    /// <inheritdoc/>
    public int GetMetaLen(int offset, int plus) { return SmfFileHandle[SelectedTrackNumber, offset + 2]; }

    /// <inheritdoc/>
    public byte[] GetMetaValue(int offset)
    {
      switch ((MetaMsgU16FF)SmfFileHandle.Get16BitInt32(SelectedTrackNumber, offset)) {
        /* 0xff00 */ case MetaMsgU16FF.SequenceNo:      return SmfFileHandle[SelectedTrackNumber, offset, 5];
        /* 0xff20 */ case MetaMsgU16FF.Chanel:          return SmfFileHandle[SelectedTrackNumber, offset, 4];
        /* 0xff51 */ case MetaMsgU16FF.Tempo:           return SmfFileHandle[SelectedTrackNumber, offset, 6];
        /* 0xff54 */ case MetaMsgU16FF.SMPTE:           return SmfFileHandle[SelectedTrackNumber, offset, 4];
        /* 0xff58 */ case MetaMsgU16FF.TimeSignature:   return SmfFileHandle[SelectedTrackNumber, offset, 7];
        /* 0xff59 */ case MetaMsgU16FF.KeySignature:    return SmfFileHandle[SelectedTrackNumber, offset, 5];
        /* 0xff2f */ case MetaMsgU16FF.EndOfTrack:      return SmfFileHandle[SelectedTrackNumber, offset, 2];
        /* 0xff7f */ case MetaMsgU16FF.SystemSpecific:  return SmfFileHandle[SelectedTrackNumber, offset, 3];
        /* 0xff7f */ case MetaMsgU16FF.SystemExclusive: return SmfFileHandle[SelectedTrackNumber, offset, 4];
        default:
          Log.ErrorMessage(StringRes.String_Unknown_Message, RunningStatus32, SmfFileHandle[SelectedTrackNumber, offset + 1]);
          return SmfFileHandle[SelectedTrackNumber, offset, SmfFileHandle[SelectedTrackNumber, offset + 2] + 3];
      }
    }

    /// <inheritdoc/>
    public byte[] GetMetaData(int offset) { return SmfFileHandle[SelectedTrackNumber, offset, SmfFileHandle[SelectedTrackNumber, offset + 2] + 3]; }

    // 
    // Event Insight
    // ---------------------------------

    /// <inheritdoc/>
    public int GetNextRsePosition(int offset) { return Increment(offset, 0); }
    
    /// <inheritdoc/>
    public int GetNextPosition(int offset) { return Increment(offset, 1); }
    
    // 
    // Event Insight (name, value) and specialized helpers
    // ---------------------------------

    /// <inheritdoc/>
    public string GetRseEventValueString(int offset) { return GetEventValueString(offset, 0); }
    
    /// <inheritdoc/>
    public string GetEventValueString   (int offset) { return GetEventValueString(offset, 1); }
    
    /// <inheritdoc/>
    string GetEventValueString(int offset, int plus)
    {
      int ExpandedRSE = RunningStatus32 << 8;
      int delta = GetNextRsePosition(offset + plus);
      if (delta == -1)                                           Debug.Assert(false, string.Format("warning… {0:X2}", ExpandedRSE));
      else if (MidiMessageInfo.IsNoteOn(ExpandedRSE))            return (SmfFileHandle[SelectedTrackNumber, offset + plus, 2]).StringifyHex();
      else if (MidiMessageInfo.IsNoteOff(ExpandedRSE))           return (SmfFileHandle[SelectedTrackNumber, offset + plus, 2]).StringifyHex();
      else if (MidiMessageInfo.IsKeyAftertouch(ExpandedRSE))     return (SmfFileHandle[SelectedTrackNumber, offset + plus, 2]).StringifyHex();
      else if (MidiMessageInfo.IsControlChange(ExpandedRSE))     return (SmfFileHandle[SelectedTrackNumber, offset + plus, 2]).StringifyHex();
      else if (MidiMessageInfo.IsProgramChange(ExpandedRSE))     return (SmfFileHandle[SelectedTrackNumber, offset + plus, 2]).StringifyHex();
      else if (MidiMessageInfo.IsChannelAftertouch(ExpandedRSE)) return (SmfFileHandle[SelectedTrackNumber, offset + plus, 2]).StringifyHex();
      else if (MidiMessageInfo.IsPitchBend(ExpandedRSE))         return (SmfFileHandle[SelectedTrackNumber, offset + plus, 2]).StringifyHex();
      else if (MidiMessageInfo.IsSystemMessage(ExpandedRSE))     return (SmfFileHandle[SelectedTrackNumber, offset + plus, SmfFileHandle[SelectedTrackNumber, offset + plus] + 1]).StringifyHex();
      else if (MidiMessageInfo.IsSystemCommon(ExpandedRSE))      return (SmfFileHandle[SelectedTrackNumber, offset + plus, SmfFileHandle[SelectedTrackNumber, offset + plus] + 1]).StringifyHex();
      else if (MidiMessageInfo.IsSystemRealtime(ExpandedRSE))    return (SmfFileHandle[SelectedTrackNumber, offset + plus, 4]).StringifyHex();
      return null;
    }
    
    /// <inheritdoc/>
    public byte[] GetRseEventValue(int offset) { return GetEventValue(offset, 0); }

    /// <inheritdoc/>
    public byte[] GetEventValue   (int offset) { return GetEventValue(offset, 1); }
    
    byte[] GetEventValue(int offset, int plus)
    {
      List<byte> returned = new List<byte> { (byte)(RunningStatus32 & 0xff) };
      int ExpandedRSE = RunningStatus32 << 8;
      int delta = GetNextRsePosition(offset + plus);
      
      if (MidiMessageInfo.IsNoteOn(ExpandedRSE))                 returned.AddRange(SmfFileHandle[SelectedTrackNumber, offset + plus, 2]);
      else if (MidiMessageInfo.IsNoteOff(ExpandedRSE))           returned.AddRange(SmfFileHandle[SelectedTrackNumber, offset + plus, 2]);
      else if (MidiMessageInfo.IsKeyAftertouch(ExpandedRSE))     returned.AddRange(SmfFileHandle[SelectedTrackNumber, offset + plus, 2]);
      else if (MidiMessageInfo.IsControlChange(ExpandedRSE))     returned.AddRange(SmfFileHandle[SelectedTrackNumber, offset + plus, 2]);
      else if (MidiMessageInfo.IsProgramChange(ExpandedRSE))     returned.AddRange(SmfFileHandle[SelectedTrackNumber, offset + plus, 2]);
      else if (MidiMessageInfo.IsChannelAftertouch(ExpandedRSE)) returned.AddRange(SmfFileHandle[SelectedTrackNumber, offset + plus, 2]);
      else if (MidiMessageInfo.IsPitchBend(ExpandedRSE))         returned.AddRange(SmfFileHandle[SelectedTrackNumber, offset + plus, 2]);
      else if (MidiMessageInfo.IsSystemMessage(ExpandedRSE)) {
        returned.Clear();
        returned.AddRange(SmfFileHandle[selectedTrackNumber, offset, 1]);
        returned.AddRange(SmfFileHandle[SelectedTrackNumber, offset + plus, SmfFileHandle[SelectedTrackNumber, offset + plus] + 1]);
      } else if (MidiMessageInfo.IsSystemCommon(ExpandedRSE)) {
        returned.Clear();
        returned.AddRange(SmfFileHandle[selectedTrackNumber, offset, 2]);
        returned.AddRange(SmfFileHandle[SelectedTrackNumber, offset + plus, SmfFileHandle[SelectedTrackNumber, offset + plus] + 1]);
      } else if (MidiMessageInfo.IsSystemRealtime(ExpandedRSE))
        returned.AddRange(SmfFileHandle[SelectedTrackNumber, offset + plus, 2]);
      else if (delta == -1)                                      Debug.Assert(false, string.Format("warning… {0:X2}", ExpandedRSE));
      byte[] bytes = returned.ToArray();
      return bytes;
    }
    
    // 
    // Event Length
    // ---------------------------------
    
    int GetEventLength(int offset, int plus)
    {
      int ExpandedRSE = RunningStatus32 << 8;
      int delta = GetNextRsePosition(offset + plus);
      if (delta == -1) Debug.Assert(false, string.Format("warning… {0:X2}", ExpandedRSE));
      else if (MidiMessageInfo.IsNoteOn(ExpandedRSE))            return 2;
      else if (MidiMessageInfo.IsNoteOff(ExpandedRSE))           return 2;
      else if (MidiMessageInfo.IsKeyAftertouch(ExpandedRSE))     return 2;
      else if (MidiMessageInfo.IsControlChange(ExpandedRSE))     return 2;
      else if (MidiMessageInfo.IsProgramChange(ExpandedRSE))     return 2;
      else if (MidiMessageInfo.IsChannelAftertouch(ExpandedRSE)) return 2;
      // this may never occur here and should in the Meta part
      else if (MidiMessageInfo.IsPitchBend(ExpandedRSE))         return 2;
      else if (MidiMessageInfo.IsSystemMessage(ExpandedRSE))     return GetMetaLen(offset, plus);
      else if (MidiMessageInfo.IsSystemCommon(ExpandedRSE))      return GetMetaLen(offset, plus);
      else if (MidiMessageInfo.IsSystemRealtime(ExpandedRSE))    return GetMetaLen(offset, plus);
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
      int ExpandedRSE = RunningStatus32 << 8;
      if (!MidiMessageInfo.IsMidiBMessage(RunningStatus32)) {
        Debug.Assert(false, string.Format("warning… {0:X2}", ExpandedRSE));
        return null;
      } else if (MidiMessageInfo.IsNoteOn(ExpandedRSE))
        return GetNoteMsg(0, offset + plus, StringRes.m9); else if (MidiMessageInfo.IsNoteOff(ExpandedRSE))
        return GetNoteMsg(0, offset + plus, StringRes.m8); else if (MidiMessageInfo.IsKeyAftertouch(ExpandedRSE))
        return string.Format(StringRes.mA, SmfFileHandle[SelectedTrackNumber, offset + plus], SmfFileHandle[SelectedTrackNumber, offset + plus + 1]); else if (MidiMessageInfo.IsControlChange(ExpandedRSE))
        return string.Format(StringRes.mB, SmfStringFormatter.cc[SmfFileHandle.Get8Bit(SelectedTrackNumber, offset + plus)].Replace((char)0xa, (char)0x20).Trim(), SmfFileHandle.Get8Bit(SelectedTrackNumber, offset + plus + 1)); else if (MidiMessageInfo.IsProgramChange(ExpandedRSE))
        return SmfStringFormatter.patches[SmfFileHandle[SelectedTrackNumber, offset + plus]].Replace((char)0xa, (char)0x20).Trim(); else if (MidiMessageInfo.IsChannelAftertouch(ExpandedRSE))
        return MidiMessageInfo.ChannelAftertouchRange.Name; else if (MidiMessageInfo.IsPitchBend(ExpandedRSE))
        return MidiMessageInfo.PitchBendRange.Name; else if (MidiMessageInfo.IsSystemMessage(ExpandedRSE))
        return MidiMessageInfo.SystemExclusiveMessageRange.Name; else if (MidiMessageInfo.IsSystemCommon(ExpandedRSE))
        return MidiMessageInfo.SystemCommonMessageRange.Name; else if (MidiMessageInfo.IsSystemRealtime(ExpandedRSE))
        return MidiMessageInfo.SystemRealtimeMessageRange.Name;
      return null;
    }

    // 
    // String Formatters
    // ---------------------------------

    /// <inheritdoc/>
    string GetNoteMsg(int shift, int offset, string format) { return string.Format(format, SmfFileHandle[SelectedTrackNumber, offset + shift], SmfFileHandle[SelectedTrackNumber, offset + shift + 1], SmfStringFormatter.GetKeySharp(SmfFileHandle[SelectedTrackNumber, offset + shift]), SmfStringFormatter.GetOctave(SmfFileHandle[SelectedTrackNumber, offset + shift])); }

    /// <inheritdoc/>
    public string chV(int v) { return string.Format("{0} {1}", string.Format("{0:X2}", RunningStatus32), GetEventValueString(v)); }
    
    /// <inheritdoc/>
    public string chRseV(int v) { return string.Format("{0} {1}", string.Format("{0:X2}", RunningStatus32), GetRseEventValueString(v)); }

  }
}
