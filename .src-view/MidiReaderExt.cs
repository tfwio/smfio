/*
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
			for (int i = 0; i < parser.SmfFileHandle.NumberOfTracks; i++)
				yield return i;
		}

		static public IEnumerable<KeyValuePair<int, string>> GetMidiTrackNameDictionary(this IMidiParser parser)
		{
			foreach (int i in parser.EnumerateTrackIndex()) {
				string trackname = string.Format(Strings.Filter_MidiTrack, i + 1);
				yield return new KeyValuePair<int, string>(i, trackname);
			}
		}

		static public IEnumerable<MidiMessage> MidiTrackDistinctChannels(this IMidiParser parser, int trackid)
		{
      return parser.MidiDataList[trackid].Distinct(ChannelComparer);
		}
		
    class MidiChannelComparer : IEqualityComparer<MidiMessage>
    {
      public bool Equals(MidiMessage x, MidiMessage y) { return x.ChannelBit == y.ChannelBit; }
      public int GetHashCode(MidiMessage obj) { return base.GetHashCode(); }
    } readonly static MidiChannelComparer ChannelComparer = new MidiChannelComparer();

	}
}




