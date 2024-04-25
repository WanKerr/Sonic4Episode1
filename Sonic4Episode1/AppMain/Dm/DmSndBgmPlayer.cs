public partial class AppMain
{

    public static void DmSndBgmPlayerInit()
    {
        dmSndBgmPlayerInit();
    }

    public static void DmSndBgmPlayerExit()
    {
        dm_snd_bgm_player_flag |= 4U;
    }

    public static void DmSndBgmPlayerBgmStop()
    {
        dm_snd_bgm_player_flag |= 8U;
    }

    private static bool DmSndBgmPlayerIsTaskExit()
    {
        return dm_snd_bgm_player_tcb == null;
    }

    private static bool DmSndBgmPlayerIsSndSysBuild()
    {
        return DmSoundBuildCheck();
    }

    public static void DmSndBgmPlayerPlayBgm(int idx)
    {
        if (dm_snd_bgm_player_tcb == null)
            dmSndBgmPlayerInit();
        switch (idx)
        {
            case 0:
                if (((int)dm_snd_bgm_player_flag & 16) != 0)
                {
                    dm_snd_bgm_player_flag |= 128U;
                    break;
                }
                if (((int)dm_snd_bgm_player_flag & 32) != 0)
                    break;
                dm_snd_bgm_player_flag |= 32U;
                break;
            case 1:
                if (((int)dm_snd_bgm_player_flag & 32) != 0)
                {
                    dm_snd_bgm_player_flag |= 64U;
                    break;
                }
                if (((int)dm_snd_bgm_player_flag & 16) != 0)
                    break;
                dm_snd_bgm_player_flag |= 16U;
                break;
            case 2:
                if (((int)dm_snd_bgm_player_flag & 256) != 0)
                    break;
                dm_snd_bgm_player_flag |= 256;
                break;
        }
    }

    private static void dmSndBgmPlayerInit()
    {
        if (dm_snd_bgm_player_tcb != null)
            return;
        dm_snd_bgm_player_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(dmSndBgmPlayerProcMain), new GSF_TASK_PROCEDURE(dmSndBgmPlayerDest), 0U, 0, 4096U, 0, () => new DMS_SND_BGM_PLAYER_MAIN_WORK(), "DM_SND_BGM_PLAYER_MAIN");
        DMS_SND_BGM_PLAYER_MAIN_WORK work = (DMS_SND_BGM_PLAYER_MAIN_WORK)dm_snd_bgm_player_tcb.work;
        dm_snd_bgm_player_flag = 0U;
        work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcInit);
    }

    private static void dmSndBgmPlayerProcMain(MTS_TASK_TCB tcb)
    {
        DMS_SND_BGM_PLAYER_MAIN_WORK work = (DMS_SND_BGM_PLAYER_MAIN_WORK)tcb.work;
        if (((int)dm_snd_bgm_player_flag & 1) != 0)
        {
            mtTaskClearTcb(tcb);
            dm_snd_bgm_player_flag = 0U;
            dm_snd_bgm_player_tcb = null;
        }
        if (work.proc_update == null)
            return;
        work.proc_update(work);
    }

    private static void dmSndBgmPlayerDest(MTS_TASK_TCB tcb)
    {
    }

    private static void dmSndBgmPlayerProcInit(DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        DmSoundBuild();
        main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcBuildIdle);
    }

    private static void dmSndBgmPlayerProcBuildIdle(DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        if (!DmSoundBuildCheck())
            return;
        DmSoundInit();
        main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcWaitSetBgm);
        main_work.end_timer = 0;
    }

    private static void dmSndBgmPlayerProcWaitSetBgm(DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        GsGetMainSysInfo();
        if (((int)dm_snd_bgm_player_flag & 8) != 0 || ((int)dm_snd_bgm_player_flag & 4) != 0)
        {
            DmSoundStopJingle(24);
            DmSoundStopBGM(24);
            main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcStopIdle);
        }
        else if (((int)dm_snd_bgm_player_flag & 16) != 0 || ((int)dm_snd_bgm_player_flag & 32) != 0 || ((int)dm_snd_bgm_player_flag & 256) != 0)
        {
            if (((int)dm_snd_bgm_player_flag & 16) != 0)
            {
                DmSoundPlayJingle(0, 0);
                dm_snd_bgm_player_flag &= 4294967263U;
            }
            else if (((int)dm_snd_bgm_player_flag & 256) != 0)
            {
                DmSoundPlayMenuBGM(0, 32);
                dm_snd_bgm_player_flag &= 4294967279U;
            }
            else
            {
                DmSoundPlayMenuBGM(0, 32);
                dm_snd_bgm_player_flag &= 4294967279U;
            }
            main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcPlayIdle);
            main_work.end_timer = 0;
        }
        else if (((int)dm_snd_bgm_player_flag & 64) != 0)
        {
            DmSoundStopBGM(0);
            main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcStopIdle);
        }
        else
        {
            if (((int)dm_snd_bgm_player_flag & 128) == 0)
                return;
            DmSoundStopJingle(24);
            main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcStopIdle);
        }
    }

    private static void dmSndBgmPlayerProcPlayIdle(DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        if (((int)dm_snd_bgm_player_flag & 8) != 0 || ((int)dm_snd_bgm_player_flag & 4) != 0)
        {
            DmSoundStopJingle(24);
            DmSoundStopBGM(24);
            main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcStopIdle);
        }
        else if (((int)dm_snd_bgm_player_flag & 64) != 0)
        {
            DmSoundStopBGM(0);
            main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcStopIdle);
        }
        else
        {
            if (((int)dm_snd_bgm_player_flag & 128) == 0)
                return;
            DmSoundStopJingle(24);
            main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcStopIdle);
        }
    }

    private static void dmSndBgmPlayerProcStopIdle(DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        if (!DmSoundIsStopStageBGM() || !DmSoundIsStopJingle())
            return;
        if (((int)dm_snd_bgm_player_flag & 4) != 0)
        {
            dm_snd_bgm_player_flag &= 4294967291U;
            main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcSndRelease);
        }
        else
        {
            if (((int)dm_snd_bgm_player_flag & 64) != 0)
            {
                dm_snd_bgm_player_flag |= 16U;
                dm_snd_bgm_player_flag &= 4294967231U;
            }
            else if (((int)dm_snd_bgm_player_flag & 64) == 0 && ((int)dm_snd_bgm_player_flag & 128) == 0 && ((int)dm_snd_bgm_player_flag & 8) == 0)
                dm_snd_bgm_player_flag |= 16U;
            else if (((int)dm_snd_bgm_player_flag & 8) != 0)
            {
                dm_snd_bgm_player_flag &= 4294967279U;
                dm_snd_bgm_player_flag &= 4294967263U;
                dm_snd_bgm_player_flag &= 4294967287U;
            }
            else
            {
                dm_snd_bgm_player_flag |= 32U;
                dm_snd_bgm_player_flag &= 4294967167U;
            }
            main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcWaitSetBgm);
        }
    }

    private static void dmSndBgmPlayerProcSndRelease(DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        DmSoundExit();
        DmSoundFlush();
        main_work.proc_update = new DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(dmSndBgmPlayerProcSndFinish);
    }

    private static void dmSndBgmPlayerProcSndFinish(DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        main_work.proc_update = null;
        dm_snd_bgm_player_flag |= 1U;
    }
}