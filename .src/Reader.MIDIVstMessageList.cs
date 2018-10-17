/* Date: 11/12/2005 - Time: 4:19 PM */
using System;
using System.Diagnostics;
namespace on.smfio
{
  /// <summary>
  ///   
  /// **Reader.MIDIVstMessageList.cs**  
  /// This file (partial class implementation) represents a particular
  /// use-case as found in "modest vstsmfui" which is a strange brute-force,
  /// prototype VSTHost implementation lacking tempo-changes and many other features.
  /// 
  /// The implementation was (yet still at this time) lacking various
  /// needed characteristics.  One major reason it was written was to test
  /// out weather or not I could use this parser as a VSTHost implementation.
  /// 
  /// This remains here so that 'modest' can be migrated out of these features
  /// and into the newer MidiMessage semantic where we collect midi message events
  /// in a very general form such as <see cref="MidiMessage"/>.
  /// </summary>
  public partial class Reader
  {
    /// <inheritdoc/>
    public DictionaryList<int, MIDIMessageVST> MidiVSTMessageList {
      get { return midiVSTMessageList; }
      set { midiVSTMessageList = value; }
    } DictionaryList<int, MIDIMessageVST> midiVSTMessageList = new DictionaryList<int, MIDIMessageVST>();

    /// <summary>
    /// This should be explicitly be set to TRUE if the VST Message List
    /// is to be populated.
    /// </summary>
    public bool GenerateVSTMessageList { get; set; }

    void VSTMessageListHandler(MidiMsgType msgType, int nTrackIndex, int nTrackOffset, int midiMsg32, byte midiMsg8, long pulse, int delta, bool isRunningStatus)
    {
      switch (msgType)
      {
        case MidiMsgType.MetaStr:
          MidiVSTMessageList.AddV(ReaderIndex, new MetaMessageVST(MidiMsgType.MetaStr, pulse, midiMsg32, GetMetaBString(nTrackOffset)));
          break;
        case MidiMsgType.MetaInf:
          byte[] bytes = GetMetaBString(nTrackOffset);
          var midiMsg = new MetaMessageVST(pulse, midiMsg32, bytes);
          MidiVSTMessageList.AddV(ReaderIndex, midiMsg);
          break;
        case MidiMsgType.SystemExclusive:
          Debug.WriteLine("Skip System Exclusive Message (for now)");
          break;
        case MidiMsgType.ChannelVoice:
        case MidiMsgType.NoteOff:
        case MidiMsgType.NoteOn:
          MidiVSTMessageList.AddV(ReaderIndex, new ChannelMessageVST(pulse, midiMsg32, GetEventValue(nTrackOffset)));
          break;
        case MidiMsgType.SequencerSpecific:
          MidiVSTMessageList.AddV(ReaderIndex, new SequencerSpecificVST(pulse, midiMsg32, GetEventValue(nTrackOffset)));
          break;
        case MidiMsgType.EOT:
          MidiVSTMessageList.AddV(ReaderIndex, new MetaMessageVST(pulse, midiMsg32));
          break;
        default:
          if (isRunningStatus) MidiVSTMessageList.AddV(ReaderIndex, new ChannelMessageVST(pulse, delta, GetRseEventValue(nTrackOffset)));
          else MidiVSTMessageList.AddV(ReaderIndex, new ChannelMessageVST(pulse, delta, GetEventValue(nTrackOffset)));
          break;
      }
    }

    /// <summary>
    /// This is for the most part a stand-alone parser.
    /// 
    /// <seealso cref="ParseTempoMap(int)"/>
    /// </summary>
    void GetVSTMessageList(string smfFilePath)
    {
      ResetTempoMap();
      FileHandle = new chunk.MThd(smfFilePath);
      ParseTempoMap(0);

      // override default message handler.
      MidiEventDelegate backup = MessageHandler;
      MessageHandler = VSTMessageListHandler;

      ParseAll();
      TempoMap.Finalize(this);

      // put back the message handler.
      MessageHandler = backup;
    }
  }
}
