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
    private static void GmBoss2Build()
    {
        AppMain.AMS_AMB_HEADER gameDatEnemyArc = AppMain.GmBoss2GetGameDatEnemyArc();
        AppMain.gm_boss2_obj_3d_list = AppMain.GmGameDBuildRegBuildModel((AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, gameDatEnemyArc), (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 1, gameDatEnemyArc), 0U);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(711), 2, gameDatEnemyArc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(713), 4, gameDatEnemyArc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(712), 3, gameDatEnemyArc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(714), 6, gameDatEnemyArc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(715), 7, gameDatEnemyArc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(716), 8, gameDatEnemyArc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(717), 9, gameDatEnemyArc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(718), 10, gameDatEnemyArc);
        AppMain.GmEfctBossBuildSingleDataReg(14, AppMain.ObjDataGet(722), AppMain.ObjDataGet(723), 0, (AppMain.OBS_DATA_WORK)null, (AppMain.OBS_DATA_WORK)null, gameDatEnemyArc);
        AppMain.GmEfctBossBuildSingleDataReg(14, AppMain.ObjDataGet(722), AppMain.ObjDataGet(723), 0, (AppMain.OBS_DATA_WORK)null, (AppMain.OBS_DATA_WORK)null, gameDatEnemyArc);
        AppMain.GmEfctBossBuildSingleDataReg(14, AppMain.ObjDataGet(722), AppMain.ObjDataGet(723), 0, (AppMain.OBS_DATA_WORK)null, (AppMain.OBS_DATA_WORK)null, gameDatEnemyArc);
        AppMain.GmEfctBossBuildSingleDataReg(14, AppMain.ObjDataGet(722), AppMain.ObjDataGet(723), 0, (AppMain.OBS_DATA_WORK)null, (AppMain.OBS_DATA_WORK)null, gameDatEnemyArc);
        AppMain.GmEfctBossBuildSingleDataReg(14, AppMain.ObjDataGet(722), AppMain.ObjDataGet(723), 0, (AppMain.OBS_DATA_WORK)null, (AppMain.OBS_DATA_WORK)null, gameDatEnemyArc);
        AppMain.GmEfctBossBuildSingleDataReg(15, AppMain.ObjDataGet(726), AppMain.ObjDataGet(727), 5, AppMain.ObjDataGet(725), AppMain.ObjDataGet(724), gameDatEnemyArc);
        AppMain.GmEfctBossBuildSingleDataReg(14, AppMain.ObjDataGet(722), AppMain.ObjDataGet(723), 0, (AppMain.OBS_DATA_WORK)null, (AppMain.OBS_DATA_WORK)null, gameDatEnemyArc);
        AppMain.GmEfctBossBuildSingleDataReg(15, AppMain.ObjDataGet(726), AppMain.ObjDataGet(727), 5, AppMain.ObjDataGet(725), AppMain.ObjDataGet(724), gameDatEnemyArc);
    }

    private static void GmBoss2Flush()
    {
        AppMain.GmEfctBossFlushSingleDataInit();
        AppMain.ObjDataRelease(AppMain.ObjDataGet(718));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(717));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(716));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(715));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(714));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(713));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(711));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(712));
        AppMain.AMS_AMB_HEADER amsAmbHeader = (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, AppMain.GmBoss2GetGameDatEnemyArc());
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_boss2_obj_3d_list, amsAmbHeader.file_num);
        AppMain.gm_boss2_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.AMS_AMB_HEADER GmBoss2GetGameDatEnemyArc()
    {
        return AppMain.g_gm_gamedat_enemy_arc;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss2Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_BOSS2_MGR_WORK work = (AppMain.GMS_BOSS2_MGR_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS2_MGR_WORK()), "BOSS2_MGR");
        AppMain.OBS_OBJECT_WORK objWork = work.ene_3d.ene_com.obj_work;
        objWork.flag |= 16U;
        objWork.disp_flag |= 32U;
        objWork.move_flag |= 8448U;
        work.ene_3d.ene_com.enemy_flag |= 32768U;
        objWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2MgrMainFuncWaitLoad);
        work.life = AppMain.GmBsCmnIsFinalZoneType(objWork) == 0 ? 8 : 4;
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss2BodyInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_BOSS2_BODY_WORK work = (AppMain.GMS_BOSS2_BODY_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS2_BODY_WORK()), "BOSS2_BODY");
        AppMain.GMS_ENEMY_3D_WORK ene3d = work.ene_3d;
        AppMain.OBS_OBJECT_WORK objWork = ene3d.ene_com.obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.gm_boss2_obj_3d_list[1], ene3d.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(objWork, 0, true, AppMain.ObjDataGet(711), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        ene3d.ene_com.vit = (byte)1;
        AppMain.ObjRectWorkSet(ene3d.ene_com.rect_work[2], (short)-16, (short)-16, (short)16, (short)16);
        AppMain.ObjRectGroupSet(ene3d.ene_com.rect_work[2], (byte)1, (byte)3);
        ene3d.ene_com.rect_work[2].flag &= 4294967291U;
        ene3d.ene_com.rect_work[2].flag |= 1024U;
        work.ene_3d.ene_com.rect_work[1].flag |= 1024U;
        AppMain.ObjRectWorkSet(ene3d.ene_com.rect_work[0], (short)-26, (short)-26, (short)26, (short)26);
        ene3d.ene_com.rect_work[0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss2BodyDefFunc);
        ene3d.ene_com.rect_work[0].flag |= 1024U;
        AppMain.gmBoss2BodySetRectArm(work);
        AppMain.gmBoss2BodySetRectNormal(work);
        AppMain.ObjObjectFieldRectSet(objWork, (short)-24, (short)-24, (short)24, (short)24);
        objWork.pos.z = 131072;
        objWork.flag |= 16U;
        objWork.disp_flag |= 4194309U;
        objWork.move_flag &= 4294967167U;
        objWork.move_flag |= 49168U;
        objWork.obj_3d.blend_spd = 0.125f;
        AppMain.ObjDrawObjectSetToon(objWork);
        objWork.disp_flag |= 134217728U;
        work.se_handle = AppMain.GsSoundAllocSeHandle();
        objWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2BodyMainFuncWaitSetup);
        objWork.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2BodyOutFunc);
        AppMain.gmBoss2BodyChangeState(work, 0);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss2EggInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_ENEMY_3D_WORK ene3d = ((AppMain.GMS_BOSS2_EGG_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS2_EGG_WORK()), "BOSS2_EGG")).ene_3d;
        AppMain.OBS_OBJECT_WORK objWork = ene3d.ene_com.obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.gm_boss2_obj_3d_list[2], ene3d.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(objWork, 0, true, AppMain.ObjDataGet(713), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
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
        objWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EggmanMainFuncWaitSetup);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss2BallInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_ENEMY_3D_WORK ene3d = ((AppMain.GMS_BOSS2_BALL_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS2_BALL_WORK()), "BOSS2_BALL")).ene_3d;
        AppMain.OBS_OBJECT_WORK objWork = ene3d.ene_com.obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.gm_boss2_obj_3d_list[0], ene3d.obj_3d);
        AppMain.ObjRectWorkSet(ene3d.ene_com.rect_work[1], (short)-8, (short)-8, (short)8, (short)8);
        ene3d.ene_com.rect_work[1].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss2BallHitFunc);
        ene3d.ene_com.rect_work[1].flag |= 1024U;
        ene3d.ene_com.rect_work[0].flag |= 2048U;
        ene3d.ene_com.rect_work[2].flag |= 2048U;
        ene3d.ene_com.enemy_flag |= 32768U;
        objWork.disp_flag |= 4194304U;
        objWork.obj_3d.blend_spd = 0.125f;
        AppMain.ObjDrawObjectSetToon(objWork);
        objWork.disp_flag |= 134217728U;
        AppMain.ObjObjectFieldRectSet(objWork, (short)-4, (short)-8, (short)4, (short)6);
        objWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2BallMainFuncWaitSetup);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return objWork;
    }

    private static void gmBoss2ChangeTextureBurnt(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d.drawflag |= 268435456U;
        obj_work.obj_3d.draw_state.texoffset[0].mode = 2;
        obj_work.obj_3d.draw_state.texoffset[0].u = 0.5f;
    }

    private static void gmBoss2ExitFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmBoss2MgrDeleteObject(AppMain.mtTaskGetTcbWork(tcb));
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss2EffectExitFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmBoss2MgrDeleteObject(AppMain.mtTaskGetTcbWork(tcb));
        AppMain.GmEffectDefaultExit(tcb);
    }

    private static int gmBoss2CheckScrollLocked()
    {
        return ((int)AppMain.g_gm_main_system.game_flag & 32768) != 0 ? 1 : 0;
    }

    private static int gmBoss2MgrCheckSetupComplete(AppMain.GMS_BOSS2_MGR_WORK mgr_work)
    {
        return ((int)mgr_work.flag & 1) != 0 ? 1 : 0;
    }

    private static AppMain.GMS_BOSS2_MGR_WORK gmBoss2MgrGetMgrWork(
      AppMain.OBS_OBJECT_WORK obj_work_parts)
    {
        return (AppMain.GMS_BOSS2_MGR_WORK)obj_work_parts.user_work_OBJECT;
    }

    private static void gmBoss2MgrAddObject(
      AppMain.GMS_BOSS2_MGR_WORK mgr_work,
      AppMain.OBS_OBJECT_WORK obj_work_parts)
    {
        ++mgr_work.obj_create_count;
        obj_work_parts.user_work_OBJECT = (object)mgr_work;
    }

    private static void gmBoss2MgrDeleteObject(AppMain.OBS_OBJECT_WORK obj_work_parts)
    {
        --AppMain.gmBoss2MgrGetMgrWork(obj_work_parts).obj_create_count;
        obj_work_parts.user_work = 0U;
    }

    private static void gmBoss2MgrMainFuncWaitLoad(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0 && !AppMain.GmMainDatLoadBossBattleLoadCheck(1))
            return;
        AppMain.GMS_BOSS2_MGR_WORK mgr_work = (AppMain.GMS_BOSS2_MGR_WORK)obj_work;
        AppMain.GMS_BOSS2_BODY_WORK gmsBosS2BodyWork = (AppMain.GMS_BOSS2_BODY_WORK)AppMain.GmEventMgrLocalEventBirth((ushort)316, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        AppMain.OBS_OBJECT_WORK objWork1 = gmsBosS2BodyWork.ene_3d.ene_com.obj_work;
        objWork1.parent_obj = obj_work;
        gmsBosS2BodyWork.parts_objs[0] = objWork1;
        mgr_work.body_work = gmsBosS2BodyWork;
        AppMain.gmBoss2MgrAddObject(mgr_work, objWork1);
        AppMain.mtTaskChangeTcbDestructor(objWork1.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss2BodyExit));
        AppMain.OBS_OBJECT_WORK objWork2 = ((AppMain.GMS_BOSS2_EGG_WORK)AppMain.GmEventMgrLocalEventBirth((ushort)317, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0)).ene_3d.ene_com.obj_work;
        objWork2.parent_obj = objWork1;
        AppMain.gmBoss2MgrAddObject(mgr_work, objWork2);
        AppMain.mtTaskChangeTcbDestructor(objWork2.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss2ExitFunc));
        gmsBosS2BodyWork.parts_objs[1] = objWork2;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2MgrMainFuncWaitSetup);
    }

    private static void gmBoss2MgrMainFuncWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_MGR_WORK gmsBosS2MgrWork = (AppMain.GMS_BOSS2_MGR_WORK)obj_work;
        AppMain.GMS_BOSS2_BODY_WORK bodyWork = gmsBosS2MgrWork.body_work;
        for (int index = 0; 2 > index; ++index)
        {
            if (bodyWork.parts_objs[index] == null)
                return;
        }
        gmsBosS2MgrWork.flag |= 1U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2MgrMainFunc);
    }

    private static void gmBoss2MgrMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_MGR_WORK gmsBosS2MgrWork = (AppMain.GMS_BOSS2_MGR_WORK)obj_work;
        if (((int)gmsBosS2MgrWork.flag & 2) == 0)
            return;
        AppMain.GMM_BS_OBJ((object)gmsBosS2MgrWork.body_work).flag |= 8U;
        gmsBosS2MgrWork.body_work = (AppMain.GMS_BOSS2_BODY_WORK)null;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2MgrMainFuncWaitRelease);
    }

    private static void gmBoss2MgrMainFuncWaitRelease(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_MGR_WORK gmsBosS2MgrWork = (AppMain.GMS_BOSS2_MGR_WORK)obj_work;
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
        {
            if (gmsBosS2MgrWork.obj_create_count > 0)
                return;
            ((AppMain.GMS_ENEMY_COM_WORK)obj_work).enemy_flag |= 65536U;
            obj_work.flag |= 4U;
            AppMain.GmGameDatReleaseBossBattleStart(1);
            AppMain.GmGmkCamScrLimitRelease((byte)14);
            AppMain.OBS_OBJECT_WORK obj_work1 = AppMain.gmBoss2BodySearchShutterOut();
            if (obj_work1 != null)
                AppMain.GmGmkShutterOutChangeModeOpen(obj_work1);
        }
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmBoss2BodyExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_BOSS2_BODY_WORK tcbWork = (AppMain.GMS_BOSS2_BODY_WORK)AppMain.mtTaskGetTcbWork(tcb);
        AppMain.OBS_OBJECT_WORK objWork = tcbWork.ene_3d.ene_com.obj_work;
        AppMain.GmBsCmnClearBossMotionCBSystem(objWork);
        AppMain.GmBsCmnDeleteSNMWork(tcbWork.snm_work);
        AppMain.GmBsCmnClearCNMCb(objWork);
        AppMain.GmBsCmnDeleteCNMMgrWork(tcbWork.cnm_mgr_work);
        if (tcbWork.se_handle != null)
        {
            AppMain.GmSoundStopSE(tcbWork.se_handle);
            AppMain.GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.gmBoss2ExitFunc(tcb);
    }

    private static void gmBoss2BodyReactionPlayer(
      AppMain.OBS_OBJECT_WORK obj_work_player,
      AppMain.OBS_OBJECT_WORK obj_work_body)
    {
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)obj_work_player;
        AppMain.GmPlySeqAtkReactionInit(ply_work);
        AppMain.GmPlySeqSetJumpState(ply_work, 0, 5U);
        obj_work_player.spd_m = 0;
        obj_work_player.spd.x = obj_work_player.move.x < 0 ? 20480 : -20480;
        obj_work_player.spd.y = obj_work_player.pos.y > obj_work_body.pos.y ? 16384 : -16384;
        AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, 102400);
    }

    private static void gmBoss2BodyRecFuncRegistArmRect(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK gmsBosS2BodyWork = (AppMain.GMS_BOSS2_BODY_WORK)obj_work;
        AppMain.ObjObjectRectRegist(obj_work, gmsBosS2BodyWork.rect_work_arm);
    }

    private static void gmBoss2BodyCatchChangeArmRectNormal(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_RECT_WORK rectWorkArm = body_work.rect_work_arm;
        rectWorkArm.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        rectWorkArm.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(rectWorkArm, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(rectWorkArm, (ushort)0, (short)0);
        rectWorkArm.flag &= 4294967291U;
    }

    private static void gmBoss2BodyCatchChangeArmRectActive(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_RECT_WORK rectWorkArm = body_work.rect_work_arm;
        rectWorkArm.ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss2BodyHitFunc);
        rectWorkArm.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(rectWorkArm, (ushort)2, (short)1);
        AppMain.ObjRectDefSet(rectWorkArm, ushort.MaxValue, (short)0);
        rectWorkArm.flag |= 4U;
    }

    private static void gmBoss2BodyCatchChangeArmRectCatch(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_RECT_WORK rectWorkArm = body_work.rect_work_arm;
        rectWorkArm.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        rectWorkArm.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss2BodyCatchHitFuncArmCatch);
        AppMain.ObjRectAtkSet(rectWorkArm, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(rectWorkArm, (ushort)65534, (short)0);
        rectWorkArm.flag |= 4U;
    }

    private static void gmBoss2BodySetRectNormal(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], (short)0, (short)0, (short)0, (short)0);
        body_work.ene_3d.ene_com.rect_work[0].flag |= 4U;
        AppMain.gmBoss2BodyCatchChangeArmRectNormal(body_work);
    }

    private static void gmBoss2BodySetRectActive(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], (short)-8, (short)-8, (short)8, (short)8);
        body_work.ene_3d.ene_com.rect_work[0].flag |= 4U;
        AppMain.gmBoss2BodyCatchChangeArmRectActive(body_work);
    }

    private static void gmBoss2BodySetRectRoll(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], (short)-36, (short)-36, (short)36, (short)36);
        body_work.ene_3d.ene_com.rect_work[0].flag &= 4294967291U;
        AppMain.gmBoss2BodyCatchChangeArmRectNormal(body_work);
    }

    private static void gmBoss2BodySetRectArm(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)body_work;
        AppMain.OBS_RECT_WORK rectWorkArm = body_work.rect_work_arm;
        AppMain.ObjRectGroupSet(rectWorkArm, (byte)1, (byte)1);
        AppMain.gmBoss2BodyRectApplyOffsetArm(body_work);
        rectWorkArm.ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss2BodyHitFunc);
        rectWorkArm.parent_obj = obsObjectWork;
        rectWorkArm.flag |= 1028U;
        obsObjectWork.ppRec = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2BodyRecFuncRegistArmRect);
        AppMain.gmBoss2BodyCatchChangeArmRectNormal(body_work);
    }

    private static void gmBoss2BodyRectApplyOffsetArm(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_RECT_WORK rectWorkArm = body_work.rect_work_arm;
        uint flag = rectWorkArm.flag;
        AppMain.ObjRectWorkSet(rectWorkArm, (short)-40, (short)(24.0 - (double)body_work.offset_arm), (short)40, (short)(40.0 - (double)body_work.offset_arm));
        rectWorkArm.flag = flag;
    }

    private static void gmBoss2BodySetActionAllParts(
      AppMain.GMS_BOSS2_BODY_WORK body_work,
      int action_id)
    {
        AppMain.gmBoss2BodySetActionAllParts(body_work, action_id, 0);
    }

    private static void gmBoss2BodySetActionAllParts(
      AppMain.GMS_BOSS2_BODY_WORK body_work,
      int action_id,
      int force_change)
    {
        if (force_change == 0 && body_work.action_id == action_id)
            return;
        body_work.action_id = action_id;
        for (int index = 0; 2 > index; ++index)
        {
            AppMain.OBS_OBJECT_WORK partsObj = body_work.parts_objs[index];
            if (partsObj != null)
            {
                AppMain.GMS_BOSS2_PART_ACT_INFO bosS2PartActInfo = AppMain.gm_boss2_act_info_tbl[action_id][index];
                if (index != 1 || ((int)((AppMain.GMS_BOSS2_EGG_WORK)partsObj).flag & 1) == 0)
                {
                    if (bosS2PartActInfo.is_maintain != (byte)0)
                    {
                        if (bosS2PartActInfo.is_repeat != (byte)0)
                            partsObj.disp_flag |= 4U;
                    }
                    else
                        AppMain.GmBsCmnSetAction(partsObj, (int)bosS2PartActInfo.mtn_id, (int)bosS2PartActInfo.is_repeat, bosS2PartActInfo.is_blend);
                    partsObj.obj_3d.speed[0] = bosS2PartActInfo.mtn_spd;
                    partsObj.obj_3d.blend_spd = bosS2PartActInfo.blend_spd;
                }
            }
        }
    }

    private static void gmBoss2BodyOutFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK gmsBosS2BodyWork = (AppMain.GMS_BOSS2_BODY_WORK)obj_work;
        AppMain.GmBsCmnUpdateCNMParam(obj_work, gmsBosS2BodyWork.cnm_mgr_work);
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void gmBoss2BodyDefFunc(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = target_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = own_rect.parent_obj;
        AppMain.GMS_BOSS2_BODY_WORK body_work = (AppMain.GMS_BOSS2_BODY_WORK)parentObj2;
        if (parentObj1 == null || (ushort)1 != parentObj1.obj_type)
            return;
        AppMain.gmBoss2BodyReactionPlayer(parentObj1, parentObj2);
        AppMain.gmBoss2BodySetNoHitTime(body_work, 10U);
        if (((int)body_work.rect_work_arm.flag & 4) != 0 && parentObj2.pos.y < parentObj1.pos.y)
            return;
        AppMain.gmBoss2BodyDamage(body_work);
    }

    private static void gmBoss2BodyHitFunc(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.UNREFERENCED_PARAMETER((object)target_rect);
        ((AppMain.GMS_BOSS2_BODY_WORK)own_rect.parent_obj).flag |= 268435456U;
    }

    private static AppMain.OBS_OBJECT_WORK gmBoss2BodySearchShutterIn()
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)3);
        while (obj_work != null && ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id != (ushort)261)
            obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, (ushort)3);
        return obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK gmBoss2BodySearchShutterOut()
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)3);
        while (obj_work != null && ((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec.id != (ushort)262)
            obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, (ushort)3);
        return obj_work;
    }

    private static void gmBoss2BodySetNoHitTime(AppMain.GMS_BOSS2_BODY_WORK body_work, uint time)
    {
        AppMain.GMS_ENEMY_COM_WORK eneCom = body_work.ene_3d.ene_com;
        body_work.counter_no_hit = time;
        eneCom.rect_work[0].flag |= 2048U;
    }

    private static void gmBoss2BodyUpdateNoHitTime(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (body_work.counter_no_hit > 0U)
            --body_work.counter_no_hit;
        else
            body_work.ene_3d.ene_com.rect_work[0].flag &= 4294965247U;
    }

    private static void gmBoss2BodySetInvincibleTime(AppMain.GMS_BOSS2_BODY_WORK body_work, uint time)
    {
        body_work.counter_invincible = time;
        body_work.flag |= 1U;
    }

    private static void gmBoss2BodyUpdateInvincibleTime(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (body_work.counter_invincible > 0U)
            --body_work.counter_invincible;
        else
            body_work.flag &= 4294967294U;
    }

    private static void gmBoss2BodySetDirection(AppMain.GMS_BOSS2_BODY_WORK body_work, short deg)
    {
        body_work.angle_current = deg;
    }

    private static void gmBoss2BodySetDirectionNormal(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) != 0)
            AppMain.gmBoss2BodySetDirection(body_work, AppMain.GMD_BOSS2_ANGLE_LEFT);
        else
            AppMain.gmBoss2BodySetDirection(body_work, AppMain.GMD_BOSS2_ANGLE_RIGHT);
    }

    private static void gmBoss2BodySetDirectionRoll(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) != 0)
            AppMain.gmBoss2BodySetDirection(body_work, AppMain.GMD_BOSS2_BODY_PINBALL_ANGLE_LEFT_ROLL);
        else
            AppMain.gmBoss2BodySetDirection(body_work, AppMain.GMD_BOSS2_BODY_PINBALL_ANGLE_RIGHT_ROLL);
    }

    private static void gmBoss2BodyUpdateDirection(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)body_work).dir.y = (ushort)body_work.angle_current;
    }

    private static float gmBoss2BodyCalcMoveXNormalFrame(
      AppMain.GMS_BOSS2_BODY_WORK body_work,
      int x,
      int speed)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        return (float)AppMain.MTM_MATH_ABS(x - obsObjectWork.pos.x) / (float)speed;
    }

    private static void gmBoss2BodyInitMoveNormal(
      AppMain.GMS_BOSS2_BODY_WORK body_work,
      AppMain.VecFx32 dest_pos,
      float frame)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        body_work.start_pos.x = obsObjectWork.pos.x;
        body_work.start_pos.y = obsObjectWork.pos.y;
        body_work.start_pos.z = obsObjectWork.pos.z;
        body_work.end_pos.x = dest_pos.x;
        body_work.end_pos.y = dest_pos.y;
        body_work.end_pos.z = body_work.start_pos.z;
        body_work.move_counter = 0.0f;
        if ((double)frame > 0.0)
            body_work.move_frame = frame;
        else
            body_work.move_frame = 1f;
    }

    private static float gmBoss2BodyUpdateMoveNormal(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.VecFx32 vecFx32_1 = new AppMain.VecFx32();
        ++body_work.move_counter;
        if ((double)body_work.move_counter >= (double)body_work.move_frame)
        {
            vecFx32_1.x = body_work.end_pos.x;
            vecFx32_1.y = body_work.end_pos.y;
            vecFx32_1.z = body_work.end_pos.z;
        }
        else
        {
            float num = (float)(0.5 * (1.0 - (double)AppMain.nnCos(AppMain.AKM_DEGtoA32(180f * (body_work.move_counter / body_work.move_frame)))));
            AppMain.VecFx32 vecFx32_2 = new AppMain.VecFx32();
            vecFx32_2.x = (int)((double)(body_work.end_pos.x - body_work.start_pos.x) * (double)num);
            vecFx32_2.y = (int)((double)(body_work.end_pos.y - body_work.start_pos.y) * (double)num);
            vecFx32_2.z = (int)((double)(body_work.end_pos.z - body_work.start_pos.z) * (double)num);
            vecFx32_1.x = body_work.start_pos.x + vecFx32_2.x;
            vecFx32_1.y = body_work.start_pos.y + vecFx32_2.y;
            vecFx32_1.z = body_work.start_pos.z + vecFx32_2.z;
        }
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.pos.x = vecFx32_1.x;
        obsObjectWork.pos.y = vecFx32_1.y;
        obsObjectWork.pos.z = vecFx32_1.z;
        return body_work.move_frame - body_work.move_counter;
    }

    private static void gmBoss2BodyInitMovePinBall(
      AppMain.GMS_BOSS2_BODY_WORK body_work,
      AppMain.VecFx32 dir_pos,
      int speed)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        vecFx32.x = dir_pos.x - obsObjectWork.pos.x;
        vecFx32.y = dir_pos.y - obsObjectWork.pos.y;
        vecFx32.z = 0;
        if (vecFx32.x > 2048000)
            vecFx32.x = 2048000;
        else if (vecFx32.x < -2048000)
            vecFx32.x = -2048000;
        if (vecFx32.y > 2048000)
            vecFx32.y = 2048000;
        else if (vecFx32.y < -2048000)
            vecFx32.y = -2048000;
        int denom = AppMain.FX_Sqrt(AppMain.FX_Mul(vecFx32.x, vecFx32.x) + AppMain.FX_Mul(vecFx32.y, vecFx32.y));
        if (denom == 0)
            return;
        obsObjectWork.spd.x = AppMain.FX_Mul(AppMain.FX_Div(vecFx32.x, denom), speed);
        obsObjectWork.spd.y = AppMain.FX_Mul(AppMain.FX_Div(vecFx32.y, denom), speed);
    }

    private static int gmBoss2BodyPinBallCheckFieldUnder(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.move_flag & 1) != 0 ? 1 : 0;
    }

    private static int gmBoss2BodyPinBallCheckFieldOver(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.move_flag & 2) != 0 ? 1 : 0;
    }

    private static int gmBoss2BodyPinBallCheckFieldFront(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.move_flag & 4) != 0 ? 1 : 0;
    }

    private static int gmBoss2BodyPinBallCheckFieldBack(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.move_flag & 8) != 0 ? 1 : 0;
    }

    private static void gmBoss2BodyUpdateMovePinBall(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        int num1 = 0;
        int num2 = AppMain.MTM_MATH_ABS(obj_work.spd.x);
        int num3 = AppMain.MTM_MATH_ABS(obj_work.spd.y);
        if (AppMain.gmBoss2BodyPinBallCheckFieldUnder(obj_work) != 0)
        {
            obj_work.spd.y = -num3;
            if (AppMain.MTM_MATH_ABS(obj_work.spd.x) < 256)
                obj_work.spd.x = 256;
            obj_work.move_flag |= 32784U;
            num1 = 1;
        }
        else if (AppMain.gmBoss2BodyPinBallCheckFieldOver(obj_work) != 0)
        {
            obj_work.spd.y = num3;
            if (AppMain.MTM_MATH_ABS(obj_work.spd.x) < 256)
                obj_work.spd.x = 256;
            obj_work.move_flag |= 32784U;
            num1 = 1;
        }
        if (AppMain.gmBoss2BodyPinBallCheckFieldFront(obj_work) != 0 || AppMain.gmBoss2BodyPinBallCheckFieldBack(obj_work) != 0)
        {
            obj_work.spd.x = obj_work.spd.x >= 0 ? -num2 : num2;
            if (AppMain.MTM_MATH_ABS(obj_work.spd.y) < 256)
                obj_work.spd.y = 256;
            obj_work.move_flag |= 32784U;
            num1 = 1;
        }
        if (num1 != 0)
        {
            if (((int)body_work.flag & 128) != 0)
                return;
            AppMain.GmSoundPlaySE("Boss2_05");
            body_work.flag |= 128U;
        }
        else
            body_work.flag &= 4294967167U;
    }

    private static void gmBoss2BodyInitTurn(
      AppMain.GMS_BOSS2_BODY_WORK body_work,
      short dest_angle,
      float frame,
      int flag_positive)
    {
        body_work.turn_counter = 0.0f;
        body_work.turn_frame = frame;
        body_work.turn_start = body_work.angle_current;
        ushort num1 = (ushort)((uint)dest_angle - (uint)body_work.angle_current);
        body_work.turn_amount = (int)num1;
        if (flag_positive != 0)
            return;
        ushort num2 = (ushort)(body_work.turn_amount - AppMain.AKM_DEGtoA32(360));
        body_work.turn_amount = (int)num2 - AppMain.AKM_DEGtoA32(360);
    }

    private static float gmBoss2BodyUpdateTurn(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if ((double)body_work.turn_frame < 1.0)
            return 0.0f;
        ++body_work.turn_counter;
        short deg;
        if ((double)body_work.turn_counter >= (double)body_work.turn_frame)
        {
            deg = (short)((int)body_work.turn_start + (int)(short)body_work.turn_amount);
        }
        else
        {
            int ang = AppMain.AKM_DEGtoA32(180f * (body_work.turn_counter / body_work.turn_frame));
            float num = (float)((double)body_work.turn_amount * 0.5 * (1.0 - (double)AppMain.nnCos(ang)));
            deg = (short)((int)body_work.turn_start + (int)(short)num);
        }
        AppMain.gmBoss2BodySetDirection(body_work, deg);
        return body_work.turn_frame - body_work.turn_counter;
    }

    private static int gmBoss2BodyCatchArmCheckTarget(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        return ((int)body_work.flag & 16) != 0 || playerObj.pos.y < obsObjectWork.pos.y || AppMain.MTM_MATH_ABS(playerObj.pos.x - obsObjectWork.pos.x) > 32768 ? 0 : 1;
    }

    private static int gmBoss2BodyCatchArmCountReleaseKey(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.GMS_PLAYER_WORK playerObj = (AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj();
        int num = 0;
        for (int index = 0; 16 > index; ++index)
        {
            if (((int)byte.MaxValue & (int)playerObj.key_push & 1 << index) != 0)
            {
                ++body_work.count_release_key;
                num = 1;
            }
        }
        return num != 0 ? 1 : 0;
    }

    private static int gmBoss2BodyCatchArmCheckRelease(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        return body_work.count_release_key < 6 ? 0 : 1;
    }

    private static void gmBoss2BodyCatchArmUpdateShakePlayer(
      AppMain.GMS_BOSS2_BODY_WORK body_work,
      uint shake_count)
    {
        body_work.shake_pos += body_work.shake_speed;
        int num1 = AppMain.MTM_MATH_ABS(body_work.shake_pos);
        int num2 = AppMain.MTM_MATH_ABS(body_work.shake_speed);
        if (num1 > 5)
            body_work.shake_speed = -body_work.shake_speed;
        else if (num1 < num2)
            ++body_work.shake_count;
        if (body_work.shake_count < shake_count * 2U)
            return;
        body_work.shake_speed = 0;
        body_work.shake_pos = 0;
        body_work.shake_count = 0U;
    }

    private static void gmBoss2BodyCatchSetArmLength(
      AppMain.GMS_BOSS2_BODY_WORK body_work,
      float length)
    {
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(body_work.snm_work, body_work.snm_reg_id[2]);
        AppMain.AkMathExtractScaleMtx(new AppMain.NNS_MATRIX(), snmMtx);
        AppMain.NNS_MATRIX nnsMatrix = new AppMain.NNS_MATRIX();
        AppMain.nnMakeTranslateMatrix(nnsMatrix, 0.0f, length / snmMtx.M11, 0.0f);
        for (int index = 0; 13 > index; ++index)
        {
            int cnm_reg_id = body_work.cnm_reg_id[index];
            AppMain.GmBsCmnChangeCNMModeNode(body_work.cnm_mgr_work, cnm_reg_id, 1U);
            AppMain.GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix, cnm_reg_id);
            AppMain.GmBsCmnEnableCNMLocalCoordinate(body_work.cnm_mgr_work, cnm_reg_id, 1);
            AppMain.GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, cnm_reg_id, 1);
            AppMain.nnMakeUnitMatrix(nnsMatrix);
        }
    }

    private static void gmBoss2BodyCatchHitFuncArmCatch(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj1 = (AppMain.GMS_BOSS2_BODY_WORK)own_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != (ushort)1)
            return;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)parentObj2;
        own_rect.flag &= 4294967291U;
        AppMain.GmPlySeqGmkInitBoss2Catch(ply_work);
        parentObj1.flag |= 8U;
        parentObj1.flag |= 268435456U;
    }

    private static void gmBoss2BodyCatchChangeNeedleModeActive()
    {
        for (AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)3); obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, (ushort)3))
        {
            AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
            if (gmsEnemy3DWork == null)
            {
                if (((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec.id == (ushort)337)
                    AppMain.GmGmkNeedleNeonChangeModeActive(obj_work);
            }
            else if (gmsEnemy3DWork.ene_com.eve_rec.id == (ushort)337)
                AppMain.GmGmkNeedleNeonChangeModeActive(obj_work);
        }
    }

    private static void gmBoss2BodyCatchChangeNeedleModeWait()
    {
        for (AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)3); obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, (ushort)3))
        {
            AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
            if (gmsEnemy3DWork == null)
            {
                if (((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec.id == (ushort)337)
                    AppMain.GmGmkNeedleNeonChangeModeWait(obj_work);
            }
            else if (gmsEnemy3DWork.ene_com.eve_rec.id == (ushort)337)
                AppMain.GmGmkNeedleNeonChangeModeWait(obj_work);
        }
    }

    private static int gmBoss2BodyBallShootCheckTarget(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        return ((int)body_work.flag & 16) != 0 || playerObj.pos.y < obsObjectWork.pos.y || AppMain.MTM_MATH_ABS(playerObj.pos.x - obsObjectWork.pos.x) > 32768 ? 0 : 1;
    }

    private static int gmBoss2BodyPinBallCheckTurn(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (playerObj.pos.x < obsObjectWork.pos.x)
                return 0;
        }
        else if (obsObjectWork.pos.x < playerObj.pos.x)
            return 0;
        return 1;
    }

    private static void gmBoss2BodyPinBallAdjustMoveSpeed(
      AppMain.GMS_BOSS2_BODY_WORK body_work,
      int speed)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        int denom = AppMain.FX_Sqrt(AppMain.FX_Mul(obsObjectWork.spd.x, obsObjectWork.spd.x) + AppMain.FX_Mul(obsObjectWork.spd.y, obsObjectWork.spd.y));
        if (denom == 0)
        {
            vecFx32.x = 4096;
            vecFx32.y = 0;
        }
        else
        {
            vecFx32.x = AppMain.FX_Div(obsObjectWork.spd.x, denom);
            vecFx32.y = AppMain.FX_Div(obsObjectWork.spd.y, denom);
        }
        vecFx32.z = 0;
        int num = (speed - denom) / 10;
        obsObjectWork.spd.x = AppMain.FX_Mul(vecFx32.x, denom + num);
        obsObjectWork.spd.y = AppMain.FX_Mul(vecFx32.y, denom + num);
        obsObjectWork.spd.z = 0;
    }

    private static int gmBoss2BodyPinBallCheckAreaStop(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num1 = AppMain.g_gm_main_system.map_fcol.left + (AppMain.g_gm_main_system.map_fcol.right - AppMain.g_gm_main_system.map_fcol.left) / 2;
        int num2 = AppMain.g_gm_main_system.map_fcol.top + (AppMain.g_gm_main_system.map_fcol.bottom - AppMain.g_gm_main_system.map_fcol.top) / 2;
        return (num1 - 70) * 4096 < obsObjectWork.pos.x && obsObjectWork.pos.x < (num1 + 70) * 4096 && ((num2 - 110) * 4096 < obsObjectWork.pos.y && obsObjectWork.pos.y < (num2 + 110) * 4096) ? 0 : 1;
    }

    private static void gmBoss2BodyDamage(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work_parts = AppMain.GMM_BS_OBJ((object)body_work);
        if (((int)body_work.flag & 1) != 0)
            return;
        AppMain.GMS_BOSS2_MGR_WORK mgrWork = AppMain.gmBoss2MgrGetMgrWork(obj_work_parts);
        --mgrWork.life;
        if (mgrWork.life > 0)
            body_work.flag |= 1073741824U;
        else
            body_work.flag |= 2147483648U;
        AppMain.GmSoundPlaySE("Boss0_01");
        AppMain.gmBoss2EffDamageInit(body_work);
        AppMain.GMM_PAD_VIB_SMALL();
        AppMain.gmBoss2BodySetInvincibleTime(body_work, 90U);
    }

    private static int gmBoss2BodyEscapeCheckScrollUnlock(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        return AppMain.GMM_BS_OBJ((object)body_work).pos.y <= AppMain.g_gm_main_system.map_fcol.top * 4096 + 131072 ? 1 : 0;
    }

    private static int gmBoss2BodyEscapeCheckScreenOut(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        return AppMain.GMM_BS_OBJ((object)body_work).pos.y <= (AppMain.g_gm_main_system.map_fcol.top - 64) * 4096 ? 1 : 0;
    }

    private static void gmBoss2BodyEscapeAddjustSpeed(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.MTM_MATH_ABS(obsObjectWork.spd.x) <= -3276)
            return;
        obsObjectWork.spd.x = 0;
        obsObjectWork.spd.y = -3276;
        obsObjectWork.spd_add.x = 0;
        obsObjectWork.spd_add.y = 0;
    }

    private static void gmBoss2BodyChangeState(AppMain.GMS_BOSS2_BODY_WORK body_work, int state)
    {
        AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK gmsBosS2BodyWork1 = AppMain.gm_boss2_body_state_func_tbl_leave[body_work.state];
        if (gmsBosS2BodyWork1 != null)
            gmsBosS2BodyWork1(body_work);
        body_work.prev_state = body_work.state;
        body_work.state = state;
        AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK gmsBosS2BodyWork2 = AppMain.gm_boss2_body_state_func_tbl_enter[body_work.state];
        if (gmsBosS2BodyWork2 == null)
            return;
        gmsBosS2BodyWork2(body_work);
    }

    private static void gmBoss2BodyStateStartEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = body_work.ene_3d.ene_com.obj_work;
        objWork.flag |= 2U;
        AppMain.gmBoss2BodySetActionAllParts(body_work, 0, 1);
        AppMain.GmBsCmnSetObjSpdZero(objWork);
        AppMain.gmBoss2BodySetDirectionNormal(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateStartUpdateWait);
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
    }

    private static void gmBoss2BodyStateStartLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)body_work).flag &= 4294967293U;
        body_work.flag &= 4294967279U;
        body_work.ene_3d.ene_com.enemy_flag &= 4294934527U;
    }

    private static void gmBoss2BodyStateStartUpdateWait(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (AppMain.gmBoss2CheckScrollLocked() == 0)
            return;
        AppMain.GmMapSetMapDrawSize(4);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateStartUpdateEnd);
    }

    private static void gmBoss2BodyStateStartUpdateEnd(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.ObjViewOutCheck(obsObjectWork.pos.x, obsObjectWork.pos.y, (short)0, (short)0, (short)0, (short)0, (short)0) != 0)
            return;
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 180)
            return;
        obsObjectWork.user_timer = 0;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmBoss2BodySearchShutterIn();
        if (obj_work != null)
            AppMain.GmGmkShutterInChangeModeClose(obj_work);
        AppMain.gmBoss2BodyChangeState(body_work, 2);
    }

    private static void gmBoss2BodyStateCatchMoveEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 1);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchMoveUpdateMove);
        AppMain.gmBoss2EffAfterburnerRequestCreate(body_work);
        AppMain.VecFx32 dest_pos = new AppMain.VecFx32(((int)obsObjectWork.disp_flag & 1) == 0 ? AppMain.g_gm_main_system.map_fcol.right * 4096 - 348160 : AppMain.g_gm_main_system.map_fcol.left * 4096 + 348160, obsObjectWork.pos.y, obsObjectWork.pos.z);
        int speed = 4915;
        float frame = AppMain.gmBoss2BodyCalcMoveXNormalFrame(body_work, dest_pos.x, speed);
        AppMain.gmBoss2BodyInitMoveNormal(body_work, dest_pos, frame);
    }

    private static void gmBoss2BodyStateCatchMoveLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.gmBoss2EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss2BodyStateCatchMoveUpdateMove(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss2BodySetDirectionNormal(body_work);
        AppMain.GMS_BOSS2_MGR_WORK mgrWork = AppMain.gmBoss2MgrGetMgrWork(obsObjectWork);
        if (AppMain.GmBsCmnIsFinalZoneType(obsObjectWork) != 0)
        {
            if (mgrWork.life <= 3)
            {
                AppMain.gmBoss2BodyChangeState(body_work, 5);
                return;
            }
        }
        else if (8 - mgrWork.life >= 3)
        {
            AppMain.gmBoss2BodyChangeState(body_work, 5);
            return;
        }
        if (AppMain.gmBoss2BodyCatchArmCheckTarget(body_work) != 0)
        {
            AppMain.gmBoss2BodyChangeState(body_work, 3);
        }
        else
        {
            if ((double)AppMain.gmBoss2BodyUpdateMoveNormal(body_work) > 60.0)
                return;
            short dest_angle;
            int flag_positive;
            if (((int)obsObjectWork.disp_flag & 1) != 0)
            {
                obsObjectWork.disp_flag &= 4294967294U;
                dest_angle = AppMain.GMD_BOSS2_ANGLE_RIGHT;
                flag_positive = 1;
            }
            else
            {
                obsObjectWork.disp_flag |= 1U;
                dest_angle = AppMain.GMD_BOSS2_ANGLE_LEFT;
                flag_positive = 0;
            }
            AppMain.gmBoss2BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
            body_work.flag &= 4294967279U;
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchMoveUpdateTurn);
            AppMain.gmBoss2EffAfterburnerRequestDelete(body_work);
        }
    }

    private static void gmBoss2BodyStateCatchMoveUpdateTurn(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        double num = (double)AppMain.gmBoss2BodyUpdateMoveNormal(body_work);
        if (0.0 < (double)AppMain.gmBoss2BodyUpdateTurn(body_work))
            return;
        AppMain.gmBoss2BodyChangeState(body_work, 2);
    }

    private static void gmBoss2BodyStateCatchArmEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 2);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchArmUpdateOpen);
        AppMain.gmBoss2EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss2BodyStateCatchArmLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.offset_arm = 0.0f;
        AppMain.gmBoss2BodyCatchSetArmLength(body_work, body_work.offset_arm);
        body_work.count_release_key = 0;
        body_work.flag |= 16U;
    }

    private static void gmBoss2BodyStateCatchArmUpdateOpen(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchArmUpdateReady);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 3);
    }

    private static void gmBoss2BodyStateCatchArmUpdateReady(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 5)
            return;
        obsObjectWork.user_timer = 0;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchArmUpdateDown);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 5);
        AppMain.gmBoss2BodyCatchChangeArmRectCatch(body_work);
        AppMain.GmSoundPlaySE("Boss2_01", body_work.se_handle);
    }

    private static void gmBoss2BodyStateCatchArmUpdateDown(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.offset_arm -= 6f;
        AppMain.gmBoss2BodyCatchSetArmLength(body_work, body_work.offset_arm);
        AppMain.gmBoss2BodyRectApplyOffsetArm(body_work);
        if (((int)body_work.flag & 8) == 0 && (double)body_work.offset_arm > -60.0)
            return;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchArmUpdateClose);
        AppMain.GmSoundStopSE(body_work.se_handle);
        AppMain.GmSoundPlaySE("Boss2_02");
    }

    private static void gmBoss2BodyStateCatchArmUpdateClose(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        if (((int)((AppMain.GMS_PLAYER_WORK)playerObj).player_flag & 1024) != 0)
            body_work.flag &= 4294967287U;
        if (((int)body_work.flag & 8) != 0)
        {
            AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(playerObj, body_work.snm_work, body_work.snm_reg_id[2], 1);
            playerObj.pos.y += -AppMain.FX_F32_TO_FX32(body_work.offset_arm) + 163840;
        }
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchArmUpdateUp);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 6);
        if (((int)body_work.flag & 8) != 0)
            return;
        AppMain.gmBoss2BodyCatchChangeArmRectNormal(body_work);
    }

    private static void gmBoss2BodyStateCatchArmUpdateUp(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)playerObj;
        ++body_work.offset_arm;
        AppMain.gmBoss2BodyCatchSetArmLength(body_work, body_work.offset_arm);
        AppMain.gmBoss2BodyRectApplyOffsetArm(body_work);
        AppMain.gmBoss2BodyCatchArmUpdateShakePlayer(body_work, 1U);
        obsObjectWork.dir.x = (ushort)AppMain.AKM_DEGtoA16(body_work.shake_pos);
        if (((int)ply_work.player_flag & 1024) != 0)
            body_work.flag &= 4294967287U;
        if (((int)body_work.flag & 8) != 0)
        {
            if (AppMain.gmBoss2BodyCatchArmCountReleaseKey(body_work) != 0 && body_work.shake_speed == 0)
                body_work.shake_speed = 2;
            AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(playerObj, body_work.snm_work, body_work.snm_reg_id[2], 1);
            playerObj.pos.y += -AppMain.FX_F32_TO_FX32(body_work.offset_arm) + 163840;
            if (AppMain.gmBoss2BodyCatchArmCheckRelease(body_work) != 0)
            {
                body_work.flag &= 4294967287U;
                AppMain.GmPlySeqChangeSequence(ply_work, 16);
                ply_work.player_flag |= 160U;
                playerObj.move_flag &= 4294958847U;
                playerObj.spd.x = 0;
                playerObj.spd.y = 0;
                playerObj.spd_add.x = 0;
                playerObj.spd_add.y = 0;
            }
        }
        if ((double)body_work.offset_arm < 0.0 || obsObjectWork.dir.x != (ushort)0)
            return;
        body_work.offset_arm = 0.0f;
        AppMain.gmBoss2BodyCatchSetArmLength(body_work, body_work.offset_arm);
        AppMain.gmBoss2BodyRectApplyOffsetArm(body_work);
        if (((int)body_work.flag & 8) != 0)
            AppMain.gmBoss2BodyChangeState(body_work, 4);
        else
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchArmUpdateEnd);
    }

    private static void gmBoss2BodyStateCatchArmUpdateEnd(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        ++obsObjectWork.user_timer;
        if ((double)obsObjectWork.user_timer < 10.0)
            return;
        obsObjectWork.user_timer = 0;
        AppMain.gmBoss2BodyChangeState(body_work, 2);
        AppMain.gmBoss2BodyCatchChangeArmRectNormal(body_work);
    }

    private static void gmBoss2BodyStateCatchCarryEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 7);
        int num1 = AppMain.g_gm_main_system.map_fcol.right - AppMain.g_gm_main_system.map_fcol.left;
        int _x = (AppMain.g_gm_main_system.map_fcol.left + num1 / 2) * 4096;
        AppMain.VecFx32 dest_pos = new AppMain.VecFx32(_x, obsObjectWork.pos.y, obsObjectWork.pos.z);
        int speed = 4915;
        float frame = AppMain.gmBoss2BodyCalcMoveXNormalFrame(body_work, dest_pos.x, speed);
        AppMain.gmBoss2BodyInitMoveNormal(body_work, dest_pos, frame);
        AppMain.gmBoss2BodySetDirectionNormal(body_work);
        short dest_angle = 0;
        int flag_positive = 0;
        int num2 = 0;
        if (((int)obsObjectWork.disp_flag & 1) != 0 && _x - obsObjectWork.pos.x >= 0)
        {
            obsObjectWork.disp_flag &= 4294967294U;
            dest_angle = AppMain.GMD_BOSS2_ANGLE_RIGHT;
            flag_positive = 1;
            num2 = 1;
        }
        else if (((int)obsObjectWork.disp_flag & 1) == 0 && _x - obsObjectWork.pos.x < 0)
        {
            obsObjectWork.disp_flag |= 1U;
            dest_angle = AppMain.GMD_BOSS2_ANGLE_LEFT;
            flag_positive = 0;
            num2 = 1;
        }
        if (num2 != 0)
            AppMain.gmBoss2BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchCarryUpdateMove);
        AppMain.gmBoss2EffAfterburnerRequestCreate(body_work);
        AppMain.gmBoss2BodyCatchChangeNeedleModeActive();
    }

    private static void gmBoss2BodyStateCatchCarryLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.gmBoss2EffAfterburnerRequestDelete(body_work);
        body_work.flag &= 4294967287U;
        AppMain.gmBoss2BodyCatchChangeArmRectNormal(body_work);
        AppMain.gmBoss2BodyCatchChangeNeedleModeWait();
    }

    private static void gmBoss2BodyStateCatchCarryUpdateMove(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        if (((int)((AppMain.GMS_PLAYER_WORK)playerObj).player_flag & 1024) != 0)
            body_work.flag &= 4294967287U;
        if (((int)body_work.flag & 8) != 0)
        {
            AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(playerObj, body_work.snm_work, body_work.snm_reg_id[2], 1);
            playerObj.pos.y += -AppMain.FX_F32_TO_FX32(body_work.offset_arm) + 163840;
        }
        float num = AppMain.gmBoss2BodyUpdateTurn(body_work);
        if (0.0 < (double)AppMain.gmBoss2BodyUpdateMoveNormal(body_work) || 0.0 < (double)num)
            return;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchCarryUpdateOpen);
        AppMain.gmBoss2EffAfterburnerRequestDelete(body_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 8);
    }

    private static void gmBoss2BodyStateCatchCarryUpdateOpen(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)playerObj;
        ++obsObjectWork.user_timer;
        if ((double)obsObjectWork.user_timer < 40.0)
            return;
        obsObjectWork.user_timer = 0;
        if (((int)ply_work.player_flag & 1024) != 0)
            body_work.flag &= 4294967287U;
        if (((int)body_work.flag & 8) != 0)
        {
            body_work.flag &= 4294967287U;
            AppMain.GmPlySeqChangeSequence(ply_work, 16);
            ply_work.player_flag |= 160U;
            playerObj.move_flag &= 4294958847U;
            playerObj.spd.x = 0;
            playerObj.spd.y = 0;
            playerObj.spd_add.x = 0;
            playerObj.spd_add.y = 0;
        }
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateCatchCarryUpdateEnd);
    }

    private static void gmBoss2BodyStateCatchCarryUpdateEnd(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.gmBoss2BodyChangeState(body_work, 2);
    }

    private static void gmBoss2BodyStatePreBallEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.gmBoss2BodySetActionAllParts(body_work, 17, 1);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePreBallUpdateAngry);
        AppMain.gmBoss2EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss2BodyStatePreBallLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.gmBoss2EffAfterburnerRequestDelete(body_work);
        body_work.flag &= 4294967279U;
    }

    private static void gmBoss2BodyStatePreBallUpdateAngry(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePreBallUpdateRise);
        AppMain.VecFx32 dest_pos = new AppMain.VecFx32(obj_work.pos.x, obj_work.pos.y - 409600, obj_work.pos.z);
        AppMain.gmBoss2BodyInitMoveNormal(body_work, dest_pos, 60f);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePreBallUpdateRise);
        AppMain.GmCameraScaleSet(0.82f, 3f / 1000f);
    }

    private static void gmBoss2BodyStatePreBallUpdateRise(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (0.0 < (double)AppMain.gmBoss2BodyUpdateMoveNormal(body_work))
            return;
        AppMain.gmBoss2BodyChangeState(body_work, 6);
    }

    private static void gmBoss2BodyStateBallMoveEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 9);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateBallMoveUpdateMove);
        AppMain.gmBoss2EffAfterburnerRequestCreate(body_work);
        AppMain.VecFx32 dest_pos = new AppMain.VecFx32(((int)obsObjectWork.disp_flag & 1) == 0 ? AppMain.g_gm_main_system.map_fcol.right * 4096 - 348160 : AppMain.g_gm_main_system.map_fcol.left * 4096 + 348160, obsObjectWork.pos.y, obsObjectWork.pos.z);
        int speed = 4915;
        float frame = AppMain.gmBoss2BodyCalcMoveXNormalFrame(body_work, dest_pos.x, speed);
        AppMain.gmBoss2BodyInitMoveNormal(body_work, dest_pos, frame);
    }

    private static void gmBoss2BodyStateBallMoveLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.gmBoss2EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss2BodyStateBallMoveUpdateMove(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss2BodySetDirectionNormal(body_work);
        AppMain.GMS_BOSS2_MGR_WORK mgrWork = AppMain.gmBoss2MgrGetMgrWork(obsObjectWork);
        if (AppMain.GmBsCmnIsFinalZoneType(obsObjectWork) != 0)
            AppMain.gmBoss2BodyChangeState(body_work, 8);
        else if (8 - mgrWork.life >= 5)
        {
            if (AppMain.GmBsCmnIsActionEnd(obsObjectWork) == 0)
                return;
            AppMain.gmBoss2BodyChangeState(body_work, 8);
        }
        else if (AppMain.gmBoss2BodyBallShootCheckTarget(body_work) != 0)
        {
            AppMain.gmBoss2BodyChangeState(body_work, 7);
        }
        else
        {
            if ((double)AppMain.gmBoss2BodyUpdateMoveNormal(body_work) > 60.0)
                return;
            short dest_angle;
            int flag_positive;
            if (((int)obsObjectWork.disp_flag & 1) != 0)
            {
                obsObjectWork.disp_flag &= 4294967294U;
                dest_angle = AppMain.GMD_BOSS2_ANGLE_RIGHT;
                flag_positive = 1;
            }
            else
            {
                obsObjectWork.disp_flag |= 1U;
                dest_angle = AppMain.GMD_BOSS2_ANGLE_LEFT;
                flag_positive = 0;
            }
            AppMain.gmBoss2BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
            body_work.flag &= 4294967279U;
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateBallMoveUpdateTurn);
            AppMain.gmBoss2EffAfterburnerRequestDelete(body_work);
        }
    }

    private static void gmBoss2BodyStateBallMoveUpdateTurn(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        double num = (double)AppMain.gmBoss2BodyUpdateMoveNormal(body_work);
        if (0.0 < (double)AppMain.gmBoss2BodyUpdateTurn(body_work))
            return;
        AppMain.gmBoss2BodyChangeState(body_work, 6);
    }

    private static void gmBoss2BodyStateBallShootEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 10);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateBallShootUpdateWaitCreate);
        AppMain.gmBoss2EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss2BodyStateBallShootLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.flag |= 16U;
    }

    private static void gmBoss2BodyStateBallShootUpdateWaitCreate(
      AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work_parts1 = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK obj_work_parts2 = AppMain.GmEventMgrLocalEventBirth((ushort)318, obj_work_parts1.pos.x, obj_work_parts1.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        obj_work_parts2.parent_obj = obj_work_parts1;
        AppMain.gmBoss2MgrAddObject(AppMain.gmBoss2MgrGetMgrWork(obj_work_parts1), obj_work_parts2);
        AppMain.mtTaskChangeTcbDestructor(obj_work_parts2.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss2ExitFunc));
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateBallShootUpdateCatch);
    }

    private static void gmBoss2BodyStateBallShootUpdateCatch(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        body_work.flag |= 4194304U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateBallShootUpdateShoot);
    }

    private static void gmBoss2BodyStateBallShootUpdateShoot(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.gmBoss2BodyChangeState(body_work, 6);
    }

    private static void gmBoss2BodyStatePrePinBallEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 12, 1);
        body_work.ene_3d.ene_com.rect_work[2].flag |= 4U;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePrePinBallUpdateWaitEffect);
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
            return;
        AppMain.GmSoundChangeAngryBossBGM();
    }

    private static void gmBoss2BodyStatePrePinBallLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.flag &= 4294967279U;
    }

    private static void gmBoss2BodyStatePrePinBallUpdateWaitEffect(
      AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        ++obsObjectWork.user_timer;
        if ((double)obsObjectWork.user_timer < 119.0)
            return;
        obsObjectWork.user_timer = 0;
        AppMain.gmBoss2EffBlitzInit(body_work);
        AppMain.GmSoundPlaySE("FinalBoss11", body_work.se_handle);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePrePinBallUpdateWaitMotion);
    }

    private static void gmBoss2BodyStatePrePinBallUpdateWaitMotion(
      AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.gmBoss2BodyChangeState(body_work, 9);
    }

    private static void gmBoss2BodyStatePinBallMoveEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 15);
        obj_work.move_flag &= 4294963199U;
        AppMain.gmBoss2BodySetRectActive(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePinBallMoveUpdateMove);
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) == 0)
            return;
        AppMain.gmBoss2BodyCatchChangeNeedleModeActive();
    }

    private static void gmBoss2BodyStatePinBallMoveLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)body_work).move_flag |= 4096U;
    }

    private static void gmBoss2BodyStatePinBallMoveUpdateMove(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        int num = 0;
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
            num = AppMain.FX_Mul(num, 6144);
        AppMain.gmBoss2BodyPinBallAdjustMoveSpeed(body_work, num);
        AppMain.gmBoss2BodyUpdateMovePinBall(body_work);
        AppMain.gmBoss2BodySetDirectionNormal(body_work);
        ++body_work.counter_pinball;
        if (body_work.counter_pinball == 90U && AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
            AppMain.gmBoss2BodyCatchChangeNeedleModeWait();
        if (body_work.counter_pinball < 360U)
            return;
        body_work.counter_pinball = 0U;
        AppMain.gmBoss2BodyChangeState(body_work, 10);
    }

    private static void gmBoss2BodyStatePinBallRollEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        obj_work.move_flag &= 4294963199U;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 15);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePinBallRollUpdateSearch);
    }

    private static void gmBoss2BodyStatePinBallRollLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)body_work).move_flag |= 4096U;
        body_work.flag &= 4294967231U;
    }

    private static void gmBoss2BodyStatePinBallRollUpdateSearch(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.gmBoss2BodySetActionAllParts(body_work, 16);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePinBallRollUpdateFind);
    }

    private static void gmBoss2BodyStatePinBallRollUpdateFind(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        if (AppMain.gmBoss2BodyPinBallCheckTurn(body_work) != 0)
        {
            short dest_angle;
            int flag_positive;
            if (((int)obj_work.disp_flag & 1) != 0)
            {
                obj_work.disp_flag &= 4294967294U;
                dest_angle = AppMain.GMD_BOSS2_ANGLE_RIGHT;
                flag_positive = 1;
            }
            else
            {
                obj_work.disp_flag |= 1U;
                dest_angle = AppMain.GMD_BOSS2_ANGLE_LEFT;
                flag_positive = 0;
            }
            AppMain.gmBoss2BodyInitTurn(body_work, dest_angle, 20f, flag_positive);
        }
        AppMain.gmBoss2BodySetActionAllParts(body_work, 14);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePinBallRollReady);
    }

    private static void gmBoss2BodyStatePinBallRollReady(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        float num1 = AppMain.gmBoss2BodyUpdateTurn(body_work);
        ++obj_work.user_timer;
        if (obj_work.user_timer < 10 || 0.0 < (double)num1)
            return;
        obj_work.user_timer = 0;
        int num2 = 20480;
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
            num2 = AppMain.FX_Mul(num2, 6144);
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        AppMain.gmBoss2BodyInitMovePinBall(body_work, playerObj.pos, num2);
        AppMain.gmBoss2BodySetRectRoll(body_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 14);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePinBallRollUpdateMove);
        AppMain.gmBoss2EffCreateRollModel(body_work);
    }

    private static void gmBoss2BodyStatePinBallRollUpdateMove(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        int num = 20480;
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
            num = AppMain.FX_Mul(num, 6144);
        AppMain.gmBoss2BodyPinBallAdjustMoveSpeed(body_work, num);
        AppMain.gmBoss2BodyUpdateMovePinBall(body_work);
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.dir.z -= (ushort)AppMain.GMD_BOSS2_BODY_PINBALL_ROLL_ROT_Z;
        else
            obj_work.dir.z += (ushort)AppMain.GMD_BOSS2_BODY_PINBALL_ROLL_ROT_Z;
        AppMain.gmBoss2BodySetDirectionRoll(body_work);
        body_work.flag |= 64U;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 180 || AppMain.gmBoss2BodyPinBallCheckAreaStop(body_work) == 0)
            return;
        obj_work.user_timer = 0;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStatePinBallRollUpdateStop);
        body_work.flag &= 4294967263U;
        AppMain.gmBoss2EffCreateRollModelLost(body_work);
    }

    private static void gmBoss2BodyStatePinBallRollUpdateStop(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        int num = 20480;
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
            num = AppMain.FX_Mul(num, 6144);
        AppMain.gmBoss2BodyPinBallAdjustMoveSpeed(body_work, num);
        AppMain.gmBoss2BodyUpdateMovePinBall(body_work);
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.dir.z -= (ushort)AppMain.GMD_BOSS2_BODY_PINBALL_ROLL_ROT_Z;
        else
            obj_work.dir.z += (ushort)AppMain.GMD_BOSS2_BODY_PINBALL_ROLL_ROT_Z;
        AppMain.gmBoss2BodySetDirectionRoll(body_work);
        ++obj_work.user_timer;
        if (obj_work.user_timer < 10 || AppMain.gmBoss2BodyPinBallCheckAreaStop(body_work) == 0)
            return;
        obj_work.dir.z = (ushort)0;
        obj_work.user_timer = 0;
        obj_work.dir.z = (ushort)0;
        AppMain.gmBoss2BodyChangeState(body_work, 9);
    }

    private static void gmBoss2BodyStateDefeatEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        obj_work.flag |= 2U;
        obj_work.disp_flag |= 16U;
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateDefeatUpdateStart);
        body_work.flag &= 4294967291U;
        AppMain.GmSoundStopSE(body_work.se_handle);
        body_work.flag &= 4294967263U;
        AppMain.gmBoss2BodyCatchChangeNeedleModeWait();
        AppMain.GmSoundChangeWinBossBGM();
    }

    private static void gmBoss2BodyStateDefeatLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.disp_flag &= 4294967279U;
        obsObjectWork.flag &= 4294967293U;
    }

    private static void gmBoss2BodyStateDefeatUpdateStart(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 40)
            return;
        obsObjectWork.user_timer = 0;
        AppMain.OBS_OBJECT_WORK parent_obj = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss2EffBombsInit(body_work.bomb_work, parent_obj, parent_obj.pos.x, parent_obj.pos.y, 327680, 327680, 10U, 30U);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateDefeatUpdateFall);
    }

    private static void gmBoss2BodyStateDefeatUpdateFall(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 120)
        {
            AppMain.gmBoss2EffBombsUpdate(body_work.bomb_work);
        }
        else
        {
            obsObjectWork.user_timer = 0;
            obsObjectWork.move_flag |= 128U;
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateDefeatUpdateExplode);
        }
    }

    private static void gmBoss2BodyStateDefeatUpdateExplode(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        int num = AppMain.g_gm_main_system.map_fcol.bottom * 4096 - 614400;
        if (obj_work.pos.y < num)
            return;
        obj_work.move_flag &= 4294967167U;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        body_work.flag |= 134217728U;
        AppMain.GmSoundPlaySE("Boss0_03");
        AppMain.GMM_PAD_VIB_MID_TIME(120f);
        AppMain.GmBsCmnInitFlashScreen(body_work.flash_work, 4f, 5f, 30f);
        AppMain.OBS_OBJECT_WORK parent_obj = AppMain.GMM_BS_OBJ((object)body_work);
        ((AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate(parent_obj, 8)).pos.z = parent_obj.pos.z + 131072;
        AppMain.GmPlayerAddScoreNoDisp((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), 1000);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateDefeatUpdateScatter);
    }

    private static void gmBoss2BodyStateDefeatUpdateScatter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBsCmnUpdateFlashScreen(body_work.flash_work);
        ++obj_work.user_timer;
        if (obj_work.user_timer < 40)
            return;
        obj_work.user_timer = 0;
        AppMain.gmBoss2ChangeTextureBurnt(obj_work);
        body_work.flag |= 16777216U;
        AppMain.gmBoss2EffAfterburnerSmokeInit(body_work);
        AppMain.gmBoss2EffBodySmokeInit(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateDefeatUpdateEnd);
        AppMain.gmBoss2EffScatterInit(body_work);
        AppMain.GmCameraScaleSet(1f, 0.0015f);
        AppMain.GmMapSetDrawMarginNormal();
    }

    private static void gmBoss2BodyStateDefeatUpdateEnd(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 120)
            return;
        obsObjectWork.user_timer = 0;
        AppMain.gmBoss2BodyChangeState(body_work, 12);
    }

    private static void gmBoss2BodyStateEscapeEnter(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        obj_work.spd.x = 0;
        obj_work.spd_add.x = ((int)obj_work.disp_flag & 1) == 0 ? 0 : 0;
        obj_work.spd_add.y = -655;
        obj_work.flag |= 2U;
        obj_work.move_flag |= 4352U;
        AppMain.gmBoss2BodySetDirectionNormal(body_work);
        AppMain.gmBoss2BodySetActionAllParts(body_work, 18, 1);
        body_work.flag |= 8388608U;
        body_work.proc_update = AppMain.GmBsCmnIsFinalZoneType(obj_work) == 0 ? new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateEscapeUpdateScrollLock) : new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateEscapeUpdateFinalZone);
        AppMain.GmMapSetMapDrawSize(1);
    }

    private static void gmBoss2BodyStateEscapeLeave(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)body_work).flag &= 4294967293U;
    }

    private static void gmBoss2BodyStateEscapeUpdateScrollLock(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.gmBoss2BodyEscapeAddjustSpeed(body_work);
        if (AppMain.gmBoss2BodyEscapeCheckScrollUnlock(body_work) == 0)
            return;
        AppMain.GmGmkCamScrLimitRelease((byte)4);
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmBoss2BodySearchShutterOut();
        if (obj_work != null)
            AppMain.GmGmkShutterOutChangeModeOpen(obj_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateEscapeUpdateWaitScreenOut);
        AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 1U);
    }

    private static void gmBoss2BodyStateEscapeUpdateWaitScreenOut(
      AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.gmBoss2BodyEscapeAddjustSpeed(body_work);
        if (AppMain.gmBoss2BodyEscapeCheckScreenOut(body_work) == 0)
            return;
        AppMain.gmBoss2MgrGetMgrWork(AppMain.GMM_BS_OBJ((object)body_work)).flag |= 2U;
        body_work.proc_update = (AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK)null;
    }

    private static void gmBoss2BodyStateEscapeUpdateFinalZone(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.gmBoss2BodyEscapeAddjustSpeed(body_work);
        if (AppMain.gmBoss2BodyEscapeCheckScrollUnlock(body_work) == 0)
            return;
        AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 1U);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BODY_WORK(AppMain.gmBoss2BodyStateEscapeUpdateWaitScreenOut);
    }

    private static void gmBoss2BodyMainFuncWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK body_work = (AppMain.GMS_BOSS2_BODY_WORK)obj_work;
        if (AppMain.gmBoss2MgrCheckSetupComplete(AppMain.gmBoss2MgrGetMgrWork(obj_work)) == 0)
            return;
        AppMain.GmBsCmnInitBossMotionCBSystem(obj_work, body_work.bmcb_mgr);
        AppMain.GmBsCmnCreateSNMWork(body_work.snm_work, obj_work.obj_3d._object, (ushort)15);
        AppMain.GmBsCmnAppendBossMotionCallback(body_work.bmcb_mgr, body_work.snm_work.bmcb_link);
        for (int index = 0; 15 > index; ++index)
            body_work.snm_reg_id[index] = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.g_boss2_node_index_list[index]);
        AppMain.GmBsCmnCreateCNMMgrWork(body_work.cnm_mgr_work, obj_work.obj_3d._object, (ushort)13);
        AppMain.GmBsCmnInitCNMCb(obj_work, body_work.cnm_mgr_work);
        for (int index = 0; 13 > index; ++index)
            body_work.cnm_reg_id[index] = AppMain.GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, AppMain.g_boss2_node_index_list[2 + index]);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2BodyMainFunc);
        AppMain.gmBoss2BodyChangeState(body_work, 1);
    }

    private static void gmBoss2BodyMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK gmsBosS2BodyWork = (AppMain.GMS_BOSS2_BODY_WORK)obj_work;
        AppMain.gmBoss2BodyUpdateNoHitTime(gmsBosS2BodyWork);
        AppMain.gmBoss2BodyUpdateInvincibleTime(gmsBosS2BodyWork);
        if (gmsBosS2BodyWork.proc_update != null)
            gmsBosS2BodyWork.proc_update(gmsBosS2BodyWork);
        if (((int)gmsBosS2BodyWork.flag & 33554432) != 0)
            AppMain.gmBoss2EffAfterburnerInit(gmsBosS2BodyWork);
        if (((int)gmsBosS2BodyWork.flag & int.MinValue) != 0)
        {
            gmsBosS2BodyWork.flag &= 1073741823U;
            AppMain.gmBoss2BodyChangeState(gmsBosS2BodyWork, 11);
        }
        else
        {
            if (((int)gmsBosS2BodyWork.flag & 1073741824) != 0)
            {
                gmsBosS2BodyWork.flag &= 3221225471U;
                gmsBosS2BodyWork.flag |= 536870912U;
                AppMain.GmBsCmnInitObject3DNNDamageFlicker(obj_work, gmsBosS2BodyWork.flk_work, 32f);
            }
            AppMain.GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, gmsBosS2BodyWork.flk_work);
            AppMain.gmBoss2BodyUpdateDirection(gmsBosS2BodyWork);
        }
    }

    private static void gmBoss2EggChangeAction(AppMain.GMS_BOSS2_EGG_WORK egg_work, int action_id)
    {
        AppMain.gmBoss2EggChangeAction(egg_work, action_id, 0);
    }

    private static void gmBoss2EggChangeAction(
      AppMain.GMS_BOSS2_EGG_WORK egg_work,
      int action_id,
      int force_change)
    {
        AppMain.GMS_BOSS2_PART_ACT_INFO bosS2PartActInfo = AppMain.gm_boss2_egg_act_info_tbl[action_id];
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        if (force_change == 0 && egg_work.egg_action_id == action_id && ((int)egg_work.flag & 1) != 0)
            return;
        egg_work.egg_action_id = action_id;
        egg_work.flag |= 1U;
        if (bosS2PartActInfo.is_maintain != (byte)0)
        {
            if (bosS2PartActInfo.is_repeat != (byte)0)
                obj_work.disp_flag |= 4U;
        }
        else
            AppMain.GmBsCmnSetAction(obj_work, (int)bosS2PartActInfo.mtn_id, (int)bosS2PartActInfo.is_repeat, bosS2PartActInfo.is_blend);
        obj_work.obj_3d.speed[0] = bosS2PartActInfo.mtn_spd;
        obj_work.obj_3d.blend_spd = bosS2PartActInfo.blend_spd;
    }

    private static void gmBoss2EggRevertAction(AppMain.GMS_BOSS2_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)parentObj);
        egg_work.flag &= 4294967294U;
        AppMain.GMS_BOSS2_PART_ACT_INFO bosS2PartActInfo = AppMain.gm_boss2_act_info_tbl[parentObj.action_id][1];
        AppMain.GmBsCmnSetAction(obj_work, (int)bosS2PartActInfo.mtn_id, (int)bosS2PartActInfo.is_repeat, 1);
        obj_work.obj_3d.frame[0] = obsObjectWork.obj_3d.frame[0];
    }

    private static void gmBoss2EggStateIdleInit(AppMain.GMS_BOSS2_EGG_WORK egg_work)
    {
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_EGG_WORK(AppMain.gmBoss2EggStateIdleUpdate);
    }

    private static void gmBoss2EggStateIdleUpdate(AppMain.GMS_BOSS2_EGG_WORK egg_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)AppMain.GMM_BS_OBJ((object)egg_work).parent_obj;
        if (((int)parentObj.flag & 268435456) == 0)
            return;
        parentObj.flag &= 4026531839U;
        AppMain.gmBoss2EggStateLaughInit(egg_work);
    }

    private static void gmBoss2EggStateLaughInit(AppMain.GMS_BOSS2_EGG_WORK egg_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)egg_work).parent_obj.disp_flag & 1) != 0)
            AppMain.gmBoss2EggChangeAction(egg_work, 0);
        else
            AppMain.gmBoss2EggChangeAction(egg_work, 1);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_EGG_WORK(AppMain.gmBoss2EggStateLaughUpdate);
    }

    private static void gmBoss2EggStateLaughUpdate(AppMain.GMS_BOSS2_EGG_WORK egg_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)egg_work)) == 0)
            return;
        AppMain.gmBoss2EggRevertAction(egg_work);
        AppMain.gmBoss2EggStateIdleInit(egg_work);
    }

    private static void gmBoss2EggStateDamageInit(AppMain.GMS_BOSS2_EGG_WORK egg_work)
    {
        AppMain.gmBoss2EggChangeAction(egg_work, 2);
        AppMain.gmBoss2EffSweatInit(egg_work);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_EGG_WORK(AppMain.gmBoss2EggStateDamageUpdate);
    }

    private static void gmBoss2EggStateDamageUpdate(AppMain.GMS_BOSS2_EGG_WORK egg_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)egg_work)) == 0)
            return;
        egg_work.flag &= 4294967293U;
        AppMain.gmBoss2EggRevertAction(egg_work);
        AppMain.gmBoss2EggStateIdleInit(egg_work);
    }

    private static void gmBoss2EggStateEscapeInit(AppMain.GMS_BOSS2_EGG_WORK egg_work)
    {
        if (((int)egg_work.flag & 2) == 0)
            AppMain.gmBoss2EffSweatInit(egg_work);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_EGG_WORK(AppMain.gmBoss2EggStateEscapeUpdate);
    }

    private static void gmBoss2EggStateEscapeUpdate(AppMain.GMS_BOSS2_EGG_WORK egg_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)egg_work);
    }

    private static void gmBoss2EggmanMainFuncWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.gmBoss2MgrCheckSetupComplete(AppMain.gmBoss2MgrGetMgrWork(((AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj).ene_3d.ene_com.obj_work)) == 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EggmanMainFunc);
        AppMain.gmBoss2EggStateIdleInit((AppMain.GMS_BOSS2_EGG_WORK)obj_work);
    }

    private static void gmBoss2EggmanMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS2_EGG_WORK gmsBosS2EggWork = (AppMain.GMS_BOSS2_EGG_WORK)obj_work;
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
        if (gmsBosS2EggWork.proc_update != null)
            gmsBosS2EggWork.proc_update(gmsBosS2EggWork);
        if (((int)parentObj.flag & 8388608) != 0)
        {
            parentObj.flag &= 4286578687U;
            AppMain.gmBoss2EggStateEscapeInit(gmsBosS2EggWork);
        }
        if (((int)parentObj.flag & 536870912) != 0)
        {
            parentObj.flag &= 3758096383U;
            AppMain.gmBoss2EggStateDamageInit(gmsBosS2EggWork);
        }
        if (((int)parentObj.flag & 16777216) != 0)
        {
            parentObj.flag &= 4278190079U;
            AppMain.gmBoss2ChangeTextureBurnt(obj_work);
        }
        if (((int)AppMain.GMM_BS_OBJ((object)parentObj).disp_flag & 16) != 0)
            obj_work.disp_flag |= 16U;
        else
            obj_work.disp_flag &= 4294967279U;
        if (((int)parentObj.flag & 64) != 0)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
    }

    private static void gmBoss2BallHitFunc(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        ((AppMain.GMS_BOSS2_BODY_WORK)own_rect.parent_obj.parent_obj).flag |= 268435456U;
    }

    private static void gmBoss2BallMainFuncWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.gmBoss2MgrCheckSetupComplete(AppMain.gmBoss2MgrGetMgrWork(AppMain.GMM_BS_OBJ((object)(AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj))) == 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2BallMainFunc);
        AppMain.gmBoss2BallInit((AppMain.GMS_BOSS2_BALL_WORK)obj_work);
    }

    private static void gmBoss2BallMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BALL_WORK wrk = (AppMain.GMS_BOSS2_BALL_WORK)obj_work;
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    private static void gmBoss2BallInit(AppMain.GMS_BOSS2_BALL_WORK ball_work)
    {
        ball_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BALL_WORK(AppMain.gmBoss2BallUpdateCatch);
    }

    private static void gmBoss2BallUpdateCatch(AppMain.GMS_BOSS2_BALL_WORK ball_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)ball_work);
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[1], 1);
        ball_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BALL_WORK(AppMain.gmBoss2BallUpdateWaitShoot);
    }

    private static void gmBoss2BallUpdateWaitShoot(AppMain.GMS_BOSS2_BALL_WORK ball_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)ball_work);
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[1], 1);
        if (((int)parentObj.flag & 4194304) == 0)
            return;
        parentObj.flag &= 4290772991U;
        ball_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BALL_WORK(AppMain.gmBoss2BallUpdateShoot);
        obj_work.move_flag |= 128U;
    }

    private static void gmBoss2BallUpdateShoot(AppMain.GMS_BOSS2_BALL_WORK ball_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)ball_work).move_flag & 1) == 0)
            return;
        ball_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BALL_WORK(AppMain.gmBoss2BallUpdateWaitBomb);
    }

    private static void gmBoss2BallUpdateWaitBomb(AppMain.GMS_BOSS2_BALL_WORK ball_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)ball_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 120)
            return;
        obsObjectWork.user_timer = 0;
        ball_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS2_BALL_WORK(AppMain.gmBoss2BallUpdateFlicker);
        AppMain.GmBsCmnInitObject3DNNDamageFlicker(obsObjectWork, ball_work.flk_work, 16f);
        AppMain.gmBoss2EffBallBombInit(obsObjectWork.pos, obsObjectWork);
    }

    private static void gmBoss2BallUpdateFlicker(AppMain.GMS_BOSS2_BALL_WORK ball_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)ball_work);
        AppMain.OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        if (AppMain.GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, ball_work.flk_work) == 0)
            return;
        AppMain.gmBoss2EffBallBombPartInit(obj_work.pos, parentObj, 4096);
        AppMain.gmBoss2EffBallBombPartInit(obj_work.pos, parentObj, -4096);
        ((AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 10)).pos.Assign(obj_work.pos);
        obj_work.flag |= 4U;
        AppMain.GmSoundPlaySE(AppMain.GMD_ENE_KAMA_SE_BOMB);
    }

    
}