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
    private enum GMD_MAP_FAR_OBJ_INDEX
    {
        GMD_MAP_FAR_OBJ_INDEX_ZONE_1_SEA = 0,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_2DBG = 0,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_3_ISEKI = 0,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_4_DUMMY = 0,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_FINAL_SPACE = 0,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_SS_KALEIDO = 0,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_1_SKY = 1,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_2_WHEEL = 1,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_3_MAX = 1,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_4_MAX = 1,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_FINAL_MAX = 1,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_SS_MAX = 1,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_1_ROCKA = 2,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_2_SLIGHT = 2,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_1_ROCKB = 3,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_2_MAX = 3,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_1_ROCKC = 4,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_1_2DBG = 5,
        GMD_MAP_FAR_OBJ_INDEX_ZONE_1_MAX = 6,
    }
}
