/*
 * Date: 11/12/2005
 * Time: 4:19 PM
 */
using System;
using System.Collections.Generic;

namespace on.smfio
{
  public interface INoteParser : IMidiParser
  {
    /// <summary>
    /// This should perhaps be externalized as a parser extension.<br />
    /// See CloseNote, GetNote and all the other methods in IMidiParer_Notes of course...
    /// </summary>
    /// <param name="type">Currently supported types are NoteOn and NoteOff</param>
    /// <param name="ppq">The PPQ/Time Position in the class.</param>
    /// <param name="ch">(not properly supported) Channel ID</param>
    /// <param name="offset">The offset within the currently active/selected track.</param>
    /// <param name="b">The cursor's byte at '<paramref name="offset"/>'.</param>
    /// <param name="rs">True/False indication that the message is a Running Status messgae.</param>
    void CheckNote(MidiMsgType type, long ppq, byte ch, int offset, byte b, bool rs);

    /// <summary>
    /// Looks into MidiData for a MidiNote with a VelocityOff value of -1 and the same note-byte
    /// and updates it's note-off velocity.
    /// </summary>
    /// <param name="ppq">The PPQ/Time Position in the class.</param>
    /// <param name="k">Midi Key Value</param>
    /// <param name="v">The Velocity to add to the un-closed NoteOn event.</param>
    /// <remarks>Very few midi files cause errors with this if there are
    /// note on messages that are not terminated.</remarks>
    void CloseNote(long ppq, byte k, short v);

    /// <summary>
    /// Searches in the MidiData List for 
    /// </summary>
    /// <param name="k">Midi Key/Note Value</param>
    /// <param name="v">Velocity (off)</param>
    /// <returns></returns>
    MidiData GetNote(byte k, short v);

    /// <summary>
    /// A Set of MidiData.  The data is used by <see cref="CheckNote" /> and related methods.
    /// </summary>
    List<MidiData> Notes { get; set; }
  }
}
