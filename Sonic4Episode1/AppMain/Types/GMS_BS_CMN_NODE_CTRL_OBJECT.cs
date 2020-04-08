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
    public class GMS_BS_CMN_NODE_CTRL_OBJECT : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.NNS_VECTOR user_ofst = new AppMain.NNS_VECTOR();
        public readonly AppMain.NNS_MATRIX w_mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public object m_pHolder;
        public readonly AppMain.GMS_EFFECT_COM_WORK efct_com;
        public AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work;
        public int cnm_reg_id;
        public AppMain.GMS_BS_CMN_SNM_WORK snm_work;
        public int snm_reg_id;
        public AppMain.NNS_QUATERNION user_quat;
        public uint user_timer;
        public int is_enable;
        public AppMain.MPP_VOID_OBS_OBJECT_WORK proc_update;

        public GMS_BS_CMN_NODE_CTRL_OBJECT()
        {
            this.efct_com = new AppMain.GMS_EFFECT_COM_WORK((object)this);
        }

        public GMS_BS_CMN_NODE_CTRL_OBJECT(object holder)
        {
            this.efct_com = new AppMain.GMS_EFFECT_COM_WORK((object)this);
            this.m_pHolder = holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_SCT_PART_NDC_WORK(
          AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT p)
        {
            return (AppMain.GMS_BOSS5_SCT_PART_NDC_WORK)p.m_pHolder;
        }

        public static explicit operator AppMain.GMS_BOSS2_EFFECT_SCATTER_WORK(
          AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT p)
        {
            return (AppMain.GMS_BOSS2_EFFECT_SCATTER_WORK)p.m_pHolder;
        }

        public static explicit operator AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK(
          AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT p)
        {
            return (AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK)p.m_pHolder;
        }

        public static explicit operator AppMain.GMS_EFFECT_COM_WORK(
          AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT p)
        {
            return (AppMain.GMS_EFFECT_COM_WORK)p.efct_com.obj_work;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.efct_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(
          AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT work)
        {
            return work.efct_com.obj_work;
        }
    }
}
