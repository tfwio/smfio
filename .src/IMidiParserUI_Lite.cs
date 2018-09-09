/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;
using System.Linq;
namespace on.smfio
{
	public interface IMidiParserUI_Lite
	{
		/// <summary></summary>
		IMidiParser MidiParser {
			get;
		}

		/// <summary></summary>
		void Action_MidiFileOpen();

		/// <summary></summary>
		void Action_MidiFileOpen(string filename, int trackNo);

		/// <summary></summary>
		event EventHandler ClearMidiTrack;

		/// <summary></summary>
		event EventHandler GotMidiFile;
	}
}




