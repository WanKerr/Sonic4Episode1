public partial class AppMain
{
    private class gmFixVirtualPadOutClassGMD_FIX_MGR_FLAG_HIDE_VIRTUAL_PAD_PART_SUPER_SONIC
    {
        public static void OutFunc(OBS_OBJECT_WORK obj_work)
        {
            if (gm_fix_tcb == null || (512 & (int)((GMS_FIX_MGR_WORK)gm_fix_tcb.work).flag) != 0)
                return;
            ObjDrawActionSummary(obj_work);
        }
    }
}
