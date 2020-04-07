using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{

    private static AppMain.OBS_OBJECT_WORK GmGmkPopSteamUInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.gmGmkPopSteamInit(eve_rec, pos_x, pos_y, type, 0);
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPopSteamStart);
        return obsObjectWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPopSteamRInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.gmGmkPopSteamInit(eve_rec, pos_x, pos_y, type, 1);
        obsObjectWork.dir.z = (ushort)16384;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPopSteamStart);
        return obsObjectWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPopSteamDInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.gmGmkPopSteamInit(eve_rec, pos_x, pos_y, type, 2);
        obsObjectWork.dir.z = (ushort)32768;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPopSteamStart);
        return obsObjectWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPopSteamLInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.gmGmkPopSteamInit(eve_rec, pos_x, pos_y, type, 3);
        obsObjectWork.dir.z = (ushort)49152;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPopSteamStart);
        return obsObjectWork;
    }

    public static void GmGmkPopSteamBuild()
    {
        AppMain.gm_gmk_popsteam_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(926), AppMain.GmGameDatGetGimmickData(927), 0U);
    }

    public static void GmGmkPopSteamFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(926);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_popsteam_obj_3d_list, gimmickData.file_num);
    }

    private static void gmGmkBeltPopSteam_ppOutUseDirModel(AppMain.OBS_OBJECT_WORK obj_work)
    {
        ushort z = obj_work.dir.z;
        obj_work.dir.z = (ushort)0;
        AppMain.ObjDrawActionSummary(obj_work);
        obj_work.dir.z = z;
    }

    private static void _gmGmkPopSteam(AppMain.GMS_GMK_P_STEAM_WORK pwork)
    {
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        if (((int)pwork.status & 1) != 0)
        {
            if (ply_work.seq_state == 24)
            {
                pwork.ply_work = (AppMain.GMS_PLAYER_WORK)null;
                pwork.status &= (ushort)65532;
                return;
            }
            int spd_x;
            int spd_y;
            if (((int)pwork.steamvect & 16384) != 0)
            {
                spd_x = 0;
                spd_y = pwork.steampower;
                if (((int)pwork.steamvect & 32768) != 0)
                    spd_y = -spd_y;
            }
            else
            {
                spd_y = 0;
                spd_x = pwork.steampower;
                if (((int)pwork.steamvect & 32768) != 0)
                    spd_x = -spd_x;
            }
            AppMain.GmPlySeqGmkInitPopSteamJump(ply_work, spd_x, spd_y, (int)pwork.gmk_work.ene_com.eve_rec.top << 13);
            if (((int)pwork.status & 2) == 0)
            {
                AppMain.GmSoundPlaySE("Steam");
                AppMain.GMM_PAD_VIB_SMALL();
            }
            pwork.status |= (ushort)2;
        }
        else
            pwork.status &= (ushort)65533;
        pwork.status &= (ushort)65534;
    }

    private static void gmGmkPopSteamStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPopSteamStay_100);
        AppMain.gmGmkPopSteamStay_100(obj_work);
    }

    private static void gmGmkPopSteamStay_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain._gmGmkPopSteam((AppMain.GMS_GMK_P_STEAM_WORK)obj_work);
    }

    private static void gmGmkPopSteamInterval(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_P_STEAM_WORK gmsGmkPSteamWork = (AppMain.GMS_GMK_P_STEAM_WORK)obj_work;
        obj_work.flag &= 4294967293U;
        gmsGmkPSteamWork.opt_steam = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate(obj_work, (int)AppMain.tbl_popsteam_effct[0][(int)gmsGmkPSteamWork.steamsize]);
        gmsGmkPSteamWork.opt_steam.dir.z = obj_work.dir.z;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPopSteamInterval_100);
        AppMain.gmGmkPopSteamInterval_100(obj_work);
    }

    private static void gmGmkPopSteamInterval_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_P_STEAM_WORK pwork = (AppMain.GMS_GMK_P_STEAM_WORK)obj_work;
        pwork.timer -= (short)((int)pwork.steamwait / 60);
        if (pwork.timer <= (short)0)
        {
            if (pwork.opt_steam != null)
            {
                AppMain.ObjDrawKillAction3DES(pwork.opt_steam);
                pwork.opt_steam = (AppMain.OBS_OBJECT_WORK)null;
            }
            pwork.timer = (short)0;
            AppMain.gmGmkPopSteamWait(obj_work);
        }
        else
            AppMain._gmGmkPopSteam(pwork);
    }

    private static void gmGmkPopSteamWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_P_STEAM_WORK gmsGmkPSteamWork = (AppMain.GMS_GMK_P_STEAM_WORK)obj_work;
        for (int index = 0; index < 3; ++index)
            gmsGmkPSteamWork.opt_steam_int[index] = (AppMain.OBS_OBJECT_WORK)null;
        obj_work.flag |= 2U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPopSteamWait_100);
        AppMain.gmGmkPopSteamWait_100(obj_work);
    }

    private static void gmGmkPopSteamWait_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_P_STEAM_WORK pwork = (AppMain.GMS_GMK_P_STEAM_WORK)obj_work;
        ++pwork.timer;
        if ((int)pwork.timer >= (int)pwork.steamwait)
        {
            if (pwork.opt_steam_int[0] != null)
            {
                for (int index = 0; index < 3; ++index)
                {
                    AppMain.ObjDrawKillAction3DES(pwork.opt_steam_int[index]);
                    pwork.opt_steam_int[index] = (AppMain.OBS_OBJECT_WORK)null;
                }
            }
            obj_work.user_timer = 0;
            obj_work.pos.x = pwork.pos_x;
            obj_work.pos.y = pwork.pos_y;
            AppMain.gmGmkPopSteamInterval(obj_work);
        }
        else
        {
            if ((int)pwork.timer >= (int)pwork.steamwait * 3 / 4)
            {
                obj_work.pos.x = pwork.pos_x + ((int)AppMain.tbl_psteam_viv[obj_work.user_timer][0] << 12);
                obj_work.pos.y = pwork.pos_y + ((int)AppMain.tbl_psteam_viv[obj_work.user_timer][1] << 12);
                ++obj_work.user_timer;
                obj_work.user_timer %= 10;
            }
            AppMain._gmGmkPopSteam(pwork);
        }
    }

    private static void _ModgmGmkPopSteamStart(AppMain.GMS_GMK_P_STEAM_WORK pwork, ref short u)
    {
        if (pwork.steamsize == (short)2)
        {
            u /= (short)96;
            u *= (short)128;
        }
        else
        {
            if (pwork.steamsize != (short)0)
                return;
            u /= (short)96;
            u *= (short)64;
        }
    }

    private static void gmGmkPopSteamStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_P_STEAM_WORK pwork = (AppMain.GMS_GMK_P_STEAM_WORK)obj_work;
        pwork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        pwork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = pwork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkPopSteamHit);
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec, AppMain.tbl_gm_gmk_upbumper_rect[pwork.obj_type][0], AppMain.tbl_gm_gmk_upbumper_rect[pwork.obj_type][1], AppMain.tbl_gm_gmk_upbumper_rect[pwork.obj_type][2], AppMain.tbl_gm_gmk_upbumper_rect[pwork.obj_type][3]);
        pRec.flag &= 4294966271U;
        obj_work.flag &= 4294967293U;
        switch (pwork.obj_type)
        {
            case 0:
                obj_work.dir.z = (ushort)0;
                pwork.steamvect = (ushort)49152;
                AppMain._ModgmGmkPopSteamStart(pwork, ref pRec.rect.top);
                break;
            case 1:
                obj_work.dir.z = (ushort)16384;
                pwork.steamvect = (ushort)0;
                AppMain._ModgmGmkPopSteamStart(pwork, ref pRec.rect.right);
                break;
            case 2:
                obj_work.dir.z = (ushort)32768;
                pwork.steamvect = (ushort)16384;
                AppMain._ModgmGmkPopSteamStart(pwork, ref pRec.rect.bottom);
                break;
            case 3:
                obj_work.dir.z = (ushort)49152;
                pwork.steamvect = (ushort)32768;
                AppMain._ModgmGmkPopSteamStart(pwork, ref pRec.rect.left);
                break;
            default:
                AppMain._ModgmGmkPopSteamStart(pwork, ref pRec.rect.top);
                break;
        }
        pwork.timer = (short)0;
        pwork.status = (ushort)0;
        pwork.opt_steam = (AppMain.OBS_OBJECT_WORK)null;
        pwork.pos_x = obj_work.pos.x;
        pwork.pos_y = obj_work.pos.y;
        if (pwork.steamwait > (short)0)
        {
            uint num = AppMain.g_gm_main_system.sync_time % ((uint)pwork.steamwait + 60U);
            if (num < 60U)
            {
                pwork.timer = (short)((long)num * (long)pwork.steamwait / 60L);
                AppMain.gmGmkPopSteamInterval(obj_work);
            }
            else
            {
                pwork.timer = (short)((long)pwork.steamwait - (long)(num - 60U));
                AppMain.gmGmkPopSteamWait(obj_work);
            }
            AppMain.create_steampipe(obj_work, pwork.obj_type);
            pwork.opt_timer = AppMain.create_steamtimer(obj_work, pwork.obj_type);
        }
        else
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate(obj_work, (int)AppMain.tbl_popsteam_effct[1][(int)pwork.steamsize]);
            obsObjectWork.dir.z = obj_work.dir.z;
            obsObjectWork.pos.z -= 4096;
            AppMain.gmGmkPopSteamStay(obj_work);
        }
    }

    private static void gmGmkPopSteamHit(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_GMK_P_STEAM_WORK parentObj1 = (AppMain.GMS_GMK_P_STEAM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 == AppMain.g_gm_main_system.ply_work[0])
        {
            parentObj1.ply_work = parentObj2;
            parentObj1.status |= (ushort)1;
        }
        mine_rect.flag &= 4294573823U;
    }

    private static void create_steampipe(AppMain.OBS_OBJECT_WORK parent_obj, int obj_type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_POPSTEAMPARTS_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "Gmk_PopSteamPipe");
        AppMain.GMS_GMK_POPSTEAMPARTS_WORK popsteampartsWork = (AppMain.GMS_GMK_POPSTEAMPARTS_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_popsteam_obj_3d_list[AppMain.tbl_popsteam_pipe_model_id[obj_type]], popsteampartsWork.eff_work.obj_3d);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBeltPopSteam_ppOutUseDirModel);
        work.parent_obj = parent_obj;
        work.pos.x = parent_obj.pos.x + AppMain.tbl_popsteam_pipe_off[obj_type][0];
        work.pos.y = parent_obj.pos.y + AppMain.tbl_popsteam_pipe_off[obj_type][1];
        work.pos.z = parent_obj.pos.z - 65536;
        work.dir.z = parent_obj.dir.z;
        work.flag &= 4294966271U;
        work.move_flag |= 256U;
        work.disp_flag |= 4194304U;
        work.disp_flag &= 4294967039U;
        work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmGmkPopSteamTimer(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_P_STEAM_WORK parentObj = (AppMain.GMS_GMK_P_STEAM_WORK)obj_work.parent_obj;
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

    private static AppMain.OBS_OBJECT_WORK create_steamtimer(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int obj_type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_POPSTEAMPARTS_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "Gmk_PopSteamTimer");
        AppMain.GMS_GMK_POPSTEAMPARTS_WORK popsteampartsWork = (AppMain.GMS_GMK_POPSTEAMPARTS_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_popsteam_obj_3d_list[12], popsteampartsWork.eff_work.obj_3d);
        work.parent_obj = parent_obj;
        work.parent_ofst.x = AppMain.tbl_popsteam_timer_off[obj_type][0];
        work.parent_ofst.y = AppMain.tbl_popsteam_timer_off[obj_type][1];
        work.parent_ofst.z = 0;
        work.dir.z = parent_obj.dir.z;
        work.flag |= 1024U;
        work.move_flag |= 256U;
        work.disp_flag |= 4194304U;
        work.disp_flag &= 4294967039U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPopSteamTimer);
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK gmGmkPopSteamInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      int obj_type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_P_STEAM_WORK()), "Gmk_PopSteam");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        int index = AppMain.tbl_popsteam_model_id[obj_type][0];
        if (eve_rec.height == (byte)0)
            index = AppMain.tbl_popsteam_model_id[obj_type][1];
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_popsteam_obj_3d_list[index], gmsEnemy3DWork.obj_3d);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBeltPopSteam_ppOutUseDirModel);
        work.pos.z = 622592;
        work.move_flag |= 256U;
        work.disp_flag |= 4194304U;
        AppMain.GMS_GMK_P_STEAM_WORK gmsGmkPSteamWork = (AppMain.GMS_GMK_P_STEAM_WORK)work;
        gmsGmkPSteamWork.steamsize = ((int)eve_rec.flag & 3) != 2 ? (((int)eve_rec.flag & 3) != 1 ? (short)1 : (short)0) : (short)2;
        gmsGmkPSteamWork.steampower = (int)eve_rec.width * 2;
        gmsGmkPSteamWork.steampower <<= 12;
        if (gmsGmkPSteamWork.steampower == 0)
            gmsGmkPSteamWork.steampower = 61440;
        gmsGmkPSteamWork.steamwait = (short)((int)eve_rec.height * 2);
        gmsGmkPSteamWork.obj_type = obj_type;
        return work;
    }
}