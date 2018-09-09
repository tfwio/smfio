/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
namespace on.smfio
{
	public struct MidiMessagePacket
	{
		public MidiMsgType MessageType;

		public int Track;

		public int Offset;

		public int IntMessage;

		public byte ByteMessage;

		public ulong Ppq;

		public int IntRunningStatus;

		public bool IsRunningStatus;
	}
}


