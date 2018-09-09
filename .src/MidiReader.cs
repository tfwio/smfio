/* Date: 11/12/2005 - Time: 4:19 PM */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using on.smfio.Common;
using on.smfio.chunk;
using on.smfio.util;
using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;

namespace on.smfio
{
	
	/// <summary>
	/// Internally, we load three text files from a subdirectory named ‘ext’.
	/// Controller change values, drum kit names and instrument names.
	/// </summary>
	public partial class MidiReader : IDisposable, IMidiParser
	{
		const int default_Fs = 44100;
		const int default_Tempo = 120;
		const int default_Division = 480;
		
		
		#region INotifyPropertyChanged
		
		public event PropertyChangedEventHandler PropertyChanged;
		void Notify(string property)
		{
			if (PropertyChanged!=null) PropertyChanged(this, new PropertyChangedEventArgs(property));
		}
		
		#endregion
		
		#region PROP
		/// <inheritdoc/>
		public int SelectedTrackChannel {
			get { return selectedTrackChannel; }
			
		} int selectedTrackChannel	= -1;
		/// <inheritdoc/>
		public DictionaryList<int,MidiMessage> MidiDataList {
			get { return midiDataList; }
			set { midiDataList = value; }
		} DictionaryList<int,MidiMessage> midiDataList = new DictionaryList<int,MidiMessage>();
		
		/// <inheritdoc/>
		public List<int> ChannelFilter {
			get { return channelFilter; }
		} public List<int> channelFilter = new List<int>();
		
		#endregion
		
		#region POSITION

		public int RunningStatus32 { get; set; } 

		public ulong TicksPerQuarterNote {
			get { return ticksPerQuarterNote; }
			set { ticksPerQuarterNote = value; }
		} internal ulong ticksPerQuarterNote = 0;
		#endregion

		#region TIME
		/// <inheritdoc/>
		public Stack<MidiMessage> TempoChanges {
			get { return tempoChanges; }
		} Stack<MidiMessage> tempoChanges = new Stack<MidiMessage>();
		
		/// <inheritdoc/>
		public MidiKeySignature KeySignature {
			get { return keySignature; }
			set { keySignature = value; }
		} MidiKeySignature keySignature = new MidiKeySignature();
		
		/// <inheritdoc/>
		public MidiTimeSignature TimeSignature {
			get { return timeSignature; }
			set { timeSignature = value; }
		} MidiTimeSignature timeSignature = new MidiTimeSignature();
		
		/// <inheritdoc/>
		public SampleClock MidiTimeInfo {
			get { return midiTimeInfo; }
			set { midiTimeInfo = value; }
		} SampleClock midiTimeInfo = new SampleClock(0, default_Fs, default_Tempo, default_Division);
		
		#endregion
		#region TIME (int) DivMeasure, DivBar, DivNote, FileDivision
		
		/// <summary>Bar x4</summary>
		public static int DivMeasure {
			get { return divMeasure; }
			set { divMeasure = value; }
		} static int divMeasure;
		
		/// <summary>Quarter x4</summary>
		public static int DivBar {
			get { return divBar; }
			set { divBar = value; }
		} static int divBar;
		
		/// <summary>PPQ (division) x4</summary>
		public static int DivQuarter {
			get { return divQuarter; }
			set { divQuarter = value; }
		} static int divQuarter;
		
		/// <summary>Midi header-&gt;Division.</summary>
		public static int FileDivision {
			get { return fileDivision; }
			set { fileDivision = value; }
		} static int fileDivision;

		public void GetDivision()
		{
			FileDivision = SmfFileHandle.Division;
			DivQuarter = FileDivision * 4;
			DivBar = DivQuarter * 4;
			DivMeasure = DivBar * 4;
		}

		#endregion
		
		public virtual void ResetTiming()
		{
			RunningStatus32 = -1;
			ticksPerQuarterNote = 0;
//			Notify("Timing");
		}
		
		#region FILE read, getmem
		
		/// <inheritdoc/>
		public void Read()
		{
			if (UseEventHandler) GetMemory();
			OnFileLoaded(EventArgs.Empty);
		}
		
		/// <inheritdoc/>
		public void GetMemory()
		{
			this.SmfFileHandle = MidiUtil.GetMthd(MidiFileName);
			tempoChanges.Clear();
			midiTimeInfo.Division = this.smfFileHandle.Division;
			
			Parse(0);
		}
		
