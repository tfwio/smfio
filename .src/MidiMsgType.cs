/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;

namespace on.smfio
{
	/// <summary>
	/// provided for a little clarity, though you can identify the
	/// type of midi event being processed by the actual int/byte msg.
	/// </summary>
	/// 
	/// TODO: OBSOLETE; Use StatusByte, Word or Integer in stead.
	public enum MidiMsgType {
		Undefined,
		MetaInf,
		MetaStr,
		/// <summary>0xFF7F</summary>
		///
		SequencerSpecific,
    SystemExclusive,
    /// <summary>
    /// A channel message is a message event which points at a specific channel in the lower 4 byte MSB.
    /// 
    /// E.G. 0xAc is channel c+1 (to make a human readable channel index)
    /// 
    /// Or we can say `var ch = 0xAc &amp; 0x0F`;
    /// 
    /// In fact <see cref="MidiMsgType.ControllerChange"/>, <see cref="MidiMsgType.NoteOn"/>,
		/// <see cref="MidiMsgType.NoteOff"/>, <see cref="MidiMsgType.MetaInf"/> and
		/// <see cref="MidiMsgType.MetaStr"/> can all fit into this criterion.
    /// </summary>
    ChannelVoice,
		ControllerChange,
		NoteOn,
		NoteOff,
	}
}
