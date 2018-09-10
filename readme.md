
SMF / MIDI Parser (reader) for DOTNET
=========================================
Reads normative [SMF-1.0] of ~1996.

[SMF-1.0]: https://www.midi.org/specifications/item/the-midi-1-0-specification

ALPHA NOTES
----------------

**BIG VS LITTLE ENDIAN**

Endian-ness has not been tested on LE architecture.  
Its assumed to work while possible that some strings (for humans)
may print in-correctly.  
*Help in this matter would be appreciated.*

**SET TEMPO**

Needs tempo changes to project proper sample positions `HH:MM:SS.TTTTT`.

Usage
------

[TODO: example use-case]

See [branch: smfio.view](https://github.com/tfwio/smfio/tree/smfio.view) for
the current list-view example.

Version History
------------------

0.1.1

- Removed a lot of un-used code
- Application of more comprehensive Endian utility
- Slight change of (interface) IMidiParser and its impl,
  and possibly other interfaces/impl.
- Tempo changes are collected (albeit it needs work)
  to `List<TempoChange>`.  
  TODO: changes are expected to soon reflect in `MidiReader`'s
  (`DictionaryList<int,MidiMessage> MidiDataList`)
  or perhaps some kind of wrapper that is more
  user-friendly when it comes to yielding adequate
  timing in Samples or Seconds.  
  Or at the very least there will be examples
  showing how to obtain adequate time info.

0.1.0

- Initial Dump
- Semantic versioning starts here.
- namespace (title) and other refactoring.
- The project is from around 2005, reworked
  into a prototypical VSTHost to test the engine.
- lacking feature: lacking SetTempo message support.

<!-- See or checkout branch: [smfio.view] for the current example scenario -->

<!--

allowing for manual handling of such as the following scenarios.

### MIDI Format Version

- MIDI Format 0:
    - Single track contains all meta and messages.
- MIDI Format 1:
    - One or more tracks
    - Timings work the same as Format 0.
- MIDI Format 2:
    - One or more tracks.
    - A collection of different songs.
-->
