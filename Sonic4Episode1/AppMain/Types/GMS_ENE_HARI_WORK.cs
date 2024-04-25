public partial class AppMain
{
    public class GMS_ENE_HARI_WORK : IOBS_OBJECT_WORK
    {
        public GMS_ENEMY_3D_WORK ene_3d = new GMS_ENEMY_3D_WORK();
        public NNS_MATRIX jet_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public GMS_EFFECT_3DES_WORK efct_jet;

        public GMS_ENE_HARI_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_ENE_HARI_WORK work)
        {
            return work.ene_3d.ene_com.obj_work;
        }
    }
}
