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
    private static void GmBoss3Build()
    {
        AppMain.AMS_AMB_HEADER gameDatEnemyArc = AppMain.GmBoss3GetGameDatEnemyArc();
        AppMain.gm_boss3_obj_3d_list = AppMain.GmGameDBuildRegBuildModel((AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, gameDatEnemyArc), (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 1, gameDatEnemyArc), 0U);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(728), 2, gameDatEnemyArc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(729), 3, gameDatEnemyArc);
    }

    private static void GmBoss3Flush()
    {
        AppMain.GmEfctBossFlushSingleDataInit();
        AppMain.ObjDataRelease(AppMain.ObjDataGet(729));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(728));
        AppMain.AMS_AMB_HEADER amsAmbHeader = (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, AppMain.GmBoss3GetGameDatEnemyArc());
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_boss3_obj_3d_list, amsAmbHeader.file_num);
        AppMain.gm_boss3_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.AMS_AMB_HEADER GmBoss3GetGameDatEnemyArc()
    {
        return AppMain.g_gm_gamedat_enemy_arc;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss3Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_BOSS3_MGR_WORK work = (AppMain.GMS_BOSS3_MGR_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS3_MGR_WORK()), "BOSS3_MGR");
        AppMain.OBS_OBJECT_WORK objWork = work.ene_3d.ene_com.obj_work;
        objWork.flag |= 16U;
        objWork.disp_flag |= 32U;
        objWork.move_flag |= 8448U;
        work.ene_3d.ene_com.enemy_flag |= 32768U;
        objWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3MgrMainFuncWaitLoad);
        work.life = AppMain.GmBsCmnIsFinalZoneType(objWork) == 0 ? 8 : 4;
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss3BodyInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_BOSS3_BODY_WORK work = (AppMain.GMS_BOSS3_BODY_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS3_BODY_WORK()), "BOSS3_BODY");
        AppMain.GMS_ENEMY_3D_WORK ene3d = work.ene_3d;
        AppMain.OBS_OBJECT_WORK objWork = ene3d.ene_com.obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.gm_boss3_obj_3d_list[0], ene3d.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(objWork, 0, true, AppMain.ObjDataGet(728), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        ene3d.ene_com.vit = (byte)1;
        AppMain.ObjRectWorkSet(ene3d.ene_com.rect_work[2], (short)-24, (short)-24, (short)24, (short)24);
        AppMain.ObjRectGroupSet(ene3d.ene_com.rect_work[2], (byte)1, (byte)3);
        ene3d.ene_com.rect_work[2].flag &= 4294967291U;
        ene3d.ene_com.rect_work[2].flag |= 1024U;
        work.ene_3d.ene_com.rect_work[1].flag |= 1024U;
        AppMain.ObjRectWorkSet(ene3d.ene_com.rect_work[0], (short)-28, (short)-28, (short)28, (short)24);
        ene3d.ene_com.rect_work[0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss3BodyDefFunc);
        ene3d.ene_com.rect_work[0].flag |= 1024U;
        AppMain.gmBoss3BodySetRectNormal(work);
        objWork.pos.z = 655360;
        objWork.flag |= 16U;
        objWork.disp_flag |= 4194309U;
        objWork.move_flag &= 4294967167U;
        objWork.move_flag |= 53776U;
        work.is_move = 0;
        objWork.obj_3d.blend_spd = 0.125f;
        AppMain.ObjDrawObjectSetToon(objWork);
        objWork.disp_flag |= 134217728U;
        objWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3BodyMainFuncWaitSetup);
        objWork.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3BodyOutFunc);
        objWork.ppMove = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3BodyChaseMoveFunc);
        AppMain.gmBoss3BodyChangeState(work, 0);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss3EggInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_ENEMY_3D_WORK ene3d = ((AppMain.GMS_BOSS3_EGG_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS3_EGG_WORK()), "BOSS3_EGG")).ene_3d;
        AppMain.OBS_OBJECT_WORK objWork = ene3d.ene_com.obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.gm_boss3_obj_3d_list[1], ene3d.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(objWork, 0, true, AppMain.ObjDataGet(729), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        ene3d.ene_com.rect_work[1].flag |= 3072U;
        ene3d.ene_com.rect_work[0].flag |= 3072U;
        ene3d.ene_com.rect_work[2].flag |= 3072U;
        objWork.flag |= 16U;
        objWork.disp_flag |= 4194309U;
        objWork.move_flag |= 4352U;
        objWork.move_flag &= 4294967167U;
        objWork.obj_3d.blend_spd = 0.125f;
        AppMain.ObjDrawObjectSetToon(objWork);
        objWork.disp_flag |= 134217728U;
        ene3d.ene_com.enemy_flag |= 32768U;
        objWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3EggmanMainFuncWaitSetup);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return objWork;
    }

    private static void gmBoss3ChangeTextureBurnt(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d.drawflag |= 268435456U;
        obj_work.obj_3d.draw_state.texoffset[0].mode = 2;
        obj_work.obj_3d.draw_state.texoffset[0].u = 0.5f;
    }

    private static void gmBoss3ExitFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmBoss3MgrDeleteObject(AppMain.mtTaskGetTcbWork(tcb));
        AppMain.GmEnemyDefaultExit(tcb);
    }

}