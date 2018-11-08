namespace on.smfio
{
  public enum ControllerType : byte
  {
    /// <summary>BankSelect_MSB (0)</summary>
    [System.ComponentModel.Description("BankSelect MSB")]
    BankSelect_MSB                      = 0,
    /// <summary>ModulationWheel_MSB (1)</summary>
    [System.ComponentModel.Description("ModulationWheel MSB")]
    ModulationWheel_MSB                 = 1,
    /// <summary>BreathControl_MSB (2)</summary>
    [System.ComponentModel.Description("BreathControl MSB")]
    BreathControl_MSB                   = 2,
    /// <summary>CC_003 (3)</summary>
    [System.ComponentModel.Description("CC 003")]
    CC_003                              = 3,
    /// <summary>FootController_MSB (4)</summary>
    [System.ComponentModel.Description("FootController MSB")]
    FootController_MSB                  = 4,
    /// <summary>PortamentoTime_MSB (5)</summary>
    [System.ComponentModel.Description("PortamentoTime MSB")]
    PortamentoTime_MSB                  = 5,
    /// <summary>DataEntry_MSB (6)</summary>
    [System.ComponentModel.Description("DataEntry MSB")]
    DataEntry_MSB                       = 6,
    /// <summary>ChannelVolume_MSB (7)</summary>
    [System.ComponentModel.Description("ChannelVolume MSB")]
    ChannelVolume_MSB                   = 7,
    /// <summary>Balance_MSB (8)</summary>
    [System.ComponentModel.Description("Balance MSB")]
    Balance_MSB                         = 8,
    /// <summary>CC_009 (9)</summary>
    [System.ComponentModel.Description("CC 009")]
    CC_009                              = 9,
    /// <summary>Pan_MSB (10)</summary>
    [System.ComponentModel.Description("Pan MSB")]
    Pan_MSB                             = 10,
    /// <summary>ExpressionController_MSB (11)</summary>
    [System.ComponentModel.Description("ExpressionController MSB")]
    ExpressionController_MSB            = 11,
    /// <summary>EffectControl1_MSB (12)</summary>
    [System.ComponentModel.Description("EffectControl1 MSB")]
    EffectControl1_MSB                  = 12,
    /// <summary>EffectControl2_MSB (13)</summary>
    [System.ComponentModel.Description("EffectControl2 MSB")]
    EffectControl2_MSB                  = 13,
    /// <summary>CC_014 (14)</summary>
    [System.ComponentModel.Description("CC 014")]
    CC_014                              = 14,
    /// <summary>CC_015 (15)</summary>
    [System.ComponentModel.Description("CC 015")]
    CC_015                              = 15,
    /// <summary>GeneralPurposeController_1_MSB (16)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 1 MSB")]
    GeneralPurposeController_1_MSB      = 16,
    /// <summary>GeneralPurposeController_2_MSB (17)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 2 MSB")]
    GeneralPurposeController_2_MSB      = 17,
    /// <summary>GeneralPurposeController_3_MSB (18)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 3 MSB")]
    GeneralPurposeController_3_MSB      = 18,
    /// <summary>GeneralPurposeController_4_MSB (19)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 4 MSB")]
    GeneralPurposeController_4_MSB      = 19,
    /// <summary>CC_020 (20)</summary>
    [System.ComponentModel.Description("CC 020")]
    CC_020                              = 20,
    /// <summary>CC_021 (21)</summary>
    [System.ComponentModel.Description("CC 021")]
    CC_021                              = 21,
    /// <summary>CC_022 (22)</summary>
    [System.ComponentModel.Description("CC 022")]
    CC_022                              = 22,
    /// <summary>CC_023 (23)</summary>
    [System.ComponentModel.Description("CC 023")]
    CC_023                              = 23,
    /// <summary>CC_024 (24)</summary>
    [System.ComponentModel.Description("CC 024")]
    CC_024                              = 24,
    /// <summary>CC_025 (25)</summary>
    [System.ComponentModel.Description("CC 025")]
    CC_025                              = 25,
    /// <summary>CC_026 (26)</summary>
    [System.ComponentModel.Description("CC 026")]
    CC_026                              = 26,
    /// <summary>CC_027 (27)</summary>
    [System.ComponentModel.Description("CC 027")]
    CC_027                              = 27,
    /// <summary>CC_028 (28)</summary>
    [System.ComponentModel.Description("CC 028")]
    CC_028                              = 28,
    /// <summary>CC_029 (29)</summary>
    [System.ComponentModel.Description("CC 029")]
    CC_029                              = 29,
    /// <summary>CC_030 (30)</summary>
    [System.ComponentModel.Description("CC 030")]
    CC_030                              = 30,
    /// <summary>CC_031 (31)</summary>
    [System.ComponentModel.Description("CC 031")]
    CC_031                              = 31,
    /// <summary>BankSelect_LSB (32)</summary>
    [System.ComponentModel.Description("BankSelect LSB")]
    BankSelect_LSB                      = 32,
    /// <summary>ModulationWheel_LSB (33)</summary>
    [System.ComponentModel.Description("ModulationWheel LSB")]
    ModulationWheel_LSB                 = 33,
    /// <summary>BreathControl_LSB (34)</summary>
    [System.ComponentModel.Description("BreathControl LSB")]
    BreathControl_LSB                   = 34,
    /// <summary>CC_032 (35)</summary>
    [System.ComponentModel.Description("CC 032")]
    CC_032                              = 35,
    /// <summary>FootController_LSB (36)</summary>
    [System.ComponentModel.Description("FootController LSB")]
    FootController_LSB                  = 36,
    /// <summary>PortamentoTime_LSB (37)</summary>
    [System.ComponentModel.Description("PortamentoTime LSB")]
    PortamentoTime_LSB                  = 37,
    /// <summary>DataEntry_LSB (38)</summary>
    [System.ComponentModel.Description("DataEntry LSB")]
    DataEntry_LSB                       = 38,
    /// <summary>ChannelVolume_LSB (39)</summary>
    [System.ComponentModel.Description("ChannelVolume LSB")]
    ChannelVolume_LSB                   = 39,
    /// <summary>Balance_LSB (40)</summary>
    [System.ComponentModel.Description("Balance LSB")]
    Balance_LSB                         = 40,
    /// <summary>CC_41 (41)</summary>
    [System.ComponentModel.Description("CC 41")]
    CC_41                               = 41,
    /// <summary>Pan_LSB (42)</summary>
    [System.ComponentModel.Description("Pan LSB")]
    Pan_LSB                             = 42,
    /// <summary>ExpressionController_LSB (43)</summary>
    [System.ComponentModel.Description("ExpressionController LSB")]
    ExpressionController_LSB            = 43,
    /// <summary>EffectControl_1_LSB (44)</summary>
    [System.ComponentModel.Description("EffectControl 1 LSB")]
    EffectControl_1_LSB                 = 44,
    /// <summary>EffectControl_2_LSB (45)</summary>
    [System.ComponentModel.Description("EffectControl 2 LSB")]
    EffectControl_2_LSB                 = 45,
    /// <summary>CC_046 (46)</summary>
    [System.ComponentModel.Description("CC 046")]
    CC_046                              = 46,
    /// <summary>CC_047 (47)</summary>
    [System.ComponentModel.Description("CC 047")]
    CC_047                              = 47,
    /// <summary>GeneralPurposeController_1_LSB (48)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 1 LSB")]
    GeneralPurposeController_1_LSB      = 48,
    /// <summary>GeneralPurposeController_2_LSB (49)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 2 LSB")]
    GeneralPurposeController_2_LSB      = 49,
    /// <summary>GeneralPurposeController_3_LSB (50)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 3 LSB")]
    GeneralPurposeController_3_LSB      = 50,
    /// <summary>GeneralPurposeController_4_LSB (51)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 4 LSB")]
    GeneralPurposeController_4_LSB      = 51,
    /// <summary>CC_052 (52)</summary>
    [System.ComponentModel.Description("CC 052")]
    CC_052                              = 52,
    /// <summary>CC_053 (53)</summary>
    [System.ComponentModel.Description("CC 053")]
    CC_053                              = 53,
    /// <summary>CC_054 (54)</summary>
    [System.ComponentModel.Description("CC 054")]
    CC_054                              = 54,
    /// <summary>CC_055 (55)</summary>
    [System.ComponentModel.Description("CC 055")]
    CC_055                              = 55,
    /// <summary>CC_056 (56)</summary>
    [System.ComponentModel.Description("CC 056")]
    CC_056                              = 56,
    /// <summary>CC_057 (57)</summary>
    [System.ComponentModel.Description("CC 057")]
    CC_057                              = 57,
    /// <summary>CC_058 (58)</summary>
    [System.ComponentModel.Description("CC 058")]
    CC_058                              = 58,
    /// <summary>CC_059 (59)</summary>
    [System.ComponentModel.Description("CC 059")]
    CC_059                              = 59,
    /// <summary>CC_060 (60)</summary>
    [System.ComponentModel.Description("CC 060")]
    CC_060                              = 60,
    /// <summary>CC_061 (61)</summary>
    [System.ComponentModel.Description("CC 061")]
    CC_061                              = 61,
    /// <summary>CC_062 (62)</summary>
    [System.ComponentModel.Description("CC 062")]
    CC_062                              = 62,
    /// <summary>CC_063 (63)</summary>
    [System.ComponentModel.Description("CC 063")]
    CC_063                              = 63,
    /// <summary>DamperPedal (64)</summary>
    [System.ComponentModel.Description("DamperPedal")]
    DamperPedal                         = 64,
    /// <summary>PortamentoToggle (65)</summary>
    [System.ComponentModel.Description("PortamentoToggle")]
    PortamentoToggle                    = 65,
    /// <summary>Sustenuto (66)</summary>
    [System.ComponentModel.Description("Sustenuto")]
    Sustenuto                           = 66,
    /// <summary>SoftPedal (67)</summary>
    [System.ComponentModel.Description("SoftPedal")]
    SoftPedal                           = 67,
    /// <summary>LegatoFootswitch (68)</summary>
    [System.ComponentModel.Description("LegatoFootswitch")]
    LegatoFootswitch                    = 68,
    /// <summary>Hold2 (69)</summary>
    [System.ComponentModel.Description("Hold2")]
    Hold2                               = 69,
    /// <summary>SoundController_01_Variation (70)</summary>
    [System.ComponentModel.Description("SoundController 01 Variation")]
    SoundController_01_Variation        = 70,
    /// <summary>SoundController_02_Timbre_Resonance (71)</summary>
    [System.ComponentModel.Description("SoundController 02 Timbre_Resonance")]
    SoundController_02_Timbre_Resonance = 71,
    /// <summary>SoundController_03_Release (72)</summary>
    [System.ComponentModel.Description("SoundController 03 Release")]
    SoundController_03_Release          = 72,
    /// <summary>SoundController_04_Attack (73)</summary>
    [System.ComponentModel.Description("SoundController 04 Attack")]
    SoundController_04_Attack           = 73,
    /// <summary>SoundController_05_Brightness (74)</summary>
    [System.ComponentModel.Description("SoundController 05 Brightness")]
    SoundController_05_Brightness       = 74,
    /// <summary>SoundController_06_Decay (75)</summary>
    [System.ComponentModel.Description("SoundController 06 Decay")]
    SoundController_06_Decay            = 75,
    /// <summary>SoundController_07_VibratoRate (76)</summary>
    [System.ComponentModel.Description("SoundController 07 VibratoRate")]
    SoundController_07_VibratoRate      = 76,
    /// <summary>SoundController_08_Vibrato_Depth (77)</summary>
    [System.ComponentModel.Description("SoundController 08 Vibrato Depth")]
    SoundController_08_Vibrato_Depth    = 77,
    /// <summary>SoundController_09_Vibrato_Delay (78)</summary>
    [System.ComponentModel.Description("SoundController 09 Vibrato Delay")]
    SoundController_09_Vibrato_Delay    = 78,
    /// <summary>SoundController10 (79)</summary>
    [System.ComponentModel.Description("SoundController10")]
    SoundController10                   = 79,
    /// <summary>GeneralPurposeController_5_LSB (80)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 5 LSB")]
    GeneralPurposeController_5_LSB      = 80,
    /// <summary>GeneralPurposeController_6_LSB (81)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 6 LSB")]
    GeneralPurposeController_6_LSB      = 81,
    /// <summary>GeneralPurposeController_7_LSB (82)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 7 LSB")]
    GeneralPurposeController_7_LSB      = 82,
    /// <summary>GeneralPurposeController_8_LSB (83)</summary>
    [System.ComponentModel.Description("GeneralPurposeController 8 LSB")]
    GeneralPurposeController_8_LSB      = 83,
    /// <summary>PortamentoControl (84)</summary>
    [System.ComponentModel.Description("PortamentoControl")]
    PortamentoControl                   = 84,
    /// <summary>CC_085 (85)</summary>
    [System.ComponentModel.Description("CC 085")]
    CC_085                              = 85,
    /// <summary>CC_086 (86)</summary>
    [System.ComponentModel.Description("CC 086")]
    CC_086                              = 86,
    /// <summary>CC_087 (87)</summary>
    [System.ComponentModel.Description("CC 087")]
    CC_087                              = 87,
    /// <summary>CC_088 (88)</summary>
    [System.ComponentModel.Description("CC 088")]
    CC_088                              = 88,
    /// <summary>CC_089 (89)</summary>
    [System.ComponentModel.Description("CC 089")]
    CC_089                              = 89,
    /// <summary>CC_090 (90)</summary>
    [System.ComponentModel.Description("CC 090")]
    CC_090                              = 90,
    /// <summary>Effects_1_Depth_ReverbSendLevel (91)</summary>
    [System.ComponentModel.Description("Effects 1 Depth ReverbSendLevel")]
    Effects_1_Depth_ReverbSendLevel     = 91,
    /// <summary>Effects_2_Depth (92)</summary>
    [System.ComponentModel.Description("Effects 2 Depth")]
    Effects_2_Depth                     = 92,
    /// <summary>Effects_3_Depth_ChorusSendLevel (93)</summary>
    [System.ComponentModel.Description("Effects 3 Depth ChorusSendLevel")]
    Effects_3_Depth_ChorusSendLevel     = 93,
    /// <summary>Effects_4_Depth (94)</summary>
    [System.ComponentModel.Description("Effects 4 Depth")]
    Effects_4_Depth                     = 94,
    /// <summary>Effects_5_Depth (95)</summary>
    [System.ComponentModel.Description("Effects 5 Depth")]
    Effects_5_Depth                     = 95,
    /// <summary>DataEntry_PlusOne (96)</summary>
    [System.ComponentModel.Description("DataEntry PlusOne")]
    DataEntry_PlusOne                   = 96,
    /// <summary>DataEntry_MinusOne (97)</summary>
    [System.ComponentModel.Description("DataEntry MinusOne")]
    DataEntry_MinusOne                  = 97,
    /// <summary>NRPN_LSB (98)</summary>
    [System.ComponentModel.Description("NRPN LSB")]
    NRPN_LSB                            = 98,
    /// <summary>NRPN_MSB (99)</summary>
    [System.ComponentModel.Description("NRPN MSB")]
    NRPN_MSB                            = 99,
    /// <summary>RPN_LSB (100)</summary>
    [System.ComponentModel.Description("RPN LSB")]
    RPN_LSB                             = 100,
    /// <summary>RPN_MSB (101)</summary>
    [System.ComponentModel.Description("RPN MSB")]
    RPN_MSB                             = 101,
    /// <summary>CC_102 (102)</summary>
    [System.ComponentModel.Description("CC 102")]
    CC_102                              = 102,
    /// <summary>CC_103 (103)</summary>
    [System.ComponentModel.Description("CC 103")]
    CC_103                              = 103,
    /// <summary>CC_104 (104)</summary>
    [System.ComponentModel.Description("CC 104")]
    CC_104                              = 104,
    /// <summary>CC_105 (105)</summary>
    [System.ComponentModel.Description("CC 105")]
    CC_105                              = 105,
    /// <summary>CC_106 (106)</summary>
    [System.ComponentModel.Description("CC 106")]
    CC_106                              = 106,
    /// <summary>CC_107 (107)</summary>
    [System.ComponentModel.Description("CC 107")]
    CC_107                              = 107,
    /// <summary>CC_108 (108)</summary>
    [System.ComponentModel.Description("CC 108")]
    CC_108                              = 108,
    /// <summary>CC_109 (109)</summary>
    [System.ComponentModel.Description("CC 109")]
    CC_109                              = 109,
    /// <summary>CC_110 (110)</summary>
    [System.ComponentModel.Description("CC 110")]
    CC_110                              = 110,
    /// <summary>CC_111 (111)</summary>
    [System.ComponentModel.Description("CC 111")]
    CC_111                              = 111,
    /// <summary>CC_112 (112)</summary>
    [System.ComponentModel.Description("CC 112")]
    CC_112                              = 112,
    /// <summary>CC_113 (113)</summary>
    [System.ComponentModel.Description("CC 113")]
    CC_113                              = 113,
    /// <summary>CC_114 (114)</summary>
    [System.ComponentModel.Description("CC 114")]
    CC_114                              = 114,
    /// <summary>CC_115 (115)</summary>
    [System.ComponentModel.Description("CC 115")]
    CC_115                              = 115,
    /// <summary>CC_116 (116)</summary>
    [System.ComponentModel.Description("CC 116")]
    CC_116                              = 116,
    /// <summary>CC_117 (117)</summary>
    [System.ComponentModel.Description("CC 117")]
    CC_117                              = 117,
    /// <summary>CC_118 (118)</summary>
    [System.ComponentModel.Description("CC 118")]
    CC_118                              = 118,
    /// <summary>CC_119 (119)</summary>
    [System.ComponentModel.Description("CC 119")]
    CC_119                              = 119,
    /// <summary>AllSoundOff (120)</summary>
    [System.ComponentModel.Description("AllSoundOff")]
    AllSoundOff                         = 120,
    /// <summary>ResetAllControllers (121)</summary>
    [System.ComponentModel.Description("ResetAllControllers")]
    ResetAllControllers                 = 121,
    /// <summary>LocalControl_OnOffToggle (122)</summary>
    [System.ComponentModel.Description("LocalControl OnOffToggle")]
    LocalControl_OnOffToggle            = 122,
    /// <summary>AllNotesOFF (123)</summary>
    [System.ComponentModel.Description("AllNotesOFF")]
    AllNotesOFF                         = 123,
    /// <summary>OmniMode_OFF (124)</summary>
    [System.ComponentModel.Description("OmniMode OFF")]
    OmniMode_OFF                        = 124,
    /// <summary>OmniMode_ON (125)</summary>
    [System.ComponentModel.Description("OmniMode ON")]
    OmniMode_ON                         = 125,
    /// <summary>PolyMode_ON_OFF (126)</summary>
    [System.ComponentModel.Description("PolyMode ON OFF")]
    PolyMode_ON_OFF                     = 126,
    /// <summary>PolyMode_ON (127)</summary>
    [System.ComponentModel.Description("PolyMode ON")]
    PolyMode_ON                         = 127,
  }
}
