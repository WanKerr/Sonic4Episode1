using System;

public partial class AppMain
{
    private static void gmBoss4MgrWaitRelease(OBS_OBJECT_WORK obj_work)
    {
        if (!GmBoss4IsAllCreatedObjDeleted())
            return;
        ((GMS_ENEMY_COM_WORK)obj_work).enemy_flag |= 65536U;
        obj_work.flag |= 4U;
        GmGameDatReleaseBossBattleStart(3);
    }

    private static void gmBoss4MgrMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        if (obsCamera == null)
            return;
        GMS_BOSS4_MGR_WORK gmsBosS4MgrWork = (GMS_BOSS4_MGR_WORK)obj_work;
        if (((int)gmsBosS4MgrWork.flag & 2) != 0)
        {
            if (gmsBosS4MgrWork.body_work != null)
            {
                GMM_BS_OBJ(gmsBosS4MgrWork.body_work).flag |= 8U;
                gmsBosS4MgrWork.body_work = null;
            }
            if (GmBsCmnIsFinalZoneType(obj_work) != 0)
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4MgrWaitRelease);
        }
        GmBoss4CapsuleUpdateRol(GMD_BOSS4_CAP_ROTATE_SPD);
        obsCamera.flag &= 4294967294U;
        float move_size = obsCamera.disp_pos.x - obsCamera.prev_disp_pos.x;
        if (move_size < -(GMD_BOSS4_SCROLL_SPD_MAX + 8.0))
            move_size = gmBoss4MgrMainStatics.xold;
        if (move_size > GMD_BOSS4_SCROLL_SPD_MAX + 8.0)
            move_size = gmBoss4MgrMainStatics.xold;
        if (g_gs_main_sys_info.stage_id != 16)
            GmMapSetAddMapUserScrlXAddSize(move_size);
        gmBoss4MgrMainStatics.xold = move_size;
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        GMS_PLAYER_WORK gmsPlayerWork = (GMS_PLAYER_WORK)playerObj;
        if (gm_boss4_n_scroll == 0 && gm_boss4_f_scroll_spd <= 0.0)
            return;
        int left = g_gm_main_system.map_fcol.left;
        int right = g_gm_main_system.map_fcol.right;
        NNS_VECTOR offset = new NNS_VECTOR(FX_FX32_TO_F32(GmBoss4GetScrollOffset()), 0.0f, 0.0f);
        if (offset.x < 0.0)
            amTrailEFOffsetPos(1, offset);
        playerObj.pos.x += GmBoss4GetScrollOffset();
        int num1 = (int)gm_boss4_f_scroll_spd * 4096;
        if (((int)gmsPlayerWork.player_flag & GMD_PLF_DIE) != 0)
            num1 = 0;
        if (gmsPlayerWork.seq_state == GME_PLY_SEQ_STATE_JUMP && gmsPlayerWork.obj_work.spd.x < FX_F32_TO_FX32(2f))
            num1 /= 4;
        if (gmsPlayerWork.seq_state == GME_PLY_SEQ_STATE_HOMING_REF && gmsPlayerWork.obj_work.spd.x < FX_F32_TO_FX32(3f))
            num1 /= 4;
        playerObj.pos.x -= num1;
        int num2 = playerObj.pos.x / 4096;
        if (left + 48 > num2)
            playerObj.pos.x = (left + 48) * 4096;
        if (right < num2)
            playerObj.pos.x = (right - 2) * 4096;
        GMS_BOSS4_BODY_WORK bodyWork = gmsBosS4MgrWork.body_work;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)bodyWork;
        if (obsObjectWork != null)
        {
            obsObjectWork.pos.x += GmBoss4GetScrollOffset();
            obsObjectWork.pos.x -= FX_F32_TO_FX32(gm_boss4_f_scroll_spd);
            if (gm_boss4_n_scroll == 1)
                obsObjectWork.pos.x += FX_F32_TO_FX32(GMD_BOSS4_SCROLL_SPD_BOSS);
            else
                obsObjectWork.pos.x += FX_F32_TO_FX32(GMD_BOSS4_SCROLL_SPD_BOSS_BROKEN);
            int f32_1 = (int)FX_FX32_TO_F32(obsObjectWork.pos.x);
            if (left > f32_1)
                obsObjectWork.pos.x = FX_F32_TO_FX32(left);
            if (gm_boss4_n_scroll == 1)
            {
                int f32_2 = (int)FX_FX32_TO_F32(obsObjectWork.pos.x);
                if (right - 50.0 > f32_2)
                    obsObjectWork.pos.x = FX_F32_TO_FX32(right - 50f);
            }
        }
        if (obsObjectWork == null)
            return;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)bodyWork;
        if (Math.Abs(obsObjectWork.pos.x - playerObj.pos.x) > FX_F32_TO_FX32(140f))
            gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        else
            gmsEnemy3DWork.ene_com.enemy_flag &= 4294934527U;
    }

    private static void gmBoss4MgrWaitLoad(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_MGR_WORK gmsBosS4MgrWork = (GMS_BOSS4_MGR_WORK)obj_work;
        bool flag = false;
        int x = obj_work.pos.x;
        int y = obj_work.pos.y;
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
        {
            if (GmMainDatLoadBossBattleLoadCheck(3))
                flag = true;
        }
        else
            flag = true;
        if (flag)
        {
            OBS_OBJECT_WORK obsObjectWork1 = GmEventMgrLocalEventBirth(321, x, y, 0, 0, 0, 0, 0, 0);
            GmBoss4IncObjCreateCount();
            OBS_OBJECT_WORK obsObjectWork2 = GmEventMgrLocalEventBirth(322, x, y, 0, 0, 0, 0, 0, 0);
            GmBoss4IncObjCreateCount();
            GmBoss4CapsuleClear();
            for (int index = 0; index < 6; ++index)
            {
                GmEventMgrLocalEventBirth(323, x, y, 0, 0, 0, 0, 0, 0).parent_obj = obsObjectWork1;
                GmBoss4IncObjCreateCount();
            }
            GMS_BOSS4_BODY_WORK gmsBosS4BodyWork = (GMS_BOSS4_BODY_WORK)obsObjectWork1;
            gmsBosS4MgrWork.body_work = gmsBosS4BodyWork;
            gmsBosS4BodyWork.mgr_work = gmsBosS4MgrWork;
            obsObjectWork1.parent_obj = obj_work;
            obsObjectWork2.parent_obj = obsObjectWork1;
            gmsBosS4BodyWork.parts_objs[0] = obsObjectWork1;
            gmsBosS4BodyWork.parts_objs[1] = obsObjectWork2;
        }
        if (!flag)
            return;
        gmsBosS4MgrWork.flag |= 1U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4MgrMain);
    }


}