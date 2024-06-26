﻿public partial class AppMain
{
    public void DmLogoSonicInit(object arg)
    {
        if (this.DmLogoSonicBuildCheck())
        {
            this.dmLogoSonicStart();
        }
        else
        {
            MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(this.dmLogoSonicLoadWait), null, 0U, ushort.MaxValue, 4096U, 0, null, "DM_LSONT_LW");
            this.DmLogoSonicLoad();
        }
    }

    public void DmLogoSonicLoad()
    {
        this.DmLogoComLoadFileCreate(dm_logo_sonic_load_tcb);
        this.DmLogoComLoadFileReg(dm_logo_sonic_load_tcb, dm_logo_sonic_com_fileinfo_list, dm_logo_sonic_com_file_num);
        this.DmLogoComLoadFileStart(dm_logo_sonic_load_tcb);
    }

    public bool DmLogoSonicLoadCheck()
    {
        return dm_logo_sonic_load_tcb.Target == null && dm_logo_sonic_data[0] != null;
    }

    public void DmLogoSonicBuild()
    {
        AMS_AMB_HEADER[] amsAmbHeaderArray = new AMS_AMB_HEADER[1];
        dm_logo_sonic_build_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(this.dmLogoSonicDataBuildMain), new GSF_TASK_PROCEDURE(this.dmLogoSonicDataBuildDest), 0U, ushort.MaxValue, 4096U, 0, null, "DM_LSONT_BUILD");
        dm_logo_sonic_aos_tex = New<AOS_TEXTURE>(1);
        string sPath;
        amsAmbHeaderArray[0] = readAMBFile(amBindGet(dm_logo_sonic_data[0], 1, out sPath));
        amsAmbHeaderArray[0].dir = sPath;
        AOS_TEXTURE[] dmLogoSonicAosTex = dm_logo_sonic_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            AoTexBuild(dmLogoSonicAosTex[index], amsAmbHeaderArray[index]);
            AoTexLoad(dmLogoSonicAosTex[index]);
        }
    }

    public bool DmLogoSonicBuildCheck()
    {
        return dm_logo_sonic_build_state;
    }

    public void DmLogoSonicFlush()
    {
        dm_logo_sonic_flush_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(this.dmLogoSonicDataFlushMain), new GSF_TASK_PROCEDURE(this.dmLogoSonicDataFlushDest), 0U, ushort.MaxValue, 4096U, 0, null, "DM_LSONT_FLUSH");
        AOS_TEXTURE[] dmLogoSonicAosTex = dm_logo_sonic_aos_tex;
        for (int index = 0; index < 1; ++index)
            AoTexRelease(dmLogoSonicAosTex[index]);
    }

    public bool DmLogoSonicFlushCheck()
    {
        return !dm_logo_sonic_build_state;
    }

    public void DmLogoSonicRelease()
    {
        for (int index = 0; index < 1; ++index)
            dm_logo_sonic_data[index] = null;
    }

    public bool DmLogoSonicReleaseCheck()
    {
        return dm_logo_sonic_load_tcb.Target == null && dm_logo_sonic_data[0] == null;
    }

    public void dmLogoSonicLoadWait(MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSonicLoadCheck())
            return;
        this.DmLogoSonicBuild();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(this.dmLogoSonicBuildWait));
    }

    public void dmLogoSonicBuildWait(MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSonicBuildCheck())
            return;
        mtTaskClearTcb(tcb);
        this.dmLogoSonicStart();
    }

    public void dmLogoSonicActionCreate(DMS_LOGO_SONIC_WORK logo_work)
    {
        A2S_AMA_HEADER ama = readAMAFile(amBindGet(dm_logo_sonic_data[0], 0));
        for (uint id = 0; id < 2U; ++id)
        {
            AoActSetTexture(AoTexGetTexList(dm_logo_sonic_aos_tex[dm_logo_sonic_tex_id_tbl[(int)id]]));
            logo_work.act[(int)id] = AoActCreate(ama, id);
        }
    }

    public void dmLogoSonicActionDelete(DMS_LOGO_SONIC_WORK logo_work)
    {
        for (int index = 0; index < 2; ++index)
            AoActDelete(logo_work.act[index]);
    }

    public void dmLogoSonicStart()
    {
        NNS_RGBA diffuse = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_RGB ambient = new NNS_RGB(1f, 1f, 1f);
        DMS_LOGO_SONIC_WORK work = (DMS_LOGO_SONIC_WORK)MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(this.dmLogoSonicMainFunc), null, 0U, 0, 4096U, 0, () => new DMS_LOGO_SONIC_WORK(), "DM_LSONT_MAIN").work;
        nnSetPrimitive3DMaterial(ref diffuse, ref ambient, 1f);
        AoActSysSetDrawStateEnable(false);
        this.dmLogoSonicActionCreate(work);
        IzFadeInitEasy(0U, 2U, 60f, true);
        work.func = new DMS_LOGO_SONIC_WORK._func_(this.dmLogoSonicFadeInWaitFunc);
    }

    public void dmLogoSonicMainFunc(MTS_TASK_TCB tcb)
    {
        DMS_LOGO_SONIC_WORK work = (DMS_LOGO_SONIC_WORK)tcb.work;
        if (AoSysIsShowPlatformUI())
        {
            if (IzFadeIsExe())
                IzFadeSetStopUpdate1Frame(null);
        }
        else
        {
            if (work.func != null)
                work.func(work);
            if (((int)work.flag & 1) != 0 && ((int)work.flag & 2) == 0 && (amTpIsTouchPush(0) || isBackKeyPressed()))
            {
                setBackKeyRequest(false);
                work.flag |= 2U;
                if (IzFadeIsEnd())
                    IzFadeInitEasy(0U, 3U, 10f, true);
                work.func = new DMS_LOGO_SONIC_WORK._func_(this.dmLogoSonicFadeOutWaitFunc);
            }
            if (((int)work.flag & 4) != 0)
            {
                mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(this.dmLogoSonicPreEndWait));
                work.timer = 0;
                return;
            }
        }
        float frame = 0.0f;
        if (!AoSysIsShowPlatformUI())
            frame = 1f;
        AoActSysSetDrawTaskPrio();
        for (int index = 0; index < 2; ++index)
        {
            AoActSetTexture(AoTexGetTexList(dm_logo_sonic_aos_tex[dm_logo_sonic_tex_id_tbl[index]]));
            AoActUpdate(work.act[index], frame);
            AoActDraw(work.act[index]);
        }
    }

    public void dmLogoSonicFadeInWaitFunc(DMS_LOGO_SONIC_WORK logo_work)
    {
        if (!IzFadeIsEnd())
            return;
        logo_work.func = new DMS_LOGO_SONIC_WORK._func_(this.dmLogoSonicDispWaitFunc);
        logo_work.timer = 0;
    }

    public void dmLogoSonicDispWaitFunc(DMS_LOGO_SONIC_WORK logo_work)
    {
        ++logo_work.timer;
        if (logo_work.timer >= 120)
        {
            logo_work.func = new DMS_LOGO_SONIC_WORK._func_(this.dmLogoSonicFadeOutWaitFunc);
            IzFadeInitEasy(0U, 3U, 60f, true);
            logo_work.flag &= 4294967294U;
        }
        else
        {
            if (logo_work.timer != 30)
                return;
            logo_work.flag |= 1U;
        }
    }

    public void dmLogoSonicFadeOutWaitFunc(DMS_LOGO_SONIC_WORK logo_work)
    {
        if (!IzFadeIsEnd())
            return;
        logo_work.flag |= 4U;
    }

    public void dmLogoSonicPreEndWait(MTS_TASK_TCB tcb)
    {
        DMS_LOGO_SONIC_WORK work = (DMS_LOGO_SONIC_WORK)tcb.work;
        ++work.timer;
        if (work.timer <= 2)
            return;
        this.dmLogoSonicActionDelete(work);
        this.DmLogoSonicFlush();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(this.dmLogoSonicFlushWaitFunc));
    }

    public void dmLogoSonicFlushWaitFunc(MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSonicFlushCheck())
            return;
        this.DmLogoSonicRelease();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(this.dmLogoSonicRelesehWaitFunc));
    }

    public void dmLogoSonicRelesehWaitFunc(MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSonicReleaseCheck())
            return;
        mtTaskClearTcb(tcb);
        SyChangeNextEvt();
    }

    public static void dmLogoSonicLoadPostFunc(DMS_LOGO_COM_LOAD_CONTEXT context)
    {
        dm_logo_sonic_data[context.no] = readAMBFile(context.fs_req);
    }

    public void dmLogoSonicDataBuildMain(MTS_TASK_TCB tcb)
    {
        bool flag = true;
        AOS_TEXTURE[] dmLogoSonicAosTex = dm_logo_sonic_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            if (!AoTexIsLoaded(dmLogoSonicAosTex[index]))
                flag = false;
        }
        if (!flag)
            return;
        mtTaskClearTcb(tcb);
        dm_logo_sonic_build_state = true;
    }

    public void dmLogoSonicDataBuildDest(MTS_TASK_TCB tcb)
    {
        dm_logo_sonic_build_tcb = null;
    }

    public void dmLogoSonicDataFlushMain(MTS_TASK_TCB tcb)
    {
        bool flag = true;
        AOS_TEXTURE[] dmLogoSonicAosTex = dm_logo_sonic_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            if (!AoTexIsReleased(dmLogoSonicAosTex[index]))
                flag = false;
        }
        if (!flag)
            return;
        dm_logo_sonic_aos_tex = null;
        mtTaskClearTcb(tcb);
        dm_logo_sonic_build_state = false;
    }

    public void dmLogoSonicDataFlushDest(MTS_TASK_TCB tcb)
    {
        dm_logo_sonic_flush_tcb = null;
    }

}