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
    private enum DME_STFRL_ACT
    {
        ACT_LIGHT_BG_LT,
        ACT_LIGHT_BG_LB,
        ACT_LIGHT_BG_RT,
        ACT_LIGHT_BG_RB,
        ACT_METAL_SONIC,
        ACT_M_SONIC_EYE,
        ACT_BLACK_BG,
        ACT_WHITE_BG,
        ACT_TEX_TRYAGAIN,
        ACT_TEX_CONTINUED,
        ACT_TEX_WIN_MSG,
        ACT_NUM,
        ACT_NONE,
    }
}
