public partial class AppMain
{
    public class GMS_BOSS5_ARM_PART_ANIM_INFO
    {
        public readonly NNS_ROTATE_A32 start_rot = new NNS_ROTATE_A32();
        public readonly NNS_ROTATE_A32 end_rot = new NNS_ROTATE_A32();
        public int is_anim;

        public GMS_BOSS5_ARM_PART_ANIM_INFO()
        {
        }

        public GMS_BOSS5_ARM_PART_ANIM_INFO(
          int anim,
          NNS_ROTATE_A32 rot,
          NNS_ROTATE_A32 erot)
        {
            this.is_anim = anim;
            this.start_rot = rot;
            this.end_rot = erot;
        }
    }
}
