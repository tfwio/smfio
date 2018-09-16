using System;
namespace on.smfio
{
  /// <summary></summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public delegate void MidiMessageHandler(object sender, MidiMessageEvent e);
  /// <summary></summary>
  /// <param name="track"></param>
  /// <param name="offset"></param>
  /// <returns></returns>
  public delegate int MidiReaderLoadTrackDelegate(int track, int offset);

  /// <summary>
  /// A callback to help parsing incoming (parser) messages.
  /// </summary>
  /// <param name="msgType"></param>
  /// <param name="nTrackIndex"></param>
  /// <param name="nTrackOffset"></param>
  /// <param name="midiMsg32"></param>
  /// <param name="midiMsg8"></param>
  /// <param name="pulse"></param>
  /// <param name="runningStatusOrDelta"></param>
  /// <param name="isRunningStatus"></param>
  public delegate void MidiEventDelegate(
    MidiMsgType msgType,
    int nTrackIndex,
    int nTrackOffset,
    int midiMsg32,
    byte midiMsg8,
    ulong pulse,
    int runningStatusOrDelta,
    bool isRunningStatus);
}