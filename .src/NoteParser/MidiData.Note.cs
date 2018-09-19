/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{

  public class MidiNote : MidiData
  {
    /// <summary>
    /// key number.
    /// </summary>
    public byte K;
    /// <summary>
    /// Key depressed length in ticks.
    /// don't think this is used whatsoever.
    /// </summary>
    public int PulseWidth { get; set; }
    /// <summary>
    /// Velocity or Volume for 1: ‘Note ON’ and 2: “Note OFF”.
    /// </summary>
    public short noteOnVelocity, noteOffVelocity;
    /// <param name="channel">Channel (Nullable)—or is this color?</param>
    /// <param name="key"></param>
    /// <param name="pulse"></param>
    /// <param name="noteOnVelocity"></param>
    public MidiNote(byte? channel, byte key, long pulse, short noteOnVelocity) : this(channel, key, pulse, noteOnVelocity, -1)
    {
    }
    /// <summary>This really doesn't make sense.</summary>
    /// <param name="channel"></param>
    /// <param name="key"></param>
    /// <param name="pulse"></param>
    /// <param name="noteOnVelocity"></param>
    /// <param name="noteOffVelocity"></param>
    /// <returns></returns>
    public MidiNote(byte? channel, byte key, long pulse, short noteOnVelocity, short noteOffVelocity) : base(channel, pulse)
    {
      this.K = key;
      this.noteOnVelocity = noteOnVelocity;
      this.noteOffVelocity = noteOffVelocity;
      this.PulseWidth = 0;
    }
    public string KeySharp { get { return SmfString.GetKeySharp(K); } }
    public string KeyFlat { get { return SmfString.GetKeyFlat(K); } }
    public int Octave { get { return SmfString.GetOctave(K); } }
    public string KeyStr { get { return string.Format("{0,-2}{1}", KeySharp, Octave); } }
  }


}
