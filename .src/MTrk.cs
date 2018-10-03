/*
 * tfooo 11/12/2005 4:19 PM
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace on.smfio.chunk
{
	public struct MTrk
	{
	  internal byte this[int kOffset] { get { return this.Data[kOffset]; } }
	  
		static readonly bool ConvertEndian = BitConverter.IsLittleEndian;
		public char[] CkID;
		public byte[] SizeBytes;
		public byte[] Data;
		
		public uint ReadU16(int poffset) { return Read(poffset, 2); }
		public uint ReadU24(int poffset) { return Read(poffset, 3); }
		public uint ReadU32(int poffset) { return Read(poffset, 4); }
		uint Read(int poffset, int plength)
    {
      byte[] result = new byte[4];
      Array.ConstrainedCopy(Data, poffset, result, 4-plength, plength);
      return BitConverter.ToUInt32(ConvertEndian ? EndianUtil.Flip(result) : result, 0);
		}
		
		public int Size { get { return BitConverter.ToInt32(SizeBytes, 0); } }

		/// <summary>read the track's bytes; Doesn't parse the track though.</summary>
		/// <param name="bi"></param>
		public MTrk( BinaryReader bi )
		{
			CkID = bi.ReadChars(4);
			SizeBytes  = EndianUtil.Flip(bi.ReadBytes(4));
			Data = bi.ReadBytes( BitConverter.ToInt32( SizeBytes, 0 ) );
		}
		public string GetString(params int[] values)
		{
			var ct = new List<string>();
			foreach (int i in values) ct.Add(string.Format("{0:N0}", Data[i]));
			string returnvalue = string.Join(", ",ct.ToArray());
			ct.Clear();
			ct = null;
			return returnvalue;
		}
	}
}
