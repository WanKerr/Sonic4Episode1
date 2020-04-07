using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    private enum AOE_PRESENCE
    {
        AOD_PRESENCE_DEFAULT = 0,
        AOD_PRESENCE_STANDBY = 0,
        AOD_PRESENCE_Z11T = 1,
        AOD_PRESENCE_Z11S = 2,
        AOD_PRESENCE_Z12T = 3,
        AOD_PRESENCE_Z12S = 4,
        AOD_PRESENCE_Z13T = 5,
        AOD_PRESENCE_Z13S = 6,
        AOD_PRESENCE_Z1BT = 7,
        AOD_PRESENCE_Z1BS = 8,
        AOD_PRESENCE_Z21T = 9,
        AOD_PRESENCE_Z21S = 10, // 0x0000000A
        AOD_PRESENCE_Z22T = 11, // 0x0000000B
        AOD_PRESENCE_Z22S = 12, // 0x0000000C
        AOD_PRESENCE_Z23T = 13, // 0x0000000D
        AOD_PRESENCE_Z23S = 14, // 0x0000000E
        AOD_PRESENCE_Z2BT = 15, // 0x0000000F
        AOD_PRESENCE_Z2BS = 16, // 0x00000010
        AOD_PRESENCE_Z31T = 17, // 0x00000011
        AOD_PRESENCE_Z31S = 18, // 0x00000012
        AOD_PRESENCE_Z32T = 19, // 0x00000013
        AOD_PRESENCE_Z32S = 20, // 0x00000014
        AOD_PRESENCE_Z33T = 21, // 0x00000015
        AOD_PRESENCE_Z33S = 22, // 0x00000016
        AOD_PRESENCE_Z3BT = 23, // 0x00000017
        AOD_PRESENCE_Z3BS = 24, // 0x00000018
        AOD_PRESENCE_Z41T = 25, // 0x00000019
        AOD_PRESENCE_Z41S = 26, // 0x0000001A
        AOD_PRESENCE_Z42T = 27, // 0x0000001B
        AOD_PRESENCE_Z42S = 28, // 0x0000001C
        AOD_PRESENCE_Z43T = 29, // 0x0000001D
        AOD_PRESENCE_Z43S = 30, // 0x0000001E
        AOD_PRESENCE_Z4BT = 31, // 0x0000001F
        AOD_PRESENCE_Z4BS = 32, // 0x00000020
        AOD_PRESENCE_ZFBT = 33, // 0x00000021
        AOD_PRESENCE_ZFBS = 34, // 0x00000022
        AOD_PRESENCE_SS1T = 35, // 0x00000023
        AOD_PRESENCE_SS1S = 36, // 0x00000024
        AOD_PRESENCE_SS2T = 37, // 0x00000025
        AOD_PRESENCE_SS2S = 38, // 0x00000026
        AOD_PRESENCE_SS3T = 39, // 0x00000027
        AOD_PRESENCE_SS3S = 40, // 0x00000028
        AOD_PRESENCE_SS4T = 41, // 0x00000029
        AOD_PRESENCE_SS4S = 42, // 0x0000002A
        AOD_PRESENCE_SS5T = 43, // 0x0000002B
        AOD_PRESENCE_SS5S = 44, // 0x0000002C
        AOD_PRESENCE_SS6T = 45, // 0x0000002D
        AOD_PRESENCE_SS6S = 46, // 0x0000002E
        AOD_PRESENCE_SS7T = 47, // 0x0000002F
        AOD_PRESENCE_SS7S = 48, // 0x00000030
        AOD_PRESENCE_NUM = 49, // 0x00000031
        AOD_PRESENCE_NONE = 50, // 0x00000032
    }
    private static void AoPresenceInit()
    {
    }

    private static bool AoPresenceInitialized()
    {
        return true;
    }

    private static void AoPresenceExit()
    {
    }

    private static void AoPresenceSet(AppMain.AOE_PRESENCE presence, bool is_trial)
    {
    }

}