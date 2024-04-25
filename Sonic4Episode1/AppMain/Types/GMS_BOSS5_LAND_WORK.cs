public partial class AppMain
{
    public class GMS_BOSS5_LAND_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public MPP_VOID_GMS_BOSS5_LAND_WORK proc_update;
        public GMS_BOSS5_MGR_WORK mgr_work;
        public uint flag;
        public uint wait_timer;

        public GMS_BOSS5_LAND_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator GMS_ENEMY_COM_WORK(GMS_BOSS5_LAND_WORK p)
        {
            return p.ene_3d.ene_com;
        }

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_BOSS5_LAND_WORK p)
        {
            return p.ene_3d;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
