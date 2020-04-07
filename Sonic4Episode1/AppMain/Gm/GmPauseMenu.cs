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
    public static void GmPauseMenuLoadStart()
    {
        AppMain.CMain_PauseMenu instance = AppMain.CMain_PauseMenu.CreateInstance();
        instance.Create();
        instance.LoadFile();
    }

    public static bool GmPauseMenuLoadIsFinished()
    {
        return AppMain.CMain_PauseMenu.CreateInstance().IsLoadFile();
    }

    public static void GmPauseMenuBuildStart()
    {
        AppMain.CMain_PauseMenu.CreateInstance().CreateTexture();
    }

    public static bool GmPauseMenuBuildIsFinished()
    {
        return AppMain.CMain_PauseMenu.CreateInstance().IsCreatedTexture();
    }

    public static void GmPauseMenuFlushStart()
    {
        AppMain.CMain_PauseMenu.CreateInstance().ReleaseTexture();
    }

    public static bool GmPauseMenuFlushIsFinished()
    {
        return AppMain.CMain_PauseMenu.CreateInstance().IsReleasedTexture();
    }

    public static void GmPauseMenuRelease()
    {
        AppMain.CMain_PauseMenu.CreateInstance().Release();
    }

    public static void GmPauseMenuStart(uint prio)
    {
        AppMain.CMain_PauseMenu.CreateInstance().Start(prio);
    }

    public static void GmPauseMenuCancel()
    {
        AppMain.CMain_PauseMenu.CreateInstance().Cancel();
    }

    public static bool GmPauseMenuIsFinished()
    {
        return AppMain.CMain_PauseMenu.CreateInstance().IsPlay();
    }

    public static int GmPauseMenuGetResult()
    {
        return AppMain.CMain_PauseMenu.CreateInstance().GetResult();
    }

}