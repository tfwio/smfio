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
    SequenceNumber          = 0xFF00,
    /// <summary>0xFF01</summary>
    Text                    = 0xFF01,
    /// <summary>0xFF02</summary>
    Copyright               = 0xFF02,
    /// <summary>0xFF03</summary>
    SequenceName            = 0xFF03,
    /// <summary>0xFF04</summary>
    InstrumentName          = 0xFF04,
    /// <summary>0xFF05</summary>
    Lyric                   = 0xFF05,
    /// <summary>0xFF06</summary>
    Marker                  = 0xFF06,
    /// <summary>0xFF07</summary>
    Cue                     = 0xFF07,
    /// <summary>0xFF20</summary>
    ChannelPrefix           = 0xFF20,
    /// <summary>0xFF21</summary>
    PortMessage             = 0xFF21,
    /// <summary>0xFF2F</summary>
    EndOfTrack              = 0xFF2F,
    /// <summary>0xFF51</summary>
    SetTempo                = 0xFF51,
    /// <summary>0xFF54</summary>
    SMPTEOffset             = 0xFF54,
    /// <summary>0xFF58</summary>
    TimeSignature           = 0xFF58,
    /// <summary>0xFF59</summary>
    KeySignature            = 0xFF59,
    /// <summary>0xFF7F (Was SystemSpecific)</summary>
    SequencerSpecific       = 0xFF7F,

    // Voice (Channel)

    /// <summary>0x8c</summary>
    NoteOff                 = 0x80,
    /// <summary>0x9c</summary>
    NoteOn                  = 0x90,
    /// <summary>0xAc</summary>
    PolyphonicKeyPressure   = 0xA0,
    /// <summary>0xBc</summary>
    ControlChange           = 0xB0,
    /// <summary>0xCc</summary>
    ProgramChange           = 0xC0,
    /// <summary>0xDc</summary>
    ChannelPressure         = 0xD0,
    /// <summary>0xEc</summary>
    PitchWheel              = 0xE0,

    // System Common

    /// <summary>0xF0</summary>
    SystemExclusive         = 0xF0,
    /// <summary>0xF1</summary>
    MTC_QuarterFrameMessage = 0xF1,
    /// <summary>0xF2</summary>
    SongPositionPointer     = 0xF2,
    /// <summary>0xF3</summary>
    SongSelect              = 0xF3,
    /// <summary>0xF4</summary>
    Unknown0                = 0xF4,
    /// <summary>0xF5</summary>
    Unknown1                = 0xF5,
    /// <summary>0xF6</summary>
    TuneRequest             = 0xF6,
    /// <summary>0xF7</summary>
    Unknown2                = 0xF7,
    /// <summary>0xF7</summary>
    EndOfExclusive          = Unknown2,

    // System Realtime

    /// <summary>0xF8</summary>
    MIDI_Clock              = 0xF8,
    /// <summary>0xF8</summary>
    Unknown3                = 0xF9,
    /// <summary>0xFA</summary>
    MIDI_Start              = 0xFA,
    /// <summary>0xFB</summary>
    MIDI_Continue           = 0xFB,
    /// <summary>0xFC</summary>
    MIDI_Stop               = 0xFC,
    /// <summary>0xFD</summary>
    Unknown4                = 0xFD,
    /// <summary>0xFE</summary>
    ActiveSense             = 0xFE,
    /// <summary>0xFF</summary>
    Reset                   = 0xFF,

  }

}
