/*
 * User: xo
 * Date: 9/8/2018
 * Time: 3:38 AM
 */
using System;
using System.Collections.Generic;
using System.Internals;
using System.Windows.Forms;

namespace SMFIOViewer
{
  /// <summary>
  /// Class with program entry point.
  /// </summary>
  internal sealed class Program
  {
    static List<MasterViewContainer> ViewCollection;
    /// <summary>
    /// Program entry point.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      ViewMaster<MasterViewContainer,MidiControlBase> Viewer = new ViewMaster<MasterViewContainer,MidiControlBase>(System.Reflection.Assembly.GetExecutingAssembly());
      Application.Run(new MainForm(Viewer.ViewCollection));
      Application.Exit();
    }
    
  }
}
