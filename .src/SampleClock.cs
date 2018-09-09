/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio.util
{
  /// <summary>
  /// <para>
  /// The SampleClock is ignorant to number of channels.
  /// Aside form that, it can be used to determine MBQT
  /// from Sample-Position and (maybe) vice-vs. (I hadn't
  /// gone over this in a while...)
  /// </para><para>
  /// We are missing tempo change locations here.
  /// Sample-accurate timing  should be stored in IList or such,
  /// yet how are we to index the data?
  /// </para>
  /// <code>Dictionary&lt;long,ushort></code>
  /// <code>Dictionary&lt;ticks,tempo></code>
  /// </summary>
  public partial class SampleClock : IClock
  {

    const double Minute_Hex = 60000000.0;
    
    // =========================================
    // IAudioClock
    // =========================================
    
    /// <inheritdoc/>
    public int Rate { get; set; }
    
    /// <inheritdoc/>
    public double Samples {
      get { return samples; }
      set { pulses = (samples = value) / (60 / Tempo * Rate) * Division; }
    } double samples;
    
    /// <inheritdoc/>
    public int Samples32 { get { return Convert.ToInt32(samples); } }
    
    /// <inheritdoc/>
    public int Samples32Floor { get { return Convert.ToInt32(Math.Floor(samples)); } }

    // =========================================
    // IMidiClock
    // =========================================
    
    /// <inheritdoc/>
    public int Division { get; set; }

    /// <inheritdoc/>
    public double PulsesPerPPQDivision { get { return Division / 24; } }
    
    /// <inheritdoc/>
    public double MSPQN { get { return Minute_Hex / Tempo; } }
    
    // =========================================
    // IAudioClock
    // =========================================
    
    /// <inheritdoc/>
    public double SamplesPerClock { get { return SamplesFromPulses(PulsesPerPPQDivision, Tempo, Rate, Division); } }
    
    /// <inheritdoc/>
    public double ClocksAtPosition { get { return Pulses / PulsesPerPPQDivision; } }
    
    /// <inheritdoc/>
    public double ClocksAtPositionFloor { get { return ClocksAtPosition.Floor(); } }
    
    // =========================================
    // IClock
    // =========================================
    
    /// <inheritdoc/>
    public double Tempo { get; set; }

    // ------------------
    // Properties: Read Only; Samples|Seconds Per Quarter
    // ------------------
    
    /// <inheritdoc/>
    public double SPQN { get { return 60.0 / Tempo; } }
    
    /// <inheritdoc/>
    public TimeSpan Time {
      get { return TimeSpan.FromSeconds(Samples / Rate); }
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
    
    /// <inheritdoc/>
    public double SamplesPerQuarter { get { return SPQN * Rate; } }

    /// <inheritdoc/>
    public double QuarterHelper {
      get { return (QuartersOffset / Division); }
    }

    /// <inheritdoc/>
    public double QuarterHelperInhibited { get { return QuartersOffsetInQuarters1 / Division; } }

    /// <inheritdoc/>
    public double QuartersOffsetInQuarters1 { get { return QuartersOffset % 4; } }

    /// <inheritdoc/>
    public double QuartersOffset { get { return Samples / SamplesPerQuarter; } }

    /// <inheritdoc/>
    string MeasureStringFormat { get { return IsQuarterOffsetInTicks ? Strings.ms_mbqt : Strings.ms_mbt; } }

    /// <inheritdoc/>
    public string MeasureString { get { return string.Format(MeasureStringFormat, Measure, Bar, Math.Floor(Tick), Math.Floor(Quarter)); } }


    // MBQT

    /// <inheritdoc/>
    public bool IsQuarterOffsetInTicks { get; set; }
    
    // IMidiClock.IMidiClock_MBQTPF
    
    /// <inheritdoc/>
    public double Measure { get { return (QuartersOffset / 16).Floor() + 1; } }
    
    /// <inheritdoc/>
    public double Bar { get { return ((QuartersOffset / 4).Floor() % 4) + 1; } }
    
    /// <inheritdoc/>
    public double Tick { get { return (Division * (QuarterHelperInhibited * Division)) % Division; } }
    
    /// See: <see cref="SamplesFromPulses(double,double,int,int)"/>
    public double Pulses {
      get { return pulses; }
      set { pulses = value; samples = SamplesFromPulses(pulses, Tempo, (int)Rate, (int)Division); }
    } double pulses;

    /// <inheritdoc/>
    public double Frame { get { return Pulses / Division; } }

    /// <inheritdoc/>
    public double Quarter { get { return QuartersOffsetInQuarters1 + 1; } }

    /// <summary>This one depends on IsQuarterOffsetInTicks</summary>
    public double Quarters { get { return (IsQuarterOffsetInTicks ? Tick : Quarter); } }

    // ------------------
    // PULSES
    // ------------------
    
    /// <inheritdoc/>
    public double SamplesFromPulses(ulong p, double tempo, int rate, int division) {
      return SamplesFromPulses(Convert.ToDouble(p), tempo, rate, division);
    }
    
    /// <inheritdoc/>
    public double SamplesFromPulses(double p, double tempo, int rate, int division) {
      return (60 / tempo * rate) * (p / division);
    }
    
    // ------------------
    // PPQ
    // ------------------
    
    /// <inheritdoc/>
    public SampleClock SolvePPQ(double samples)
    {
      Samples = samples;
      return this;
    }
    
    /// <inheritdoc/>
    public SampleClock SolvePPQ(double samples, int rate, double tempo, int division)
    {
      return SolvePPQ(samples, rate, tempo, division, true);
    }
    
    /// <inheritdoc/>
    public SampleClock SolvePPQ(double samples, int rate, double tempo, int division, bool inTicks)
    {
      Rate                   = rate;
      Tempo                  = tempo;
      Division               = division;
      IsQuarterOffsetInTicks = inTicks;
      Samples                = samples;
      return this;
    }
    
    /// <inheritdoc/>
    public SampleClock SolvePPQ(double samples, ITimeConfiguration config)
    {
      Rate                   = config.Rate;
      Tempo                  = config.Tempo;
      Division               = config.Division;
      IsQuarterOffsetInTicks = true;
      Samples                = samples;
      return this;
    }
    
    // Samples
    
    /// <inheritdoc/>
    public int GetSamples32()
    {
      throw new NotImplementedException();
    }
    
    /// <inheritdoc/>
    public SampleClock SolveSamples(double pulses)
    {
      Pulses = pulses;
      return this;
    }
    
    /// <inheritdoc/>
    public SampleClock SolveSamples(double pulses, int rate, double tempo, int division, bool inTicks)
    {
      Rate = rate;
      Tempo = tempo;
      Division = division;
      IsQuarterOffsetInTicks = inTicks;
      Pulses = pulses;
      return this;
    }

    /// <inheritdoc/>
    public SampleClock SolveSamples(double pulses, ITimeConfiguration config)
    {
      Rate = config.Rate;
      Tempo = config.Tempo;
      Division = config.Division;
      IsQuarterOffsetInTicks = true;
      Pulses = pulses;
      return this;
    }
    
    public SampleClock() {}
    
    public SampleClock(ITimeConfiguration config)
    {
      Rate = config.Rate;
      Division = config.Division;
      Tempo = config.Tempo;
    }

    /// <inheritdoc/>
    public SampleClock(double samples, int rate, double tempo, int division)
      : this(samples, rate, tempo, division, true) {}
    
    /// <inheritdoc/>
    public SampleClock(double samples, int rate, double tempo, int division, bool inTicks)
    { SolvePPQ(samples, rate, tempo, division, inTicks); }

    public static SampleClock Create(double pulses, int rate, double tempo, int division)
    {
      var clock = new SampleClock(0, rate, tempo, division, true);
      return clock.SolveSamples(pulses);
    }
    
    /// <inheritdoc/>
    public object Clone()
    {
      return new SampleClock(this.samples, this.Rate, this.Tempo, this.Division, this.IsQuarterOffsetInTicks);
    }
    
    /// <inheritdoc/>
    public T Clone<T>() where T:class,IClock
    {
      return (T)(object)(new SampleClock(this.samples, this.Rate, this.Tempo, this.Division, this.IsQuarterOffsetInTicks));
    }
  }
}
