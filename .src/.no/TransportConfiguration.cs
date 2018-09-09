/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace DspAudio.Midi
{
	public class TransportConfiguration
	{
		/// <summary>
		/// Each MIDI file has within it a Division setting which stores
		/// the number of ticks per quarter note (AKA: TPQN, TPQ, pulses per quarter or PPQ).
		/// </summary> 
		public int Division { get;set; }
		
		/// <summary>
		/// A number starting at 1.  This is the track's start position in ticks.
		/// </summary>
		public double QuarterCycleStart { get;set; }
		/// <summary>
		/// This is the number of quarters that will play until the track loops.
		/// </summary>
		public double QuarterCycleLength   { get;set; }
		/// <summary>
		/// If set to true, when parsing a midi track all notes retain the channel bit when sent to the target plugin.
		/// If false, all channel specific info is simply ignored and mapped to Zero.
		/// </summary>
		public bool IsChannelSpecific { get; set; }
		public double Tempo { get;set; }
		public int Channels { get;set; }
		public double Rate { get;set; }
		public int Rate32 { get { return Convert.ToInt32(Rate); } }
		public float RateF { get { return Convert.ToSingle(Rate); } }
		/// <summary>We'll have to document this in the future…</summary>
		public int Latency { get;set; }
		
		public TimeSignature TSig { get;set; }
	}
}
