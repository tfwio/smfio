/* Date: 11/12/2005 - Time: 4:19 PM */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using on.smfio.Common;

using FileMode=System.IO.FileMode;
using FileAccess=System.IO.FileAccess;
using FileShare=System.IO.FileShare;
using FileStream=System.IO.FileStream;
using BinaryReader=System.IO.BinaryReader;

using on.smfio.chunk;

using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;

namespace on.smfio
{
  /// <summary>
  /// Internally, we load three text files from a subdirectory named ‘ext’.
  /// Controller change values, drum kit names and instrument names.
  /// </summary>
  public partial class Reader : IDisposable, IReader
  {
    public bool FileIsIncomplete { get; set; } = false;

    public MTrk this[int kTrackID] { get { return FileHandle[kTrackID]; } }
    public byte this[int kTrackID, int kTrackOffset] { get { return FileHandle[kTrackID, kTrackOffset]; } }
    public byte[] this[int kTrackID, int kTrackOffset, int kSize] { get { return FileHandle[kTrackID, kTrackOffset, kSize]; } }

    const int default_Fs = 44100;
    const int default_Tempo = 120;
    const int default_Division = 480;

    /// <summary>Static MTHD loader.</summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    MTHd GetMthd(string fileName)
    {
      int i = 0;
      bool hasError = false;
      MTHd FileHandle = null;
      using (var STREAM = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var READER = new BinaryReader(STREAM))
      {
        FileHandle = new MTHd(READER);
        FileHandle.Tracks = new MTrk[FileHandle.NumberOfTracks];
        for (i = 0; i < FileHandle.NumberOfTracks; i++)
        {
          if (READER.BaseStream.Position >= READER.BaseStream.Length) {
            FileHandle.OverrideNumberOfTracks((short)i);
            FileIsIncomplete = true;
            hasError = true;
          }
          if (hasError) continue;
          FileHandle.Tracks[i] = new MTrk(READER);
        }
      }
      return FileHandle;
    }

    #region INotifyPropertyChanged (isn't used)

