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
    public class HGS_TROPHY_CHECK_INFO
    {
        public int trophy_type;
        public uint trophy_id;
        public AppMain.HGF_TROPHY_ACQUIRE_CHECK_FUNC acquire_check_func;

        public HGS_TROPHY_CHECK_INFO(
          int trophy,
          uint trophy_id,
          AppMain.HGF_TROPHY_ACQUIRE_CHECK_FUNC acquire_check_func)
        {
            this.trophy_type = trophy;
            this.trophy_id = trophy_id;
            this.acquire_check_func = acquire_check_func;
        }

        public HGS_TROPHY_CHECK_INFO()
        {
        }
    }
}
