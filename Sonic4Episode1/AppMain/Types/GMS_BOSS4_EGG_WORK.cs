public partial class AppMain
{
    public class GMS_BOSS4_EGG_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_BOSS4_DIRECTION dir_work = new GMS_BOSS4_DIRECTION();
        public readonly GMS_BOSS4_NODE_MATRIX node_work = new GMS_BOSS4_NODE_MATRIX();
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public uint flag;
        public int egg_act_id;
        public MPP_VOID_GMS_BOSS4_EGG_WORK proc_update;

        public GMS_BOSS4_EGG_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_BOSS4_EGG_WORK work)
        {
            return work.ene_3d.ene_com.obj_work;
        }

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_BOSS4_EGG_WORK work)
        {
            return work.ene_3d;
        }
    }
}
