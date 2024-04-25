using System;

public partial class AppMain
{
    public class OBS_OBJECT_WORK : IClearable
    {
        public readonly short[] view_out_ofst_plus = new short[4];
        public VecU16 dir = new VecU16();
        public VecFx32 scale = new VecFx32();
        public VecFx32 pos = new VecFx32();
        public VecFx32 ofst = new VecFx32();
        public VecFx32 prev_ofst = new VecFx32();
        public VecFx32 parent_ofst = new VecFx32();
        public VecFx32 lock_ofst = new VecFx32();
        public VecFx32 prev_pos = new VecFx32();
        public VecFx32 spd = new VecFx32();
        public VecFx32 spd_add = new VecFx32();
        public VecFx32 flow = new VecFx32();
        public VecFx32 move = new VecFx32();
        public readonly short[] field_rect = new short[4];
        public VecFx32 temp_ofst = new VecFx32();
        public VecFx32 prev_temp_ofst = new VecFx32();
        public OBS_OBJECT_WORK prev;
        public OBS_OBJECT_WORK next;
        public OBS_OBJECT_WORK draw_prev;
        public OBS_OBJECT_WORK draw_next;
        public MTS_TASK_TCB tcb;
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
        public MPP_VOID_OBS_OBJECT_WORK ppFunc;
        public MPP_VOID_OBS_OBJECT_WORK ppIn;
        public MPP_VOID_OBS_OBJECT_WORK ppOut;
        public MPP_VOID_OBS_OBJECT_WORK ppOutSub;
        public MPP_VOID_OBS_OBJECT_WORK ppMove;
        public OBS_OBJECT_WORK_Delegate2 ppActCall;
        public MPP_VOID_OBS_OBJECT_WORK ppRec;
        public MPP_VOID_OBS_OBJECT_WORK ppLast;
        public MPP_VOID_OBS_OBJECT_WORK ppCol;
        public OBS_OBJECT_WORK_Delegate3 ppViewCheck;
        public OBS_OBJECT_WORK_Delegate4 ppUserRelease;
        public OBS_OBJECT_WORK_Delegate4 ppUserReleaseWait;
        public OBS_OBJECT_WORK ride_obj;
        public OBS_OBJECT_WORK touch_obj;
        public OBS_OBJECT_WORK parent_obj;
        public OBS_OBJECT_WORK lock_obj;
        public OBS_OBJECT_WORK locker_obj;
        public object ex_work;
        public OBS_ACTION3D_NN_WORK obj_3d;
        public OBS_ACTION3D_ES_WORK obj_3des;
        public OBS_ACTION2D_AMA_WORK obj_2d;
        public OBS_COLLISION_WORK col_work;
        public OBS_TBL_WORK tbl_work;
        public uint rect_num;
        public ArrayPointer<OBS_RECT_WORK> rect_work;
        public object holder;
        public object m_primaryHolder;

        public OBS_OBJECT_WORK()
        {
        }

        private OBS_OBJECT_WORK(object _holder)
          : this(_holder, null)
        {
        }

        private OBS_OBJECT_WORK(object _holder, object _primaryHolder)
        {
            this.holder = _holder;
            this.m_primaryHolder = _primaryHolder;
        }

        public static OBS_OBJECT_WORK Create()
        {
            return new OBS_OBJECT_WORK();
        }

        public static OBS_OBJECT_WORK Create(object _holder)
        {
            OBS_OBJECT_WORK obsObjectWork = Create();
            obsObjectWork.holder = _holder;
            obsObjectWork.m_primaryHolder = null;
            return obsObjectWork;
        }

        public static OBS_OBJECT_WORK Create(object _holder, object _primaryHolder)
        {
            OBS_OBJECT_WORK obsObjectWork = Create();
            obsObjectWork.holder = _holder;
            obsObjectWork.m_primaryHolder = _primaryHolder;
            return obsObjectWork;
        }

