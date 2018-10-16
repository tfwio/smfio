/* Date: 11/12/2005 - Time: 4:19 PM */


using System;
using System.Diagnostics;

namespace on.smfio
{
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
          midiVSTMessageList.AddV(ReaderIndex, new MetaMessageVST(MidiMsgType.MetaStr, pulse, midiMsg32, GetMetaBString(nTrackOffset)));
          break;
        case MidiMsgType.MetaInf:
          byte[] bytes = GetMetaBString(nTrackOffset);
          var midiMsg = new MetaMessageVST(pulse, midiMsg32, bytes);
          midiVSTMessageList.AddV(ReaderIndex, midiMsg);
          break;
        case MidiMsgType.SystemExclusive:
          Debug.WriteLine("Skip System Exclusive Message (for now)");
          break;
        case MidiMsgType.ChannelVoice:
        case MidiMsgType.NoteOff:
        case MidiMsgType.NoteOn:
          midiVSTMessageList.AddV(ReaderIndex, new ChannelMessageVST(pulse, midiMsg32, GetEventValue(nTrackOffset)));
          break;
        case MidiMsgType.SequencerSpecific:
          midiVSTMessageList.AddV(ReaderIndex, new SequencerSpecificVST(pulse, midiMsg32, GetEventValue(nTrackOffset)));
          break;
        case MidiMsgType.EOT:
          midiVSTMessageList.AddV(ReaderIndex, new MetaMessageVST(pulse, midiMsg32));
          break;
        default:
          if (isRunningStatus) MidiVSTMessageList.AddV(ReaderIndex, new ChannelMessageVST(pulse, delta, GetRseEventValue(nTrackOffset)));
          else MidiVSTMessageList.AddV(ReaderIndex, new ChannelMessageVST(pulse, delta, GetEventValue(nTrackOffset)));
          break;
      }
    }

    /// <summary>
    /// <seealso cref="ParseTempoMap(int)"/>
    /// </summary>
    void GetVSTMessageList()
    {
      MidiEventDelegate backup = MessageHandler; // override default message handler.
      MessageHandler = VSTMessageListHandler;      // set it to our vst message-list parser.
      ParseAll();
      TempoMap.Finalize(this);
      MessageHandler = backup;
    }
  }
}
