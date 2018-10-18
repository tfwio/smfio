/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */

// Horse barn for rent

using System;
using CliEvent = System.EventArgs;

namespace on.smfio
{
  public interface IReaderParser
  {
    /// <summary>Gets a string value.</summary>
    /// <param name="nTrackIndex"></param>
    /// <param name="nTrackOffset"></param>
    /// <returns>UTF8 Decoded</returns>
    string GetMetadataString(int nTrackIndex, int nTrackOffset);

    /// <summary>Return a string value per meta-event or throw exception.</summary>
    string GetMessageString(int nTrackIndex, int nTrackOffset);

    /// <summary>Documentation needed</summary>
    /// <param name="nTrackIndex"></param>
    /// <param name="nTrackOffset"></param>
    /// <returns></returns>
    byte[] GetMetadataBytes(int nTrackIndex, int nTrackOffset);

    /// <summary>
    /// Next (Running Status) Position — next buffer read position.
    /// 
    /// When calling this method, the reader is likely leaving off
    /// from the following 
    /// 
    /// 1. [**Delta-Time**] [**Status**] — following a status byte  
    ///    add one to offset
    /// 2. [**Delta-Time**] — following delta-time (running status message)  
    ///    offset increment not necessary.
    /// </summary>
    int Increment(int offset);

    /// <summary>Documentation needed</summary>
    byte[] GetEventValue(int nTrackIndex, int nTrackOffset);

    string GetEventString(int nTrackIndex, int nTrackOffset);

    /// <summary>
    /// This function is typically called directly after either of the following cases:
    /// 
    /// 1. **[delta-time]** **[status]**; following a status.  
    ///    If we're following a status, then we increment the offset by 1.
    /// 2. **[delta-time]** following a delta-time with running status.
    ///    If we're dealing with a running status, then no incrementing the offset.
    /// </summary>
    string GetEventValueString(int nTrackIndex, int nTrackOffset);
  }
}
