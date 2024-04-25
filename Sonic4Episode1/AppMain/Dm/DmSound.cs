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
        GsSoundReset();
        dm_sound_bgm_scb = GsSoundAssignScb(0);
        dm_sound_jingle_scb = GsSoundAssignScb(0);
        GsSoundBegin(4096, 1U, 3);
    }

    private static void DmSoundExit()
    {
        GsSoundHalt();
        GsSoundEnd();
        if (dm_sound_jingle_scb != null)
        {
            GsSoundStopBgm(dm_sound_jingle_scb, 0);
            GsSoundResignScb(dm_sound_jingle_scb);
            dm_sound_jingle_scb = null;
        }
        if (dm_sound_bgm_scb != null)
        {
            GsSoundStopBgm(dm_sound_bgm_scb, 0);
            GsSoundResignScb(dm_sound_bgm_scb);
            dm_sound_bgm_scb = null;
        }
        GsSoundReset();
    }

    private static void DmSoundPlaySE(string cue_name)
    {
        GsSoundPlaySe(cue_name);
    }

    private static void DmSoundPlayBGM(string cue_name, int fade_frame)
    {
        if (dm_sound_bgm_scb != null)
            GsSoundPlayBgm(dm_sound_bgm_scb, cue_name, fade_frame);
        dm_sound_bgm_scb.flag |= 2147483648U;
    }

    private static void DmSoundStopBGM(int fade_frame)
    {
        DmSoundStopStageBGM(fade_frame);
    }

    private static void DmSoundPlayMenuBGM(int idx, int fade_frame)
    {
        if (dm_sound_bgm_scb != null)
            GsSoundPlayBgm(dm_sound_bgm_scb, dm_sound_bgm_name_list[idx], fade_frame);
        dm_sound_bgm_scb.flag |= 2147483648U;
    }

    private static void DmSoundStopStageBGM(int fade_frame)
    {
        if (dm_sound_bgm_scb == null)
            return;
        GsSoundStopBgm(dm_sound_bgm_scb, fade_frame);
    }

    private static void DmSoundPauseStageBGM(int fade_frame)
    {
        if (dm_sound_bgm_scb == null)
            return;
        GsSoundPauseBgm(dm_sound_bgm_scb, fade_frame);
    }

    private static void DmSoundResumeStageBGM(int fade_frame)
    {
        if (dm_sound_bgm_scb == null)
            return;
        GsSoundResumeBgm(dm_sound_bgm_scb, fade_frame);
    }

    private static void DmSoundPlayJingle(int jngl_idx, int fade_frame)
    {
        GsSoundStopBgm(dm_sound_jingle_scb);
        GsSoundPlayBgm(dm_sound_jingle_scb, dm_sound_jingle_name_list[jngl_idx], fade_frame);
        dm_sound_jingle_scb.flag |= 2147483648U;
    }

    private static void DmSoundStopJingle(int fade_frame)
    {
        GsSoundStopBgm(dm_sound_jingle_scb, fade_frame);
    }

    private static void DmSoundSetVolumeSE(float volume)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        float vol = volume == 0.0 ? 0.0f : volume / 10f;
        mainSysInfo.se_volume = vol;
        GsSoundSetVolume(1, vol);
    }

    private static void DmSoundSetVolumeBGM(float volume)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        float vol = volume == 0.0 ? 0.0f : volume / 10f;
        mainSysInfo.bgm_volume = vol;
        GsSoundSetVolume(0, vol);
    }

    private static bool DmSoundIsStopStageBGM()
    {
        return GsSoundIsBgmStop(dm_sound_bgm_scb);
    }

    private static bool DmSoundIsStopJingle()
    {
        return GsSoundIsBgmStop(dm_sound_jingle_scb);
    }

}