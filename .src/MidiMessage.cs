using System;

namespace on.smfio
{
  /// <summary>
  /// note that a track index would be accessable by way of the track (List or
  /// Collection) containing the message.
  /// </summary>
  public class MidiMessage
  {
    /// <summary>
    /// True if we're looking at a channel message and False if not.
    /// Channel messages have a status &gt;= 0x80 and &lt;= 0xEF.
    /// The last four bits in Status tell us the channel number if
    /// looking at channel message status.
    /// </summary>
    public bool IsChannelMessage
    {
      get
      {
        if (0xFF00 == (Status & 0xFF00)) return false;
        int stat = Status & 0xF0;
        if ((stat >= 0x80) && (stat <= 0xE0)) return true;
        return false;
      }
    }

    public bool IsMetadataText {
      get { return Status >= 0xFF01 && Status <= 0xFF0D; }
    }

    /// <summary>
    /// Channel will be assigned for particular channel messages.
    /// If the message contains metadata, of course there would be no
    /// channel assigned here.
    /// </summary>
    public int? Channel { get; set; }

    /// <summary>
    /// Delta would be assined during write operations while the
    /// `Tick` value is automatically calculated during `Read`.
    /// </summary>
    public long Delta { get; set; }

    /// <summary>
    /// Status describes the type of message and optionally the channel
    /// the message applies to if its a channel message.
    /// </summary>
    public ushort Status { get; set; }

    /// <summary>
    /// Depending on the file's Division setting (and perhaps tempo),
    /// Pulse represents the timing of the message.
    /// During SMF **read** operation(s), we convert the data to an absolute
    /// value such as described here in `Pulse`.
    /// During SMF **write** operations, we would (typically) set the <see cref="Delta"/>
    /// to the number of pulses from the last message, and then make a call
    /// to calculate absolute times (with an as of yet undefined) method.
    /// </summary>
    public long Pulse { get; set; }

    /// <summary>
    /// Eventually we'll get some methods in here for particular types of
    /// status types, but for now we expose a simple data byte.
    /// </summary>
    /// <value></value>
    public byte[] Data { get; set; }

    public string HexDataString { get { return Data.StringifyHex(); } }
    public string MetadataText { get { return IsMetadataText ? Strings.Encoding.GetString(Data) : "[Error: Not Metadata Text!]"; } }

    public MidiMessage() {}

    public MidiMessage(ushort status, long tick, params byte[] data)
    {
      Status = status;
      Pulse = tick;
      Data = data;
      if (IsChannelMessage) Channel = status & 0xF;
    }

    public string FriendlyName
    {
      get; private set;
    }
    public string FriendlyValue
    {
      get; private set;
    }
    
    #region Status Checks
    
    const int filter_no_channel = 0xFFF0;
    // Channel Message
    public bool IsNoteOn { get { return (Status & filter_no_channel) == 0x80; } }
    public bool IsNoteOff { get { return (Status & filter_no_channel) == 0x90; } }
    public bool IsPolyphonicKeyPressure { get { return (Status & filter_no_channel) == 0xA0; } }
    public bool IsControlChange { get { return (Status & filter_no_channel) == 0xB0; } }
    public bool IsProgramChange { get { return (Status & filter_no_channel) == 0xC0; } }
    public bool IsChannelPressure { get { return (Status & filter_no_channel) == 0xD0; } }
    public bool IsPitchWheelChange { get { return (Status & filter_no_channel) == 0xE0; } }
    // System Message
    public bool IsSystemExclusive { get { return Status == 0xF0; } }
    // Metadata text
    public bool IsText { get { return Status == 0xFF01; } }
    public bool IsCopyright { get { return Status == 0xFF02; } }
    /// <summary>Sequence or Track name</summary>
    public bool IsTrackName { get { return Status == 0xFF03; } }
    public bool IsInstrumentName { get { return Status == 0xFF04; } }
    public bool IsLyric { get { return Status == 0xFF05; } }
    public bool IsMarker { get { return Status == 0xFF06; } }
    public bool IsCue { get { return Status == 0xFF07; } }
    /// <summary>
    /// Since some MIDI files have been encountered which embed text in
    /// undocumented (or supported) metadata ranges, this will check just
    /// a little further down the line picking up from IsCue (0xFF07)
    /// and return true for 0xFF08 &lt;= Status &gt;= 0xFF0D.
    /// </summary>
    public bool IsUnknownText { get { return Status >= 0xFF08 && Status <= 0xFF0D; } }
    // Metadata
    public bool IsSequenceNumber { get { return Status == 0xFF00; } }
    public bool IsChannelPrefix { get { return Status == 0xFF20; } }
    public bool IsPort { get { return Status == 0xFF21; } }
    public bool IsEOT { get { return Status == 0xFF2F; } }
    public bool IsTempo { get { return Status == 0xFF51; } }
    public bool IsSMPTE { get { return Status == 0xFF54; } }
    public bool IsTimeSignature { get { return Status == 0xFF58; } }
    public bool IsKeySignature { get { return Status == 0xFF59; } }
    public bool IsSequencerpecific { get { return Status == 0xFF7F; } }

