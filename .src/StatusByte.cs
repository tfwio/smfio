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
namespace on.smfio
{
  public enum StatusByte : byte
  {
    // Metadata

    /// <summary>0x00</summary>
    SequenceNumber             = Stat8.SequenceNumber,
    /// <summary>0x01</summary>
    Text                       = Stat8.Text,
    /// <summary>0x02</summary>
    Copyright                  = Stat8.Copyright,
    /// <summary>0x03</summary>
    SequenceName               = Stat8.SequenceName,
    /// <summary>0x04</summary>
    InstrumentName             = Stat8.InstrumentName,
    /// <summary>0x05</summary>
    Lyric                      = Stat8.Lyric,
    /// <summary>0x06</summary>
    Marker                     = Stat8.Marker,
    /// <summary>0x07</summary>
    Cue                        = Stat8.Cue,
    /// <summary>0x08</summary>
    MetaStrFF08                = Stat8.MetaStrFF08,
    /// <summary>0x09</summary>
    MetaStrFF09                = Stat8.MetaStrFF09,
    /// <summary>0x0A</summary>
    MetaStrFF0A                = Stat8.MetaStrFF0A,
    /// <summary>0x0B</summary>
    MetaStrFF0B                = Stat8.MetaStrFF0B,
    /// <summary>0x0C</summary>
    MetaStrFF0C                = Stat8.MetaStrFF0C,
    /// <summary>0x20</summary>
    ChannelPrefix              = Stat8.ChannelPrefix,
    /// <summary>0x21</summary>
    /// TODO: Check me
    PortMessage                = Stat8.PortMessage,
    /// <summary>0x2F</summary>
    EndOfTrack                 = Stat8.EndOfTrack,
    /// <summary>0x51</summary>
    SetTempo                   = Stat8.SetTempo,
    /// <summary>0x54</summary>
    SMPTEOffset                = Stat8.SMPTEOffset,
    /// <summary>0x58</summary>
    TimeSignature              = Stat8.TimeSignature,
    /// <summary>0x59</summary>
    KeySignature               = Stat8.KeySignature,
    /// <summary>0x7F</summary>
    SequencerSpecific          = Stat8.SequencerSpecific,

    // Voice

    /// <summary>0x80</summary>
    NoteOff                    = Stat8.NoteOff,
    /// <summary>0x90</summary>
    NoteOn                     = Stat8.NoteOn,
    /// <summary>0xA0 (Key Aftertouch)</summary>
    PolyphonicKeyPressure      = Stat8.PolyphonicKeyPressure,
    /// <summary>0xB0</summary>
    ControlChange              = Stat8.ControlChange,
    /// <summary>0xC0</summary>
    ProgramChange              = Stat8.ProgramChange,
    /// <summary>0xD0 (Aftertouch)</summary>
    ChannelPressure            = Stat8.ChannelPressure,
    /// <summary>0xE0</summary>
    PitchWheel                 = Stat8.PitchWheel,

    // System Common

    /// <summary>0xF0</summary>
    SystemExclusive            = Stat8.SystemExclusive,
    /// <summary>0xF1</summary>
    MTC_QuarterFrameMessage    = Stat8.MTC_QuarterFrameMessage,
    /// <summary>0xF2</summary>
    SongPositionPointer        = Stat8.SongPositionPointer,
    /// <summary>0xF3</summary>
    SongSelect                 = Stat8.SongSelect,
    /// <summary>0xF4</summary>
    Unknown0                   = Stat8.Unknown0,
    /// <summary>0xF5</summary>
    Unknown1                   = Stat8.Unknown1,
    /// <summary>0xF6</summary>
    TuneRequest                = Stat8.TuneRequest,
    /// <summary>0xF7</summary>
    Unknown2                   = Stat8.Unknown2,
    /// <summary>0xF7 (only within system exclusive)</summary>
    EndOfExclusive             = Unknown2,
    // System Realtime

    /// <summary>0xF8</summary>
    MIDI_Clock                 = Stat8.MIDI_Clock,

    /// <summary>0xF9</summary>
    Unknown3                   = Stat8.Unknown3,

    /// <summary>0xFA</summary>
    MIDI_Start                 = Stat8.MIDI_Start,
    /// <summary>0xFB</summary>
    MIDI_Continue              = Stat8.MIDI_Continue,
    /// <summary>0xFC</summary>
    MIDI_Stop                  = Stat8.MIDI_Stop,
    /// <summary>0xFD</summary>
    Unknown4                   = Stat8.Unknown4,
    /// <summary>0xFE</summary>
    ActiveSense                = Stat8.ActiveSense,
    /// <summary>0xFF</summary>
    Reset                      = Stat8.Reset,
  }
}
