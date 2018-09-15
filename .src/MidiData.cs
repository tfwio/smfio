/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
  /// <seealso cref="IMidiParser_Notes"/>
  public class MidiData
	{
		// TODO: Validate actually channel byte?
		public byte? Channel;
		public ulong Pulse;
		public MidiData(byte? c, ulong pulse)
		{
			this.Channel = c;
			this.Pulse = pulse;
		}
		public string GetMBTString(short division)
		{
      return TimeUtil.GetMBT(Pulse, division);
    }
	}
}
