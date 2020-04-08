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
    public class GMS_BOSS5_BODY_SUBSEQ_ENTER_INFO
    {
        public AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK enter_func;
        public int super_state;

        public GMS_BOSS5_BODY_SUBSEQ_ENTER_INFO(AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK a, int b)
        {
            this.enter_func = a;
            this.super_state = b;
        }
    }
}
