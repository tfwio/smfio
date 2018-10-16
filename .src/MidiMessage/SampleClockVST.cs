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
    double Frame { get; }
    /// <summary>
    /// <code>( D * ( ( ( ( S / ( ( 60.0 / T ) * R ) ) % 4 )  / D) * D ) ) % D</code>
    /// <para>
    /// Value is S; where S=Samples, D=Division, T=Tempo, R=Rate
    /// </para>
    /// </summary>
    double Tick     { get; }

    SampleClockVST SolveSamples(long pulse);
    SampleClockVST SolveSamples(long pulse, ITimeConfiguration settings);
    SampleClockVST SolvePPQ(long sampleOffset, ITimeConfiguration settings);
  }
  public class SampleClockVST : ISampleClock
  {
    public SampleClockVST() { }
    public SampleClockVST(ITimeConfiguration conf)
    {
      Fs = conf.Rate;
      Division = conf.Division;
      //Tempo = conf.Tempo; // FIXME: ITimeConfiguration should use muspqn.
      MusPQN = conf.MusPQN;
    }
    public double Frame { get { return Pulse / Division; } }
    
    public long Pulse
    {
      get { return mPulse; }
      // set { mPulse = value; mSample = TimeUtil.GetSamples(mPulse, Tempo, Fs, Division); }
      set { mPulse = value; mSample = SamplesFromPulses(mPulse, Tempo, Fs, Division); }
    } long mPulse;
    
    /// <summary>
    /// <code>( D * ( ( ( ( S / ( ( 60.0 / T ) * R ) ) % 4 )  / D) * D ) ) % D</code>
    /// <para>
    /// Value is S; where S=Samples, D=Division, T=Tempo, R=Rate
    /// </para>
    /// </summary>
    public double Tick { get { return (Division * (QuarterHelperInhibited * Division)) % Division; } }
    public int Samples32Floor { get { return Convert.ToInt32(Math.Floor(Samples)); } }
    public int Samples32 { get { return Convert.ToInt32(Samples); } }
    public double Samples
    {
      get { return mSample; }
      set { Pulse = (long)((mSample = value) / (TimeUtil.s60 / Tempo * Fs) * Division); }
    } double mSample;

    public int Fs { get; set; } = 44100;
    public short Division { get; set; } = 120;
    /// <summary>microseconds per quarter note</summary>
    public uint MusPQN { get; set; } = 500000;

    /// <summary>
    /// We don't reccomend using this setter.
    /// 
    /// Set MSPQN (microseconds per quarter note) and request the tempo.
    /// </summary>
    public double Tempo
    {
      get { return TimeUtil.MicroMinute / MusPQN; }
      set { MusPQN = (uint)Math.Floor((TimeUtil.s60 / value) * TimeUtil.MicroSecond); }
    }

    public double PulsesPerPPQDivision { get { return (double)Division / 24; } }
    public double ClocksAtPosition { get { return Pulse / PulsesPerPPQDivision; } }

    public double SPQN { get { return TimeUtil.s60 / Tempo; } }
    public double SamplesPerQuarter { get { return SPQN * Fs; } }

    public double QuarterHelper { get { return (QuartersOffset / Division); } }
    public double QuarterHelperInhibited { get { return QuartersOffsetInQuarters1 / Division; } }
    public double QuartersOffsetInQuarters1 { get { return QuartersOffset % 4; } }
    public double QuartersOffset { get { return (double)Samples / SamplesPerQuarter; } }
    public double Quarter { get { return QuartersOffsetInQuarters1 + 1; } }

    public bool IsQuarterOffsetInTicks { get; set; }
    public double Measure { get { return (QuartersOffset / 16).Floor() + 1; } }
    public double Bar { get { return ((QuartersOffset / 4).Floor() % 4) + 1; } }

    public double SamplesFromPulses(long p, double tempo, int rate, int division) {
      return SamplesFromPulses(Convert.ToDouble(p), tempo, rate, division);
    }
    public double SamplesFromPulses(double p, double tempo, int rate, int division) {
      return (TimeUtil.s60 / tempo * rate) * (p / division);
    }

    public SampleClockVST SolvePPQ(long samples, int fs, double tempo, short division, bool inTicks = true)
    {
      Fs = fs;
      Tempo = tempo;
      Division = division;
      IsQuarterOffsetInTicks = inTicks;
      Samples = samples;
      return this;
      //return this.Copy();
    }
    public SampleClockVST SolvePPQ(long samples, int fs, uint muspqn, short division, bool inTicks = true)
    {
      Fs = fs;
      MusPQN = muspqn;
      Division = division;
      IsQuarterOffsetInTicks = inTicks;
      Samples = samples;
      return this;
      //return this.Copy();
    }
    public SampleClockVST SolvePPQ(long sampleOffset, ITimeConfiguration conf)
    {
      Fs = conf.Rate;
      MusPQN = conf.MusPQN;
      Division = conf.Division;
      IsQuarterOffsetInTicks = true;
      Samples = sampleOffset;
      return this;
    }

    public SampleClockVST SolveSamples(long pulse)
    {
      this.Pulse = pulse;
      //return this.Copy();
      return this;
    }
    public SampleClockVST SolveSamples(long pulse, ITimeConfiguration settings)
    {
      this.Pulse = pulse;
      this.Division = settings.Division;
      this.Fs = settings.Rate;
      this.MusPQN = settings.MusPQN;
      return this;
      //return this.Copy();
    }
    public SampleClockVST SolveSamples(long pulse, int fs, uint muspqn, short division, bool inTicks = true)
    {
      Pulse = pulse;
      Fs = fs;
      MusPQN = muspqn;
      Division = division;
      IsQuarterOffsetInTicks = inTicks;
      return this;
      //return this.Copy();
    }
    public SampleClockVST SolveSamples(long pulse, int fs, double tempo, short division, bool inTicks = true)
    {
      Pulse = pulse;
      Fs = fs;
      Tempo = tempo;
      Division = division;
      IsQuarterOffsetInTicks = inTicks;
      return this;
      //return this.Copy();
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

    public SampleClockVST Copy()
    {
      return new SampleClockVST
      {
        Division = Division,
        Fs = Fs,
        MusPQN = MusPQN,
        Samples = Samples,
        Pulse = Pulse,
        IsQuarterOffsetInTicks = IsQuarterOffsetInTicks,
      };
    }

  }
}


