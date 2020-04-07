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
    private static void GmEneUnidesBuild()
    {
        AppMain.gm_ene_unides_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(690)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(691)), 0U);
    }

    private static void GmEneUnidesFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(690));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_unides_obj_3d_list, amsAmbHeader.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmEneUnidesInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_UNIDES_WORK()), "ENE_UNIDES");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (AppMain.GMS_ENE_UNIDES_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_unides_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(692), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-8, (short)0, (short)8, (short)16);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        AppMain.ObjRectWorkSet(pRec2, (short)-16, (short)-8, (short)16, (short)16);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-19, (short)-16, (short)19, (short)16);
        pRec3.flag &= 4294967291U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        if (((int)eve_rec.flag & 1) == 0)
        {
            work.disp_flag |= 1U;
            work.dir.y = (ushort)AppMain.AKM_DEGtoA16(45);
        }
        else
            work.dir.y = (ushort)AppMain.AKM_DEGtoA16(-45);
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        gmsEneUnidesWork.spd_dec = 76;
        gmsEneUnidesWork.spd_dec_dist = 15360;
        gmsEneUnidesWork.len = 17.5f;
        gmsEneUnidesWork.rot_x = AppMain.AKM_DEGtoA32(90f);
        gmsEneUnidesWork.rot_y = AppMain.AKM_DEGtoA32(0.0f);
        gmsEneUnidesWork.rot_z = AppMain.AKM_DEGtoA32(0.0f);
        gmsEneUnidesWork.num = 0;
        AppMain.gmEneUnidesWaitInit(work);
        for (int index = 0; index < 4; ++index)
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)309, pos_x, pos_y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
            obsObjectWork.parent_obj = work;
            ((AppMain.GMS_ENE_UNIDES_WORK)obsObjectWork).num = index;
            ++gmsEneUnidesWork.num;
        }
        gmsEneUnidesWork.attack = 0;
        gmsEneUnidesWork.attack_first = 0;
        gmsEneUnidesWork.zoom_now = 0;
        gmsEneUnidesWork.zoom = 1f;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmEneUnidesNeedleInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_UNIDES_WORK()), "ENE_UNIDES");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (AppMain.GMS_ENE_UNIDES_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_unides_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(692), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-6, (short)-4, (short)6, (short)8);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec2, (short)-19, (short)0, (short)19, (short)32);
        pRec2.flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        work.disp_flag |= 4194304U;
        AppMain.gmEneUnidesNeedleWaitInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static int gmEneUnidesGetLength2N(AppMain.OBS_OBJECT_WORK obj_work)
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

    private static void gmEneUnidesWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUnidesWaitMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
    }

    private static void gmEneUnidesWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (AppMain.GMS_ENE_UNIDES_WORK)obj_work;
        if (AppMain.gmEneUnidesGetLength2N(obj_work) < 9216)
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUnidesAttackInit);
        if (((int)obj_work.disp_flag & 1) != 0)
            gmsEneUnidesWork.rot_y += AppMain.AKM_DEGtoA32(1);
        else
            gmsEneUnidesWork.rot_y += AppMain.AKM_DEGtoA32(-1);
    }

    private static void gmEneUnidesWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (AppMain.GMS_ENE_UNIDES_WORK)obj_work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUnidesWalkMain);
        obj_work.move_flag &= 4294967291U;
        gmsEneUnidesWork.timer = 60;
    }

    private static void gmEneUnidesWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (AppMain.GMS_ENE_UNIDES_WORK)obj_work;
        if (gmsEneUnidesWork.timer > 0)
            --gmsEneUnidesWork.timer;
        else if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -1536;
        else
            obj_work.spd.x = 1536;
    }

    private static void gmEneUnidesFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        AppMain.gmEneUnidesFlipInit(obj_work);
    }

    private static void gmEneUnidesFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUnidesFlipMain);
    }

    private static void gmEneUnidesFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmEneUnidesSetWalkSpeed((AppMain.GMS_ENE_UNIDES_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        AppMain.gmEneUnidesWalkInit(obj_work);
    }

    private static void gmEneUnidesAttackInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUnidesAttackMain);
        obj_work.move_flag &= 4294967291U;
    }

    private static void gmEneUnidesAttackMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (AppMain.GMS_ENE_UNIDES_WORK)obj_work;
        if (gmsEneUnidesWork.stop == 0)
        {
            if (((int)obj_work.disp_flag & 1) != 0)
                gmsEneUnidesWork.rot_y += AppMain.AKM_DEGtoA32(1);
            else
                gmsEneUnidesWork.rot_y += AppMain.AKM_DEGtoA32(-1);
        }
        gmsEneUnidesWork.attack = 1;
        if (gmsEneUnidesWork.zoom_now == 1)
        {
            gmsEneUnidesWork.zoom += 0.07f;
            if ((double)gmsEneUnidesWork.zoom > 1.39999997615814)
                gmsEneUnidesWork.zoom_now = 2;
        }
        else if (gmsEneUnidesWork.zoom_now >= 2 && gmsEneUnidesWork.zoom_now <= 12)
        {
            ++gmsEneUnidesWork.zoom_now;
            gmsEneUnidesWork.zoom -= 0.07f;
        }
        else if (gmsEneUnidesWork.zoom_now >= 13 && gmsEneUnidesWork.zoom_now <= 23)
        {
            ++gmsEneUnidesWork.zoom_now;
            gmsEneUnidesWork.zoom += 0.07f;
        }
        else if ((double)gmsEneUnidesWork.zoom > 1.0)
        {
            gmsEneUnidesWork.zoom -= 0.07f;
            if ((double)gmsEneUnidesWork.zoom <= 1.0)
            {
                gmsEneUnidesWork.zoom = 1f;
                gmsEneUnidesWork.stop = 0;
                gmsEneUnidesWork.zoom_now = 0;
            }
        }
        obj_work.scale.x = AppMain.FX_F32_TO_FX32(gmsEneUnidesWork.zoom);
        obj_work.scale.y = AppMain.FX_F32_TO_FX32(gmsEneUnidesWork.zoom);
        obj_work.scale.z = AppMain.FX_F32_TO_FX32(gmsEneUnidesWork.zoom);
        if (gmsEneUnidesWork.num != 0 || (double)gmsEneUnidesWork.zoom != 1.0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUnidesWalkInit);
    }

    private static int gmEneUnidesSetWalkSpeed(AppMain.GMS_ENE_UNIDES_WORK unides_work)
    {
        return 0;
    }

    private static void gmEneUnidesNeedleWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUnidesNeedleWaitMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
    }

    private static void gmEneUnidesNeedleWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (AppMain.GMS_ENE_UNIDES_WORK)obj_work;
        AppMain.GMS_ENE_UNIDES_WORK parentObj = (AppMain.GMS_ENE_UNIDES_WORK)obj_work.parent_obj;
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix3 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        int rotY = parentObj.rot_y;
        int rotX = parentObj.rot_x;
        int rotZ = parentObj.rot_z;
        float len = parentObj.len;
        int ay = (rotY + AppMain.AKM_DEGtoA32(360) / 4 * gmsEneUnidesWork.num) % AppMain.AKM_DEGtoA32(360);
        AppMain.nnMakeRotateXMatrix(nnsMatrix1, rotX);
        AppMain.nnRotateZMatrix(nnsMatrix1, nnsMatrix1, rotZ);
        AppMain.nnRotateYMatrix(nnsMatrix1, nnsMatrix1, ay);
        AppMain.nnMakeTranslateMatrix(nnsMatrix2, len, 0.0f, 0.0f);
        AppMain.nnMultiplyMatrix(nnsMatrix3, nnsMatrix1, nnsMatrix2);
        AppMain.SNNS_VECTOR dst;
        AppMain.nnCopyMatrixTranslationVector(out dst, nnsMatrix3);
        obj_work.pos.x = AppMain.FX_F32_TO_FX32(dst.x) + parentObj.ene_3d_work.ene_com.obj_work.pos.x;
        obj_work.pos.y = AppMain.FX_F32_TO_FX32(dst.y) + parentObj.ene_3d_work.ene_com.obj_work.pos.y;
        obj_work.pos.z = 655360;
        if (parentObj.attack != 0 && (double)dst.y >= (double)len * 0.98 && parentObj.stop == 0)
        {
            if (parentObj.attack_first != 0)
            {
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUnidesNeedleAttackInit);
            }
            else
            {
                parentObj.zoom_now = 1;
                parentObj.attack_first = 1;
                parentObj.stop = 1;
            }
        }
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix1);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix2);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix3);
    }

    private static void gmEneUnidesNeedleAttackInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_UNIDES_WORK parentObj = (AppMain.GMS_ENE_UNIDES_WORK)obj_work.parent_obj;
        --parentObj.num;
        obj_work.spd.x = ((int)parentObj.ene_3d_work.ene_com.obj_work.disp_flag & 1) == 0 ? AppMain.FX_F32_TO_FX32(1f) : -AppMain.FX_F32_TO_FX32(1f);
        obj_work.parent_obj = (AppMain.OBS_OBJECT_WORK)null;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUnidesNeedleAttackMain);
    }

    private static void gmEneUnidesNeedleAttackMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)obj_work);
    }


}