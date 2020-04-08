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
    public class GMS_DECOGLARE_PARAM
    {
        public readonly uint color;
        public float size;
        public float sort_z;
        public int ablend;

        public GMS_DECOGLARE_PARAM(uint rgba, float Size, float Sort, int Ablend)
        {
            this.color = rgba;
            this.size = Size;
            this.sort_z = Sort;
            this.ablend = Ablend;
        }
    }
}
