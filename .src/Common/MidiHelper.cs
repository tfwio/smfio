/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;
namespace on.smfio
{
	static public class MidiHelper
	{
		static public string[] OctaveMacro()
		{
			string[] ax = new string[128];
			for (int i = 0; i <= 127; i++)
				ax[i] = string.Format("{0}{1}", keys[i % 12], Convert.ToInt32(i / 12));
			return ax;
		}

		static public string[] OctaveMacro(int lo, int hi)
		{
			string[] ax = new string[hi - lo];
			int octave = 0;
			for (int i = octave; i < hi; i += 12) {
				ax[i] = string.Format("{0}{1}", keys[i % 12], Convert.ToInt32(i / 12));
			}
			return ax;
		}

		public static readonly bool[] IsIvory = new bool[12] {
			true,
			false,
			true,
			false,
			true,
			true,
			false,
			true,
			false,
			true,
			false,
			true
		};

		public static readonly string[] keys = new string[12] {
			"C ",
			"C#",
			"D ",
			"D#",
			"E ",
			"F ",
			"F#",
			"G ",
			"G#",
			"A ",
			"A#",
			"B "
		};
	}
}


