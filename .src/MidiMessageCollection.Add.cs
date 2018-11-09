using System;
using on.smfio.Common;

namespace on.smfio
{
  public partial class MidiMessageCollection
  {

    #region METADATA MESSAGE (TEXT)

    /// <summary>
    /// Text: General (any)
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="text">Text</param>
    public void AddText(int channel, long tick, string text)
    {
      AddV(channel, new MidiMessage(Stat16.Text, tick, System.Text.Encoding.Default.GetBytes(text)));
    }
    /// <summary>
    /// Text: Copyright
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="text">Text</param>
    public void AddTextCopyright(int channel, long tick, string text)
    {
      AddV(channel, new MidiMessage(Stat16.Copyright, tick, System.Text.Encoding.Default.GetBytes(text)));
    }
    /// <summary>
    /// Text: Track/Sequence Name
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="text">Text</param>
    public void AddTextTrackOrSequenceName(int channel, long tick, string text)
    {
      AddV(channel, new MidiMessage(Stat16.SequenceName, tick, System.Text.Encoding.Default.GetBytes(text)));
    }
    /// <summary>
    /// Text: Instrument Name
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="text">Text</param>
    public void AddTextInstrumentName(int channel, long tick, string text)
    {
      AddV(channel, new MidiMessage(Stat16.InstrumentName, tick, System.Text.Encoding.Default.GetBytes(text)));
    }
    /// <summary>
    /// Text: Lyric
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="text">Text</param>
    public void AddTextLyric(int channel, long tick, string text)
    {
      AddV(channel, new MidiMessage(Stat16.Lyric, tick, System.Text.Encoding.Default.GetBytes(text)));
    }
    /// <summary>
    /// Text: Marker
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="text">Text</param>
    public void AddTextMarker(int channel, long tick, string text)
    {
      AddV(channel, new MidiMessage(Stat16.Marker, tick, System.Text.Encoding.Default.GetBytes(text)));
    }
    /// <summary>
    /// Text: Cue Point
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="text">Text</param>
    public void AddTextCuePoint(int channel, long tick, string text)
    {
      AddV(channel, new MidiMessage(Stat16.Cue, tick, System.Text.Encoding.Default.GetBytes(text)));
    }
    
    #endregion

    #region METADATA MESSAGE

