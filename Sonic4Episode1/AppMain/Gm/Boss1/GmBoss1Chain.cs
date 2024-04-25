public partial class AppMain
{
    private static OBS_OBJECT_WORK GmBoss1ChainInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS1_CHAIN_WORK(), "BOSS1_CHAIN");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS1_CHAIN_WORK gmsBosS1ChainWork = (GMS_BOSS1_CHAIN_WORK)work;
        work.move_flag |= 256U;
        ObjObjectCopyAction3dNNModel(work, gm_boss1_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(704), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = 0.125f;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1ChainWaitSetup);
        work.flag |= 16U;
        work.disp_flag |= 4U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[1], -16, -16, 16, 16);
        gmsEnemy3DWork.ene_com.rect_work[1].ppHit = new OBS_RECT_WORK_Delegate1(gmBoss1ChainAtkHitFunc);
        gmsEnemy3DWork.ene_com.rect_work[0].flag |= 2048U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag |= 2048U;
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1ChainOutFunc);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss1ChainExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss1ChainExit(MTS_TASK_TCB tcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        GMS_BOSS1_CHAIN_WORK gmsBosS1ChainWork = (GMS_BOSS1_CHAIN_WORK)tcbWork;
        gmBoss1MgrDecObjCreateCount(gmsBosS1ChainWork.mgr_work);
        GmBsCmnClearBossMotionCBSystem(tcbWork);
        GmBsCmnDeleteSNMWork(gmsBosS1ChainWork.snm_work);
        GmBsCmnClearCNMCb(tcbWork);
        GmBsCmnDeleteCNMMgrWork(gmsBosS1ChainWork.cnm_mgr_work);
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss1ChainUpdateAtkRectPosition(GMS_BOSS1_CHAIN_WORK chain_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(chain_work);
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(chain_work.snm_work, chain_work.ball_snm_reg_id);
        VEC_Set(ref chain_work.ene_3d.ene_com.rect_work[1].rect.pos, FX_F32_TO_FX32(snmMtx.M03) - obsObjectWork.pos.x, FX_F32_TO_FX32(-snmMtx.M13) - obsObjectWork.pos.y, 0);
    }

    private static void gmBoss1ChainAtkHitFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        ((GMS_BOSS1_BODY_WORK)my_rect.parent_obj.parent_obj).flag |= 268435456U;
        GmEnemyDefaultAtkFunc(my_rect, your_rect);
    }

    private static void gmBoss1ChainOutFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_CHAIN_WORK gmsBosS1ChainWork = (GMS_BOSS1_CHAIN_WORK)obj_work;
        GmBsCmnUpdateCNMParam(obj_work, gmsBosS1ChainWork.cnm_mgr_work);
        ObjDrawActionSummary(obj_work);
    }

    private static void gmBoss1ChainWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_CHAIN_WORK gmsBosS1ChainWork = (GMS_BOSS1_CHAIN_WORK)obj_work;
        if (((int)GMM_BOSS1_MGR((GMS_BOSS1_BODY_WORK)obj_work.parent_obj).flag & 1) == 0)
            return;
        GmBsCmnInitBossMotionCBSystem(obj_work, gmsBosS1ChainWork.bmcb_mgr);
        GmBsCmnCreateSNMWork(gmsBosS1ChainWork.snm_work, obj_work.obj_3d._object, 10);
        GmBsCmnAppendBossMotionCallback(gmsBosS1ChainWork.bmcb_mgr, gmsBosS1ChainWork.snm_work.bmcb_link);
        gmsBosS1ChainWork.ball_snm_reg_id = GmBsCmnRegisterSNMNode(gmsBosS1ChainWork.snm_work, 11);
        for (int index = 0; index < 9; ++index)
            gmsBosS1ChainWork.sct_snm_reg_ids[index] = GmBsCmnRegisterSNMNode(gmsBosS1ChainWork.snm_work, 2 + index);
        GmBsCmnCreateCNMMgrWork(gmsBosS1ChainWork.cnm_mgr_work, obj_work.obj_3d._object, 9);
        GmBsCmnInitCNMCb(obj_work, gmsBosS1ChainWork.cnm_mgr_work);
        for (int index = 0; index < 9; ++index)
            gmsBosS1ChainWork.sct_cnm_reg_ids[index] = GmBsCmnRegisterCNMNode(gmsBosS1ChainWork.cnm_mgr_work, 2 + index);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1ChainMain);
    }

    private static void gmBoss1ChainMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_CHAIN_WORK chain_work = (GMS_BOSS1_CHAIN_WORK)obj_work;
        GMS_BOSS1_BODY_WORK parentObj = (GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        bool flag;
        if (((int)parentObj.flag & 1) != 0)
        {
            obj_work.disp_flag |= 4194304U;
            flag = true;
        }
        else
        {
            obj_work.disp_flag &= 4290772991U;
            flag = false;
        }
        if (((int)chain_work.flag & 1) != 0)
        {
            obj_work.obj_3d.flag &= 4294967294U;
            if (obj_work.obj_3d.marge > 0.0)
            {
                obj_work.obj_3d.marge -= obj_work.obj_3d.blend_spd;
            }
            else
            {
                chain_work.flag &= 4294967294U;
                if (gm_boss1_act_id_tbl[parentObj.whole_act_id][1].is_repeat != 0)
                    obj_work.disp_flag |= 4U;
                else
                    obj_work.disp_flag &= 4294967291U;
                obj_work.obj_3d.marge = 0.0f;
            }
        }
        if (((int)GMM_BS_OBJ(parentObj).disp_flag & 16) != 0)
            obj_work.disp_flag |= 16U;
        else if (((int)parentObj.flag & 128) == 0)
            obj_work.disp_flag &= 4294967279U;
        if (((int)parentObj.flag & 32) != 0)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
        if (((int)parentObj.flag & 64) != 0)
            obj_work.flag |= 2U;
        else
            obj_work.flag &= 4294967293U;
        if (((int)parentObj.flag & 8) != 0)
        {
            obj_work.disp_flag |= 16U;
            obj_work.flag |= 2U;
        }
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.chain_snm_reg_id, flag ? 1 : 0);
        if (((int)parentObj.flag & 134217728) != 0)
        {
            parentObj.flag &= 4160749567U;
            gmBoss1EffShockwaveInit(chain_work);
        }
        if (((int)parentObj.flag & 67108864) != 0)
        {
            parentObj.flag &= 4227858431U;
            gmBoss1EffScatterInit(chain_work);
        }
        gmBoss1ChainUpdateAtkRectPosition(chain_work);
    }


}