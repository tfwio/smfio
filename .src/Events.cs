using System;
namespace on.smfio
{
  /// <summary></summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public delegate void MidiMessageHandler(object sender, MidiMessageEvent e);

  /// <summary>
  /// A callback to help parsing incoming (parser) messages.
	/// 
	/// - MidiMsgType
	/// - nTrackIndex
	/// - nTrackOffset
	/// - midiMsg32
	/// - midiMsg8
	/// - pulse
	/// - delta
	/// - isRunningStatus
  /// </summary>
  public delegate void MidiEventDelegate(
    MidiMsgType msgType,
    int nTrackIndex,
    int nTrackOffset,
    int midiMsg32,
    byte midiMsg8,
    long pulse,
    int delta,
    bool isRunningStatus);
    
	public class MidiMessageEvent : EventArgs
	{
		public byte MessageByte;
		
    public int MessageInt32;
    
    public int NTrackIndex;
    
    public int NTrackOffset;
    
		public int Delta;
		
		public long Pulse;
		
		public bool IsRunningStatus = false;
		
		public MidiMsgType MessageType = MidiMsgType.ChannelVoice;
		
		public byte[] Data = null;

    public MidiMessageEvent(MidiMsgType msgType, int nTrackIndex, int nTrackOffset, int midiMsg32, byte midiMsg8, long pulse, int delta) : this(msgType,nTrackIndex,nTrackOffset,midiMsg32,midiMsg8,pulse,delta,false) { }

    public MidiMessageEvent(MidiMsgType msgType, int nTrackIndex, int nTrackOffset, int midiMsg32, byte midiMsg8, long pulse, int delta, bool isRunningStatus)
		{
			MessageType = msgType;
			NTrackIndex = nTrackIndex;
			NTrackOffset = nTrackOffset;
			MessageInt32 = midiMsg32;
			MessageByte = midiMsg8;
			Pulse = pulse;
			Delta = delta;
			IsRunningStatus = isRunningStatus;
		}
	}
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