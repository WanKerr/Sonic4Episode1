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
    public class NNS_TEXLIST
    {
        public int nTex;
        public AppMain.NNS_TEXINFO[] pTexInfoList;

        public NNS_TEXLIST()
        {
        }

        public NNS_TEXLIST(AppMain.NNS_TEXLIST pFrom)
        {
            this.nTex = pFrom.nTex;
            this.pTexInfoList = new AppMain.NNS_TEXINFO[this.nTex];
            for (int index = 0; index < this.nTex; ++index)
                this.pTexInfoList[index] = new AppMain.NNS_TEXINFO(pFrom.pTexInfoList[index]);
        }

        public void Clear()
        {
            this.nTex = 0;
            this.pTexInfoList = (AppMain.NNS_TEXINFO[])null;
        }
    }
}
