/* Date: 11/12/2005 - Time: 4:19 PM */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;
using FileMode=System.IO.FileMode;
using FileAccess=System.IO.FileAccess;
using FileShare=System.IO.FileShare;
using FileStream=System.IO.FileStream;
using BinaryReader=System.IO.BinaryReader;

using on.smfio.chunk;
using on.smfio.Common;

namespace on.smfio
{
  /// <summary>
  /// **Reader.cs**  
  /// Internally, we load three text files from a subdirectory named ‘ext’.
  /// Controller change values, drum kit names and instrument names.
  /// </summary>
  public partial class Reader : IDisposable, IReader
  {
    #region MidiMessage Collection
    /// <summary>
    /// This can be set during `.ctor` or set here explicitly before
    /// calling a a Parse or Read function.  
    /// The message List generated is a simple message structure (class)
    /// containing only pulse-count, status and byte data (array) at this
    /// time and may in the future elaborate specific types depending
    /// on the status-type.
    /// </summary>
    /// <value></value>
    public bool GenerateMessageList { get; set; }

    public MidiMessageCollection MidiMessages {
      get { return midiMessageCollection; }
    } MidiMessageCollection midiMessageCollection = new MidiMessageCollection();

    #endregion

    public MTrk this[int kTrackID] { get { return FileHandle[kTrackID]; } }
    public byte this[int kTrackID, int kTrackOffset] { get { return FileHandle[kTrackID, kTrackOffset]; } }
    public byte[] this[int kTrackID, int kTrackOffset, int kSize] { get { return FileHandle[kTrackID, kTrackOffset, kSize]; } }

    #region INotifyPropertyChanged (isn't used)

