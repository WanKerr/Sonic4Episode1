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

    public static void DmSndBgmPlayerInit()
    {
        AppMain.dmSndBgmPlayerInit();
    }

    public static void DmSndBgmPlayerExit()
    {
        AppMain.dm_snd_bgm_player_flag |= 4U;
    }

    public static void DmSndBgmPlayerBgmStop()
    {
        AppMain.dm_snd_bgm_player_flag |= 8U;
    }

    private static bool DmSndBgmPlayerIsTaskExit()
    {
        return AppMain.dm_snd_bgm_player_tcb == null;
    }

    private static bool DmSndBgmPlayerIsSndSysBuild()
    {
        return AppMain.DmSoundBuildCheck();
    }

    public static void DmSndBgmPlayerPlayBgm(int idx)
    {
        if (AppMain.dm_snd_bgm_player_tcb == null)
            AppMain.dmSndBgmPlayerInit();
        switch (idx)
        {
            case 0:
                if (((int)AppMain.dm_snd_bgm_player_flag & 16) != 0)
                {
                    AppMain.dm_snd_bgm_player_flag |= 128U;
                    break;
                }
                if (((int)AppMain.dm_snd_bgm_player_flag & 32) != 0)
                    break;
                AppMain.dm_snd_bgm_player_flag |= 32U;
                break;
            case 1:
                if (((int)AppMain.dm_snd_bgm_player_flag & 32) != 0)
                {
                    AppMain.dm_snd_bgm_player_flag |= 64U;
                    break;
                }
                if (((int)AppMain.dm_snd_bgm_player_flag & 16) != 0)
                    break;
                AppMain.dm_snd_bgm_player_flag |= 16U;
                break;
        }
    }

    private static void dmSndBgmPlayerInit()
    {
        if (AppMain.dm_snd_bgm_player_tcb != null)
            return;
        AppMain.dm_snd_bgm_player_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.dmSndBgmPlayerProcMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.dmSndBgmPlayerDest), 0U, (ushort)0, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK()), "DM_SND_BGM_PLAYER_MAIN");
        AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK work = (AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK)AppMain.dm_snd_bgm_player_tcb.work;
        AppMain.dm_snd_bgm_player_flag = 0U;
        work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcInit);
    }

    private static void dmSndBgmPlayerProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK work = (AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK)tcb.work;
        if (((int)AppMain.dm_snd_bgm_player_flag & 1) != 0)
        {
            AppMain.mtTaskClearTcb(tcb);
            AppMain.dm_snd_bgm_player_flag = 0U;
            AppMain.dm_snd_bgm_player_tcb = (AppMain.MTS_TASK_TCB)null;
        }
        if (work.proc_update == null)
            return;
        work.proc_update(work);
    }

    private static void dmSndBgmPlayerDest(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static void dmSndBgmPlayerProcInit(AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        AppMain.DmSoundBuild();
        main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcBuildIdle);
    }

    private static void dmSndBgmPlayerProcBuildIdle(AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        if (!AppMain.DmSoundBuildCheck())
            return;
        AppMain.DmSoundInit();
        main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcWaitSetBgm);
        main_work.end_timer = 0;
    }

    private static void dmSndBgmPlayerProcWaitSetBgm(AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        AppMain.GsGetMainSysInfo();
        if (((int)AppMain.dm_snd_bgm_player_flag & 8) != 0 || ((int)AppMain.dm_snd_bgm_player_flag & 4) != 0)
        {
            AppMain.DmSoundStopJingle(24);
            AppMain.DmSoundStopBGM(24);
            main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcStopIdle);
        }
        else if (((int)AppMain.dm_snd_bgm_player_flag & 16) != 0 || ((int)AppMain.dm_snd_bgm_player_flag & 32) != 0)
        {
            if (((int)AppMain.dm_snd_bgm_player_flag & 16) != 0)
            {
                AppMain.DmSoundPlayJingle(0, 0);
                AppMain.dm_snd_bgm_player_flag &= 4294967263U;
            }
            else
            {
                AppMain.DmSoundPlayMenuBGM(0, 32);
                AppMain.dm_snd_bgm_player_flag &= 4294967279U;
            }
            main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcPlayIdle);
            main_work.end_timer = 0;
        }
        else if (((int)AppMain.dm_snd_bgm_player_flag & 64) != 0)
        {
            AppMain.DmSoundStopBGM(0);
            main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcStopIdle);
        }
        else
        {
            if (((int)AppMain.dm_snd_bgm_player_flag & 128) == 0)
                return;
            AppMain.DmSoundStopJingle(24);
            main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcStopIdle);
        }
    }

    private static void dmSndBgmPlayerProcPlayIdle(AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        if (((int)AppMain.dm_snd_bgm_player_flag & 8) != 0 || ((int)AppMain.dm_snd_bgm_player_flag & 4) != 0)
        {
            AppMain.DmSoundStopJingle(24);
            AppMain.DmSoundStopBGM(24);
            main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcStopIdle);
        }
        else if (((int)AppMain.dm_snd_bgm_player_flag & 64) != 0)
        {
            AppMain.DmSoundStopBGM(0);
            main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcStopIdle);
        }
        else
        {
            if (((int)AppMain.dm_snd_bgm_player_flag & 128) == 0)
                return;
            AppMain.DmSoundStopJingle(24);
            main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcStopIdle);
        }
    }

    private static void dmSndBgmPlayerProcStopIdle(AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        if (!AppMain.DmSoundIsStopStageBGM() || !AppMain.DmSoundIsStopJingle())
            return;
        if (((int)AppMain.dm_snd_bgm_player_flag & 4) != 0)
        {
            AppMain.dm_snd_bgm_player_flag &= 4294967291U;
            main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcSndRelease);
        }
        else
        {
            if (((int)AppMain.dm_snd_bgm_player_flag & 64) != 0)
            {
                AppMain.dm_snd_bgm_player_flag |= 16U;
                AppMain.dm_snd_bgm_player_flag &= 4294967231U;
            }
            else if (((int)AppMain.dm_snd_bgm_player_flag & 64) == 0 && ((int)AppMain.dm_snd_bgm_player_flag & 128) == 0 && ((int)AppMain.dm_snd_bgm_player_flag & 8) == 0)
                AppMain.dm_snd_bgm_player_flag |= 16U;
            else if (((int)AppMain.dm_snd_bgm_player_flag & 8) != 0)
            {
                AppMain.dm_snd_bgm_player_flag &= 4294967279U;
                AppMain.dm_snd_bgm_player_flag &= 4294967263U;
                AppMain.dm_snd_bgm_player_flag &= 4294967287U;
            }
            else
            {
                AppMain.dm_snd_bgm_player_flag |= 32U;
                AppMain.dm_snd_bgm_player_flag &= 4294967167U;
            }
            main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcWaitSetBgm);
        }
    }

    private static void dmSndBgmPlayerProcSndRelease(AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        AppMain.DmSoundExit();
        AppMain.DmSoundFlush();
        main_work.proc_update = new AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_(AppMain.dmSndBgmPlayerProcSndFinish);
    }

    private static void dmSndBgmPlayerProcSndFinish(AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK main_work)
    {
        main_work.proc_update = (AppMain.DMS_SND_BGM_PLAYER_MAIN_WORK._proc_)null;
        AppMain.dm_snd_bgm_player_flag |= 1U;
    }
}