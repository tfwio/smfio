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
    /// <summary>#EBE1E1 light pink</summary>
    internal static readonly Color c0            = Color.FromArgb(235, 225, 225);
    /// <summary>#956DB1 violet</summary>
    internal static readonly Color c1            = Color.FromArgb(149, 109, 177);
    /// <summary>#6D95B1 blue</summary>
    internal static readonly Color c2            = Color.FromArgb(109, 149, 177);
    /// <summary>#6BB483 green</summary>
    internal static readonly Color c3            = Color.FromArgb(107, 180, 131);
    /// <summary>#FFFFEB faint yellow (light)</summary>
    internal static readonly Color c4            = Color.FromArgb(225, 225, 235);
    /// <summary>#FF0000 100% red</summary>
    internal static readonly Color cR            = Color.FromArgb(255, 0, 0);


    
    /// <summary>#D7A8D7 light magenta</summary>
    internal static readonly Color PitchBend     = Color.FromArgb(215, 168, 215);
    /// <summary>#FDDE82 yellow/orange</summary>
    internal static readonly Color ProgramChange = Color.FromArgb(253, 222, 130);
    /// <summary>#D2E168 green</summary>
    internal static readonly Color ControlChange = Color.FromArgb(210, 225, 104);
    /// <summary>#68A5E1 blue</summary>
    internal static readonly Color NoteOn        = Color.FromArgb(104, 165, 225);
    /// <summary>#C468E1 violet</summary>
    internal static readonly Color NoteOff       = Color.FromArgb(196, 104, 225);
    internal static readonly Dictionary<string, Color> Colors = new Dictionary<string, Color>(){
      {"225", Color.FromArgb(225, 255, 225)  }, // #FFFFFF white
      {"rse1", Color.FromArgb(225, 255, 225) }, // #E1FFE1 light green
      {"rse2", Color.FromArgb(0, 127, 255)   }, // #007FFF blue
      {"rse3", Color.FromArgb(125, 225, 125) }, // #7DE17D green (slightly yellowish)
      {"tempo", Color.FromArgb(225, 235, 225)}, // #E1EBE1 very light cyan? (light bluish, slightly greenish)
      {"tsig", Color.FromArgb(225, 235, 225) }, // ""
      {"ksig", Color.FromArgb(225, 235, 225) }, // ""
      {"end", Color.FromArgb(125, 135, 125)  }, // #7D877D dark grayish cyan (-ish)
      {"ssx", Color.FromArgb(125, 135, 125)  }, // ""
      {"red", Color.Red    },
      {"meta", Color.Red   },
      {"white", Color.White},
      {"chanel", Color.Red },
    };
    // 
    // Color Helper (For WinForms)
    // ---------------------------------

    /// <summary>
    /// this method absolutely depends on RunningStatus32 value.
    /// </summary>
    internal static Color GetRseEventColor(int status)
		{
      int ExpandedRSE = status;// << 8;
			if (!StatusQuery.IsMidiBMessage(status)) { Log.ErrorMessage("warning… {0:X2}", ExpandedRSE); return Color.Red; }
			
			else if (StatusQuery.IsKeyAftertouch(ExpandedRSE))     return Colors["rse2"]; // blue
			// *custom
			else if (StatusQuery.IsNoteOn(ExpandedRSE))            return NoteOn;
      else if (StatusQuery.IsNoteOff(ExpandedRSE))           return NoteOff;
      else if (StatusQuery.IsPitchBend(ExpandedRSE))         return PitchBend;
			else if (StatusQuery.IsProgramChange(ExpandedRSE))     return ProgramChange;
			else if (StatusQuery.IsControlChange(ExpandedRSE))     return ControlChange;

      else if (StatusQuery.IsChannelAftertouch(ExpandedRSE)) return Colors["rse3"]; // green
      else if (StatusQuery.IsSequencerSpecific(ExpandedRSE)) return Colors["rse3"]; // when is this running status?
			else if (StatusQuery.IsSystemCommon(ExpandedRSE))      return Colors["rse3"];
			else if (StatusQuery.IsSystemRealtime(ExpandedRSE))    return Colors["rse3"];
			// ?
			return ListView.DefaultBackColor;
    }


		/// <summary>
		/// this method absolutely depends on RunningStatus32 value.
		/// </summary>
    internal static Color GetEventColor(int status)
		{
      int ExpandedRSE = status;// << 8;

			if (!StatusQuery.IsMidiBMessage(status)) { Log.ErrorMessage("warning… {0:X2}", ExpandedRSE); return Color.Red; }
			
			else if (StatusQuery.IsNoteOn(ExpandedRSE))            return NoteOn;
			else if (StatusQuery.IsNoteOff(ExpandedRSE))           return NoteOff;
			else if (StatusQuery.IsPitchBend(ExpandedRSE))         return PitchBend;
			else if (StatusQuery.IsProgramChange(ExpandedRSE))     return ProgramChange;
      else if (StatusQuery.IsControlChange(ExpandedRSE))     return ControlChange;

			else if (StatusQuery.IsKeyAftertouch(ExpandedRSE))     return c2;
			else if (StatusQuery.IsChannelAftertouch(ExpandedRSE)) return c3;

			else if (StatusQuery.IsSequencerSpecific(ExpandedRSE)) return c0; // light pink
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
				case (int)Stat16.SequenceNumber:
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
