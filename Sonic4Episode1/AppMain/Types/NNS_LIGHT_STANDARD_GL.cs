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
    public class NNS_LIGHT_STANDARD_GL
    {
        public readonly AppMain.NNS_VECTOR4D Position = new AppMain.NNS_VECTOR4D();
        public readonly AppMain.NNS_VECTOR SpotDirection = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public uint User;
        public AppMain.NNS_RGBA Ambient;
        public AppMain.NNS_RGBA Diffuse;
        public AppMain.NNS_RGBA Specular;
        public float SpotExponent;
        public float SpotCutoff;
        public float ConstantAttenuation;
        public float LinearAttenuation;
        public float QuadraticAttenuation;
    }
}
