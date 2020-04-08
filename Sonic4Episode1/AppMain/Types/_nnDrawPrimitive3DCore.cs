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
    private class _nnDrawPrimitive3DCore
    {
        public static AppMain.RGBA_U8[] cbuf = new AppMain.RGBA_U8[6];
        public static AppMain.NNS_PRIM3D_PCT[] prim_d = AppMain.New<AppMain.NNS_PRIM3D_PCT>(2048);
        public static AppMain.RGBA_U8[] prim_c = AppMain.New<AppMain.RGBA_U8>(2048);
        public static AppMain.NNS_PRIM3D_PCT_VertexData vertexData = new AppMain.NNS_PRIM3D_PCT_VertexData();
        public static AppMain.NNS_PRIM3D_PC_VertexData vertexDataPC = new AppMain.NNS_PRIM3D_PC_VertexData();
        public static AppMain.NNS_PRIM3D_PCT_TexCoordData texCoordData = new AppMain.NNS_PRIM3D_PCT_TexCoordData();
        public static AppMain.RGBA_U8_ColorData colorData = new AppMain.RGBA_U8_ColorData();
    }
}
