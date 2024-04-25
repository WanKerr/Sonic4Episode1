public partial class AppMain
{
    public class GMS_ENE_HARO_WORK : IOBS_OBJECT_WORK
    {
        public VecFx32 vvv = new VecFx32();
        public VecFx32 vec = new VecFx32();
        public readonly GMS_ENE_NODE_MATRIX node_work = new GMS_ENE_NODE_MATRIX();
        public readonly GMS_ENEMY_3D_WORK ene_3d_work;
        public int spd_dec;
        public int spd_dec_dist;
        public int angle;
        public int angle_add;
        public int spd;
        public int timer;
        public int lighton;
        public int targetAngle;

        public GMS_ENE_HARO_WORK()
        {
            this.ene_3d_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d_work.ene_com.obj_work;
        }
    }
}
