public partial class AppMain
{
    private static void gmGmkStopperStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)obj_work;
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)tbl_gm_gmk_piston_col_rect[0];
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)tbl_gm_gmk_piston_col_rect[1];
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.ofst_x = tbl_gm_gmk_piston_col_rect[2];
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.ofst_y = tbl_gm_gmk_piston_col_rect[3];
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.dir = 0;
        gmsGmkStopperWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsGmkStopperWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsGmkStopperWork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkStopperHit);
        pRec.ppHit = null;
        ObjRectAtkSet(pRec, 0, 0);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectWorkSet(pRec, tbl_gm_gmk_piston_col_rect[4], tbl_gm_gmk_piston_col_rect[5], tbl_gm_gmk_piston_col_rect[6], tbl_gm_gmk_piston_col_rect[7]);
        obj_work.flag &= 4294967293U;
        gmsGmkStopperWork.se_handle = null;
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkStopperExit));
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkStopperStay);
    }

    private static void gmGmkStopperStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)obj_work;
        if (gmsGmkStopperWork.ply_work == null)
            return;
        if (gmsGmkStopperWork.call_slot_id == -1)
            gmGmkStopperStay_Norm(obj_work);
        else
            gmGmkStopperStay_Slot(obj_work);
    }

    private static void gmGmkStopperStay_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)obj_work;
        if (obj_work.user_timer >= gmsGmkStopperWork.ply_work.obj_work.pos.y >> 12)
            return;
        gmGmkStopperReset(obj_work);
    }

    private static void gmGmkStopperStay_Norm(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)obj_work;
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag &= 4294967279U;
        obj_work.disp_flag |= 4U;
        gmsGmkStopperWork.player_pass_timer = 143;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkStopperStay_Norm_100);
        GmCameraAllowSet(15f, 32f, 0.0f);
    }

    private static void gmGmkStopperStay_Norm_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)obj_work;
        if (gmsGmkStopperWork.ply_work != g_gm_main_system.ply_work[0] || gmsGmkStopperWork.ply_work.gmk_obj != obj_work || gmsGmkStopperWork.ply_work.seq_state != GME_PLY_SEQ_STATE_GMK_STOPPER)
        {
            gmGmkStopperReset(obj_work);
        }
        else
        {
            OBS_CAMERA obsCamera = ObjCameraGet(0);
            GmCameraPosSet(FXM_FLOAT_TO_FX32(obsCamera.pos.x), FXM_FLOAT_TO_FX32(-obsCamera.pos.y) + 16384, FXM_FLOAT_TO_FX32(obsCamera.pos.z));
            --gmsGmkStopperWork.player_pass_timer;
            if (gmsGmkStopperWork.player_pass_timer <= 0)
            {
                obj_work.disp_flag &= 4294967291U;
                gmGmkStopperStay_Norm_110(obj_work);
            }
            else
            {
                if (gmsGmkStopperWork.player_pass_timer % 16 != 0)
                    return;
                GmPlayerAddScore(gmsGmkStopperWork.ply_work, 1000, gmsGmkStopperWork.ply_work.obj_work.pos.x, gmsGmkStopperWork.ply_work.obj_work.pos.y);
                gsSoundStopSe(gmsGmkStopperWork.se_handle);
                gmsGmkStopperWork.se_handle.snd_ctrl_param.pitch = (float)(0.800000011920929 - gmsGmkStopperWork.player_pass_timer / 160.0);
                GmSoundPlaySE("Casino3", gmsGmkStopperWork.se_handle);
                if (gmsGmkStopperWork.player_pass_timer > 16)
                    return;
                GmSoundPlaySE("Casino3_1");
            }
        }
    }

    private static void gmGmkStopperStay_Norm_110(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)obj_work;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        gmsGmkStopperWork.player_pass_timer = 0;
        gmsGmkStopperWork.gmk_work.obj_3d.mat_frame = 0.0f;
        obj_work.disp_flag |= 16U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkStopperStay_100);
        if (gmsPlayerWork.seq_state != GME_PLY_SEQ_STATE_GMK_STOPPER)
            return;
        GmPlySeqInitStopperEnd(gmsGmkStopperWork.ply_work);
    }

    private static void gmGmkStopperStay_Slot(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag &= 4294967279U;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkStopperStay_Slot_100);
        GmCameraAllowSet(15f, 56f, 0.0f);
    }

    private static void gmGmkStopperStay_Slot_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)obj_work;
        if (gmsGmkStopperWork.ply_work != g_gm_main_system.ply_work[0] || gmsGmkStopperWork.ply_work.gmk_obj != obj_work || gmsGmkStopperWork.ply_work.seq_state != GME_PLY_SEQ_STATE_GMK_STOPPER)
        {
            gmGmkStopperReset(obj_work);
        }
        else
        {
            OBS_CAMERA obsCamera = ObjCameraGet(0);
            GmCameraPosSet(FXM_FLOAT_TO_FX32(obsCamera.pos.x), FXM_FLOAT_TO_FX32(-obsCamera.pos.y) + 16384, FXM_FLOAT_TO_FX32(obsCamera.pos.z));
            if (GmGmkSlotIsStatus(gmsGmkStopperWork.call_slot_id) == 0)
                return;
            obj_work.disp_flag &= 4294967291U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkStopperStay_Slot_110);
            gmGmkStopperStay_Slot_110(obj_work);
        }
    }

    private static void gmGmkStopperStay_Slot_110(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)obj_work;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        gmsGmkStopperWork.gmk_work.obj_3d.mat_frame = 0.0f;
        obj_work.disp_flag |= 16U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkStopperStay_100);
        if (gmsPlayerWork.seq_state != GME_PLY_SEQ_STATE_GMK_STOPPER)
            return;
        GmPlySeqInitStopperEnd(gmsGmkStopperWork.ply_work);
    }

    private static void gmGmkStopperReset(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)obj_work;
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        obj_work.flag &= 4294967293U;
        gmsGmkStopperWork.player_pass_timer = 0;
        gmsGmkStopperWork.ply_work = null;
        gmsGmkStopperWork.gmk_work.obj_3d.mat_frame = 0.0f;
        obj_work.disp_flag |= 16U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkStopperStay);
        if (gmsGmkStopperWork.se_handle == null)
            return;
        GmSoundStopSE(gmsGmkStopperWork.se_handle);
        GsSoundFreeSeHandle(gmsGmkStopperWork.se_handle);
        gmsGmkStopperWork.se_handle = null;
    }

    private static void gmGmkStopperLockWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)obj_work;
        if (gmsGmkStopperWork.ply_work != g_gm_main_system.ply_work[0] || gmsGmkStopperWork.ply_work.gmk_obj != obj_work || gmsGmkStopperWork.ply_work.seq_state != GME_PLY_SEQ_STATE_GMK_STOPPER)
        {
            gmGmkStopperReset(obj_work);
        }
        else
        {
            GMS_PLAYER_WORK plyWork = gmsGmkStopperWork.ply_work;
            if (obj_work.pos.x != plyWork.obj_work.pos.x || obj_work.pos.y != plyWork.obj_work.pos.y)
                return;
            if (gmsGmkStopperWork.call_slot_id >= 0)
            {
                GmGmkSlotStartRequest(gmsGmkStopperWork.call_slot_id, plyWork);
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkStopperStay_Slot);
                GmSoundPlaySE("Casino3");
            }
            else
            {
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkStopperStay_Norm);
                if (gmsGmkStopperWork.se_handle != null)
                {
                    GmSoundStopSE(gmsGmkStopperWork.se_handle);
                    GsSoundFreeSeHandle(gmsGmkStopperWork.se_handle);
                }
                gmsGmkStopperWork.se_handle = GsSoundAllocSeHandle();
                gmsGmkStopperWork.se_handle.flag |= 2147483648U;
                GmSoundPlaySE("Casino3", gmsGmkStopperWork.se_handle);
            }
        }
    }

    private static void gmGmkStopperHit(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (GMS_GMK_STOPPER_WORK)parentObj1;
        int num = 0;
        if (parentObj2 != g_gm_main_system.ply_work[0])
            return;
        if (parentObj1.pos.y > parentObj2.obj_work.pos.y)
        {
            if (parentObj2.obj_work.spd.y >= 0)
                num = 1;
        }
        else if (parentObj2.obj_work.spd.y <= 0)
            num = 1;
        if (num != 0)
        {
            GmPlySeqInitStopper(parentObj2, gmsGmkStopperWork.gmk_work.ene_com);
            gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.obj = null;
            parentObj1.flag |= 2U;
            gmsGmkStopperWork.ply_work = parentObj2;
            parentObj1.user_timer = (parentObj1.pos.y >> 12) + gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.height + gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.ofst_y - parentObj2.rect_work[0].rect.top;
            parentObj1.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkStopperLockWait);
        }
        else
            mine_rect.flag &= 4294573823U;
    }

    private static void gmGmkStopperExit(MTS_TASK_TCB tcb)
    {
        GMS_GMK_STOPPER_WORK tcbWork = (GMS_GMK_STOPPER_WORK)mtTaskGetTcbWork(tcb);
        if (tcbWork.se_handle != null)
        {
            GsSoundStopSeHandle(tcbWork.se_handle);
            GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = null;
        }
        GmEnemyDefaultExit(tcb);
    }

    private static OBS_OBJECT_WORK GmGmkStopperNormInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        GMS_GMK_STOPPER_WORK work = (GMS_GMK_STOPPER_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_STOPPER_WORK(), "Gmk_StopperRod");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_stopper_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = 393216;
        ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, null, null, 0, (AMS_AMB_HEADER)ObjDataGet(849).pData);
        gmsEnemy3DWork.obj_3d.mat_speed = 1f;
        obj_work.disp_flag |= 20U;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        work.call_slot_id = -1;
        gmGmkStopperStart(obj_work);
        return obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkStopperSlotInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_STOPPER_WORK work = (GMS_GMK_STOPPER_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_STOPPER_WORK(), "Gmk_StopperRod");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_stopper_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = 393216;
        ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, null, null, 1, (AMS_AMB_HEADER)ObjDataGet(849).pData);
        gmsEnemy3DWork.obj_3d.mat_speed = 1f;
        obj_work.disp_flag |= 20U;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        work.call_slot_id = eve_rec.left;
        gmGmkStopperStart(obj_work);
        return obj_work;
    }

    private static void GmGmkStopperBuild()
    {
        gm_gmk_stopper_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(847), GmGameDatGetGimmickData(848), 0U);
    }

    private static void GmGmkStopperFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(847);
        GmGameDBuildRegFlushModel(gm_gmk_stopper_obj_3d_list, gimmickData.file_num);
    }
}
