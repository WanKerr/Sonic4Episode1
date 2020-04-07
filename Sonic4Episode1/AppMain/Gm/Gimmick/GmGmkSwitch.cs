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
    private static void GmGmkSwitchBuildTypeZone3()
    {
        AppMain.gm_gmk_switch_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(931), AppMain.GmGameDatGetGimmickData(932), 0U);
        AppMain.ClearArray<AppMain.GMS_GMK_SW_STATE_WORK>(AppMain.gm_gmk_switch_state);
    }

    private static void GmGmkSwitchBuildTypeZone4()
    {
        AppMain.gm_gmk_switch_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(931), AppMain.GmGameDatGetGimmickData(932), 0U);
        AppMain.ClearArray<AppMain.GMS_GMK_SW_STATE_WORK>(AppMain.gm_gmk_switch_state);
    }

    private static void GmGmkSwitchReBuild()
    {
        AppMain.ClearArray<AppMain.GMS_GMK_SW_STATE_WORK>(AppMain.gm_gmk_switch_state);
    }

    private static void GmGmkSwitchFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(931);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_switch_obj_3d_list, gimmickData.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSwitchInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SW_WORK()), "GMK_SWITCH");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_GMK_SW_WORK gmsGmkSwWork = (AppMain.GMS_GMK_SW_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_switch_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 2)
        {
            AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, AppMain.ObjDataGet(933), (string)null, 0, (AppMain.AMS_AMB_HEADER)null, 1, 1);
            AppMain.ObjDrawAction3dActionSet3DNNMaterial(gmsEnemy3DWork.obj_3d, 0);
            work.disp_flag |= 4U;
        }
        AppMain.ObjCopyAction3dNNModel(AppMain.gm_gmk_switch_obj_3d_list[0], gmsGmkSwWork.obj_3d_base);
        work.pos.z = -262144;
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSwDispFunc);
        AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work;
        colWork.obj_col.width = (ushort)32;
        colWork.obj_col.height = (ushort)24;
        colWork.obj_col.ofst_x = (short)-16;
        colWork.obj_col.ofst_y = (short)-14;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)9)
        {
            colWork.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
            pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkSwitchDefFunc);
            pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
            AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
            AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
            AppMain.ObjRectWorkSet(pRec, (short)-16, (short)-20, (short)16, (short)-4);
            pRec.flag |= 132U;
        }
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        gmsGmkSwWork.id = (uint)AppMain.MTM_MATH_CLIP((int)eve_rec.left, 0, 64);
        gmsGmkSwWork.time = (int)eve_rec.width * 60 * 4096 + (int)eve_rec.top * 4096;
        if (gmsGmkSwWork.time != 0 && gmsGmkSwWork.time < 12288)
            gmsGmkSwWork.time = 12288;
        if (AppMain.gm_gmk_switch_state[(int)gmsGmkSwWork.id].sw)
        {
            gmsGmkSwWork.top_pos_y = -10;
            AppMain.gmGmkSwOnInit(work, false);
        }
        else
        {
            gmsGmkSwWork.top_pos_y = -14;
            AppMain.gmGmkSwOffInit(work);
        }
        AppMain.gmGmkSwSetCol(gmsGmkSwWork.gmk_work.ene_com.col_work, gmsGmkSwWork.top_pos_y);
        return work;
    }

    private static bool GmGmkSwitchIsOn(uint sw_id)
    {
        return AppMain.gm_gmk_switch_state[(int)sw_id].sw;
    }

    private static bool GmGmkSwitchTypeIsGear(uint sw_id)
    {
        return AppMain.gm_gmk_switch_state[(int)sw_id].gear;
    }

    private static void GmGmkSwitchSetOnGearSwitch(uint sw_id, int per)
    {
        AppMain.gm_gmk_switch_state[(int)sw_id].sw = true;
        AppMain.gm_gmk_switch_state[(int)sw_id].gear = true;
        AppMain.gm_gmk_switch_state[(int)sw_id].per = per;
    }

    private static void GmGmkSwitchSetOffGearSwitch(uint sw_id, int per)
    {
        AppMain.gm_gmk_switch_state[(int)sw_id].sw = false;
        AppMain.gm_gmk_switch_state[(int)sw_id].gear = true;
        AppMain.gm_gmk_switch_state[(int)sw_id].per = per;
    }

    private static int GmGmkSwitchGetPer(uint sw_id)
    {
        return AppMain.gm_gmk_switch_state[(int)sw_id].per;
    }

    private static void gmGmkSwOffInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SW_WORK gmsGmkSwWork = (AppMain.GMS_GMK_SW_WORK)obj_work;
        AppMain.gm_gmk_switch_state[(int)gmsGmkSwWork.id].sw = false;
        AppMain.gm_gmk_switch_state[(int)gmsGmkSwWork.id].time = 0;
        obj_work.flag &= 4294967279U;
        if (gmsGmkSwWork.top_pos_y < -14)
            gmsGmkSwWork.top_pos_y = -14;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSwOffMain);
    }

    private static void gmGmkSwOffMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SW_WORK gmsGmkSwWork = (AppMain.GMS_GMK_SW_WORK)obj_work;
        if (gmsGmkSwWork.top_pos_y > -14)
        {
            gmsGmkSwWork.top_pos_y += -2;
            if (gmsGmkSwWork.top_pos_y < -14)
                gmsGmkSwWork.top_pos_y = -14;
            AppMain.gmGmkSwSetCol(gmsGmkSwWork.gmk_work.ene_com.col_work, gmsGmkSwWork.top_pos_y);
        }
        if (gmsGmkSwWork.gmk_work.ene_com.col_work.obj_col.rider_obj != null && gmsGmkSwWork.gmk_work.ene_com.col_work.obj_col.rider_obj.obj_type == (ushort)1 || ((int)gmsGmkSwWork.gmk_work.ene_com.enemy_flag & 1) != 0)
            AppMain.gmGmkSwOnInit(obj_work, true);
        gmsGmkSwWork.gmk_work.ene_com.enemy_flag &= 4294967294U;
    }

    private static void gmGmkSwOnInit(AppMain.OBS_OBJECT_WORK obj_work, bool now_on)
    {
        AppMain.GMS_GMK_SW_WORK gmsGmkSwWork = (AppMain.GMS_GMK_SW_WORK)obj_work;
        AppMain.gm_gmk_switch_state[(int)gmsGmkSwWork.id].sw = true;
        AppMain.gm_gmk_switch_state[(int)gmsGmkSwWork.id].time = gmsGmkSwWork.time;
        if (gmsGmkSwWork.time != 0)
            obj_work.flag |= 16U;
        if (gmsGmkSwWork.top_pos_y > -10)
            gmsGmkSwWork.top_pos_y = -10;
        if (now_on)
        {
            AppMain.GmSoundPlaySE("Switch");
            AppMain.GMM_PAD_VIB_SMALL();
            AppMain.GmComEfctCreateSpring(obj_work, 0, (int)short.MinValue, -obj_work.pos.z);
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSwOnMain);
    }

    private static void gmGmkSwOnMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SW_WORK gmsGmkSwWork = (AppMain.GMS_GMK_SW_WORK)obj_work;
        if (gmsGmkSwWork.top_pos_y < -10)
        {
            gmsGmkSwWork.top_pos_y += 2;
            if (gmsGmkSwWork.top_pos_y > -10)
                gmsGmkSwWork.top_pos_y = -10;
            AppMain.gmGmkSwSetCol(gmsGmkSwWork.gmk_work.ene_com.col_work, gmsGmkSwWork.top_pos_y);
        }
        gmsGmkSwWork.gmk_work.ene_com.enemy_flag &= 4294967294U;
        if (gmsGmkSwWork.gmk_work.ene_com.col_work.obj_col.rider_obj != null && gmsGmkSwWork.gmk_work.ene_com.col_work.obj_col.rider_obj.obj_type == (ushort)1 || ((int)gmsGmkSwWork.gmk_work.ene_com.enemy_flag & 1) != 0)
            AppMain.gm_gmk_switch_state[(int)gmsGmkSwWork.id].time = gmsGmkSwWork.time;
        else if (AppMain.gm_gmk_switch_state[(int)gmsGmkSwWork.id].time != 0)
        {
            AppMain.gm_gmk_switch_state[(int)gmsGmkSwWork.id].time = AppMain.ObjTimeCountDown(AppMain.gm_gmk_switch_state[(int)gmsGmkSwWork.id].time);
            if (AppMain.gm_gmk_switch_state[(int)gmsGmkSwWork.id].time == 0)
                AppMain.gmGmkSwOffInit(obj_work);
        }
        gmsGmkSwWork.gmk_work.ene_com.enemy_flag &= 4294967294U;
    }

    private static void gmGmkSwitchDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != (ushort)1)
            return;
        parentObj1.enemy_flag |= 1U;
    }

    private static void gmGmkSwDispFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SW_WORK gmsGmkSwWork = (AppMain.GMS_GMK_SW_WORK)obj_work;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        uint dispFlag = obj_work.disp_flag;
        vecFx32.Assign(obj_work.pos);
        vecFx32.y += gmsGmkSwWork.top_pos_y << 12;
        AppMain.ObjDrawAction3DNN(obj_work.obj_3d, new AppMain.VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref obj_work.disp_flag);
        AppMain.ObjDrawAction3DNN(gmsGmkSwWork.obj_3d_base, new AppMain.VecFx32?(obj_work.pos), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref dispFlag);
    }

    private static void gmGmkSwSetCol(AppMain.OBS_COLLISION_WORK col_work, int top_pos_y)
    {
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)9)
            return;
        col_work.obj_col.ofst_y = (short)top_pos_y;
    }

}