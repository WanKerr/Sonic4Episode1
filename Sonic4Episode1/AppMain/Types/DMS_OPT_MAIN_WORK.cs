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
    public class DMS_OPT_MAIN_WORK
    {
        public AppMain.AMS_FS[] arc_cmn_amb_fs = new AppMain.AMS_FS[5];
        public AppMain.AMS_AMB_HEADER[] arc_cmn_amb = new AppMain.AMS_AMB_HEADER[5];
        public AppMain.A2S_AMA_HEADER[] cmn_ama = new AppMain.A2S_AMA_HEADER[5];
        public AppMain.AMS_AMB_HEADER[] cmn_amb = new AppMain.AMS_AMB_HEADER[5];
        public AppMain.AMS_FS[] arc_amb_fs = new AppMain.AMS_FS[2];
        public AppMain.AMS_FS[] user_arc_amb_fs = new AppMain.AMS_FS[2];
        public AppMain.AMS_FS[] manual_arc_amb_fs = new AppMain.AMS_FS[2];
        public AppMain.AMS_AMB_HEADER[] arc_amb = new AppMain.AMS_AMB_HEADER[2];
        public AppMain.AMS_AMB_HEADER[] user_arc_amb = new AppMain.AMS_AMB_HEADER[2];
        public AppMain.AMS_AMB_HEADER[] manual_arc_amb = new AppMain.AMS_AMB_HEADER[2];
        public AppMain.A2S_AMA_HEADER[] ama = new AppMain.A2S_AMA_HEADER[2];
        public AppMain.AMS_AMB_HEADER[] amb = new AppMain.AMS_AMB_HEADER[2];
        public AppMain.AOS_TEXTURE win_tex = new AppMain.AOS_TEXTURE();
        public AppMain.AOS_ACTION[] act = new AppMain.AOS_ACTION[102];
        public float[] push_efct_timer = new float[4];
        public int[] volume_data = new int[2];
        public CTrgAoAction[] trg_se_btn = new CTrgAoAction[2];
        public CTrgAoAction[] trg_ctrl_btn = new CTrgAoAction[2];
        public CTrgAoAction[] ctrl_win_trg_btn = new CTrgAoAction[2];
        public float[] ctrl_tab_pos_x = new float[2];
        public float[] ctrl_tab_pos_y = new float[2];
        public float[][] ctrl_move_src = AppMain.New<float>(2, 2);
        public float[][] ctrl_move_dest = AppMain.New<float>(2, 2);
        public float[] obi_tex_pos = new float[2];
        public float[] win_size_rate = new float[2];
        public AppMain.AOS_ACT_COL decide_menu_col = new AppMain.AOS_ACT_COL();
        public AppMain.AOS_ACT_COL vol_icon_col = new AppMain.AOS_ACT_COL();
        public AppMain.AOS_ACT_COL win_col = new AppMain.AOS_ACT_COL();
        public AppMain.AOS_TEXTURE[] cmn_tex;
        public AppMain.AMS_FS[] win_amb_fs;
        public AppMain.AMS_AMB_HEADER[] win_amb;
        public AppMain.AOS_TEXTURE[] tex;
        public AppMain.AOS_ACTION bg_icon_node;
        public AppMain.DMS_OPT_MAIN_WORK._proc_input_ proc_input;
        public AppMain.DMS_OPT_MAIN_WORK._proc_update_ proc_update;
        public AppMain.DMS_OPT_MAIN_WORK._proc_draw_ proc_draw;
        public AppMain.DMS_OPT_MAIN_WORK._proc_menu_draw_ proc_menu_draw;
        public float frm_update_time;
        public float timer;
        public float efct_timer;
        public float win_timer;
        public float vib_timer;
        public uint flag;
        public int state;
        public uint disp_flag;
        public int next_evt;
        public int prev_evt;
        public CTrgAoAction[] trg_slct;
        public CTrgAoAction trg_return;
        public int ctrl_mode;
        public int prev_ctrl_mode;
        public int set_vbrt;
        public CTrgAoAction[] trg_bgm_btn;
        public CTrgRect trg_bgm_slider;
        public CTrgRect trg_se_slider;
        public float ctrl_win_window_prgrs;
        public int cur_slct_top;
        public int cur_slct_set;
        public float top_crsr_pos_y;
        public float src_crsr_pos_y;
        public float dst_crsr_pos_y;
        public float obi_pos_y;
        public float set_icon_dir;
        public float set_icon_shdw_dir;
        public uint draw_state;
        public int nrml_disp_type;
        public int prev_nrml_disp_type;
        public bool is_jp_region;
        public AppMain.GSS_SND_SCB bgm_scb;
        public AppMain.GSS_SND_SE_HANDLE se_handle;

        public DMS_OPT_MAIN_WORK()
        {
            this.cmn_tex = AppMain.New<AppMain.AOS_TEXTURE>(5);
            this.tex = AppMain.New<AppMain.AOS_TEXTURE>(2);
            this.trg_slct = AppMain.New<CTrgAoAction>(4);
            this.trg_bgm_btn = AppMain.New<CTrgAoAction>(2);
            this.trg_se_btn = AppMain.New<CTrgAoAction>(2);
            this.trg_ctrl_btn = AppMain.New<CTrgAoAction>(2);
            this.ctrl_win_trg_btn = AppMain.New<CTrgAoAction>(2);
            this.trg_bgm_slider = new CTrgRect();
            this.trg_se_slider = new CTrgRect();
            this.trg_return = new CTrgAoAction();
        }

        public delegate void _proc_input_(AppMain.DMS_OPT_MAIN_WORK work);

        public delegate void _proc_update_(AppMain.DMS_OPT_MAIN_WORK work);

        public delegate void _proc_draw_(AppMain.DMS_OPT_MAIN_WORK work);

        public delegate void _proc_menu_draw_(AppMain.DMS_OPT_MAIN_WORK work);
    }
}
