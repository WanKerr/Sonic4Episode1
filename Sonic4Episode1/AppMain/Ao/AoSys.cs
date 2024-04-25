using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

public partial class AppMain
{
    private static void AoSysInit()
    {
        g_ao_sys_global.is_show_ui = false;
        g_ao_sys_global.is_signin_changed = false;
        if (!MediaPlayer.GameHasControl || MediaPlayer.State == MediaState.Playing)
        {
            g_ao_sys_global.is_playing_device_bgm_music = true;
            List<string> stringList = new List<string>();
            stringList.Add(Sonic4ep1.Strings.ID_YES);
            stringList.Add(Sonic4ep1.Strings.ID_NO);
            string musicInterruptCapton = Sonic4ep1.Strings.ID_MUSIC_INTERRUPT_CAPTON;
            string musicInterruptText = Sonic4ep1.Strings.ID_MUSIC_INTERRUPT_TEXT;
            // show message?
        }
        else
            g_ao_sys_global.is_playing_device_bgm_music = false;
        aoSysInit();
    }

    private static void AoSysExit()
    {
        aoSysExit();
    }

    private static bool AoSysIsShowPlatformUI()
    {
        return g_ao_sys_global.is_show_ui;
    }

    private static bool AoSysIsChangeSigninState()
    {
        return g_ao_sys_global.is_signin_changed;
    }

    private static void AoSysClearSigninState()
    {
        g_ao_sys_global.is_signin_changed = false;
    }

    private static bool AoSysIsPlaySystemBgm()
    {
        return g_ao_sys_global.is_playing_device_bgm_music;
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
        return null;
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
        return AoSysMsgIsShowReal();
    }

    private static bool AoSysMsgIsShowReal()
    {
        return false;
    }
}