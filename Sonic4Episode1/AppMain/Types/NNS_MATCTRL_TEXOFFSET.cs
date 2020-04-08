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
    public class NNS_MATCTRL_TEXOFFSET
    {
        public AppMain.NNS_TEXCOORD offset = new AppMain.NNS_TEXCOORD();
        public int mode;

        public NNS_MATCTRL_TEXOFFSET()
        {
        }

        public NNS_MATCTRL_TEXOFFSET(int _mode, float _u, float _v)
        {
            this.mode = _mode;
            this.offset.u = _u;
            this.offset.v = _v;
        }
    }
}
