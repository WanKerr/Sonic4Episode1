public partial class AppMain
{
    public class GMS_BOSS1_EGG_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public GMS_BOSS1_MGR_WORK mgr_work;
        public uint flag;
        public int egg_act_id;
        public MPP_VOID_GMS_BOSS1_EGG_WORK proc_update;

        public GMS_BOSS1_EGG_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
