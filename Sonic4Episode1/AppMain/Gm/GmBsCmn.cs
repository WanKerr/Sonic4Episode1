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
    private static AppMain.OBS_OBJECT_WORK GmBsCmnGetPlayerObj()
    {
        return (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_main_system.ply_work[0];
    }

    private static int GmBsCmnIsFinalZoneType(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return AppMain.GsGetMainSysInfo().stage_id == (ushort)16 ? 1 : 0;
    }

    private static void GmBsCmnSetAction(AppMain.OBS_OBJECT_WORK obj_work, int act_id, int is_repeat)
    {
        AppMain.GmBsCmnSetAction(obj_work, act_id, is_repeat, 0);
    }

    private static void GmBsCmnSetAction(
      AppMain.OBS_OBJECT_WORK obj_work,
      int act_id,
      int is_repeat,
      int is_blend)
    {
        if (is_blend != 0)
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, act_id);
        else
            AppMain.ObjDrawObjectActionSet(obj_work, act_id);
        if (is_repeat != 0)
            obj_work.disp_flag |= 4U;
        else
            obj_work.disp_flag &= 4294967291U;
    }

    private static int GmBsCmnIsActionEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.disp_flag & 8) != 0 ? 1 : 0;
    }

    private static void GmBsCmnSetObjSpd(AppMain.OBS_OBJECT_WORK obj_work, int spd_x, int spd_y)
    {
        AppMain.GmBsCmnSetObjSpd(obj_work, spd_x, spd_y, 0);
    }

    private static void GmBsCmnSetObjSpd(
      AppMain.OBS_OBJECT_WORK obj_work,
      int spd_x,
      int spd_y,
      int spd_z)
    {
        obj_work.spd.x = spd_x;
        obj_work.spd.y = spd_y;
        obj_work.spd.z = spd_z;
    }

    private static void GmBsCmnSetObjSpdZero(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.x = obj_work.spd.y = obj_work.spd.z = 0;
        obj_work.spd_add.x = obj_work.spd_add.y = obj_work.spd_add.z = 0;
    }

    private static int GmBsCmnIsActionEndPrecisely(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.disp_flag & 8) != 0 ? 1 : AppMain.gmBsCmnCheckActionFrameOverrunOnNextUpdate(obj_work);
    }

    private static int GmBsCmnIsActionEndFlexibly(AppMain.OBS_OBJECT_WORK obj_work, float allow_ratio)
    {
        float num = obj_work.obj_3d.speed[0] * AppMain.FX_FX32_TO_F32(AppMain.g_obj.speed);
        float overrun_frame = 0.0f;
        if (((int)obj_work.disp_flag & 8) != 0)
            return 1;
        AppMain.gmBsCmnCheckActionFrameOverrunOnNextUpdate(obj_work, ref overrun_frame);
        return (double)overrun_frame > (double)num * (double)allow_ratio ? AppMain.GmBsCmnIsActionEndPrecisely(obj_work) : AppMain.GmBsCmnIsActionEnd(obj_work);
    }

    private static void GmBsCmnSetEfctAtkVsPly(
      AppMain.GMS_EFFECT_COM_WORK efct_com,
      short view_out_ofst)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)efct_com;
        obsObjectWork.flag &= 4294967277U;
        obsObjectWork.move_flag |= 256U;
        obsObjectWork.view_out_ofst = view_out_ofst;
        AppMain.GmEffectRectInit(efct_com, AppMain.gm_bs_cmn_efct_atk_flag_tbl, AppMain.gm_bs_cmn_efct_def_flag_tbl, (byte)1, (byte)1);
    }

    private static int GmBsCmnCheckRectMajorOverlapH(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect,
      ref int center_ofst_x)
    {
        int lLeft1 = 0;
        int lLeft2 = 0;
        ushort usWidth1 = 0;
        ushort usWidth2 = 0;
        int num1 = 0;
        AppMain.ObjRectLTBSet(my_rect, ref lLeft1, new int?(), new int?());
        AppMain.ObjRectWHDSet(my_rect, ref usWidth1, new ushort?(), new ushort?());
        int num2 = lLeft1 + (int)usWidth1;
        int num3 = lLeft1 + ((int)usWidth1 >> 1);
        AppMain.ObjRectLTBSet(your_rect, ref lLeft2, new int?(), new int?());
        AppMain.ObjRectWHDSet(your_rect, ref usWidth2, new ushort?(), new ushort?());
        int num4 = lLeft2 + (int)usWidth2;
        int num5 = lLeft2 + ((int)usWidth2 >> 1);
        if (num3 < num5)
        {
            if (num2 >= num5 || num3 >= lLeft2)
                num1 = 1;
        }
        else if (num5 < num3)
        {
            if (num4 >= num3 || num5 >= lLeft1)
                num1 = 1;
        }
        else
            num1 = 1;
        if (center_ofst_x != 0)
            center_ofst_x = num5 - num3 << 12;
        return num1;
    }

    private static int GmBsCmnCheckRectMajorOverlapV(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect,
      ref int center_ofst_y)
    {
        int lTop1 = 0;
        int lTop2 = 0;
        ushort usHeight1 = 0;
        int num1 = 0;
        AppMain.ObjRectLTBSet(my_rect, new int?(), ref lTop1, new int?());
        AppMain.ObjRectWHDSet(my_rect, new ushort?(), ref usHeight1, new ushort?());
        int num2 = lTop1 + (int)usHeight1;
        int num3 = lTop1 + ((int)usHeight1 >> 1);
        AppMain.ObjRectLTBSet(your_rect, new int?(), ref lTop2, new int?());
        ushort usHeight2 = 0;
        AppMain.ObjRectWHDSet(your_rect, new ushort?(), ref usHeight2, new ushort?());
        ushort num4 = usHeight2;
        int num5 = lTop2 + (int)num4;
        int num6 = lTop2 + ((int)num4 >> 1);
        if (num3 < num6)
        {
            if (num2 >= num6 || num3 >= lTop2)
                num1 = 1;
        }
        else if (num6 < num3)
        {
            if (num5 >= num3 || num6 >= lTop1)
                num1 = 1;
        }
        else
            num1 = 1;
        if (center_ofst_y != 0)
            center_ofst_y = num6 - num3 << 12;
        return num1;
    }

    private uint GmBsCmnCheckRectHitSideHFirst(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        int center_ofst_x = 0;
        int center_ofst_y = 0;
        int num = AppMain.GmBsCmnCheckRectMajorOverlapH(my_rect, your_rect, ref center_ofst_x);
        AppMain.GmBsCmnCheckRectMajorOverlapV(my_rect, your_rect, ref center_ofst_y);
        return num != 0 ? (center_ofst_y < 0 ? AppMain.GMD_BS_CMN_RECT_HIT_SIDE_LEFT : AppMain.GMD_BS_CMN_RECT_HIT_SIDE_RIGHT) : (center_ofst_x < 0 ? AppMain.GMD_BS_CMN_RECT_HIT_SIDE_TOP : AppMain.GMD_BS_CMN_RECT_HIT_SIDE_BOTTOM);
    }

    public static uint GmBsCmnCheckRectHitSideVFirst(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        int center_ofst_x = 0;
        int center_ofst_y = 0;
        int num = AppMain.GmBsCmnCheckRectMajorOverlapV(my_rect, your_rect, ref center_ofst_y);
        AppMain.GmBsCmnCheckRectMajorOverlapH(my_rect, your_rect, ref center_ofst_x);
        return num != 0 ? (center_ofst_x < 0 ? AppMain.GMD_BS_CMN_RECT_HIT_SIDE_LEFT : AppMain.GMD_BS_CMN_RECT_HIT_SIDE_RIGHT) : (center_ofst_y < 0 ? AppMain.GMD_BS_CMN_RECT_HIT_SIDE_TOP : AppMain.GMD_BS_CMN_RECT_HIT_SIDE_BOTTOM);
    }

    private static void GmBsCmnInitBossMotionCBSystem(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_BMCB_MGR bmcb_mgr)
    {
        bmcb_mgr.Clear();
        bmcb_mgr.bmcb_head.next = bmcb_mgr.bmcb_tail;
        bmcb_mgr.bmcb_head.prev = (AppMain.GMS_BS_CMN_BMCB_LINK)null;
        bmcb_mgr.bmcb_tail.next = (AppMain.GMS_BS_CMN_BMCB_LINK)null;
        bmcb_mgr.bmcb_tail.prev = bmcb_mgr.bmcb_head;
        obj_work.obj_3d.mtn_cb_func = new AppMain.mtn_cb_func_delegate(AppMain.gmBsCmnBossMotionCallbackFunc);
        obj_work.obj_3d.mtn_cb_param = (object)bmcb_mgr;
    }

    private static void GmBsCmnClearBossMotionCBSystem(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BS_CMN_BMCB_MGR mtnCbParam = (AppMain.GMS_BS_CMN_BMCB_MGR)obj_work.obj_3d.mtn_cb_param;
        if (mtnCbParam == null)
            return;
        AppMain.GMS_BS_CMN_BMCB_LINK next;
        for (AppMain.GMS_BS_CMN_BMCB_LINK gmsBsCmnBmcbLink = mtnCbParam.bmcb_head.next; gmsBsCmnBmcbLink != null; gmsBsCmnBmcbLink = next)
        {
            next = gmsBsCmnBmcbLink.next;
            gmsBsCmnBmcbLink.next = (AppMain.GMS_BS_CMN_BMCB_LINK)null;
            gmsBsCmnBmcbLink.prev = (AppMain.GMS_BS_CMN_BMCB_LINK)null;
            if (gmsBsCmnBmcbLink.bmcb_func == null)
                break;
        }
        mtnCbParam.bmcb_head.next = mtnCbParam.bmcb_head.prev = (AppMain.GMS_BS_CMN_BMCB_LINK)null;
        mtnCbParam.bmcb_tail.next = mtnCbParam.bmcb_tail.prev = (AppMain.GMS_BS_CMN_BMCB_LINK)null;
        mtnCbParam.Clear();
        obj_work.obj_3d.mtn_cb_func = (AppMain.mtn_cb_func_delegate)null;
        obj_work.obj_3d.mtn_cb_param = (object)null;
    }

    private static void GmBsCmnAppendBossMotionCallback(
      AppMain.GMS_BS_CMN_BMCB_MGR bmcb_mgr,
      AppMain.GMS_BS_CMN_BMCB_LINK bmcb_link)
    {
        bmcb_link.prev = bmcb_mgr.bmcb_tail.prev;
        bmcb_link.prev.next = bmcb_link;
        bmcb_link.next = bmcb_mgr.bmcb_tail;
        bmcb_mgr.bmcb_tail.prev = bmcb_link;
    }

    private static void GmBsCmnCreateSNMWork(
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      AppMain.NNS_OBJECT _object,
      ushort reg_max)
    {
        AppMain.UNREFERENCED_PARAMETER((object)_object);
        AppMain.gmBsCmnInitBossMotionCBLink(snm_work.bmcb_link, new AppMain.MPP_VOID_MOTION_NSSOBJECT_OBJECT(AppMain.gmBsCmnMotionCallbackStoreNodeMatrix), (object)snm_work);
        snm_work.reg_node_cnt = (ushort)0;
        snm_work.reg_node_max = reg_max;
        snm_work.node_info_list = AppMain.New<AppMain.GMS_BS_CMN_SNM_NODE_INFO>((int)reg_max);
    }

    private static void GmBsCmnDeleteSNMWork(AppMain.GMS_BS_CMN_SNM_WORK snm_work)
    {
        AppMain.gmBsCmnClearBossMotionCBLink(snm_work.bmcb_link);
        snm_work.reg_node_cnt = (ushort)0;
        snm_work.reg_node_max = (ushort)0;
        if (snm_work.node_info_list == null)
            return;
        snm_work.node_info_list = (AppMain.GMS_BS_CMN_SNM_NODE_INFO[])null;
    }

    private static int GmBsCmnRegisterSNMNode(AppMain.GMS_BS_CMN_SNM_WORK snm_work, int node_index)
    {
        snm_work.node_info_list[(int)snm_work.reg_node_cnt].node_index = node_index;
        int regNodeCnt = (int)snm_work.reg_node_cnt;
        ++snm_work.reg_node_cnt;
        return regNodeCnt;
    }

    private static AppMain.NNS_MATRIX GmBsCmnGetSNMMtx(
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id)
    {
        return snm_work.node_info_list[snm_reg_id].node_w_mtx;
    }

    private static void GmBsCmnUpdateObjectGeneralStuckWithNode(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      AppMain.NNS_MATRIX ofst_mtx)
    {
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(snm_work, snm_reg_id);
        obj_work.pos.x = AppMain.FX_F32_TO_FX32(snmMtx.M03);
        obj_work.pos.y = -AppMain.FX_F32_TO_FX32(snmMtx.M13);
        obj_work.pos.z = AppMain.FX_F32_TO_FX32(snmMtx.M23);
        if (ofst_mtx == null)
            return;
        AppMain.VEC_Set(ref obj_work.pos, obj_work.pos.x + AppMain.FX_F32_TO_FX32(ofst_mtx.M03), obj_work.pos.y - AppMain.FX_F32_TO_FX32(ofst_mtx.M13), obj_work.pos.z + AppMain.FX_F32_TO_FX32(ofst_mtx.M23));
    }

    private static void GmBsCmnUpdateObjectGeneralStuckWithNodeRelative(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      AppMain.VecFx32 pivot_cur_pos,
      AppMain.VecFx32 pivot_prev_pos)
    {
        AppMain.GmBsCmnUpdateObjectGeneralStuckWithNodeRelative(obj_work, snm_work, snm_reg_id, pivot_cur_pos, pivot_prev_pos, (AppMain.NNS_MATRIX)null);
    }

    private static void GmBsCmnUpdateObjectGeneralStuckWithNodeRelative(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      AppMain.VecFx32 pivot_cur_pos,
      AppMain.VecFx32 pivot_prev_pos,
      AppMain.NNS_MATRIX ofst_mtx)
    {
        AppMain.GmBsCmnUpdateObjectGeneralStuckWithNode(obj_work, snm_work, snm_reg_id, ofst_mtx);
        AppMain.VEC_Set(ref obj_work.pos, obj_work.pos.x - pivot_prev_pos.x + pivot_cur_pos.x, obj_work.pos.y - pivot_prev_pos.y + pivot_cur_pos.y, obj_work.pos.z - pivot_prev_pos.z + pivot_cur_pos.z);
    }

    private static void GmBsCmnUpdateObject3DNNStuckWithNode(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation)
    {
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, snm_work, snm_reg_id, b_rotation, (AppMain.NNS_MATRIX)null);
    }

    private static void GmBsCmnUpdateObject3DNNStuckWithNode(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation,
      AppMain.NNS_MATRIX ofst_mtx)
    {
        AppMain.NNS_MATRIX userObjMtxR = obj_work.obj_3d.user_obj_mtx_r;
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(snm_work, snm_reg_id);
        obj_work.pos.x = AppMain.FX_F32_TO_FX32(snmMtx.M03);
        obj_work.pos.y = -AppMain.FX_F32_TO_FX32(snmMtx.M13);
        obj_work.pos.z = AppMain.FX_F32_TO_FX32(snmMtx.M23);
        if (b_rotation != 0)
        {
            obj_work.disp_flag |= 16777216U;
            AppMain.AkMathNormalizeMtx(userObjMtxR, snmMtx);
        }
        else
        {
            obj_work.disp_flag &= 4278190079U;
            AppMain.nnMakeUnitMatrix(userObjMtxR);
        }
        if (ofst_mtx == null)
            return;
        AppMain.NNS_MATRIX stuckWithNodeRotMtx = AppMain.GmBsCmnUpdateObject3DNNStuckWithNode_rot_mtx;
        AppMain.NNS_VECTOR dst = new AppMain.NNS_VECTOR();
        AppMain.NNS_MATRIX withNodeNodeWRot = AppMain.GmBsCmnUpdateObject3DNNStuckWithNode_node_w_rot;
        AppMain.nnCopyMatrix(withNodeNodeWRot, snmMtx);
        withNodeNodeWRot.M03 = withNodeNodeWRot.M13 = withNodeNodeWRot.M23 = 0.0f;
        AppMain.nnMultiplyMatrix(withNodeNodeWRot, withNodeNodeWRot, ofst_mtx);
        AppMain.nnCopyMatrixTranslationVector(dst, withNodeNodeWRot);
        AppMain.VEC_Set(ref obj_work.pos, obj_work.pos.x + AppMain.FX_F32_TO_FX32(dst.x), obj_work.pos.y - AppMain.FX_F32_TO_FX32(dst.y), obj_work.pos.z + AppMain.FX_F32_TO_FX32(dst.z));
        AppMain.nnCopyMatrix(stuckWithNodeRotMtx, ofst_mtx);
        stuckWithNodeRotMtx.M03 = stuckWithNodeRotMtx.M13 = stuckWithNodeRotMtx.M23 = 0.0f;
        obj_work.disp_flag |= 16777216U;
        AppMain.nnMultiplyMatrix(userObjMtxR, userObjMtxR, stuckWithNodeRotMtx);
    }

    private static void GmBsCmnUpdateObject3DNNStuckWithNodeRelative(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation,
      AppMain.VecFx32 pivot_cur_pos,
      AppMain.VecFx32 pivot_prev_pos,
      AppMain.NNS_MATRIX ofst_mtx)
    {
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, snm_work, snm_reg_id, b_rotation, ofst_mtx);
        AppMain.VEC_Set(ref obj_work.pos, obj_work.pos.x - pivot_prev_pos.x + pivot_cur_pos.x, obj_work.pos.y - pivot_prev_pos.y + pivot_cur_pos.y, obj_work.pos.z - pivot_prev_pos.z + pivot_cur_pos.z);
    }

    private static void GmBsCmnUpdateObject3DESStuckWithNode(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation)
    {
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, snm_work, snm_reg_id, b_rotation, (AppMain.NNS_MATRIX)null);
    }

    private static void GmBsCmnUpdateObject3DESStuckWithNode(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation,
      AppMain.NNS_MATRIX ofst_mtx)
    {
        AppMain.NNS_MATRIX stuckWithNodeNmlWMtx = AppMain.GmBsCmnUpdateObject3DESStuckWithNode_nml_w_mtx;
        AppMain.NNS_QUATERNION dst1 = obj_work.obj_3des.user_dir_quat;
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(snm_work, snm_reg_id);
        obj_work.pos.x = AppMain.FX_F32_TO_FX32(snmMtx.M03);
        obj_work.pos.y = -AppMain.FX_F32_TO_FX32(snmMtx.M13);
        obj_work.pos.z = AppMain.FX_F32_TO_FX32(snmMtx.M23);
        if (b_rotation != 0)
        {
            obj_work.obj_3des.flag |= 32U;
            AppMain.AkMathNormalizeMtx(stuckWithNodeNmlWMtx, snmMtx);
            AppMain.nnMakeRotateMatrixQuaternion(out dst1, stuckWithNodeNmlWMtx);
        }
        else
        {
            obj_work.obj_3des.flag &= 4294967263U;
            AppMain.nnMakeUnitQuaternion(ref dst1);
            AppMain.nnMakeUnitMatrix(stuckWithNodeNmlWMtx);
        }
        if (ofst_mtx != null)
        {
            AppMain.NNS_MATRIX stuckWithNodeRotMtx = AppMain.GmBsCmnUpdateObject3DESStuckWithNode_rot_mtx;
            AppMain.NNS_QUATERNION dst2 = new AppMain.NNS_QUATERNION();
            AppMain.NNS_VECTOR dst3 = new AppMain.NNS_VECTOR();
            AppMain.NNS_MATRIX withNodeNodeWRot = AppMain.GmBsCmnUpdateObject3DESStuckWithNode_node_w_rot;
            AppMain.nnCopyMatrix(withNodeNodeWRot, snmMtx);
            withNodeNodeWRot.M03 = withNodeNodeWRot.M13 = withNodeNodeWRot.M23 = 0.0f;
            AppMain.nnMultiplyMatrix(withNodeNodeWRot, withNodeNodeWRot, ofst_mtx);
            AppMain.nnCopyMatrixTranslationVector(dst3, withNodeNodeWRot);
            AppMain.VEC_Set(ref obj_work.pos, obj_work.pos.x + AppMain.FX_F32_TO_FX32(dst3.x), obj_work.pos.y - AppMain.FX_F32_TO_FX32(dst3.y), obj_work.pos.z + AppMain.FX_F32_TO_FX32(dst3.z));
            AppMain.AkMathNormalizeMtx(stuckWithNodeRotMtx, ofst_mtx);
            AppMain.nnMakeRotateMatrixQuaternion(out dst2, stuckWithNodeRotMtx);
            obj_work.obj_3des.flag |= 32U;
            AppMain.nnMultiplyQuaternion(ref dst1, ref dst1, ref dst2);
        }
        obj_work.obj_3des.user_dir_quat.Assign(dst1);
    }

    private static void GmBsCmnUpdateObject3DESStuckWithNodeRelative(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation,
      AppMain.VecFx32 pivot_cur_pos,
      AppMain.VecFx32 pivot_prev_pos)
    {
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, snm_work, snm_reg_id, b_rotation, pivot_cur_pos, pivot_prev_pos, (AppMain.NNS_MATRIX)null);
    }

    private static void GmBsCmnUpdateObject3DESStuckWithNodeRelative(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation,
      AppMain.VecFx32 pivot_cur_pos,
      AppMain.VecFx32 pivot_prev_pos,
      AppMain.NNS_MATRIX ofst_mtx)
    {
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, snm_work, snm_reg_id, b_rotation, ofst_mtx);
        AppMain.VEC_Set(ref obj_work.pos, obj_work.pos.x - pivot_prev_pos.x + pivot_cur_pos.x, obj_work.pos.y - pivot_prev_pos.y + pivot_cur_pos.y, obj_work.pos.z - pivot_prev_pos.z + pivot_cur_pos.z);
    }

    private static void GmBsCmnInitCNMCb(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work)
    {
        obj_work.obj_3d.mplt_cb_func = new AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT(AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix);
        obj_work.obj_3d.mplt_cb_param = (object)null;
    }

    private static void GmBsCmnClearCNMCb(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d.mplt_cb_func = (AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT)null;
        obj_work.obj_3d.mplt_cb_param = (object)null;
    }

    private static void GmBsCmnCreateCNMMgrWork(
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      AppMain.NNS_OBJECT _object,
      ushort reg_max)
    {
        AppMain.UNREFERENCED_PARAMETER((object)_object);
        cnm_mgr_work.reg_node_cnt = (ushort)0;
        cnm_mgr_work.reg_node_max = reg_max;
        cnm_mgr_work.node_info_list = AppMain.New<AppMain.GMS_BS_CMN_CNM_NODE_INFO>((int)reg_max);
    }

    private static void GmBsCmnDeleteCNMMgrWork(AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work)
    {
        cnm_mgr_work.reg_node_cnt = (ushort)0;
        cnm_mgr_work.reg_node_max = (ushort)0;
        if (cnm_mgr_work.node_info_list == null)
            return;
        cnm_mgr_work.node_info_list = (AppMain.GMS_BS_CMN_CNM_NODE_INFO[])null;
    }

    private static void GmBsCmnUpdateCNMParam(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work)
    {
        if (obj_work.obj_3d.mplt_cb_func == null)
            return;
        AppMain.amDrawAlloc_GMS_BS_CMN_CNM_PARAM().reg_node_cnt = cnm_mgr_work.reg_node_cnt;
        AppMain.GMS_BS_CMN_CNM_NODE_INFO[] bsCmnCnmNodeInfoArray = AppMain.amDrawAlloc_GMS_BS_CMN_CNM_NODE_INFO((int)cnm_mgr_work.reg_node_max);
        for (int index = 0; index < (int)cnm_mgr_work.reg_node_max; ++index)
        {
            bsCmnCnmNodeInfoArray[index] = AppMain.amDrawAlloc_GMS_BS_CMN_CNM_NODE_INFO();
            bsCmnCnmNodeInfoArray[index].Assign(cnm_mgr_work.node_info_list[index]);
        }
        obj_work.obj_3d.mplt_cb_param = (object)new AppMain.OBS_ACTION3D_NN_WORK.CMPLT_Wrapper()
        {
            m_pInfos = bsCmnCnmNodeInfoArray,
            reg_node_cnt = cnm_mgr_work.reg_node_cnt
        };
    }

    private static int GmBsCmnRegisterCNMNode(
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int node_index)
    {
        cnm_mgr_work.node_info_list[(int)cnm_mgr_work.reg_node_cnt].node_index = node_index;
        int regNodeCnt = (int)cnm_mgr_work.reg_node_cnt;
        ++cnm_mgr_work.reg_node_cnt;
        return regNodeCnt;
    }

    private static void GmBsCmnSetCNMMtx(
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      AppMain.NNS_MATRIX w_mtx,
      int cnm_reg_id)
    {
        AppMain.GmBsCmnSetCNMMtx(cnm_mgr_work, w_mtx, cnm_reg_id, 0);
    }

    private static void GmBsCmnSetCNMMtx(
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      AppMain.NNS_MATRIX w_mtx,
      int cnm_reg_id,
      int enables)
    {
        AppMain.GMS_BS_CMN_CNM_NODE_INFO nodeInfo = cnm_mgr_work.node_info_list[cnm_reg_id];
        AppMain.nnCopyMatrix(nodeInfo.node_w_mtx, w_mtx);
        if (enables == 0)
            return;
        nodeInfo.enable = 1;
    }

    private static void GmBsCmnChangeCNMModeNode(
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int cnm_reg_id,
      uint mode)
    {
        cnm_mgr_work.node_info_list[cnm_reg_id].mode = mode;
    }

    private static void GmBsCmnEnableCNMLocalCoordinate(
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int cnm_reg_id,
      int enable)
    {
        if (enable != 0)
            cnm_mgr_work.node_info_list[cnm_reg_id].flag |= AppMain.GMD_BS_CMN_CNM_FLAG_LOCAL_COORDINATE;
        else
            cnm_mgr_work.node_info_list[cnm_reg_id].flag &= ~AppMain.GMD_BS_CMN_CNM_FLAG_LOCAL_COORDINATE;
    }

    private static void GmBsCmnEnableCNMInheritNodeScale(
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int cnm_reg_id,
      int enable)
    {
        if (enable != 0)
            cnm_mgr_work.node_info_list[cnm_reg_id].flag |= AppMain.GMD_BS_CMN_CNM_FLAG_INHERIT_SCALE;
        else
            cnm_mgr_work.node_info_list[cnm_reg_id].flag &= ~AppMain.GMD_BS_CMN_CNM_FLAG_INHERIT_SCALE;
    }

    private static void GmBsCmnEnableCNMMtxNode(
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int cnm_reg_id,
      int enable)
    {
        if (enable != 0)
            cnm_mgr_work.node_info_list[cnm_reg_id].enable = 1;
        else
            cnm_mgr_work.node_info_list[cnm_reg_id].enable = 0;
    }

    private static AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT GmBsCmnCreateNodeControlObjectBySize(
      AppMain.OBS_OBJECT_WORK parent_obj,
      AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int cnm_reg_id,
      AppMain.GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      AppMain.TaskWorkFactoryDelegate work_size)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK(work_size, parent_obj, (ushort)0, "bs_cmn_node_ctl_obj");
        AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT cmnNodeCtrlObject = (AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)work;
        cmnNodeCtrlObject.cnm_mgr_work = cnm_mgr_work;
        cmnNodeCtrlObject.cnm_reg_id = cnm_reg_id;
        cmnNodeCtrlObject.snm_work = snm_work;
        cmnNodeCtrlObject.snm_reg_id = snm_reg_id;
        cmnNodeCtrlObject.is_enable = 0;
        AppMain.nnMakeUnitMatrix(cmnNodeCtrlObject.w_mtx);
        work.disp_flag |= 32U;
        work.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBsCmnNodeControlObjectMainFunc);
        return cmnNodeCtrlObject;
    }

    private static void GmBsCmnAttachNCObjectToSNMNode(AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)ndc_obj;
        AppMain.NNS_VECTOR dst = new AppMain.NNS_VECTOR();
        AppMain.NNS_MATRIX nnsMatrix1 = new AppMain.NNS_MATRIX();
        AppMain.NNS_MATRIX nnsMatrix2 = new AppMain.NNS_MATRIX();
        AppMain.NNS_MATRIX nnsMatrix3 = new AppMain.NNS_MATRIX();
        AppMain.nnCopyMatrix(nnsMatrix1, AppMain.GmBsCmnGetSNMMtx(ndc_obj.snm_work, ndc_obj.snm_reg_id));
        AppMain.AkMathNormalizeMtx(nnsMatrix2, nnsMatrix1);
        AppMain.nnMakeRotateMatrixQuaternion(out ndc_obj.user_quat, nnsMatrix2);
        AppMain.nnTransformVector(dst, nnsMatrix2, ndc_obj.user_ofst);
        AppMain.nnMakeTranslateMatrix(nnsMatrix3, -dst.x, -dst.y, -dst.z);
        AppMain.nnMultiplyMatrix(nnsMatrix1, nnsMatrix3, nnsMatrix1);
        AppMain.GmBsCmnEnableCNMInheritNodeScale(ndc_obj.cnm_mgr_work, ndc_obj.cnm_reg_id, 1);
        obsObjectWork.pos.x = AppMain.FX_F32_TO_FX32(nnsMatrix1.M03);
        obsObjectWork.pos.y = AppMain.FX_F32_TO_FX32(-nnsMatrix1.M13);
        obsObjectWork.pos.z = AppMain.FX_F32_TO_FX32(nnsMatrix1.M23);
    }

    private static void GmBsCmnSetWorldMtxFromNCObjectPosture(
      AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)ndc_obj;
        AppMain.nnMakeTranslateMatrix(ndc_obj.w_mtx, AppMain.FX_FX32_TO_F32(obsObjectWork.pos.x), AppMain.FX_FX32_TO_F32(-obsObjectWork.pos.y), AppMain.FX_FX32_TO_F32(obsObjectWork.pos.z));
        AppMain.nnQuaternionMatrix(ndc_obj.w_mtx, ndc_obj.w_mtx, ref ndc_obj.user_quat);
        AppMain.nnTranslateMatrix(ndc_obj.w_mtx, ndc_obj.w_mtx, ndc_obj.user_ofst.x, ndc_obj.user_ofst.y, ndc_obj.user_ofst.z);
    }

    private static void GmBsCmnSetObject3DNNFadedColor(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.NNS_RGB color,
      float intensity)
    {
        AppMain.GmBsCmnSetObject3DNNFadedColor(obj_work, color, intensity, 0.0f, 10000f);
    }

    private static void GmBsCmnSetObject3DNNFadedColor(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.NNS_RGB color,
      float intensity,
      float radius)
    {
        AppMain.GmBsCmnSetObject3DNNFadedColor(obj_work, color, intensity, radius, 10000f);
    }

    private static void GmBsCmnSetObject3DNNFadedColor(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.NNS_RGB color,
      float intensity,
      float radius,
      float length)
    {
        AppMain.SNNS_VECTOR disp_pos = new AppMain.SNNS_VECTOR();
        AppMain.AMS_DRAWSTATE drawState = obj_work.obj_3d.draw_state;
        drawState.fog.flag = 1;
        drawState.fog_color.r = color.r;
        drawState.fog_color.g = color.g;
        drawState.fog_color.b = color.b;
        AppMain.ObjCameraDispPosGet(AppMain.g_obj.glb_camera_id, out disp_pos);
        float f32 = AppMain.FX_FX32_TO_F32(obj_work.pos.z);
        float num1 = AppMain.nnAbs((double)disp_pos.z - (double)f32);
        float num2;
        float num3;
        if ((double)length * (double)intensity > (double)num1)
        {
            num2 = 1.175494E-38f;
            num3 = num2 + num1 / intensity;
        }
        else
        {
            num2 = num1 - length * intensity;
            if ((double)num2 <= 0.0)
                num2 = 1.175494E-38f;
            num3 = num2 + length;
        }
        drawState.fog_range.fnear = num2 + radius;
        drawState.fog_range.ffar = num3 - radius;
    }

    private static void GmBsCmnClearObject3DNNFadedColor(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.AMS_DRAWSTATE drawState = obj_work.obj_3d.draw_state;
        drawState.fog.flag = AppMain.g_obj_draw_3dnn_draw_state.fog.flag;
        drawState.fog_color.Assign(AppMain.g_obj_draw_3dnn_draw_state.fog_color);
        drawState.fog_range.Assign(AppMain.g_obj_draw_3dnn_draw_state.fog_range);
    }

    private static int GmBsCmnIsSetSafeObject3DNNFadedColor(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return obj_work.obj_3d.draw_state.fog.flag == 0 ? 1 : 0;
    }

    private static void GmBsCmnInitObject3DNNDamageFlicker(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_DMG_FLICKER_WORK flk_work,
      float radius)
    {
        flk_work.is_active = 1;
        flk_work.cycles = AppMain.GMD_BS_CMN_DMG_FLICKER_DEFAULT_CYCLE;
        flk_work.interval_timer = 0U;
        flk_work.cur_angle = 0;
        flk_work.radius = radius;
        AppMain.GmBsCmnClearObject3DNNFadedColor(obj_work);
    }

    private static int GmBsCmnUpdateObject3DNNDamageFlicker(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_DMG_FLICKER_WORK flk_work)
    {
        if (flk_work.is_active == 0)
            return 1;
        if (flk_work.cycles != 0U)
        {
            if (flk_work.interval_timer != 0U)
            {
                --flk_work.interval_timer;
            }
            else
            {
                flk_work.cur_angle += AppMain.AKM_DEGtoA32(45f);
                if (flk_work.cur_angle >= AppMain.AKM_DEGtoA32(360f))
                {
                    flk_work.cur_angle = 0;
                    --flk_work.cycles;
                }
            }
            AppMain.GmBsCmnSetObject3DNNFadedColor(obj_work, AppMain.gm_bs_cmn_dmg_flicker_default_color, (float)((1.0 - (double)AppMain.nnCos(flk_work.cur_angle)) / 2.0));
            return 0;
        }
        if (flk_work.is_active != 0)
            AppMain.GmBsCmnEndObject3DNNDamageFlicker(obj_work, flk_work);
        return 1;
    }

    private static void GmBsCmnEndObject3DNNDamageFlicker(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BS_CMN_DMG_FLICKER_WORK flk_work)
    {
        flk_work.Clear();
        AppMain.GmBsCmnClearObject3DNNFadedColor(obj_work);
    }

    private static AppMain.GMS_FADE_OBJ_WORK GmBsCmnInitScreenFadingColor(
      AppMain.NNS_RGBA_U8 start_color,
      AppMain.NNS_RGBA_U8 end_color,
      float frame)
    {
        AppMain.GMS_FADE_OBJ_WORK fadeObj = AppMain.GmFadeCreateFadeObj((ushort)6656, (byte)3, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_FADE_OBJ_WORK()), (ushort)61439, 10U);
        AppMain.GmFadeSetFade(fadeObj, 0U, start_color.r, start_color.g, start_color.b, start_color.a, end_color.r, end_color.g, end_color.b, end_color.a, frame, 0, 0);
        return fadeObj;
    }

    private static int GmBsCmnUpdateScreenFadingColor(AppMain.GMS_FADE_OBJ_WORK fade_obj_work)
    {
        return AppMain.GmFadeIsEnd(fade_obj_work) != 0 ? 1 : 0;
    }

    private static void GmBsCmnClearScreenFadingColor(AppMain.GMS_FADE_OBJ_WORK fade_obj_work)
    {
        fade_obj_work.obj_work.flag |= 8U;
    }

    private static void GmBsCmnInitFlashScreen(
      AppMain.GMS_CMN_FLASH_SCR_WORK flash_work,
      float fo_frame,
      float duration_frame,
      float fi_frame)
    {
        AppMain.NNS_RGBA_U8 start_color = new AppMain.NNS_RGBA_U8((byte)0, (byte)0, (byte)0, (byte)0);
        AppMain.NNS_RGBA_U8 end_color = new AppMain.NNS_RGBA_U8(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        flash_work.Clear();
        flash_work.active_flag |= 3U;
        flash_work.fi_frame = fi_frame;
        flash_work.duration_timer = duration_frame;
        flash_work.fade_obj_work = AppMain.GmBsCmnInitScreenFadingColor(start_color, end_color, fo_frame);
    }

    private static int GmBsCmnUpdateFlashScreen(AppMain.GMS_CMN_FLASH_SCR_WORK flash_work)
    {
        AppMain.NNS_RGBA_U8 end_color = new AppMain.NNS_RGBA_U8((byte)0, (byte)0, (byte)0, (byte)0);
        AppMain.NNS_RGBA_U8 start_color = new AppMain.NNS_RGBA_U8(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        if (flash_work.active_flag == 0U)
            return 1;
        if (AppMain.GmBsCmnUpdateScreenFadingColor(flash_work.fade_obj_work) != 0)
        {
            if (((int)flash_work.active_flag & 1) != 0)
            {
                if ((double)flash_work.duration_timer > 0.0)
                {
                    --flash_work.duration_timer;
                }
                else
                {
                    flash_work.active_flag &= 4294967294U;
                    AppMain.GmBsCmnClearScreenFadingColor(flash_work.fade_obj_work);
                    flash_work.fade_obj_work = AppMain.GmBsCmnInitScreenFadingColor(start_color, end_color, flash_work.fi_frame);
                }
            }
            else if (((int)flash_work.active_flag & 2) != 0)
            {
                AppMain.GmBsCmnClearScreenFadingColor(flash_work.fade_obj_work);
                flash_work.fade_obj_work = (AppMain.GMS_FADE_OBJ_WORK)null;
                flash_work.active_flag &= 4294967293U;
            }
        }
        return 0;
    }

    private static void GmBsCmnClearFlashScreen(AppMain.GMS_CMN_FLASH_SCR_WORK flash_work)
    {
        if (flash_work.fade_obj_work != null)
        {
            AppMain.GmBsCmnClearScreenFadingColor(flash_work.fade_obj_work);
            flash_work.fade_obj_work = (AppMain.GMS_FADE_OBJ_WORK)null;
        }
        flash_work.Clear();
    }

    private static void GmBsCmnInitDelaySearch(
      AppMain.GMS_BS_CMN_DELAY_SEARCH_WORK dsearch_work,
      AppMain.OBS_OBJECT_WORK targ_obj,
      AppMain.VecFx32[] pos_hist_buf,
      int hist_num)
    {
        dsearch_work.pos_hist_buf = pos_hist_buf;
        dsearch_work.cur_point = -1;
        dsearch_work.hist_num = hist_num;
        dsearch_work.targ_obj = targ_obj;
        dsearch_work.record_cnt = 0;
        AppMain.GmBsCmnUpdateDelaySearch(dsearch_work);
    }

    private static void GmBsCmnUpdateDelaySearch(AppMain.GMS_BS_CMN_DELAY_SEARCH_WORK dsearch_work)
    {
        ++dsearch_work.cur_point;
        if (dsearch_work.cur_point >= dsearch_work.hist_num)
            dsearch_work.cur_point = 0;
        ++dsearch_work.record_cnt;
        dsearch_work.pos_hist_buf[dsearch_work.cur_point].Assign(dsearch_work.targ_obj.pos);
    }

    private static void GmBsCmnGetDelaySearchPos(
      AppMain.GMS_BS_CMN_DELAY_SEARCH_WORK dsearch_work,
      int delay_time,
      out AppMain.VecFx32 pos)
    {
        int index;
        if (delay_time < dsearch_work.record_cnt)
        {
            index = dsearch_work.cur_point - delay_time;
            if (index < 0)
                index = dsearch_work.hist_num + index;
        }
        else
            index = 0;
        pos = dsearch_work.pos_hist_buf[index];
    }

    private static int gmBsCmnCheckActionFrameOverrunOnNextUpdate(
      AppMain.OBS_OBJECT_WORK obj_work,
      ref float overrun_frame)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        float num1 = obj3d.speed[0] * AppMain.FX_FX32_TO_F32(AppMain.g_obj.speed);
        float num2 = AppMain.amMotionGetEndFrame(obj3d.motion, obj3d.act_id[0]) - AppMain.amMotionGetStartFrame(obj3d.motion, obj3d.act_id[0]);
        if ((double)obj3d.frame[0] + (double)num1 > (double)num2 - 1.0)
        {
            overrun_frame = (float)((double)obj3d.frame[0] + (double)num1 - ((double)num2 - 1.0));
            return 1;
        }
        overrun_frame = 0.0f;
        return 0;
    }

    private static int gmBsCmnCheckActionFrameOverrunOnNextUpdate(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        float num1 = obj3d.speed[0] * AppMain.FX_FX32_TO_F32(AppMain.g_obj.speed);
        float num2 = AppMain.amMotionGetEndFrame(obj3d.motion, obj3d.act_id[0]) - AppMain.amMotionGetStartFrame(obj3d.motion, obj3d.act_id[0]);
        return (double)obj3d.frame[0] + (double)num1 > (double)num2 - 1.0 ? 1 : 0;
    }

    private static void gmBsCmnInitBossMotionCBLink(
      AppMain.GMS_BS_CMN_BMCB_LINK bmcb_link,
      AppMain.MPP_VOID_MOTION_NSSOBJECT_OBJECT bmcb_func,
      object bmcb_param)
    {
        bmcb_link.Clear();
        bmcb_link.bmcb_func = bmcb_func;
        bmcb_link.bmcb_param = bmcb_param;
    }

    private static void gmBsCmnClearBossMotionCBLink(AppMain.GMS_BS_CMN_BMCB_LINK bmcb_link)
    {
        bmcb_link.Clear();
    }

    private static void gmBsCmnBossMotionCallbackFunc(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_OBJECT _object,
      object mtn_cb_param)
    {
        for (AppMain.GMS_BS_CMN_BMCB_LINK next = ((AppMain.GMS_BS_CMN_BMCB_MGR)mtn_cb_param).bmcb_head.next; next != null && next.bmcb_func != null; next = next.next)
            next.bmcb_func(motion, _object, next.bmcb_param);
    }

    private static void gmBsCmnMotionCallbackStoreNodeMatrix(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_OBJECT _object,
      object mtn_cb_param)
    {
        AppMain.GMS_BS_CMN_SNM_WORK gmsBsCmnSnmWork = (AppMain.GMS_BS_CMN_SNM_WORK)mtn_cb_param;
        AppMain.NNS_MATRIX nodeMatrixBaseMtx = AppMain.gmBsCmnMotionCallbackStoreNodeMatrix_base_mtx;
        AppMain.nnMakeUnitMatrix(nodeMatrixBaseMtx);
        AppMain.nnMultiplyMatrix(nodeMatrixBaseMtx, nodeMatrixBaseMtx, AppMain.amMatrixGetCurrent());
        for (int index = 0; index < (int)gmsBsCmnSnmWork.reg_node_cnt; ++index)
        {
            int nodeIndex = gmsBsCmnSnmWork.node_info_list[index].node_index;
            AppMain.NNS_MATRIX nodeMatrixNodeMtx = AppMain.gmBsCmnMotionCallbackStoreNodeMatrix_node_mtx;
            AppMain.nnCalcNodeMatrixTRSList(nodeMatrixNodeMtx, _object, nodeIndex, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, nodeMatrixBaseMtx);
            gmsBsCmnSnmWork.node_info_list[index].node_w_mtx.Assign(nodeMatrixNodeMtx);
        }
    }

    private static void gmBsCmnMtxpltCallbackControlNodeMatrix(
      AppMain.NNS_MATRIX[] mtx_plt,
      AppMain.NNS_OBJECT _object,
      object mplt_cb_param)
    {
        if (mplt_cb_param == null)
            return;
        ushort regNodeCnt = ((AppMain.OBS_ACTION3D_NN_WORK.CMPLT_Wrapper)mplt_cb_param).reg_node_cnt;
        AppMain.GMS_BS_CMN_CNM_NODE_INFO[] pInfos = ((AppMain.OBS_ACTION3D_NN_WORK.CMPLT_Wrapper)mplt_cb_param).m_pInfos;
        AppMain.NNS_MATRIX[] mtx_plt1 = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_orig_mtx_plt;
        if (mtx_plt1 == null || mtx_plt1.Length < _object.nMtxPal)
        {
            mtx_plt1 = new AppMain.NNS_MATRIX[_object.nMtxPal];
            for (int index = 0; index < _object.nMtxPal; ++index)
                mtx_plt1[index] = AppMain.amDrawAlloc_NNS_MATRIX();
        }
        for (int index = 0; index < _object.nMtxPal; ++index)
            mtx_plt1[index].Assign(mtx_plt[index]);
        for (int index = 0; index < (int)regNodeCnt; ++index)
        {
            AppMain.GMS_BS_CMN_CNM_NODE_INFO bsCmnCnmNodeInfo = pInfos[index];
            if (bsCmnCnmNodeInfo.enable != 0)
            {
                int iMatrix = (int)_object.pNodeList[bsCmnCnmNodeInfo.node_index].iMatrix;
                if (iMatrix != -1)
                {
                    AppMain.NNS_MATRIX matrixCandidateMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_candidate_mtx;
                    AppMain.NNS_MATRIX matrixInvViewMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_inv_view_mtx;
                    AppMain.NNS_MATRIX nodeMatrixNodeWMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_node_w_mtx;
                    AppMain.nnInvertMatrix(matrixInvViewMtx, AppMain.amDrawGetWorldViewMatrix());
                    AppMain.nnMultiplyMatrix(matrixCandidateMtx, matrixInvViewMtx, mtx_plt[iMatrix]);
                    if (((int)bsCmnCnmNodeInfo.flag & (int)AppMain.GMD_BS_CMN_CNM_FLAG_LOCAL_COORDINATE) != 0)
                    {
                        AppMain.NNS_MATRIX nodeMatrixCurMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_cur_mtx;
                        AppMain.NNS_MATRIX nodeMatrixInvCurMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_inv_cur_mtx;
                        AppMain.NNS_MATRIX nodeMatrixInitMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_init_mtx;
                        int iParent = (int)_object.pNodeList[bsCmnCnmNodeInfo.node_index].iParent;
                        AppMain.nnInvertMatrix(nodeMatrixInitMtx, _object.pNodeList[bsCmnCnmNodeInfo.node_index].InvInitMtx);
                        if (bsCmnCnmNodeInfo.mode == 0U)
                        {
                            AppMain.NNS_MATRIX nodeMatrixParentMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_parent_mtx;
                            AppMain.NNS_MATRIX nodeMatrixDiffMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_diff_mtx;
                            AppMain.NNS_MATRIX matrixParentInitMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_parent_init_mtx;
                            AppMain.nnMultiplyMatrix(nodeMatrixParentMtx, matrixInvViewMtx, mtx_plt[(int)_object.pNodeList[iParent].iMatrix]);
                            AppMain.nnInvertMatrix(matrixParentInitMtx, _object.pNodeList[iParent].InvInitMtx);
                            AppMain.nnMultiplyMatrix(nodeMatrixParentMtx, nodeMatrixParentMtx, matrixParentInitMtx);
                            AppMain.nnMultiplyMatrix(nodeMatrixDiffMtx, _object.pNodeList[iParent].InvInitMtx, nodeMatrixInitMtx);
                            AppMain.nnMultiplyMatrix(nodeMatrixCurMtx, nodeMatrixParentMtx, nodeMatrixDiffMtx);
                            AppMain.nnMultiplyMatrix(matrixCandidateMtx, nodeMatrixCurMtx, _object.pNodeList[bsCmnCnmNodeInfo.node_index].InvInitMtx);
                            AppMain.nnMultiplyMatrix(nodeMatrixNodeWMtx, nodeMatrixCurMtx, bsCmnCnmNodeInfo.node_w_mtx);
                        }
                        else
                        {
                            AppMain.NNS_MATRIX matrixParentCurMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_parent_cur_mtx;
                            AppMain.NNS_MATRIX invParentOrigMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_inv_parent_orig_mtx;
                            AppMain.nnCopyMatrix(nodeMatrixCurMtx, matrixCandidateMtx);
                            AppMain.nnMultiplyMatrix(nodeMatrixCurMtx, nodeMatrixCurMtx, nodeMatrixInitMtx);
                            AppMain.gmBsCmnGetNodeInvWorldMtx(invParentOrigMtx, _object.pNodeList[iParent], matrixInvViewMtx, mtx_plt1);
                            AppMain.gmBsCmnGetNodeWorldMtx(matrixParentCurMtx, _object.pNodeList[iParent], matrixInvViewMtx, mtx_plt);
                            AppMain.nnInvertMatrix(nodeMatrixInvCurMtx, nodeMatrixCurMtx);
                            AppMain.nnMultiplyMatrix(nodeMatrixNodeWMtx, bsCmnCnmNodeInfo.node_w_mtx, nodeMatrixInvCurMtx);
                            AppMain.nnMultiplyMatrix(nodeMatrixNodeWMtx, nodeMatrixCurMtx, nodeMatrixNodeWMtx);
                            AppMain.nnMultiplyMatrix(nodeMatrixNodeWMtx, invParentOrigMtx, nodeMatrixNodeWMtx);
                            AppMain.nnMultiplyMatrix(nodeMatrixNodeWMtx, matrixParentCurMtx, nodeMatrixNodeWMtx);
                        }
                    }
                    else
                        AppMain.nnCopyMatrix(nodeMatrixNodeWMtx, bsCmnCnmNodeInfo.node_w_mtx);
                    if (bsCmnCnmNodeInfo.mode == 1U)
                        AppMain.nnMultiplyMatrix(matrixCandidateMtx, nodeMatrixNodeWMtx, matrixCandidateMtx);
                    else if (bsCmnCnmNodeInfo.mode == 2U)
                    {
                        AppMain.NNS_MATRIX nodeMatrixInitMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_init_mtx;
                        AppMain.nnInvertMatrix(nodeMatrixInitMtx, _object.pNodeList[bsCmnCnmNodeInfo.node_index].InvInitMtx);
                        AppMain.nnMultiplyMatrix(matrixCandidateMtx, matrixCandidateMtx, nodeMatrixInitMtx);
                        AppMain.nnMultiplyMatrix(matrixCandidateMtx, matrixCandidateMtx, nodeMatrixNodeWMtx);
                    }
                    else if (((int)bsCmnCnmNodeInfo.flag & (int)AppMain.GMD_BS_CMN_CNM_FLAG_INHERIT_SCALE) != 0 && ((int)bsCmnCnmNodeInfo.flag & (int)AppMain.GMD_BS_CMN_CNM_FLAG_LOCAL_COORDINATE) == 0)
                    {
                        AppMain.NNS_MATRIX nodeMatrixInitMtx = AppMain.gmBsCmnMtxpltCallbackControlNodeMatrix_init_mtx;
                        AppMain.nnInvertMatrix(nodeMatrixInitMtx, _object.pNodeList[bsCmnCnmNodeInfo.node_index].InvInitMtx);
                        AppMain.nnMultiplyMatrix(matrixCandidateMtx, matrixCandidateMtx, nodeMatrixInitMtx);
                        AppMain.AkMathExtractScaleMtx(matrixCandidateMtx, matrixCandidateMtx);
                        AppMain.nnMultiplyMatrix(matrixCandidateMtx, nodeMatrixNodeWMtx, matrixCandidateMtx);
                    }
                    else
                        AppMain.nnCopyMatrix(matrixCandidateMtx, nodeMatrixNodeWMtx);
                    if (bsCmnCnmNodeInfo.mode != 1U)
                        AppMain.nnMultiplyMatrix(matrixCandidateMtx, matrixCandidateMtx, _object.pNodeList[bsCmnCnmNodeInfo.node_index].InvInitMtx);
                    AppMain.nnMultiplyMatrix(mtx_plt[iMatrix], AppMain.amDrawGetWorldViewMatrix(), matrixCandidateMtx);
                }
            }
        }
    }

    private static void gmBsCmnNodeControlObjectMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT cmnNodeCtrlObject = (AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work;
        if (cmnNodeCtrlObject.proc_update != null)
            cmnNodeCtrlObject.proc_update(obj_work);
        else
            AppMain.nnMakeUnitMatrix(cmnNodeCtrlObject.w_mtx);
        AppMain.GmBsCmnSetCNMMtx(cmnNodeCtrlObject.cnm_mgr_work, cmnNodeCtrlObject.w_mtx, cmnNodeCtrlObject.cnm_reg_id);
        AppMain.GmBsCmnEnableCNMMtxNode(cmnNodeCtrlObject.cnm_mgr_work, cmnNodeCtrlObject.cnm_reg_id, cmnNodeCtrlObject.is_enable);
    }

    private static void gmBsCmnGetNodeWorldMtx(
      AppMain.NNS_MATRIX dest_mtx,
      AppMain.NNS_NODE node,
      AppMain.NNS_MATRIX inv_view_mtx,
      AppMain.NNS_MATRIX[] mtx_plt)
    {
        AppMain.NNS_MATRIX nodeWorldMtxInitMtx = AppMain.gmBsCmnGetNodeWorldMtx_init_mtx;
        AppMain.nnMultiplyMatrix(dest_mtx, inv_view_mtx, mtx_plt[(int)node.iMatrix]);
        AppMain.nnInvertMatrix(nodeWorldMtxInitMtx, node.InvInitMtx);
        AppMain.nnMultiplyMatrix(dest_mtx, dest_mtx, nodeWorldMtxInitMtx);
    }

    private static void gmBsCmnGetNodeInvWorldMtx(
      AppMain.NNS_MATRIX dest_mtx,
      AppMain.NNS_NODE node,
      AppMain.NNS_MATRIX inv_view_mtx,
      AppMain.NNS_MATRIX[] mtx_plt)
    {
        AppMain.nnMultiplyMatrix(dest_mtx, inv_view_mtx, mtx_plt[(int)node.iMatrix]);
        AppMain.nnInvertMatrix(dest_mtx, dest_mtx);
        AppMain.nnMultiplyMatrix(dest_mtx, node.InvInitMtx, dest_mtx);
    }

}