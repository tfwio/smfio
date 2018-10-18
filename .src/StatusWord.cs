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
namespace on.smfio
{
  /// <summary>
  /// Unsigned IntU16 or ushort NativeType.
  /// </summary>
  public enum StatusWord : ushort
  {
    // Metadata

    /// <summary>0xFF00</summary>
    [System.ComponentModel.Description("Sequence Number")]
    SequenceNumber          = 0xFF00,
    /// <summary>0xFF01</summary>
    [System.ComponentModel.Description("Text")]
    Text                    = 0xFF01,
    /// <summary>0xFF02</summary>
    [System.ComponentModel.Description("Copyright")]
    Copyright               = 0xFF02,
    /// <summary>0xFF03</summary>
    [System.ComponentModel.Description("Sequence Name")]
    SequenceName            = 0xFF03,
    /// <summary>0xFF04</summary>
    [System.ComponentModel.Description("Instrument Name")]
    InstrumentName          = 0xFF04,
    /// <summary>0xFF05</summary>
    [System.ComponentModel.Description("Lyric")]
    Lyric                   = 0xFF05,
    /// <summary>0xFF06</summary>
    [System.ComponentModel.Description("Marker")]
    Marker                  = 0xFF06,
    /// <summary>0xFF07</summary>
    [System.ComponentModel.Description("Cue")]
    Cue                     = 0xFF07,
    /// <summary>0xFF08</summary>
    [System.ComponentModel.Description("MetaStrFF08")]
    MetaStrFF08             = 0xFF08,
    /// <summary>0xFF09 - encountered</summary>
    [System.ComponentModel.Description("MetaStrFF09")]
    MetaStrFF09             = 0xFF09,
    /// <summary>0xFF0A</summary>
    [System.ComponentModel.Description("MetaStrFF0A")]
    MetaStrFF0A             = 0xFF0A,
    /// <summary>0xFF0A</summary>
    [System.ComponentModel.Description("MetaStrFF0B")]
    MetaStrFF0B             = 0xFF0B,
    /// <summary>0xFF0A</summary>
    [System.ComponentModel.Description("MetaStrFF0C")]
    MetaStrFF0C             = 0xFF0C,
    /// <summary>0xFF20</summary>
    [System.ComponentModel.Description("Channel Prefix")]
    ChannelPrefix           = 0xFF20,
    /// <summary>0xFF21</summary>
    [System.ComponentModel.Description("Port Message")]
    PortMessage             = 0xFF21,
    /// <summary>0xFF2F</summary>
    [System.ComponentModel.Description("End of Track")]
    EndOfTrack              = 0xFF2F,
    /// <summary>0xFF51</summary>
    [System.ComponentModel.Description("Set Tempo")]
    SetTempo                = 0xFF51,
    /// <summary>0xFF54</summary>
    [System.ComponentModel.Description("SMPTE Offset")]
    SMPTEOffset             = 0xFF54,
    /// <summary>0xFF58</summary>
    [System.ComponentModel.Description("Time Signature")]
    TimeSignature           = 0xFF58,
    /// <summary>0xFF59</summary>
    [System.ComponentModel.Description("Key Signature")]
    KeySignature            = 0xFF59,
    /// <summary>0xFF7F (Was SystemSpecific)</summary>
    [System.ComponentModel.Description("Sequencer Specific")]
    SequencerSpecific       = 0xFF7F,

    // Voice (Channel)

    /// <summary>0x8c</summary>
    [System.ComponentModel.Description("Note Off")]
    NoteOff                 = 0x80,
    /// <summary>0x9c</summary>
    [System.ComponentModel.Description("Note On")]
    NoteOn                  = 0x90,
    /// <summary>0xAc</summary>
    [System.ComponentModel.Description("Polyphonic Key Pressure")]
    PolyphonicKeyPressure   = 0xA0,
    /// <summary>0xBc</summary>
    [System.ComponentModel.Description("Control Change")]
    ControlChange           = 0xB0,
    /// <summary>0xCc</summary>
    [System.ComponentModel.Description("Program Change")]
    ProgramChange           = 0xC0,
    /// <summary>0xDc</summary>
    [System.ComponentModel.Description("Channel Pressure")]
    ChannelPressure         = 0xD0,
    /// <summary>0xEc</summary>
    [System.ComponentModel.Description("Pitch Wheel")]
    PitchWheel              = 0xE0,

    // System Common

    /// <summary>0xF0</summary>
    [System.ComponentModel.Description("System Exclusive")]
    SystemExclusive         = 0xF0,
    /// <summary>0xF1</summary>
    [System.ComponentModel.Description("MTC Quarter FrameM essage")]
    MTC_QuarterFrameMessage = 0xF1,
    /// <summary>0xF2</summary>
    [System.ComponentModel.Description("Song Position Pointer")]
    SongPositionPointer     = 0xF2,
    /// <summary>0xF3</summary>
    [System.ComponentModel.Description("Song Select")]
    SongSelect              = 0xF3,
    /// <summary>0xF4</summary>
    [System.ComponentModel.Description("Unknown0")]
    Unknown0                = 0xF4,
    /// <summary>0xF5</summary>
    [System.ComponentModel.Description("Unknown1")]
    Unknown1                = 0xF5,
    /// <summary>0xF6</summary>
    [System.ComponentModel.Description("Tune Request")]
    TuneRequest             = 0xF6,
    /// <summary>0xF7</summary>
    [System.ComponentModel.Description("Unknown2")]
    Unknown2                = 0xF7,
    /// <summary>0xF7</summary>
    [System.ComponentModel.Description("End of System Exclusive")]
    EndOfExclusive          = Unknown2,

    // System Realtime

    /// <summary>0xF8</summary>
    [System.ComponentModel.Description("MIDI Clock")]
    MIDI_Clock              = 0xF8,
    /// <summary>0xF8</summary>
    [System.ComponentModel.Description("Unknown3")]
    Unknown3                = 0xF9,
    /// <summary>0xFA</summary>
    [System.ComponentModel.Description("MIDI Start")]
    MIDI_Start              = 0xFA,
    /// <summary>0xFB</summary>
    [System.ComponentModel.Description("MIDI Continue")]
    MIDI_Continue           = 0xFB,
    /// <summary>0xFC</summary>
    [System.ComponentModel.Description("MIDI Stop")]
    MIDI_Stop               = 0xFC,
    /// <summary>0xFD</summary>
    [System.ComponentModel.Description("Unknown4")]
    Unknown4                = 0xFD,
    /// <summary>0xFE</summary>
    [System.ComponentModel.Description("ActiveSense")]
    ActiveSense             = 0xFE,
    /// <summary>0xFF</summary>
    [System.ComponentModel.Description("Reset")]
    Reset                   = 0xFF,

  }

}
