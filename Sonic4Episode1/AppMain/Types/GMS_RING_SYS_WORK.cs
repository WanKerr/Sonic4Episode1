using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using accel;
using dbg;
using er;
using er.web;
using gs;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using mpp;
using setting;

public partial class AppMain
{
    public class GMS_RING_SYS_WORK : AppMain.IClearable
    {
        public byte[] damage_num = new byte[1];
        public AppMain.GMS_RING_WORK[] ring_list = new AppMain.GMS_RING_WORK[96];
        public AppMain.GMS_RING_WORK[] ring_list_buf = AppMain.New<AppMain.GMS_RING_WORK>(96);
        public AppMain.VecFx32[] draw_ring_pos = AppMain.New<AppMain.VecFx32>(96);
        public AppMain.GSS_SND_SE_HANDLE[] h_snd_ring = new AppMain.GSS_SND_SE_HANDLE[8];
        public uint flag;
        public AppMain._ring_work_func_delegate_ ring_draw_func;
        public AppMain._rec_func_ rec_func;
        public AppMain._ring_work_func_delegate_ col_func;
        public ushort dir;
        public byte player_num;
        public short ref_spd_base;
        public AppMain.GMS_RING_WORK ring_list_start;
        public AppMain.GMS_RING_WORK ring_list_end;
        public AppMain.GMS_RING_WORK twinkle_list_start;
        public AppMain.GMS_RING_WORK twinkle_list_end;
        public AppMain.GMS_RING_WORK damage_ring_list_start;
        public AppMain.GMS_RING_WORK damage_ring_list_end;
        public AppMain.GMS_RING_WORK slot_ring_list_start;
        public AppMain.GMS_RING_WORK slot_ring_list_end;
        public int ring_list_cnt;
        public int wait_slot_ring_num;
        public ushort slot_ring_create_dir;
        public AppMain.OBS_OBJECT_WORK slot_target_obj;
        public int slot_ring_timer;
        public ushort draw_ring_count;
        public ushort draw_ring_uv_frame;
        public int ring_se_cnt;
        public uint color;
        public int se_wait;

        public void Clear()
        {
            this.flag = 0U;
            this.ring_draw_func = (AppMain._ring_work_func_delegate_)null;
            this.rec_func = (AppMain._rec_func_)null;
            this.col_func = (AppMain._ring_work_func_delegate_)null;
            this.dir = (ushort)0;
            Array.Clear((Array)this.damage_num, 0, 1);
            this.player_num = (byte)0;
            this.ref_spd_base = (short)0;
            this.ring_list_start = (AppMain.GMS_RING_WORK)null;
            this.ring_list_end = (AppMain.GMS_RING_WORK)null;
            this.twinkle_list_start = (AppMain.GMS_RING_WORK)null;
            this.twinkle_list_end = (AppMain.GMS_RING_WORK)null;
            this.damage_ring_list_start = (AppMain.GMS_RING_WORK)null;
            this.damage_ring_list_end = (AppMain.GMS_RING_WORK)null;
            this.slot_ring_list_start = (AppMain.GMS_RING_WORK)null;
            this.slot_ring_list_end = (AppMain.GMS_RING_WORK)null;
            this.ring_list_cnt = 0;
            Array.Clear((Array)this.ring_list, 0, 96);
            AppMain.ClearArray<AppMain.GMS_RING_WORK>(this.ring_list_buf);
            this.wait_slot_ring_num = 0;
            this.slot_ring_create_dir = (ushort)0;
            this.slot_target_obj = (AppMain.OBS_OBJECT_WORK)null;
            this.slot_ring_timer = 0;
            this.draw_ring_count = (ushort)0;
            this.draw_ring_uv_frame = (ushort)0;
            AppMain.ClearArray<AppMain.VecFx32>(this.draw_ring_pos);
            Array.Clear((Array)this.h_snd_ring, 0, 2);
            this.ring_se_cnt = 0;
            this.color = 0U;
            this.se_wait = 0;
        }
    }
}
