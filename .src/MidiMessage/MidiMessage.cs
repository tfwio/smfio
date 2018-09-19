/*
 * Date: 11/12/2005 * Time: 4:19 PM
 */
using System;
using on.smfio.util;

namespace on.smfio
{
  public interface ISampleClock
  {
    double Samples { get; set; }
    int Samples32 { get; }
    int Samples32Floor { get; }
    long Pulse { get; set; }

    SampleClock SolveSamples(long pulse);
    SampleClock SolveSamples(long pulse, ITimeConfiguration settings);
    SampleClock SolvePPQ(long sampleOffset, ITimeConfiguration settings);
  }
  public class SampleClock : ISampleClock
  {
    const double Minute_Hex = 60000000.0;
    const double mus = 0.00001;
    const int micro = 100000;
    const double s60 = 60.0;

    public SampleClock() { }
    public SampleClock(ITimeConfiguration conf)
    {
      Fs = conf.Rate;
      Division = conf.Division;
      Tempo = conf.Tempo; // FIXME: ITimeConfiguration should use muspqn.
    }

    public double Frame { get { return Pulse / Division; } }

    public long Pulse
    {
      get { return mPulse; }
      set { mPulse = value; mSample = TimeUtil.GetSamples(mPulse, Tempo, Fs, Division); }
    }
    long mPulse;
    public int Samples32Floor { get { return Convert.ToInt32(Math.Floor(Samples)); } }
    public int Samples32 { get { return Convert.ToInt32(Samples); } }
    public double Samples
    {
      get { return mSample; }
      set { Pulse = (long)((mSample = value) / (60.0 / Tempo * Fs) * Division); }
    }
    double mSample;

    public int Fs { get; set; } = 44100;
    public short Division { get; set; } = 120;
    /// <summary>microseconds per quarter note</summary>
    public ushort MSPQN { get; set; } = 50000;


    /// <summary>
    /// We don't reccomend using this setter.
    /// 
    /// Set MSPQN (microseconds per quarter note) and request the tempo.
    /// </summary>
    public double Tempo
    {
      get { return Minute_Hex / MSPQN; }
      set { MSPQN = (ushort)Math.Floor((s60 / value) * micro); }
    }

    public double PulsesPerPPQDivision { get { return (double)Division / 24; } }
    public double ClocksAtPosition { get { return Pulse / PulsesPerPPQDivision; } }

    public double SPQN { get { return 60.0 / Tempo; } }
    public double SamplesPerQuarter { get { return SPQN * Fs; } }

    public double QuarterHelper { get { return (QuartersOffset / Division); } }
    public double QuarterHelperInhibited { get { return QuartersOffsetInQuarters1 / Division; } }
    public double QuartersOffsetInQuarters1 { get { return QuartersOffset % 4; } }
    public double QuartersOffset { get { return (double)Samples / SamplesPerQuarter; } }
    public double Quarter { get { return QuartersOffsetInQuarters1 + 1; } }

    public bool IsQuarterOffsetInTicks { get; set; }
    public double Measure { get { return (QuartersOffset / 16).Floor() + 1; } }
    public double Bar { get { return ((QuartersOffset / 4).Floor() % 4) + 1; } }
    public double Tick { get { return (Division * (QuarterHelperInhibited * Division)) % Division; } }

    public SampleClock SolvePPQ(long samples, int fs, double tempo, short division, bool inTicks = true)
    {
      Fs = fs;
      Tempo = tempo;
      Division = division;
      IsQuarterOffsetInTicks = inTicks;
      Samples = samples;
      return this;
    }
    public SampleClock SolvePPQ(long samples, int fs, ushort muspqn, short division, bool inTicks = true)
    {
      Fs = fs;
      MSPQN = muspqn;
      Division = division;
      IsQuarterOffsetInTicks = inTicks;
      Samples = samples;
      return this;
    }
    public SampleClock SolvePPQ(long sampleOffset, ITimeConfiguration conf)
    {
      // this.Pulse = TimeUtil . FromSamples(Fs, sampleOffset);
      SolvePPQ(sampleOffset, conf.Rate, conf.Tempo, conf.Division, true);
      return this;
    }

    public SampleClock SolveSamples(long pulse)
    {
      this.Pulse = pulse;
      return this;
    }
    public SampleClock SolveSamples(long pulse, ITimeConfiguration settings)
    {
      this.Pulse = pulse;
      return this;
    }
    public SampleClock SolveSamples(long pulse, int fs, ushort muspqn, short division, bool inTicks = true)
    {
      Pulse = pulse;
      Fs = fs;
      MSPQN = muspqn;
      Division = division;
      IsQuarterOffsetInTicks = inTicks;
      return this;
    }
    public SampleClock SolveSamples(long pulse, int fs, double tempo, short division, bool inTicks = true)
    {
      Pulse = pulse;
      Fs = fs;
      Tempo = tempo;
      Division = division;
      IsQuarterOffsetInTicks = inTicks;
      return this;
    }

    /// <inheritdoc/>
    string MeasureStringFormat { get { return IsQuarterOffsetInTicks ? Strings.ms_mbqt : Strings.ms_mbt; } }

    /// <inheritdoc/>
    public string MeasureString { get { return string.Format(MeasureStringFormat, Measure, Bar, Math.Floor(Tick), Math.Floor(Quarter)); } }


    /// <inheritdoc/>
    public TimeSpan Time
    {
      get { return TimeSpan.FromSeconds(Samples / Fs); }
    }

    /// <inheritdoc/>
    public string TimeString
    {
      get
      {
        TimeSpan t = Time;
        return string.Format(
          "{0:00}:{1:00}:{2:00}.{3:000}",
          t.Hours,
          t.Minutes,
          t.Seconds,
          t.Milliseconds);
      }
    }


    public SampleClock Copy()
    {
      return new SampleClock
      {
        Division = Division,
        Samples = Samples,
        MSPQN = MSPQN,
        Fs = Fs,
      };
    }

  }
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
