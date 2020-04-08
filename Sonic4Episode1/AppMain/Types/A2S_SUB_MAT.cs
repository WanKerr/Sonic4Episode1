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
    public class A2S_SUB_MAT
    {
        public AppMain.A2S_SUB_COL base_;
        public AppMain.A2S_SUB_COL fade;
        public float base_accele;
        public float fade_accele;
        public uint blend;

        internal void Assign(AppMain.A2S_SUB_MAT old)
        {
            this.base_ = old.base_;
            this.fade = old.fade;
            this.base_accele = old.base_accele;
            this.fade_accele = old.fade_accele;
            this.blend = old.blend;
        }
    }
}
