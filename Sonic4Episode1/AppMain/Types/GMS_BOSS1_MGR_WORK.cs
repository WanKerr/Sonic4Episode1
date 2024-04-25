public partial class AppMain
{
    public class GMS_BOSS1_MGR_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public int life;
        public uint flag;
        public int obj_create_cnt;
        public GMS_BOSS1_BODY_WORK body_work;

        public GMS_BOSS1_MGR_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
