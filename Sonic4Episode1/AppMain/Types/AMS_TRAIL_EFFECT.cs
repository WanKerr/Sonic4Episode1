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
    public class AMS_TRAIL_EFFECT
    {
        public AppMain.AMS_TRAIL_PARAM Work = new AppMain.AMS_TRAIL_PARAM();
        public AppMain.AMS_TRAIL_EFFECT pNext;
        public AppMain.AMS_TRAIL_EFFECT pPrev;
        public DoubleType<AppMain.AMTREffectProc, int> Procedure;
        public DoubleType<AppMain.AMTREffectProc, int> Destractor;
        public float fFrame;
        public float fEndFrame;
        public uint drawState;
        public ushort handleId;
        public short flag;

        public void Clear()
        {
            this.pNext = (AppMain.AMS_TRAIL_EFFECT)null;
            this.pPrev = (AppMain.AMS_TRAIL_EFFECT)null;
            this.Procedure = (DoubleType<AppMain.AMTREffectProc, int>)(AppMain.AMTREffectProc)null;
            this.Destractor = (DoubleType<AppMain.AMTREffectProc, int>)(AppMain.AMTREffectProc)null;
            this.fFrame = this.fEndFrame = 0.0f;
            this.drawState = 0U;
            this.handleId = (ushort)0;
            this.flag = (short)0;
            this.Work.Clear();
        }
    }
}
