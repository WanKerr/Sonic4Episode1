public partial class AppMain
{
    public class GMS_GMK_GEAR_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK obj_3d_gear_opt = new OBS_ACTION3D_NN_WORK();
        public readonly OBS_ACTION3D_NN_WORK obj_3d_gear_opt_ashiba = new OBS_ACTION3D_NN_WORK();
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public uint col_type;
        public float dir_speed;
        public float dir_temp;
        public ushort prev_dir;
        public ushort move_draw_dir;
        public ushort old_move_draw_dir;
        public short move_draw_dir_spd;
        public short move_draw_dir_ofst;
        public short move_draw_dir_limit;
        public ushort move_stagger_dir_cnt;
        public ushort move_stagger_step;
        public int move_stagger_dir_spd;
        public int stop_timer;
        public int rect_ret_timer;
        public int move_end_x;
        public int move_end_y;
        public int ret_max_speed;
        public bool vib_end;
        public int open_rot_dist;
        public ushort gear_sw_dir_base;
        public int close_rot_spd;
        public OBS_OBJECT_WORK gear_end_obj;
        public OBS_OBJECT_WORK move_gear_obj;
        public OBS_OBJECT_WORK sw_gear_obj;
        public GSS_SND_SE_HANDLE h_snd_gear;

        public GMS_GMK_GEAR_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_GEAR_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }
    }
}
