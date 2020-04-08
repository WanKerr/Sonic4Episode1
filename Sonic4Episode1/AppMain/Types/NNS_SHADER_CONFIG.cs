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
    public struct NNS_SHADER_CONFIG
    {
        private int bNormalizeVertexNormal;
        private int bRescaleVertexNormal;
        private int nMaxParallelLight;
        private int nMaxPointLight;
        private int nMaxSpotLight;
        private int bLightAmbient;
        private AppMain.NNE_ATTEN_FUNC PointLightDistAtten;
        private AppMain.NNE_ATTEN_FUNC SpotLightDistAtten;
        private AppMain.NNE_FOG_MODEL FogModel;
        private int bDistanceFog;
        private int bFragmentFog;
        private uint nUserUniform;
        private int bHalfFloat;
        private int bNoScaleEnvelope;
        private int bVertexSpecular;
        private int bCalcBinormal;
    }
}
