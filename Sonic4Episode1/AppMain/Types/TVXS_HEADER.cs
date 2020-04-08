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
    public struct TVXS_HEADER
    {
        public uint tex_num;
        public uint tex_tbl_ofst;

        public TVXS_HEADER(byte[] data, int offset)
        {
            this.tex_num = MppBitConverter.ToUInt32(data, offset + 4);
            this.tex_tbl_ofst = MppBitConverter.ToUInt32(data, offset + 8);
        }

        public static uint SizeBytes
        {
            get
            {
                return 16;
            }
        }
    }
}
