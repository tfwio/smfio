/*
 * Date: 11/12/2005 * Time: 4:19 PM
 */
using System;
using on.smfio.util;

namespace on.smfio
{
	public class MidiMessage
	{
		#region Check Range
		
		/// <summary>
		/// Check sample-range
		/// </summary>
		/// <param name="c">must have Division and PPQ set.</param>
		/// <param name="b">We currently check the end position of the loop.</param>
		/// <param name="min">first sample in search block</param>
		/// <param name="max">last sample in search block</param>
		/// <returns>True if delta-time is contained within sample ranges min and max</returns>
		public bool IsContained(SampleClock c, Loop b, double min, double max)
		{
			double samplePos = c.SolveSamples(DeltaTime).GetSamples32();
			return samplePos >= min && samplePos < max && samplePos < b.End;
		}
		/// <summary>
		/// Check sample-range
		/// </summary>
		/// <param name="c">must have Division and PPQ set.</param>
		/// <param name="b">We currently check the end position of the loop.</param>
		/// <returns>True if delta-time is contained within sample ranges min and max</returns>
		public bool IsContained(SampleClock c, Loop b)
		{
			double samplePos = c.SolveSamples(DeltaTime).GetSamples32();
			return samplePos >= b.Begin && samplePos < b.End;
		}
		#endregion
		
		#region Properties
		
		public MidiMsgType MessageFlag {
			get { return messageFlag; }
		} internal MidiMsgType messageFlag = MidiMsgType.Undefined;
		
		public byte ChannelBit { get { return Convert.ToByte(Message & 0x000F); } }
		public byte MessageBit { get { return Convert.ToByte(Message & 0x00F0); } }
		
		public int Message {
			get { return message; } set { message = value; }
		} int message;
		
		public ulong DeltaTime {
			get { return deltaTime; } set { deltaTime = value; }
		} ulong deltaTime;
		
		public byte[] Data {
			get { return data; } set { data = value; }
		} byte[] data;

		#endregion
		
		/// <summary>
		/// </summary>
		/// <param name="delta"></param>
		/// <param name="msgType">Typically 8, 9, A, B and C</param>
		/// <param name="data">Must be a length of 4 bytes.</param>
		public MidiMessage(MidiMsgType t, ulong delta, int message, params byte[] data)
		{
			this.Data = data;
			this.DeltaTime = delta;
			this.Message = message;
			this.messageFlag = t;
		}
	}

}
