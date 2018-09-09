/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{

	public class MidiNote : MidiData
	{
		/// <summary>
		/// key number.
		/// </summary>
		public byte K;
		/// <summary>
		/// Key depressed length in ticks
		/// </summary>
		public int Len;
		/// <summary>
		/// Velocity or Volume for 1: ‘Note ON’ and 2: “Note OFF”.
		/// </summary>
		public short V1, V2;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="c">Channel (Nullable)—or is this color?</param>
		/// <param name="k"></param>
		/// <param name="s"></param>
		/// <param name="v1"></param>
		public MidiNote(byte? c, byte k, ulong s, short v1) : this(c,k,s,v1,-1)
		{
		}
		public MidiNote(byte? c, byte k, ulong s, short v1, short v2) : base(c,s)
		{
			this.K = k;
			this.V1 = v1;
			this.V2 = v2;
			this.Len = 0;
		}
		public string GetMbtLen(int division)
		{
			return MBT.GetString(Convert.ToUInt64(Len),division);
		}
		public string GetMbtLen2(int division)
		{
			return MBT.GetString(Convert.ToUInt64(Len),division,false);
		}
		public string KeySharp { get { return MidiReader.SmfStringFormatter.GetKeySharp(K); } }
		public string KeyFlat  { get { return MidiReader.SmfStringFormatter.GetKeyFlat(K); } }
		public int		Octave	 { get { return MidiReader.SmfStringFormatter.GetOctave(K); } }
		public string KeyStr	 { get { return string.Format("{0,-2}{1}",KeySharp,Octave); } }
	}


}
