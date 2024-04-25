using System;

public partial class AppMain
{
    private static OBS_OBJECT_WORK GmEneStingInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_STING_WORK(), "ENE_STING");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_STING_WORK sting_work = (GMS_ENE_STING_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_sting_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, false, ObjDataGet(669), null, 0, null);
        ObjDrawObjectSetToon(work);
        gmsEnemy3DWork.obj_3d.mtn_cb_func = new mtn_cb_func_delegate(gmEneStingMotionCallback);
        gmsEnemy3DWork.obj_3d.mtn_cb_param = sting_work;
        work.pos.z = 0;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -10, -8, 20, 8);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        ObjRectWorkSet(pRec2, -18, -16, 28, 16);
        pRec2.flag |= 4U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -18, -16, 28, 16);
        pRec3.flag &= 4294967291U;
        OBS_RECT_WORK searchRectWork = sting_work.search_rect_work;
        searchRectWork.ppDef = new OBS_RECT_WORK_Delegate1(gmEneStingSearchDefFunc);
        ObjRectGroupSet(searchRectWork, 1, 1);
        ObjRectAtkSet(searchRectWork, 0, 0);
        ObjRectDefSet(searchRectWork, 65534, 0);
        searchRectWork.parent_obj = work;
        ObjRectWorkSet(searchRectWork, 0, 0, 128, 128);
        searchRectWork.flag |= 1048804U;
        work.ppRec = new MPP_VOID_OBS_OBJECT_WORK(gmEneStingRegRectFunc);
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        if ((eve_rec.flag & GMD_ENE_STING_EVE_FLAG_RIGHT) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        sting_work.spd_dec = GMD_ENE_STING_SPD_X / (GMD_ENE_STING_TURN_FRAME / 2);
        sting_work.spd_dec_dist = GMD_ENE_STING_SPD_X * (GMD_ENE_STING_TURN_FRAME / 2) / 2;
        gmEneStingWalkInit(work);
        gmEneStingCreateJetEfct(sting_work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static int gmEneStingSetWalkSpeed(GMS_ENE_STING_WORK sting_work)
    {
        int num = 0;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)sting_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 3 && obsObjectWork.obj_3d.frame[0] >= (double)(GMD_ENE_STING_TURN_FRAME / 2))
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, sting_work.spd_dec, GMD_ENE_STING_SPD_X);
            else if (obsObjectWork.pos.x <= Convert.ToInt32(obsObjectWork.user_work) + sting_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, sting_work.spd_dec);
                num = 1;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > (int)obsObjectWork.user_work)
                {
                    obsObjectWork.spd.x = (int)obsObjectWork.user_work - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -sting_work.spd_dec)
                        obsObjectWork.spd.x = -sting_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -GMD_ENE_STING_SPD_X)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -sting_work.spd_dec, GMD_ENE_STING_SPD_X);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 2 && obsObjectWork.obj_3d.frame[0] >= (double)(GMD_ENE_STING_TURN_FRAME / 2))
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -sting_work.spd_dec, GMD_ENE_STING_SPD_X);
        else if (obsObjectWork.pos.x >= Convert.ToUInt32(obsObjectWork.user_flag) - sting_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, sting_work.spd_dec);
            num = 1;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < Convert.ToInt32(obsObjectWork.user_flag))
            {
                obsObjectWork.spd.x = Convert.ToInt32(obsObjectWork.user_flag) - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > sting_work.spd_dec)
                    obsObjectWork.spd.x = sting_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < GMD_ENE_STING_SPD_X)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, sting_work.spd_dec, GMD_ENE_STING_SPD_X);
        return num;
    }

    public static void GmEneStingBuild()
    {
        gm_ene_sting_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(667)), readAMBFile(GmGameDatGetEnemyData(668)), 0U);
    }

    public static void GmEneStingFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(667));
        GmGameDBuildRegFlushModel(gm_ene_sting_obj_3d_list, amsAmbHeader.file_num);
    }

    public static void GmEneStingCreateBullet(
      OBS_OBJECT_WORK parent_obj,
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
        GMS_EFFECT_COM_WORK atkObject = (GMS_EFFECT_COM_WORK)GmEneComCreateAtkObject(parent_obj, 16);
        atkObject.obj_work.parent_obj = null;
        atkObject.obj_work.pos.x += ((int)parent_obj.disp_flag & 1) != 0 ? -ofst_bul_x : ofst_bul_x;
        atkObject.obj_work.pos.y += ofst_bul_y;
        atkObject.obj_work.pos.z += ofst_bul_z;
        OBS_RECT_WORK pRec = atkObject.rect_work[1];
        ObjRectWorkSet(pRec, -8, -8, 8, 8);
        pRec.flag |= 4U;
        atkObject.obj_work.spd.x = spd_x;
        atkObject.obj_work.spd.y = spd_y;
        atkObject.obj_work.view_out_ofst = 16;
        GMS_EFFECT_3DES_WORK efct_work1 = GmEfctCmnEsCreate(atkObject.obj_work, 15);
        GmComEfctSetDispRotationS(efct_work1, 0, 0, dir);
        efct_work1.efct_com.obj_work.flag |= 1024U;
        GMS_EFFECT_3DES_WORK efct_work2 = GmEfctCmnEsCreate(parent_obj, 14);
        GmComEfctSetDispRotationS(efct_work2, 0, 0, (short)(dir - 32768));
        efct_work2.efct_com.obj_work.parent_obj = null;
        efct_work2.efct_com.obj_work.pos.x += ((int)parent_obj.disp_flag & 1) != 0 ? -ofst_flash_x : ofst_flash_x;
        efct_work2.efct_com.obj_work.pos.y += ofst_flash_y;
        efct_work2.efct_com.obj_work.pos.z += ofst_flash_z;
    }

    public static void gmEneStingJetEfctMain(OBS_OBJECT_WORK obj_work)
    {
        NNS_MATRIX userWorkObject = (NNS_MATRIX)obj_work.user_work_OBJECT;
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        if (obj_work.parent_obj == null)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            nnsVector.x = userWorkObject.M03 - FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.x);
            nnsVector.y = -userWorkObject.M13 - FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.y);
            nnsVector.z = userWorkObject.M23 - FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.z);
            if (((int)obj_work.parent_obj.disp_flag & 1) != 0)
            {
                nnsVector.x = -nnsVector.x;
                nnsVector.z = -nnsVector.z;
            }
            nnsVector.x += -3f;
            GmComEfctSetDispOffsetF((GMS_EFFECT_3DES_WORK)obj_work, nnsVector.x, nnsVector.y, nnsVector.z);
            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
            GlobalPool<NNS_VECTOR>.Release(nnsVector);
        }
    }

    public static void gmEneStingWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 0, 1);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneStingWalkMain);
    }

    public static void gmEneStingWalkMain(OBS_OBJECT_WORK obj_work)
    {
        if (gmEneStingSetWalkSpeed((GMS_ENE_STING_WORK)obj_work) == 0)
            return;
        gmEneStingFlipInit(obj_work);
    }

    public static void gmEneStingFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_STING_WORK sting_work = (GMS_ENE_STING_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 2, 3);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneStingFlipMain);
        sting_work.search_rect_work.flag &= 4294967291U;
        gmEneStingClearJetEfct(sting_work);
    }

    public static void gmEneStingCreateJetEfct(GMS_ENE_STING_WORK sting_work)
    {
        if (sting_work.efct_r_jet == null)
        {
            sting_work.efct_r_jet = GmEfctEneEsCreate((OBS_OBJECT_WORK)sting_work, 0);
            GmComEfctAddDispOffsetF(sting_work.efct_r_jet, -11f, -9f, 0.0f);
            sting_work.efct_r_jet.efct_com.obj_work.flag |= 524304U;
            sting_work.efct_r_jet.efct_com.obj_work.user_work_OBJECT = sting_work.jet_r_mtx;
            sting_work.efct_r_jet.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneStingJetEfctMain);
        }
        if (sting_work.efct_l_jet != null)
            return;
        sting_work.efct_l_jet = GmEfctEneEsCreate((OBS_OBJECT_WORK)sting_work, 0);
        GmComEfctAddDispOffsetF(sting_work.efct_l_jet, -11f, -9f, 0.0f);
        sting_work.efct_l_jet.efct_com.obj_work.flag |= 524304U;
        sting_work.efct_l_jet.efct_com.obj_work.user_work_OBJECT = sting_work.jet_l_mtx;
        sting_work.efct_l_jet.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneStingJetEfctMain);
    }

    public static void gmEneStingClearJetEfct(GMS_ENE_STING_WORK sting_work)
    {
        if (sting_work.efct_r_jet != null)
        {
            ObjDrawKillAction3DES(sting_work.efct_r_jet.efct_com.obj_work);
            sting_work.efct_r_jet = null;
        }
        if (sting_work.efct_l_jet == null)
            return;
        ObjDrawKillAction3DES(sting_work.efct_l_jet.efct_com.obj_work);
        sting_work.efct_l_jet = null;
    }

    public static void gmEneStingFlipMain(OBS_OBJECT_WORK obj_work)
    {
        gmEneStingSetWalkSpeed((GMS_ENE_STING_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        GMS_ENE_STING_WORK sting_work = (GMS_ENE_STING_WORK)obj_work;
        obj_work.disp_flag ^= 1U;
        gmEneStingCreateJetEfct(sting_work);
        gmEneStingWalkInit(obj_work);
        ((GMS_ENE_STING_WORK)obj_work).search_rect_work.flag |= 4U;
    }

    public static void gmEneStingAtkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_STING_WORK gmsEneStingWork = (GMS_ENE_STING_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 4, 7);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneStingAtkMain);
        obj_work.spd.x = 0;
    }

    public static void gmEneStingMotionCallback(
      AMS_MOTION motion,
      NNS_OBJECT _object,
      object param)
    {
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
        GMS_ENE_STING_WORK gmsEneStingWork = (GMS_ENE_STING_WORK)param;
        nnMakeUnitMatrix(nnsMatrix2);
        nnMultiplyMatrix(nnsMatrix2, nnsMatrix2, amMatrixGetCurrent());
        nnCalcNodeMatrixTRSList(nnsMatrix1, _object, GMD_ENE_STING_NODE_ID_R_JET, motion.data, nnsMatrix2);
        gmsEneStingWork.jet_r_mtx.Assign(nnsMatrix1);
        nnCalcNodeMatrixTRSList(nnsMatrix1, _object, GMD_ENE_STING_NODE_ID_L_JET, motion.data, nnsMatrix2);
        gmsEneStingWork.jet_l_mtx.Assign(nnsMatrix1);
        nnCalcNodeMatrixTRSList(nnsMatrix1, _object, GMD_ENE_STING_NODE_ID_GUN, motion.data, nnsMatrix2);
        gmsEneStingWork.gun_mtx.Assign(nnsMatrix1);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix1);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix2);
    }

    public static void gmEneStingSearchDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        if (match_rect.parent_obj.obj_type != 1)
            return;
        GMS_ENE_STING_WORK parentObj1 = (GMS_ENE_STING_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        OBS_RECT_WORK obsRectWork1 = parentObj2.rect_work[2];
        float num1 = FXM_FX32_TO_FLOAT(parentObj2.obj_work.pos.x) + (obsRectWork1.rect.left + obsRectWork1.rect.right >> 1);
        float num2 = FXM_FX32_TO_FLOAT(parentObj2.obj_work.pos.y) + (obsRectWork1.rect.top + obsRectWork1.rect.bottom >> 1);
        OBS_RECT_WORK obsRectWork2 = parentObj1.ene_3d_work.ene_com.rect_work[2];
        float num3 = FXM_FX32_TO_FLOAT(parentObj1.ene_3d_work.ene_com.obj_work.pos.x) + (obsRectWork2.rect.left + obsRectWork2.rect.right >> 1);
        float num4 = FXM_FX32_TO_FLOAT(parentObj1.ene_3d_work.ene_com.obj_work.pos.y + GMD_ENE_STING_GUN_OFST_Y) + (obsRectWork2.rect.top + obsRectWork2.rect.bottom >> 1);
        int a;
        int b;
        if (((int)parentObj1.ene_3d_work.ene_com.obj_work.disp_flag & 1) != 0)
        {
            a = 32768 - GMD_ENE_STING_SEARCH_DIR_START;
            b = 32768 - GMD_ENE_STING_SEARCH_DIR_END;
        }
        else
        {
            a = GMD_ENE_STING_SEARCH_DIR_START;
            b = GMD_ENE_STING_SEARCH_DIR_END;
        }
        if (b < a)
            MTM_MATH_SWAP(ref a, ref b);
        float num5 = num1 - num3;
        int ang = nnArcTan2(num2 - num4, num5);
        if (ang < a || ang > b)
            return;
        parentObj1.bullet_spd_x = (int)(GMD_ENE_STING_BULLET_SPD * (double)nnCos(ang));
        parentObj1.bullet_spd_y = (int)(GMD_ENE_STING_BULLET_SPD * (double)nnSin(ang));
        parentObj1.bullet_dir = ((int)parentObj1.ene_3d_work.ene_com.obj_work.disp_flag & 1) == 0 ? (short)ang : (short)(ang - 32768);
        gmEneStingAtkInit((OBS_OBJECT_WORK)parentObj1);
        parentObj1.search_rect_work.flag &= 4294967291U;
    }

    public static void gmEneStingRegRectFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_STING_WORK gmsEneStingWork = (GMS_ENE_STING_WORK)obj_work;
        ObjObjectRectRegist(obj_work, gmsEneStingWork.search_rect_work);
    }

    public static void gmEneStingAtkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_STING_WORK gmsEneStingWork = (GMS_ENE_STING_WORK)obj_work;
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
            if (obj_work.user_timer != 0)
                return;
            if (obj_work.obj_3d.act_id[0] == 4 || obj_work.obj_3d.act_id[0] == 7)
            {
                GmEneStingCreateBullet(obj_work, GMD_ENE_STING_BULLET_FLASH_OFST_X, GMD_ENE_STING_BULLET_FLASH_OFST_Y, GMD_ENE_STING_BULLET_FLASH_OFST_Z, GMD_ENE_STING_BULLET_OFST_X, GMD_ENE_STING_BULLET_OFST_Y, GMD_ENE_STING_BULLET_OFST_Z, gmsEneStingWork.bullet_spd_x, gmsEneStingWork.bullet_spd_y, gmsEneStingWork.bullet_dir);
                GmEneComActionSetDependHFlip(obj_work, 5, 8);
                GmSoundPlaySE("Sting");
            }
            else
                GmEneComActionSetDependHFlip(obj_work, 6, 9);
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        switch (obj_work.obj_3d.act_id[0])
        {
            case 4:
            case 7:
                obj_work.user_timer = GMD_ENE_STING_BULLET_WAIT;
                break;
            case 5:
            case 8:
                obj_work.user_timer = GMD_ENE_STING_BULLET_AFTER_WAIT;
                break;
            case 6:
            case 9:
                gmEneStingWalkInit(obj_work);
                break;
        }
    }

}