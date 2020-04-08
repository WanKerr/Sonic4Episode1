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
    public class GMS_EVE_RECORD_EVENT
    {
        public byte[] byte_param = new byte[2];
        public byte pos_x;
        public byte pos_y;
        public ushort id;
        public ushort flag;
        public sbyte left;
        public sbyte top;
        public byte width;
        public byte height;

        public ushort word_param
        {
            get
            {
                return (ushort)((uint)this.byte_param[1] << 8 | (uint)this.byte_param[0]);
            }
            set
            {
                this.byte_param[0] = (byte)((uint)value & (uint)byte.MaxValue);
                this.byte_param[1] = (byte)((int)value >> 8 & (int)byte.MaxValue);
            }
        }
    }
}
