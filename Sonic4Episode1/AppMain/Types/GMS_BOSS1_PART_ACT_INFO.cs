public partial class AppMain
{
    public class GMS_BOSS1_PART_ACT_INFO
    {
        public ushort act_id;
        public byte is_maintain;
        public byte is_repeat;
        public float mtn_spd;
        public bool is_blend;
        public float blend_spd;
        public bool is_merge_manual;

        public GMS_BOSS1_PART_ACT_INFO(
          ushort _act_id,
          bool _is_maintain,
          bool _is_repeat,
          float _mtn_spd,
          bool _is_blend,
          float _blend_spd,
          bool _is_merge_manual)
        {
            this.act_id = _act_id;
            this.is_maintain = _is_maintain ? (byte)1 : (byte)0;
            this.is_repeat = _is_repeat ? (byte)1 : (byte)0;
            this.mtn_spd = _mtn_spd;
            this.is_blend = _is_blend;
            this.blend_spd = _blend_spd;
            this.is_merge_manual = _is_merge_manual;
        }
    }
}
