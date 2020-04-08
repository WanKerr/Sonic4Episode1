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
    public class A2S_SUB_ANM
    {
        public int tex_id;
        public uint clamp;
        public uint filter;
        public float texel_accele;
        public AppMain.A2S_SUB_RECT texel;

        internal void Assign(AppMain.A2S_SUB_ANM old)
        {
            this.tex_id = old.tex_id;
            this.clamp = old.clamp;
            this.filter = old.filter;
            this.texel_accele = old.texel_accele;
            this.texel = old.texel;
        }
    }
}