    /// <summary>
    /// Sequence Number message.
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="seqNo"></param>
    public void AddSequenceNumber(int channel, long tick, byte seqNo)
    {
      AddV(channel, new MidiMessage(Stat16.SequenceNumber, tick, seqNo));
    }
    /// <summary>
    /// MIDI Channel Prefix message.
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="prefix"></param>
    public void AddMidiChannelPrefix(int channel, long tick, byte prefix)
    {
      AddV(channel, new MidiMessage(Stat16.ChannelPrefix, tick, prefix));
    }
    /// <summary>
    /// Port message.
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="port">Port ID (0 - 15)</param>
    public void AddPort(int channel, long tick, byte port)
    {
      AddV(channel, new MidiMessage(Stat16.PortMessage, tick, port));
    }
    /// <summary>
    /// End of Track message.
    /// 
    /// Adds a 'End of Track' message followed by a data value of zero.
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="tick"></param>
    public void AddEOT(int channel, long tick)
    {
      AddV(channel, new MidiMessage(Stat16.ChannelPrefix, tick, 0));
    }
    /// <summary>
    /// Set Tempo message.
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="muspqn">the value must be convertable to 24 bit</param>
    public void AddSetTempo(int channel, long tick, int muspqn)
    {
      // muspqn
      byte[] bytes = BitConverter.GetBytes(muspqn);
      if (!BitConverter.IsLittleEndian) Array.Reverse(bytes);
      AddV(channel, new MidiMessage(Stat16.SetTempo, tick, bytes[1], bytes[2], bytes[3]));
      bytes = null;
    }
    /// <summary>
    /// Set Tempo message.
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="bpm">the value must be convertable to 24 bit</param>
    public void AddSetTempo(int channel, long tick, double bpm)
    {
      // TODO: CHECK ME!
      const double mussec = 60000000d;
      byte[] bytes = BitConverter.GetBytes(Convert.ToInt32(mussec / bpm));
      if (!BitConverter.IsLittleEndian) Array.Reverse(bytes);
      AddV(channel, new MidiMessage(Stat16.SetTempo, tick, bytes[1], bytes[2], bytes[3]));
      bytes = null;
    }
    /// <summary>
    /// SMPTE Offset message.
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="type">SmpteType tells us the frame-rate and is encoded into the hour(s) byte when written to MTrk data.</param>
    /// <param name="h">Hour</param>
    /// <param name="m">Minute</param>
    /// <param name="s">Second</param>
    /// <param name="fr">Frame</param>
    /// <param name="ff">Frame Fraction</param>
    public void AddSmpteOffset(int channel, long tick, SmpteType type, byte h, byte m, byte s, byte fr, byte ff)
    {
      AddV(channel, new MidiMessage(Stat16.SMPTEOffset, tick, (byte)(((int)type << 5) + h), m, s, fr, ff));
    }
    /// <summary>
    /// **Time Signature** message.
    /// 
    /// For programs that display musical notation.
    /// 
    /// Usually we see 4/4 (n=4, d=4) at 24 clocks and 8 32nds.
    /// 
    /// [status: ff 54, size: 05, numerator: nn, denominator: dd,
    /// clocks: cc, 32nds: bb]  
    /// The time signature is expressed as four numbers.
    /// nn and dd represent the numerator and denominator of the time
    /// signature as it would be notated.The denominator is a negative
    /// power of two: 2 represents a quarter-note, 3 represents an
    /// eighth-note, etc.
    /// 
    /// The cc parameter expresses the
    /// number of MIDI clocks in a metronome click.The bb parameter
    /// expresses the number of notated 32nd-notes in a MIDI quarter
    /// note(24 MIDI clocks). This was added because there are already
    /// multiple programs which allow a user to specify that what MIDI
    /// thinks of as a quarter-note(24 clocks) is to be notated as, or
    /// related to in terms of, something else.
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="numerator">n in n / d (numerator)</param>
    /// <param name="denominator">
    /// `d` in `n / d` (denominator).  
    /// In MTrk, denominator is provided by providing the power of 2.
    /// Here, we simply calculate it for you.
    /// </param>
    /// <param name="clocks">MIDI clocks in a metronome click</param>
    /// <param name="thirtySecondsPQN">The bb parameter expresses the number of notated 32nd-notes in a MIDI quarter-note(24 MIDI clocks)</param>
    public void AddTimeSignature(int channel, long tick, byte numerator, byte denominator, byte clocks, byte thirtySecondsPQN)
    {
      int d = denominator;
      byte pow = 0;
      while (true) {
        if (d == 2) break;
        pow++;
        d = d / 2;
      }
      AddV(channel, new MidiMessage(Stat16.TimeSignature, tick, numerator, pow, clocks, thirtySecondsPQN));
    }
    /// <summary>
    /// **Key Signature** message.
    /// 
    /// For programs that display musical notation.
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="sf">KeySignatureType</param>
    /// <param name="mi">if True, 'Minor' else 'Major'</param>
    public void AddKeySignature(int channel, long tick, KeySignatureType sf, bool mi)
    {
      AddV(channel, new MidiMessage(Stat16.KeySignature, tick, (byte)sf, (byte)(mi ? 1 : 0)));
    }
    /// <summary>
    /// Special requirements for particular sequencers may use this event type:
    /// the first byte or bytes of data is a manufacturer ID (these are one byte,
    /// or if the first byte is 00, three bytes).
    /// As with MIDI System Exclusive, manufacturers who define something using
    /// this meta-event should publish it so that others may be used by a
    /// sequencer which elects to use this as its only file format; sequencers
    /// with their established feature-specific formats should probably stick
    /// to the standard features when using this format.
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="tick"></param>
    /// <param name="data"></param>
    public void AddSystemSpecific(int channel, long tick, params byte[] data)
    {
      AddV(channel, new MidiMessage(Stat16.SequencerSpecific, tick, data));
    }

    #endregion
    #region VOICE MESSAGES

