using System;
namespace on.smfio
{
  public class TempoChange
  {
    /// <summary>MIDI Track (in midi file-structure)</summary>
    public int TrackID { get; set; }
    
    /// <summary>index into the track's bytes (buffer)</summary>
    public int TrackOffset { get; set; }
    
    /// <summary>The delta time</summary>
    public ulong TPQ { get; set; }
    
    /// <summary>This is the actual tempo value as translated from the ReferenceValue.</summary>
    public double TempoValue { get; set; }
    
    /// <summary>The raw 24-bit byte value out of the track's byte-stream.</summary>
    public uint ReferenceValue { get; set; }
  }
}




