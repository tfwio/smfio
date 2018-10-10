/*
 * Created by SharpDevelop.
 * User: tfooo
 * Date: 11/12/2005
 * Time: 4:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Windows.Forms;
using on.smfio;
using on.smfio.Common;
using on.smfio.util;

namespace SMFIOViewer
{
	public class MidiEventControl : MidiControlBase
	{
		IReader Reader { get { return UserInterface.MidiParser; } }
		TempoMap map = null;

		#region ListView/Registry
		const string regpath = @"Software\tfoxo\smfio";
		const string reg_list_columns 	= "EventList2.Columns[]";
		const string reg_list_fontsize 	= "EventList2.FontSize";
		
		[System.ComponentModel.BrowsableAttribute(false),System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public string ListColumns {
			get { return Reg.GetKeyValueString(regpath,reg_list_columns); }
			set { Reg.SetKeyValueString(regpath,reg_list_columns,value); Debug.Print("Saving Col Sizes: {0}",value); }
		}
		[System.ComponentModel.BrowsableAttribute(false),System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public string ListFontSize {
			get { return Reg.GetKeyValueString(regpath,reg_list_fontsize); }
			set { Reg.SetKeyValueString(regpath,reg_list_fontsize,value); }
		}
		
		/// <summary>
		/// Get: Returns integer values from ListView.
		/// <para>set: Sets values to the list view.</para>
		/// </summary>
		int[] ListColSize
		{
			get
			{
				Debug.Print("Loading Column Sizes");
				var cols = new int[lve.Columns.Count];
				for (int i =0;i < lve.Columns.Count; i++)
					cols[i] = lve.Columns[i].Width;
				return cols;
			}
			set
			{
				Debug.Print("Setting Col Size");
				for (int i =0;i < lve.Columns.Count; i++)
					lve.Columns[i].Width = value[i];
			}
		}
		
		internal void FontLarger(object sender, EventArgs e)
		{
			Reg.SetControlFontSize(lve,12);
			ListFontSize = "12";
		}
		internal void FontSmaller(object sender, EventArgs e)
		{
			Reg.SetControlFontSize(lve,9);
			ListFontSize = "9";
		}
		public override void ApplyRegistrySettings()
		{
			Debug.Print("Loading Settings for MidiListView");
			try
			{
				if (!string.IsNullOrEmpty(ListFontSize))
				{
					Reg.SetControlFontSize(this.lve, float.Parse(ListFontSize));
				}
			}
			catch
			{
				Debug.Print("Error updating font size");
			}
			try
			{
				Debug.Print("got value: {0}", ListColumns);
				if (string.IsNullOrEmpty(ListColumns))
				{
					Debug.Print("got error!: {0}", ListColumns);
				}
				var vals = Reg.TranslateString(ListColumns);
				if (vals != null) this.ListColSize = vals;
			}
			catch
			{
				Debug.Print("Error updating list-col size");
			}
			Debug.Print("Adding Col-Resized Event");

			this.lve.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.LveColumnWidthChanged);
		}
		void SaveColumnSetting()
		{
			string v = Reg.TranslateString(ListColSize);
			Debug.Print("Saving Col Sizes");
			Debug.Print(v);
			if (!string.IsNullOrEmpty(v)) ListColumns = v;
			ListFontSize = lve.Font.SizeInPoints.ToString();
		}

		void LveColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			SaveColumnSetting();
		}

		#endregion
		
		public MidiEventControl() : base()
		{
			this.InitializeComponent();
			this.Text = "Event View";
			WindowsInterop.WindowsTheme.HandleTheme(this.lve);
			this.lve.ShowItemToolTips = true;
		}
		public MidiEventControl(IMidiParserUI parserUI) : this()
		{
			SetUI(parserUI);
		}
		
		#region Overrides
		
		public override void SetUI(IMidiParserUI ui)
		{
			base.SetUI(ui);
			ApplyRegistrySettings();
		}
		public override void ClearTrack(object sender, EventArgs e)
		{
			base.ClearTrack(sender, e);
			lve.Items.Clear();
		}
		public override void FileLoaded(object sender, EventArgs e)
		{
			base.FileLoaded(sender, e);
			
			if (!Reader.MessageHandlers.Contains(GotMidiEventD))
				Reader.MessageHandlers.Add(GotMidiEventD);
		}
		public override void BeforeTrackLoaded(object sender, EventArgs e)
		{
			Debug.Print("Midi Event List Hidden");
			this.lve.Visible = false;
			base.BeforeTrackLoaded(sender, e);
			map = Reader.TempoMap.Copy();
		}
		public override void AfterTrackLoaded(object sender, EventArgs e)
		{
			Debug.Print("Midi Event List Shown");
			base.AfterTrackLoaded(sender, e);
			this.lve.Visible = true;
		}
		
		#endregion

		int Division { get { return Reader.FileHandle.Division; } }
		
		void GotMidiEventD(MidiMsgType msgType, int nTrackIndex, int nTrackOffset, int midiMsg32, byte midiMsg8, long pulse, int statusRunning, bool isRunningStatus)
		{
			if (Reader.TempoMap.Top.Pulse > 0) Reader.TempoMap.Finalize(Reader, true);
      var tempo = (!map.Top.Match(pulse) ? map.Seek(pulse) : map.Top) ?? TempoState.Default;
      
			double seconds = TimeUtil.GetSeconds(Division, tempo.MusPQN, (long)pulse-tempo.Pulse, tempo.Second);
			string sseconds = TimeUtil.GetSSeconds(seconds);
			string smbt = TimeUtil.GetMBT((long)pulse, Division);

			switch (msgType)
			{
				case MidiMsgType.MetaStr:
					var item = lve.AddItem( tempo, pulse, ColorResources.c4, smbt, sseconds, string.Empty, MetaHelpers.MetaNameFF( midiMsg32 ), Reader.GetMetadataString( nTrackOffset ) );
					break;
				case MidiMsgType.MetaInf:
					lve.AddItem(tempo, pulse, ColorResources.GetEventColor(midiMsg32, ColorResources.cR, Reader.CurrentRunningStatus8), smbt, sseconds, string.Empty, MetaHelpers.MetaNameFF(midiMsg32), Reader.GetMessageString(nTrackOffset));
					break;
				case MidiMsgType.SequencerSpecific:
					lve.AddItem(tempo, pulse, ColorResources.GetEventColor(midiMsg32, ColorResources.cR, Reader.CurrentRunningStatus8), smbt, sseconds, string.Empty, MetaHelpers.MetaNameFF(midiMsg32), Reader.GetMessageString(nTrackOffset));
					break;
				case MidiMsgType.SystemExclusive:
					var bytes = Reader[nTrackIndex, nTrackOffset, Reader[nTrackIndex].GetEndOfSystemExclusive(nTrackOffset) - nTrackOffset];
					lve.AddItem(tempo, pulse, ColorResources.GetEventColor(midiMsg32, ColorResources.cR, Reader.CurrentRunningStatus8), smbt, sseconds, string.Empty, MetaHelpers.MetaNameFF(midiMsg32), Reader.GetMessageString(nTrackOffset));
					break;
				case MidiMsgType.EOT:
					lve.AddItem(tempo, pulse, ColorResources.GetEventColor(midiMsg32, ColorResources.cR, Reader.CurrentRunningStatus8), smbt, sseconds, string.Empty, MetaHelpers.MetaNameFF(midiMsg32), Reader.GetMessageString(nTrackOffset));
					break;
				default: // expecting channel voice messages
					if (isRunningStatus) lve.AddItem( tempo, pulse, ColorResources.GetRseEventColor( ColorResources.Colors["225"], Reader.CurrentRunningStatus8 ), smbt, sseconds, midiMsg8==0xF0 ? "" :(statusRunning & 0x0F).ToString(), Reader.GetRseEventString( nTrackOffset ), Reader.chRseV( nTrackOffset ) );
					else                 lve.AddItem( tempo, pulse, ColorResources.GetEventColor   ( ColorResources.Colors["225"], Reader.CurrentRunningStatus8 ), smbt, sseconds, midiMsg8==0xF0 ? "" :(statusRunning & 0x0F).ToString(), Reader.GetEventString   ( nTrackOffset ), Reader.chV   ( nTrackOffset ) );
					break;
			}
		}
		
		#region Design
		private System.ComponentModel.IContainer components;
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		void InitializeComponent()
		{
			this.lve = new System.Windows.Forms.ListView();
			this.cctime = new System.Windows.Forms.ColumnHeader();
			this.ccchanel = new System.Windows.Forms.ColumnHeader();
			this.ccevent = new System.Windows.Forms.ColumnHeader();
			this.ccmsg = new System.Windows.Forms.ColumnHeader();
			this.cchhmmss = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// lve
			// 
			this.lve.BackColor = System.Drawing.SystemColors.Window;
			this.lve.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lve.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																	this.cctime,
																	this.cchhmmss,
																	this.ccchanel,
																	this.ccevent,
																	this.ccmsg});
			this.lve.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lve.Font = new System.Drawing.Font("FreeMono", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.lve.FullRowSelect = true;
			this.lve.LabelWrap = false;
			this.lve.Location = new System.Drawing.Point(0, 0);
			this.lve.MultiSelect = false;
			this.lve.Name = "lve";
			this.lve.Size = new System.Drawing.Size(575, 285);
			this.lve.TabIndex = 17;
			this.lve.UseCompatibleStateImageBehavior = false;
			this.lve.View = System.Windows.Forms.View.Details;
			// 
			// cctime
			// 
			this.cctime.Text = "MBT";
			this.cctime.Width = 130;
			// 
			// ccchanel
			// 
			this.ccchanel.Text = "@";
			// 
			// ccevent
			// 
			this.ccevent.Text = "Info";
			this.ccevent.Width = 181;
			// 
			// ccmsg
			// 
			this.ccmsg.Text = "Byte Data";
			this.ccmsg.Width = 304;
			// 
			// cchhmmss
			// 
			this.cchhmmss.Text = "hh:mm:ss";
			// 
			// MidiListView
			// 
			this.Controls.Add(this.lve);
			this.Name = "MidiListView";
			this.Size = new System.Drawing.Size(575, 285);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ColumnHeader cchhmmss;
		private System.Windows.Forms.ColumnHeader ccmsg;
		private System.Windows.Forms.ColumnHeader ccevent;
		private System.Windows.Forms.ColumnHeader ccchanel;
		private System.Windows.Forms.ColumnHeader cctime;
		internal System.Windows.Forms.ListView lve;
		#endregion
		
	}
}
