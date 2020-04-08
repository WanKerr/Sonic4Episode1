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
    public class AMS_TRAIL_PARTS
    {
        public readonly AppMain.NNS_VECTOR pos = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR sub_pos = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR dir = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public ushort[] Dummy = new ushort[5];
        public float time;
        public AppMain.AMS_TRAIL_PARTS pNext;
        public AppMain.AMS_TRAIL_PARTS pPrev;
        public uint m_Flag;
        public short partsId;

        public void Clear()
        {
            this.pos.Clear();
            this.sub_pos.Clear();
            this.dir.Clear();
            this.time = 0.0f;
            this.pNext = (AppMain.AMS_TRAIL_PARTS)null;
            this.pPrev = (AppMain.AMS_TRAIL_PARTS)null;
            this.m_Flag = 0U;
            this.partsId = (short)0;
            Array.Clear((Array)this.Dummy, 0, this.Dummy.Length);
        }
    }
}
