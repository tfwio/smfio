/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */

namespace on.smfio
{
  public class EnumFile
  {
    string[] data_map;
    
    public string this[int index] { get { return (index > (Length - 1)) ? null : data_map[index]; } }
    
    public int Length { get { return data_map.Length; } }
    
    public EnumFile(string file) { Load(file); }
    
    void Load(string file)
    {
      if (!System.IO.File.Exists(file))
      {
        Log.ErrorMessage($"Error: EnumFile: {file} not found!");
        return;
      }
      try
      {
        using (var bi = System.IO.File.OpenText(file))
        {
          string spoof = bi.ReadToEnd();
          data_map = spoof.Split(new char[] { (char)0x0D });
          bi.Close();
        }
      }
      catch
      {
        Log.ErrorMessage($"Error[unknown] reading EnumFile {file}");
      }
    }
  }
}
