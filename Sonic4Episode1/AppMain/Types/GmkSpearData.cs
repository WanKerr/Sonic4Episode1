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
    public class GmkSpearData
    {
        public const int GME_GMK_TYPE_SPEAR_U = 0;
        public const int GME_GMK_TYPE_SPEAR_D = 1;
        public const int GME_GMK_TYPE_SPEAR_L = 2;
        public const int GME_GMK_TYPE_SPEAR_R = 3;
        public const int GME_GMK_TYPE_MAX = 4;
        public const int GME_GMK_RECT_DATA_LEFT = 0;
        public const int GME_GMK_RECT_DATA_TOP = 1;
        public const int GME_GMK_RECT_DATA_RIGHT = 2;
        public const int GME_GMK_RECT_DATA_BOTTOM = 3;
        public const int GME_GMK_RECT_DATA_MAX = 4;
    }
}
