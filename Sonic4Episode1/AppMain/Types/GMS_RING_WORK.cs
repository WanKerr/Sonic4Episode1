public partial class AppMain
{
    public class GMS_RING_WORK : IClearable
    {
        public VecFx32 pos = new VecFx32();
        public VecFx32 scale = new VecFx32();
        public int spd_x;
        public int spd_y;
        public short timer;
        public ushort flag;
        public GMS_EVE_RECORD_RING eve_rec;
        public GMS_RING_WORK pre_ring;
        public GMS_RING_WORK post_ring;
        public OBS_OBJECT_WORK duct_obj;

        public void Clear()
        {
            this.pos.Clear();
            this.scale.Clear();
            this.spd_x = 0;
            this.spd_y = 0;
            this.timer = 0;
            this.flag = 0;
            this.eve_rec = null;
            this.pre_ring = null;
            this.post_ring = null;
            this.duct_obj = null;
        }
    }
}
