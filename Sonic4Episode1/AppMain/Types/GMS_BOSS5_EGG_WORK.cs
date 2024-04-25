public partial class AppMain
{
    public class GMS_BOSS5_EGG_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public MPP_VOID_GMS_BOSS5_EGG_WORK proc_update;
        public uint flag;
        public uint wait_timer;
        public int jump_dest_pos_x;

        public GMS_BOSS5_EGG_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
