using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    public static void GmEneHaroBuild()
    {
        AppMain.gm_ene_haro_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(687)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(688)), 0U);
    }

    public static void GmEneHaroFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(687));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_haro_obj_3d_list, amsAmbHeader.file_num);
    }

    public static AppMain.OBS_OBJECT_WORK GmEneHaroInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_HARO_WORK()), "ENE_HARO");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_HARO_WORK gmsEneHaroWork = (AppMain.GMS_ENE_HARO_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_haro_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(689), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        work.disp_flag |= 4194304U;
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
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        if (((int)eve_rec.flag & (int)AppMain.GMD_ENE_HARO_EVE_FLAG_RIGHT) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        gmsEneHaroWork.spd_dec = (int)AppMain.GMD_ENE_HARO_MOVE_SPD_X / (AppMain.GMD_ENE_HARO_TURN_FRAME / 2);
        gmsEneHaroWork.spd_dec_dist = (int)AppMain.GMD_ENE_HARO_MOVE_SPD_X * (AppMain.GMD_ENE_HARO_TURN_FRAME / 2) / 2;
        gmsEneHaroWork.vec.x = 0;
        gmsEneHaroWork.vec.y = AppMain.FX_F32_TO_FX32(1.0);
        gmsEneHaroWork.angle = 0;
        gmsEneHaroWork.angle_add = 0;
        gmsEneHaroWork.lighton = 0;
        AppMain.GmEneUtilInitNodeMatrix(gmsEneHaroWork.node_work, work, 16);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmEneHaroExit));
        AppMain.GmEneUtilGetNodeMatrix(gmsEneHaroWork.node_work, 2);
        AppMain.gmEneHaroWaitInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static void gmEneHaroExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GmEneUtilExitNodeMatrix(((AppMain.GMS_ENE_HARO_WORK)AppMain.mtTaskGetTcbWork(tcb)).node_work);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    public static int gmEneHaroGetLength2N(AppMain.OBS_OBJECT_WORK obj_work)
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

    public static int gmEneHaroIsPlayerLeft(AppMain.GMS_ENE_HARO_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_main_system.ply_work[0];
        if (((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
            return 1;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        vecFx32.x = obsObjectWork.pos.x - obj_work.ene_3d_work.ene_com.obj_work.pos.x;
        vecFx32.y = obsObjectWork.pos.y - obj_work.ene_3d_work.ene_com.obj_work.pos.y;
        return AppMain.FX_Mul(vecFx32.x, obj_work.vec.y) - AppMain.FX_Mul(vecFx32.y, obj_work.vec.x) > 0 ? 0 : 1;
    }

    public static int gmEneHaroIsPlayerCenter(AppMain.GMS_ENE_HARO_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_main_system.ply_work[0];
        if (((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
            return 1;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        vecFx32.x = obsObjectWork.pos.x - obj_work.ene_3d_work.ene_com.obj_work.pos.x;
        vecFx32.y = obsObjectWork.pos.y - obj_work.ene_3d_work.ene_com.obj_work.pos.y;
        int num = AppMain.FX_Mul(vecFx32.x, obj_work.vec.y) - AppMain.FX_Mul(vecFx32.y, obj_work.vec.x);
        return num < AppMain.FX_F32_TO_FX32(0.2f) && num > -AppMain.FX_F32_TO_FX32(0.2f) ? 1 : 0;
    }

    public static int gmEneHaroIsPlayerFront(AppMain.GMS_ENE_HARO_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_main_system.ply_work[0];
        if (((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
            return 1;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        vecFx32.x = obsObjectWork.pos.x - obj_work.ene_3d_work.ene_com.obj_work.pos.x;
        vecFx32.y = obsObjectWork.pos.y - obj_work.ene_3d_work.ene_com.obj_work.pos.y;
        return AppMain.FX_Mul(vecFx32.x, obj_work.vec.x) + AppMain.FX_Mul(vecFx32.y, obj_work.vec.y) > 0 ? 1 : 0;
    }

    public static void gmEneHaroWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_HARO_WORK gmsEneHaroWork = (AppMain.GMS_ENE_HARO_WORK)obj_work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet(obj_work, 0);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneHaroWaitMain);
        obj_work.move_flag &= 4294967291U;
        int denom = AppMain.FX_Sqrt(AppMain.FX_Mul(gmsEneHaroWork.vec.x, gmsEneHaroWork.vec.x) + AppMain.FX_Mul(gmsEneHaroWork.vec.y, gmsEneHaroWork.vec.y));
        if (denom == 0)
        {
            gmsEneHaroWork.vec.x = 0;
            gmsEneHaroWork.vec.y = AppMain.FX_F32_TO_FX32(1f);
        }
        else
        {
            gmsEneHaroWork.vec.x = AppMain.FX_Div(gmsEneHaroWork.vec.x, denom);
            gmsEneHaroWork.vec.y = AppMain.FX_Div(gmsEneHaroWork.vec.y, denom);
        }
    }

    public static void gmEneHaroWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.gmEneHaroGetLength2N(obj_work) > 10000)
            return;
        AppMain.GmSoundPlaySE("Halogen");
        obj_work.obj_3d.blend_spd = 0.05f;
        AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 1);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneHaroWalkInit);
    }

    public static void gmEneHaroWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_HARO_WORK gmsEneHaroWork = (AppMain.GMS_ENE_HARO_WORK)obj_work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneHaroWalkMain);
        obj_work.move_flag &= 4294967291U;
        int denom = AppMain.FX_Sqrt(AppMain.FX_Mul(gmsEneHaroWork.vec.x, gmsEneHaroWork.vec.x) + AppMain.FX_Mul(gmsEneHaroWork.vec.y, gmsEneHaroWork.vec.y));
        if (denom == 0)
        {
            gmsEneHaroWork.vec.x = 0;
            gmsEneHaroWork.vec.y = AppMain.FX_F32_TO_FX32(1f);
        }
        else
        {
            gmsEneHaroWork.vec.x = AppMain.FX_Div(gmsEneHaroWork.vec.x, denom);
            gmsEneHaroWork.vec.y = AppMain.FX_Div(gmsEneHaroWork.vec.y, denom);
        }
        gmsEneHaroWork.timer = 120;
        if (gmsEneHaroWork.lighton == 0)
        {
            AppMain.GmEfctEneEsCreate(obj_work, 6).efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneEffectMainFuncHarogen);
            gmsEneHaroWork.lighton = 1;
        }
        if (AppMain.gmEneHaroGetLength2N(obj_work) <= 10000)
        {
            AppMain.GmSoundPlaySE("Halogen");
            AppMain.ObjDrawObjectActionSet(obj_work, 1);
            obj_work.disp_flag |= 4U;
        }
        else
        {
            AppMain.ObjDrawObjectActionSet(obj_work, 1);
            obj_work.disp_flag |= 4U;
        }
    }

    public static void gmEneHaroWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_HARO_WORK obj_work1 = (AppMain.GMS_ENE_HARO_WORK)obj_work;
        if (AppMain.gmEneHaroIsPlayerCenter(obj_work1) == 0)
        {
            if (AppMain.gmEneHaroIsPlayerLeft(obj_work1) != 0)
            {
                obj_work1.angle_add -= AppMain.AKM_DEGtoA32(0.03f);
                if (obj_work1.angle_add < -AppMain.AKM_DEGtoA32(0.35f))
                    obj_work1.angle_add = -AppMain.AKM_DEGtoA32(0.35f);
                obj_work1.angle += obj_work1.angle_add;
            }
            else
            {
                obj_work1.angle_add += AppMain.AKM_DEGtoA32(0.03f);
                if (obj_work1.angle_add > AppMain.AKM_DEGtoA32(0.35f))
                    obj_work1.angle_add = AppMain.AKM_DEGtoA32(0.35f);
                obj_work1.angle += obj_work1.angle_add;
            }
            if (obj_work1.angle < -AppMain.AKM_DEGtoA32(1.3f))
                obj_work1.angle = -AppMain.AKM_DEGtoA32(1.3f);
            if (obj_work1.angle > AppMain.AKM_DEGtoA32(1.3f))
                obj_work1.angle = AppMain.AKM_DEGtoA32(1.3f);
        }
        int v2_1 = AppMain.FX_Cos(obj_work1.angle);
        int v2_2 = AppMain.FX_Sin(obj_work1.angle);
        obj_work1.vec.x = AppMain.FX_Mul(obj_work1.vec.x, v2_1) + AppMain.FX_Mul(obj_work1.vec.y, v2_2);
        obj_work1.vec.y = AppMain.FX_Mul(obj_work1.vec.x, -v2_2) + AppMain.FX_Mul(obj_work1.vec.y, v2_1);
        obj_work1.vvv.x = (int)((double)obj_work1.vvv.x * 0.959999978542328);
        obj_work1.vvv.y = (int)((double)obj_work1.vvv.x * 0.959999978542328);
        obj_work1.vvv.x += obj_work1.vec.x;
        obj_work1.vvv.y += obj_work1.vec.y;
        obj_work1.spd = AppMain.FX_F32_TO_FX32(1.5f);
        obj_work.spd.x = AppMain.FX_Mul(obj_work1.vec.x, obj_work1.spd);
        obj_work.spd.y = AppMain.FX_Mul(obj_work1.vec.y, obj_work1.spd);
        obj_work.spd.x += AppMain.FX_Mul(obj_work1.vvv.x, AppMain.FX_F32_TO_FX32(0.025));
        obj_work.spd.y += AppMain.FX_Mul(obj_work1.vvv.y, AppMain.FX_F32_TO_FX32(0.025));
        if (obj_work1.timer > 0)
            --obj_work1.timer;
        else
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneHaroWalkInit);
        if (obj_work1.vec.x < 0)
        {
            obj_work.disp_flag &= 4294967294U;
            obj_work1.targetAngle = AppMain.AKM_DEGtoA32(250);
        }
        else
        {
            obj_work.disp_flag &= 4294967294U;
            obj_work1.targetAngle = AppMain.AKM_DEGtoA32(330);
        }
        if ((int)obj_work.dir.y > obj_work1.targetAngle)
            obj_work.dir.y -= (ushort)AppMain.AKM_DEGtoA32(5);
        if ((int)obj_work.dir.y >= obj_work1.targetAngle)
            return;
        obj_work.dir.y += (ushort)AppMain.AKM_DEGtoA32(5);
    }

    public static void gmEneHaroFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        AppMain.gmEneHaroFlipInit(obj_work);
    }

    public static void gmEneHaroFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneHaroFlipMain);
    }

    public static void gmEneHaroFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmEneHaroSetWalkSpeed((AppMain.GMS_ENE_HARO_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        AppMain.gmEneHaroWalkInit(obj_work);
    }

    public static int gmEneHaroSetWalkSpeed(AppMain.GMS_ENE_HARO_WORK haro_work)
    {
        return 0;
    }

    public static void gmEneEffectMainFuncHarogen(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.parent_obj != null)
        {
            AppMain.GMS_ENE_HARO_WORK parentObj = (AppMain.GMS_ENE_HARO_WORK)obj_work.parent_obj;
            AppMain.NNS_MATRIX nodeMatrix = AppMain.GmEneUtilGetNodeMatrix(parentObj.node_work, 2);
            if (nodeMatrix != null)
            {
                float num1 = nodeMatrix.M03 - AppMain.FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.pos.x);
                float num2 = nodeMatrix.M13 + AppMain.FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.pos.y);
                float num3 = nodeMatrix.M23 - AppMain.FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.pos.z);
                float x1 = num1 + AppMain.FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.spd.x);
                float num4 = num2 - AppMain.FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.spd.y);
                float x2 = num3 + AppMain.FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.spd.z);
                obj_work.parent_ofst.x = AppMain.FX_F32_TO_FX32(x1);
                obj_work.parent_ofst.y = -AppMain.FX_F32_TO_FX32(num4 - 10f);
                obj_work.parent_ofst.z = AppMain.FX_F32_TO_FX32(x2);
            }
        }
        else
            obj_work.flag |= 4U;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }
}