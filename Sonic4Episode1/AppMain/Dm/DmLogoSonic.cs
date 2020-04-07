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
    public void DmLogoSonicInit(object arg)
    {
        if (this.DmLogoSonicBuildCheck())
        {
            this.dmLogoSonicStart();
        }
        else
        {
            AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSonicLoadWait), (AppMain.GSF_TASK_PROCEDURE)null, 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "DM_LSONT_LW");
            this.DmLogoSonicLoad();
        }
    }

    public void DmLogoSonicLoad()
    {
        this.DmLogoComLoadFileCreate(AppMain.dm_logo_sonic_load_tcb);
        this.DmLogoComLoadFileReg(AppMain.dm_logo_sonic_load_tcb, AppMain.dm_logo_sonic_com_fileinfo_list, AppMain.dm_logo_sonic_com_file_num);
        this.DmLogoComLoadFileStart(AppMain.dm_logo_sonic_load_tcb);
    }

    public bool DmLogoSonicLoadCheck()
    {
        return AppMain.dm_logo_sonic_load_tcb.Target == null && AppMain.dm_logo_sonic_data[0] != null;
    }

    public void DmLogoSonicBuild()
    {
        AppMain.AMS_AMB_HEADER[] amsAmbHeaderArray = new AppMain.AMS_AMB_HEADER[1];
        AppMain.dm_logo_sonic_build_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSonicDataBuildMain), new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSonicDataBuildDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "DM_LSONT_BUILD");
        AppMain.dm_logo_sonic_aos_tex = AppMain.New<AppMain.AOS_TEXTURE>(1);
        string sPath;
        amsAmbHeaderArray[0] = AppMain.readAMBFile(AppMain.amBindGet(AppMain.dm_logo_sonic_data[0], 1, out sPath));
        amsAmbHeaderArray[0].dir = sPath;
        AppMain.AOS_TEXTURE[] dmLogoSonicAosTex = AppMain.dm_logo_sonic_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            AppMain.AoTexBuild(dmLogoSonicAosTex[index], amsAmbHeaderArray[index]);
            AppMain.AoTexLoad(dmLogoSonicAosTex[index]);
        }
    }

    public bool DmLogoSonicBuildCheck()
    {
        return AppMain.dm_logo_sonic_build_state;
    }

    public void DmLogoSonicFlush()
    {
        AppMain.dm_logo_sonic_flush_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSonicDataFlushMain), new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSonicDataFlushDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "DM_LSONT_FLUSH");
        AppMain.AOS_TEXTURE[] dmLogoSonicAosTex = AppMain.dm_logo_sonic_aos_tex;
        for (int index = 0; index < 1; ++index)
            AppMain.AoTexRelease(dmLogoSonicAosTex[index]);
    }

    public bool DmLogoSonicFlushCheck()
    {
        return !AppMain.dm_logo_sonic_build_state;
    }

    public void DmLogoSonicRelease()
    {
        for (int index = 0; index < 1; ++index)
            AppMain.dm_logo_sonic_data[index] = (AppMain.AMS_AMB_HEADER)null;
    }

    public bool DmLogoSonicReleaseCheck()
    {
        return AppMain.dm_logo_sonic_load_tcb.Target == null && AppMain.dm_logo_sonic_data[0] == null;
    }

    public void dmLogoSonicLoadWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSonicLoadCheck())
            return;
        this.DmLogoSonicBuild();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSonicBuildWait));
    }

    public void dmLogoSonicBuildWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSonicBuildCheck())
            return;
        AppMain.mtTaskClearTcb(tcb);
        this.dmLogoSonicStart();
    }

    public void dmLogoSonicActionCreate(AppMain.DMS_LOGO_SONIC_WORK logo_work)
    {
        AppMain.A2S_AMA_HEADER ama = AppMain.readAMAFile(AppMain.amBindGet(AppMain.dm_logo_sonic_data[0], 0));
        for (uint id = 0; id < 2U; ++id)
        {
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_logo_sonic_aos_tex[(int)AppMain.dm_logo_sonic_tex_id_tbl[(int)id]]));
            logo_work.act[(int)id] = AppMain.AoActCreate(ama, id);
        }
    }

    public void dmLogoSonicActionDelete(AppMain.DMS_LOGO_SONIC_WORK logo_work)
    {
        for (int index = 0; index < 2; ++index)
            AppMain.AoActDelete(logo_work.act[index]);
    }

    public void dmLogoSonicStart()
    {
        AppMain.NNS_RGBA diffuse = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_RGB ambient = new AppMain.NNS_RGB(1f, 1f, 1f);
        AppMain.DMS_LOGO_SONIC_WORK work = (AppMain.DMS_LOGO_SONIC_WORK)AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSonicMainFunc), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_LOGO_SONIC_WORK()), "DM_LSONT_MAIN").work;
        AppMain.nnSetPrimitive3DMaterial(ref diffuse, ref ambient, 1f);
        AppMain.AoActSysSetDrawStateEnable(false);
        this.dmLogoSonicActionCreate(work);
        AppMain.IzFadeInitEasy(0U, 2U, 60f, true);
        work.func = new AppMain.DMS_LOGO_SONIC_WORK._func_(this.dmLogoSonicFadeInWaitFunc);
    }

    public void dmLogoSonicMainFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_LOGO_SONIC_WORK work = (AppMain.DMS_LOGO_SONIC_WORK)tcb.work;
        if (AppMain.AoSysIsShowPlatformUI())
        {
            if (AppMain.IzFadeIsExe())
                AppMain.IzFadeSetStopUpdate1Frame((AppMain.IZS_FADE_WORK)null);
        }
        else
        {
            if (work.func != null)
                work.func(work);
            if (((int)work.flag & 1) != 0 && ((int)work.flag & 2) == 0 && (AppMain.amTpIsTouchPush(0) || AppMain.isBackKeyPressed()))
            {
                AppMain.setBackKeyRequest(false);
                work.flag |= 2U;
                if (AppMain.IzFadeIsEnd())
                    AppMain.IzFadeInitEasy(0U, 3U, 10f, true);
                work.func = new AppMain.DMS_LOGO_SONIC_WORK._func_(this.dmLogoSonicFadeOutWaitFunc);
            }
            if (((int)work.flag & 4) != 0)
            {
                AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSonicPreEndWait));
                work.timer = 0;
                return;
            }
        }
        float frame = 0.0f;
        if (!AppMain.AoSysIsShowPlatformUI())
            frame = 1f;
        AppMain.AoActSysSetDrawTaskPrio();
        for (int index = 0; index < 2; ++index)
        {
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_logo_sonic_aos_tex[(int)AppMain.dm_logo_sonic_tex_id_tbl[index]]));
            AppMain.AoActUpdate(work.act[index], frame);
            AppMain.AoActDraw(work.act[index]);
        }
    }

    public void dmLogoSonicFadeInWaitFunc(AppMain.DMS_LOGO_SONIC_WORK logo_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        logo_work.func = new AppMain.DMS_LOGO_SONIC_WORK._func_(this.dmLogoSonicDispWaitFunc);
        logo_work.timer = 0;
    }

    public void dmLogoSonicDispWaitFunc(AppMain.DMS_LOGO_SONIC_WORK logo_work)
    {
        ++logo_work.timer;
        if (logo_work.timer >= 120)
        {
            logo_work.func = new AppMain.DMS_LOGO_SONIC_WORK._func_(this.dmLogoSonicFadeOutWaitFunc);
            AppMain.IzFadeInitEasy(0U, 3U, 60f, true);
            logo_work.flag &= 4294967294U;
        }
        else
        {
            if (logo_work.timer != 30)
                return;
            logo_work.flag |= 1U;
        }
    }

    public void dmLogoSonicFadeOutWaitFunc(AppMain.DMS_LOGO_SONIC_WORK logo_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        logo_work.flag |= 4U;
    }

    public void dmLogoSonicPreEndWait(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_LOGO_SONIC_WORK work = (AppMain.DMS_LOGO_SONIC_WORK)tcb.work;
        ++work.timer;
        if (work.timer <= 2)
            return;
        this.dmLogoSonicActionDelete(work);
        this.DmLogoSonicFlush();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSonicFlushWaitFunc));
    }

    public void dmLogoSonicFlushWaitFunc(AppMain.MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSonicFlushCheck())
            return;
        this.DmLogoSonicRelease();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSonicRelesehWaitFunc));
    }

    public void dmLogoSonicRelesehWaitFunc(AppMain.MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSonicReleaseCheck())
            return;
        AppMain.mtTaskClearTcb(tcb);
        AppMain.SyChangeNextEvt();
    }

    public static void dmLogoSonicLoadPostFunc(AppMain.DMS_LOGO_COM_LOAD_CONTEXT context)
    {
        AppMain.dm_logo_sonic_data[context.no] = AppMain.readAMBFile(context.fs_req);
    }

    public void dmLogoSonicDataBuildMain(AppMain.MTS_TASK_TCB tcb)
    {
        bool flag = true;
        AppMain.AOS_TEXTURE[] dmLogoSonicAosTex = AppMain.dm_logo_sonic_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            if (!AppMain.AoTexIsLoaded(dmLogoSonicAosTex[index]))
                flag = false;
        }
        if (!flag)
            return;
        AppMain.mtTaskClearTcb(tcb);
        AppMain.dm_logo_sonic_build_state = true;
    }

    public void dmLogoSonicDataBuildDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.dm_logo_sonic_build_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    public void dmLogoSonicDataFlushMain(AppMain.MTS_TASK_TCB tcb)
    {
        bool flag = true;
        AppMain.AOS_TEXTURE[] dmLogoSonicAosTex = AppMain.dm_logo_sonic_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            if (!AppMain.AoTexIsReleased(dmLogoSonicAosTex[index]))
                flag = false;
        }
        if (!flag)
            return;
        AppMain.dm_logo_sonic_aos_tex = (AppMain.AOS_TEXTURE[])null;
        AppMain.mtTaskClearTcb(tcb);
        AppMain.dm_logo_sonic_build_state = false;
    }

    public void dmLogoSonicDataFlushDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.dm_logo_sonic_flush_tcb = (AppMain.MTS_TASK_TCB)null;
    }

}