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
  /// A static class containing unsigned short 16 bit integer Status Bytes Enumeration constants.
  /// 
  /// /// See https://www.midi.org/specifications-old/item/table-2-expanded-messages-list-status-bytes
  /// </summary>
  static public class Stat16
  {
    // Metadata

    /// <summary>0xFF00</summary>
    public const ushort SequenceNumber          = 0xFF00;
    /// <summary>0xFF01</summary>
    public const ushort Text                    = 0xFF01;
    /// <summary>0xFF02</summary>
    public const ushort Copyright               = 0xFF02;
    /// <summary>0xFF03</summary>
    public const ushort SequenceName            = 0xFF03;
    /// <summary>0xFF04</summary>
    public const ushort InstrumentName          = 0xFF04;
    /// <summary>0xFF05</summary>
    public const ushort Lyric                   = 0xFF05;
    /// <summary>0xFF06</summary>
    public const ushort Marker                  = 0xFF06;
    /// <summary>0xFF07</summary>
    public const ushort Cue                     = 0xFF07;
    /// <summary>0xFF08</summary>
    public const ushort MetaStrFF08             = 0xFF08;
    /// <summary>0xFF09 - encountered</summary>
    public const ushort MetaStrFF09             = 0xFF09;
    /// <summary>0xFF0A</summary>
    public const ushort MetaStrFF0A             = 0xFF0A;
    /// <summary>0xFF0A</summary>
    public const ushort MetaStrFF0B             = 0xFF0B;
    /// <summary>0xFF0A</summary>
    public const ushort MetaStrFF0C             = 0xFF0C;
    /// <summary>0xFF20</summary>
    public const ushort ChannelPrefix           = 0xFF20;
    /// <summary>0xFF21</summary>
    public const ushort PortMessage             = 0xFF21;
    /// <summary>0xFF2F</summary>
    public const ushort EndOfTrack              = 0xFF2F;
    /// <summary>0xFF51</summary>
    public const ushort SetTempo                = 0xFF51;
    /// <summary>0xFF54</summary>
    public const ushort SMPTEOffset             = 0xFF54;
    /// <summary>0xFF58</summary>
    public const ushort TimeSignature           = 0xFF58;
    /// <summary>0xFF59</summary>
    public const ushort KeySignature            = 0xFF59;
    /// <summary>0xFF7F</summary>
    public const ushort SequencerSpecific       = 0xFF7F;

    // Voice (Channel)

    /// <summary>0x80</summary>
    public const ushort NoteOff                 = 0x80;
    /// <summary>0x90</summary>
    public const ushort NoteOn                  = 0x90;
    /// <summary>0xA0</summary>
    public const ushort PolyphonicKeyPressure   = 0xA0;
    /// <summary>0xB0</summary>
    public const ushort ControlChange           = 0xB0;
    /// <summary>0xC0</summary>
    public const ushort ProgramChange           = 0xC0;
    /// <summary>0xD0</summary>
    public const ushort ChannelPressure         = 0xD0;
    /// <summary>0xE0</summary>
    public const ushort PitchWheel              = 0xE0;

    // System Common

    /// <summary>0xF0</summary>
    public const ushort SystemExclusive         = 0xF0;
    /// <summary>0xF1</summary>
    public const ushort MTC_QuarterFrameMessage = 0xF1;
    /// <summary>0xF2</summary>
    public const ushort SongPositionPointer     = 0xF2;
    /// <summary>0xF3</summary>
    public const ushort SongSelect              = 0xF3;
    /// <summary>0xF4</summary>
    public const ushort Unknown0                = 0xF4;
    /// <summary>0xF5</summary>
    public const ushort Unknown1                = 0xF5;
    /// <summary>0xF6</summary>
    public const ushort TuneRequest             = 0xF6;
    /// <summary>0xF7</summary>
    public const ushort Unknown2                = 0xF7;
    /// <summary>0xF7</summary>
    public const ushort EndOfExclusive          = Unknown2;

    // System Realtime

    /// <summary>0xF8</summary>
    public const ushort MIDI_Clock              = 0xF8;
    /// <summary>0xF8</summary>
    public const ushort Unknown3                = 0xF9;
    /// <summary>0xFA</summary>
    public const ushort MIDI_Start              = 0xFA;
    /// <summary>0xFB</summary>
    public const ushort MIDI_Continue           = 0xFB;
    /// <summary>0xFC</summary>
    public const ushort MIDI_Stop               = 0xFC;
    /// <summary>0xFD</summary>
    public const ushort Unknown4                = 0xFD;
    /// <summary>0xFE</summary>
    public const ushort ActiveSense             = 0xFE;
    /// <summary>0xFF</summary>
    public const ushort Reset                   = 0xFF;

  }

}
