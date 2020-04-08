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
    public class DMS_SAVE_MAIN_WORK
    {
        public readonly AppMain.AMS_FS[] arc_cmn_amb_fs = new AppMain.AMS_FS[2];
        public readonly AppMain.AMS_AMB_HEADER[] arc_cmn_amb = new AppMain.AMS_AMB_HEADER[2];
        public readonly AppMain.A2S_AMA_HEADER[] cmn_ama = new AppMain.A2S_AMA_HEADER[2];
        public readonly AppMain.AMS_AMB_HEADER[] cmn_amb = new AppMain.AMS_AMB_HEADER[2];
        public readonly AppMain.AOS_TEXTURE[] cmn_tex = AppMain.New<AppMain.AOS_TEXTURE>(2);
        public readonly AppMain.AOS_ACTION[] act = new AppMain.AOS_ACTION[6];
        public readonly float[][] win_act_pos = AppMain.New<float>(5, 2);
        public readonly float[] win_size_rate = new float[2];
        public AppMain._saveproc_input_update proc_input;
        public AppMain._saveproc_input_update proc_menu_update;
        public AppMain._saveproc_draw proc_draw;
        public uint flag;
        public uint announce_flag;
        public uint disp_flag;
        public int state;
        public int timer;
        public int win_timer;
        public int win_mode;
        public int win_cur_slct;
        public uint draw_state;
        public AppMain.GSS_SND_SCB bgm_scb;
    }
}
