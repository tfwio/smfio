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
		static TreeNode NodeMidi;
		static public void InitializeTreeNodes(TreeView tree, IMidiParserUI ui)
		{
			WindowsInterop.WindowsTheme.HandleTheme(tree,true);
			tree.Nodes.Clear();
			NodeMidi=new TreeNode("MIDI");
			
			tree.Nodes.Add(NodeMidi);
		}
		static public void TracksToTreeView(IMidiParserUI ui)
		{
			NodeMidi.Nodes.Clear();
			if (ui.MidiParser==null) return;
			if (ui.MidiParser.SmfFileHandle==null) return;
			if (ui.MidiParser.SmfFileHandle.NumberOfTracks==0) return;
			
			for (int i = 0; i < ui.MidiParser.SmfFileHandle.NumberOfTracks; i++)
			{
				var tn = new TreeNode(string.Format("{0}",/*Strings.Filter_MidiTrack*/ i )); //Event_MidiChangeTrack_MenuItemSelected
				tn.Tag = i;
				NodeMidi.Nodes.Add( tn );
			}
		}
		
		
	}

}