        public static explicit operator GMS_GMK_WATER_SLIDER_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_WATER_SLIDER_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_UPBUMPER_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_UPBUMPER_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_STEAMP_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_STEAMP_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_PWALL_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_PWALL_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_SEESAWPARTS_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_SEESAWPARTS_WORK)(GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator GMS_GMK_PRESSWALL_PARTS(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_PRESSWALL_PARTS)(GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator GMS_GMK_PWALLCTRL_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_PWALLCTRL_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_SEESAW_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_SEESAW_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_SHUTTER_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_SHUTTER_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_P_STEAM_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_P_STEAM_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_PISTONROD_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_PISTONROD_WORK)(GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator GMS_GMK_PISTON_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_PISTON_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_DRAIN_TANK_OUT_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_DRAIN_TANK_OUT_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_BOSS3_PILLAR_WALL_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_BOSS3_PILLAR_WALL_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS5_LAND_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS5_LAND_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS5_CTPLT_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS5_CTPLT_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS5_ROCKET_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS5_ROCKET_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS5_MGR_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS5_MGR_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS5_BODY_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS5_BODY_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS4_BODY_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS4_BODY_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS4_CAP_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS4_CAP_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS4_CHIBI_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS4_CHIBI_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS4_EGG_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS4_EGG_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS4_MGR_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS4_MGR_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_TRUCK_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_TRUCK_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS2_BALL_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS2_BALL_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS2_EGG_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS2_EGG_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS2_BODY_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS2_BODY_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS2_MGR_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS2_MGR_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS3_EGG_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS3_EGG_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS3_BODY_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS3_BODY_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS3_MGR_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS3_MGR_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_ENE_UNIDES_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_UNIDES_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_ENE_UNIUNI_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_UNIUNI_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_ENE_BUKU_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_BUKU_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_ENE_T_STAR_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_T_STAR_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_ENE_KANI_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_KANI_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_ENE_KAMA_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_KAMA_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_ENE_MOGU_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_MOGU_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_PLAYER_WORK(OBS_OBJECT_WORK p)
        {
            return p == null ? null : (GMS_PLAYER_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_SPCTPLT_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_SPCTPLT_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_SLOT_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_SLOT_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_CANNONPARTS_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_CANNONPARTS_WORK)(GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator GMS_BOSS5_LDPART_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS5_LDPART_WORK)(GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator GMS_GMK_SLOTPARTS_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_SLOTPARTS_WORK)(GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK)(GMS_EFFECT_3DES_WORK)p;
        }

        public static explicit operator GMS_BOSS5_EFCT_GENERAL_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS5_EFCT_GENERAL_WORK)(GMS_EFFECT_3DES_WORK)p;
        }

