public partial class AppMain
{
    private static OBS_OBJECT_WORK GMM_COCKPIT_CREATE_WORK(
      TaskWorkFactoryDelegate work_size,
      OBS_OBJECT_WORK parent_obj,
      ushort sort_prio,
      string name)
    {
        return GmCockpitCreateWork(work_size, parent_obj, sort_prio, name);
    }

    private static OBS_OBJECT_WORK GmCockpitCreateWork(
      TaskWorkFactoryDelegate work_size,
      OBS_OBJECT_WORK parent_obj,
      ushort sort_prio,
      string name)
    {
        OBS_OBJECT_WORK obsObjectWork = OBM_OBJECT_TASK_DETAIL_INIT((ushort)(18432U + sort_prio), 5, 0, 0, work_size, name);
        if (obsObjectWork == null)
        {
            mppAssertNotImpl();
            return null;
        }
        obsObjectWork.obj_type = 6;
        obsObjectWork.ppOut = new MPP_VOID_OBS_OBJECT_WORK(ObjDrawActionSummary);
        obsObjectWork.ppOutSub = null;
        obsObjectWork.ppIn = null;
        obsObjectWork.ppMove = null;
        obsObjectWork.ppActCall = null;
        obsObjectWork.ppRec = null;
        obsObjectWork.ppLast = null;
        obsObjectWork.ppViewCheck = null;
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