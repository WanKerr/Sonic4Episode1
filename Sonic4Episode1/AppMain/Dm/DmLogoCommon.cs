using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    private AppMain.MTS_TASK_TCB DmLogoComLoadFileCreate(
        AppMain.Reference<AppMain.MTS_TASK_TCB> load_tcb_addr)
    {
        AppMain.MTS_TASK_TCB mtsTaskTcb = AppMain.MTM_TASK_MAKE_TCB((AppMain.GSF_TASK_PROCEDURE)null, new AppMain.GSF_TASK_PROCEDURE(this.dmLogoComDataLoadDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_LOGO_COM_LOAD_WORK()), "DM_LC_LOAD");
        AppMain.DMS_LOGO_COM_LOAD_WORK work = (AppMain.DMS_LOGO_COM_LOAD_WORK)mtsTaskTcb.work;
        load_tcb_addr.Target = mtsTaskTcb;
        work.load_tcb_addr = load_tcb_addr;
        return mtsTaskTcb;
    }

    private void DmLogoComLoadFileReg(
      AppMain.Reference<AppMain.MTS_TASK_TCB> tcb,
      AppMain.DMS_LOGO_COM_LOAD_FILE_INFO[] file_info,
      int file_num)
    {
        AppMain.DMS_LOGO_COM_LOAD_WORK work = (AppMain.DMS_LOGO_COM_LOAD_WORK)tcb.Target.work;
        AppMain.ArrayPointer<AppMain.DMS_LOGO_COM_LOAD_CONTEXT> arrayPointer = new AppMain.ArrayPointer<AppMain.DMS_LOGO_COM_LOAD_CONTEXT>(work.context, work.context_num);
        int index = 0;
        while (index < file_num && work.context_num < 10)
        {
            ((AppMain.DMS_LOGO_COM_LOAD_CONTEXT)~arrayPointer).no = work.context_num;
            ((AppMain.DMS_LOGO_COM_LOAD_CONTEXT)~arrayPointer).file_info = file_info[index];
            int num = (int)this.DmLogoComLoadFile((AppMain.DMS_LOGO_COM_LOAD_CONTEXT)arrayPointer);
            ++index;
            ++work.context_num;
            ++arrayPointer;
        }
    }

    private void DmLogoComLoadFileStart(AppMain.Reference<AppMain.MTS_TASK_TCB> tcb)
    {
        AppMain.mtTaskChangeTcbProcedure(tcb.Target, new AppMain.GSF_TASK_PROCEDURE(this.dmLogoComDataLoadMain));
    }

    private AppMain.DME_LOGO_COM_LOAD_STATE DmLogoComLoadFile(
      AppMain.DMS_LOGO_COM_LOAD_CONTEXT context)
    {
        switch (context.state)
        {
            case AppMain.DME_LOGO_COM_LOAD_STATE.DMD_LOGO_COM_LOAD_STATE_LOAD_WAIT:
                context.file_path_buf = context.file_info.file_path;
                context.fs_req = AppMain.amFsReadBackground(context.file_path_buf);
                if (context.fs_req != null)
                {
                    context.state = AppMain.DME_LOGO_COM_LOAD_STATE.DMD_LOGO_COM_LOAD_STATE_LOADING;
                    break;
                }
                break;
            case AppMain.DME_LOGO_COM_LOAD_STATE.DMD_LOGO_COM_LOAD_STATE_LOADING:
                if (AppMain.amFsIsComplete(context.fs_req))
                {
                    if (context.file_info.post_func != null)
                        context.file_info.post_func(context);
                    AppMain.amFsClearRequest(context.fs_req);
                    context.fs_req = (AppMain.AMS_FS)null;
                    context.state = AppMain.DME_LOGO_COM_LOAD_STATE.DMD_LOGO_COM_LOAD_STATE_COMPLETE;
                    break;
                }
                break;
        }
        return context.state;
    }

    private void dmLogoComDataLoadMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_LOGO_COM_LOAD_WORK work = (AppMain.DMS_LOGO_COM_LOAD_WORK)tcb.work;
        AppMain.ArrayPointer<AppMain.DMS_LOGO_COM_LOAD_CONTEXT> context = (AppMain.ArrayPointer<AppMain.DMS_LOGO_COM_LOAD_CONTEXT>)work.context;
        int num = 0;
        while (num < work.context_num)
        {
            if (this.DmLogoComLoadFile((AppMain.DMS_LOGO_COM_LOAD_CONTEXT)context) != AppMain.DME_LOGO_COM_LOAD_STATE.DMD_LOGO_COM_LOAD_STATE_COMPLETE)
                return;
            ++num;
            ++context;
        }
        AppMain.mtTaskClearTcb(tcb);
    }

    private void dmLogoComDataLoadDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_LOGO_COM_LOAD_WORK work = (AppMain.DMS_LOGO_COM_LOAD_WORK)tcb.work;
        if (work.load_tcb_addr == null || work.load_tcb_addr.Target != tcb)
            return;
        work.load_tcb_addr.Target = (AppMain.MTS_TASK_TCB)null;
    }
}