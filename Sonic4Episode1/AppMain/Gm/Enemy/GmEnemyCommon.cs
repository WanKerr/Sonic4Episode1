using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

public partial class AppMain
{
    private static AppMain.OBS_OBJECT_WORK GMM_ENEMY_CREATE_RIDE_WORK(
        AppMain.GMS_EVE_RECORD_EVENT eve_rec,
        int pos_x,
        int pos_y,
        AppMain.TaskWorkFactoryDelegate work_size,
        string name)
    {
        return AppMain.GmEnemyCreateWork(eve_rec, pos_x, pos_y, work_size, (ushort)4342, name);
    }

    private static AppMain.OBS_OBJECT_WORK GMM_ENEMY_CREATE_WORK(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      AppMain.TaskWorkFactoryDelegate work_size,
      string name)
    {
        return AppMain.GmEnemyCreateWork(eve_rec, pos_x, pos_y, work_size, (ushort)5376, name);
    }

    private static AppMain.OBS_OBJECT_WORK GmEnemyCreateWork(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      AppMain.TaskWorkFactoryDelegate work_size,
      ushort prio,
      string name)
    {
        ushort[] numArray1 = new ushort[3]
        {
      (ushort) 0,
      (ushort) 2,
      (ushort) 1
        };
        ushort[] numArray2 = new ushort[3]
        {
      (ushort) 65533,
      ushort.MaxValue,
      (ushort) 65534
        };
        AppMain.OBS_OBJECT_WORK pWork = AppMain.OBM_OBJECT_TASK_DETAIL_INIT(prio, (byte)2, (byte)0, (byte)0, work_size, name);
        if (pWork == null)
            return (AppMain.OBS_OBJECT_WORK)null;
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)pWork;
        AppMain.mtTaskChangeTcbDestructor(pWork.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.GmEnemyDefaultExit));
        if (eve_rec != null)
        {
            gmsEnemyComWork.eve_rec = eve_rec;
            gmsEnemyComWork.eve_x = eve_rec.pos_x;
            eve_rec.pos_x = byte.MaxValue;
            pWork.obj_type = eve_rec.id < (ushort)60 || (ushort)300 <= eve_rec.id && eve_rec.id < (ushort)300 || (ushort)308 <= eve_rec.id && eve_rec.id < (ushort)335 ? (ushort)2 : (ushort)3;
            pWork.view_out_ofst = (short)((int)AppMain.g_gm_event_size_tbl[(int)eve_rec.id] + 16 + 32 + 16 + 128);
            if (((int)eve_rec.flag & 2048) != 0)
                pWork.flag |= 16U;
            else
                pWork.ppViewCheck = new AppMain.OBS_OBJECT_WORK_Delegate3(AppMain.ObjObjectViewOutCheck);
        }
        else
            pWork.obj_type = (ushort)2;
        pWork.ppOut = AppMain._ObjDrawActionSummary;
        pWork.ppOutSub = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        pWork.ppIn = AppMain._GmEnemyDefaultInFunc;
        pWork.ppMove = AppMain._GmEnemyDefaultMoveFunc;
        pWork.ppActCall = AppMain._gmEnemyActionCallBack;
        pWork.ppRec = AppMain._gmEnemyDefaultRecFunc;
        pWork.ppLast = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        pWork.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        gmsEnemyComWork.born_pos_x = pos_x;
        gmsEnemyComWork.born_pos_y = pos_y;
        pWork.pos.x = pos_x;
        pWork.pos.y = pos_y;
        pWork.spd_fall = 672;
        pWork.spd_fall_max = 61440;
        pWork.flag |= 1U;
        pWork.move_flag |= 524288U;
        pWork.scale.x = pWork.scale.y = pWork.scale.z = 4096;
        AppMain.ObjObjectGetRectBuf(pWork, (AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>)gmsEnemyComWork.rect_work, (ushort)3);
        for (int index = 0; index < 3; ++index)
        {
            AppMain.ObjRectGroupSet(gmsEnemyComWork.rect_work[index], (byte)1, (byte)1);
            AppMain.ObjRectAtkSet(gmsEnemyComWork.rect_work[index], numArray1[index], (short)1);
            AppMain.ObjRectDefSet(gmsEnemyComWork.rect_work[index], numArray2[index], (short)0);
            gmsEnemyComWork.rect_work[index].parent_obj = pWork;
            gmsEnemyComWork.rect_work[index].flag &= 4294967291U;
        }
        gmsEnemyComWork.rect_work[0].ppDef = AppMain._GmEnemyDefaultDefFunc;
        gmsEnemyComWork.rect_work[1].ppHit = AppMain._GmEnemyDefaultAtkFunc;
        gmsEnemyComWork.rect_work[0].flag |= 128U;
        gmsEnemyComWork.rect_work[2].flag |= 1048800U;
        pWork.col_work = gmsEnemyComWork.col_work;
        return pWork;
    }

    private static void GmEnemyDefaultExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_ENEMY_COM_WORK tcbWork = (AppMain.GMS_ENEMY_COM_WORK)AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.eve_rec != null && tcbWork.eve_rec.pos_x == byte.MaxValue && tcbWork.eve_rec.pos_y == byte.MaxValue)
            AppMain.GmEventMgrLocalEventRelease(tcbWork.eve_rec);
        else if (((int)tcbWork.enemy_flag & 65536) == 0 && tcbWork.eve_rec != null)
            tcbWork.eve_rec.pos_x = tcbWork.eve_x;
        AppMain.ObjObjectExit(tcb);
    }

    private static void GmEnemyActionSet(AppMain.GMS_ENEMY_COM_WORK ene_com, ushort id)
    {
        ene_com.rect_work[0].flag &= 4294967291U;
        ene_com.rect_work[1].flag &= 4294967291U;
        ene_com.rect_work[2].flag &= 4294967291U;
        if (ene_com.obj_work.obj_3d == null)
            return;
        AppMain.ObjDrawObjectActionSet3DNN(ene_com.obj_work, (int)id, 0);
    }

    private static void GmEnemyDefaultDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)null;
        if (match_rect.parent_obj != null && match_rect.parent_obj.obj_type == (ushort)1)
            ply_work = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj.vit == (byte)0)
        {
            if (((int)parentObj.obj_work.move_flag & 4096) == 0 || parentObj.obj_work.obj_type == (ushort)3)
                parentObj.enemy_flag |= 65536U;
            parentObj.obj_work.flag |= 2U;
            parentObj.rect_work[0].flag |= 2048U;
            parentObj.rect_work[1].flag |= 2048U;
            parentObj.rect_work[2].flag |= 2048U;
            if (parentObj.obj_work.obj_type == (ushort)2)
            {
                AppMain.GmSoundPlaySE("Enemy");
                AppMain.GmComEfctCreateHitPlayer(parentObj.obj_work, ((int)mine_rect.rect.left + (int)mine_rect.rect.right) * 4096 / 2, ((int)mine_rect.rect.top + (int)mine_rect.rect.bottom) * 4096 / 2);
                AppMain.GmComEfctCreateEneDeadSmoke(parentObj.obj_work, ((int)mine_rect.rect.left + (int)mine_rect.rect.right) * 4096 / 2, ((int)mine_rect.rect.top + (int)mine_rect.rect.bottom) * 4096 / 2);
                AppMain.GmGmkAnimalInit(parentObj.obj_work, 0, 0, 0, (byte)0, (byte)0, (ushort)0);
                AppMain.GMM_PAD_VIB_SMALL();
                if (ply_work != null)
                    AppMain.GmPlayerComboScore(ply_work, parentObj.obj_work.pos.x, parentObj.obj_work.pos.y - 65536);
                AppMain.HgTrophyIncEnemyKillCount(parentObj.obj_work);
            }
            parentObj.obj_work.flag |= 8U;
        }
        else
        {
            --parentObj.vit;
            ++parentObj.eve_rec.byte_param[1];
            parentObj.invincible_timer = 245760;
            parentObj.rect_work[1].hit_power = (short)0;
        }
        if (ply_work == null || ply_work.obj_work.obj_type != (ushort)1)
            return;
        AppMain.GmPlySeqAtkReactionInit(ply_work);
    }

    private static void GmEnemyDefaultAtkFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
    }

    private static void GmEnemyDefaultMoveFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjObjectMove(obj_work);
    }

    private static void GmEnemyDefaultInFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        if (gmsEnemyComWork.target_obj == null || ((int)gmsEnemyComWork.target_obj.flag & 4) == 0)
            return;
        gmsEnemyComWork.target_obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmEnemyDefaultRecFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmEnemyActionCallBack(object cmd_work, object act_work, uint data)
    {
    }
    private static void GmEneComActionSetDependHFlip(
      AppMain.OBS_OBJECT_WORK obj_work,
      int act_id_r,
      int act_id_l)
    {
        if (((int)obj_work.disp_flag & 1) != 0)
            AppMain.ObjDrawObjectActionSet(obj_work, act_id_l);
        else
            AppMain.ObjDrawObjectActionSet(obj_work, act_id_r);
    }

    private static void GmEneComActionSet3DNNBlendDependHFlip(
      AppMain.OBS_OBJECT_WORK obj_work,
      int act_id_r,
      int act_id_l)
    {
        if (((int)obj_work.disp_flag & 1) != 0)
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, act_id_l);
        else
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, act_id_r);
    }

    private static int GmEneComTargetIsLeft(
      AppMain.OBS_OBJECT_WORK mine_obj,
      AppMain.OBS_OBJECT_WORK target_obj)
    {
        return target_obj.pos.x < mine_obj.pos.x ? 1 : 0;
    }

    private static int GmEneComCheckMoveLimit(
      AppMain.OBS_OBJECT_WORK obj_work,
      int limit_left,
      int limit_right)
    {
        return ((int)obj_work.disp_flag & 1) != 0 && obj_work.pos.x <= limit_left || ((int)obj_work.disp_flag & 1) == 0 && obj_work.pos.x >= limit_right ? 0 : 1;
    }

    private static AppMain.OBS_OBJECT_WORK GmEneComCreateAtkObject(
      AppMain.OBS_OBJECT_WORK parent_obj,
      short view_out_ofst)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_COM_WORK()), parent_obj, (ushort)0, parent_obj.tcb.am_tcb.name);
        AppMain.GMS_EFFECT_COM_WORK efct_com = (AppMain.GMS_EFFECT_COM_WORK)work;
        work.flag &= 4294967277U;
        work.move_flag |= 256U;
        work.view_out_ofst = view_out_ofst;
        AppMain.GmEffectRectInit(efct_com, AppMain.gm_ene_com_atk_obj_atk_flag_tbl, AppMain.gm_ene_com_atk_obj_def_flag_tbl, (byte)1, (byte)1);
        return work;
    }

}