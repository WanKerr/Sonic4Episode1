using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    public static AppMain.OBS_OBJECT_WORK GMM_EFFECT_CREATE_WORK(
      AppMain.TaskWorkFactoryDelegate work_size,
      AppMain.OBS_OBJECT_WORK parent_obj,
      ushort sort_prio,
      string name)
    {
        return AppMain.GmEffectCreateWork(work_size, parent_obj, sort_prio);
    }

    private static void GmEffectInit()
    {
    }

    private static void GmEffectExit()
    {
    }

    private static AppMain.OBS_OBJECT_WORK GmEffectCreateWork(
      AppMain.TaskWorkFactoryDelegate work_size,
      AppMain.OBS_OBJECT_WORK parent_obj,
      ushort sort_prio)
    {
        if (work_size == null)
            work_size = AppMain._GmEffectCreateWork_Delegate;
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)(6656U + (uint)sort_prio), (byte)3, (byte)0, (byte)0, work_size, (string)null);
        if (obsObjectWork == null)
            return (AppMain.OBS_OBJECT_WORK)null;
        AppMain.mtTaskChangeTcbDestructor(obsObjectWork.tcb, AppMain._GmEffectDefaultExit);
        obsObjectWork.obj_type = (ushort)5;
        obsObjectWork.ppOut = AppMain._ObjDrawActionSummaryDelegate;
        obsObjectWork.ppOutSub = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppIn = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppMove = AppMain._ObjObjectMoveDelegate;
        obsObjectWork.ppActCall = (AppMain.OBS_OBJECT_WORK_Delegate2)null;
        obsObjectWork.ppRec = AppMain._gmEffectDefaultRecFuncDelegate;
        obsObjectWork.ppLast = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
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

    private static void GmEffectDefaultExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.ObjObjectExit(tcb);
    }

    private static AppMain.GMS_EFFECT_3DES_WORK GmEffect3dESCreateByParam(
      AppMain.GMS_EFFECT_CREATE_PARAM create_param,
      AppMain.OBS_OBJECT_WORK parent_obj,
      object arc,
      AppMain.OBS_DATA_WORK ame_dwork,
      AppMain.OBS_DATA_WORK ambtex_dwork,
      AppMain.OBS_DATA_WORK texlist_dwork,
      AppMain.OBS_DATA_WORK model_dwork,
      AppMain.OBS_DATA_WORK object_dwork)
    {
        return AppMain.GmEffect3dESCreateByParam(create_param, parent_obj, arc, ame_dwork, ambtex_dwork, texlist_dwork, model_dwork, object_dwork, AppMain._GmEffect3dESTaskDelegate);
    }

    private static AppMain.GMS_EFFECT_3DES_WORK GmEffect3dESCreateByParam(
      AppMain.GMS_EFFECT_CREATE_PARAM create_param,
      AppMain.OBS_OBJECT_WORK parent_obj,
      object arc,
      AppMain.OBS_DATA_WORK ame_dwork,
      AppMain.OBS_DATA_WORK ambtex_dwork,
      AppMain.OBS_DATA_WORK texlist_dwork,
      AppMain.OBS_DATA_WORK model_dwork,
      AppMain.OBS_DATA_WORK object_dwork,
      AppMain.TaskWorkFactoryDelegate work_size)
    {
        if (work_size == null)
            work_size = (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DES_WORK());
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK(work_size, parent_obj, (ushort)0, "EF_3DES_CREATE_BY_PARAM");
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = (AppMain.GMS_EFFECT_3DES_WORK)work;
        AppMain.ObjObjectAction3dESEffectLoad(work, efct_3des.obj_3des, ame_dwork, (string)null, create_param.ame_idx, (AppMain.AMS_AMB_HEADER)arc);
        AppMain.ObjObjectAction3dESTextureLoad(work, work.obj_3des, ambtex_dwork, (string)null, 0, (AppMain.AMS_AMB_HEADER)null, false);
        AppMain.ObjObjectAction3dESTextureSetByDwork(work, texlist_dwork);
        if (model_dwork != null && create_param.model_idx != -1)
        {
            AppMain.ObjObjectAction3dESModelLoad(work, work.obj_3des, model_dwork, (string)null, 0, (AppMain.AMS_AMB_HEADER)null, 0U, false);
            if (object_dwork != null)
                AppMain.ObjObjectAction3dESModelSetByDwork(work, object_dwork);
        }
        AppMain.GmEffect3DESSetupBase(efct_3des, create_param.pos_type, create_param.init_flag);
        AppMain.GmEffect3DESSetDispOffset(efct_3des, create_param.disp_ofst.x, create_param.disp_ofst.y, create_param.disp_ofst.z);
        AppMain.GmEffect3DESSetDispRotation(efct_3des, create_param.disp_rot.x, create_param.disp_rot.y, create_param.disp_rot.z);
        AppMain.GmEffect3DESSetScale(efct_3des, create_param.scale);
        work.ppFunc = create_param.main_func;
        return efct_3des;
    }

    private static AppMain.GMS_EFFECT_3DES_WORK GmEffect3dESCreateDummy(
      AppMain.OBS_OBJECT_WORK parent_obj)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DES_WORK()), parent_obj, (ushort)0, "EF_3DES_DUMMY");
        work.disp_flag |= 8U;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (AppMain.GMS_EFFECT_3DES_WORK)work;
        work.obj_3des = gmsEffect3DesWork.obj_3des;
        work.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd);
        return gmsEffect3DesWork;
    }

    private static void GmEffectRectInit(
      AppMain.GMS_EFFECT_COM_WORK efct_com,
      ushort[] atk_flag_tbl,
      ushort[] def_flag_tbl,
      byte my_group,
      byte target_group_flag)
    {
        AppMain.OBS_OBJECT_WORK objWork = efct_com.obj_work;
        AppMain.ObjObjectGetRectBuf(objWork, (AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>)efct_com.rect_work, (ushort)2);
        for (int index = 0; index < 2; ++index)
        {
            AppMain.ObjRectGroupSet(efct_com.rect_work[index], my_group, target_group_flag);
            AppMain.ObjRectAtkSet(efct_com.rect_work[index], atk_flag_tbl[index], (short)1);
            AppMain.ObjRectDefSet(efct_com.rect_work[index], def_flag_tbl[index], (short)0);
            efct_com.rect_work[index].parent_obj = objWork;
            efct_com.rect_work[index].flag &= 4294967291U;
        }
        efct_com.rect_work[0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.GmEffectDefaultDefFunc);
        efct_com.rect_work[1].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.GmEffectDefaultAtkFunc);
    }

    private static void GmEffectDefaultDefFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
    }

    private static void GmEffectDefaultAtkFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
    }

    private static void GmEffect3DESSetupBase(
      AppMain.GMS_EFFECT_3DES_WORK efct_3des,
      uint pos_type,
      uint init_flag)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)efct_3des;
        AppMain.OBS_ACTION3D_ES_WORK obj3des = obsObjectWork.obj_3des;
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
                AppMain.mppAssertNotImpl();
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
        obsObjectWork.ppFunc = AppMain._GmEffectDefaultMainFuncDeleteAtEnd;
    }

    private static void GmEffect3DESChangeBase(
      AppMain.GMS_EFFECT_3DES_WORK efct_3des,
      uint pos_type,
      uint init_flag)
    {
        AppMain.OBS_OBJECT_WORK objWork = efct_3des.efct_com.obj_work;
        AppMain.MPP_VOID_OBS_OBJECT_WORK ppFunc = objWork.ppFunc;
        objWork.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.GmEffect3DESSetupBase(efct_3des, pos_type, init_flag);
        objWork.ppFunc = ppFunc;
    }

    private static void GmEffect3DESSetDispOffset(
      AppMain.GMS_EFFECT_3DES_WORK efct_3des,
      float ofst_x,
      float ofst_y,
      float ofst_z)
    {
        AppMain.amVectorSet(efct_3des.efct_com.obj_work.obj_3des.disp_ofst, ofst_x, ofst_y, ofst_z);
    }

    private static void GmEffect3DESAddDispOffset(
      AppMain.GMS_EFFECT_3DES_WORK efct_3des,
      float ofst_add_x,
      float ofst_add_y,
      float ofst_add_z)
    {
        AppMain.amVectorAdd(efct_3des.efct_com.obj_work.obj_3des.disp_ofst, ofst_add_x, ofst_add_y, ofst_add_z);
    }

    private static void GmEffect3DESSetDispOffsetCircleX(
      AppMain.GMS_EFFECT_3DES_WORK efct_3des,
      float radius,
      short angle)
    {
        AppMain.GmEffect3DESSetDispOffset(efct_3des, 0.0f, radius * AppMain.nnSin((int)angle), radius * AppMain.nnCos((int)angle));
    }

    private static void GmEffect3DESSetDispRotation(
      AppMain.GMS_EFFECT_3DES_WORK efct_3des,
      short rot_x,
      short rot_y,
      short rot_z)
    {
        AppMain.OBS_ACTION3D_ES_WORK obj3des = efct_3des.efct_com.obj_work.obj_3des;
        obj3des.disp_rot.x = (ushort)rot_x;
        obj3des.disp_rot.y = (ushort)rot_y;
        obj3des.disp_rot.z = (ushort)rot_z;
    }

    private static void GmEffect3DESAddDispRotation(
      AppMain.GMS_EFFECT_3DES_WORK efct_3des,
      short rot_add_x,
      short rot_add_y,
      short rot_add_z)
    {
        AppMain.OBS_ACTION3D_ES_WORK obj3des = efct_3des.efct_com.obj_work.obj_3des;
        obj3des.disp_rot.x = (ushort)(short)((long)ushort.MaxValue & (long)((int)obj3des.disp_rot.x + (int)rot_add_x));
        obj3des.disp_rot.y = (ushort)(short)((long)ushort.MaxValue & (long)((int)obj3des.disp_rot.y + (int)rot_add_y));
        obj3des.disp_rot.z = (ushort)(short)((long)ushort.MaxValue & (long)((int)obj3des.disp_rot.z + (int)rot_add_z));
    }

    private static void GmEffect3DESSetDuplicateDraw(
      AppMain.GMS_EFFECT_3DES_WORK efct_3des,
      float ofst_x,
      float ofst_y,
      float ofst_z)
    {
        AppMain.OBS_ACTION3D_ES_WORK obj3des = efct_3des.efct_com.obj_work.obj_3des;
        AppMain.amVectorSet(obj3des.dup_draw_ofst, ofst_x, ofst_y, ofst_z);
        obj3des.flag |= 64U;
    }

    private static void GmEffect3DESClearDuplicateDraw(AppMain.GMS_EFFECT_3DES_WORK efct_3des)
    {
        AppMain.OBS_ACTION3D_ES_WORK obj3des = efct_3des.efct_com.obj_work.obj_3des;
        AppMain.amVectorInit(obj3des.dup_draw_ofst);
        obj3des.flag &= 4294967231U;
    }

    private static void GmEffectDefaultMainFuncDeleteAtEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (obj_work.parent_obj == null)
            return;
        obj_work.dir.z = obj_work.parent_obj.dir.z;
    }

    private static void gmEffectDefaultRecFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void GmEffect3DESSetScale(AppMain.GMS_EFFECT_3DES_WORK efct_3des, float scale_rate)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)efct_3des;
        obsObjectWork.scale.x = obsObjectWork.scale.y = obsObjectWork.scale.z = AppMain.FX_F32_TO_FX32(scale_rate);
    }

}