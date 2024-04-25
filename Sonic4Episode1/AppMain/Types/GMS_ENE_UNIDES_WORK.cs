public partial class AppMain
{
    public class GMS_ENE_UNIDES_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK ene_3d_work;
        public int spd_dec;
        public int spd_dec_dist;
        public int rot_x;
        public int rot_y;
        public int rot_z;
        public float len;
        public int num;
        public int attack_first;
        public int attack;
        public int zoom_now;
        public int stop;
        public int timer;
        public float zoom;

        public GMS_ENE_UNIDES_WORK()
        {
            this.ene_3d_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d_work.ene_com.obj_work;
        }
    }
}
