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
    public class NNS_PRIMLIST_COMMON_TRIANGLE_STRIP
    {
        public uint fType;
        public int nIndexSetSize;
        public int nStrip;
        public ushort[] pLengthList;
        public ushort[] pStripList;

        public AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_STRIP Assign(
          AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_STRIP desc)
        {
            this.fType = desc.fType;
            this.nIndexSetSize = desc.nIndexSetSize;
            this.nStrip = desc.nStrip;
            this.pLengthList = desc.pLengthList;
            this.pStripList = desc.pStripList;
            return this;
        }
    }
}
