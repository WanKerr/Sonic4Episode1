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
    public struct MP_BLOCK
    {
        public ushort id;
        public ushort rot;
        public ushort flip_h;
        public ushort flip_v;

        public MP_BLOCK(ushort bitFieldValue)
        {
            this.id = (ushort)((uint)bitFieldValue & 4095U);
            this.rot = (ushort)((int)bitFieldValue >> 12 & 3);
            this.flip_h = (ushort)((int)bitFieldValue >> 14 & 1);
            this.flip_v = (ushort)((int)bitFieldValue >> 15 & 1);
        }
    }
}