        public static explicit operator GMS_BOSS4_EFF_COMMON_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS4_EFF_COMMON_WORK)(GMS_EFFECT_3DES_WORK)p;
        }

        public static explicit operator GMS_BOSS1_EFF_SHOCKWAVE_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS1_EFF_SHOCKWAVE_WORK)(GMS_EFFECT_3DES_WORK)p;
        }

        public static explicit operator GMS_GMK_POPSTEAMPARTS_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_POPSTEAMPARTS_WORK)(GMS_EFFECT_COM_WORK)p;
        }

        public static explicit operator GMS_BOSS1_FLASH_SCREEN_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS1_FLASH_SCREEN_WORK)(GMS_EFFECT_COM_WORK)p;
        }

        public static explicit operator GMS_BS_CMN_NODE_CTRL_OBJECT(
          OBS_OBJECT_WORK p)
        {
            return (GMS_BS_CMN_NODE_CTRL_OBJECT)(GMS_EFFECT_COM_WORK)p;
        }

        public static explicit operator GMS_ENE_HARO_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_HARO_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_STOPPER_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_STOPPER_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS1_CHAIN_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS1_CHAIN_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_ENE_GARDON_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_GARDON_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_BUMPER_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_BUMPER_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS1_EGG_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS1_EGG_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS1_MGR_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS1_MGR_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS1_BODY_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS1_BODY_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_BOSS3_PILLAR_MANAGER_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_BLAND_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_BLAND_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_BWALL_PARTS(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_BWALL_PARTS)(GMS_EFFECT_3DNN_WORK)p;
        }

        public static explicit operator GMS_GMK_BWALL_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_BWALL_WORK)((GMS_ENEMY_3D_WORK)p.m_primaryHolder).holder;
        }

        public static explicit operator GMS_GMK_SW_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_SW_WORK)((GMS_ENEMY_3D_WORK)p.m_primaryHolder).holder;
        }

        public static explicit operator GMS_GMK_PMARKER_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_PMARKER_WORK)(GMS_ENEMY_3D_WORK)p.m_primaryHolder;
        }

        public static explicit operator GMS_EFFECT_3DNN_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_EFFECT_3DNN_WORK)((GMS_EFFECT_COM_WORK)p.holder).holder;
        }

        public static explicit operator GMS_GMK_CAM_SCR_LIMIT_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_CAM_SCR_LIMIT_WORK)p.m_primaryHolder;
        }

        public static explicit operator GMS_ENE_STING_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_STING_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS5_EGG_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS5_EGG_WORK)(GMS_ENEMY_3D_WORK)p.m_primaryHolder;
        }

        public static explicit operator GMS_ENEMY_3D_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENEMY_3D_WORK)(GMS_ENEMY_COM_WORK)p;
        }

        public static explicit operator GMS_DECO_SUBMODEL_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_DECO_SUBMODEL_WORK)(GMS_DECO_WORK)p.holder;
        }

        public static explicit operator GMS_DECO_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_DECO_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS5_CORE_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS5_CORE_WORK)(GMS_ENEMY_COM_WORK)p;
        }

        public static explicit operator GMS_ENEMY_COM_WORK(OBS_OBJECT_WORK p)
        {
            return p == null ? null : (GMS_ENEMY_COM_WORK)p.holder;
        }

        public static explicit operator GMS_SCORE_DISP_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_SCORE_DISP_WORK)p.holder;
        }

        public static explicit operator GMS_GMK_CANNON_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_CANNON_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_BOSS3_PILLAR_MAIN_WORK(
          OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_BOSS5_TURRET_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_BOSS5_TURRET_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator GMS_ENE_HARI_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_HARI_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator GMS_GMK_PULLEY_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_PULLEY_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator GMS_ENE_MOTORA_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_ENE_MOTORA_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator GMS_GMK_BOBJ_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_BOBJ_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator GMS_GMK_BOBJ_PARTS(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_BOBJ_PARTS)((GMS_EFFECT_3DNN_WORK)((GMS_EFFECT_COM_WORK)p.holder).holder).holder;
        }

        public static explicit operator GMS_EFFECT_3DES_WORK(OBS_OBJECT_WORK work)
        {
            return (GMS_EFFECT_3DES_WORK)(GMS_EFFECT_COM_WORK)work.holder;
        }

        public static explicit operator DMS_STFRL_BOSS_BODY_WORK(
          OBS_OBJECT_WORK work)
        {
            return (DMS_STFRL_BOSS_BODY_WORK)work.holder;
        }

        public static explicit operator DMS_STFRL_BOSS_EGG_WORK(
          OBS_OBJECT_WORK work)
        {
            return (DMS_STFRL_BOSS_EGG_WORK)work.holder;
        }

        public static explicit operator DMS_STFRL_RING_WORK(OBS_OBJECT_WORK work)
        {
            return (DMS_STFRL_RING_WORK)work.holder;
        }

        public static explicit operator DMS_STFRL_SONIC_WORK(OBS_OBJECT_WORK work)
        {
            return (DMS_STFRL_SONIC_WORK)work.holder;
        }

        public static explicit operator GMS_FADE_OBJ_WORK(OBS_OBJECT_WORK work)
        {
            return (GMS_FADE_OBJ_WORK)work.holder;
        }

        public static explicit operator GMS_SMSG_2D_OBJ_WORK(OBS_OBJECT_WORK work)
        {
            return (GMS_SMSG_2D_OBJ_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_GEAR_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_GEAR_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_BELTC_WORK(OBS_OBJECT_WORK p)
        {
            return (GMS_GMK_BELTC_WORK)(GMS_ENEMY_3D_WORK)p;
        }

        public static explicit operator GMS_GMK_ROCK_FALL_WORK(
          OBS_OBJECT_WORK work)
        {
            return (GMS_GMK_ROCK_FALL_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator GMS_GMK_ROCK_FALL_MGR_WORK(
          OBS_OBJECT_WORK work)
        {
            return (GMS_GMK_ROCK_FALL_MGR_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator GMS_BOSS5_ALARM_FADE_WORK(
          OBS_OBJECT_WORK work)
        {
            return (GMS_BOSS5_ALARM_FADE_WORK)(GMS_FADE_OBJ_WORK)work.holder;
        }

        public static explicit operator GMS_BOSS5_FLASH_SCREEN_WORK(
          OBS_OBJECT_WORK work)
        {
            return (GMS_BOSS5_FLASH_SCREEN_WORK)(GMS_EFFECT_COM_WORK)work.holder;
        }

        public static explicit operator GMS_GMK_ROCK_CHASE_WORK(
          OBS_OBJECT_WORK work)
        {
            return (GMS_GMK_ROCK_CHASE_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator GMS_GMK_SPEARPARTS_WORK(
          OBS_OBJECT_WORK work)
        {
            return (GMS_GMK_SPEARPARTS_WORK)((GMS_EFFECT_3DNN_WORK)((GMS_EFFECT_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator GMS_GMK_SPEAR_WORK(OBS_OBJECT_WORK work)
        {
            return (GMS_GMK_SPEAR_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator GMS_GMK_ROCK_WORK(OBS_OBJECT_WORK work)
        {
            return (GMS_GMK_ROCK_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public static explicit operator GMS_GMK_SWWALL_WORK(OBS_OBJECT_WORK work)
        {
            return (GMS_GMK_SWWALL_WORK)((GMS_ENEMY_3D_WORK)((GMS_ENEMY_COM_WORK)work.holder).holder).holder;
        }

        public void Clear()
        {
            this.prev = this.next = this.draw_prev = this.draw_next = null;
            this.tcb = null;
            this.pause_level = 0;
            this.obj_type = 0;
            this.vib_timer = this.hitstop_timer = this.invincible_timer = 0;
            this.view_out_ofst = 0;
            Array.Clear(view_out_ofst_plus, 0, this.view_out_ofst_plus.Length);
            this.user_work_OBJECT = null;
            this.user_flag_OBJECT = null;
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
            this.lock_obj = null;
            this.prev_pos.Clear();
            this.spd.Clear();
            this.spd_add.Clear();
            this.flow.Clear();
            this.move.Clear();
            this.spd_m = 0;
            this.dir_slope = this.dir_fall = 0;
            this.spd_slope = this.spd_slope_max = this.spd_fall = this.spd_fall_max = this.push_max = 0;
            this.col_flag = this.col_flag_prev = 0U;
            Array.Clear(field_rect, 0, this.field_rect.Length);
            this.field_ajst_w_db_f = this.field_ajst_w_db_b = this.field_ajst_w_dl_f = this.field_ajst_w_dl_b = 0;
            this.field_ajst_w_dt_f = this.field_ajst_w_dt_b = this.field_ajst_w_dr_f = this.field_ajst_w_dr_b = 0;
            this.field_ajst_h_db_r = this.field_ajst_h_db_l = this.field_ajst_h_dl_r = this.field_ajst_h_dl_l = 0;
            this.field_ajst_h_dt_r = this.field_ajst_h_dt_l = this.field_ajst_h_dr_r = this.field_ajst_h_dr_l = 0;
            this.ppFunc = this.ppIn = this.ppOut = this.ppOutSub = this.ppMove = null;
            this.ppActCall = null;
            this.ppRec = this.ppLast = this.ppCol = null;
            this.ppViewCheck = null;
            this.ppUserRelease = this.ppUserReleaseWait;
            this.ride_obj = this.touch_obj = this.lock_obj = this.locker_obj = null;
            this.parent_obj = null;
            this.ex_work = null;
            this.obj_3d = null;
            this.obj_3des = null;
            this.obj_2d = null;
            this.col_work = null;
            this.tbl_work = null;
            this.temp_ofst.Clear();
            this.prev_temp_ofst.Clear();
            this.rect_num = 0U;
            this.rect_work = null;
        }

        public uint user_flag
        {
            get => this.__user_flag != null ? Convert.ToUInt32(this.__user_flag is ValueType ? this.__user_flag : 1) : 0U;
            set => this.__user_flag = value;
        }

        public uint user_work
        {
            get => Convert.ToUInt32(this.__user_work);
            set => this.__user_work = value;
        }

        public object user_flag_OBJECT
        {
            get => this.__user_flag;
            set => this.__user_flag = value;
        }

        public object user_work_OBJECT
        {
            get => this.__user_work;
            set => this.__user_work = value;
        }

        public OBS_OBJECT_WORK Assign(OBS_OBJECT_WORK objectWork)
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
