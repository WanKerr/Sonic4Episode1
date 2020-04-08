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
    public class NNS_CAMERA_TARGET_ROLL : AppMain.IClearable
    {
        public readonly AppMain.NNS_VECTOR Position = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR Target = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public uint User;
        public int Fovy;
        public float Aspect;
        public float ZNear;
        public float ZFar;
        public int Roll;

        public void Clear()
        {
            this.User = 0U;
            this.Fovy = 0;
            this.Aspect = 0.0f;
            this.ZNear = 0.0f;
            this.ZFar = 0.0f;
            this.Roll = 0;
            this.Position.Clear();
            this.Target.Clear();
        }
    }
}
