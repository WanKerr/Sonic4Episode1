public partial class AppMain
{
    public class GMS_GMK_BUMPER_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK obj_3d_parts = new OBS_ACTION3D_NN_WORK();
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public GSS_SND_SE_HANDLE se_handle;

        public GMS_GMK_BUMPER_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
