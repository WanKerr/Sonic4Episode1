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
    private static void GmGmkCapsuleBuild()
    {
        AppMain.gm_gmk_capsule_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(860), AppMain.GmGameDatGetGimmickData(861), 0U);
    }

    private static void GmGmkCapsuleFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(860);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_capsule_obj_3d_list, gimmickData.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkCapsuleInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_CAPSULE");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork1 = (AppMain.GMS_ENEMY_3D_WORK)work1;
        AppMain.ObjObjectCopyAction3dNNModel(work1, AppMain.gm_gmk_capsule_obj_3d_list[2], gmsEnemy3DWork1.obj_3d);
        work1.pos.z = -393216;
        work1.move_flag |= 8448U;
        work1.disp_flag |= 4194304U;
        AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork1.ene_com.col_work;
        colWork.obj_col.obj = work1;
        colWork.obj_col.width = (ushort)32;
        colWork.obj_col.height = (ushort)40;
        colWork.obj_col.ofst_x = (short)((int)-colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-76;
        gmsEnemy3DWork1.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork1.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork1.ene_com.rect_work[2];
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        pRec.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec, (short)-4, (short)-80, (short)4, (short)-72);
        work1.user_flag = (uint)((ulong)work1.user_flag & 18446744073709551614UL);
        work1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCapsuleSwitchMain);
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)301, pos_x, pos_y, gmsEnemy3DWork1.ene_com.eve_rec.flag, gmsEnemy3DWork1.ene_com.eve_rec.left, gmsEnemy3DWork1.ene_com.eve_rec.top, gmsEnemy3DWork1.ene_com.eve_rec.width, gmsEnemy3DWork1.ene_com.eve_rec.height, (byte)0);
        obsObjectWork.parent_obj = work1;
        obsObjectWork.view_out_ofst = work1.view_out_ofst;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork2 = (AppMain.GMS_ENEMY_3D_WORK)obsObjectWork;
        AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), work1, (ushort)0, "GMK_CAPSULE_BODY");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (AppMain.GMS_EFFECT_3DNN_WORK)work2;
        AppMain.ObjObjectCopyAction3dNNModel(work2, AppMain.gm_gmk_capsule_obj_3d_list[1], gmsEffect3DnnWork.obj_3d);
        work2.pos.z = -131072;
        work2.move_flag |= 8448U;
        work2.disp_flag |= 4194304U;
        work2.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCapsuleKeyMain);
        return work1;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkCapsuleBodyInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_CAPSULE_BODY");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_capsule_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, false, AppMain.ObjDataGet(862), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectActionSet(work, 0);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work;
        colWork.obj_col.width = (ushort)64;
        colWork.obj_col.height = (ushort)60;
        colWork.obj_col.ofst_x = (short)((int)-colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-colWork.obj_col.height;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCapsuleBodyMain);
        return work;
    }

    private static void gmGmkCapsuleSwitchMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.col_work.obj_col.rider_obj != null)
        {
            obj_work.ofst.y = 24576;
            if (((int)obj_work.user_flag & 1) == 0)
            {
                AppMain.g_gm_main_system.game_flag &= 4294966271U;
                AppMain.g_gm_main_system.game_flag |= 1048576U;
                AppMain.GmEffect3DESSetDispOffset(AppMain.GmEfctCmnEsCreate(obj_work, 23), 0.0f, 24f, 40f);
                AppMain.gmGmkCapsuleAnimalMake(obj_work);
                obj_work.user_timer = 1;
                AppMain.GmGmkCamScrLimitSet(new AppMain.GMS_EVE_RECORD_EVENT()
                {
                    flag = (ushort)7,
                    left = (sbyte)-96,
                    top = (sbyte)-104,
                    width = (byte)192,
                    height = (byte)112
                }, obj_work.pos.x, obj_work.pos.y);
                AppMain.GMM_PAD_VIB_SMALL();
                AppMain.GmSoundPlaySE("Capsule");
                AppMain.GmPlySeqChangeBossGoal(AppMain.g_gm_main_system.ply_work[0], obj_work.pos.x, obj_work.pos.y);
            }
            obj_work.user_flag |= 1U;
        }
        else
            obj_work.ofst.y = 0;
        if (obj_work.user_timer == 0)
            return;
        ++obj_work.user_timer;
        if (obj_work.user_timer != 420)
            return;
        AppMain.g_gm_main_system.game_flag |= 4U;
    }

    private static void gmGmkCapsuleBodyMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.parent_obj.user_flag & 1) == 0)
            return;
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, 1, 0);
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmGmkCapsuleKeyMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.parent_obj.user_flag & 1) == 0)
            return;
        obj_work.spd.x = 24576;
        obj_work.spd.y = -16384;
        obj_work.move_flag &= 4294959103U;
        obj_work.move_flag |= 128U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.GmEffect3DESSetDispOffset(AppMain.GmEfctCmnEsCreate(obj_work, 21), 0.0f, 46f, 40f);
        AppMain.GmEffect3DESSetDispOffset(AppMain.GmEfctCmnEsCreate(obj_work, 22), 0.0f, 46f, 40f);
    }

    private static void gmGmkCapsuleAnimalMake(AppMain.OBS_OBJECT_WORK obj_work)
    {
        for (ushort index = 0; index < (ushort)20; ++index)
            AppMain.GmGmkAnimalInit(obj_work, (int)AppMain.g_gm_gmk_capsule_animal_set[(int)index].ofs_x << 12, (int)AppMain.g_gm_gmk_capsule_animal_set[(int)index].ofs_y << 12, (int)AppMain.g_gm_gmk_capsule_animal_set[(int)index].ofs_z << 12, AppMain.g_gm_gmk_capsule_animal_set[(int)index].type, AppMain.g_gm_gmk_capsule_animal_set[(int)index].vec, AppMain.g_gm_gmk_capsule_animal_set[(int)index].time);
    }

}