---
title: Standard MIDI-File Format Spec. 1.1, updated
date: ~1999
author: The International MIDI Association
margin-left: 0.5in
margin-right: 0.5in
margin-top: 0.75in
margin-bottom: 0.75in
mainfont: Kerkis
papersize: a5
colorlinks: #007FFF
abstract: A detailed Specification of the Standard MIDI file format
website: http://www.csw2.co.uk/tech/midi2.htm
...
<!-- MiKTeX (windows command) -->
<!--
pandoc -s --toc -t latex+abbreviations+smart+abbreviations+auto_identifiers+autolink_bare_uris+backtick_code_blocks+definition_lists+escaped_line_breaks+example_lists+fancy_lists+fenced_code_attributes+footnotes+header_attributes+implicit_figures+implicit_header_references+inline_notes+link_attributes+startnum+strikeout+table_captions+yaml_metadata_block --pdf-engine=xelatex  "F:\home\oio\My Documents\MMD-DOCS\20180926 SMF-1.1-1999.md" -o "20180926 SMF-1.1-1999.pdf"
-->
<!-- mainfont: Roboto Slab -->
**Acknowledgement:**

```text
This document was originally distributed in text format by
The International MIDI Association.

I have updated it and added new Appendices.

© Copyright 1999 David Back.

EMail: david@csw2.co.uk
Web: http://www.csw2.co.uk

THIS DOCUMENT MAY BE FREELY COPIED IN WHOLE OR IN PART
PROVIDED THE COPY CONTAINS THIS ACKNOWLEDGEMENT.

formatted as multimarkdown for pandoc in 2018 by tfw.
```

Introduction
=============

This document details the structure of MIDI Files. The purpose of MIDI Files is to provide a way of interchanging time-stamped MIDI data between different programs on the same or different computers. One of the primary design goals is compact representation, which makes it very appropriate for  disk-based file format, but which might make it inappropriate for storing in memory for quick access by a sequencer program.

MIDI Files contain one or more MIDI streams, with time information for each event. Song, sequence, and track structures, tempo and time signature information, are all supported. Track names and other descriptive information may be stored with the MIDI data. This format supports multiple tracks and multiple sequences so that if the user of a program which supports multiple tracks intends to move a file to another one, this format can allow that to happen.

The specification defines the 8-bit binary data stream used in the file. The data can be stored in a binary file, nibbilized, 7-bit-ized for efficient MIDI transmission, converted to Hex ASCII, or translated symbolically to a printable text file. This spec addresses what's in the 8-bit stream. It does not address how a MIDI File will be transmitted over MIDI.

Sequences, Tracks, Chunks: File Block Structure
===============================================

In this document, bit 0 means the least significant bit of a byte, and bit 7 is the most significant.

Variable Length Quantity
------------------------

Some numbers in MIDI Files are represented in a form called VARIABLE-LENGTH QUANTITY. These numbers are represented 7 bits per byte, most significant bits first. All bytes except the last have bit 7 set, and the last byte has bit 7 clear. If the number is between 0 and 127, it is thus represented exactly as one byte.

**Some examples of numbers represented as variable-length quantities:**

```text
00000000 -------> 00
00000040 -------> 40
0000007F -------> 7F
00000080 -------> 81 00
00002000 -------> C0 00
00003FFF -------> FF 7F
00004000 -------> 81 80 00
00100000 -------> C0 80 00
001FFFFF -------> FF FF 7F
00200000 -------> 81 80 80 00
08000000 -------> C0 80 80 00
0FFFFFFF -------> FF FF FF 7F
```

The largest number which is allowed is 0FFFFFFF so that the variable-length representations must fit in 32 bits in a routine to write variable-length numbers. Theoretically, larger numbers are possible, but 2 x 10 8 96ths of a beat at a fast tempo of 500 beats per minute is four days, long enough for any delta-time!

Files
--------

To any file system, a MIDI File is simply a series of 8-bit bytes. On the Macintosh, this byte stream is stored in the data fork of a file (with file type 'MIDI'), or on the Clipboard (with data type 'MIDI'). Most other computers store 8-bit byte streams in files.

Chunks
------------

MIDI Files are made up of -chunks-. Each chunk has a 4-character type and a 32-bit length, which is the number of bytes in the chunk. This structure allows future chunk types to be designed which may be easily be ignored if encountered by a program written before the chunk type is introduced. Your programs should EXPECT alien chunks and treat them as if they weren't there.

Each chunk begins with a 4-character ASCII type. It is followed by a 32-bit length, most significant byte first (a length of 6 is stored as 00 00 00 06). This length refers to the number of bytes of data which follow: the eight bytes of type and length are not included. Therefore, a chunk with a length of 6 would actually occupy 14 bytes in the disk file.

This chunk architecture is similar to that used by Electronic Arts' IFF format, and the chunks described herein could easily be placed in an IFF file. The MIDI File itself is not an IFF file: it contains no nested chunks, and chunks are not constrained to be an even number of bytes long. Converting it to an IFF file is as easy as padding odd length chunks, and sticking the whole thing inside a FORM chunk.

Chunk Types
---------------

MIDI Files contain two types of chunks: header chunks and track chunks. A -header- chunk provides a minimal amount of information pertaining to the entire MIDI file. A -track- chunk contains a sequential stream of MIDI data which may contain information for up to 16 MIDI channels. The concepts of multiple tracks, multiple MIDI outputs, patterns, sequences, and songs may all be implemented using several track chunks.

