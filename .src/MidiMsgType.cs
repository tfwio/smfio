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
	public enum MidiMsgType {
		Undefined, MetaInf, MetaStr, System, SysCommon, Channel, CC, NoteOn, NoteOff,
	}
}
