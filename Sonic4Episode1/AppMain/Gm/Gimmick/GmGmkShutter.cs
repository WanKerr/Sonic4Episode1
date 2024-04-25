public partial class AppMain
{
    public static void GmGmkShutterBuild()
    {
        g_gm_gmk_shutter_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(938), GmGameDatGetGimmickData(939), 0U);
    }

    public static void GmGmkShutterFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(938);
        GmGameDBuildRegFlushModel(g_gm_gmk_shutter_obj_3d_list, gimmickData.file_num);
        g_gm_gmk_shutter_obj_3d_list = null;
    }

    private static OBS_OBJECT_WORK GmGmkShutterInInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkShutterLoadObj(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        gmGmkShutterInInit(objWork);
        return objWork;
    }

    private static OBS_OBJECT_WORK GmGmkShutterOutInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkShutterLoadObj(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        gmGmkShutterOutInit(objWork);
        return objWork;
    }

    private static void GmGmkShutterInChangeModeClose(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.pos.y <= (int)obj_work.user_work)
            return;
        obj_work.spd.y = -16384;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkShutterInMainClose);
        obj_work.disp_flag &= 4294967263U;
        GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (GMS_GMK_SHUTTER_WORK)obj_work;
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] != 4 || gmsGmkShutterWork.effect_work != null)
            return;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(null, 44);
        obsObjectWork.pos.x = obj_work.pos.x + 65536;
        obsObjectWork.pos.y = obj_work.pos.y - 131072;
        obsObjectWork.pos.z = 393216;
        gmsGmkShutterWork.effect_work = (GMS_EFFECT_3DES_WORK)obsObjectWork;
    }

    private static void GmGmkShutterOutChangeModeOpen(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.pos.y >= (int)obj_work.user_work)
            return;
        obj_work.spd.y = 16384;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkShutterOutMainOpen);
        GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (GMS_GMK_SHUTTER_WORK)obj_work;
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] != 4 || gmsGmkShutterWork.effect_work != null)
            return;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(null, 44);
        obsObjectWork.pos.x = obj_work.pos.x - 65536;
        obsObjectWork.pos.y = obj_work.pos.y + 131072;
        obsObjectWork.pos.z = 393216;
        gmsGmkShutterWork.effect_work = (GMS_EFFECT_3DES_WORK)obsObjectWork;
    }

    private static GMS_ENEMY_3D_WORK gmGmkShutterLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_SHUTTER_WORK(), "GMK_SHUTTER");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkShutterLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int num = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        int index1;
        switch (num)
        {
            case 1:
                index1 = 0;
                break;
            case 4:
                index1 = 0;
                break;
            default:
                return null;
        }
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkShutterLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        ObjObjectCopyAction3dNNModel(objWork, g_gm_gmk_shutter_obj_3d_list[index1], gmsEnemy3DWork.obj_3d);
        if (num == 4)
        {
            GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (GMS_GMK_SHUTTER_WORK)objWork;
            int index2 = 2;
            ObjCopyAction3dNNModel(g_gm_gmk_shutter_obj_3d_list[index2], gmsGmkShutterWork.obj_3d_parts);
            ObjAction3dNNMaterialMotionLoad(gmsGmkShutterWork.obj_3d_parts, 0, null, null, 1, (AMS_AMB_HEADER)ObjDataGet(940).pData);
        }
        return gmsEnemy3DWork;
    }

    private static void gmGmkShutterDestFuncForFinaleZone(MTS_TASK_TCB tcb)
    {
        ObjAction3dNNMotionRelease(((GMS_GMK_SHUTTER_WORK)mtTaskGetTcbWork(tcb)).obj_3d_parts);
        GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkShutterInInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = 64;
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = 64;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)(-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)(-gmsEnemy3DWork.ene_com.col_work.obj_col.height / 2);
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194336U;
        obj_work.flag |= 16U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        obj_work.pos.z = -655360;
        obj_work.user_work = (uint)(obj_work.pos.y - 262144);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkShutterInMainWaitClose);
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] != 4)
            return;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkShutterInOutFuncForFinalZone);
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkShutterDestFuncForFinaleZone));
    }

    private static void gmGmkShutterInOutFuncForFinalZone(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (GMS_GMK_SHUTTER_WORK)obj_work;
        obj_work.ofst.x = gm_gmk_shutter_disp_offset_for_final_zone[0] * 4096;
        obj_work.ofst.y = gm_gmk_shutter_disp_offset_for_final_zone[1] * 4096;
        ObjDrawActionSummary(obj_work);
        VecFx32 pos = obj_work.pos;
        pos.x += obj_work.ofst.x;
        pos.y += obj_work.ofst.y;
        uint p_disp_flag = obj_work.disp_flag | 4U;
        if (ObjObjectPauseCheck(0U) == 0U)
            ObjDrawAction3DNNMaterialUpdate(gmsGmkShutterWork.obj_3d_parts, ref p_disp_flag);
        ObjDrawAction3DNN(gmsGmkShutterWork.obj_3d_parts, new VecFx32?(pos), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref p_disp_flag);
    }

    private static void gmGmkShutterInMainWaitClose(OBS_OBJECT_WORK obj_work)
    {
        if (((OBS_OBJECT_WORK)g_gm_main_system.ply_work[0]).pos.x - obj_work.pos.x < 262144)
            return;
        GmGmkShutterInChangeModeClose(obj_work);
    }

    private static void gmGmkShutterInMainClose(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)g_gm_main_system.ply_work[0];
        int a = (int)(obj_work.user_work - obj_work.pos.y);
        if (MTM_MATH_ABS(obsObjectWork.pos.x - obj_work.pos.x) < 131072 && obsObjectWork.pos.y <= obj_work.pos.y && MTM_MATH_ABS(a) < 262144)
        {
            int num1 = g_gm_main_system.map_fcol.left + (g_gm_main_system.map_fcol.right - g_gm_main_system.map_fcol.left) / 2;
            if (((int)obsObjectWork.move_flag & 1) != 0)
            {
                int num2 = 16384;
                if (num1 * 4096 < obsObjectWork.pos.x)
                    num2 *= -1;
                obsObjectWork.flow.x += num2;
            }
            else
            {
                int spd_x = 4096;
                if (num1 * 4096 < obsObjectWork.pos.x)
                    spd_x *= -1;
                GmPlySeqGmkInitGmkJump((GMS_PLAYER_WORK)obsObjectWork, spd_x, 0);
                GmPlySeqChangeSequenceState((GMS_PLAYER_WORK)obsObjectWork, 17);
            }
        }
        if (obj_work.pos.y > (int)obj_work.user_work)
            return;
        obj_work.pos.y = (int)obj_work.user_work;
        obj_work.spd.y = 0;
        obj_work.ppFunc = null;
        obj_work.ppMove = null;
        GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (GMS_GMK_SHUTTER_WORK)obj_work;
        if (gmsGmkShutterWork.effect_work == null)
            return;
        ObjDrawKillAction3DES((OBS_OBJECT_WORK)gmsGmkShutterWork.effect_work);
        gmsGmkShutterWork.effect_work = null;
    }

    private static void gmGmkShutterOutInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = 64;
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = 64;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)(-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)(-gmsEnemy3DWork.ene_com.col_work.obj_col.height / 2);
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        obj_work.flag |= 16U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        obj_work.pos.z = -655360;
        obj_work.user_work = (uint)(obj_work.pos.y + 262144);
        obj_work.ppFunc = null;
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] != 4)
            return;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkShutterOutOutFuncForFinalZone);
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkShutterDestFuncForFinaleZone));
    }

    private static void gmGmkShutterOutOutFuncForFinalZone(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (GMS_GMK_SHUTTER_WORK)obj_work;
        obj_work.ofst.x = -gm_gmk_shutter_disp_offset_for_final_zone[0] * 4096;
        obj_work.ofst.y = gm_gmk_shutter_disp_offset_for_final_zone[1] * 4096;
        ObjDrawActionSummary(obj_work);
        VecFx32 pos = obj_work.pos;
        pos.x += obj_work.ofst.x;
        pos.y += obj_work.ofst.y;
        uint p_disp_flag = obj_work.disp_flag | 4U;
        if (ObjObjectPauseCheck(0U) == 0U)
            ObjDrawAction3DNNMaterialUpdate(gmsGmkShutterWork.obj_3d_parts, ref p_disp_flag);
        ObjDrawAction3DNN(gmsGmkShutterWork.obj_3d_parts, new VecFx32?(pos), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref p_disp_flag);
    }

    private static void gmGmkShutterOutMainOpen(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.pos.y < (int)obj_work.user_work)
            return;
        obj_work.pos.y = (int)obj_work.user_work;
        obj_work.spd.y = 0;
        obj_work.ppFunc = null;
        obj_work.ppMove = null;
        obj_work.disp_flag |= 32U;
        GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (GMS_GMK_SHUTTER_WORK)obj_work;
        if (gmsGmkShutterWork.effect_work == null)
            return;
        ObjDrawKillAction3DES((OBS_OBJECT_WORK)gmsGmkShutterWork.effect_work);
        gmsGmkShutterWork.effect_work = null;
    }

}