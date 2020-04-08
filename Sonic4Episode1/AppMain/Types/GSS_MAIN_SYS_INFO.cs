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
    public class GSS_MAIN_SYS_INFO
    {
        public readonly int[] char_id = new int[1];
        public readonly SSave Save = new SSave();
        public uint main_flag;
        public uint game_flag;
        public uint sys_flag;
        public float sys_disp_width;
        public float sys_disp_height;
        public int level;
        public int game_mode;
        public ushort stage_id;
        public uint clear_ring;
        public uint clear_score;
        public int clear_time;
        public uint rest_player_num;
        public float se_volume;
        public float bgm_volume;
        public uint ene_kill_count;
        public uint final_clear_count;
        public uint is_save_run;
        public ushort prev_stage_id;
        public bool is_spe_clear;
        public bool is_first_play;

        public void Clear()
        {
            this.main_flag = 0U;
            this.game_flag = 0U;
            this.sys_flag = 0U;
            this.sys_disp_width = 0.0f;
            this.sys_disp_height = 0.0f;
            this.char_id[0] = 0;
            this.clear_ring = 0U;
            this.clear_score = 0U;
            this.clear_time = 0;
            this.rest_player_num = 0U;
            this.se_volume = 0.0f;
            this.bgm_volume = 0.0f;
            this.ene_kill_count = 0U;
            this.final_clear_count = 0U;
            this.is_save_run = 0U;
            this.Save.Init();
            this.prev_stage_id = (ushort)0;
            this.is_spe_clear = false;
            this.is_first_play = false;
        }
    }
}
