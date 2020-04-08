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
    public enum NNE_CIRCUM_COL
    {
        NNE_CIRCUM_COL_NONE,
        NNE_CIRCUM_COL_HIDE,
        NNE_CIRCUM_COL_CLIPHIDE,
        NNE_CIRCUM_COL_INSIDE,
        NNE_CIRCUM_COL_GSINSIDE,
        NNE_CIRCUM_COL_CROSSNEAR,
        NNE_CIRCUM_COL_ERR,
    }
}
