
SMF / MIDI Parser (reader) for DOTNET
=========================================
Reads normative [SMF-1.0] of ~1996.

[SMF-1.0]: https://www.midi.org/specifications/item/the-midi-1-0-specification

ALPHA NOTES
----------------

**BIG VS LITTLE ENDIAN**

big-endian works (Windows architectures), little-endian has
yet to be looked at on such an architecture.  
*Help is welcome in this area! –as notes will be prepared to assist tackling
this simple matter.*

More consolidation, documentation and examples are needed geared towards read,
write and export of MIDI (smf) Formats 1-3 in addition to 'filtering' or applying
effects to a given channel/range.

**SET TEMPO**

Needs tempo changes to project proper sample positions `HH:MM:SS.TTTTT`.

Usage
------

[TODO: example use-case]

See [branch: smfio.view](https://github.com/tfwio/smfio/tree/smfio.view) for
the current list-view example.

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
    - Timings are exclusive per track.
-->
