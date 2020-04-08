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
    public struct SNNS_VECTOR
    {
        public float x;
        public float y;
        public float z;

        public SNNS_VECTOR(AppMain.NNS_VECTOR vec)
        {
            this.x = vec.x;
            this.y = vec.y;
            this.z = vec.z;
        }

        public void Assign(AppMain.NNS_VECTOR vec)
        {
            this.x = vec.x;
            this.y = vec.y;
            this.z = vec.z;
        }

        public void Assign(ref AppMain.SNNS_VECTOR vec)
        {
            this.x = vec.x;
            this.y = vec.y;
            this.z = vec.z;
        }
    }
}
