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
    public class DMS_STFRL_MAIN_WORK
    {
        public readonly AppMain.DMS_STFRL_DATA_MGR arc_data = new AppMain.DMS_STFRL_DATA_MGR();
        public readonly AppMain.AMS_AMB_HEADER[] arc_cmn_amb_fs = new AppMain.AMS_AMB_HEADER[2];
        public readonly AppMain.AOS_TEXTURE font_tex = new AppMain.AOS_TEXTURE();
        public readonly AppMain.AOS_TEXTURE[] scr_tex = AppMain.New<AppMain.AOS_TEXTURE>(3);
        public readonly AppMain.AOS_TEXTURE end_tex = new AppMain.AOS_TEXTURE();
        public readonly AppMain.AOS_TEXTURE end_jp_tex = new AppMain.AOS_TEXTURE();
        public readonly AppMain.A2S_AMA_HEADER[] cmn_ama = new AppMain.A2S_AMA_HEADER[2];
        public readonly AppMain.AMS_AMB_HEADER[] cmn_amb = new AppMain.AMS_AMB_HEADER[2];
        public readonly AppMain.AOS_TEXTURE[] cmn_tex = AppMain.New<AppMain.AOS_TEXTURE>(2);
        public readonly AppMain.AOS_ACTION[] act = new AppMain.AOS_ACTION[11];
        public readonly float[] win_size_rate = new float[2];
        public readonly bool[] data_disp_yet = new bool[3];
        public readonly bool[] check_file_load = new bool[3];
        public readonly AppMain.DMS_STFRL_RING_WORK[] ring_work = new AppMain.DMS_STFRL_RING_WORK[3];
        public AppMain.AMS_AMB_HEADER arc_list_font_amb;
        public AppMain.AMS_AMB_HEADER arc_scr_amb_fs;
        public AppMain.AMS_AMB_HEADER arc_end_amb_fs;
        public AppMain.AMS_AMB_HEADER arc_end_jp_amb_fs;
        public AppMain.AMS_FS arc_list_font_amb_fs;
        public object staff_list_fs;
        public AppMain.A2S_AMA_HEADER end_ama;
        public AppMain.AMS_AMB_HEADER end_amb;
        public AppMain.A2S_AMA_HEADER end_jp_ama;
        public AppMain.AMS_AMB_HEADER end_jp_amb;
        public object stf_list_ysd;
        public AppMain.DMS_STFRL_MAIN_WORK._proc_input_ proc_input;
        public AppMain.DMS_STFRL_MAIN_WORK._proc_update_ proc_update;
        public AppMain.DMS_STFRL_MAIN_WORK._proc_data_load_ proc_data_load;
        public AppMain.DMS_STFRL_MAIN_WORK._proc_draw_ proc_draw;
        public float timer;
        public uint flag;
        public float efct_timer;
        public float fade_timer;
        public float win_timer;
        public float disp_frm_time;
        public uint disp_mode;
        public bool is_eme_comp;
        public uint win_mode;
        public uint announce_flag;
        public float sonic_set_frame;
        public float list_disp_pos_x;
        public AppMain.AOS_ACT_COL list_col;
        public float sonic_move_spd;
        public int end_act_frm;
        public int continue_act_frm;
        public int load_data_num;
        public bool is_full_staffroll;
        public uint draw_state;
        public uint cur_disp_scr_id;
        public uint disp_list_page_num;
        public uint cur_disp_list_page;
        public uint prev_disp_list_page;
        public uint disp_page_time;
        public uint cur_disp_image;
        public uint[] page_line_type;
        public uint cur_page_list_alpha_data;
        public uint cur_page_scr_alpha_data;
        public AppMain.AOS_ACT_COL question_act_alpha;
        public AppMain.DMS_STFRL_SONIC_WORK sonic_work;
        public AppMain.DMS_STFRL_BOSS_BODY_WORK body_work;
        public AppMain.DMS_STFRL_BOSS_EGG_WORK egg_work;
        public AppMain.GSS_SND_SCB bgm_scb;
        public AppMain.GSS_SND_SE_HANDLE se_handle;

        public delegate void _proc_input_(AppMain.DMS_STFRL_MAIN_WORK work);

        public delegate void _proc_update_(AppMain.DMS_STFRL_MAIN_WORK work);

        public delegate void _proc_data_load_(AppMain.DMS_STFRL_MAIN_WORK work);

        public delegate void _proc_draw_(AppMain.DMS_STFRL_MAIN_WORK work);
    }
}
