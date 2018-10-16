/*
 * Date: 11/12/2005 * Time: 4:19 PM
 */

namespace on.smfio
{
  public class MidiMessage
  {
    public ushort Status { get; set; }
    public long Tick { get; set; }
    public byte[] Data { get; set; }
  }

}
