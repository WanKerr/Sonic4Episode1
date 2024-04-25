public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkBoss3PillarInitManager(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)gmGmkBoss3PillarLoadObjNoModel(eve_rec, pos_x, pos_y, () => new GMS_GMK_BOSS3_PILLAR_MANAGER_WORK());
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)pillarManagerWork;
        gmGmkBoss3PillarManagerInit(obj_work);
        for (int index = 0; 2 > index; ++index)
        {
            int num1 = 0;
            int num2 = 0;
            switch (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id])
            {
                case 2:
                    num1 = g_gm_gmk_boss3_pillar_wall_default_pos[index][0];
                    num2 = g_gm_gmk_boss3_pillar_wall_default_pos[index][1];
                    break;
                case 4:
                    num1 = g_gm_gmk_boss3_pillar_f_wall_default_pos[index][0];
                    num2 = g_gm_gmk_boss3_pillar_f_wall_default_pos[index][1];
                    break;
            }
            OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(343, obj_work.pos.x + num1 * 4096, obj_work.pos.y + num2 * 4096, eve_rec.flag, eve_rec.left, eve_rec.top, eve_rec.width, eve_rec.height, (byte)index);
            obsObjectWork.parent_obj = obj_work;
            pillarManagerWork.obj_work_wall[index] = obsObjectWork;
            if ((pillarManagerWork.gimmick_work.ene_com.eve_rec.byte_param[1] & 1) != 0 && index == 0)
            {
                int num3 = 917504;
                obsObjectWork.pos.y -= num3;
            }
        }
        return obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkBoss3PillarInitParts(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        uint pillar_type = gmGmkBoss3PillarCalcPillarType(eve_rec.id);
        int num;
        int[] numArray;
        switch (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id])
        {
            case 2:
                num = g_gm_gmk_boss3_pillar_parts_num[(int)pillar_type];
                numArray = g_gm_boss3_pillar_model_id[(int)pillar_type];
                break;
            case 4:
                num = g_gm_gmk_boss3_pillar_f_parts_num[(int)pillar_type];
                numArray = g_gm_boss3_pillar_f_model_id[(int)pillar_type];
                break;
            default:
                return null;
        }
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkBoss3PillarLoadObj(eve_rec, pos_x, pos_y, g_gm_gmk_boss3_pillar_obj_3d_list, (uint)numArray[0], () => new GMS_GMK_BOSS3_PILLAR_MAIN_WORK());
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)gmsEnemy3DWork;
        gmGmkBoss3PillarPartsInitMain(obj_work, pillar_type);
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)gmsEnemy3DWork;
        for (int index1 = 1; num > index1; ++index1)
        {
            int index2 = numArray[index1];
            ObjCopyAction3dNNModel(g_gm_gmk_boss3_pillar_obj_3d_list[index2], s3PillarMainWork.obj_3d_parts[index1 - 1]);
            s3PillarMainWork.obj_3d_parts[index1 - 1].drawflag |= 32U;
        }
        return obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkBoss3PillarInitWall(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)gmGmkBoss3PillarLoadObj(eve_rec, pos_x, pos_y, g_gm_gmk_boss3_wall_obj_3d_list, 0U, () => new GMS_GMK_BOSS3_PILLAR_WALL_WORK());
        GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        int index = g_gm_gmk_boss3_wall_obj_3d_list.Length > 1 ? 1 : 0;
        ObjCopyAction3dNNModel(g_gm_gmk_boss3_wall_obj_3d_list[index], s3PillarWallWork.obj_3d_parts[0]);
        gmGmkBoss3PillarWallInit(obj_work);
        return obj_work;
    }

    public static void GmGmkBoss3PillarBuild()
    {
        g_gm_gmk_boss3_pillar_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(941), GmGameDatGetGimmickData(942), 0U);
        g_gm_gmk_boss3_wall_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(945), GmGameDatGetGimmickData(946), 0U);
        g_gm_gmk_boss3_pillar_obj_tvx_list = GmGameDatGetGimmickData(944);
        g_gm_gmk_boss3_wall_obj_tvx_list = GmGameDatGetGimmickData(948);
        gm_gmk_boss3_pillar_se_use_count = 0;
        gm_gmk_boss3_pillar_global_flag = 0;
    }

    public static void GmGmkBoss3PillarFlush()
    {
        AMS_AMB_HEADER gimmickData1 = GmGameDatGetGimmickData(941);
        GmGameDBuildRegFlushModel(g_gm_gmk_boss3_pillar_obj_3d_list, gimmickData1.file_num);
        g_gm_gmk_boss3_pillar_obj_3d_list = null;
        AMS_AMB_HEADER gimmickData2 = GmGameDatGetGimmickData(945);
        GmGameDBuildRegFlushModel(g_gm_gmk_boss3_wall_obj_3d_list, gimmickData2.file_num);
        g_gm_gmk_boss3_wall_obj_3d_list = null;
        g_gm_gmk_boss3_pillar_obj_tvx_list = null;
        g_gm_gmk_boss3_wall_obj_tvx_list = null;
        gm_gmk_boss3_pillar_se_use_count = 0;
        gm_gmk_boss3_pillar_global_flag = 0;
    }

    private static void GmGmkBoss3PillarChangeModeActive(
      OBS_OBJECT_WORK obj_work,
      int pattern_no)
    {
        GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        float num1 = 80f;
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] == 4)
            num1 /= 1.5f;
        for (int index = 0; 26 > index; ++index)
        {
            OBS_OBJECT_WORK obj_work1 = pillarManagerWork.obj_work_pillar[index];
            int num2 = g_gm_gmk_boss3_pillar_move_distance[pattern_no][index] * 40;
            if (num2 == 0)
            {
                if (obj_work1 != null)
                {
                    obj_work1.flag |= 8U;
                    GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work1;
                    if (s3PillarMainWork.effect_work != null)
                    {
                        ObjDrawKillAction3DES((OBS_OBJECT_WORK)s3PillarMainWork.effect_work);
                        s3PillarMainWork.effect_work = null;
                    }
                    pillarManagerWork.obj_work_pillar[index] = null;
                }
            }
            else
            {
                if (obj_work1 == null)
                {
                    obj_work1 = GmEventMgrLocalEventBirth((ushort)g_gm_gmk_boss3_pillar_event_id[index], obj_work.pos.x + g_gm_gmk_boss3_pillar_default_pos[index][0] * 40 * 4096, obj_work.pos.y + g_gm_gmk_boss3_pillar_default_pos[index][1] * 40 * 4096, gmsEnemy3DWork.ene_com.eve_rec.flag, gmsEnemy3DWork.ene_com.eve_rec.left, gmsEnemy3DWork.ene_com.eve_rec.top, gmsEnemy3DWork.ene_com.eve_rec.width, gmsEnemy3DWork.ene_com.eve_rec.height, 0);
                    obj_work1.parent_obj = obj_work;
                    pillarManagerWork.obj_work_pillar[index] = obj_work1;
                }
                else
                    gmGmkBoss3PillarPartsChangeModeWait(obj_work1);
                if (g_gm_gmk_boss3_pillar_flag_hit_effect[pattern_no][index] != 0)
                {
                    obj_work1.user_flag |= 9U;
                    gm_gmk_boss3_pillar_global_flag |= 8;
                }
                int wait_time = g_gm_gmk_boss3_pillar_wait_frame[pattern_no][index] * (int)num1;
                gmGmkBoss3PillarPartsChangeModeNormal(obj_work1, (num2 + 16) * 4096, wait_time);
            }
        }
        pillarManagerWork.pattern_no = pattern_no;
        obj_work.user_timer = (int)(g_gm_gmk_boss3_pillar_frame_change_hurry[pillarManagerWork.pattern_no] * (double)num1);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarManagerMainFuncWaitHurry);
    }

    private static void GmGmkBoss3PillarChangeModeHurry(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        for (int index = 0; 26 > index; ++index)
        {
            OBS_OBJECT_WORK obj_work1 = pillarManagerWork.obj_work_pillar[index];
            if (obj_work1 != null)
                gmGmkBoss3PillarPartsChangeModeHurry(obj_work1, 50);
        }
    }

    private static void GmGmkBoss3PillarChangeModeReturn(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        for (int index = 0; 26 > index; ++index)
        {
            OBS_OBJECT_WORK obj_work1 = pillarManagerWork.obj_work_pillar[index];
            if (obj_work1 != null)
                gmGmkBoss3PillarPartsChangeModeReturn(obj_work1, 0, 30);
        }
        obj_work.user_timer = 30;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarManagerMainFuncWaitReturn);
    }

    private static void GmGmkBoss3PillarChangeModeDelete(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        for (int index = 0; 26 > index; ++index)
        {
            OBS_OBJECT_WORK obsObjectWork = pillarManagerWork.obj_work_pillar[index];
            if (obsObjectWork != null)
            {
                obsObjectWork.flag |= 8U;
                pillarManagerWork.obj_work_pillar[index] = null;
            }
        }
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarManagerMainFuncFw);
    }

    private static int GmGmkBoss3PillarGetActiveTime(int pattern_no)
    {
        float num = 80f;
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] == 4)
            num /= 1.5f;
        return (int)(g_gm_gmk_boss3_pillar_frame_change_hurry[pattern_no] * (double)num + 50.0);
    }

    private static void GmGmkBoss3PillarWallChangeModeActive(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        for (int index = 0; 2 > index; ++index)
        {
            OBS_OBJECT_WORK obj_work1 = pillarManagerWork.obj_work_wall[index];
            obj_work1.user_flag |= 1U;
            obj_work1.user_flag |= 4U;
            gmGmkBoss3PillarWallChangeModeActive(obj_work1);
        }
        pillarManagerWork.obj_work_wall[0].user_flag |= 10U;
        gm_gmk_boss3_pillar_global_flag |= 12;
    }

    private static void GmGmkBoss3PillarWallChangeModeReturn(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        pillarManagerWork.gimmick_work.ene_com.eve_rec.byte_param[1] |= 1;
        gmGmkBoss3PillarWallChangeModeReturn(pillarManagerWork.obj_work_wall[1]);
    }

    private static void GmGmkBoss3PillarWallClearFlagNoPressDie(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        for (int index = 0; 2 > index; ++index)
            ((GMS_GMK_BOSS3_PILLAR_WALL_WORK)pillarManagerWork.obj_work_wall[index]).gimmick_work.ene_com.enemy_flag &= 4294950911U;
    }

    private static GMS_ENEMY_3D_WORK gmGmkBoss3PillarLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      TaskWorkFactoryDelegate work_size)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, work_size, "GMK_BOSS3_PILLAR");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkBoss3PillarLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      OBS_ACTION3D_NN_WORK[] data_work_list,
      uint model_id,
      TaskWorkFactoryDelegate work_size)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkBoss3PillarLoadObjNoModel(eve_rec, pos_x, pos_y, work_size);
        ObjObjectCopyAction3dNNModel(gmsEnemy3DWork.ene_com.obj_work, data_work_list[(int)model_id], gmsEnemy3DWork.obj_3d);
        return gmsEnemy3DWork;
    }

    private static void gmGmkBoss3PillarManagerInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 256U;
        obj_work.ppMove = null;
        obj_work.ppOut = null;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarManagerMainFuncFw);
    }

    private static void gmGmkBoss3PillarManagerMainFuncWaitHurry(OBS_OBJECT_WORK obj_work)
    {
        gm_gmk_boss3_pillar_global_flag = -1;
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            GmGmkBoss3PillarChangeModeHurry(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarManagerMainFuncWaitHurryEnd);
            obj_work.user_timer = 50;
        }
    }

    private static void gmGmkBoss3PillarManagerMainFuncWaitHurryEnd(OBS_OBJECT_WORK obj_work)
    {
        gm_gmk_boss3_pillar_global_flag = -1;
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            GmCameraVibrationSet(0, 12288, 0);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarManagerMainFuncFw);
        }
    }

    private static void gmGmkBoss3PillarManagerMainFuncWaitReturn(OBS_OBJECT_WORK obj_work)
    {
        gm_gmk_boss3_pillar_global_flag = -1;
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            GmGmkBoss3PillarChangeModeDelete(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarManagerMainFuncFw);
        }
    }

    private static void gmGmkBoss3PillarManagerMainFuncFw(OBS_OBJECT_WORK obj_work)
    {
        gm_gmk_boss3_pillar_global_flag = -1;
    }

    private static uint gmGmkBoss3PillarCalcPillarType(int event_id)
    {
        return (uint)(event_id - 339);
    }

    private static void gmGmkBoss3PillarSetFieldRect(
      OBS_COLLISION_OBJ field_obj,
      uint pillar_type)
    {
        switch (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id])
        {
            case 2:
                field_obj.width = (ushort)g_gm_boss3_pillar_field_rect_wh[(int)pillar_type][0];
                field_obj.height = (ushort)g_gm_boss3_pillar_field_rect_wh[(int)pillar_type][1];
                field_obj.ofst_x = (short)g_gm_boss3_pillar_field_rect_offset[(int)pillar_type][0];
                field_obj.ofst_y = (short)g_gm_boss3_pillar_field_rect_offset[(int)pillar_type][1];
                break;
            case 4:
                field_obj.width = (ushort)g_gm_boss3_pillar_f_field_rect_wh[(int)pillar_type][0];
                field_obj.height = (ushort)g_gm_boss3_pillar_f_field_rect_wh[(int)pillar_type][1];
                field_obj.ofst_x = (short)g_gm_boss3_pillar_f_field_rect_offset[(int)pillar_type][0];
                field_obj.ofst_y = (short)g_gm_boss3_pillar_f_field_rect_offset[(int)pillar_type][1];
                break;
        }
    }

    private static void gmGmkBoss3PillarSetMoveSpeed(
      OBS_OBJECT_WORK obj_work,
      uint pillar_type,
      int speed)
    {
        int num1 = FX_Mul(speed, g_gm_boss3_pillar_adjust_dir[(int)pillar_type][0] * 4096);
        int num2 = FX_Mul(speed, g_gm_boss3_pillar_adjust_dir[(int)pillar_type][1] * 4096);
        obj_work.spd.x = num1;
        obj_work.spd.y = num2;
    }

    private static void gmGmkBoss3PillarSetMoveTarget(
      OBS_OBJECT_WORK obj_work,
      uint pillar_type,
      int distance)
    {
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        int num1 = FX_Mul(distance, g_gm_boss3_pillar_adjust_dir[(int)pillar_type][0] * 4096);
        int num2 = FX_Mul(distance, g_gm_boss3_pillar_adjust_dir[(int)pillar_type][1] * 4096);
        s3PillarMainWork.target_pos.x = s3PillarMainWork.default_pos.x + num1;
        s3PillarMainWork.target_pos.y = s3PillarMainWork.default_pos.y + num2;
    }

    private static void gmGmkBoss3PillarSetMoveHurry(OBS_OBJECT_WORK obj_work, int frame)
    {
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        if (frame <= 0)
        {
            obj_work.pos.Assign(s3PillarMainWork.target_pos);
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        else
        {
            int numer1 = s3PillarMainWork.target_pos.x - obj_work.pos.x;
            int numer2 = s3PillarMainWork.target_pos.y - obj_work.pos.y;
            int num1 = FX_Div(numer1, frame * 4096);
            int num2 = FX_Div(numer2, frame * 4096);
            obj_work.spd.x = num1;
            obj_work.spd.y = num2;
        }
    }

    private static bool gmGmkBoss3PillarCheckMoveEnd(
      OBS_OBJECT_WORK obj_work,
      uint pillar_type)
    {
        UNREFERENCED_PARAMETER(pillar_type);
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        bool flag = false;
        int x = obj_work.spd.x;
        int y = obj_work.spd.y;
        if (x == 0 && y == 0)
        {
            flag = true;
        }
        else
        {
            int num1 = s3PillarMainWork.target_pos.x - obj_work.pos.x;
            int num2 = s3PillarMainWork.target_pos.y - obj_work.pos.y;
            if (x < 0 && num1 >= 0 || x > 0 && num1 <= 0 || (y < 0 && num2 >= 0 || y > 0 && num2 <= 0))
                flag = true;
        }
        return flag;
    }

    private static void gmBoss3PillarDestFunc(MTS_TASK_TCB tcb)
    {
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK tcbWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)mtTaskGetTcbWork(tcb);
        if (tcbWork.effect_work != null)
        {
            ObjDrawKillAction3DES((OBS_OBJECT_WORK)tcbWork.effect_work);
            tcbWork.effect_work = null;
        }
        if (tcbWork.se_handle != null)
            GmSoundStopSE(tcbWork.se_handle);
        GMM_PAD_VIB_STOP();
        if (tcbWork.se_handle != null)
        {
            GmSoundStopSE(tcbWork.se_handle);
            GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = null;
        }
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss3PillarOutFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        uint num1 = gmGmkBoss3PillarCalcPillarType(((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        int num2 = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        int num3 = 0;
        int[] numArray1 = null;
        int[] numArray2 = null;
        AMS_AMB_HEADER pillarObjTvxList = g_gm_gmk_boss3_pillar_obj_tvx_list;
        switch (num2)
        {
            case 2:
                num3 = g_gm_gmk_boss3_pillar_parts_num[(int)num1];
                numArray1 = g_gm_boss3_pillar_parts_offset[(int)num1];
                numArray2 = g_gm_boss3_pillar_model_id[(int)num1];
                break;
            case 4:
                num3 = g_gm_gmk_boss3_pillar_f_parts_num[(int)num1];
                numArray1 = g_gm_boss3_pillar_f_parts_offset[(int)num1];
                numArray2 = g_gm_boss3_pillar_f_model_id[(int)num1];
                int num4 = g_gm_boss3_pillar_f_sub_model_id[(int)num1];
                break;
        }
        uint gmdTvxDispScale = GMD_TVX_DISP_SCALE;
        VecFx32 scale;
        scale.x = FX_F32_TO_FX32(1.25f);
        scale.y = FX_F32_TO_FX32(1.25f);
        scale.z = obj_work.scale.z;
        if (((int)obj_work.disp_flag & 1) != 0)
            scale.x *= -1;
        else if (((int)obj_work.disp_flag & 2) != 0)
            scale.y *= -1;
        TVX_FILE model_tvx;
        if (pillarObjTvxList.buf[numArray2[0]] == null)
        {
            model_tvx = new TVX_FILE((AmbChunk)amBindGet(pillarObjTvxList, numArray2[0]));
            pillarObjTvxList.buf[numArray2[0]] = model_tvx;
        }
        else
            model_tvx = (TVX_FILE)pillarObjTvxList.buf[numArray2[0]];
        GmTvxSetModel(model_tvx, obj_work.obj_3d.texlist, ref obj_work.pos, ref scale, gmdTvxDispScale, 0);
        for (int index = 1; num3 > index; ++index)
        {
            int num5 = -numArray1[index];
            int num6 = -numArray1[index];
            int num7 = num5 * (g_gm_boss3_pillar_adjust_dir[(int)num1][0] * 40);
            int num8 = num6 * (g_gm_boss3_pillar_adjust_dir[(int)num1][1] * 40);
            VecFx32 pos = new VecFx32(obj_work.pos);
            pos.x += num7 * 4096;
            pos.y += num8 * 4096;
            GmTvxSetModel(new TVX_FILE((AmbChunk)amBindGet(pillarObjTvxList, numArray2[index])), obj_work.obj_3d.texlist, ref pos, ref scale, gmdTvxDispScale, 0);
        }
    }

    private static void gmGmkBoss3PillarPartsInitMain(
      OBS_OBJECT_WORK obj_work,
      uint pillar_type)
    {
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
        gmGmkBoss3PillarSetFieldRect(gmsEnemy3DWork.ene_com.col_work.obj_col, pillar_type);
        obj_work.disp_flag |= 67108864U;
        obj_work.flag |= 16U;
        obj_work.move_flag |= 4112U;
        obj_work.move_flag &= 4294967167U;
        obj_work.move_flag |= 256U;
        switch (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id])
        {
            case 2:
                switch (pillar_type)
                {
                    case 0:
                        obj_work.disp_flag |= 1U;
                        break;
                    case 2:
                        obj_work.disp_flag |= 2U;
                        break;
                }
                gmsEnemy3DWork.obj_3d.drawflag |= 32U;
                obj_work.pos.x += FX_F32_TO_FX32(g_gm_gmk_boss3_pillar_adjust_default_pos[(int)pillar_type][0]);
                obj_work.pos.y += FX_F32_TO_FX32(g_gm_gmk_boss3_pillar_adjust_default_pos[(int)pillar_type][1]);
                break;
            case 4:
                obj_work.pos.x += FX_F32_TO_FX32(g_gm_gmk_boss3_pillar_f_adjust_default_pos[(int)pillar_type][0]);
                obj_work.pos.y += FX_F32_TO_FX32(g_gm_gmk_boss3_pillar_f_adjust_default_pos[(int)pillar_type][1]);
                break;
        }
        obj_work.pos.z = -655360;
        s3PillarMainWork.default_pos.Assign(obj_work.pos);
        s3PillarMainWork.target_pos.Assign(s3PillarMainWork.default_pos);
        s3PillarMainWork.se_handle = GsSoundAllocSeHandle();
        gmGmkBoss3PillarPartsChangeModeWait(obj_work);
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3PillarOutFunc);
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmBoss3PillarDestFunc));
    }

    private static void gmGmkBoss3PillarPartsChangeModeWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        obj_work.pos.Assign(s3PillarMainWork.target_pos);
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarPartsMainWait);
        if (s3PillarMainWork.effect_work == null)
            return;
        ObjDrawKillAction3DES((OBS_OBJECT_WORK)s3PillarMainWork.effect_work);
        s3PillarMainWork.effect_work = null;
    }

    private static void gmGmkBoss3PillarPartsChangeModeNormal(
      OBS_OBJECT_WORK obj_work,
      int distance,
      int wait_time)
    {
        uint pillar_type = gmGmkBoss3PillarCalcPillarType(((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        gmGmkBoss3PillarSetMoveTarget(obj_work, pillar_type, distance);
        float x = 0.5f;
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] == 4)
            x *= 1.5f;
        gmGmkBoss3PillarSetMoveSpeed(obj_work, pillar_type, FX_F32_TO_FX32(x));
        obj_work.user_flag |= 4U;
        gm_gmk_boss3_pillar_global_flag |= 4;
        obj_work.user_timer = wait_time;
        obj_work.move_flag |= 8192U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarPartsMainReady);
    }

    private static void gmGmkBoss3PillarPartsChangeModeHurry(
      OBS_OBJECT_WORK obj_work,
      int move_frame)
    {
        gmGmkBoss3PillarSetMoveHurry(obj_work, move_frame);
        obj_work.user_timer = 0;
        obj_work.move_flag &= 4294959103U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarPartsMainActive);
    }

    private static void gmGmkBoss3PillarPartsChangeModeReturn(
      OBS_OBJECT_WORK obj_work,
      int wait_time,
      int move_frame)
    {
        uint pillar_type = gmGmkBoss3PillarCalcPillarType(((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        gmGmkBoss3PillarSetMoveTarget(obj_work, pillar_type, 0);
        gmGmkBoss3PillarSetMoveHurry(obj_work, move_frame);
        obj_work.user_flag &= 4294967294U;
        obj_work.user_flag |= 4U;
        obj_work.user_flag &= 4294967287U;
        gm_gmk_boss3_pillar_global_flag |= 4;
        gm_gmk_boss3_pillar_global_flag = (int)(gm_gmk_boss3_pillar_global_flag & 4294967287L);
        obj_work.user_timer = wait_time;
        obj_work.move_flag |= 8192U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarPartsMainReady);
    }

    private static void gmGmkBoss3PillarPartsMainWait(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkBoss3PillarPartsMainReady(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            obj_work.move_flag &= 4294959103U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarPartsMainActive);
            gmGmkBoss3PillarEffectCreatePillarAppear(obj_work);
            obj_work.user_timer = 45;
            if ((obj_work.user_flag & gm_gmk_boss3_pillar_global_flag & 4L) == 0L)
                return;
            GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
            if (s3PillarMainWork.se_handle != null)
                GmSoundPlaySE("Boss3_01", s3PillarMainWork.se_handle);
            obj_work.user_flag &= 4294967291U;
            gm_gmk_boss3_pillar_global_flag = (int)(gm_gmk_boss3_pillar_global_flag & 4294967291L);
            GMM_PAD_VIB_SMALL_NOEND();
        }
    }

    private static void gmGmkBoss3PillarPartsMainActive(OBS_OBJECT_WORK obj_work)
    {
        uint pillar_type = gmGmkBoss3PillarCalcPillarType(((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        if (--obj_work.user_timer == 0)
        {
            GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
            if (s3PillarMainWork.effect_work != null)
            {
                ObjDrawKillAction3DES((OBS_OBJECT_WORK)s3PillarMainWork.effect_work);
                s3PillarMainWork.effect_work = null;
            }
        }
        if (!gmGmkBoss3PillarCheckMoveEnd(obj_work, pillar_type))
            return;
        if (((int)obj_work.user_flag & 1) != 0)
        {
            gmGmkBoss3PillarEffectCreatePillarHit(obj_work);
            obj_work.user_flag &= 4294967294U;
            GMM_PAD_VIB_STOP();
        }
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork1 = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        if (s3PillarMainWork1.se_handle != null)
        {
            GmSoundStopSE(s3PillarMainWork1.se_handle);
            if ((obj_work.user_flag & gm_gmk_boss3_pillar_global_flag & 8L) != 0L)
            {
                GmSoundPlaySE("Boss3_02", s3PillarMainWork1.se_handle);
                obj_work.user_flag &= 4294967287U;
                gm_gmk_boss3_pillar_global_flag = (int)(gm_gmk_boss3_pillar_global_flag & 4294967287L);
            }
        }
        gmGmkBoss3PillarPartsChangeModeWait(obj_work);
    }

    private static GSS_SND_SE_HANDLE gmGmkBoss3PillarGetSeHandle()
    {
        GSS_SND_SE_HANDLE gssSndSeHandle = null;
        if (gm_gmk_boss3_pillar_se_use_count <= 0)
        {
            gssSndSeHandle = GsSoundAllocSeHandle();
            ++gm_gmk_boss3_pillar_se_use_count;
        }
        return gssSndSeHandle;
    }

    private static void gmGmkBoss3PillarFreeHandle(GSS_SND_SE_HANDLE se_handle)
    {
        if (se_handle == null)
            return;
        GmSoundStopSE(se_handle);
        GsSoundFreeSeHandle(se_handle);
        --gm_gmk_boss3_pillar_se_use_count;
    }

    private static void gmGmkBoss3PillarWallInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        int num = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        switch (num)
        {
            case 2:
                gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
                gmsEnemy3DWork.ene_com.col_work.obj_col.width = 32;
                gmsEnemy3DWork.ene_com.col_work.obj_col.height = 192;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = -16;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = -16;
                break;
            case 4:
                gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
                gmsEnemy3DWork.ene_com.col_work.obj_col.width = 32;
                gmsEnemy3DWork.ene_com.col_work.obj_col.height = 192;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = -16;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = 0;
                break;
        }
        obj_work.disp_flag |= 67108864U;
        obj_work.flag |= 16U;
        obj_work.move_flag |= 272U;
        obj_work.move_flag &= 4294967167U;
        s3PillarWallWork.gimmick_work.ene_com.enemy_flag |= 16384U;
        obj_work.pos.z = -393216;
        s3PillarWallWork.default_pos.Assign(obj_work.pos);
        s3PillarWallWork.target_pos.Assign(s3PillarWallWork.default_pos);
        s3PillarWallWork.se_handle = GsSoundAllocSeHandle();
        obj_work.user_flag |= 16U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarWallMainWait);
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmBoss3PillarWallDestFunc));
        if (num == 2)
        {
            obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3PillarWallOutFunc);
        }
        else
        {
            if (num != 4)
                return;
            obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3PillarWallOutFuncForFinalZone);
        }
    }

    private static void gmBoss3PillarWallDestFunc(MTS_TASK_TCB tcb)
    {
        GMS_GMK_BOSS3_PILLAR_WALL_WORK tcbWork = (GMS_GMK_BOSS3_PILLAR_WALL_WORK)mtTaskGetTcbWork(tcb);
        if (tcbWork.effect_work != null)
        {
            ObjDrawKillAction3DES((OBS_OBJECT_WORK)tcbWork.effect_work);
            tcbWork.effect_work = null;
        }
        GMM_PAD_VIB_STOP();
        if (tcbWork.se_handle != null)
        {
            GmSoundStopSE(tcbWork.se_handle);
            GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = null;
        }
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss3PillarWallOutFunc(OBS_OBJECT_WORK obj_work)
    {
        int num1 = 6;
        int num2 = 32;
        if (((int)obj_work.disp_flag & 32) != 0)
            return;
        TVX_FILE model_tvx = new TVX_FILE((AmbChunk)amBindGet(g_gm_gmk_boss3_wall_obj_tvx_list, 0));
        VecFx32 scale = new VecFx32(4096, 4096, 4096);
        for (int index = 0; num1 > index; ++index)
        {
            VecFx32 pos = new VecFx32(obj_work.pos);
            pos.y += num2 * index * 4096;
            GmTvxSetModel(model_tvx, obj_work.obj_3d.texlist, ref pos, ref scale, 0U, 0);
        }
    }

    private static void gmBoss3PillarWallOutFuncForFinalZone(OBS_OBJECT_WORK obj_work)
    {
        int num1 = 3;
        int num2 = 64;
        GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        uint num3 = obj_work.disp_flag | 4U | 4096U;
        obj_work.ofst.y = num2 * 4096;
        ObjDrawActionSummary(obj_work);
        for (int index = 1; num1 > index; ++index)
        {
            VecFx32 vecFx32 = new VecFx32(obj_work.pos);
            vecFx32.y += obj_work.ofst.y + num2 * (index - 1) * 4096;
            ObjDrawAction3DNN(s3PillarWallWork.obj_3d_parts[0], new VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref obj_work.disp_flag);
        }
    }

    private static void gmGmkBoss3PillarWallChangeModeWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        obj_work.spd.y = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarWallMainWait);
        if (s3PillarWallWork.effect_work == null)
            return;
        ObjDrawKillAction3DES((OBS_OBJECT_WORK)s3PillarWallWork.effect_work);
        s3PillarWallWork.effect_work = null;
    }

    private static void gmGmkBoss3PillarWallChangeModeActive(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        int num1 = 917504;
        s3PillarWallWork.target_pos.y = s3PillarWallWork.default_pos.y - num1;
        obj_work.spd.y = -12288;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarWallMainActive);
        gmGmkBoss3PillarEffectCreateWallAppear(obj_work);
        if (((int)obj_work.user_flag & 4) != 0)
        {
            if (s3PillarWallWork.se_handle != null)
                GmSoundPlaySE("Boss3_01", s3PillarWallWork.se_handle);
            obj_work.user_flag &= 4294967291U;
            GMM_PAD_VIB_SMALL_NOEND();
        }
        short num2 = -16;
        if ((g_gm_main_system.map_fcol.left + (g_gm_main_system.map_fcol.right - g_gm_main_system.map_fcol.left) / 2) * 4096 > obj_work.pos.x)
            num2 = -32;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = num2;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = 48;
    }

    private static void gmGmkBoss3PillarWallChangeModeReturn(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        s3PillarWallWork.target_pos.Assign(s3PillarWallWork.default_pos);
        obj_work.spd.y = 12288;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3PillarWallMainActive);
        obj_work.user_flag &= 4294967279U;
        gmGmkBoss3PillarEffectCreateWallAppear(obj_work);
        if (s3PillarWallWork.se_handle != null)
            GmSoundPlaySE("Boss3_01", s3PillarWallWork.se_handle);
        GMM_PAD_VIB_SMALL_NOEND();
    }

    private static int gmGmkBoss3PillarWallCheckMoveEnd(
      GMS_GMK_BOSS3_PILLAR_WALL_WORK wall_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)wall_work;
        if (wall_work.target_pos.y == obsObjectWork.pos.y || obsObjectWork.spd.y == 0)
            return 0;
        int num = wall_work.target_pos.y - obsObjectWork.pos.y;
        if (obsObjectWork.spd.y < 0)
        {
            if (num >= 0)
            {
                obsObjectWork.pos.y = wall_work.target_pos.y;
                return 0;
            }
        }
        else if (obsObjectWork.spd.y > 0 && num <= 0)
        {
            obsObjectWork.pos.y = wall_work.target_pos.y;
            return 0;
        }
        return num;
    }

    private static void gmGmkBoss3PillarWallMainWait(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkBoss3PillarWallMainActive(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_WALL_WORK wall_work = (GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        int a = gmGmkBoss3PillarWallCheckMoveEnd(wall_work);
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)g_gm_main_system.ply_work[0];
        if (obj_work.spd.y < 0 && MTM_MATH_ABS(obsObjectWork.pos.x - obj_work.pos.x) < 65536 && (obsObjectWork.pos.y <= obj_work.pos.y && MTM_MATH_ABS(a) < 262144))
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
        if (a != 0)
            return;
        obj_work.pos.Assign(wall_work.target_pos);
        gmGmkBoss3PillarWallChangeModeWait(obj_work);
        if (((int)obj_work.user_flag & 1) != 0)
        {
            gmGmkBoss3PillarEffectCreateWallHit(obj_work);
            obj_work.user_flag &= 4294967294U;
        }
        GMM_PAD_VIB_STOP();
        if (((int)obj_work.user_flag & 2) != 0)
        {
            GmCameraVibrationSet(0, 12288, 0);
            obj_work.user_flag &= 4294967293U;
        }
        if (wall_work.se_handle == null)
            return;
        GmSoundStopSE(wall_work.se_handle);
        if (((int)obj_work.user_flag & 8) == 0)
            return;
        GmSoundPlaySE("Boss3_02", wall_work.se_handle);
        obj_work.user_flag &= 4294967287U;
    }

    private static void gmGmkBoss3PillarWallDrawBack(
      float left,
      float top,
      float right,
      float bottom,
      float z)
    {
        AMS_PARAM_DRAW_PRIMITIVE setParam = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.aTest = 0;
        setParam.zMask = 0;
        setParam.zTest = 1;
        setParam.ablend = 0;
        setParam.noSort = 1;
        NNS_PRIM3D_PC[] nnsPriM3DPcArray = amDrawAlloc_NNS_PRIM3D_PC(6);
        amVectorSet(nnsPriM3DPcArray[0], left, top, z);
        amVectorSet(nnsPriM3DPcArray[1], right, top, z);
        amVectorSet(nnsPriM3DPcArray[2], left, bottom, z);
        amVectorSet(nnsPriM3DPcArray[5], right, bottom, z);
        uint num = AMD_RGBA8888(0, 0, 0, byte.MaxValue);
        nnsPriM3DPcArray[0].Col = num;
        nnsPriM3DPcArray[1].Col = num;
        nnsPriM3DPcArray[2].Col = num;
        nnsPriM3DPcArray[5].Col = num;
        nnsPriM3DPcArray[3] = nnsPriM3DPcArray[1];
        nnsPriM3DPcArray[4] = nnsPriM3DPcArray[2];
        setParam.format3D = 2;
        setParam.type = 0;
        setParam.vtxPC3D = nnsPriM3DPcArray;
        setParam.texlist = null;
        setParam.texId = 0;
        setParam.count = 6;
        setParam.sortZ = -1f;
        gmGmkBoss3PillarWallMatrixPush(0U);
        amDrawPrimitive3D(0U, setParam);
        gmGmkBoss3PillarWallMatrixPop(0U);
        GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    private static void gmGmkBoss3PillarWallMatrixPush(uint command_state)
    {
        ObjDraw3DNNUserFunc(new OBF_DRAW_USER_DT_FUNC(gmGmkBoss3PillarWallUserFuncMatrixPush), null, 0, command_state);
    }

    private static void gmGmkBoss3PillarWallMatrixPop(uint command_state)
    {
        ObjDraw3DNNUserFunc(new OBF_DRAW_USER_DT_FUNC(gmGmkBoss3PillarWallUserFuncPop), null, 0, command_state);
    }

    private static void gmGmkBoss3PillarWallUserFuncMatrixPush(object param)
    {
        UNREFERENCED_PARAMETER(param);
        amMatrixPush();
        NNS_MATRIX current = amMatrixGetCurrent();
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        nnMultiplyMatrix(nnsMatrix, amDrawGetWorldViewMatrix(), current);
        nnSetPrimitive3DMatrix(nnsMatrix);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmGmkBoss3PillarWallUserFuncPop(object param)
    {
        amMatrixPop();
    }

    private static void gmGmkBoss3PillarEffectCreatePillarAppear(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        if (s3PillarMainWork.effect_work != null)
            return;
        int x = s3PillarMainWork.default_pos.x;
        int y = s3PillarMainWork.default_pos.y;
        int num1 = 0;
        uint num2 = 0;
        uint num3 = gmGmkBoss3PillarCalcPillarType(s3PillarMainWork.gimmick_work.ene_com.eve_rec.id);
        int zone_no = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork;
        if (zone_no == 2)
        {
            switch (num3)
            {
                case 0:
                    x += 188416;
                    num1 = 14;
                    num2 = 1U;
                    break;
                case 1:
                    x -= 188416;
                    num1 = 14;
                    break;
                case 2:
                    y += 188416;
                    num1 = 15;
                    break;
                case 3:
                    y -= 188416;
                    num1 = 13;
                    break;
            }
            gmsEffect3DesWork = GmEfctZoneEsCreate(null, zone_no, num1);
        }
        else
        {
            switch (num3)
            {
                case 0:
                    x += 229376;
                    num1 = 45;
                    num2 = 1U;
                    break;
                case 1:
                    x -= 229376;
                    num1 = 45;
                    break;
                case 2:
                    y += 229376;
                    num1 = 46;
                    break;
                case 3:
                    y -= 229376;
                    num1 = 44;
                    break;
            }
            gmsEffect3DesWork = GmEfctCmnEsCreate(null, num1);
        }
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)gmsEffect3DesWork;
        obsObjectWork.pos.x = x;
        obsObjectWork.pos.y = y;
        obsObjectWork.pos.z = -524288;
        obsObjectWork.disp_flag |= num2;
        s3PillarMainWork.effect_work = gmsEffect3DesWork;
    }

    private static void gmGmkBoss3PillarEffectCreatePillarHit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        int x = s3PillarMainWork.target_pos.x;
        int y = s3PillarMainWork.target_pos.y;
        ushort num1 = 0;
        uint num2 = gmGmkBoss3PillarCalcPillarType(s3PillarMainWork.gimmick_work.ene_com.eve_rec.id);
        int zone_no = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork;
        if (zone_no == 2)
        {
            int efct_zone_idx = 16;
            switch (num2)
            {
                case 0:
                    x += 122880;
                    num1 = (ushort)NNM_DEGtoA16(-90f);
                    break;
                case 1:
                    x -= 122880;
                    num1 = (ushort)NNM_DEGtoA16(90f);
                    break;
                case 2:
                    y += 122880;
                    break;
                case 3:
                    y -= 122880;
                    num1 = (ushort)NNM_DEGtoA16(180f);
                    break;
            }
            gmsEffect3DesWork = GmEfctZoneEsCreate(null, zone_no, efct_zone_idx);
        }
        else
        {
            int efct_cmn_idx = 47;
            switch (num2)
            {
                case 0:
                    x += 163840;
                    num1 = (ushort)NNM_DEGtoA16(-90f);
                    break;
                case 1:
                    x -= 163840;
                    num1 = (ushort)NNM_DEGtoA16(90f);
                    break;
                case 2:
                    y += 163840;
                    break;
                case 3:
                    y -= 163840;
                    num1 = (ushort)NNM_DEGtoA16(180f);
                    break;
            }
            gmsEffect3DesWork = GmEfctCmnEsCreate(null, efct_cmn_idx);
        }
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)gmsEffect3DesWork;
        obsObjectWork.pos.x = x;
        obsObjectWork.pos.y = y;
        obsObjectWork.pos.z = 655360;
        obsObjectWork.dir.z = num1;
    }

    private static void gmGmkBoss3PillarEffectCreateWallAppear(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        if (s3PillarWallWork.effect_work != null)
            return;
        int x = s3PillarWallWork.default_pos.x;
        int y = s3PillarWallWork.default_pos.y;
        int zone_no = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        int num;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork;
        if (zone_no == 2)
        {
            num = y - 196608;
            gmsEffect3DesWork = GmEfctZoneEsCreate(null, zone_no, 13);
        }
        else
        {
            num = y - 131072;
            gmsEffect3DesWork = GmEfctCmnEsCreate(null, 44);
        }
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)gmsEffect3DesWork;
        obsObjectWork.pos.x = x;
        obsObjectWork.pos.y = num;
        obsObjectWork.pos.z = 393216;
        s3PillarWallWork.effect_work = gmsEffect3DesWork;
    }

    private static void gmGmkBoss3PillarEffectCreateWallHit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        int x = s3PillarWallWork.target_pos.x;
        int y = s3PillarWallWork.target_pos.y;
        int zone_no = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        int num;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork;
        if (zone_no == 2)
        {
            num = y - 65536;
            gmsEffect3DesWork = GmEfctZoneEsCreate(null, zone_no, 16);
        }
        else
        {
            num = y;
            gmsEffect3DesWork = GmEfctCmnEsCreate(null, 47);
        }
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)gmsEffect3DesWork;
        obsObjectWork.pos.x = x;
        obsObjectWork.pos.y = num;
        obsObjectWork.pos.z = 655360;
        obsObjectWork.dir.z = (ushort)NNM_DEGtoA16(180f);
    }

}