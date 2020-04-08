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
    public class A2S_SUB_TRS
    {
        public float trs_x;
        public float trs_y;
        public float trs_z;
        public float trs_accele;

        internal void Assign(AppMain.A2S_SUB_TRS old)
        {
            this.trs_x = old.trs_x;
            this.trs_y = old.trs_y;
            this.trs_z = old.trs_z;
            this.trs_accele = old.trs_accele;
        }
    }
}
