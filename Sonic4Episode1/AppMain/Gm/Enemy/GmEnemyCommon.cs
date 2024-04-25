public partial class AppMain
{
    private static OBS_OBJECT_WORK GMM_ENEMY_CREATE_RIDE_WORK(
        GMS_EVE_RECORD_EVENT eve_rec,
        int pos_x,
        int pos_y,
        TaskWorkFactoryDelegate work_size,
        string name)
    {
        return GmEnemyCreateWork(eve_rec, pos_x, pos_y, work_size, 4342, name);
    }

    private static OBS_OBJECT_WORK GMM_ENEMY_CREATE_WORK(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      TaskWorkFactoryDelegate work_size,
      string name)
    {
        return GmEnemyCreateWork(eve_rec, pos_x, pos_y, work_size, 5376, name);
    }

    private static OBS_OBJECT_WORK GmEnemyCreateWork(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      TaskWorkFactoryDelegate work_size,
      ushort prio,
      string name)
    {
        ushort[] numArray1 = new ushort[3]
        {
       0,
       2,
       1
        };
        ushort[] numArray2 = new ushort[3]
        {
       65533,
      ushort.MaxValue,
       65534
        };
        OBS_OBJECT_WORK pWork = OBM_OBJECT_TASK_DETAIL_INIT(prio, 2, 0, 0, work_size, name);
        if (pWork == null)
            return null;
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)pWork;
        mtTaskChangeTcbDestructor(pWork.tcb, new GSF_TASK_PROCEDURE(GmEnemyDefaultExit));
        if (eve_rec != null)
        {
            gmsEnemyComWork.eve_rec = eve_rec;
            gmsEnemyComWork.eve_x = eve_rec.pos_x;
            eve_rec.pos_x = byte.MaxValue;
            pWork.obj_type = eve_rec.id < 60 || 300 <= eve_rec.id && eve_rec.id < 300 || 308 <= eve_rec.id && eve_rec.id < 335 ? (ushort)2 : (ushort)3;
            pWork.view_out_ofst = (short)(g_gm_event_size_tbl[eve_rec.id] + 16 + 32 + 16 + 128);
            if ((eve_rec.flag & 2048) != 0)
                pWork.flag |= 16U;
            else
                pWork.ppViewCheck = new OBS_OBJECT_WORK_Delegate3(ObjObjectViewOutCheck);
        }
        else
            pWork.obj_type = 2;
        pWork.ppOut = _ObjDrawActionSummary;
        pWork.ppOutSub = null;
        pWork.ppIn = _GmEnemyDefaultInFunc;
        pWork.ppMove = _GmEnemyDefaultMoveFunc;
        pWork.ppActCall = _gmEnemyActionCallBack;
        pWork.ppRec = _gmEnemyDefaultRecFunc;
        pWork.ppLast = null;
        pWork.ppFunc = null;
        gmsEnemyComWork.born_pos_x = pos_x;
        gmsEnemyComWork.born_pos_y = pos_y;
        pWork.pos.x = pos_x;
        pWork.pos.y = pos_y;
        pWork.spd_fall = 672;
        pWork.spd_fall_max = 61440;
        pWork.flag |= 1U;
        pWork.move_flag |= 524288U;
        pWork.scale.x = pWork.scale.y = pWork.scale.z = 4096;
        ObjObjectGetRectBuf(pWork, gmsEnemyComWork.rect_work, 3);
        for (int index = 0; index < 3; ++index)
        {
            ObjRectGroupSet(gmsEnemyComWork.rect_work[index], 1, 1);
            ObjRectAtkSet(gmsEnemyComWork.rect_work[index], numArray1[index], 1);
            ObjRectDefSet(gmsEnemyComWork.rect_work[index], numArray2[index], 0);
            gmsEnemyComWork.rect_work[index].parent_obj = pWork;
            gmsEnemyComWork.rect_work[index].flag &= 4294967291U;
        }
        gmsEnemyComWork.rect_work[0].ppDef = _GmEnemyDefaultDefFunc;
        gmsEnemyComWork.rect_work[1].ppHit = _GmEnemyDefaultAtkFunc;
        gmsEnemyComWork.rect_work[0].flag |= 128U;
        gmsEnemyComWork.rect_work[2].flag |= 1048800U;
        pWork.col_work = gmsEnemyComWork.col_work;
        return pWork;
    }

    private static void GmEnemyDefaultExit(MTS_TASK_TCB tcb)
    {
        GMS_ENEMY_COM_WORK tcbWork = (GMS_ENEMY_COM_WORK)mtTaskGetTcbWork(tcb);
        if (tcbWork.eve_rec != null && tcbWork.eve_rec.pos_x == byte.MaxValue && tcbWork.eve_rec.pos_y == byte.MaxValue)
            GmEventMgrLocalEventRelease(tcbWork.eve_rec);
        else if (((int)tcbWork.enemy_flag & 65536) == 0 && tcbWork.eve_rec != null)
            tcbWork.eve_rec.pos_x = tcbWork.eve_x;
        ObjObjectExit(tcb);
    }

    private static void GmEnemyActionSet(GMS_ENEMY_COM_WORK ene_com, ushort id)
    {
        ene_com.rect_work[0].flag &= 4294967291U;
        ene_com.rect_work[1].flag &= 4294967291U;
        ene_com.rect_work[2].flag &= 4294967291U;
        if (ene_com.obj_work.obj_3d == null)
            return;
        ObjDrawObjectActionSet3DNN(ene_com.obj_work, id, 0);
    }

    private static void GmEnemyDefaultDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK ply_work = null;
        if (match_rect.parent_obj != null && match_rect.parent_obj.obj_type == 1)
            ply_work = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj.vit == 0)
        {
            if (((int)parentObj.obj_work.move_flag & 4096) == 0 || parentObj.obj_work.obj_type == 3)
                parentObj.enemy_flag |= 65536U;
            parentObj.obj_work.flag |= 2U;
            parentObj.rect_work[0].flag |= 2048U;
            parentObj.rect_work[1].flag |= 2048U;
            parentObj.rect_work[2].flag |= 2048U;
            if (parentObj.obj_work.obj_type == 2)
            {
                GmSoundPlaySE("Enemy");
                GmComEfctCreateHitPlayer(parentObj.obj_work, (mine_rect.rect.left + mine_rect.rect.right) * 4096 / 2, (mine_rect.rect.top + mine_rect.rect.bottom) * 4096 / 2);
                GmComEfctCreateEneDeadSmoke(parentObj.obj_work, (mine_rect.rect.left + mine_rect.rect.right) * 4096 / 2, (mine_rect.rect.top + mine_rect.rect.bottom) * 4096 / 2);
                GmGmkAnimalInit(parentObj.obj_work, 0, 0, 0, 0, 0, 0);
                GMM_PAD_VIB_SMALL();
                if (ply_work != null)
                    GmPlayerComboScore(ply_work, parentObj.obj_work.pos.x, parentObj.obj_work.pos.y - 65536);
                HgTrophyIncEnemyKillCount(parentObj.obj_work);
            }
            parentObj.obj_work.flag |= 8U;
        }
        else
        {
            --parentObj.vit;
            ++parentObj.eve_rec.byte_param[1];
            parentObj.invincible_timer = 245760;
            parentObj.rect_work[1].hit_power = 0;
        }
        if (ply_work == null || ply_work.obj_work.obj_type != 1)
            return;
        GmPlySeqAtkReactionInit(ply_work);
    }

    private static void GmEnemyDefaultAtkFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
    }

    private static void GmEnemyDefaultMoveFunc(OBS_OBJECT_WORK obj_work)
    {
        ObjObjectMove(obj_work);
    }

    private static void GmEnemyDefaultInFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
        if (gmsEnemyComWork.target_obj == null || ((int)gmsEnemyComWork.target_obj.flag & 4) == 0)
            return;
        gmsEnemyComWork.target_obj = null;
    }

    private static void gmEnemyDefaultRecFunc(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmEnemyActionCallBack(object cmd_work, object act_work, uint data)
    {
    }
    private static void GmEneComActionSetDependHFlip(
      OBS_OBJECT_WORK obj_work,
      int act_id_r,
      int act_id_l)
    {
        if (((int)obj_work.disp_flag & 1) != 0)
            ObjDrawObjectActionSet(obj_work, act_id_l);
        else
            ObjDrawObjectActionSet(obj_work, act_id_r);
    }

    private static void GmEneComActionSet3DNNBlendDependHFlip(
      OBS_OBJECT_WORK obj_work,
      int act_id_r,
      int act_id_l)
    {
        if (((int)obj_work.disp_flag & 1) != 0)
            ObjDrawObjectActionSet3DNNBlend(obj_work, act_id_l);
        else
            ObjDrawObjectActionSet3DNNBlend(obj_work, act_id_r);
    }

    private static int GmEneComTargetIsLeft(
      OBS_OBJECT_WORK mine_obj,
      OBS_OBJECT_WORK target_obj)
    {
        return target_obj.pos.x < mine_obj.pos.x ? 1 : 0;
    }

    private static int GmEneComCheckMoveLimit(
      OBS_OBJECT_WORK obj_work,
      int limit_left,
      int limit_right)
    {
        return ((int)obj_work.disp_flag & 1) != 0 && obj_work.pos.x <= limit_left || ((int)obj_work.disp_flag & 1) == 0 && obj_work.pos.x >= limit_right ? 0 : 1;
    }

    private static OBS_OBJECT_WORK GmEneComCreateAtkObject(
      OBS_OBJECT_WORK parent_obj,
      short view_out_ofst)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_COM_WORK(), parent_obj, 0, parent_obj.tcb.am_tcb.name);
        GMS_EFFECT_COM_WORK efct_com = (GMS_EFFECT_COM_WORK)work;
        work.flag &= 4294967277U;
        work.move_flag |= 256U;
        work.view_out_ofst = view_out_ofst;
        GmEffectRectInit(efct_com, gm_ene_com_atk_obj_atk_flag_tbl, gm_ene_com_atk_obj_def_flag_tbl, 1, 1);
        return work;
    }

}