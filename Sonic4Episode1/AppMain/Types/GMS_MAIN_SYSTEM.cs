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
    public class GMS_MAIN_SYSTEM
    {
        public AppMain.GMS_PLAYER_WORK[] ply_work = new AppMain.GMS_PLAYER_WORK[1];
        public uint[] player_rest_num = new uint[1];
        public readonly AppMain.OBS_DIFF_COLLISION map_fcol = new AppMain.OBS_DIFF_COLLISION();
        public int[] map_size = new int[2];
        public readonly AppMain.NNS_VECTOR def_light_vec = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR ply_light_vec = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public uint game_flag;
        public AppMain.MTS_TASK_TCB pre_tcb;
        public AppMain.MTS_TASK_TCB post_tcb;
        public uint game_time;
        public uint sync_time;
        public uint marker_pri;
        public uint time_save;
        public int resume_pos_x;
        public int resume_pos_y;
        public ushort water_level;
        public ushort pseudofall_dir;
        public int die_event_wait_time;
        public AppMain.NNS_RGBA def_light_col;
        public AppMain.NNS_RGBA ply_light_col;
        public uint ply_dmg_count;
        public int boss_load_no;
        public int polar_diff;
        public int polar_now;
        public AppMain.GME_MAIN_CAMSCALE_STATE camscale_state;
        public float camera_scale;

        public void Clear()
        {
            this.game_flag = 0U;
            this.pre_tcb = (AppMain.MTS_TASK_TCB)null;
            this.post_tcb = (AppMain.MTS_TASK_TCB)null;
            this.game_time = 0U;
            this.sync_time = 0U;
            for (int index = 0; index < this.ply_work.Length; ++index)
                this.ply_work[index] = (AppMain.GMS_PLAYER_WORK)null;
            this.marker_pri = 0U;
            this.time_save = 0U;
            this.resume_pos_x = 0;
            this.resume_pos_y = 0;
            for (int index = 0; index < 1; ++index)
                this.player_rest_num[index] = 0U;
            this.map_fcol.Clear();
            for (int index = 0; index < 2; ++index)
                this.map_size[index] = 0;
            this.water_level = (ushort)0;
            this.pseudofall_dir = (ushort)0;
            this.die_event_wait_time = 0;
            this.def_light_vec.Clear();
            this.def_light_col.Clear();
            this.ply_light_vec.Clear();
            this.ply_light_col.Clear();
            this.ply_dmg_count = 0U;
            this.boss_load_no = 0;
            this.polar_diff = 0;
            this.polar_now = 0;
            this.camscale_state = AppMain.GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_NON;
            this.camera_scale = 0.0f;
        }
    }
}
