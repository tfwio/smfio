/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.Linq;
using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;
namespace on.smfio
{
  public enum SmpteType : byte {
    /// <summary>Film</summary>
    [System.ComponentModel.Description("24fps")]
    TwentyFour = 0,
    /// <summary>A/V Europe (SECAM or PAL)</summary>
    [System.ComponentModel.Description("25fps")]
    TwentyFive = 1,
    /// <summary>A/V U.S. (NSTC) ~29.97fps Drop-Frame</summary>
    [System.ComponentModel.Description("30fps (Drop-Frame)")]
    Thirty_Drop = 2,
    /// <summary>Film</summary>
    [System.ComponentModel.Description("30fps")]
    Thirty = 3
  }
	public class SmpteOffset
	{
    public string SmpteTypeString {
      get { return SMPTE_Type.GetEnumDescriptionAttribute(); }
    }
		public byte ByteHour, ByteMinute, ByteSecond, ByteFrame, ByteFrameFrac;
    public SmpteType SMPTE_Type { get { return (SmpteType)(ByteHour >> 5); } }
    public int Hour { get { return ByteHour & 0x1F; } }

    public SmpteOffset() : this(0,0,0,0,0) {}
    public SmpteOffset(byte h, byte m, byte s, byte fr, byte ff) { SetSMPTE(h,m,s,fr,ff); }
    public void Reset() { SetSMPTE(0, 0, 0, 0, 0); }
    public void SetSMPTE(byte h, byte m, byte s, byte fr, byte ff) { ByteHour = h; ByteMinute = m; ByteSecond = s; ByteFrame = fr; ByteFrameFrac = ff; }
    public bool IsEmpty { get { return (ByteHour == 0) & (ByteMinute == 0) & (ByteSecond == 0) & (ByteFrame == 0) & (ByteFrameFrac == 0); } }
    public override string ToString() { return IsEmpty ? "[MIDI Clock]" : $"{Hour:#,#00}:{ByteMinute:00}:{ByteSecond:00}:{ByteFrame:00}:{ByteFrameFrac:00} @{SmpteTypeString}"; }

    /// <summary>
    /// Data **MUST BE FIVE BYTES**
    /// 
    /// - HOUR (first) byte has a special meaning...  
    ///     - binary byte: 'aaab bbbb' &lt;-- 'b' bytes are hour, 'a' bytes are SmpteType.
    /// 
    /// One could say...
    /// ```
    /// var result = SmpteOffset.SmpteString(
    ///   (byte)((int)SmpteType.TwentyFour &lt;&lt; 5) ) &amp; nHours),
    ///   0, 0, 0, 0);
    /// ```
    /// </summary>
    /// <param name="data"></param>
    /// <returns>formatted string</returns>
    public static string GetString(params byte[] data)
    {
      if (data.Length > 5) throw new ArgumentException("data takes at most 5 byte parameters.");
      var list = data.ToList();
      do { list.Add(0); } while (list.Count < 5);
      var offset = new SmpteOffset(list[0], list[1], list[2], list[3], list[4]);
      var result = offset.ToString();
      list.Clear();
      list = null;
      offset = null;
      return result;
    }
    public SmpteOffset Copy() { return new SmpteOffset(ByteHour, ByteMinute, ByteSecond, ByteFrame, ByteFrameFrac); }
  }
}


