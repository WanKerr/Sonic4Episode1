public partial class AppMain
{
    public class GMS_BOSS2_MGR_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public int life;
        public uint flag;
        public GMS_BOSS2_BODY_WORK body_work;
        public int obj_create_count;

        public GMS_BOSS2_MGR_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_BOSS2_MGR_WORK p)
        {
            return p.ene_3d.ene_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
