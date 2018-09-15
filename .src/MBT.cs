/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
	public class MBT : IComparable
	{
		public ulong Pulse { get; set; }

    public int Measure { get; private set; }
		public int Bar { get; private set; }
    public int Tick { get; private set; }

		public MBT(ulong value, short division)
		{
			Pulse = Convert.ToUInt64(value);
			Update(division);
		}

		public void Update(short division)
		{
      Measure = Convert.ToInt32(Math.Floor((double)Pulse / division) + 1);
      Bar = Convert.ToInt32((Math.Floor((double)Pulse / division) % 4) + 1);
      Tick = (int)((long)Pulse % division);
		}
		
		public override string ToString()
		{
			return string.Format("{0:##,###,###,000}:{1:0#}:{2:00#}",  Measure, Bar, Tick);
		}
		
		int IComparable.CompareTo(object obj)
		{
			MBT o = null;
			try { o = (MBT) obj; } catch { throw new ArgumentException("Object must be (or conver to) typeof(MBT)."); }
			if (this.Pulse < o.Pulse) return -1;
			if (this.Pulse > o.Pulse) return 1;
			return 0;
		}
		
		static public bool operator >(MBT a, MBT b)  { return a.Pulse > b.Pulse; }
		static public bool operator <(MBT a, MBT b)  { return a.Pulse < b.Pulse; }
		static public bool operator >=(MBT a, MBT b) { return a.Pulse >= b.Pulse; }
		static public bool operator <=(MBT a, MBT b) { return a.Pulse <= b.Pulse; }
		
		static public MBT operator +(MBT a, MBT b) { a.Pulse += b.Pulse; return a; }
		static public MBT operator -(MBT a, MBT b) { a.Pulse -= b.Pulse; return a; }
		
		static public MBT operator +(MBT mbt, ulong ticks) { mbt.Pulse += ticks; return mbt; }
		static public MBT operator -(MBT mbt, ulong ticks) { mbt.Pulse -= ticks; return mbt; }
		
		static public MBT operator +(MBT mbt, int ticks) { mbt.Pulse += Convert.ToUInt64(ticks); return mbt; }
		static public MBT operator -(MBT mbt, int ticks) { mbt.Pulse -= Convert.ToUInt64(ticks); return mbt; }
		
		static public MBT operator *(MBT mbt, int ticks) { mbt.Pulse *= Convert.ToUInt64(ticks); return mbt; }
		static public MBT operator /(MBT mbt, int ticks) { mbt.Pulse /= Convert.ToUInt64(ticks); return mbt; }

	}
}
