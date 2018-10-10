/*
 * User: xo
 * Date: 9/8/2018
 * Time: 3:38 AM
 */
using System;
using on.smfio;
namespace SMFIOViewer
{
  
	public interface IMidiParserUI
	//: INaudioVstWin
	{
		/// <summary></summary>
		IReader MidiParser { get; }

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


