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
    private enum NNE_USER_SAMPLER
    {
        NNE_USER_SAMPLER_2D_1,
        NNE_USER_SAMPLER_2D_2,
        NNE_USER_SAMPLER_3D_1,
        NNE_USER_SAMPLER_3D_2,
        NNE_USER_SAMPLER_CUBE_1,
        NNE_USER_SAMPLER_CUBE_2,
        NNE_USER_SAMPLER_MAX,
    }
}
