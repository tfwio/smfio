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
namespace SMFIOViewer
{
  public interface IMidiViewContainer : System.Internals.IViewPoint<MidiControlBase>
  {
  }
	public class MasterViewContainer : System.Internals.ViewPoint<MidiControlBase>, IMidiViewContainer
	{
		public MasterViewContainer()
		{
			this.ViewType = System.Internals.ViewPointType.UserControl;
		}
	}
}


