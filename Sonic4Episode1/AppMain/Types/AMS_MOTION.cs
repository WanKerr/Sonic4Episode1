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
    public class AMS_MOTION
    {
        public readonly AppMain.AMS_MOTION_FILE[] mtnfile = AppMain.New<AppMain.AMS_MOTION_FILE>(4);
        public readonly AppMain.AMS_MOTION_BUF[] mbuf = AppMain.New<AppMain.AMS_MOTION_BUF>(2);
        public AppMain.NNS_OBJECT _object;
        public int node_num;
        public int motion_num;
        public AppMain.NNS_MOTION[] mtnbuf;
        public AppMain.NNS_TRS[] data;
        public AppMain.ArrayPointer<AppMain.NNS_TRS> mmbuf;
        public AppMain.NNS_OBJECT mmobject;
        public uint mmobj_size;
        public int mmotion_num;
        public AppMain.NNS_MOTION[] mmtn;
        public int mmotion_id;
        public float mmotion_frame;
    }
}
