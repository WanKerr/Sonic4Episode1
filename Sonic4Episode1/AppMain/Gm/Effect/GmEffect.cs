public partial class AppMain
{
    public static OBS_OBJECT_WORK GMM_EFFECT_CREATE_WORK(
      TaskWorkFactoryDelegate work_size,
      OBS_OBJECT_WORK parent_obj,
      ushort sort_prio,
      string name)
    {
        return GmEffectCreateWork(work_size, parent_obj, sort_prio);
    }

    private static void GmEffectInit()
    {
    }

    private static void GmEffectExit()
    {
    }

    private static OBS_OBJECT_WORK GmEffectCreateWork(
      TaskWorkFactoryDelegate work_size,
      OBS_OBJECT_WORK parent_obj,
      ushort sort_prio)
    {
        if (work_size == null)
            work_size = _GmEffectCreateWork_Delegate;
        OBS_OBJECT_WORK obsObjectWork = OBM_OBJECT_TASK_DETAIL_INIT((ushort)(6656U + sort_prio), 3, 0, 0, work_size, null);
        if (obsObjectWork == null)
            return null;
        mtTaskChangeTcbDestructor(obsObjectWork.tcb, _GmEffectDefaultExit);
        obsObjectWork.obj_type = 5;
        obsObjectWork.ppOut = _ObjDrawActionSummaryDelegate;
        obsObjectWork.ppOutSub = null;
        obsObjectWork.ppIn = null;
        obsObjectWork.ppMove = _ObjObjectMoveDelegate;
        obsObjectWork.ppActCall = null;
        obsObjectWork.ppRec = _gmEffectDefaultRecFuncDelegate;
        obsObjectWork.ppLast = null;
        obsObjectWork.spd_fall = 672;
        obsObjectWork.spd_fall_max = 61440;
        if (parent_obj != null)
        {
            obsObjectWork.parent_obj = parent_obj;
            obsObjectWork.pos.x = parent_obj.pos.x;
            obsObjectWork.pos.y = parent_obj.pos.y;
            obsObjectWork.pos.z = parent_obj.pos.z;
        }
        obsObjectWork.disp_flag |= 256U;
        obsObjectWork.flag |= 18U;
        obsObjectWork.move_flag |= 256U;
        obsObjectWork.flag |= 1U;
        return obsObjectWork;
    }

    private static void GmEffectDefaultExit(MTS_TASK_TCB tcb)
    {
        ObjObjectExit(tcb);
    }

    private static GMS_EFFECT_3DES_WORK GmEffect3dESCreateByParam(
      GMS_EFFECT_CREATE_PARAM create_param,
      OBS_OBJECT_WORK parent_obj,
      object arc,
      OBS_DATA_WORK ame_dwork,
      OBS_DATA_WORK ambtex_dwork,
      OBS_DATA_WORK texlist_dwork,
      OBS_DATA_WORK model_dwork,
      OBS_DATA_WORK object_dwork)
    {
        return GmEffect3dESCreateByParam(create_param, parent_obj, arc, ame_dwork, ambtex_dwork, texlist_dwork, model_dwork, object_dwork, _GmEffect3dESTaskDelegate);
    }

    private static GMS_EFFECT_3DES_WORK GmEffect3dESCreateByParam(
      GMS_EFFECT_CREATE_PARAM create_param,
      OBS_OBJECT_WORK parent_obj,
      object arc,
      OBS_DATA_WORK ame_dwork,
      OBS_DATA_WORK ambtex_dwork,
      OBS_DATA_WORK texlist_dwork,
      OBS_DATA_WORK model_dwork,
      OBS_DATA_WORK object_dwork,
      TaskWorkFactoryDelegate work_size)
    {
        if (work_size == null)
            work_size = () => new GMS_EFFECT_3DES_WORK();
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(work_size, parent_obj, 0, "EF_3DES_CREATE_BY_PARAM");
        GMS_EFFECT_3DES_WORK efct_3des = (GMS_EFFECT_3DES_WORK)work;
        ObjObjectAction3dESEffectLoad(work, efct_3des.obj_3des, ame_dwork, null, create_param.ame_idx, (AMS_AMB_HEADER)arc);
        ObjObjectAction3dESTextureLoad(work, work.obj_3des, ambtex_dwork, null, 0, null, false);
        ObjObjectAction3dESTextureSetByDwork(work, texlist_dwork);
        if (model_dwork != null && create_param.model_idx != -1)
        {
            ObjObjectAction3dESModelLoad(work, work.obj_3des, model_dwork, null, 0, null, 0U, false);
            if (object_dwork != null)
                ObjObjectAction3dESModelSetByDwork(work, object_dwork);
        }
        GmEffect3DESSetupBase(efct_3des, create_param.pos_type, create_param.init_flag);
        GmEffect3DESSetDispOffset(efct_3des, create_param.disp_ofst.x, create_param.disp_ofst.y, create_param.disp_ofst.z);
        GmEffect3DESSetDispRotation(efct_3des, create_param.disp_rot.x, create_param.disp_rot.y, create_param.disp_rot.z);
        GmEffect3DESSetScale(efct_3des, create_param.scale);
        work.ppFunc = create_param.main_func;
        return efct_3des;
    }

    private static GMS_EFFECT_3DES_WORK GmEffect3dESCreateDummy(
      OBS_OBJECT_WORK parent_obj)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DES_WORK(), parent_obj, 0, "EF_3DES_DUMMY");
        work.disp_flag |= 8U;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (GMS_EFFECT_3DES_WORK)work;
        work.obj_3des = gmsEffect3DesWork.obj_3des;
        work.ppOut = null;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEnd);
        return gmsEffect3DesWork;
    }

    private static void GmEffectRectInit(
      GMS_EFFECT_COM_WORK efct_com,
      ushort[] atk_flag_tbl,
      ushort[] def_flag_tbl,
      byte my_group,
      byte target_group_flag)
    {
        OBS_OBJECT_WORK objWork = efct_com.obj_work;
        ObjObjectGetRectBuf(objWork, efct_com.rect_work, 2);
        for (int index = 0; index < 2; ++index)
        {
            ObjRectGroupSet(efct_com.rect_work[index], my_group, target_group_flag);
            ObjRectAtkSet(efct_com.rect_work[index], atk_flag_tbl[index], 1);
            ObjRectDefSet(efct_com.rect_work[index], def_flag_tbl[index], 0);
            efct_com.rect_work[index].parent_obj = objWork;
            efct_com.rect_work[index].flag &= 4294967291U;
        }
        efct_com.rect_work[0].ppDef = new OBS_RECT_WORK_Delegate1(GmEffectDefaultDefFunc);
        efct_com.rect_work[1].ppHit = new OBS_RECT_WORK_Delegate1(GmEffectDefaultAtkFunc);
    }

    private static void GmEffectDefaultDefFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
    }

    private static void GmEffectDefaultAtkFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
    }

    private static void GmEffect3DESSetupBase(
      GMS_EFFECT_3DES_WORK efct_3des,
      uint pos_type,
      uint init_flag)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)efct_3des;
        OBS_ACTION3D_ES_WORK obj3des = obsObjectWork.obj_3des;
        efct_3des.saved_pos_type = pos_type;
        efct_3des.saved_init_flag = init_flag;
        switch (pos_type)
        {
            case 0:
                obj3des.flag &= 4294967294U;
                obj3des.flag &= 4294967293U;
                break;
            case 1:
                obj3des.flag |= 1U;
                obj3des.flag &= 4294967293U;
                break;
            case 2:
                obj3des.flag |= 1U;
                obj3des.flag |= 2U;
                break;
            default:
                mppAssertNotImpl();
                break;
        }
        if (((int)init_flag & 1) != 0)
            obsObjectWork.disp_flag |= 4194304U;
        else
            obsObjectWork.disp_flag &= 4290772991U;
        if (((int)init_flag & 2) != 0)
            obsObjectWork.flag |= 1024U;
        else
            obsObjectWork.flag &= 4294966271U;
        if (((int)init_flag & 4) != 0)
            obj3des.flag |= 8U;
        else
            obj3des.flag &= 4294967287U;
        if (((int)init_flag & 16) != 0)
            obsObjectWork.disp_flag &= 4294967039U;
        else
            obsObjectWork.disp_flag |= 256U;
        if (((int)init_flag & 64) != 0)
            obj3des.flag |= 16U;
        else
            obj3des.flag &= 4294967279U;
        if (((int)init_flag & 32) != 0)
            obsObjectWork.flag &= 4294443007U;
        else
            obsObjectWork.flag |= 524288U;
        obsObjectWork.ppFunc = _GmEffectDefaultMainFuncDeleteAtEnd;
    }

    private static void GmEffect3DESChangeBase(
      GMS_EFFECT_3DES_WORK efct_3des,
      uint pos_type,
      uint init_flag)
    {
        OBS_OBJECT_WORK objWork = efct_3des.efct_com.obj_work;
        MPP_VOID_OBS_OBJECT_WORK ppFunc = objWork.ppFunc;
        objWork.ppFunc = null;
        GmEffect3DESSetupBase(efct_3des, pos_type, init_flag);
        objWork.ppFunc = ppFunc;
    }

    private static void GmEffect3DESSetDispOffset(
      GMS_EFFECT_3DES_WORK efct_3des,
      float ofst_x,
      float ofst_y,
      float ofst_z)
    {
        amVectorSet(efct_3des.efct_com.obj_work.obj_3des.disp_ofst, ofst_x, ofst_y, ofst_z);
    }

    private static void GmEffect3DESAddDispOffset(
      GMS_EFFECT_3DES_WORK efct_3des,
      float ofst_add_x,
      float ofst_add_y,
      float ofst_add_z)
    {
        amVectorAdd(efct_3des.efct_com.obj_work.obj_3des.disp_ofst, ofst_add_x, ofst_add_y, ofst_add_z);
    }

    private static void GmEffect3DESSetDispOffsetCircleX(
      GMS_EFFECT_3DES_WORK efct_3des,
      float radius,
      short angle)
    {
        GmEffect3DESSetDispOffset(efct_3des, 0.0f, radius * nnSin(angle), radius * nnCos(angle));
    }

    private static void GmEffect3DESSetDispRotation(
      GMS_EFFECT_3DES_WORK efct_3des,
      short rot_x,
      short rot_y,
      short rot_z)
    {
        OBS_ACTION3D_ES_WORK obj3des = efct_3des.efct_com.obj_work.obj_3des;
        obj3des.disp_rot.x = (ushort)rot_x;
        obj3des.disp_rot.y = (ushort)rot_y;
        obj3des.disp_rot.z = (ushort)rot_z;
    }

    private static void GmEffect3DESAddDispRotation(
      GMS_EFFECT_3DES_WORK efct_3des,
      short rot_add_x,
      short rot_add_y,
      short rot_add_z)
    {
        OBS_ACTION3D_ES_WORK obj3des = efct_3des.efct_com.obj_work.obj_3des;
        obj3des.disp_rot.x = (ushort)(short)(ushort.MaxValue & (long)(obj3des.disp_rot.x + rot_add_x));
        obj3des.disp_rot.y = (ushort)(short)(ushort.MaxValue & (long)(obj3des.disp_rot.y + rot_add_y));
        obj3des.disp_rot.z = (ushort)(short)(ushort.MaxValue & (long)(obj3des.disp_rot.z + rot_add_z));
    }

    private static void GmEffect3DESSetDuplicateDraw(
      GMS_EFFECT_3DES_WORK efct_3des,
      float ofst_x,
      float ofst_y,
      float ofst_z)
    {
        OBS_ACTION3D_ES_WORK obj3des = efct_3des.efct_com.obj_work.obj_3des;
        amVectorSet(obj3des.dup_draw_ofst, ofst_x, ofst_y, ofst_z);
        obj3des.flag |= 64U;
    }

    private static void GmEffect3DESClearDuplicateDraw(GMS_EFFECT_3DES_WORK efct_3des)
    {
        OBS_ACTION3D_ES_WORK obj3des = efct_3des.efct_com.obj_work.obj_3des;
        amVectorInit(obj3des.dup_draw_ofst);
        obj3des.flag &= 4294967231U;
    }

    public static void GmEffectDefaultMainFuncDeleteAtEnd(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (obj_work.parent_obj == null)
            return;
        obj_work.dir.z = obj_work.parent_obj.dir.z;
    }

    private static void gmEffectDefaultRecFunc(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void GmEffect3DESSetScale(GMS_EFFECT_3DES_WORK efct_3des, float scale_rate)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)efct_3des;
        obsObjectWork.scale.x = obsObjectWork.scale.y = obsObjectWork.scale.z = FX_F32_TO_FX32(scale_rate);
    }

}