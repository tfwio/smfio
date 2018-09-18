/*
 * User: xo
 * Date: 9/8/2018
 * Time: 3:38 AM
 */
using System;
using on.smfio;
using on.smfio.Common;
using on.smfio.util;
namespace SMFIOViewer
{
	public class TimeConfiguration : ITimeConfiguration
	{
		/// <summary>
		/// to be marked as internal
		/// </summary>
		public static readonly TimeConfiguration Instance = new TimeConfiguration() {
			// AUDIO
			Rate = 44100,
			Channels = 2,
			Latency = 8096,
			// MIDI
			Division = 480,
			Tempo = 120,
			TimeSignature = new MidiTimeSignature(4, 4, 24, 4),
			KeySignature = new MidiKeySignature(KeySignatureType.C, true),
			IsSingleZeroChannel = false
		};

		public void FromMidi(IMidiParser parser)
		{
			this.TimeSignature = parser.TimeSignature;
			this.KeySignature = parser.KeySignature;
			this.Division = parser.SmfFileHandle.Division;
			this.Tempo = parser.TempoMap.Top.Tempo;
		}

		public int Channels {
			get;
			set;
		}

		public int Rate {
			get;
			set;
		}

		public float RateF {
			get {
				return Convert.ToSingle(Rate);
			}
		}

		public int Latency {
			get;
			set;
		}

		/// <summary>
		/// Each MIDI file has within it a Division setting which stores
		/// the number of ticks per quarter note (AKA: TPQN, TPQ, pulses per quarter or PPQ).
		/// </summary>
		public int Division {
			get;
			set;
		}

		public double Tempo {
			get;
			set;
		}

		public bool IsSingleZeroChannel {
			get;
			set;
		}

		public MidiKeySignature KeySignature {
			get;
			set;
		}

		public MidiTimeSignature TimeSignature {
			get;
			set;
		}

		double barStart;

		public double BarStart {
			get {
				return barStart;
			}
			set {
				barStart = value;
			}
		}

		double barLength = 4;

		public double BarLength {
			get {
				return barLength;
			}
			set {
				barLength = value;
			}
		}

		public double BarStartPulses {
			get {
				return barStartPulsesPerQuarter;
			}
			set {
				barStartPulsesPerQuarter = value;
			}
		}

		double barStartPulsesPerQuarter = 4;

		public double BarLengthPulses {
			get {
				return barLengthPulsesPerQuarter;
			}
			set {
				barLengthPulsesPerQuarter = value;
			}
		}

		double barLengthPulsesPerQuarter = 4;
	}
}




