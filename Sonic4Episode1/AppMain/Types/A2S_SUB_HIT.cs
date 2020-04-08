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
    public class A2S_SUB_HIT
    {
        public uint flag;
        public uint type;
        public float hit_accele;
        public uint pad;
        public AppMain.A2S_SUB_RECT rect;
        public AppMain.A2S_SUB_CIRCLE circle;

        internal void Assign(AppMain.A2S_SUB_HIT old)
        {
            this.flag = old.flag;
            this.type = old.type;
            this.hit_accele = old.hit_accele;
            this.pad = old.pad;
            this.rect = old.rect;
            this.circle = old.circle;
        }
    }
}