		/// <param name="metaTrackId"></param>
		/// <seealso cref="ParseTrackMeta(int)"/>
		void Parse(int metaTrackId)
		{
			ParseTrackMeta(metaTrackId);
			
			MidiEventDelegate backup = this.MessageHandler;
			MessageHandler = PARSER_MidiDataList;
			ParseAll();
			this.MessageHandler = backup;
		}
		
		#endregion
		
		#region MESSAGE PARSER GetMsgTyp
		
		MidiMsgType GetMsgTyp(int status, MidiMsgType def = MidiMsgType.Channel)
		{
			return GetMsgTyp(Convert.ToByte(status & 0xf0), def);
		}
		MidiMsgType GetMsgTyp(byte status, MidiMsgType def = MidiMsgType.Channel)
		{
//			Debug.Print("? {0:X}",status);
			switch (status & 0xf0) {
				case 0x80:
					return MidiMsgType.NoteOff;
				case 0x90:
					return MidiMsgType.NoteOn;
				case 0xb0:
					return MidiMsgType.CC;
				case 0xF0:
				case 0xF7:
					return MidiMsgType.System;
				default:
//					case 12: return MsgType.CC;
					return def;
			}
		}
		
		#endregion
		#region MESSAGE PARSER GetTrackMessage, GetNTrackMessage, ?
		/// MESSAGE PARSER
		/// get track (selected) message
		public virtual int GetTrackMessage(int position, int delta)
		{
			return GetNTrackMessage(SelectedTrackNumber,position,delta);
			#region hidden
			#if no
			
			int DELTA_Returned = delta;
			byte CurrentByte = SmfFileHandle.Get8Bit(SelectedTrackNumber, position);
			int CurrentIntMessage = SmfFileHandle.Get16BitInt32(SelectedTrackNumber, position);
			switch (CurrentIntMessage) {
				case (int)MetaMsgU16FF.Text:
					// FF01
				case (int)MetaMsgU16FF.Copyright:
					// FF02
				case (int)MetaMsgU16FF.SequenceName:
					// FF03
				case (int)MetaMsgU16FF.InstrumentName:
					// FF04
				case (int)MetaMsgU16FF.Lyric:
					// FF05
				case (int)MetaMsgU16FF.Marker:
					// FF06
				case (int)MetaMsgU16FF.Cue:
					// FF07
				case (int)MetaMsgU16FF.Port:
					// FF08
					this.MessageHandler(MidiMsgType.MetaStr, SelectedTrackNumber, position, CurrentIntMessage, CurrentByte, TicksPerQuarterNote, RunningStatus32, false);
					DELTA_Returned = GetMetaNextPos(position);
					break;
				case (int)MetaMsgU16FF.SequenceNo:
					// FF00
				case (int)MetaMsgU16FF.Chanel:
					// FF20
				case (int)MetaMsgU16FF.EndOfTrack:
					// FF2F
				case (int)MetaMsgU16FF.Tempo:
					// FF51
				case (int)MetaMsgU16FF.SMPTE:
					// FF54
				case (int)MetaMsgU16FF.TimeSignature:
					// FF58
				case (int)MetaMsgU16FF.KeySignature:
					// FF59
					// why is this filtered and no others ? see getNtrackmessage
					if (HasTrackReaderDelegate) this.MessageHandler(MidiMsgType.MetaInf, SelectedTrackNumber, position, CurrentIntMessage, CurrentByte, TicksPerQuarterNote, RunningStatus32, false);
					DELTA_Returned = GetMetaNextPos(position);
					break;
				case (int)MetaMsgU16FF.SystemSpecific:
					// FF7F
				case (int)MetaMsgU16FF.SystemExclusive:
					// FF7F
					this.MessageHandler(MidiMsgType.System, SelectedTrackNumber, position, CurrentIntMessage, CurrentByte, TicksPerQuarterNote, RunningStatus32, false);
					DELTA_Returned = GetMetaNextPos(position);
					break;
				default:
					{
						if (CurrentByte < 0x80) {
							// Running Status
							int ExpandedRSE = RunningStatus32 << 8;
							int delta1 = -1;
							if ((delta1 = GetNextRsePosition(position)) == -1) {
								int test = GetOffset(position);
								Debug.Assert(false, string.Format("warning… {0:X2}, {1:X}|{1:N0}", ExpandedRSE, test));
							} else {
								DELTA_Returned = delta1;
								this.MessageHandler(GetMsgTyp(RunningStatus32), SelectedTrackNumber, position, RunningStatus32, (byte)RunningStatus32, TicksPerQuarterNote, RunningStatus32, true);
							}
						} else if (MidiMessageInfo.IsMidiMessage(CurrentIntMessage)) {
							RunningStatus32 = (SmfFileHandle[SelectedTrackNumber, position]);
							DELTA_Returned = GetNextPosition(position);
							this.MessageHandler(GetMsgTyp(RunningStatus32), SelectedTrackNumber, position, RunningStatus32, (byte)RunningStatus32, TicksPerQuarterNote, RunningStatus32, false);
							DELTA_Returned++;
							return DELTA_Returned;
						} else
							throw new FormatException("Bad format!\nThere is probably a problem with the Input File unless we made an error reading it!)");
					}

					break;
			}
			DELTA_Returned++;
			// what is roy[1] if not the current track position?
			return DELTA_Returned;
			#endif
			#endregion
		}
		/// MESSAGE PARSER
		/// get track (specific) message
		public virtual int GetNTrackMessage(int ntrack, int position, int delta)
		{
			int DELTA_Returned = delta;
			byte CurrentByte = SmfFileHandle.Get8Bit(ntrack, position);
			int CurrentIntMessage = SmfFileHandle.Get16BitInt32(ntrack, position);
			switch (CurrentIntMessage) {
				case (int)MetaMsgU16FF.Text:
					// FF01
				case (int)MetaMsgU16FF.Copyright:
					// FF02
				case (int)MetaMsgU16FF.SequenceName:
					// FF03
				case (int)MetaMsgU16FF.InstrumentName:
					// FF04
				case (int)MetaMsgU16FF.Lyric:
					// FF05
				case (int)MetaMsgU16FF.Marker:
					// FF06
				case (int)MetaMsgU16FF.Cue:
					// FF07
				case (int)MetaMsgU16FF.Port:
					// FF08
					this.MessageHandler(MidiMsgType.MetaStr, ntrack, position, CurrentIntMessage, CurrentByte, TicksPerQuarterNote, RunningStatus32, false);
					//lve.AddItem( c4, MeasureBarTick( TicksPerQuarterNote ), TicksPerQuarterNote.ToString(), ""/*(RunningStatus32 & 0x0F)+1*/, MetaHelpers.MetaNameFF( CurrentIntMessage ) , GetMetaString( position ) );
					DELTA_Returned = GetMetaNextPos(position);
					break;
				case (int)MetaMsgU16FF.SequenceNo:
					// FF00
				case (int)MetaMsgU16FF.Chanel:
					// FF20
				case (int)MetaMsgU16FF.EndOfTrack:
					// FF2F
				case (int)MetaMsgU16FF.Tempo:
					// FF51
				case (int)MetaMsgU16FF.SMPTE:
					// FF54
				case (int)MetaMsgU16FF.TimeSignature:
					// FF58
				case (int)MetaMsgU16FF.KeySignature:
					// FF59
					// why is this filtered and no others ? see gettrackmessage
					this.MessageHandler(MidiMsgType.MetaInf, ntrack, position, CurrentIntMessage, CurrentByte, TicksPerQuarterNote, RunningStatus32, false);
					DELTA_Returned = GetMetaNextPos(position);
					break;
				case (int)MetaMsgU16FF.SystemSpecific:
					// FF7F
				case (int)MetaMsgU16FF.SystemExclusive:
					// FF7F
					Debug.Print("?---------sys");
					this.MessageHandler(MidiMsgType.System, ntrack, position, CurrentIntMessage, CurrentByte, TicksPerQuarterNote, RunningStatus32, false);
					DELTA_Returned = GetMetaNextPos(position);
					break;
				default:
					{
						if (CurrentByte < 0x80) {
							// Running Status
							int ExpandedRSE = RunningStatus32 << 8;
							int delta1 = -1;
							if ((delta1 = GetNextRsePosition(position)) == -1) {
								int test = GetOffset(position);
								Debug.Assert(false, string.Format("warning… {0:X2}, {1:X}|{1:N0}", ExpandedRSE, test));
							} else {
								DELTA_Returned = delta1;
								this.MessageHandler(GetMsgTyp(RunningStatus32), ntrack, position, RunningStatus32, (byte)RunningStatus32, TicksPerQuarterNote, RunningStatus32, true);
							}
						} else if (MidiMessageInfo.IsMidiMessage(CurrentIntMessage)) {
							RunningStatus32 = (SmfFileHandle[ntrack, position]);
							DELTA_Returned = GetNextPosition(position);
//							Debug.Print("{0:X}",runningStatus32);
							this.MessageHandler(GetMsgTyp(RunningStatus32), ntrack, position, RunningStatus32, (byte)RunningStatus32, TicksPerQuarterNote, RunningStatus32, false);
							DELTA_Returned++;
							return DELTA_Returned;
						} else
							throw new FormatException("Bad format!\nThere is probably a problem with the Input File unless we made an error reading it!)");
					}

					break;
			}
			DELTA_Returned++;
			// what is roy[1] if not the current track position?
			return DELTA_Returned;
		}
		/// MESSAGE PARSER see 'ParseTrackMeta'
		public virtual int GetTrackTiming(int ntrack, int position, int delta)
		{
			int	DELTA_Returned		= delta;
			byte CurrentByte			 = SmfFileHandle.Get8Bit(ntrack, position);
			int	CurrentIntMessage = SmfFileHandle.Get16BitInt32(ntrack, position);
			
			switch (CurrentIntMessage) {
				case (int)MetaMsgU16FF.Text:
				case (int)MetaMsgU16FF.Copyright:
				case (int)MetaMsgU16FF.SequenceName:
				case (int)MetaMsgU16FF.InstrumentName:
				case (int)MetaMsgU16FF.Lyric:
				case (int)MetaMsgU16FF.Marker:
				case (int)MetaMsgU16FF.Cue:
				case (int)MetaMsgU16FF.Port:
				case (int)MetaMsgU16FF.SequenceNo:
				case (int)MetaMsgU16FF.Chanel:
				case (int)MetaMsgU16FF.EndOfTrack:
				case (int)MetaMsgU16FF.SMPTE:
					DELTA_Returned = GetMetaNextPos(position);
					break;
				case (int)MetaMsgU16FF.Tempo:
					MidiTimeInfo.Tempo = 60000000 / Convert.ToInt32(SmfFileHandle[ntrack].Get24Bit(position + 3));
					DELTA_Returned = GetMetaNextPos(position);
					break;
				case (int)MetaMsgU16FF.TimeSignature:
					TimeSignature.SetSignature(
						(int)this[ntrack,position+3],
						(int)Math.Pow(-this[ntrack,position+4],2),
						(int)this[ntrack,position+5],
						(int)this[ntrack,position+6]
					);
					DELTA_Returned = GetMetaNextPos(position);
					break;
				case (int)MetaMsgU16FF.KeySignature:
					byte b = (this[ntrack,position+3]);
					KeySignature.SetSignature((KeySignatureType)b,this[ntrack,position+4]==0);
					DELTA_Returned = GetMetaNextPos(position);
					break;
				case (int)MetaMsgU16FF.SystemSpecific:
				case (int)MetaMsgU16FF.SystemExclusive:
					DELTA_Returned = GetMetaNextPos(position);
					break;
				default:
					{
						if (CurrentByte < 0x80) {
							// Running Status
							int ExpandedRSE = RunningStatus32 << 8;
							int delta1 = -1;
							if ((delta1 = GetNextRsePosition(position)) == -1) {
								int test = GetOffset(position);
								Debug.Assert(false, string.Format("warning… {0:X2}, {1:X}|{1:N0}", ExpandedRSE, test));
							} else {
								DELTA_Returned = delta1;
							}
						} else if (MidiMessageInfo.IsMidiMessage(CurrentIntMessage)) {
							RunningStatus32 = (SmfFileHandle[ntrack, position]);
							DELTA_Returned = GetNextPosition(position);
							DELTA_Returned++;
							return DELTA_Returned;
						} else
							throw new FormatException("Bad format!\nThere is probably a problem with the Input File unless we made an error reading it!)");
					}

					break;
			}
			DELTA_Returned++;
			return DELTA_Returned;
		}
		#endregion
		#region MESSAGE methods
		
