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
    public class GMS_EVE_MGR_WORK
    {
        public float[] prev_pos = new float[2];
        public ushort[] map_size = new ushort[2];
        public AppMain._sts_proc_ sts_proc;
        public uint flag;

        public void Clear()
        {
            this.sts_proc = (AppMain._sts_proc_)null;
            this.flag = 0U;
            Array.Clear((Array)this.prev_pos, 0, 2);
            Array.Clear((Array)this.map_size, 0, 2);
        }
    }
}
