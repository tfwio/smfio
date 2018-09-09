/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
namespace gen.snd.Midi
{
	public class Divisions
	{
		/// <summary>Bar x4</summary>
		public int TicksPerMeasure { get { return TicksPerBar * 4; } }

		/// <summary>Quarter x4</summary>
		public int TicksPerBar { get { return TicksPerQuarterNote * 4; } }

		/// <summary>PPQ (division) x4</summary>
		public int TicksPerNote { get { return TicksPerQuarterNote * 4; } }

		/// <summary>PPQ (division) x4</summary>
		public int TicksPerQuarterNote { get { return Ticks * 4; } }

		/// <summary>Midi header-&gt;Division.</summary>
		public int Ticks { get; set; }
	}
}


