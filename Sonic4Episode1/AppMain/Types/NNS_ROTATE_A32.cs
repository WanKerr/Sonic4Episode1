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
    public struct NNS_ROTATE_A32
    {
        public int x;
        public int y;
        public int z;

        public NNS_ROTATE_A32(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static explicit operator int[] (AppMain.NNS_ROTATE_A32 rv)
        {
            return new int[3] { rv.x, rv.y, rv.z };
        }

        public static explicit operator AppMain.NNS_ROTATE_A32(int[] array)
        {
            return new AppMain.NNS_ROTATE_A32()
            {
                x = array[0],
                y = array[1],
                z = array[2]
            };
        }
    }
}
