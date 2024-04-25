public partial class AppMain
{
    public class GMS_GMK_SPEAR_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public uint obj_type;
        public ushort vect;
        public int stroke_spd;
        public int timer_dec;
        public int timer_set_move;
        public short timer_set_wait_upper;
        public short timer_set_wait_lower;

        public GMS_GMK_SPEAR_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }
    }
}
