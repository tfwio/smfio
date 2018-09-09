/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
    
	/// <summary>
	/// four byte data slot convertable to int.
	/// Thus far the byte data hasn't been used.
	/// </summary>
	public struct MidiByteData
	{
		static public implicit operator int(MidiByteData data) { return data.IntData; }
		static public implicit operator byte[](MidiByteData data) { return data.Data; }
		static public implicit operator MidiByteData(int data) { return new MidiByteData(data); }
		static public implicit operator MidiByteData(byte[] data) { return new MidiByteData(data); }
		
		byte[] bitvalue;
		
		public int IntData {
			get { return BitConverter.ToInt32(bitvalue,0); }
			set { bitvalue = BitConverter.GetBytes(value); }
		}
		public byte[] Data {
			get { return bitvalue; }
			set { bitvalue = value; }
		}
		
		public MidiByteData(byte[] data)
		{
			bitvalue = data;
		}
		public MidiByteData(int data)
		{
			bitvalue = BitConverter.GetBytes(data);
		}
	}
}
