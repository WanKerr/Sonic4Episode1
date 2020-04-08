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
    public class A2S_SUB_MTN
    {
        public float scl_x;
        public float scl_y;
        public float rot;
        public float scl_accele;
        public float rot_accele;

        internal void Assign(AppMain.A2S_SUB_MTN old)
        {
            this.scl_x = old.scl_x;
            this.scl_y = old.scl_y;
            this.rot = old.rot;
            this.scl_accele = old.scl_accele;
            this.rot_accele = old.rot_accele;
        }
    }
}
