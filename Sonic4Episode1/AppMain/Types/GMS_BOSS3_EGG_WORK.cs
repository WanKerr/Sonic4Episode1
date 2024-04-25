public partial class AppMain
{
    public class GMS_BOSS3_EGG_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public int egg_action_id;
        public uint flag;
        public GMF_BOSS3_EGG_STATE_FUNC proc_update;

        public GMS_BOSS3_EGG_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_BOSS3_EGG_WORK p)
        {
            return p.ene_3d.ene_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