		/// MESSAGE methods
		void DispatchHandlers(MidiMsgType t, int track, int offset, int imsg, byte bmsg, ulong ppq, int rse, bool isrse)
		{
			foreach (MidiEventDelegate method in MessageHandlers) method(t, track, offset, imsg, bmsg, ppq, rse, isrse);
		}
		
		/// MESSAGE methods
		/// <inheritdoc/>
		public void OnMidiMessage(MidiMsgType t, int track, int offset, int imsg, byte bmsg, ulong ppq, int rse, bool isrse)
		{
			if (ProcessMidiMessage != null) ProcessMidiMessage(this, new MidiMessageEvent(t, track, offset, imsg, bmsg, ppq, rse, isrse));
			if (MessageHandlers.Count>0) DispatchHandlers(t, track, offset, imsg, bmsg, ppq, rse, isrse);
		}
		
		#endregion
		#region MESSAGE properties/events
		
		/// MESSAGE properties/events
		public MidiReaderLoadTrackDelegate LoadTrack {
			get { return loadTrack; } set { loadTrack = value; }
		} MidiReaderLoadTrackDelegate loadTrack = null;
		
		/// MESSAGE properties/events
		public List<MidiEventDelegate> MessageHandlers {
			get { return messageHandlers; }
		} List<MidiEventDelegate> messageHandlers = new List<MidiEventDelegate>();
		
