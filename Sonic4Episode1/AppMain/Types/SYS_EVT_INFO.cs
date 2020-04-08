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
    private class SYS_EVT_INFO
    {
        public sbyte[] arg = new sbyte[8];
        public AppMain.SYS_EVT_DATA[] evt_data;
        public int evt_data_num;
        public AppMain.SYS_EVT_DATA cur_evt_data;
        public short cur_evt_id;
        public short old_evt_id;
        public AppMain.SYS_EVT_DATA next_evt_data;
        public short req_evt_id;
        public short req_evt_case;
        public uint flag;
        public uint arg_size;
    }
}
