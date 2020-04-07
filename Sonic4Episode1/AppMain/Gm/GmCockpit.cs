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
    private static AppMain.OBS_OBJECT_WORK GMM_COCKPIT_CREATE_WORK(
      AppMain.TaskWorkFactoryDelegate work_size,
      AppMain.OBS_OBJECT_WORK parent_obj,
      ushort sort_prio,
      string name)
    {
        return AppMain.GmCockpitCreateWork(work_size, parent_obj, sort_prio, name);
    }

    private static AppMain.OBS_OBJECT_WORK GmCockpitCreateWork(
      AppMain.TaskWorkFactoryDelegate work_size,
      AppMain.OBS_OBJECT_WORK parent_obj,
      ushort sort_prio,
      string name)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)(18432U + (uint)sort_prio), (byte)5, (byte)0, (byte)0, work_size, name);
        if (obsObjectWork == null)
        {
            AppMain.mppAssertNotImpl();
            return (AppMain.OBS_OBJECT_WORK)null;
        }
        obsObjectWork.obj_type = (ushort)6;
        obsObjectWork.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.ObjDrawActionSummary);
        obsObjectWork.ppOutSub = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppIn = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppActCall = (AppMain.OBS_OBJECT_WORK_Delegate2)null;
        obsObjectWork.ppRec = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppLast = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppViewCheck = (AppMain.OBS_OBJECT_WORK_Delegate3)null;
        obsObjectWork.spd_fall = 672;
        obsObjectWork.spd_fall_max = 61440;
        if (parent_obj != null)
        {
            obsObjectWork.parent_obj = parent_obj;
            obsObjectWork.pos.x = parent_obj.pos.x;
            obsObjectWork.pos.y = parent_obj.pos.y;
            obsObjectWork.pos.z = parent_obj.pos.z;
        }
        obsObjectWork.flag |= 18U;
        obsObjectWork.move_flag |= 256U;
        return obsObjectWork;
    }

}