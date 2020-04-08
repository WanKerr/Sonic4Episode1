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
    public class GMDECO_GLARE_INTERFACE : AppMain.IClearable
    {
        public AppMain.AMS_AMB_HEADER amb_header;
        public AppMain.NNS_TEXFILELIST tex_buf;
        public object texlistbuf;
        public AppMain.NNS_TEXLIST texlist;
        public int texId;
        public int regId;
        public int drawFlag;

        public void Clear()
        {
            this.amb_header = (AppMain.AMS_AMB_HEADER)null;
            this.tex_buf = (AppMain.NNS_TEXFILELIST)null;
            this.texlistbuf = (object)null;
            this.texlist = (AppMain.NNS_TEXLIST)null;
            this.texId = this.regId = this.drawFlag = 0;
        }
    }
}