    public event PropertyChangedEventHandler PropertyChanged;
    void Notify(string property)
    {
      if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    #endregion

    #region PROP
    /// <inheritdoc/>
    public int SelectedTrackChannel
    {
      get { return selectedTrackChannel; }

    }
    int selectedTrackChannel = -1;
    /// <inheritdoc/>
    public DictionaryList<int, MIDIMessage> MidiDataList
    {
      get { return midiDataList; }
      set { midiDataList = value; }
    }
    DictionaryList<int, MIDIMessage> midiDataList = new DictionaryList<int, MIDIMessage>();

    /// <inheritdoc/>
    public List<int> ChannelFilter
    {
      get { return channelFilter; }
    }
    public List<int> channelFilter = new List<int>();

    #endregion

    public int CurrentRunningStatus8 { get; private set; }
    public int CurrentRunningStatus16 { get; private set; }
    public int CurrentStatus { get; private set; }

    /// <summary>
    /// Current Track's pulse ocunt in ticks per quarter-note.
    /// </summary>
    public long CurrentTrackPulse { get; private set; }

    #region TIME
    /// <inheritdoc/>
    public TempoMap TempoMap
    {
      get { return tempoMap; }
    }
    TempoMap tempoMap = new TempoMap();

    /// <inheritdoc/>
    public MidiKeySignature KeySignature
    {
      get { return keySignature; }
      set { keySignature = value; }
    }
    MidiKeySignature keySignature = new MidiKeySignature();

    /// <inheritdoc/>
    public MidiTimeSignature TimeSignature
    {
      get { return timeSignature; }
      set { timeSignature = value; }
    }
    MidiTimeSignature timeSignature = new MidiTimeSignature();
    
    public SmpteOffset SMPTE_Offset {
      get { return smpte; } set { smpte = value; }
    } SmpteOffset smpte = new SmpteOffset();
    
    #endregion
    #region TIME (int) DivMeasure, DivBar, DivNote, FileDivision

    /// <summary>Bar x4</summary>
    public static int DivMeasure { get; private set; }

    /// <summary>Quarter x4</summary>
    public int DivBar { get; private set; }

    /// <summary>PPQ (division) x4</summary>
    public int DivQuarter { get; private set; }

    /// <summary>Midi header-&gt;Division.</summary>
    public short Division { get { return FileHandle.Division; } }

    public void GetDivision()
    {
      DivQuarter = Division * 4;
      DivBar = DivQuarter * 4;
      DivMeasure = DivBar * 4;
    }

    #endregion

    /// <summary>
    /// Not to be confused with the trigger when a file
    /// is loaded.
    /// 
    /// This is called before each track is parsed.
    /// </summary>
    public virtual void ResetTiming()
    {
      CurrentRunningStatus8 = -1; // 0xFF
      CurrentRunningStatus16 = -1;
      CurrentStatus = -1;
      CurrentTrackPulse = 0;
    }

    #region FILE read, getmem

    /// <inheritdoc/>
    public void Read()
    {
      TempoMap.Clear();
      smpte.SetSMPTE(0,0,0,0,0);
      
      if (UseEventHandler) GetMemory();
      OnFileLoaded(EventArgs.Empty);
    }

    /// <inheritdoc/>
    public void GetMemory()
    {
      FileHandle = GetMthd(MidiFileName);
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
      TempoMap.Finalize(this);
      MessageHandler = backup;
    }

    #endregion

    #region MESSAGE PARSER GetMsgTyp

    MidiMsgType GetMsgTyp(int status, MidiMsgType def = MidiMsgType.ChannelVoice)
    {
      return GetMsgTyp(Convert.ToByte(status & 0xFF), def);
    }
    MidiMsgType GetMsgTyp(byte status, MidiMsgType def = MidiMsgType.ChannelVoice)
    {
      // Debug.Print("? {0:X}", status);
      switch (status & 0xF0)
      {
        case 0x80:
          return MidiMsgType.NoteOff;
        case 0x90:
          return MidiMsgType.NoteOn;
        case 0xB0:
          return MidiMsgType.ControllerChange;
        case 0xF0:
          return MidiMsgType.SystemExclusive;
        case 0xF7:
          return MidiMsgType.SequencerSpecific;
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
      return GetNTrackMessage(ReaderIndex, position, delta);
    }

    private const string error_smpte = "SMPTE Offset not currently supported";

    /// <summary>
    /// Parse Metadata
    /// 
    /// This method is particularly suited to 
    /// </summary>
    public virtual int GetNTrackMessage(int nTrackIndex, int nTrackOffset, int delta)
    {
      int DELTA_Returned = delta;
      ushort msg16 = FileHandle.Get16Bit(nTrackIndex, nTrackOffset);
      byte msg8 = (byte)(msg16 & 0xFF);
      CurrentStatus = msg16;
      var hexMsg = $"{msg16:X2}";
      
      switch (msg16)
      {
        case Stat16.SequenceNumber:  // FF00
        case Stat16.Text:            // FF01
        case Stat16.Copyright:       // FF02
        case Stat16.SequenceName:    // FF03
        case Stat16.InstrumentName:  // FF04
        case Stat16.Lyric:           // FF05
        case Stat16.Marker:          // FF06
        case Stat16.Cue:             // FF07
        case Stat16.MetaStrFF08:     // FF08 -- added since 09 was found to be used as text.
        case Stat16.MetaStrFF09:     // FF09 -- encountered - engine?
        case Stat16.MetaStrFF0A:     // FF0A -- encountered
        case Stat16.MetaStrFF0B:     // FF0B
        case Stat16.MetaStrFF0C:     // FF0C -- encountered
          MessageHandler(MidiMsgType.MetaStr, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.EndOfTrack:      // FF2F
          MessageHandler(MidiMsgType.EOT, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = NTrack.Data.Length;// NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.ChannelPrefix:   // FF20
        case Stat16.PortMessage:     // FF21?
        case Stat16.SetTempo:        // FF51
          MessageHandler(MidiMsgType.MetaInf, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.SMPTEOffset:     // FF54
          MessageHandler(MidiMsgType.MetaInf, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.TimeSignature:   // FF58
        case Stat16.KeySignature:    // FF59
          MessageHandler(MidiMsgType.MetaInf, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.SequencerSpecificMetaEvent:  // FF7F
          MessageHandler(MidiMsgType.SequencerSpecific, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.SystemExclusive: // 0xF0
          MessageHandler(MidiMsgType.SystemExclusive, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = FileHandle[nTrackIndex].GetEndOfSystemExclusive(nTrackOffset);
          break;
        default:
          {
            if (FileHandle.Tracks[nTrackIndex].Data[nTrackOffset] < 0x80) // running-status message
            {
              // Running Status
              CurrentStatus = CurrentRunningStatus16;
              
              int ExpandedRSE = CurrentRunningStatus8;// << 8;
              int delta1 = -1;
              if ((delta1 = IncrementRun(nTrackOffset)) == -1)
              {
                int test = GetOffset(nTrackOffset);
                Debug.Assert(false, string.Format("warning… {0:X2}, {1:X}|{1:N0}", ExpandedRSE, test));
              }
              else
              {
                DELTA_Returned = delta1;
                MessageHandler(GetMsgTyp(CurrentRunningStatus8), nTrackIndex, nTrackOffset, CurrentRunningStatus16, (byte)CurrentRunningStatus8, CurrentTrackPulse, CurrentRunningStatus8, true);
              }
            }
            //else if (StatusQuery.IsMidiMessage(msg32))
            else if (StatusQuery.IsMidiMessage(msg8))
            {
              //CurrentTrackRunningStatus = (FileHandle[nTrackIndex, nTrackOffset]);
              CurrentRunningStatus8 = msg8;
              CurrentRunningStatus16 = msg16;
              DELTA_Returned = GetNextPosition(nTrackOffset);
              MessageHandler(GetMsgTyp(CurrentRunningStatus8), nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
              DELTA_Returned++;
              return DELTA_Returned;
            }
            else
              throw new FormatException("Bad format!\nThere is probably a problem with the Input File unless we made an error reading it!)");
          }

          break;
      }
      return ++DELTA_Returned;
    }

    /// <summary>MESSAGE PARSER</summary>
    public virtual int GetTrackTiming(int nTrackIndex, int nTrackOffset, int delta)
    {
      int DELTA_Returned = delta;
      var msg16 = FileHandle.Get16Bit(nTrackIndex, nTrackOffset);
      byte msg8 = (byte)(msg16 & 0xFF);
      CurrentStatus = msg16; // This is just an attempt at aligning running status.
      var hexMsg = $"{msg16:X2}";
      
      switch (msg16)
      {
          // text
        case Stat16.SequenceNumber: // 0xFF00
        case Stat16.Text:           // 0xFF01
        case Stat16.Copyright:      // 0xFF02
        case Stat16.SequenceName:   // 0xFF03
        case Stat16.InstrumentName: // 0xFF04
        case Stat16.Lyric:          // 0xFF05
        case Stat16.Marker:         // 0xFF06
        case Stat16.Cue:            // 0xFF07
        case Stat16.MetaStrFF08:    // FF08
        case Stat16.MetaStrFF09:    // FF09
        case Stat16.MetaStrFF0A:    // FF0A
        case Stat16.MetaStrFF0B:    // FF0B
        case Stat16.MetaStrFF0C:    // FF0C
        case Stat16.ChannelPrefix:  // 0xFF20
        case Stat16.PortMessage:    /* 0xFF21 */ DELTA_Returned = NTrack.DeltaSeek(nTrackOffset); break;
        case Stat16.EndOfTrack:     /* 0xFF2F */ DELTA_Returned = NTrack.Data.Length-1; break;
        case Stat16.SetTempo: // 0xFF51
          var muspqn = FileHandle[ReaderIndex].ReadU24(nTrackOffset + 3);
          TempoMap.Push(muspqn, Division, CurrentTrackPulse);
          OnTempoChanged(DELTA_Returned, muspqn);
          DELTA_Returned = NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.SMPTEOffset: // 0xFF54
          SMPTE_Offset.SetSMPTE(
            FileHandle.Tracks[nTrackIndex].Data[nTrackOffset+3],
            FileHandle.Tracks[nTrackIndex].Data[nTrackOffset+4],
            FileHandle.Tracks[nTrackIndex].Data[nTrackOffset+5],
            FileHandle.Tracks[nTrackIndex].Data[nTrackOffset+6],
            FileHandle.Tracks[nTrackIndex].Data[nTrackOffset+7]
           );
          DELTA_Returned = NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.TimeSignature: // 0xFF58
          TimeSignature.SetSignature(
            (int)this[nTrackIndex, nTrackOffset + 3],
            (int)Math.Pow(-this[nTrackIndex, nTrackOffset + 4], 2),
            (int)this[nTrackIndex, nTrackOffset + 5],
            (int)this[nTrackIndex, nTrackOffset + 6]
           );
          DELTA_Returned = NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.KeySignature: // 0xFF59
          byte b = (this[nTrackIndex, nTrackOffset + 3]);
          KeySignature.SetSignature((KeySignatureType)b, this[nTrackIndex, nTrackOffset + 4] == 0);
          DELTA_Returned = NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.SequencerSpecificMetaEvent: // 0xFF7F
          DELTA_Returned = NTrack.DeltaSeek(nTrackOffset);
          break;
        case Stat16.SystemExclusive:
          var pLastIndex = FileHandle[nTrackIndex].GetEndOfSystemExclusive(nTrackOffset);
          DELTA_Returned = pLastIndex;
          break;
        default:
          {
            if (FileHandle.Tracks[nTrackIndex].Data[nTrackOffset] < 0x80)
            {
              CurrentStatus = CurrentRunningStatus16;
              // Running Status
              // int ExpandedRSE = CurrentTrackRunningStatus;// << 8;
              int ExpandedRSE = CurrentRunningStatus8;// << 8;
              int delta1 = -1;
              if ((delta1 = IncrementRun(nTrackOffset)) == -1)
              {
                int test = GetOffset(nTrackOffset);
                Debug.Assert(false, string.Format("warning… {0:X2}, {1:X}|{1:N0}", ExpandedRSE, test));
              }
              else
              {
                DELTA_Returned = delta1;
              }
            }
            else if (StatusQuery.IsMidiMessage(msg8))
            {
              CurrentRunningStatus8 = msg8;
              CurrentRunningStatus16 = msg16;
              DELTA_Returned = GetNextPosition(nTrackOffset);
              return ++DELTA_Returned;
            }
            else
              throw new FormatException("Bad format!\nThere is probably a problem with the Input File unless we made an error reading it!)");
          }

          break;
      }
      return ++DELTA_Returned;
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

      if (MessageHandlers.Count > 0)
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
    public MidiReaderLoadTrackDelegate LoadTrack
    {
      get { return loadTrack; }
      set { loadTrack = value; }
    }
    MidiReaderLoadTrackDelegate loadTrack = null;

    /// MESSAGE properties/events
    public List<MidiEventDelegate> MessageHandlers
    {
      get { return messageHandlers; }
    }
    List<MidiEventDelegate> messageHandlers = new List<MidiEventDelegate>();

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

    public Reader() : this(true)
    {
      LoadTrack = GetTrackMessage;
    }
    public Reader(bool useEventHandler)
    {
      if (UseEventHandler = useEventHandler)
        MessageHandler = OnMidiMessage;
    }

    public Reader(MidiMessageHandler handler) : this(true) { }
    public Reader(MidiEventDelegate handler) : this(false) { MessageHandler = handler; }
    public Reader(string fileName) : this(true)
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
      for (int i = 0; i <= ReaderIndex; i++)
      {
        result += FileHandle[i].Data.Length + 8; // if not 8, this will be 4
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
      FileHandle = default(MTHd);
      MidiFileName = null;
      selectedTrackNumber = -1;
      GC.Collect();
    }

    #endregion

    readonly object ParseAllLock = new object();

    #region READ VAR INT

    /// <summary>
    /// Read from track with `ntrack` index at `offset`.
    /// 
    /// into (long) `result` is stored our variable delta.
    /// </summary>
    /// <param name="ntrack">track index.</param>
    /// <param name="offset">offset in bytes into the track</param>
    /// <param name="result">The current running number of elapsed pulses.</param>
    /// <returns>next byte offset (read) position.</returns>
    int NextDelta(int ntrack, int offset, out long result) {
      return FileHandle.ReadDelta(ntrack, offset, out result);
    }

    #endregion
    #region READ META TRACK

    /// <summary>
    /// Retruns the SMF Track at <see cref="ReaderIndex"/>
    /// </summary>
    /// <value></value>
    MTrk NTrack { get { return FileHandle.Tracks[ReaderIndex]; } }

    /// <inheritdoc/>
    public void ParseTrackMeta(int tk)
    {
      long delta;
      int i = 0;
      selectedTrackNumber = tk;
      while (i < FileHandle.Tracks[ReaderIndex].Data.Length)
      {
        i = FileHandle.ReadDelta(tk, i, out delta);
        CurrentTrackPulse += delta;
        i = GetTrackTiming(selectedTrackNumber, i, Convert.ToInt32(delta));
      }
    }

    #endregion

    /// <summary>
    /// Parse the selected track
    /// </summary>
    /// <returns>The length (in bytes) of the track.</returns>
    public virtual long ParseTrack()
    {
      OnBeforeTrackLoaded(EventArgs.Empty);

      long mSevenBitDelta;
      int i = 0;
      CurrentTrackPulse = 0;

      while (i < NTrack.Data.Length)
      {
        i = FileHandle[ReaderIndex].DeltaRead(i, out mSevenBitDelta);
        
        CurrentTrackPulse += mSevenBitDelta;
        i = GetTrackMessage(i, Convert.ToInt32(mSevenBitDelta));
        OnTrackLoadProgressChanged(i);
      }

      OnAfterTrackLoaded(EventArgs.Empty);

      return NTrack.Data.Length;
    }
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
      for (int i = 0; i < FileHandle.NumberOfTracks; i++)
      {
        MidiDataList.CreateKey(i);
        TrackLength.Add(-1);
      }
      CurrentTrackPulse = 0;
      lock (ParseAllLock)
      {
        for (int nTrackIndex = 0; nTrackIndex < FileHandle.NumberOfTracks; nTrackIndex++)
        {
          ResetTiming();
          selectedTrackNumber = nTrackIndex; // without triggering the event.
          long delta = 0;
          int nTrackOffset = 0;
          do
          {
            nTrackOffset = FileHandle.ReadDelta(nTrackIndex, nTrackOffset, out delta);
            CurrentTrackPulse += delta;
            nTrackOffset = GetNTrackMessage(nTrackIndex, nTrackOffset, Convert.ToInt32(delta));
          }
          while (nTrackOffset < FileHandle.Tracks[nTrackIndex].Data.Length);
          
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
          midiDataList.AddV(ReaderIndex, new MetaMessage(MidiMsgType.MetaStr, pulse, midiMsg32, GetMetaBString(nTrackOffset)));
          break;
        case MidiMsgType.MetaInf:
          byte[] bytes = GetMetaBString(nTrackOffset);
          var midiMsg = new MetaMessage(pulse, midiMsg32, bytes);
          midiDataList.AddV(ReaderIndex, midiMsg);
          break;
        case MidiMsgType.SystemExclusive:
          Debug.WriteLine("Skip System Exclusive Message (for now)");
          break;
        case MidiMsgType.ChannelVoice:
        case MidiMsgType.NoteOff:
        case MidiMsgType.NoteOn:
          midiDataList.AddV(ReaderIndex, new SequencerSpecific(pulse, midiMsg32, GetEventValue(nTrackOffset)));
          break;
        case MidiMsgType.SequencerSpecific:
          midiDataList.AddV(ReaderIndex, new SequencerSpecific(pulse, midiMsg32, GetEventValue(nTrackOffset)));
          break;
          // case MidiMsgType.SystemExclusive:
          //   Debug.WriteLine("Skip System Exclusive Message (for now)");
          //   if (midiMsg32 == 0xFF7F) midiDataList.AddV(ReaderIndex, new SequencerSpecific(pulse, midiMsg32, GetMetaValue(nTrackOffset)));
          //   else Log.ErrorMessage("Improper MidiMsgType classification?");
          //   break;
        case MidiMsgType.EOT:
          midiDataList.AddV(ReaderIndex, new MetaMessage(pulse, midiMsg32));
          break;
        default:
          if (isRunningStatus) MidiDataList.AddV(ReaderIndex, new ChannelMessage(pulse, delta, GetRseEventValue(nTrackOffset)));
          else MidiDataList.AddV(ReaderIndex, new ChannelMessage(pulse, delta, GetEventValue(nTrackOffset)));
          break;
      }
    }

    #endregion

    #region TRACK
    /// <summary>
    /// <para>Get/Set Selected Track Number;</para>
    /// <para>Setting the track number triggers the TrackChanged event!</para>
    /// </summary>
    public int ReaderIndex
    {
      get { return selectedTrackNumber; }
      set
      {
        selectedTrackNumber = value;
        OnTrackChanged(EventArgs.Empty);
      }
    }
    internal int selectedTrackNumber;

    ///<summary>a track is selected</summary>
    public virtual string TrackSelectAction()
    {
      GetDivision();
      ResetTiming();
      if (ReaderIndex >= 0)
      {
        totlen = ParseTrack();
        return StringTrackInfo;
      }
      else
        return StringRes.STRING_APP_NAME;
    }

    #endregion

    #region FILE

    public string MidiFileName { get; set; }

    public MTHd FileHandle { get; set; }

    #endregion

    // -----------------------------------------------

    #region EVENT clearview,fileloaded,trackchanged,beforetrackload,aftertrackload
    public event CliHandler ClearView;
    protected virtual void OnClearView(CliEvent e)
    {
      if (ClearView != null)
      {
        ClearView(this, e);
      }
    }

    public event CliHandler FileLoaded;
    protected virtual void OnFileLoaded(CliEvent e)
    {
      if (FileLoaded != null)
      {
        FileLoaded(this, e);
      }
    }

    /// <summary>
    /// This is triggered after the track is parsed.
    /// </summary>
    public event CliHandler BeforeTrackLoaded;
    protected virtual void OnBeforeTrackLoaded(CliEvent e)
    {
      if (BeforeTrackLoaded != null)
      {
        BeforeTrackLoaded(this, e);
      }
    }
    /// <summary>
    /// This is triggered after the track is parsed.
    /// </summary>
    public event CliHandler AfterTrackLoaded;
    protected virtual void OnAfterTrackLoaded(CliEvent e)
    {
      if (AfterTrackLoaded != null)
      {
        AfterTrackLoaded(this, e);
      }
    }
    /// <summary>
    /// This is triggered after a track number is selected,
    /// and before the track is parsed.
    /// See <see cref="ReaderIndex" />.
    /// </summary>
    public event CliHandler TrackChanged;
    protected virtual void OnTrackChanged(CliEvent e)
    {
      if (TrackChanged != null)
      {
        TrackChanged(this, e);
      }
    }
    #endregion

    // -----------------------------------------------

    #region STR track info, reader status (caption/tooltip)

    public const string Resource_TrackLoaded =
      "{12} MIDI Track — Format: v{11}, SMPTE: {13}," +
      "Track: {0,3:000},  PPQ: {3}, (first) Tempo: {4}\n" +
      "TSig: {5}/{6} Clocks: {7}, {8} 32nds, KeySig: {9} {10}";

    string StringTrackInfo
    {
      get
      {
        var smpte_offset = $"{SMPTE_Offset}";
        return string.Format(
          Resource_TrackLoaded,
          /*  0 */ ReaderIndex,
          /*  1 */ totlen,
          /*  2 */ FileHandle[ReaderIndex].Size,
          /*  3 */ Convert.ToInt32(FileHandle.Division),
          /*  4 */ Convert.ToSingle(TempoMap.Top.Tempo),
          /*  5 */ TimeSignature.Numerator,
          /*  6 */ TimeSignature.Denominator,
          /*  7 */ TimeSignature.Clocks,
          /*  8 */ TimeSignature.ThirtySeconds,
          /*  9 */ KeySignature.KeyType,
          /* 10 */ KeySignature.IsMajor ? "Major" : "Minor",
          /* 11 */ FileHandle.Format,
          /* 12 */ StringRes.STRING_APP_NAME,
          /* 13 */ SMPTE_Offset
         );
      }
    }

    #endregion

    // obsolete (not replaced, just absolute junk)
    #region Failed Progress Attempt

    public event EventHandler<ProgressChangedEventArgs> TrackLoadProgressChanged;

    protected virtual void OnTrackLoadProgressChanged(int i)
    {
      int progressAmount = (i / FileHandle[selectedTrackNumber].Data.Length) * 100;
      ProgressChangedEventArgs e = new ProgressChangedEventArgs(i, null);
      if (TrackLoadProgressChanged != null) TrackLoadProgressChanged(this, e);
    }

    #endregion
  }
}
