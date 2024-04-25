public partial class AppMain
{
    public class GMS_GMK_NEEDLE_WORK : IOBS_OBJECT_WORK
    {
        public GMS_ENEMY_3D_WORK gmk_work;
        public int timer;
        public uint state;
        public int scale_timer;
        public uint scale_flag;
        public ushort needle_type;
        public ushort is_first_disp;
        public uint color;

        public GMS_GMK_NEEDLE_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }
    }
}
