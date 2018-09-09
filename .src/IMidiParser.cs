/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;

using on.smfio.chunk;
using on.smfio.util;

namespace on.smfio
{
	#region DELEGATES
	public delegate void HandleMidiMessage();
	public delegate void MidiMessageHandler(object sender, MidiMessageEvent e);
	public delegate int MidiReaderLoadTrackDelegate(int track, int offset);
	public delegate void MidiEventDelegate(MidiMsgType t, int track, int offset, int imsg, byte bmsg, ulong ppq, int rse, bool isrse);
	#endregion
	
	public interface IMidiParser:
		IMidiParser_Parser /* normative */,
		IMidiParser_Resources /* ui */,
		IMidiParser_Notes /* optional */,
		INotifyPropertyChanged
	{
		#region INotifyPropertyChanged

//		event PropertyChangedEventHandler PropertyChanged;
		
		#endregion
		
		#region CH
		
		/// <summary>
		/// if not -1, the Midi Track @SelectedTrackNumber will contain data primary to a view.
		/// </summary>
		int SelectedTrackChannel { get; }
		
		/// <summary>
		/// A set of midi data channels (not track-number) used for filtering data for ui.
		/// </summary>
		List<int> ChannelFilter { get; }
		
		#endregion
		
		#region DATA (DictionaryList<int,MidiMessage>)
		
		DictionaryList<int,MidiMessage> MidiDataList { get; }
		
		#endregion
		
		#region DATA standard global
		
		List<TempoChange> TempoChanges { get; }
		
		MidiKeySignature KeySignature { get;set; }
		
		MidiTimeSignature TimeSignature { get;set; }
		
		#endregion
		
		#region TIME
		
		ulong TicksPerQuarterNote { get; set; }
		
		int RunningStatus32 { get; set; }
		
		SampleClock MidiTimeInfo { get;set; }
		
		void ResetTiming();
		
		void GetDivision();
		
		#endregion
		
		void OnMidiMessage(MidiMsgType t, int track, int offset, int imsg, byte bmsg, ulong ppq, int rse, bool isrse);

		#region PARSE (handlers)
		
		bool UseEventHandler { get; }
		
		bool HasTrackReaderDelegate { get; }

		/// <summary>
		/// this is a test currently in use;
		/// A list of delegates contains a enumerable number of parsers,
		/// perhaps including the main parsing method.
		/// </summary>
		/// <summary>
		/// A set of delegates
		/// </summary>
		List<MidiEventDelegate> MessageHandlers { get; }
		
		#endregion
		
		#region PARSE TRACK
		
		int SelectedTrackNumber { get; set; }
		
		bool IsTrackSelected { get; }
		
		MidiReaderLoadTrackDelegate LoadTrack { get; set; }
		
		/// <summary>
		/// Specifically targeting Meta information helpful for a main-parse
		/// algorithm.
		/// </summary>
		string TrackSelectAction();
		
		long ParseTrack();
		
		/// <summary>
		/// Get default META-data information such as ITimeConfiguration.
		/// 
		/// This is likely a simple pre-scan for meta information such
		/// scanning for tempo-changes, time-signatures and so forth.
		/// </summary>
		/// <param name="trackNo"></param>
		void ParseTrackMeta(int trackNo);
		
		int GetTrackMessage(int position, int delta);
		
		#endregion
		
		#region PARSE TRACK event
		
    event EventHandler<TempoChangedEventArgs> TempoChangedEvent;
		
		event EventHandler<MidiMessageEvent> ProcessMidiMessage;
		
		event EventHandler<ProgressChangedEventArgs> TrackLoadProgressChanged;
		
		event EventHandler AfterTrackLoaded;
		
		event EventHandler BeforeTrackLoaded;
		
		event EventHandler TrackChanged;
		
		#endregion
		
		#region FILE
		
		smf_mthd SmfFileHandle { get; set; }
		
		string MidiFileName { get; set; }
		
		#endregion
		
		#region FILE event
		
		/// <summary>
		/// FILE event Loaded
		/// </summary>
		event EventHandler FileLoaded;
		
		/// <summary>
		/// FILE event Unloaded
		/// </summary>
		event EventHandler ClearView;
		
		#endregion
		
		void Dispose();
		
		string ToString();

		#region NO
		#if no
		/// <summary>
		/// This is just a test.
		/// I Never implemented and varified a File-Position (offset)
		/// calculation from within a track.  This is a minor attempt.
		/// </summary>
		/// <param name="offset"></param>
		/// <returns></returns>
		int GetOffset(int offset);
		#endif
		#endregion
	}
	
}