		/// MESSAGE properties/events
		public MidiEventDelegate MessageHandler = null;
		
		/// MESSAGE properties/events
		public event EventHandler<MidiMessageEvent> ProcessMidiMessage;
		#endregion
		
		#region Boolean Handler-Flags
		
		/// <inheritdoc/>
		public bool UseEventHandler {
			get { return propogateEvents; }
		} readonly bool propogateEvents;
		
		/// <inheritdoc/>
		public bool HasTrackReaderDelegate {
			get { return MessageHandler != null; }
		}

		#endregion
		
		static MidiReader()
		{
			try {
				SmfStringFormatter.cc = MidiUtil.LoadEnumerationFile("ext/cc.map");
				SmfStringFormatter.patches = MidiUtil.LoadEnumerationFile("ext/inst.map");
				SmfStringFormatter.drums = MidiUtil.LoadEnumerationFile("ext/dk.map");
			} catch {
			}
		}
		
		#region .ctor
		
		public MidiReader() : this(true)
		{
			this.LoadTrack = this.GetTrackMessage;
		}
		public MidiReader(bool useEventHandler)
		{
			if (this.propogateEvents = useEventHandler)
				this.MessageHandler = this.OnMidiMessage;
		}
		
		public MidiReader(MidiMessageHandler handler) : this(true) {}
		public MidiReader(MidiEventDelegate handler) : this(false) { this.MessageHandler = handler; }
		public MidiReader( string fileName ) : this(true)
		{
			this.MidiFileName = fileName;
		}

