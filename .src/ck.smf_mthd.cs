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
		
		public MemoryStream GetStream(int trackId) { return new MemoryStream(Tracks[trackId].track); }
		public smf_mtrk this[int Track] { get { return Tracks[Track]; } }
		public byte this[int Track, int Start] { get { return this[Track].track[Start]; } }
		public byte[] this[int Track, int Start, int Length] { get { return GetByteMessage(Start,Length,Track); } }
		
		#region Properties
		internal byte[] bHead = new byte[4];
		public char[] head { get { return System.Text.Encoding.ASCII.GetChars(bHead); } }
		public string Head { get { return System.Text.Encoding.ASCII.GetString(bHead); } }

		public int Size {
			get { return BitConverter.ToInt32(size,0); }
		} byte[] size; //=6

		// it could be possible that in stead of short, we use ushort...
		
		public short Format {
			get { return BitConverter.ToInt16(fmt,0); }
		} byte[] fmt; // ( 0 | 1 | 2 )

		public short NumberOfTracks {
			get { return BitConverter.ToInt16(ntk,0); }
		} byte[] ntk; // NumberOf Tracks

		public short Division {
			get { return BitConverter.ToInt16(div,0); }
		} byte[] div; // division

		/// <summary>Internal ‘MTrk’ tracks</summary>
		public smf_mtrk[] Tracks;
		#endregion
		
		#region Construct
		/// <summary>Constructor</summary>
		public smf_mthd ( byte[] head , byte[] size, byte[] fmt, byte[] ntk, byte[] div, smf_mtrk[] tk)
		{
			this.bHead = head;
			this.size = size;
			this.fmt = fmt;
			this.ntk = ntk;
			this.div = div;
			this.Tracks = tk;
		}
		/// <summary>
		/// Constructor for IO.  I would usually think to consider the Stream as the
		/// Input type, however the BinaryReader exposes the owning stream.
		/// </summary>
		/// <param name="br">System.IO.BinaryReader</param>
		public smf_mthd ( BinaryReader br , params smf_mtrk[] tracks )
		{
			bHead = br.ReadBytes(4);
			size =  EndianUtil.Reverse(br.ReadBytes(4));
			fmt =  EndianUtil.Reverse(br.ReadBytes(2));
			ntk =  EndianUtil.Reverse(br.ReadBytes(2));
			div =  EndianUtil.Reverse(br.ReadBytes(2));
			Tracks = tracks;
			//tk =  br.ReadBytes(4);
		}
		#endregion
		
		#region Utility Functions
		/// <returns>
		/// loads two bytes (UInt16's allocation type/size) to Int32.
		/// </returns>
		public Int32 Get16BitInt32(int trackId, int position)
		{
			return ( Get8Bit(trackId,position) << 8 ) + Get8Bit( trackId, position+1 );
		}
		
		/// <returns>tk[tkid].track[track_pos] which is a single byte where ‘tk’ is typeof(smf_mtrk).</returns>
		public Byte Get8Bit (int tk_id, int track_pos) {
			
//			try {
			return this[tk_id,track_pos];
//			} catch {
//				Debug.Assert(false,"MIDI file error.  A byte was requested out of range of the stream.\nif you continue, a value of zero will be assumed.");
//				return 0;
//			}
		}
		#endregion
		
		#region ToString
		/// Convets to a human readable string.
		public string str_fmt()
		{
			return string.Format(
				"‘{0}’ — size “{1:##,###,###}” — fmt: “{2}” — div : {3} | 0x{3:X} ] — ntk : {4}",
				StringHelper.GetAnsiChars(head),
				BitConverter.ToInt16(size,0),
				BitConverter.ToInt16(fmt,0),
				BitConverter.ToInt16(div,0),
				BitConverter.ToInt16(ntk,0)
			);
		}
		public override string ToString() { return str_fmt(); }
		#endregion
		
		/// retrieves from pos to pos+length into an array of bytes
		public byte[] GetByteMessage( int start, int length, int track )
		{
			byte[] bssx = new byte [ length ];
			Array.Copy(Tracks[track].track,start,bssx,0,length);
			return bssx;
		}
	}
}
