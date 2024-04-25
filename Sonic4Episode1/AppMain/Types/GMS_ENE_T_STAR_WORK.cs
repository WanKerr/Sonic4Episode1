public partial class AppMain
{
    public class GMS_ENE_T_STAR_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENE_NODE_MATRIX node_work = new GMS_ENE_NODE_MATRIX();
        public readonly GMS_ENEMY_3D_WORK ene_3d_work;
        public int spd_dec;
        public int spd_dec_dist;
        public int timer;
        public float fSpd;
        public ushort rotate;

        public GMS_ENE_T_STAR_WORK()
        {
            this.ene_3d_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d_work.ene_com.obj_work;
        }
    }
}
