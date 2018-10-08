﻿/*
 * tfooo 11/12/2005 4:19 PM
 */
using System;
using System.IO;

namespace on.smfio.chunk
{
  /// <summary>
  /// This is the main SMF File Capsule
  /// <para>• SMF ‘MThd’ header</para>
  /// </summary>
  public class MTHd
  {
    // FIXME: never used
    // public MemoryStream GetStream(int trackId) { return new MemoryStream(Tracks[trackId].track); }
    public MTrk this[int ntrack] { get { return Tracks[ntrack]; } }
    public byte this[int ntrack, int offset] { get { return this[ntrack].Data[offset]; } }
    public byte[] this[int ntrack, int offset, int length] { get { return GetByteMessage(ntrack, offset, length); } }

    #region Properties
    internal byte[] ByteHead = new byte[4];
    public char[] CkHead { get { return System.Text.Encoding.ASCII.GetChars(ByteHead); } }
    public string Head { get { return System.Text.Encoding.ASCII.GetString(ByteHead); } }

    public int Size { get { return BitConverter.ToInt32(ByteSize,0); } } byte[] ByteSize; //=6

    // it could be possible that in stead of short, we use ushort...
    
    public short Format { get { return BitConverter.ToInt16(ByteFormat,0); } } byte[] ByteFormat; // ( 0 | 1 | 2 )

    public short NumberOfTracks { get { return BitConverter.ToInt16(ByteNTrack,0); } } byte[] ByteNTrack; // NumberOf Tracks

    public short Division { get { return BitConverter.ToInt16(ByteDivision,0); } } byte[] ByteDivision; // division

    /// <summary>Internal ‘MTrk’ tracks</summary>
    public MTrk[] Tracks;
    
    #endregion

    #region Construct
    /// <summary>
    /// Constructor for IO.  I would usually think to consider the Stream as the
    /// Input type, however the BinaryReader exposes the owning stream.
    /// </summary>
    /// <param name="br">System.IO.BinaryReader</param>
    /// <param name="tracks">track byte stream.</param>
    public MTHd ( BinaryReader br , params MTrk[] tracks )
    {
      ByteHead     = br.ReadBytes(4);
      ByteSize     = EndianUtil.Flip(br.ReadBytes(4));
      ByteFormat   = EndianUtil.Flip(br.ReadBytes(2));
      ByteNTrack   = EndianUtil.Flip(br.ReadBytes(2));
      ByteDivision = EndianUtil.Flip(br.ReadBytes(2));
      Tracks = tracks;
      //tk =  br.ReadBytes(4);
    }
    #endregion

    public int ReadDelta(int pTrackID, int pTrackOffset, out long pDeltaVar)
    {
      return Tracks[pTrackID].DeltaRead(pTrackOffset, out pDeltaVar);
    }

    #region Message Utility Functions

    public ushort Get16Bit(int ntrack, int offset)
    {
      uint msg8 = Get8Bit(ntrack, offset);
      return Convert.ToUInt16(msg8 == 0xFF ? ((msg8 << 8) + Get8Bit(ntrack, offset + 1)) : msg8);
    }
    /// <returns>tk[tkid].track[track_pos] which is a single byte where ‘tk’ is typeof(MTrk).</returns>
    public byte Get8Bit (int ntrack, int offset) { return this[ntrack, offset]; }
    #endregion
    
    #region ToString
    /// Convets to a human readable string.
    public string str_fmt()
    {
      return string.Format(
        StringRes.STRING_MTHD_TOSTRING,
        StringHelper.GetAnsiChars(CkHead),
        BitConverter.ToInt16(ByteSize,0),
        BitConverter.ToInt16(ByteFormat,0),
        BitConverter.ToInt16(ByteDivision,0),
        BitConverter.ToInt16(ByteNTrack,0)
       );
    }

    public override string ToString() { return str_fmt(); }
    #endregion

    /// <summary>retrieves from pos to pos+length into an array of bytes</summary>
    byte[] GetByteMessage( int ntrack, int start, int length )
    {
      byte[] bytes = new byte [ length ];
      Array.Copy(Tracks[ntrack].Data, start, bytes, 0, length);
      return bytes;
    }
  }
  
  static class Extender
  {
    public static byte[] Byte32(this BinaryReader reader)
    {
      return EndianUtil.Flip(reader.ReadBytes(4));
    }
    public static byte[] Byte16(this BinaryReader reader)
    {
      return EndianUtil.Flip(reader.ReadBytes(4));
    }
  }
}
