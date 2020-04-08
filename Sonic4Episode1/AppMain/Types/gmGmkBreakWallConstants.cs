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
    public static class gmGmkBreakWallConstants
    {
        public const int GME_GMK_RECT_DATA_COL_WIDTH = 0;
        public const int GME_GMK_RECT_DATA_COL_HEIGHT = 1;
        public const int GME_GMK_RECT_DATA_COL_OFST_X = 2;
        public const int GME_GMK_RECT_DATA_COL_OFST_Y = 3;
        public const int GME_GMK_RECT_DATA_DEF_LEFT = 4;
        public const int GME_GMK_RECT_DATA_DEF_TOP = 5;
        public const int GME_GMK_RECT_DATA_DEF_RIGHT = 6;
        public const int GME_GMK_RECT_DATA_DEF_BOTTOM = 7;
        public const int GME_GMK_RECT_DATA_MAX = 8;
    }
}
