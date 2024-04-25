public partial class AppMain
{
    public class GMS_MAIN_SYSTEM
    {
        public GMS_PLAYER_WORK[] ply_work = new GMS_PLAYER_WORK[1];
        public uint[] player_rest_num = new uint[1];
        public readonly OBS_DIFF_COLLISION map_fcol = new OBS_DIFF_COLLISION();
        public int[] map_size = new int[2];
        public readonly NNS_VECTOR def_light_vec = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR ply_light_vec = GlobalPool<NNS_VECTOR>.Alloc();
        public uint game_flag;
        public MTS_TASK_TCB pre_tcb;
        public MTS_TASK_TCB post_tcb;
        public uint game_time;
        public uint sync_time;
        public uint marker_pri;
        public uint time_save;
        public int resume_pos_x;
        public int resume_pos_y;
        public ushort water_level;
        public ushort pseudofall_dir;
        public int die_event_wait_time;
        public NNS_RGBA def_light_col;
        public NNS_RGBA ply_light_col;
        public uint ply_dmg_count;
        public int boss_load_no;
        public int polar_diff;
        public int polar_now;
        public GME_MAIN_CAMSCALE_STATE camscale_state;
        public float camera_scale;

        public void Clear()
        {
            this.game_flag = 0U;
            this.pre_tcb = null;
            this.post_tcb = null;
            this.game_time = 0U;
            this.sync_time = 0U;
            for (int index = 0; index < this.ply_work.Length; ++index)
                this.ply_work[index] = null;
            this.marker_pri = 0U;
            this.time_save = 0U;
            this.resume_pos_x = 0;
            this.resume_pos_y = 0;
            for (int index = 0; index < 1; ++index)
                this.player_rest_num[index] = 0U;
            this.map_fcol.Clear();
            for (int index = 0; index < 2; ++index)
                this.map_size[index] = 0;
            this.water_level = 0;
            this.pseudofall_dir = 0;
            this.die_event_wait_time = 0;
            this.def_light_vec.Clear();
            this.def_light_col.Clear();
            this.ply_light_vec.Clear();
            this.ply_light_col.Clear();
            this.ply_dmg_count = 0U;
            this.boss_load_no = 0;
            this.polar_diff = 0;
            this.polar_now = 0;
            this.camscale_state = GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_NON;
            this.camera_scale = 0.0f;
        }
    }
}
