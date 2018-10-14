/* oIo * 2/23/2011 3:51 AM */

namespace System
{
  /// <summary>
  /// Description of AppStrings.
  /// </summary>
  static class Strings
  {
    internal static System.Text.Encoding Encoding = System.Text.Encoding.Default;
    
    public const string Filter_MidiTrack = "MTrk {0}";
    public const string AutomationUnitToString = "{{AutomationUnit: Value={0}, DeltaMode={1}}}";
    public const string DeltaArgumentException = "Expected: DeltaType.Samples and DeltaType.Pulses";
    public const string Dialog_Title_0 = "cs midi/smf";
    public const string Dialog_Title_1 = "cs midi/smf [{0}]";
    public const string DictionaryList_ErrorMessage = "Argument ‘{0}’ as allready been added to the Dictionary.";
    public const string DictionaryList_ErrorMessage_Title = "DictionaryList Usage Error";
    public const string FileFilter_MidiFile = "Standard MIDI Format|*.mid;*.midi|All Files|*";
    public const string FileFilter_VstConfig = "VST Host Configuration|*.vstcfg|Configuration File|*.cfg|All Files|*";
    public const string Format_LabelTimeInfo_Main = "MBQT: {1} — Time: {4} — Frame: {5,7:N3} ; ‘{6}’=>‘{7}’ : {8}";
    public const string ms_mbq = "{0:00}:{1:00}:{2:00}";
    public const string ms_mbqt = "{0:00}:{1:00}:{3:00}:{2:000}";
    public const string ms_mbt = "{0:00}:{1:00}:{2:000}";
    public const string Registry_Main = @"Software\tfoxo\midi";
    public const string PianoMBQFormat = @"""{0:N0}:{1:00}:{2:00}""";
    public const string PianoMBQTFormat = "{0:N0}:{1:00}:{2:00}.{3:000}";
  }
	#if !NOCOR3
	/// <para>
	/// quick helpers for string data conversions.
	/// This has been written particularly to replace a ‘mop’ function
	/// from the ‘STR’ class (my old string lib)
	/// </para>
	static class StringHelper
	{
		static public string GetAnsiChars(params char[] chars)
		{
			byte[] copy = System.Text.Encoding.ASCII.GetBytes(chars);
			string returnValue = System.Text.Encoding.ASCII.GetString(copy);
			Array.Clear(copy,0,copy.Length);
			return returnValue;
		}
  
		/// <summary>reverse a bit array</summary>
		/// <param name="bits">the result is reversed (for little-endian/big-endian swapping)</param>
		/// <returns>a reversed array of bits.</returns>
		static public byte[] convb(byte[] bits)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(bits);
			return bits;
		}
  
		#region GetBit
		/// <summary>string to byte[] conversion</summary>
		/// <remarks>uses System.Text.Encoding.Default</remarks>
		static public byte[] getBit(string inpoo)
		{
			return Strings.Encoding.GetBytes(inpoo);
		}
		/// <summary>byte[] to string conversion</summary>
		static public byte[] getBit(string inpoo, System.Text.Encoding Enc)
		{
			return Enc.GetBytes(inpoo);
		}
		#endregion
		
		static public string ToHexString(this byte[] inb)
      {
        if (inb==null) return string.Empty;
        string bish = "";
        foreach (byte c in inb) { bish += c.ToString("X2").PadLeft(2,'0')+" "; }
        return bish.TrimEnd();
      }
		
	}
#endif
}
