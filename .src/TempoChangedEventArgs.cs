using System;
namespace on.smfio
{
	public class TempoChangedEventArgs : EventArgs
	{
	  public int TrackID { get; private set; }
	  
	  public int Delta { get; private set; }
	  
		public double TempoValue {
			get;
			private set;
		}

		public uint ReferenceValue {
			get;
			private set;
		}

		public TempoChangedEventArgs(int delta, uint tValue)
		{
		  Delta = delta;
			ReferenceValue = tValue;
			TempoValue = 60000000.0 / ReferenceValue;
		}
	}
}


