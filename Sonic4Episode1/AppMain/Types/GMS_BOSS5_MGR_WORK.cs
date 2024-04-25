public partial class AppMain
{
    public class GMS_BOSS5_MGR_WORK : IOBS_OBJECT_WORK
    {
        public readonly short[] save_camera_offset = new short[2];
        public readonly GMS_BOSS5_EXPL_WORK small_expl_work = new GMS_BOSS5_EXPL_WORK();
        public readonly GMS_BOSS5_EXPL_WORK big_expl_work = new GMS_BOSS5_EXPL_WORK();
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public int life;
        public MPP_VOID_GMS_BOSS5_MGR_WORK proc_update;
        public uint flag;
        public uint wait_timer;
        public int ply_demo_run_dest_x;
        public int alarm_level;
        public GMS_BOSS5_BODY_WORK body_work;

        public GMS_BOSS5_MGR_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_BOSS5_MGR_WORK p)
        {
            return p.ene_3d.ene_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
