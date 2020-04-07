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
    private static void AoAccountInit()
    {
        AppMain.g_ao_account_current_id = -1;
    }

    private static void AoAccountDebugInit()
    {
    }

    private static void AoAccountExit()
    {
    }

    private static void AoAccountClearCurrentId()
    {
        AppMain.g_ao_account_current_id = -1;
    }

    private static void AoAccountSetCurrentIdStart(uint id)
    {
        AppMain.g_ao_account_current_id = (int)id;
    }

    private static bool AoAccountSetCurrentIdIsFinished()
    {
        return true;
    }

    public static int AoAccountGetCurrentId()
    {
        return AppMain.g_ao_account_current_id;
    }

    private static bool AoAccountIsCurrentSignin()
    {
        return AppMain.AoAccountGetCurrentId() >= 0;
    }

    private static bool AoAccountIsCurrentOnline()
    {
        return XBOXLive.signinStatus == XBOXLive.SigninStatus.LIVE;
    }

    public static bool AoAccountIsCurrentEnable()
    {
        return (uint)AppMain.g_ao_account_current_id < 4U;
    }


}