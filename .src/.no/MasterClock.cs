/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace DspAudio.Midi
{
	public class MasterClock
	{
		const int ClockBase = 60000000;
		/// <summary>
		/// get/set Tempo.  Internally, QuarterNotesPerSecond is set.
		/// </summary>
		public double Tempo { get { return ClockBase / QuarterNotesPerSecond; } set { QuarterNotesPerSecond = SetTempo(value); } }
		/// <summary>
		/// To set use, set <see cref="Tempo" />.
		/// </summary>
		public float TempoF { get { return Convert.ToSingle(Tempo); } }
		
		int SetTempo(double value) {
			return Convert.ToInt32(ClockBase / value);
		}
		
		/// <summary>
		/// 60,000,000 / 500,000 = 120; Hence 500,000 parts/ticks
		/// per quarter equates to 120 Beats per measure.
		/// </summary>
		public int QuarterNotesPerSecond {
			get { return quarterNotesPerSecond; }
			set { quarterNotesPerSecond = value; }
		} int quarterNotesPerSecond;
		
		static public double TicksToSeconds(MasterClock clock, long ticks)
		{
			int quart = Convert.ToInt32(ticks / clock.QuarterNotesPerSecond);
			return 0;
		}
		
		public MasterClock(long tpqn)
		{
			
		}
	}
}
