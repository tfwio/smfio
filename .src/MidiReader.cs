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
    byte this[int track, int index] {
      get { return SmfFileHandle[track, index]; }
    }
    const int default_Fs = 44100;
    const int default_Tempo = 120;
    const int default_Division = 480;
    
    
    
    #region INotifyPropertyChanged (isn't used)
    
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
    public DictionaryList<int,MIDIMessage> MidiDataList {
      get { return midiDataList; }
      set { midiDataList = value; }
    } DictionaryList<int,MIDIMessage> midiDataList = new DictionaryList<int,MIDIMessage>();
    
    /// <inheritdoc/>
    public List<int> ChannelFilter {
      get { return channelFilter; }
    } public List<int> channelFilter = new List<int>();
    
    #endregion
    
    public int CurrentTrackRunningStatus { get; private set; }

    /// <summary>
    /// Current Track's pulse ocunt in ticks per quarter-note.
    /// </summary>
    public long CurrentTrackPulse { get; private set; }

    #region TIME
    /// <inheritdoc/>
    public TempoMap TempoMap {
      get { return tempoMap; }
    } TempoMap tempoMap = new TempoMap();

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
    
    #endregion
    #region TIME (int) DivMeasure, DivBar, DivNote, FileDivision
    
    /// <summary>Bar x4</summary>
    public static int DivMeasure { get; private set; }
    
    /// <summary>Quarter x4</summary>
    public int DivBar { get; private set; }
    
    /// <summary>PPQ (division) x4</summary>
    public int DivQuarter { get; private set; }
    
    /// <summary>Midi header-&gt;Division.</summary>
    public short Division { get { return SmfFileHandle.Division; } }

    public void GetDivision()
    {
      DivQuarter = Division * 4;
      DivBar = DivQuarter * 4;
      DivMeasure = DivBar * 4;
    }

    #endregion
    
    public virtual void ResetTiming()
    {
      CurrentTrackRunningStatus = -1;
      CurrentTrackPulse = 0;
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
      SmfFileHandle = MidiUtil.GetMthd(MidiFileName);
      TempoMap.Clear();
      ParseTrackMeta(0); // pre-scan? ;)
      Parse();
      // Log.ErrorMessage("Parsed the default track.");
    }
    
    /// <seealso cref="ParseTrackMeta(int)"/>
    void Parse()
    {
      MidiEventDelegate backup = MessageHandler;
      MessageHandler = PARSER_MidiDataList;
      ParseAll();
      TempoMap.Sync(this);
      MessageHandler = backup;
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
    }
    /// MESSAGE PARSER
    /// get track (specific) message
    public virtual int GetNTrackMessage(int nTrackIndex, int nTrackOffset, int delta)
    {
      int DELTA_Returned = delta;
      byte CurrentByte = SmfFileHandle.Get8Bit(nTrackIndex, nTrackOffset);
      int CurrentIntMessage = SmfFileHandle.Get16BitInt32(nTrackIndex, nTrackOffset);
      switch (CurrentIntMessage) {
        case (int)MetaMsgU16FF.Text:            // FF01
        case (int)MetaMsgU16FF.Copyright:       // FF02
        case (int)MetaMsgU16FF.SequenceName:    // FF03
        case (int)MetaMsgU16FF.InstrumentName:  // FF04
        case (int)MetaMsgU16FF.Lyric:           // FF05
        case (int)MetaMsgU16FF.Marker:          // FF06
        case (int)MetaMsgU16FF.Cue:             // FF07
        case (int)MetaMsgU16FF.Port:            // FF08
          MessageHandler(MidiMsgType.MetaStr, nTrackIndex, nTrackOffset, CurrentIntMessage, CurrentByte, CurrentTrackPulse, CurrentTrackRunningStatus, false);
          //lve.AddItem( c4, MeasureBarTick( TicksPerQuarterNote ), TicksPerQuarterNote.ToString(), ""/*(RunningStatus32 & 0x0F)+1*/, MetaHelpers.MetaNameFF( CurrentIntMessage ) , GetMetaString( position ) );
          DELTA_Returned = GetMetaNextPos(nTrackOffset);
          break;
        case (int)MetaMsgU16FF.SequenceNo:      // FF00
        case (int)MetaMsgU16FF.Chanel:          // FF20
        case (int)MetaMsgU16FF.EndOfTrack:      // FF2F
        case (int)MetaMsgU16FF.Tempo:           // FF51
        case (int)MetaMsgU16FF.SMPTE:           // FF54
        case (int)MetaMsgU16FF.TimeSignature:   // FF58
        case (int)MetaMsgU16FF.KeySignature:    // FF59
          // why is this filtered and no others ? see gettrackmessage
          MessageHandler(MidiMsgType.MetaInf, nTrackIndex, nTrackOffset, CurrentIntMessage, CurrentByte, CurrentTrackPulse, CurrentTrackRunningStatus, false);
          DELTA_Returned = GetMetaNextPos(nTrackOffset);
          break;
        case (int)MetaMsgU16FF.SystemSpecific:  // FF7F
        case (int)MetaMsgU16FF.SystemExclusive: // FF7F
          Debug.Print("?---------sys");
          MessageHandler(MidiMsgType.System, nTrackIndex, nTrackOffset, CurrentIntMessage, CurrentByte, CurrentTrackPulse, CurrentTrackRunningStatus, false);
          DELTA_Returned = GetMetaNextPos(nTrackOffset);
          break;
        default:
          {
            if (CurrentByte < 0x80) {
              // Running Status
              int ExpandedRSE = CurrentTrackRunningStatus << 8;
              int delta1 = -1;
              if ((delta1 = GetNextRsePosition(nTrackOffset)) == -1)
              {
                int test = GetOffset(nTrackOffset);
                Debug.Assert(false, string.Format("warning… {0:X2}, {1:X}|{1:N0}", ExpandedRSE, test));
              }
              else
              {
                DELTA_Returned = delta1;
                MessageHandler(GetMsgTyp(CurrentTrackRunningStatus), nTrackIndex, nTrackOffset, CurrentTrackRunningStatus, (byte)CurrentTrackRunningStatus, CurrentTrackPulse, CurrentTrackRunningStatus, true);
              }
            }
            else if (MidiMessageInfo.IsMidiMessage(CurrentIntMessage))
            {
              CurrentTrackRunningStatus = (SmfFileHandle[nTrackIndex, nTrackOffset]);
              DELTA_Returned = GetNextPosition(nTrackOffset);
              // Debug.Print("{0:X}",runningStatus32);
              MessageHandler(GetMsgTyp(CurrentTrackRunningStatus), nTrackIndex, nTrackOffset, CurrentTrackRunningStatus, (byte)CurrentTrackRunningStatus, CurrentTrackPulse, CurrentTrackRunningStatus, false);
              DELTA_Returned++;
              return DELTA_Returned;
            }
            else
              throw new FormatException("Bad format!\nThere is probably a problem with the Input File unless we made an error reading it!)");
          }

          break;
      }
      DELTA_Returned++;
      // what is roy[1] if not the current track position?
      return DELTA_Returned;
    }

    /// MESSAGE PARSER see 'ParseTrackMeta';
    /// 
    /// TODO: Examine relevant context and explain.
    public virtual int GetTrackTiming(int nTrackIndex, int nTrackOffset, int delta)
    {
      int	DELTA_Returned		= delta;
      byte CurrentByte			= SmfFileHandle.Get8Bit(nTrackIndex, nTrackOffset);
      int	CurrentIntMessage = SmfFileHandle.Get16BitInt32(nTrackIndex, nTrackOffset);
      
      switch (CurrentIntMessage) {
        // text
        case (int)MetaMsgU16FF.Text:
        case (int)MetaMsgU16FF.Copyright:
        case (int)MetaMsgU16FF.SequenceName:
        case (int)MetaMsgU16FF.InstrumentName:
        case (int)MetaMsgU16FF.Lyric:
        case (int)MetaMsgU16FF.Marker:
        case (int)MetaMsgU16FF.Cue:
        // channel
        case (int)MetaMsgU16FF.Port:
        case (int)MetaMsgU16FF.SequenceNo:
        case (int)MetaMsgU16FF.Chanel:
        case (int)MetaMsgU16FF.EndOfTrack:
        case (int)MetaMsgU16FF.SMPTE:
          DELTA_Returned = GetMetaNextPos(nTrackOffset);
          break;
        case (int)MetaMsgU16FF.Tempo:
          var muspqn = SmfFileHandle[SelectedTrackNumber].Get24Bit(nTrackOffset + 3);
          TempoMap.Push(muspqn, Division, CurrentTrackPulse);
          // Log.ErrorMessage($"Delta: {delta}, Pulse: {CurrentTrackPulse}, Pulse: {CurrentTrackPulse/Division}");
          OnTempoChanged(DELTA_Returned, muspqn);
          DELTA_Returned = GetMetaNextPos(nTrackOffset);
          break;
        case (int)MetaMsgU16FF.TimeSignature:
          TimeSignature.SetSignature(
            (int)this[nTrackIndex,nTrackOffset+3],
            (int)Math.Pow(-this[nTrackIndex,nTrackOffset+4],2),
            (int)this[nTrackIndex,nTrackOffset+5],
            (int)this[nTrackIndex,nTrackOffset+6]
           );
          DELTA_Returned = GetMetaNextPos(nTrackOffset);
          break;
        case (int)MetaMsgU16FF.KeySignature:
          byte b = (this[nTrackIndex,nTrackOffset+3]);
          KeySignature.SetSignature((KeySignatureType)b,this[nTrackIndex,nTrackOffset+4]==0);
          DELTA_Returned = GetMetaNextPos(nTrackOffset);
          break;
        case (int)MetaMsgU16FF.SystemSpecific:
        case (int)MetaMsgU16FF.SystemExclusive:
          DELTA_Returned = GetMetaNextPos(nTrackOffset);
          break;
        default:
          {
            if (CurrentByte < 0x80) {
              // Running Status
              int ExpandedRSE = CurrentTrackRunningStatus << 8;
              int delta1 = -1;
              if ((delta1 = GetNextRsePosition(nTrackOffset)) == -1) {
                int test = GetOffset(nTrackOffset);
                Debug.Assert(false, string.Format("warning… {0:X2}, {1:X}|{1:N0}", ExpandedRSE, test));
              } else {
                DELTA_Returned = delta1;
              }
            } else if (MidiMessageInfo.IsMidiMessage(CurrentIntMessage)) {
              CurrentTrackRunningStatus = (SmfFileHandle[nTrackIndex, nTrackOffset]);
              DELTA_Returned = GetNextPosition(nTrackOffset);
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
    void DispatchHandlers(MidiMsgType t, int track, int offset, int imsg, byte bmsg, long ppq, int rse, bool isrse)
    {
      foreach (MidiEventDelegate method in MessageHandlers)
        method(t, track, offset, imsg, bmsg, ppq, rse, isrse);
    }
    
    /// MESSAGE methods

    /// <inheritdoc/>
    public void OnMidiMessage(
      MidiMsgType msgType,
      int nTrackIndex,
      int nTrackOffset,
      int midiMsg32,
      byte midiMsg8,
      long pulse,
      int delta,
      bool isRunningStatus)
    {
      if (ProcessMidiMessage != null)
        ProcessMidiMessage(this, new MidiMessageEvent(msgType, nTrackIndex, nTrackOffset, midiMsg32, midiMsg8, pulse, delta, isRunningStatus));
      
      if (MessageHandlers.Count>0)
        DispatchHandlers(msgType, nTrackIndex, nTrackOffset, midiMsg32, midiMsg8, pulse, delta, isRunningStatus);
    }
    
    #endregion
    #region MESSAGE properties/events
    
    public event EventHandler<TempoChangedEventArgs> TempoChangedEvent;
    
    /// <summary>
    /// ¡This delta may not be correct? Wha? Why delta and not the RunningStatus?
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="tValue"></param>
    protected virtual void OnTempoChanged(int delta, uint tValue)
    {
      var handler = TempoChangedEvent;
      if (handler != null)
        handler(this, new TempoChangedEventArgs(delta, tValue));
    }

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
    public bool UseEventHandler { get; private set; }
    
    /// <inheritdoc/>
    public bool HasTrackReaderDelegate { get { return MessageHandler != null; } }

    #endregion
    
    #region .ctor
    
    public MidiReader() : this(true)
    {
      LoadTrack = GetTrackMessage;
    }
    public MidiReader(bool useEventHandler)
    {
      if (UseEventHandler = useEventHandler)
        MessageHandler = OnMidiMessage;
    }
    
    public MidiReader(MidiMessageHandler handler) : this(true) {}
    public MidiReader(MidiEventDelegate handler) : this(false) { MessageHandler = handler; }
    public MidiReader( string fileName ) : this(true)
    {
      MidiFileName = fileName;
    }

    #endregion
    
    #region ¿¡WHAT?!
    ///	(¿¡WHAT?!) this is only called on errors
    /// <inheritdoc/>
    public int GetOffset(int offset)
    {
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
      ClearAll();
    }

    void ClearAll()
    {
      ResetTiming();
      SmfFileHandle = default(smf_mthd);
      MidiFileName = null;
      selectedTrackNumber = -1;
      GC.Collect();
    }

    #endregion

    readonly object ParseAllLock = new object();
    
    #region READ VAR INT
    
    int NextDelta(int offset, out long result) {
      return NextDelta(SelectedTrackNumber, offset, out result);
    }
    
    /// <summary>
    /// Use offset - result to get your length.
    /// </summary>
    /// <param name="ntrack">track index.</param>
    /// <param name="offset">offset in bytes into the track</param>
    /// <param name="result">The current running number of elapsed pulses.</param>
    /// <returns>next byte offset (read) position.</returns>
    int NextDelta(int ntrack, int offset, out long result)
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

    /// <summary>
    /// Retruns the SMF Track at <see cref="SelectedTrackNumber"/>
    /// </summary>
    /// <value></value>
    smf_mtrk NTrack { get { return SmfFileHandle[SelectedTrackNumber]; } }
    
    /// <inheritdoc/>
    public void ParseTrackMeta(int tk)
    {
      long delta;
      int i = 0;
      selectedTrackNumber = tk;
      while (i < NTrack.track.Length)
      {
        i = NextDelta(tk, i, out delta);
        CurrentTrackPulse += delta;
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
      CurrentTrackPulse = 0;
      
      while (i < NTrack.track.Length)
      {
        i = NextDelta(i, out delta);
        CurrentTrackPulse += delta;
        i = GetTrackMessage(i, Convert.ToInt32(delta));
        OnTrackLoadProgressChanged(i);
      }
      
      OnAfterTrackLoaded(EventArgs.Empty);
      
      return NTrack.track.Length;
    }
    #endregion
    #region READ (PARSE) ALL TRACKS

    // when IsTrackSelected, the total number of ticks in the track.
    long totlen = 0;
    public List<long> TrackLength { get; private set; } = new List<long>();
    /// <summary>
    /// Parse all tracks to mididatalist
    /// </summary>
    /// <returns>The length (in bytes) of the track.</returns>
    public void ParseAll()
    {
      MidiDataList.Clear();
      TrackLength.Clear();
      for (int i = 0; i < SmfFileHandle.NumberOfTracks; i++) {
        MidiDataList.CreateKey(i);
        TrackLength.Add(-1);
      }
      CurrentTrackPulse = 0;
      lock (ParseAllLock)
      {
        for (int nTrackIndex = 0; nTrackIndex < SmfFileHandle.NumberOfTracks; nTrackIndex++)
        {
          ResetTiming();
          // Log.ErrorMessage($"Parsing track {nTrackIndex}");
          selectedTrackNumber = nTrackIndex;
          lock (this)
          {
            long delta = 0;
            int nTrackOffset = 0;
            while (nTrackOffset < SmfFileHandle.Tracks[nTrackIndex].track.Length)
            {
              nTrackOffset = NextDelta(nTrackIndex, nTrackOffset, out delta);
              CurrentTrackPulse += delta;
              nTrackOffset = GetNTrackMessage(nTrackIndex, nTrackOffset, Convert.ToInt32(delta));
            }
          }
          TrackLength[nTrackIndex] = CurrentTrackPulse;
        }
        
      }
    // OnAfterTrackLoaded(EventArgs.Empty);
    // return NTrack.track.Length;
    }

    void PARSER_MidiDataList(MidiMsgType msgType, int nTrackIndex, int nTrackOffset, int midiMsg32, byte midiMsg8, long pulse, int delta, bool isRunningStatus)
    {
      switch (msgType)
      {
        case MidiMsgType.MetaStr:
          midiDataList.AddV(SelectedTrackNumber, new MetaMessage(MidiMsgType.MetaStr,pulse,midiMsg32,GetMetaBString(nTrackOffset)));
          break;
        case MidiMsgType.MetaInf:
          var midiMsg = new MetaMessage(pulse,midiMsg32,GetMetaData(nTrackOffset));
          midiDataList.AddV(SelectedTrackNumber,midiMsg);
          break;
        case MidiMsgType.System:
        case MidiMsgType.SysCommon:
          if (midiMsg32==0xFF7F) midiDataList.AddV(SelectedTrackNumber,new SysExMessage(pulse,midiMsg32,GetMetaValue(nTrackOffset)));
          else if (midiMsg32==0xF0) midiDataList.AddV(SelectedTrackNumber,new SysExMessage(pulse,midiMsg32,GetEventValue(nTrackOffset)));
          else Log.ErrorMessage("Improper MidiMsgType classification?");
          break;
        default:
          if (isRunningStatus) MidiDataList.AddV(SelectedTrackNumber,new ChannelMessage(pulse,delta,GetRseEventValue(nTrackOffset)));
          else MidiDataList.AddV(SelectedTrackNumber,new ChannelMessage(pulse,delta,GetEventValue(nTrackOffset)));
          break;
      }
    }

    #endregion

    #region TRACK
    /// <summary>
    /// <para>Get/Set Selected Track Number;</para>
    /// <para>Setting the track number triggers the TrackChanged event!</para>
    /// </summary>
    public int SelectedTrackNumber {
      get { return selectedTrackNumber; }
      set {
        selectedTrackNumber = value;
        OnTrackChanged(EventArgs.Empty);
      }
    } internal int selectedTrackNumber;
    
    ///<summary>a track is selected</summary>
    public virtual string TrackSelectAction()
    {
      GetDivision();
      ResetTiming();
      if (SelectedTrackNumber >= 0)
      {
        totlen = ParseTrack();
        return StringTrackInfo;
      } else
        return StringRes.STRING_APP_NAME;
    }

    #endregion
    
    #region FILE

    public string MidiFileName { get; set; }

    public smf_mthd SmfFileHandle { get; set; }

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

    public const string Resource_TrackLoaded =
      "{12} MIDI Track — Format: v{11}, " +
      "Track: {0,3:000},  PPQ: {3}, (first) Tempo: {4}\n" +
      "TSig: {5}/{6} Clocks: {7}, {8} 32nds, KeySig: {9} {10}";

    string StringTrackInfo
    {
      get {
        return string.Format(
                   Resource_TrackLoaded,
          /*  0 */ SelectedTrackNumber,
          /*  1 */ totlen,
          /*  2 */ SmfFileHandle[SelectedTrackNumber].Size,
          /*  3 */ Convert.ToInt32(SmfFileHandle.Division),
          /*  4 */ Convert.ToSingle(TempoMap.Top.Tempo),
          /*  5 */ TimeSignature.Numerator,
          /*  6 */ TimeSignature.Denominator,
          /*  7 */ TimeSignature.Clocks,
          /*  8 */ TimeSignature.ThirtySeconds,
          /*  9 */ KeySignature.KeyType,
          /* 10 */ KeySignature.IsMajor ? "Major" : "Minor",
          /* 11 */ SmfFileHandle.Format,
          /* 12 */ StringRes.STRING_APP_NAME
         );
      }
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
