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
    static public void AddItem(this ListView lvx, System.Drawing.Color colar, params string[] content)
    {
      var item = new ListViewItem(content);
      item.BackColor = colar;
      lvx.Items.Add(item);
    }
    static public void AddItem(this ListView lvx, long pulse, System.Drawing.Color colar, params string[] content)
    {
      var item = new ListViewItem(content);
      item.BackColor = colar;
      item.ToolTipText = pulse.ToString();
      lvx.Items.Add(item);
    }

		static public void AddItem(this ListView lvx, params string[] content)
		{
			lvx.Items.Add(new ListViewItem(content));
		}
	}
}




