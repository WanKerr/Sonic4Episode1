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
    private static void GmSoundBuild()
    {
    }

    private static bool GmSoundBuildCheck()
    {
        return true;
    }

    private static void GmSoundFlush()
    {
    }

    private static void GmSoundInit()
    {
        AppMain.GsSoundReset();
        AppMain.gm_sound_bgm_scb = AppMain.GsSoundAssignScb(0);
        AppMain.gm_sound_bgm_sub_scb = AppMain.GsSoundAssignScb(0);
        AppMain.gm_sound_jingle_scb = AppMain.GsSoundAssignScb(0);
        AppMain.gm_sound_jingle_bgm_scb = AppMain.GsSoundAssignScb(0);
        AppMain.GsSoundBegin((ushort)3, (uint)short.MaxValue, 5);
        AppMain.gm_sound_flag = 0U;
        AppMain.gm_sound_1shot_tcb = (AppMain.MTS_TASK_TCB)null;
        AppMain.gm_sound_bgm_win_boss_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void GmSoundExit()
    {
        if (AppMain.gm_sound_1shot_tcb != null)
            AppMain.mtTaskClearTcb(AppMain.gm_sound_1shot_tcb);
        if (AppMain.gm_sound_bgm_fade_tcb != null)
            AppMain.mtTaskClearTcb(AppMain.gm_sound_bgm_fade_tcb);
        if (AppMain.gm_sound_bgm_win_boss_tcb != null)
            AppMain.mtTaskClearTcb(AppMain.gm_sound_bgm_win_boss_tcb);
        AppMain.GsSoundHalt();
        AppMain.GsSoundEnd();
        if (AppMain.gm_sound_jingle_scb != null)
        {
            AppMain.GsSoundStopBgm(AppMain.gm_sound_jingle_scb, 0);
            AppMain.GsSoundResignScb(AppMain.gm_sound_jingle_scb);
            AppMain.gm_sound_jingle_scb = (AppMain.GSS_SND_SCB)null;
        }
        if (AppMain.gm_sound_bgm_scb != null)
        {
            AppMain.GsSoundStopBgm(AppMain.gm_sound_bgm_scb, 0);
            AppMain.GsSoundResignScb(AppMain.gm_sound_bgm_scb);
            AppMain.gm_sound_bgm_scb = (AppMain.GSS_SND_SCB)null;
        }
        if (AppMain.gm_sound_bgm_sub_scb != null)
        {
            AppMain.GsSoundStopBgm(AppMain.gm_sound_bgm_sub_scb, 0);
            AppMain.GsSoundResignScb(AppMain.gm_sound_bgm_sub_scb);
            AppMain.gm_sound_bgm_sub_scb = (AppMain.GSS_SND_SCB)null;
        }
        if (AppMain.gm_sound_jingle_bgm_scb == null)
            return;
        AppMain.GsSoundStopBgm(AppMain.gm_sound_jingle_bgm_scb, 0);
        AppMain.GsSoundResignScb(AppMain.gm_sound_jingle_bgm_scb);
        AppMain.gm_sound_jingle_bgm_scb = (AppMain.GSS_SND_SCB)null;
    }

    public static void GmSoundPlaySE(string cue_name)
    {
        AppMain.GmSoundPlaySE(cue_name, (AppMain.GSS_SND_SE_HANDLE)null);
    }

    private static void GmSoundPlaySE(string cue_name, AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.GsSoundPlaySe(cue_name, se_handle);
    }

    private static void GmSoundPlaySEForce(string cue_name, AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.GsSoundPlaySeForce(cue_name, se_handle, 0, false);
    }

    private static void GmSoundPlaySEForce(
      string cue_name,
      AppMain.GSS_SND_SE_HANDLE se_handle,
      bool dontplay)
    {
        AppMain.GsSoundPlaySeForce(cue_name, se_handle, 0, dontplay);
    }

    private static void GmSoundPlayBGM(string cue_name, int fade_frame)
    {
        AppMain.GmSoundPlayStageBGM(fade_frame);
    }

    private static void GmSoundPlayStageBGM(int fade_frame)
    {
        AppMain.GsSoundScbSetVolume(AppMain.gm_sound_bgm_scb, 1f);
        AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_bgm_scb, false);
        AppMain.GsSoundPlayBgm(AppMain.gm_sound_bgm_scb, AppMain.gm_sound_bgm_name_list[(int)AppMain.g_gs_main_sys_info.stage_id], fade_frame);
        AppMain.gm_sound_bgm_scb.flag |= 2147483648U;
    }

    private static void GmSoundStopBGM(int fade_frame)
    {
        AppMain.GmSoundStopStageBGM(fade_frame);
    }

    private static void GmSoundStopStageBGM(int fade_frame)
    {
        AppMain.GsSoundStopBgm(AppMain.gm_sound_bgm_scb, fade_frame);
    }

    private static void GmSoundPauseStageBGM(int fade_frame)
    {
        AppMain.GsSoundPauseBgm(AppMain.gm_sound_bgm_scb, fade_frame);
    }

    private static void GmSoundResumeStageBGM(int fade_frame)
    {
        AppMain.GsSoundResumeBgm(AppMain.gm_sound_bgm_scb, fade_frame);
    }

    private static void GmSoundChangeSpeedupBGM()
    {
        bool flag1 = false;
        bool flag2 = false;
        AppMain.GsSoundStopBgm(AppMain.gm_sound_bgm_sub_scb, 0);
        if (AppMain.GsSoundIsBgmPause(AppMain.gm_sound_bgm_scb))
            flag1 = true;
        if (((int)AppMain.gm_sound_flag & 80) != 0)
            flag2 = true;
        if (flag1 | flag2)
            AppMain.GmSoundStopStageBGM(0);
        else
            AppMain.GmSoundStopStageBGM(0);
        if (flag1)
            AppMain.GmSoundPauseStageBGM(0);
        AppMain.GSS_SND_SCB gmSoundBgmScb = AppMain.gm_sound_bgm_scb;
        AppMain.gm_sound_bgm_scb = AppMain.gm_sound_bgm_sub_scb;
        AppMain.gm_sound_bgm_sub_scb = gmSoundBgmScb;
        AppMain.GsSoundScbSetVolume(AppMain.gm_sound_bgm_scb, 1f);
        AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_bgm_scb, false);
        AppMain.GsSoundPlayBgm(AppMain.gm_sound_bgm_scb, AppMain.gm_sound_speedup_bgm_name_list[(int)AppMain.g_gs_main_sys_info.stage_id], 0);
        AppMain.gm_sound_bgm_scb.flag |= 2147483648U;
        if (!flag2)
            return;
        AppMain.gmSoundSetBGMFadeEnd(AppMain.gm_sound_bgm_scb);
        AppMain.GsSoundScbSetVolume(AppMain.gm_sound_bgm_scb, 0.0f);
        AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_bgm_scb, true);
    }

    private static void GmSoundChangeAngryBossBGM()
    {
        bool flag1 = false;
        bool flag2 = false;
        AppMain.GsSoundStopBgm(AppMain.gm_sound_bgm_sub_scb, 0);
        if (AppMain.GsSoundIsBgmPause(AppMain.gm_sound_bgm_scb))
            flag1 = true;
        if (((int)AppMain.gm_sound_flag & 80) != 0)
            flag2 = true;
        if (flag1 | flag2)
            AppMain.GmSoundStopStageBGM(0);
        else
            AppMain.GmSoundStopStageBGM(15);
        AppMain.GSS_SND_SCB gmSoundBgmScb = AppMain.gm_sound_bgm_scb;
        AppMain.gm_sound_bgm_scb = AppMain.gm_sound_bgm_sub_scb;
        AppMain.gm_sound_bgm_sub_scb = gmSoundBgmScb;
        AppMain.GsSoundScbSetVolume(AppMain.gm_sound_bgm_scb, 1f);
        AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_bgm_scb, false);
        AppMain.GsSoundPlayBgm(AppMain.gm_sound_bgm_scb, "snd_sng_boss2", 15);
        AppMain.gm_sound_bgm_scb.flag |= 2147483648U;
        if (flag1)
            AppMain.GmSoundPauseStageBGM(0);
        if (!flag2)
            return;
        AppMain.gmSoundSetBGMFadeEnd(AppMain.gm_sound_bgm_scb);
        AppMain.GsSoundScbSetVolume(AppMain.gm_sound_bgm_scb, 0.0f);
        AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_bgm_scb, true);
    }

    private static void GmSoundChangeWinBossBGM()
    {
        if (AppMain.g_gs_main_sys_info.stage_id >= (ushort)16 || AppMain.gm_sound_bgm_win_boss_tcb != null)
            return;
        AppMain.gm_sound_bgm_win_boss_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSoundBGMWinBossFunc), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSoundBGMWinBossDest), 0U, (ushort)0, (uint)short.MaxValue, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_SOUND_BGM_WIN_BOSS_MGR_WORK()), "GM_SOUND_WB");
        AppMain.GMS_SOUND_BGM_WIN_BOSS_MGR_WORK work = (AppMain.GMS_SOUND_BGM_WIN_BOSS_MGR_WORK)AppMain.gm_sound_bgm_win_boss_tcb.work;
        work.Clear();
        work.timer = AppMain.gm_sound_bgm_win_boss_wait_frame_list[AppMain.GMM_MAIN_GET_ZONE_TYPE()];
    }

    private static void GmSoundChangeFinalBossBGM()
    {
        bool flag1 = false;
        bool flag2 = false;
        AppMain.GsSoundStopBgm(AppMain.gm_sound_bgm_sub_scb, 0);
        if (AppMain.GsSoundIsBgmPause(AppMain.gm_sound_bgm_scb))
            flag1 = true;
        if (((int)AppMain.gm_sound_flag & 80) != 0)
            flag2 = true;
        if (flag1 | flag2)
            AppMain.GmSoundStopStageBGM(0);
        else
            AppMain.GmSoundStopStageBGM(15);
        AppMain.GSS_SND_SCB gmSoundBgmScb = AppMain.gm_sound_bgm_scb;
        AppMain.gm_sound_bgm_scb = AppMain.gm_sound_bgm_sub_scb;
        AppMain.gm_sound_bgm_sub_scb = gmSoundBgmScb;
        AppMain.GsSoundScbSetVolume(AppMain.gm_sound_bgm_scb, 1f);
        AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_bgm_scb, false);
        AppMain.GsSoundPlayBgm(AppMain.gm_sound_bgm_scb, "snd_sng_final", 15);
        AppMain.gm_sound_bgm_scb.flag |= 2147483648U;
        if (flag1)
            AppMain.GmSoundPauseStageBGM(0);
        if (!flag2)
            return;
        AppMain.gmSoundSetBGMFadeEnd(AppMain.gm_sound_bgm_scb);
        AppMain.GsSoundScbSetVolume(AppMain.gm_sound_bgm_scb, 0.0f);
        AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_bgm_scb, true);
    }

    public static void GmSoundPlayJingle(uint jngl_idx)
    {
        AppMain.GmSoundPlayJingle(jngl_idx, 0);
    }

    public static void GmSoundPlayJingle(uint jngl_idx, int fade_frame)
    {
        AppMain.GsSoundScbSetVolume(AppMain.gm_sound_jingle_scb, 1f);
        AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_jingle_scb, false);
        AppMain.GsSoundStopBgm(AppMain.gm_sound_jingle_scb);
        AppMain.GsSoundPlayBgm(AppMain.gm_sound_jingle_scb, AppMain.gm_sound_jingle_name_list[(int)jngl_idx], fade_frame);
        AppMain.gm_sound_jingle_scb.flag |= 2147483648U;
    }

    private static void GmSoundStopJingle(int fade_frame)
    {
        AppMain.GsSoundStopBgm(AppMain.gm_sound_jingle_scb, fade_frame);
    }

    private static void GmSoundPlayBGMJingle(uint jngl_idx, int fade_frame)
    {
        AppMain.GsSoundScbSetVolume(AppMain.gm_sound_jingle_bgm_scb, 1f);
        AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_jingle_bgm_scb, false);
        AppMain.GsSoundStopBgm(AppMain.gm_sound_jingle_bgm_scb);
        AppMain.GsSoundPlayBgm(AppMain.gm_sound_jingle_bgm_scb, AppMain.gm_sound_jingle_name_list[(int)jngl_idx], fade_frame);
        AppMain.gm_sound_jingle_bgm_scb.flag |= 2147483648U;
    }

    private static void GmSoundStopBGMJingle(int fade_frame)
    {
        AppMain.GsSoundStopBgm(AppMain.gm_sound_jingle_bgm_scb, fade_frame);
    }

    private static void GmSoundPauseBGMJingle(int fade_frame)
    {
        AppMain.GsSoundPauseBgm(AppMain.gm_sound_jingle_bgm_scb, fade_frame);
    }

    private static void GmSoundResumeBGMJingle(int fade_frame)
    {
        AppMain.GsSoundResumeBgm(AppMain.gm_sound_jingle_bgm_scb, fade_frame);
    }

    private static void GmSoundSetVolumeSE(float volume)
    {
        AppMain.GsSoundSetVolume(1, volume);
    }

    private static void GmSoundSetVolumeBGM(float volume)
    {
        AppMain.GsSoundSetVolume(0, volume);
    }

    private static void GmSoundPlayJingleObore()
    {
        if (!AppMain.GsSoundIsBgmStop(AppMain.gm_sound_jingle_bgm_scb) || ((int)AppMain.gm_sound_flag & 1) != 0)
            return;
        if (!AppMain.GsSoundIsBgmPause(AppMain.gm_sound_bgm_scb))
            AppMain.GmSoundPauseStageBGM(0);
        AppMain.GmSoundPlayBGMJingle(6U, 0);
        if (((int)AppMain.gm_sound_flag & 32) != 0)
        {
            AppMain.gmSoundSetBGMFadeEnd(AppMain.gm_sound_jingle_bgm_scb);
            AppMain.GsSoundScbSetVolume(AppMain.gm_sound_jingle_bgm_scb, 0.0f);
            AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_jingle_bgm_scb, true);
        }
        AppMain.gm_sound_flag |= 1U;
    }

    private static void GmSoundStopJingleObore()
    {
        if (((int)AppMain.gm_sound_flag & 1) == 0)
            return;
        if (!AppMain.GsSoundIsBgmPause(AppMain.gm_sound_jingle_bgm_scb) && ((int)AppMain.gm_sound_flag & 32) == 0)
            AppMain.GmSoundStopBGMJingle(15);
        else
            AppMain.GmSoundStopBGMJingle(0);
        if (((int)AppMain.gm_sound_flag & int.MinValue) == 0)
            AppMain.GmSoundResumeStageBGM(0);
        AppMain.gm_sound_flag &= 4294967294U;
    }

    private static void GmSoundPlayJingleInvincible()
    {
        if (((int)AppMain.gm_sound_flag & 4) != 0)
            return;
        if (!AppMain.GsSoundIsBgmPause(AppMain.gm_sound_bgm_scb))
            AppMain.GmSoundPauseStageBGM(0);
        AppMain.GmSoundPlayBGMJingle(4U, 0);
        if (((int)AppMain.gm_sound_flag & 32) != 0)
        {
            AppMain.gmSoundSetBGMFadeEnd(AppMain.gm_sound_jingle_bgm_scb);
            AppMain.GsSoundScbSetVolume(AppMain.gm_sound_jingle_bgm_scb, 0.0f);
            AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_jingle_bgm_scb, true);
        }
        AppMain.gm_sound_flag |= 4U;
        AppMain.gm_sound_flag &= 4294967294U;
    }

    private static void GmSoundStopJingleInvincible()
    {
        if (((int)AppMain.gm_sound_flag & 4) == 0)
            return;
        if (!AppMain.GsSoundIsBgmPause(AppMain.gm_sound_jingle_bgm_scb) && ((int)AppMain.gm_sound_flag & 32) == 0)
            AppMain.GmSoundStopBGMJingle(0);
        else
            AppMain.GmSoundStopBGMJingle(0);
        if (((int)AppMain.gm_sound_flag & int.MinValue) == 0)
            AppMain.GmSoundResumeStageBGM(0);
        AppMain.gm_sound_flag &= 4294967291U;
    }

    private static bool GmBGMIsAlreadyPlaying()
    {
        AppMain.AMS_CRIAUDIO_INTERFACE global = AppMain.amCriAudioGetGlobal();
        return global.auply[AppMain.gm_sound_bgm_scb.auply_no] != null && global.auply[AppMain.gm_sound_bgm_scb.auply_no].se_name == AppMain.gm_sound_bgm_name_list[(int)AppMain.g_gs_main_sys_info.stage_id] && !global.auply[AppMain.gm_sound_bgm_scb.auply_no].IsPaused();
    }

    private static void GmSoundPlayJingle1UP(bool ret_last_sound)
    {
        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().ModernSoundEffects;
        if (betterSfx)
        {
            AppMain.GmSoundPlaySE("1Up");
        }
        else
        {
            if (ret_last_sound)
                AppMain.gmSoundPlay1ShotJingle(0U, 0, 0, 0);
            else
                AppMain.GmSoundPlayJingle(0U, 0);
        }
    }

    private static void GmSoundPlayGameOver()
    {
        AppMain.GmSoundStopStageBGM(15);
        AppMain.GmSoundStopBGMJingle(15);
        if (AppMain.gm_sound_1shot_tcb != null)
            AppMain.mtTaskClearTcb(AppMain.gm_sound_1shot_tcb);
        AppMain.GmSoundPlayJingle(7U, 0);
    }

    private static void GmSoundPlayClear()
    {
        AppMain.GmSoundStopStageBGM(15);
        AppMain.GmSoundStopBGMJingle(15);
        if (AppMain.gm_sound_1shot_tcb != null)
            AppMain.mtTaskClearTcb(AppMain.gm_sound_1shot_tcb);
        AppMain.GmSoundPlayJingle(1U, 0);
    }

    private static void GmSoundPlayClearFinal()
    {
        AppMain.GmSoundStopStageBGM(15);
        AppMain.GmSoundStopBGMJingle(15);
        if (AppMain.gm_sound_1shot_tcb != null)
            AppMain.mtTaskClearTcb(AppMain.gm_sound_1shot_tcb);
        AppMain.GmSoundPlayJingle(2U, 0);
    }

    private static void GmSoundAllPause()
    {
        AppMain.gm_sound_flag &= 4043309055U;
        if (!AppMain.GsSoundIsBgmStop(AppMain.gm_sound_jingle_scb) && !AppMain.GsSoundIsBgmPause(AppMain.gm_sound_jingle_scb))
        {
            AppMain.GsSoundPauseBgm(AppMain.gm_sound_jingle_scb, 0);
            AppMain.gm_sound_flag |= 67108864U;
        }
        if (!AppMain.GsSoundIsBgmStop(AppMain.gm_sound_jingle_bgm_scb) && !AppMain.GsSoundIsBgmPause(AppMain.gm_sound_jingle_bgm_scb))
        {
            AppMain.GsSoundPauseBgm(AppMain.gm_sound_jingle_bgm_scb, 0);
            AppMain.gm_sound_flag |= 33554432U;
        }
        if (!AppMain.GsSoundIsBgmStop(AppMain.gm_sound_bgm_scb) && !AppMain.GsSoundIsBgmPause(AppMain.gm_sound_bgm_scb))
        {
            AppMain.GsSoundPauseBgm(AppMain.gm_sound_bgm_scb, 0);
            AppMain.gm_sound_flag |= 16777216U;
        }
        AppMain.GsSoundStopBgm(AppMain.gm_sound_bgm_sub_scb, 0);
        AppMain.GsSoundPauseSe(128U);
        AppMain.gm_sound_flag |= 134217728U;
    }

    private static void GmSoundAllResume()
    {
        if (((int)AppMain.gm_sound_flag & 67108864) != 0)
            AppMain.GsSoundResumeBgm(AppMain.gm_sound_jingle_scb, 0);
        else if (((int)AppMain.gm_sound_flag & 33554432) != 0)
            AppMain.GsSoundResumeBgm(AppMain.gm_sound_jingle_bgm_scb, 0);
        else if (((int)AppMain.gm_sound_flag & 16777216) != 0)
            AppMain.GsSoundResumeBgm(AppMain.gm_sound_bgm_scb, 0);
        AppMain.GsSoundResumeSe(128U);
        AppMain.gm_sound_flag &= 4043309055U;
    }

    private static void gmSoundPlay1ShotJingle(
      uint jngl_idx,
      int jingle_fade_in_frame,
      int bgm_fade_out_frame,
      int bgm_fade_in_frame)
    {
        AppMain.gm_sound_flag |= 2147483648U;
        if (!AppMain.GsSoundIsBgmStop(AppMain.gm_sound_bgm_scb) && !AppMain.GsSoundIsBgmPause(AppMain.gm_sound_bgm_scb))
            AppMain.GmSoundPauseStageBGM(bgm_fade_out_frame);
        if (!AppMain.GsSoundIsBgmStop(AppMain.gm_sound_jingle_bgm_scb) && !AppMain.GsSoundIsBgmPause(AppMain.gm_sound_jingle_bgm_scb))
            AppMain.GmSoundPauseBGMJingle(bgm_fade_out_frame);
        if (AppMain.gm_sound_1shot_tcb != null)
            AppMain.GmSoundStopJingle(0);
        else
            AppMain.gm_sound_1shot_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSound1ShotJingleFunc), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSound1ShotJingleDest), 0U, (ushort)0, (uint)short.MaxValue, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_SOUND_1SHOT_JINGLE_WORK()), "GM_SOUND_1SH");
        AppMain.GMS_SOUND_1SHOT_JINGLE_WORK work = (AppMain.GMS_SOUND_1SHOT_JINGLE_WORK)AppMain.gm_sound_1shot_tcb.work;
        work.Clear();
        AppMain.GmSoundPlayJingle(jngl_idx, jingle_fade_in_frame);
        work.bgm_fade_in_frame = bgm_fade_in_frame;
    }

    private static void gmSound1ShotJingleFunc(AppMain.MTS_TASK_TCB tcb)
    {
        if (((int)AppMain.gm_sound_flag & 134217728) != 0)
            return;
        AppMain.GMS_SOUND_1SHOT_JINGLE_WORK work = (AppMain.GMS_SOUND_1SHOT_JINGLE_WORK)AppMain.gm_sound_1shot_tcb.work;
        if (!AppMain.GsSoundIsBgmStop(AppMain.gm_sound_jingle_scb))
            return;
        AppMain.GmSoundStopJingle(0);
        AppMain.gm_sound_flag &= (uint)int.MaxValue;
        if (!AppMain.GsSoundIsBgmStop(AppMain.gm_sound_jingle_bgm_scb) && AppMain.GsSoundIsBgmPause(AppMain.gm_sound_jingle_bgm_scb))
            AppMain.GmSoundResumeBGMJingle(work.bgm_fade_in_frame);
        else if (!AppMain.GsSoundIsBgmStop(AppMain.gm_sound_bgm_scb) && AppMain.GsSoundIsBgmPause(AppMain.gm_sound_bgm_scb))
            AppMain.GmSoundResumeStageBGM(work.bgm_fade_in_frame);
        AppMain.mtTaskClearTcb(tcb);
    }

    private static void gmSound1ShotJingleDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gm_sound_1shot_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmSoundSetBGMFade(
      AppMain.GSS_SND_SCB snd_scb,
      float start_vol,
      float end_vol,
      int frame)
    {
        if (AppMain.GsSoundIsBgmStop(AppMain.gm_sound_bgm_scb) || AppMain.GsSoundIsBgmPause(AppMain.gm_sound_bgm_scb))
            return;
        AppMain.gmSoundSetBGMFadeEnd(snd_scb);
        if (frame <= 0)
            frame = 1;
        if (AppMain.gm_sound_bgm_fade_tcb == null)
        {
            AppMain.gm_sound_bgm_fade_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSoundBGMFadeFunc), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSoundBGMFadeDest), 0U, (ushort)0, (uint)short.MaxValue, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_SOUND_BGM_FADE_MGR_WORK()), "GM_SOUND_BFADE");
            ((AppMain.GMS_SOUND_BGM_FADE_MGR_WORK)AppMain.gm_sound_bgm_fade_tcb.work).Clear();
        }
        AppMain.gmSoundBGMFadeAttachList((AppMain.GMS_SOUND_BGM_FADE_MGR_WORK)AppMain.gm_sound_bgm_fade_tcb.work, new AppMain.GMS_SOUND_BGM_FADE_WORK()
        {
            snd_scb = snd_scb,
            start_vol = start_vol,
            end_vol = end_vol,
            frame = frame,
            fade_spd = (end_vol - start_vol) / (float)frame,
            now_vol = start_vol
        });
    }

    private static void gmSoundSetBGMFadeEnd(AppMain.GSS_SND_SCB snd_scb)
    {
        if (AppMain.gm_sound_bgm_fade_tcb == null)
            return;
        AppMain.GMS_SOUND_BGM_FADE_MGR_WORK work = (AppMain.GMS_SOUND_BGM_FADE_MGR_WORK)AppMain.gm_sound_bgm_fade_tcb.work;
        AppMain.GMS_SOUND_BGM_FADE_WORK next;
        for (AppMain.GMS_SOUND_BGM_FADE_WORK fade_work = work.head; fade_work != null; fade_work = next)
        {
            next = fade_work.next;
            if (fade_work.snd_scb == snd_scb)
                AppMain.gmSoundBGMFadeDetachList(work, fade_work);
        }
        if (work.num > 0)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_sound_bgm_fade_tcb);
    }

    private static void gmSoundBGMFadeFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_SOUND_BGM_FADE_MGR_WORK work = (AppMain.GMS_SOUND_BGM_FADE_MGR_WORK)tcb.work;
        AppMain.GMS_SOUND_BGM_FADE_WORK next;
        for (AppMain.GMS_SOUND_BGM_FADE_WORK fade_work = work.head; fade_work != null; fade_work = next)
        {
            next = fade_work.next;
            fade_work.now_vol += fade_work.fade_spd;
            --fade_work.frame;
            if (fade_work.frame <= 0)
                fade_work.now_vol = fade_work.end_vol;
            AppMain.GsSoundScbSetVolume(fade_work.snd_scb, fade_work.now_vol);
            if (fade_work.frame <= 0 || AppMain.GsSoundIsBgmStop(fade_work.snd_scb))
            {
                if ((double)fade_work.now_vol > 0.0)
                    AppMain.GsSoundScbSetSeqMute(fade_work.snd_scb, false);
                else
                    AppMain.GsSoundScbSetSeqMute(fade_work.snd_scb, true);
                AppMain.gmSoundBGMFadeDetachList(work, fade_work);
            }
        }
        if (work.num > 0)
            return;
        AppMain.mtTaskClearTcb(tcb);
    }

    private static void gmSoundBGMFadeDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_SOUND_BGM_FADE_MGR_WORK work = (AppMain.GMS_SOUND_BGM_FADE_MGR_WORK)tcb.work;
        AppMain.GMS_SOUND_BGM_FADE_WORK next;
        for (AppMain.GMS_SOUND_BGM_FADE_WORK fade_work = work.head; fade_work != null; fade_work = next)
        {
            next = fade_work.next;
            AppMain.gmSoundBGMFadeDetachList(work, fade_work);
        }
        if (AppMain.gm_sound_bgm_fade_tcb != tcb)
            return;
        AppMain.gm_sound_bgm_fade_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmSoundBGMFadeAttachList(
      AppMain.GMS_SOUND_BGM_FADE_MGR_WORK mgr_work,
      AppMain.GMS_SOUND_BGM_FADE_WORK fade_work)
    {
        if (mgr_work.tail != null)
        {
            fade_work.prev = mgr_work.tail;
            mgr_work.tail.next = fade_work;
            mgr_work.tail = fade_work;
        }
        else
        {
            mgr_work.head = fade_work;
            mgr_work.tail = fade_work;
        }
        ++mgr_work.num;
    }

    private static void gmSoundBGMFadeDetachList(
      AppMain.GMS_SOUND_BGM_FADE_MGR_WORK mgr_work,
      AppMain.GMS_SOUND_BGM_FADE_WORK fade_work)
    {
        if (fade_work.prev != null)
            fade_work.prev.next = fade_work.next;
        else
            mgr_work.head = fade_work.next;
        if (fade_work.next != null)
            fade_work.next.prev = fade_work.prev;
        else
            mgr_work.tail = fade_work.prev;
        --mgr_work.num;
    }

    private static void gmSoundBGMWinBossFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_SOUND_BGM_WIN_BOSS_MGR_WORK work = (AppMain.GMS_SOUND_BGM_WIN_BOSS_MGR_WORK)tcb.work;
        if (((int)AppMain.gm_sound_flag & 134217728) != 0)
            return;
        --work.timer;
        if (work.timer > 0)
            return;
        bool flag1 = false;
        bool flag2 = false;
        AppMain.GsSoundStopBgm(AppMain.gm_sound_bgm_sub_scb, 0);
        if (AppMain.GsSoundIsBgmPause(AppMain.gm_sound_bgm_scb))
            flag1 = true;
        if (((int)AppMain.gm_sound_flag & 80) != 0)
            flag2 = true;
        if (flag1 | flag2)
            AppMain.GmSoundStopStageBGM(0);
        else
            AppMain.GmSoundStopStageBGM(30);
        AppMain.GSS_SND_SCB gmSoundBgmScb = AppMain.gm_sound_bgm_scb;
        AppMain.gm_sound_bgm_scb = AppMain.gm_sound_bgm_sub_scb;
        AppMain.gm_sound_bgm_sub_scb = gmSoundBgmScb;
        AppMain.GsSoundScbSetVolume(AppMain.gm_sound_bgm_scb, 1f);
        AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_bgm_scb, false);
        AppMain.GsSoundPlayBgm(AppMain.gm_sound_bgm_scb, AppMain.gm_sound_bgm_win_boss_name_list[AppMain.GMM_MAIN_GET_ZONE_TYPE()], 30);
        AppMain.gm_sound_bgm_scb.flag |= 2147483648U;
        if (flag1)
            AppMain.GmSoundPauseStageBGM(0);
        if (flag2)
        {
            AppMain.gmSoundSetBGMFadeEnd(AppMain.gm_sound_bgm_scb);
            AppMain.GsSoundScbSetVolume(AppMain.gm_sound_bgm_scb, 0.0f);
            AppMain.GsSoundScbSetSeqMute(AppMain.gm_sound_bgm_scb, true);
        }
        AppMain.mtTaskClearTcb(tcb);
    }

    private static void gmSoundBGMWinBossDest(AppMain.MTS_TASK_TCB tcb)
    {
        if (tcb != AppMain.gm_sound_bgm_win_boss_tcb)
            return;
        AppMain.gm_sound_bgm_win_boss_tcb = (AppMain.MTS_TASK_TCB)null;
    }
}