A MIDI File always starts with a header chunk, and is followed by one or more track chunks.

```text
MThd <length of header data>
<header data>
MTrk <length of track data>
<track data>
MTrk <length of track data>
<track data>
. . .
```

Chunk Descriptions
======================

Header Chunks
--------------

The header chunk at the beginning of the file specifies some basic information about the data in the file.  
Here's the syntax of the complete chunk:

\<Header Chunk\> = \<chunk type\>\<length\>\<format\>\<ntrks\>\<division\>

As described above, \<chunk type\> is the four ASCII characters 'MThd'; \<length\> is a 32-bit representation of
the number 6 (high byte first).

The data section contains three 16-bit words, stored most-significant byte first.

The first word, \<format\>, specifies the overall organisation of the file. Only three values of \<format\> are specified:

0-the file contains a single multi-channel track  
1-the file contains one or more simultaneous tracks (or MIDI outputs) of a sequence  
2-the file contains one or more sequentially independent single-track patterns

More information about these formats is provided below.

The next word, \<ntrks\>, is the number of track chunks in the file. It will always be 1 for a format 0 file.

The third word, \<division\>, specifies the meaning of the delta-times. It has two formats, one for metrical time, and one for time-code-based time:

| bit 15 | bits 14 thru 8         | bits 7 thru 0 |
| ------ | ---------------------- | ------------- |
| 0      | ticks per quarter-note ||
| 1      | negative SMPTE format  | ticks per frame |

If bit 15 of \<division\> is zero, the bits 14 thru 0 represent the number of delta time "ticks" which make up a quarter-note. For instance, if division is 96, then a time interval of an eighth-note between two events in the file would be 48.

If bit 15 of \<division\> is a one, delta times in a file correspond to subdivisions of a second, in a way consistent with SMPTE and MIDI Time Code. Bits 14 thru 8 contain one of the four values -24, -25, -29, or -30, corresponding to the four standard SMPTE and MIDI Time Code formats (-29 corresponds to 30 drop frame), and represents the number of frames per second. These negative numbers are stored in two's compliment form. The second byte (stored positive) is the resolution within a frame: typical values may be 4 (MIDI Time Code resolution), 8, 10, 80 (bit resolution), or 100. This stream allows exact specifications of time-code-based tracks, but also allows millisecond-based tracks by specifying 25 frames/sec and a resolution of 40 units per frame. If the events in a file are stored with a bit resolution of thirty-frame time code, the division word would be E250 hex.

MIDI File Formats 0, 1 and 2
----------------------------

A Format 0 file has a header chunk followed by one track chunk. It is the most interchangeable representation of data. It is very useful for a simple single-track player in a program which needs to make synthesisers make sounds, but which is primarily concerned with something else such as mixers or sound effect boxes. It is very desirable to be able to produce such a format, even if your program is track-based, in order to work with these simple programs.

A Format 1 or 2 file has a header chunk followed by one or more track chunks. programs which support several simultaneous tracks should be able to save and read data in format 1, a vertically one dimensional form, that is, as a collection of tracks. Programs which support several independent patterns should be able to save and read data in format 2, a horizontally one dimensional form. Providing these minimum capabilities will ensure maximum interchangeability.

In a MIDI system with a computer and a SMPTE synchroniser which uses Song Pointer and Timing Clock, tempo maps (which describe the tempo throughout the track, and may also include time signature information, so that the bar number may be derived) are generally created on the computer. To use them with the synchroniser, it is necessary to transfer them from the computer. To make it easy for the synchroniser to extract this data from a MIDI File, tempo information should always be stored in the first MTrk chunk. For a format 0 file, the tempo will be scattered through the track and the tempo map reader should ignore the intervening events; for a format 1 file, the tempo map must be stored as the first track. It is polite to a tempo map reader to offer your user the ability to make a format 0 file with just the tempo, unless you can use format 1.

All MIDI Files should specify tempo and time signature. If they don't, the time signature is assumed to be 4/4, and the tempo 120 beats per minute. In format 0, these meta-events should occur at least at the beginning of the single multi-channel track. In format 1, these meta-events should be contained in the first track. In format 2, each of the temporally independent patterns should contain at least initial time signature and tempo information.

Format IDs to support other structures may be defined in the future. A program encountering an unknown format ID may still read other MTrk chunks it finds from the file, as format 1 or 2, if its user can make sense of them and arrange them into some other structure if appropriate. Also, more parameters may be added to the MThd chunk in the future: it is important to read and honour the length, even if it is longer than 6.

Track Chunks
--------------

The track chunks (type MTrk) are where actual song data is stored. Each track chunk is simply a stream of MIDI events (and non-MIDI events), preceded by delta-time values. The format for Track Chunks (described below) is exactly the same for all three formats (0, 1, and 2: see "Header Chunk" above) of MIDI Files.

Here is the syntax of an MTrk chunk (the + means "one or more": at least one MTrk event must be present):

\<Track Chunk\> = \<chunk type\>\<length\>\<MTrk event\>+

The syntax of an MTrk event is very simple:

\<MTrk event\> = \<delta-time\>\<event\>

