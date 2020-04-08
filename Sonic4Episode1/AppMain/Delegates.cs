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
    public delegate int _obj_block_collision_func_delegate(AppMain.OBS_COL_CHK_DATA data);

    public delegate int AMTREffectProc(AppMain.AMS_TRAIL_EFFECT pEffect);

    public delegate void _sts_proc_();

    public delegate void OBJF_CAMERA_USER_FUNC(AppMain.OBS_CAMERA camera);

    public delegate void _am_draw_command_delegate(AppMain.AMS_COMMAND_HEADER ch, uint n);

    public delegate void _am_draw_regist_delegate(AppMain.AMS_REGISTLIST l);

    public delegate void TaskProc(AppMain.AMS_TCB tcb);

    public delegate int NNS_MATERIALCALLBACK_FUNC(AppMain.NNS_DRAWCALLBACK_VAL val);

    public delegate void post_func_Delegate(AppMain.DMS_LOGO_COM_LOAD_CONTEXT context);

    public delegate void GMF_DECO_RECT_FUNC(
        AppMain.OBS_RECT_WORK _obs_rect_work1,
        AppMain.OBS_RECT_WORK _obs_rect_work2);

    public delegate void OBF_DRAW_USER_DT_FUNC(object o);

    public delegate void OBF_DRAW_USER_FUNC(object o);

    public delegate void OBF_DRAW_3DNN_MPLT_CB_FUNC(
        ref Matrix mtx_plt,
        AppMain.NNS_OBJECT _object,
        object mplt_cb_param);

    public delegate void OBF_DRAW_3DNN_MOTION_CB_FUNC(
        AppMain.AMS_MOTION motion,
        AppMain.NNS_OBJECT _object,
        object mtn_cb_param);

    public delegate void MPP_VOID_OBJECT_DELEGATE(object o);

    public delegate bool MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE(
        AppMain.NNS_DRAWCALLBACK_VAL drawCallback,
        object p);

    public delegate void MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT(
        AppMain.NNS_MATRIX[] m,
        AppMain.NNS_OBJECT nnso,
        object o);

    public delegate void OBS_DRAW_PARAM_3DNN_DRAW_MOTION_FUNC(object pObj);

    public delegate void MPP_VOID_AMSCOMMANDHEADER_NNFDRAWOBJ(AppMain.AMS_COMMAND_HEADER h, uint o);

    public delegate void MPP_VOID_NNSMATRIX_NNSOBJECT_OBJECT(
        ref AppMain.NNS_MATRIX m,
        AppMain.NNS_OBJECT nnso,
        object o);

    public delegate void MPP_VOID_GMS_FIX_PART_WORK(AppMain.GMS_FIX_PART_WORK pPart);

    public delegate void MPP_VOID_OBS_OBJECT_WORK(AppMain.OBS_OBJECT_WORK pPart);

    public delegate void GMF_FIX_PART_INIT_FUNC(AppMain.GMS_FIX_MGR_WORK pArg);

    public delegate void MPP_VOID_GMSGAMEDATLOADCONTEXT(AppMain.GMS_GAMEDAT_LOAD_CONTEXT p);

    public delegate void MPP_OBJECT_STRING(string p);

    public delegate void MPP_VOID_MOTION_NSSOBJECT_OBJECT(
        AppMain.AMS_MOTION motion,
        AppMain.NNS_OBJECT _object,
        object bmcb_param);

    public delegate void MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.GMS_BOSS1_BODY_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS1_EGG_WORK(AppMain.GMS_BOSS1_EGG_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS1_CHAIN_WORK(AppMain.GMS_BOSS1_CHAIN_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.GMS_BOSS2_BODY_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS2_EGG_WORK(AppMain.GMS_BOSS2_EGG_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS2_BALL_WORK(AppMain.GMS_BOSS2_BALL_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.GMS_BOSS4_BODY_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS4_EGG_WORK(AppMain.GMS_BOSS4_EGG_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS4_CHIBI_WORK(AppMain.GMS_BOSS4_CHIBI_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS4_CAP_WORK(AppMain.GMS_BOSS4_CAP_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.GMS_BOSS5_BODY_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_ALARM_FADE_WORK(AppMain.GMS_BOSS5_ALARM_FADE_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.GMS_BOSS5_MGR_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_CORE_WORK(AppMain.GMS_BOSS5_CORE_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.GMS_BOSS5_ROCKET_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_TURRET_WORK(AppMain.GMS_BOSS5_TURRET_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_EGG_WORK(AppMain.GMS_BOSS5_EGG_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_CTPLT_WORK(AppMain.GMS_BOSS5_CTPLT_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_LAND_WORK(AppMain.GMS_BOSS5_LAND_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_LDPART_WORK(AppMain.GMS_BOSS5_LDPART_WORK wrk);

    public delegate void pfnGMS_GMK_TRUCK_WORK(
        AppMain.GMS_GMK_TRUCK_WORK a,
        AppMain.GMS_PLAYER_WORK b);

    public delegate void GMS_GMK_CAM_SCR_LIMIT_SETTING_func(AppMain.GMS_GMK_CAM_SCR_LIMIT_WORK p);

    public delegate void DMS_LOGO_SEGA_WORK_Delegate(AppMain.DMS_LOGO_SEGA_WORK work);

    public delegate void gamedat_build_area_func();

    public delegate void _saveproc_input_update(AppMain.DMS_SAVE_MAIN_WORK work);

    public delegate void _saveproc_draw();

    public delegate void GSF_INIT_FUNC();

    public delegate void FsBackgroundReadComplete(AppMain.AMS_FS fs);

    public delegate void pfnGMS_SMSG_MGR_WORK(AppMain.GMS_SMSG_MGR_WORK p);

    public delegate void seq_func_delegate(AppMain.GMS_PLAYER_WORK ply_work);

    public delegate void _GMS_OVER_MGR_WORK_UD_(AppMain.GMS_OVER_MGR_WORK _gms_over_mgr_work);

    public delegate void _ring_work_func_delegate_(AppMain.GMS_RING_WORK gms_ring_work);

    public delegate ushort _rec_func_(AppMain.OBS_RECT obs_rect1, AppMain.OBS_RECT obs_rect2);

    public delegate void OBS_RECT_WORK_Delegate1(
        AppMain.OBS_RECT_WORK rectWork1,
        AppMain.OBS_RECT_WORK rectWork2);

    public delegate uint OBS_RECT_WORK_Delegate2(
        AppMain.OBS_RECT_WORK rectWork1,
        AppMain.OBS_RECT_WORK rectWork2);

    public delegate bool HGF_TROPHY_ACQUIRE_CHECK_FUNC();

    public delegate void GMF_BOSS3_BODY_STATE_FUNC(AppMain.GMS_BOSS3_BODY_WORK body_work);

    public delegate void GMF_BOSS3_EGG_STATE_FUNC(AppMain.GMS_BOSS3_EGG_WORK egg_work);

    public delegate AppMain.OBS_OBJECT_WORK Del__(
        AppMain.GMS_EVE_RECORD_EVENT eve_rec,
        int x,
        int y,
        byte type);

    public delegate AppMain.OBS_OBJECT_WORK Del__2(
        AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
        int x,
        int y,
        byte type);

    public delegate void GMF_BOSS1_BODY_STATE_ENTER_FUNC(AppMain.GMS_BOSS1_BODY_WORK work);

    public delegate void GMF_BOSS1_BODY_STATE_LEAVE_FUNC(AppMain.GMS_BOSS1_BODY_WORK work);

    public delegate void GMF_BOSS4_BODY_STATE_ENTER_FUNC(AppMain.GMS_BOSS4_BODY_WORK bodeWork);

    public delegate void GMF_BOSS4_BODY_STATE_LEAVE_FUNC(AppMain.GMS_BOSS4_BODY_WORK bodyWork);

    public delegate void GSF_TASK_PROCEDURE(AppMain.MTS_TASK_TCB task);

    public delegate object TaskWorkFactoryDelegate();

    public delegate void OBJECT_Cam_Delegate(ref int fx1, ref int fx2);

    public delegate void OBJECT_Delegate();

    public delegate void OBJECT_WORK_Delegate(AppMain.OBS_OBJECT_WORK work);

    public delegate void user_func_delegate(object o);

    public delegate void mplt_cb_func_delegate(
        ref AppMain.NNS_MATRIX m,
        AppMain.NNS_OBJECT nnso,
        object o);

    public delegate void mtn_cb_func_delegate(
        AppMain.AMS_MOTION m,
        AppMain.NNS_OBJECT nnso,
        object p);

    public delegate bool material_cb_func_delegate(AppMain.NNS_DRAWCALLBACK_VAL callback, object o);

    public delegate void OBS_OBJECT_WORK_Delegate2(object o1, object o2, uint u);

    public delegate int OBS_OBJECT_WORK_Delegate3(AppMain.OBS_OBJECT_WORK objectWork);

    public delegate bool OBS_OBJECT_WORK_Delegate4(AppMain.OBS_OBJECT_WORK objectWork);

    public delegate void OBF_ACT_CALLBACK(object o1, object o2, uint u);

    public delegate void NNS_NODEUSRMOT_CALLBACK_FUNC(AppMain.NNS_NODEUSRMOT_CALLBACK_VAL val);

    public delegate void AMF_THREAD_PROC();

    public delegate int AmeDelegateFunc(object o);

    public delegate void AmeFieldFunc(
        AppMain.AMS_AME_ECB ecb,
        AppMain.AMS_AME_NODE node,
        AppMain.AMS_AME_RUNTIME_WORK work);
        
    private delegate void _eve_func_(uint flag, ushort bx, ushort by, int[] r_on, int[] r_off);
    private delegate int ObjDiffCollision_pFunc_delegate(
        int i1,
        int i2,
        ushort us,
        ushort[] usa,
        uint[] uia);
    private delegate int pFunc_Delegate(
        AppMain.OBS_COLLISION_OBJ pColObj,
        int lPosX,
        int lPosY,
        ushort ucSuf,
        ushort[] pDir,
        uint[] pAttr);

}