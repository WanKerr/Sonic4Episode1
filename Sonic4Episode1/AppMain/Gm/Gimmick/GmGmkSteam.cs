public partial class AppMain
{
    private static void gmGmkSteamGateHit(
     OBS_RECT_WORK mine_rect,
     OBS_RECT_WORK match_rect)
    {
        OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 == g_gm_main_system.ply_work[0] && ((int)parentObj2.player_flag & 1024) == 0)
        {
            GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (GMS_GMK_STEAMP_WORK)parentObj1;
            parentObj2.obj_work.pos.y = parentObj1.pos.y;
            GmPlySeqInitSteamPipeIn(parentObj2);
            gmsGmkSteampWork.status = 1;
            gmsGmkSteampWork.ply_work = parentObj2;
            parentObj1.flag |= 2U;
            GMM_PAD_VIB_SMALL_TIME(60f);
        }
        mine_rect.flag &= 4294573823U;
    }

    private static void gmGmkSteamExitHit(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 != g_gm_main_system.ply_work[0] || parentObj2.seq_state != GME_PLY_SEQ_STATE_GMK_STEAMPIPE || ((int)parentObj2.player_flag & 1024) != 0)
            return;
        GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (GMS_GMK_STEAMP_WORK)parentObj1;
        parentObj2.obj_work.spd.x = parentObj2.obj_work.spd.y = 0;
        gmsGmkSteampWork.status = 1;
        gmsGmkSteampWork.ply_work = parentObj2;
        parentObj1.flag |= 2U;
    }

    private static void gmGmkSteamCrankCheck(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (gmsPlayerWork.obj_work.pos.x < obj_work.pos.x + 65536 && gmsPlayerWork.obj_work.pos.x > obj_work.pos.x - 65536 && (gmsPlayerWork.obj_work.pos.y < obj_work.pos.y + 65536 && gmsPlayerWork.obj_work.pos.y > obj_work.pos.y - 65536))
        {
            if (((int)gmsPlayerWork.player_flag & 1024) != 0 || gmsPlayerWork.seq_state != GME_PLY_SEQ_STATE_GMK_STEAMPIPE || obj_work.user_flag != 0U)
                return;
            GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (GMS_GMK_STEAMP_WORK)obj_work;
            if (gmsPlayerWork.obj_work.spd.x != 0)
            {
                int x = gmsPlayerWork.obj_work.spd.x;
                int num = gmsPlayerWork.obj_work.pos.x - x;
                if ((x <= 0 || (tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 2) == 0 || (num > obj_work.pos.x || obj_work.pos.x > gmsPlayerWork.obj_work.pos.x)) && (x >= 0 || (tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 1) == 0 || (gmsPlayerWork.obj_work.pos.x > obj_work.pos.x || obj_work.pos.x > num)))
                    return;
                int a = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
                gmsPlayerWork.obj_work.pos.x = obj_work.pos.x;
                gmsPlayerWork.obj_work.spd.x = 0;
                gmsPlayerWork.obj_work.pos.y = obj_work.pos.y;
                if ((tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 8) == 0)
                {
                    gmsPlayerWork.obj_work.spd.y = -61440;
                    gmsPlayerWork.obj_work.pos.y -= MTM_MATH_ABS(a);
                }
                else
                {
                    gmsPlayerWork.obj_work.spd.y = 61440;
                    gmsPlayerWork.obj_work.pos.y += MTM_MATH_ABS(a);
                }
                obj_work.flag |= 2U;
                GmSoundPlaySE("PipeMoving");
                obj_work.user_flag = 1U;
            }
            else
            {
                int y = gmsPlayerWork.obj_work.spd.y;
                int num = gmsPlayerWork.obj_work.pos.y - y;
                if ((y <= 0 || (tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 4) == 0 || (num > obj_work.pos.y || obj_work.pos.y > gmsPlayerWork.obj_work.pos.y)) && (y >= 0 || (tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 8) == 0 || (gmsPlayerWork.obj_work.pos.y > obj_work.pos.y || obj_work.pos.y > num)))
                    return;
                int a = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
                gmsPlayerWork.obj_work.pos.y = obj_work.pos.y;
                gmsPlayerWork.obj_work.spd.y = 0;
                gmsPlayerWork.obj_work.pos.x = obj_work.pos.x;
                if ((tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 2) == 0)
                {
                    gmsPlayerWork.obj_work.spd.x = 61440;
                    gmsPlayerWork.obj_work.pos.x += MTM_MATH_ABS(a);
                }
                else
                {
                    gmsPlayerWork.obj_work.spd.x = -61440;
                    gmsPlayerWork.obj_work.pos.x -= MTM_MATH_ABS(a);
                }
                obj_work.flag |= 2U;
                GmSoundPlaySE("PipeMoving");
                obj_work.user_flag = 1U;
            }
        }
        else
            obj_work.user_flag = 0U;
    }

    private static void gmGmkSteamPipeStay(OBS_OBJECT_WORK obj_work)
    {
        switch (((GMS_GMK_STEAMP_WORK)obj_work).obj_type)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                gmGmkSteamCrankCheck(obj_work);
                break;
            case 4:
            case 5:
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSteamPipeStay_100);
                break;
            case 6:
            case 7:
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSteamPipeStay_Exit);
                break;
            default:
                obj_work.ppFunc = null;
                break;
        }
    }

    private static void gmGmkSteamPipeStay_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (GMS_GMK_STEAMP_WORK)obj_work;
        if (gmsGmkSteampWork.status == 0)
            return;
        switch (gmsGmkSteampWork.obj_type)
        {
            case 4:
            case 5:
                gmsGmkSteampWork.timer = 60;
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSteamPipe_GateIn);
                GmSoundPlaySE("PipeIn");
                break;
            case 6:
            case 7:
                gmsGmkSteampWork.timer = 0;
                gmGmkSteamPipe_GateOutColClear(obj_work);
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSteamPipe_GateOut);
                break;
        }
    }

    private static void gmGmkSteamPipeStay_Exit(OBS_OBJECT_WORK obj_work)
    {
        gmGmkSteamPipeStay_100(obj_work);
    }

    private static void gmGmkSteamPipe_GateIn(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (GMS_GMK_STEAMP_WORK)obj_work;
        --gmsGmkSteampWork.timer;
        if (gmsGmkSteampWork.timer > 0)
            return;
        int spd_x = gmsGmkSteampWork.obj_type != 4 ? -61440 : 61440;
        gmsGmkSteampWork.ply_work.obj_work.move_flag |= 16U;
        GmPlySeqGmkSpdSet(gmsGmkSteampWork.ply_work, spd_x, 0);
        gmsGmkSteampWork.ply_work.gmk_flag2 |= 6U;
        GmSoundPlaySE("PipeMoving");
        obj_work.ppFunc = null;
    }

    private static void gmGmkSteamPipe_GateOut(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (GMS_GMK_STEAMP_WORK)obj_work;
        --gmsGmkSteampWork.timer;
        if (gmsGmkSteampWork.timer > 0)
            return;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(obj_work, 92);
        obsObjectWork.pos.x = obj_work.pos.x;
        obsObjectWork.pos.y = obj_work.pos.y;
        int spd_x;
        if (((int)obj_work.user_flag & 1) == 0)
        {
            spd_x = 61440;
            obsObjectWork.dir.z = 16384;
            obsObjectWork.pos.x += 229376;
        }
        else
        {
            spd_x = -61440;
            obsObjectWork.dir.z = 49152;
            obsObjectWork.pos.x -= 229376;
        }
        GmPlySeqInitSteamPipeOut(gmsGmkSteampWork.ply_work, spd_x);
        GmSoundPlaySE("PipeOut");
        gmsGmkSteampWork.timer = 8;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSteamPipe_GateOut_100);
    }

    private static void gmGmkSteamPipe_GateOut_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (GMS_GMK_STEAMP_WORK)obj_work;
        --gmsGmkSteampWork.timer;
        if (gmsGmkSteampWork.timer > 0)
            return;
        gmGmkSteamPipe_GateOutColSet(obj_work);
        gmsGmkSteampWork.status = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSteamPipeStay_Exit);
    }

    private static void gmGmkSteamPipe_GateOutColSet(OBS_OBJECT_WORK obj_work)
    {
        if ((((GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 2) != 0)
            return;
        GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (GMS_GMK_STEAMP_WORK)obj_work;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.width = 64;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.height = 64;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.ofst_x = 0;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.ofst_y = -32;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.dir = 0;
    }

    private static void gmGmkSteamPipe_GateOutColClear(OBS_OBJECT_WORK obj_work)
    {
        ((GMS_GMK_STEAMP_WORK)obj_work).gmk_work.ene_com.col_work.obj_col.obj = null;
    }

    private static void gmGmkSteamPipeStart(OBS_OBJECT_WORK obj_work, int type)
    {
        GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (GMS_GMK_STEAMP_WORK)obj_work;
        if (type >= 4 && type < 8)
        {
            gmsGmkSteampWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
            gmsGmkSteampWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
            OBS_RECT_WORK pRec = gmsGmkSteampWork.gmk_work.ene_com.rect_work[2];
            pRec.ppHit = null;
            if (type < 6)
            {
                pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkSteamGateHit);
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.width = 32;
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.height = 16;
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.ofst_x = -14;
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.ofst_y = -34;
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.dir = 0;
            }
            else
                pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkSteamExitHit);
            ObjRectAtkSet(pRec, 0, 0);
            ObjRectDefSet(pRec, 65534, 0);
            ObjRectWorkSet(pRec, tbl_gm_gmk_steampipe_rect[type][0], tbl_gm_gmk_steampipe_rect[type][1], tbl_gm_gmk_steampipe_rect[type][2], tbl_gm_gmk_steampipe_rect[type][3]);
            pRec.flag |= 4U;
            obj_work.flag &= 4294967293U;
        }
        gmsGmkSteampWork.obj_type = type;
        gmsGmkSteampWork.status = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSteamPipeStay);
    }

    public static OBS_OBJECT_WORK gmGmkSteamPipeInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      ushort model)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_STEAMP_WORK(), "Gmk_SteamPipe");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_steampipe_obj_3d_list[model], gmsEnemy3DWork.obj_3d);
        work.pos.z = eve_rec.left * 8 * 4096;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 2U;
        return work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeGateRInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, 1);
        obj_work.disp_flag |= 4194304U;
        obj_work.pos.z += 65536;
        gmGmkSteamPipeStart(obj_work, 4);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeGateLInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, 0);
        obj_work.pos.z += 65536;
        gmGmkSteamPipeStart(obj_work, 5);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeGateEInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model;
        int type1;
        if ((eve_rec.flag & 1) == 0)
        {
            model = 2;
            type1 = 6;
        }
        else
        {
            model = 3;
            type1 = 7;
        }
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        obj_work.pos.z += 131072;
        obj_work.user_flag = eve_rec.flag;
        if (((int)obj_work.user_flag & 1) == 0)
            obj_work.pos.x -= 131072;
        else
            obj_work.pos.x += 131072;
        gmGmkSteamPipe_GateOutColSet(obj_work);
        gmGmkSteamPipeStart(obj_work, type1);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeA1Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 4;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = 4;
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        gmGmkSteamPipeStart(obj_work, 9);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeA2Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 5;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = 5;
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        gmGmkSteamPipeStart(obj_work, 8);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeA3Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 4;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = 4;
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        gmGmkSteamPipeStart(obj_work, 9);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeA4Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 6;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = 5;
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        gmGmkSteamPipeStart(obj_work, 8);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeB1Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 7;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = 6;
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        gmGmkSteamPipeStart(obj_work, 9);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeB2Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 8;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = 7;
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        gmGmkSteamPipeStart(obj_work, 8);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeB3Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 7;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = 6;
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        gmGmkSteamPipeStart(obj_work, 9);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeB4Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 9;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = 7;
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        gmGmkSteamPipeStart(obj_work, 8);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeJ1Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 10;
        ushort num = 0;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
        {
            model = 9;
            num = 16384;
        }
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        obj_work.dir.z = num;
        gmGmkSteamPipeStart(obj_work, 0);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeJ2Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 11;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = 8;
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        gmGmkSteamPipeStart(obj_work, 1);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeJ3Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 12;
        ushort num = 0;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
        {
            model = 8;
            num = 16384;
        }
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        obj_work.dir.z = num;
        gmGmkSteamPipeStart(obj_work, 2);
        return obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkSteamPipeJ4Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 13;
        if (GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = 9;
        OBS_OBJECT_WORK obj_work = gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        gmGmkSteamPipeStart(obj_work, 3);
        return obj_work;
    }

    public static void GmGmkSteamPipeBuild()
    {
        gm_gmk_steampipe_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(921), GmGameDatGetGimmickData(922), 0U);
    }

    public static void GmGmkSteamPipeFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(921);
        GmGameDBuildRegFlushModel(gm_gmk_steampipe_obj_3d_list, gimmickData.file_num);
    }

    private static OBS_OBJECT_WORK GmGmkStartInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        if (((int)g_gs_main_sys_info.game_flag & 4) == 0)
        {
            g_gm_main_system.resume_pos_x = pos_x;
            g_gm_main_system.resume_pos_y = pos_y - 4096;
        }
        eve_rec.pos_x = byte.MaxValue;
        GmCameraPosSet(g_gm_main_system.resume_pos_x, g_gm_main_system.resume_pos_y, 0);
        OBS_CAMERA obj_camera = ObjCameraGet(g_obj.glb_camera_id);
        ObjObjectCameraSet(FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - OBD_LCD_Y / 2), FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - OBD_LCD_Y / 2));
        GmCameraSetClipCamera(obj_camera);
        return null;
    }

}