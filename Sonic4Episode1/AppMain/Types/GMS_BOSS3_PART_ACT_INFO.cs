public partial class AppMain
{
    public class GMS_BOSS3_PART_ACT_INFO
    {
        public ushort mtn_id;
        public byte is_maintain;
        public byte is_repeat;
        public float mtn_spd;
        public int is_blend;
        public float blend_spd;
        public int is_merge_manual;

        public GMS_BOSS3_PART_ACT_INFO(
          ushort id,
          byte mntain,
          byte isrep,
          float spd,
          int blnd,
          float bspd,
          int mergeman)
        {
            this.mtn_id = id;
            this.is_maintain = mntain;
            this.is_repeat = isrep;
            this.mtn_spd = spd;
            this.is_blend = blnd;
            this.blend_spd = bspd;
            this.is_merge_manual = mergeman;
        }
    }
}
