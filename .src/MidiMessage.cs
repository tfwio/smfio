using System;

namespace on.smfio
{
  public class MidiMessageCollection : DictionaryList<int, MidiMessage>
  {
    public short MidiFormat { get; set; }
    
    public TempoMap TempoMap { get; set; }
    public SmpteOffset SMPTE { get; set; }
    public MidiTimeSignature TimeSignature { get; set; }
    public MidiKeySignature KeySignature { get; set; }
    
    public short Division { get; set; }
    public MidiMessageCollection(): base()
    {
    }

    public MidiMessageCollection(MidiMessageCollection collection)
    : base((System.Collections.Generic.IDictionary<int, System.Collections.Generic.List<MidiMessage>>)collection)
    {
      MidiFormat = collection.MidiFormat;
      Division = collection.Division;
      // 
      if (collection.TempoMap!=null) TempoMap = collection.TempoMap.Copy();
      if (collection.KeySignature!=null) KeySignature = collection.KeySignature.Copy();
      if (collection.TimeSignature!=null) TimeSignature = collection.TimeSignature.Copy();
      if (collection.SMPTE!=null) SMPTE = collection.SMPTE.Copy();
    }

    void CopyReaderTempoMap(Reader reader)
    {
      MidiFormat = reader.FileHandle.Format;
      Division = reader.Division;
      // 
      TempoMap = reader.TempoMap.Copy();
      KeySignature = reader.KeySignature.Copy();
      TimeSignature = reader.TimeSignature.Copy();
      SMPTE = reader.SMPTE.Copy();
    }

    static public MidiMessageCollection FromFile(string smfFilePath)
    {
      MidiMessageCollection collection = null;
      using (Reader reader= new Reader(){ GenerateMessageList = true })
      {
        reader.FileHandle = new chunk.MThd(smfFilePath);

        reader.MidiMessages.Division = reader.Division;
        reader.MidiMessages.MidiFormat = reader.FileHandle.Format;

        reader.ParseTempoMap(0);

        reader.MessageHandler = reader.OnMidiMessage;

        reader.ParseAll();
        reader.TempoMap.Finalize(reader);

        collection = new MidiMessageCollection(reader.MidiMessages);
        collection.CopyReaderTempoMap(reader);

        reader.ResetTempoMap();
      }
      return collection;
    }
  }
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
        int t1 = Status & 0xF0;
        if ((Status <= 0x80) && (Status >= 0xE0)) return true;
        return false;
      }
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
    public uint? Delta { get; set; }

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

    public MidiMessage() {}

    public MidiMessage(ushort status, long tick, params byte[] data)
    {
      Status = status;
      Pulse = tick;
      Data = data;
      if (IsChannelMessage) Channel = status & 0xF;
    }
  }

}
