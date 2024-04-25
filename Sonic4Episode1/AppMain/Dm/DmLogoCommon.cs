public partial class AppMain
{
    private MTS_TASK_TCB DmLogoComLoadFileCreate(
        Reference<MTS_TASK_TCB> load_tcb_addr)
    {
        MTS_TASK_TCB mtsTaskTcb = MTM_TASK_MAKE_TCB(null, new GSF_TASK_PROCEDURE(this.dmLogoComDataLoadDest), 0U, ushort.MaxValue, 4096U, 0, () => new DMS_LOGO_COM_LOAD_WORK(), "DM_LC_LOAD");
        DMS_LOGO_COM_LOAD_WORK work = (DMS_LOGO_COM_LOAD_WORK)mtsTaskTcb.work;
        load_tcb_addr.Target = mtsTaskTcb;
        work.load_tcb_addr = load_tcb_addr;
        return mtsTaskTcb;
    }

    private void DmLogoComLoadFileReg(
      Reference<MTS_TASK_TCB> tcb,
      DMS_LOGO_COM_LOAD_FILE_INFO[] file_info,
      int file_num)
    {
        DMS_LOGO_COM_LOAD_WORK work = (DMS_LOGO_COM_LOAD_WORK)tcb.Target.work;
        ArrayPointer<DMS_LOGO_COM_LOAD_CONTEXT> arrayPointer = new ArrayPointer<DMS_LOGO_COM_LOAD_CONTEXT>(work.context, work.context_num);
        int index = 0;
        while (index < file_num && work.context_num < 10)
        {
            (~arrayPointer).no = work.context_num;
            (~arrayPointer).file_info = file_info[index];
            int num = (int)this.DmLogoComLoadFile(arrayPointer);
            ++index;
            ++work.context_num;
            ++arrayPointer;
        }
    }

    private void DmLogoComLoadFileStart(Reference<MTS_TASK_TCB> tcb)
    {
        mtTaskChangeTcbProcedure(tcb.Target, new GSF_TASK_PROCEDURE(this.dmLogoComDataLoadMain));
    }

    private DME_LOGO_COM_LOAD_STATE DmLogoComLoadFile(
      DMS_LOGO_COM_LOAD_CONTEXT context)
    {
        switch (context.state)
        {
            case DME_LOGO_COM_LOAD_STATE.DMD_LOGO_COM_LOAD_STATE_LOAD_WAIT:
                context.file_path_buf = context.file_info.file_path;
                context.fs_req = amFsReadBackground(context.file_path_buf);
                if (context.fs_req != null)
                {
                    context.state = DME_LOGO_COM_LOAD_STATE.DMD_LOGO_COM_LOAD_STATE_LOADING;
                    break;
                }
                break;
            case DME_LOGO_COM_LOAD_STATE.DMD_LOGO_COM_LOAD_STATE_LOADING:
                if (amFsIsComplete(context.fs_req))
                {
                    if (context.file_info.post_func != null)
                        context.file_info.post_func(context);
                    amFsClearRequest(context.fs_req);
                    context.fs_req = null;
                    context.state = DME_LOGO_COM_LOAD_STATE.DMD_LOGO_COM_LOAD_STATE_COMPLETE;
                    break;
                }
                break;
        }
        return context.state;
    }

    private void dmLogoComDataLoadMain(MTS_TASK_TCB tcb)
    {
        DMS_LOGO_COM_LOAD_WORK work = (DMS_LOGO_COM_LOAD_WORK)tcb.work;
        ArrayPointer<DMS_LOGO_COM_LOAD_CONTEXT> context = work.context;
        int num = 0;
        while (num < work.context_num)
        {
            if (this.DmLogoComLoadFile(context) != DME_LOGO_COM_LOAD_STATE.DMD_LOGO_COM_LOAD_STATE_COMPLETE)
                return;
            ++num;
            ++context;
        }
        mtTaskClearTcb(tcb);
    }

    private void dmLogoComDataLoadDest(MTS_TASK_TCB tcb)
    {
        DMS_LOGO_COM_LOAD_WORK work = (DMS_LOGO_COM_LOAD_WORK)tcb.work;
        if (work.load_tcb_addr == null || work.load_tcb_addr.Target != tcb)
            return;
        work.load_tcb_addr.Target = null;
    }
}