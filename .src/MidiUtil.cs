/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System.IO;
using on.smfio.chunk;

namespace on.smfio
{
  static class MidiUtil
  {

    /// <summary>Static MTHD loader.</summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    static public smf_mthd GetMthd(string fileName)
    {
      smf_mthd SmfFileHandle = null;
      using (FileStream __filestream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      using (BinaryReader __binaryreader = new BinaryReader(__filestream))
      {
        SmfFileHandle = new smf_mthd(__binaryreader);
        SmfFileHandle.Tracks = new smf_mtrk[SmfFileHandle.NumberOfTracks];
        for (int i = 0; i < SmfFileHandle.NumberOfTracks; i++)
          SmfFileHandle.Tracks[i] = new smf_mtrk(__binaryreader);
      }
      return SmfFileHandle;
    }
  }

}
