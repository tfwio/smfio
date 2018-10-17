/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace on.smfio
{
  public class NoteParser : Reader, INoteParser
  {
    public NoteParser() : base() { }
    public NoteParser(MidiEventDelegate handler) : base(handler) { }
    public NoteParser(string fileName) : base(fileName) { }
    
    public List<MidiData> Notes {
      get { return notes; } set { notes = value; }
    } List<MidiData> notes = new List<MidiData>();

    // ==========================================================================
    //
    // This is an example usage having added this to the MidiParser as follows 
    // 
    // ==========================================================================
    // 
    //  if (!SomeContainer.MidiParser.MessageHandlers.Contains(GotMidiEventD))
    //    SomeContainer.MidiParser.MessageHandlers.Add(GotMidiEventD);
    // 
    // ==========================================================================
    // 
    // The following method is triggered on channel-selection.
    // 
    // - see MidiReader.OnMidiMessage()
    // - see MidiReader.DispatchHandlers()
    // 
    // ==========================================================================
    // 
    // static public void GotMidiEventD(MidiMsgType t, int track, int offset, int imsg, byte bmsg, ulong ppq, int rse, bool isrse)
    // {
    //   if ( t== MidiMsgType.NoteOn || t== MidiMsgType.NoteOff )
    //   {
    //     UserInterface.MidiParser.CheckNote(t,ppq,Convert.ToByte((rse) & 0x0F),offset,bmsg,isrse);
    //   }
    // }
    // 
    // ==========================================================================

    /// <inheritdoc/>
    public void CheckNote(MidiMsgType type, long ppq, byte ch, int offset, byte b, bool rs)
    {
      byte n = 0, v = 0;
      switch (type)
      {
        case MidiMsgType.ControllerChange:
          n = this[ReaderIndex, offset + (rs ? 0 : 1)];
          v = this[ReaderIndex, offset + (rs ? 0 : 1) + 1];
          break;
        case MidiMsgType.NoteOn:
          n = this[ReaderIndex, offset + (rs ? 0 : 1)];
          v = this[ReaderIndex, offset + (rs ? 0 : 1) + 1];
          if (v == 0) CloseNote(ppq, n, v);
          else Notes.Add(new MidiNote(ch, n, ppq, v));
          break;
        case MidiMsgType.NoteOff:
          n = this[ReaderIndex, offset + (rs ? 0 : 1)];
          v = this[ReaderIndex, offset + (rs ? 0 : 1) + 1];
          CloseNote(ppq, n, v);
          break;
      }
    }

    /// <inheritdoc/>
    public void CloseNote(long ppq, byte k, short v)
    {
      var note = GetNote(k, -1) as MidiNote;
      if (note == null)
      {
        Console.WriteLine("note wasn't found: {0} {1}:{2}", TimeUtil.GetMBT(ppq, Division), k, v);
        return;
      }
      note.PulseWidth = Convert.ToInt32(ppq - note.Pulse);
      note.noteOffVelocity = v;
    }

    /// <inheritdoc/>
    public MidiData GetNote(byte k, short v)
    {
      return Notes.FirstOrDefault((N => CheckNote(N, k, -1)));
    }

    bool CheckNote(MidiData d, byte k, short v)
    {
      if (d is MidiNote) // check for key and velocity?
        return (d as MidiNote).K == k && (d as MidiNote).noteOffVelocity == v;
      return false;
    }
  }
}
