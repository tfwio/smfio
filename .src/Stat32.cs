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
  static public class Stat32
  {
    // Metadata

    /// <summary>0xFF00</summary>
    public const int SequenceNo                 = 0xFF00;
    /// <summary>0xFF01</summary>
    public const int Text                       = 0xFF01;
    /// <summary>0xFF02</summary>
    public const int Copyright                  = 0xFF02;
    /// <summary>0xFF03</summary>
    public const int SequenceName               = 0xFF03;
    /// <summary>0xFF04</summary>
    public const int InstrumentName             = 0xFF04;
    /// <summary>0xFF05</summary>
    public const int Lyric                      = 0xFF05;
    /// <summary>0xFF06</summary>
    public const int Marker                     = 0xFF06;
    /// <summary>0xFF07</summary>
    public const int Cue                        = 0xFF07;
    /// <summary>0xFF20</summary>
    public const int ChannelPrefix              = 0xFF20;
    /// <summary>0xFF21</summary>
    public const int PortMessage                = 0xFF21;
    /// <summary>0xFF2F</summary>
    public const int EndOfTrack                 = 0xFF2F;
    /// <summary>0xFF51</summary>
    public const int SetTempo                   = 0xFF51;
    /// <summary>0xFF54</summary>
    public const int SMPTEOffset                = 0xFF54;
    /// <summary>0xFF58</summary>
    public const int TimeSignature              = 0xFF58;
    /// <summary>0xFF59</summary>
    public const int KeySignature               = 0xFF59;
    /// <summary>0xFF7F (Was SystemSpecific)</summary>
    public const int SequencerSpecific          = 0xFF7F;

    // Voice (Channel)

    public const int NoteOff                    = 0x80;
    public const int NoteOn                     = 0x90;
    public const int PolyphonicKeyPressure      = 0xA0;
    public const int ControlChange              = 0xB0;
    public const int ProgramChange              = 0xC0;
    public const int ChannelPressure            = 0xD0;
    public const int PitchWheel                 = 0xE0;

    // System Common

    /// <summary>0xF0</summary>
    public const int SystemExclusive            = 0xF0;
    /// <summary>0xF1</summary>
    public const int MTC_QuarterFrameMessage    = 0xF1;
    /// <summary>0xF2</summary>
    public const int SongPositionPointer        = 0xF2;
    /// <summary>0xF3</summary>
    public const int SongSelect                 = 0xF3;
    /// <summary>0xF4</summary>
    public const int Unknown0                   = 0xF4;
    /// <summary>0xF5</summary>
    public const int Unknown1                   = 0xF5;
    /// <summary>0xF6</summary>
    public const int TuneRequest                = 0xF6;
    /// <summary>0xF7</summary>
    public const int Unknown2                   = 0xF7;
    /// <summary>0xF7</summary>
    public const int EndOfExclusive             = Unknown2;

    // System Realtime

    /// <summary>0xF8</summary>
    public const int MIDI_Clock                 = 0xF8;
    /// <summary>0xF8</summary>
    public const int Unknown3                   = 0xF9;
    /// <summary>0xFA</summary>
    public const int MIDI_Start                 = 0xFA;
    /// <summary>0xFB</summary>
    public const int MIDI_Continue              = 0xFB;
    /// <summary>0xFC</summary>
    public const int MIDI_Stop                  = 0xFC;
    /// <summary>0xFD</summary>
    public const int Unknown4                   = 0xFD;
    /// <summary>0xFE</summary>
    public const int ActiveSense                = 0xFE;
    /// <summary>0xFF</summary>
    public const int Reset                      = 0xFF;

  }
}
