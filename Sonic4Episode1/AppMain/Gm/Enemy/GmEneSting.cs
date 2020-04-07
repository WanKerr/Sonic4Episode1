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
    private static AppMain.OBS_OBJECT_WORK GmEneStingInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_STING_WORK()), "ENE_STING");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_STING_WORK sting_work = (AppMain.GMS_ENE_STING_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_sting_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, false, AppMain.ObjDataGet(669), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        gmsEnemy3DWork.obj_3d.mtn_cb_func = new AppMain.mtn_cb_func_delegate(AppMain.gmEneStingMotionCallback);
        gmsEnemy3DWork.obj_3d.mtn_cb_param = (object)sting_work;
        work.pos.z = 0;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-10, (short)-8, (short)20, (short)8);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        AppMain.ObjRectWorkSet(pRec2, (short)-18, (short)-16, (short)28, (short)16);
        pRec2.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-18, (short)-16, (short)28, (short)16);
        pRec3.flag &= 4294967291U;
        AppMain.OBS_RECT_WORK searchRectWork = sting_work.search_rect_work;
        searchRectWork.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmEneStingSearchDefFunc);
        AppMain.ObjRectGroupSet(searchRectWork, (byte)1, (byte)1);
        AppMain.ObjRectAtkSet(searchRectWork, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(searchRectWork, (ushort)65534, (short)0);
        searchRectWork.parent_obj = work;
        AppMain.ObjRectWorkSet(searchRectWork, (short)0, (short)0, (short)128, (short)128);
        searchRectWork.flag |= 1048804U;
        work.ppRec = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneStingRegRectFunc);
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        if (((int)eve_rec.flag & (int)AppMain.GMD_ENE_STING_EVE_FLAG_RIGHT) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        sting_work.spd_dec = (int)AppMain.GMD_ENE_STING_SPD_X / (AppMain.GMD_ENE_STING_TURN_FRAME / 2);
        sting_work.spd_dec_dist = (int)AppMain.GMD_ENE_STING_SPD_X * (AppMain.GMD_ENE_STING_TURN_FRAME / 2) / 2;
        AppMain.gmEneStingWalkInit(work);
        AppMain.gmEneStingCreateJetEfct(sting_work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static int gmEneStingSetWalkSpeed(AppMain.GMS_ENE_STING_WORK sting_work)
    {
        int num = 0;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)sting_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 3 && (double)obsObjectWork.obj_3d.frame[0] >= (double)(AppMain.GMD_ENE_STING_TURN_FRAME / 2))
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, sting_work.spd_dec, (int)AppMain.GMD_ENE_STING_SPD_X);
            else if (obsObjectWork.pos.x <= Convert.ToInt32(obsObjectWork.user_work) + sting_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, sting_work.spd_dec);
                num = 1;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > (int)obsObjectWork.user_work)
                {
                    obsObjectWork.spd.x = (int)obsObjectWork.user_work - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -sting_work.spd_dec)
                        obsObjectWork.spd.x = -sting_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > (int)-AppMain.GMD_ENE_STING_SPD_X)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -sting_work.spd_dec, (int)AppMain.GMD_ENE_STING_SPD_X);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 2 && (double)obsObjectWork.obj_3d.frame[0] >= (double)(AppMain.GMD_ENE_STING_TURN_FRAME / 2))
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -sting_work.spd_dec, (int)AppMain.GMD_ENE_STING_SPD_X);
        else if ((long)obsObjectWork.pos.x >= (long)Convert.ToUInt32(obsObjectWork.user_flag) - (long)sting_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, sting_work.spd_dec);
            num = 1;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < Convert.ToInt32(obsObjectWork.user_flag))
            {
                obsObjectWork.spd.x = Convert.ToInt32(obsObjectWork.user_flag) - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > sting_work.spd_dec)
                    obsObjectWork.spd.x = sting_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < (int)AppMain.GMD_ENE_STING_SPD_X)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, sting_work.spd_dec, (int)AppMain.GMD_ENE_STING_SPD_X);
        return num;
    }

    public static void GmEneStingBuild()
    {
        AppMain.gm_ene_sting_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(667)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(668)), 0U);
    }

    public static void GmEneStingFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(667));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_sting_obj_3d_list, amsAmbHeader.file_num);
    }

    public static void GmEneStingCreateBullet(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int ofst_flash_x,
      int ofst_flash_y,
      int ofst_flash_z,
      int ofst_bul_x,
      int ofst_bul_y,
      int ofst_bul_z,
      int spd_x,
      int spd_y,
      short dir)
    {
        AppMain.GMS_EFFECT_COM_WORK atkObject = (AppMain.GMS_EFFECT_COM_WORK)AppMain.GmEneComCreateAtkObject(parent_obj, (short)16);
        atkObject.obj_work.parent_obj = (AppMain.OBS_OBJECT_WORK)null;
        atkObject.obj_work.pos.x += ((int)parent_obj.disp_flag & 1) != 0 ? -ofst_bul_x : ofst_bul_x;
        atkObject.obj_work.pos.y += ofst_bul_y;
        atkObject.obj_work.pos.z += ofst_bul_z;
        AppMain.OBS_RECT_WORK pRec = atkObject.rect_work[1];
        AppMain.ObjRectWorkSet(pRec, (short)-8, (short)-8, (short)8, (short)8);
        pRec.flag |= 4U;
        atkObject.obj_work.spd.x = spd_x;
        atkObject.obj_work.spd.y = spd_y;
        atkObject.obj_work.view_out_ofst = (short)16;
        AppMain.GMS_EFFECT_3DES_WORK efct_work1 = AppMain.GmEfctCmnEsCreate(atkObject.obj_work, 15);
        AppMain.GmComEfctSetDispRotationS(efct_work1, (short)0, (short)0, dir);
        efct_work1.efct_com.obj_work.flag |= 1024U;
        AppMain.GMS_EFFECT_3DES_WORK efct_work2 = AppMain.GmEfctCmnEsCreate(parent_obj, 14);
        AppMain.GmComEfctSetDispRotationS(efct_work2, (short)0, (short)0, (short)((int)dir - 32768));
        efct_work2.efct_com.obj_work.parent_obj = (AppMain.OBS_OBJECT_WORK)null;
        efct_work2.efct_com.obj_work.pos.x += ((int)parent_obj.disp_flag & 1) != 0 ? -ofst_flash_x : ofst_flash_x;
        efct_work2.efct_com.obj_work.pos.y += ofst_flash_y;
        efct_work2.efct_com.obj_work.pos.z += ofst_flash_z;
    }

    public static void gmEneStingJetEfctMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.NNS_MATRIX userWorkObject = (AppMain.NNS_MATRIX)obj_work.user_work_OBJECT;
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        if (obj_work.parent_obj == null)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            nnsVector.x = userWorkObject.M03 - AppMain.FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.x);
            nnsVector.y = -userWorkObject.M13 - AppMain.FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.y);
            nnsVector.z = userWorkObject.M23 - AppMain.FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.z);
            if (((int)obj_work.parent_obj.disp_flag & 1) != 0)
            {
                nnsVector.x = -nnsVector.x;
                nnsVector.z = -nnsVector.z;
            }
            nnsVector.x += -3f;
            AppMain.GmComEfctSetDispOffsetF((AppMain.GMS_EFFECT_3DES_WORK)obj_work, nnsVector.x, nnsVector.y, nnsVector.z);
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        }
    }

    public static void gmEneStingWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 0, 1);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneStingWalkMain);
    }

    public static void gmEneStingWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.gmEneStingSetWalkSpeed((AppMain.GMS_ENE_STING_WORK)obj_work) == 0)
            return;
        AppMain.gmEneStingFlipInit(obj_work);
    }

    public static void gmEneStingFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_STING_WORK sting_work = (AppMain.GMS_ENE_STING_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 2, 3);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneStingFlipMain);
        sting_work.search_rect_work.flag &= 4294967291U;
        AppMain.gmEneStingClearJetEfct(sting_work);
    }

    public static void gmEneStingCreateJetEfct(AppMain.GMS_ENE_STING_WORK sting_work)
    {
        if (sting_work.efct_r_jet == null)
        {
            sting_work.efct_r_jet = AppMain.GmEfctEneEsCreate((AppMain.OBS_OBJECT_WORK)sting_work, 0);
            AppMain.GmComEfctAddDispOffsetF(sting_work.efct_r_jet, -11f, -9f, 0.0f);
            sting_work.efct_r_jet.efct_com.obj_work.flag |= 524304U;
            sting_work.efct_r_jet.efct_com.obj_work.user_work_OBJECT = (object)sting_work.jet_r_mtx;
            sting_work.efct_r_jet.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneStingJetEfctMain);
        }
        if (sting_work.efct_l_jet != null)
            return;
        sting_work.efct_l_jet = AppMain.GmEfctEneEsCreate((AppMain.OBS_OBJECT_WORK)sting_work, 0);
        AppMain.GmComEfctAddDispOffsetF(sting_work.efct_l_jet, -11f, -9f, 0.0f);
        sting_work.efct_l_jet.efct_com.obj_work.flag |= 524304U;
        sting_work.efct_l_jet.efct_com.obj_work.user_work_OBJECT = (object)sting_work.jet_l_mtx;
        sting_work.efct_l_jet.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneStingJetEfctMain);
    }

    public static void gmEneStingClearJetEfct(AppMain.GMS_ENE_STING_WORK sting_work)
    {
        if (sting_work.efct_r_jet != null)
        {
            AppMain.ObjDrawKillAction3DES(sting_work.efct_r_jet.efct_com.obj_work);
            sting_work.efct_r_jet = (AppMain.GMS_EFFECT_3DES_WORK)null;
        }
        if (sting_work.efct_l_jet == null)
            return;
        AppMain.ObjDrawKillAction3DES(sting_work.efct_l_jet.efct_com.obj_work);
        sting_work.efct_l_jet = (AppMain.GMS_EFFECT_3DES_WORK)null;
    }

    public static void gmEneStingFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmEneStingSetWalkSpeed((AppMain.GMS_ENE_STING_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        AppMain.GMS_ENE_STING_WORK sting_work = (AppMain.GMS_ENE_STING_WORK)obj_work;
        obj_work.disp_flag ^= 1U;
        AppMain.gmEneStingCreateJetEfct(sting_work);
        AppMain.gmEneStingWalkInit(obj_work);
        ((AppMain.GMS_ENE_STING_WORK)obj_work).search_rect_work.flag |= 4U;
    }

    public static void gmEneStingAtkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_STING_WORK gmsEneStingWork = (AppMain.GMS_ENE_STING_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 4, 7);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneStingAtkMain);
        obj_work.spd.x = 0;
    }

    public static void gmEneStingMotionCallback(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_OBJECT _object,
      object param)
    {
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.GMS_ENE_STING_WORK gmsEneStingWork = (AppMain.GMS_ENE_STING_WORK)param;
        AppMain.nnMakeUnitMatrix(nnsMatrix2);
        AppMain.nnMultiplyMatrix(nnsMatrix2, nnsMatrix2, AppMain.amMatrixGetCurrent());
        AppMain.nnCalcNodeMatrixTRSList(nnsMatrix1, _object, AppMain.GMD_ENE_STING_NODE_ID_R_JET, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, nnsMatrix2);
        gmsEneStingWork.jet_r_mtx.Assign(nnsMatrix1);
        AppMain.nnCalcNodeMatrixTRSList(nnsMatrix1, _object, AppMain.GMD_ENE_STING_NODE_ID_L_JET, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, nnsMatrix2);
        gmsEneStingWork.jet_l_mtx.Assign(nnsMatrix1);
        AppMain.nnCalcNodeMatrixTRSList(nnsMatrix1, _object, AppMain.GMD_ENE_STING_NODE_ID_GUN, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, nnsMatrix2);
        gmsEneStingWork.gun_mtx.Assign(nnsMatrix1);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix1);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix2);
    }

    public static void gmEneStingSearchDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        if (match_rect.parent_obj.obj_type != (ushort)1)
            return;
        AppMain.GMS_ENE_STING_WORK parentObj1 = (AppMain.GMS_ENE_STING_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        AppMain.OBS_RECT_WORK obsRectWork1 = parentObj2.rect_work[2];
        float num1 = AppMain.FXM_FX32_TO_FLOAT(parentObj2.obj_work.pos.x) + (float)((int)obsRectWork1.rect.left + (int)obsRectWork1.rect.right >> 1);
        float num2 = AppMain.FXM_FX32_TO_FLOAT(parentObj2.obj_work.pos.y) + (float)((int)obsRectWork1.rect.top + (int)obsRectWork1.rect.bottom >> 1);
        AppMain.OBS_RECT_WORK obsRectWork2 = parentObj1.ene_3d_work.ene_com.rect_work[2];
        float num3 = AppMain.FXM_FX32_TO_FLOAT(parentObj1.ene_3d_work.ene_com.obj_work.pos.x) + (float)((int)obsRectWork2.rect.left + (int)obsRectWork2.rect.right >> 1);
        float num4 = AppMain.FXM_FX32_TO_FLOAT(parentObj1.ene_3d_work.ene_com.obj_work.pos.y + AppMain.GMD_ENE_STING_GUN_OFST_Y) + (float)((int)obsRectWork2.rect.top + (int)obsRectWork2.rect.bottom >> 1);
        int a;
        int b;
        if (((int)parentObj1.ene_3d_work.ene_com.obj_work.disp_flag & 1) != 0)
        {
            a = 32768 - AppMain.GMD_ENE_STING_SEARCH_DIR_START;
            b = 32768 - AppMain.GMD_ENE_STING_SEARCH_DIR_END;
        }
        else
        {
            a = AppMain.GMD_ENE_STING_SEARCH_DIR_START;
            b = AppMain.GMD_ENE_STING_SEARCH_DIR_END;
        }
        if (b < a)
            AppMain.MTM_MATH_SWAP<int>(ref a, ref b);
        float num5 = num1 - num3;
        int ang = AppMain.nnArcTan2((double)(num2 - num4), (double)num5);
        if (ang < a || ang > b)
            return;
        parentObj1.bullet_spd_x = (int)((double)AppMain.GMD_ENE_STING_BULLET_SPD * (double)AppMain.nnCos(ang));
        parentObj1.bullet_spd_y = (int)((double)AppMain.GMD_ENE_STING_BULLET_SPD * (double)AppMain.nnSin(ang));
        parentObj1.bullet_dir = ((int)parentObj1.ene_3d_work.ene_com.obj_work.disp_flag & 1) == 0 ? (short)ang : (short)(ang - 32768);
        AppMain.gmEneStingAtkInit((AppMain.OBS_OBJECT_WORK)parentObj1);
        parentObj1.search_rect_work.flag &= 4294967291U;
    }

    public static void gmEneStingRegRectFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_STING_WORK gmsEneStingWork = (AppMain.GMS_ENE_STING_WORK)obj_work;
        AppMain.ObjObjectRectRegist(obj_work, gmsEneStingWork.search_rect_work);
    }

    public static void gmEneStingAtkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_STING_WORK gmsEneStingWork = (AppMain.GMS_ENE_STING_WORK)obj_work;
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
            if (obj_work.user_timer != 0)
                return;
            if (obj_work.obj_3d.act_id[0] == 4 || obj_work.obj_3d.act_id[0] == 7)
            {
                AppMain.GmEneStingCreateBullet(obj_work, AppMain.GMD_ENE_STING_BULLET_FLASH_OFST_X, AppMain.GMD_ENE_STING_BULLET_FLASH_OFST_Y, AppMain.GMD_ENE_STING_BULLET_FLASH_OFST_Z, AppMain.GMD_ENE_STING_BULLET_OFST_X, AppMain.GMD_ENE_STING_BULLET_OFST_Y, AppMain.GMD_ENE_STING_BULLET_OFST_Z, gmsEneStingWork.bullet_spd_x, gmsEneStingWork.bullet_spd_y, gmsEneStingWork.bullet_dir);
                AppMain.GmEneComActionSetDependHFlip(obj_work, 5, 8);
                AppMain.GmSoundPlaySE("Sting");
            }
            else
                AppMain.GmEneComActionSetDependHFlip(obj_work, 6, 9);
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        switch (obj_work.obj_3d.act_id[0])
        {
            case 4:
            case 7:
                obj_work.user_timer = AppMain.GMD_ENE_STING_BULLET_WAIT;
                break;
            case 5:
            case 8:
                obj_work.user_timer = AppMain.GMD_ENE_STING_BULLET_AFTER_WAIT;
                break;
            case 6:
            case 9:
                AppMain.gmEneStingWalkInit(obj_work);
                break;
        }
    }

}