/*
 * Date: 11/12/2005 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
	public class ChannelMessageVST : MIDIMessageVST
	{
		public ChannelMessageVST(long delta, int message, params byte[] data) : base(MidiMsgType.ChannelVoice,delta,message,data)
		{
		}
	}
}
