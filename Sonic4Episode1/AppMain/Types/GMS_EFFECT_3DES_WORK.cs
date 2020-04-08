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
    public class GMS_EFFECT_3DES_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_EFFECT_COM_WORK efct_com = new AppMain.GMS_EFFECT_COM_WORK();
        public readonly AppMain.OBS_ACTION3D_ES_WORK obj_3des = new AppMain.OBS_ACTION3D_ES_WORK();
        public uint saved_pos_type;
        public uint saved_init_flag;
        public object m_pHolder;

        public void Clear()
        {
            this.efct_com.Clear();
            this.obj_3des.Clear();
            this.saved_pos_type = 0U;
            this.saved_init_flag = 0U;
        }

        public static explicit operator AppMain.GMS_EFFECT_COM_WORK(
          AppMain.GMS_EFFECT_3DES_WORK work)
        {
            return work.efct_com;
        }

        public static explicit operator AppMain.GMS_BOSS5_EFCT_GENERAL_WORK(
          AppMain.GMS_EFFECT_3DES_WORK work)
        {
            return (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)work.m_pHolder;
        }

        public static explicit operator AppMain.GMS_BOSS4_EFF_COMMON_WORK(
          AppMain.GMS_EFFECT_3DES_WORK work)
        {
            return (AppMain.GMS_BOSS4_EFF_COMMON_WORK)work.m_pHolder;
        }

        public static explicit operator AppMain.GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK(
          AppMain.GMS_EFFECT_3DES_WORK work)
        {
            return (AppMain.GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK)work.m_pHolder;
        }

        public static explicit operator AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK(
          AppMain.GMS_EFFECT_3DES_WORK work)
        {
            return (AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK)work.m_pHolder;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.efct_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_EFFECT_3DES_WORK work)
        {
            return work.efct_com.obj_work;
        }

        public GMS_EFFECT_3DES_WORK()
        {
            this.efct_com = new AppMain.GMS_EFFECT_COM_WORK((object)this);
        }

        public GMS_EFFECT_3DES_WORK(object pHolder)
          : this()
        {
            this.m_pHolder = pHolder;
        }
    }
}
