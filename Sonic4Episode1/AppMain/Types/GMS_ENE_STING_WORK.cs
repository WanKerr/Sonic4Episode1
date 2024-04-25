public partial class AppMain
{
    public class GMS_ENE_STING_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_RECT_WORK search_rect_work = new OBS_RECT_WORK();
        public readonly NNS_MATRIX jet_r_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public readonly NNS_MATRIX jet_l_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public readonly NNS_MATRIX gun_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public readonly GMS_ENEMY_3D_WORK ene_3d_work;
        public int spd_dec;
        public int spd_dec_dist;
        public GMS_EFFECT_3DES_WORK efct_r_jet;
        public GMS_EFFECT_3DES_WORK efct_l_jet;
        public GMS_EFFECT_3DES_WORK efct_smoke;
        public int bullet_spd_x;
        public int bullet_spd_y;
        public short bullet_dir;

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d_work.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_ENE_STING_WORK work)
        {
            return work.ene_3d_work.ene_com.obj_work;
        }

        public GMS_ENE_STING_WORK()
        {
            this.ene_3d_work = new GMS_ENEMY_3D_WORK(this);
        }
    }
}
