using System;

public partial class AppMain
{
    public class GMS_RING_SYS_WORK : IClearable
    {
        public byte[] damage_num = new byte[1];
        public GMS_RING_WORK[] ring_list = new GMS_RING_WORK[96];
        public GMS_RING_WORK[] ring_list_buf = New<GMS_RING_WORK>(96);
        public VecFx32[] draw_ring_pos = New<VecFx32>(96);
        public GSS_SND_SE_HANDLE[] h_snd_ring = new GSS_SND_SE_HANDLE[8];
        public uint flag;
        public _ring_work_func_delegate_ ring_draw_func;
        public _rec_func_ rec_func;
        public _ring_work_func_delegate_ col_func;
        public ushort dir;
        public byte player_num;
        public short ref_spd_base;
        public GMS_RING_WORK ring_list_start;
        public GMS_RING_WORK ring_list_end;
        public GMS_RING_WORK twinkle_list_start;
        public GMS_RING_WORK twinkle_list_end;
        public GMS_RING_WORK damage_ring_list_start;
        public GMS_RING_WORK damage_ring_list_end;
        public GMS_RING_WORK slot_ring_list_start;
        public GMS_RING_WORK slot_ring_list_end;
        public int ring_list_cnt;
        public int wait_slot_ring_num;
        public ushort slot_ring_create_dir;
        public OBS_OBJECT_WORK slot_target_obj;
        public int slot_ring_timer;
        public ushort draw_ring_count;
        public ushort draw_ring_uv_frame;
        public int ring_se_cnt;
        public uint color;
        public int se_wait;

        public void Clear()
        {
            this.flag = 0U;
            this.ring_draw_func = null;
            this.rec_func = null;
            this.col_func = null;
            this.dir = 0;
            Array.Clear(damage_num, 0, 1);
            this.player_num = 0;
            this.ref_spd_base = 0;
            this.ring_list_start = null;
            this.ring_list_end = null;
            this.twinkle_list_start = null;
            this.twinkle_list_end = null;
            this.damage_ring_list_start = null;
            this.damage_ring_list_end = null;
            this.slot_ring_list_start = null;
            this.slot_ring_list_end = null;
            this.ring_list_cnt = 0;
            Array.Clear(ring_list, 0, 96);
            ClearArray(this.ring_list_buf);
            this.wait_slot_ring_num = 0;
            this.slot_ring_create_dir = 0;
            this.slot_target_obj = null;
            this.slot_ring_timer = 0;
            this.draw_ring_count = 0;
            this.draw_ring_uv_frame = 0;
            ClearArray(this.draw_ring_pos);
            Array.Clear(h_snd_ring, 0, 2);
            this.ring_se_cnt = 0;
            this.color = 0U;
            this.se_wait = 0;
        }
    }
}
