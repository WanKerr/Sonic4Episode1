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
    private static class gmBoss4ChibiGetAttackTypeStatics
    {
        public static int _index = 0;
        public static readonly int[] life2_tbl = new int[20]
        {
      0,
      0,
      0,
      0,
      1,
      0,
      0,
      0,
      0,
      1,
      0,
      0,
      0,
      0,
      1,
      0,
      0,
      1,
      0,
      0
        };
        public static readonly int[] life1_tbl = new int[20]
        {
      0,
      1,
      0,
      1,
      0,
      2,
      1,
      2,
      1,
      2,
      0,
      1,
      2,
      1,
      0,
      2,
      1,
      0,
      1,
      2
        };
        public static readonly int[] life3_tbl_f = new int[20]
        {
      0,
      0,
      3,
      0,
      0,
      0,
      0,
      3,
      0,
      0,
      0,
      0,
      3,
      0,
      0,
      0,
      0,
      3,
      0,
      0
        };
        public static readonly int[] life2_tbl_f = new int[20]
        {
      0,
      0,
      3,
      0,
      1,
      0,
      0,
      3,
      0,
      1,
      0,
      0,
      3,
      0,
      1,
      0,
      1,
      3,
      0,
      0
        };
        public static readonly int[] life1_tbl_f = new int[20]
        {
      0,
      1,
      3,
      1,
      0,
      2,
      1,
      0,
      1,
      2,
      3,
      1,
      0,
      1,
      3,
      2,
      1,
      3,
      1,
      2
        };
    }
}
