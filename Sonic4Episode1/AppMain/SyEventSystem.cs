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
    private static void SyInitEvtSys(
     AppMain.SYS_EVT_DATA[] evt_data,
     int evt_data_num,
     short start_evt_id,
     bool tcb_use,
     ushort pri,
     byte group)
    {
        if (tcb_use)
            AppMain.sy_evt_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.syEvtSys), (AppMain.GSF_TASK_PROCEDURE)null, 3U, (ushort)0, (uint)pri, (int)group, (AppMain.TaskWorkFactoryDelegate)null, "SY_EVT_SYS");
        AppMain.sy_evt_info = new AppMain.SYS_EVT_INFO();
        AppMain.sy_evt_info.evt_data = evt_data;
        AppMain.sy_evt_info.evt_data_num = evt_data_num;
        AppMain.sy_evt_info.cur_evt_data = evt_data[0];
        AppMain.sy_evt_info.next_evt_data = (AppMain.SYS_EVT_DATA)null;
        AppMain.sy_evt_info.cur_evt_id = (short)0;
        AppMain.sy_evt_info.req_evt_id = (short)0;
        AppMain.sy_evt_info.flag = 0U;
        AppMain.SyDecideEvt(start_evt_id);
        AppMain.SyChangeNextEvt();
    }

    private static void SyExitEvtSys()
    {
        if (AppMain.sy_evt_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.sy_evt_tcb);
        AppMain.sy_evt_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static AppMain.SYS_EVT_INFO SyGetEvtInfo()
    {
        return AppMain.sy_evt_info;
    }

    public static void SyDecideEvtCase(short evt_case)
    {
        if (AppMain.sy_evt_info.cur_evt_data.next_evt_id[(int)evt_case] == (short)0)
            evt_case = (short)0;
        AppMain.sy_evt_info.req_evt_case = evt_case;
        AppMain.SyDecideEvt(AppMain.sy_evt_info.cur_evt_data.next_evt_id[(int)evt_case]);
    }

    private static void SyDecideEvtIdCase(short req_id)
    {
        int index = 0;
        while (index < 8 && AppMain.sy_evt_info.cur_evt_data.next_evt_id[index] != (short)0 && (int)AppMain.sy_evt_info.cur_evt_data.next_evt_id[index] != (int)req_id)
            ++index;
        if (index >= 8 || AppMain.sy_evt_info.cur_evt_data.next_evt_id[index] == (short)0)
            req_id = AppMain.sy_evt_info.cur_evt_data.next_evt_id[0];
        AppMain.SyDecideEvt(req_id);
    }

    private static void SyDecideEvt(short req_id)
    {
        if (req_id <= (short)0 || AppMain.sy_evt_info.evt_data_num <= (int)req_id)
            return;
        AppMain.sy_evt_info.req_evt_id = req_id;
        AppMain.syDecideNextEvt();
    }

    public static void SyChangeNextEvt()
    {
        AppMain.SYS_EVT_DATA curEvtData = AppMain.sy_evt_info.cur_evt_data;
        if (AppMain.sy_evt_info.req_evt_id < (short)0)
            AppMain.sy_evt_info.req_evt_id = curEvtData.next_evt_id[0];
        AppMain.sy_evt_info.flag = 1U;
        AppMain.sy_evt_info.arg_size = 0U;
        AppMain.sy_evt_info.arg = new sbyte[8];
    }

    private static void SyChangeNextEvtArg(uint arg_size, sbyte[] arg_buf)
    {
        AppMain.SyChangeNextEvt();
        if (arg_size > 8U)
            arg_size = 8U;
        if (arg_size == 0U)
            return;
        Array.Copy((Array)AppMain.sy_evt_info.arg, (Array)arg_buf, (int)arg_size);
        arg_buf = new sbyte[(int)arg_size];
        AppMain.sy_evt_info.arg_size = arg_size;
    }

    private static bool SYM_CHECK_EVT_DATA_BRUNCH(AppMain.SYS_EVT_DATA _sy_evt_data)
    {
        return (short)0 < _sy_evt_data.next_evt_id[1];
    }

    private static void syEvtSys(AppMain.MTS_TASK_TCB tcb)
    {
        if (AppMain.sy_evt_info.flag != 1U)
            return;
        AppMain.SYS_EVT_DATA curEvtData1 = AppMain.sy_evt_info.cur_evt_data;
        if (curEvtData1.exit_func != null)
            curEvtData1.exit_func();
        if (curEvtData1.exit_sys_func != null)
            curEvtData1.exit_sys_func();
        AppMain.sy_evt_info.old_evt_id = AppMain.sy_evt_info.cur_evt_id;
        AppMain.sy_evt_info.cur_evt_id = AppMain.sy_evt_info.req_evt_id;
        AppMain.sy_evt_info.cur_evt_data = AppMain.sy_evt_info.evt_data[(int)AppMain.sy_evt_info.cur_evt_id];
        AppMain.SYS_EVT_DATA curEvtData2 = AppMain.sy_evt_info.cur_evt_data;
        AppMain.sy_evt_info.req_evt_id = (short)-1;
        if (!AppMain.SYM_CHECK_EVT_DATA_BRUNCH(curEvtData2))
        {
            AppMain.sy_evt_info.req_evt_id = curEvtData2.next_evt_id[0];
            AppMain.syDecideNextEvt();
        }
        AppMain.syEvtSysOvlCallBack();
    }

    private static void syEvtSysOvlCallBack()
    {
        AppMain.SYS_EVT_DATA curEvtData = AppMain.sy_evt_info.cur_evt_data;
        AppMain.sy_evt_info.flag = 0U;
        if (curEvtData.init_sys_func != null)
            curEvtData.init_sys_func();
        if (curEvtData.init_func == null)
            return;
        sbyte[] numArray = (sbyte[])null;
        if (AppMain.sy_evt_info.arg_size != 0U)
            numArray = AppMain.sy_evt_info.arg;
        curEvtData.init_func((object)numArray);
    }

    private static void syDecideNextEvt()
    {
        if (AppMain.sy_evt_info.req_evt_id < (short)0)
            AppMain.sy_evt_info.req_evt_id = AppMain.sy_evt_info.cur_evt_data.next_evt_id[0];
        AppMain.sy_evt_info.next_evt_data = AppMain.sy_evt_info.evt_data[(int)AppMain.sy_evt_info.req_evt_id];
    }

}