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
    public class OBS_COLLISION_OBJ
    {
        public AppMain.VecFx32 pos = new AppMain.VecFx32();
        public AppMain.VecFx32 check_pos = new AppMain.VecFx32();
        public AppMain.OBS_OBJECT_WORK obj;
        public AppMain.OBS_OBJECT_WORK rider_obj;
        public AppMain.OBS_OBJECT_WORK toucher_obj;
        public short ofst_x;
        public short ofst_y;
        public uint flag;
        public ushort dir;
        public ushort attr;
        public byte[] diff_data;
        public byte[] dir_data;
        public byte[] attr_data;
        public ushort width;
        public ushort height;
        public short check_ofst_x;
        public short check_ofst_y;
        public int left;
        public int top;
        public int right;
        public int bottom;
        public ushort check_dir;
    }
}
