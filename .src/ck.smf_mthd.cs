/*
 * Created by SharpDevelop.
 * User: tfooo
 * Date: 11/12/2005
 * Time: 4:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;

namespace on.smfio.chunk
{
	/// <summary>
	/// This is the main SMF File Capsule
	/// <para>• SMF ‘MThd’ header</para>
	/// </summary>
	public class smf_mthd
	{
		// FIXME: never used
		// public MemoryStream GetStream(int trackId) { return new MemoryStream(Tracks[trackId].track); }
		public smf_mtrk this[int ntrack] { get { return Tracks[ntrack]; } }
		public byte this[int ntrack, int offset] { get { return this[ntrack].track[offset]; } }
		public byte[] this[int ntrack, int offset, int length] { get { return GetByteMessage(ntrack,offset,length); } }
		
		#region Properties
		internal byte[] bHead = new byte[4];
		public char[] head { get { return System.Text.Encoding.ASCII.GetChars(bHead); } }
		public string Head { get { return System.Text.Encoding.ASCII.GetString(bHead); } }

		public int Size { get { return BitConverter.ToInt32(size,0); } } byte[] size; //=6

		// it could be possible that in stead of short, we use ushort...
		
		public short Format { get { return BitConverter.ToInt16(fmt,0); } } byte[] fmt; // ( 0 | 1 | 2 )

		public short NumberOfTracks { get { return BitConverter.ToInt16(ntk,0); } } byte[] ntk; // NumberOf Tracks

		public short Division { get { return BitConverter.ToInt16(div,0); } } byte[] div; // division

		/// <summary>Internal ‘MTrk’ tracks</summary>
		public smf_mtrk[] Tracks;
    
    #endregion

    #region Construct
    /// <summary>
    /// Constructor for IO.  I would usually think to consider the Stream as the
    /// Input type, however the BinaryReader exposes the owning stream.
    /// </summary>
    /// <param name="br">System.IO.BinaryReader</param>
    /// <param name="tracks">track byte stream.</param>
    public smf_mthd ( BinaryReader br , params smf_mtrk[] tracks )
		{
			bHead  = br.ReadBytes(4);
      size   = EndianUtil.Flip(br.ReadBytes(4));
			fmt    = EndianUtil.Flip(br.ReadBytes(2));
			ntk    = EndianUtil.Flip(br.ReadBytes(2));
			div    = EndianUtil.Flip(br.ReadBytes(2));
			Tracks = tracks;
			//tk =  br.ReadBytes(4);
		}
		#endregion
		
		#region Utility Functions
		/// <returns>
		/// loads two bytes (UInt16's allocation type/size) to Int32.
		/// </returns>
		public Int32 Get16BitInt32(int ntrack, int offset) { return ( Get8Bit(ntrack, offset) << 8 ) + Get8Bit( ntrack, offset+1 ); }
		
		/// <returns>tk[tkid].track[track_pos] which is a single byte where ‘tk’ is typeof(smf_mtrk).</returns>
		public Byte Get8Bit (int ntrack, int offset) { return this[ntrack, offset]; }
		#endregion
		
		#region ToString
    /// Convets to a human readable string.
    public string str_fmt()
		{
			return string.Format(
        StringRes.STRING_MTHD_TOSTRING,
				StringHelper.GetAnsiChars(head),
				BitConverter.ToInt16(size,0),
				BitConverter.ToInt16(fmt,0),
				BitConverter.ToInt16(div,0),
				BitConverter.ToInt16(ntk,0)
			);
		}

		public override string ToString() { return str_fmt(); }
    #endregion

    /// <summary>retrieves from pos to pos+length into an array of bytes</summary>
    byte[] GetByteMessage( int ntrack, int start, int length )
		{
			byte[] bytes = new byte [ length ];
			Array.Copy(Tracks[ntrack].track, start, bytes, 0, length);
			return bytes;
		}
	}
}
