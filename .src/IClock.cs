/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio.util
{
  public interface IMidiTiming
  {
		/// <summary>
		/// typically multiples of 24;
		/// EG: 48, 72, 96, 120, 144, 168, 192, 216, 240, …, 360, 384, …,480, …
		/// </summary>
		/// <seealso cref="IMidiClock.MSPQN"/>
    int    Division             { get; set; }
    
    /// <summary>
    /// </summary>
    /// <remarks>
    /// Unlike various other settings in our midi-clock (interface),
    /// Tempo is likely to change; Therefore, we are going to need
    /// to (not implemented) store a list of tempos at a given offset
    /// in samples, and translate our midi-pulse-domain to/from sample-domain.
    /// </remarks>
    double Tempo                { get; set; }
  }
  
  public interface IMidiClock
  {
    /// <summary>
    /// typically: <code>Division / 24</code>
    /// </summary>
    /// <seealso cref="MSPQN"/>
    double PulsesPerPPQDivision { get; }

    /// <summary>
    /// <strong>Milliseconds per quarter-note</strong>
    /// Calculated by way of the number of <see cref="IMidiTiming.Division">divisions</see>
    /// in a given quarter-note segment (<see cref="PulsesPerPPQDivision"/>).
    /// </summary>
    /// <seealso cref="IAudioClock.SamplesFromPulses(double,double,int,int)" />
    double MSPQN                { get; }
  }
  /// <summary>
  /// Clock: Measure, bar, quarter, tick, pulse &amp; frame
  /// </summary>
  public interface IMidiClock_MBQTPF
  {
    /// <summary>
    /// M:B:T (Measure);
    /// <code>(QuartersOffset / 16).Floor() + 1</code>
    /// </summary>
    double Measure  { get; }
    
    /// <summary>
    /// M:B:T (Bar);
    /// <code>V / 4 % 4 … +1</code></summary>
    double Bar      { get; }
    
    /// <summary>
    /// <code>( D * ( ( ( ( S / ( ( 60.0 / T ) * R ) ) % 4 )  / D) * D ) ) % D</code>
    /// <para>
    /// M:B:T (Ticks);
    /// Value is S; where S=Samples, D=Division, T=Tempo, R=Rate
    /// </para>
    /// </summary>
    double Tick     { get; }
    
    double Pulses   { get; set; }
    
    double Frame    { get; }
    double Quarter  { get; }
    double Quarters { get; }
  }
  
  public interface IAudioClock
  {
    /// <summary>SampleRate</summary>
    int Rate { get; set; }

    /// <summary>
    /// provided sample position;
    /// Set explicity or indirectly by <see cref="IMidiClock_MBQTPF.Pulses"/>.
    /// </summary>
    /// <seealso cref="Samples32"/>
    /// <seealso cref="Samples32Floor"/>
    /// <remarks>
    /// When setting samples, we immediately use it so solve
    /// <pre>pulses = samples / (60 / Tempo * Rate) * Division;</pre>
    /// </remarks>
    double Samples { get; set; }

    /// <summary>Number of elapsed samples.</summary>
    /// <seealso cref="Samples"/>
    /// <seealso cref="Samples32Floor"/>
    int Samples32 { get; }
    
    /// <summary>Floored</summary>
    /// <seealso cref="Samples"/>
    /// <seealso cref="Samples32"/>
    int Samples32Floor { get; }
		
    /// <summary>SecondsPerQuarter * Rate</summary>
    double SamplesPerQuarter { get; }
		
    // TODO: double-check documentation.
    /// <summary>?</summary>
    /// <seealso cref="SamplesFromPulses(double,double,int,int)"/>
    double SamplesPerClock { get; }
    
    // 
    // Solve samples
    // ----------------------
    
    /// <summary><code>(60 / tempo * rate) * (p / division)</code></summary>
    /// <param name="p">tick position</param>
    /// <param name="tempo"></param>
    /// <param name="rate"></param>
    /// <param name="division"></param>
    /// <returns></returns>
    double SamplesFromPulses(ulong p, double tempo, int rate, int division);
    
    /// <summary><code>(60 / tempo * rate) * (p / division)</code></summary>
    /// <param name="p">tick position</param>
    /// <param name="tempo"></param>
    /// <param name="rate"></param>
    /// <param name="division"></param>
    /// <returns></returns>
    double SamplesFromPulses(double p, double tempo, int rate, int division);
    
  }
  
  public interface IClock : IMidiClock, IMidiTiming, IMidiClock_MBQTPF, IAudioClock
  {
    /// <summary>TODO: Documentation</summary>
    double ClocksAtPosition          { get; }
    
    /// <summary>TODO: Documentation</summary>
    double ClocksAtPositionFloor     { get; }
    
    /// <summary>TODO: Documentation</summary>
    double SPQN                      { get; }
		
    /// <summary>TODO: Documentation</summary>
    TimeSpan Time                    { get; }
    
    /// <summary>TODO: Documentation</summary>
    string TimeString                { get; }
		
    /// <summary>
    /// QuartersOffsetInQuarters (Zero inclusive)
    /// </summary>
    double QuarterHelper             { get; }
    
    /// <summary>
    /// QuartersOffsetInQuarters (Zero inclusive)
    /// </summary>
    double QuarterHelperInhibited    { get; }
    
    /// <summary>
    /// This provides a number from 0 to a maxumum value &lt; 4.
    /// Use QuarterOffset for a value that is not limited to a boundary &lt; 4.
    /// </summary>
    double QuartersOffsetInQuarters1 { get; }
    
    /// <summary>
    /// The number of elapsed quarters.  Calculated from Samples / SamplesPerQuarter
    /// where SamplesPerQuarter = SecondsPerQuarter * Rate and SecondsPerQuarter is
    /// Tempo / SampleRate.
    /// </summary>
    /// <remarks>
    /// A value of 1 is in quarter notes would translate to 480 if we're using 480ppq.
    /// To be clear, 1ppq in this variable would be stored as 1.
    /// </remarks>
    double QuartersOffset { get; }
    
    /// <summary>
    /// UI (MBT string) helper.
    /// </summary>
    bool IsQuarterOffsetInTicks { get; set; }
    
    // 
    // Helper methods
    // ----------------------
		
    /// <summary>TODO: Document</summary>
    SampleClock SolvePPQ(double samples);
    
    /// <summary>TODO: Document</summary>
    
    SampleClock SolvePPQ(double samples, int rate, double tempo, int division);
    
    /// <summary>TODO: Document</summary>
    
    SampleClock SolvePPQ(double samples, int rate, double tempo, int division, bool inTicks);
    
    /// <summary>TODO: Document</summary>
    SampleClock SolvePPQ(double samples, ITimeConfiguration config);
		
    /// <summary>TODO: Document</summary>
    SampleClock SolveSamples(double pulses);
    
    /// <summary>TODO: Document</summary>
    SampleClock SolveSamples(double pulses, int rate, double tempo, int division, bool inTicks);
    
    /// <summary>TODO: Document</summary>
    SampleClock SolveSamples(double pulses, ITimeConfiguration config);
    
    // 
    // UI helper methods
    // ----------------------
    
    string MeasureString { get; }
    
    // 
    // General helper methods
    // ----------------------
    
    /// <summary>
    /// (was SampleClock) abstract class should virtualize this and return an object
    /// so that derived classes may implement.
    /// </summary>
    object Clone();
    
    /// <summary>
    /// An overload of <see cref="Clone"/> with implicit <see cref="Type"/>.
    /// </summary>
    T Clone<T>() where T:class,IClock;
  }
}
