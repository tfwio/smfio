/*
 * Date: 11/12/2005 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
  /// <summary>
  /// This is exactly the same as MidiMetaMessage with clarification on the type.
  /// I do believe that I've come across RSE Sysex messages, but I guess I'll have
  /// to find out the hard way if this data is getting collected.
  /// </summary>
  public class SequencerSpecificVST : MIDIMessageVST
  {
    #region Properties
    
    public string MetaString { get { return Strings.Encoding.GetString(base.Data); } }
    
    public int Length { get; set; }
    
    public override byte[] Data { get { return GetSystemMessage(); } set { data = value; } }
    byte[] data;
    
    public byte[] GetSystemMessage()
    {
      byte[] bitset = new byte[base.Data.Length-1];
      bitset[0] = base.Data[0];
      for (int i=2; i < base.Data.Length; i++) bitset[i - 1] = base.Data[i];
      return bitset;
    }
  
    #endregion
    public SequencerSpecificVST(long delta, int message, params byte[] data) : base(MidiMsgType.SequencerSpecific,delta,message,data)
    {
      Length = data.Length-1;
    }
  }
  public class SequencerSpecificUnkn : MIDIMessageVST
  {

    #region Properties

    public string MetaString { get { return Strings.Encoding.GetString(base.Data); } }

    public int Length { get; set; }

    public override byte[] Data { get { return GetSystemMessage(); } set { data = value; } }
    byte[] data;

    public byte[] GetSystemMessage()
    {
      byte[] bitset = new byte[base.Data.Length - 1];
      bitset[0] = base.Data[0];
      for (int i = 2; i < base.Data.Length; i++) bitset[i - 1] = base.Data[i];
      return bitset;
    }

    #endregion
    public SequencerSpecificUnkn(ushort msg16, long delta, int message, params byte[] data) : base(MidiMsgType.SequencerSpecificUnknown, delta, message, data)
    {
      Length = data.Length - 1;
    }
  }
}
