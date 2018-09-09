/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
	public class MidiMessageEvent : EventArgs
	{
		public byte ByteMsg;
		public int IntMsg,Track,Offset,Rse;
		public ulong Ppq;
		public bool IsRse = false;
		public MidiMsgType MsgT = MidiMsgType.Channel;
		public byte[] Data = null;
		public MidiMessageEvent(MidiMsgType t, int track, int offset, int imsg, byte bmsg, ulong ppq, int rse) : this(t,track,offset,imsg,bmsg,ppq,rse,false) { }
		public MidiMessageEvent(MidiMsgType t, int track, int offset, int imsg, byte bmsg, ulong ppq, int rse, bool isrse)
		{
			this.MsgT = t;
			this.Track = track;
			this.Offset = offset;
			this.IntMsg = imsg;
			this.ByteMsg = bmsg;
			this.Ppq = ppq;
			this.Rse = rse;
			this.IsRse = isrse;
		}
	}
}
