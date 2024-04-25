public partial class AppMain
{

    private static OBS_OBJECT_WORK GmGmkPopSteamUInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obsObjectWork = gmGmkPopSteamInit(eve_rec, pos_x, pos_y, type, 0);
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPopSteamStart);
        return obsObjectWork;
    }

    private static OBS_OBJECT_WORK GmGmkPopSteamRInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obsObjectWork = gmGmkPopSteamInit(eve_rec, pos_x, pos_y, type, 1);
        obsObjectWork.dir.z = 16384;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPopSteamStart);
        return obsObjectWork;
    }

    private static OBS_OBJECT_WORK GmGmkPopSteamDInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obsObjectWork = gmGmkPopSteamInit(eve_rec, pos_x, pos_y, type, 2);
        obsObjectWork.dir.z = 32768;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPopSteamStart);
        return obsObjectWork;
    }

    private static OBS_OBJECT_WORK GmGmkPopSteamLInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obsObjectWork = gmGmkPopSteamInit(eve_rec, pos_x, pos_y, type, 3);
        obsObjectWork.dir.z = 49152;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPopSteamStart);
        return obsObjectWork;
    }

    public static void GmGmkPopSteamBuild()
    {
        gm_gmk_popsteam_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(926), GmGameDatGetGimmickData(927), 0U);
    }

    public static void GmGmkPopSteamFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(926);
        GmGameDBuildRegFlushModel(gm_gmk_popsteam_obj_3d_list, gimmickData.file_num);
    }

    private static void gmGmkBeltPopSteam_ppOutUseDirModel(OBS_OBJECT_WORK obj_work)
    {
        ushort z = obj_work.dir.z;
        obj_work.dir.z = 0;
        ObjDrawActionSummary(obj_work);
        obj_work.dir.z = z;
    }

    private static void _gmGmkPopSteam(GMS_GMK_P_STEAM_WORK pwork)
    {
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        if ((pwork.status & 1) != 0)
        {
            if (ply_work.seq_state == GME_PLY_SEQ_STATE_TRANS_SUPER)
            {
                pwork.ply_work = null;
                pwork.status &= 65532;
                return;
            }
            int spd_x;
            int spd_y;
            if ((pwork.steamvect & 16384) != 0)
            {
                spd_x = 0;
                spd_y = pwork.steampower;
                if ((pwork.steamvect & 32768) != 0)
                    spd_y = -spd_y;
            }
            else
            {
                spd_y = 0;
                spd_x = pwork.steampower;
                if ((pwork.steamvect & 32768) != 0)
                    spd_x = -spd_x;
            }
            GmPlySeqGmkInitPopSteamJump(ply_work, spd_x, spd_y, pwork.gmk_work.ene_com.eve_rec.top << 13);
            if ((pwork.status & 2) == 0)
            {
                GmSoundPlaySE("Steam");
                GMM_PAD_VIB_SMALL();
            }
            pwork.status |= 2;
        }
        else
            pwork.status &= 65533;
        pwork.status &= 65534;
    }

    private static void gmGmkPopSteamStay(OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPopSteamStay_100);
        gmGmkPopSteamStay_100(obj_work);
    }

    private static void gmGmkPopSteamStay_100(OBS_OBJECT_WORK obj_work)
    {
        _gmGmkPopSteam((GMS_GMK_P_STEAM_WORK)obj_work);
    }

    private static void gmGmkPopSteamInterval(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_P_STEAM_WORK gmsGmkPSteamWork = (GMS_GMK_P_STEAM_WORK)obj_work;
        obj_work.flag &= 4294967293U;
        gmsGmkPSteamWork.opt_steam = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(obj_work, tbl_popsteam_effct[0][gmsGmkPSteamWork.steamsize]);
        gmsGmkPSteamWork.opt_steam.dir.z = obj_work.dir.z;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPopSteamInterval_100);
        gmGmkPopSteamInterval_100(obj_work);
    }

    private static void gmGmkPopSteamInterval_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_P_STEAM_WORK pwork = (GMS_GMK_P_STEAM_WORK)obj_work;
        pwork.timer -= (short)(pwork.steamwait / 60);
        if (pwork.timer <= 0)
        {
            if (pwork.opt_steam != null)
            {
                ObjDrawKillAction3DES(pwork.opt_steam);
                pwork.opt_steam = null;
            }
            pwork.timer = 0;
            gmGmkPopSteamWait(obj_work);
        }
        else
            _gmGmkPopSteam(pwork);
    }

    private static void gmGmkPopSteamWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_P_STEAM_WORK gmsGmkPSteamWork = (GMS_GMK_P_STEAM_WORK)obj_work;
        for (int index = 0; index < 3; ++index)
            gmsGmkPSteamWork.opt_steam_int[index] = null;
        obj_work.flag |= 2U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPopSteamWait_100);
        gmGmkPopSteamWait_100(obj_work);
    }

    private static void gmGmkPopSteamWait_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_P_STEAM_WORK pwork = (GMS_GMK_P_STEAM_WORK)obj_work;
        ++pwork.timer;
        if (pwork.timer >= pwork.steamwait)
        {
            if (pwork.opt_steam_int[0] != null)
            {
                for (int index = 0; index < 3; ++index)
                {
                    ObjDrawKillAction3DES(pwork.opt_steam_int[index]);
                    pwork.opt_steam_int[index] = null;
                }
            }
            obj_work.user_timer = 0;
            obj_work.pos.x = pwork.pos_x;
            obj_work.pos.y = pwork.pos_y;
            gmGmkPopSteamInterval(obj_work);
        }
        else
        {
            if (pwork.timer >= pwork.steamwait * 3 / 4)
            {
                obj_work.pos.x = pwork.pos_x + (tbl_psteam_viv[obj_work.user_timer][0] << 12);
                obj_work.pos.y = pwork.pos_y + (tbl_psteam_viv[obj_work.user_timer][1] << 12);
                ++obj_work.user_timer;
                obj_work.user_timer %= 10;
            }
            _gmGmkPopSteam(pwork);
        }
    }

    private static void _ModgmGmkPopSteamStart(GMS_GMK_P_STEAM_WORK pwork, ref short u)
    {
        if (pwork.steamsize == 2)
        {
            u /= 96;
            u *= 128;
        }
        else
        {
            if (pwork.steamsize != 0)
                return;
            u /= 96;
            u *= 64;
        }
    }

    private static void gmGmkPopSteamStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_P_STEAM_WORK pwork = (GMS_GMK_P_STEAM_WORK)obj_work;
        pwork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        pwork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec = pwork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkPopSteamHit);
        pRec.ppHit = null;
        ObjRectAtkSet(pRec, 0, 0);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectWorkSet(pRec, tbl_gm_gmk_upbumper_rect[pwork.obj_type][0], tbl_gm_gmk_upbumper_rect[pwork.obj_type][1], tbl_gm_gmk_upbumper_rect[pwork.obj_type][2], tbl_gm_gmk_upbumper_rect[pwork.obj_type][3]);
        pRec.flag &= 4294966271U;
        obj_work.flag &= 4294967293U;
        switch (pwork.obj_type)
        {
            case 0:
                obj_work.dir.z = 0;
                pwork.steamvect = 49152;
                _ModgmGmkPopSteamStart(pwork, ref pRec.rect.top);
                break;
            case 1:
                obj_work.dir.z = 16384;
                pwork.steamvect = 0;
                _ModgmGmkPopSteamStart(pwork, ref pRec.rect.right);
                break;
            case 2:
                obj_work.dir.z = 32768;
                pwork.steamvect = 16384;
                _ModgmGmkPopSteamStart(pwork, ref pRec.rect.bottom);
                break;
            case 3:
                obj_work.dir.z = 49152;
                pwork.steamvect = 32768;
                _ModgmGmkPopSteamStart(pwork, ref pRec.rect.left);
                break;
            default:
                _ModgmGmkPopSteamStart(pwork, ref pRec.rect.top);
                break;
        }
        pwork.timer = 0;
        pwork.status = 0;
        pwork.opt_steam = null;
        pwork.pos_x = obj_work.pos.x;
        pwork.pos_y = obj_work.pos.y;
        if (pwork.steamwait > 0)
        {
            uint num = g_gm_main_system.sync_time % ((uint)pwork.steamwait + 60U);
            if (num < 60U)
            {
                pwork.timer = (short)(num * pwork.steamwait / 60L);
                gmGmkPopSteamInterval(obj_work);
            }
            else
            {
                pwork.timer = (short)(pwork.steamwait - (num - 60U));
                gmGmkPopSteamWait(obj_work);
            }
            create_steampipe(obj_work, pwork.obj_type);
            pwork.opt_timer = create_steamtimer(obj_work, pwork.obj_type);
        }
        else
        {
            OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(obj_work, tbl_popsteam_effct[1][pwork.steamsize]);
            obsObjectWork.dir.z = obj_work.dir.z;
            obsObjectWork.pos.z -= 4096;
            gmGmkPopSteamStay(obj_work);
        }
    }

    private static void gmGmkPopSteamHit(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_GMK_P_STEAM_WORK parentObj1 = (GMS_GMK_P_STEAM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 == g_gm_main_system.ply_work[0])
        {
            parentObj1.ply_work = parentObj2;
            parentObj1.status |= 1;
        }
        mine_rect.flag &= 4294573823U;
    }

    private static void create_steampipe(OBS_OBJECT_WORK parent_obj, int obj_type)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_POPSTEAMPARTS_WORK(), null, 0, "Gmk_PopSteamPipe");
        GMS_GMK_POPSTEAMPARTS_WORK popsteampartsWork = (GMS_GMK_POPSTEAMPARTS_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_popsteam_obj_3d_list[tbl_popsteam_pipe_model_id[obj_type]], popsteampartsWork.eff_work.obj_3d);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBeltPopSteam_ppOutUseDirModel);
        work.parent_obj = parent_obj;
        work.pos.x = parent_obj.pos.x + tbl_popsteam_pipe_off[obj_type][0];
        work.pos.y = parent_obj.pos.y + tbl_popsteam_pipe_off[obj_type][1];
        work.pos.z = parent_obj.pos.z - 65536;
        work.dir.z = parent_obj.dir.z;
        work.flag &= 4294966271U;
        work.move_flag |= 256U;
        work.disp_flag |= 4194304U;
        work.disp_flag &= 4294967039U;
        work.ppFunc = null;
    }

    private static void gmGmkPopSteamTimer(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_P_STEAM_WORK parentObj = (GMS_GMK_P_STEAM_WORK)obj_work.parent_obj;
        uint num1 = 65536;
        int num2 = 4096;
        uint num3 = num1 * (uint)parentObj.timer / (uint)parentObj.steamwait;
        obj_work.dir.z = obj_work.parent_obj.dir.z;
        obj_work.dir.z += (ushort)num3;
        int num4 = num2 + (int)num3;
        obj_work.parent_ofst.z = num4 >> 3;
        obj_work.ofst.x = obj_work.parent_obj.ofst.x;
        obj_work.ofst.y = obj_work.parent_obj.ofst.x;
    }

    private static OBS_OBJECT_WORK create_steamtimer(
      OBS_OBJECT_WORK parent_obj,
      int obj_type)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_POPSTEAMPARTS_WORK(), null, 0, "Gmk_PopSteamTimer");
        GMS_GMK_POPSTEAMPARTS_WORK popsteampartsWork = (GMS_GMK_POPSTEAMPARTS_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_popsteam_obj_3d_list[12], popsteampartsWork.eff_work.obj_3d);
        work.parent_obj = parent_obj;
        work.parent_ofst.x = tbl_popsteam_timer_off[obj_type][0];
        work.parent_ofst.y = tbl_popsteam_timer_off[obj_type][1];
        work.parent_ofst.z = 0;
        work.dir.z = parent_obj.dir.z;
        work.flag |= 1024U;
        work.move_flag |= 256U;
        work.disp_flag |= 4194304U;
        work.disp_flag &= 4294967039U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPopSteamTimer);
        return work;
    }

    private static OBS_OBJECT_WORK gmGmkPopSteamInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      int obj_type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_P_STEAM_WORK(), "Gmk_PopSteam");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        int index = tbl_popsteam_model_id[obj_type][0];
        if (eve_rec.height == 0)
            index = tbl_popsteam_model_id[obj_type][1];
        ObjObjectCopyAction3dNNModel(work, gm_gmk_popsteam_obj_3d_list[index], gmsEnemy3DWork.obj_3d);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBeltPopSteam_ppOutUseDirModel);
        work.pos.z = 622592;
        work.move_flag |= 256U;
        work.disp_flag |= 4194304U;
        GMS_GMK_P_STEAM_WORK gmsGmkPSteamWork = (GMS_GMK_P_STEAM_WORK)work;
        gmsGmkPSteamWork.steamsize = (eve_rec.flag & 3) != 2 ? ((eve_rec.flag & 3) != 1 ? (short)1 : (short)0) : (short)2;
        gmsGmkPSteamWork.steampower = eve_rec.width * 2;
        gmsGmkPSteamWork.steampower <<= 12;
        if (gmsGmkPSteamWork.steampower == 0)
            gmsGmkPSteamWork.steampower = 61440;
        gmsGmkPSteamWork.steamwait = (short)(eve_rec.height * 2);
        gmsGmkPSteamWork.obj_type = obj_type;
        return work;
    }
}