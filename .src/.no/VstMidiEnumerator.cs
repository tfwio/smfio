/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using gen.snd.Midi.Common;
using gen.snd.Midi.Structures;
using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;
using System.Linq;

namespace gen.snd.Midi
{
  
	static public class VstMidiEnumeratorLite
	{
		readonly static MidiChannelComparer ChannelComparer = new MidiChannelComparer();

		class MidiChannelComparer : IEqualityComparer<MidiMessage>
		{
			public bool Equals(MidiMessage x, MidiMessage y)
			{
				return x.ChannelBit == y.ChannelBit;
			}

			public int GetHashCode(MidiMessage obj)
			{
				return base.GetHashCode();
			}
		}

		#region MIDI compliant
		static public IEnumerable<int> GetTrackIndex(IMidiParser parser)
		{
			for (int i = 0; i < parser.SmfFileHandle.NumberOfTracks; i++) {
				yield return i;
			}
		}

		static public IEnumerable<KeyValuePair<int, string>> GetMidiTrackNamesByIndex(IMidiParser parser)
		{
			foreach (int i in GetTrackIndex(parser)) {
				string trackname = string.Format(Strings.Filter_MidiTrack, i + 1);
				yield return new KeyValuePair<int, string>(i, trackname);
			}
		}

		static public IEnumerable<MidiMessage> MidiTrackDistinctChannels(int trackid, IMidiParser parser)
		{
			return parser.MidiDataList[trackid].Distinct(ChannelComparer);
		}

		#endregion
		#region MIDI compliant (hasmidiparser)
		// not used, just uncommented and left here.
		/// <summary>
		/// returns true if a parser is present
		/// </summary>
		/// <param name="ui"></param>
		/// <returns></returns>
		static bool HasMidiParser(IMidiParserUI_Lite ui)
		{
			if (ui.MidiParser != null && ui.MidiParser.MidiDataList.Count > 0 && ui.MidiParser.SmfFileHandle != null)
				return false;
			return true;
		}

		#endregion
		#region VST complient

		static VstEvent[] GetSampleOffsetBlock(IMidiParserUI_Lite ui, bool ignoreMidiPgm, int blockSize)
		{
			return VstEvent_Range(ui, ignoreMidiPgm, ui.VstContainer.VstPlayer.SampleOffset, blockSize).ToArray();
		}

		/// <summary>
		/// Process messages looking for Channel and Sysex messages.
		/// look at channel-message parsing for channel message types (or look into this).
		/// Sort the elements by timing.
		/// </summary>
		/// <param name="Parser">core</param>
		/// <param name="start">Begin in samples</param>
		/// <param name="len">Length from begin in samples</param>
		/// <returns>Filtered Events</returns>
		static public IEnumerable<VstEvent> VstEvent_Range(IMidiParserUI ui, bool ignoreMidiPgm, double start, int len)
		{
			List<VstEvent> list = new List<VstEvent>();
			{
				SampleClock c = new SampleClock(ui.VstContainer.VstPlayer.Settings);
				foreach (MidiMessage item in MidiMessage_Range(ui, new Loop() {
					Begin = start,
					Length = len
				})) {
					if (item.MessageBit == 0xC0 && ignoreMidiPgm)
						continue;
					if (item is MidiChannelMessage)
						list.Add(item.ToVstMidiEvent(Convert.ToInt32(ui.VstContainer.VstPlayer.SampleOffset), ui.VstContainer.VstPlayer.Settings, c));
					else
						if (item is MidiSysexMessage)
							list.Add(item.ToVstMidiSysex(Convert.ToInt32(ui.VstContainer.VstPlayer.SampleOffset), ui.VstContainer.VstPlayer.Settings, c));
				}
				c = null;
			}
			list.Sort(SortAlgo);
			foreach (VstEvent vstevent in list)
				yield return vstevent;
		}

		/// <summary>
		/// Retrieve all MIDI events from all tracks as found in the MIDI parser.
		/// </summary>
		/// <param name="Parser"></param>
		/// <param name="loop"></param>
		/// <returns></returns>
		static IEnumerable<MidiMessage> MidiMessage_Range(IMidiParserUI Parser, Loop loop)
		{
			SampleClock c = new SampleClock(Parser.VstContainer.VstPlayer.Settings);
			for (int trackId = 0; trackId < Parser.MidiParser.SmfFileHandle.NumberOfTracks; trackId++) {
				var elements = Parser.MidiParser.MidiDataList[trackId].Where(msg0 => msg0.IsContained(c, loop));
				foreach (MidiMessage item in elements)
					yield return item;
			}
			c = null;
		}

		static int SortAlgo(VstEvent a, VstEvent b)
		{
			return a.DeltaFrames.CompareTo(b.DeltaFrames);
		}

		#endregion
		#region Midi Enumerations
		/// <summary>
		/// Seems not to be used.
		/// </summary>
		/// <param name="ui"></param>
		/// <param name="loop"></param>
		/// <returns></returns>
		static VstEvent[] EnumerateMidiData(IMidiParserUI ui, Loop loop)
		{
			return FilterSampleRange(ui, loop.Begin, loop.Length.FloorMinimum(0).ToInt32());
		}

		/// <summary>
		/// Process messages looking for Channel and Sysex messages.
		/// look at channel-message parsing for channel message types (or look into this).
		/// </summary>
		/// <param name="ui">core</param>
		/// <param name="start">Begin in samples</param>
		/// <param name="len">Length from begin in samples</param>
		/// <returns>Filtered Events</returns>
		static VstEvent[] FilterSampleRange(IMidiParserUI ui, double start, int len)
		{
			if (HasParserErrors(ui))
				return null;
			List<VstEvent> list = new List<VstEvent>();
			{
				SampleClock c = new SampleClock(ui.VstContainer.VstPlayer.Settings);
				foreach (MidiMessage item in MidiMessage_Range(ui, new Loop() {
					Begin = start,
					Length = len
				})) {
					if (item is MidiChannelMessage)
						list.Add(item.ToVstMidiEvent(Convert.ToInt32(ui.VstContainer.VstPlayer.SampleOffset), ui.VstContainer.VstPlayer.Settings, c));
					else
						if (item is MidiSysexMessage)
							list.Add(item.ToVstMidiSysex(Convert.ToInt32(ui.VstContainer.VstPlayer.SampleOffset), ui.VstContainer.VstPlayer.Settings, c));
				}
				c = null;
			}
			list.Sort(SortAlgo);
			return list.ToArray();
		}

		static bool HasParserErrors(IMidiParserUI ui)
		{
			if (ui == null)
				return true;
			if (ui.MidiParser.MidiDataList.Count == 0)
				return true;
			return false;
		}
	#endregion
	}
}


