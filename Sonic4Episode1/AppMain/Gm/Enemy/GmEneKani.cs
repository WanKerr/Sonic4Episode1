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
    public static void GmEneKaniBuild()
    {
        AppMain.gm_ene_kani_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(684)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(685)), 0U);
    }

    public static void GmEneKaniFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(684));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_kani_obj_3d_list, amsAmbHeader.file_num);
    }

    public static AppMain.OBS_OBJECT_WORK GmEneKaniInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_KANI_WORK()), "ENE_KANI");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_KANI_WORK gmsEneKaniWork = (AppMain.GMS_ENE_KANI_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_kani_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(686), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-11, (short)-24, (short)11, (short)0);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        AppMain.ObjRectWorkSet(pRec2, (short)-19, (short)-32, (short)19, (short)0);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-19, (short)-32, (short)19, (short)0);
        pRec3.flag &= 4294967291U;
        AppMain.ObjObjectFieldRectSet(work, (short)-4, (short)-8, (short)4, (short)-2);
        work.disp_flag |= 4194304U;
        work.move_flag |= 128U;
        gmsEneKaniWork.walk_s = 0;
        if (((int)eve_rec.flag & 1) != 0)
            gmsEneKaniWork.walk_s = 1;
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        gmsEneKaniWork.spd_dec = 102;
        gmsEneKaniWork.spd_dec_dist = 20480;
        AppMain.gmEneKaniWalkInit(work);
        AppMain.GmEneUtilInitNodeMatrix(gmsEneKaniWork.node_work, work, 3);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmEneExit));
        AppMain.GmEneUtilGetNodeMatrix(gmsEneKaniWork.node_work, 16);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        work.flag |= 1073741824U;
        gmsEneKaniWork.ata_futa = 0;
        return work;
    }

    public static int gmEneKaniGetLength2N(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
            return int.MaxValue;
        int x1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int x2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        float f32_1 = AppMain.FX_FX32_TO_F32(x1);
        float f32_2 = AppMain.FX_FX32_TO_F32(x2);
        return (int)((double)f32_1 * (double)f32_1 + (double)f32_2 * (double)f32_2);
    }

    public static int gmEneKaniIsPlayerLeft(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        return AppMain.GmEneComTargetIsLeft(obj_work, gmsPlayerWork.obj_work);
    }

    public static void gmEneKaniWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 2, 2);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKaniWalkMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec, (short)-11, (short)-24, (short)11, (short)0);
        pRec.flag |= 4U;
        AppMain.GMS_ENE_KANI_WORK gmsEneKaniWork = (AppMain.GMS_ENE_KANI_WORK)obj_work;
        if (gmsEneKaniWork.walk_s != 0)
            gmsEneKaniWork.timer = 15;
        else
            gmsEneKaniWork.timer = 10 + (int)AppMain.mtMathRand() % 20;
    }

    public static void gmEneKaniWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KANI_WORK gmsEneKaniWork = (AppMain.GMS_ENE_KANI_WORK)obj_work;
        if (gmsEneKaniWork.ata_futa != 0)
        {
            if (gmsEneKaniWork.timer > 0)
            {
                --gmsEneKaniWork.timer;
                return;
            }
            obj_work.obj_3d.speed[0] = 2f;
            obj_work.disp_flag ^= 1U;
            obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKaniWalkMain);
            gmsEneKaniWork.timer = gmsEneKaniWork.walk_s == 0 ? 10 + (int)AppMain.mtMathRand() % 20 : 15;
        }
        else
        {
            obj_work.obj_3d.speed[0] = 1f;
            if (((int)obj_work.move_flag & 4) != 0 || AppMain.GmEneComCheckMoveLimit(obj_work, (int)obj_work.user_work, (int)obj_work.user_flag) == 0)
            {
                AppMain.gmEneKaniFlipInit(obj_work);
                gmsEneKaniWork.timer = 0;
            }
        }
        if (AppMain.gmEneKaniIsPlayerLeft(obj_work) != 0)
        {
            gmsEneKaniWork.ata_futa = 0;
            if (AppMain.gmEneKaniGetLength2N(obj_work) >= 8464)
                return;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKaniAttackInit);
        }
        else
            gmsEneKaniWork.ata_futa = 1;
    }

    public static void gmEneKaniAttackInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet(obj_work, 0);
        obj_work.disp_flag &= 4294967291U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKaniAttackMain);
        obj_work.spd.x = 0;
        AppMain.GmSoundPlaySE("Kani");
    }

    public static void gmEneKaniAttackMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_RECT_WORK pRec = ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1];
        AppMain.NNS_MATRIX nodeMatrix = AppMain.GmEneUtilGetNodeMatrix(((AppMain.GMS_ENE_KANI_WORK)obj_work).node_work, 16);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.x = nodeMatrix.M03 - AppMain.FX_FX32_TO_F32(obj_work.pos.x);
        nnsVector.y = nodeMatrix.M13 - AppMain.FX_FX32_TO_F32(-obj_work.pos.y);
        nnsVector.z = nodeMatrix.M23 - AppMain.FX_FX32_TO_F32(obj_work.pos.z);
        if (((int)obj_work.disp_flag & 1) != 0)
            nnsVector.x = -nnsVector.x;
        AppMain.ObjRectWorkSet(pRec, (short)((int)(short)nnsVector.x - 11), (short)(-24 - (int)(short)nnsVector.y), (short)(11 + (int)(short)nnsVector.x), (short)-nnsVector.y);
        pRec.flag |= 4U;
        if (AppMain.GmBsCmnIsActionEnd(obj_work) != 0)
        {
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKaniAttackEnd);
            AppMain.ObjDrawObjectActionSet(obj_work, 1);
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    public static void gmEneKaniAttackEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_RECT_WORK pRec = ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1];
        AppMain.NNS_MATRIX nodeMatrix = AppMain.GmEneUtilGetNodeMatrix(((AppMain.GMS_ENE_KANI_WORK)obj_work).node_work, 16);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.x = nodeMatrix.M03 - AppMain.FX_FX32_TO_F32(obj_work.pos.x);
        nnsVector.y = nodeMatrix.M13 - AppMain.FX_FX32_TO_F32(-obj_work.pos.y);
        nnsVector.z = nodeMatrix.M23 - AppMain.FX_FX32_TO_F32(obj_work.pos.z);
        if (((int)obj_work.disp_flag & 1) != 0)
            nnsVector.x = -nnsVector.x;
        AppMain.ObjRectWorkSet(pRec, (short)((int)(short)nnsVector.x - 11), (short)(-24 - (int)(short)nnsVector.y), (short)(11 + (int)(short)nnsVector.x), (short)-nnsVector.y);
        pRec.flag |= 4U;
        if (AppMain.GmBsCmnIsActionEnd(obj_work) != 0)
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKaniWalkInit);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    public static void gmEneKaniFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        AppMain.gmEneKaniFlipInit(obj_work);
    }

    public static void gmEneKaniFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.spd.x = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKaniFlipMain);
    }

    public static void gmEneKaniFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmEneKaniSetWalkSpeed((AppMain.GMS_ENE_KANI_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        AppMain.gmEneKaniWalkInit(obj_work);
    }

    public static int gmEneKaniSetWalkSpeed(AppMain.GMS_ENE_KANI_WORK kani_work)
    {
        return 0;
    }

}