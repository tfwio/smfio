
SMF / MIDI Parser/Reader in CSHARP
=========================================

[SMF]: https://www.midi.org/specifications/item/the-midi-1-0-specification

ALPHA NOTES
----------------

### BIG VS LITTLE ENDIAN

Hasn't been tested on little-endian architecture.

Usage Example
------

See branch: smfio.view

- https://github.com/tfwio/smfio/tree/smfio.view

Version History
------------------

Goal(s)

- consolidate, refactor
- simple(r) example(s)
- MIDI Format 1 to Format 0 (in memory)
- Writer (file export/write)

0.1.3

- re-introduce modest-vst dependencies:
    - NoteParser derived on SmfReader

0.1.2

- tempo map & state
- simplify time calculations  
  Old SampleClock MTC and IClock classes are obsoleted.
- refactor/rename params/props of what is now Events.cs  
  see: MidiEventDelegate and MidiMessageEvent (and similarly derived functions)
- using long (vs ulong) for delta pulse in reader & events

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
