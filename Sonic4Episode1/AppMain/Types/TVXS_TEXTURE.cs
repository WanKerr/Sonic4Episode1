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
    public struct TVXS_TEXTURE
    {
        public int tex_id;
        public uint vtx_num;
        public uint vtx_tbl_ofst;
        public uint prim_type;

        public TVXS_TEXTURE(byte[] data, int offset)
        {
            this.tex_id = MppBitConverter.ToInt32(data, offset);
            this.vtx_num = MppBitConverter.ToUInt32(data, offset + 4);
            this.vtx_tbl_ofst = MppBitConverter.ToUInt32(data, offset + 8);
            this.prim_type = MppBitConverter.ToUInt32(data, offset + 12);
        }

        public static int SizeBytes
        {
            get
            {
                return 16;
            }
        }
    }
}