    public event PropertyChangedEventHandler PropertyChanged;
    void Notify(string property)
    {
      if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    #endregion

    public int CurrentRunningStatus8 { get; private set; }
    public int CurrentRunningStatus16 { get; private set; }
    public int CurrentStatus { get; private set; }

    /// <summary>
    /// Current Track's pulse count in ticks per quarter-note.
    /// </summary>
    public long CurrentTrackPulse { get; private set; }

    #region TempoMap Track info: TempoMap, KeySignature, TimeSignature, SMPTE_Offset

    /// <inheritdoc/>
    public TempoMap TempoMap {
      get { return tempoMap; }
    } TempoMap tempoMap = new TempoMap();

    /// <inheritdoc/>
    public MidiKeySignature KeySignature
    {
      get { return keySignature; }
      set { keySignature = value; }
    } MidiKeySignature keySignature = new MidiKeySignature();

    /// <inheritdoc/>
    public MidiTimeSignature TimeSignature {
      get { return timeSignature; }
      set { timeSignature = value; }
    } MidiTimeSignature timeSignature = new MidiTimeSignature();
    
    public SmpteOffset SMPTE {
      get { return smpte; } set { smpte = value; }
    } SmpteOffset smpte = new SmpteOffset();
    
    #endregion

    /// <summary>Midi header-&gt;Division.</summary>
    public short Division { get { return FileHandle.Division; } }

    /// <summary>
    /// Not to be confused with the trigger when a file is loaded.
    /// This is called before each track is parsed.
    /// 
    /// Callers:  
    /// - <see cref="on.smfio.Reader.ClearAll"/>  
    /// - <see cref="on.smfio.Reader.ParseAll"/>  
    /// - <see cref="on.smfio.IReader.TrackSelectAction"/>
    /// </summary>
    public virtual void ResetTrackTiming()
    {
      CurrentRunningStatus8 = -1; // 0xFF == -1
      CurrentRunningStatus16 = -1;
      CurrentStatus = -1;
      CurrentTrackPulse = 0;
    }

    internal void ResetTempoMap()
    {
      TempoMap.Clear();
      SMPTE.Reset();
      TimeSignature.Reset();
      KeySignature.Reset();

      MidiMessages.Clear();
      MidiVSTMessageList.Clear();
    }
    /// <summary>
    /// A default read operation generally designated for UI application.
    /// It triggers BeforeFileLoaded and FileLoaded events.  
    /// This could otherwise be thought of as a `Initialize()` function
    /// collecting general characteristics of a SMF/MIDI file.
    /// 
    /// - clears TempoMap (MIDI metadata messages including TempoMap)
    /// - parses the first track of a MIDI/SMF file (I.E. the Tempo Map)
    /// - No tracks other than the TempoMap (track 0) are parsed and
    ///   no parser delegates or event handlers are used.
    /// </summary>
    public void Read()
    {
      OnBeforeFileLoaded(EventArgs.Empty);

      FileHandle = new MThd(MidiFileName);
      ParseTempoMap(0);
      
      OnFileLoaded(EventArgs.Empty);
    }

    #region MESSAGE PARSER: GetTrackMessage, GetTempoMap
    string GetDebugInfo(int nTrackIndex, int nTrackOffset, int delta)
    {
      ushort msg16 = FileHandle.Get16Bit(nTrackIndex, nTrackOffset);
      byte msg8 = (byte)(msg16 & 0xFF);
      // we want to skip a two-byte header?
      byte msg8Plus1 = FileHandle.Get8Bit(nTrackIndex, nTrackOffset + 2);
      CurrentStatus = msg16;
      return $"{{ Track Index: {nTrackIndex}, Track Offset: {nTrackOffset}, delta: {delta} }}\n" +
        $"{{ 16 Bit Message: {msg16:X4}, 8 Bit Message: {msg8:X2}, next 8 Bit Message: {msg8Plus1:X2};";
    }

    /// <summary>
    /// provides **default parser semantic** in that from here we delegate
    /// each message to <see cref="MessageHandler"/>.  
    /// `MessageHandler` can be set via the constructor, or explicitly after
    /// initializing (creating/.ctor) `Reader`.
    /// 
    /// Additionally, <see cref="OnMidiMessage(MidiMsgType, int, int, int, byte, long, int, bool)"/>
    /// exists and can be set as the default message-handler in which case any assigned
    /// event handler(s) (`ProcessMidiMessage`) or delegates (`MessageHandler`)
    /// can and will be used.
    /// </summary>
    public virtual int GetNTrackMessage(int nTrackIndex, int nTrackOffset, int delta)
    {
      int DELTA_Returned = delta;
      ushort msg16 = FileHandle.Get16Bit(nTrackIndex, nTrackOffset);
      byte msg8 = (byte)(msg16 & 0xFF);
      CurrentStatus = msg16;
      // var hexMsg = $"{msg16:X2}";
      if (msg16 >= 0xFF00 && msg16 <= 0xFF0C)
      {
        MessageHandler(MidiMsgType.MetaStr, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
        DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
        return ++DELTA_Returned;
      }
      switch (msg16)
      {
        case Stat16.EndOfTrack:      // FF2F
          MessageHandler(MidiMsgType.EOT, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].Data.Length;
          break;
        case Stat16.SequenceNumber: // 0xFF00
        case Stat16.ChannelPrefix:   // FF20
        case Stat16.PortMessage:     // FF21?
        case Stat16.SetTempo:        // FF51
          MessageHandler(MidiMsgType.MetaInf, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
          break;
        case Stat16.SMPTEOffset:     // FF54
          MessageHandler(MidiMsgType.MetaInf, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
          break;
        case Stat16.TimeSignature:   // FF58
        case Stat16.KeySignature:    // FF59
          MessageHandler(MidiMsgType.MetaInf, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
          break;
        case Stat16.SequencerSpecific_70: // 0xFF70
        case Stat16.SequencerSpecific_71: // 0xFF71
        case Stat16.SequencerSpecific_72: // 0xFF72
        case Stat16.SequencerSpecific_73: // 0xFF73
        case Stat16.SequencerSpecific_74: // 0xFF74
        case Stat16.SequencerSpecific_75: // 0xFF75
        case Stat16.SequencerSpecific_76: // 0xFF76
        case Stat16.SequencerSpecific_77: // 0xFF77
        case Stat16.SequencerSpecific_78: // 0xFF78
        case Stat16.SequencerSpecific_79: // 0xFF79
        case Stat16.SequencerSpecific_7A: // 0xFF7A
        case Stat16.SequencerSpecific_7B: // 0xFF7B
        case Stat16.SequencerSpecific_7C: // 0xFF7C
        case Stat16.SequencerSpecific_7D: // 0xFF7D
        case Stat16.SequencerSpecific_7E: // 0xFF7E
          // we have FF70LLNN where LL is a byte length (assumed: variable bit) and NN is the data we're being provided.
          // MPC Pro software generates it.
          // Theoretically, this could probably happen for other FF70-FF7E?
          MessageHandler(MidiMsgType.SequencerSpecificUnknown, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
          break;
        case Stat16.SequencerSpecific:  // FF7F
          MessageHandler(MidiMsgType.SequencerSpecific, nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
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
              if ((delta1 = Increment(nTrackOffset)) == -1)
              {
                int test = GetOffset(nTrackIndex, nTrackOffset);
                Debug.Assert(false, string.Format("warning… {0:X2}, {1:X}|{1:N0}", ExpandedRSE, test));
              }
              else
              {
                DELTA_Returned = delta1;
                MessageHandler(GetMidiMessageType(CurrentRunningStatus8), nTrackIndex, nTrackOffset, CurrentRunningStatus16, (byte)CurrentRunningStatus8, CurrentTrackPulse, CurrentRunningStatus8, true);
              }
            }
            //else if (StatusQuery.IsMidiMessage(msg32))
            else if (StatusQuery.IsMidiMessage(msg8))
            {
              //CurrentTrackRunningStatus = (FileHandle[nTrackIndex, nTrackOffset]);
              CurrentRunningStatus8 = msg8;
              CurrentRunningStatus16 = msg16;
              DELTA_Returned = Increment(nTrackOffset + 1);
              MessageHandler(GetMidiMessageType(CurrentRunningStatus8), nTrackIndex, nTrackOffset, msg16, msg8, CurrentTrackPulse, CurrentRunningStatus8, false);
              DELTA_Returned++;
              return DELTA_Returned;
            }
            else
              throw new FormatException(
                $"Bad format(?)!\n" +
                $"There is probably a problem with the Input File (unless we made an error reading it)!\n" +
                $"Here is some debug info: {GetDebugInfo(nTrackIndex, nTrackOffset, delta)}");
          }

          break;
      }
      return ++DELTA_Returned;
    }

    /// <summary>
    /// In MIDI Format 1, this would be the first track (index = 0).  
    /// Otherwise Format 0: index = 0 and with  
    /// Format 2, each track will essentially be like a Format 0 track.
    /// 
    /// This method collects information from the 'tempo map' track such as
    /// 
    /// - Tempo Information
    /// - SMPTE Offset
    /// - Time Signature
    /// - Key Signatuer
    /// - Sequencer Specific Data
    /// - System Exclusive Data (in tempo map)
    /// </summary>
    int GetTempoMap(int nTrackIndex, int nTrackOffset, int delta)
    {
      int DELTA_Returned = delta;
      var msg16 = FileHandle.Get16Bit(nTrackIndex, nTrackOffset);
      byte msg8 = (byte)(msg16 & 0xFF);
      CurrentStatus = msg16; // This is just an attempt at aligning running status.
      // var hexMsg = $"{msg16:X2}";
      if (msg16 >= 0xFF00 && msg16 <= 0xFF0C)
      {
        DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
        return ++DELTA_Returned;
      }
      switch (msg16)
      {
        // text
        case Stat16.SequenceNumber: // 0xFF00
        case Stat16.ChannelPrefix:  // 0xFF20
        case Stat16.PortMessage:    /* 0xFF21 */ DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset); break;
        case Stat16.EndOfTrack:     /* 0xFF2F */ DELTA_Returned = FileHandle.Tracks[nTrackIndex].Data.Length-1; break;
        case Stat16.SetTempo: // 0xFF51
          var muspqn = FileHandle[ReaderIndex].ReadU24(nTrackOffset + 3);
          TempoMap.Push(muspqn, Division, CurrentTrackPulse);
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
          break;
        case Stat16.SMPTEOffset: // 0xFF54
          SMPTE.SetSMPTE(
            FileHandle.Tracks[nTrackIndex].Data[nTrackOffset+3],
            FileHandle.Tracks[nTrackIndex].Data[nTrackOffset+4],
            FileHandle.Tracks[nTrackIndex].Data[nTrackOffset+5],
            FileHandle.Tracks[nTrackIndex].Data[nTrackOffset+6],
            FileHandle.Tracks[nTrackIndex].Data[nTrackOffset+7]
           );
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
          break;
        case Stat16.TimeSignature: // 0xFF58
          TimeSignature.SetSignature(
            (int)this[nTrackIndex, nTrackOffset + 3],
            (int)Math.Pow(-this[nTrackIndex, nTrackOffset + 4], 2),
            (int)this[nTrackIndex, nTrackOffset + 5],
            (int)this[nTrackIndex, nTrackOffset + 6]
           );
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
          break;
        case Stat16.KeySignature: // 0xFF59
          KeySignature.SetSignature(
            this[nTrackIndex, nTrackOffset + 3],
            this[nTrackIndex, nTrackOffset + 4]);
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
          break;
        case Stat16.SequencerSpecific_70: // 0xFF70
        case Stat16.SequencerSpecific_71: // 0xFF71
        case Stat16.SequencerSpecific_72: // 0xFF72
        case Stat16.SequencerSpecific_73: // 0xFF73
        case Stat16.SequencerSpecific_74: // 0xFF74
        case Stat16.SequencerSpecific_75: // 0xFF75
        case Stat16.SequencerSpecific_76: // 0xFF76
        case Stat16.SequencerSpecific_77: // 0xFF77
        case Stat16.SequencerSpecific_78: // 0xFF78
        case Stat16.SequencerSpecific_79: // 0xFF79
        case Stat16.SequencerSpecific_7A: // 0xFF7A
        case Stat16.SequencerSpecific_7B: // 0xFF7B
        case Stat16.SequencerSpecific_7C: // 0xFF7C
        case Stat16.SequencerSpecific_7D: // 0xFF7D
        case Stat16.SequencerSpecific_7E: // 0xFF7E
        case Stat16.SequencerSpecific: // 0xFF7F
          DELTA_Returned = FileHandle.Tracks[nTrackIndex].DeltaSeek(nTrackOffset);
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
              if ((delta1 = Increment(nTrackOffset)) == -1)
              {
                int test = GetOffset(nTrackIndex, nTrackOffset);
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
              DELTA_Returned = Increment(nTrackOffset + 1);
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
    void DispatchHandlers(MidiMsgType t, int nTrackIndex, int nTrackOffset, int stat32, byte stat8, long pulse, int statR, bool isRunningStatus)
    {
      foreach (MidiEventDelegate method in MessageHandlers)
        method(t, nTrackIndex, nTrackOffset, stat32, stat8, pulse, statR, isRunningStatus);
    }

    /// MESSAGE methods


    /// <inheritdoc/>
    public void OnMidiMessage(
      MidiMsgType msgType,
      int nTrackIndex,
      int nTrackOffset,
      int stat32,
      byte stat8,
      long pulse,
      int statR,
      bool isRunningStatus)
    {
      if (GenerateMessageList)
        // we need to first clear the MidiMessageCollection!
        MidiMessages.AddV(
          nTrackIndex,
          new MidiMessage(
            (ushort)stat32,
            pulse,
            GetMessageBytes(nTrackIndex, isRunningStatus ? nTrackOffset-1 : nTrackOffset, (ushort)stat32)
            ));

      if (ProcessMidiMessage != null)
        ProcessMidiMessage(this, new MidiMessageEvent(msgType, nTrackIndex, nTrackOffset, stat32, stat8, pulse, statR, isRunningStatus));

      if (MessageHandlers.Count > 0)
        DispatchHandlers(msgType, nTrackIndex, nTrackOffset, stat32, stat8, pulse, statR, isRunningStatus);
    }

    #endregion
    #region MESSAGE properties/events

    /// MESSAGE properties/events
    public List<MidiEventDelegate> MessageHandlers
    {
      get { return messageHandlers; }
    } List<MidiEventDelegate> messageHandlers = new List<MidiEventDelegate>();

    /// <summary>
    /// This is a place-holder for delegating the parsers primary
    /// parsing function (mechanism).  Its here because during development
    /// there were multiple parser implementations being used and/or
    /// selectively assigned such has become the case currently where
    /// we have <see cref="GetTempoMap(int, int, int)"/> and
    /// <see cref="GetNTrackMessage(int, int, int)"/> which also
    /// includes the dependent parser-handlers such as the (now demarked VST)
    /// <see cref="MIDIMessageVST"/> and derived message types.
    /// 
    /// Presently this is the only handler supplied and yet it
    /// will eventually be privately used, exposing only
    /// MidiMessages in a prospective event handler and
    /// on completion of parsing all tracks, a collection
    /// of events will be accessable.
    /// </summary>
    public MidiEventDelegate MessageHandler = null;

    /// <summary><see cref="MessageHandler"/></summary>
    public event EventHandler<MidiMessageEvent> ProcessMidiMessage;
    #endregion

    /// <inheritdoc/>
    public bool UserDefinedMessageHandler { get; private set; }

    #region .ctor

    /// <summary>
    /// initializes Reader allowing user defined events by default.
    /// </summary>
    public Reader() : this(true, false)
    {
    }

    public Reader(MidiEventDelegate handler, bool generateMessageList=false)
    : this(true, generateMessageList)
    {
      MessageHandlers.Add(handler);
    }

    public Reader(string fileName, bool generateMessageList=false)
    : this(true, generateMessageList)
    {
      MidiFileName = fileName;
    }

    protected Reader(bool userDefinedEvents, bool generateMessageList = false)
    {
      GenerateMessageList = generateMessageList;
      if (UserDefinedMessageHandler = userDefinedEvents) MessageHandler = OnMidiMessage;
    }

    #endregion

    #region ¿¡WHAT?!
    ///	<summary>FIXME: this does not belong!</summary>
    /// <inheritdoc/>
    int GetOffset(int nTrackIndex, int nTrackOffset)
    {
      int result = 14;
      for (int i = 0; i <= nTrackIndex; i++)
      {
        result += FileHandle[i].Data.Length + 8; // if not 8, this will be 4
      }
      return result + nTrackOffset;
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
      ResetTrackTiming();
      FileHandle = default(MThd);
      MidiFileName = null;
      selectedTrackNumber = -1;
      GC.Collect();
    }

    #endregion

    readonly object ParseAllLock = new object();

    #region READ META TRACK

    /// <inheritdoc/>
    public void ParseTempoMap(int nTrackIndex)
    {
      ResetTempoMap();
      long delta_time;
      int i = 0;
      selectedTrackNumber = nTrackIndex; // Sets ReaderIndex
      while (i < FileHandle.Tracks[nTrackIndex].Data.Length)
      {
        i = FileHandle.ReadDelta(nTrackIndex, i, out delta_time);
        CurrentTrackPulse += delta_time;
        i = GetTempoMap(selectedTrackNumber, i, Convert.ToInt32(delta_time));
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

      while (i < FileHandle.Tracks[ReaderIndex].Data.Length)
      {
        i = FileHandle[ReaderIndex].DeltaRead(i, out mSevenBitDelta);
        
        CurrentTrackPulse += mSevenBitDelta;
        i = GetNTrackMessage(ReaderIndex, i, Convert.ToInt32(mSevenBitDelta));
        OnTrackLoadProgressChanged(i);
      }

      OnAfterTrackLoaded(EventArgs.Empty);

      return FileHandle.Tracks[ReaderIndex].Data.Length;
    }
    #region READ (PARSE) ALL TRACKS

    /// <summary>when `IsTrackSelected=true`, provides the total number of ticks in the track.</summary>
    long totlen = 0;

    /// <summary>
    /// This is set during the call to <see cref="ParseAll"/>.  
    /// After a successfull call to the above, this provides
    /// a list containing total number of pulses or ticks in each track (including EOT message).
    /// </summary>
    public List<long> TrackLength { get; private set; } = new List<long>();

    /// <summary>
    /// Parse all tracks to (vst) mididatalist and our internal delegation.
    /// </summary>
    public void ParseAll()
    {
      MidiVSTMessageList.Clear();
      TrackLength.Clear();
      for (int i = 0; i < FileHandle.NumberOfTracks; i++)
      {
        MidiVSTMessageList.CreateKey(i);
        TrackLength.Add(-1);
      }
      CurrentTrackPulse = 0;
      lock (ParseAllLock)
      {
        for (int nTrackIndex = 0; nTrackIndex < FileHandle.NumberOfTracks; nTrackIndex++)
        {
          ResetTrackTiming();
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

    ///<summary>
    /// This is primarily for use in UI.  
    /// Refresh a track into a list or view.
    /// </summary>
    public virtual string TrackSelectAction()
    {
      ResetTrackTiming();

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

    public MThd FileHandle { get; set; }

    #endregion

    // -----------------------------------------------

    #region EVENT clearview,fileloaded,trackchanged,beforetrackload,aftertrackload
    public event CliHandler ClearView;
    protected virtual void OnClearView(CliEvent e)
    {
      if (ClearView != null) ClearView(this, e);
    }

    public event CliHandler BeforeFileLoaded;
    protected virtual void OnBeforeFileLoaded(CliEvent e)
    {
      if (BeforeFileLoaded != null) BeforeFileLoaded(this, e);
    }

    public event CliHandler FileLoaded;
    protected virtual void OnFileLoaded(CliEvent e)
    {
      if (FileLoaded != null) FileLoaded(this, e);
    }

    /// <summary>
    /// This is triggered after the track is parsed.
    /// </summary>
    public event CliHandler BeforeTrackLoaded;
    protected virtual void OnBeforeTrackLoaded(CliEvent e)
    {
      if (BeforeTrackLoaded != null) BeforeTrackLoaded(this, e);
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
      if (TrackChanged != null) TrackChanged(this, e);
    }
    #endregion

    // -----------------------------------------------

    #region STR track info, reader status (caption/tooltip)

    public const string Resource_TrackLoaded =
      "{11} MIDI Track — Format: v{10}, SMPTE: {12}," +
      "Track: {0,3:000},  PPQ: {3}, (first) Tempo: {4}\n" +
      "TSig: {5}/{6} Clocks: {7}, {8} 32nds, KeySig: {9}";

    string StringTrackInfo
    {
      get
      {
        TempoState tempo = TempoMap.Top ?? TempoState.Default;
        return string.Format(
          Resource_TrackLoaded,
          /*  0 */ ReaderIndex,
          /*  1 */ totlen,
          /*  2 */ FileHandle[ReaderIndex].Size,
          /*  3 */ Convert.ToInt32(FileHandle.Division),
          /*  4 */ Convert.ToSingle(tempo.Tempo),
          /*  5 */ TimeSignature.Numerator,
          /*  6 */ TimeSignature.Denominator,
          /*  7 */ TimeSignature.Clocks,
          /*  8 */ TimeSignature.ThirtySeconds,
          /*  9 */ KeySignature,
          /* 10 */ FileHandle.Format,
          /* 11 */ StringRes.STRING_APP_NAME,
          /* 12 */ SMPTE
         );
      }
    }

    #endregion

    // obsolete (not replaced, just absolute junk)
    #region (Failed) Progress Attempt

    public event EventHandler<ProgressChangedEventArgs> TrackLoadProgressChanged;

    protected virtual void OnTrackLoadProgressChanged(int nTrackOffset)
    {
      int progressAmount = (nTrackOffset / FileHandle[selectedTrackNumber].Data.Length) * 100;
      ProgressChangedEventArgs e = new ProgressChangedEventArgs(nTrackOffset, null);
      if (TrackLoadProgressChanged != null) TrackLoadProgressChanged(this, e);
    }

    #endregion


    internal static MidiMsgType GetMidiMessageType(int status, MidiMsgType def = MidiMsgType.ChannelVoice)
    {
      switch (status & 0xF0)
      {
        case 0x80: return MidiMsgType.NoteOff;
        case 0x90: return MidiMsgType.NoteOn;
        case 0xB0: return MidiMsgType.ControllerChange;
        case 0xF0: return MidiMsgType.SystemExclusive;
        case 0xF7: return MidiMsgType.SequencerSpecific;
        // case 12: return MsgType.CC;
        default: return def;
      }
    }


  }
}
