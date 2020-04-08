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
    public enum GMD_MAIN_NEXT_EVT
    {
        GMD_MAIN_NEXT_EVT_WORLDMAP,
        GMD_MAIN_NEXT_EVT_MAINGAME,
        GMD_MAIN_NEXT_EVT_ENDING,
        GMD_MAIN_NEXT_EVT_SPSTAGE_BRA,
        GMD_MAIN_NEXT_EVT_MAINMENU,
        GMD_MAIN_NEXT_EVT_TITLE,
        GMD_MAIN_NEXT_EVT_BUYSCREEN,
        GMD_MAIN_NEXT_EVT_MAX,
    }
}