		#endregion
		
		#region ¿¡WHAT?!
		///	(¿¡WHAT?!) this is only called on errors
		/// <inheritdoc/>
		public int GetOffset(int offset)
		{
			#if no
			#endif
			int result = 14;
			for (int i = 0; i <= SelectedTrackNumber; i++) {
				result += SmfFileHandle[i].track.Length + 8; // if not 8, this will be 4
			}
			return result + offset;
		}
		#endregion

		// ------------------------------
		
		#region Cleanup, Dispose

		public virtual void Dispose()
		{
			this.ClearAll();
		}

		void ClearAll()
		{
			ResetTiming();
			smfFileHandle = default(smf_mthd);
			midiFileName = null;
			selectedTrackNumber = -1;
			GC.Collect();
		}

		#endregion

		readonly object ParseAllLock = new object();
		
		#region READ VAR INT
		
		int GetIntVar(int offset, out long result)
		{
			return GetIntVar(SelectedTrackNumber,offset,out result);
		}
		
		/// <summary>
		/// Use offset - result to get your length.
		/// </summary>
		/// <param name="ntrack"></param>
		/// <param name="offset"></param>
		/// <param name="result"></param>
		/// <returns>the int position after reading data.</returns>
		int GetIntVar(int ntrack, int offset, out long result)
		{
			byte tempBit;
			int i = offset;
			if ((result = Convert.ToUInt32(SmfFileHandle[ntrack, i++])) > 0x7f) {
				result &= 0x7f;
				do {
					result = (result << 7) + ((tempBit = SmfFileHandle[ntrack, i++]) & 0x7f);
				} while (tempBit > 0x7f);
			}
			return i;
		}
		
		#endregion
		#region READ META TRACK
		
