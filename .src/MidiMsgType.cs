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
		/// <summary>was System (0xFF7F)</summary>
		///
		SystemSpecific,
    SystemExclusive,
		SysCommon,
    /// <summary>
    /// This has been used to distinguish a StatusByte message
    /// such as F0 from 0xAn where n is the channel and the
    /// StatusByte prior contains just a pure status.
    /// 
		/// It does not make much sense that we're using Channel
		/// message as it is here.
		/// 
    /// In fact <see cref="MidiMsgType.ControllerChange"/>, <see cref="MidiMsgType.NoteOn"/>,
		/// <see cref="MidiMsgType.NoteOff"/>, <see cref="MidiMsgType.MetaInf"/> and
		/// <see cref="MidiMsgType.MetaStr"/> can all fit into this criterion.
    /// </summary>
    Channel,
		ControllerChange,
		NoteOn,
		NoteOff,
	}
}
