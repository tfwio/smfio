/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;

namespace on.smfio
{
	public class MidiTimeSignature
	{
		public int Numerator, Denominator, Clocks, ThirtySeconds;
		
		public void SetSignature(int n, int d, int c, int t32)
		{
			Numerator = n; Denominator = d; Clocks = c; ThirtySeconds = t32;
		}
		public MidiTimeSignature() : this(4,4,24,0)
		{
		}
		public MidiTimeSignature(int n, int d, int c, int t32)
		{
			SetSignature(n,d,c,t32);
		}
	}
}
