using gs.backup;

public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSsEmeraldInit(
     GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        bool flag = false;
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_SS_EMERALD");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= 128;
        ushort num = (ushort)(g_gs_main_sys_info.stage_id - 21U);
        if (SSpecial.CreateInstance()[num].IsGetEmerald())
        {
            flag = true;
            ObjObjectCopyAction3dNNModel(work, gm_gmk_ss_1up_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        }
        else
        {
            ObjObjectCopyAction3dNNModel(work, gm_gmk_ss_emerald_obj_3d_list[num], gmsEnemy3DWork.obj_3d);
            ObjObjectAction3dNNMotionLoad(work, 0, false, ObjDataGet(912), null, 0, null);
            ObjDrawObjectActionSet(work, num);
        }
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194308U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsEmeraldMain);
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkSsEmeraldDefFunc);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectWorkSet(pRec, -4, -4, 4, 4);
        int efct_zone_idx = !flag ? 1 + num : 0;
        gm_gmk_ss_emerald_effct = GmEfctZoneEsCreate(work, 5, efct_zone_idx);
        return work;
    }

    public static void GmGmkSsEmeraldBuild()
    {
        gm_gmk_ss_emerald_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(910), GmGameDatGetGimmickData(911), 0U);
        gm_gmk_ss_1up_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(913), GmGameDatGetGimmickData(914), 0U);
    }

    public static void GmGmkSsEmeraldFlush()
    {
        AMS_AMB_HEADER gimmickData1 = GmGameDatGetGimmickData(910);
        GmGameDBuildRegFlushModel(gm_gmk_ss_emerald_obj_3d_list, gimmickData1.file_num);
        AMS_AMB_HEADER gimmickData2 = GmGameDatGetGimmickData(913);
        GmGameDBuildRegFlushModel(gm_gmk_ss_1up_obj_3d_list, gimmickData2.file_num);
    }

    private static void gmGmkSsEmeraldMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.dir.z = GmMainGetObjectRotation();
    }

    private static void gmGmkSsEmeraldDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != 1 || parentObj2.gmk_obj == (OBS_OBJECT_WORK)parentObj1))
            return;
        GmSoundPlayJingle(3U, 0);
        GmSoundPlaySE("Special5");
        GmComEfctCreateRing(parentObj1.obj_work.pos.x, parentObj1.obj_work.pos.y);
        gmGmkSsEmeraldEfctKill();
        parentObj1.obj_work.flag |= 4U;
        g_gm_main_system.game_flag |= 65536U;
    }

    private static void gmGmkSsEmeraldEfctKill()
    {
        if (gm_gmk_ss_emerald_effct == null)
            return;
        ObjDrawKillAction3DES((OBS_OBJECT_WORK)gm_gmk_ss_emerald_effct);
        gm_gmk_ss_emerald_effct = null;
    }

}