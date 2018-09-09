/*
 * Date: 11/12/2005 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
	/// <summary>
	/// it seems that all meta messages have contained within them a
	/// length (variable length) bit weather or not they are strings.
	/// </summary>
	public class MidiMetaMessage : MidiMessage
	{
		#region Properties
		
		public string MetaString { get { return System.Text.Encoding.UTF8.GetString(Data); } }
		public int MessageLength {
			get { return messageLength; } set { messageLength = value; }
		} int messageLength;
	
		#endregion
		public MidiMetaMessage(ulong delta, int message, params byte[] data) : this(MidiMsgType.MetaInf,delta,message,data) {}
		public MidiMetaMessage(MidiMsgType t, ulong delta, int message, params byte[] data) : base(t,delta,message,data)
		{
			MessageLength = data.Length;
		}
	}
}
