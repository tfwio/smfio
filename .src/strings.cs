using System;
namespace on.smfio {
  class StringRes {
    internal const string STRING_APP_NAME = "smfio";
    // used
    internal const string STRING_MTHD_TOSTRING = "‘{0}’ — size “{1:##,###,###}” — fmt: “{2}” — div : {3} | 0x{3:X} ] — ntk : {4}";
    internal const string STRING_META_FF58_FMT = "{0}/{1}, clocks: {2}, 32nds:{3}";
    internal const string msg_time_ms_pqn = "mspqn: {0:###,###,###,##0}/{1:###,###,###,##0} = {2}";
    internal const string String_Unknown_Message = "We're not sure how to handle this message {0:X4} {1:X2}.  Treating data as META event.";
    internal const string m8 = "Note Off {0,-3}/{2,3}{3,2}, Velocity: {1}";
    internal const string m9 = "Note  On {0,-3}/{2,3}{3,2}, Velocity: {1}";
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
  }
}