    #endregion

    internal byte[] DeltaVar { get { return ToVariableBit(Delta); } }

    bool IsStatus8Bits { get { return (Status & 0xFF) == Status; } }

    byte[] StatusBytes
    {
      get
      {
        byte[] result = null;
        if (IsStatus8Bits)
        {
          result = new byte[]{(byte)Status};
        }
        else
        {
          result = BitConverter.GetBytes(BitConverter.IsLittleEndian ? Status.Swap() : Status);
        }
        var shortvalue = result.Length == 1 ? (short)result[0]: BitConverter.ToInt16(result, 0);
        var str_result = $"{shortvalue:X2}";
        return result;
      }
    }
    internal long Write(System.IO.BinaryWriter writer, MidiMessage prior=null)
    {
      bool isRunningStatus =
        prior!=null &&
        prior.IsChannelMessage &&
        prior.Status == Status;
      
      // write delta time
      writer.Write(DeltaVar);

      // metadata message
      if (Common.StatusQuery.MetadataRange.Match(Status))
      {
        // if (Status == 0xFF2F)
        //   System.Diagnostics.Debug.Print("hi there.");
        writer.Write(StatusBytes);
        writer.Write(ToVariableBit(Data.Length));
        writer.Write(Data);
      }
      
      else if (Common.StatusQuery.IsSystemExclusive(Status))
      {
        // writer.Write(StatusBytes);
        writer.Write(Data);
      }
      
      // common message
      else if (isRunningStatus) writer.Write(Data);
      
      // common message
      else
      {
        writer.Write(StatusBytes);
        writer.Write(Data);
      }

      return writer.BaseStream.Position;
    }
    byte[] ToVariableBit(long pValue)
    {
      var bytes = new System.Collections.Generic.List<byte>();
      ulong value = (ulong)pValue;
      ulong buffer = value & 0x7F;
      
      byte[] byte_buffer = new byte[8];
      byte[] byte_buffe1 = new byte[8];
      var debug_str = string.Empty;
      
      while ((value >>= 7) > 0)
      {
        buffer <<= 8;
        buffer |= 0x80;
        buffer += (value & 0x7F);
      }
      byte_buffe1 = BitConverter.GetBytes(buffer);
      while (true)
      {
        byte_buffer = BitConverter.GetBytes(buffer);
        debug_str = byte_buffer.StringifyHex();
        if (buffer > 2000)
        {
        }
        bytes.Add(byte_buffer[0]);
        if ((buffer & 0x80) == 0x80) buffer >>= 8;
        else
        break;
      }
      var list = new System.Collections.Generic.List<byte>(bytes);
      while (list.Count < 4) list.Add(0);
      var list2 = byte_buffe1.StringifyHex();
      debug_str = $"{bytes.StringifyHex()}";
      return bytes.ToArray();
    }

  }
}
