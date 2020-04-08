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
    public struct _last_key
    {
        public int trs;
        public int mtn;
        public int anm;
        public int mat;
        public int atrs;
        public int amtn;
        public int amat;
        public int usr;
        public int hit;

        public void Clear()
        {
            this.trs = this.mtn = this.anm = this.mat = this.atrs = this.amtn = this.amat = this.usr = this.hit = 0;
        }
    }
}
