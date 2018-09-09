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
    OpenFileDialog MidiFileDialog = new OpenFileDialog();
    
    int trackLen { get { return this.midiFile.SmfFileHandle[midiFile.SelectedTrackNumber].track.Length; } }
    
    // don't know what this was for any more!
    // nor do I get why it is causing an exception
    // (hence the try/catch-block in SetProgress(int))
    int cycles = 0, cycle = 12;
    
    void SetProgress(int offset)
    {
      this.toolStripProgressBar1.Value = (int)(((double)offset / trackLen) * 100f);
      try {
        cycles = (cycles++ % cycle);
      } catch {
      }
    }
    
    delegate void ProcessInt(int param);
    ProcessInt SetProgressDelegate;
    
    void ShowProgress(MidiMsgType t, int track, int offset, int imsg, byte bmsg, ulong ppq, int rse, bool isrse)
    {
      if (CanRaiseEvents) SetProgress(offset);
      try {
        Invoke(SetProgressDelegate, new object[]{offset});
      } catch {
        MessageBox.Show("Error invoking...");
      }
    }

    
    public IMidiParser MidiParser {
      get { return midiFile; }
    }
    
    protected internal on.smfio.MidiReader midiFile;
    /// <inheritdoc/>
    public ITimeConfiguration Settings { get { return TimeConfiguration.Instance; } }
    
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
      this.numPpq.Value = Settings.Division;
      this.numTempo.Value = Convert.ToDecimal(Settings.Tempo);
      if (GotMidiFile != null)
        GotMidiFile(this, EventArgs.Empty);
    }
    
    #endregion
    
    // internal event EventHandler Event_MidiClearMemory;
    // internal event EventHandler Event_MidiFileLoaded;
    void Event_MidiFileLoaded(object sender, EventArgs e)
    {
      TracksToToolStripMenu();
      TracksToListBox();
      TracksToListBoxContext();
      MidiTree.TracksToTreeView(this);
      LoadTracks = midiFile.TrackSelectAction;
    }
    
    #region MIDI ListBox (Event_MidiChangeTrack_MenuItemSelected,Event_FormToggleMidiListBox)
    void Event_MidiChangeTrack_MenuItemSelected(object sender, EventArgs e)
    {
      cycles = 0;
      cycle = trackLen > 230 ? 1 : (int)(trackLen * 0.07f);
      this.toolStripProgressBar1.Maximum = 100;
      this.toolStripProgressBar1.Enabled = true;
      
      OnClearMidiTrack();
      if (midiFile == null)
        return;
      if (sender is ToolStripMenuItem)
        midiFile.SelectedTrackNumber = (int)(sender as ToolStripMenuItem).Tag;
      else if (sender is ListBox && listBox1.SelectedIndex > -1)
        midiFile.SelectedTrackNumber = listBox1.SelectedIndex;
    }
    void Event_FormToggleMidiListBox(object sender, EventArgs e)
    {
      this.splitContainer1.Panel1Collapsed = !this.splitContainer1.Panel1Collapsed;
    }
    #endregion
    #region MIDI ListBox (TracksToListBox,TracksToToolStripMenu)
    void TracksToToolStripMenu()
    {
      foreach (KeyValuePair<int,string> track in midiFile.GetMidiTrackNameDictionary()) {
        var tn = new ToolStripMenuItem(track.Value, null, Event_MidiChangeTrack_MenuItemSelected);
        tn.Tag = track.Key;
        btn_pick_track.DropDownItems.Add(tn);
      }
    }
    void TracksToListBox()
    {
      listBox1.DataSource = btn_pick_track.DropDownItems;
      listBox1.DisplayMember = "Text";
    }
    #endregion
    
    public void Action_MidiFileOpen()
    {
      if (midiFile != null) {
        midiFile.Dispose();
        midiFile = null;
      }
      
      MidiFileDialog.Filter = Strings.FileFilter_MidiFile;
      Text = Strings.Dialog_Title_0;
      
      if (MidiFileDialog.ShowDialog() == DialogResult.OK)
        if (File.Exists(MidiFileDialog.FileName))
          Action_MidiFileOpen(MidiFileDialog.FileName, 0);
    }
    
    public void Action_MidiFileOpen(string filename, int trackNo)
    {
      Event_MidiClearMemory(null, EventArgs.Empty);
      
      if (string.IsNullOrEmpty(filename))
        return;
      if (!System.IO.File.Exists(filename)) {
        MessageBox.Show(filename, "Error loading file.");
        return;
      }
      
      Text = string.Format(
        Strings.Dialog_Title_1,
        System.IO.Path.GetFileNameWithoutExtension(filename));
      
      midiFile = new MidiReader(filename);
      midiFile.SelectedTrackNumber = trackNo;
      
      midiFile.ClearView -= Event_MidiClearMemory;
      midiFile.FileLoaded -= Event_MidiFileLoaded;
      midiFile.TrackChanged -= Event_MidiActiveTrackChanged_ListBoxItemSelected;
      
      midiFile.ClearView += Event_MidiClearMemory;
      midiFile.FileLoaded += Event_MidiFileLoaded;
      midiFile.TrackChanged += Event_MidiActiveTrackChanged_ListBoxItemSelected;
      #if DEBUG
      midiFile.MessageHandlers.Add(ShowProgress);
      #endif
      
      midiFile.Read();
      Settings.FromMidi(midiFile);
      //foreach (Action a in afteropen) a();
      
      OnGotMidiFile();
      
    }
    
    void Event_MidiFileOpen(object sender, EventArgs e)
    {
      Action_MidiFileOpen();
      //      this.midiPianoView1.ParserUI = this;
    }
  }
  
}


