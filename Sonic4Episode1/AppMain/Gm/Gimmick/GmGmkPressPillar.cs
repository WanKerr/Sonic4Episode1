using System;

public partial class AppMain
{
    private static void GmGmkPressPillarBuild()
    {
        gm_gmk_press_pillar_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(951)), readAMBFile(GmGameDatGetGimmickData(952)), 0U);
        GmGmkPressPillarClear();
    }

    private static void GmGmkPressPillarFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(951));
        GmGameDBuildRegFlushModel(gm_gmk_press_pillar_obj_3d_list, amsAmbHeader.file_num);
    }

    private static OBS_OBJECT_WORK GmGmkPressPillarInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work1 = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_P_PIL_TOP");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work1;
        uint num = 0;
        if (eve_rec.id == 285)
            num = 1U;
        ObjObjectCopyAction3dNNModel(work1, gm_gmk_press_pillar_obj_3d_list[(int)(2U + num)], gmsEnemy3DWork.obj_3d);
        work1.pos.z = -126976;
        work1.disp_flag |= 4194304U;
        work1.move_flag |= 512U;
        work1.move_flag |= 1040U;
        work1.flag |= 1U;
        work1.user_flag = 0U;
        OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work1;
        colWork.obj_col.width = (ushort)GMD_GMK_PPIL_COL_WIDTH;
        colWork.obj_col.height = (ushort)GMD_GMK_PPIL_COL_HEIGHT;
        colWork.obj_col.ofst_x = (short)(-colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = 0;
        if (eve_rec.id == 285)
            colWork.obj_col.ofst_y = (short)-colWork.obj_col.height;
        if (eve_rec.id == 284)
            ObjObjectFieldRectSet(work1, (short)(-GMD_GMK_PPIL_COL_WIDTH / 2 + 2), -1, (short)(GMD_GMK_PPIL_COL_WIDTH / 2 - 2), GMD_GMK_PPIL_COL_HEIGHT);
        else
            ObjObjectFieldRectSet(work1, (short)(-GMD_GMK_PPIL_COL_WIDTH / 2 + 2), (short)-GMD_GMK_PPIL_COL_HEIGHT, (short)(GMD_GMK_PPIL_COL_WIDTH / 2 - 2), -1);
        work1.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPPillarTopWait);
        OBS_OBJECT_WORK work2 = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DNN_WORK(), work1, 0, "GMK_P_PIL_BODY");
        GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork1 = (GMS_EFFECT_3DNN_WORK)work2;
        ObjObjectCopyAction3dNNModel(work2, gm_gmk_press_pillar_obj_3d_list[(int)num], gmsEffect3DnnWork1.obj_3d);
        ObjAction3dNNMaterialMotionLoad(gmsEffect3DnnWork1.obj_3d, 0, null, null, (int)num, readAMBFile(ObjDataGet(953).pData));
        ObjDrawObjectActionSet3DNNMaterial(work2, 0);
        work2.pos.z = -131072;
        work2.move_flag |= 256U;
        work2.disp_flag |= 4194308U;
        work2.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPPillarBodyFollow);
        OBS_OBJECT_WORK work3 = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DNN_WORK(), work1, 0, "GMK_P_PIL_SPRING");
        GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork2 = (GMS_EFFECT_3DNN_WORK)work3;
        ObjObjectCopyAction3dNNModel(work3, gm_gmk_press_pillar_obj_3d_list[4], gmsEffect3DnnWork2.obj_3d);
        work3.pos.z = -131072;
        work3.move_flag |= 256U;
        work3.disp_flag |= 4194304U;
        work3.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPPillarSpringFollow);
        return work1;
    }

    private static void GmGmkPressPillarStartup(uint id_num)
    {
        id_num &= GMD_GMK_PPIL_ID_NUM_MASK;
        gm_gmk_press_pillar_sw[(int)id_num] = 1;
    }

    private static void GmGmkPressPillarClear()
    {
        Array.Clear(gm_gmk_press_pillar_sw, 0, gm_gmk_press_pillar_sw.Length);
    }

    private static void gmGmkPPillarTopWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_EVE_RECORD_EVENT eveRec = ((GMS_ENEMY_COM_WORK)obj_work).eve_rec;
        byte num1 = (byte)(eveRec.flag & GMD_GMK_PPIL_ID_NUM_MASK);
        if (gm_gmk_press_pillar_sw[num1] == 0)
            return;
        int num2 = MTM_MATH_ABS(eveRec.top << 10);
        if (num2 == 0)
            num2 = 4096;
        obj_work.spd.y = eveRec.id != 284 ? num2 : -num2;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPPillarTopMove);
    }

    private static void gmGmkPPillarTopMove(OBS_OBJECT_WORK obj_work)
    {
        GMS_EVE_RECORD_EVENT eveRec = ((GMS_ENEMY_COM_WORK)obj_work).eve_rec;
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
        int num1 = eveRec.height << 12;
        bool flag = false;
        if (num1 != 0)
        {
            if (eveRec.id == 284)
            {
                int num2 = gmsEnemyComWork.born_pos_y - num1;
                if (obj_work.pos.y <= num2)
                {
                    obj_work.pos.y = num2;
                    flag = true;
                }
            }
            else
            {
                int num2 = gmsEnemyComWork.born_pos_y + num1;
                if (obj_work.pos.y >= num2)
                {
                    obj_work.pos.y = num2;
                    flag = true;
                }
            }
        }
        if (((int)obj_work.move_flag & 15) != 0)
            flag = true;
        if (!flag)
            return;
        obj_work.spd.y = 0;
        obj_work.ppFunc = null;
        obj_work.user_flag |= GMD_GMK_PPIL_COLHIT;
        if ((eveRec.flag & GMD_GMK_PPIL_FLAG_EFFECT) != 0)
            return;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctZoneEsCreate(obj_work, 3, 1);
        if (eveRec.id != 284)
            return;
        obsObjectWork.dir.z = 32768;
    }

    private static void gmGmkPPillarBodyFollow(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        int y = parentObj.pos.y;
        int num = ((GMS_ENEMY_COM_WORK)parentObj).eve_rec.id != 284 ? y - GMD_GMK_PPIL_PIL_OFS_MAX : y + GMD_GMK_PPIL_PIL_OFS_MAX;
        obj_work.pos.y = num;
        if (((int)parentObj.user_flag & (int)GMD_GMK_PPIL_COLHIT) != 0)
        {
            if ((((GMS_ENEMY_COM_WORK)parentObj).eve_rec.flag & GMD_GMK_PPIL_FLAG_SHOCK_ABS) != 0)
            {
                obj_work.spd.y = 0;
                obj_work.ppFunc = null;
            }
            else
            {
                obj_work.spd.y = obj_work.user_timer;
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPPillarBodyMove);
                obj_work.user_flag = 0U;
            }
        }
        else
        {
            if (parentObj.spd.y == 0)
                return;
            obj_work.user_timer = parentObj.spd.y;
        }
    }

    private static void gmGmkPPillarBodyMove(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        GMS_EVE_RECORD_EVENT eveRec = ((GMS_ENEMY_COM_WORK)parentObj).eve_rec;
        int a = obj_work.spd.y * 3 / 4;
        if (16 < MTM_MATH_ABS(a))
        {
            obj_work.spd.y = a;
        }
        else
        {
            obj_work.spd.y = eveRec.id != 284 ? -obj_work.user_timer / 32 : -obj_work.user_timer / 32;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPPillarBodyMoveEx);
        }
        if (eveRec.id == 284)
        {
            if (obj_work.pos.y <= parentObj.pos.y + GMD_GMK_PPIL_PIL_OFS_MIN)
            {
                obj_work.pos.y = parentObj.pos.y + GMD_GMK_PPIL_PIL_OFS_MIN;
                obj_work.spd.y = 32;
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPPillarBodyMoveEx);
            }
        }
        else if (obj_work.pos.y >= parentObj.pos.y - GMD_GMK_PPIL_PIL_OFS_MIN)
        {
            obj_work.pos.y = parentObj.pos.y - GMD_GMK_PPIL_PIL_OFS_MIN;
            obj_work.spd.y = -32;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPPillarBodyMoveEx);
        }
        parentObj.user_work = (uint)obj_work.pos.y;
    }

    private static void gmGmkPPillarBodyMoveEx(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        GMS_EVE_RECORD_EVENT eveRec = ((GMS_ENEMY_COM_WORK)parentObj).eve_rec;
        if (obj_work.user_flag == 0U)
        {
            obj_work.spd.y = obj_work.spd.y * 8 / 7;
            if (2048 <= MTM_MATH_ABS(obj_work.spd.y))
                obj_work.user_flag = 1U;
        }
        else
        {
            int num = obj_work.spd.y * 7 / 8;
            if (128 < MTM_MATH_ABS(obj_work.spd.y))
                obj_work.spd.y = num;
        }
        if (eveRec.id == 284)
        {
            if (obj_work.pos.y >= parentObj.pos.y + GMD_GMK_PPIL_PIL_OFS_MAX)
            {
                obj_work.pos.y = parentObj.pos.y + GMD_GMK_PPIL_PIL_OFS_MAX;
                obj_work.spd.y = 0;
                obj_work.ppFunc = null;
            }
        }
        else if (obj_work.pos.y <= parentObj.pos.y - GMD_GMK_PPIL_PIL_OFS_MAX)
        {
            obj_work.pos.y = parentObj.pos.y - GMD_GMK_PPIL_PIL_OFS_MAX;
            obj_work.spd.y = 0;
            obj_work.ppFunc = null;
        }
        parentObj.user_work = (uint)obj_work.pos.y;
    }

    private static void gmGmkPPillarSpringFollow(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        int y = parentObj.pos.y;
        int num = ((GMS_ENEMY_COM_WORK)parentObj).eve_rec.id != 284 ? y - GMD_GMK_PPIL_SPR_OFS_MAX : y + GMD_GMK_PPIL_SPR_OFS_MAX;
        obj_work.pos.y = num;
        if (((int)parentObj.user_flag & (int)GMD_GMK_PPIL_COLHIT) == 0)
            return;
        obj_work.ppFunc = (((GMS_ENEMY_COM_WORK)parentObj).eve_rec.flag & GMD_GMK_PPIL_FLAG_SHOCK_ABS) == 0 ? new MPP_VOID_OBS_OBJECT_WORK(gmGmkPPillarSpringMove) : null;
        obj_work.spd.y = 0;
    }

    private static void gmGmkPPillarSpringMove(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        if (((GMS_ENEMY_COM_WORK)parentObj).eve_rec.id == 284)
        {
            int num = (parentObj.pos.y + GMD_GMK_PPIL_PIL_OFS_MIN - (int)parentObj.user_work) / 2;
            obj_work.pos.y = parentObj.pos.y + GMD_GMK_PPIL_SPR_OFS_MIN - num;
            float a = MTM_MATH_ABS(obj_work.pos.y - (parentObj.pos.y + GMD_GMK_PPIL_SPR_OFS_MIN)) / (float)(GMD_GMK_PPIL_SPR_OFS_MAX - GMD_GMK_PPIL_SPR_OFS_MIN);
            obj_work.scale.y = FXM_FLOAT_TO_FX32(a);
        }
        else
        {
            int num = (parentObj.pos.y - GMD_GMK_PPIL_PIL_OFS_MIN - (int)parentObj.user_work) / 2;
            obj_work.pos.y = parentObj.pos.y - GMD_GMK_PPIL_SPR_OFS_MIN - num;
            float a = MTM_MATH_ABS(obj_work.pos.y - (parentObj.pos.y - GMD_GMK_PPIL_SPR_OFS_MIN)) / (float)(GMD_GMK_PPIL_SPR_OFS_MAX - GMD_GMK_PPIL_SPR_OFS_MIN);
            obj_work.scale.y = FXM_FLOAT_TO_FX32(a);
        }
    }

}