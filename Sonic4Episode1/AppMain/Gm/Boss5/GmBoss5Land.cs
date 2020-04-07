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
    private static AppMain.OBS_OBJECT_WORK GmBoss5LandInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_BOSS5_LAND_PLACEMENT_INFO place_info = new AppMain.GMS_BOSS5_LAND_PLACEMENT_INFO();
        if (AppMain.gmBoss5LandGetPlacementInfo(place_info) == 0)
            return (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, place_info.pos_x, place_info.pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_LAND_WORK()), "BOSS5_LAND");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS5_LAND_WORK land_work = (AppMain.GMS_BOSS5_LAND_WORK)work;
        work.pos.z = -524288;
        work.flag |= 16U;
        work.flag &= 4294966271U;
        work.disp_flag &= 4294967263U;
        work.disp_flag |= 4194304U;
        work.move_flag |= 8448U;
        work.move_flag &= 4294967167U;
        for (int part_index = 0; part_index < place_info.part_num; ++part_index)
        {
            uint num = (uint)((ulong)part_index % 3UL);
            AppMain.gmBoss5LandCreateLdPart(land_work, AppMain.gm_boss5_land_place_pattern_tbl[(int)num], part_index);
        }
        AppMain.gmBoss5LandSetObjCollisionRect(land_work, place_info.part_num);
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5LandMain);
        AppMain.gmBoss5LandProcInit(land_work);
        return work;
    }

    private static AppMain.GMS_BOSS5_LAND_WORK GmBoss5LandCreate(
      AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork1 = AppMain.GMM_BS_OBJ((object)mgr_work);
        AppMain.OBS_OBJECT_WORK obsObjectWork2 = AppMain.GmEventMgrLocalEventBirth((ushort)344, obsObjectWork1.pos.x, obsObjectWork1.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        if (obsObjectWork2 == null)
            return (AppMain.GMS_BOSS5_LAND_WORK)null;
        ((AppMain.GMS_BOSS5_LAND_WORK)obsObjectWork2).mgr_work = mgr_work;
        return (AppMain.GMS_BOSS5_LAND_WORK)obsObjectWork2;
    }

    private static int gmBoss5LandGetPlacementInfo(AppMain.GMS_BOSS5_LAND_PLACEMENT_INFO place_info)
    {
        AppMain.GMS_EVE_RECORD_EVENT gmsEveRecordEvent = (AppMain.GMS_EVE_RECORD_EVENT)null;
        AppMain.OBS_OBJECT_WORK obj_work;
        for (obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)3); obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, (ushort)3))
        {
            AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
            if (gmsEnemyComWork.eve_rec != null && gmsEnemyComWork.eve_rec.id == (ushort)282)
            {
                gmsEveRecordEvent = gmsEnemyComWork.eve_rec;
                break;
            }
        }
        if (obj_work == null)
        {
            AppMain.mppAssertNotImpl();
            return 0;
        }
        place_info.pos_x = obj_work.pos.x;
        place_info.pos_y = obj_work.pos.y;
        int num = (int)gmsEveRecordEvent.left + (int)gmsEveRecordEvent.width << 3;
        place_info.part_num = (int)((long)num / (long)AppMain.GMD_BOSS5_LAND_LDPART_WIDTH_INT);
        return 1;
    }

    private static void gmBoss5LandSetObjCollisionRect(
      AppMain.GMS_BOSS5_LAND_WORK land_work,
      int part_num)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)land_work);
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)land_work;
        gmsEnemyComWork.col_work.obj_col.obj = AppMain.GMM_BS_OBJ((object)land_work);
        gmsEnemyComWork.col_work.obj_col.width = (ushort)((ulong)part_num * (ulong)AppMain.GMD_BOSS5_LAND_LDPART_WIDTH_INT);
        gmsEnemyComWork.col_work.obj_col.height = AppMain.GMD_BOSS5_LAND_LDPART_HEIGHT_INT;
        gmsEnemyComWork.col_work.obj_col.ofst_x = (short)0;
        gmsEnemyComWork.col_work.obj_col.ofst_y = (short)0;
        obsObjectWork.view_out_ofst_plus[0] = (short)((int)gmsEnemyComWork.col_work.obj_col.ofst_x - (int)(short)gmsEnemyComWork.col_work.obj_col.width);
        obsObjectWork.view_out_ofst_plus[2] = (short)((int)gmsEnemyComWork.col_work.obj_col.ofst_x + (int)(short)gmsEnemyComWork.col_work.obj_col.width);
    }

    private static void gmBoss5LandDisableObjCollision(AppMain.GMS_BOSS5_LAND_WORK land_work)
    {
        ((AppMain.GMS_ENEMY_COM_WORK)land_work).col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmBoss5LandMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_LAND_WORK wrk = (AppMain.GMS_BOSS5_LAND_WORK)obj_work;
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    private static void gmBoss5LandProcInit(AppMain.GMS_BOSS5_LAND_WORK land_work)
    {
        land_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_LAND_WORK(AppMain.gmBoss5LandProcUpdateIdle);
    }

    private static void gmBoss5LandProcUpdateIdle(AppMain.GMS_BOSS5_LAND_WORK land_work)
    {
        if (((int)land_work.mgr_work.flag & 536870912) == 0)
            return;
        land_work.flag |= AppMain.GMD_BOSS5_LAND_FLAG_SHAKE_ACTIVE;
        land_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_LAND_WORK(AppMain.gmBoss5LandProcUpdateShake);
    }

    private static void gmBoss5LandProcUpdateShake(AppMain.GMS_BOSS5_LAND_WORK land_work)
    {
        if (((int)land_work.mgr_work.flag & 1073741824) == 0)
            return;
        land_work.flag |= AppMain.GMD_BOSS5_LAND_FLAG_BREAK_ACTIVE;
        AppMain.gmBoss5LandDisableObjCollision(land_work);
        land_work.proc_update = (AppMain.MPP_VOID_GMS_BOSS5_LAND_WORK)null;
    }

    private static AppMain.GMS_BOSS5_LDPART_WORK gmBoss5LandCreateLdPart(
      AppMain.GMS_BOSS5_LAND_WORK land_work,
      int land_type,
      int part_index)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_LDPART_WORK()), AppMain.GMM_BS_OBJ((object)land_work), (ushort)0, "BOSS5_LAND_PART");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (AppMain.GMS_EFFECT_3DNN_WORK)work;
        AppMain.GMS_BOSS5_LDPART_WORK ldpart_work = (AppMain.GMS_BOSS5_LDPART_WORK)work;
        ldpart_work.part_index = part_index;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss5GetObject3dList()[AppMain.gm_boss5_land_mdl_amb_idx_tbl[land_type]], gmsEffect3DnnWork.obj_3d);
        work.obj_3d.drawflag = (uint)((ulong)work.obj_3d.drawflag & ulong.MaxValue);
        AppMain.ObjObjectAction3dNNMaterialMotionLoad(work, 0, AppMain.ObjDataGet(AppMain.gm_boss5_land_mat_mtn_dwork_no_tbl[land_type]), (string)null, AppMain.gm_boss5_land_mat_mtn_data_tbl[land_type], (object)null);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.flag |= 1024U;
        work.flag |= 18U;
        work.disp_flag |= 4194304U;
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        work.parent_ofst.x = AppMain.GMD_BOSS5_LAND_LDPART_WIDTH_FX / 2 + part_index * AppMain.GMD_BOSS5_LAND_LDPART_WIDTH_FX + AppMain.GMD_BOSS5_LAND_LDPART_CENTER_OFST_X_FX;
        work.parent_ofst.y = AppMain.GMD_BOSS5_LAND_LDPART_HEIGHT_FX / 2 + AppMain.GMD_BOSS5_LAND_LDPART_CENTER_OFST_Y_FX;
        work.parent_ofst.z = 0;
        ldpart_work.pivot_parent_ofst[0] = work.parent_ofst.x;
        ldpart_work.pivot_parent_ofst[1] = work.parent_ofst.y;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5LdPartMain);
        AppMain.gmBoss5LdPartProcInit(ldpart_work);
        return ldpart_work;
    }

    private static void gmBoss5LdPartInitSpin(AppMain.GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        AppMain.nnMakeUnitQuaternion(ref ldpart_work.cur_rot_quat);
        AppMain.nnMakeUnitQuaternion(ref ldpart_work.rot_diff_quat);
        for (int index = 0; (long)index < (long)AppMain.GMD_BOSS5_LAND_LDPART_SPIN_ROT_AXIS_NUM; ++index)
        {
            AppMain.NNS_VECTOR dst_vec = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            float rand_z = AppMain.MTM_MATH_CLIP((float)((double)AppMain.FX_FX32_TO_F32(AppMain.AkMathRandFx()) * 2.0 - 1.0), -1f, 1f);
            short rand_angle = AppMain.AKM_DEGtoA16(360f * AppMain.FX_FX32_TO_F32(AppMain.AkMathRandFx()));
            AppMain.AkMathGetRandomUnitVector(dst_vec, rand_z, rand_angle);
            AppMain.NNS_QUATERNION dst;
            AppMain.nnMakeRotateAxisQuaternion(out dst, dst_vec.x, dst_vec.y, dst_vec.z, AppMain.GMD_BOSS5_LAND_LDPART_SPIN_ROT_SPD_DEG);
            AppMain.nnMultiplyQuaternion(ref ldpart_work.rot_diff_quat, ref dst, ref ldpart_work.rot_diff_quat);
        }
    }

    private static void gmBoss5LdPartUpdateSpin(AppMain.GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)ldpart_work);
        AppMain.nnMultiplyQuaternion(ref ldpart_work.cur_rot_quat, ref ldpart_work.rot_diff_quat, ref ldpart_work.cur_rot_quat);
        AppMain.nnMakeQuaternionMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, ref ldpart_work.cur_rot_quat);
        obsObjectWork.disp_flag |= 16777216U;
    }

    private static void gmBoss5LdPartInitFall(AppMain.GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)ldpart_work);
        int ang = AppMain.AKM_DEGtoA32((int)AppMain.mtMathRand() % AppMain.GMD_BOSS5_LAND_LDPART_FALL_XY_DIR_RANGE_DEG + (270 - AppMain.GMD_BOSS5_LAND_LDPART_FALL_XY_DIR_RANGE_DEG / 2));
        int num = AppMain.FX_Mul(AppMain.AkMathRandFx(), AppMain.GMD_BOSS5_LAND_LDPART_FALL_Z_SPD_MAX * 2) - AppMain.GMD_BOSS5_LAND_LDPART_FALL_Z_SPD_MAX;
        obsObjectWork.spd.y = (int)(4096.0 * (double)AppMain.GMD_BOSS5_LAND_LDPART_FALL_XY_SPD_FL * (double)AppMain.nnSin(ang));
        obsObjectWork.spd.x = (int)(4096.0 * (double)AppMain.GMD_BOSS5_LAND_LDPART_FALL_XY_SPD_FL * (double)AppMain.nnCos(ang));
        obsObjectWork.spd.z = num;
        obsObjectWork.flag &= 4294966271U;
        obsObjectWork.move_flag |= 128U;
    }

    private static void gmBoss5LdPartInitVib(AppMain.GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        ldpart_work.vib_cnt = (int)AppMain.mtMathRand() % 40;
        ldpart_work.vib_ofst[0] = ldpart_work.vib_ofst[1] = 0;
    }

    private static void gmBoss5LdPartUpdateVib(AppMain.GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        ldpart_work.vib_ofst[0] = AppMain.FX_Mul(AppMain.gm_boss5_land_vib_tbl[ldpart_work.vib_cnt][0], AppMain.GMD_BOSS5_LAND_LDPART_VIB_AMPLITUDE);
        ldpart_work.vib_ofst[1] = AppMain.FX_Mul(AppMain.gm_boss5_land_vib_tbl[ldpart_work.vib_cnt][1], AppMain.GMD_BOSS5_LAND_LDPART_VIB_AMPLITUDE);
        ++ldpart_work.vib_cnt;
        if (ldpart_work.vib_cnt < 40)
            return;
        ldpart_work.vib_cnt = 0;
    }

    private static void gmBoss5LdPartClearVib(AppMain.GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        ldpart_work.vib_cnt = 0;
        ldpart_work.vib_ofst[0] = ldpart_work.vib_ofst[1] = 0;
    }

    private static void gmBoss5LdPartMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_LDPART_WORK wrk = (AppMain.GMS_BOSS5_LDPART_WORK)obj_work;
        obj_work.parent_ofst.x = wrk.pivot_parent_ofst[0] + wrk.vib_ofst[0];
        obj_work.parent_ofst.y = wrk.pivot_parent_ofst[1] + wrk.vib_ofst[1];
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    private static void gmBoss5LdPartProcInit(AppMain.GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)ldpart_work);
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag |= 4U;
        ldpart_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_LDPART_WORK(AppMain.gmBoss5LdPartProcUpdateIdle);
    }

    private static void gmBoss5LdPartProcUpdateIdle(AppMain.GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        if (((int)((AppMain.GMS_BOSS5_LAND_WORK)AppMain.GMM_BS_OBJ((object)ldpart_work).parent_obj).flag & (int)AppMain.GMD_BOSS5_LAND_FLAG_SHAKE_ACTIVE) == 0)
            return;
        AppMain.gmBoss5LdPartInitVib(ldpart_work);
        ldpart_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_LDPART_WORK(AppMain.gmBoss5LdPartProcUpdateShake);
    }

    private static void gmBoss5LdPartProcUpdateShake(AppMain.GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        AppMain.GMS_BOSS5_LAND_WORK parentObj = (AppMain.GMS_BOSS5_LAND_WORK)AppMain.GMM_BS_OBJ((object)ldpart_work).parent_obj;
        AppMain.gmBoss5LdPartUpdateVib(ldpart_work);
        if (((int)parentObj.flag & (int)AppMain.GMD_BOSS5_LAND_FLAG_BREAK_ACTIVE) == 0)
            return;
        AppMain.gmBoss5LdPartClearVib(ldpart_work);
        AppMain.gmBoss5LdPartInitSpin(ldpart_work);
        AppMain.gmBoss5LdPartInitFall(ldpart_work);
        ldpart_work.wait_timer = (uint)(ldpart_work.part_index & 1);
        ldpart_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_LDPART_WORK(AppMain.gmBoss5LdPartProcUpdateFall);
    }

    private static void gmBoss5LdPartProcUpdateFall(AppMain.GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        if (ldpart_work.wait_timer != 0U)
            --ldpart_work.wait_timer;
        else if (ldpart_work.brk_glass_cnt == 0U)
        {
            AppMain.GmBoss5EfctCreateBreakingGlass(AppMain.GMM_BS_OBJ((object)ldpart_work));
            ++ldpart_work.brk_glass_cnt;
        }
        AppMain.gmBoss5LdPartUpdateSpin(ldpart_work);
    }

    private static void GmBoss5LandSetLight()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.x = 0.0f;
        nnsVector.y = -0.2f;
        nnsVector.z = -1f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_1, ref col, 1f, nnsVector);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }


    private static AppMain.OBS_OBJECT_WORK GmGmkBoss5LandPlaceInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BOSS5_LAND_PLACE_WORK()), "BOSS5_LAND_PLACE");
        work.flag |= 16U;
        work.disp_flag &= 4294967263U;
        work.move_flag |= 8448U;
        work.move_flag &= 4294967167U;
        return work;
    }
}