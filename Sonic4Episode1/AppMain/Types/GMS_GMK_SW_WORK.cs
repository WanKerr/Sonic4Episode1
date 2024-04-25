public partial class AppMain
{
    public class GMS_GMK_SW_WORK : IOBS_OBJECT_WORK
    {
        public OBS_ACTION3D_NN_WORK obj_3d_base = new OBS_ACTION3D_NN_WORK();
        public GMS_ENEMY_3D_WORK gmk_work;
        public int top_pos_y;
        public uint id;
        public int time;

        public GMS_GMK_SW_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_SW_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }
    }
}
