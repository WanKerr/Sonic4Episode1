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
    public class AMS_DRAWSTATE_ENVMAP
    {
        public readonly AppMain.NNS_MATRIX texmtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public int texsrc;

        public AMS_DRAWSTATE_ENVMAP()
        {
        }

        public AMS_DRAWSTATE_ENVMAP(AppMain.AMS_DRAWSTATE_ENVMAP drawState)
        {
            this.texsrc = drawState.texsrc;
            this.texmtx.Assign(drawState.texmtx);
        }

        public AppMain.AMS_DRAWSTATE_ENVMAP Assign(AppMain.AMS_DRAWSTATE_ENVMAP drawState)
        {
            if (this != drawState)
            {
                this.texsrc = drawState.texsrc;
                this.texmtx.Assign(drawState.texmtx);
            }
            return this;
        }

        public void Clear()
        {
            this.texsrc = 0;
            this.texmtx.Clear();
        }
    }
}
