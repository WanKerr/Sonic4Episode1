public partial class AppMain
{
    public class GMS_GMK_PWALL_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public int master_posy;
        public int wall_speed;
        public short wall_vibration;
        public short wall_effect_build_timer;
        public int wall_height;
        public int wall_brake;
        public int wall_timer;
        public bool ply_death;
        public bool stop_wall;
        public OBS_OBJECT_WORK efct_obj;
        public GSS_SND_SE_HANDLE se_handle;
        public uint mat_timer;
        public uint mat_timer_line;

        public GMS_GMK_PWALL_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_GMK_PWALL_WORK p)
        {
            return p.gmk_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_PWALL_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }
    }
}
