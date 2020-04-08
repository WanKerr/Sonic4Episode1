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
    private enum DME_TITLE_ACT
    {
        ACT_BTN_L = 0,
        ACT_BTN_C = 1,
        ACT_BTN_R = 2,
        ACT_BTN_L2 = 3,
        ACT_BTN_C2 = 4,
        ACT_BTN_R2 = 5,
        ACT_BTN_L3 = 6,
        ACT_BTN_C3 = 7,
        ACT_BTN_R3 = 8,
        ACT_BTN_L4 = 9,
        ACT_BTN_C4 = 10, // 0x0000000A
        ACT_BTN_R4 = 11, // 0x0000000B
        ACT_BTN_L5 = 12, // 0x0000000C
        ACT_BTN_C5 = 13, // 0x0000000D
        ACT_BTN_R5 = 14, // 0x0000000E
        ACT_BACK_BTN_L = 15, // 0x0000000F
        ACT_BACK_BTN_R = 16, // 0x00000010
        ACT_GAME_BTN_L = 17, // 0x00000011
        ACT_GAME_BTN_R = 18, // 0x00000012
        ACT_LANG = 19, // 0x00000013
        ACT_TEX_START = 19, // 0x00000013
        ACT_NONE = 20, // 0x00000014
        ACT_TEX_GAME = 20, // 0x00000014
        ACT_TEX_TUDUKI = 21, // 0x00000015
        ACT_TEX_OPTION = 22, // 0x00000016
        ACT_TEX_KANZENBAN = 23, // 0x00000017
        ACT_TEX_ACHIEVEMENTS = 24, // 0x00000018
        ACT_TEX_LEADERBOARDS = 25, // 0x00000019
        ACT_TEX_TOP_BACK = 26, // 0x0000001A
        ACT_TEX_TOP_GAME = 27, // 0x0000001B
        ACT_WIN_TEX_MSG1 = 28, // 0x0000001C
        ACT_WIN_TEX_MSG2 = 29, // 0x0000001D
        ACT_BTN_CANCEL = 30, // 0x0000001E
        ACT_BTN_X = 31, // 0x0000001F
        ACT_WIN_NO_BTN_L = 32, // 0x00000020
        ACT_WIN_NO_BTN_C = 33, // 0x00000021
        ACT_WIN_NO_BTN_R = 34, // 0x00000022
        ACT_WIN_YES_BTN_L = 35, // 0x00000023
        ACT_WIN_YES_BTN_C = 36, // 0x00000024
        ACT_WIN_YES_BTN_R = 37, // 0x00000025
        ACT_TEX_YES = 38, // 0x00000026
        ACT_TEX_NO = 39, // 0x00000027
        ACT_TEX_BACK = 40, // 0x00000028
        ACT_NUM = 41, // 0x00000029
    }
}
