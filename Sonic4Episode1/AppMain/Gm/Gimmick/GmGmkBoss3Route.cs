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
    private static AppMain.OBS_OBJECT_WORK GmGmkBoss3RouteInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkBoss3RouteLoadObjNoModel(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        AppMain.gmGmkBoss3RouteInit(objWork);
        return objWork;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkBoss3RouteLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_BOSS3_ROUTE");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static void gmGmkBoss3RouteInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 8448U;
        obj_work.flag |= 16U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3RouteMainFunc);
        obj_work.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static bool gmGmkBoss3RouteCheckHit(
      AppMain.OBS_OBJECT_WORK target_obj_work,
      AppMain.OBS_OBJECT_WORK gimmick_obj_work)
    {
        int num1 = target_obj_work.pos.x - gimmick_obj_work.pos.x;
        int num2 = target_obj_work.pos.y - gimmick_obj_work.pos.y;
        return AppMain.MTM_MATH_ABS(num1) <= 262144 && AppMain.MTM_MATH_ABS(num2) <= 262144 && AppMain.FX_Mul(num1, num1) + AppMain.FX_Mul(num2, num2) <= AppMain.FX_Mul(262144, 262144);
    }

    private static bool gmGmkBoss3RouteSetMoveParam(
      AppMain.OBS_OBJECT_WORK target_obj_work,
      AppMain.OBS_OBJECT_WORK gimmick_obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)gimmick_obj_work;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        float num1 = (float)gmsEnemy3DWork.ene_com.eve_rec.width / 10f;
        if (gmsPlayerWork.obj_work.pos.y >= target_obj_work.pos.y && ((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0 && AppMain.ObjViewOutCheck(target_obj_work.pos.x, target_obj_work.pos.y, (short)96, (short)0, (short)0, (short)0, (short)0) != 0)
        {
            target_obj_work.spd.x = 0;
            target_obj_work.spd.y = 0;
            return false;
        }
        int x1 = gimmick_obj_work.pos.x + (int)gmsEnemy3DWork.ene_com.eve_rec.left * 64 * 4096;
        int x2 = gimmick_obj_work.pos.y + (int)gmsEnemy3DWork.ene_com.eve_rec.top * 64 * 4096;
        float f32_1 = AppMain.FX_FX32_TO_F32(x1);
        float f32_2 = AppMain.FX_FX32_TO_F32(x2);
        float f32_3 = AppMain.FX_FX32_TO_F32(target_obj_work.pos.x);
        float f32_4 = AppMain.FX_FX32_TO_F32(target_obj_work.pos.y);
        float x3 = f32_1 - f32_3;
        float y = f32_2 - f32_4;
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.amVectorSet(nnsVector, x3, y, 0.0f);
        float num2 = 1f / AppMain.nnLengthVector(nnsVector);
        float x4 = x3 * num2 * num1;
        float x5 = y * num2 * num1;
        target_obj_work.spd.x = AppMain.FX_F32_TO_FX32(x4);
        target_obj_work.spd.y = AppMain.FX_F32_TO_FX32(x5);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        return true;
    }

    private static void gmGmkBoss3RouteMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        for (AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)2); obsObjectWork != null; obsObjectWork = AppMain.ObjObjectSearchRegistObject(obsObjectWork, (ushort)2))
        {
            if (((AppMain.GMS_ENEMY_COM_WORK)obsObjectWork).eve_rec.id == (ushort)319)
            {
                if (!AppMain.gmGmkBoss3RouteCheckHit(obsObjectWork, obj_work))
                    break;
                if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 2) != 0)
                {
                    obsObjectWork.spd.x = 0;
                    obsObjectWork.spd.y = 0;
                    obsObjectWork.user_flag = 1U;
                    obj_work.flag |= 4U;
                    obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
                    gmsEnemy3DWork.ene_com.enemy_flag |= 65536U;
                    break;
                }
                if (!AppMain.gmGmkBoss3RouteSetMoveParam(obsObjectWork, obj_work))
                    break;
                obj_work.flag |= 4U;
                obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
                gmsEnemy3DWork.ene_com.enemy_flag |= 65536U;
                break;
            }
        }
    }

}