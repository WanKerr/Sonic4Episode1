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
    private enum GSE_EVT_ID
    {
        GSD_EVT_ID_NOP,
        GSD_EVT_ID_SYS_INIT,
        GSD_EVT_ID_LOGO_SEGA,
        GSD_EVT_ID_TITLE,
        GSD_EVT_ID_MAINMENU,
        GSD_EVT_ID_MAP,
        GSD_EVT_ID_MAINGAME,
        GSD_EVT_ID_RANKING,
        GSD_EVT_ID_OPTION,
        GSD_EVT_ID_ENDING,
        GSD_EVT_ID_STAFFROLL,
        GSD_EVT_ID_SPSTAGE_BRANCH,
        GSD_EVT_ID_BUYSCREEN,
        GSD_EVT_ID_LOGO_SONIC,
        GSD_EVT_ID_MOVIE,
        GSD_EVT_ID_NUM,
    }
}
