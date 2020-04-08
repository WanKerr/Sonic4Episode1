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
    private enum NNE_TEXTURETYPE_GL
    {
        NNE_TEXTURETYPE_NORMAL,
        NNE_TEXTURETYPE_BASE,
        NNE_TEXTURETYPE_DECAL,
        NNE_TEXTURETYPE_DECAL2,
        NNE_TEXTURETYPE_DECAL3,
        NNE_TEXTURETYPE_SPECULAR,
        NNE_TEXTURETYPE_SHININESS,
        NNE_TEXTURETYPE_ENVMASK,
        NNE_TEXTURETYPE_MODULATE,
        NNE_TEXTURETYPE_ADD,
        NNE_TEXTURETYPE_OPACITY,
        NNE_TEXTURETYPE_USER1,
        NNE_TEXTURETYPE_USER2,
        NNE_TEXTURETYPE_USER3,
        NNE_TEXTURETYPE_USER4,
        NNE_TEXTURETYPE_USER5,
        NNE_TEXTURETYPE_USER6,
        NNE_TEXTURETYPE_USER7,
        NNE_TEXTURETYPE_USER8,
        NNE_TEXTURETYPE_DUALPARABOLOID,
        NNE_TEXTURETYPE_MAX,
    }
}
