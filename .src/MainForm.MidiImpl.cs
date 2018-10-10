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
using on.smfio;
using on.smfio.util;
namespace SMFIOViewer
{
  public partial class MainForm : IMidiParserUI
  {
    OpenFileDialog MidiFileDialog = new OpenFileDialog(){
      Filter=Strings.FileFilter_MidiFile
    };
    
    public IReader MidiParser {
      get { return midiParser; }
    } protected internal on.smfio.Reader midiParser;
    
    // /// <inheritdoc/>
    //public ITimeConfiguration Settings {
    //  get { return mSettings; }
    //} internal protected TimeConfiguration mSettings;
    
    // don't know what this was for any more!
    // nor do I get why it is causing an exception
    // (hence the try/catch-block in SetProgress(int))
    int cycles = 0, cycle = 12;
    
    int trackLen { get { return this.MidiParser.FileHandle[MidiParser.ReaderIndex].Data.Length; } }
    
    void SetProgress(int offset)
    {
      this.toolStripProgressBar1.Value = (int)(((double)offset / trackLen) * 100f);
      try {
        cycles = (cycles++ % cycle);
      } catch {
      }
    }
    
    #region MIDI FILE event
    
    public event EventHandler ClearMidiTrack;
    protected virtual void OnClearMidiTrack()
    {
      if (ClearMidiTrack != null)
        ClearMidiTrack(this, EventArgs.Empty);
    }
    
    public event EventHandler GotMidiFile;
    protected virtual void OnGotMidiFile()
    {
      numPpq.Value = MidiParser.Division;
      numTempo.Value = Convert.ToDecimal(midiParser.TempoMap.Top.Tempo);
      if (GotMidiFile != null)
        GotMidiFile(this, EventArgs.Empty);
    }
    
    #endregion
    
    void Event_MidiFileLoaded(object sender, EventArgs e)
    {
      TracksToToolStripMenu();
      TracksToListBox();
      TracksToListBoxContext();
      MidiTree.TracksToTreeView(this);
      
      LoadTracks = MidiParser.TrackSelectAction;

      MidiParser.ReaderIndex = 0;
      numTempo.Value = Convert.ToDecimal(MidiParser.TempoMap.Top.Tempo);
      numPpq.Value = Convert.ToDecimal(MidiParser.Division);
    }
    
    #region MIDI ListBox (Event_MidiChangeTrack_MenuItemSelected)
    
    void Event_MidiChangeTrack(object sender, EventArgs e)
    {
      cycles = 0;
      cycle = trackLen > 230 ? 1 : (int)(trackLen * 0.07f);
      this.toolStripProgressBar1.Maximum = 100;
      this.toolStripProgressBar1.Enabled = true;
      
      OnClearMidiTrack();
      
      if (MidiParser == null)
        return;
      
      var toolStripMenuItem = sender as ToolStripMenuItem;
      if (toolStripMenuItem != null)
        MidiParser.ReaderIndex = (int)toolStripMenuItem.Tag;
      
      else if (sender is ListBox && listBox1.SelectedIndex > -1)
        MidiParser.ReaderIndex = listBox1.SelectedIndex;
      
    }
    
    #endregion
    
    void TracksToToolStripMenu()
    {
      foreach (KeyValuePair<int,string> track in MidiParser.GetMidiTrackNameDictionary()) {
        var tn = new ToolStripMenuItem(track.Value, null, Event_MidiChangeTrack);
        tn.Tag = track.Key;
        btn_pick_track.DropDownItems.Add(tn);
      }
    }
    
    void TracksToListBox()
    {
      listBox1.DataSource = btn_pick_track.DropDownItems;
      listBox1.DisplayMember = "Text";
    }
    
    public void Action_MidiFileOpen() { if (MidiFileDialog.ShowDialog() == DialogResult.OK) LoadMidiFile(MidiFileDialog.FileName); }

    void LoadMidiFile(string midiFile)
    {
      if (CheckFile(midiFile))
      {
        bool hasError = true;
        Exception ERR = null;
        if (MidiParser != null)
        {
          MidiParser.Dispose();
          midiParser = null;
        }
        try {
          Action_MidiFileOpen(midiFile, 0);
          hasError = false;
        } catch(Exception error) {
          ERR = error;
        }
        if (!hasError) Text = Strings.Dialog_Title_0;
        else
        {
          string filter =
            $"{ERR}\n\n"+
            "Would you like to [debug] throw exception?\n\n" +
            "• CANCEL to EXIT the application.\n" +
            "• NO to continue\n" +
            "• YES to throw the exception (for debugging)";
          switch (MessageBox.Show(filter, $"{ERR.Source}", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button3))
          {
            case DialogResult.Yes:
              //throw ERR;
              throw new Exception(ERR.Message, ERR);
            case DialogResult.Cancel: Application.Exit(); break;
          }

        }
      }
    }
    
    public void Action_MidiFileOpen(string filename, int trackNo)
    {
      if (string.IsNullOrEmpty(filename))
        return;
      if (!System.IO.File.Exists(filename)) {
        Log.ErrorMessage(filename, "Error loading file.");
        return;
      }
      
      Event_MidiClearMemory(null, EventArgs.Empty);

      Text = string.Format(
        Strings.Dialog_Title_1,
        System.IO.Path.GetFileNameWithoutExtension(filename));

      midiParser = new Reader(filename);

      MidiParser.ClearView -= Event_MidiClearMemory;
      MidiParser.FileLoaded -= Event_MidiFileLoaded;
      MidiParser.TrackChanged -= Event_MidiActiveTrackChanged_ListBoxItemSelected;

      MidiParser.ClearView += Event_MidiClearMemory;
      MidiParser.FileLoaded += Event_MidiFileLoaded;
      MidiParser.TrackChanged += Event_MidiActiveTrackChanged_ListBoxItemSelected;

      midiParser.Read();
      
      OnGotMidiFile();

      MidiParser.ReaderIndex = 0; // MidiParser.FileHandle.Format % 2 == 1

    }
    
    void Event_MidiFileOpen(object sender, EventArgs e)
    {
      Action_MidiFileOpen();
      //      this.midiPianoView1.ParserUI = this;
    }
    
  }
  
}


