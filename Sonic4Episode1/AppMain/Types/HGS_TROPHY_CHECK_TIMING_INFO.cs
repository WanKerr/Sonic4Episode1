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
    public class HGS_TROPHY_CHECK_TIMING_INFO
    {
        public AppMain.HGS_TROPHY_CHECK_INFO[] check_info_tbl;
        public int num;

        public HGS_TROPHY_CHECK_TIMING_INFO(AppMain.HGS_TROPHY_CHECK_INFO[] tbl, int num)
        {
            this.check_info_tbl = tbl;
            this.num = num;
        }

        public HGS_TROPHY_CHECK_TIMING_INFO()
        {
        }
    }
}
