public partial class AppMain
{
    private static OBS_OBJECT_WORK GmBsCmnGetPlayerObj()
    {
        return (OBS_OBJECT_WORK)g_gm_main_system.ply_work[0];
    }

    private static int GmBsCmnIsFinalZoneType(OBS_OBJECT_WORK obj_work)
    {
        return GsGetMainSysInfo().stage_id == 16 ? 1 : 0;
    }

    private static void GmBsCmnSetAction(OBS_OBJECT_WORK obj_work, int act_id, int is_repeat)
    {
        GmBsCmnSetAction(obj_work, act_id, is_repeat, 0);
    }

    private static void GmBsCmnSetAction(
      OBS_OBJECT_WORK obj_work,
      int act_id,
      int is_repeat,
      int is_blend)
    {
        if (is_blend != 0)
            ObjDrawObjectActionSet3DNNBlend(obj_work, act_id);
        else
            ObjDrawObjectActionSet(obj_work, act_id);
        if (is_repeat != 0)
            obj_work.disp_flag |= 4U;
        else
            obj_work.disp_flag &= 4294967291U;
    }

    private static int GmBsCmnIsActionEnd(OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.disp_flag & 8) != 0 ? 1 : 0;
    }

    private static void GmBsCmnSetObjSpd(OBS_OBJECT_WORK obj_work, int spd_x, int spd_y)
    {
        GmBsCmnSetObjSpd(obj_work, spd_x, spd_y, 0);
    }

    private static void GmBsCmnSetObjSpd(
      OBS_OBJECT_WORK obj_work,
      int spd_x,
      int spd_y,
      int spd_z)
    {
        obj_work.spd.x = spd_x;
        obj_work.spd.y = spd_y;
        obj_work.spd.z = spd_z;
    }

    private static void GmBsCmnSetObjSpdZero(OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.x = obj_work.spd.y = obj_work.spd.z = 0;
        obj_work.spd_add.x = obj_work.spd_add.y = obj_work.spd_add.z = 0;
    }

    private static int GmBsCmnIsActionEndPrecisely(OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.disp_flag & 8) != 0 ? 1 : gmBsCmnCheckActionFrameOverrunOnNextUpdate(obj_work);
    }

    private static int GmBsCmnIsActionEndFlexibly(OBS_OBJECT_WORK obj_work, float allow_ratio)
    {
        float num = obj_work.obj_3d.speed[0] * FX_FX32_TO_F32(g_obj.speed);
        float overrun_frame = 0.0f;
        if (((int)obj_work.disp_flag & 8) != 0)
            return 1;
        gmBsCmnCheckActionFrameOverrunOnNextUpdate(obj_work, ref overrun_frame);
        return overrun_frame > num * (double)allow_ratio ? GmBsCmnIsActionEndPrecisely(obj_work) : GmBsCmnIsActionEnd(obj_work);
    }

    private static void GmBsCmnSetEfctAtkVsPly(
      GMS_EFFECT_COM_WORK efct_com,
      short view_out_ofst)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)efct_com;
        obsObjectWork.flag &= 4294967277U;
        obsObjectWork.move_flag |= 256U;
        obsObjectWork.view_out_ofst = view_out_ofst;
        GmEffectRectInit(efct_com, gm_bs_cmn_efct_atk_flag_tbl, gm_bs_cmn_efct_def_flag_tbl, 1, 1);
    }

    private static int GmBsCmnCheckRectMajorOverlapH(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect,
      ref int center_ofst_x)
    {
        int lLeft1 = 0;
        int lLeft2 = 0;
        ushort usWidth1 = 0;
        ushort usWidth2 = 0;
        int num1 = 0;
        ObjRectLTBSet(my_rect, ref lLeft1, new int?(), new int?());
        ObjRectWHDSet(my_rect, ref usWidth1, new ushort?(), new ushort?());
        int num2 = lLeft1 + usWidth1;
        int num3 = lLeft1 + (usWidth1 >> 1);
        ObjRectLTBSet(your_rect, ref lLeft2, new int?(), new int?());
        ObjRectWHDSet(your_rect, ref usWidth2, new ushort?(), new ushort?());
        int num4 = lLeft2 + usWidth2;
        int num5 = lLeft2 + (usWidth2 >> 1);
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
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect,
      ref int center_ofst_y)
    {
        int lTop1 = 0;
        int lTop2 = 0;
        ushort usHeight1 = 0;
        int num1 = 0;
        ObjRectLTBSet(my_rect, new int?(), ref lTop1, new int?());
        ObjRectWHDSet(my_rect, new ushort?(), ref usHeight1, new ushort?());
        int num2 = lTop1 + usHeight1;
        int num3 = lTop1 + (usHeight1 >> 1);
        ObjRectLTBSet(your_rect, new int?(), ref lTop2, new int?());
        ushort usHeight2 = 0;
        ObjRectWHDSet(your_rect, new ushort?(), ref usHeight2, new ushort?());
        ushort num4 = usHeight2;
        int num5 = lTop2 + num4;
        int num6 = lTop2 + (num4 >> 1);
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
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        int center_ofst_x = 0;
        int center_ofst_y = 0;
        int num = GmBsCmnCheckRectMajorOverlapH(my_rect, your_rect, ref center_ofst_x);
        GmBsCmnCheckRectMajorOverlapV(my_rect, your_rect, ref center_ofst_y);
        return num != 0 ? (center_ofst_y < 0 ? GMD_BS_CMN_RECT_HIT_SIDE_LEFT : GMD_BS_CMN_RECT_HIT_SIDE_RIGHT) : (center_ofst_x < 0 ? GMD_BS_CMN_RECT_HIT_SIDE_TOP : GMD_BS_CMN_RECT_HIT_SIDE_BOTTOM);
    }

    public static uint GmBsCmnCheckRectHitSideVFirst(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        int center_ofst_x = 0;
        int center_ofst_y = 0;
        int num = GmBsCmnCheckRectMajorOverlapV(my_rect, your_rect, ref center_ofst_y);
        GmBsCmnCheckRectMajorOverlapH(my_rect, your_rect, ref center_ofst_x);
        return num != 0 ? (center_ofst_x < 0 ? GMD_BS_CMN_RECT_HIT_SIDE_LEFT : GMD_BS_CMN_RECT_HIT_SIDE_RIGHT) : (center_ofst_y < 0 ? GMD_BS_CMN_RECT_HIT_SIDE_TOP : GMD_BS_CMN_RECT_HIT_SIDE_BOTTOM);
    }

    private static void GmBsCmnInitBossMotionCBSystem(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_BMCB_MGR bmcb_mgr)
    {
        bmcb_mgr.Clear();
        bmcb_mgr.bmcb_head.next = bmcb_mgr.bmcb_tail;
        bmcb_mgr.bmcb_head.prev = null;
        bmcb_mgr.bmcb_tail.next = null;
        bmcb_mgr.bmcb_tail.prev = bmcb_mgr.bmcb_head;
        obj_work.obj_3d.mtn_cb_func = new mtn_cb_func_delegate(gmBsCmnBossMotionCallbackFunc);
        obj_work.obj_3d.mtn_cb_param = bmcb_mgr;
    }

    private static void GmBsCmnClearBossMotionCBSystem(OBS_OBJECT_WORK obj_work)
    {
        GMS_BS_CMN_BMCB_MGR mtnCbParam = (GMS_BS_CMN_BMCB_MGR)obj_work.obj_3d.mtn_cb_param;
        if (mtnCbParam == null)
            return;
        GMS_BS_CMN_BMCB_LINK next;
        for (GMS_BS_CMN_BMCB_LINK gmsBsCmnBmcbLink = mtnCbParam.bmcb_head.next; gmsBsCmnBmcbLink != null; gmsBsCmnBmcbLink = next)
        {
            next = gmsBsCmnBmcbLink.next;
            gmsBsCmnBmcbLink.next = null;
            gmsBsCmnBmcbLink.prev = null;
            if (gmsBsCmnBmcbLink.bmcb_func == null)
                break;
        }
        mtnCbParam.bmcb_head.next = mtnCbParam.bmcb_head.prev = null;
        mtnCbParam.bmcb_tail.next = mtnCbParam.bmcb_tail.prev = null;
        mtnCbParam.Clear();
        obj_work.obj_3d.mtn_cb_func = null;
        obj_work.obj_3d.mtn_cb_param = null;
    }

    private static void GmBsCmnAppendBossMotionCallback(
      GMS_BS_CMN_BMCB_MGR bmcb_mgr,
      GMS_BS_CMN_BMCB_LINK bmcb_link)
    {
        bmcb_link.prev = bmcb_mgr.bmcb_tail.prev;
        bmcb_link.prev.next = bmcb_link;
        bmcb_link.next = bmcb_mgr.bmcb_tail;
        bmcb_mgr.bmcb_tail.prev = bmcb_link;
    }

    private static void GmBsCmnCreateSNMWork(
      GMS_BS_CMN_SNM_WORK snm_work,
      NNS_OBJECT _object,
      ushort reg_max)
    {
        UNREFERENCED_PARAMETER(_object);
        gmBsCmnInitBossMotionCBLink(snm_work.bmcb_link, new MPP_VOID_MOTION_NSSOBJECT_OBJECT(gmBsCmnMotionCallbackStoreNodeMatrix), snm_work);
        snm_work.reg_node_cnt = 0;
        snm_work.reg_node_max = reg_max;
        snm_work.node_info_list = New<GMS_BS_CMN_SNM_NODE_INFO>(reg_max);
    }

    private static void GmBsCmnDeleteSNMWork(GMS_BS_CMN_SNM_WORK snm_work)
    {
        gmBsCmnClearBossMotionCBLink(snm_work.bmcb_link);
        snm_work.reg_node_cnt = 0;
        snm_work.reg_node_max = 0;
        if (snm_work.node_info_list == null)
            return;
        snm_work.node_info_list = null;
    }

    private static int GmBsCmnRegisterSNMNode(GMS_BS_CMN_SNM_WORK snm_work, int node_index)
    {
        snm_work.node_info_list[snm_work.reg_node_cnt].node_index = node_index;
        int regNodeCnt = snm_work.reg_node_cnt;
        ++snm_work.reg_node_cnt;
        return regNodeCnt;
    }

    private static NNS_MATRIX GmBsCmnGetSNMMtx(
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id)
    {
        return snm_work.node_info_list[snm_reg_id].node_w_mtx;
    }

    private static void GmBsCmnUpdateObjectGeneralStuckWithNode(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      NNS_MATRIX ofst_mtx)
    {
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(snm_work, snm_reg_id);
        obj_work.pos.x = FX_F32_TO_FX32(snmMtx.M03);
        obj_work.pos.y = -FX_F32_TO_FX32(snmMtx.M13);
        obj_work.pos.z = FX_F32_TO_FX32(snmMtx.M23);
        if (ofst_mtx == null)
            return;
        VEC_Set(ref obj_work.pos, obj_work.pos.x + FX_F32_TO_FX32(ofst_mtx.M03), obj_work.pos.y - FX_F32_TO_FX32(ofst_mtx.M13), obj_work.pos.z + FX_F32_TO_FX32(ofst_mtx.M23));
    }

    private static void GmBsCmnUpdateObjectGeneralStuckWithNodeRelative(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      VecFx32 pivot_cur_pos,
      VecFx32 pivot_prev_pos)
    {
        GmBsCmnUpdateObjectGeneralStuckWithNodeRelative(obj_work, snm_work, snm_reg_id, pivot_cur_pos, pivot_prev_pos, null);
    }

    private static void GmBsCmnUpdateObjectGeneralStuckWithNodeRelative(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      VecFx32 pivot_cur_pos,
      VecFx32 pivot_prev_pos,
      NNS_MATRIX ofst_mtx)
    {
        GmBsCmnUpdateObjectGeneralStuckWithNode(obj_work, snm_work, snm_reg_id, ofst_mtx);
        VEC_Set(ref obj_work.pos, obj_work.pos.x - pivot_prev_pos.x + pivot_cur_pos.x, obj_work.pos.y - pivot_prev_pos.y + pivot_cur_pos.y, obj_work.pos.z - pivot_prev_pos.z + pivot_cur_pos.z);
    }

    private static void GmBsCmnUpdateObject3DNNStuckWithNode(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation)
    {
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, snm_work, snm_reg_id, b_rotation, null);
    }

    private static void GmBsCmnUpdateObject3DNNStuckWithNode(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation,
      NNS_MATRIX ofst_mtx)
    {
        NNS_MATRIX userObjMtxR = obj_work.obj_3d.user_obj_mtx_r;
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(snm_work, snm_reg_id);
        obj_work.pos.x = FX_F32_TO_FX32(snmMtx.M03);
        obj_work.pos.y = -FX_F32_TO_FX32(snmMtx.M13);
        obj_work.pos.z = FX_F32_TO_FX32(snmMtx.M23);
        if (b_rotation != 0)
        {
            obj_work.disp_flag |= 16777216U;
            AkMathNormalizeMtx(userObjMtxR, snmMtx);
        }
        else
        {
            obj_work.disp_flag &= 4278190079U;
            nnMakeUnitMatrix(userObjMtxR);
        }
        if (ofst_mtx == null)
            return;
        NNS_MATRIX stuckWithNodeRotMtx = GmBsCmnUpdateObject3DNNStuckWithNode_rot_mtx;
        NNS_VECTOR dst = new NNS_VECTOR();
        NNS_MATRIX withNodeNodeWRot = GmBsCmnUpdateObject3DNNStuckWithNode_node_w_rot;
        nnCopyMatrix(withNodeNodeWRot, snmMtx);
        withNodeNodeWRot.M03 = withNodeNodeWRot.M13 = withNodeNodeWRot.M23 = 0.0f;
        nnMultiplyMatrix(withNodeNodeWRot, withNodeNodeWRot, ofst_mtx);
        nnCopyMatrixTranslationVector(dst, withNodeNodeWRot);
        VEC_Set(ref obj_work.pos, obj_work.pos.x + FX_F32_TO_FX32(dst.x), obj_work.pos.y - FX_F32_TO_FX32(dst.y), obj_work.pos.z + FX_F32_TO_FX32(dst.z));
        nnCopyMatrix(stuckWithNodeRotMtx, ofst_mtx);
        stuckWithNodeRotMtx.M03 = stuckWithNodeRotMtx.M13 = stuckWithNodeRotMtx.M23 = 0.0f;
        obj_work.disp_flag |= 16777216U;
        nnMultiplyMatrix(userObjMtxR, userObjMtxR, stuckWithNodeRotMtx);
    }

    private static void GmBsCmnUpdateObject3DNNStuckWithNodeRelative(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation,
      VecFx32 pivot_cur_pos,
      VecFx32 pivot_prev_pos,
      NNS_MATRIX ofst_mtx)
    {
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, snm_work, snm_reg_id, b_rotation, ofst_mtx);
        VEC_Set(ref obj_work.pos, obj_work.pos.x - pivot_prev_pos.x + pivot_cur_pos.x, obj_work.pos.y - pivot_prev_pos.y + pivot_cur_pos.y, obj_work.pos.z - pivot_prev_pos.z + pivot_cur_pos.z);
    }

    private static void GmBsCmnUpdateObject3DESStuckWithNode(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation)
    {
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, snm_work, snm_reg_id, b_rotation, null);
    }

    private static void GmBsCmnUpdateObject3DESStuckWithNode(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation,
      NNS_MATRIX ofst_mtx)
    {
        NNS_MATRIX stuckWithNodeNmlWMtx = GmBsCmnUpdateObject3DESStuckWithNode_nml_w_mtx;
        NNS_QUATERNION dst1 = obj_work.obj_3des.user_dir_quat;
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(snm_work, snm_reg_id);
        obj_work.pos.x = FX_F32_TO_FX32(snmMtx.M03);
        obj_work.pos.y = -FX_F32_TO_FX32(snmMtx.M13);
        obj_work.pos.z = FX_F32_TO_FX32(snmMtx.M23);
        if (b_rotation != 0)
        {
            obj_work.obj_3des.flag |= 32U;
            AkMathNormalizeMtx(stuckWithNodeNmlWMtx, snmMtx);
            nnMakeRotateMatrixQuaternion(out dst1, stuckWithNodeNmlWMtx);
        }
        else
        {
            obj_work.obj_3des.flag &= 4294967263U;
            nnMakeUnitQuaternion(ref dst1);
            nnMakeUnitMatrix(stuckWithNodeNmlWMtx);
        }
        if (ofst_mtx != null)
        {
            NNS_MATRIX stuckWithNodeRotMtx = GmBsCmnUpdateObject3DESStuckWithNode_rot_mtx;
            NNS_QUATERNION dst2 = new NNS_QUATERNION();
            NNS_VECTOR dst3 = new NNS_VECTOR();
            NNS_MATRIX withNodeNodeWRot = GmBsCmnUpdateObject3DESStuckWithNode_node_w_rot;
            nnCopyMatrix(withNodeNodeWRot, snmMtx);
            withNodeNodeWRot.M03 = withNodeNodeWRot.M13 = withNodeNodeWRot.M23 = 0.0f;
            nnMultiplyMatrix(withNodeNodeWRot, withNodeNodeWRot, ofst_mtx);
            nnCopyMatrixTranslationVector(dst3, withNodeNodeWRot);
            VEC_Set(ref obj_work.pos, obj_work.pos.x + FX_F32_TO_FX32(dst3.x), obj_work.pos.y - FX_F32_TO_FX32(dst3.y), obj_work.pos.z + FX_F32_TO_FX32(dst3.z));
            AkMathNormalizeMtx(stuckWithNodeRotMtx, ofst_mtx);
            nnMakeRotateMatrixQuaternion(out dst2, stuckWithNodeRotMtx);
            obj_work.obj_3des.flag |= 32U;
            nnMultiplyQuaternion(ref dst1, ref dst1, ref dst2);
        }
        obj_work.obj_3des.user_dir_quat.Assign(dst1);
    }

    private static void GmBsCmnUpdateObject3DESStuckWithNodeRelative(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation,
      VecFx32 pivot_cur_pos,
      VecFx32 pivot_prev_pos)
    {
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, snm_work, snm_reg_id, b_rotation, pivot_cur_pos, pivot_prev_pos, null);
    }

    private static void GmBsCmnUpdateObject3DESStuckWithNodeRelative(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      int b_rotation,
      VecFx32 pivot_cur_pos,
      VecFx32 pivot_prev_pos,
      NNS_MATRIX ofst_mtx)
    {
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, snm_work, snm_reg_id, b_rotation, ofst_mtx);
        VEC_Set(ref obj_work.pos, obj_work.pos.x - pivot_prev_pos.x + pivot_cur_pos.x, obj_work.pos.y - pivot_prev_pos.y + pivot_cur_pos.y, obj_work.pos.z - pivot_prev_pos.z + pivot_cur_pos.z);
    }

    private static void GmBsCmnInitCNMCb(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work)
    {
        obj_work.obj_3d.mplt_cb_func = new MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT(gmBsCmnMtxpltCallbackControlNodeMatrix);
        obj_work.obj_3d.mplt_cb_param = null;
    }

    private static void GmBsCmnClearCNMCb(OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d.mplt_cb_func = null;
        obj_work.obj_3d.mplt_cb_param = null;
    }

    private static void GmBsCmnCreateCNMMgrWork(
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      NNS_OBJECT _object,
      ushort reg_max)
    {
        UNREFERENCED_PARAMETER(_object);
        cnm_mgr_work.reg_node_cnt = 0;
        cnm_mgr_work.reg_node_max = reg_max;
        cnm_mgr_work.node_info_list = New<GMS_BS_CMN_CNM_NODE_INFO>(reg_max);
    }

    private static void GmBsCmnDeleteCNMMgrWork(GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work)
    {
        cnm_mgr_work.reg_node_cnt = 0;
        cnm_mgr_work.reg_node_max = 0;
        if (cnm_mgr_work.node_info_list == null)
            return;
        cnm_mgr_work.node_info_list = null;
    }

    private static void GmBsCmnUpdateCNMParam(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work)
    {
        if (obj_work.obj_3d.mplt_cb_func == null)
            return;
        amDrawAlloc_GMS_BS_CMN_CNM_PARAM().reg_node_cnt = cnm_mgr_work.reg_node_cnt;
        GMS_BS_CMN_CNM_NODE_INFO[] bsCmnCnmNodeInfoArray = amDrawAlloc_GMS_BS_CMN_CNM_NODE_INFO(cnm_mgr_work.reg_node_max);
        for (int index = 0; index < cnm_mgr_work.reg_node_max; ++index)
        {
            bsCmnCnmNodeInfoArray[index] = amDrawAlloc_GMS_BS_CMN_CNM_NODE_INFO();
            bsCmnCnmNodeInfoArray[index].Assign(cnm_mgr_work.node_info_list[index]);
        }
        obj_work.obj_3d.mplt_cb_param = new OBS_ACTION3D_NN_WORK.CMPLT_Wrapper()
        {
            m_pInfos = bsCmnCnmNodeInfoArray,
            reg_node_cnt = cnm_mgr_work.reg_node_cnt
        };
    }

    private static int GmBsCmnRegisterCNMNode(
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int node_index)
    {
        cnm_mgr_work.node_info_list[cnm_mgr_work.reg_node_cnt].node_index = node_index;
        int regNodeCnt = cnm_mgr_work.reg_node_cnt;
        ++cnm_mgr_work.reg_node_cnt;
        return regNodeCnt;
    }

    private static void GmBsCmnSetCNMMtx(
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      NNS_MATRIX w_mtx,
      int cnm_reg_id)
    {
        GmBsCmnSetCNMMtx(cnm_mgr_work, w_mtx, cnm_reg_id, 0);
    }

    private static void GmBsCmnSetCNMMtx(
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      NNS_MATRIX w_mtx,
      int cnm_reg_id,
      int enables)
    {
        GMS_BS_CMN_CNM_NODE_INFO nodeInfo = cnm_mgr_work.node_info_list[cnm_reg_id];
        nnCopyMatrix(nodeInfo.node_w_mtx, w_mtx);
        if (enables == 0)
            return;
        nodeInfo.enable = 1;
    }

    private static void GmBsCmnChangeCNMModeNode(
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int cnm_reg_id,
      uint mode)
    {
        cnm_mgr_work.node_info_list[cnm_reg_id].mode = mode;
    }

    private static void GmBsCmnEnableCNMLocalCoordinate(
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int cnm_reg_id,
      int enable)
    {
        if (enable != 0)
            cnm_mgr_work.node_info_list[cnm_reg_id].flag |= GMD_BS_CMN_CNM_FLAG_LOCAL_COORDINATE;
        else
            cnm_mgr_work.node_info_list[cnm_reg_id].flag &= ~GMD_BS_CMN_CNM_FLAG_LOCAL_COORDINATE;
    }

    private static void GmBsCmnEnableCNMInheritNodeScale(
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int cnm_reg_id,
      int enable)
    {
        if (enable != 0)
            cnm_mgr_work.node_info_list[cnm_reg_id].flag |= GMD_BS_CMN_CNM_FLAG_INHERIT_SCALE;
        else
            cnm_mgr_work.node_info_list[cnm_reg_id].flag &= ~GMD_BS_CMN_CNM_FLAG_INHERIT_SCALE;
    }

    private static void GmBsCmnEnableCNMMtxNode(
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int cnm_reg_id,
      int enable)
    {
        if (enable != 0)
            cnm_mgr_work.node_info_list[cnm_reg_id].enable = 1;
        else
            cnm_mgr_work.node_info_list[cnm_reg_id].enable = 0;
    }

    private static GMS_BS_CMN_NODE_CTRL_OBJECT GmBsCmnCreateNodeControlObjectBySize(
      OBS_OBJECT_WORK parent_obj,
      GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work,
      int cnm_reg_id,
      GMS_BS_CMN_SNM_WORK snm_work,
      int snm_reg_id,
      TaskWorkFactoryDelegate work_size)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(work_size, parent_obj, 0, "bs_cmn_node_ctl_obj");
        GMS_BS_CMN_NODE_CTRL_OBJECT cmnNodeCtrlObject = (GMS_BS_CMN_NODE_CTRL_OBJECT)work;
        cmnNodeCtrlObject.cnm_mgr_work = cnm_mgr_work;
        cmnNodeCtrlObject.cnm_reg_id = cnm_reg_id;
        cmnNodeCtrlObject.snm_work = snm_work;
        cmnNodeCtrlObject.snm_reg_id = snm_reg_id;
        cmnNodeCtrlObject.is_enable = 0;
        nnMakeUnitMatrix(cmnNodeCtrlObject.w_mtx);
        work.disp_flag |= 32U;
        work.ppOut = null;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBsCmnNodeControlObjectMainFunc);
        return cmnNodeCtrlObject;
    }

    private static void GmBsCmnAttachNCObjectToSNMNode(GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)ndc_obj;
        NNS_VECTOR dst = new NNS_VECTOR();
        NNS_MATRIX nnsMatrix1 = new NNS_MATRIX();
        NNS_MATRIX nnsMatrix2 = new NNS_MATRIX();
        NNS_MATRIX nnsMatrix3 = new NNS_MATRIX();
        nnCopyMatrix(nnsMatrix1, GmBsCmnGetSNMMtx(ndc_obj.snm_work, ndc_obj.snm_reg_id));
        AkMathNormalizeMtx(nnsMatrix2, nnsMatrix1);
        nnMakeRotateMatrixQuaternion(out ndc_obj.user_quat, nnsMatrix2);
        nnTransformVector(dst, nnsMatrix2, ndc_obj.user_ofst);
        nnMakeTranslateMatrix(nnsMatrix3, -dst.x, -dst.y, -dst.z);
        nnMultiplyMatrix(nnsMatrix1, nnsMatrix3, nnsMatrix1);
        GmBsCmnEnableCNMInheritNodeScale(ndc_obj.cnm_mgr_work, ndc_obj.cnm_reg_id, 1);
        obsObjectWork.pos.x = FX_F32_TO_FX32(nnsMatrix1.M03);
        obsObjectWork.pos.y = FX_F32_TO_FX32(-nnsMatrix1.M13);
        obsObjectWork.pos.z = FX_F32_TO_FX32(nnsMatrix1.M23);
    }

    private static void GmBsCmnSetWorldMtxFromNCObjectPosture(
      GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)ndc_obj;
        nnMakeTranslateMatrix(ndc_obj.w_mtx, FX_FX32_TO_F32(obsObjectWork.pos.x), FX_FX32_TO_F32(-obsObjectWork.pos.y), FX_FX32_TO_F32(obsObjectWork.pos.z));
        nnQuaternionMatrix(ndc_obj.w_mtx, ndc_obj.w_mtx, ref ndc_obj.user_quat);
        nnTranslateMatrix(ndc_obj.w_mtx, ndc_obj.w_mtx, ndc_obj.user_ofst.x, ndc_obj.user_ofst.y, ndc_obj.user_ofst.z);
    }

    private static void GmBsCmnSetObject3DNNFadedColor(
      OBS_OBJECT_WORK obj_work,
      NNS_RGB color,
      float intensity)
    {
        GmBsCmnSetObject3DNNFadedColor(obj_work, color, intensity, 0.0f, 10000f);
    }

    private static void GmBsCmnSetObject3DNNFadedColor(
      OBS_OBJECT_WORK obj_work,
      NNS_RGB color,
      float intensity,
      float radius)
    {
        GmBsCmnSetObject3DNNFadedColor(obj_work, color, intensity, radius, 10000f);
    }

    private static void GmBsCmnSetObject3DNNFadedColor(
      OBS_OBJECT_WORK obj_work,
      NNS_RGB color,
      float intensity,
      float radius,
      float length)
    {
        SNNS_VECTOR disp_pos = new SNNS_VECTOR();
        AMS_DRAWSTATE drawState = obj_work.obj_3d.draw_state;
        drawState.fog.flag = 1;
        drawState.fog_color.r = color.r;
        drawState.fog_color.g = color.g;
        drawState.fog_color.b = color.b;
        ObjCameraDispPosGet(g_obj.glb_camera_id, out disp_pos);
        float f32 = FX_FX32_TO_F32(obj_work.pos.z);
        float num1 = nnAbs(disp_pos.z - (double)f32);
        float num2;
        float num3;
        if (length * (double)intensity > num1)
        {
            num2 = 1.175494E-38f;
            num3 = num2 + num1 / intensity;
        }
        else
        {
            num2 = num1 - length * intensity;
            if (num2 <= 0.0)
                num2 = 1.175494E-38f;
            num3 = num2 + length;
        }
        drawState.fog_range.fnear = num2 + radius;
        drawState.fog_range.ffar = num3 - radius;
    }

    private static void GmBsCmnClearObject3DNNFadedColor(OBS_OBJECT_WORK obj_work)
    {
        AMS_DRAWSTATE drawState = obj_work.obj_3d.draw_state;
        drawState.fog.flag = g_obj_draw_3dnn_draw_state.fog.flag;
        drawState.fog_color.Assign(g_obj_draw_3dnn_draw_state.fog_color);
        drawState.fog_range.Assign(g_obj_draw_3dnn_draw_state.fog_range);
    }

    private static int GmBsCmnIsSetSafeObject3DNNFadedColor(OBS_OBJECT_WORK obj_work)
    {
        return obj_work.obj_3d.draw_state.fog.flag == 0 ? 1 : 0;
    }

    private static void GmBsCmnInitObject3DNNDamageFlicker(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_DMG_FLICKER_WORK flk_work,
      float radius)
    {
        flk_work.is_active = 1;
        flk_work.cycles = GMD_BS_CMN_DMG_FLICKER_DEFAULT_CYCLE;
        flk_work.interval_timer = 0U;
        flk_work.cur_angle = 0;
        flk_work.radius = radius;
        GmBsCmnClearObject3DNNFadedColor(obj_work);
    }

    private static int GmBsCmnUpdateObject3DNNDamageFlicker(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_DMG_FLICKER_WORK flk_work)
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
                flk_work.cur_angle += AKM_DEGtoA32(45f);
                if (flk_work.cur_angle >= AKM_DEGtoA32(360f))
                {
                    flk_work.cur_angle = 0;
                    --flk_work.cycles;
                }
            }
            GmBsCmnSetObject3DNNFadedColor(obj_work, gm_bs_cmn_dmg_flicker_default_color, (float)((1.0 - nnCos(flk_work.cur_angle)) / 2.0));
            return 0;
        }
        if (flk_work.is_active != 0)
            GmBsCmnEndObject3DNNDamageFlicker(obj_work, flk_work);
        return 1;
    }

    private static void GmBsCmnEndObject3DNNDamageFlicker(
      OBS_OBJECT_WORK obj_work,
      GMS_BS_CMN_DMG_FLICKER_WORK flk_work)
    {
        flk_work.Clear();
        GmBsCmnClearObject3DNNFadedColor(obj_work);
    }

    private static GMS_FADE_OBJ_WORK GmBsCmnInitScreenFadingColor(
      NNS_RGBA_U8 start_color,
      NNS_RGBA_U8 end_color,
      float frame)
    {
        GMS_FADE_OBJ_WORK fadeObj = GmFadeCreateFadeObj(GMD_TASK_PRIO_EFFECT, 3, 0, () => new GMS_FADE_OBJ_WORK(), IZD_FADE_DT_PRIO_DEF, 10U);
        GmFadeSetFade(fadeObj, 0U, start_color.r, start_color.g, start_color.b, start_color.a, end_color.r, end_color.g, end_color.b, end_color.a, frame, 0, 0);
        return fadeObj;
    }

    private static int GmBsCmnUpdateScreenFadingColor(GMS_FADE_OBJ_WORK fade_obj_work)
    {
        return GmFadeIsEnd(fade_obj_work) != 0 ? 1 : 0;
    }

    private static void GmBsCmnClearScreenFadingColor(GMS_FADE_OBJ_WORK fade_obj_work)
    {
        fade_obj_work.obj_work.flag |= 8U;
    }

    private static void GmBsCmnInitFlashScreen(
      GMS_CMN_FLASH_SCR_WORK flash_work,
      float fo_frame,
      float duration_frame,
      float fi_frame)
    {
        NNS_RGBA_U8 start_color = new NNS_RGBA_U8(0, 0, 0, 0);
        NNS_RGBA_U8 end_color = new NNS_RGBA_U8(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        flash_work.Clear();
        flash_work.active_flag |= 3U;
        flash_work.fi_frame = fi_frame;
        flash_work.duration_timer = duration_frame;
        flash_work.fade_obj_work = GmBsCmnInitScreenFadingColor(start_color, end_color, fo_frame);
    }

    private static int GmBsCmnUpdateFlashScreen(GMS_CMN_FLASH_SCR_WORK flash_work)
    {
        NNS_RGBA_U8 end_color = new NNS_RGBA_U8(0, 0, 0, 0);
        NNS_RGBA_U8 start_color = new NNS_RGBA_U8(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        if (flash_work.active_flag == 0U)
            return 1;
        if (GmBsCmnUpdateScreenFadingColor(flash_work.fade_obj_work) != 0)
        {
            if (((int)flash_work.active_flag & 1) != 0)
            {
                if (flash_work.duration_timer > 0.0)
                {
                    --flash_work.duration_timer;
                }
                else
                {
                    flash_work.active_flag &= 4294967294U;
                    GmBsCmnClearScreenFadingColor(flash_work.fade_obj_work);
                    flash_work.fade_obj_work = GmBsCmnInitScreenFadingColor(start_color, end_color, flash_work.fi_frame);
                }
            }
            else if (((int)flash_work.active_flag & 2) != 0)
            {
                GmBsCmnClearScreenFadingColor(flash_work.fade_obj_work);
                flash_work.fade_obj_work = null;
                flash_work.active_flag &= 4294967293U;
            }
        }
        return 0;
    }

    private static void GmBsCmnClearFlashScreen(GMS_CMN_FLASH_SCR_WORK flash_work)
    {
        if (flash_work.fade_obj_work != null)
        {
            GmBsCmnClearScreenFadingColor(flash_work.fade_obj_work);
            flash_work.fade_obj_work = null;
        }
        flash_work.Clear();
    }

    private static void GmBsCmnInitDelaySearch(
      GMS_BS_CMN_DELAY_SEARCH_WORK dsearch_work,
      OBS_OBJECT_WORK targ_obj,
      VecFx32[] pos_hist_buf,
      int hist_num)
    {
        dsearch_work.pos_hist_buf = pos_hist_buf;
        dsearch_work.cur_point = -1;
        dsearch_work.hist_num = hist_num;
        dsearch_work.targ_obj = targ_obj;
        dsearch_work.record_cnt = 0;
        GmBsCmnUpdateDelaySearch(dsearch_work);
    }

    private static void GmBsCmnUpdateDelaySearch(GMS_BS_CMN_DELAY_SEARCH_WORK dsearch_work)
    {
        ++dsearch_work.cur_point;
        if (dsearch_work.cur_point >= dsearch_work.hist_num)
            dsearch_work.cur_point = 0;
        ++dsearch_work.record_cnt;
        dsearch_work.pos_hist_buf[dsearch_work.cur_point].Assign(dsearch_work.targ_obj.pos);
    }

    private static void GmBsCmnGetDelaySearchPos(
      GMS_BS_CMN_DELAY_SEARCH_WORK dsearch_work,
      int delay_time,
      out VecFx32 pos)
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
      OBS_OBJECT_WORK obj_work,
      ref float overrun_frame)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        float num1 = obj3d.speed[0] * FX_FX32_TO_F32(g_obj.speed);
        float num2 = amMotionGetEndFrame(obj3d.motion, obj3d.act_id[0]) - amMotionGetStartFrame(obj3d.motion, obj3d.act_id[0]);
        if (obj3d.frame[0] + (double)num1 > num2 - 1.0)
        {
            overrun_frame = (float)(obj3d.frame[0] + (double)num1 - (num2 - 1.0));
            return 1;
        }
        overrun_frame = 0.0f;
        return 0;
    }

    private static int gmBsCmnCheckActionFrameOverrunOnNextUpdate(OBS_OBJECT_WORK obj_work)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        float num1 = obj3d.speed[0] * FX_FX32_TO_F32(g_obj.speed);
        float num2 = amMotionGetEndFrame(obj3d.motion, obj3d.act_id[0]) - amMotionGetStartFrame(obj3d.motion, obj3d.act_id[0]);
        return obj3d.frame[0] + (double)num1 > num2 - 1.0 ? 1 : 0;
    }

    private static void gmBsCmnInitBossMotionCBLink(
      GMS_BS_CMN_BMCB_LINK bmcb_link,
      MPP_VOID_MOTION_NSSOBJECT_OBJECT bmcb_func,
      object bmcb_param)
    {
        bmcb_link.Clear();
        bmcb_link.bmcb_func = bmcb_func;
        bmcb_link.bmcb_param = bmcb_param;
    }

    private static void gmBsCmnClearBossMotionCBLink(GMS_BS_CMN_BMCB_LINK bmcb_link)
    {
        bmcb_link.Clear();
    }

    private static void gmBsCmnBossMotionCallbackFunc(
      AMS_MOTION motion,
      NNS_OBJECT _object,
      object mtn_cb_param)
    {
        for (GMS_BS_CMN_BMCB_LINK next = ((GMS_BS_CMN_BMCB_MGR)mtn_cb_param).bmcb_head.next; next != null && next.bmcb_func != null; next = next.next)
            next.bmcb_func(motion, _object, next.bmcb_param);
    }

    private static void gmBsCmnMotionCallbackStoreNodeMatrix(
      AMS_MOTION motion,
      NNS_OBJECT _object,
      object mtn_cb_param)
    {
        GMS_BS_CMN_SNM_WORK gmsBsCmnSnmWork = (GMS_BS_CMN_SNM_WORK)mtn_cb_param;
        NNS_MATRIX nodeMatrixBaseMtx = gmBsCmnMotionCallbackStoreNodeMatrix_base_mtx;
        nnMakeUnitMatrix(nodeMatrixBaseMtx);
        nnMultiplyMatrix(nodeMatrixBaseMtx, nodeMatrixBaseMtx, amMatrixGetCurrent());
        for (int index = 0; index < gmsBsCmnSnmWork.reg_node_cnt; ++index)
        {
            int nodeIndex = gmsBsCmnSnmWork.node_info_list[index].node_index;
            NNS_MATRIX nodeMatrixNodeMtx = gmBsCmnMotionCallbackStoreNodeMatrix_node_mtx;
            nnCalcNodeMatrixTRSList(nodeMatrixNodeMtx, _object, nodeIndex, motion.data, nodeMatrixBaseMtx);
            gmsBsCmnSnmWork.node_info_list[index].node_w_mtx.Assign(nodeMatrixNodeMtx);
        }
    }

    private static void gmBsCmnMtxpltCallbackControlNodeMatrix(
      NNS_MATRIX[] mtx_plt,
      NNS_OBJECT _object,
      object mplt_cb_param)
    {
        if (mplt_cb_param == null)
            return;
        ushort regNodeCnt = ((OBS_ACTION3D_NN_WORK.CMPLT_Wrapper)mplt_cb_param).reg_node_cnt;
        GMS_BS_CMN_CNM_NODE_INFO[] pInfos = ((OBS_ACTION3D_NN_WORK.CMPLT_Wrapper)mplt_cb_param).m_pInfos;
        NNS_MATRIX[] mtx_plt1 = gmBsCmnMtxpltCallbackControlNodeMatrix_orig_mtx_plt;
        if (mtx_plt1 == null || mtx_plt1.Length < _object.nMtxPal)
        {
            mtx_plt1 = new NNS_MATRIX[_object.nMtxPal];
            for (int index = 0; index < _object.nMtxPal; ++index)
                mtx_plt1[index] = amDrawAlloc_NNS_MATRIX();
        }
        for (int index = 0; index < _object.nMtxPal; ++index)
            mtx_plt1[index].Assign(mtx_plt[index]);
        for (int index = 0; index < regNodeCnt; ++index)
        {
            GMS_BS_CMN_CNM_NODE_INFO bsCmnCnmNodeInfo = pInfos[index];
            if (bsCmnCnmNodeInfo.enable != 0)
            {
                int iMatrix = _object.pNodeList[bsCmnCnmNodeInfo.node_index].iMatrix;
                if (iMatrix != -1)
                {
                    NNS_MATRIX matrixCandidateMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_candidate_mtx;
                    NNS_MATRIX matrixInvViewMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_inv_view_mtx;
                    NNS_MATRIX nodeMatrixNodeWMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_node_w_mtx;
                    nnInvertMatrix(matrixInvViewMtx, amDrawGetWorldViewMatrix());
                    nnMultiplyMatrix(matrixCandidateMtx, matrixInvViewMtx, mtx_plt[iMatrix]);
                    if (((int)bsCmnCnmNodeInfo.flag & (int)GMD_BS_CMN_CNM_FLAG_LOCAL_COORDINATE) != 0)
                    {
                        NNS_MATRIX nodeMatrixCurMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_cur_mtx;
                        NNS_MATRIX nodeMatrixInvCurMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_inv_cur_mtx;
                        NNS_MATRIX nodeMatrixInitMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_init_mtx;
                        int iParent = _object.pNodeList[bsCmnCnmNodeInfo.node_index].iParent;
                        nnInvertMatrix(nodeMatrixInitMtx, _object.pNodeList[bsCmnCnmNodeInfo.node_index].InvInitMtx);
                        if (bsCmnCnmNodeInfo.mode == 0U)
                        {
                            NNS_MATRIX nodeMatrixParentMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_parent_mtx;
                            NNS_MATRIX nodeMatrixDiffMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_diff_mtx;
                            NNS_MATRIX matrixParentInitMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_parent_init_mtx;
                            nnMultiplyMatrix(nodeMatrixParentMtx, matrixInvViewMtx, mtx_plt[_object.pNodeList[iParent].iMatrix]);
                            nnInvertMatrix(matrixParentInitMtx, _object.pNodeList[iParent].InvInitMtx);
                            nnMultiplyMatrix(nodeMatrixParentMtx, nodeMatrixParentMtx, matrixParentInitMtx);
                            nnMultiplyMatrix(nodeMatrixDiffMtx, _object.pNodeList[iParent].InvInitMtx, nodeMatrixInitMtx);
                            nnMultiplyMatrix(nodeMatrixCurMtx, nodeMatrixParentMtx, nodeMatrixDiffMtx);
                            nnMultiplyMatrix(matrixCandidateMtx, nodeMatrixCurMtx, _object.pNodeList[bsCmnCnmNodeInfo.node_index].InvInitMtx);
                            nnMultiplyMatrix(nodeMatrixNodeWMtx, nodeMatrixCurMtx, bsCmnCnmNodeInfo.node_w_mtx);
                        }
                        else
                        {
                            NNS_MATRIX matrixParentCurMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_parent_cur_mtx;
                            NNS_MATRIX invParentOrigMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_inv_parent_orig_mtx;
                            nnCopyMatrix(nodeMatrixCurMtx, matrixCandidateMtx);
                            nnMultiplyMatrix(nodeMatrixCurMtx, nodeMatrixCurMtx, nodeMatrixInitMtx);
                            gmBsCmnGetNodeInvWorldMtx(invParentOrigMtx, _object.pNodeList[iParent], matrixInvViewMtx, mtx_plt1);
                            gmBsCmnGetNodeWorldMtx(matrixParentCurMtx, _object.pNodeList[iParent], matrixInvViewMtx, mtx_plt);
                            nnInvertMatrix(nodeMatrixInvCurMtx, nodeMatrixCurMtx);
                            nnMultiplyMatrix(nodeMatrixNodeWMtx, bsCmnCnmNodeInfo.node_w_mtx, nodeMatrixInvCurMtx);
                            nnMultiplyMatrix(nodeMatrixNodeWMtx, nodeMatrixCurMtx, nodeMatrixNodeWMtx);
                            nnMultiplyMatrix(nodeMatrixNodeWMtx, invParentOrigMtx, nodeMatrixNodeWMtx);
                            nnMultiplyMatrix(nodeMatrixNodeWMtx, matrixParentCurMtx, nodeMatrixNodeWMtx);
                        }
                    }
                    else
                        nnCopyMatrix(nodeMatrixNodeWMtx, bsCmnCnmNodeInfo.node_w_mtx);
                    if (bsCmnCnmNodeInfo.mode == 1U)
                        nnMultiplyMatrix(matrixCandidateMtx, nodeMatrixNodeWMtx, matrixCandidateMtx);
                    else if (bsCmnCnmNodeInfo.mode == 2U)
                    {
                        NNS_MATRIX nodeMatrixInitMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_init_mtx;
                        nnInvertMatrix(nodeMatrixInitMtx, _object.pNodeList[bsCmnCnmNodeInfo.node_index].InvInitMtx);
                        nnMultiplyMatrix(matrixCandidateMtx, matrixCandidateMtx, nodeMatrixInitMtx);
                        nnMultiplyMatrix(matrixCandidateMtx, matrixCandidateMtx, nodeMatrixNodeWMtx);
                    }
                    else if (((int)bsCmnCnmNodeInfo.flag & (int)GMD_BS_CMN_CNM_FLAG_INHERIT_SCALE) != 0 && ((int)bsCmnCnmNodeInfo.flag & (int)GMD_BS_CMN_CNM_FLAG_LOCAL_COORDINATE) == 0)
                    {
                        NNS_MATRIX nodeMatrixInitMtx = gmBsCmnMtxpltCallbackControlNodeMatrix_init_mtx;
                        nnInvertMatrix(nodeMatrixInitMtx, _object.pNodeList[bsCmnCnmNodeInfo.node_index].InvInitMtx);
                        nnMultiplyMatrix(matrixCandidateMtx, matrixCandidateMtx, nodeMatrixInitMtx);
                        AkMathExtractScaleMtx(matrixCandidateMtx, matrixCandidateMtx);
                        nnMultiplyMatrix(matrixCandidateMtx, nodeMatrixNodeWMtx, matrixCandidateMtx);
                    }
                    else
                        nnCopyMatrix(matrixCandidateMtx, nodeMatrixNodeWMtx);
                    if (bsCmnCnmNodeInfo.mode != 1U)
                        nnMultiplyMatrix(matrixCandidateMtx, matrixCandidateMtx, _object.pNodeList[bsCmnCnmNodeInfo.node_index].InvInitMtx);
                    nnMultiplyMatrix(mtx_plt[iMatrix], amDrawGetWorldViewMatrix(), matrixCandidateMtx);
                }
            }
        }
    }

    private static void gmBsCmnNodeControlObjectMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BS_CMN_NODE_CTRL_OBJECT cmnNodeCtrlObject = (GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work;
        if (cmnNodeCtrlObject.proc_update != null)
            cmnNodeCtrlObject.proc_update(obj_work);
        else
            nnMakeUnitMatrix(cmnNodeCtrlObject.w_mtx);
        GmBsCmnSetCNMMtx(cmnNodeCtrlObject.cnm_mgr_work, cmnNodeCtrlObject.w_mtx, cmnNodeCtrlObject.cnm_reg_id);
        GmBsCmnEnableCNMMtxNode(cmnNodeCtrlObject.cnm_mgr_work, cmnNodeCtrlObject.cnm_reg_id, cmnNodeCtrlObject.is_enable);
    }

    private static void gmBsCmnGetNodeWorldMtx(
      NNS_MATRIX dest_mtx,
      NNS_NODE node,
      NNS_MATRIX inv_view_mtx,
      NNS_MATRIX[] mtx_plt)
    {
        NNS_MATRIX nodeWorldMtxInitMtx = gmBsCmnGetNodeWorldMtx_init_mtx;
        nnMultiplyMatrix(dest_mtx, inv_view_mtx, mtx_plt[node.iMatrix]);
        nnInvertMatrix(nodeWorldMtxInitMtx, node.InvInitMtx);
        nnMultiplyMatrix(dest_mtx, dest_mtx, nodeWorldMtxInitMtx);
    }

    private static void gmBsCmnGetNodeInvWorldMtx(
      NNS_MATRIX dest_mtx,
      NNS_NODE node,
      NNS_MATRIX inv_view_mtx,
      NNS_MATRIX[] mtx_plt)
    {
        nnMultiplyMatrix(dest_mtx, inv_view_mtx, mtx_plt[node.iMatrix]);
        nnInvertMatrix(dest_mtx, dest_mtx);
        nnMultiplyMatrix(dest_mtx, node.InvInitMtx, dest_mtx);
    }

}