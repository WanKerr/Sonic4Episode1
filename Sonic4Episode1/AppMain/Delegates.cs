using Microsoft.Xna.Framework;

public partial class AppMain
{
    public delegate int _obj_block_collision_func_delegate(OBS_COL_CHK_DATA data);

    public delegate int AMTREffectProc(AMS_TRAIL_EFFECT pEffect);

    public delegate void _sts_proc_();

    public delegate void OBJF_CAMERA_USER_FUNC(OBS_CAMERA camera);

    public delegate void _am_draw_command_delegate(AMS_COMMAND_HEADER ch, uint n);

    public delegate void _am_draw_regist_delegate(AMS_REGISTLIST l);

    public delegate void TaskProc(AMS_TCB tcb);

    public delegate int NNS_MATERIALCALLBACK_FUNC(NNS_DRAWCALLBACK_VAL val);

    public delegate void post_func_Delegate(DMS_LOGO_COM_LOAD_CONTEXT context);

    public delegate void GMF_DECO_RECT_FUNC(
        OBS_RECT_WORK _obs_rect_work1,
        OBS_RECT_WORK _obs_rect_work2);

    public delegate void OBF_DRAW_USER_DT_FUNC(object o);

    public delegate void OBF_DRAW_USER_FUNC(object o);

    public delegate void OBF_DRAW_3DNN_MPLT_CB_FUNC(
        ref Matrix mtx_plt,
        NNS_OBJECT _object,
        object mplt_cb_param);

    public delegate void OBF_DRAW_3DNN_MOTION_CB_FUNC(
        AMS_MOTION motion,
        NNS_OBJECT _object,
        object mtn_cb_param);

    public delegate void MPP_VOID_OBJECT_DELEGATE(object o);

    public delegate bool MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE(
        NNS_DRAWCALLBACK_VAL drawCallback,
        object p);

    public delegate void MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT(
        NNS_MATRIX[] m,
        NNS_OBJECT nnso,
        object o);

    public delegate void OBS_DRAW_PARAM_3DNN_DRAW_MOTION_FUNC(object pObj);

    public delegate void MPP_VOID_AMSCOMMANDHEADER_NNFDRAWOBJ(AMS_COMMAND_HEADER h, uint o);

    public delegate void MPP_VOID_NNSMATRIX_NNSOBJECT_OBJECT(
        ref NNS_MATRIX m,
        NNS_OBJECT nnso,
        object o);

    public delegate void MPP_VOID_GMS_FIX_PART_WORK(GMS_FIX_PART_WORK pPart);

    public delegate void MPP_VOID_OBS_OBJECT_WORK(OBS_OBJECT_WORK pPart);

    public delegate void GMF_FIX_PART_INIT_FUNC(GMS_FIX_MGR_WORK pArg);

    public delegate void MPP_VOID_GMSGAMEDATLOADCONTEXT(GMS_GAMEDAT_LOAD_CONTEXT p);

    public delegate void MPP_OBJECT_STRING(string p);

    public delegate void MPP_VOID_MOTION_NSSOBJECT_OBJECT(
        AMS_MOTION motion,
        NNS_OBJECT _object,
        object bmcb_param);

