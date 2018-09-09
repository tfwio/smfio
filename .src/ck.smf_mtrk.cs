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
		public char[] title;
		public byte[] size;
		public byte[] track;
		
		public long Get16Bit(int pos)
		{
			return track[pos+1] | track[pos] << 8;
		}
		public long Get24Bit(int pos)
		{
			return track[pos+2] | track[pos+1] << 8 | track[pos] << 16;
		}
		public long Get32Bit(int pos)
		{
			return track[pos+3] | track[pos+2] << 8 | track[pos+1] << 16 | track[pos] << 24;
		}
		
		public int Read24(int pos, int offset)
		{
			return
				track[pos+offset+1] << 16 |
				track[pos+offset+2] << 8 |
				track[pos+offset+3];
		}
		
		public int Size { get { return BitConverter.ToInt32(size,0); } }
		
		public MemoryStream GetTrackBuffer() { return new MemoryStream(track); }

		public smf_mtrk (char[] title, byte[] size, byte[] track)
		{
			this.title = title;
			this.size = size;
			this.track = track;
		}

		// if we were working in c we would of course allocate
		// memory into a block, and…
		public byte[] ReadTrack(int start, int length)
		{
			byte[] data = new byte[length];
			using (MemoryStream ms = GetTrackBuffer()) ms.Read(data,start,length);
			return data;
		}
		/// <summary>
		/// read the track's bytes; Doesn't parse the track though.
		/// </summary>
		/// <param name="bi"></param>
		public smf_mtrk( BinaryReader bi )
		{
			title = bi.ReadChars(4);
			size = EndianUtil.Reverse(bi.ReadBytes(4));
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
