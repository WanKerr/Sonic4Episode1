using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    private static void gmGmkSpCtpltStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (AppMain.GMS_GMK_SPCTPLT_WORK)obj_work;
        AppMain.ObjObjectAction3dNNMotionLoad(obj_work, 0, false, AppMain.ObjDataGet(885), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectActionSet(obj_work, 0);
        AppMain.ObjObjectAction3dNNMaterialMotionLoad(obj_work, 0, AppMain.ObjDataGet(886), (string)null, 0, (object)null);
        obj_work.obj_3d.mat_speed = 1f;
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 1);
        obj_work.disp_flag &= 4294967291U;
        ((AppMain.NNS_MATERIAL_GLES11_DESC)obj_work.obj_3d._object.pMatPtrList[0].pMaterial).fFlag |= 1U;
        ((AppMain.NNS_MATERIAL_GLES11_DESC)obj_work.obj_3d._object.pMatPtrList[1].pMaterial).fFlag |= 1U;
        gmsGmkSpctpltWork.gmk_work.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsGmkSpctpltWork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
        pRec.ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkSpCtplt_PlayerHit);
        AppMain.ObjRectWorkSet(pRec, AppMain.tbl_gm_gmk_spctplt_rect[(int)gmsGmkSpctpltWork.ctplt_id][0], AppMain.tbl_gm_gmk_spctplt_rect[(int)gmsGmkSpctpltWork.ctplt_id][1], AppMain.tbl_gm_gmk_spctplt_rect[(int)gmsGmkSpctpltWork.ctplt_id][2], AppMain.tbl_gm_gmk_spctplt_rect[(int)gmsGmkSpctpltWork.ctplt_id][3]);
        obj_work.flag &= 4294967293U;
        obj_work.dir.z = gmsGmkSpctpltWork.ctplt_tilt;
        gmsGmkSpctpltWork.ply_work = (AppMain.GMS_PLAYER_WORK)null;
        gmsGmkSpctpltWork.ctplt_height = 319488;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpCtpltStay);
    }

    private static void gmGmkSpCtpltStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_GMK_SPCTPLT_WORK)obj_work).ply_work != AppMain.g_gm_main_system.ply_work[0])
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpCtplt_PlayerHold);
        AppMain.gmGmkSpCtplt_PlayerHold(obj_work);
    }

    private static void gmGmkSpCtplt_PlayerHold(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (AppMain.GMS_GMK_SPCTPLT_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK plyWork = gmsGmkSpctpltWork.ply_work;
        if (((int)plyWork.player_flag & 1024) != 0 || ((int)AppMain.g_gm_main_system.game_flag & 262656) != 0)
            return;
        if (((int)AppMain.g_gm_main_system.game_flag & 16781312) == 0)
        {
            if (((int)plyWork.key_release & 160) != 0)
            {
                int spd_x = 0;
                int num = (319488 - gmsGmkSpctpltWork.ctplt_height) / 2;
                plyWork.obj_work.spd_m = num;
                if (gmsGmkSpctpltWork.ctplt_tilt != (ushort)0)
                {
                    spd_x = num = (int)(2896.3093 * (double)num / 4096.0);
                    if (gmsGmkSpctpltWork.ctplt_tilt == (ushort)57344)
                        spd_x = -spd_x;
                }
                else if (num < 8192)
                    num = 8192;
                else if (num > 36864)
                    num = 36864 + 9 * num / 21;
                AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 1);
                obj_work.disp_flag &= 4294967291U;
                AppMain.ObjDrawObjectActionSet(obj_work, 2);
                gmsGmkSpctpltWork.ctplt_height = 319488;
                plyWork.obj_work.dir.z = (ushort)((uint)gmsGmkSpctpltWork.ctplt_tilt + 49152U);
                AppMain.GmPlySeqInitPinballCtplt(plyWork, spd_x, -num);
                AppMain.GMM_PAD_VIB_SMALL();
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpCtplt_PlayerHold_100);
                gmsGmkSpctpltWork.ctplt_return_timer = 4;
                gmsGmkSpctpltWork.ply_work = (AppMain.GMS_PLAYER_WORK)null;
                AppMain.gmGmkSpCtpltSeStop(obj_work);
            }
            else if (((int)plyWork.key_on & 160) != 0)
            {
                if (gmsGmkSpctpltWork.ctplt_height == 319488)
                {
                    AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
                    obj_work.disp_flag &= 4294967291U;
                    AppMain.ObjDrawObjectActionSet(obj_work, 1);
                    gmsGmkSpctpltWork.se_handle = AppMain.GsSoundAllocSeHandle();
                    AppMain.GmSoundPlaySE("Catapult1", gmsGmkSpctpltWork.se_handle);
                }
                if (gmsGmkSpctpltWork.ctplt_height > 147456)
                {
                    gmsGmkSpctpltWork.ctplt_height -= 3018;
                    if (gmsGmkSpctpltWork.ctplt_height < 147456)
                        gmsGmkSpctpltWork.ctplt_height = 147456;
                }
            }
        }
        int num1;
        int num2;
        if (gmsGmkSpctpltWork.ctplt_tilt == (ushort)0)
        {
            num1 = 0;
            num2 = -gmsGmkSpctpltWork.ctplt_height;
        }
        else
        {
            num1 = num2 = -(int)(2896.3093 * (double)gmsGmkSpctpltWork.ctplt_height / 4096.0);
            if (gmsGmkSpctpltWork.ctplt_tilt == (ushort)8192)
                num1 = -num1;
        }
        plyWork.obj_work.pos.x = obj_work.pos.x + num1;
        plyWork.obj_work.pos.y = obj_work.pos.y + num2;
    }

    private static void gmGmkSpCtplt_PlayerHold_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (AppMain.GMS_GMK_SPCTPLT_WORK)obj_work;
        --gmsGmkSpctpltWork.ctplt_return_timer;
        if (gmsGmkSpctpltWork.ctplt_return_timer > 0)
            return;
        obj_work.flag &= 4294967293U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpCtpltStay);
        AppMain.gmGmkSpCtpltStay(obj_work);
    }

    private static void gmGmkSpCtplt_PlayerHit(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (((int)parentObj2.player_flag & 1024) != 0 || ((int)parentObj2.obj_work.flag & 2) != 0 || (((int)AppMain.g_gm_main_system.game_flag & 262656) != 0 || parentObj2 != AppMain.g_gm_main_system.ply_work[0]))
            return;
        AppMain.GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (AppMain.GMS_GMK_SPCTPLT_WORK)parentObj1;
        AppMain.GmPlySeqInitPinballCtpltHold(parentObj2, gmsGmkSpctpltWork.gmk_work.ene_com);
        parentObj2.obj_work.flag |= 2U;
        parentObj1.flag |= 2U;
        gmsGmkSpctpltWork.ply_work = parentObj2;
    }

    private static void gmGmkSpCtpltSeStop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (AppMain.GMS_GMK_SPCTPLT_WORK)obj_work;
        if (gmsGmkSpctpltWork.se_handle == null)
            return;
        AppMain.GsSoundStopSeHandle(gmsGmkSpctpltWork.se_handle);
        AppMain.GsSoundFreeSeHandle(gmsGmkSpctpltWork.se_handle);
        gmsGmkSpctpltWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
    }

    private static void gmGmkSpCtpltExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmGmkSpCtpltSeStop(AppMain.mtTaskGetTcbWork(tcb));
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static AppMain.OBS_OBJECT_WORK gmGmkSpCtpltInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.GMS_GMK_SPCTPLT_WORK work = (AppMain.GMS_GMK_SPCTPLT_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SPCTPLT_WORK()), "Gmk_Seesaw");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_spctplt_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -4096;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkSpCtpltExit));
        work.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        return obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSpCtplt0Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (AppMain.GMS_GMK_SPCTPLT_WORK)AppMain.gmGmkSpCtpltInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSpctpltWork.ctplt_tilt = (ushort)0;
        gmsGmkSpctpltWork.ctplt_id = (ushort)0;
        AppMain.gmGmkSpCtpltStart(gmsGmkSpctpltWork.gmk_work.ene_com.obj_work);
        return gmsGmkSpctpltWork.gmk_work.ene_com.obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSpCtplt45Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (AppMain.GMS_GMK_SPCTPLT_WORK)AppMain.gmGmkSpCtpltInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSpctpltWork.ctplt_tilt = (ushort)8192;
        gmsGmkSpctpltWork.ctplt_id = (ushort)1;
        AppMain.gmGmkSpCtpltStart(gmsGmkSpctpltWork.gmk_work.ene_com.obj_work);
        return gmsGmkSpctpltWork.gmk_work.ene_com.obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSpCtplt315Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (AppMain.GMS_GMK_SPCTPLT_WORK)AppMain.gmGmkSpCtpltInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSpctpltWork.ctplt_tilt = (ushort)57344;
        gmsGmkSpctpltWork.ctplt_id = (ushort)2;
        AppMain.gmGmkSpCtpltStart(gmsGmkSpctpltWork.gmk_work.ene_com.obj_work);
        return gmsGmkSpctpltWork.gmk_work.ene_com.obj_work;
    }

    private static void GmGmkSpCtpltBuild()
    {
        AppMain.gm_gmk_spctplt_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(883), AppMain.GmGameDatGetGimmickData(884), 0U);
    }

    private static void GmGmkSpCtpltFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(883);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_spctplt_obj_3d_list, gimmickData.file_num);
    }
}
