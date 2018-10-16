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
	public interface IReader:
		IReaderParser,
		INotifyPropertyChanged
  {
    MTrk this[int kTrackID] { get; }
    byte this[int kTrackID, int kTrackOffset] { get; }
    byte[] this[int kTrackID, int kTrackOffset, int kSize] { get; }

    // =============================================
    // DATA (DictionaryList<int,MidiMessage>)
    // =============================================
		
		DictionaryList<int,MIDIMessage> MidiDataList { get; }

    // =============================================
    // DATA (Back-Reference)
    // =============================================

    List<long> TrackLength { get; }
    /// <summary>
    /// Contains TempoStates to help calculate event times.
    /// </summary>
    TempoMap TempoMap { get; }

    MidiKeySignature KeySignature { get; set; }

    MidiTimeSignature TimeSignature { get; set; }
    SmpteOffset SMPTE_Offset { get; set; }

    // =============================================
    // TIMING
    // =============================================

    short Division { get; }
    long CurrentTrackPulse { get; }
		
		int CurrentRunningStatus8 { get; }
		
		void ResetTiming();
		
		void GetDivision();

    // =============================================
    // MIDIEVENT MESSAGE (HANDLERS)
    // =============================================

    /// <param name="msgType"></param>
    /// <param name="nTrackIndex"></param>
    /// <param name="nTrackOffset"></param>
    /// <param name="midiMsg32"></param>
    /// <param name="midiMsg8"></param>
    /// <param name="pulse"></param>
    /// <param name="delta"></param>
    /// <param name="isRunningStatus"></param>
    void OnMidiMessage(
			MidiMsgType msgType,
			int nTrackIndex,
			int nTrackOffset,
			int midiMsg32,
			byte midiMsg8,
			long pulse,
			int delta,
			bool isRunningStatus);

    bool UseEventHandler { get; }
		
		/// <summary>
		/// this is a test currently in use;
		/// A list of delegates contains a enumerable number of parsers,
		/// perhaps including the main parsing method.
		/// </summary>
		/// <summary>
		/// A set of delegates
		/// </summary>
		List<MidiEventDelegate> MessageHandlers { get; }

    // =============================================
    // PARSE TRACK
    // =============================================

    int ReaderIndex { get; set; }
		
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
		void ParseTempoMap(int trackNo);

    int GetTrackMessage(int position, int delta);

    // =============================================
    // PARSE TRACK (EVENT)
    // =============================================

    event EventHandler<TempoChangedEventArgs> TempoChangedEvent;
		event EventHandler<MidiMessageEvent> ProcessMidiMessage;
		event EventHandler<ProgressChangedEventArgs> TrackLoadProgressChanged;
		event EventHandler AfterTrackLoaded;
		event EventHandler BeforeTrackLoaded;
		event EventHandler TrackChanged;
		
		#region FILE
		
		MThd FileHandle { get; set; }
		
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

	}
	
}
