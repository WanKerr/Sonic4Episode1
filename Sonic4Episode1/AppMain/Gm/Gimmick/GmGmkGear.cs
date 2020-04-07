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
    private static AppMain.OBS_OBJECT_WORK GmGmkGearInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        AppMain.OBS_OBJECT_WORK obsObjectWork = eve_rec.id != (ushort)184 ? AppMain.GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_GEAR_WORK()), "GMK_GEAR") : AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_GEAR_WORK()), "GMK_GEAR_SW");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obsObjectWork;
        AppMain.GMS_GMK_GEAR_WORK gear_work = (AppMain.GMS_GMK_GEAR_WORK)obsObjectWork;
        if (eve_rec.id == (ushort)182 && eve_rec.byte_param[1] != (byte)0)
        {
            obsObjectWork.disp_flag |= 32U;
            obsObjectWork.move_flag |= 8448U;
            obsObjectWork.flag |= 2U;
            return obsObjectWork;
        }
        obsObjectWork.ppIn = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearInFunc);
        AppMain.mtTaskChangeTcbDestructor(obsObjectWork.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkGearDest));
        gear_work.h_snd_gear = AppMain.GsSoundAllocSeHandle();
        AppMain.GmSoundStopSE(gear_work.h_snd_gear);
        AppMain.GmSoundPlaySE("Gear", gear_work.h_snd_gear);
        if (eve_rec.id == (ushort)181)
        {
            float num = 0.2f;
            AppMain.ObjObjectCopyAction3dNNModel(obsObjectWork, AppMain.gm_gmk_gear_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            obsObjectWork.pos.z = -131072;
            obsObjectWork.ppLast = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearLastFunc);
            float val = num + 0.2f * (float)AppMain.MTM_MATH_CLIP((int)eve_rec.left, 0, 3);
            gear_work.h_snd_gear.au_player.SetAisac("Speed", val);
        }
        else if (eve_rec.id == (ushort)182)
        {
            AppMain.ObjObjectCopyAction3dNNModel(obsObjectWork, AppMain.gm_gmk_gear_move_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            obsObjectWork.pos.z = -131072;
            AppMain.ObjCopyAction3dNNModel(AppMain.gm_gmk_gear_opt_obj_3d_list[0], gear_work.obj_3d_gear_opt);
            AppMain.ObjCopyAction3dNNModel(AppMain.gm_gmk_gear_opt_obj_3d_list[1], gear_work.obj_3d_gear_opt_ashiba);
            obsObjectWork.ppLast = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearMoveLastFunc);
        }
        else
        {
            AppMain.ObjObjectCopyAction3dNNModel(obsObjectWork, AppMain.gm_gmk_gear_sw_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            obsObjectWork.pos.z = -131072;
            AppMain.ObjCopyAction3dNNModel(AppMain.gm_gmk_gear_opt_obj_3d_list[0], gear_work.obj_3d_gear_opt);
            AppMain.ObjCopyAction3dNNModel(AppMain.gm_gmk_gear_opt_obj_3d_list[1], gear_work.obj_3d_gear_opt_ashiba);
            obsObjectWork.ppLast = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearMoveLastFunc);
        }
        AppMain.OBS_COLLISION_WORK colWork = gear_work.gmk_work.ene_com.col_work;
        colWork.obj_col.obj = (AppMain.OBS_OBJECT_WORK)gear_work;
        colWork.obj_col.width = (ushort)176;
        colWork.obj_col.height = (ushort)176;
        colWork.obj_col.ofst_x = (short)-88;
        colWork.obj_col.ofst_y = (short)-88;
        colWork.obj_col.flag |= 402653216U;
        obsObjectWork.move_flag &= 4294967167U;
        obsObjectWork.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        gmsEnemy3DWork.obj_3d.use_light_flag &= 4294967294U;
        gmsEnemy3DWork.obj_3d.use_light_flag |= 2U;
        gear_work.obj_3d_gear_opt_ashiba.use_light_flag &= 4294967294U;
        gear_work.obj_3d_gear_opt_ashiba.use_light_flag |= 2U;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
        {
            gear_work.obj_3d_gear_opt.use_light_flag &= 4294967294U;
            gear_work.obj_3d_gear_opt.use_light_flag |= 2U;
        }
        if (eve_rec.id == (ushort)181)
        {
            obsObjectWork.move_flag |= 8192U;
            obsObjectWork.move_flag |= 256U;
            gear_work.dir_speed = (float)(32.0 + (double)AppMain.MTM_MATH_CLIP((int)eve_rec.left, 0, 3) * 32.0);
            if (((int)eve_rec.flag & 1) != 0)
                gear_work.dir_speed = -gear_work.dir_speed;
            int num = (int)(65536.0 / (double)AppMain.MTM_MATH_ABS(gear_work.dir_speed));
            gear_work.dir_temp = (float)((long)AppMain.g_gm_main_system.sync_time % (long)num) * gear_work.dir_speed;
            gear_work.dir_temp += (float)(AppMain.MTM_MATH_CLIP((int)eve_rec.top, 0, 3) * 2048);
            float a = gear_work.dir_temp / 65536f;
            if ((double)AppMain.MTM_MATH_ABS(a) >= 1.0)
                gear_work.dir_temp -= a * 65536f;
            obsObjectWork.dir.z = (double)gear_work.dir_speed <= 0.0 ? (ushort)((uint)((double)AppMain.nnRoundOff(gear_work.dir_temp) + 1023.0) / 1024U * 1024U) : (ushort)((uint)AppMain.nnRoundOff(gear_work.dir_temp) / 1024U * 1024U);
            gear_work.prev_dir = obsObjectWork.dir.z;
            AppMain.gmGmkGearChangeCol(gear_work);
            AppMain.gmGmkGearFwInit(obsObjectWork);
        }
        else if (eve_rec.id == (ushort)182)
        {
            obsObjectWork.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearMoveSwDraw);
            byte[] numArray1;
            if (AppMain.gm_gmk_gear_add_data[16] is AppMain.AmbChunk)
            {
                AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)AppMain.gm_gmk_gear_add_data[16];
                numArray1 = new byte[ambChunk.length];
                Buffer.BlockCopy((Array)ambChunk.array, ambChunk.offset, (Array)numArray1, 0, ambChunk.length);
                AppMain.gm_gmk_gear_add_data[16] = (object)numArray1;
            }
            else
                numArray1 = (byte[])AppMain.gm_gmk_gear_add_data[16];
            colWork.obj_col.diff_data = numArray1;
            byte[] numArray2;
            if (AppMain.gm_gmk_gear_add_data[17] is AppMain.AmbChunk)
            {
                AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)AppMain.gm_gmk_gear_add_data[17];
                numArray2 = new byte[ambChunk.length];
                Buffer.BlockCopy((Array)ambChunk.array, ambChunk.offset, (Array)numArray2, 0, ambChunk.length);
                AppMain.gm_gmk_gear_add_data[17] = (object)numArray2;
            }
            else
                numArray2 = (byte[])AppMain.gm_gmk_gear_add_data[17];
            colWork.obj_col.dir_data = numArray2;
            obsObjectWork.flag &= 4294967294U;
            obsObjectWork.disp_flag |= 16777472U;
            obsObjectWork.move_flag &= 4294967039U;
            obsObjectWork.move_flag |= 704U;
            if (((int)eve_rec.flag & 8) != 0)
                obsObjectWork.user_flag |= 4U;
            AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[2];
            pRec1.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkMoveGearBodyDefFunc);
            pRec1.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
            AppMain.ObjRectWorkSet(pRec1, (short)-16, (short)-72, (short)16, (short)-48);
            AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
            pRec2.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkMoveGearDefFunc);
            pRec2.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
            AppMain.ObjRectAtkSet(pRec2, (ushort)0, (short)0);
            AppMain.ObjRectDefSet(pRec2, (ushort)65527, (short)0);
            AppMain.ObjRectWorkSet(pRec2, (short)-80, (short)-80, (short)80, (short)80);
            pRec2.flag |= 32U;
            AppMain.ObjObjectFieldRectSet(obsObjectWork, (short)-8, (short)-8, (short)8, (short)8);
            if (type == (byte)0)
            {
                obsObjectWork.move_flag |= 1U;
                obsObjectWork.prev_pos.Assign(obsObjectWork.pos);
                if (((int)eve_rec.flag & 1) != 0)
                {
                    obsObjectWork.dir.z = (ushort)16384;
                    obsObjectWork.pos.x += 32768;
                    obsObjectWork.prev_pos.x -= 32768;
                    obsObjectWork.move.x = 65536;
                    obsObjectWork.spd.x = 65536;
                    obsObjectWork.move_flag &= 4294967167U;
                    obsObjectWork.user_flag |= 8U;
                }
                else
                {
                    obsObjectWork.pos.y += 32768;
                    obsObjectWork.prev_pos.y -= 32768;
                    obsObjectWork.move.y = 65536;
                    obsObjectWork.spd.y = 65536;
                    obsObjectWork.user_flag &= 4294967287U;
                }
                AppMain.g_obj.ppCollision(obsObjectWork);
                obsObjectWork.move_flag |= 1U;
                vecFx32.Assign(obsObjectWork.pos);
                obsObjectWork.prev_pos.Assign(obsObjectWork.pos);
                obsObjectWork.move.x = obsObjectWork.move.y = 0;
                obsObjectWork.spd.x = obsObjectWork.spd.y = 0;
                if (((int)eve_rec.flag & 1) != 0)
                {
                    if (((int)eve_rec.flag & 2) != 0)
                    {
                        obsObjectWork.prev_pos.y -= 32768;
                        obsObjectWork.move.y = 32768;
                        obsObjectWork.spd.y = 32768;
                        obsObjectWork.disp_flag |= 1U;
                    }
                    else
                    {
                        obsObjectWork.prev_pos.y += 32768;
                        obsObjectWork.move.y = (int)short.MinValue;
                        obsObjectWork.spd.y = (int)short.MinValue;
                    }
                }
                else if (((int)eve_rec.flag & 2) != 0)
                {
                    obsObjectWork.prev_pos.x -= 32768;
                    obsObjectWork.move.x = 32768;
                    obsObjectWork.spd.x = 32768;
                    obsObjectWork.disp_flag |= 1U;
                }
                else
                {
                    obsObjectWork.prev_pos.x += 32768;
                    obsObjectWork.move.x = (int)short.MinValue;
                    obsObjectWork.spd.x = (int)short.MinValue;
                }
                AppMain.g_obj.ppCollision(obsObjectWork);
                if (((int)obsObjectWork.move_flag & 8) != 0)
                {
                    if (((int)eve_rec.flag & 1) != 0)
                        obsObjectWork.prev_pos.y = vecFx32.y;
                    else
                        obsObjectWork.prev_pos.x = vecFx32.x;
                }
                else
                    obsObjectWork.pos.Assign(vecFx32);
                gear_work.move_end_x = ((int)eve_rec.flag & 2) == 0 ? (gear_work.move_end_y = int.MaxValue) : (gear_work.move_end_y = 0);
                obsObjectWork.prev_pos.Assign(obsObjectWork.pos);
                obsObjectWork.move.x = obsObjectWork.move.y = 0;
                obsObjectWork.spd.x = obsObjectWork.spd.y = 0;
                obsObjectWork.disp_flag &= 4294967294U;
                obsObjectWork.move_flag |= 1U;
                AppMain.gmGmkMoveGearFwInit(obsObjectWork);
            }
            else
            {
                AppMain.nnMakeRotateZMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, 2730);
                AppMain.gmGmkMoveGearEndInit(obsObjectWork);
            }
        }
        else
        {
            obsObjectWork.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearMoveSwDraw);
            byte[] numArray1;
            if (AppMain.gm_gmk_gear_add_data[16] is AppMain.AmbChunk)
            {
                AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)AppMain.gm_gmk_gear_add_data[16];
                numArray1 = new byte[ambChunk.length];
                Buffer.BlockCopy((Array)ambChunk.array, ambChunk.offset, (Array)numArray1, 0, ambChunk.length);
                AppMain.gm_gmk_gear_add_data[16] = (object)numArray1;
            }
            else
                numArray1 = (byte[])AppMain.gm_gmk_gear_add_data[16];
            colWork.obj_col.diff_data = numArray1;
            byte[] numArray2;
            if (AppMain.gm_gmk_gear_add_data[17] is AppMain.AmbChunk)
            {
                AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)AppMain.gm_gmk_gear_add_data[17];
                numArray2 = new byte[ambChunk.length];
                Buffer.BlockCopy((Array)ambChunk.array, ambChunk.offset, (Array)numArray2, 0, ambChunk.length);
                AppMain.gm_gmk_gear_add_data[17] = (object)numArray2;
            }
            else
                numArray2 = (byte[])AppMain.gm_gmk_gear_add_data[17];
            colWork.obj_col.dir_data = numArray2;
            AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[1];
            AppMain.ObjRectGroupSet(pRec, (byte)0, (byte)2);
            pRec.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
            pRec.ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkGearSwitchAtkHitFunc);
            AppMain.ObjRectAtkSet(pRec, (ushort)8, (short)1);
            AppMain.ObjRectDefSet(pRec, ushort.MaxValue, (short)0);
            AppMain.ObjRectWorkSet(pRec, (short)-92, (short)-92, (short)92, (short)92);
            pRec.flag |= 1024U;
            obsObjectWork.disp_flag |= 16777472U;
            obsObjectWork.move_flag |= 8448U;
            gear_work.open_rot_dist = 65536;
            if (eve_rec.height > (byte)0)
                gear_work.close_rot_spd = 65536 / ((int)eve_rec.height * 30);
            AppMain.gmGmkGearSwFwInit(obsObjectWork);
        }
        return obsObjectWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkGearMoveEndInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK rideWork = AppMain.GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_GEAR_WORK()), "GMK_GEAR_END");
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)rideWork;
        if (eve_rec.byte_param[1] != (byte)0)
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)182, pos_x, pos_y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)1);
            rideWork.parent_obj = obsObjectWork;
            rideWork.flag |= 18U;
        }
        else
        {
            rideWork.ppIn = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearInFunc);
            AppMain.OBS_RECT_WORK pRec = gmsEnemyComWork.rect_work[1];
            AppMain.ObjRectGroupSet(pRec, (byte)0, (byte)2);
            pRec.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
            pRec.ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkMoveGearEndAtkHitFunc);
            AppMain.ObjRectAtkSet(pRec, (ushort)8, (short)1);
            AppMain.ObjRectDefSet(pRec, ushort.MaxValue, (short)0);
            AppMain.ObjRectWorkSet(pRec, (short)-32, (short)-32, (short)32, (short)32);
            pRec.flag |= 1024U;
            AppMain.gmGmkMoveGearEndSwitchFwInit(rideWork);
        }
        rideWork.move_flag |= 8448U;
        rideWork.disp_flag |= 32U;
        return rideWork;
    }

    public static void GmGmkGearSetLight()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.x = -0.35f;
        nnsVector.y = 2.25f;
        nnsVector.z = -0.9f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        float intensity = AppMain.g_gs_main_sys_info.stage_id != (ushort)14 ? 1f : 0.9f;
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_1, ref col, intensity, nnsVector);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    public static void GmGmkGearBuild()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(887);
        for (int index = 0; index < 19; ++index)
            AppMain.gm_gmk_gear_add_data[index] = (object)(AppMain.AmbChunk)AppMain.amBindGet(gimmickData, index);
        AppMain.gm_gmk_gear_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(888), AppMain.GmGameDatGetGimmickData(889), 0U);
        AppMain.gm_gmk_gear_move_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(890), AppMain.GmGameDatGetGimmickData(889), 0U);
        AppMain.gm_gmk_gear_sw_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(890), AppMain.GmGameDatGetGimmickData(889), 0U, AppMain.readTXBfile(AppMain.gm_gmk_gear_add_data[18]));
        AppMain.gm_gmk_gear_opt_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(891), AppMain.GmGameDatGetGimmickData(889), 0U);
    }

    public static void GmGmkGearFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData1 = AppMain.GmGameDatGetGimmickData(888);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_gear_obj_3d_list, gimmickData1.file_num);
        AppMain.AMS_AMB_HEADER gimmickData2 = AppMain.GmGameDatGetGimmickData(890);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_gear_move_obj_3d_list, gimmickData2.file_num);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_gear_sw_obj_3d_list, gimmickData2.file_num);
        AppMain.AMS_AMB_HEADER gimmickData3 = AppMain.GmGameDatGetGimmickData(891);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_gear_opt_obj_3d_list, gimmickData3.file_num);
        for (int index = 0; index < AppMain.gm_gmk_gear_add_data.Length; ++index)
            AppMain.gm_gmk_gear_add_data[index] = (object)null;
    }

    private static void gmGmkGearInFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        AppMain.GmEnemyDefaultInFunc(obj_work);
        if (gmsGmkGearWork.gear_end_obj != null && ((int)gmsGmkGearWork.gear_end_obj.flag & 12) != 0)
            gmsGmkGearWork.gear_end_obj = (AppMain.OBS_OBJECT_WORK)null;
        if (gmsGmkGearWork.move_gear_obj != null && ((int)gmsGmkGearWork.move_gear_obj.flag & 12) != 0)
            gmsGmkGearWork.move_gear_obj = (AppMain.OBS_OBJECT_WORK)null;
        if (gmsGmkGearWork.sw_gear_obj == null || ((int)gmsGmkGearWork.sw_gear_obj.flag & 12) == 0)
            return;
        gmsGmkGearWork.sw_gear_obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmGmkGearDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GMK_GEAR_WORK tcbWork = (AppMain.GMS_GMK_GEAR_WORK)AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.h_snd_gear != null)
        {
            AppMain.GmSoundStopSE(tcbWork.h_snd_gear);
            AppMain.GsSoundFreeSeHandle(tcbWork.h_snd_gear);
            tcbWork.h_snd_gear = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkGearFwInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearFwMain);
    }

    private static void gmGmkGearFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gear_work = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        gear_work.dir_temp += gear_work.dir_speed;
        if ((double)gear_work.dir_temp < -65536.0)
            gear_work.dir_temp += 65536f;
        else if ((double)gear_work.dir_temp > 65536.0)
            gear_work.dir_temp -= 65536f;
        gear_work.prev_dir = obj_work.dir.z;
        obj_work.dir.z = (double)gear_work.dir_speed <= 0.0 ? (ushort)(((int)AppMain.nnRoundOff(gear_work.dir_temp) + 1023) / 1024 * 1024) : (ushort)((int)AppMain.nnRoundOff(gear_work.dir_temp) / 1024 * 1024);
        gear_work.col_type = (uint)((int)obj_work.dir.z / 1024 % 8);
        AppMain.gmGmkGearChangeCol(gear_work);
        if ((int)gear_work.prev_dir == (int)obj_work.dir.z || obj_work.col_work.obj_col.rider_obj == null || obj_work.col_work.obj_col.rider_obj.obj_type != (ushort)1)
            return;
        AppMain.OBS_OBJECT_WORK riderObj = obj_work.col_work.obj_col.rider_obj;
        float num1 = AppMain.FXM_FX32_TO_FLOAT(riderObj.pos.x - obj_work.pos.x);
        float num2 = -AppMain.FXM_FX32_TO_FLOAT(riderObj.pos.y - obj_work.pos.y);
        int ang = (int)obj_work.dir.z - (int)gear_work.prev_dir;
        float num3 = (float)((double)AppMain.nnCos(ang) * (double)num1 + (double)AppMain.nnSin(ang) * (double)num2);
        float num4 = (float)(-(double)AppMain.nnSin(ang) * (double)num1 + (double)AppMain.nnCos(ang) * (double)num2);
        riderObj.prev_pos.x = riderObj.pos.x;
        riderObj.prev_pos.y = riderObj.pos.y;
        riderObj.pos.x += AppMain.FXM_FLOAT_TO_FX32(num3 - num1);
        riderObj.pos.y += -AppMain.FXM_FLOAT_TO_FX32(num4 - num2);
        riderObj.move.x = riderObj.pos.x - riderObj.prev_pos.x;
        riderObj.move.y = riderObj.pos.y - riderObj.prev_pos.y;
    }

    private static void gmGmkGearChangeCol(AppMain.GMS_GMK_GEAR_WORK gear_work)
    {
        byte[] numArray;
        if (AppMain.gm_gmk_gear_add_data[(int)(gear_work.col_type * 2U)] is AppMain.AmbChunk)
        {
            AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)AppMain.gm_gmk_gear_add_data[(int)(gear_work.col_type * 2U)];
            numArray = new byte[ambChunk.length];
            Buffer.BlockCopy((Array)ambChunk.array, ambChunk.offset, (Array)numArray, 0, ambChunk.length);
            AppMain.gm_gmk_gear_add_data[(int)(gear_work.col_type * 2U)] = (object)numArray;
        }
        else
            numArray = (byte[])AppMain.gm_gmk_gear_add_data[(int)(gear_work.col_type * 2U)];
        gear_work.gmk_work.ene_com.col_work.obj_col.diff_data = numArray;
    }
    private static void gmGmkGearSwFwInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        gmsGmkGearWork.open_rot_dist = 65536;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearSwFwMain);
        gmsGmkGearWork.move_gear_obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmGmkGearSwFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkGearSwRotExtWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.ppFunc == new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearSwFwMain))
            AppMain.GmSoundPlaySE("Gear2", (AppMain.GSS_SND_SE_HANDLE)null);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearSwRotExtWaitMain);
    }

    private static void gmGmkGearSwRotExtWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.move_gear_obj == null)
        {
            AppMain.gmGmkGearSwFwInit(obj_work);
        }
        else
        {
            AppMain.GMS_GMK_GEAR_WORK moveGearObj = (AppMain.GMS_GMK_GEAR_WORK)gmsGmkGearWork.move_gear_obj;
            int num1 = (int)(ushort)(((int)gmsGmkGearWork.move_draw_dir - 5461) / 5461 * 2);
            int num2 = (int)(ushort)(((int)moveGearObj.move_draw_dir - 2730) / 5461 * 2);
            if (num1 == num2)
                return;
            gmsGmkGearWork.move_draw_dir = (ushort)-((int)moveGearObj.move_draw_dir - (2730 - num2 * 5461) / 2);
            AppMain.gmGmkGearSwRotExtInit(obj_work);
        }
    }

    private static void gmGmkGearSwRotExtInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGearSwRotExtMain);
    }

    private static void gmGmkGearSwRotExtMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.move_gear_obj == null)
        {
            obj_work.flag &= 4294967279U;
            AppMain.gmGmkGearSwFwInit(obj_work);
        }
        else
        {
            AppMain.GMS_GMK_GEAR_WORK moveGearObj = (AppMain.GMS_GMK_GEAR_WORK)gmsGmkGearWork.move_gear_obj;
            int num = (int)moveGearObj.move_draw_dir_spd;
            gmsGmkGearWork.move_draw_dir -= (ushort)num;
            gmsGmkGearWork.move_draw_dir_ofst = (short)-moveGearObj.move_draw_dir_ofst;
            AppMain.nnMakeRotateZMatrix(obj_work.obj_3d.user_obj_mtx_r, (int)gmsGmkGearWork.move_draw_dir * 2 + (int)gmsGmkGearWork.move_draw_dir_ofst);
            if (gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.rider_obj != null && gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.rider_obj.obj_type == (ushort)1)
                AppMain.gmGmkGearSetRotFlow(obj_work, (AppMain.GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.rider_obj, -num * 2);
            else if (gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.toucher_obj != null && gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.toucher_obj.obj_type == (ushort)1)
                AppMain.gmGmkGearSetRotFlow(obj_work, (AppMain.GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.toucher_obj, -num * 2);
            if (((int)moveGearObj.gmk_work.ene_com.eve_rec.flag & 1) != 0)
            {
                if (((int)moveGearObj.gmk_work.ene_com.eve_rec.flag & 2) == 0)
                    num = -num;
            }
            else if (((int)moveGearObj.gmk_work.ene_com.eve_rec.flag & 2) != 0)
                num = -num;
            gmsGmkGearWork.open_rot_dist += num;
            int numer = AppMain.MTM_MATH_CLIP(gmsGmkGearWork.open_rot_dist, 0, 65536);
            if (num <= 0)
                AppMain.GmGmkSwitchSetOnGearSwitch((uint)gmsGmkGearWork.gmk_work.ene_com.eve_rec.width, AppMain.FX_Div(numer, 65536));
            else
                AppMain.GmGmkSwitchSetOffGearSwitch((uint)gmsGmkGearWork.gmk_work.ene_com.eve_rec.width, AppMain.FX_Div(numer, 65536));
            if (gmsGmkGearWork.open_rot_dist <= 0)
            {
                if (gmsGmkGearWork.gmk_work.ene_com.eve_rec.height == (byte)0)
                {
                    gmsGmkGearWork.gmk_work.ene_com.eve_rec.byte_param[1] = (byte)1;
                    obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
                    gmsGmkGearWork.move_gear_obj = (AppMain.OBS_OBJECT_WORK)null;
                }
                else
                    obj_work.flag |= 16U;
            }
            else
            {
                if (gmsGmkGearWork.open_rot_dist < 65536)
                    return;
                obj_work.flag &= 4294967279U;
                AppMain.gmGmkGearSwFwInit(obj_work);
            }
        }
    }

    private static void gmGmkGearSwitchAtkHitFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        if (match_rect.parent_obj.obj_type != (ushort)3)
        {
            AppMain.ObjRectFuncNoHit(mine_rect, match_rect);
        }
        else
        {
            AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)match_rect.parent_obj;
            if (parentObj1.eve_rec.id != (ushort)182)
                AppMain.ObjRectFuncNoHit(mine_rect, match_rect);
            else if (parentObj1.target_obj == null)
            {
                AppMain.ObjRectFuncNoHit(mine_rect, match_rect);
            }
            else
            {
                AppMain.GMS_ENEMY_COM_WORK parentObj2 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
                if ((int)parentObj1.eve_rec.width != (int)parentObj2.eve_rec.width)
                    return;
                if (!AppMain.gmGmkMoveGearCheckSwitchMove(match_rect.parent_obj))
                {
                    AppMain.ObjRectFuncNoHit(mine_rect, match_rect);
                }
                else
                {
                    ((AppMain.GMS_GMK_GEAR_WORK)mine_rect.parent_obj).move_gear_obj = match_rect.parent_obj;
                    AppMain.gmGmkMoveGearSwitchExeInit(match_rect.parent_obj, (short)parentObj2.eve_rec.left, (short)-parentObj2.eve_rec.top);
                    AppMain.gmGmkGearSwRotExtWaitInit(mine_rect.parent_obj);
                }
            }
        }
    }

    private static void gmGmkGearSetRotFlow(
      AppMain.OBS_OBJECT_WORK gear_obj,
      AppMain.GMS_PLAYER_WORK ply_work,
      int move_dir)
    {
        int a1 = ply_work.obj_work.pos.x - gear_obj.pos.x;
        int a2 = ply_work.obj_work.pos.y - gear_obj.pos.y;
        float num1 = AppMain.FXM_FX32_TO_FLOAT(a1);
        float num2 = AppMain.FXM_FX32_TO_FLOAT(a2);
        int num3 = AppMain.nnArcTan2(-(double)num2, (double)num1);
        float num4 = AppMain.nnSqrt((float)((double)num1 * (double)num1 + (double)num2 * (double)num2));
        int ang = num3 + move_dir;
        int fx32 = AppMain.FXM_FLOAT_TO_FX32(AppMain.nnCos(ang) * num4);
        int num5 = -AppMain.FXM_FLOAT_TO_FX32(AppMain.nnSin(ang) * num4);
        int a3 = AppMain.FX_Mul(fx32 - a1, 5120);
        if (AppMain.MTM_MATH_ABS(a3) < 4096)
            a3 = a3 > 0 || gear_obj.spd_m > 0 ? 4096 : -4096;
        if (ply_work.obj_work.ride_obj == gear_obj)
        {
            ply_work.obj_work.flow.x += a3;
            ply_work.obj_work.flow.y += num5 - a2;
        }
        else
        {
            if (ply_work.obj_work.ride_obj != null || ply_work.obj_work.touch_obj != gear_obj)
                return;
            if (move_dir > 0 && ply_work.obj_work.spd_m < (int)short.MinValue && ply_work.obj_work.pos.y > gear_obj.pos.y || move_dir < 0 && ply_work.obj_work.spd_m > 32768 && ply_work.obj_work.pos.y < gear_obj.pos.y)
            {
                ply_work.obj_work.move_flag &= 4294967294U;
            }
            else
            {
                if (((int)ply_work.obj_work.move_flag & 1) == 0 || (move_dir >= 0 || ply_work.obj_work.spd_m < 0 || ply_work.obj_work.pos.x <= gear_obj.pos.x) && (move_dir <= 0 || ply_work.obj_work.spd_m > 0 || ply_work.obj_work.pos.x >= gear_obj.pos.x))
                    return;
                ply_work.obj_work.flow.x += (a3 + gear_obj.move.x) * 2;
                ply_work.obj_work.flow.y += num5 - a2;
                ply_work.obj_work.move_flag &= 4294967294U;
            }
        }
    }

    private static void gmGmkGearMoveSwDraw(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        AppMain.ObjDrawActionSummary(obj_work);
        AppMain.VecFx32 vecFx32;
        vecFx32.x = obj_work.pos.x;
        vecFx32.y = obj_work.pos.y;
        vecFx32.z = obj_work.pos.z + 786432;
        gmsGmkGearWork.obj_3d_gear_opt.user_obj_mtx_r.Assign(obj_work.obj_3d.user_obj_mtx_r);
        AppMain.ObjDrawAction3DNN(gmsGmkGearWork.obj_3d_gear_opt, new AppMain.VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref obj_work.disp_flag);
        vecFx32.z = obj_work.pos.z + 524288;
        gmsGmkGearWork.obj_3d_gear_opt_ashiba.user_obj_mtx_r.Assign(obj_work.obj_3d.user_obj_mtx_r);
        AppMain.ObjDrawAction3DNN(gmsGmkGearWork.obj_3d_gear_opt_ashiba, new AppMain.VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref obj_work.disp_flag);
    }

    private static void gmGmkGearLastFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        if (((int)obj_work.flag & 12) != 0 || gmsGmkGearWork.h_snd_gear == null)
            return;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        float num1 = AppMain.FXM_FX32_TO_FLOAT(gmsGmkGearWork.gmk_work.ene_com.obj_work.pos.x) - obsCamera.disp_pos.x;
        float num2 = AppMain.FXM_FX32_TO_FLOAT(gmsGmkGearWork.gmk_work.ene_com.obj_work.pos.y) - -obsCamera.disp_pos.y;
        float num3;
        if ((double)num1 < 400.0 && (double)num2 < 400.0)
        {
            float num4 = (float)((double)num1 * (double)num1 + (double)num2 * (double)num2);
            if ((double)num4 <= 10000.0)
                num3 = 1f;
            else if ((double)num4 <= 160000.0)
            {
                num3 = (float)((160000.0 - (double)num4) / 90000.0);
                if ((double)num3 > 1.0)
                    num3 = 1f;
                else if ((double)num3 < 0.0)
                    num3 = 0.0f;
            }
            else
                num3 = 0.0f;
        }
        else
            num3 = 0.0f;
        gmsGmkGearWork.h_snd_gear.snd_ctrl_param.volume = num3;
    }
}