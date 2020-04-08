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
    public class OBS_OBJECT_WORK : AppMain.IClearable
    {
        public readonly short[] view_out_ofst_plus = new short[4];
        public AppMain.VecU16 dir = new AppMain.VecU16();
        public AppMain.VecFx32 scale = new AppMain.VecFx32();
        public AppMain.VecFx32 pos = new AppMain.VecFx32();
        public AppMain.VecFx32 ofst = new AppMain.VecFx32();
        public AppMain.VecFx32 prev_ofst = new AppMain.VecFx32();
        public AppMain.VecFx32 parent_ofst = new AppMain.VecFx32();
        public AppMain.VecFx32 lock_ofst = new AppMain.VecFx32();
        public AppMain.VecFx32 prev_pos = new AppMain.VecFx32();
        public AppMain.VecFx32 spd = new AppMain.VecFx32();
        public AppMain.VecFx32 spd_add = new AppMain.VecFx32();
        public AppMain.VecFx32 flow = new AppMain.VecFx32();
        public AppMain.VecFx32 move = new AppMain.VecFx32();
        public readonly short[] field_rect = new short[4];
        public AppMain.VecFx32 temp_ofst = new AppMain.VecFx32();
        public AppMain.VecFx32 prev_temp_ofst = new AppMain.VecFx32();
        public AppMain.OBS_OBJECT_WORK prev;
        public AppMain.OBS_OBJECT_WORK next;
        public AppMain.OBS_OBJECT_WORK draw_prev;
        public AppMain.OBS_OBJECT_WORK draw_next;
        public AppMain.MTS_TASK_TCB tcb;
        public int pause_level;
        public ushort obj_type;
        public int vib_timer;
        public int hitstop_timer;
        public int invincible_timer;
        public short view_out_ofst;
        public uint flag;
        public uint move_flag;
        public uint disp_flag;
        public uint gmk_flag;
        public uint sys_flag;
        private object __user_flag;
        private object __user_work;
        public int user_timer;
        public int spd_m;
        public ushort dir_slope;
        public ushort dir_fall;
        public int spd_slope;
        public int spd_slope_max;
        public int spd_fall;
        public int spd_fall_max;
        public int push_max;
        public uint col_flag;
        public uint col_flag_prev;
        public sbyte field_ajst_w_db_f;
        public sbyte field_ajst_w_db_b;
        public sbyte field_ajst_w_dl_f;
        public sbyte field_ajst_w_dl_b;
        public sbyte field_ajst_w_dt_f;
        public sbyte field_ajst_w_dt_b;
        public sbyte field_ajst_w_dr_f;
        public sbyte field_ajst_w_dr_b;
        public sbyte field_ajst_h_db_r;
        public sbyte field_ajst_h_db_l;
        public sbyte field_ajst_h_dl_r;
        public sbyte field_ajst_h_dl_l;
        public sbyte field_ajst_h_dt_r;
        public sbyte field_ajst_h_dt_l;
        public sbyte field_ajst_h_dr_r;
        public sbyte field_ajst_h_dr_l;
        public AppMain.MPP_VOID_OBS_OBJECT_WORK ppFunc;
        public AppMain.MPP_VOID_OBS_OBJECT_WORK ppIn;
        public AppMain.MPP_VOID_OBS_OBJECT_WORK ppOut;
        public AppMain.MPP_VOID_OBS_OBJECT_WORK ppOutSub;
        public AppMain.MPP_VOID_OBS_OBJECT_WORK ppMove;
        public AppMain.OBS_OBJECT_WORK_Delegate2 ppActCall;
        public AppMain.MPP_VOID_OBS_OBJECT_WORK ppRec;
        public AppMain.MPP_VOID_OBS_OBJECT_WORK ppLast;
        public AppMain.MPP_VOID_OBS_OBJECT_WORK ppCol;
        public AppMain.OBS_OBJECT_WORK_Delegate3 ppViewCheck;
        public AppMain.OBS_OBJECT_WORK_Delegate4 ppUserRelease;
        public AppMain.OBS_OBJECT_WORK_Delegate4 ppUserReleaseWait;
        public AppMain.OBS_OBJECT_WORK ride_obj;
        public AppMain.OBS_OBJECT_WORK touch_obj;
        public AppMain.OBS_OBJECT_WORK parent_obj;
        public AppMain.OBS_OBJECT_WORK lock_obj;
        public AppMain.OBS_OBJECT_WORK locker_obj;
        public object ex_work;
        public AppMain.OBS_ACTION3D_NN_WORK obj_3d;
        public AppMain.OBS_ACTION3D_ES_WORK obj_3des;
        public AppMain.OBS_ACTION2D_AMA_WORK obj_2d;
        public AppMain.OBS_COLLISION_WORK col_work;
        public AppMain.OBS_TBL_WORK tbl_work;
        public uint rect_num;
        public AppMain.ArrayPointer<AppMain.OBS_RECT_WORK> rect_work;
        public object holder;
        public object m_primaryHolder;

        public OBS_OBJECT_WORK()
        {
        }

        private OBS_OBJECT_WORK(object _holder)
          : this(_holder, (object)null)
        {
        }

        private OBS_OBJECT_WORK(object _holder, object _primaryHolder)
        {
            this.holder = _holder;
            this.m_primaryHolder = _primaryHolder;
        }

        public static AppMain.OBS_OBJECT_WORK Create()
        {
            return new AppMain.OBS_OBJECT_WORK();
        }

        public static AppMain.OBS_OBJECT_WORK Create(object _holder)
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.OBS_OBJECT_WORK.Create();
            obsObjectWork.holder = _holder;
            obsObjectWork.m_primaryHolder = (object)null;
            return obsObjectWork;
        }

        public static AppMain.OBS_OBJECT_WORK Create(object _holder, object _primaryHolder)
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.OBS_OBJECT_WORK.Create();
            obsObjectWork.holder = _holder;
            obsObjectWork.m_primaryHolder = _primaryHolder;
            return obsObjectWork;
        }

        public static explicit operator AppMain.GMS_GMK_WATER_SLIDER_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_WATER_SLIDER_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_UPBUMPER_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_UPBUMPER_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_STEAMP_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_STEAMP_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_PWALL_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_PWALL_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_SEESAWPARTS_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_SEESAWPARTS_WORK)(AppMain.GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_PRESSWALL_PARTS(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_PRESSWALL_PARTS)(AppMain.GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_PWALLCTRL_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_PWALLCTRL_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_SEESAW_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_SEESAW_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_SHUTTER_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_SHUTTER_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_P_STEAM_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_P_STEAM_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_PISTONROD_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_PISTONROD_WORK)(AppMain.GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_PISTON_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_PISTON_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS5_LAND_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS5_LAND_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS5_CTPLT_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS5_CTPLT_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS5_ROCKET_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS5_ROCKET_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS5_MGR_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS5_MGR_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS5_BODY_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS5_BODY_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS4_BODY_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS4_BODY_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS4_CAP_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS4_CAP_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS4_CHIBI_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS4_CHIBI_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS4_EGG_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS4_EGG_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS4_MGR_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS4_MGR_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_TRUCK_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_TRUCK_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS2_BALL_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS2_BALL_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS2_EGG_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS2_EGG_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS2_BODY_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS2_BODY_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS2_MGR_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS2_MGR_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS3_EGG_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS3_EGG_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS3_BODY_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS3_BODY_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS3_MGR_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS3_MGR_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_ENE_UNIDES_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_UNIDES_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_ENE_UNIUNI_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_UNIUNI_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_ENE_BUKU_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_BUKU_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_ENE_T_STAR_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_T_STAR_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_ENE_KANI_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_KANI_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_ENE_KAMA_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_KAMA_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_ENE_MOGU_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_MOGU_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_PLAYER_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return p == null ? (AppMain.GMS_PLAYER_WORK)null : (AppMain.GMS_PLAYER_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_SPCTPLT_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_SPCTPLT_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_SLOT_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_SLOT_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_CANNONPARTS_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_CANNONPARTS_WORK)(AppMain.GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS5_LDPART_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS5_LDPART_WORK)(AppMain.GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_SLOTPARTS_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_SLOTPARTS_WORK)(AppMain.GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK)(AppMain.GMS_EFFECT_3DES_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS5_EFCT_GENERAL_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)(AppMain.GMS_EFFECT_3DES_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS4_EFF_COMMON_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS4_EFF_COMMON_WORK)(AppMain.GMS_EFFECT_3DES_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK)(AppMain.GMS_EFFECT_3DES_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_POPSTEAMPARTS_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_POPSTEAMPARTS_WORK)(AppMain.GMS_EFFECT_COM_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS1_FLASH_SCREEN_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS1_FLASH_SCREEN_WORK)(AppMain.GMS_EFFECT_COM_WORK)p;
        }

        public static explicit operator AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)(AppMain.GMS_EFFECT_COM_WORK)p;
        }

        public static explicit operator AppMain.GMS_ENE_HARO_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_HARO_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_STOPPER_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_STOPPER_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS1_CHAIN_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS1_CHAIN_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_ENE_GARDON_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_GARDON_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_BUMPER_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_BUMPER_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS1_EGG_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS1_EGG_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS1_MGR_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS1_MGR_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS1_BODY_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS1_BODY_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_BLAND_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_BLAND_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_BWALL_PARTS(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_BWALL_PARTS)(AppMain.GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_BWALL_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_BWALL_WORK)((AppMain.GMS_ENEMY_3D_WORK)p.m_primaryHolder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_SW_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_SW_WORK)((AppMain.GMS_ENEMY_3D_WORK)p.m_primaryHolder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_PMARKER_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_PMARKER_WORK)(AppMain.GMS_ENEMY_3D_WORK)p.m_primaryHolder;
        }

        public static explicit operator AppMain.GMS_EFFECT_3DNN_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_EFFECT_3DNN_WORK)((AppMain.GMS_EFFECT_COM_WORK)p.holder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_CAM_SCR_LIMIT_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_CAM_SCR_LIMIT_WORK)p.m_primaryHolder;
        }

        public static explicit operator AppMain.GMS_ENE_STING_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_STING_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS5_EGG_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS5_EGG_WORK)(AppMain.GMS_ENEMY_3D_WORK)p.m_primaryHolder;
        }

        public static explicit operator AppMain.GMS_ENEMY_3D_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENEMY_3D_WORK)(AppMain.GMS_ENEMY_COM_WORK)p;
        }

        public static explicit operator AppMain.GMS_DECO_SUBMODEL_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_DECO_SUBMODEL_WORK)(AppMain.GMS_DECO_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_DECO_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_DECO_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_CORE_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS5_CORE_WORK)(AppMain.GMS_ENEMY_COM_WORK)p;
        }

        public static explicit operator AppMain.GMS_ENEMY_COM_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return p == null ? (AppMain.GMS_ENEMY_COM_WORK)null : (AppMain.GMS_ENEMY_COM_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_SCORE_DISP_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_SCORE_DISP_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_CANNON_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_CANNON_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK(
          AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_BOSS5_TURRET_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_BOSS5_TURRET_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_ENE_HARI_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_HARI_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_PULLEY_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_PULLEY_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_ENE_MOTORA_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_ENE_MOTORA_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_BOBJ_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_BOBJ_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_BOBJ_PARTS(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_BOBJ_PARTS)((AppMain.GMS_EFFECT_3DNN_WORK)((AppMain.GMS_EFFECT_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_EFFECT_3DES_WORK(AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_EFFECT_3DES_WORK)(AppMain.GMS_EFFECT_COM_WORK)work.holder;
        }

        public static explicit operator AppMain.DMS_STFRL_BOSS_BODY_WORK(
          AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.DMS_STFRL_BOSS_BODY_WORK)work.holder;
        }

        public static explicit operator AppMain.DMS_STFRL_BOSS_EGG_WORK(
          AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.DMS_STFRL_BOSS_EGG_WORK)work.holder;
        }

        public static explicit operator AppMain.DMS_STFRL_RING_WORK(AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.DMS_STFRL_RING_WORK)work.holder;
        }

        public static explicit operator AppMain.DMS_STFRL_SONIC_WORK(AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.DMS_STFRL_SONIC_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_FADE_OBJ_WORK(AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_FADE_OBJ_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_SMSG_2D_OBJ_WORK(AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_SMSG_2D_OBJ_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_GEAR_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_GEAR_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_BELTC_WORK(AppMain.OBS_OBJECT_WORK p)
        {
            return (AppMain.GMS_GMK_BELTC_WORK)(AppMain.GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator AppMain.GMS_GMK_ROCK_FALL_WORK(
          AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_GMK_ROCK_FALL_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_ROCK_FALL_MGR_WORK(
          AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_GMK_ROCK_FALL_MGR_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_ALARM_FADE_WORK(
          AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_BOSS5_ALARM_FADE_WORK)(AppMain.GMS_FADE_OBJ_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_FLASH_SCREEN_WORK(
          AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_BOSS5_FLASH_SCREEN_WORK)(AppMain.GMS_EFFECT_COM_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_ROCK_CHASE_WORK(
          AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_GMK_ROCK_CHASE_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_SPEARPARTS_WORK(
          AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_GMK_SPEARPARTS_WORK)((AppMain.GMS_EFFECT_3DNN_WORK)((AppMain.GMS_EFFECT_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_SPEAR_WORK(AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_GMK_SPEAR_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_ROCK_WORK(AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_GMK_ROCK_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator AppMain.GMS_GMK_SWWALL_WORK(AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.GMS_GMK_SWWALL_WORK)((AppMain.GMS_ENEMY_3D_WORK)((AppMain.GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public void Clear()
        {
            this.prev = this.next = this.draw_prev = this.draw_next = (AppMain.OBS_OBJECT_WORK)null;
            this.tcb = (AppMain.MTS_TASK_TCB)null;
            this.pause_level = 0;
            this.obj_type = (ushort)0;
            this.vib_timer = this.hitstop_timer = this.invincible_timer = 0;
            this.view_out_ofst = (short)0;
            Array.Clear((Array)this.view_out_ofst_plus, 0, this.view_out_ofst_plus.Length);
            this.user_work_OBJECT = (object)null;
            this.user_flag_OBJECT = (object)null;
            this.flag = 0U;
            this.move_flag = 0U;
            this.disp_flag = 0U;
            this.gmk_flag = 0U;
            this.sys_flag = 0U;
            this.user_timer = 0;
            this.dir.Clear();
            this.scale.Clear();
            this.pos.Clear();
            this.ofst.Clear();
            this.prev_ofst.Clear();
            this.parent_ofst.Clear();
            this.lock_obj = (AppMain.OBS_OBJECT_WORK)null;
            this.prev_pos.Clear();
            this.spd.Clear();
            this.spd_add.Clear();
            this.flow.Clear();
            this.move.Clear();
            this.spd_m = 0;
            this.dir_slope = this.dir_fall = (ushort)0;
            this.spd_slope = this.spd_slope_max = this.spd_fall = this.spd_fall_max = this.push_max = 0;
            this.col_flag = this.col_flag_prev = 0U;
            Array.Clear((Array)this.field_rect, 0, this.field_rect.Length);
            this.field_ajst_w_db_f = this.field_ajst_w_db_b = this.field_ajst_w_dl_f = this.field_ajst_w_dl_b = (sbyte)0;
            this.field_ajst_w_dt_f = this.field_ajst_w_dt_b = this.field_ajst_w_dr_f = this.field_ajst_w_dr_b = (sbyte)0;
            this.field_ajst_h_db_r = this.field_ajst_h_db_l = this.field_ajst_h_dl_r = this.field_ajst_h_dl_l = (sbyte)0;
            this.field_ajst_h_dt_r = this.field_ajst_h_dt_l = this.field_ajst_h_dr_r = this.field_ajst_h_dr_l = (sbyte)0;
            this.ppFunc = this.ppIn = this.ppOut = this.ppOutSub = this.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
            this.ppActCall = (AppMain.OBS_OBJECT_WORK_Delegate2)null;
            this.ppRec = this.ppLast = this.ppCol = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
            this.ppViewCheck = (AppMain.OBS_OBJECT_WORK_Delegate3)null;
            this.ppUserRelease = this.ppUserReleaseWait;
            this.ride_obj = this.touch_obj = this.lock_obj = this.locker_obj = (AppMain.OBS_OBJECT_WORK)null;
            this.parent_obj = (AppMain.OBS_OBJECT_WORK)null;
            this.ex_work = (object)null;
            this.obj_3d = (AppMain.OBS_ACTION3D_NN_WORK)null;
            this.obj_3des = (AppMain.OBS_ACTION3D_ES_WORK)null;
            this.obj_2d = (AppMain.OBS_ACTION2D_AMA_WORK)null;
            this.col_work = (AppMain.OBS_COLLISION_WORK)null;
            this.tbl_work = (AppMain.OBS_TBL_WORK)null;
            this.temp_ofst.Clear();
            this.prev_temp_ofst.Clear();
            this.rect_num = 0U;
            this.rect_work = (AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>)(AppMain.OBS_RECT_WORK[])null;
        }

        public uint user_flag
        {
            get
            {
                return this.__user_flag != null ? Convert.ToUInt32(this.__user_flag is ValueType ? this.__user_flag : (object)1) : 0U;
            }
            set
            {
                this.__user_flag = (object)value;
            }
        }

        public uint user_work
        {
            get
            {
                return Convert.ToUInt32(this.__user_work);
            }
            set
            {
                this.__user_work = (object)value;
            }
        }

        public object user_flag_OBJECT
        {
            get
            {
                return this.__user_flag;
            }
            set
            {
                this.__user_flag = value;
            }
        }

        public object user_work_OBJECT
        {
            get
            {
                return this.__user_work;
            }
            set
            {
                this.__user_work = value;
            }
        }

        public AppMain.OBS_OBJECT_WORK Assign(AppMain.OBS_OBJECT_WORK objectWork)
        {
            if (this != objectWork)
            {
                this.prev = objectWork.prev;
                this.next = objectWork.next;
                this.draw_prev = objectWork.draw_prev;
                this.draw_next = objectWork.draw_next;
                this.tcb = objectWork.tcb;
                this.pause_level = objectWork.pause_level;
                this.obj_type = objectWork.obj_type;
                this.vib_timer = objectWork.vib_timer;
                this.hitstop_timer = objectWork.hitstop_timer;
                this.invincible_timer = objectWork.invincible_timer;
                this.view_out_ofst = objectWork.view_out_ofst;
                for (int index = 0; index < this.view_out_ofst_plus.Length; ++index)
                    this.view_out_ofst_plus[index] = objectWork.view_out_ofst_plus[index];
                this.flag = objectWork.flag;
                this.move_flag = objectWork.move_flag;
                this.disp_flag = objectWork.disp_flag;
                this.gmk_flag = objectWork.gmk_flag;
                this.sys_flag = objectWork.sys_flag;
                this.user_flag = objectWork.user_flag;
                this.user_work = objectWork.user_work;
                this.user_timer = objectWork.user_timer;
                this.dir.Assign(objectWork.dir);
                this.scale.Assign(objectWork.scale);
                this.pos.Assign(objectWork.pos);
                this.ofst.Assign(objectWork.ofst);
                this.prev_ofst.Assign(objectWork.prev_ofst);
                this.parent_ofst.Assign(objectWork.parent_ofst);
                this.lock_ofst.Assign(objectWork.lock_ofst);
                this.prev_pos.Assign(objectWork.prev_pos);
                this.spd.Assign(objectWork.spd);
                this.spd_add.Assign(objectWork.spd_add);
                this.flow.Assign(objectWork.flow);
                this.move.Assign(objectWork.move);
                this.spd_m = objectWork.spd_m;
                this.dir_slope = objectWork.dir_slope;
                this.dir_fall = objectWork.dir_fall;
                this.spd_slope = objectWork.spd_slope;
                this.spd_slope_max = objectWork.spd_slope_max;
                this.spd_fall = objectWork.spd_fall;
                this.spd_fall_max = objectWork.spd_fall_max;
                this.push_max = objectWork.push_max;
                this.col_flag = objectWork.col_flag;
                this.col_flag_prev = objectWork.col_flag_prev;
                for (int index = 0; index < this.field_rect.Length; ++index)
                    this.field_rect[index] = objectWork.field_rect[index];
                this.field_ajst_w_db_f = objectWork.field_ajst_w_db_f;
                this.field_ajst_w_db_b = objectWork.field_ajst_w_db_b;
                this.field_ajst_w_dl_f = objectWork.field_ajst_w_dl_f;
                this.field_ajst_w_dl_b = objectWork.field_ajst_w_dl_b;
                this.field_ajst_w_dt_f = objectWork.field_ajst_w_dt_f;
                this.field_ajst_w_dt_b = objectWork.field_ajst_w_dt_b;
                this.field_ajst_w_dr_f = objectWork.field_ajst_w_dr_f;
                this.field_ajst_w_dr_b = objectWork.field_ajst_w_dr_b;
                this.field_ajst_h_db_r = objectWork.field_ajst_h_db_r;
                this.field_ajst_h_db_l = objectWork.field_ajst_h_db_l;
                this.field_ajst_h_dl_r = objectWork.field_ajst_h_dl_r;
                this.field_ajst_h_dl_l = objectWork.field_ajst_h_dl_l;
                this.field_ajst_h_dt_r = objectWork.field_ajst_h_dt_r;
                this.field_ajst_h_dt_l = objectWork.field_ajst_h_dt_l;
                this.field_ajst_h_dr_r = objectWork.field_ajst_h_dr_r;
                this.field_ajst_h_dr_l = objectWork.field_ajst_h_dr_l;
                this.ppFunc = objectWork.ppFunc;
                this.ppIn = objectWork.ppIn;
                this.ppOut = objectWork.ppOut;
                this.ppOutSub = objectWork.ppOutSub;
                this.ppMove = objectWork.ppMove;
                this.ppActCall = objectWork.ppActCall;
                this.ppRec = objectWork.ppRec;
                this.ppLast = objectWork.ppLast;
                this.ppCol = objectWork.ppCol;
                this.ppViewCheck = objectWork.ppViewCheck;
                this.ppUserRelease = objectWork.ppUserRelease;
                this.ppUserReleaseWait = objectWork.ppUserReleaseWait;
                this.ride_obj = objectWork.ride_obj;
                this.touch_obj = objectWork.touch_obj;
                this.parent_obj = objectWork.parent_obj;
                this.lock_obj = objectWork.lock_obj;
                this.locker_obj = objectWork.locker_obj;
                this.ex_work = objectWork.ex_work;
                this.obj_3d = objectWork.obj_3d;
                this.obj_3des = objectWork.obj_3des;
                this.obj_2d = objectWork.obj_2d;
                this.col_work = objectWork.col_work;
                this.tbl_work = objectWork.tbl_work;
                this.temp_ofst.Assign(objectWork.temp_ofst);
                this.prev_temp_ofst.Assign(objectWork.prev_temp_ofst);
                this.rect_num = objectWork.rect_num;
                this.rect_work = objectWork.rect_work;
            }
            return this;
        }
    }
}
