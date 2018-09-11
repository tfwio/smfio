/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{

	public class MBT : IComparable
	{
		
		#region (Global) Division Multipliers
		/// #4
		// static int MthdMeasure { get { return MidiReader.DivMeasure; } }
		// /// #3
		// static int MthdBar { get { return MidiReader.DivBar; } }
		// /// #2
		// static int MthdNote { get { return MidiReader.DivQuarter; } }
		// /// #1 (the most significant)
		// static int MthdDivision { get { return MidiReader.FileDivision; } }
		#endregion
		
		#region Property: Value
		public double DoubleValue {
			get { return Convert.ToDouble(ValueUInt64); }
			set { ValueUInt64 = Convert.ToUInt64(value); }
		}
		public int Int32Value {
			get { return Convert.ToInt32(ValueUInt64); }
			set { ValueUInt64 = Convert.ToUInt64(value); }
		}
		public ulong Value {
			get { return ValueUInt64; } set { ValueUInt64 = value; }
		} ulong ValueUInt64;
    #endregion

    #region Property {get}: Measure, Bar, Ticks
    public int Measure { get { return Convert.ToInt32(Math.Floor(DoubleValue / Division) + 1); } }
		public int Bar { get { return Convert.ToInt32((Math.Floor(DoubleValue / Division) % 4)+1); } }
		public int Ticks { get { return Int32Value % Division; } }
		#endregion
		
		#region Property: Division
		public short Division { get; internal set; }
		#endregion
		
		public MBT(ulong value, short division)
		{
			this.Division = division;
			this.Value = Convert.ToUInt64(value);
		}
		
		public override string ToString()
		{
			return string.Format("{0:##,###,###,000}:{1:0#}:{2:00#}",  Measure, Bar, Ticks);
		}
		
		#region IComparable
		
		int IComparable.CompareTo(object obj)
		{
			MBT o = null;
			try { o = (MBT) obj; } catch { throw new ArgumentException("Object must be (or conver to) typeof(MBT)."); }
			if (this.Value < o.Value) return -1;
			if (this.Value > o.Value) return 1;
			return 0;
		}
		
		#endregion
		#region Static: (float) GetTicksF(int value, int division)
		static public float GetTicksF(int value, int division)
		{
			return value / division;
		}
		#endregion
		#region Static: GetString(ulong value, int division)
		static public string GetString(ulong value, short division, bool plusOne=true, string strFormat="{0:##,###,###,000}:{1:0#}:{2:00#}")
		{
		  double orig = Convert.ToDouble(value).Floor();
			
			int divQuarter = division * 4;
			//double part = (double)value / MidiReader.FileDivision;
			double mBar = (orig / division).Floor() % 4 + (plusOne ? 1 : 0);
			double mMeasure = (orig / divQuarter).Floor() + (plusOne ? 1 : 0);
			int mTicks = Convert.ToInt32(value) % division;
			return string.Format(strFormat,  mMeasure, mBar, mTicks);
		}

		#endregion
		#region Static:Operators
		
		// Division isn't set so it appears that we would have to provide a division of 120 by default.
		// static public implicit operator MBT(byte value)   { return new MBT(Convert.ToUInt64(value)); }
		// static public implicit operator MBT(sbyte value)  { return new MBT(Convert.ToUInt64(value)); }
		// static public implicit operator MBT(int value)    { return new MBT(Convert.ToUInt64(value)); }
		// static public implicit operator MBT(uint value)   { return new MBT(Convert.ToUInt64(value)); }
		// static public implicit operator MBT(short value)  { return new MBT(Convert.ToUInt64(value)); }
		// static public implicit operator MBT(double value) { return new MBT(Convert.ToUInt64(value)); }
		// static public implicit operator MBT(float value)  { return new MBT(Convert.ToUInt64(value)); }
		
//	static public bool operator >(MBT a, int b) { return a.Int32Value > b; }
//	static public bool operator <(MBT a, int b) { return a.Int32Value < b; }
		
		static public bool operator >(MBT a, MBT b)  { return a.Value > b.Value; }
		static public bool operator <(MBT a, MBT b)  { return a.Value < b.Value; }
		static public bool operator >=(MBT a, MBT b) { return a.Value >= b.Value; }
		static public bool operator <=(MBT a, MBT b) { return a.Value <= b.Value; }
		
		static public MBT operator +(MBT a, MBT b) { a.Value += b.Value; return a; }
		static public MBT operator -(MBT a, MBT b) { a.Value -= b.Value; return a; }
		
		static public MBT operator +(MBT mbt, ulong ticks) { mbt.Value += ticks; return mbt; }
		static public MBT operator -(MBT mbt, ulong ticks) { mbt.Value -= ticks; return mbt; }
		
		static public MBT operator +(MBT mbt, int ticks) { mbt.Value += Convert.ToUInt64(ticks); return mbt; }
		static public MBT operator -(MBT mbt, int ticks) { mbt.Value -= Convert.ToUInt64(ticks); return mbt; }
		
		static public MBT operator *(MBT mbt, int ticks) { mbt.Value *= Convert.ToUInt64(ticks); return mbt; }
		static public MBT operator /(MBT mbt, int ticks) { mbt.Value /= Convert.ToUInt64(ticks); return mbt; }

		#endregion
		
	}
}
