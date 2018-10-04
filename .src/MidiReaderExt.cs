﻿/*
 * User: xo
 * Date: 9/8/2018
 * Time: 3:38 AM
 */
using System;
using System.Collections.Generic;
using System.Linq;
using on.smfio;
namespace SMFIOViewer
{
  static class MidiReaderExt
	{
    static IEnumerable<int> EnumerateTrackIndex(this IMidiParser parser)
		{
			for (int i = 0; i < parser.FileHandle.NumberOfTracks; i++)
				yield return i;
		}

		/// <summary>
		/// This just gathers simple "MTrk {id+1}" string keys into an index.
		/// </summary>
		/// <param name="parser">the MIDI parser</param>
		/// <returns></returns>
		static public IEnumerable<KeyValuePair<int, string>> GetMidiTrackNameDictionary(this IMidiParser parser)
		{
			foreach (int i in parser.EnumerateTrackIndex()) {
				string trackname = string.Format(Strings.Filter_MidiTrack, i + 1);
				yield return new KeyValuePair<int, string>(i, trackname);
			}
		}

		/// <summary>
		/// Uses Linq to find/filter all channel-specific events from parser.MidiDataList.
		/// </summary>
		/// <param name="parser"></param>
		/// <param name="trackid"></param>
		/// <returns></returns>
		static public IEnumerable<MIDIMessage> MidiTrackDistinctChannels(this IMidiParser parser, int trackid)
		{
      return parser.MidiDataList[trackid].Distinct(ChannelComparer);
		}
		
    class MidiChannelComparer : IEqualityComparer<MIDIMessage>
    {
      public bool Equals(MIDIMessage x, MIDIMessage y) { return x.ChannelBit == y.ChannelBit; }
      public int GetHashCode(MIDIMessage obj) { return base.GetHashCode(); }
    } readonly static MidiChannelComparer ChannelComparer = new MidiChannelComparer();

	}
}




