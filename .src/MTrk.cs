/*
 * tfooo 11/12/2005 4:19 PM
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace on.smfio.chunk
{
	public struct MTrk
	{
	  internal byte this[int kOffset] { get { return Data[kOffset]; } }
    internal IEnumerable<byte> this[int kOffset, int kSize] {
			get
			{
				var kiss = kOffset+kSize;
				if (kOffset >= Data.Length) throw new IndexOutOfRangeException();
				if (kiss >= Data.Length) throw new IndexOutOfRangeException();
        for (int i = kOffset; i < kiss; i++) yield return Data[i];
			}
		}
	  
		static readonly bool ConvertEndian = BitConverter.IsLittleEndian;
		public char[] CkID;
		public byte[] SizeBytes;
		public byte[] Data;
		
		public uint ReadU16(int poffset) { return ReadInteger(poffset, 2); }
		public uint ReadU24(int poffset) { return ReadInteger(poffset, 3); }
		public uint ReadU32(int poffset) { return ReadInteger(poffset, 4); }
		uint ReadInteger(int poffset, int plength)
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
		public int DeltaSeek(int pTrackOffset, int increment = 2, int backstep = 1)
    {
      long result = 0;
      int index = DeltaRead(pTrackOffset + increment, out result);
      int value = index + (int)result - backstep;
      return value;
    }
    public int DeltaRead(int pTrackOffset, out long pDeltaVar)
    {
      byte tempBit;
      int i = pTrackOffset;
      if ((pDeltaVar = Data[i++]) > 0x7F)
      {
        pDeltaVar &= 0x7F;
        do
        {
          pDeltaVar = (pDeltaVar << 7) + ((tempBit = Data[i++]) & 0x7F);
        } while (tempBit > 0x7F && i < Data.Length);
      }
      return i;
    }
    public int GetEndOfSystemExclusive(int nTrackOffset)
    {
      int offset = nTrackOffset;
      while (Data[offset] != 0xF7) offset++;
      return offset;
    }

    public struct MessageBlock
    {
      long DeltaTime;
      uint Status;
			int Offset;
      int Size;

		  // public MessageBlock(MTrk mTrack, int offset)
		  // {
		  // 	
		  // }

      public byte[] GetData(MThd pHandle, int pTrackID) { return pHandle[pTrackID, Offset, Size];}
			public byte[] GetData(MTrk pTrack) { return pTrack[Offset, Size].ToArray(); }
    }
	}
}
