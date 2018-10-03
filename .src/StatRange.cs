/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
	/// <summary>
	/// Note-Range is a range of possible values and is used to determine weather or
	/// not a particular message value is of the type specifed by a given range. 
	/// </summary>
	public struct StatRange
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
		
		public bool Contains(int pMin, int pMax, int pValue) { return pValue >= pMin && pValue <= pMax; }

		public bool IsInRange(int value) { return value >= Min && value <= Max; }
		
		public static StatRange ForChannel(int pStatus, string pName) {
			return new StatRange(){
				Value = pStatus,
				Min = pStatus & 0xF0,
				Max = pStatus & 0xF0 | 1,
			};
		}
		public StatRange(StatusByte msg, string name)
		{
			Value = (int)msg;
			Min = Value;
			Max = Value;
			Name = name;
		}
		/// <summary>
		/// input variables ‘Min &lt;= Value &gt;= Max’ are mapped to ‘Min &lt; Value &gt; Max’.
		/// </summary>
		/// <param name="min">This is our actual Minimum value</param>
		/// <param name="max">Our maximum value</param>
		/// <param name="name"></param>
		public StatRange(StatusByte min, StatusByte max, string name)
		{
			Value = (int)min;
			Min = Value;
			Max = (int)max;
			Name = name;
		}
	}

}
