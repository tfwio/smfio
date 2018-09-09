/*
 * Date: 11/12/2005 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
	/// <summary>
	/// This is exactly the same as MidiMetaMessage with clarification on the type.
	/// I do believe that I've come across RSE Sysex messages, but I guess I'll have
	/// to find out the hard way if this data is getting collected.
	/// </summary>
	public class MidiSysexMessage : MidiMessage
	{
		#region Properties
		
		public string MetaString { get { return System.Text.Encoding.UTF8.GetString(Data); } }
		
		public int MessageLength {
			get { return messageLength; } set { messageLength = value; }
		} int messageLength;
		
		public byte[] SystemData { get { return GetSystemMessage(); } }
		
		public byte[] GetSystemMessage()
		{
			byte[] bitset = new byte[Data.Length-1];
			bitset[0] = Data[0];
			for (int i=2; i < Data.Length; i++) bitset[i-1] = Data[i];
			return bitset;
		}
	
		#endregion
		public MidiSysexMessage(ulong delta, int message, params byte[] data) : base(MidiMsgType.System,delta,message,data)
		{
			MessageLength = data.Length-1;
		}
	}
}
