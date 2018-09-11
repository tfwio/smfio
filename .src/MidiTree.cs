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
			if (ui.MidiParser.SmfFileHandle==null) return;
			if (ui.MidiParser.SmfFileHandle.NumberOfTracks==0) return;
			
			for (int i = 0; i < ui.MidiParser.SmfFileHandle.NumberOfTracks; i++)
			{
				var tn = new TreeNode(string.Format("{0}",/*Strings.Filter_MidiTrack*/ i )); //Event_MidiChangeTrack_MenuItemSelected
				tn.Tag = i;
				NodeMidi.Nodes.Add( tn );
			}
			
			for (int i = 0; i < ui.MidiParser.TempoChanges.Count; i++)
			{
			  var T = ui.MidiParser.TempoChanges[i];
			  var C = new on.smfio.util.SampleClock(0,44100,T.TempoValue,ui.MidiParser.SmfFileHandle.Division){Rate=44100};
			  // C.SolveSamples();
			  string F = string.Format(
			    "TK: {0}, Pulses: {1}, BPM: ~{2:0.00000}, MSPQ: {3}",
			    T.TrackID, T.Pulses, T.TempoValue, T.MSPQ
			   );
			  NodeTempo.Nodes.Add(new TreeNode(F));
			}
		}
		
		
	}

}
