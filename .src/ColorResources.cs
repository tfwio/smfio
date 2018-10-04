/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using on.smfio.Common;
using CliEvent = System.EventArgs;

namespace on.smfio
{
	class ColorResources
  {
    internal static readonly Color c0 = Color.FromArgb(235, 225, 225);
    internal static readonly Color c1 = Color.FromArgb(149, 109, 177);
    internal static readonly Color c2 = Color.FromArgb(109, 149, 177);
    internal static readonly Color c3 = Color.FromArgb(107, 180, 131);
    internal static readonly Color c4 = Color.FromArgb(225, 225, 235);
    internal static readonly Color cR = Color.FromArgb(255, 0, 0);

    internal static Dictionary<string, Color> Colors
    {
      get
      {
        Dictionary<string, Color> dic = new Dictionary<string, Color>();
        dic.Add("red", Color.Red);
        dic.Add("meta", Color.Red);
        dic.Add("white", Color.White);
        dic.Add("chanel", Color.Red);
        dic.Add("225", Color.FromArgb(225, 255, 225));
        dic.Add("rse1", Color.FromArgb(225, 255, 225));
        dic.Add("rse2", Color.FromArgb(0, 127, 255));
        dic.Add("rse3", Color.FromArgb(125, 225, 125));
        dic.Add("tempo", Color.FromArgb(225, 235, 225));
        dic.Add("tsig", Color.FromArgb(225, 235, 225));
        dic.Add("ksig", Color.FromArgb(225, 235, 225));
        dic.Add("end", Color.FromArgb(125, 135, 125));
        dic.Add("ssx", Color.FromArgb(125, 135, 125));
        return dic;
      }
    }
    // 
    // Color Helper (For WinForms)
    // ---------------------------------

    /// <summary>
    /// this method absolutely depends on RunningStatus32 value.
    /// </summary>
    internal static Color GetRseEventColor(Color clr, int RunningStatus32)
		{
			int ExpandedRSE = RunningStatus32 << 8;
			if (!StatusQuery.IsMidiBMessage(RunningStatus32)) { Log.ErrorMessage("warning… {0:X2}", ExpandedRSE); return clr; }
			else if (StatusQuery.IsNoteOn(ExpandedRSE))            return Colors["rse2"];
			else if (StatusQuery.IsNoteOff(ExpandedRSE))           return Colors["rse2"];
			else if (StatusQuery.IsKeyAftertouch(ExpandedRSE))     return Colors["rse2"];
			else if (StatusQuery.IsControlChange(ExpandedRSE))     return Colors["rse3"];
			else if (StatusQuery.IsProgramChange(ExpandedRSE))     return Colors["rse3"];
			else if (StatusQuery.IsChannelAftertouch(ExpandedRSE)) return Colors["rse3"];
			else if (StatusQuery.IsPitchBend(ExpandedRSE))         return Colors["rse3"];
			else if (StatusQuery.IsSequencerSpecific(ExpandedRSE)) return Colors["rse3"];
			else if (StatusQuery.IsSystemCommon(ExpandedRSE))      return Colors["rse3"];
			else if (StatusQuery.IsSystemRealtime(ExpandedRSE))    return Colors["rse3"];
			return ListView.DefaultBackColor;
    }


		/// <summary>
		/// this method absolutely depends on RunningStatus32 value.
		/// </summary>
    internal static Color GetEventColor(Color clr, int RunningStatus32)
		{
			int ExpandedRSE = RunningStatus32 << 8;

			if (!StatusQuery.IsMidiBMessage(RunningStatus32)) { Log.ErrorMessage("warning… {0:X2}", ExpandedRSE); return clr; }
			else if (StatusQuery.IsNoteOn(ExpandedRSE))            return c1;
			else if (StatusQuery.IsNoteOff(ExpandedRSE))           return c2;
			else if (StatusQuery.IsKeyAftertouch(ExpandedRSE))     return c2;
			else if (StatusQuery.IsControlChange(ExpandedRSE))     return c3;
			else if (StatusQuery.IsProgramChange(ExpandedRSE))     return c3;
			else if (StatusQuery.IsChannelAftertouch(ExpandedRSE)) return c3;
			else if (StatusQuery.IsPitchBend(ExpandedRSE))         return c3;
			else if (StatusQuery.IsSequencerSpecific(ExpandedRSE)) return c0;
			else if (StatusQuery.IsSystemCommon(ExpandedRSE))      return c0;
			else if (StatusQuery.IsSystemRealtime(ExpandedRSE))    return c0;
			return ListView.DefaultBackColor;
		}
    /// <summary>
    /// this method absolutely depends on RunningStatus32 value.
		/// NOT.
    /// </summary>
    internal static Color GetEventColor(int intMsg, Color clr, int RunningStatus32)
		{
			switch (intMsg) {
				case (int)Stat16.Text:
				case (int)Stat16.Copyright:
				case (int)Stat16.SequenceName:
				case (int)Stat16.InstrumentName:
				case (int)Stat16.Lyric:
				case (int)Stat16.Marker:
				case (int)Stat16.Cue:
				case (int)Stat16.PortMessage:
					return c4;
				case (int)Stat16.ChannelPrefix:
				case (int)Stat16.SequenceNo:
				case (int)Stat16.SMPTEOffset:
				case (int)Stat16.SystemExclusive:
					return Colors["red"];
				case (int)Stat16.EndOfTrack:
					return Colors["end"];
				case (int)Stat16.SetTempo:
					return Colors["tempo"];
				case (int)Stat16.TimeSignature:
					return Colors["tsig"];
				case (int)Stat16.KeySignature:
					return Colors["ksig"];
				default:
					return ListView.DefaultBackColor;
			}
		}
	}
}
