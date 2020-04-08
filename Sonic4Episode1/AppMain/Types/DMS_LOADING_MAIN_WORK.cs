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
    public class DMS_LOADING_MAIN_WORK
    {
        public AppMain.AOS_ACTION[] act = new AppMain.AOS_ACTION[8];
        public AppMain.AMS_FS arc_amb;
        public byte[] ama;
        public byte[] amb;
        public AppMain.AOS_TEXTURE tex;
        public AppMain.DMS_LOADING_MAIN_WORK._proc_update_ proc_update;
        public AppMain.DMS_LOADING_MAIN_WORK._proc_draw_ proc_draw;
        public float timer;
        public uint flag;
        public float efct_timer;
        public float sonic_set_frame;
        public float sonic_pos_x;
        public float sonic_move_spd;
        public bool is_maingame_load;
        public bool is_play_maingame;
        public uint draw_state;
        public int lang_id;

        public delegate void _proc_update_(AppMain.DMS_LOADING_MAIN_WORK work);

        public delegate void _proc_draw_(AppMain.DMS_LOADING_MAIN_WORK work);
    }
}
