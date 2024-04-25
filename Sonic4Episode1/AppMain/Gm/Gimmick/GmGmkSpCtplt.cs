public partial class AppMain
{
    private static void gmGmkSpCtpltStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (GMS_GMK_SPCTPLT_WORK)obj_work;
        ObjObjectAction3dNNMotionLoad(obj_work, 0, false, ObjDataGet(885), null, 0, null);
        ObjDrawObjectActionSet(obj_work, 0);
        ObjObjectAction3dNNMaterialMotionLoad(obj_work, 0, ObjDataGet(886), null, 0, null);
        obj_work.obj_3d.mat_speed = 1f;
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 1);
        obj_work.disp_flag &= 4294967291U;
        ((NNS_MATERIAL_GLES11_DESC)obj_work.obj_3d._object.pMatPtrList[0].pMaterial).fFlag |= 1U;
        ((NNS_MATERIAL_GLES11_DESC)obj_work.obj_3d._object.pMatPtrList[1].pMaterial).fFlag |= 1U;
        gmsGmkSpctpltWork.gmk_work.ene_com.rect_work[2].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsGmkSpctpltWork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = null;
        pRec.ppHit = new OBS_RECT_WORK_Delegate1(gmGmkSpCtplt_PlayerHit);
        ObjRectWorkSet(pRec, tbl_gm_gmk_spctplt_rect[gmsGmkSpctpltWork.ctplt_id][0], tbl_gm_gmk_spctplt_rect[gmsGmkSpctpltWork.ctplt_id][1], tbl_gm_gmk_spctplt_rect[gmsGmkSpctpltWork.ctplt_id][2], tbl_gm_gmk_spctplt_rect[gmsGmkSpctpltWork.ctplt_id][3]);
        obj_work.flag &= 4294967293U;
        obj_work.dir.z = gmsGmkSpctpltWork.ctplt_tilt;
        gmsGmkSpctpltWork.ply_work = null;
        gmsGmkSpctpltWork.ctplt_height = 319488;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpCtpltStay);
    }

    private static void gmGmkSpCtpltStay(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_GMK_SPCTPLT_WORK)obj_work).ply_work != g_gm_main_system.ply_work[0])
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpCtplt_PlayerHold);
        gmGmkSpCtplt_PlayerHold(obj_work);
    }

    private static void gmGmkSpCtplt_PlayerHold(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (GMS_GMK_SPCTPLT_WORK)obj_work;
        GMS_PLAYER_WORK plyWork = gmsGmkSpctpltWork.ply_work;
        if (((int)plyWork.player_flag & 1024) != 0 || ((int)g_gm_main_system.game_flag & 262656) != 0)
            return;
        if (((int)g_gm_main_system.game_flag & 16781312) == 0)
        {
            if ((plyWork.key_release & 160) != 0)
            {
                int spd_x = 0;
                int num = (319488 - gmsGmkSpctpltWork.ctplt_height) / 2;
                plyWork.obj_work.spd_m = num;
                if (gmsGmkSpctpltWork.ctplt_tilt != 0)
                {
                    spd_x = num = (int)(2896.3093 * num / 4096.0);
                    if (gmsGmkSpctpltWork.ctplt_tilt == 57344)
                        spd_x = -spd_x;
                }
                else if (num < 8192)
                    num = 8192;
                else if (num > 36864)
                    num = 36864 + 9 * num / 21;
                ObjDrawObjectActionSet3DNNMaterial(obj_work, 1);
                obj_work.disp_flag &= 4294967291U;
                ObjDrawObjectActionSet(obj_work, 2);
                gmsGmkSpctpltWork.ctplt_height = 319488;
                plyWork.obj_work.dir.z = (ushort)(gmsGmkSpctpltWork.ctplt_tilt + 49152U);
                GmPlySeqInitPinballCtplt(plyWork, spd_x, -num);
                GMM_PAD_VIB_SMALL();
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpCtplt_PlayerHold_100);
                gmsGmkSpctpltWork.ctplt_return_timer = 4;
                gmsGmkSpctpltWork.ply_work = null;
                gmGmkSpCtpltSeStop(obj_work);
            }
            else if ((plyWork.key_on & 160) != 0)
            {
                if (gmsGmkSpctpltWork.ctplt_height == 319488)
                {
                    ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
                    obj_work.disp_flag &= 4294967291U;
                    ObjDrawObjectActionSet(obj_work, 1);
                    gmsGmkSpctpltWork.se_handle = GsSoundAllocSeHandle();
                    GmSoundPlaySE("Catapult1", gmsGmkSpctpltWork.se_handle);
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
        if (gmsGmkSpctpltWork.ctplt_tilt == 0)
        {
            num1 = 0;
            num2 = -gmsGmkSpctpltWork.ctplt_height;
        }
        else
        {
            num1 = num2 = -(int)(2896.3093 * gmsGmkSpctpltWork.ctplt_height / 4096.0);
            if (gmsGmkSpctpltWork.ctplt_tilt == 8192)
                num1 = -num1;
        }
        plyWork.obj_work.pos.x = obj_work.pos.x + num1;
        plyWork.obj_work.pos.y = obj_work.pos.y + num2;
    }

    private static void gmGmkSpCtplt_PlayerHold_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (GMS_GMK_SPCTPLT_WORK)obj_work;
        --gmsGmkSpctpltWork.ctplt_return_timer;
        if (gmsGmkSpctpltWork.ctplt_return_timer > 0)
            return;
        obj_work.flag &= 4294967293U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpCtpltStay);
        gmGmkSpCtpltStay(obj_work);
    }

    private static void gmGmkSpCtplt_PlayerHit(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (((int)parentObj2.player_flag & 1024) != 0 || ((int)parentObj2.obj_work.flag & 2) != 0 || (((int)g_gm_main_system.game_flag & 262656) != 0 || parentObj2 != g_gm_main_system.ply_work[0]))
            return;
        GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (GMS_GMK_SPCTPLT_WORK)parentObj1;
        GmPlySeqInitPinballCtpltHold(parentObj2, gmsGmkSpctpltWork.gmk_work.ene_com);
        parentObj2.obj_work.flag |= 2U;
        parentObj1.flag |= 2U;
        gmsGmkSpctpltWork.ply_work = parentObj2;
    }

    private static void gmGmkSpCtpltSeStop(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (GMS_GMK_SPCTPLT_WORK)obj_work;
        if (gmsGmkSpctpltWork.se_handle == null)
            return;
        GsSoundStopSeHandle(gmsGmkSpctpltWork.se_handle);
        GsSoundFreeSeHandle(gmsGmkSpctpltWork.se_handle);
        gmsGmkSpctpltWork.se_handle = null;
    }

    private static void gmGmkSpCtpltExit(MTS_TASK_TCB tcb)
    {
        gmGmkSpCtpltSeStop(mtTaskGetTcbWork(tcb));
        GmEnemyDefaultExit(tcb);
    }

    private static OBS_OBJECT_WORK gmGmkSpCtpltInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        GMS_GMK_SPCTPLT_WORK work = (GMS_GMK_SPCTPLT_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_SPCTPLT_WORK(), "Gmk_Seesaw");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_spctplt_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -4096;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkSpCtpltExit));
        work.se_handle = null;
        return obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkSpCtplt0Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (GMS_GMK_SPCTPLT_WORK)gmGmkSpCtpltInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSpctpltWork.ctplt_tilt = 0;
        gmsGmkSpctpltWork.ctplt_id = 0;
        gmGmkSpCtpltStart(gmsGmkSpctpltWork.gmk_work.ene_com.obj_work);
        return gmsGmkSpctpltWork.gmk_work.ene_com.obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkSpCtplt45Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (GMS_GMK_SPCTPLT_WORK)gmGmkSpCtpltInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSpctpltWork.ctplt_tilt = 8192;
        gmsGmkSpctpltWork.ctplt_id = 1;
        gmGmkSpCtpltStart(gmsGmkSpctpltWork.gmk_work.ene_com.obj_work);
        return gmsGmkSpctpltWork.gmk_work.ene_com.obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkSpCtplt315Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SPCTPLT_WORK gmsGmkSpctpltWork = (GMS_GMK_SPCTPLT_WORK)gmGmkSpCtpltInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSpctpltWork.ctplt_tilt = 57344;
        gmsGmkSpctpltWork.ctplt_id = 2;
        gmGmkSpCtpltStart(gmsGmkSpctpltWork.gmk_work.ene_com.obj_work);
        return gmsGmkSpctpltWork.gmk_work.ene_com.obj_work;
    }

    private static void GmGmkSpCtpltBuild()
    {
        gm_gmk_spctplt_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(883), GmGameDatGetGimmickData(884), 0U);
    }

    private static void GmGmkSpCtpltFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(883);
        GmGameDBuildRegFlushModel(gm_gmk_spctplt_obj_3d_list, gimmickData.file_num);
    }
}
