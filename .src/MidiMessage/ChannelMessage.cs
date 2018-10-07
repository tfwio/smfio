﻿/*
 * Date: 11/12/2005 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
	public class ChannelMessage : MIDIMessage
	{
		public ChannelMessage(long delta, int message, params byte[] data) : base(MidiMsgType.ChannelVoice,delta,message,data)
		{
		}
	}
}
