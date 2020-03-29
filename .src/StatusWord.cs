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
    /// <summary>0xFF70 Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF70")] SequencerSpecific_FF70  = 0xFF70,
    /// <summary>0xFF71 Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF71")] SequencerSpecific_FF71  = 0xFF71,
    /// <summary>0xFF72 Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF72")] SequencerSpecific_FF72  = 0xFF72,
    /// <summary>0xFF73 Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF73")] SequencerSpecific_FF73  = 0xFF73,
    /// <summary>0xFF74 Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF74")] SequencerSpecific_FF74  = 0xFF74,
    /// <summary>0xFF75 Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF75")] SequencerSpecific_FF75  = 0xFF75,
    /// <summary>0xFF76 Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF76")] SequencerSpecific_FF76  = 0xFF76,
    /// <summary>0xFF77 Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF77")] SequencerSpecific_FF77  = 0xFF77,
    /// <summary>0xFF78 Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF78")] SequencerSpecific_FF78  = 0xFF78,
    /// <summary>0xFF79 Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF79")] SequencerSpecific_FF79  = 0xFF79,
    /// <summary>0xFF7A Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF7A")] SequencerSpecific_FF7A  = 0xFF7A,
    /// <summary>0xFF7B Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF7B")] SequencerSpecific_FF7B  = 0xFF7B,
    /// <summary>0xFF7C Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF7C")] SequencerSpecific_FF7C  = 0xFF7C,
    /// <summary>0xFF7D Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF7D")] SequencerSpecific_FF7D  = 0xFF7D,
    /// <summary>0xFF7E Sequencer Specific? (was: System Specific)</summary>
    [System.ComponentModel.Description("Sequencer Specific? 0xFF7E")] SequencerSpecific_FF7E  = 0xFF7E,
    /// <summary>0xFF7F Sequencer Specific (was: System Specific)</summary>
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