\<delta-time\> is stored as a variable-length quantity. It represents the amount of time before the following event. If the first event in a track occurs at the very beginning of a track, or if two events occur simultaneously, a delta-time of zero is used. Delta-times are always present. (Not storing delta-times of 0 requires at least two bytes for any other value, and most delta-times aren't zero.) Delta-time is in some fraction of a beat (or a second, for recording a track with SMPTE times), as specified in the header chunk.

\<event\> = \<MIDI event\> | \<sysex event\> | \<meta-event\>

\<MIDI event\> is any MIDI channel message See [Appendix 1 - MIDI Messages]. Running status is used: status bytes of MIDI channel messages may be omitted if the preceding event is a MIDI channel message with the same status. The first event in each MTrk chunk must specify status. Delta-time is not considered an event itself: it is an integral part of the syntax for an MTrk event. Notice that running status occurs across delta-times.

\<sysex event\> is used to specify a MIDI system exclusive message, either as one unit or in packets, or as an "escape" to specify any arbitrary bytes to be transmitted. See [Appendix 1 - MIDI Messages]. A normal complete system exclusive message is stored in a MIDI File in this way:

F0 \<length\> \<bytes to be transmitted after F0\>

The length is stored as a variable-length quantity. It specifies the number of bytes which follow it, not including the F0 or the length itself. For instance, the transmitted message F0 43 12 00 07 F7 would be stored in a MIDI File as F0 05 43 12 00 07 F7. It is required to include the F7 at the end so that the reader of the MIDI File knows that it has read the entire message.

Another form of sysex event is provided which does not imply that an F0 should be transmitted. This may be used as an "escape" to provide for the transmission of things which would not otherwise be legal, including system realtime messages, song pointer or select, MIDI Time Code, etc. This uses the F7 code:

F7 \<length\> \<all bytes to be transmitted\>

Unfortunately, some synthesiser manufacturers specify that their system exclusive messages are to be transmitted as little packets. Each packet is only part of an entire syntactical system exclusive message, but the times they are transmitted are important. Examples of this are the bytes sent in a CZ patch dump, or the FB-01's "system exclusive mode" in which microtonal data can be transmitted. The F0 and F7 sysex events may be used together to break up syntactically complete system exclusive messages into timed packets.

An F0 sysex event is used for the first packet in a series -- it is a message in which the F0 should be transmitted. An F7 sysex event is used for the remainder of the packets, which do not begin with F0. (Of course, the F7 is not considered part of the system exclusive message).

A syntactic system exclusive message must always end with an F7, even if the real-life device didn't send one, so that you know when you've reached the end of an entire sysex message without looking ahead to the next event in the MIDI File. If it's stored in one complete F0 sysex event, the last byte must be an F7. There also must not be any transmittable MIDI events in between the packets of a multi-packet system exclusive message. This principle is illustrated in the paragraph below.

Here is a MIDI File of a multi-packet system exclusive message: suppose the bytes F0 43 12 00 were to be sent, followed by a 200-tick delay, followed by the bytes 43 12 00 43 12 00, followed by a 100-tick delay, followed by the bytes 43 12 00 F7, this would be in the MIDI File:

|                         |                     |
|-------------------------|---------------------|
| F0 03 43 12 00          |                     |
| 81 48                   | 200-tick delta time |
| F7 06 43 12 00 43 12 00 |                     |
| 64                      | 100-tick delta time |
| F7 04 43 12 00 F7       |                     |

When reading a MIDI File, and an F7 sysex event is encountered without a preceding F0 sysex event to start a multi-packet system exclusive message sequence, it should be presumed that the F7 event is being used as an "escape". In this case, it is not necessary that it end with an F7, unless it is desired that the F7 be transmitted.

\<meta-event\> specifies non-MIDI information useful to this format or to sequencers, with this syntax:

FF \<type\> \<length\> \<bytes\>

All meta-events begin with FF, then have an event type byte (which is always less than 128), and then have the length of the data stored as a variable-length quantity, and then the data itself. If there is no data, the length is 0. As with chunks, future meta-events may be designed which may not be known to existing programs, so programs must properly ignore meta-events which they do not recognise, and indeed should expect to see them. Programs must never ignore the length of a meta-event which they do not recognise, and they shouldn't be surprised if it's bigger than expected. If so, they must ignore everything past what they know about. However, they must not add anything of their own to the end of the meta- event. Sysex events and meta events cancel any running status which was in effect. Running status does not apply to and may not be used for these messages.


Meta-Events
================

A few meta-events are defined herein. It is not required for every program to support every meta-event.

In the syntax descriptions for each of the meta-events a set of conventions is used to describe parameters of the events. The FF which begins each event, the type of each event, and the lengths of events which do not have a variable amount of data are given directly in hexadecimal. A notation such as dd or se, which consists of two lower-case letters, mnemonically represents an 8-bit value. Four identical lower-case letters such as wwww mnemonically refer to a 16-bit value, stored most-significant-byte first. Six identical lower-case letters such as tttttt refer to a 24-bit value, stored most-significant-byte first. The notation len refers to the length portion of the meta-event syntax, that is, a number, stored as a variable- length quantity, which specifies how many bytes (possibly text) data were just specified by the length.

In general, meta-events in a track which occur at the same time may occur in any order. If a copyright event is used, it should be placed as early as possible in the file, so it will be noticed easily. Sequence Number and Sequence/Track Name events, if present, must appear at time 0. An end-of- track event must occur as the last event in the track.

Meta-Event Definitions
---------------------------

### FF 00 02 Sequence Number

This optional event, which must occur at the beginning of a track, before any nonzero delta-times, and before
any transmittable MIDI events, specifies the number of a sequence. In a format 2 MIDI File, it is used to
identify each "pattern" so that a "song" sequence using the Cue message can refer to the patterns. If the ID
numbers are omitted, the sequences' locations in order in the file are used as defaults. In a format 0 or 1 MIDI
File, which only contain one sequence, this number should be contained in the first (or only) track. If transfer
of several multitrack sequences is required, this must be done as a group of format 1 files, each with a
different sequence number.

###  FF 01 len text Text Event

Any amount of text describing anything. It is a good idea to put a text event right at the beginning of a track,
with the name of the track, a description of its intended orchestration, and any other information which the
user wants to put there. Text events may also occur at other times in a track, to be used as lyrics, or
descriptions of cue points. The text in this event should be printable ASCII characters for maximum
interchange. However, other character codes using the high-order bit may be used for interchange of files
between different programs on the same computer which supports an extended character set. Programs on a
computer which does not support non-ASCII characters should ignore those characters.

Meta-event types 01 through 0F are reserved for various types of text events, each of which meets the
specification of text events (above) but is used for a different purpose:

###  FF 02 len text Copyright Notice

Contains a copyright notice as printable ASCII text. The notice should contain the characters (C), the year of
the copyright, and the owner of the copyright. If several pieces of music are in the same MIDI File, all of the
copyright notices should be placed together in this event so that it will be at the beginning of the file. This
event should be the first event in the track chunk, at time 0.

###  FF 03 len text Sequence/Track Name

If in a format 0 track, or the first track in a format 1 file, the name of the sequence. Otherwise, the name of the
track.

###  FF 04 len text Instrument Name

A description of the type of instrumentation to be used in that track. May be used with the MIDI Prefix
meta-event to specify which MIDI channel the description applies to, or the channel may be specified as text
in the event itself.

###  FF 05 len text Lyric

A lyric to be sung. Generally, each syllable will be a separate lyric event which begins at the event's time.

###  FF 06 len text Marker

Normally in a format 0 track, or the first track in a format 1 file. The name of that point in the sequence, such
as a rehearsal letter or section name ("First Verse", etc.)

###  FF 07 len text Cue Point

A description of something happening on a film or video screen or stage at that point in the musical score
("Car crashes into house", "curtain opens", "she slaps his face", etc.)

###  FF 20 01 cc MIDI Channel Prefix

The MIDI channel (0-15) contained in this event may be used to associate a MIDI channel with all events
which follow, including System exclusive and meta-events. This channel is "effective" until the next normal
MIDI event (which contains a channel) or the next MIDI Channel Prefix meta-event. If MIDI channels refer
to "tracks", this message may be put into a format 0 file, keeping their non-MIDI data associated with a track.
This capability is also present in Yamaha's ESEQ file format.

###  FF 2F 00 End of Track

This event is not optional. It is included so that an exact ending point may be specified for the track, so that
an exact length is defined, which is necessary for tracks which are looped or concatenated.

###  FF 51 03 tttttt Set Tempo (in microseconds per MIDI quarter-note)

This event indicates a tempo change. Another way of putting "microseconds per quarter-note" is "24ths of a
microsecond per MIDI clock". Representing tempos as time per beat instead of beat per time allows
absolutely exact long-term synchronisation with a time-based sync protocol such as SMPTE time code or
MIDI time code. The amount of accuracy provided by this tempo resolution allows a four-minute piece at 120
beats per minute to be accurate within 500 usec at the end of the piece. Ideally, these events should only occur
where MIDI clocks would be located -- this convention is intended to guarantee, or at least increase the
likelihood, of compatibility with other synchronisation devices so that a time signature/tempo map stored in
this format may easily be transferred to another device.

###  FF 54 05 hr mn se fr ff SMPTE Offset

This event, if present, designates the SMPTE time at which the track chunk is supposed to start. It should be
present at the beginning of the track, that is, before any nonzero delta-times, and before any transmittable
MIDI events. the hour must be encoded with the SMPTE format, just as it is in MIDI Time Code. In a format
1 file, the SMPTE Offset must be stored with the tempo map, and has no meaning in any of the other tracks.
The ff field contains fractional frames, in 100ths of a frame, even in SMPTE-based tracks which specify a
different frame subdivision for delta-times.



###  FF 58 04 nn dd cc bb Time Signature

The time signature is expressed as four numbers. nn and dd represent the numerator and denominator of the time signature as it would be notated. The denominator is a negative power of two: 2 represents a quarter-note, 3 represents an eighth-note, etc. The cc parameter expresses the number of MIDI clocks in a metronome click. The bb parameter expresses the number of notated 32nd-notes in a MIDI quarter-note (24 MIDI clocks). This was added because there are already multiple programs which allow a user to specify that what MIDI thinks of as a quarter-note (24 clocks) is to be notated as, or related to in terms of, something else.

Therefore, the complete event for 6/8 time, where the metronome clicks every three eighth-notes, but there are 24 clocks per quarter-note, 72 to the bar, would be (in hex):

```text
FF 58 04 06 03 24 08
```

That is, 6/8 time (8 is 2 to the 3rd power, so this is 06 03), 36 MIDI clocks per dotted-quarter (24 hex!), and eight notated 32nd-notes per quarter-note.


###  FF 59 02 sf mi Key Signature

```text
sf = -7: 7 flats
sf = -1: 1 flat
sf = 0: key of C
sf = 1: 1 sharp
sf = 7: 7 sharps

mi = 0: major key
mi = 1: minor key
```

###  FF 7F len data Sequencer Specific Meta-Event

Special requirements for particular sequencers may use this event type: the first byte or bytes of data is a manufacturer ID (these are one byte, or if the first byte is 00, three bytes). As with MIDI System Exclusive, manufacturers who define something using this meta-event should publish it so that others may be used by a sequencer which elects to use this as its only file format; sequencers with their established feature-specific formats should probably stick to the standard features when using this format.

See [Appendix 2 - Program Fragments and Example MIDI Files]

Appendix
==========================

MIDI Messages
-------------

A MIDI message is made up of an eight-bit status byte which is generally followed by one or two data bytes.  There are a number of different types of MIDI messages. At the highest level, MIDI messages are classified as being either Channel Messages or System Messages. Channel messages are those which apply to a specific Channel, and the Channel number is included in the status byte for these messages. System messages are not Channel specific, and no Channel number is indicated in their status bytes.

Channel Messages may be further classified as being either Channel Voice Messages, or Mode Messages.  Channel Voice Messages carry musical performance data, and these messages comprise most of the traffic in a typical MIDI data stream. Channel Mode messages affect the way a receiving instrument will respond to the Channel Voice messages.

MIDI System Messages are classified as being System Common Messages, System Real Time Messages, or System Exclusive Messages. System Common messages are intended for all receivers in the system. System Real Time messages are used for synchronisation between clock-based MIDI components. System Exclusive messages include a Manufacturer's Identification (ID) code, and are used to transfer any number of data bytes in a format specified by the referenced manufacturer.

### Channel Voice Messages

--------------------------------------------------------------------------------------------
status D7-D0\   Data Byte(s)   Description
nnnn            D7-D0
is the MIDI
channel no.
--------------  -------------  -------------------------------------------------------------
1000nnnn        `0kkkkkkk`     Note Off event.\
                `0vvvvvvv`     This message is sent when a note is released (ended).\
                               `kkkkkkk` is the key (note) number.\
                               `vvvvvvv` is the velocity.

1001nnnn        `0kkkkkkk`     Note Off event.\
                `0vvvvvvv`     This message is sent when a note is depressed (start).\
                               `kkkkkkk` is the key (note) number.\
                               `vvvvvvv` is the velocity.

1010nnnn        `0kkkkkkk`     Polyphonic Key Pressure (Aftertouch).\
                `0vvvvvvv`     This message is sent when a note is depressed (start).\
                               `kkkkkkk` is the key (note) number.\
                               `vvvvvvv` is the velocity.

1011nnnn        `0kkkkkkk`     Control Change.\
                `0vvvvvvv`     This message is sent when a controller value changes.
                               Controllers include devices such as pedals and levers.
                               Certain controller numbers are reserved for specific purposes.
                               See [Channel Mode Messages].\
                               `ccccccc` is the controller number.\
                               `vvvvvvv` is the new value.

1100nnnn        `0ppppppp`     Program Change.\
                               This message sent when the patch number changes.\
                               `ppppppp` is the new program number.

1101nnnn        `0vvvvvvv`     Channel Pressure (After-touch).\
                               This message is most often sent by pressing down on the key
                               after it "bottoms out". This message is different from
                               polyphonic after-touch. Use this message to send the single
                               greatest pressure value (of all the current depressed keys).\
                               `vvvvvvv` is the pressure value.

1110nnnn        `0lllllll`     Pitch Wheel Change.\
                `0mmmmmmm`     This message is sent to indicate a change in the pitch wheel.\
                               The pitch wheel is measured by a fourteen bit value. Centre
                               (no pitch change) is 2000H.\
                               Sensitivity is a function of the transmitter.\
                               `lllllll` are the least significant 7 bits.\
                               `mmmmmmm` are the most significant 7 bits.
-----------------------------------------------------------------------------------------

### Channel Mode Messages (See also Control Change, above)

-----------------------------------------------------------------------------------------
status D7-D0\  Data Byte(s)   Description
nnnn            D7-D0
is the MIDI
channel no.
-------------- ------------   -------------------------------------------------------------
1011nnnn        `0ccccccc`    Channel Mode Messages.\
                `0vvvvvvv`    This the same code as the Control Change (above), but implements Mode
                              control by using reserved controller numbers. The numbers are:
                              Local Control.\
                              When Local Control is Off, all devices on a given channel will respond only to
                              data received over MIDI. Played data, etc. will be ignored. Local Control On
                              restores the functions of the normal controllers.\
                              c = 122, v = 0: Local Control Off\
                              c = 122, v = 127: Local Control On\
                              \
                              All Notes Off.\
                              When an All Notes Off is received all oscillators will turn off.\
                              c = 123, v = 0: All Notes Off\
                              c = 124, v = 0: Omni Mode Off\
                              c = 125, v = 0: Omni Mode On\
                              c = 126, v = M: Mono Mode On (Poly Off) where M is the number of
                              channels (Omni Off) or 0 (Omni On)\
                              c = 127, v = 0: Poly Mode On (Mono Off) (Note: These four messages also
                              cause All Notes Off)
-----------------------------------------------------------------------------------------

#### System Common Messages

-----------------------------------------------------------------------------------------
status D7-D0\  Data Byte(s)   Description
nnnn            D7-D0
is the MIDI
channel no.
-------------- ------------   -------------------------------------------------------------
11110000       `0iiiiiii`\    System Exclusive.\
               `0ddddddd`\    This message makes up for all that MIDI doesn't support. (iiiiiii) is usually a
               `..`\          seven-bit Manufacturer's I.D. code. If the synthesiser recognises the I.D. code
               `..`\          as its own, it will listen to the rest of the message (ddddddd). Otherwise, the
               `0ddddddd`\    message will be ignored. System Exclusive is used to send bulk dumps such as
               `11110111`\    patch parameters and other non-spec data. (Note: Real-Time messages ONLY
                              may be interleaved with a System Exclusive.) This message also is used for
                              extensions called Universal Exclusive Messages.

11110001                      Undefined

11110010       `0lllllll`     Song Position Pointer.
               `0mmmmmmm`     This is an internal 14 bit register that holds the number of MIDI beats (1 beat=
                              six MIDI clocks) since the start of the song. l is the LSB, m the MSB.

11110011       `0sssssss`     Song Select.\
                              The Song Select specifies which sequence or song is to be played.

11110100                      Undefined

11110101                      Undefined

11110110                      Tune Request.\
                              Upon receiving a Tune Request, all analog synthesisers should tune their
                              oscillators.

11110111                      End of Exclusive.\
                              Used to terminate a System Exclusive dump (see above).
-----------------------------------------------------------------------------------------

#### System Realtime Messages

-------------------------------------------------------------------------------------------
status D7-D0\  Data Byte(s)   Description
nnnn            D7-D0
is the MIDI
channel no.
-------------- ------------   -------------------------------------------------------------
11111000                      Timing Clock.\
                              Sent 24 times per quarter note when synchronisation is required.

11111001                      Undefined.

11111010                      Start.\
                              Start the current sequence playing. (This message will be followed with
                              Timing Clocks).

11111011                      Continue.\
                              Continue at the point the sequence was Stopped.

11111100                      Stop.\
                              Stop the current sequence.

11111101                      Undefined.

11111110                      Active Sensing.\
                              Use of this message is optional. When initially sent, the receiver will expect to
                              receive another Active Sensing message each 300ms (max), or it will be
                              assume that the connection has been terminated. At termination, the receiver
                              will turn off all voices and return to normal (non-active sensing) operation.

11111111                      Reset\
                              Reset all receivers in the system to power-up status. This should be used
                              sparingly, preferably under manual control. In particular, it should not be sent
                              on power-up.\
                              In a MIDI file this is used as an escape to introduce \<meta events\>.
-------------------------------------------------------------------------------------------


### Table of MIDI Note Numbers

This table lists all MIDI Note Numbers by octave.

*The absolute octave number designations are based on Middle C = C4, which is an arbitrary but widely used
assignment.*

------------------------------------------------------
Octave   C   C#  D  D#   E   F  F#   G  G#   A  A#   B
------ --- --- --- --- --- --- --- --- --- --- --- ---
-1      0    1   2   3   4   5   6  7    8   9  10  11

 0     12   13  14  15  16  17  18 19   20  21  22  23

 1     24   25  26  27  28  29  30 31   32  33  34  35

 2     36   37  38  39  40  41  42 43   44  45  46  47

 3     48   49  50  51  52  53  54 55   56  57  58  59

 4     60   61  62  63  64  65  66 67   68  69  70  71

 5     72   73  74  75  76  77  78 79   80  81  82  83

 6     84   85  86  87  88  89  90 91   92  93  94  95

 7     96   97  98  99 100 101 102 103 104 105 106 107

 8     108 109 110 111 112 113 114 115 116 117 118 119

 9     120 121 122 123 124 125 126 127
------------------------------------------------------

### General MIDI Instrument Patch Map


* These sounds are the same for all MIDI Channels except Channel 10, which has only percussion
  sounds and some sound "effects". (See Appendix 1.5 - General MIDI Percussion Key Map)
* The names of the instruments indicate what sort of sound will be heard when that instrument number
  (MIDI Program Change or "PC#") is selected on the GM synthesizer.

#### GM Instrument Families

The General MIDI instrument sounds are grouped by families. In each family are 8 specific instruments.

------------------------------------------------------------------
PC#      Family                     PC#        Family
-------- -------------------------- ---------- -------------------
1-8      Piano                      65-72      Reed

9-16     Chromatic Percussion       73-80      Pipe

17-24    Organ                      81-88      Synth Lead

25-32    Guitar                     89-96      Synth Pad

33-40    Bass                       97-104     Synth Effects

41-48    Strings                    95-112     Ethnic

49-56    Ensemble                   113-120    Percussive

57-64    Brass                      120-128    Sound Effects
------------------------------------------------------------------

#### GM Instrument Patch Map

> Note: While GM does not define the actual characteristics of any sounds, the names in parentheses after each
> of the synth leads, pads, and sound effects are, in particular, intended only as guides.

```text
 1.   Acoustic Grand Piano                65.  Soprano Sax
 2.   Bright Acoustic Piano               66.  Alto Sax
 3.   Electric Grand Piano                67.  Tenor Sax
 4.   Honky-tonk Piano                    68.  Baritone Sax
 5.   Electric Piano 1 (Rhodes Piano)     69.  Oboe
 6.   Electric Piano 2 (Chorused Piano)   70.  English Horn
 7.   Harpsichord                         71.  Bassoon
 8.   Clavinet                            72.  Clarinet
 9.   Celesta                             73.  Piccolo
10.  Glockenspiel                         74.  Flute
11.  Music Box                            75.  Recorder
12.  Vibraphone                           76.  Pan Flute
13.  Marimba                              77.  Blown Bottle
14.  Xylophone                            78.  Shakuhachi
15.  Tubular Bells                        79.  Whistle
16.  Dulcimer (Santur)                    80.  Ocarina
17.  Drawbar Organ (Hammond)              81.  Lead 1 (square wave)
18.  Percussive Organ                     82.  Lead 2 (sawtooth wave)
19.  Rock Organ                           83.  Lead 3 (calliope)
20.  Church Organ                         84.  Lead 4 (chiffer)
21.  Reed Organ                           85.  Lead 5 (charang)
22.  Accordion (French)                   86.  Lead 6 (voice solo)
23.  Harmonica                            87.  Lead 7 (fifths)
24.  Tango Accordion (Band neon)          88.  Lead 8 (bass + lead)
25.  Acoustic Guitar (nylon)              89.  Pad 1 (new age Fantasia)
26.  Acoustic Guitar (steel)              90.  Pad 2 (warm)
27.  Electric Guitar (jazz)               91.  Pad 3 (polysynth)
28.  Electric Guitar (clean)              92.  Pad 4 (choir space voice)
29.  Electric Guitar (muted)              93.  Pad 5 (bowed glass)
30.  Overdriven Guitar                    94.  Pad 6 (metallic pro)
31.  Distortion Guitar                    95.  Pad 7 (halo)
32.  Guitar harmonics                     96.  Pad 8 (sweep)
33.  Acoustic Bass                        97.  FX 1 (rain)
34.  Electric Bass (fingered)             98.  FX 2 (soundtrack)
35.  Electric Bass (picked)               99.  FX 3 (crystal)
36.  Fretless Bass                       100. FX4 (atmosphere)
37.  Slap Bass 1                         101. FX 5 (brightness)
38.  Slap Bass 2                         102. FX 6 (goblins)
39.  Synth Bass 1                        103. FX 7 (echoes, drops)
40.  Synth Bass 2                        104. FX 8 (sci-fi, star theme)
41.  Violin                              105. Sitar
42.  Viola                               106. Banjo
43.  Cello                               107. Shamisen
44.  Contrabass                          108. Koto
45.  Tremolo Strings                     109. Kalimba
46.  Pizzicato Strings                   110. Bag pipe
47.  Orchestral Harp                     111. Fiddle
48.  Timpani                             112. Shanai
49.  String Ensemble 1 (strings)         113. Tinkle Bell
50.  String Ensemble 2 (slow strings)    114. Agogo
51.  SynthStrings 1                      115. Steel Drums
52.  SynthStrings 2                      116. Woodblock
53.  Choir Aahs                          117. Taiko Drum
54.  Voice Oohs                          118. Melodic Tom
55.  Synth Voice                         119. Synth Drum
56.  Orchestra Hit                       120. Reverse Cymbal
57.  Trumpet                             121. Guitar Fret Noise
58.  Trombone                            122. Breath Noise
59.  Tuba                                123. Seashore
60.  Muted Trumpet                       124. Bird Tweet
61.  French Horn                         125. Telephone Ring
62.  Brass Section                       126. Helicopter
63.  SynthBrass 1                        127. Applause
64.  SynthBrass 2                        128. Gunshot
```

### General MIDI Percussion Key Map

```text
35 B1 Acoustic Bass Drum   59 B3 Ride Cymbal 2
36 C2 Bass Drum 1          60 C4 Hi Bongo
37 C#2 Side Stick          61 C#4 Low Bongo
38 D2 Acoustic Snare       62 D4 Mute Hi Conga
39 D#2 Hand Clap           63 D#4 Open Hi Conga
40 E2 Electric Snare       64 E4 Low Conga
41 F2 Low Floor Tom        65 F4 High Timbale
42 F#2 Closed Hi Hat       66 F#4 Low Timbale
43 G2 High Floor Tom       67 G4 High Agogo
44 G#2 Pedal Hi-Hat        68 G#4 Low Agogo
45 A2 Low Tom              69 A4 Cabasa
46 A#2 Open Hi-Hat         70 A#4 Maracas
47 B2 Low-Mid Tom          71 B4 Short Whistle
48 C3 Hi Mid Tom           72 C5 Long Whistle
49 C#3 Crash Cymbal 1      73 C#5 Short Guiro
50 D3 High Tom             74 D5 Long Guiro
51 D#3 Ride Cymbal 1       75 D#5 Claves
52 E3 Chinese Cymbal       76 E5 Hi Wood Block
53 F3 Ride Bell            77 F5 Low Wood Block
54 F#3 Tambourine          78 F#5 Mute Cuica
55 G3 Splash Cymbal        79 G5 Open Cuica
56 G#3 Cowbell             80 G#5 Mute Triangle
57 A3 Crash Cymbal 2       81 A5 Open Triangle
58 A#3 Vibraslap           
```

Program Fragments and Example MIDI Files
----------------------------------------

Here are some of the routines to read and write variable-length numbers in MIDI Files. These routines are in
C, and use getc and putc, which read and write single 8-bit characters from/to the files infile and outfile.

```c
WriteVarLen(value) register long value;
{
  register long buffer;
  buffer = value & 0x7f;
  while((value >>= 7) > 0)
  {
    buffer <<= 8;
    buffer |= 0x80;
    buffer += (value &0x7f);
  }
  while (TRUE)
  {
    putc(buffer,outfile);
    if(buffer & 0x80) buffer >>= 8;
    else
    break;
  }
}
doubleword ReadVarLen()
{
  register doubleword value;
  register byte c;
  if((value = getc(infile)) & 0x80)
  {
    value &= 0x7f;
    do
    {
      value = (value << 7) + ((c = getc(infile))) & 0x7f);
    } while (c & 0x80);
  }
  return(value);
}
```

As an example, MIDI Files for the following excerpt are shown below. First, a format 0 file is shown, with all
information intermingled; then, a format 1 file is shown with all data separated into four tracks: one for tempo
and time signature, and three for the notes. A resolution of 96 "ticks" per quarter note is used. A time
signature of 4/4 and a tempo of 120, though implied, are explicitly stated.

![](data:image/jpeg;base64,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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==)

The contents of the MIDI stream represented by this example are broken down here:

--------- ------------- -------------- ----------
Delta\    Event-Code\   Other\         Comment
Time\     (hex)         bytes\
(decimal)               (decimal)
--------- ------------- -------------- ----------
0         FF 58         04 04 02 24 08 4 bytes; 4/4 time; 24 MIDI clocks/click, 8 32nd notes/ 24 MIDI clocks (24 MIDI clocks = 1 crotchet = 1 beat)

0         FF 51         03 500000      3 bytes: 500,000 usec/ quarter note = 120 beats/minute

0         C0            5              Ch.1 Program Change 5 = GM Patch 6 = Electric Piano 2

0         C1            46             Ch.2 Program Change 46 = GM Patch 47 = Harp

0         C2            70             Ch.3 Program Change 70 = GM Patch 71 = Bassoon

0         92            48 96          Ch.3 Note On C3, forte

0         92            60 96          Ch.3 Note On C4, forte

96        91            67 64          Ch.2 Note On G4, mezzo-forte

96        90            76 32          Ch.1 Note On E5, piano

192       82            48 64          Ch.3 Note Off C3, standard

0         82            60 64          Ch.3 Note Off C4, standard

0         81            67 64          Ch.2 Note Off G4, standard

0         80            76 64          Ch.1 Note Off E5, standard

0         FF 2F         00             Track End
-----------------------------------------------------------------------------

The entire format 0 MIDI file contents in hex follow. First, the header chunk:

```text
4D 54 68 64 MThd
00 00 00 06 chunk length
00 00       format 0
00 01       one track
00 60       96 per quarter-note
```

Then the track chunk. Its header followed by the events (notice the running status is used in places):

-------------------------------------------------------------------------------
Delta-Time              Event                Comments
----------------------- -------------------- ---------------------------------
00                      FF 58 04 04 02 18 08 time signature

00                      FF 51 03 07 A1 20    tempo

00                      C0 05

00                      C1 2E

00                      C2 46

00                      92 30 60

00                      3C 60                running status

60                      91 43 40

60                      90 4C 20

81 40                   82 30 40             two-byte delta-time

00                      3C 40                running status

00                      81 43 40

00                      80 4C 40

00                      FF 2F 00             end of track
-------------------------------------------------------------------------------

A format 1 representation of the file is slightly different. Its header chunk:

```text
4D 54 68 64 MThd
00 00 00 06 chunk length
00 01       format 1
00 04       four tracks
00 60       96 per quarter note
```

```text
4D 54 72 6B MTrk
00 00 00 14 chunk length (20)
```


-------------------------------------------------------------------------------
Delta-Time              Event                Comments
----------------------- -------------------- ---------------------------------
00                      FF 58 04 04 02 18 08 time signature

00                      FF 51 03 07 A1 20    tempo

83 00                   FF 2F 00             end of track
-------------------------------------------------------------------------------

Then, the track chunk for the first music track. The MIDI convention for note
on/off running status is used in this example:

```text
4D 54 72 6B MTrk
00 00 00 10 chunk length (16)
```


-------------------------------------------------------------------------------
Delta-Time              Event                Comments
----------------------- -------------------- ---------------------------------
00                      C0 05                time signature

00                      90 4C 20             tempo

83 00                   4C 00                Running status: note on, vel=0

00 FF 2F 00                                  end of track
-------------------------------------------------------------------------------

Then, the track chunk for the second music track:

```text
4D 54 72 6B MTrk
00 00 00 0F chunk length (15)
```

-------------------------------------------------------------------------------
Delta-Time              Event                Comments
----------------------- -------------------- ---------------------------------
00                      C1 2E                time signature

00                      91 43 40

83 00                   43 00                running status

00 FF 2F 00                                  end of track
-------------------------------------------------------------------------------

Then, the track chunk for the third music track:


```text
4D 54 72 6B MTrk
00 00 00 15 chunk length (21)
```

-------------------------------------------------------------------------------
Delta-Time              Event                Comments
----------------------- -------------------- ---------------------------------
00                      C2 46                time signature

00                      92 30 60

82 20                   3C 60                running status

83 00                   30 00                two-byte delta-time, running status

00                      3C 00                running status

00 FF 2F 00                                  end of track
-------------------------------------------------------------------------------

