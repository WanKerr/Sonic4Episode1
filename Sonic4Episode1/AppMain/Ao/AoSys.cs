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
    private static void AoSysInit()
    {
        AppMain.g_ao_sys_global.is_show_ui = false;
        AppMain.g_ao_sys_global.is_signin_changed = false;
        if (!MediaPlayer.GameHasControl || MediaPlayer.State == MediaState.Playing)
        {
            AppMain.g_ao_sys_global.is_playing_device_bgm_music = true;
            List<string> stringList = new List<string>();
            stringList.Add(Sonic4ep1.Strings.ID_YES);
            stringList.Add(Sonic4ep1.Strings.ID_NO);
            string musicInterruptCapton = Sonic4ep1.Strings.ID_MUSIC_INTERRUPT_CAPTON;
            string musicInterruptText = Sonic4ep1.Strings.ID_MUSIC_INTERRUPT_TEXT;
            // show message?
        }
        else
            AppMain.g_ao_sys_global.is_playing_device_bgm_music = false;
        AppMain.aoSysInit();
    }

    private static void AoSysExit()
    {
        AppMain.aoSysExit();
    }

    private static bool AoSysIsShowPlatformUI()
    {
        return AppMain.g_ao_sys_global.is_show_ui;
    }

    private static bool AoSysIsChangeSigninState()
    {
        return AppMain.g_ao_sys_global.is_signin_changed;
    }

    private static void AoSysClearSigninState()
    {
        AppMain.g_ao_sys_global.is_signin_changed = false;
    }

    private static bool AoSysIsPlaySystemBgm()
    {
        return AppMain.g_ao_sys_global.is_playing_device_bgm_music;
    }

    private static void aoSysInit()
    {
    }

    private static void aoSysExit()
    {
    }

    private static void AoSysMsgSetBaseMsgFile(object file)
    {
    }

    private static object AoSysMsgGetBaseMsgFile()
    {
        return (object)null;
    }

    private static void AoSysMsgStart(int id, int select)
    {
    }

    private static void AoSysMsgStart(object file, uint id, int select)
    {
    }

    private static void AoSysMsgCancel()
    {
    }

    private static bool AoSysMsgIsFinished()
    {
        return true;
    }

    private static int AoSysMsgGetResult()
    {
        return 6;
    }

    private static bool AoSysMsgIsShow()
    {
        return AppMain.AoSysMsgIsShowReal();
    }

    private static bool AoSysMsgIsShowReal()
    {
        return false;
    }
}