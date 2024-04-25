using System;

public partial class AppMain
{
    private static void SyInitEvtSys(
     SYS_EVT_DATA[] evt_data,
     int evt_data_num,
     short start_evt_id,
     bool tcb_use,
     ushort pri,
     byte group)
    {
        if (tcb_use)
            sy_evt_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(syEvtSys), null, 3U, 0, pri, group, null, "SY_EVT_SYS");
        sy_evt_info = new SYS_EVT_INFO();
        sy_evt_info.evt_data = evt_data;
        sy_evt_info.evt_data_num = evt_data_num;
        sy_evt_info.cur_evt_data = evt_data[0];
        sy_evt_info.next_evt_data = null;
        sy_evt_info.cur_evt_id = 0;
        sy_evt_info.req_evt_id = 0;
        sy_evt_info.flag = 0U;
        SyDecideEvt(start_evt_id);
        SyChangeNextEvt();
    }

    private static void SyExitEvtSys()
    {
        if (sy_evt_tcb == null)
            return;
        mtTaskClearTcb(sy_evt_tcb);
        sy_evt_tcb = null;
    }

    private static SYS_EVT_INFO SyGetEvtInfo()
    {
        return sy_evt_info;
    }

    public static void SyDecideEvtCase(short evt_case)
    {
        if (sy_evt_info.cur_evt_data.next_evt_id[evt_case] == 0)
            evt_case = 0;
        sy_evt_info.req_evt_case = evt_case;
        SyDecideEvt(sy_evt_info.cur_evt_data.next_evt_id[evt_case]);
    }

    private static void SyDecideEvtIdCase(short req_id)
    {
        int index = 0;
        while (index < 8 && sy_evt_info.cur_evt_data.next_evt_id[index] != 0 && sy_evt_info.cur_evt_data.next_evt_id[index] != req_id)
            ++index;
        if (index >= 8 || sy_evt_info.cur_evt_data.next_evt_id[index] == 0)
            req_id = sy_evt_info.cur_evt_data.next_evt_id[0];
        SyDecideEvt(req_id);
    }

    private static void SyDecideEvt(short req_id)
    {
        if (req_id <= 0 || sy_evt_info.evt_data_num <= req_id)
            return;
        sy_evt_info.req_evt_id = req_id;
        syDecideNextEvt();
    }

    public static void SyChangeNextEvt()
    {
        SYS_EVT_DATA curEvtData = sy_evt_info.cur_evt_data;
        if (sy_evt_info.req_evt_id < 0)
            sy_evt_info.req_evt_id = curEvtData.next_evt_id[0];
        sy_evt_info.flag = 1U;
        sy_evt_info.arg_size = 0U;
        sy_evt_info.arg = new sbyte[8];
    }

    private static void SyChangeNextEvtArg(uint arg_size, sbyte[] arg_buf)
    {
        SyChangeNextEvt();
        if (arg_size > 8U)
            arg_size = 8U;
        if (arg_size == 0U)
            return;
        Array.Copy(sy_evt_info.arg, arg_buf, (int)arg_size);
        arg_buf = new sbyte[(int)arg_size];
        sy_evt_info.arg_size = arg_size;
    }

    private static bool SYM_CHECK_EVT_DATA_BRUNCH(SYS_EVT_DATA _sy_evt_data)
    {
        return 0 < _sy_evt_data.next_evt_id[1];
    }

    private static void syEvtSys(MTS_TASK_TCB tcb)
    {
        if (sy_evt_info.flag != 1U)
            return;
        SYS_EVT_DATA curEvtData1 = sy_evt_info.cur_evt_data;
        if (curEvtData1.exit_func != null)
            curEvtData1.exit_func();
        if (curEvtData1.exit_sys_func != null)
            curEvtData1.exit_sys_func();
        sy_evt_info.old_evt_id = sy_evt_info.cur_evt_id;
        sy_evt_info.cur_evt_id = sy_evt_info.req_evt_id;
        sy_evt_info.cur_evt_data = sy_evt_info.evt_data[sy_evt_info.cur_evt_id];
        SYS_EVT_DATA curEvtData2 = sy_evt_info.cur_evt_data;
        sy_evt_info.req_evt_id = -1;
        if (!SYM_CHECK_EVT_DATA_BRUNCH(curEvtData2))
        {
            sy_evt_info.req_evt_id = curEvtData2.next_evt_id[0];
            syDecideNextEvt();
        }
        syEvtSysOvlCallBack();
    }

    private static void syEvtSysOvlCallBack()
    {
        SYS_EVT_DATA curEvtData = sy_evt_info.cur_evt_data;
        sy_evt_info.flag = 0U;
        if (curEvtData.init_sys_func != null)
            curEvtData.init_sys_func();
        if (curEvtData.init_func == null)
            return;
        sbyte[] numArray = null;
        if (sy_evt_info.arg_size != 0U)
            numArray = sy_evt_info.arg;
        curEvtData.init_func(numArray);
    }

    private static void syDecideNextEvt()
    {
        if (sy_evt_info.req_evt_id < 0)
            sy_evt_info.req_evt_id = sy_evt_info.cur_evt_data.next_evt_id[0];
        sy_evt_info.next_evt_data = sy_evt_info.evt_data[sy_evt_info.req_evt_id];
    }

}