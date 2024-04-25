public partial class AppMain
{
    public class GMS_GMK_SEESAW_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public ushort seesaw_id;
        public short initial_tilt;
        public short tilt;
        public short tilt_d;
        public short tilt_acc;
        public short tilt_timer;
        public short tilt_se_timer;
        public int hold_x;
        public int hold_y;
        public long player_distance;
        public int player_speed;
        public GMS_PLAYER_WORK ply_work;

        public GMS_GMK_SEESAW_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_SEESAW_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_GMK_SEESAW_WORK p)
        {
            return p.gmk_work;
        }
    }
}
