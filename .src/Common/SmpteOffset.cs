/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;
namespace on.smfio
{
  public enum SmpteType : byte {
    /// <summary>Film</summary>
    TwentyFour = 0,
    /// <summary>A/V Europe (SECAM or PAL)</summary>
    TwentyFive = 1,
    /// <summary>A/V U.S. (NSTC) ~29.97fps</summary>
    Thirty_Drop = 2,
    /// <summary>Film</summary>
    Thirty = 3
  }
	public class SmpteOffset
	{
    public string SmpteTypeString {
      get { switch (SMPTE_Type){ case SmpteType.TwentyFour: return "24fps"; case SmpteType.TwentyFive: return "25fps"; case SmpteType.Thirty_Drop: return "30fps (~29.97 Drop-Frame)"; default: return "30fps"; } }
    }
		public byte bHH, bM, bSS, bFR, bFF;
    public SmpteType SMPTE_Type { get { return (SmpteType)(bHH >> 5); } }
    public int Hour { get { return bHH & 0x1F; } }

    public SmpteOffset() : this(0,0,0,0,0) {}
    public SmpteOffset(byte h, byte m, byte s, byte fr, byte ff) { SetSMPTE(h,m,s,fr,ff); }
    public void Reset() { SetSMPTE(0, 0, 0, 0, 0); }
    public void SetSMPTE(byte h, byte m, byte s, byte fr, byte ff) { bHH = h; bM = m; bSS = s; bFR = fr; bFF = ff; }
    public bool IsEmpty { get { return (bHH == 0) & (bM == 0) & (bSS == 0) & (bFR == 0) & (bFF == 0); } }
    public override string ToString() { return IsEmpty ? "[none; uses MIDI Clock]" : $"{Hour:#,#00}:{bM:00}:{bSS:00}:{bFR:00}:{bFF:00} @{SmpteTypeString}"; }
  
    public SmpteOffset Copy() { return new SmpteOffset(bHH, bM, bSS, bFR, bFF); }
  }
}


