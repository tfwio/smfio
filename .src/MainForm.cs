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

namespace SMFIOViewer
{

  /// <summary>
  /// Description of MainForm.
  /// </summary>
  public partial class MainForm : Form
  {
    string startupfile = null;
    static bool CheckFile(params string[] args)
    {
      foreach (var arg in args)
      {
        var ext = Path.GetExtension(args[0]).ToLower();
        if (!File.Exists(args[0])) return false;
        if (!(ext == ".mid" || ext == ".midi")) return false;
      }
      return true;
    }

    // public MainForm(params string[] args) { InitializeComponent(); }
    public MainForm(IList<MasterViewContainer> tasks, params string[] args)
    {
      mSettings = new TimeConfiguration() {
        // AUDIO
        Rate = 44100,
        Channels = 2,
        // Latency = 8096,
        // MIDI
        Division = 480,
        Tempo = 120,
        TimeSignature = new MidiTimeSignature(4, 4, 24, 4),
        KeySignature = new MidiKeySignature(on.smfio.Common.KeySignatureType.C, true),
        IsSingleZeroChannel = false
      };
      InitializeComponent();
      
      // SetProgressDelegate = SetProgress;
      
      numPpq.NumericUpDownControl.Increment = 24;
      this.InitializeModestForm(tasks);
      numPpq.NumericUpDownControl.ReadOnly = true;
      numTempo.NumericUpDownControl.ReadOnly = true;

      if ((args.Length > 0) && CheckFile(args[0]))
        startupfile = args[0];
    }

    protected override void OnLoad(EventArgs e)
    {
      Action_MidiFileOpen(startupfile, 0);
    }
    
    #region Action_PlayerUpDown2Ppq
    void Action_PlayerUpDown2Ppq()
    {
      Settings.Division = Convert.ToInt32(numPpq.Value);
    }
    void Event_PlayerUpDown2Ppq(object sender, EventArgs e)
    {
      Action_PlayerUpDown2Ppq();
    }
    #endregion
    
    #region MIDI (Clear) Memory
    /// <remarks>Action_ClearMemory: Reset (ListBox) listBox1 and (ToolStripMenuItem) btn_pick_track bound to Midi</remarks>
    void Event_MidiClearMemory(object sender, EventArgs e)
    {
      Action_ClearMemory();
    }
    
    /// <summary>Clear Midi (Parsed) Memory, Unload Midi Track Lists, Etc,,</summary>
    /// <remarks>Reset (ListBox) listBox1 and (ToolStripMenuItem) btn_pick_track bound to Midi</remarks>
    void Action_ClearMemory()
    {
      btn_pick_track.DropDownItems.Clear();
      listBox1.DataSource = null;
    }
    
    #endregion
    
    void DoTrackSelect()
    {
      //#if DEBUG
      StartLoad();
      //#else
      //KillThread();
      //thread = new System.Threading.Thread(StartLoad);
      //thread.Priority = System.Threading.ThreadPriority.Highest;
      //thread.Start();
      ////#endif
      //this.toolStripProgressBar1.Value = 0;
      //this.toolStripProgressBar1.Maximum = 0;
      //this.toolStripProgressBar1.Enabled = false;
      //this.toolStripProgressBar1.Visible = true;
    }

    void Event_MidiActiveTrackChanged_ListBoxItemSelected(object o, EventArgs e)
    {
      DoTrackSelect();
    }
    
    void TracksToListBoxContext()
    {
      listBoxContextMenuStrip.Items.Clear();
      List<int> channels = new List<int>();
      foreach (KeyValuePair<int,string> track in midiFile.GetMidiTrackNameDictionary())
      {
        channels.Clear();
        ToolStripMenuItem tn = new ToolStripMenuItem(track.Value);
        tn.Tag = track.Key;
        listBoxContextMenuStrip.Items.Add(tn);
        
        foreach (MidiMessage i in midiFile.MidiTrackDistinctChannels(track.Key))
          if (i is MidiChannelMessage)
            channels.Add(i.ChannelBit);
        // foreach (int i in channels) {
        //   tn.DropDownItems.Add(i.ToString(), null, Event_ListBoxContextMenuItem)
        //     .Tag = new KeyValuePair<int,int>(track.Key, i);
        //
        // }
        //        if (channels.Count > 0) tn.DropDownItems.Insert(0, new ToolStripSeparator());
        // if (channels.Count > 0) // all channels node // only added to tracks that have channels
        // {
        //   ToolStripItem another = new ToolStripMenuItem("All Channels", null, Event_ListBoxContextMenuItem);
        //   another.Tag = new KeyValuePair<int,int>(track.Key, -1);
        //   tn.DropDownItems.Insert(0, another);
        // }
      }
      channels.Clear();
      channels = null;
    }
    
    void InitializeModestForm(IList<MasterViewContainer> tasks)
    {
      // initialize the views
      this.InitializeEnumerable(tasks);
      
      MidiTree.InitializeTreeNodes(this.tree, this);
      
      // there is however no player.
      // this.numTempo.ValueChanged += Event_PlayerUpDown2Tempo;
      // this.numPpq.ValueChanged += this.Event_PlayerUpDown2Ppq;
    }
    
    public List<MidiControlBase> ChildrenControls = new List<MidiControlBase>();
    
    //
    void InitializeEnumerable(IEnumerable<MasterViewContainer> tasks)
    {
      foreach (MasterViewContainer view in tasks)
      {
        MidiControlBase control = view.GetView();
        control.SuspendLayout();
        this.ChildrenControls.Add(control);
        this.splitContainer1.Panel2.Controls.Add(control);
        control.Show();
        control.Dock = DockStyle.Fill;
        control.SetUI(this);
        control.BringToFront();
        control.ResumeLayout(true);
        ToolStripItem item = new ToolStripMenuItem(view.Title, null, ShowElement);
        item.Tag = control;
        viewToolStripMenuItem.DropDownItems.Add(item);
      }
      // btn_pick_track.Image = Icons.midi_in;
      // btn_pick_track.Image = fam3.famfam_silky.cursor;
      // #if !DEBUG
      // toolStripProgressBar1.Visible = false;
      // #endif
    }
    
    public void ShowElement(object sender, EventArgs e)
    {
      foreach (Control ctl in ChildrenControls) {
        ctl.Hide();
      }
      ((sender as ToolStripMenuItem).Tag as Control).BringToFront();
      ((sender as ToolStripMenuItem).Tag as Control).Show();
      ((sender as ToolStripMenuItem).Tag as Control).Invalidate();
    }
    
    #region Thread Helpers for MIDI Track Loader
    
    /// <summary>
    /// This is a placeholder for midiFile.TrackSelectAction
    /// </summary>
    /// <seealso cref="on.smfio.MidiReader.TrackSelectAction()">on.smfio.MidiReader.TrackSelectAction()</seealso>
    Func<string> LoadTracks = null;
    
    System.Threading.Thread thread; // FIXME: Is this never used?
    
    void StartLoad()
    {
      label1.Text = LoadTracks();
    }
    void KillThread()
    {
      if (thread != null) {
        if (thread.IsAlive)
          try {
          thread.Abort();
        } catch {
        }
        thread = null;
      }
    }
    
    #endregion
    
  }
  
}