		/// <inheritdoc/>
		public void ParseTrackMeta(int tk)
		{
			long delta;
			int i = 0;
			selectedTrackNumber = tk;
			while (i < SmfFileHandle.Tracks[selectedTrackNumber].track.Length)
			{
				i = GetIntVar(i, out delta);
				TicksPerQuarterNote += Convert.ToUInt64(delta);
				i = GetTrackTiming(selectedTrackNumber,i, Convert.ToInt32(delta));
			}
		}
		
		#endregion
		#region READ 1	 TRACK
		
		/// <summary>
		/// Parse the selected track
		/// </summary>
		/// <returns>The length (in bytes) of the track.</returns>
		public virtual long ParseTrack()
		{
			OnBeforeTrackLoaded(EventArgs.Empty);
			
			long delta;
			int i = 0;
			
			while (i < SmfFileHandle.Tracks[SelectedTrackNumber].track.Length)
			{
				i = GetIntVar(i, out delta);
				TicksPerQuarterNote += Convert.ToUInt64(delta);
				i = GetTrackMessage(i, Convert.ToInt32(delta));
				OnTrackLoadProgressChanged(i);
			}
			
			OnAfterTrackLoaded(EventArgs.Empty);
			
			return SmfFileHandle.Tracks[SelectedTrackNumber].track.Length;
		}
		#endregion
		#region READ (PARSE) ALL TRACKS

		// when IsTrackSelected, the total number of ticks in the track.
		long totlen = 0;

		/// <summary>
		/// Parse all tracks to mididatalist
		/// </summary>
		/// <returns>The length (in bytes) of the track.</returns>
		public void ParseAll()
		{
			MidiDataList.Clear();
			for (int i = 0; i < SmfFileHandle.NumberOfTracks; i++) MidiDataList.CreateKey(i);
			lock (ParseAllLock)
			{
				for (int TrackToParse = 0; TrackToParse < this.SmfFileHandle.NumberOfTracks; TrackToParse++)
				{
					ResetTiming();
					this.selectedTrackNumber = TrackToParse;
					lock (this)
					{
						long delta = 0;
						int i = 0;
						while (i < SmfFileHandle.Tracks[TrackToParse].track.Length)
						{
							i = GetIntVar(TrackToParse, i, out delta);
							TicksPerQuarterNote += Convert.ToUInt64(delta);
							i = GetNTrackMessage(TrackToParse, i, Convert.ToInt32(delta));
						}
					}
				}
			}
//			OnAfterTrackLoaded(EventArgs.Empty);
//			return SmfFileHandle.Tracks[SelectedTrackNumber].track.Length;
		}

		#region PARSER_MidiDataList (DEFAULT PARSER!)

		void PARSER_MidiDataList(MidiMsgType t, int track, int offset, int imsg, byte bmsg, ulong ppq, int rse, bool isrse)
		{
			switch (t)
			{
				case MidiMsgType.MetaStr:
					midiDataList.AddV(SelectedTrackNumber,new MidiMetaMessage(MidiMsgType.MetaStr,ppq,imsg,GetMetaBString(offset)));
					break;
				case MidiMsgType.MetaInf:
					midiDataList.AddV(SelectedTrackNumber,new MidiMetaMessage(ppq,imsg,GetMetaData(offset)));
					break;
				case MidiMsgType.System:
				case MidiMsgType.SysCommon:
//					MetaHelpers.MetaNameFF( imsg )
					if (imsg==0xFF7F)
//					{
//						Debug.Print("rse? {0} -----------sys---------------\n{1}",isrse ? 0 :1, SmfStringFormatter.byteToString(GetEventValue(offset)));
						midiDataList.AddV(SelectedTrackNumber,new MidiSysexMessage(ppq,imsg,GetMetaValue(offset)));
//					}
					else if (imsg==0xF0)
					{
						Debug.Print("0xF0 {0} {1}",isrse, GetEventValue(offset).StringifyHex());
						midiDataList.AddV(SelectedTrackNumber,new MidiSysexMessage(ppq,imsg,GetEventValue(offset)));
					}
					break;
				default:
					if (isrse) MidiDataList.AddV(SelectedTrackNumber,new MidiChannelMessage(ppq,rse,GetRseEventValue(offset)));
					else MidiDataList.AddV(SelectedTrackNumber,new MidiChannelMessage(ppq,rse,GetEventValue(offset)));
					break;
			}
		}
		#endregion
		
		#endregion

