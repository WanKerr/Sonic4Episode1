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
    public static void GmEneTStarBuild()
    {
        AppMain.gm_ene_t_star_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(680)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(681)), 0U);
    }

    public static void GmEneTStarFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(680));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_t_star_obj_3d_list, amsAmbHeader.file_num);
    }

    public static AppMain.OBS_OBJECT_WORK GmEneTStarInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_T_STAR_WORK()), "ENE_T_STAR");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_T_STAR_WORK gmsEneTStarWork = (AppMain.GMS_ENE_T_STAR_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_t_star_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(682), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.OBS_DATA_WORK data_work = AppMain.ObjDataGet(683);
        AppMain.ObjObjectAction3dNNMaterialMotionLoad(work, 0, data_work, (string)null, 0, (object)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-16, (short)-16, (short)16, (short)16);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        AppMain.ObjRectWorkSet(pRec2, (short)-10, (short)-10, (short)10, (short)10);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-20, (short)-20, (short)20, (short)20);
        pRec3.flag &= 4294967291U;
        work.disp_flag |= 4194304U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        if (((int)eve_rec.flag & 7) == 0)
        {
            gmsEneTStarWork.fSpd = 1f;
        }
        else
        {
            gmsEneTStarWork.fSpd = 0.0f;
            if (((int)eve_rec.flag & 1) != 0)
                gmsEneTStarWork.fSpd += 0.5f;
            if (((int)eve_rec.flag & 2) != 0)
                gmsEneTStarWork.fSpd += 0.25f;
            if (((int)eve_rec.flag & 4) != 0)
                gmsEneTStarWork.fSpd += 0.125f;
        }
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        AppMain.gmEneTStarWaitInit(work);
        AppMain.GmEneUtilInitNodeMatrix(gmsEneTStarWork.node_work, work, 10);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmEneTStarExit));
        AppMain.GmEneUtilGetNodeMatrix(gmsEneTStarWork.node_work, 4);
        AppMain.GmEneUtilGetNodeMatrix(gmsEneTStarWork.node_work, 5);
        AppMain.GmEneUtilGetNodeMatrix(gmsEneTStarWork.node_work, 6);
        AppMain.GmEneUtilGetNodeMatrix(gmsEneTStarWork.node_work, 7);
        AppMain.GmEneUtilGetNodeMatrix(gmsEneTStarWork.node_work, 8);
        ((AppMain.GMS_ENEMY_3D_WORK)work).ene_com.enemy_flag |= 32768U;
        work.scale.x = AppMain.FX_F32_TO_FX32(1.25f);
        work.scale.y = AppMain.FX_F32_TO_FX32(1.25f);
        work.scale.z = AppMain.FX_F32_TO_FX32(1.25f);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static AppMain.OBS_OBJECT_WORK GmEneTStarNeedleInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_T_STAR_WORK()), "ENE_T_STAR");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_T_STAR_WORK gmsEneTStarWork = (AppMain.GMS_ENE_T_STAR_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_t_star_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(682), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-11, (short)-12, (short)11, (short)12);
        pRec1.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec2, (short)-19, (short)-16, (short)19, (short)16);
        pRec2.flag &= 4294967291U;
        work.disp_flag |= 4194304U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneTStarNeedleMain);
        work.scale.x = AppMain.FX_F32_TO_FX32(1.25f);
        work.scale.y = AppMain.FX_F32_TO_FX32(1.25f);
        work.scale.z = AppMain.FX_F32_TO_FX32(1.25f);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static void gmEneTStarExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GmEneUtilExitNodeMatrix(((AppMain.GMS_ENE_T_STAR_WORK)AppMain.mtTaskGetTcbWork(tcb)).node_work);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    public static int gmEneTStarGetLength2N(AppMain.OBS_OBJECT_WORK obj_work)
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

    public static AppMain.VecFx32 gmEneTStarGetPlayerVectorFx(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        int num1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int num2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
        {
            num1 = 2965504;
            num2 = 2965504;
        }
        int denom = AppMain.FX_Sqrt(AppMain.FX_Mul(num1, num1) + AppMain.FX_Mul(num2, num2));
        if (denom == 0)
        {
            vecFx32.x = 0;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = AppMain.FX_Div(4096, denom);
            vecFx32.x = AppMain.FX_Mul(num1, v2);
            vecFx32.y = AppMain.FX_Mul(num2, v2);
        }
        vecFx32.z = 0;
        return vecFx32;
    }

    public static void gmEneTStarWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneTStarWaitMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
    }

    public static void gmEneTStarWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.gmEneTStarGetLength2N(obj_work) >= 16384)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneTStarWalkInit);
    }

    public static void gmEneTStarWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_ENE_T_STAR_WORK gmsEneTStarWork = (AppMain.GMS_ENE_T_STAR_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneTStarWalkMain);
        obj_work.move_flag &= 4294967291U;
        AppMain.VecFx32 playerVectorFx = AppMain.gmEneTStarGetPlayerVectorFx(obj_work);
        obj_work.spd.x = (int)((double)playerVectorFx.x * 0.5 * (double)gmsEneTStarWork.fSpd);
        obj_work.spd.y = (int)((double)playerVectorFx.y * 0.5 * (double)gmsEneTStarWork.fSpd);
        gmsEneTStarWork.timer = 120;
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        gmsEneTStarWork.rotate = (ushort)0;
    }

    public static void gmEneTStarWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_T_STAR_WORK gmsEneTStarWork = (AppMain.GMS_ENE_T_STAR_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        if (gmsEneTStarWork.rotate > (ushort)0)
        {
            obj_work.dir.z += (ushort)AppMain.AKM_DEGtoA16(10);
            --gmsEneTStarWork.rotate;
            if (gmsEneTStarWork.rotate == (ushort)0)
                obj_work.dir.z = (ushort)0;
        }
        if (gmsEneTStarWork.timer > 0)
        {
            --gmsEneTStarWork.timer;
            if (gmsEneTStarWork.timer != 60)
                return;
            gmsEneTStarWork.rotate = (ushort)36;
        }
        else
        {
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
            gmsEneTStarWork.timer = 15;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneTStarStopMain);
        }
    }

    public static void gmEneTStarStopMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_T_STAR_WORK gmsEneTStarWork = (AppMain.GMS_ENE_T_STAR_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        if (gmsEneTStarWork.timer > 0)
        {
            --gmsEneTStarWork.timer;
        }
        else
        {
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneTStarAttackInit);
            AppMain.GmEfctEneEsCreate(obj_work, 11);
        }
    }

    public static void gmEneTStarAttackInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_T_STAR_WORK gmsEneTStarWork = (AppMain.GMS_ENE_T_STAR_WORK)obj_work;
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMakeUnitMatrix(nnsMatrix1);
        AppMain.nnMakeUnitMatrix(nnsMatrix2);
        AppMain.nnMakeRotateZMatrix(nnsMatrix2, AppMain.AKM_DEGtoA32(72));
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        for (int index = 0; index < 5; ++index)
        {
            AppMain.OBS_OBJECT_WORK parent_obj = AppMain.GmEventMgrLocalEventBirth((ushort)308, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
            parent_obj.parent_obj = obj_work;
            parent_obj.dir.y = (ushort)49152;
            parent_obj.dir.z = (ushort)AppMain.AKM_DEGtoA16(-72 * index);
            nnsVector.x = nnsMatrix1.M01;
            nnsVector.y = nnsMatrix1.M11;
            nnsVector.z = 0.0f;
            parent_obj.spd.x = AppMain.FX_F32_TO_FX32(nnsVector.x * 4f);
            parent_obj.spd.y = -AppMain.FX_F32_TO_FX32(nnsVector.y * 4f);
            parent_obj.pos.x += AppMain.FX_F32_TO_FX32(nnsVector.x * 10f);
            parent_obj.pos.y += -AppMain.FX_F32_TO_FX32(nnsVector.y * 10f);
            AppMain.nnMultiplyMatrix(nnsMatrix1, nnsMatrix1, nnsMatrix2);
            ((AppMain.GMS_ENEMY_3D_WORK)parent_obj).ene_com.enemy_flag |= 32768U;
            AppMain.GmEfctEneEsCreate(parent_obj, 10).efct_com.obj_work.dir.z = (ushort)AppMain.AKM_DEGtoA16(-72 * index);
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        obj_work.disp_flag |= 32U;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneTStarAttackMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        gmsEneTStarWork.timer = 300;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.GmSoundPlaySE(AppMain.GMD_ENE_KAMA_SE_BOMB);
        gmsEnemy3DWork.ene_com.enemy_flag |= 65536U;
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix1);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix2);
    }

    public static void gmEneTStarAttackMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_T_STAR_WORK gmsEneTStarWork = (AppMain.GMS_ENE_T_STAR_WORK)obj_work;
        if (gmsEneTStarWork.timer > 0)
            --gmsEneTStarWork.timer;
        else
            obj_work.flag |= 8U;
    }

    public static void gmEneTStarNeedleMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)obj_work);
    }

    public static void gmEneTStarFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        AppMain.gmEneTStarFlipInit(obj_work);
    }

    public static void gmEneTStarFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneTStarFlipMain);
    }

    public static void gmEneTStarFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmEneTStarSetWalkSpeed((AppMain.GMS_ENE_T_STAR_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        AppMain.gmEneTStarWalkInit(obj_work);
    }

    public static int gmEneTStarSetWalkSpeed(AppMain.GMS_ENE_T_STAR_WORK t_star_work)
    {
        return 0;
    }


}