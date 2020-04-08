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
    public class NNS_VTXLIST_COMMON_DESC
    {
        public readonly AppMain.NNS_VTXLIST_COMMON_ARRAY List0 = new AppMain.NNS_VTXLIST_COMMON_ARRAY();
        public readonly AppMain.NNS_VTXLIST_COMMON_ARRAY List1 = new AppMain.NNS_VTXLIST_COMMON_ARRAY();
        public readonly AppMain.NNS_VTXLIST_COMMON_ARRAY List2 = new AppMain.NNS_VTXLIST_COMMON_ARRAY();
        public readonly AppMain.NNS_VTXLIST_COMMON_ARRAY List3 = new AppMain.NNS_VTXLIST_COMMON_ARRAY();

        public AppMain.NNS_VTXLIST_COMMON_DESC Assign(AppMain.NNS_VTXLIST_COMMON_DESC desc)
        {
            if (this != desc)
            {
                this.List0.Assign(desc.List0);
                this.List1.Assign(desc.List1);
                this.List2.Assign(desc.List2);
                this.List3.Assign(desc.List3);
            }
            return this;
        }
    }
}
