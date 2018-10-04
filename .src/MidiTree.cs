/*
 * User: tfooo
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.Windows.Forms;

namespace SMFIOViewer
{
	static class MidiTree
	{
		internal static TreeNode NodeMidi, NodeTempo;
		static public void InitializeTreeNodes(TreeView tree, IMidiParserUI ui)
		{
			WindowsInterop.WindowsTheme.HandleTheme(tree,true);
			tree.Nodes.Clear();
      NodeMidi=new TreeNode("MIDI");
      NodeTempo=new TreeNode("TEMPO");
      NodeTempo.ToolTipText = "set tempo events";
      tree.Nodes.Add(NodeMidi);
			tree.Nodes.Add(NodeTempo);
		}
		static public void TracksToTreeView(IMidiParserUI ui)
		{
      NodeMidi.Nodes.Clear();
			NodeTempo.Nodes.Clear();
			
			if (ui.MidiParser==null) return;
			if (ui.MidiParser.FileHandle==null) return;
			if (ui.MidiParser.FileHandle.NumberOfTracks==0) return;
			
			for (int i = 0; i < ui.MidiParser.FileHandle.NumberOfTracks; i++)
			{
				var tn = new TreeNode(string.Format("{0}",/*Strings.Filter_MidiTrack*/ i )); //Event_MidiChangeTrack_MenuItemSelected
				tn.Tag = i;
				NodeMidi.Nodes.Add( tn );
			}

      var map = ui.MidiParser.TempoMap.Copy();
			while (map.HasItems)
			{
        var T = map.Pop(true);
        var min = on.smfio.TimeUtil.GetMBT(T.Pulse, ui.MidiParser.FileHandle.Division);
        var max = on.smfio.TimeUtil.GetMBT(T.PulseMax, ui.MidiParser.FileHandle.Division);
				var ss = on.smfio.TimeUtil.GetSSeconds(T.Second);
			  NodeTempo.Nodes.Add(new TreeNode($"Pulses: {min}-{max}, SS={ss}, BPM: {(float)T.Tempo} muspqn: {T.MusPQN:##,###,##0}, {T.Second:0.000}"));
			}
		}
	}
}
