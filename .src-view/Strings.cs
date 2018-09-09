/*
 * User: xo
 * Date: 9/8/2018
 * Time: 3:38 AM
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace SMFIOViewer
{
	/// <summary>
	/// Description of AppStrings.
	/// </summary>
	static public class Strings
	{
		public const string Filter_MidiTrack = "MTrk {0}";

		public const string AutomationUnitToString = "{{AutomationUnit: Value={0}, DeltaMode={1}}}";

		public const string DeltaArgumentException = "Expected: DeltaType.Samples and DeltaType.Pulses";

		public const string Dialog_Title_0 = "cs midi/smf";

		public const string Dialog_Title_1 = "cs midi/smf [{0}]";

		public const string DictionaryList_ErrorMessage = "Argument ‘{0}’ as allready been added to the Dictionary.";

		public const string DictionaryList_ErrorMessage_Title = "DictionaryList Usage Error";

		public const string FileFilter_MidiFile = "Standard MIDI Format|*.mid;*.midi|All Files|*";

		public const string FileFilter_VstConfig = "VST Host Configuration|*.vstcfg|Configuration File|*.cfg|All Files|*";

		public const string Format_LabelTimeInfo_Main = "MBQT: {1} — Time: {4} — Frame: {5,7:N3} ; ‘{6}’=>‘{7}’ : {8}";

		public const string ms_mbq = "{0:00}:{1:00}:{2:00}";

		public const string ms_mbqt = "{0:00}:{1:00}:{3:00}:{2:000}";

		public const string ms_mbt = "{0:00}:{1:00}:{2:000}";

		public const string Registry_Main = @"Software\tfoxo\midi";

		public const string PianoMBQFormat = @"""{0:N0}:{1:00}:{2:00}""";

		public const string PianoMBQTFormat = "{0:N0}:{1:00}:{2:00}.{3:000}";
	}
}


