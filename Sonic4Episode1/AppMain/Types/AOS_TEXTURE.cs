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
    public class AOS_TEXTURE : AppMain.IClearable
    {
        public AppMain.NNS_TEXLIST texlist;
        public object texlist_buf;
        public int reg_id;
        public AppMain.AMS_AMB_HEADER amb;
        public AppMain.TXB_HEADER txb;

        public void Clear()
        {
            this.texlist = (AppMain.NNS_TEXLIST)null;
            this.texlist_buf = (object)null;
            this.reg_id = 0;
            this.amb = (AppMain.AMS_AMB_HEADER)null;
            this.txb = (AppMain.TXB_HEADER)null;
        }
    }
}
