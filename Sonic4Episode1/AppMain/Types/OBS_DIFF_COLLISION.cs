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
    public class OBS_DIFF_COLLISION
    {
        public readonly AppMain.MP_BLOCK[][] block_map_datap = new AppMain.MP_BLOCK[2][];
        public AppMain.DF_BLOCK[] cl_diff_datap;
        public AppMain.DI_BLOCK[] direc_datap;
        public AppMain.AT_BLOCK[] char_attr_datap;
        public ushort map_block_num_x;
        public ushort map_block_num_y;
        public uint diff_block_num;
        public uint dir_block_num;
        public uint attr_block_num;
        public int left;
        public int top;
        public int right;
        public int bottom;

        internal void Clear()
        {
            this.cl_diff_datap = (AppMain.DF_BLOCK[])null;
            this.direc_datap = (AppMain.DI_BLOCK[])null;
            this.block_map_datap[0] = (AppMain.MP_BLOCK[])null;
            this.block_map_datap[1] = (AppMain.MP_BLOCK[])null;
            this.char_attr_datap = (AppMain.AT_BLOCK[])null;
            this.map_block_num_x = (ushort)0;
            this.map_block_num_y = (ushort)0;
            this.diff_block_num = 0U;
            this.dir_block_num = 0U;
            this.attr_block_num = 0U;
            this.left = 0;
            this.top = 0;
            this.right = 0;
            this.bottom = 0;
        }
    }
}
