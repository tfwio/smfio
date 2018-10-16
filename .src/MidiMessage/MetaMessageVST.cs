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
	public class MetaMessageVST : MIDIMessageVST
	{
		
		public string MetaString { get { return Strings.Encoding.GetString(Data); } }
		public int MessageLength { get; set; }

		public MetaMessageVST(long delta, int message, params byte[] data) : this(MidiMsgType.MetaInf,delta,message,data) {}
		public MetaMessageVST(MidiMsgType t, long delta, int message, params byte[] data) : base(t,delta,message,data)
		{
			MessageLength = data.Length;
		}
	}
}
