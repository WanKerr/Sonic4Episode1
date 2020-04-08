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
    public class AMS_AME_BOUNDING
    {
        public readonly AppMain.NNS_VECTOR4D center = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        public float radius;
        public float radius2;

        public AppMain.AMS_AME_BOUNDING Assign(AppMain.AMS_AME_BOUNDING bound)
        {
            this.center.Assign(bound.center);
            this.radius = bound.radius;
            this.radius2 = bound.radius2;
            return this;
        }

        public void Clear()
        {
            this.center.Clear();
            this.radius = 0.0f;
            this.radius2 = 0.0f;
        }
    }
}
