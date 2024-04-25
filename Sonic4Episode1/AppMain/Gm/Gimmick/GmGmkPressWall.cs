public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkPressWallInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        GMS_GMK_PWALL_WORK work = (GMS_GMK_PWALL_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_PWALL_WORK(), "Gmk_PressWall");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] == 2)
        {
            ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_presswall_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWall_ppOut);
            if (eve_rec.height == 0)
            {
                work.wall_height = 0;
                obj_work.pos.y -= 524288;
            }
            else
            {
                work.wall_height = eve_rec.height * 64 * 4096;
                obj_work.pos.y -= work.wall_height;
            }
            obj_work.pos.z = 913408;
        }
        else
        {
            ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_presswall_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWall_ppOut);
            ObjAction3dNNMaterialMotionLoad(obj_work.obj_3d, 0, null, null, 1, (AMS_AMB_HEADER)ObjDataGet(895).pData);
            ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
            obj_work.disp_flag |= 4U;
            if (eve_rec.height == 0)
            {
                work.wall_height = 0;
                obj_work.pos.y -= 786432;
            }
            else
            {
                work.wall_height = eve_rec.height * 192 * 4096;
                obj_work.pos.y -= work.wall_height;
            }
            obj_work.pos.z = 1044480;
        }
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        obj_work.flag |= 2U;
        work.wall_speed = eve_rec.width == 0 ? 4096 : eve_rec.width * 4096 / 10;
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkPressWallExit));
        work.se_handle = null;
        gmGmkPressWallStart(obj_work);
        return obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkPressWallStopInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] == 2 && pwall == null)
            GmGmkPressWallInit(eve_rec, pos_x, pos_y, type).user_flag = 1U;
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "Gmk_PressWallStopper");
        work.user_flag = 0U;
        if ((eve_rec.flag & 1) != 0)
            work.user_flag = 1U;
        gmGmkPressWallStopperStart(work);
        return work;
    }

    private static OBS_OBJECT_WORK GmGmkPressWallControlerInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_PWALLCTRL_WORK(), "Gmk_PressWallControler");
        GMS_GMK_PWALLCTRL_WORK gmkPwallctrlWork = (GMS_GMK_PWALLCTRL_WORK)work;
        OBS_RECT_WORK pRec = ((GMS_ENEMY_3D_WORK)work).ene_com.rect_work[2];
        pRec.ppDef = null;
        pRec.ppHit = null;
        pRec.flag &= 4294967291U;
        if (eve_rec.left != 0)
        {
            ObjRectWorkSet(pRec, (short)(eve_rec.left * 2), 0, (short)(eve_rec.width * 2), 1);
            work.user_flag = 0U;
            work.user_timer = eve_rec.height * 819;
        }
        else
        {
            pRec.ppDef = null;
            pRec.ppHit = null;
            ObjRectWorkSet(pRec, 0, (short)(eve_rec.top * 2), 1, (short)(eve_rec.height * 2));
            gmkPwallctrlWork.line_top = eve_rec.top * 2 * 4096 + work.pos.y;
            gmkPwallctrlWork.line_bottom = eve_rec.height * 2 * 4096 + work.pos.y;
            work.user_flag = 1U;
            work.user_timer = eve_rec.width * 819;
        }
        work.flag &= 4294967293U;
        if ((eve_rec.flag & 1) != 0)
            work.user_flag |= 2U;
        if ((eve_rec.flag & 2) != 0)
            work.user_flag |= 4U;
        gmGmkPressWallControlerStart(work);
        return work;
    }

    public static void GmGmkPressWallBuild()
    {
        gm_gmk_presswall_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(892), GmGameDatGetGimmickData(893), 0U);
        pwall = null;
    }

    public static void GmGmkPressWallFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(892);
        GmGameDBuildRegFlushModel(gm_gmk_presswall_obj_3d_list, gimmickData.file_num);
    }

    private static void gmGmkPressWall_ppOut(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALL_WORK gmsGmkPwallWork = (GMS_GMK_PWALL_WORK)obj_work;
        obj_work.pos.y = gmsGmkPwallWork.master_posy;
        obj_work.pos.y += obj_work.user_timer;
        if (gmsGmkPwallWork.wall_height == 0)
        {
            while (obj_work.pos.y + 786432 < g_obj.camera[0][1])
                obj_work.pos.y += 786432;
            while (obj_work.pos.y > g_obj.camera[0][1])
                obj_work.pos.y -= 786432;
            for (int index = obj_work.pos.y - g_obj.camera[0][1]; index < 1048576; index += 786432)
            {
                ObjDrawActionSummary(obj_work);
                obj_work.pos.y += 786432;
            }
        }
        else
        {
            for (int index = 0; index < gmsGmkPwallWork.wall_height; index += 786432)
            {
                ObjDrawActionSummary(obj_work);
                obj_work.pos.y += 786432;
            }
        }
        obj_work.pos.y = g_obj.camera[0][1];
    }

    private static void gmGmkPressWallStay(OBS_OBJECT_WORK obj_work)
    {
        if (g_obj.camera[0][0] < obj_work.pos.x && (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] != 2 || obj_work.user_flag == 0U))
            return;
        GMS_GMK_PWALL_WORK gmsGmkPwallWork = (GMS_GMK_PWALL_WORK)obj_work;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.diff_data = g_gm_default_col;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.width = 192;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.ofst_x = -192;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.height = 256;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.ofst_y = 0;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.flag |= 134217824U;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.attr &= 65534;
        obj_work.disp_flag &= 4294967263U;
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] == 2 && gmsGmkPwallWork.wall_height > 0)
            gmGmkPressWallCreateRail(obj_work, gmsGmkPwallWork.wall_height, gmsGmkPwallWork.master_posy);
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] == 3)
        {
            gmGmkPressWallCreateParts(obj_work, gmsGmkPwallWork.master_posy, gmsGmkPwallWork.wall_height);
            gmsGmkPwallWork.gmk_work.ene_com.rect_work[2].flag &= 4294967291U;
            gmsGmkPwallWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
            gmsGmkPwallWork.gmk_work.ene_com.rect_work[1].flag |= 4U;
            OBS_RECT_WORK pRec = gmsGmkPwallWork.gmk_work.ene_com.rect_work[1];
            ObjRectWorkZSet(pRec, -16, 0, -32, 0, 192, 32);
            pRec.flag |= 4U;
            pRec.flag |= 1024U;
            obj_work.flag &= 4294967293U;
            pRec.ppHit = new OBS_RECT_WORK_Delegate1(gmGmkPressWallZ4Hit);
        }
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] == 2 && obj_work.user_flag != 0U)
        {
            obj_work.user_flag_OBJECT = null;
        }
        else
        {
            GMM_PAD_VIB_MID_TIME(60f);
            gmsGmkPwallWork.se_handle = GsSoundAllocSeHandle();
            GmSoundPlaySEForce("MovingWall", gmsGmkPwallWork.se_handle);
        }
        gmsGmkPwallWork.efct_obj = null;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallForce);
        gmGmkPressWallForce(obj_work);
    }

    private static void gmGmkPressWallForce(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALL_WORK gmsGmkPwallWork = (GMS_GMK_PWALL_WORK)obj_work;
        if (((int)g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
        {
            gmsGmkPwallWork.ply_death = true;
            gmsGmkPwallWork.wall_speed = 0;
            gmGmkPressWallSeStop(obj_work);
        }
        if (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id] == 2)
        {
            if (gmsGmkPwallWork.wall_speed == 0 || gmsGmkPwallWork.ply_death)
            {
                gmsGmkPwallWork.wall_vibration &= 3;
                gmsGmkPwallWork.wall_vibration += 3;
                gmsGmkPwallWork.wall_brake = gmsGmkPwallWork.wall_speed;
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallForce_100);
                gmGmkPressWallForce_100(obj_work);
                return;
            }
            obj_work.pos.x += gmsGmkPwallWork.wall_speed;
            obj_work.user_timer = wall_vib[gmsGmkPwallWork.wall_vibration & 7];
            ++gmsGmkPwallWork.wall_vibration;
            if (gmsGmkPwallWork.wall_effect_build_timer == 0)
            {
                OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctZoneEsCreate(null, 2, 32);
                obsObjectWork.pos.x = obj_work.pos.x;
                obsObjectWork.pos.y = g_obj.camera[0][1];
                obsObjectWork.pos.z = obj_work.pos.z;
                obsObjectWork.spd.x = gmsGmkPwallWork.wall_speed;
                gmsGmkPwallWork.wall_effect_build_timer = (short)((mtMathRand() & 63) + 90);
            }
            --gmsGmkPwallWork.wall_effect_build_timer;
            obj_work.pos.y = g_obj.camera[0][1];
        }
        else
        {
            obj_work.pos.x += gmsGmkPwallWork.wall_speed;
            if (gmsGmkPwallWork.ply_death || gmsGmkPwallWork.wall_speed == 0)
            {
                if (gmsGmkPwallWork.efct_obj != null)
                {
                    ObjDrawKillAction3DES(gmsGmkPwallWork.efct_obj);
                    gmsGmkPwallWork.efct_obj = null;
                    gmsGmkPwallWork.wall_effect_build_timer = 0;
                }
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallForceZ4_Stop);
            }
            else if (((int)obj_work.user_flag & 1) != 0 || gmsGmkPwallWork.ply_death)
            {
                gmsGmkPwallWork.wall_brake = gmsGmkPwallWork.wall_speed;
                obj_work.user_flag &= 4294967294U;
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallForceZ4_Hit);
                if (gmsGmkPwallWork.efct_obj != null)
                {
                    ObjDrawKillAction3DES(gmsGmkPwallWork.efct_obj);
                    gmsGmkPwallWork.efct_obj = null;
                    gmsGmkPwallWork.wall_effect_build_timer = 0;
                }
            }
            else
                --gmsGmkPwallWork.wall_effect_build_timer;
        }
        if (gmsGmkPwallWork == pwall)
            return;
        obj_work.flag |= 8U;
        gmsGmkPwallWork.gmk_work.ene_com.enemy_flag |= 65536U;
        gmGmkPressWallSeStop(obj_work);
    }

    private static void gmGmkPressWallForce_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALL_WORK gmsGmkPwallWork = (GMS_GMK_PWALL_WORK)obj_work;
        gmsGmkPwallWork.wall_brake -= gmsGmkPwallWork.wall_speed / 20;
        obj_work.pos.x += gmsGmkPwallWork.wall_brake;
        if (gmsGmkPwallWork.wall_vibration < 20)
        {
            obj_work.user_timer = wall_vib[gmsGmkPwallWork.wall_vibration];
            ++gmsGmkPwallWork.wall_vibration;
        }
        else
        {
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallForce_200);
            gmGmkPressWallSeStop(obj_work);
        }
    }

    private static void gmGmkPressWallForce_200(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALL_WORK gmsGmkPwallWork = (GMS_GMK_PWALL_WORK)obj_work;
        if (gmsGmkPwallWork == pwall)
            return;
        obj_work.flag |= 8U;
        gmsGmkPwallWork.gmk_work.ene_com.enemy_flag |= 65536U;
    }

    private static void gmGmkPressWallForceZ4_Hit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALL_WORK gmsGmkPwallWork = (GMS_GMK_PWALL_WORK)obj_work;
        if (((int)g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
        {
            gmsGmkPwallWork.ply_death = true;
            gmsGmkPwallWork.wall_speed = 0;
            gmGmkPressWallSeStop(obj_work);
        }
        gmsGmkPwallWork.wall_brake -= gmsGmkPwallWork.wall_speed / 64;
        if (gmsGmkPwallWork.wall_brake <= 0 || gmsGmkPwallWork.wall_speed == 0)
        {
            gmsGmkPwallWork.wall_brake = 0;
            obj_work.ppFunc = gmsGmkPwallWork.ply_death || gmsGmkPwallWork.wall_speed == 0 ? new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallForceZ4_Stop) : new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallForceZ4_Hit_100);
        }
        if (gmsGmkPwallWork.gmk_work.obj_3d.speed[0] > 0.0)
        {
            gmsGmkPwallWork.gmk_work.obj_3d.speed[0] -= 1f / 64f;
            gmsGmkPwallWork.gmk_work.obj_3d.speed[1] -= 1f / 64f;
            ++gmsGmkPwallWork.mat_timer;
            if (gmsGmkPwallWork.mat_timer > gmsGmkPwallWork.mat_timer_line)
            {
                obj_work.disp_flag &= 4294963199U;
                gmsGmkPwallWork.mat_timer_line = gmsGmkPwallWork.mat_timer;
                gmsGmkPwallWork.mat_timer = 0U;
            }
            else
                obj_work.disp_flag |= 4096U;
        }
        obj_work.pos.x += gmsGmkPwallWork.wall_brake;
    }

    private static void gmGmkPressWallForceZ4_Hit_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALL_WORK gmsGmkPwallWork = (GMS_GMK_PWALL_WORK)obj_work;
        if (((int)g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
        {
            gmsGmkPwallWork.ply_death = true;
            gmsGmkPwallWork.wall_speed = 0;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallForceZ4_Stop);
        }
        else
        {
            gmsGmkPwallWork.wall_brake += gmsGmkPwallWork.wall_speed / 64;
            if (gmsGmkPwallWork.wall_brake >= gmsGmkPwallWork.wall_speed)
            {
                gmsGmkPwallWork.wall_brake = gmsGmkPwallWork.wall_speed;
                gmsGmkPwallWork.gmk_work.obj_3d.speed[0] = 1f;
                gmsGmkPwallWork.gmk_work.obj_3d.speed[1] = 1f;
                gmsGmkPwallWork.mat_timer = 0U;
                gmsGmkPwallWork.mat_timer_line = 0U;
                obj_work.disp_flag &= 4294963199U;
                obj_work.flag &= 4294967293U;
                gmsGmkPwallWork.ply_death = false;
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallForce);
            }
            if (gmsGmkPwallWork.gmk_work.obj_3d.speed[0] < 1.0)
            {
                gmsGmkPwallWork.gmk_work.obj_3d.speed[0] += 1f / 64f;
                gmsGmkPwallWork.gmk_work.obj_3d.speed[1] += 1f / 64f;
                ++gmsGmkPwallWork.mat_timer;
                if (gmsGmkPwallWork.mat_timer > gmsGmkPwallWork.mat_timer_line)
                {
                    obj_work.disp_flag |= 4096U;
                    gmsGmkPwallWork.mat_timer_line = gmsGmkPwallWork.mat_timer;
                    gmsGmkPwallWork.mat_timer = 0U;
                }
                else
                    obj_work.disp_flag &= 4294963199U;
            }
            obj_work.pos.x += gmsGmkPwallWork.wall_brake;
        }
    }

    private static void gmGmkPressWallForceZ4_Stop(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALL_WORK gmsGmkPwallWork = (GMS_GMK_PWALL_WORK)obj_work;
        if (gmsGmkPwallWork.gmk_work.obj_3d.speed[0] <= 0.0)
            return;
        gmsGmkPwallWork.gmk_work.obj_3d.speed[0] -= 1f / 64f;
        gmsGmkPwallWork.gmk_work.obj_3d.speed[1] -= 1f / 64f;
        ++gmsGmkPwallWork.mat_timer;
        if (gmsGmkPwallWork.mat_timer > gmsGmkPwallWork.mat_timer_line)
        {
            obj_work.disp_flag &= 4294963199U;
            gmsGmkPwallWork.mat_timer_line = gmsGmkPwallWork.mat_timer;
            gmsGmkPwallWork.mat_timer = 0U;
        }
        else
            obj_work.disp_flag |= 4096U;
        if (gmsGmkPwallWork.gmk_work.obj_3d.speed[0] > 0.0)
            return;
        gmsGmkPwallWork.gmk_work.obj_3d.speed[0] = 0.0f;
        gmsGmkPwallWork.gmk_work.obj_3d.speed[1] = 0.0f;
        obj_work.disp_flag |= 4096U;
        gmGmkPressWallSeStop(obj_work);
    }

    private static void gmGmkPressWallZ4Hit(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        OBS_OBJECT_WORK parentObj = mine_rect.parent_obj;
        parentObj.flag |= 2U;
        parentObj.user_flag |= 1U;
        GmEnemyDefaultAtkFunc(mine_rect, match_rect);
    }

    private static void gmGmkPressWallSeStop(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALL_WORK gmsGmkPwallWork = (GMS_GMK_PWALL_WORK)obj_work;
        if (gmsGmkPwallWork.se_handle == null)
            return;
        GsSoundStopSeHandle(gmsGmkPwallWork.se_handle);
        GsSoundFreeSeHandle(gmsGmkPwallWork.se_handle);
        gmsGmkPwallWork.se_handle = null;
    }

    private static void gmGmkPressWallStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALL_WORK gmsGmkPwallWork = (GMS_GMK_PWALL_WORK)obj_work;
        pwall = gmsGmkPwallWork;
        obj_work.flag |= 16U;
        gmsGmkPwallWork.wall_vibration = 0;
        gmsGmkPwallWork.wall_effect_build_timer = 0;
        gmsGmkPwallWork.master_posy = obj_work.pos.y;
        pwall.stop_wall = false;
        obj_work.disp_flag |= 32U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallStay);
    }

    private static void gmGmkPressWallStopperMain(OBS_OBJECT_WORK obj_work)
    {
        if (pwall == null)
        {
            obj_work.flag |= 8U;
        }
        else
        {
            if (obj_work.user_flag != 0U && (obj_work.pos.y < g_obj.camera[0][1] - 524288 || obj_work.pos.y > g_obj.camera[0][1] + 1572864))
                return;
            if (pwall.gmk_work.ene_com.obj_work.pos.x > obj_work.pos.x)
            {
                GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
                pwall.gmk_work.ene_com.obj_work.pos.x = obj_work.pos.x;
                pwall.wall_speed = 0;
                obj_work.flag |= 8U;
                gmsEnemyComWork.enemy_flag |= 65536U;
            }
            if (obj_work.user_work_OBJECT == pwall)
                return;
            GMS_ENEMY_COM_WORK gmsEnemyComWork1 = (GMS_ENEMY_COM_WORK)obj_work;
            obj_work.flag |= 8U;
            gmsEnemyComWork1.enemy_flag |= 65536U;
        }
    }

    private static void gmGmkPressWallStopperStart(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_work_OBJECT = pwall;
        obj_work.disp_flag |= 32U;
        obj_work.move_flag |= 8960U;
        obj_work.flag |= 16U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallStopperMain);
    }

    private static void gmGmkPressWallControler(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALLCTRL_WORK gmkPwallctrlWork = (GMS_GMK_PWALLCTRL_WORK)obj_work;
        GMS_PLAYER_WORK plyWork = gmkPwallctrlWork.ply_work;
        if (pwall != null && ((int)obj_work.user_flag & 1) != 0 && (obj_work.pos.x > gmkPwallctrlWork.last_ply_x && obj_work.pos.x <= plyWork.obj_work.pos.x) && (((int)obj_work.user_flag & 2) == 0 || plyWork.obj_work.pos.y >= gmkPwallctrlWork.line_top && plyWork.obj_work.pos.y <= gmkPwallctrlWork.line_bottom))
        {
            if (((int)obj_work.user_flag & 4) != 0 && pwall.gmk_work.ene_com.obj_work.pos.x <= g_obj.camera[0][0] - 32768)
                pwall.gmk_work.ene_com.obj_work.pos.x = g_obj.camera[0][0] - 32768;
            pwall.wall_speed = obj_work.user_timer;
            if (pwall.wall_speed == 0)
                pwall.stop_wall = true;
            obj_work.flag |= 8U;
            gmkPwallctrlWork.gmk_work.ene_com.enemy_flag |= 65536U;
        }
        else
        {
            gmkPwallctrlWork.last_ply_x = plyWork.obj_work.pos.x;
            gmkPwallctrlWork.last_ply_y = plyWork.obj_work.pos.y;
        }
    }

    private static void gmGmkPressWallControlerStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PWALLCTRL_WORK gmkPwallctrlWork = (GMS_GMK_PWALLCTRL_WORK)obj_work;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        gmkPwallctrlWork.ply_work = gmsPlayerWork;
        gmkPwallctrlWork.last_ply_x = gmsPlayerWork.obj_work.pos.x;
        gmkPwallctrlWork.last_ply_y = gmsPlayerWork.obj_work.pos.y;
        obj_work.disp_flag |= 32U;
        obj_work.move_flag |= 8960U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallControler);
    }

    private static void gmGmkPressWallParts(OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x = obj_work.parent_obj.pos.x;
    }

    private static void gmGmkPressWallRail(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PRESSWALL_PARTS gmkPresswallParts = (GMS_GMK_PRESSWALL_PARTS)obj_work;
        GMS_GMK_PWALL_WORK parentObj = (GMS_GMK_PWALL_WORK)obj_work.parent_obj;
        obj_work.pos.x = obj_work.parent_obj.pos.x;
        obj_work.pos.y = parentObj.master_posy;
        obj_work.pos.y += obj_work.parent_obj.user_timer;
        obj_work.pos.y += gmkPresswallParts.ofst_y;
    }

    private static void gmGmkPressWallCreateRail(
      OBS_OBJECT_WORK parent_obj,
      int height,
      int pos_y)
    {
        OBS_OBJECT_WORK work1 = GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_PRESSWALL_PARTS(), parent_obj, 0, "PresswallRail-Top");
        GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork1 = (GMS_EFFECT_3DNN_WORK)work1;
        ObjObjectCopyAction3dNNModel(work1, gm_gmk_presswall_obj_3d_list[2], gmsEffect3DnnWork1.obj_3d);
        work1.flag &= 4294966271U;
        work1.pos.y = pos_y;
        work1.pos.z = parent_obj.pos.z + 4096;
        work1.disp_flag |= 4194304U;
        work1.disp_flag |= 256U;
        work1.disp_flag |= 134217728U;
        ((GMS_GMK_PRESSWALL_PARTS)work1).ofst_y = -8192;
        work1.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallRail);
        OBS_OBJECT_WORK work2 = GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_PRESSWALL_PARTS(), parent_obj, 0, "PresswallRail-Botom");
        GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork2 = (GMS_EFFECT_3DNN_WORK)work2;
        ObjObjectCopyAction3dNNModel(work2, gm_gmk_presswall_obj_3d_list[1], gmsEffect3DnnWork2.obj_3d);
        work2.flag &= 4294966271U;
        work2.pos.y = pos_y + height;
        work2.pos.z = parent_obj.pos.z + 4096;
        work2.disp_flag |= 4194304U;
        work2.disp_flag |= 256U;
        work2.disp_flag |= 134217728U;
        ((GMS_GMK_PRESSWALL_PARTS)work2).ofst_y = height - 65536;
        work2.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallRail);
    }

    private static void gmGmkPressWallZ4Parts_ppOut(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PRESSWALL_PARTS gmkPresswallParts = (GMS_GMK_PRESSWALL_PARTS)obj_work;
        obj_work.pos.y = gmkPresswallParts.master_posy;
        while (obj_work.pos.y + 786432 < g_obj.camera[0][1])
            obj_work.pos.y += 786432;
        while (obj_work.pos.y > g_obj.camera[0][1])
            obj_work.pos.y -= 786432;
        for (int index = obj_work.pos.y - g_obj.camera[0][1]; index < 1048576; index += 786432)
        {
            ObjDrawActionSummary(obj_work);
            obj_work.pos.y += 786432;
        }
    }

    private static void gmGmkPressWallCreateParts(
      OBS_OBJECT_WORK parent_obj,
      int pos_y,
      int height)
    {
        OBS_OBJECT_WORK obj_work = null;
        for (int index = 0; index < 3; ++index)
        {
            obj_work = GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_PRESSWALL_PARTS(), parent_obj, 0, "PresswallZ4Parts");
            GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (GMS_EFFECT_3DNN_WORK)obj_work;
            ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_presswall_obj_3d_list[(int)tbl_gmk_z4PressWall_model[index]], gmsEffect3DnnWork.obj_3d);
            obj_work.flag &= 4294966271U;
            obj_work.pos.y = pos_y;
            obj_work.pos.z = parent_obj.pos.z + tbl_gmk_z4PressWall_ofst_z[index];
            obj_work.disp_flag |= 4194304U;
            obj_work.disp_flag |= 256U;
            obj_work.disp_flag |= 134217728U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallParts);
            ((GMS_GMK_PRESSWALL_PARTS)obj_work).master_posy = pos_y;
            if (height == 0)
                obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPressWallZ4Parts_ppOut);
        }
        ObjAction3dNNMaterialMotionLoad(obj_work.obj_3d, 0, null, null, 0, (AMS_AMB_HEADER)ObjDataGet(895).pData);
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.obj_3d.mat_speed = 1f;
        obj_work.disp_flag |= 4U;
    }

    private static void gmGmkPressWallExit(MTS_TASK_TCB tcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        if (pwall == (GMS_GMK_PWALL_WORK)tcbWork)
            pwall = null;
        gmGmkPressWallSeStop(tcbWork);
        GmEnemyDefaultExit(tcb);
    }
}