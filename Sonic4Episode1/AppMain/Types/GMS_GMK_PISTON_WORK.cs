public partial class AppMain
{
    public class GMS_GMK_PISTON_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public uint obj_type;
        public ushort piston_vect;
        public int stroke_spd;
        public int timer_dec;
        public int timer_set_move;
        public int timer_set_wait_upper;
        public int timer_set_wait_lower;
        public bool efct_di;

        public GMS_GMK_PISTON_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_PISTON_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_GMK_PISTON_WORK p)
        {
            return p.gmk_work;
        }
    }
}
