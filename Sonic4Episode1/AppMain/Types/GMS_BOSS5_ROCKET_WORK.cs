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
    public class GMS_BOSS5_ROCKET_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public AppMain.VecFx32 launch_pos = new AppMain.VecFx32();
        public AppMain.VecFx32 dest_pos = new AppMain.VecFx32();
        public AppMain.VecFx32 rvs_acc = new AppMain.VecFx32();
        public AppMain.NNS_QUATERNION sct_cur_quat = new AppMain.NNS_QUATERNION();
        public AppMain.NNS_QUATERNION sct_spin_quat = new AppMain.NNS_QUATERNION();
        public readonly AppMain.GMS_BS_CMN_BMCB_MGR bmcb_mgr = new AppMain.GMS_BS_CMN_BMCB_MGR();
        public readonly AppMain.GMS_BS_CMN_SNM_WORK snm_work = new AppMain.GMS_BS_CMN_SNM_WORK();
        public AppMain.VecFx32 pivot_prev_pos = new AppMain.VecFx32();
        public AppMain.NNS_QUATERNION stuck_lerp_src_quat = new AppMain.NNS_QUATERNION();
        public readonly AppMain.GMS_BS_CMN_DELAY_SEARCH_WORK dsearch_work = new AppMain.GMS_BS_CMN_DELAY_SEARCH_WORK();
        public AppMain.VecFx32[] search_hist_buf = AppMain.New<AppMain.VecFx32>(21U);
        public readonly AppMain.GMS_ENEMY_3D_WORK ene_3d;
        public uint flag;
        public int rkt_type;
        public uint wait_timer;
        public uint hit_count;
        public uint no_hit_timer;
        public uint wfall_atk_toggle_timer;
        public AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK proc_update;
        public int move_dir;
        public int acc;
        public int max_spd;
        public int rot_spd;
        public int stuck_dir;
        public float stuck_lean_ratio;
        public float hit_vib_amp_deg;
        public int hit_vib_sin_angle;
        public int pivot_fall_angle;
        public int wobble_sin_param_angle;
        public int arm_snm_id;
        public int drill_snm_reg_id;
        public float stuck_lerp_ratio;
        public float stuck_lerp_ratio_spd;
        public int ply_search_delay;

        public GMS_BOSS5_ROCKET_WORK()
        {
            this.ene_3d = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public static explicit operator AppMain.GMS_ENEMY_COM_WORK(
          AppMain.GMS_BOSS5_ROCKET_WORK p)
        {
            return p.ene_3d.ene_com;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
