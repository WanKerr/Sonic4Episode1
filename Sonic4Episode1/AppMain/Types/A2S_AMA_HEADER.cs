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
    public class A2S_AMA_HEADER
    {
        public uint version;
        public uint node_num;
        public uint act_num;
        public int node_tbl_offset;
        public AppMain.A2S_AMA_NODE[] node_tbl;
        public int act_tbl_offset;
        public AppMain.A2S_AMA_ACT[] act_tbl;
        public int node_name_tbl_offset;
        public int act_name_tbl_offset;
    }
}
