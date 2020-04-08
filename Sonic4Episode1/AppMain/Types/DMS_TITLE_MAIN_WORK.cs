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
    public class DMS_TITLE_MAIN_WORK
    {
        public readonly AppMain.AMS_FS[] arc_cmn_amb_fs = new AppMain.AMS_FS[4];
        public readonly AppMain.AMS_AMB_HEADER[] arc_cmn_amb = new AppMain.AMS_AMB_HEADER[4];
        public readonly AppMain.A2S_AMA_HEADER[] cmn_ama = new AppMain.A2S_AMA_HEADER[4];
        public readonly AppMain.AMS_AMB_HEADER[] cmn_amb = new AppMain.AMS_AMB_HEADER[4];
        public readonly AppMain.AOS_TEXTURE[] cmn_tex = AppMain.New<AppMain.AOS_TEXTURE>(4);
        public readonly AppMain.DMS_BUY_SCR_WORK buy_scr_work = new AppMain.DMS_BUY_SCR_WORK();
        public readonly AppMain.AMS_FS[] arc_amb_fs = new AppMain.AMS_FS[2];
        public readonly AppMain.AMS_FS[] file_arc_amb_fs = new AppMain.AMS_FS[2];
        public readonly AppMain.AMS_FS[] user_arc_amb_fs = new AppMain.AMS_FS[2];
        public readonly AppMain.AMS_AMB_HEADER[] arc_amb = new AppMain.AMS_AMB_HEADER[2];
        public readonly AppMain.AMS_AMB_HEADER[] file_arc_amb = new AppMain.AMS_AMB_HEADER[2];
        public readonly AppMain.AMS_AMB_HEADER[] user_arc_amb = new AppMain.AMS_AMB_HEADER[2];
        public readonly AppMain.A2S_AMA_HEADER[] ama = new AppMain.A2S_AMA_HEADER[2];
        public readonly AppMain.AMS_AMB_HEADER[] amb = new AppMain.AMS_AMB_HEADER[2];
        public readonly AppMain.AOS_TEXTURE[] tex = AppMain.New<AppMain.AOS_TEXTURE>(2);
        public readonly AppMain.AOS_TEXTURE win_tex = new AppMain.AOS_TEXTURE();
        public readonly AppMain.AOS_ACTION[] act = new AppMain.AOS_ACTION[41];
        public readonly float[] decide_menu_frm = new float[5];
        public readonly float[] mmenu_win_size_rate = new float[2];
        public readonly float[] win_size_rate = new float[2];
        public readonly CTrgAoAction[] trg_slct = AppMain.New<CTrgAoAction>(5);
        public readonly CTrgAoAction[] trg_answer = AppMain.New<CTrgAoAction>(2);
        public readonly CTrgAoAction trg_return = new CTrgAoAction();
        public readonly CTrgAoAction trg_game = new CTrgAoAction();
        public AppMain.AMS_FS cmn_win_amb_fs;
        public AppMain.AMS_FS win_amb_fs;
        public AppMain.AMS_AMB_HEADER cmn_win_amb;
        public AppMain.AMS_AMB_HEADER win_amb;
        public AppMain.DMS_TITLE_MAIN_WORK._proc_ proc_input;
        public AppMain.DMS_TITLE_MAIN_WORK._proc_ proc_update;
        public AppMain.DMS_TITLE_MAIN_WORK._proc_ proc_win_input;
        public AppMain.DMS_TITLE_MAIN_WORK._proc_ proc_win_update;
        public AppMain.DMS_TITLE_MAIN_WORK._proc_ proc_draw;
        public float timer;
        public float disp_timer;
        public float win_timer;
        public float mmenu_win_timer;
        public uint flag;
        public int disp_change_time;
        public uint announce_flag;
        public int win_mode;
        public int cur_slct_menu;
        public int prev_slct_menu;
        public int win_cur_slct;
        public int next_evt;
        public bool is_init_play;
        public bool is_jp_region;
        public bool is_no_save_data;
        public float cur_crsr_pos_y;
        public float src_crsr_pos_y;
        public float dst_crsr_pos_y;
        public int slct_menu_num;
        public uint disp_flag;
        public uint flag_prev;

        public delegate void _proc_(AppMain.DMS_TITLE_MAIN_WORK work);
    }
}
