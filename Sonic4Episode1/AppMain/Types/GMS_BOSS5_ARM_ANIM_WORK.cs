public partial class AppMain
{
    public class GMS_BOSS5_ARM_ANIM_WORK
    {
        public NNS_QUATERNION[] start_quat = New<NNS_QUATERNION>(3);
        public NNS_QUATERNION[] end_quat = New<NNS_QUATERNION>(3);
        public int is_anim;
        public uint anim_wait_timer;
        public float cur_rate;
        public float rate_add;
    }
}
