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
    public class GmkPistonData
    {
        public static readonly short[][] tbl_gm_gmk_piston_col_rect = new short[2][]
        {
      new short[8]
      {
        (short) 56,
        (short) 32,
        (short) -28,
        (short) 0,
        (short) -28,
        (short) 32,
        (short) 28,
        (short) 0
      },
      new short[8]
      {
        (short) 56,
        (short) 32,
        (short) -28,
        (short) -32,
        (short) -28,
        (short) 0,
        (short) 28,
        (short) 32
      }
        };
        public const int GME_GMK_TYPE_PISTON_UP = 0;
        public const int GME_GMK_TYPE_PISTON_DOWN = 1;
        public const int GME_GMK_TYPE_MAX = 2;
        public const int GMD_GMK_BOBJ1_RECT_WIDTH = 56;
        public const int GMD_GMK_BOBJ1_RECT_HEIGHT = 32;
        public const int GMD_GMK_BOBJ1_RECT_MARGIN = 32;
    }
}
