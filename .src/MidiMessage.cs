/*
 * Date: 11/12/2005 * Time: 4:19 PM
 */

namespace on.smfio
{
  /// <summary>
  /// note that a track index would be accessable by way of
  /// the track (List or Collection) containing the message.
  /// </summary>
  public class MidiMessage
  {
    public ushort Status { get; set; }
    public long Tick { get; set; }
    public byte[] Data { get; set; }
  }

}
