/*
 * Created by SharpDevelop.
 * User: tfooo
 * Date: 11/12/2005
 * Time: 4:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
namespace SMFIOViewer
{
  public class Reg
  {
    static public int[] TranslateString(string input, string delimiter = ",")
    {
      if (string.IsNullOrEmpty(input))
        return null;
      if (!input.Contains(delimiter))
        return null;
      string[] a = input.Split(',');
      int[] l = new int[a.Length];
      for (int i = 0; i < a.Length; i++)
        l[i] = int.Parse(a[i]);
      a = null;
      GC.Collect();
      return l;
    }

    static public string TranslateString(int[] input, string delimiter = ",")
    {
      if (input == null)
        return null;
      List<string> list = new List<string>();
      foreach (int i in input)
        list.Add(i.ToString());
      string[] l = list.ToArray();
      string value = string.Join(delimiter, l);
      l = null;
      list = null;
      GC.Collect();
      return value;
    }

    static public void SetControlFontSize(System.Windows.Forms.Control listview, float size)
    {
      listview.Font = new System.Drawing.Font(listview.Font.FontFamily, size, System.Drawing.GraphicsUnit.Point);
    }

    /// <summary>
    /// Current User
    /// </summary>
    /// <param name="path"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    static public string GetKeyValueString(string path, string key)
    {
      string value = null;
      using (Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path, false)) {
        if (rkey == null)
          return null;
        value = string.Format("{0}", rkey.GetValue(key));
        rkey.Close();
      }
      return value;
    }

    static public void SetKeyValueString(string path, string key, object value)
    {
      Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path, true);
      if (rkey == null) {
        rkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path, Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);
      }
      if (value != null)
        rkey.SetValue(key, value, Microsoft.Win32.RegistryValueKind.String);
      rkey.Close();
      rkey = null;
      GC.Collect();
    }
  }
  
}


