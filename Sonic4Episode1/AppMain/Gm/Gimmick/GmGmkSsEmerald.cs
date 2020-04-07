using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static AppMain.OBS_OBJECT_WORK GmGmkSsEmeraldInit(
     AppMain.GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        bool flag = false;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SS_EMERALD");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= (short)128;
        ushort num = (ushort)((uint)AppMain.g_gs_main_sys_info.stage_id - 21U);
        if (SSpecial.CreateInstance()[(int)num].IsGetEmerald())
        {
            flag = true;
            AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_ss_1up_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        }
        else
        {
            AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_ss_emerald_obj_3d_list[(int)num], gmsEnemy3DWork.obj_3d);
            AppMain.ObjObjectAction3dNNMotionLoad(work, 0, false, AppMain.ObjDataGet(912), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
            AppMain.ObjDrawObjectActionSet(work, (int)num);
        }
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194308U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsEmeraldMain);
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkSsEmeraldDefFunc);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec, (short)-4, (short)-4, (short)4, (short)4);
        int efct_zone_idx = !flag ? 1 + (int)num : 0;
        AppMain.gm_gmk_ss_emerald_effct = AppMain.GmEfctZoneEsCreate(work, 5, efct_zone_idx);
        return work;
    }

    public static void GmGmkSsEmeraldBuild()
    {
        AppMain.gm_gmk_ss_emerald_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(910), AppMain.GmGameDatGetGimmickData(911), 0U);
        AppMain.gm_gmk_ss_1up_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(913), AppMain.GmGameDatGetGimmickData(914), 0U);
    }

    public static void GmGmkSsEmeraldFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData1 = AppMain.GmGameDatGetGimmickData(910);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_ss_emerald_obj_3d_list, gimmickData1.file_num);
        AppMain.AMS_AMB_HEADER gimmickData2 = AppMain.GmGameDatGetGimmickData(913);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_ss_1up_obj_3d_list, gimmickData2.file_num);
    }

    private static void gmGmkSsEmeraldMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.dir.z = AppMain.GmMainGetObjectRotation();
    }

    private static void gmGmkSsEmeraldDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != (ushort)1 || parentObj2.gmk_obj == (AppMain.OBS_OBJECT_WORK)parentObj1))
            return;
        AppMain.GmSoundPlayJingle(3U, 0);
        AppMain.GmSoundPlaySE("Special5");
        AppMain.GmComEfctCreateRing(parentObj1.obj_work.pos.x, parentObj1.obj_work.pos.y);
        AppMain.gmGmkSsEmeraldEfctKill();
        parentObj1.obj_work.flag |= 4U;
        AppMain.g_gm_main_system.game_flag |= 65536U;
    }

    private static void gmGmkSsEmeraldEfctKill()
    {
        if (AppMain.gm_gmk_ss_emerald_effct == null)
            return;
        AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)AppMain.gm_gmk_ss_emerald_effct);
        AppMain.gm_gmk_ss_emerald_effct = (AppMain.GMS_EFFECT_3DES_WORK)null;
    }

}