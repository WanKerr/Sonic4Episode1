public partial class AppMain
{
    public class GMS_BOSS5_ARM_ANIM_INFO
    {
        public GMS_BOSS5_ARM_PART_ANIM_INFO[] part_anim_info = New<GMS_BOSS5_ARM_PART_ANIM_INFO>(3);
        public uint wait_time;
        public float slerp_inc_rate;

        public GMS_BOSS5_ARM_ANIM_INFO()
        {
        }

        public GMS_BOSS5_ARM_ANIM_INFO(
          uint wait,
          float sincrate,
          GMS_BOSS5_ARM_PART_ANIM_INFO[] part)
        {
            this.wait_time = wait;
            this.slerp_inc_rate = sincrate;
            this.part_anim_info = part;
        }
    }
}
