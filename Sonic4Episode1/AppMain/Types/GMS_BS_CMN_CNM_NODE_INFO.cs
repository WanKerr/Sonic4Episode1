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
    public class GMS_BS_CMN_CNM_NODE_INFO
    {
        public readonly AppMain.NNS_MATRIX node_w_mtx = new AppMain.NNS_MATRIX();
        public int node_index;
        public int enable;
        public uint mode;
        public uint flag;

        public void Assign(AppMain.GMS_BS_CMN_CNM_NODE_INFO p)
        {
            this.node_w_mtx.Assign(p.node_w_mtx);
            this.node_index = p.node_index;
            this.enable = p.enable;
            this.mode = p.mode;
            this.flag = p.flag;
        }
    }
}
