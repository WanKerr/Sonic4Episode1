public partial class AppMain
{
    public class GMS_BOSS5_PART_ACT_INFO
    {
        public ushort act_id;
        public byte is_maintain;
        public byte is_repeat;
        public float mtn_spd;
        public int is_blend;
        public float blend_spd;
        public int is_merge_manual;

        public GMS_BOSS5_PART_ACT_INFO()
        {
            this.act_id = 0;
            this.is_maintain = this.is_repeat = 0;
            this.mtn_spd = this.blend_spd = 0.0f;
            this.is_blend = this.is_merge_manual = 0;
        }

        public GMS_BOSS5_PART_ACT_INFO(
          ushort actId,
          byte isMantain,
          byte isRepeat,
          float mspd,
          int blend,
          float bspd,
          int merge)
        {
            this.act_id = actId;
            this.is_maintain = isMantain;
            this.is_repeat = isRepeat;
            this.mtn_spd = mspd;
            this.is_blend = blend;
            this.blend_spd = bspd;
            this.is_merge_manual = merge;
        }
    }
}
