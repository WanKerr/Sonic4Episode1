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
    private static void GmEneUniuniBuild()
    {
        AppMain.gm_ene_uniuni_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(693)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(694)), 0U);
    }

    private static void GmEneUniuniFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(693));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_uniuni_obj_3d_list, amsAmbHeader.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmEneUniuniInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_UNIUNI_WORK()), "ENE_UNIUNI");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_UNIUNI_WORK gmsEneUniuniWork = (AppMain.GMS_ENE_UNIUNI_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_uniuni_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(695), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
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
        gmsEneUniuniWork.spd_dec = 76;
        gmsEneUniuniWork.spd_dec_dist = 15360;
        gmsEneUniuniWork.len = 17.5f;
        gmsEneUniuniWork.len_target = 35.5f;
        gmsEneUniuniWork.len_spd = 1f;
        gmsEneUniuniWork.rot_x = AppMain.AKM_DEGtoA32(90f);
        gmsEneUniuniWork.rot_y = AppMain.AKM_DEGtoA32(0.0f);
        gmsEneUniuniWork.rot_z = AppMain.AKM_DEGtoA32(0.0f);
        gmsEneUniuniWork.num = 0;
        AppMain.gmEneUniuniWalkInit(work);
        for (int index = 0; index < 4; ++index)
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)310, pos_x, pos_y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
            obsObjectWork.parent_obj = work;
            ((AppMain.GMS_ENE_UNIUNI_WORK)obsObjectWork).num = index;
            ++gmsEneUniuniWork.num;
        }
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmEneUniuniNeedleInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_UNIUNI_WORK()), "ENE_UNIUNI");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_UNIUNI_WORK gmsEneUniuniWork = (AppMain.GMS_ENE_UNIUNI_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_uniuni_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(695), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-4, (short)-4, (short)4, (short)4);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec2, (short)-19, (short)0, (short)19, (short)32);
        pRec2.flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        work.disp_flag |= 4194304U;
        AppMain.gmEneUniuniNeedleWaitInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static int gmEneUniuniGetLength2N(AppMain.OBS_OBJECT_WORK obj_work)
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

    private static void gmEneUniuniWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_UNIUNI_WORK gmsEneUniuniWork = (AppMain.GMS_ENE_UNIUNI_WORK)obj_work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUniuniWalkMain);
        obj_work.move_flag &= 4294967291U;
        gmsEneUniuniWork.timer = 1;
    }

    private static void gmEneUniuniWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_UNIUNI_WORK gmsEneUniuniWork = (AppMain.GMS_ENE_UNIUNI_WORK)obj_work;
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 1536 : -1536;
        if ((double)gmsEneUniuniWork.len_target == 17.5)
        {
            if (((int)obj_work.disp_flag & 1) != 0)
                gmsEneUniuniWork.rot_y += AppMain.AKM_DEGtoA32(1);
            else
                gmsEneUniuniWork.rot_y += AppMain.AKM_DEGtoA32(-1);
        }
        else
        {
            if (((int)obj_work.disp_flag & 1) != 0)
                gmsEneUniuniWork.rot_y += AppMain.AKM_DEGtoA32(0.5f);
            else
                gmsEneUniuniWork.rot_y += AppMain.AKM_DEGtoA32(-0.5f);
            obj_work.spd.x = 0;
        }
        if ((double)gmsEneUniuniWork.len_target > (double)gmsEneUniuniWork.len)
        {
            gmsEneUniuniWork.len += gmsEneUniuniWork.len_spd;
            if ((double)gmsEneUniuniWork.len_target <= (double)gmsEneUniuniWork.len)
                gmsEneUniuniWork.len = gmsEneUniuniWork.len_target;
            gmsEneUniuniWork.len_spd += 0.03f;
        }
        if ((double)gmsEneUniuniWork.len_target < (double)gmsEneUniuniWork.len)
        {
            gmsEneUniuniWork.len -= gmsEneUniuniWork.len_spd;
            if ((double)gmsEneUniuniWork.len_target >= (double)gmsEneUniuniWork.len)
                gmsEneUniuniWork.len = gmsEneUniuniWork.len_target;
            gmsEneUniuniWork.len_spd -= 0.05f;
            if ((double)gmsEneUniuniWork.len_spd < 0.100000001490116)
                gmsEneUniuniWork.len_spd = 0.1f;
        }
        if (gmsEneUniuniWork.timer > 0)
            --gmsEneUniuniWork.timer;
        else if ((double)gmsEneUniuniWork.len_target == 17.5)
        {
            gmsEneUniuniWork.timer = 120;
            gmsEneUniuniWork.len_spd = 0.0f;
            gmsEneUniuniWork.len_target = 35.5f;
        }
        else
        {
            if ((double)gmsEneUniuniWork.len_target != 35.5)
                return;
            gmsEneUniuniWork.timer = 120;
            gmsEneUniuniWork.len_spd = 1f;
            gmsEneUniuniWork.len_target = 17.5f;
        }
    }

    private static void gmEneUniuniFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        AppMain.gmEneUniuniFlipInit(obj_work);
    }

    private static void gmEneUniuniFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUniuniFlipMain);
    }

    private static void gmEneUniuniFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmEneUniuniSetWalkSpeed((AppMain.GMS_ENE_UNIUNI_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        AppMain.gmEneUniuniWalkInit(obj_work);
    }

    private static int gmEneUniuniSetWalkSpeed(AppMain.GMS_ENE_UNIUNI_WORK uniuni_work)
    {
        return 0;
    }

    private static void gmEneUniuniNeedleWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUniuniNeedleWaitMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
    }

    private static void gmEneUniuniNeedleWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_UNIUNI_WORK gmsEneUniuniWork = (AppMain.GMS_ENE_UNIUNI_WORK)obj_work;
        AppMain.GMS_ENE_UNIUNI_WORK parentObj = (AppMain.GMS_ENE_UNIUNI_WORK)obj_work.parent_obj;
        int rotY = parentObj.rot_y;
        int rotX = parentObj.rot_x;
        int rotZ = parentObj.rot_z;
        float len = parentObj.len;
        int ay = (rotY + AppMain.AKM_DEGtoA32(360) / 4 * gmsEneUniuniWork.num) % AppMain.AKM_DEGtoA32(360);
        AppMain.SNNS_MATRIX dst1;
        AppMain.nnMakeRotateXMatrix(out dst1, rotX);
        AppMain.nnRotateZMatrix(ref dst1, ref dst1, rotZ);
        AppMain.nnRotateYMatrix(ref dst1, ref dst1, ay);
        AppMain.SNNS_MATRIX dst2;
        AppMain.nnMakeTranslateMatrix(out dst2, len, 0.0f, 0.0f);
        AppMain.SNNS_MATRIX dst3;
        AppMain.nnMultiplyMatrix(out dst3, ref dst1, ref dst2);
        AppMain.SNNS_VECTOR dst4;
        AppMain.nnCopyMatrixTranslationVector(out dst4, ref dst3);
        obj_work.pos.x = AppMain.FX_F32_TO_FX32(dst4.x) + parentObj.ene_3d_work.ene_com.obj_work.pos.x;
        obj_work.pos.y = AppMain.FX_F32_TO_FX32(dst4.y) + parentObj.ene_3d_work.ene_com.obj_work.pos.y;
        obj_work.pos.z = 655360;
        if (parentObj.attack == 0 || (double)dst4.y < (double)len * 0.98)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUniuniNeedleAttackInit);
    }

    private static void gmEneUniuniNeedleAttackInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_UNIUNI_WORK parentObj = (AppMain.GMS_ENE_UNIUNI_WORK)obj_work.parent_obj;
        --parentObj.num;
        obj_work.spd.x = ((int)parentObj.ene_3d_work.ene_com.obj_work.disp_flag & 1) == 0 ? AppMain.FX_F32_TO_FX32(1f) : -AppMain.FX_F32_TO_FX32(1f);
        obj_work.parent_obj = (AppMain.OBS_OBJECT_WORK)null;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneUniuniNeedleAttackMain);
    }

    private static void gmEneUniuniNeedleAttackMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)obj_work);
    }

}