public partial class AppMain
{
    public class GMS_BOSS5_LDPART_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_COLLISION_WORK col_work = new OBS_COLLISION_WORK();
        public readonly int[] vib_ofst = new int[2];
        public readonly int[] pivot_parent_ofst = new int[2];
        public NNS_QUATERNION rot_diff_quat = new NNS_QUATERNION();
        public NNS_QUATERNION cur_rot_quat = new NNS_QUATERNION();
        public readonly GMS_EFFECT_3DNN_WORK efct_3d;
        public MPP_VOID_GMS_BOSS5_LDPART_WORK proc_update;
        public int vib_cnt;
        public int part_index;
        public uint wait_timer;
        public uint brk_glass_cnt;

        public GMS_BOSS5_LDPART_WORK()
        {
            this.efct_3d = new GMS_EFFECT_3DNN_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.efct_3d.efct_com.obj_work;
        }
    }
}
