//#define USEFLOAT
using System.Collections.Generic;

namespace on.smfio
{
  public class TempoState
  {
    static public TempoState Default
    {
      get { return new TempoState { PulseMax = 0, Pulse = 0, MusPQN = 500000, Second = 0.0 }; }
    }
    public TempoState() { }
    public TempoState(uint muspqn, int division, long pulse, TempoState prior)
    {
      MusPQN = muspqn;
      Pulse = pulse;
      PulseMax = prior?.Pulse ?? 0;
      Second = TimeUtil.GetSeconds(division, prior?.MusPQN ?? muspqn, pulse - PulseMax, prior?.Second ?? 0.0);
    }
    public TempoState(uint muspqn, int division, long pulse, long lastPulse = 0, double seconds = 0.0)
    {
      MusPQN = muspqn;
      Pulse = pulse;
      PulseMax = lastPulse;
      Second = TimeUtil.GetSeconds(division, muspqn, pulse - lastPulse, seconds);
    }

    public bool Match(long pulse) { return (pulse >= Pulse) && (pulse < PulseMax); }

    /// <summary>The delta time</summary>
    public long PulseMax { get; set; }
    public long Pulse { get; set; }
    public double Second { get; set; }
    /// <summary>The raw 24-bit byte value out of the track's byte-stream.</summary>
    public uint MusPQN { get; set; }

    /// <summary>This is the actual tempo value as translated from the ReferenceValue.</summary>
    public double Tempo
    {
#if USEFLOAT
      get { return TimeUtil.Minute_Hex / MusPQN; }
#else
      get { return TimeUtil.MicroMinute / MusPQN; }
#endif
    }
  }
  public class TempoMap
  {
    List<TempoState> list = new List<TempoState>();
    public bool HasItems { get { return Count > 0; } }
    public bool IsMulti { get { return Count > 1; } }
    public TempoState Peek { get { return HasItems ? list[0] : null; } }
    public TempoState Top { get { return HasItems ? list[Count - 1] : null; } }

    public int Count { get { return list.Count; } }

    public void Clear() { list.Clear(); }

    void Push(TempoState tempo) { list.Insert(0, tempo); }
    public void Push(uint muspqn, int Division, long pulse)
    {
      if (HasItems) Peek.PulseMax = pulse;
      var prior = Peek ?? TempoState.Default;
      Push(new TempoState(muspqn, Division, pulse, prior));
    }
    public TempoState Pop(bool top = false)
    {
      var result = top ? Top : Peek;
      if (HasItems) list.RemoveAt(top ? Count - 1 : 0);
      return result;
    }
    int IndexOf(TempoState state)
    {
      for (int listIndex = 0; listIndex < Count; listIndex++)
      {
        if (list[listIndex].Match(state.Pulse)) return listIndex;
      }
      return -1;
    }
    public TempoState Seek(long pulse)
    {
      while (HasItems)
      {
        if (Top.Match(pulse)) {
          // Log.ErrorMessage($"Resolved mus: {Top.MusPQN}\nindex: {IndexOf(Top)},\nPulse: {Top.Pulse} <= {pulse} < {Top.PulseMax},\n{Top.Second}");
          return Top;
        }
        Pop(true);
      }
      return null;
    }

    public TempoMap Copy()
    {
      return new TempoMap{list = new List<TempoState>(list)};
    }
    /// <summary>
    /// This is to be called after all events have been processed
    /// so that we can determine each Min-Max range properly.
    /// </summary>
    public void Sync(Reader reader, int plus=24)
    {
      long longest = 0;
      for (int nTrackIndex = 0; nTrackIndex < reader.FileHandle.NumberOfTracks; nTrackIndex++)
      {
        if (longest < reader.TrackLength[nTrackIndex])
          longest = reader.TrackLength[nTrackIndex];
      }
      if (list[0].PulseMax <= longest) list[0].PulseMax = longest+12;
      // list[Count - 1].Second = 0.0;
      // list.Reverse();
      // for (int i = 1; i < Count; i++)
      // {
      //   list[i].Second = TimeUtil.GetSeconds(
      //     reader.Division,
      //     list[i-1].MusPQN,
      //     list[i].Pulse - list[i - 1].Pulse,
      //     list[i - 1].Second
      //     );
      // }
      // list.Reverse();
    }
  }
}
