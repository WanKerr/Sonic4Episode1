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
    public class OBS_COL_CHK_DATA : AppMain.IClearable
    {
        public int pos_x;
        public int pos_y;
        public ushort[] dir;
        public uint[] attr;
        public ushort flag;
        public ushort vec;

        public AppMain.OBS_COL_CHK_DATA Assign(AppMain.OBS_COL_CHK_DATA data)
        {
            if (this != data)
            {
                this.pos_x = data.pos_x;
                this.pos_y = data.pos_y;
                this.dir = data.dir;
                this.dir = data.dir;
                this.attr = data.attr;
                this.attr = data.attr;
                this.flag = data.flag;
                this.vec = data.vec;
            }
            return this;
        }

        public void Clear()
        {
            this.pos_x = 0;
            this.pos_y = 0;
            this.dir = (ushort[])null;
            this.attr = (uint[])null;
            this.flag = (ushort)0;
            this.vec = (ushort)0;
        }
    }
}
