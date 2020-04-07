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
    private static AppMain.OBS_OBJECT_WORK GmGmkCannonInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_CANNON_WORK work = (AppMain.GMS_GMK_CANNON_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_CANNON_WORK()), "Gmk_Cannon");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_cannon_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = 131072;
        obj_work.pos.y -= 18432;
        obj_work.dir.y = (ushort)32768;
        obj_work.move_flag |= 256U;
        work.cannon_power = eve_rec.width == (byte)0 ? 61440 : (int)eve_rec.width;
        AppMain.gmGmkCannon_CreateParts(work);
        AppMain.gmGmkCannonStart(obj_work);
        return obj_work;
    }

    private static void gmGmkCannonFieldColOn(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_CANNON_WORK gmsGmkCannonWork = (AppMain.GMS_GMK_CANNON_WORK)obj_work;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.diff_data = AppMain.g_gm_default_col;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)24;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)56;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.ofst_x = (short)-12;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.ofst_y = (short)-30;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.flag |= 134217824U;
    }

    private static void gmGmkCannonFieldColOff(AppMain.OBS_OBJECT_WORK obj_work)
    {
        ((AppMain.GMS_GMK_CANNON_WORK)obj_work).gmk_work.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmGmkCannon_CannonTurn(AppMain.GMS_GMK_CANNON_WORK pwork)
    {
        if ((int)pwork.angle_set > (int)pwork.angle_now)
        {
            pwork.angle_now += (short)342;
            if ((int)pwork.angle_now <= (int)pwork.angle_set)
                return;
            pwork.angle_now = pwork.angle_set;
        }
        else
        {
            pwork.angle_now -= (short)342;
            if ((int)pwork.angle_now >= (int)pwork.angle_set)
                return;
            pwork.angle_now = pwork.angle_set;
        }
    }

    private static void gmGmkCannonStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_CANNON_WORK gmsGmkCannonWork = (AppMain.GMS_GMK_CANNON_WORK)obj_work;
        AppMain.gmGmkCannonFieldColOn(obj_work);
        gmsGmkCannonWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsGmkCannonWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsGmkCannonWork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkCannonHit);
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec, (short)-12, (short)-38, (short)12, (short)-6);
        obj_work.flag &= 4294967293U;
        gmsGmkCannonWork.ply_work = (AppMain.GMS_PLAYER_WORK)null;
        gmsGmkCannonWork.angle_set = (short)0;
        gmsGmkCannonWork.angle_now = (short)0;
        gmsGmkCannonWork.gmk_work.ene_com.enemy_flag &= 4294934527U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCannonStay);
    }

    private static void gmGmkCannonStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_CANNON_WORK gmsGmkCannonWork = (AppMain.GMS_GMK_CANNON_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.obj != null)
        {
            if (gmsPlayerWork.act_state == 31)
                AppMain.gmGmkCannonFieldColOff(obj_work);
        }
        else if (gmsPlayerWork.act_state != 31)
            AppMain.gmGmkCannonFieldColOn(obj_work);
        if (gmsGmkCannonWork.ply_work == null)
            return;
        gmsGmkCannonWork.gmk_work.ene_com.rect_work[2].flag &= 4294967291U;
        if (gmsPlayerWork.seq_state != 41)
        {
            AppMain.gmGmkCannonStart(obj_work);
        }
        else
        {
            if (obj_work.pos.y > gmsGmkCannonWork.ply_work.obj_work.pos.y)
                return;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCannonReady);
        }
    }

    private static short gmGmkCannon_GetAngle(ushort key)
    {
        if (((int)key & 8) != 0)
            return 2730;
        return ((int)key & 4) != 0 ? (short)-2730 : (short)0;
    }

    private static void gmGmkCannonReady(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_CANNON_WORK pwork = (AppMain.GMS_GMK_CANNON_WORK)obj_work;
        short angleSet = pwork.angle_set;
        short angleNow = pwork.angle_now;
        if ((int)pwork.angle_set == (int)pwork.angle_now)
        {
            if (((int)AppMain.g_gs_main_sys_info.game_flag & 1) == 0)
            {
                int num1 = (int)((double)AppMain._am_iphone_accel_data.sensor.x * 16384.0) * 3;
                if (num1 > 32768)
                    num1 = 32768;
                else if (num1 < (int)short.MinValue)
                    num1 = (int)short.MinValue;
                int num2 = num1 / 2;
                int num3 = num2 < (int)pwork.angle_now + 2730 ? (pwork.angle_now != (short)13650 || num2 < 16380 ? (num2 > (int)pwork.angle_now - 2730 ? (pwork.angle_now != (short)-13650 || num2 > -16380 ? (int)pwork.angle_now : -16380) : (int)pwork.angle_now - 2730) : 16380) : (int)pwork.angle_now + 2730;
                pwork.angle_set = (short)num3;
            }
            else
            {
                pwork.angle_set += AppMain.gmGmkCannon_GetAngle(pwork.ply_work.key_on);
                if (pwork.angle_set > (short)16380 && (ushort)pwork.angle_set < (ushort)49156)
                    pwork.angle_set = (short)16380;
                if (pwork.angle_set < (short)-16380 && (ushort)pwork.angle_set > (ushort)16380)
                    pwork.angle_set = (short)-16380;
            }
            if ((int)angleSet != (int)pwork.angle_set)
                AppMain.GmSoundPlaySE("Cannon1");
        }
        if ((int)pwork.angle_set != (int)pwork.angle_now)
        {
            AppMain.gmGmkCannon_CannonTurn(pwork);
            obj_work.dir.z = (ushort)pwork.angle_now;
        }
        if ((int)pwork.angle_set != (int)pwork.angle_now || (int)angleNow != (int)pwork.angle_now || !AppMain.GmPlayerKeyCheckJumpKeyPush(pwork.ply_work))
            return;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate(pwork.ply_work.obj_work, 20);
        gmsEffect3DesWork.efct_com.obj_work.dir.z = obj_work.dir.z;
        gmsEffect3DesWork.efct_com.obj_work.pos.x += AppMain.mtMathSin((int)obj_work.dir.z) * 32;
        gmsEffect3DesWork.efct_com.obj_work.pos.y -= AppMain.mtMathCos((int)obj_work.dir.z) * 32;
        AppMain.GmPlySeqInitCannonShoot(pwork.ply_work, AppMain.mtMathCos((int)obj_work.dir.z - 16384) * pwork.cannon_power, AppMain.mtMathSin((int)obj_work.dir.z - 16384) * pwork.cannon_power);
        AppMain.gmGmkCannonFieldColOff(obj_work);
        pwork.gmk_work.ene_com.enemy_flag |= 32768U;
        pwork.shoot_after = (short)0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCannonShoot);
        AppMain.gmGmkCannonShoot(obj_work);
        AppMain.GmSoundPlaySE("Cannon2");
        AppMain.GMM_PAD_VIB_SMALL();
    }

    private static void gmGmkCannonShoot(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_CANNON_WORK gmsGmkCannonWork = (AppMain.GMS_GMK_CANNON_WORK)obj_work;
        ++gmsGmkCannonWork.shoot_after;
        if (gmsGmkCannonWork.shoot_after == (short)16)
        {
            AppMain.gmGmkCannonFieldColOn(obj_work);
            if (gmsGmkCannonWork.angle_now == (short)0)
            {
                gmsGmkCannonWork.ply_work = (AppMain.GMS_PLAYER_WORK)null;
                AppMain.gmGmkCannonStart(obj_work);
                return;
            }
        }
        if (gmsGmkCannonWork.shoot_after <= (short)32)
            return;
        gmsGmkCannonWork.ply_work = (AppMain.GMS_PLAYER_WORK)null;
        gmsGmkCannonWork.angle_set = (short)0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCannonShootEnd);
    }

    private static void gmGmkCannonShootEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_CANNON_WORK pwork = (AppMain.GMS_GMK_CANNON_WORK)obj_work;
        AppMain.gmGmkCannon_CannonTurn(pwork);
        obj_work.dir.z = (ushort)pwork.angle_now;
        if ((int)pwork.angle_now != (int)pwork.angle_set)
            return;
        AppMain.gmGmkCannonStart(obj_work);
    }

    private static void gmGmkCannonHit(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        AppMain.GMS_GMK_CANNON_WORK gmsGmkCannonWork = (AppMain.GMS_GMK_CANNON_WORK)parentObj1;
        gmsGmkCannonWork.hitpass = false;
        if (parentObj2 == AppMain.g_gm_main_system.ply_work[0])
        {
            if (gmsGmkCannonWork.ply_work != parentObj2)
            {
                short num1 = (short)((parentObj1.pos.y >> 12) + (int)mine_rect.rect.top);
                if (parentObj2.obj_work.pos.y >> 12 < (int)num1 && parentObj2.obj_work.move.y >= 0 || parentObj2.act_state == 31)
                {
                    short num2 = 1;
                    short num3 = (short)(AppMain.MTM_MATH_ABS((int)mine_rect.rect.left - (int)match_rect.rect.left) + AppMain.MTM_MATH_ABS((int)mine_rect.rect.right - (int)match_rect.rect.right));
                    short num4 = (short)AppMain.MTM_MATH_ABS(parentObj2.obj_work.move.x >> 12);
                    if (num4 != (short)0)
                        num2 = (short)((int)num4 / (int)num3 + 1);
                    if (parentObj2.obj_work.move.x < 0)
                        num3 = (short)-num3;
                    short num5 = (short)((parentObj1.pos.x >> 12) + (int)mine_rect.rect.left - (int)match_rect.rect.left);
                    short num6 = (short)((parentObj1.pos.x >> 12) + (int)mine_rect.rect.right - (int)match_rect.rect.right);
                    short num7 = (short)(parentObj2.obj_work.pos.x >> 12);
                    for (; num2 != (short)0; --num2)
                    {
                        if ((int)num7 >= (int)num5 && (int)num7 <= (int)num6)
                        {
                            gmsGmkCannonWork.ply_work = parentObj2;
                            AppMain.GmPlySeqInitCannon(parentObj2, (AppMain.GMS_ENEMY_COM_WORK)parentObj1);
                            AppMain.GmSoundPlaySE("Cannon3");
                            break;
                        }
                        num7 += num3;
                    }
                }
            }
            gmsGmkCannonWork.hitpass = true;
        }
        mine_rect.flag &= 4294573823U;
    }

    private static void gmGmkCannon_CreateParts(AppMain.GMS_GMK_CANNON_WORK pwork)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)pwork;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_CANNONPARTS_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "Gmk_CannonBase");
        AppMain.GMS_GMK_CANNONPARTS_WORK gmkCannonpartsWork = (AppMain.GMS_GMK_CANNONPARTS_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_cannon_obj_3d_list[1], gmkCannonpartsWork.eff_work.obj_3d);
        work.parent_obj = obsObjectWork;
        work.flag &= 4294966271U;
        work.pos.x = obsObjectWork.pos.x;
        work.pos.y = obsObjectWork.pos.y + 122880;
        work.pos.z = obsObjectWork.pos.z + 122880;
        work.dir.y = obsObjectWork.dir.y;
        work.move_flag |= 256U;
        work.disp_flag &= 4294967039U;
        work.flag |= 2U;
        work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void GmGmkCannonBuild()
    {
        AppMain.gm_gmk_cannon_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(858), AppMain.GmGameDatGetGimmickData(859), 0U);
    }

    private static void GmGmkCannonFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(858);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_cannon_obj_3d_list, gimmickData.file_num);
    }

}