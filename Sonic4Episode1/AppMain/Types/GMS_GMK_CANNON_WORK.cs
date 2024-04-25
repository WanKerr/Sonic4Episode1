public partial class AppMain
{
    public class GMS_GMK_CANNON_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public GMS_PLAYER_WORK ply_work;
        public bool hitpass;
        public short shoot_after;
        public short angle_set;
        public short angle_now;
        public int cannon_power;

        public GMS_GMK_CANNON_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_GMK_CANNON_WORK p)
        {
            return p.gmk_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_CANNON_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }
    }
}
