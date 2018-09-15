/* Date: 11/12/2005 - Time: 4:19 PM */

namespace on.smfio
{
  /// <summary>
  /// For future use.
  /// </summary>
  class TempoState {

    public int MusPerQuarterNote { get; set; }
    public double Tempo { get { return 60000000.0 / MusPerQuarterNote; } }
    public double Seconds { get; set; }
    public long Min { get; set; }
    public long Max { get; set; }

    public bool Contains(long pulse)
    {
      return (pulse >= Min) && (pulse < Max);
    }
  }
}
