using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using accel;
using dbg;
using er;
using er.web;
using gs;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using mpp;
using setting;

public partial class AppMain
{
    private enum DME_FILESLCT_ACT
    {
        ACT_BACK_BG = 0,
        ACT_ARROW_UP = 1,
        ACT_ARROW_DOWN = 2,
        ACT_UP_HIDE_BG = 3,
        ACT_DOWN_HIDE_BG = 4,
        ACT_TEX_DEL = 5,
        ACT_TEX_EXP1 = 6,
        ACT_TEX_EXP2 = 7,
        ACT_FILE_TAB1 = 8,
        ACT_TAB_ALL_SRC = 8,
        ACT_FILE_SCR_BASE = 9,
        ACT_FILE_TAB_NUM = 10, // 0x0000000A
        ACT_FILE_TAB_EMER = 11, // 0x0000000B
        ACT_FILE_SCR = 12, // 0x0000000C
        ACT_FILE_TIME_H_1 = 13, // 0x0000000D
        ACT_FILE_TIME_H_2 = 14, // 0x0000000E
        ACT_FILE_TIME_COLON = 15, // 0x0000000F
        ACT_FILE_TIME_M_1 = 16, // 0x00000010
        ACT_FILE_TIME_M_2 = 17, // 0x00000011
        ACT_FILE_ICON_EMER1 = 18, // 0x00000012
        ACT_FILE_ICON_EMER2 = 19, // 0x00000013
        ACT_FILE_ICON_EMER3 = 20, // 0x00000014
        ACT_FILE_ICON_EMER4 = 21, // 0x00000015
        ACT_FILE_ICON_EMER5 = 22, // 0x00000016
        ACT_FILE_ICON_EMER6 = 23, // 0x00000017
        ACT_FILE_ICON_EMER7 = 24, // 0x00000018
        ACT_FILE_TEX_ALPHA1 = 25, // 0x00000019
        ACT_FILE_TEX_ALPHA2 = 26, // 0x0000001A
        ACT_FILE_TEX_ALPHA3 = 27, // 0x0000001B
        ACT_FILE_TEX_ALPHA4 = 28, // 0x0000001C
        ACT_FILE_TEX_ALPHA5 = 29, // 0x0000001D
        ACT_FILE_TEX_ALPHA6 = 30, // 0x0000001E
        ACT_FILE_TEX_ALPHA7 = 31, // 0x0000001F
        ACT_FILE_TEX_ALPHA8 = 32, // 0x00000020
        ACT_FILE_TEX_ALPHA9 = 33, // 0x00000021
        ACT_FILE_TEX_ALPHA10 = 34, // 0x00000022
        ACT_FILE_TAB3_B = 35, // 0x00000023
        ACT_FILE_TAB3_A = 36, // 0x00000024
        ACT_FILE_TAB3_C = 37, // 0x00000025
        ACT_TAB_ALL_DST = 37, // 0x00000025
        ACT_FILE_TAB4_B = 38, // 0x00000026
        ACT_NONE = 38, // 0x00000026
        ACT_FILE_TAB4_A = 39, // 0x00000027
        ACT_FILE_TAB4_C = 40, // 0x00000028
        ACT_FILE_YEAR1 = 41, // 0x00000029
        ACT_FILE_YEAR2 = 42, // 0x0000002A
        ACT_FILE_YEAR3 = 43, // 0x0000002B
        ACT_FILE_YEAR4 = 44, // 0x0000002C
        ACT_FILE_SLASH1 = 45, // 0x0000002D
        ACT_FILE_MON1 = 46, // 0x0000002E
        ACT_FILE_MON2 = 47, // 0x0000002F
        ACT_FILE_SLASH2 = 48, // 0x00000030
        ACT_FILE_DAY1 = 49, // 0x00000031
        ACT_FILE_DAY2 = 50, // 0x00000032
        ACT_FILE_YEAR1_US = 51, // 0x00000033
        ACT_FILE_YEAR2_US = 52, // 0x00000034
        ACT_FILE_YEAR3_US = 53, // 0x00000035
        ACT_FILE_YEAR4_US = 54, // 0x00000036
        ACT_FILE_SLASH1_US = 55, // 0x00000037
        ACT_FILE_MON1_US = 56, // 0x00000038
        ACT_FILE_MON2_US = 57, // 0x00000039
        ACT_FILE_SLASH2_US = 58, // 0x0000003A
        ACT_FILE_DAY1_US = 59, // 0x0000003B
        ACT_FILE_DAY2_US = 60, // 0x0000003C
        ACT_FILE_YEAR1_EU = 61, // 0x0000003D
        ACT_FILE_YEAR2_EU = 62, // 0x0000003E
        ACT_FILE_YEAR3_EU = 63, // 0x0000003F
        ACT_FILE_YEAR4_EU = 64, // 0x00000040
        ACT_FILE_SLASH1_EU = 65, // 0x00000041
        ACT_FILE_MON1_EU = 66, // 0x00000042
        ACT_FILE_MON2_EU = 67, // 0x00000043
        ACT_FILE_SLASH2_EU = 68, // 0x00000044
        ACT_FILE_DAY1_EU = 69, // 0x00000045
        ACT_FILE_DAY2_EU = 70, // 0x00000046
        ACT_FILE_TEX_NAME = 71, // 0x00000047
        ACT_FILE_TEX_NEWGAME = 72, // 0x00000048
        ACT_WIN_TEX_BACK = 73, // 0x00000049
        ACT_WIN_TEX_MSG = 74, // 0x0000004A
        ACT_FILESLCT_BACK_BTN = 75, // 0x0000004B
        ACT_BTN_CANCEL = 76, // 0x0000004C
        ACT_DEL_BTN = 77, // 0x0000004D
        ACT_OBI_C = 78, // 0x0000004E
        ACT_OBI_L = 79, // 0x0000004F
        ACT_OBI_R = 80, // 0x00000050
        ACT_OBI_R2 = 81, // 0x00000051
        ACT_WIN_LINE = 82, // 0x00000052
        ACT_TEX_WINTITLE = 83, // 0x00000053
        ACT_TEX_YES = 84, // 0x00000054
        ACT_TEX_NO = 85, // 0x00000055
        ACT_TEX_BACK = 86, // 0x00000056
        ACT_TEX_FIX_BACK = 87, // 0x00000057
        ACT_NUM = 88, // 0x00000058
    }
}
