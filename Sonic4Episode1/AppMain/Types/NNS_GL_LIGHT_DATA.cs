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
    private class NNS_GL_LIGHT_DATA
    {
        public AppMain.NNS_RGBA Ambient = new AppMain.NNS_RGBA();
        public AppMain.NNS_RGBA Diffuse = new AppMain.NNS_RGBA();
        public AppMain.NNS_RGBA Specular = new AppMain.NNS_RGBA();
        public AppMain.NNS_VECTOR Direction = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public AppMain.NNS_VECTOR4D Position = new AppMain.NNS_VECTOR4D();
        public AppMain.NNS_VECTOR Target = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public int bEnable;
        public uint fType;
        public float Intensity;
        public int RotType;
        public AppMain.NNS_ROTATE_A32 Rotation;
        public int InnerAngle;
        public int OuterAngle;
        public float InnerRange;
        public float OuterRange;
        public float FallOffStart;
        public float FallOffEnd;
        public float SpotExponent;
        public float SpotCutoff;
        public float ConstantAttenuation;
        public float LinearAttenuation;
        public float QuadraticAttenuation;
    }
}
