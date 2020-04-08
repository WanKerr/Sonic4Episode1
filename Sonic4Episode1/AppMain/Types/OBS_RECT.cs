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
    public class OBS_RECT : AppMain.IClearable
    {
        public AppMain.VecFx32 pos = new AppMain.VecFx32();
        public short left;
        public short top;
        public short back;
        public short right;
        public short bottom;
        public short front;

        public void Clear()
        {
            this.left = this.top = this.back = (short)0;
            this.right = (short)0;
            this.width = (ushort)0;
            this.height = (ushort)0;
            this.bottom = (short)0;
            this.height = (ushort)0;
            this.front = (short)0;
            this.depth = (short)0;
        }

        public ushort width
        {
            get
            {
                return (ushort)this.right;
            }
            set
            {
                this.right = (short)value;
            }
        }

        public ushort height
        {
            get
            {
                return (ushort)this.bottom;
            }
            set
            {
                this.bottom = (short)value;
            }
        }

        public short depth
        {
            get
            {
                return this.front;
            }
            set
            {
                this.front = value;
            }
        }
    }
}
