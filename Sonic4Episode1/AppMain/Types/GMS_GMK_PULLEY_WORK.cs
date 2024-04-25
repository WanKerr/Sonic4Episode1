public partial class AppMain
{
    public class GMS_GMK_PULLEY_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public GSS_SND_SE_HANDLE se_handle;
        public GMS_EFFECT_3DES_WORK efct_work;

        public GMS_GMK_PULLEY_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }
    }
}
