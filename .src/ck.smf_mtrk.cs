/*
 * Created by SharpDevelop.
 * User: tfooo
 * Date: 11/12/2005
 * Time: 4:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace on.smfio.chunk
{

	public struct smf_mtrk
	{
		static bool ConvertEndian = true;
		public char[] ck_id;
		public byte[] size;
		public byte[] track;
		
		public uint Get16Bit(int pos) { return u32(pos, 2); }
		public uint Get24Bit(int pos) { return u32(pos, 3); }
		public uint GetU32(int pos) { return u32(pos, 4); }
		uint u32(int offset, int length)
    {
      byte[] result = new byte[4];
      Array.ConstrainedCopy(track, offset, result, 4-length, length);
      return BitConverter.ToUInt32(ConvertEndian ? EndianUtil.Flip(result) : result, 0);
		}
		
		public int Size { get { return BitConverter.ToInt32(size, 0); } }

		/// <summary>read the track's bytes; Doesn't parse the track though.</summary>
		/// <param name="bi"></param>
		public smf_mtrk( BinaryReader bi )
		{
			ck_id = bi.ReadChars(4);
			size  = EndianUtil.Flip(bi.ReadBytes(4));
			track = bi.ReadBytes( BitConverter.ToInt32( size, 0 ) );
		}
		
		public string GetString(params int[] values)
		{
			List<string> ct = new List<string>();
			foreach (int i in values) ct.Add(string.Format("{0:N0}",this.track[i]));
			string returnvalue = string.Join(", ",ct.ToArray());
			ct.Clear();
			ct = null;
			return returnvalue;
		}
	}
}
