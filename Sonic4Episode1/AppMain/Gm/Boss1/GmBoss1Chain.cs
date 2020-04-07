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
    private static AppMain.OBS_OBJECT_WORK GmBoss1ChainInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS1_CHAIN_WORK()), "BOSS1_CHAIN");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS1_CHAIN_WORK gmsBosS1ChainWork = (AppMain.GMS_BOSS1_CHAIN_WORK)work;
        work.move_flag |= 256U;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_boss1_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(704), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = 0.125f;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1ChainWaitSetup);
        work.flag |= 16U;
        work.disp_flag |= 4U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[1], (short)-16, (short)-16, (short)16, (short)16);
        gmsEnemy3DWork.ene_com.rect_work[1].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss1ChainAtkHitFunc);
        gmsEnemy3DWork.ene_com.rect_work[0].flag |= 2048U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag |= 2048U;
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1ChainOutFunc);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss1ChainExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss1ChainExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        AppMain.GMS_BOSS1_CHAIN_WORK gmsBosS1ChainWork = (AppMain.GMS_BOSS1_CHAIN_WORK)tcbWork;
        AppMain.gmBoss1MgrDecObjCreateCount(gmsBosS1ChainWork.mgr_work);
        AppMain.GmBsCmnClearBossMotionCBSystem(tcbWork);
        AppMain.GmBsCmnDeleteSNMWork(gmsBosS1ChainWork.snm_work);
        AppMain.GmBsCmnClearCNMCb(tcbWork);
        AppMain.GmBsCmnDeleteCNMMgrWork(gmsBosS1ChainWork.cnm_mgr_work);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss1ChainUpdateAtkRectPosition(AppMain.GMS_BOSS1_CHAIN_WORK chain_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)chain_work);
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(chain_work.snm_work, chain_work.ball_snm_reg_id);
        AppMain.VEC_Set(ref chain_work.ene_3d.ene_com.rect_work[1].rect.pos, AppMain.FX_F32_TO_FX32(snmMtx.M03) - obsObjectWork.pos.x, AppMain.FX_F32_TO_FX32(-snmMtx.M13) - obsObjectWork.pos.y, 0);
    }

    private static void gmBoss1ChainAtkHitFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        ((AppMain.GMS_BOSS1_BODY_WORK)my_rect.parent_obj.parent_obj).flag |= 268435456U;
        AppMain.GmEnemyDefaultAtkFunc(my_rect, your_rect);
    }

    private static void gmBoss1ChainOutFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_CHAIN_WORK gmsBosS1ChainWork = (AppMain.GMS_BOSS1_CHAIN_WORK)obj_work;
        AppMain.GmBsCmnUpdateCNMParam(obj_work, gmsBosS1ChainWork.cnm_mgr_work);
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void gmBoss1ChainWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_CHAIN_WORK gmsBosS1ChainWork = (AppMain.GMS_BOSS1_CHAIN_WORK)obj_work;
        if (((int)AppMain.GMM_BOSS1_MGR((AppMain.GMS_BOSS1_BODY_WORK)obj_work.parent_obj).flag & 1) == 0)
            return;
        AppMain.GmBsCmnInitBossMotionCBSystem(obj_work, gmsBosS1ChainWork.bmcb_mgr);
        AppMain.GmBsCmnCreateSNMWork(gmsBosS1ChainWork.snm_work, obj_work.obj_3d._object, (ushort)10);
        AppMain.GmBsCmnAppendBossMotionCallback(gmsBosS1ChainWork.bmcb_mgr, gmsBosS1ChainWork.snm_work.bmcb_link);
        gmsBosS1ChainWork.ball_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(gmsBosS1ChainWork.snm_work, 11);
        for (int index = 0; index < 9; ++index)
            gmsBosS1ChainWork.sct_snm_reg_ids[index] = AppMain.GmBsCmnRegisterSNMNode(gmsBosS1ChainWork.snm_work, 2 + index);
        AppMain.GmBsCmnCreateCNMMgrWork(gmsBosS1ChainWork.cnm_mgr_work, obj_work.obj_3d._object, (ushort)9);
        AppMain.GmBsCmnInitCNMCb(obj_work, gmsBosS1ChainWork.cnm_mgr_work);
        for (int index = 0; index < 9; ++index)
            gmsBosS1ChainWork.sct_cnm_reg_ids[index] = AppMain.GmBsCmnRegisterCNMNode(gmsBosS1ChainWork.cnm_mgr_work, 2 + index);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1ChainMain);
    }

    private static void gmBoss1ChainMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_CHAIN_WORK chain_work = (AppMain.GMS_BOSS1_CHAIN_WORK)obj_work;
        AppMain.GMS_BOSS1_BODY_WORK parentObj = (AppMain.GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
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
            if ((double)obj_work.obj_3d.marge > 0.0)
            {
                obj_work.obj_3d.marge -= obj_work.obj_3d.blend_spd;
            }
            else
            {
                chain_work.flag &= 4294967294U;
                if (AppMain.gm_boss1_act_id_tbl[parentObj.whole_act_id][1].is_repeat != (byte)0)
                    obj_work.disp_flag |= 4U;
                else
                    obj_work.disp_flag &= 4294967291U;
                obj_work.obj_3d.marge = 0.0f;
            }
        }
        if (((int)AppMain.GMM_BS_OBJ((object)parentObj).disp_flag & 16) != 0)
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
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.chain_snm_reg_id, flag ? 1 : 0);
        if (((int)parentObj.flag & 134217728) != 0)
        {
            parentObj.flag &= 4160749567U;
            AppMain.gmBoss1EffShockwaveInit(chain_work);
        }
        if (((int)parentObj.flag & 67108864) != 0)
        {
            parentObj.flag &= 4227858431U;
            AppMain.gmBoss1EffScatterInit(chain_work);
        }
        AppMain.gmBoss1ChainUpdateAtkRectPosition(chain_work);
    }


}