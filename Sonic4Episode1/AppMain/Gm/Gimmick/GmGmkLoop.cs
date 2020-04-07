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
    private static AppMain.OBS_OBJECT_WORK GmGmkLoopInit(
     AppMain.GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkLoopLoadObjNoModel(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        AppMain.gmGmkLoopInit(objWork);
        return objWork;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkLoopLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_LOOP");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static void gmGmkLoopInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkLoopSetRect(obj_work);
        obj_work.move_flag |= 8448U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkLoopMainFunc);
    }

    private static void gmGmkLoopSetRect(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        short cLeft = (short)((int)-gmsEnemy3DWork.ene_com.eve_rec.width * 64 / 2);
        short cRight = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.width * 64 / 2);
        short cTop = (short)((int)-gmsEnemy3DWork.ene_com.eve_rec.height * 64 / 2);
        short cBottom = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.height * 64 / 2);
        AppMain.ObjRectWorkZSet(pRec, cLeft, cTop, (short)-500, cRight, cBottom, (short)500);
        pRec.flag |= 1024U;
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkLoopDefFunc);
    }

    private static void gmGmkLoopDefFunc(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.UNREFERENCED_PARAMETER((object)target_rect);
        own_rect.parent_obj.user_flag |= 1U;
    }

    private static void gmGmkLoopMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.user_flag & 1) == 0)
            return;
        AppMain.gmGmkLoopExecute(obj_work);
        obj_work.user_flag &= 4294967294U;
    }

    private static void gmGmkLoopExecute(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        int loop_x = (int)gmsEnemy3DWork.ene_com.eve_rec.left * 64 * 4096;
        int loop_y = (int)gmsEnemy3DWork.ene_com.eve_rec.top * 64 * 4096;
        AppMain.gmGmkLoopExecuteObj(loop_x, loop_y, 1);
        AppMain.gmGmkLoopExecuteObj(loop_x, loop_y, 2);
        AppMain.gmGmkLoopExecuteEffect(loop_x, loop_y);
        AppMain.gmGmkLoopExecuteRing(loop_x, loop_y);
        AppMain.gmGmkLoopExecuteCamera(loop_x, loop_y);
        AppMain.GmEveMgrCreateEventLcd(0U);
    }

    private static void gmGmkLoopExecuteObj(int loop_x, int loop_y, int obj_type)
    {
        for (AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)obj_type); obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, (ushort)obj_type))
        {
            obj_work.pos.x += loop_x;
            obj_work.pos.y += loop_y;
        }
    }

    private static void gmGmkLoopExecuteEffect(int loop_x, int loop_y)
    {
        for (AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)5); obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, (ushort)5))
        {
            obj_work.pos.x += loop_x;
            obj_work.pos.y += loop_y;
            if (obj_work.obj_3des != null)
                AppMain.GmEffect3DESSetDuplicateDraw((AppMain.GMS_EFFECT_3DES_WORK)obj_work, AppMain.FX_FX32_TO_F32(loop_x), AppMain.FX_FX32_TO_F32(loop_y), 0.0f);
        }
    }

    private static void gmGmkLoopExecuteRing(int loop_x, int loop_y)
    {
        for (AppMain.GMS_RING_WORK gmsRingWork = AppMain.GmRingGetWork().damage_ring_list_start; gmsRingWork != null; gmsRingWork = gmsRingWork.post_ring)
        {
            gmsRingWork.pos.x += loop_x;
            gmsRingWork.pos.y += loop_y;
        }
    }

    private static void gmGmkLoopExecuteCamera(int loop_x, int loop_y)
    {
        AppMain.OBS_CAMERA obj_camera = AppMain.ObjCameraGet(0);
        AppMain.GmCameraPosSet(AppMain.FX_F32_TO_FX32(obj_camera.pos.x) + loop_x, -AppMain.FX_F32_TO_FX32(obj_camera.pos.y) + loop_y, AppMain.FX_F32_TO_FX32(obj_camera.pos.z));
        AppMain.ObjObjectCameraSet(AppMain.FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - (float)((int)AppMain.OBD_LCD_X / 2)), AppMain.FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - (float)((int)AppMain.OBD_LCD_Y / 2)), AppMain.FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - (float)((int)AppMain.OBD_LCD_X / 2)), AppMain.FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - (float)((int)AppMain.OBD_LCD_Y / 2)));
        AppMain.GmCameraSetClipCamera(obj_camera);
    }

}