    /// <summary>
    /// Note Off
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="key">0-127</param>
    /// <param name="velocity">0-127</param>
    public void AddNoteOff(int channel, long tick, byte key, byte velocity)
    {
      AddV(channel, new MidiMessage(Stat16.NoteOff, tick, key, velocity));
    }
    /// <summary>
    /// Note On
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="key">0-127</param>
    /// <param name="velocity">0-127</param>
    public void AddNoteOn(int channel, long tick, byte key, byte velocity)
    {
      AddV(channel, new MidiMessage(Stat16.NoteOn, tick, key, velocity));
    }
    /// <summary>
    /// NOTE: be aware that you have to make sure to call
    /// <see cref="MidiMessageCollection.FinalSort"/>
    /// when using this as it sends two messages which will span multiple
    /// indexes apart before calling <see cref="MidiMessageCollection.ToFile"/>.
    /// 
    /// Different from other AddNoteOn/Off methods, this one accepts `tickLength`
    /// which allows us to send a note (note on and off: two messages) in full
    /// which entails sending a note-on message followed by a note-off message.
    /// 
    /// If velocityOff is set to zero allows for us to send a note-on with
    /// velocity set to 0 as the note-off message which in turn enables the
    /// writer to write a shorter MTrk in the smf-midi file (via running-status
    /// messages).
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="tick"></param>
    /// <param name="tickLength"></param>
    /// <param name="key"></param>
    /// <param name="velocityOn"></param>
    /// <param name="velocityOff"></param>
    public void AddNote(int channel, long tick, int tickLength, byte key, byte velocityOn, byte velocityOff=0)
    {
      AddV(channel, new MidiMessage(Stat16.PolyphonicKeyPressure, tick, key, velocityOn));
      if (velocityOff > 0) AddV(channel, new MidiMessage(Stat16.NoteOff, tick+tickLength, key, velocityOff));
      else AddV(channel, new MidiMessage(Stat16.NoteOn, tick + tickLength, key, velocityOff));
    }
    /// <summary>
    /// Polyphonic Key Pressure
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="key">0-127</param>
    /// <param name="velocity">0-127</param>
    public void AddPolyKeyPressure(int channel, long tick, byte key, byte velocity)
    {
      AddV(channel, new MidiMessage(Stat16.PolyphonicKeyPressure, tick, key, velocity));
    }
    /// <summary>
    /// Control Change
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="controller">0-127</param>
    /// <param name="value">0-127</param>
    public void AddControlChange(int channel, long tick, byte controller, byte value)
    {
      AddV(channel, new MidiMessage(Stat16.ControlChange, tick, controller, value));
    }
    /// <summary>
    /// Control Change
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="controller">ControllerType enumeration value</param>
    /// <param name="value">0-127</param>
    public void AddControlChange(int channel, long tick, ControllerType controller, byte value)
    {
      AddV(channel, new MidiMessage(Stat16.ControlChange, tick, (byte)controller, value));
    }
    /// <summary>
    /// Program/Patch Change
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="patch">0-127</param>
    public void AddProgramChange(int channel, long tick, byte patch)
    {
      AddV(channel, new MidiMessage(Stat16.ProgramChange, tick, patch));
    }
    /// <summary>
    /// Channel Pressure
    /// </summary>
    /// <param name="channel">MIDI Channel No.</param>
    /// <param name="tick">absolute pulse count</param>
    /// <param name="pressure">pressure value</param>
    public void AddChannelPressure(int channel, long tick, byte pressure)
    {
      AddV(channel, new MidiMessage(Stat16.ChannelPressure, tick, pressure));
    }
    public void AddPitchWheelChange(int channel, long tick, byte lsb, byte msb)
    {
      AddV(channel, new MidiMessage(Stat16.PitchWheel, tick, lsb, msb));
    }

    #endregion

    #region System
    /// <summary>
    /// When adding system exclusive data here, (*NOT SYSTEM-SPECIFIC*)
    /// be sure that the data starts with 0xF0 and ends with 0xF7.
    /// 
    /// I.E. `AddSystemExclusive(0, 0, 0xF0, …, 0xF7);`
    /// 
    /// See: <see cref="AddSystemSpecific(int,long,byte[])"/>
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="tick"></param>
    /// <param name="data"></param>
    public void AddSystemExclusive(int channel, long tick, params byte[] data)
    {
      AddV(channel, new MidiMessage(Stat16.SystemExclusive, tick, data));
    }
    #endregion

  }
}
