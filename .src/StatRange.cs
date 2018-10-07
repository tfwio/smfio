/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.Collections.Generic;

namespace on.smfio
{
  public struct Range
  {
    static public implicit operator Range(int a){ return new Range(a); }
    
    /// <summary>
    /// Min is a step before a possible value and Max is a step beyond the possible value;
    /// Therefore Min &lt; Value &gt; Max.
    /// </summary>
    internal int Min, Max;
    public bool Match(int value) { return value >= Min && value <= Max; }
    public Range(int a, int b) { Min = a; Max = b; }
    public Range(int a) { Min = a; Max = a; }
  }
	/// <summary>
	/// Note-Range is a range of possible values and is used to determine weather or
	/// not a particular message value is of the type specifed by a given range. 
	/// </summary>
	public class StatRange
	{
		public string Name;
		
		public bool Match(int value) {
      // just one
      foreach (var range in Ranges) if (range.Match(value)) return true;
      return false;
    }

    List<Range> Ranges = new List<Range>();

    public StatRange(string name, params Range[] items )
    {
      this.Name = name;
      this.Ranges = new List<Range>(items);
    }
		
    /// <summary>
    /// `Min = value &amp; 0xF0;`
		/// `Max = value &amp; 0xF0 | 0xF;`
    /// </summary>
    /// <param name="value"></param>
    /// <param name="name"></param>
    public StatRange(string name, int value) : this(name, value & 0xF0, value & 0xF0 | 0xF)
    {
    }
    /// <summary>
    /// input variables ‘Min &lt;= Value &gt;= Max’ are mapped to ‘Min &lt; Value &gt; Max’.
    /// </summary>
    /// <param name="min">Minimum</param>
    /// <param name="max">Maximum</param>
    /// <param name="name">Human readable ID.</param>
    public StatRange(string name, int min, int max)
    {
      Name = name;
      Ranges.Add(new Range(min, max));
    }
	}

}
