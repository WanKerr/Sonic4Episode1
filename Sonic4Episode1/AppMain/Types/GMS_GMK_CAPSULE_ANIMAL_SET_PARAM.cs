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
    private struct GMS_GMK_CAPSULE_ANIMAL_SET_PARAM
    {
        public short ofs_x;
        public short ofs_y;
        public short ofs_z;
        public byte type;
        public byte vec;
        public ushort time;

        public GMS_GMK_CAPSULE_ANIMAL_SET_PARAM(
          short ofx,
          short ofy,
          short ofz,
          byte tp,
          byte vc,
          ushort tm)
        {
            this.ofs_x = ofx;
            this.ofs_y = ofy;
            this.ofs_z = ofz;
            this.type = tp;
            this.vec = vc;
            this.time = tm;
        }
    }
}
