#region Info
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
#endregion
#region Using
using System;
#endregion

namespace on.smfio.Common
{
	static public class MidiMessageInfo
	{
		static public readonly NoteRange NoteOffRange							= new NoteRange(ChMessageU16.NoteOff, "Note Off");
		static public readonly NoteRange NoteOnRange							= new NoteRange(ChMessageU16.NoteOn, "Note On");
		static public readonly NoteRange KeyAftertouchRange				= new NoteRange(ChMessageU16.KeyAftertouch, "Key Aftertouch");
		static public readonly NoteRange ControlChangeRange				= new NoteRange(ChMessageU16.ControlChange, "Control Change");
		static public readonly NoteRange ProgramChangeRange				= new NoteRange(ChMessageU16.ProgramChange, "Program Change");
		static public readonly NoteRange ChannelAftertouchRange		= new NoteRange(ChMessageU16.ChannelAftertouch,"Channel Aftertouch");
		static public readonly NoteRange PitchBendRange						= new NoteRange(ChMessageU16.PitchBend,"Pitch Bend");
		/// <summary>F0</summary>
		static public readonly NoteRange SystemExclusiveMessageRange	= new NoteRange(ChMessageU16.SystemMessage,ChMessageU16.SystemMessage, "System Exclusive Message");
		/// <summary>F1 to F7</summary>
		static public readonly NoteRange SystemCommonMessageRange	= new NoteRange(ChMessageU16.SystemCommonLo,ChMessageU16.SystemCommonHi, "System Exclusive Message");
		/// <summary>F8 to FF</summary>
		static public readonly NoteRange SystemRealtimeMessageRange	= new NoteRange(ChMessageU16.SystemRealtimeLo,ChMessageU16.SystemRealtimeHi, "System Exclusive Message");
		static public readonly NoteRange MidiEventRange						= new NoteRange(ChMessageU16.NoteOff,ChMessageU16.SystemMessageMax, "System Exclusive Message");
		
		static public string MessageName(ChMessageU16 message)
		{
			switch (message)
			{
				case ChMessageU16.NoteOff: return NoteOffRange.Name;
				case ChMessageU16.NoteOn: return NoteOnRange.Name;
				case ChMessageU16.ChannelAftertouch: return ChannelAftertouchRange.Name;
				case ChMessageU16.ControlChange: return ControlChangeRange.Name;
				case ChMessageU16.KeyAftertouch: return KeyAftertouchRange.Name;
				case ChMessageU16.PitchBend: return PitchBendRange.Name;
				case ChMessageU16.ProgramChange: return ProgramChangeRange.Name;
				case ChMessageU16.SystemMessage: return SystemExclusiveMessageRange.Name;
				default: return "unknown message";
			}
		}
		
		/// 0xF000
		static public bool IsSystemMessage(int msg) { return SystemExclusiveMessageRange.IsInRange(msg); }
		/// f1-f7
		static public bool IsSystemCommon(int msg) { return SystemCommonMessageRange.IsInRange(msg); }
		/// f8-ff
		static public bool IsSystemRealtime(int msg) { return SystemRealtimeMessageRange.IsInRange(msg); }
		/// 0x8000
		static public bool IsNoteOff(int msg) { return NoteOffRange.IsInRange(msg); }
		/// 0x9000
		static public bool IsNoteOn(int msg) { return NoteOnRange.IsInRange(msg); }
		/// 0xA000
		static public bool IsKeyAftertouch(int msg) { return KeyAftertouchRange.IsInRange(msg); }
		/// 0xB000
		static public bool IsControlChange(int msg) { return ControlChangeRange.IsInRange(msg); }
		/// 0xC000
		static public bool IsProgramChange(int msg) { return ProgramChangeRange.IsInRange(msg); }
		/// 0xD000
		static public bool IsChannelAftertouch(int msg) { return ChannelAftertouchRange.IsInRange(msg); }
		/// 0xE000
		static public bool IsPitchBend(int msg) { return PitchBendRange.IsInRange(msg); }
	
		/// True if the value is between 0x7FFF and 0x10000;
		static public bool IsMidiMessage(int value) { return ((value < SystemRealtimeMessageRange.Max) &&(value > NoteOffRange.Min)); }
		
		/// True if the value is between 0x7FFF and 0x10000;
		static public bool IsMidiBMessage(int value) { return ((value < SystemRealtimeMessageRange.MaxDown) && (value > NoteOffRange.MinDown)); }
		
	}
}
