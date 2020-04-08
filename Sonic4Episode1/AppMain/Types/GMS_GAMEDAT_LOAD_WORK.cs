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
    public class GMS_GAMEDAT_LOAD_WORK
    {
        public AppMain.GMS_GAMEDAT_LOAD_CONTEXT[] context = AppMain.New<AppMain.GMS_GAMEDAT_LOAD_CONTEXT>(AppMain.GMD_GAMEDAT_LOAD_CONTEXT_MAX);
        public ushort[] char_id = new ushort[1];
        public int context_num;
        public int proc_type;
        public bool load_finish;
        public bool post_finish;
        public ushort stage_id;

        internal void Clear()
        {
            for (int index = 0; index < this.context.Length; ++index)
                this.context[index].Clear();
            this.proc_type = 0;
            this.load_finish = false;
            this.post_finish = false;
            this.stage_id = (ushort)0;
            for (int index = 0; index < this.char_id.Length; ++index)
                this.char_id[index] = (ushort)0;
        }
    }
}
