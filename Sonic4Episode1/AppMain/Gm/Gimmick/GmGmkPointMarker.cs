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
    private static AppMain.OBS_OBJECT_WORK GmGmkPointMarkerInit(
         AppMain.GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        if (AppMain.g_gs_main_sys_info.game_mode == 1)
        {
            eve_rec.pos_x = byte.MaxValue;
            return (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_PMARKER_WORK()), "GMK_POINT_MARKER");
        AppMain.GMS_GMK_PMARKER_WORK gmsGmkPmarkerWork = (AppMain.GMS_GMK_PMARKER_WORK)work;
        work.pos.y += 4096;
        work.pos.z = AppMain.g_gs_main_sys_info.stage_id != (ushort)9 ? -65536 : -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        gmsGmkPmarkerWork.marker_prty = (ushort)eve_rec.left;
        AppMain.gmGmkPointMarkerStart(work);
        return work;
    }

    public static void GmGmkPointMarkerBuild()
    {
        AppMain.gm_gmk_pmarker_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(838), AppMain.GmGameDatGetGimmickData(839), 0U);
    }

    public static void GmGmkPointMarkerFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(838));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_pmarker_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkPointMarkerHit(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_GMK_PMARKER_WORK parentObj1 = (AppMain.GMS_GMK_PMARKER_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        parentObj1.markerdist = parentObj1.OBJWORK.pos.x - parentObj2.obj_work.pos.x;
        if (parentObj1.markerdist <= 16384 && parentObj1.markerdist >= -16384 || parentObj1.markerdist < 16384 && parentObj1.markerdistlast >= 16384 || parentObj1.markerdist > -16384 && parentObj1.markerdistlast <= -16384)
        {
            if (AppMain.g_gm_main_system.marker_pri < (uint)parentObj1.marker_prty)
            {
                AppMain.GmPlayerSetMarkerPoint(parentObj2, parentObj1.OBJWORK.pos.x, parentObj1.OBJWORK.pos.y);
                AppMain.g_gm_main_system.marker_pri = (uint)parentObj1.marker_prty;
                parentObj1.marker_prty = (ushort)0;
                parentObj1.hitcounter = 2;
                SaveState.saveCurrentState(1);
            }
            parentObj1.OBJWORK.flag |= 2U;
        }
        else
            mine_rect.flag &= 4294573823U;
    }

    private static void gmGmkPointMarkerStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PMARKER_WORK gmsGmkPmarkerWork = (AppMain.GMS_GMK_PMARKER_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet(obj_work, 0);
        gmsGmkPmarkerWork.markerdist = 0;
        gmsGmkPmarkerWork.hitcounter = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPointMarkerStay_100);
    }

    private static void gmGmkPointMarkerStay_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PMARKER_WORK gmsGmkPmarkerWork = (AppMain.GMS_GMK_PMARKER_WORK)obj_work;
        gmsGmkPmarkerWork.markerdistlast = gmsGmkPmarkerWork.markerdist;
        gmsGmkPmarkerWork.markerdist = 0;
        if (gmsGmkPmarkerWork.hitcounter > 0)
        {
            AppMain.GmSoundPlaySE("Marker");
            AppMain.gmGmkPointMarkerStay_200(obj_work);
        }
        else
        {
            if (AppMain.g_gm_main_system.marker_pri < (uint)gmsGmkPmarkerWork.marker_prty)
                return;
            gmsGmkPmarkerWork.marker_prty = (ushort)0;
            AppMain.gmGmkPointMarkerStay_400(obj_work);
        }
    }

    private static void gmGmkPointMarkerStay_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawObjectActionSet(obj_work, 1);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPointMarkerStay_210);
    }

    private static void gmGmkPointMarkerStay_210(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PMARKER_WORK gmsGmkPmarkerWork = (AppMain.GMS_GMK_PMARKER_WORK)obj_work;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        --gmsGmkPmarkerWork.hitcounter;
        if (gmsGmkPmarkerWork.hitcounter == 0)
        {
            AppMain.ObjDrawObjectActionSet(obj_work, 0);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPointMarkerStay_300);
        }
        else
            AppMain.gmGmkPointMarkerStay_200(obj_work);
    }

    private static void gmGmkPointMarkerStay_300(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        AppMain.gmGmkPointMarkerStay_400(obj_work);
    }

    private static void gmGmkPointMarkerStay_400(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctCmnEsCreate(obj_work, 49);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 34f, 0.0f);
        efct_3des.efct_com.obj_work.pos.z = AppMain.g_gs_main_sys_info.stage_id != (ushort)9 ? obj_work.pos.z + 65536 : obj_work.pos.z + 40960;
        AppMain.ObjAction3dNNMaterialMotionLoad(obj_work.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(841).pData);
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.obj_3d.mat_speed = 1f;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmGmkPointMarkerStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PMARKER_WORK gmsGmkPmarkerWork = (AppMain.GMS_GMK_PMARKER_WORK)obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_pmarker_obj_3d_list[0], gmsGmkPmarkerWork.OBJ_3D);
        AppMain.ObjObjectAction3dNNMotionLoad(obj_work, 0, false, AppMain.ObjDataGet(840), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectActionSet(obj_work, 0);
        if (AppMain.g_gm_main_system.marker_pri < (uint)gmsGmkPmarkerWork.marker_prty)
        {
            gmsGmkPmarkerWork.COMWORK.rect_work[0].flag &= 4294967291U;
            gmsGmkPmarkerWork.COMWORK.rect_work[1].flag &= 4294967291U;
            AppMain.OBS_RECT_WORK pRec = gmsGmkPmarkerWork.COMWORK.rect_work[2];
            pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkPointMarkerHit);
            pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
            AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
            AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
            AppMain.ObjRectWorkSet(pRec, (short)-16, (short)-64, (short)16, (short)0);
        }
        else
            gmsGmkPmarkerWork.marker_prty = (ushort)0;
        AppMain.gmGmkPointMarkerStay(obj_work);
    }


}