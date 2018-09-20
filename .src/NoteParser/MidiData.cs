/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
  // /// <seealso cref="IMidiParser_Notes"/>
  public class MidiData
  {
    // TODO: Validate actually channel byte?
    public byte? Channel;
    public char CharChannel { get { return Channel.HasValue ? (char)Channel.Value : (char)0; } }

    public long Pulse;
    public MidiData(byte? c, long pulse)
    {
      this.Channel = c;
      this.Pulse = pulse;
    }
    public string GetMBTString(short division)
    {
      return TimeUtil.GetMBT(Pulse, division);
    }
    public string GetMbtLen2(int division)
    {
      return TimeUtil.GetMBT(Pulse,division,false);
    }
  }
}
