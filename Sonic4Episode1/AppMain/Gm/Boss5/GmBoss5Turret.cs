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
    public static AppMain.OBS_OBJECT_WORK GmBoss5TurretInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_TURRET_WORK()), "BOSS5_TRT");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS5_TURRET_WORK trt_work = (AppMain.GMS_BOSS5_TURRET_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss5GetObject3dList()[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = AppMain.GMD_BOSS5_DEFAULT_BLEND_SPD;
        work.flag |= 18U;
        work.disp_flag |= 4194304U;
        work.move_flag |= 256U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        AppMain.gmBoss5TurretInitDispRot(trt_work);
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5TurretMain);
        AppMain.gmBoss5TurretProcInit(trt_work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static AppMain.GMS_BOSS5_TURRET_WORK GmBoss5TurretStartUp(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork1 = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK obsObjectWork2 = AppMain.GmEventMgrLocalEventBirth((ushort)333, obsObjectWork1.pos.x, obsObjectWork1.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        obsObjectWork2.parent_obj = obsObjectWork1;
        return (AppMain.GMS_BOSS5_TURRET_WORK)obsObjectWork2;
    }

    public static void gmBoss5TurretGetDispRotatedOfstPos(
      AppMain.GMS_BOSS5_TURRET_WORK trt_work,
      ref AppMain.VecFx32 src_ofst_pos,
      out AppMain.VecFx32 dest_ofst_pos)
    {
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.amVectorSet(nnsVector, AppMain.FX_FX32_TO_F32(src_ofst_pos.x), AppMain.FX_FX32_TO_F32(-src_ofst_pos.y), AppMain.FX_FX32_TO_F32(src_ofst_pos.z));
        AppMain.nnMakeQuaternionMatrix(nnsMatrix, ref trt_work.disp_quat);
        AppMain.nnTransformVector(nnsVector, nnsMatrix, nnsVector);
        dest_ofst_pos = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(nnsVector.x), AppMain.FX_F32_TO_FX32(-nnsVector.y), AppMain.FX_F32_TO_FX32(nnsVector.z));
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    public static void gmBoss5TurretGetVulcanFirePos(
      AppMain.GMS_BOSS5_TURRET_WORK trt_work,
      ref AppMain.VecFx32 out_pos)
    {
        AppMain.VecFx32 src_ofst_pos = new AppMain.VecFx32(0, 0, AppMain.GMD_BOSS5_TURRET_VULCAN_FIRE_OFST_FORWARD);
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)trt_work);
        AppMain.VecFx32 dest_ofst_pos;
        AppMain.gmBoss5TurretGetDispRotatedOfstPos(trt_work, ref src_ofst_pos, out dest_ofst_pos);
        AppMain.VEC_Set(ref out_pos, obsObjectWork.pos.x + dest_ofst_pos.x, obsObjectWork.pos.y + dest_ofst_pos.y, obsObjectWork.pos.z + AppMain.GMD_BOSS5_TURRET_VULCAN_FIRE_OFST_Z);
    }

    public static void gmBoss5TurretGetVulcanBulletPos(
      AppMain.GMS_BOSS5_TURRET_WORK trt_work,
      ref AppMain.VecFx32 out_pos)
    {
        AppMain.VecFx32 src_ofst_pos = new AppMain.VecFx32(0, 0, AppMain.GMD_BOSS5_TURRET_VULCAN_BULLET_OFST_FORWARD);
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)trt_work);
        AppMain.VecFx32 dest_ofst_pos;
        AppMain.gmBoss5TurretGetDispRotatedOfstPos(trt_work, ref src_ofst_pos, out dest_ofst_pos);
        AppMain.VEC_Set(ref out_pos, obsObjectWork.pos.x + dest_ofst_pos.x, obsObjectWork.pos.y + dest_ofst_pos.y, obsObjectWork.pos.z + AppMain.GMD_BOSS5_TURRET_VULCAN_BULLET_OFST_Z);
    }

    public static void gmBoss5TurretInitDispRot(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)trt_work);
        obsObjectWork.disp_flag &= 4278190079U;
        AppMain.nnMakeUnitQuaternion(ref trt_work.disp_quat);
        AppMain.nnMakeUnitMatrix(obsObjectWork.obj_3d.user_obj_mtx_r);
    }

    public static void gmBoss5TurretUpdateDispRot(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)trt_work);
        obsObjectWork.disp_flag |= 16777216U;
        AppMain.nnMakeQuaternionMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, ref trt_work.disp_quat);
    }

    public static void gmBoss5TurretUpdateDirFollowingPos(
      AppMain.GMS_BOSS5_TURRET_WORK trt_work,
      ref AppMain.VecFx32 targ_pos,
      float deg)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)trt_work);
        int a1 = (int)((long)ushort.MaxValue & (long)((int)((long)ushort.MaxValue & (long)AppMain.nnArcTan2((double)AppMain.FX_FX32_TO_F32(targ_pos.y - obsObjectWork.pos.y), (double)AppMain.FX_FX32_TO_F32(targ_pos.x - obsObjectWork.pos.x))) - trt_work.fire_dir_z));
        int a2;
        if (a1 >= AppMain.AKM_DEGtoA32(180))
        {
            a1 = -(AppMain.AKM_DEGtoA32(360) - a1);
            a2 = AppMain.AKM_DEGtoA32(-deg);
        }
        else
            a2 = AppMain.AKM_DEGtoA32(deg);
        if (AppMain.MTM_MATH_ABS(a1) <= AppMain.MTM_MATH_ABS(a2))
            a2 = a1;
        trt_work.fire_dir_z = (int)(short)((long)ushort.MaxValue & (long)(trt_work.fire_dir_z + a2));
    }

    public static void gmBoss5TurretSetRoundFaceRot(
      AppMain.GMS_BOSS5_TURRET_WORK trt_work,
      int dir_z_angle,
      int tilt_near_angle)
    {
        int rz = (int)((long)ushort.MaxValue & (long)-dir_z_angle);
        AppMain.nnMakeRotateXZYQuaternion(out trt_work.disp_quat, AppMain.AKM_DEGtoA32(90), -tilt_near_angle, AppMain.AKM_DEGtoA32(90));
        AppMain.NNS_QUATERNION dst;
        AppMain.nnMakeRotateXYZQuaternion(out dst, 0, 0, rz);
        AppMain.nnMultiplyQuaternion(ref trt_work.disp_quat, ref dst, ref trt_work.disp_quat);
    }

    public static void gmBoss5TurretUpdateDirFacePly(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        AppMain.gmBoss5TurretUpdateDirFollowingPos(trt_work, ref playerObj.pos, AppMain.GMD_BOSS5_TURRET_FACE_PLY_SPD_DEG);
        float num = AppMain.nnSin(trt_work.fire_dir_z);
        if ((double)num < 0.0)
            num = 0.0f;
        int tilt_near_angle = (int)((double)num * (double)((long)ushort.MaxValue & (long)AppMain.GMD_BOSS5_TURRET_TILT_NEAR_ANGLE));
        AppMain.gmBoss5TurretSetRoundFaceRot(trt_work, trt_work.fire_dir_z, tilt_near_angle);
    }

    public static void gmBoss5TurretInitVulcanBurstShot(
      AppMain.GMS_BOSS5_TURRET_WORK trt_work,
      int shot_num)
    {
        trt_work.vul_shot_remain = shot_num;
        trt_work.vul_burst_timer = AppMain.GMD_BOSS5_TURRET_VULCAN_SHOT_INTERVAL;
        trt_work.vul_shot_angle = trt_work.fire_dir_z;
        AppMain.gmBoss5TurretGetVulcanFirePos(trt_work, ref trt_work.vul_fire_pos);
        AppMain.gmBoss5TurretGetVulcanBulletPos(trt_work, ref trt_work.vul_bullet_pos);
    }

    public static int gmBoss5TurretUpdateVulcanBurstShot(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        if (trt_work.vul_shot_remain == 0)
            return 1;
        if (trt_work.vul_burst_timer != 0)
        {
            --trt_work.vul_burst_timer;
        }
        else
        {
            AppMain.gmBoss5TurretGetVulcanFirePos(trt_work, ref trt_work.vul_fire_pos);
            AppMain.gmBoss5TurretGetVulcanBulletPos(trt_work, ref trt_work.vul_bullet_pos);
            AppMain.GmBoss5EfctCreateVulcanFire(trt_work, trt_work.vul_fire_pos, trt_work.vul_shot_angle);
            AppMain.GmBoss5EfctCreateVulcanBullet(trt_work, trt_work.vul_bullet_pos, trt_work.vul_shot_angle, AppMain.GMD_BOSS5_TURRET_VULCAN_BULLET_SPD);
            --trt_work.vul_shot_remain;
            trt_work.vul_burst_timer = AppMain.GMD_BOSS5_TURRET_VULCAN_SHOT_INTERVAL;
        }
        return 0;
    }

    public static void gmBoss5TurretClearVulcanBurstShot(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        trt_work.vul_shot_remain = 0;
        trt_work.vul_burst_timer = 0;
    }

    public static void gmBoss5TurretInitPartsPose(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)trt_work).parent_obj;
        int[] numArray = new int[2]
        {
      parentObj.pole_cnm_reg_id,
      parentObj.cover_cnm_reg_id
        };
        int num = 2;
        AppMain.nnMakeUnitMatrix(nnsMatrix);
        for (int index = 0; index < num; ++index)
        {
            AppMain.GmBsCmnChangeCNMModeNode(parentObj.cnm_mgr_work, numArray[index], 1U);
            AppMain.GmBsCmnEnableCNMLocalCoordinate(parentObj.cnm_mgr_work, numArray[index], 1);
            AppMain.GmBsCmnEnableCNMMtxNode(parentObj.cnm_mgr_work, numArray[index], 1);
            AppMain.GmBsCmnSetCNMMtx(parentObj.cnm_mgr_work, nnsMatrix, numArray[index]);
        }
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    public static void gmBoss5TurretEndPartsPose(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)trt_work).parent_obj;
        int[] numArray = new int[2]
        {
      parentObj.pole_cnm_reg_id,
      parentObj.cover_cnm_reg_id
        };
        int num = 2;
        for (int index = 0; index < num; ++index)
            AppMain.GmBsCmnEnableCNMMtxNode(parentObj.cnm_mgr_work, numArray[index], 0);
    }

    public static void gmBoss5TurretInitSlideTurret(
      AppMain.GMS_BOSS5_TURRET_WORK trt_work,
      int slide_type)
    {
        trt_work.trt_slide_type = slide_type;
        if (slide_type == 0)
            trt_work.trt_slide_length = 0.0f;
        else
            trt_work.trt_slide_length = AppMain.GMD_BOSS5_TURRET_SLIDE_LENGTH_MAX;
    }

    public static int gmBoss5TurretUpdateSlideTurret(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)trt_work).parent_obj;
        int num;
        if (trt_work.trt_slide_type == 0)
        {
            if ((double)trt_work.trt_slide_length < (double)AppMain.GMD_BOSS5_TURRET_SLIDE_LENGTH_MAX)
            {
                trt_work.trt_slide_length += AppMain.GMD_BOSS5_TURRET_SLIDE_RAISE_SPD_F;
                num = 0;
            }
            else
            {
                trt_work.trt_slide_length = AppMain.GMD_BOSS5_TURRET_SLIDE_LENGTH_MAX;
                num = 1;
            }
        }
        else if ((double)trt_work.trt_slide_length > 0.0)
        {
            trt_work.trt_slide_length -= AppMain.GMD_BOSS5_TURRET_SLIDE_LOWER_SPD_F;
            num = 0;
        }
        else
        {
            trt_work.trt_slide_length = 0.0f;
            num = 1;
        }
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMakeTranslateMatrix(nnsMatrix, 0.0f, trt_work.trt_slide_length + AppMain.GMD_BOSS5_TURRET_SLIDE_POLE_DISP_OFST_Y, 0.0f);
        AppMain.GmBsCmnSetCNMMtx(parentObj.cnm_mgr_work, nnsMatrix, parentObj.pole_cnm_reg_id);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
        return num;
    }

    public static void gmBoss5TurretInitSlideCover(
      AppMain.GMS_BOSS5_TURRET_WORK trt_work,
      int slide_type)
    {
        trt_work.cvr_slide_type = slide_type;
        if (slide_type == 0)
            trt_work.cvr_slide_ratio = 0.0f;
        else
            trt_work.cvr_slide_ratio = 1f;
    }

    public static int gmBoss5TurretUpdateSlideCover(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)trt_work).parent_obj;
        int num1;
        if (trt_work.cvr_slide_type == 0)
        {
            if ((double)trt_work.cvr_slide_ratio < 1.0)
            {
                trt_work.cvr_slide_ratio += AppMain.GMD_BOSS5_TURRET_COVER_SLIDE_OPEN_RATIO_SPD_F;
                num1 = 0;
            }
            else
            {
                trt_work.cvr_slide_ratio = 1f;
                num1 = 1;
            }
        }
        else if ((double)trt_work.cvr_slide_ratio > 0.0)
        {
            trt_work.cvr_slide_ratio -= AppMain.GMD_BOSS5_TURRET_COVER_SLIDE_CLOSE_RATIO_SPD_F;
            num1 = 0;
        }
        else
        {
            trt_work.cvr_slide_ratio = 0.0f;
            num1 = 1;
        }
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        float num2 = (float)(1.0 + (double)trt_work.cvr_slide_ratio * ((double)AppMain.GMD_BOSS5_TURRET_COVER_SLIDE_SCALE_MAX - 1.0));
        AppMain.nnMakeRotateXMatrix(nnsMatrix, AppMain.AKM_DEGtoA32(trt_work.cvr_slide_ratio * AppMain.GMD_BOSS5_TURRET_COVER_SLIDE_DEG_MAX));
        AppMain.nnScaleMatrix(nnsMatrix, nnsMatrix, num2, num2, num2);
        AppMain.GmBsCmnSetCNMMtx(parentObj.cnm_mgr_work, nnsMatrix, parentObj.cover_cnm_reg_id);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
        return num1;
    }

    public static void gmBoss5TurretMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_TURRET_WORK gmsBosS5TurretWork = (AppMain.GMS_BOSS5_TURRET_WORK)obj_work;
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.NNS_MATRIX turretMainTrtOfst = AppMain.gmBoss5TurretMain_trt_ofst;
        if (gmsBosS5TurretWork.proc_update != null)
            gmsBosS5TurretWork.proc_update(gmsBosS5TurretWork);
        AppMain.nnMakeTranslateMatrix(turretMainTrtOfst, 0.0f, gmsBosS5TurretWork.trt_slide_length, 0.0f);
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.pole_snm_reg_id, 0, obj_work.parent_obj.pos, parentObj.pivot_prev_pos, turretMainTrtOfst);
        AppMain.gmBoss5TurretUpdateDispRot(gmsBosS5TurretWork);
    }

    public static void gmBoss5TurretProcInit(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        trt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_TURRET_WORK(AppMain.gmBoss5TurretProcUpdateStandby);
    }

    public static void gmBoss5TurretProcUpdateStandby(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        if (((int)((AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)trt_work).parent_obj).flag & 512) != 0 || AppMain.gmBoss5TurretSeqGetVulcanShotNum(trt_work) <= 0)
            return;
        if (trt_work.wait_timer != 0U)
        {
            --trt_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5TurretInitPartsPose(trt_work);
            AppMain.gmBoss5TurretInitSlideCover(trt_work, 0);
            AppMain.gmBoss5TurretUpdateDirFollowingPos(trt_work, ref AppMain.GmBsCmnGetPlayerObj().pos, 360f);
            trt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_TURRET_WORK(AppMain.gmBoss5TurretProcUpdateOpen);
        }
    }

    public static void gmBoss5TurretProcUpdateOpen(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        if (AppMain.gmBoss5TurretUpdateSlideCover(trt_work) == 0)
            return;
        AppMain.gmBoss5TurretInitSlideTurret(trt_work, 0);
        trt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_TURRET_WORK(AppMain.gmBoss5TurretProcUpdateAppear);
    }

    public static void gmBoss5TurretProcUpdateAppear(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        AppMain.gmBoss5TurretUpdateDirFacePly(trt_work);
        if (AppMain.gmBoss5TurretUpdateSlideTurret(trt_work) == 0)
            return;
        trt_work.wait_timer = AppMain.GMD_BOSS5_TURRET_FACE_TIME;
        trt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_TURRET_WORK(AppMain.gmBoss5TurretProcUpdateFace);
    }

    public static void gmBoss5TurretProcUpdateFace(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)trt_work).parent_obj;
        AppMain.gmBoss5TurretUpdateDirFacePly(trt_work);
        if (((int)parentObj.flag & 512) != 0)
        {
            trt_work.wait_timer = 0U;
            trt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_TURRET_WORK(AppMain.gmBoss5TurretProcUpdateDisappear);
        }
        else if (trt_work.wait_timer != 0U)
        {
            --trt_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5TurretInitVulcanBurstShot(trt_work, AppMain.gmBoss5TurretSeqGetVulcanShotNum(trt_work));
            trt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_TURRET_WORK(AppMain.gmBoss5TurretProcUpdateFire);
        }
    }

    public static void gmBoss5TurretProcUpdateFire(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)trt_work).parent_obj;
        if (AppMain.gmBoss5TurretUpdateVulcanBurstShot(trt_work) == 0 && ((int)parentObj.flag & 512) == 0)
            return;
        AppMain.gmBoss5TurretClearVulcanBurstShot(trt_work);
        AppMain.gmBoss5TurretInitSlideTurret(trt_work, 1);
        trt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_TURRET_WORK(AppMain.gmBoss5TurretProcUpdateDisappear);
    }

    public static void gmBoss5TurretProcUpdateDisappear(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        if (AppMain.gmBoss5TurretUpdateSlideTurret(trt_work) == 0)
            return;
        trt_work.wait_timer = AppMain.gmBoss5TurretSeqGetVulcanWaitTime(trt_work);
        AppMain.gmBoss5TurretInitSlideCover(trt_work, 1);
        trt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_TURRET_WORK(AppMain.gmBoss5TurretProcUpdateClose);
    }

    public static void gmBoss5TurretProcUpdateClose(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        if (AppMain.gmBoss5TurretUpdateSlideCover(trt_work) == 0)
            return;
        AppMain.gmBoss5TurretEndPartsPose(trt_work);
        trt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_TURRET_WORK(AppMain.gmBoss5TurretProcUpdateStandby);
    }

    public static uint gmBoss5TurretSeqGetVulcanWaitTime(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        int life = ((AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)trt_work).parent_obj).mgr_work.life;
        AppMain.GMS_BOSS5_TURRET_SEQ_VUL_SHOT_INFO turretSeqVulShotInfo = (AppMain.GMS_BOSS5_TURRET_SEQ_VUL_SHOT_INFO)null;
        for (int index = 0; index < 5; ++index)
        {
            if (life <= AppMain.gm_boss5_trt_seq_vul_shot_info_tbl[index].life_threshold)
            {
                turretSeqVulShotInfo = AppMain.gm_boss5_trt_seq_vul_shot_info_tbl[index];
                break;
            }
        }
        return turretSeqVulShotInfo == null ? 0U : turretSeqVulShotInfo.wait_time;
    }

    public static int gmBoss5TurretSeqGetVulcanShotNum(AppMain.GMS_BOSS5_TURRET_WORK trt_work)
    {
        int life = ((AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)trt_work).parent_obj).mgr_work.life;
        AppMain.GMS_BOSS5_TURRET_SEQ_VUL_SHOT_INFO turretSeqVulShotInfo = (AppMain.GMS_BOSS5_TURRET_SEQ_VUL_SHOT_INFO)null;
        for (int index = 0; index < 5; ++index)
        {
            if (life <= AppMain.gm_boss5_trt_seq_vul_shot_info_tbl[index].life_threshold)
            {
                turretSeqVulShotInfo = AppMain.gm_boss5_trt_seq_vul_shot_info_tbl[index];
                break;
            }
        }
        return turretSeqVulShotInfo == null ? 0 : turretSeqVulShotInfo.shot_num;
    }

}