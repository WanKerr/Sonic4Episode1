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
    private enum DME_TITLE_NEXT_EVT
    {
        DME_TITLE_NEXT_EVT_MAINGAME_1_1,
        DME_TITLE_NEXT_EVT_STAGESELECT,
        DME_TITLE_NEXT_EVT_OPTION,
        DME_TITLE_NEXT_EVT_RANKING,
        DME_TITLE_NEXT_EVT_LOGO,
        DME_TITLE_NEXT_EVT_TITLE,
        DME_TITLE_NEXT_EVT_MAX,
        DME_TITLE_NEXT_EVT_NONE,
    }
}
