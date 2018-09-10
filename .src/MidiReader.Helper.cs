/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.IO;
using on.smfio.chunk;

namespace on.smfio
{
  partial class MidiReader
  {
    /// <summary>
    /// Perhaps supplied with SMPTE/MTC Offset.
    /// </summary>
    public enum SmpteMtcFrameCount
    {
      F24,
      F25,
      F30d,
      F30
    }
    //
    // Helpers
    // ---------------------------------
    
    #region struct MidiEvent
    
    public struct MidiEvent
    {
      public int Track, Offset, Delta, Ppq;
      public byte[] Data;
      public byte ChMsg { get { return Convert.ToByte(Data[0] & 0xF0); } }
      public byte ChByte { get { return Convert.ToByte(Data[0] & 0x0F); } }
      
      public MidiEvent(int track, int offset, int delta, int ppq, params byte[] data)
      {
        Track=track;
        Offset=offset;
        Delta=delta;
        Ppq=ppq;
        Data = data;
      }
    }

    #endregion

    #region class SmfStringFormatter
    
    /// <summary>
    /// Contains several utility functions to convert binary data
    /// to a human readable format.
    /// </summary>
    static public class SmfStringFormatter
    {
      #region Sysex Helpers
      static public byte[] Sysex_GM_On           = new byte[]{ 0xF0,0x05,0x7E,0x7F,0x09,0x01,0xF7 };
      static public byte[] Sysex_GM_Off          = new byte[]{ 0xF0,0x05,0x7E,0x7F,0x09,0x02,0xF7 };
      static public byte[] Sysex_GM2_System_On   = new byte[]{ 0xF0,0x05,0x7E,0x7F,0x09,0x03,0xF7 };
      static public byte[] Sysex_GS_Reset        = new byte[]{ 0xF0,0x0A,0x41,0x10,0x42,0x12,0x40,0x00,0x7F,0x00,0x41,0xF7 }; // sys on
      static public byte[] Sysex_XG_Master_Tune  = new byte[]{ 0xF0,0x0A,0x43,0x10,0x4C,0x00,0x00,0x00,0x04,0x00,0x00,0xF7 };
      static public byte[] Sysex_XG_Master_Vol   = new byte[]{ 0xF0,0x08,0x43,0x10,0x4C,0x00,0x00,0x04,0x07,0xF7 };
      static public byte[] Sysex_XG_Transpose    = new byte[]{ 0xF0,0x08,0x43,0x10,0x4C,0x00,0x00,0x06,0x40,0xF7 };
      static public byte[] Sysex_XG_DrumsReset   = new byte[]{ 0xF0,0x08,0x43,0x10,0x4C,0x00,0x00,0x7D,0x00,0xF7 };
      static public byte[] Sysex_XG_Reset        = new byte[]{ 0xF0,0x08,0x43,0x10,0x4C,0x00,0x00,0x7E,0x00,0xF7 }; // sys on
      //
      static public byte[] Sysex_Master_Volume   = new byte[]{ 0xF0,0x07,0x7F,0x7F,0x04,0x01,0x00,0xFF,0xF7 };
      //static public byte[] Sysex_Master_Volume  = new byte[]{ 0xF0,0x07,0x7F,0x7F,0x04,0x01,0x00,nnnn,0xF7 };
      #endregion
      
      #region Fields

      public static  string[]  cc;
      public static  string[]  patches;
      public static  string[]  drums;
      #endregion
      
      #region Keys/Notes
      static public readonly string[] KeysFlat  = new string[]{ "C","Db","D","Eb","E","F","Gb","G","Ab","A","Bb","B" };
      static public readonly string[] KeysSharp = new string[]{ "C","C#","D","D#","E","F","F#","G","G#","A","A#","B" };
      
      static public string GetKeySharp(int value) { return KeysSharp[value % 12]; }
      static public string GetKeyFlat(int value) { return KeysFlat[value % 12]; }
      
      static public int GetOctave(int value) { return (int)Math.Floor((double)value / 12); }
      #endregion
      
    }
    
    #endregion
    
    #region class MidiUtil
    
    public class MidiUtil
    {
      ///<summary>
      /// handles Controllers, Patches, and Instrument Lists
      /// writes an array of strings for each line in the ASCII text document
      ///</summary>
      static public string[] LoadEnumerationFile(string ccfile)
      {
        string[] controllers  = new string[128];
        StreamReader bi = File.OpenText(ccfile);
        string spoof = bi.ReadToEnd();
        string[] moo = spoof.Split(new char[] {(char)0x0D});
        bi.Close();
        //MessageBox.Show(moo.Length.ToString(),"");
        return moo;
      }
      
      /// <summary>Static MTHD loader.</summary>
      /// <param name="fileName"></param>
      /// <returns></returns>
      static public smf_mthd GetMthd(string fileName)
      {
        smf_mthd SmfFileHandle = null;
        using (FileStream __filestream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          using (BinaryReader __binaryreader = new BinaryReader(__filestream))
        {
          SmfFileHandle = new smf_mthd(__binaryreader);
          SmfFileHandle.Tracks = new smf_mtrk [SmfFileHandle.NumberOfTracks];
          for (int i=0; i < SmfFileHandle.NumberOfTracks; i++)
            SmfFileHandle.Tracks[i] = new smf_mtrk(__binaryreader);
        }
        return SmfFileHandle;
      }
    }
    
    #endregion

  }
}
