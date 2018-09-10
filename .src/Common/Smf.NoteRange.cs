/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio.Common
{
	/// <summary>
	/// Note-Range is a range of possible values and is used to determine weather or
	/// not a particular message value is of the type specifed by a given range. 
	/// </summary>
	public struct NoteRange
	{
		public string Name;
		/// <summary>
		/// Min is a step before a possible value and Max is a step beyond the possible value;
		/// Therefore Min &lt; Value &gt; Max.
		/// </summary>
		public int Min, Max, Value;
		
		public int MinDown { get { return Min >> 8; } }
		public int MaxDown { get { return Max >> 8; } }
		public int Down { get { return Value >> 8; } }
		
		public int Up { get { return Value << 8; } }
		public int MinUp { get { return Min << 8; } }
		public int MaxUp { get { return Max << 8; } }
		
		public bool IsInRange(int value) { return value > Min && value < Max; }
		
		public NoteRange(ChMessageU16 msg, string name)
		{
			Min = (Value = (int)msg) - 1;
			Max = (int)msg+0x1000;
			Name = name;
		}
		/// <summary>
		/// input variables ‘Min &lt;= Value &gt;= Max’ are mapped to ‘Min &lt; Value &gt; Max’.
		/// </summary>
		/// <param name="min">This is our actual Minimum value</param>
		/// <param name="max">Our maximum value</param>
		/// <param name="name"></param>
		public NoteRange(ChMessageU16 min, ChMessageU16 max, string name)
		{
			Value = (int)min;
			Min = Value-1;
			Max = (int)max+1;
			Name = name;
		}
	}

}
