/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using CliEvent = System.EventArgs;
using CliHandler = System.EventHandler;
namespace on.smfio
{
	public class SmpteOffset
	{
		public int Hour, Minute, Second, Frame, Fraction;
    public SmpteOffset() : this(0,0,0,0,0) {}
    public SmpteOffset(int h, int m, int s, int fr, int ff)
    {
      Hour = h;
      Minute = m;
      Second = s;
      Frame = fr;
      Fraction = ff;
    }
    public void SetSMPTE(int h, int m, int s, int fr, int ff)
    {
      Hour = h;
      Minute = m;
      Second = s;
      Frame = fr;
      Fraction = ff;
    }
    public bool IsEmpty {
      get { return (Hour == 0) & (Minute == 0) & (Second == 0) & (Frame == 0) & (Fraction == 0); }
    }
    public override string ToString()
    {
      return $"{Hour:#,#00}:{Minute:00}:{Second:00}:{Frame:00}:{Fraction:00}";
    }
	}
}


