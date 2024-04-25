public partial class AppMain
{
    public class GMS_GMK_STOPPER_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public readonly int obj_type;
        public GMS_PLAYER_WORK ply_work;
        public short player_pass_timer;
        public int call_slot_id;
        public GSS_SND_SE_HANDLE se_handle;

        public GMS_GMK_STOPPER_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_STOPPER_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_GMK_STOPPER_WORK p)
        {
            return p.gmk_work;
        }
    }
}
