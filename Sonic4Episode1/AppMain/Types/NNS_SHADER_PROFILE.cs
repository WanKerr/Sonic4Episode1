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
    public class NNS_SHADER_PROFILE
    {
        private int[] TexCoord = AppMain.New<int>(8);
        private int bLighting;
        private int bSpecular;
        private int bTwoSidedLighting;
        private int nFragParallelLight;
        private int nFragPointLight;
        private int NormalMapType;
        private int bBaseMap;
        private int nDecalMap;
        private int bSpecularMap;
        private int bShininessMap;
        private int bDualParaboloidMap;
        private int bEnvMaskMap;
        private int bModulateMap;
        private int bAddMap;
        private int bOpacityMap;
        private int bUser1Map;
        private int bUser2Map;
        private int bUser3Map;
        private int bUser4Map;
        private int bUser5Map;
        private int bUser6Map;
        private int bUser7Map;
        private int bUser8Map;
        private int nShadowMap;
        private int bUserSampler2D1;
        private int bUserSampler2D2;
        private int bUserSampler3D1;
        private int bUserSampler3D2;
        private int bUserSamplerCube1;
        private int bUserSamplerCube2;
        private uint UserProfile;
        private uint UserProfileDrawobj;
        private int nVertexMatrixIndex;
    }
}
