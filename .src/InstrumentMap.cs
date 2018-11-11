using System;
namespace on.smfio
{
  
  static public class InstrumentMap
  {
    /// <summary>
    /// This method is specifically intent on converting the integer stored
    /// to the InstrumentType enumeration.
    /// 
    /// It returns a two byte array with LSB (bank) at 0 and MSB at 1 where MSB (Patch or Program) represents.
    /// 
    /// TODO: Test me
    /// </summary>
    static public byte[] GetBytes(this InstrumentType value) { return GetBytes((int)value); }

    /// <summary>
    /// This method is specifically intent on converting the integer stored
    /// to the InstrumentType enumeration.
    /// 
    /// It returns a two byte array with LSB (bank) at 0 and MSB at 1 where MSB (Patch or Program) represents.
    /// 
    /// TODO: Test me
    /// </summary>
    static public byte[] GetBytes(this int intValue) { return new byte[2] { Convert.ToByte((intValue & 0xFF00) >> 8), Convert.ToByte(intValue & 0xFF) }; }
    public static Patch GM1_000_000_Acoustic_Grand_Piano       { get { return new Patch(0, 0); } }
    public static Patch GM1_000_001_Bright_Acoustic_Piano      { get { return new Patch(0, 1); } }
    public static Patch GM1_000_002_Electric_Grand_Piano       { get { return new Patch(0, 2); } }
    public static Patch GM1_000_003_HonkyTonk_Piano            { get { return new Patch(0, 3); } }
    public static Patch GM1_000_004_Electric_Piano_1           { get { return new Patch(0, 4); } }
    public static Patch GM1_000_005_Electric_Piano_2           { get { return new Patch(0, 5); } }
    public static Patch GM1_000_006_Harpsichord                { get { return new Patch(0, 6); } }
    public static Patch GM1_000_007_Clavi                      { get { return new Patch(0, 7); } }
    public static Patch GM1_000_008_Celesta                    { get { return new Patch(0, 8); } }
    public static Patch GM1_000_009_Glockenspiel               { get { return new Patch(0, 9); } }
    public static Patch GM1_000_010_Music_Box                  { get { return new Patch(0, 10); } }
    public static Patch GM1_000_011_Vibraphone                 { get { return new Patch(0, 11); } }
    public static Patch GM1_000_012_Marimba                    { get { return new Patch(0, 12); } }
    public static Patch GM1_000_013_Xylophone                  { get { return new Patch(0, 13); } }
    public static Patch GM1_000_014_Tubular_Bells              { get { return new Patch(0, 14); } }
    public static Patch GM1_000_015_Dulcimer                   { get { return new Patch(0, 15); } }
    public static Patch GM1_000_016_Drawbar_Organ              { get { return new Patch(0, 16); } }
    public static Patch GM1_000_017_Percussive_Organ           { get { return new Patch(0, 17); } }
    public static Patch GM1_000_018_Rock_Organ                 { get { return new Patch(0, 18); } }
    public static Patch GM1_000_019_Church_Organ               { get { return new Patch(0, 19); } }
    public static Patch GM1_000_020_Reed_Organ                 { get { return new Patch(0, 20); } }
    public static Patch GM1_000_021_Accordion                  { get { return new Patch(0, 21); } }
    public static Patch GM1_000_022_Harmonica                  { get { return new Patch(0, 22); } }
    public static Patch GM1_000_023_Tango_Accordion            { get { return new Patch(0, 23); } }
    public static Patch GM1_000_024_Guitar_Nylon               { get { return new Patch(0, 24); } }
    public static Patch GM1_000_025_Guitar_Steel               { get { return new Patch(0, 25); } }
    public static Patch GM1_000_026_Guitar_Electric_Jazz       { get { return new Patch(0, 26); } }
    public static Patch GM1_000_027_Guitar_Electric_Clean      { get { return new Patch(0, 27); } }
    public static Patch GM1_000_028_Guitar_Electric_Muted      { get { return new Patch(0, 28); } }
    public static Patch GM1_000_029_Guitar_OverDriven          { get { return new Patch(0, 29); } }
    public static Patch GM1_000_030_Guitar_Distortion          { get { return new Patch(0, 30); } }
    public static Patch GM1_000_031_Guitar_Harmonics           { get { return new Patch(0, 31); } }
    public static Patch GM1_000_032_Acoustic_Bass              { get { return new Patch(0, 32); } }
    public static Patch GM1_000_033_Electric_Bass_Fingered     { get { return new Patch(0, 33); } }
    public static Patch GM1_000_034_Electric_Bass_Pick         { get { return new Patch(0, 34); } }
    public static Patch GM1_000_035_Fretless_Bass              { get { return new Patch(0, 35); } }
    public static Patch GM1_000_036_Slap_Bass_1                { get { return new Patch(0, 36); } }
    public static Patch GM1_000_037_Slap_Bass_2                { get { return new Patch(0, 37); } }
    public static Patch GM1_000_038_Synth_Bass_1               { get { return new Patch(0, 38); } }
    public static Patch GM1_000_039_Synth_Bass_2               { get { return new Patch(0, 39); } }
    public static Patch GM1_000_040_Violin                     { get { return new Patch(0, 40); } }
    public static Patch GM1_000_041_Viola                      { get { return new Patch(0, 41); } }
    public static Patch GM1_000_042_Cello                      { get { return new Patch(0, 42); } }
    public static Patch GM1_000_043_Contrabass                 { get { return new Patch(0, 43); } }
    public static Patch GM1_000_044_Tremolo_Strings            { get { return new Patch(0, 44); } }
    public static Patch GM1_000_045_Pizzicato_Strings          { get { return new Patch(0, 45); } }
    public static Patch GM1_000_046_Orchestral_Harp            { get { return new Patch(0, 46); } }
    public static Patch GM1_000_047_Timpani                    { get { return new Patch(0, 47); } }
    public static Patch GM1_000_048_String_Ensemble_1          { get { return new Patch(0, 48); } }
    public static Patch GM1_000_049_String_Ensemble_2          { get { return new Patch(0, 49); } }
    public static Patch GM1_000_050_SynthStrings_1             { get { return new Patch(0, 50); } }
    public static Patch GM1_000_051_SynthStrings_2             { get { return new Patch(0, 51); } }
    public static Patch GM1_000_052_Choir_Aahs                 { get { return new Patch(0, 52); } }
    public static Patch GM1_000_053_Voice_Oohs                 { get { return new Patch(0, 53); } }
    public static Patch GM1_000_054_Synth_Voice                { get { return new Patch(0, 54); } }
    public static Patch GM1_000_055_Orchestra_Hit              { get { return new Patch(0, 55); } }
    public static Patch GM1_000_056_Trumpet                    { get { return new Patch(0, 56); } }
    public static Patch GM1_000_057_Trombone                   { get { return new Patch(0, 57); } }
    public static Patch GM1_000_058_Tuba                       { get { return new Patch(0, 58); } }
    public static Patch GM1_000_059_Muted_Trumpet              { get { return new Patch(0, 59); } }
    public static Patch GM1_000_060_French_Horn                { get { return new Patch(0, 60); } }
    public static Patch GM1_000_061_Brass_Section              { get { return new Patch(0, 61); } }
    public static Patch GM1_000_062_SynthBrass_1               { get { return new Patch(0, 62); } }
    public static Patch GM1_000_063_SynthBrass_2               { get { return new Patch(0, 63); } }
    public static Patch GM1_000_064_Soprano_Sax                { get { return new Patch(0, 64); } }
    public static Patch GM1_000_065_Alto_Sax                   { get { return new Patch(0, 65); } }
    public static Patch GM1_000_066_Tenor_Sax                  { get { return new Patch(0, 66); } }
    public static Patch GM1_000_067_Baritone_Sax               { get { return new Patch(0, 67); } }
    public static Patch GM1_000_068_Oboe                       { get { return new Patch(0, 68); } }
    public static Patch GM1_000_069_English_Horn               { get { return new Patch(0, 69); } }
    public static Patch GM1_000_070_Bassoon                    { get { return new Patch(0, 70); } }
    public static Patch GM1_000_071_Clarinet                   { get { return new Patch(0, 71); } }
    public static Patch GM1_000_072_Piccolo                    { get { return new Patch(0, 72); } }
    public static Patch GM1_000_073_Flute                      { get { return new Patch(0, 73); } }
    public static Patch GM1_000_074_Recorder                   { get { return new Patch(0, 74); } }
    public static Patch GM1_000_075_Pan_Flute                  { get { return new Patch(0, 75); } }
    public static Patch GM1_000_076_Blown_Bottle               { get { return new Patch(0, 76); } }
    public static Patch GM1_000_077_Shakuhachi                 { get { return new Patch(0, 77); } }
    public static Patch GM1_000_078_Whistle                    { get { return new Patch(0, 78); } }
    public static Patch GM1_000_079_Ocarina                    { get { return new Patch(0, 79); } }
    public static Patch GM1_000_080_Lead_1_Square              { get { return new Patch(0, 80); } }
    public static Patch GM1_000_081_Lead_2_Sawtooth            { get { return new Patch(0, 81); } }
    public static Patch GM1_000_082_Lead_3_Calliope            { get { return new Patch(0, 82); } }
    public static Patch GM1_000_083_Lead_4_Chiff               { get { return new Patch(0, 83); } }
    public static Patch GM1_000_084_Lead_5_Charang             { get { return new Patch(0, 84); } }
    public static Patch GM1_000_085_Lead_6_Voice               { get { return new Patch(0, 85); } }
    public static Patch GM1_000_086_Lead_7_Fifths              { get { return new Patch(0, 86); } }
    public static Patch GM1_000_087_Lead_8_Bass_AND_lead       { get { return new Patch(0, 87); } }
    public static Patch GM1_000_088_Pad_1_new_age              { get { return new Patch(0, 88); } }
    public static Patch GM1_000_089_Pad_2_warm                 { get { return new Patch(0, 89); } }
    public static Patch GM1_000_090_Pad_3_polysynth            { get { return new Patch(0, 90); } }
    public static Patch GM1_000_091_Pad_4_choir                { get { return new Patch(0, 91); } }
    public static Patch GM1_000_092_Pad_5_bowed                { get { return new Patch(0, 92); } }
    public static Patch GM1_000_093_Pad_6_metallic             { get { return new Patch(0, 93); } }
    public static Patch GM1_000_094_Pad_7_halo                 { get { return new Patch(0, 94); } }
    public static Patch GM1_000_095_Pad_8_sweep                { get { return new Patch(0, 95); } }
    public static Patch GM1_000_096_FX_1_Rain                  { get { return new Patch(0, 96); } }
    public static Patch GM1_000_097_FX_2_Soundtrack            { get { return new Patch(0, 97); } }
    public static Patch GM1_000_098_FX_3_Crystal               { get { return new Patch(0, 98); } }
    public static Patch GM1_000_099_FX_4_Atmosphere            { get { return new Patch(0, 99); } }
    public static Patch GM1_000_100_FX_5_Brightness            { get { return new Patch(0, 100); } }
    public static Patch GM1_000_101_FX_6_Goblins               { get { return new Patch(0, 101); } }
    public static Patch GM1_000_102_FX_7_Echoes                { get { return new Patch(0, 102); } }
    public static Patch GM1_000_103_FX_8_SciFi                 { get { return new Patch(0, 103); } }
    public static Patch GM1_000_104_Sitar                      { get { return new Patch(0, 104); } }
    public static Patch GM1_000_105_Banjo                      { get { return new Patch(0, 105); } }
    public static Patch GM1_000_106_Shamisen                   { get { return new Patch(0, 106); } }
    public static Patch GM1_000_107_Koto                       { get { return new Patch(0, 107); } }
    public static Patch GM1_000_108_Kalimba                    { get { return new Patch(0, 108); } }
    public static Patch GM1_000_109_Bag_pipe                   { get { return new Patch(0, 109); } }
    public static Patch GM1_000_110_Fiddle                     { get { return new Patch(0, 110); } }
    public static Patch GM1_000_111_Shanai                     { get { return new Patch(0, 111); } }
    public static Patch GM1_000_112_Tinkle_Bell                { get { return new Patch(0, 112); } }
    public static Patch GM1_000_113_Agogo                      { get { return new Patch(0, 113); } }
    public static Patch GM1_000_114_Steel_Drums                { get { return new Patch(0, 114); } }
    public static Patch GM1_000_115_Woodblock                  { get { return new Patch(0, 115); } }
    public static Patch GM1_000_116_Taiko_Drum                 { get { return new Patch(0, 116); } }
    public static Patch GM1_000_117_Melodic_Tom                { get { return new Patch(0, 117); } }
    public static Patch GM1_000_118_Synth_Drum                 { get { return new Patch(0, 118); } }
    public static Patch GM1_000_119_Reverse_Cymbal             { get { return new Patch(0, 119); } }
    public static Patch GM1_000_120_Guitar_Fret_Noise          { get { return new Patch(0, 120); } }
    public static Patch GM1_000_121_Breath_Noise               { get { return new Patch(0, 121); } }
    public static Patch GM1_000_122_Seashore                   { get { return new Patch(0, 122); } }
    public static Patch GM1_000_123_Bird_Tweet                 { get { return new Patch(0, 123); } }
    public static Patch GM1_000_124_Telephone_Ring             { get { return new Patch(0, 124); } }
    public static Patch GM1_000_125_Helicopter                 { get { return new Patch(0, 125); } }
    public static Patch GM1_000_126_Applause                   { get { return new Patch(0, 126); } }
    public static Patch GM1_000_127_Gunshot                    { get { return new Patch(0, 127); } }
    public static Patch GM2_001_000_Wide_Acoustic_Grand        { get { return new Patch(1, 0); } }
    public static Patch GM2_001_001_Wide_Bright_Acoustic       { get { return new Patch(1, 1); } }
    public static Patch GM2_001_002_Wide_Electric_Grand        { get { return new Patch(1, 2); } }
    public static Patch GM2_001_003_Wide_Honky_tonk            { get { return new Patch(1, 3); } }
    public static Patch GM2_001_004_Detuned_Electric_Piano_1   { get { return new Patch(1, 4); } }
    public static Patch GM2_001_005_Detuned_Electric_Piano_2   { get { return new Patch(1, 5); } }
    public static Patch GM2_001_006_Coupled_Harpsichord        { get { return new Patch(1, 6); } }
    public static Patch GM2_001_007_Pulse_Clavinet             { get { return new Patch(1, 7); } }
    public static Patch GM2_001_011_Wet_Vibraphone             { get { return new Patch(1, 11); } }
    public static Patch GM2_001_012_Wide_Marimba               { get { return new Patch(1, 12); } }
    public static Patch GM2_001_014_Church_Bell                { get { return new Patch(1, 14); } }
    public static Patch GM2_001_016_Detuned_Organ_1            { get { return new Patch(1, 16); } }
    public static Patch GM2_001_017_Detuned_Organ_2            { get { return new Patch(1, 17); } }
    public static Patch GM2_001_019_Church_Organ_2             { get { return new Patch(1, 19); } }
    public static Patch GM2_001_020_Puff_Organ                 { get { return new Patch(1, 20); } }
    public static Patch GM2_001_021_Italian_Accordion          { get { return new Patch(1, 21); } }
    public static Patch GM2_001_024_Ukulele                    { get { return new Patch(1, 24); } }
    public static Patch GM2_001_025_12_String_Guitar           { get { return new Patch(1, 25); } }
    public static Patch GM2_001_026_Hawaiian_Guitar            { get { return new Patch(1, 26); } }
    public static Patch GM2_001_027_Chorus_Guitar              { get { return new Patch(1, 27); } }
    public static Patch GM2_001_028_Funk_Guitar                { get { return new Patch(1, 28); } }
    public static Patch GM2_001_029_Guitar_Pinch               { get { return new Patch(1, 29); } }
    public static Patch GM2_001_030_Feedback_Guitar            { get { return new Patch(1, 30); } }
    public static Patch GM2_001_031_Guitar_Harmonics           { get { return new Patch(1, 31); } }
    public static Patch GM2_001_033_Finger_Slap                { get { return new Patch(1, 33); } }
    public static Patch GM2_001_038_Synth_Bass_101             { get { return new Patch(1, 38); } }
    public static Patch GM2_001_039_Synth_Bass_4               { get { return new Patch(1, 39); } }
    public static Patch GM2_001_040_Slow_Violin                { get { return new Patch(1, 10); } }
    public static Patch GM2_001_046_Yang_Qin                   { get { return new Patch(1, 46); } }
    public static Patch GM2_001_048_Orchestra_Strings          { get { return new Patch(1, 48); } }
    public static Patch GM2_001_050_Synth_Strings_3            { get { return new Patch(1, 50); } }
    public static Patch GM2_001_052_Choir_Aahs_2               { get { return new Patch(1, 52); } }
    public static Patch GM2_001_053_Humming                    { get { return new Patch(1, 53); } }
    public static Patch GM2_001_054_Analog_Voice               { get { return new Patch(1, 54); } }
    public static Patch GM2_001_055_Bass_Hit                   { get { return new Patch(1, 55); } }
    public static Patch GM2_001_056_Dark_Trumpet               { get { return new Patch(1, 56); } }
    public static Patch GM2_001_057_Trombone_2                 { get { return new Patch(1, 57); } }
    public static Patch GM2_001_059_Muted_Trumpet_2            { get { return new Patch(1, 59); } }
    public static Patch GM2_001_060_French_Horn_2              { get { return new Patch(1, 60); } }
    public static Patch GM2_001_061_Brass_Section_2            { get { return new Patch(1, 61); } }
    public static Patch GM2_001_062_Synth_Brass_3              { get { return new Patch(1, 62); } }
    public static Patch GM2_001_063_Synth_Brass_4              { get { return new Patch(1, 63); } }
    public static Patch GM2_001_080_Square_Wave                { get { return new Patch(1, 80); } }
    public static Patch GM2_001_084_Wire_Lead                  { get { return new Patch(1, 84); } }
    public static Patch GM2_001_087_Delayed_Lead               { get { return new Patch(1, 87); } }
    public static Patch GM2_001_089_Sine_Pad                   { get { return new Patch(1, 89); } }
    public static Patch GM2_001_091_Itopia                     { get { return new Patch(1, 91); } }
    public static Patch GM2_001_098_Synth_Mallet               { get { return new Patch(1, 98); } }
    public static Patch GM2_001_102_Echo_Bell                  { get { return new Patch(1, 102); } }
    public static Patch GM2_001_104_Sitar_2                    { get { return new Patch(1, 104); } }
    public static Patch GM2_001_107_Taisho_Koto                { get { return new Patch(1, 107); } }
    public static Patch GM2_001_115_Castanets                  { get { return new Patch(1, 115); } }
    public static Patch GM2_001_116_Concert_Bass_Drum          { get { return new Patch(1, 116); } }
    public static Patch GM2_001_117_Melodic_Tom_2              { get { return new Patch(1, 117); } }
    public static Patch GM2_001_118_808_Tom                    { get { return new Patch(1, 118); } }
    public static Patch GM2_001_120_Guitar_Cut_Noise           { get { return new Patch(1, 120); } }
    public static Patch GM2_001_121_Flute_Key_Click            { get { return new Patch(1, 121); } }
    public static Patch GM2_001_122_Rain                       { get { return new Patch(1, 122); } }
    public static Patch GM2_001_123_Dog                        { get { return new Patch(1, 123); } }
    public static Patch GM2_001_124_Telephone_2                { get { return new Patch(1, 124); } }
    public static Patch GM2_001_125_Car_Engine                 { get { return new Patch(1, 125); } }
    public static Patch GM2_001_126_Laughing                   { get { return new Patch(1, 126); } }
    public static Patch GM2_001_127_Machine_Gun                { get { return new Patch(1, 127); } }
    public static Patch GM2_002_000_Dark_Acoustic_Grand        { get { return new Patch(2, 0); } }
    public static Patch GM2_002_004_Electric_Piano_1_Variation { get { return new Patch(2, 4); } }
    public static Patch GM2_002_005_Electric_Piano_2_Variation { get { return new Patch(2, 5); } }
    public static Patch GM2_002_006_Wide_Harpsichord           { get { return new Patch(2, 6); } }
    public static Patch GM2_002_014_Carillon                   { get { return new Patch(2, 14); } }
    public static Patch GM2_002_016_60s_Organ_1                { get { return new Patch(2, 16); } }
    public static Patch GM2_002_017_Organ_5                    { get { return new Patch(2, 17); } }
    public static Patch GM2_002_019_Rock_Organ                 { get { return new Patch(2, 19); } }
    public static Patch GM2_002_024_Open_Nylon_Guitar          { get { return new Patch(2, 24); } }
    public static Patch GM2_002_025_Mandolin                   { get { return new Patch(2, 25); } }
    public static Patch GM2_002_027_Mid_Tone_Guitar            { get { return new Patch(2, 27); } }
    public static Patch GM2_002_028_Funk_Guitar_2              { get { return new Patch(2, 28); } }
    public static Patch GM2_002_030_Distortion_Rtm_Guitar      { get { return new Patch(2, 30); } }
    public static Patch GM2_002_038_Synth_Bass_3               { get { return new Patch(2, 38); } }
    public static Patch GM2_002_039_Rubber_Bass                { get { return new Patch(2, 39); } }
    public static Patch GM2_002_048_60s_Strings                { get { return new Patch(2, 48); } }
    public static Patch GM2_002_055_6th_Hit                    { get { return new Patch(2, 55); } }
    public static Patch GM2_002_057_Bright_Trombone            { get { return new Patch(2, 57); } }
    public static Patch GM2_002_062_Analog_Brass_1             { get { return new Patch(2, 62); } }
    public static Patch GM2_002_063_Analog_Brass_2             { get { return new Patch(2, 63); } }
    public static Patch GM2_002_080_Sine_Wave                  { get { return new Patch(2, 80); } }
    public static Patch GM2_002_081_Doctor_Solo                { get { return new Patch(2, 81); } }
    public static Patch GM2_002_102_Echo_Pan                   { get { return new Patch(2, 102); } }
    public static Patch GM2_002_118_Electric_Percussion        { get { return new Patch(2, 118); } }
    public static Patch GM2_002_120_HorseGallop                { get { return new Patch(2, 120); } }
    public static Patch GM2_002_122_Thunder                    { get { return new Patch(2, 122); } }
    public static Patch GM2_002_123_HorseGallop                { get { return new Patch(2, 123); } }
    public static Patch GM2_002_124_Door_Creaking              { get { return new Patch(2, 124); } }
    public static Patch GM2_002_125_CarStop                    { get { return new Patch(2, 125); } }
    public static Patch GM2_002_126_Screaming                  { get { return new Patch(2, 126); } }
    public static Patch GM2_002_127_Lasergun                   { get { return new Patch(2, 127); } }
    public static Patch GM2_003_004_60s_Electric_Piano         { get { return new Patch(3, 4); } }
    public static Patch GM2_003_005_Electric_Piano_Legend      { get { return new Patch(3, 5); } }
    public static Patch GM2_003_006_Open_Harpsichord           { get { return new Patch(3, 6); } }
    public static Patch GM2_003_016_Organ_4                    { get { return new Patch(3, 16); } }
    public static Patch GM2_003_024_Nylon_Guitar_2             { get { return new Patch(3, 24); } }
    public static Patch GM2_003_025_Steel_PLUS_Body            { get { return new Patch(3, 25); } }
    public static Patch GM2_003_028_Jazz_Man                   { get { return new Patch(3, 28); } }
    public static Patch GM2_003_038_Clavi_Bass                 { get { return new Patch(3, 38); } }
    public static Patch GM2_003_039_Attack_Pulse               { get { return new Patch(3, 39); } }
    public static Patch GM2_003_055_Euro_Hit                   { get { return new Patch(3, 55); } }
    public static Patch GM2_003_062_Jump_Brass                 { get { return new Patch(3, 62); } }
    public static Patch GM2_003_081_Natural_Lead               { get { return new Patch(3, 81); } }
    public static Patch GM2_003_122_Wind                       { get { return new Patch(3, 122); } }
    public static Patch GM2_003_123_Bird_2                     { get { return new Patch(3, 123); } }
    public static Patch GM2_003_124_Door_Closing               { get { return new Patch(3, 124); } }
    public static Patch GM2_003_125_CarCrash                   { get { return new Patch(3, 125); } }
    public static Patch GM2_003_126_Punch                      { get { return new Patch(3, 126); } }
    public static Patch GM2_003_127_Explosion                  { get { return new Patch(3, 127); } }
    public static Patch GM2_004_005_Electric_Piano_Phase       { get { return new Patch(4, 5); } }
    public static Patch GM2_004_038_Hammer                     { get { return new Patch(4, 38); } }
    public static Patch GM2_004_081_Sequenced_Saw              { get { return new Patch(4, 81); } }
    public static Patch GM2_004_122_Stream                     { get { return new Patch(4, 122); } }
    public static Patch GM2_004_124_Scratch                    { get { return new Patch(4, 124); } }
    public static Patch GM2_004_125_CarCrash                   { get { return new Patch(4, 125); } }
    public static Patch GM2_004_126_Heart_Beat                 { get { return new Patch(4, 126); } }
    public static Patch GM2_005_122_Bubble                     { get { return new Patch(5, 122); } }
    public static Patch GM2_005_124_Wind_Chimes                { get { return new Patch(5, 124); } }
    public static Patch GM2_005_125_Siren                      { get { return new Patch(5, 125); } }
    public static Patch GM2_005_126_FootSteps                  { get { return new Patch(5, 126); } }
    public static Patch GM2_006_125_Train                      { get { return new Patch(6, 125); } }
    public static Patch GM2_007_125_Jet_Plane                  { get { return new Patch(7, 125); } }
    public static Patch GM2_008_125_StarShip                   { get { return new Patch(8, 125); } }
  }
}