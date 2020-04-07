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

    private static void DmSoundBuild()
    {
    }

    private static bool DmSoundBuildCheck()
    {
        return true;
    }

    private static void DmSoundFlush()
    {
    }

    private static void DmSoundInit()
    {
        AppMain.GsSoundReset();
        AppMain.dm_sound_bgm_scb = AppMain.GsSoundAssignScb(0);
        AppMain.dm_sound_jingle_scb = AppMain.GsSoundAssignScb(0);
        AppMain.GsSoundBegin((ushort)4096, 1U, 3);
    }

    private static void DmSoundExit()
    {
        AppMain.GsSoundHalt();
        AppMain.GsSoundEnd();
        if (AppMain.dm_sound_jingle_scb != null)
        {
            AppMain.GsSoundStopBgm(AppMain.dm_sound_jingle_scb, 0);
            AppMain.GsSoundResignScb(AppMain.dm_sound_jingle_scb);
            AppMain.dm_sound_jingle_scb = (AppMain.GSS_SND_SCB)null;
        }
        if (AppMain.dm_sound_bgm_scb != null)
        {
            AppMain.GsSoundStopBgm(AppMain.dm_sound_bgm_scb, 0);
            AppMain.GsSoundResignScb(AppMain.dm_sound_bgm_scb);
            AppMain.dm_sound_bgm_scb = (AppMain.GSS_SND_SCB)null;
        }
        AppMain.GsSoundReset();
    }

    private static void DmSoundPlaySE(string cue_name)
    {
        AppMain.GsSoundPlaySe(cue_name);
    }

    private static void DmSoundPlayBGM(string cue_name, int fade_frame)
    {
        if (AppMain.dm_sound_bgm_scb != null)
            AppMain.GsSoundPlayBgm(AppMain.dm_sound_bgm_scb, cue_name, fade_frame);
        AppMain.dm_sound_bgm_scb.flag |= 2147483648U;
    }

    private static void DmSoundStopBGM(int fade_frame)
    {
        AppMain.DmSoundStopStageBGM(fade_frame);
    }

    private static void DmSoundPlayMenuBGM(int idx, int fade_frame)
    {
        if (AppMain.dm_sound_bgm_scb != null)
            AppMain.GsSoundPlayBgm(AppMain.dm_sound_bgm_scb, AppMain.dm_sound_bgm_name_list[idx], fade_frame);
        AppMain.dm_sound_bgm_scb.flag |= 2147483648U;
    }

    private static void DmSoundStopStageBGM(int fade_frame)
    {
        if (AppMain.dm_sound_bgm_scb == null)
            return;
        AppMain.GsSoundStopBgm(AppMain.dm_sound_bgm_scb, fade_frame);
    }

    private static void DmSoundPauseStageBGM(int fade_frame)
    {
        if (AppMain.dm_sound_bgm_scb == null)
            return;
        AppMain.GsSoundPauseBgm(AppMain.dm_sound_bgm_scb, fade_frame);
    }

    private static void DmSoundResumeStageBGM(int fade_frame)
    {
        if (AppMain.dm_sound_bgm_scb == null)
            return;
        AppMain.GsSoundResumeBgm(AppMain.dm_sound_bgm_scb, fade_frame);
    }

    private static void DmSoundPlayJingle(int jngl_idx, int fade_frame)
    {
        AppMain.GsSoundStopBgm(AppMain.dm_sound_jingle_scb);
        AppMain.GsSoundPlayBgm(AppMain.dm_sound_jingle_scb, AppMain.dm_sound_jingle_name_list[jngl_idx], fade_frame);
        AppMain.dm_sound_jingle_scb.flag |= 2147483648U;
    }

    private static void DmSoundStopJingle(int fade_frame)
    {
        AppMain.GsSoundStopBgm(AppMain.dm_sound_jingle_scb, fade_frame);
    }

    private static void DmSoundSetVolumeSE(float volume)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        float vol = (double)volume == 0.0 ? 0.0f : volume / 10f;
        mainSysInfo.se_volume = vol;
        AppMain.GsSoundSetVolume(1, vol);
    }

    private static void DmSoundSetVolumeBGM(float volume)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        float vol = (double)volume == 0.0 ? 0.0f : volume / 10f;
        mainSysInfo.bgm_volume = vol;
        AppMain.GsSoundSetVolume(0, vol);
    }

    private static bool DmSoundIsStopStageBGM()
    {
        return AppMain.GsSoundIsBgmStop(AppMain.dm_sound_bgm_scb);
    }

    private static bool DmSoundIsStopJingle()
    {
        return AppMain.GsSoundIsBgmStop(AppMain.dm_sound_jingle_scb);
    }

}