		#region TRACK
		/// <summary>
		/// Get/Set Selected Track Number;
		/// Setting the track number triggers the TrackChanged event!
		/// </summary>
		public int SelectedTrackNumber {
			get { return selectedTrackNumber; }
			set {
				selectedTrackNumber = value;
				OnTrackChanged(EventArgs.Empty);
			}
		} internal int selectedTrackNumber;
		
		public bool IsTrackSelected {
			get { return SelectedTrackNumber >= 0; }
		}
		int SelectedTrackTickCount = -1;
		
		///<summary>a track is selected</summary>
		public virtual string TrackSelectAction()
		{
			GetDivision();
			ResetTiming();
			this.Notes.Clear();
			if (SelectedTrackNumber >= 0)
			{
				totlen = ParseTrack();
				return StringTrackInfo;
			} else
				return "InterMIDI";
		}

		#endregion
		
		#region FILE

		public string MidiFileName {
			get { return midiFileName; }
			set { midiFileName = value; }
		} string midiFileName = string.Empty;

		public smf_mthd SmfFileHandle {
			get { return smfFileHandle; }
			set { smfFileHandle = value; }
		} internal smf_mthd smfFileHandle = null;


		#endregion
		
		// -----------------------------------------------

		#region EVENT clearview,fileloaded,trackchanged,beforetrackload,aftertrackload
		public event CliHandler ClearView;
		protected virtual void OnClearView(CliEvent e)
		{
			if (ClearView != null) {
				ClearView(this, e);
			}
		}

		public event CliHandler FileLoaded;
		protected virtual void OnFileLoaded(CliEvent e)
		{
			if (FileLoaded != null) {
				FileLoaded(this, e);
			}
		}

		/// <summary>
		/// This is triggered after the track is parsed.
		/// </summary>
		public event CliHandler BeforeTrackLoaded;
		protected virtual void OnBeforeTrackLoaded(CliEvent e)
		{
			if (BeforeTrackLoaded != null) {
				BeforeTrackLoaded(this, e);
			}
		}
		/// <summary>
		/// This is triggered after the track is parsed.
		/// </summary>
		public event CliHandler AfterTrackLoaded;
		protected virtual void OnAfterTrackLoaded(CliEvent e)
		{
			if (AfterTrackLoaded != null) {
				AfterTrackLoaded(this, e);
			}
		}

		/// <summary>
		/// This is triggered after a track number is selected,
		/// and before the track is parsed.
		/// See <see cref="SelectedTrackNumber" />.
		/// </summary>
		public event CliHandler TrackChanged;
		protected virtual void OnTrackChanged(CliEvent e)
		{
			if (TrackChanged != null) {
				TrackChanged(this, e);
			}
		}
		#endregion
		
		// -----------------------------------------------
		
		#region STR track info, reader status (caption/tooltip)
		string StringTrackInfo
		{
			get {
				return string.Format(
					Resource_TrackLoaded,
					SelectedTrackNumber,
					totlen,
					SmfFileHandle[SelectedTrackNumber].Size,
					Convert.ToInt32(SmfFileHandle.Division),
					Convert.ToSingle(MidiTimeInfo.Tempo),
					TimeSignature.Numerator,
					TimeSignature.Denominator,
					TimeSignature.Clocks,
					TimeSignature.ThirtySeconds,
					KeySignature.KeyType,
					KeySignature.IsMajor ? "Major" : "Minor"
				);
			}
		}

		internal string DefaultMidiString {
			get { return string.Format("{0} No Tracks Selected", class_id); }
		} readonly string class_id = "midi reader";

		public override string ToString()
		{
			return IsTrackSelected ? string.Format(Resource_TrackLoaded, SelectedTrackNumber, SelectedTrackTickCount, SmfFileHandle[SelectedTrackNumber].Size, Convert.ToInt32(SmfFileHandle.Division)) : DefaultMidiString;
		}

		#endregion

		// obsolete (not replaced, just absolute junk)
		#region Failed Progress Attempt
		
		public event EventHandler<ProgressChangedEventArgs> TrackLoadProgressChanged;
		
		protected virtual void OnTrackLoadProgressChanged(int i) {
			int progressAmount = (i / SmfFileHandle[selectedTrackNumber].track.Length) * 100;
			ProgressChangedEventArgs e = new ProgressChangedEventArgs(i,null);
			if (TrackLoadProgressChanged != null) TrackLoadProgressChanged(this, e);
		}
		
		#endregion
	}
}
