using System;

namespace on.smfio
{
  public class MidiMessageCollection : DictionaryList<int, MidiMessage>
  {
    
    public TempoMap TempoMap { get; set; }
    public SmpteOffset SMPTE { get; set; }
    public MidiTimeSignature TimeSignature { get; set; }
    public MidiKeySignature KeySignature { get; set; }
    
    public short Division { get; set; }
    public short MidiFormat { get; set; }
    
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

    void CopyReaderTempoMap(IReader reader)
    {
      MidiFormat = reader.FileHandle.Format;
      Division = reader.Division;
      // 
      TempoMap = reader.TempoMap.Copy();
      KeySignature = reader.KeySignature.Copy();
      TimeSignature = reader.TimeSignature.Copy();
      SMPTE = reader.SMPTE.Copy();
    }

    public void RecalculateDeltas()
    {
      foreach (var trackList in this)
      {
        long lastPulse = 0;
        foreach (var msg in trackList.Value)
        {
          msg.Delta = msg.Pulse - lastPulse;
          lastPulse = msg.Pulse;
        }
      }
    }

    internal short NTracks { get { return (short) Keys.Count; } }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="smfFilePath"></param>
    /// <param name="overwrite"></param>
    /// <returns>True on error.</returns>
    public bool ToFile(string smfFilePath, bool overwrite=false)
    {
      if (System.IO.File.Exists(smfFilePath) && !overwrite) return true;

      var mthd = new chunk.MThd(
        NTracks,
        Division,
        MidiFormat
        );
      
      using (var buffer = new System.IO.FileStream(smfFilePath, System.IO.FileMode.OpenOrCreate))
        using (var writer = new System.IO.BinaryWriter(buffer))
      {
        mthd.Write(this, writer);
      }

      return false;
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
        collection.RecalculateDeltas();
        reader.ResetTempoMap();
      }
      return collection;
    }
  }
}
