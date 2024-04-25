public partial class AppMain
{
    public class GMS_GMK_SWWALL_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK[] obj_3d_opt = New<OBS_ACTION3D_NN_WORK>(2);
        public byte[] col_dir_buf = new byte[128];
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public uint id;
        public int wall_size;
        public int wall_draw_size;
        public ushort wall_type;
        public ushort gear_dir;
        public ushort gear_base_dir;
        public VecFx32 gear_pos;
        public VecFx32 gearbase_pos;
        public int wall_spd;
        public GSS_SND_SE_HANDLE h_snd;

        public GMS_GMK_SWWALL_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_SWWALL_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }
    }
}
