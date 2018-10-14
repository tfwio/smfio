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
  /// A static class containing Status Byte Enumeration constants.
  /// 
  /// See https://www.midi.org/specifications-old/item/table-2-expanded-messages-list-status-bytes
  /// </summary>
  static public class Stat8
  {
    // Metadata

    /// <summary>(0xFF) 0x00</summary>
    public const byte SequenceNumber             = 0x00;
    /// <summary>(0xFF) 0x01</summary>
    public const byte Text                       = 0x01;
    /// <summary>(0xFF) 0x02</summary>
    public const byte Copyright                  = 0x02;
    /// <summary>(0xFF) 0x03</summary>
    public const byte SequenceName               = 0x03;
    /// <summary>(0xFF) 0x04</summary>
    public const byte InstrumentName             = 0x04;
    /// <summary>(0xFF) 0x05</summary>
    public const byte Lyric                      = 0x05;
    /// <summary>(0xFF) 0x06</summary>
    public const byte Marker                     = 0x06;
    /// <summary>(0xFF) 0x07</summary>
    public const byte Cue                        = 0x07;
    /// <summary>(0xFF) 0x08</summary>
    public const byte MetaStrFF08                = 0x08;
    /// <summary>(0xFF) 0x09</summary>
    public const byte MetaStrFF09                = 0x09;
    /// <summary>(0xFF) 0x0A</summary>
    public const byte MetaStrFF0A                = 0x0A;
    /// <summary>(0xFF) 0x0B</summary>
    public const byte MetaStrFF0B                = 0x0B;
    /// <summary>(0xFF) 0x0C</summary>
    public const byte MetaStrFF0C                = 0x0C;
    /// <summary>(0xFF) 0x20</summary>
    public const byte ChannelPrefix              = 0x20;

    /// <summary>(0xFF) 0x21 IDK what in the H this is.</summary>
    /// TODO: Check me
    public const byte PortMessage                = 0x21;
    /// <summary>(0xFF) 0x2F</summary>
    public const byte EndOfTrack                 = 0x2F;
    /// <summary>(0xFF) 0x51</summary>
    public const byte SetTempo                   = 0x51;
    /// <summary>(0xFF) 0x54</summary>
    public const byte SMPTEOffset                = 0x54;
    /// <summary>(0xFF) 0x58</summary>
    public const byte TimeSignature              = 0x58;
    /// <summary>(0xFF) 0x59</summary>
    public const byte KeySignature               = 0x59;
    /// <summary>(0xFF) 0x7F</summary>
    public const byte SequencerSpecific          = 0x7F;

    // Voice (Channel)

    /// <summary>0x8c</summary>
    public const byte NoteOff                    = 0x80;
    /// <summary>0x9c</summary>
    public const byte NoteOn                     = 0x90;
    /// <summary>0xAc</summary>
    public const byte PolyphonicKeyPressure      = 0xA0;
    /// <summary>0xBc</summary>
    public const byte ControlChange              = 0xB0;
    /// <summary>0xCc</summary>
    public const byte ProgramChange              = 0xC0;
    /// <summary>0xDc</summary>
    public const byte ChannelPressure            = 0xD0;
    /// <summary>0xEc</summary>
    public const byte PitchWheel                 = 0xE0;
    
    // System Common
    
    /// <summary>0xF0</summary>
    public const byte SystemExclusive            = 0xF0;
    /// <summary>0xF1</summary>
    public const byte MTC_QuarterFrameMessage    = 0xF1;
    /// <summary>0xF2</summary>
    public const byte SongPositionPointer        = 0xF2;
    /// <summary>0xF3</summary>
    public const byte SongSelect                 = 0xF3;
    /// <summary>0xF4</summary>
    public const byte Unknown0                   = 0xF4;
    /// <summary>0xF5</summary>
    public const byte Unknown1                   = 0xF5;
    /// <summary>0xF6</summary>
    public const byte TuneRequest                = 0xF6;
    /// <summary>0xF7</summary>
    public const byte Unknown2                   = 0xF7;

    /// <summary>0xF7 (only within system exclusive)</summary>
    public const byte EndOfExclusive             = Unknown2;
    
    // System Realtime
    
    /// <summary>0xF8</summary>
    public const byte MIDI_Clock                 = 0xF8;
    /// <summary>0xF8</summary>
    public const byte Unknown3                   = 0xF9;
    /// <summary>0xFA</summary>
    public const byte MIDI_Start                 = 0xFA;
    /// <summary>0xFB</summary>
    public const byte MIDI_Continue              = 0xFB;
    /// <summary>0xFC</summary>
    public const byte MIDI_Stop                  = 0xFC;
    /// <summary>0xFD</summary>
    public const byte Unknown4                   = 0xFD;
    /// <summary>0xFE</summary>
    public const byte ActiveSense                = 0xFE;
    /// <summary>0xFF</summary>
    public const byte Reset                      = 0xFF;
    
  }
}
