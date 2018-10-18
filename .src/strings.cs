using System;
namespace on.smfio {
  class StringRes {
    internal const string STRING_APP_NAME = "smfio";
    // used
    internal const string STRING_MTHD_TOSTRING = "‘{0}’ — size “{1:##,###,###}” — fmt: “{2}” — div : {3} | 0x{3:X} ] — ntk : {4}";
    internal const string STRING_META_FF58_FMT = "{0}/{1}, clocks: {2}, 32nds:{3}";
    /// <summary>"mspqn: {0:###,###,###,##0}/{1:###,###,###,##0} = {2}"</summary>
    internal const string msg_time_ms_pqn = "mspqn: {0:###,###,###,##0}/{1:###,###,###,##0} = {2}";
    internal const string String_Unknown_Message = "We're not sure how to handle this message {0:X4} {1:X2}.  Treating data as META event.";
    internal const string fotmat_note_off = "Note Off {0,-3}/{2,3}{3,2}, Velocity: {1}";
    internal const string format_note_on = "Note  On {0,-3}/{2,3}{3,2}, Velocity: {1}";
    internal const string mA = "Key Aftertouch {0,-3}, Velocity: {1}";
    internal const string mB = "CC: {0}, Value: {1}";
    // not used
    internal const string rse_fmt_note = "{0,-2} n#{1} on {2}";
    internal const string rse_fmt_keya = "{0,-2} key aft #{1} on {2}";
    internal const string rse_fmt_cc = "{0,-2}{1}, Value: {2}";
    internal const string msg_chanel = "{0,-2}";
    internal const string msg_time_format = "{0}";
    internal const string mX = "SYSEX  len: {0}";
    internal const string mC = "Patch: {0}";
    internal const string mD = "Channel Aftertouch: #{0:X2}/{0,-3}";
    internal const string mE = "Channel Pitchwheel: #{0:X2}/{0,-3}";
    public static class SystemSpecific
    {
      static public byte[] Sysex_GM_On = new byte[] { 0xF0, 0x05, 0x7E, 0x7F, 0x09, 0x01, 0xF7 };
      static public byte[] Sysex_GM_Off = new byte[] { 0xF0, 0x05, 0x7E, 0x7F, 0x09, 0x02, 0xF7 };
      static public byte[] Sysex_GM2_System_On = new byte[] { 0xF0, 0x05, 0x7E, 0x7F, 0x09, 0x03, 0xF7 };
      static public byte[] Sysex_GS_Reset = new byte[] { 0xF0, 0x0A, 0x41, 0x10, 0x42, 0x12, 0x40, 0x00, 0x7F, 0x00, 0x41, 0xF7 }; // sys on
      static public byte[] Sysex_XG_Master_Tune = new byte[] { 0xF0, 0x0A, 0x43, 0x10, 0x4C, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0xF7 };
      static public byte[] Sysex_XG_Master_Vol = new byte[] { 0xF0, 0x08, 0x43, 0x10, 0x4C, 0x00, 0x00, 0x04, 0x07, 0xF7 };
      static public byte[] Sysex_XG_Transpose = new byte[] { 0xF0, 0x08, 0x43, 0x10, 0x4C, 0x00, 0x00, 0x06, 0x40, 0xF7 };
      static public byte[] Sysex_XG_DrumsReset = new byte[] { 0xF0, 0x08, 0x43, 0x10, 0x4C, 0x00, 0x00, 0x7D, 0x00, 0xF7 };
      static public byte[] Sysex_XG_Reset = new byte[] { 0xF0, 0x08, 0x43, 0x10, 0x4C, 0x00, 0x00, 0x7E, 0x00, 0xF7 }; // sys on
                                                                                                                      //
      static public byte[] Sysex_Master_Volume = new byte[] { 0xF0, 0x07, 0x7F, 0x7F, 0x04, 0x01, 0x00, 0xFF, 0xF7 };
      //static public byte[] Sysex_Master_Volume  = new byte[]{ 0xF0,0x07,0x7F,0x7F,0x04,0x01,0x00,nnnn,0xF7 };
    }
  }
}
