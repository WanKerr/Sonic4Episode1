public partial class AppMain
{
    public class GMS_GMK_TRUCK_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK obj_3d_tire = new OBS_ACTION3D_NN_WORK();
        public readonly NNS_MATRIX tire_pos_f = new NNS_MATRIX();
        public readonly NNS_MATRIX tire_pos_b = new NNS_MATRIX();
        public readonly NNS_MATRIX light_pos = new NNS_MATRIX();
        public readonly NNS_VECTOR trans_r = new NNS_VECTOR();
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public GMS_GMK_TRUCK_WORK seq;
        public GMS_PLAYER_WORK target_player;
        public int tire_spd_for_dir;
        public int tire_dir_spd;
        public ushort tire_dir;
        public ushort slope_z_dir;
        public ushort slope_f_y_dir;
        public ushort slope_f_z_dir;
        public GMS_EFFECT_3DES_WORK efct_f_spark;
        public GMS_EFFECT_3DES_WORK efct_b_spark;
        public GSS_SND_SE_HANDLE h_snd_lorry;

        public GMS_GMK_TRUCK_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_TRUCK_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }
    }
}
