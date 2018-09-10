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
	partial class MidiReader : IMidiParser_Resources
  {
		
		/// <inheritdoc />
		public Dictionary<string, Color> Colors { get { return MetaHelpers.Colors; } }

		// 
		// Color Helper (For WinForms)
		// ---------------------------------

		/// <inheritdoc />
		public Color GetRseEventColor(Color clr)
		{
			int ExpandedRSE = RunningStatus32 << 8;
			if (!MidiMessageInfo.IsMidiBMessage(RunningStatus32)) { ErrorMessage("warning… {0:X2}", ExpandedRSE); return clr; }
			else if (MidiMessageInfo.IsNoteOn(ExpandedRSE))            return MetaHelpers.Colors["rse2"];
			else if (MidiMessageInfo.IsNoteOff(ExpandedRSE))           return MetaHelpers.Colors["rse2"];
			else if (MidiMessageInfo.IsKeyAftertouch(ExpandedRSE))     return MetaHelpers.Colors["rse2"];
			else if (MidiMessageInfo.IsControlChange(ExpandedRSE))     return MetaHelpers.Colors["rse3"];
			else if (MidiMessageInfo.IsProgramChange(ExpandedRSE))     return MetaHelpers.Colors["rse3"];
			else if (MidiMessageInfo.IsChannelAftertouch(ExpandedRSE)) return MetaHelpers.Colors["rse3"];
			else if (MidiMessageInfo.IsPitchBend(ExpandedRSE))         return MetaHelpers.Colors["rse3"];
			else if (MidiMessageInfo.IsSystemMessage(ExpandedRSE))     return MetaHelpers.Colors["rse3"];
			else if (MidiMessageInfo.IsSystemCommon(ExpandedRSE))      return MetaHelpers.Colors["rse3"];
			else if (MidiMessageInfo.IsSystemRealtime(ExpandedRSE))    return MetaHelpers.Colors["rse3"];
			return ListView.DefaultBackColor;
		}
		public static readonly Color c0 = Color.FromArgb(235, 225, 225);
		public static readonly Color c1 = Color.FromArgb(149, 109, 177);
		public static readonly Color c2 = Color.FromArgb(109, 149, 177);
		public static readonly Color c3 = Color.FromArgb(107, 180, 131);
		public static readonly Color c4 = Color.FromArgb(225, 225, 235);
		public static readonly Color cR = Color.FromArgb(255, 0, 0);
		/// <inheritdoc />
		public Color GetEventColor(Color clr)
		{
			int ExpandedRSE = RunningStatus32 << 8;

			if (!MidiMessageInfo.IsMidiBMessage(RunningStatus32)) { ErrorMessage("warning… {0:X2}", ExpandedRSE); return clr; }
			else if (MidiMessageInfo.IsNoteOn(ExpandedRSE))            return c1;
			else if (MidiMessageInfo.IsNoteOff(ExpandedRSE))           return c2;
			else if (MidiMessageInfo.IsKeyAftertouch(ExpandedRSE))     return c2;
			else if (MidiMessageInfo.IsControlChange(ExpandedRSE))     return c3;
			else if (MidiMessageInfo.IsProgramChange(ExpandedRSE))     return c3;
			else if (MidiMessageInfo.IsChannelAftertouch(ExpandedRSE)) return c3;
			else if (MidiMessageInfo.IsPitchBend(ExpandedRSE))         return c3;
			else if (MidiMessageInfo.IsSystemMessage(ExpandedRSE))     return c0;
			else if (MidiMessageInfo.IsSystemCommon(ExpandedRSE))      return c0;
			else if (MidiMessageInfo.IsSystemRealtime(ExpandedRSE))    return c0;
			return ListView.DefaultBackColor;
		}
		/// <inheritdoc />
		public Color GetEventColor(int intMsg, Color clr)
		{
			switch (intMsg) {
				case (int)MetaMsgU16FF.Text:
				case (int)MetaMsgU16FF.Copyright:
				case (int)MetaMsgU16FF.SequenceName:
				case (int)MetaMsgU16FF.InstrumentName:
				case (int)MetaMsgU16FF.Lyric:
				case (int)MetaMsgU16FF.Marker:
				case (int)MetaMsgU16FF.Cue:
				case (int)MetaMsgU16FF.Port:
					return c4;
				case (int)MetaMsgU16FF.Chanel:
				case (int)MetaMsgU16FF.SequenceNo:
				case (int)MetaMsgU16FF.SMPTE:
				case (int)MetaMsgU16FF.SystemExclusive:
					return Colors["red"];
				case (int)MetaMsgU16FF.EndOfTrack:
					return Colors["end"];
				case (int)MetaMsgU16FF.Tempo:
					return Colors["tempo"];
				case (int)MetaMsgU16FF.TimeSignature:
					return Colors["tsig"];
				case (int)MetaMsgU16FF.KeySignature:
					return Colors["ksig"];
				default:
					return ListView.DefaultBackColor;
			}
		}
	}
}
