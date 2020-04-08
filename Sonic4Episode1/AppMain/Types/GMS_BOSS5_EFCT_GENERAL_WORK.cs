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
    public class GMS_BOSS5_EFCT_GENERAL_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.NNS_MATRIX ofst_mtx = new AppMain.NNS_MATRIX();
        public readonly AppMain.GMS_BOSS5_1SHOT_TIMER se_timer = new AppMain.GMS_BOSS5_1SHOT_TIMER();
        public readonly AppMain.GMS_EFFECT_3DES_WORK efct_3des;
        public uint flag;
        public uint user_flag;
        public uint user_work;
        public int ref_node_snm_id;
        public uint timer;
        public float ratio_timer;
        public uint se_cnt;
        public AppMain.GSS_SND_SE_HANDLE se_handle;

        public GMS_BOSS5_EFCT_GENERAL_WORK()
        {
            this.efct_3des = new AppMain.GMS_EFFECT_3DES_WORK((object)this);
        }

        public static explicit operator AppMain.GMS_EFFECT_COM_WORK(
          AppMain.GMS_BOSS5_EFCT_GENERAL_WORK p)
        {
            return p.efct_3des.efct_com;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.efct_3des.efct_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(
          AppMain.GMS_BOSS5_EFCT_GENERAL_WORK work)
        {
            return work.efct_3des.efct_com.obj_work;
        }
    }
}
