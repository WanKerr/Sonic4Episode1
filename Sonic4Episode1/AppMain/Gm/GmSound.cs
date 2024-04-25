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
        GsSoundReset();
        gm_sound_bgm_scb = GsSoundAssignScb(0);
        gm_sound_bgm_sub_scb = GsSoundAssignScb(0);
        gm_sound_jingle_scb = GsSoundAssignScb(0);
        gm_sound_jingle_bgm_scb = GsSoundAssignScb(0);
        GsSoundBegin(3, (uint)short.MaxValue, 5);
        gm_sound_flag = 0U;
        gm_sound_1shot_tcb = null;
        gm_sound_bgm_win_boss_tcb = null;
    }

    private static void GmSoundExit()
    {
        if (gm_sound_1shot_tcb != null)
            mtTaskClearTcb(gm_sound_1shot_tcb);
        if (gm_sound_bgm_fade_tcb != null)
            mtTaskClearTcb(gm_sound_bgm_fade_tcb);
        if (gm_sound_bgm_win_boss_tcb != null)
            mtTaskClearTcb(gm_sound_bgm_win_boss_tcb);
        GsSoundHalt();
        GsSoundEnd();
        if (gm_sound_jingle_scb != null)
        {
            GsSoundStopBgm(gm_sound_jingle_scb, 0);
            GsSoundResignScb(gm_sound_jingle_scb);
            gm_sound_jingle_scb = null;
        }
        if (gm_sound_bgm_scb != null)
        {
            GsSoundStopBgm(gm_sound_bgm_scb, 0);
            GsSoundResignScb(gm_sound_bgm_scb);
            gm_sound_bgm_scb = null;
        }
        if (gm_sound_bgm_sub_scb != null)
        {
            GsSoundStopBgm(gm_sound_bgm_sub_scb, 0);
            GsSoundResignScb(gm_sound_bgm_sub_scb);
            gm_sound_bgm_sub_scb = null;
        }
        if (gm_sound_jingle_bgm_scb == null)
            return;
        GsSoundStopBgm(gm_sound_jingle_bgm_scb, 0);
        GsSoundResignScb(gm_sound_jingle_bgm_scb);
        gm_sound_jingle_bgm_scb = null;
    }

    public static void GmSoundPlaySE(string cue_name)
    {
        GmSoundPlaySE(cue_name, null);
    }

    private static void GmSoundPlaySE(string cue_name, GSS_SND_SE_HANDLE se_handle)
    {
        GsSoundPlaySe(cue_name, se_handle);
    }

    private static void GmSoundPlaySEForce(string cue_name, GSS_SND_SE_HANDLE se_handle)
    {
        GsSoundPlaySeForce(cue_name, se_handle, 0, false);
    }

    private static void GmSoundPlaySEForce(
      string cue_name,
      GSS_SND_SE_HANDLE se_handle,
      bool dontplay)
    {
        GsSoundPlaySeForce(cue_name, se_handle, 0, dontplay);
    }

    private static void GmSoundPlayBGM(string cue_name, int fade_frame)
    {
        GmSoundPlayStageBGM(fade_frame);
    }

    private static void GmSoundPlayStageBGM(int fade_frame)
    {
        GsSoundScbSetVolume(gm_sound_bgm_scb, 1f);
        GsSoundScbSetSeqMute(gm_sound_bgm_scb, false);
        GsSoundPlayBgm(gm_sound_bgm_scb, gm_sound_bgm_name_list[g_gs_main_sys_info.stage_id], fade_frame);
        gm_sound_bgm_scb.flag |= 2147483648U;
    }

    private static void GmSoundStopBGM(int fade_frame)
    {
        GmSoundStopStageBGM(fade_frame);
    }

    private static void GmSoundStopStageBGM(int fade_frame)
    {
        GsSoundStopBgm(gm_sound_bgm_scb, fade_frame);
    }

    private static void GmSoundPauseStageBGM(int fade_frame)
    {
        GsSoundPauseBgm(gm_sound_bgm_scb, fade_frame);
    }

    private static void GmSoundResumeStageBGM(int fade_frame)
    {
        GsSoundResumeBgm(gm_sound_bgm_scb, fade_frame);
    }

    private static void GmSoundChangeSpeedupBGM()
    {
        bool flag1 = false;
        bool flag2 = false;
        GsSoundStopBgm(gm_sound_bgm_sub_scb, 0);
        if (GsSoundIsBgmPause(gm_sound_bgm_scb))
            flag1 = true;
        if (((int)gm_sound_flag & 80) != 0)
            flag2 = true;
        if (flag1 | flag2)
            GmSoundStopStageBGM(0);
        else
            GmSoundStopStageBGM(0);
        if (flag1)
            GmSoundPauseStageBGM(0);
        GSS_SND_SCB gmSoundBgmScb = gm_sound_bgm_scb;
        gm_sound_bgm_scb = gm_sound_bgm_sub_scb;
        gm_sound_bgm_sub_scb = gmSoundBgmScb;
        GsSoundScbSetVolume(gm_sound_bgm_scb, 1f);
        GsSoundScbSetSeqMute(gm_sound_bgm_scb, false);
        GsSoundPlayBgm(gm_sound_bgm_scb, gm_sound_speedup_bgm_name_list[g_gs_main_sys_info.stage_id], 0);
        gm_sound_bgm_scb.flag |= 2147483648U;
        if (!flag2)
            return;
        gmSoundSetBGMFadeEnd(gm_sound_bgm_scb);
        GsSoundScbSetVolume(gm_sound_bgm_scb, 0.0f);
        GsSoundScbSetSeqMute(gm_sound_bgm_scb, true);
    }

    private static void GmSoundChangeAngryBossBGM()
    {
        bool flag1 = false;
        bool flag2 = false;
        GsSoundStopBgm(gm_sound_bgm_sub_scb, 0);
        if (GsSoundIsBgmPause(gm_sound_bgm_scb))
            flag1 = true;
        if (((int)gm_sound_flag & 80) != 0)
            flag2 = true;
        if (flag1 | flag2)
            GmSoundStopStageBGM(0);
        else
            GmSoundStopStageBGM(15);
        GSS_SND_SCB gmSoundBgmScb = gm_sound_bgm_scb;
        gm_sound_bgm_scb = gm_sound_bgm_sub_scb;
        gm_sound_bgm_sub_scb = gmSoundBgmScb;
        GsSoundScbSetVolume(gm_sound_bgm_scb, 1f);
        GsSoundScbSetSeqMute(gm_sound_bgm_scb, false);
        GsSoundPlayBgm(gm_sound_bgm_scb, GMD_SOUND_ANGRY_BOSS_BGM_NAME, 15);
        gm_sound_bgm_scb.flag |= 2147483648U;
        if (flag1)
            GmSoundPauseStageBGM(0);
        if (!flag2)
            return;
        gmSoundSetBGMFadeEnd(gm_sound_bgm_scb);
        GsSoundScbSetVolume(gm_sound_bgm_scb, 0.0f);
        GsSoundScbSetSeqMute(gm_sound_bgm_scb, true);
    }

    private static void GmSoundChangeWinBossBGM()
    {
        if (g_gs_main_sys_info.stage_id >= 16 || gm_sound_bgm_win_boss_tcb != null)
            return;
        gm_sound_bgm_win_boss_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmSoundBGMWinBossFunc), new GSF_TASK_PROCEDURE(gmSoundBGMWinBossDest), 0U, 0, (uint)short.MaxValue, 5, () => new GMS_SOUND_BGM_WIN_BOSS_MGR_WORK(), "GM_SOUND_WB");
        GMS_SOUND_BGM_WIN_BOSS_MGR_WORK work = (GMS_SOUND_BGM_WIN_BOSS_MGR_WORK)gm_sound_bgm_win_boss_tcb.work;
        work.Clear();
        work.timer = gm_sound_bgm_win_boss_wait_frame_list[GMM_MAIN_GET_ZONE_TYPE()];
    }

    private static void GmSoundChangeFinalBossBGM()
    {
        bool flag1 = false;
        bool flag2 = false;
        GsSoundStopBgm(gm_sound_bgm_sub_scb, 0);
        if (GsSoundIsBgmPause(gm_sound_bgm_scb))
            flag1 = true;
        if (((int)gm_sound_flag & 80) != 0)
            flag2 = true;
        if (flag1 | flag2)
            GmSoundStopStageBGM(0);
        else
            GmSoundStopStageBGM(15);
        GSS_SND_SCB gmSoundBgmScb = gm_sound_bgm_scb;
        gm_sound_bgm_scb = gm_sound_bgm_sub_scb;
        gm_sound_bgm_sub_scb = gmSoundBgmScb;
        GsSoundScbSetVolume(gm_sound_bgm_scb, 1f);
        GsSoundScbSetSeqMute(gm_sound_bgm_scb, false);
        GsSoundPlayBgm(gm_sound_bgm_scb, GMD_SOUND_FINAL_BOSS_BGM_NAME, 15);
        gm_sound_bgm_scb.flag |= 2147483648U;
        if (flag1)
            GmSoundPauseStageBGM(0);
        if (!flag2)
            return;
        gmSoundSetBGMFadeEnd(gm_sound_bgm_scb);
        GsSoundScbSetVolume(gm_sound_bgm_scb, 0.0f);
        GsSoundScbSetSeqMute(gm_sound_bgm_scb, true);
    }

    public static void GmSoundPlayJingle(uint jngl_idx)
    {
        GmSoundPlayJingle(jngl_idx, 0);
    }

    public static void GmSoundPlayJingle(uint jngl_idx, int fade_frame)
    {
        GsSoundScbSetVolume(gm_sound_jingle_scb, 1f);
        GsSoundScbSetSeqMute(gm_sound_jingle_scb, false);
        GsSoundStopBgm(gm_sound_jingle_scb);
        GsSoundPlayBgm(gm_sound_jingle_scb, gm_sound_jingle_name_list[(int)jngl_idx], fade_frame);
        gm_sound_jingle_scb.flag |= 2147483648U;
    }

    private static void GmSoundStopJingle(int fade_frame)
    {
        GsSoundStopBgm(gm_sound_jingle_scb, fade_frame);
    }

    private static void GmSoundPlayBGMJingle(uint jngl_idx, int fade_frame)
    {
        GsSoundScbSetVolume(gm_sound_jingle_bgm_scb, 1f);
        GsSoundScbSetSeqMute(gm_sound_jingle_bgm_scb, false);
        GsSoundStopBgm(gm_sound_jingle_bgm_scb);
        GsSoundPlayBgm(gm_sound_jingle_bgm_scb, gm_sound_jingle_name_list[(int)jngl_idx], fade_frame);
        gm_sound_jingle_bgm_scb.flag |= 2147483648U;
    }

    private static void GmSoundStopBGMJingle(int fade_frame)
    {
        GsSoundStopBgm(gm_sound_jingle_bgm_scb, fade_frame);
    }

    private static void GmSoundPauseBGMJingle(int fade_frame)
    {
        GsSoundPauseBgm(gm_sound_jingle_bgm_scb, fade_frame);
    }

    private static void GmSoundResumeBGMJingle(int fade_frame)
    {
        GsSoundResumeBgm(gm_sound_jingle_bgm_scb, fade_frame);
    }

    private static void GmSoundSetVolumeSE(float volume)
    {
        GsSoundSetVolume(1, volume);
    }

    private static void GmSoundSetVolumeBGM(float volume)
    {
        GsSoundSetVolume(0, volume);
    }

    private static void GmSoundPlayJingleObore()
    {
        if (!GsSoundIsBgmStop(gm_sound_jingle_bgm_scb) || ((int)gm_sound_flag & 1) != 0)
            return;
        if (!GsSoundIsBgmPause(gm_sound_bgm_scb))
            GmSoundPauseStageBGM(0);
        GmSoundPlayBGMJingle(6U, 0);
        if (((int)gm_sound_flag & 32) != 0)
        {
            gmSoundSetBGMFadeEnd(gm_sound_jingle_bgm_scb);
            GsSoundScbSetVolume(gm_sound_jingle_bgm_scb, 0.0f);
            GsSoundScbSetSeqMute(gm_sound_jingle_bgm_scb, true);
        }
        gm_sound_flag |= 1U;
    }

    private static void GmSoundStopJingleObore()
    {
        if (((int)gm_sound_flag & 1) == 0)
            return;
        if (!GsSoundIsBgmPause(gm_sound_jingle_bgm_scb) && ((int)gm_sound_flag & 32) == 0)
            GmSoundStopBGMJingle(15);
        else
            GmSoundStopBGMJingle(0);
        if (((int)gm_sound_flag & int.MinValue) == 0)
            GmSoundResumeStageBGM(0);
        gm_sound_flag &= 4294967294U;
    }

    private static void GmSoundPlayJingleInvincible()
    {
        if (((int)gm_sound_flag & 4) != 0)
            return;
        if (!GsSoundIsBgmPause(gm_sound_bgm_scb))
            GmSoundPauseStageBGM(0);
        GmSoundPlayBGMJingle(4U, 0);
        if (((int)gm_sound_flag & 32) != 0)
        {
            gmSoundSetBGMFadeEnd(gm_sound_jingle_bgm_scb);
            GsSoundScbSetVolume(gm_sound_jingle_bgm_scb, 0.0f);
            GsSoundScbSetSeqMute(gm_sound_jingle_bgm_scb, true);
        }
        gm_sound_flag |= 4U;
        gm_sound_flag &= 4294967294U;
    }

    private static void GmSoundStopJingleInvincible()
    {
        if (((int)gm_sound_flag & 4) == 0)
            return;
        if (!GsSoundIsBgmPause(gm_sound_jingle_bgm_scb) && ((int)gm_sound_flag & 32) == 0)
            GmSoundStopBGMJingle(0);
        else
            GmSoundStopBGMJingle(0);
        if (((int)gm_sound_flag & int.MinValue) == 0)
            GmSoundResumeStageBGM(0);
        gm_sound_flag &= 4294967291U;
    }

    private static bool GmBGMIsAlreadyPlaying()
    {
        AMS_CRIAUDIO_INTERFACE global = amCriAudioGetGlobal();
        return global.auply[gm_sound_bgm_scb.auply_no] != null && global.auply[gm_sound_bgm_scb.auply_no].se_name == gm_sound_bgm_name_list[g_gs_main_sys_info.stage_id] && !global.auply[gm_sound_bgm_scb.auply_no].IsPaused();
    }

    private static void GmSoundPlayJingle1UP(bool ret_last_sound)
    {
        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        if (betterSfx)
        {
            GmSoundPlaySE("1Up");
        }
        else
        {
            if (ret_last_sound)
                gmSoundPlay1ShotJingle(0U, 0, 0, 0);
            else
                GmSoundPlayJingle(0U, 0);
        }
    }

    private static void GmSoundPlayGameOver()
    {
        GmSoundStopStageBGM(15);
        GmSoundStopBGMJingle(15);
        if (gm_sound_1shot_tcb != null)
            mtTaskClearTcb(gm_sound_1shot_tcb);
        GmSoundPlayJingle(7U, 0);
    }

    private static void GmSoundPlayClear()
    {
        GmSoundStopStageBGM(15);
        GmSoundStopBGMJingle(15);
        if (gm_sound_1shot_tcb != null)
            mtTaskClearTcb(gm_sound_1shot_tcb);
        GmSoundPlayJingle(1U, 0);
    }

    private static void GmSoundPlayClearFinal()
    {
        GmSoundStopStageBGM(15);
        GmSoundStopBGMJingle(15);
        if (gm_sound_1shot_tcb != null)
            mtTaskClearTcb(gm_sound_1shot_tcb);
        GmSoundPlayJingle(2U, 0);
    }

    private static void GmSoundAllPause()
    {
        gm_sound_flag &= 4043309055U;
        if (!GsSoundIsBgmStop(gm_sound_jingle_scb) && !GsSoundIsBgmPause(gm_sound_jingle_scb))
        {
            GsSoundPauseBgm(gm_sound_jingle_scb, 0);
            gm_sound_flag |= 67108864U;
        }
        if (!GsSoundIsBgmStop(gm_sound_jingle_bgm_scb) && !GsSoundIsBgmPause(gm_sound_jingle_bgm_scb))
        {
            GsSoundPauseBgm(gm_sound_jingle_bgm_scb, 0);
            gm_sound_flag |= 33554432U;
        }
        if (!GsSoundIsBgmStop(gm_sound_bgm_scb) && !GsSoundIsBgmPause(gm_sound_bgm_scb))
        {
            GsSoundPauseBgm(gm_sound_bgm_scb, 0);
            gm_sound_flag |= 16777216U;
        }
        GsSoundStopBgm(gm_sound_bgm_sub_scb, 0);
        GsSoundPauseSe(128U);
        gm_sound_flag |= 134217728U;
    }

    private static void GmSoundAllResume()
    {
        if (((int)gm_sound_flag & 67108864) != 0)
            GsSoundResumeBgm(gm_sound_jingle_scb, 0);
        else if (((int)gm_sound_flag & 33554432) != 0)
            GsSoundResumeBgm(gm_sound_jingle_bgm_scb, 0);
        else if (((int)gm_sound_flag & 16777216) != 0)
            GsSoundResumeBgm(gm_sound_bgm_scb, 0);
        GsSoundResumeSe(128U);
        gm_sound_flag &= 4043309055U;
    }

    private static void gmSoundPlay1ShotJingle(
      uint jngl_idx,
      int jingle_fade_in_frame,
      int bgm_fade_out_frame,
      int bgm_fade_in_frame)
    {
        gm_sound_flag |= 2147483648U;
        if (!GsSoundIsBgmStop(gm_sound_bgm_scb) && !GsSoundIsBgmPause(gm_sound_bgm_scb))
            GmSoundPauseStageBGM(bgm_fade_out_frame);
        if (!GsSoundIsBgmStop(gm_sound_jingle_bgm_scb) && !GsSoundIsBgmPause(gm_sound_jingle_bgm_scb))
            GmSoundPauseBGMJingle(bgm_fade_out_frame);
        if (gm_sound_1shot_tcb != null)
            GmSoundStopJingle(0);
        else
            gm_sound_1shot_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmSound1ShotJingleFunc), new GSF_TASK_PROCEDURE(gmSound1ShotJingleDest), 0U, 0, (uint)short.MaxValue, 5, () => new GMS_SOUND_1SHOT_JINGLE_WORK(), "GM_SOUND_1SH");
        GMS_SOUND_1SHOT_JINGLE_WORK work = (GMS_SOUND_1SHOT_JINGLE_WORK)gm_sound_1shot_tcb.work;
        work.Clear();
        GmSoundPlayJingle(jngl_idx, jingle_fade_in_frame);
        work.bgm_fade_in_frame = bgm_fade_in_frame;
    }

    private static void gmSound1ShotJingleFunc(MTS_TASK_TCB tcb)
    {
        if (((int)gm_sound_flag & 134217728) != 0)
            return;
        GMS_SOUND_1SHOT_JINGLE_WORK work = (GMS_SOUND_1SHOT_JINGLE_WORK)gm_sound_1shot_tcb.work;
        if (!GsSoundIsBgmStop(gm_sound_jingle_scb))
            return;
        GmSoundStopJingle(0);
        gm_sound_flag &= int.MaxValue;
        if (!GsSoundIsBgmStop(gm_sound_jingle_bgm_scb) && GsSoundIsBgmPause(gm_sound_jingle_bgm_scb))
            GmSoundResumeBGMJingle(work.bgm_fade_in_frame);
        else if (GsSoundIsBgmStop(gm_sound_bgm_scb) || GsSoundIsBgmPause(gm_sound_bgm_scb))
            GmSoundResumeStageBGM(work.bgm_fade_in_frame);
        mtTaskClearTcb(tcb);
    }

    private static void gmSound1ShotJingleDest(MTS_TASK_TCB tcb)
    {
        gm_sound_1shot_tcb = null;
    }

    private static void gmSoundSetBGMFade(
      GSS_SND_SCB snd_scb,
      float start_vol,
      float end_vol,
      int frame)
    {
        if (GsSoundIsBgmStop(gm_sound_bgm_scb) || GsSoundIsBgmPause(gm_sound_bgm_scb))
            return;
        gmSoundSetBGMFadeEnd(snd_scb);
        if (frame <= 0)
            frame = 1;
        if (gm_sound_bgm_fade_tcb == null)
        {
            gm_sound_bgm_fade_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmSoundBGMFadeFunc), new GSF_TASK_PROCEDURE(gmSoundBGMFadeDest), 0U, 0, (uint)short.MaxValue, 5, () => new GMS_SOUND_BGM_FADE_MGR_WORK(), "GM_SOUND_BFADE");
            ((GMS_SOUND_BGM_FADE_MGR_WORK)gm_sound_bgm_fade_tcb.work).Clear();
        }
        gmSoundBGMFadeAttachList((GMS_SOUND_BGM_FADE_MGR_WORK)gm_sound_bgm_fade_tcb.work, new GMS_SOUND_BGM_FADE_WORK()
        {
            snd_scb = snd_scb,
            start_vol = start_vol,
            end_vol = end_vol,
            frame = frame,
            fade_spd = (end_vol - start_vol) / frame,
            now_vol = start_vol
        });
    }

    private static void gmSoundSetBGMFadeEnd(GSS_SND_SCB snd_scb)
    {
        if (gm_sound_bgm_fade_tcb == null)
            return;
        GMS_SOUND_BGM_FADE_MGR_WORK work = (GMS_SOUND_BGM_FADE_MGR_WORK)gm_sound_bgm_fade_tcb.work;
        GMS_SOUND_BGM_FADE_WORK next;
        for (GMS_SOUND_BGM_FADE_WORK fade_work = work.head; fade_work != null; fade_work = next)
        {
            next = fade_work.next;
            if (fade_work.snd_scb == snd_scb)
                gmSoundBGMFadeDetachList(work, fade_work);
        }
        if (work.num > 0)
            return;
        mtTaskClearTcb(gm_sound_bgm_fade_tcb);
    }

    private static void gmSoundBGMFadeFunc(MTS_TASK_TCB tcb)
    {
        GMS_SOUND_BGM_FADE_MGR_WORK work = (GMS_SOUND_BGM_FADE_MGR_WORK)tcb.work;
        GMS_SOUND_BGM_FADE_WORK next;
        for (GMS_SOUND_BGM_FADE_WORK fade_work = work.head; fade_work != null; fade_work = next)
        {
            next = fade_work.next;
            fade_work.now_vol += fade_work.fade_spd;
            --fade_work.frame;
            if (fade_work.frame <= 0)
                fade_work.now_vol = fade_work.end_vol;
            GsSoundScbSetVolume(fade_work.snd_scb, fade_work.now_vol);
            if (fade_work.frame <= 0 || GsSoundIsBgmStop(fade_work.snd_scb))
            {
                if (fade_work.now_vol > 0.0)
                    GsSoundScbSetSeqMute(fade_work.snd_scb, false);
                else
                    GsSoundScbSetSeqMute(fade_work.snd_scb, true);
                gmSoundBGMFadeDetachList(work, fade_work);
            }
        }
        if (work.num > 0)
            return;
        mtTaskClearTcb(tcb);
    }

    private static void gmSoundBGMFadeDest(MTS_TASK_TCB tcb)
    {
        GMS_SOUND_BGM_FADE_MGR_WORK work = (GMS_SOUND_BGM_FADE_MGR_WORK)tcb.work;
        GMS_SOUND_BGM_FADE_WORK next;
        for (GMS_SOUND_BGM_FADE_WORK fade_work = work.head; fade_work != null; fade_work = next)
        {
            next = fade_work.next;
            gmSoundBGMFadeDetachList(work, fade_work);
        }
        if (gm_sound_bgm_fade_tcb != tcb)
            return;
        gm_sound_bgm_fade_tcb = null;
    }

    private static void gmSoundBGMFadeAttachList(
      GMS_SOUND_BGM_FADE_MGR_WORK mgr_work,
      GMS_SOUND_BGM_FADE_WORK fade_work)
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
      GMS_SOUND_BGM_FADE_MGR_WORK mgr_work,
      GMS_SOUND_BGM_FADE_WORK fade_work)
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

    private static void gmSoundBGMWinBossFunc(MTS_TASK_TCB tcb)
    {
        var work = (GMS_SOUND_BGM_WIN_BOSS_MGR_WORK)tcb.work;
        if (((int)gm_sound_flag & 134217728) != 0)
            return;
        --work.timer;
        if (work.timer > 0)
            return;
        bool flag1 = false;
        bool flag2 = false;
        GsSoundStopBgm(gm_sound_bgm_sub_scb, 0);
        if (GsSoundIsBgmPause(gm_sound_bgm_scb))
            flag1 = true;
        if (((int)gm_sound_flag & 80) != 0)
            flag2 = true;
        if (flag1 | flag2)
            GmSoundStopStageBGM(0);
        else
            GmSoundStopStageBGM(30);
        GSS_SND_SCB gmSoundBgmScb = gm_sound_bgm_scb;
        gm_sound_bgm_scb = gm_sound_bgm_sub_scb;
        gm_sound_bgm_sub_scb = gmSoundBgmScb;
        GsSoundScbSetVolume(gm_sound_bgm_scb, 1f);
        GsSoundScbSetSeqMute(gm_sound_bgm_scb, false);
        GsSoundPlayBgm(gm_sound_bgm_scb, gm_sound_bgm_win_boss_name_list[GMM_MAIN_GET_ZONE_TYPE()], 30);
        gm_sound_bgm_scb.flag |= 2147483648U;
        if (flag1)
            GmSoundPauseStageBGM(0);
        if (flag2)
        {
            gmSoundSetBGMFadeEnd(gm_sound_bgm_scb);
            GsSoundScbSetVolume(gm_sound_bgm_scb, 0.0f);
            GsSoundScbSetSeqMute(gm_sound_bgm_scb, true);
        }
        mtTaskClearTcb(tcb);
    }

    private static void gmSoundBGMWinBossDest(MTS_TASK_TCB tcb)
    {
        if (tcb != gm_sound_bgm_win_boss_tcb)
            return;
        gm_sound_bgm_win_boss_tcb = null;
    }
}