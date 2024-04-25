public partial class AppMain
{
    public class GMS_GMK_SLOT_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_GMK_SLOT_REEL_STATUS_WORK[] reel_status = New<GMS_GMK_SLOT_REEL_STATUS_WORK>(3);
        public readonly short[] prob = new short[5];
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public int current_reel;
        public int slot_id;
        public int timer;
        public int timer_next;
        public int timer_meoshi_wait;
        public int slot_step;
        public int slot_se;
        public int slot_se_timer;
        public int suberi_cnt;
        public int suberi_input;
        public short lotresult;
        public int freestop;
        public int ppos_x;
        public int ppos_y;

        public GMS_GMK_SLOT_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_SLOT_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_GMK_SLOT_WORK p)
        {
            return p.gmk_work;
        }
    }
}
