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
    private static void gmBoss4MgrWaitRelease(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmBoss4IsAllCreatedObjDeleted())
            return;
        ((AppMain.GMS_ENEMY_COM_WORK)obj_work).enemy_flag |= 65536U;
        obj_work.flag |= 4U;
        AppMain.GmGameDatReleaseBossBattleStart(3);
    }

    private static void gmBoss4MgrMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        if (obsCamera == null)
            return;
        AppMain.GMS_BOSS4_MGR_WORK gmsBosS4MgrWork = (AppMain.GMS_BOSS4_MGR_WORK)obj_work;
        if (((int)gmsBosS4MgrWork.flag & 2) != 0)
        {
            if (gmsBosS4MgrWork.body_work != null)
            {
                AppMain.GMM_BS_OBJ((object)gmsBosS4MgrWork.body_work).flag |= 8U;
                gmsBosS4MgrWork.body_work = (AppMain.GMS_BOSS4_BODY_WORK)null;
            }
            if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4MgrWaitRelease);
        }
        AppMain.GmBoss4CapsuleUpdateRol(AppMain.GMD_BOSS4_CAP_ROTATE_SPD);
        obsCamera.flag &= 4294967294U;
        float move_size = obsCamera.disp_pos.x - obsCamera.prev_disp_pos.x;
        if ((double)move_size < -((double)AppMain.GMD_BOSS4_SCROLL_SPD_MAX + 8.0))
            move_size = AppMain.gmBoss4MgrMainStatics.xold;
        if ((double)move_size > (double)AppMain.GMD_BOSS4_SCROLL_SPD_MAX + 8.0)
            move_size = AppMain.gmBoss4MgrMainStatics.xold;
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)16)
            AppMain.GmMapSetAddMapUserScrlXAddSize(move_size);
        AppMain.gmBoss4MgrMainStatics.xold = move_size;
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = (AppMain.GMS_PLAYER_WORK)playerObj;
        if (AppMain.gm_boss4_n_scroll == 0 && (double)AppMain.gm_boss4_f_scroll_spd <= 0.0)
            return;
        int left = AppMain.g_gm_main_system.map_fcol.left;
        int right = AppMain.g_gm_main_system.map_fcol.right;
        AppMain.NNS_VECTOR offset = new AppMain.NNS_VECTOR(AppMain.FX_FX32_TO_F32(AppMain.GmBoss4GetScrollOffset()), 0.0f, 0.0f);
        if ((double)offset.x < 0.0)
            AppMain.amTrailEFOffsetPos((ushort)1, offset);
        playerObj.pos.x += AppMain.GmBoss4GetScrollOffset();
        int num1 = (int)AppMain.gm_boss4_f_scroll_spd * 4096;
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
            num1 = 0;
        if (gmsPlayerWork.seq_state == 17 && gmsPlayerWork.obj_work.spd.x < AppMain.FX_F32_TO_FX32(2f))
            num1 /= 4;
        if (gmsPlayerWork.seq_state == 20 && gmsPlayerWork.obj_work.spd.x < AppMain.FX_F32_TO_FX32(3f))
            num1 /= 4;
        playerObj.pos.x -= num1;
        int num2 = playerObj.pos.x / 4096;
        if (left + 48 > num2)
            playerObj.pos.x = (left + 48) * 4096;
        if (right < num2)
            playerObj.pos.x = (right - 2) * 4096;
        AppMain.GMS_BOSS4_BODY_WORK bodyWork = gmsBosS4MgrWork.body_work;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)bodyWork;
        if (obsObjectWork != null)
        {
            obsObjectWork.pos.x += AppMain.GmBoss4GetScrollOffset();
            obsObjectWork.pos.x -= AppMain.FX_F32_TO_FX32(AppMain.gm_boss4_f_scroll_spd);
            if (AppMain.gm_boss4_n_scroll == 1)
                obsObjectWork.pos.x += AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_SCROLL_SPD_BOSS);
            else
                obsObjectWork.pos.x += AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_SCROLL_SPD_BOSS_BROKEN);
            int f32_1 = (int)AppMain.FX_FX32_TO_F32(obsObjectWork.pos.x);
            if (left > f32_1)
                obsObjectWork.pos.x = AppMain.FX_F32_TO_FX32((float)left);
            if (AppMain.gm_boss4_n_scroll == 1)
            {
                int f32_2 = (int)AppMain.FX_FX32_TO_F32(obsObjectWork.pos.x);
                if ((double)right - 50.0 > (double)f32_2)
                    obsObjectWork.pos.x = AppMain.FX_F32_TO_FX32((float)right - 50f);
            }
        }
        if (obsObjectWork == null)
            return;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)bodyWork;
        if (Math.Abs(obsObjectWork.pos.x - playerObj.pos.x) > AppMain.FX_F32_TO_FX32(140f))
            gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        else
            gmsEnemy3DWork.ene_com.enemy_flag &= 4294934527U;
    }

    private static void gmBoss4MgrWaitLoad(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_MGR_WORK gmsBosS4MgrWork = (AppMain.GMS_BOSS4_MGR_WORK)obj_work;
        bool flag = false;
        int x = obj_work.pos.x;
        int y = obj_work.pos.y;
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
        {
            if (AppMain.GmMainDatLoadBossBattleLoadCheck(3))
                flag = true;
        }
        else
            flag = true;
        if (flag)
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork1 = AppMain.GmEventMgrLocalEventBirth((ushort)321, x, y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
            AppMain.GmBoss4IncObjCreateCount();
            AppMain.OBS_OBJECT_WORK obsObjectWork2 = AppMain.GmEventMgrLocalEventBirth((ushort)322, x, y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
            AppMain.GmBoss4IncObjCreateCount();
            AppMain.GmBoss4CapsuleClear();
            for (int index = 0; index < 6; ++index)
            {
                AppMain.GmEventMgrLocalEventBirth((ushort)323, x, y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0).parent_obj = obsObjectWork1;
                AppMain.GmBoss4IncObjCreateCount();
            }
            AppMain.GMS_BOSS4_BODY_WORK gmsBosS4BodyWork = (AppMain.GMS_BOSS4_BODY_WORK)obsObjectWork1;
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
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4MgrMain);
    }


}