    public delegate void MPP_VOID_GMS_BOSS1_BODY_WORK(GMS_BOSS1_BODY_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS1_EGG_WORK(GMS_BOSS1_EGG_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS1_CHAIN_WORK(GMS_BOSS1_CHAIN_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS2_BODY_WORK(GMS_BOSS2_BODY_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS2_EGG_WORK(GMS_BOSS2_EGG_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS2_BALL_WORK(GMS_BOSS2_BALL_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS4_BODY_WORK(GMS_BOSS4_BODY_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS4_EGG_WORK(GMS_BOSS4_EGG_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS4_CHIBI_WORK(GMS_BOSS4_CHIBI_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS4_CAP_WORK(GMS_BOSS4_CAP_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_BODY_WORK(GMS_BOSS5_BODY_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_ALARM_FADE_WORK(GMS_BOSS5_ALARM_FADE_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_MGR_WORK(GMS_BOSS5_MGR_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_CORE_WORK(GMS_BOSS5_CORE_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_ROCKET_WORK(GMS_BOSS5_ROCKET_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_TURRET_WORK(GMS_BOSS5_TURRET_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_EGG_WORK(GMS_BOSS5_EGG_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_CTPLT_WORK(GMS_BOSS5_CTPLT_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_LAND_WORK(GMS_BOSS5_LAND_WORK wrk);

    public delegate void MPP_VOID_GMS_BOSS5_LDPART_WORK(GMS_BOSS5_LDPART_WORK wrk);

    public delegate void pfnGMS_GMK_TRUCK_WORK(
        GMS_GMK_TRUCK_WORK a,
        GMS_PLAYER_WORK b);

    public delegate void GMS_GMK_CAM_SCR_LIMIT_SETTING_func(GMS_GMK_CAM_SCR_LIMIT_WORK p);

    public delegate void DMS_LOGO_SEGA_WORK_Delegate(DMS_LOGO_SEGA_WORK work);

    public delegate void gamedat_build_area_func();

    public delegate void _saveproc_input_update(DMS_SAVE_MAIN_WORK work);

    public delegate void _saveproc_draw();

    public delegate void GSF_INIT_FUNC();

    public delegate void FsBackgroundReadComplete(AMS_FS fs);

    public delegate void pfnGMS_SMSG_MGR_WORK(GMS_SMSG_MGR_WORK p);

    public delegate void seq_func_delegate(GMS_PLAYER_WORK ply_work);

    public delegate void _GMS_OVER_MGR_WORK_UD_(GMS_OVER_MGR_WORK _gms_over_mgr_work);

    public delegate void _ring_work_func_delegate_(GMS_RING_WORK gms_ring_work);

    public delegate ushort _rec_func_(OBS_RECT obs_rect1, OBS_RECT obs_rect2);

    public delegate void OBS_RECT_WORK_Delegate1(
        OBS_RECT_WORK rectWork1,
        OBS_RECT_WORK rectWork2);

    public delegate uint OBS_RECT_WORK_Delegate2(
        OBS_RECT_WORK rectWork1,
        OBS_RECT_WORK rectWork2);

    public delegate bool HGF_TROPHY_ACQUIRE_CHECK_FUNC();

    public delegate void GMF_BOSS3_BODY_STATE_FUNC(GMS_BOSS3_BODY_WORK body_work);

    public delegate void GMF_BOSS3_EGG_STATE_FUNC(GMS_BOSS3_EGG_WORK egg_work);

    public delegate OBS_OBJECT_WORK Del__(
        GMS_EVE_RECORD_EVENT eve_rec,
        int x,
        int y,
        byte type);

    public delegate OBS_OBJECT_WORK Del__2(
        GMS_EVE_RECORD_DECORATE dec_rec,
        int x,
        int y,
        byte type);

    public delegate void GMF_BOSS1_BODY_STATE_ENTER_FUNC(GMS_BOSS1_BODY_WORK work);

    public delegate void GMF_BOSS1_BODY_STATE_LEAVE_FUNC(GMS_BOSS1_BODY_WORK work);

    public delegate void GMF_BOSS4_BODY_STATE_ENTER_FUNC(GMS_BOSS4_BODY_WORK bodeWork);

    public delegate void GMF_BOSS4_BODY_STATE_LEAVE_FUNC(GMS_BOSS4_BODY_WORK bodyWork);

    public delegate void GSF_TASK_PROCEDURE(MTS_TASK_TCB task);

    public delegate object TaskWorkFactoryDelegate();

    public delegate void OBJECT_Cam_Delegate(ref int fx1, ref int fx2);

    public delegate void OBJECT_Delegate();

    public delegate void OBJECT_WORK_Delegate(OBS_OBJECT_WORK work);

    public delegate void user_func_delegate(object o);

    public delegate void mplt_cb_func_delegate(
        ref NNS_MATRIX m,
        NNS_OBJECT nnso,
        object o);

    public delegate void mtn_cb_func_delegate(
        AMS_MOTION m,
        NNS_OBJECT nnso,
        object p);

    public delegate bool material_cb_func_delegate(NNS_DRAWCALLBACK_VAL callback, object o);

    public delegate void OBS_OBJECT_WORK_Delegate2(object o1, object o2, uint u);

    public delegate int OBS_OBJECT_WORK_Delegate3(OBS_OBJECT_WORK objectWork);

    public delegate bool OBS_OBJECT_WORK_Delegate4(OBS_OBJECT_WORK objectWork);

    public delegate void OBF_ACT_CALLBACK(object o1, object o2, uint u);

    public delegate void NNS_NODEUSRMOT_CALLBACK_FUNC(NNS_NODEUSRMOT_CALLBACK_VAL val);

    public delegate void AMF_THREAD_PROC();

    public delegate int AmeDelegateFunc(object o);

    public delegate void AmeFieldFunc(
        AMS_AME_ECB ecb,
        AMS_AME_NODE node,
        AMS_AME_RUNTIME_WORK work);

    private delegate void _eve_func_(uint flag, ushort bx, ushort by, int[] r_on, int[] r_off);
    private delegate int ObjDiffCollision_pFunc_delegate(
        int i1,
        int i2,
        ushort us,
        ushort[] usa,
        uint[] uia);
    private delegate int pFunc_Delegate(
        OBS_COLLISION_OBJ pColObj,
        int lPosX,
        int lPosY,
        ushort ucSuf,
        ushort[] pDir,
        uint[] pAttr);

}