using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    private static void gmGmkStopperStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)obj_work;
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)AppMain.tbl_gm_gmk_piston_col_rect[0];
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)AppMain.tbl_gm_gmk_piston_col_rect[1];
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.ofst_x = AppMain.tbl_gm_gmk_piston_col_rect[2];
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.ofst_y = AppMain.tbl_gm_gmk_piston_col_rect[3];
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.dir = (ushort)0;
        gmsGmkStopperWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsGmkStopperWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsGmkStopperWork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkStopperHit);
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec, AppMain.tbl_gm_gmk_piston_col_rect[4], AppMain.tbl_gm_gmk_piston_col_rect[5], AppMain.tbl_gm_gmk_piston_col_rect[6], AppMain.tbl_gm_gmk_piston_col_rect[7]);
        obj_work.flag &= 4294967293U;
        gmsGmkStopperWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkStopperExit));
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkStopperStay);
    }

    private static void gmGmkStopperStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)obj_work;
        if (gmsGmkStopperWork.ply_work == null)
            return;
        if (gmsGmkStopperWork.call_slot_id == -1)
            AppMain.gmGmkStopperStay_Norm(obj_work);
        else
            AppMain.gmGmkStopperStay_Slot(obj_work);
    }

    private static void gmGmkStopperStay_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)obj_work;
        if (obj_work.user_timer >= gmsGmkStopperWork.ply_work.obj_work.pos.y >> 12)
            return;
        AppMain.gmGmkStopperReset(obj_work);
    }

    private static void gmGmkStopperStay_Norm(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag &= 4294967279U;
        obj_work.disp_flag |= 4U;
        gmsGmkStopperWork.player_pass_timer = (short)143;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkStopperStay_Norm_100);
        AppMain.GmCameraAllowSet(15f, 32f, 0.0f);
    }

    private static void gmGmkStopperStay_Norm_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)obj_work;
        if (gmsGmkStopperWork.ply_work != AppMain.g_gm_main_system.ply_work[0] || gmsGmkStopperWork.ply_work.gmk_obj != obj_work || gmsGmkStopperWork.ply_work.seq_state != 40)
        {
            AppMain.gmGmkStopperReset(obj_work);
        }
        else
        {
            AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
            AppMain.GmCameraPosSet(AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.x), AppMain.FXM_FLOAT_TO_FX32(-obsCamera.pos.y) + 16384, AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.z));
            --gmsGmkStopperWork.player_pass_timer;
            if (gmsGmkStopperWork.player_pass_timer <= (short)0)
            {
                obj_work.disp_flag &= 4294967291U;
                AppMain.gmGmkStopperStay_Norm_110(obj_work);
            }
            else
            {
                if ((int)gmsGmkStopperWork.player_pass_timer % 16 != 0)
                    return;
                AppMain.GmPlayerAddScore(gmsGmkStopperWork.ply_work, 1000, gmsGmkStopperWork.ply_work.obj_work.pos.x, gmsGmkStopperWork.ply_work.obj_work.pos.y);
                AppMain.gsSoundStopSe(gmsGmkStopperWork.se_handle);
                gmsGmkStopperWork.se_handle.snd_ctrl_param.pitch = (float)(0.800000011920929 - (double)gmsGmkStopperWork.player_pass_timer / 160.0);
                AppMain.GmSoundPlaySE("Casino3", gmsGmkStopperWork.se_handle);
                if (gmsGmkStopperWork.player_pass_timer > (short)16)
                    return;
                AppMain.GmSoundPlaySE("Casino3_1");
            }
        }
    }

    private static void gmGmkStopperStay_Norm_110(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        gmsGmkStopperWork.player_pass_timer = (short)0;
        gmsGmkStopperWork.gmk_work.obj_3d.mat_frame = 0.0f;
        obj_work.disp_flag |= 16U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkStopperStay_100);
        if (gmsPlayerWork.seq_state != 40)
            return;
        AppMain.GmPlySeqInitStopperEnd(gmsGmkStopperWork.ply_work);
    }

    private static void gmGmkStopperStay_Slot(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag &= 4294967279U;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkStopperStay_Slot_100);
        AppMain.GmCameraAllowSet(15f, 56f, 0.0f);
    }

    private static void gmGmkStopperStay_Slot_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)obj_work;
        if (gmsGmkStopperWork.ply_work != AppMain.g_gm_main_system.ply_work[0] || gmsGmkStopperWork.ply_work.gmk_obj != obj_work || gmsGmkStopperWork.ply_work.seq_state != 40)
        {
            AppMain.gmGmkStopperReset(obj_work);
        }
        else
        {
            AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
            AppMain.GmCameraPosSet(AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.x), AppMain.FXM_FLOAT_TO_FX32(-obsCamera.pos.y) + 16384, AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.z));
            if (AppMain.GmGmkSlotIsStatus(gmsGmkStopperWork.call_slot_id) == 0)
                return;
            obj_work.disp_flag &= 4294967291U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkStopperStay_Slot_110);
            AppMain.gmGmkStopperStay_Slot_110(obj_work);
        }
    }

    private static void gmGmkStopperStay_Slot_110(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        gmsGmkStopperWork.gmk_work.obj_3d.mat_frame = 0.0f;
        obj_work.disp_flag |= 16U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkStopperStay_100);
        if (gmsPlayerWork.seq_state != 40)
            return;
        AppMain.GmPlySeqInitStopperEnd(gmsGmkStopperWork.ply_work);
    }

    private static void gmGmkStopperReset(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)obj_work;
        gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        obj_work.flag &= 4294967293U;
        gmsGmkStopperWork.player_pass_timer = (short)0;
        gmsGmkStopperWork.ply_work = (AppMain.GMS_PLAYER_WORK)null;
        gmsGmkStopperWork.gmk_work.obj_3d.mat_frame = 0.0f;
        obj_work.disp_flag |= 16U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkStopperStay);
        if (gmsGmkStopperWork.se_handle == null)
            return;
        AppMain.GmSoundStopSE(gmsGmkStopperWork.se_handle);
        AppMain.GsSoundFreeSeHandle(gmsGmkStopperWork.se_handle);
        gmsGmkStopperWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
    }

    private static void gmGmkStopperLockWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)obj_work;
        if (gmsGmkStopperWork.ply_work != AppMain.g_gm_main_system.ply_work[0] || gmsGmkStopperWork.ply_work.gmk_obj != obj_work || gmsGmkStopperWork.ply_work.seq_state != 40)
        {
            AppMain.gmGmkStopperReset(obj_work);
        }
        else
        {
            AppMain.GMS_PLAYER_WORK plyWork = gmsGmkStopperWork.ply_work;
            if (obj_work.pos.x != plyWork.obj_work.pos.x || obj_work.pos.y != plyWork.obj_work.pos.y)
                return;
            if (gmsGmkStopperWork.call_slot_id >= 0)
            {
                AppMain.GmGmkSlotStartRequest(gmsGmkStopperWork.call_slot_id, plyWork);
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkStopperStay_Slot);
                AppMain.GmSoundPlaySE("Casino3");
            }
            else
            {
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkStopperStay_Norm);
                if (gmsGmkStopperWork.se_handle != null)
                {
                    AppMain.GmSoundStopSE(gmsGmkStopperWork.se_handle);
                    AppMain.GsSoundFreeSeHandle(gmsGmkStopperWork.se_handle);
                }
                gmsGmkStopperWork.se_handle = AppMain.GsSoundAllocSeHandle();
                gmsGmkStopperWork.se_handle.flag |= 2147483648U;
                AppMain.GmSoundPlaySE("Casino3", gmsGmkStopperWork.se_handle);
            }
        }
    }

    private static void gmGmkStopperHit(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        AppMain.GMS_GMK_STOPPER_WORK gmsGmkStopperWork = (AppMain.GMS_GMK_STOPPER_WORK)parentObj1;
        int num = 0;
        if (parentObj2 != AppMain.g_gm_main_system.ply_work[0])
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
            AppMain.GmPlySeqInitStopper(parentObj2, gmsGmkStopperWork.gmk_work.ene_com);
            gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
            parentObj1.flag |= 2U;
            gmsGmkStopperWork.ply_work = parentObj2;
            parentObj1.user_timer = (parentObj1.pos.y >> 12) + (int)gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.height + (int)gmsGmkStopperWork.gmk_work.ene_com.col_work.obj_col.ofst_y - (int)parentObj2.rect_work[0].rect.top;
            parentObj1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkStopperLockWait);
        }
        else
            mine_rect.flag &= 4294573823U;
    }

    private static void gmGmkStopperExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GMK_STOPPER_WORK tcbWork = (AppMain.GMS_GMK_STOPPER_WORK)AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.se_handle != null)
        {
            AppMain.GsSoundStopSeHandle(tcbWork.se_handle);
            AppMain.GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkStopperNormInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.GMS_GMK_STOPPER_WORK work = (AppMain.GMS_GMK_STOPPER_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_STOPPER_WORK()), "Gmk_StopperRod");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_stopper_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = 393216;
        AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(849).pData);
        gmsEnemy3DWork.obj_3d.mat_speed = 1f;
        obj_work.disp_flag |= 20U;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        work.call_slot_id = -1;
        AppMain.gmGmkStopperStart(obj_work);
        return obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkStopperSlotInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_STOPPER_WORK work = (AppMain.GMS_GMK_STOPPER_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_STOPPER_WORK()), "Gmk_StopperRod");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_stopper_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = 393216;
        AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 1, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(849).pData);
        gmsEnemy3DWork.obj_3d.mat_speed = 1f;
        obj_work.disp_flag |= 20U;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        work.call_slot_id = (int)eve_rec.left;
        AppMain.gmGmkStopperStart(obj_work);
        return obj_work;
    }

    private static void GmGmkStopperBuild()
    {
        AppMain.gm_gmk_stopper_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(847), AppMain.GmGameDatGetGimmickData(848), 0U);
    }

    private static void GmGmkStopperFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(847);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_stopper_obj_3d_list, gimmickData.file_num);
    }
}
