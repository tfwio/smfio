/*
 * User: xo
 * Date: 9/8/2018
 * Time: 3:38 AM
 */
namespace SMFIOViewer
{
  partial class MainForm
  {
    /// <summary>
    /// Designer variable used to keep track of non-visual components.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.TreeView tree;
    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripDropDownButton viewToolStripMenuItem;
    private System.Windows.Forms.ContextMenuStrip listBoxContextMenuStrip;
    private System.Windows.Forms.ToolStripButton toolStripButton1;
    private System.Windows.Forms.ToolStripDropDownButton btn_pick_track;
    private System.Cor3.Forms.Controls.ToolStripUpDown numPpq;
    private System.Cor3.Forms.Controls.ToolStripUpDown numTempo;
    private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
    
    /// <summary>
    /// Disposes resources used by the form.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing) {
        if (components != null) {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }
    
    /// <summary>
    /// This method is required for Windows Forms designer support.
    /// Do not change the method contents inside the source code editor. The Forms designer might
    /// not be able to load this method if it was changed manually.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.tree = new System.Windows.Forms.TreeView();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.listBoxContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.label1 = new System.Windows.Forms.Label();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
      this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripDropDownButton();
      this.btn_pick_track = new System.Windows.Forms.ToolStripDropDownButton();
      this.numTempo = new System.Cor3.Forms.Controls.ToolStripUpDown();
      this.numPpq = new System.Cor3.Forms.Controls.ToolStripUpDown();
      this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainer1
      // 
      this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitContainer1.Location = new System.Drawing.Point(0, 26);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.label1);
      this.splitContainer1.Size = new System.Drawing.Size(587, 298);
      this.splitContainer1.SplitterDistance = 184;
      this.splitContainer1.TabIndex = 19;
      // 
      // splitContainer2
      // 
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer2.Location = new System.Drawing.Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this.tree);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.listBox1);
      this.splitContainer2.Size = new System.Drawing.Size(184, 298);
      this.splitContainer2.SplitterDistance = 146;
      this.splitContainer2.TabIndex = 0;
      // 
      // tree
      // 
      this.tree.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tree.FullRowSelect = true;
      this.tree.HideSelection = false;
      this.tree.HotTracking = true;
      this.tree.LineColor = System.Drawing.Color.Silver;
      this.tree.Location = new System.Drawing.Point(0, 0);
      this.tree.Name = "tree";
      this.tree.ShowNodeToolTips = true;
      this.tree.ShowRootLines = false;
      this.tree.Size = new System.Drawing.Size(184, 146);
      this.tree.TabIndex = 0;
      // 
      // listBox1
      // 
      this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.listBox1.ContextMenuStrip = this.listBoxContextMenuStrip;
      this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listBox1.FormattingEnabled = true;
      this.listBox1.IntegralHeight = false;
      this.listBox1.Location = new System.Drawing.Point(0, 0);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new System.Drawing.Size(184, 148);
      this.listBox1.TabIndex = 17;
      this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Event_MidiChangeTrack);
      // 
      // listBoxContextMenuStrip
      // 
      this.listBoxContextMenuStrip.Font = new System.Drawing.Font("Consolas", 7F);
      this.listBoxContextMenuStrip.Name = "listBoxContextMenuStrip";
      this.listBoxContextMenuStrip.ShowImageMargin = false;
      this.listBoxContextMenuStrip.Size = new System.Drawing.Size(36, 4);
      // 
      // label1
      // 
      this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.label1.ForeColor = System.Drawing.Color.Silver;
      this.label1.Location = new System.Drawing.Point(0, 266);
      this.label1.Margin = new System.Windows.Forms.Padding(0);
      this.label1.Name = "label1";
      this.label1.Padding = new System.Windows.Forms.Padding(4);
      this.label1.Size = new System.Drawing.Size(399, 32);
      this.label1.TabIndex = 0;
      this.label1.Text = "label1";
      this.label1.UseCompatibleTextRendering = true;
      // 
      // toolStrip1
      // 
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
      this.toolStripButton1,
      this.viewToolStripMenuItem,
      this.btn_pick_track,
      this.numTempo,
      this.numPpq,
      this.toolStripProgressBar1});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
      this.toolStrip1.Size = new System.Drawing.Size(587, 26);
      this.toolStrip1.TabIndex = 21;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toolStripButton1
      // 
      this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton1.Name = "toolStripButton1";
      this.toolStripButton1.Size = new System.Drawing.Size(34, 23);
      this.toolStripButton1.Text = "SMF";
      this.toolStripButton1.Click += new System.EventHandler(this.Event_MidiFileOpen);
      // 
      // viewToolStripMenuItem
      // 
      this.viewToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.viewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
      this.viewToolStripMenuItem.Size = new System.Drawing.Size(47, 23);
      this.viewToolStripMenuItem.Text = "VIEW";
      // 
      // btn_pick_track
      // 
      this.btn_pick_track.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.btn_pick_track.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btn_pick_track.Name = "btn_pick_track";
      this.btn_pick_track.Size = new System.Drawing.Size(57, 23);
      this.btn_pick_track.Text = "TRACK";
      // 
      // numTempo
      // 
      this.numTempo.MaxValue = new decimal(new int[] {
      800,
      0,
      0,
      0});
      this.numTempo.MinValue = new decimal(new int[] {
      20,
      0,
      0,
      0});
      this.numTempo.Name = "numTempo";
      this.numTempo.Size = new System.Drawing.Size(41, 23);
      this.numTempo.Text = "120";
      this.numTempo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.numTempo.ThousandsSeparator = false;
      this.numTempo.ToolTipText = "Tempo";
      this.numTempo.Value = new decimal(new int[] {
      120,
      0,
      0,
      0});
      // 
      // numPpq
      // 
      this.numPpq.MaxValue = new decimal(new int[] {
      999999,
      0,
      0,
      0});
      this.numPpq.MinValue = new decimal(new int[] {
      24,
      0,
      0,
      0});
      this.numPpq.Name = "numPpq";
      this.numPpq.Size = new System.Drawing.Size(59, 23);
      this.numPpq.Text = "480";
      this.numPpq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.numPpq.ThousandsSeparator = false;
      this.numPpq.ToolTipText = "PPQ (Division)";
      this.numPpq.Value = new decimal(new int[] {
      480,
      0,
      0,
      0});
      // 
      // toolStripProgressBar1
      // 
      this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripProgressBar1.Name = "toolStripProgressBar1";
      this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 23);
      this.toolStripProgressBar1.Visible = false;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(587, 324);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.toolStrip1);
      this.Name = "MainForm";
      this.Text = "SMFIOViewer";
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
