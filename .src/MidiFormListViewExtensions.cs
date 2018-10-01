/*
 * Created by SharpDevelop.
 * User: tfooo
 * Date: 11/12/2005
 * Time: 4:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
namespace SMFIOViewer
{
  static public class MidiFormListViewExtensions
  {
    static public ListViewItem AddItem(this ListView lvx, System.Drawing.Color colar, params string[] content)
    {
      return lvx.Items.Add(new ListViewItem(content){BackColor=colar});
    }
    static public ListViewItem AddItem(this ListView lvx, long pulse, System.Drawing.Color colar, params string[] content)
    {
      return lvx.Items.Add(new ListViewItem(content)
      {
        BackColor = colar,
        ToolTipText = $"{pulse}",
      });
    }
    static public ListViewItem AddItem(this ListView lvx, on.smfio.TempoState tempo, long pulse, System.Drawing.Color colar, params string[] content)
    {
      var item = lvx.AddItem(pulse, colar, content);
      item.ToolTipText = $"Ticks: {pulse}, LastTempo-Ticks: {tempo.Pulse}, diff: {pulse - tempo.Pulse}";
      return item;
    }


    static public ListViewItem AddItem(this ListView lvx, params string[] content)
		{
			return lvx.Items.Add(new ListViewItem(content));
		}
	}
}




