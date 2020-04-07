using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{

    private static void GsFontInit()
    {
    }

    private static void GsFontExit()
    {
    }

    private static void GsFontBuild()
    {
        AppMain.GsFontBuild(true);
    }

    private static void GsFontBuild(bool use_mem2)
    {
        AppMain.g_gs_font_builded = true;
    }

    private static bool GsFontIsBuilding()
    {
        return false;
    }

    private static bool GsFontIsBuilded()
    {
        return AppMain.g_gs_font_builded;
    }

    private static void GsFontRelease()
    {
        AppMain.g_gs_font_builded = false;
    }
}