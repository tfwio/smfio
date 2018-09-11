/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
  /// <summary>
  /// Currently, we're not bothering to store the Midi Track Offset because
  /// this is used primarily for memory and writing.
  /// 
  /// </summary>
  /// <seealso cref="IMidiParser_Notes"/>
  public class MidiData
	{
		public byte? Ch;
		public ulong Start;
		public MidiData(byte? c, ulong s)
		{
			this.Ch = c;
			this.Start = s;
		}
		public string GetMBT(short division)
		{
			return MBT.GetString(Start,division);
		}
	}
}
