public partial class AppMain
{
    public class GMS_ENE_NODE_MATRIX
    {
        public char[] _id = new char[8];
        public readonly GMS_BS_CMN_BMCB_MGR mtn_mgr = new GMS_BS_CMN_BMCB_MGR();
        public readonly GMS_BS_CMN_SNM_WORK snm_work = new GMS_BS_CMN_SNM_WORK();
        public int[] work = new int[32];
        public int initCount;
        public int useCount;
        public OBS_OBJECT_WORK obj_work;
    }
}
