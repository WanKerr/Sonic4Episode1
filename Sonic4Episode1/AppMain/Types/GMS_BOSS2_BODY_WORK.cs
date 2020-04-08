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
    public class GMS_BOSS2_BODY_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public AppMain.OBS_OBJECT_WORK[] parts_objs = new AppMain.OBS_OBJECT_WORK[2];
        public readonly AppMain.OBS_RECT_WORK rect_work_arm = new AppMain.OBS_RECT_WORK();
        public readonly AppMain.GMS_BS_CMN_BMCB_MGR bmcb_mgr = new AppMain.GMS_BS_CMN_BMCB_MGR();
        public readonly AppMain.GMS_BS_CMN_SNM_WORK snm_work = new AppMain.GMS_BS_CMN_SNM_WORK();
        public int[] snm_reg_id = new int[15];
        public readonly AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work = new AppMain.GMS_BS_CMN_CNM_MGR_WORK();
        public int[] cnm_reg_id = new int[13];
        public readonly AppMain.GMS_BS_CMN_DMG_FLICKER_WORK flk_work = new AppMain.GMS_BS_CMN_DMG_FLICKER_WORK();
        public readonly AppMain.GMS_CMN_FLASH_SCR_WORK flash_work = new AppMain.GMS_CMN_FLASH_SCR_WORK();
        public readonly AppMain.GMS_BOSS2_EFF_BOMB_WORK bomb_work = new AppMain.GMS_BOSS2_EFF_BOMB_WORK();
        public readonly AppMain.GMS_ENEMY_3D_WORK ene_3d;
        public int state;
        public int prev_state;
        public AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK proc_update;
        public uint flag;
        public int action_id;
        public float offset_arm;
        public int count_release_key;
        public int shake_pos;
        public int shake_speed;
        public uint shake_count;
        public uint counter_pinball;
        public short angle_current;
        public AppMain.VecFx32 start_pos;
        public AppMain.VecFx32 end_pos;
        public float move_counter;
        public float move_frame;
        public short turn_start;
        public int turn_amount;
        public float turn_counter;
        public float turn_frame;
        public uint counter_no_hit;
        public uint counter_invincible;
        public AppMain.GSS_SND_SE_HANDLE se_handle;

        public GMS_BOSS2_BODY_WORK()
        {
            this.ene_3d = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_BOSS2_BODY_WORK work)
        {
            return work.ene_3d.ene_com.obj_work;
        }
    }
}
