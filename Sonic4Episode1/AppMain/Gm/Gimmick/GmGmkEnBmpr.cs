using System;

public partial class AppMain
{
    private static void GmGmkEnBmprBuild()
    {
        g_gm_gmk_en_bmpr_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(852), GmGameDatGetGimmickData(853), 0U);
    }

    private static void GmGmkEnBmprFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(852);
        GmGameDBuildRegFlushModel(g_gm_gmk_en_bmpr_obj_3d_list, gimmickData.file_num);
        g_gm_gmk_en_bmpr_obj_3d_list = null;
    }

    private static OBS_OBJECT_WORK GmGmkEnBmprInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int life_max = 3;
        if (eve_rec.left > 0 && eve_rec.left < 3)
            life_max = eve_rec.left;
        if (life_max <= eve_rec.byte_param[1])
            return null;
        int num = gmGmkEnBmprCalcType(eve_rec.id);
        OBS_OBJECT_WORK objWork = gmGmkEnBmprLoadObj(eve_rec, pos_x, pos_y, num).ene_com.obj_work;
        gmGmkEnBmprInit(objWork, num, life_max);
        return objWork;
    }

    private static uint gmGmkEnBmpreGameSystemGetSyncTime()
    {
        return g_gm_main_system.sync_time;
    }

    private static GMS_ENEMY_3D_WORK gmGmkEnBmprLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_EN_BMPR");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkEnBmprLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkEnBmprLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index = 3;
        ObjObjectCopyAction3dNNModel(objWork, g_gm_gmk_en_bmpr_obj_3d_list[index], gmsEnemy3DWork.obj_3d);
        OBS_DATA_WORK data_work1 = ObjDataGet(854);
        ObjObjectAction3dNNMotionLoad(objWork, 0, false, data_work1, null, 0, null);
        OBS_DATA_WORK data_work2 = ObjDataGet(855);
        ObjObjectAction3dNNMaterialMotionLoad(objWork, 0, data_work2, null, 0, null);
        return gmsEnemy3DWork;
    }

    private static void gmGmkEnBmprInit(
      OBS_OBJECT_WORK obj_work,
      int en_bmpr_type,
      int life_max)
    {
        GMS_ENEMY_3D_WORK gimmick_work = (GMS_ENEMY_3D_WORK)obj_work;
        gmGmkEnBmprSetRect(gimmick_work, en_bmpr_type);
        obj_work.move_flag = 8448U;
        obj_work.user_flag |= 1U;
        obj_work.dir.z = g_gm_gmk_en_bmpr_angle_z[en_bmpr_type];
        int life = life_max - gimmick_work.ene_com.eve_rec.byte_param[1];
        gmGmkEnBmperSetUserWorkLife(obj_work, life);
        ObjDrawObjectActionSet3DNNMaterial(obj_work, g_gm_gmk_en_bmpr_mat_motion_id[life]);
        obj_work.disp_flag |= 4194308U;
        obj_work.pos.z = -122880;
        obj_work.ppFunc = null;
        obj_work.ppMove = null;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkEnBmprDrawFunc);
        gmGmkEnBmprChangeModeWait(obj_work);
    }

    private static void gmGmkEnBmprSetRect(GMS_ENEMY_3D_WORK gimmick_work, int en_bmpr_type)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        short cLeft = g_gmk_en_bmpr_rect[en_bmpr_type][0];
        short cRight = g_gmk_en_bmpr_rect[en_bmpr_type][2];
        short cTop = g_gmk_en_bmpr_rect[en_bmpr_type][1];
        short cBottom = g_gmk_en_bmpr_rect[en_bmpr_type][3];
        ObjRectWorkZSet(pRec, cLeft, cTop, -500, cRight, cBottom, 500);
        pRec.flag |= 1024U;
        ObjRectDefSet(pRec, 65534, 0);
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkEnBmprDefFunc);
    }

    private static void gmGmkEnBmprDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (obj3d.motion != null)
        {
            float startFrame = amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
            float num = amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - startFrame;
            float syncTime = gmGmkEnBmpreGameSystemGetSyncTime();
            obj3d.mat_frame = syncTime % num;
        }
        ObjDrawActionSummary(obj_work);
    }

    private static int gmGmkEnBmprCalcType(int id)
    {
        return id - 164;
    }

    private static VecFx32 gmGmkEnBmprNormalizeVectorXY(VecFx32 vec)
    {
        VecFx32 vecFx32 = new VecFx32();
        int denom = FX_Sqrt(FX_Mul(vec.x, vec.x) + FX_Mul(vec.y, vec.y));
        if (denom == 0)
        {
            vecFx32.x = 4096;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = FX_Div(4096, denom);
            vecFx32.x = FX_Mul(vec.x, v2);
            vecFx32.y = FX_Mul(vec.y, v2);
        }
        vecFx32.z = 0;
        return vecFx32;
    }

    private static void gmGmkEnBmprDefFunc(
      OBS_RECT_WORK gimmick_rect,
      OBS_RECT_WORK player_rect)
    {
        OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)parentObj1;
        OBS_OBJECT_WORK parentObj2 = player_rect.parent_obj;
        if (parentObj2.obj_type != 1)
            return;
        int index = gmGmkEnBmprCalcType(gmsEnemy3DWork.ene_com.eve_rec.id);
        parentObj1.dir.z = g_gm_gmk_en_bmpr_angle_z[index];
        parentObj2.dir.z = 0;
        int num1 = parentObj2.spd.x;
        int num2 = parentObj2.spd.y;
        if (((int)parentObj2.move_flag & 32768) != 0)
        {
            if (parentObj2.spd_m != 0)
            {
                num1 = FX_Mul(parentObj2.spd_m, mtMathCos(parentObj2.dir.z));
                num2 = FX_Mul(parentObj2.spd_m, mtMathSin(parentObj2.dir.z));
            }
            else
            {
                VecFx32 vec = new VecFx32();
                vec.x = parentObj2.pos.x - parentObj1.pos.x;
                vec.y = parentObj2.pos.y - parentObj1.pos.y;
                vec.z = 0;
                vec = gmGmkEnBmprNormalizeVectorXY(vec);
                num1 = FX_Mul(vec.x, 98304);
                num2 = FX_Mul(vec.y, 98304);
            }
        }
        int num3 = -12288;
        int num4 = parentObj2.pos.x - parentObj1.pos.x;
        int num5 = parentObj2.pos.y + num3 - parentObj1.pos.y;
        switch (index)
        {
            case 0:
                num1 = 0;
                if (num5 < 0)
                {
                    num2 = -24576;
                    break;
                }
                num2 = 24576;
                parentObj1.dir.z += 32768;
                break;
            case 1:
                int num6 = FX_Mul(24576, 2896);
                if (num5 < 0)
                {
                    num1 = -num6;
                    num2 = -num6;
                    break;
                }
                num1 = num6;
                num2 = num6;
                parentObj1.dir.z += 32768;
                break;
            case 2:
                num2 = 0;
                if (num4 < 0)
                {
                    num1 = -24576;
                    break;
                }
                num1 = 24576;
                parentObj1.dir.z += 32768;
                break;
            case 3:
                int num7 = FX_Mul(24576, 2896);
                if (num5 > 0)
                {
                    num1 = -num7;
                    num2 = num7;
                    break;
                }
                num1 = num7;
                num2 = -num7;
                parentObj1.dir.z += 32768;
                break;
        }
        GmPlySeqInitPinballAir((GMS_PLAYER_WORK)parentObj2, num1, num2, 5);
        gmGmkEnBmprChangeModeHit(parentObj1);
        if (gmGmkEnBmperGetUserWorkLife(parentObj1) <= 0)
            parentObj1.user_flag = (uint)(parentObj1.user_flag & 18446744073709551614UL);
        int score = 10;
        if (gmGmkEnBmprCheckGroupBonus(parentObj1) != 0)
            score *= 50;
        GmPlayerAddScore((GMS_PLAYER_WORK)parentObj2, score, parentObj1.pos.x, parentObj1.pos.y);
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(parentObj1, 16);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj2.pos.x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj2.pos.y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
        gmsEffect3DesWork.efct_com.obj_work.dir.z = (ushort)(nnArcTan2(FX_FX32_TO_F32(num2), FX_FX32_TO_F32(num1)) - 16384);
        GMM_PAD_VIB_SMALL();
    }

    private static int gmGmkEnBmprCheckGroupBonus(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (gmGmkEnBmperGetUserWorkLife(obj_work) > 0)
            return 0;
        sbyte top = gmsEnemy3DWork.ene_com.eve_rec.top;
        if (top == 0)
            return 0;
        for (OBS_OBJECT_WORK obj_work1 = ObjObjectSearchRegistObject(null, 3); obj_work1 != null; obj_work1 = ObjObjectSearchRegistObject(obj_work1, 3))
        {
            GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work1;
            if (obj_work1 != obj_work && (gmsEnemyComWork.eve_rec.id == 164 || gmsEnemyComWork.eve_rec.id == 165 || (gmsEnemyComWork.eve_rec.id == 166 || gmsEnemyComWork.eve_rec.id == 167)) && (gmsEnemyComWork.eve_rec.top == top && (Convert.ToInt32(obj_work1.user_flag) & 1) != 0))
                return 0;
        }
        return 1;
    }

    private static void gmGmkEnBmprChangeModeWait(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawObjectActionSet3DNN(obj_work, 0, 0);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkEnBmprMainWait);
    }

    private static void gmGmkEnBmprChangeModeHit(OBS_OBJECT_WORK obj_work)
    {
        GmSoundPlaySE("Casino7");
        byte num = 1;
        ((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.byte_param[1] += num;
        int index = gmGmkEnBmperAddUserWorkLife(obj_work, -num);
        if (index < 0)
            return;
        ObjDrawObjectActionSet3DNNMaterial(obj_work, g_gm_gmk_en_bmpr_mat_motion_id[index]);
        ObjDrawObjectActionSet3DNN(obj_work, 1, 0);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkEnBmprMainHit);
    }

    private static void gmGmkEnBmprChangeModeLost(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawObjectActionSet3DNN(obj_work, 0, 0);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkEnBmprMainLost);
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(null, 19);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = obj_work.pos.x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = obj_work.pos.y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
    }

    private static void gmGmkEnBmprMainWait(OBS_OBJECT_WORK obj_work)
    {
        UNREFERENCED_PARAMETER(obj_work);
    }

    private static void gmGmkEnBmprMainHit(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (gmGmkEnBmperGetUserWorkLife(obj_work) > 0)
            gmGmkEnBmprChangeModeWait(obj_work);
        else
            gmGmkEnBmprChangeModeLost(obj_work);
    }

    private static void gmGmkEnBmprMainLost(OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag |= 4U;
    }

    private static void gmGmkEnBmperSetUserWorkLife(OBS_OBJECT_WORK obj_work, int life)
    {
        obj_work.user_work = (uint)life;
    }

    private static int gmGmkEnBmperGetUserWorkLife(OBS_OBJECT_WORK obj_work)
    {
        return (int)obj_work.user_work;
    }

    private static int gmGmkEnBmperAddUserWorkLife(OBS_OBJECT_WORK obj_work, int add)
    {
        obj_work.user_work += (uint)add;
        return (int)obj_work.user_work;
    }


}