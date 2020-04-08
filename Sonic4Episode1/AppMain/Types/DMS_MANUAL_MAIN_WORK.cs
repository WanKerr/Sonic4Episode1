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
    public class DMS_MANUAL_MAIN_WORK
    {
        public AppMain.A2S_AMA_HEADER[] ama = new AppMain.A2S_AMA_HEADER[2];
        public AppMain.AMS_AMB_HEADER[] amb = new AppMain.AMS_AMB_HEADER[2];
        public AppMain.AOS_ACTION[] act = new AppMain.AOS_ACTION[179];
        public CTrgAoAction trg_return = new CTrgAoAction();
        public AppMain.AMS_FS arc_amb;
        public AppMain.AOS_TEXTURE[] tex;
        public AppMain.DMS_MANUAL_MAIN_WORK._proc_input_ proc_input;
        public AppMain.DMS_MANUAL_MAIN_WORK._proc_update_ proc_update;
        public AppMain.DMS_MANUAL_MAIN_WORK._proc_draw_ proc_draw;
        public float timer;
        public uint flag;
        public float efct_timer;
        public int cur_disp_page;
        public int cur_disp_page_prev;
        public bool is_jp_region;
        public bool is_maingame_load;
        public uint draw_state;
        public AppMain.GSS_SND_SE_HANDLE se_handle;
        public CTrgAoAction[] trg_btn;

        public DMS_MANUAL_MAIN_WORK()
        {
            this.tex = AppMain.New<AppMain.AOS_TEXTURE>(2);
            this.trg_btn = AppMain.New<CTrgAoAction>(2);
        }

        public delegate void _proc_input_(AppMain.DMS_MANUAL_MAIN_WORK work);

        public delegate void _proc_update_(AppMain.DMS_MANUAL_MAIN_WORK work);

        public delegate void _proc_draw_(AppMain.DMS_MANUAL_MAIN_WORK work);
    }
}
