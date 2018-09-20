/*
 * Date: 11/12/2005 * Time: 4:19 PM
 */
using System;
using on.smfio.util;

namespace on.smfio
{
  /// <summary>
  /// This was at some time used to prepare MIDI Events for use in (VSTNet) host.
  /// </summary>
  public class MIDIMessage
  {
    // #region Check Range

    /// <summary>
    /// Check sample-range
    /// </summary>
    /// <param name="c">must have Division and PPQ set.</param>
    /// <param name="b">We currently check the end position of the loop.</param>
    /// <param name="min">first sample in search block</param>
    /// <param name="max">last sample in search block</param>
    /// <returns>True if delta-time is contained within sample ranges min and max</returns>
    public bool IsContained(SampleClock c, Loop b, double min, double max)
    {
      double samplePos = c.SolveSamples(Pulse).Samples32;
      return samplePos >= min && samplePos < max && samplePos < b.End;
    }

    /// <summary>
    /// Check sample-range
    /// </summary>
    /// <param name="c">must have Division and PPQ set.</param>
    /// <param name="b">We currently check the end position of the loop.</param>
    /// <returns>True if delta-time is contained within sample ranges min and max</returns>
    public bool IsContained(SampleClock c, Loop b)
    {
      double samplePos = c.SolveSamples(Pulse).Samples32;
      return samplePos >= b.Begin && samplePos < b.End;
    }
    // #endregion

    #region Properties

    public MidiMsgType MessageFlag { get; }

    public byte ChannelBit { get { return Convert.ToByte(Message & 0x000F); } }
    public byte MessageBit { get { return Convert.ToByte(Message & 0x00F0); } }

    public int Message { get; set; }

    /// <summary>Not quite sure this is the best name for this guy.</summary>
    public long Pulse { get; set; }

    public byte[] Data { get; set; }

    #endregion

    /// <summary>
    /// </summary>
    /// <param name="pMsgType">Typically 8, 9, A, B and C</param>
    /// <param name="pPulse">Pulses (timing foundation)</param>
    /// <param name="pIntMessage">Must be a length of 4 bytes.</param>
    /// <param name="pMsgData">Must be a length of 4 bytes.</param>
    public MIDIMessage(MidiMsgType pMsgType, long pPulse, int pIntMessage, params byte[] pMsgData)
    {
      Pulse = pPulse;
      MessageFlag = pMsgType;
      Message = pIntMessage;
      Data = pMsgData;
    }
  }

}
