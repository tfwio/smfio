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

namespace on.smfio.Common
{
	static public class StatusQuery
	{
		/// <summary>0x80 to 0x8F</summary>
		static public readonly StatRange NoteOffRange                 = StatRange.ForChannel(Stat8.NoteOff, "Note Off");
		
		/// <summary>0x90 to 0x9F</summary>
		static public readonly StatRange NoteOnRange                  = StatRange.ForChannel(Stat8.NoteOn, "Note On");
		
		/// <summary>0xA0 to 0xAF</summary>
		static public readonly StatRange KeyAftertouchRange           = StatRange.ForChannel(Stat8.PolyphonicKeyPressure, "Key Aftertouch");
		
		/// <summary>0xB0-F</summary>
		static public readonly StatRange ControlChangeRange           = StatRange.ForChannel(Stat8.ControlChange, "Control Change");
		
		/// <summary>0xC0-F</summary>
		static public readonly StatRange ProgramChangeRange           = StatRange.ForChannel(Stat8.ProgramChange, "Program Change");
		
		/// <summary>0xD0-F</summary>
		static public readonly StatRange ChannelAftertouchRange       = StatRange.ForChannel(Stat8.ChannelPressure,"Channel Aftertouch");
		
		/// <summary>0xE0-F</summary>
		static public readonly StatRange PitchBendRange               = StatRange.ForChannel(Stat8.PitchWheel,"Pitch Bend");
		
		/// <summary>0xF0-F</summary>
		static public readonly StatRange SystemExclusiveRange         = new StatRange(StatusByte.SystemExclusive, "System Exclusive Message");
		
		/// <summary>0xF0</summary>
		static public readonly StatRange SequencerSpeceficRange       = new StatRange(StatusByte.SequencerSpecific, "System Specific Message");
		
		/// <summary>0xF0 &lt;= value &lt;= 0xF7</summary>
		static public readonly StatRange SystemCommonMessageRange     = new StatRange(StatusByte.SystemExclusive, StatusByte.EndOfExclusive, "System Exclusive Message");
		
		/// <summary>F8 to FF</summary>
		static public readonly StatRange SystemRealtimeRange          = new StatRange(StatusByte.MIDI_Clock, StatusByte.Reset, "System Realtime Message");
		
    /// <summary>0xF0 (System Common)</summary>
    static public bool IsSequencerSpecific(int msg) { return SequencerSpeceficRange.IsInRange(msg); }
    
    /// <summary>0xF0 (System Common)</summary>
    static public bool IsSystemExclusive(int msg) { return SystemExclusiveRange.IsInRange(msg); }

		/// <summary>0xF0 &lt;= value &lt;= 0xF7</summary>
		static public bool IsSystemCommon(int msg) { return SystemCommonMessageRange.IsInRange(msg); }

		/// <summary>0XF8 &lt;= value &lt;= 0XFF</summary>
		static public bool IsSystemRealtime(int msg) { return SystemRealtimeRange.IsInRange(msg); }

		/// <summary>0x80 &lt;= value &lt;= 0x8F</summary>
		static public bool IsNoteOff(int msg) { return NoteOffRange.IsInRange(msg); }

		/// <summary>0x90 &lt;= value &lt;= 0x9F</summary>
		static public bool IsNoteOn(int msg) { return NoteOnRange.IsInRange(msg); }

		/// <summary>0xA0 &lt;= value &lt;= 0xAF</summary>
		static public bool IsKeyAftertouch(int msg) { return KeyAftertouchRange.IsInRange(msg); }

		/// <summary>0xB0 &lt;= value &lt;= 0xBF</summary>
		static public bool IsControlChange(int msg) { return ControlChangeRange.IsInRange(msg); }

		/// <summary>0xC0 &lt;= value &lt;= 0xCF</summary>
		static public bool IsProgramChange(int msg) { return ProgramChangeRange.IsInRange(msg); }

		/// <summary>0xD0  &lt;= value &lt;= 0xDF</summary>
		static public bool IsChannelAftertouch(int msg) { return ChannelAftertouchRange.IsInRange(msg); }

		/// <summary>0xE0  &lt;= value &lt;= 0xEF</summary>
		static public bool IsPitchBend(int msg) { return PitchBendRange.IsInRange(msg); }

		/// <summary>True if 0x80 &lt;= value &lt;= 0xFF;</summary>
		static public bool IsMidiMessage(int value) { return value > NoteOffRange.Min && value < SystemRealtimeRange.Max; }
		
		/// <summary>True if 0x80 &lt;= value &lt;= 0xFF;</summary>
		static public bool IsMidiBMessage(int value) { return value >= 0x80 && value <= 0xFF; }
	}
}
