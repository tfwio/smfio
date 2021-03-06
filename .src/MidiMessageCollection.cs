using System;

namespace on.smfio
{
  public partial class MidiMessageCollection : DictionaryList<int, MidiMessage>
  {
    
    public TempoMap TempoMap { get; set; }
    public SmpteOffset SMPTE { get; set; }
    public MidiTimeSignature TimeSignature { get; set; }
    public MidiKeySignature KeySignature { get; set; }
    
    public MidiMessage KeySignatureMessage, SMPTEMessage, TempoMapMessage, TimeSignatureMessage;
    
    public short Division { get; set; }
    public short MidiFormat { get; set; }
    
    public MidiMessageCollection(): base()
    {
    }

    /// <summary>
    /// Allows you to clone the messages and properties from another MidiMessageCollection.
    /// </summary>
    /// <param name="collection"></param>
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

    /// <summary>
    /// First, if sorting is requested (SortFirst=true)
    /// each track is sorted (by pulse).
    /// 
    /// Deltas are (re-) calculated.
    /// 
    /// Generally, this method will be called either called
    /// after a call to <see cref="FromFile"/>.
    /// </summary>
    /// <param name="SortFirst"></param>
    public void RecalculateDeltas(bool SortFirst=true)
    {
      if (SortFirst)
      {
        for (int ntrack=0; ntrack < NTracks; ntrack++)
          this[ntrack].Sort(MidiMessage.ComparePulse);
      }
      
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

    internal void FinalSort()
    {
      for (int n = 0; n < NTracks; n++)
      {
        this[n].Sort(MidiMessage.ComparePulse);
        //var list = new System.Collections.Generic.List<MidiMessage>(this[n].Sort(MidiMessage.ComparePulse));
      }

    }
  }
}
