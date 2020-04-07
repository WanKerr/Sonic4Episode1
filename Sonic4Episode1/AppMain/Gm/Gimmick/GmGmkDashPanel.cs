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
    private static AppMain.OBS_OBJECT_WORK GmGmkDashPanelInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_DASH_PANEL");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_dash_panel_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, false, AppMain.ObjDataGet(827), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectActionSet(work, 0);
        AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(828).pData);
        AppMain.ObjDrawObjectActionSet3DNNMaterial(work, 0);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkDashPanelDefFunc);
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)9 && (eve_rec.id == (ushort)109 || eve_rec.id == (ushort)110))
            AppMain.ObjRectWorkSet(pRec, (short)-16, (short)-8, (short)16, (short)8);
        else
            AppMain.ObjRectWorkSet(pRec, (short)-8, (short)-8, (short)8, (short)8);
        pRec.flag |= 1024U;
        if (eve_rec.id == (ushort)108)
            work.dir.y = (ushort)32768;
        else if (eve_rec.id == (ushort)109)
            work.dir.z = (ushort)49152;
        else if (eve_rec.id == (ushort)110)
        {
            work.dir.z = (ushort)16384;
            work.dir.y = (ushort)32768;
        }
        else
        {
            work.dir.z = (ushort)0;
            work.dir.y = (ushort)0;
        }
        work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        return work;
    }

    public static void GmGmkDashPanelBuild()
    {
        AppMain.gm_gmk_dash_panel_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(825), AppMain.GmGameDatGetGimmickData(826), 0U);
    }

    public static void GmGmkDashPanelFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(825));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_dash_panel_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkDashPanelDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != (ushort)1)
            return;
        int id = (int)parentObj1.eve_rec.id;
        if (((int)parentObj2.obj_work.move_flag & 1) == 0)
        {
            parentObj1.rect_work[2].flag &= 4294573823U;
        }
        else
        {
            AppMain.GmPlySeqInitDashPanel(parentObj2, (uint)parentObj1.eve_rec.id - 107U);
            AppMain.GmSoundPlaySE("DashPanel");
        }
    }

}