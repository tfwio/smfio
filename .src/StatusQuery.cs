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
using System.Linq;
namespace on.smfio.Common
{
	static public class StatusQuery
  {
    /// <summary>Metadata 0xFF00 to 0xFF07; </summary>
    static public readonly StatRange MetadataRange                = new StatRange("Metadata", new Range(0xFF00, 0xFF07)/*, 0xFF20 (port message?*/, 0xFF2F, 0xFF51, 0xFF54, 0xFF58, 0xFF59, 0xFF7F );

    /// <summary>Channel Voice 0xA0 to 0xEF — Note: 0xCd (program) and 0xDd (channel-pressure) messages only carry one byte as the rest carry two.</summary>
    static public readonly StatRange ChannelVoiceRange            = new StatRange("Channel Voice", new Range(0x80, 0xEF));

    /// <summary>Channel Voice 0xF0 to 0xF7</summary>
    static public readonly StatRange SystemCommonRange            = new StatRange("System Common", new Range(0xF0, 0xF7));
    
    /// <summary>System Realtime 0xF8 to 0xFF</summary>
    static public readonly StatRange SystemRealtimeRange          = new StatRange("System Realtime", new Range(0xF8, 0xFF));

    /// <summary>0x80 to 0x8F</summary>
    static public readonly StatRange NoteOffRange                 = new StatRange("Note Off", Stat16.NoteOff);
		
		/// <summary>0x90 to 0x9F</summary>
		static public readonly StatRange NoteOnRange                  = new StatRange("Note On", Stat16.NoteOn);
		
		/// <summary>0xA0 to 0xAF</summary>
		static public readonly StatRange KeyAftertouchRange           = new StatRange("Key Aftertouch", Stat16.PolyphonicKeyPressure);
		
		/// <summary>0xB0 to 0xBF</summary>
		static public readonly StatRange ControlChangeRange           = new StatRange("Control Change", Stat16.ControlChange);
		
		/// <summary>0xC0 to 0xCF</summary>
		static public readonly StatRange ProgramChangeRange           = new StatRange("Program Change", Stat16.ProgramChange);
		
		/// <summary>0xD0 to 0xDF</summary>
		static public readonly StatRange ChannelAftertouchRange       = new StatRange("Channel Aftertouch", Stat16.ChannelPressure);
		
		/// <summary>0xE0 to 0xEF</summary>
		static public readonly StatRange PitchBendRange               = new StatRange("Pitch Bend", Stat16.PitchWheel);
		
		/// <summary>0xF0 to 0xF0</summary>
		static public readonly StatRange SystemExclusiveRange         = new StatRange("System Exclusive Message", Stat16.SystemExclusive);

    /// <summary>0xFF 0x7F</summary>
    static public readonly StatRange SequencerSpeceficRange       = new StatRange("System Specific Message", Stat16.SequencerSpecific);
		
		/// <summary>0xF0 &lt;= value &lt;= 0xF7</summary>
		static public readonly StatRange SystemCommonMessageRange     = new StatRange("System Exclusive Message", Stat16.SystemExclusive, Stat16.EndOfExclusive);
		

    /// <summary>0xFF 0x7F (Sequencer Specific Binary Data)</summary>
    static public bool IsSequencerSpecific(int msg) { return SequencerSpeceficRange.Match(msg); }
    
    /// <summary>0xF0 (System Common)</summary>
    static public bool IsSystemExclusive(int msg) { return SystemExclusiveRange.Match(msg); }

		/// <summary>0xF0 &lt;= value &lt;= 0xF7</summary>
		static public bool IsSystemCommon(int msg) { return SystemCommonMessageRange.Match(msg); }

		/// <summary>0XF8 &lt;= value &lt;= 0XFF</summary>
		static public bool IsSystemRealtime(int msg) { return SystemRealtimeRange.Match(msg); }

		/// <summary>0x80 &lt;= value &lt;= 0x8F</summary>
		static public bool IsNoteOff(int msg) { return NoteOffRange.Match(msg); }

		/// <summary>0x90 &lt;= value &lt;= 0x9F</summary>
		static public bool IsNoteOn(int msg) { return NoteOnRange.Match(msg); }

		/// <summary>0xA0 &lt;= value &lt;= 0xAF</summary>
		static public bool IsKeyAftertouch(int msg) { return KeyAftertouchRange.Match(msg); }

		/// <summary>0xB0 &lt;= value &lt;= 0xBF</summary>
		static public bool IsControlChange(int msg) { return ControlChangeRange.Match(msg); }

		/// <summary>0xC0 &lt;= value &lt;= 0xCF</summary>
		static public bool IsProgramChange(int msg) { return ProgramChangeRange.Match(msg); }

		/// <summary>0xD0  &lt;= value &lt;= 0xDF</summary>
		static public bool IsChannelAftertouch(int msg) { return ChannelAftertouchRange.Match(msg); }

		/// <summary>0xE0  &lt;= value &lt;= 0xEF</summary>
		static public bool IsPitchBend(int msg) { return PitchBendRange.Match(msg); }

		/// <summary>True if 0x80 &lt;= value &lt;= 0xFF;</summary>
		static public bool IsMidiMessage(int value) { return ChannelVoiceRange.Match(value) | MetadataRange.Match(value); }
		
		/// <summary>True if 0x80 &lt;= value &lt;= 0xFF;</summary>
		static public bool IsMidiBMessage(int value) { return value >= 0x80 && value <= 0xFF; }
	}
}
