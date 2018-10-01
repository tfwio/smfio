/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */

namespace on.smfio
{
  static class x0 {
    //"on.smfio.ext.cc.map"
    static public string[] GetEmbeddedResourceString(this System.Reflection.Assembly asm, string resID, char separator = (char)0x0A)
    {
      string[] result = null;
      using (var strm = typeof(EnumFile).Assembly.GetManifestResourceStream(resID))
        using (var reader = new System.IO.StreamReader(strm))
      {
        result = reader.ReadToEnd().Split(separator);
      }
      return result;
    }
    static public string[] GetLines(this string enumText, char split=(char)0x0A)
    {
      var abs = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().FullName, enumText));
      if (System.IO.File.Exists(abs))
        return System.IO.File.ReadAllText(enumText, System.Text.Encoding.UTF8).Split(split);
      return null;
      
    }
  }
  public class EnumFile
  {
    public EnumFile(){}
    string[] data_map;
    
    public string this[int index] { get { return (index > (Length - 1)) ? null : data_map[index]; } }
    
    public int Length { get { return data_map.Length; } }
    
    public EnumFile(string file) { Load(file); }
    static public EnumFile CMAP { get { return new EnumFile{data_map="ext/cc.map".GetLines()   ?? typeof(EnumFile).Assembly.GetEmbeddedResourceString("on.smf.ext.cc.map") }; } }
    static public EnumFile DMAP { get { return new EnumFile{data_map="ext/dk.map".GetLines()   ?? typeof(EnumFile).Assembly.GetEmbeddedResourceString("on.smf.ext.dk.map") }; } }
    static public EnumFile IMAP { get { return new EnumFile{data_map="ext/inst.map".GetLines() ?? typeof(EnumFile).Assembly.GetEmbeddedResourceString("on.smf.ext.inst.map") }; } }
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
          data_map = spoof.Split((char)0x0A);
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
