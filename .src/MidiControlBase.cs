/*
 * User: xo
 * Date: 9/8/2018
 * Time: 3:38 AM
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace SMFIOViewer
{
	using Keys = System.Windows.Forms.Keys;
  
  public interface IMidiView : IUi
  {
    string Title { get; }
    
    IMidiParserUI UserInterface { get; set; }
    
    void ClearTrack(object sender, EventArgs e);
    void TrackChanged(object sender, EventArgs e);
    void AfterTrackLoaded(object sender, EventArgs e);
    void BeforeTrackLoaded(object sender, EventArgs e);
    void FileLoaded(object sender, EventArgs e);
  }
	public class MidiControlBase :
	  System.Internals.UserView,
    IMidiView
	{
		public string Title {
			get {
				return this.Text;
			}
		}

		virtual public string Description {
			get {
				return string.Empty;
			}
		}

		public IMidiParserUI UserInterface {
			get;
			set;
		}

		virtual public void ApplyRegistrySettings()
		{
		}

		virtual public void SetUI(IMidiParserUI ui)
		{
			this.UserInterface = ui;
			this.UserInterface.ClearMidiTrack -= ClearTrack;
			this.UserInterface.ClearMidiTrack += ClearTrack;
			this.UserInterface.GotMidiFile -= FileLoaded;
			this.UserInterface.GotMidiFile += FileLoaded;
		}

		public MidiControlBase() : base()
		{
		}

		virtual public void ClearTrack(object sender, EventArgs e)
		{
			Debug.Print("ClearTrack:{0}", this.GetType().Name);
		}

		virtual public void TrackChanged(object sender, EventArgs e)
		{
			Debug.Print("TrackChanged:{0}", this.GetType().Name);
		}

		virtual public void AfterTrackLoaded(object sender, EventArgs e)
		{
			Debug.Print("AfterTrackLoaded:{0}", this.GetType().Name);
		}

		virtual public void BeforeTrackLoaded(object sender, EventArgs e)
		{
			Debug.Print("BeforeTrackLoaded:{0}", this.GetType().Name);
		}

		virtual public void FileLoaded(object sender, EventArgs e)
		{
			Debug.Print("FileLoaded:{0}", this.GetType().Name);
			ClearTrack(sender, e);
			this.UserInterface.MidiParser.TrackChanged -= TrackChanged;
			this.UserInterface.MidiParser.TrackChanged += TrackChanged;
			this.UserInterface.MidiParser.BeforeTrackLoaded -= BeforeTrackLoaded;
			this.UserInterface.MidiParser.BeforeTrackLoaded += BeforeTrackLoaded;
			this.UserInterface.MidiParser.AfterTrackLoaded -= AfterTrackLoaded;
			this.UserInterface.MidiParser.AfterTrackLoaded += AfterTrackLoaded;
		}
	}
}




