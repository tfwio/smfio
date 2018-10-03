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
  public class SystemSpecific : MIDIMessage
  {
    #region Properties
    
    public string MetaString { get { return Strings.Encoding.GetString(Data); } }
    
    public int MessageLength { get; set; }
    
    public byte[] SystemData { get { return GetSystemMessage(); } }
    
    public byte[] GetSystemMessage()
    {
      byte[] bitset = new byte[Data.Length-1];
      bitset[0] = Data[0];
      for (int i=2; i < Data.Length; i++) bitset[i-1] = Data[i];
      return bitset;
    }
  
    #endregion
    public SystemSpecific(long delta, int message, params byte[] data) : base(MidiMsgType.SystemSpecific,delta,message,data)
    {
      MessageLength = data.Length-1;
    }
  }
  /// <summary>
  /// This is exactly the same as MidiMetaMessage with clarification on the type.
  /// I do believe that I've come across RSE Sysex messages, but I guess I'll have
  /// to find out the hard way if this data is getting collected.
  /// </summary>
  public class SystemExclusive : MIDIMessage
  {
    #region Properties
    
    public string MetaString { get { return Strings.Encoding.GetString(Data); } }
    
    public int MessageLength { get; set; }
    
    public byte[] SystemData { get { return GetSystemMessage(); } }
    
    public byte[] GetSystemMessage()
    {
      byte[] bitset = new byte[Data.Length-1];
      bitset[0] = Data[0];
      for (int i=2; i < Data.Length; i++) bitset[i-1] = Data[i];
      return bitset;
    }
  
    #endregion
    public SystemExclusive(long delta, int message, params byte[] data) : base(MidiMsgType.SystemSpecific,delta,message,data)
    {
      MessageLength = data.Length-1;
    }
  }
}
