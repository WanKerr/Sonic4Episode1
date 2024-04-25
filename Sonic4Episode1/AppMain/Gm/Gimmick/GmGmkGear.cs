using System;

public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkGearInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        VecFx32 vecFx32 = new VecFx32();
        OBS_OBJECT_WORK obsObjectWork = eve_rec.id != 184 ? GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_GEAR_WORK(), "GMK_GEAR") : GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_GEAR_WORK(), "GMK_GEAR_SW");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obsObjectWork;
        GMS_GMK_GEAR_WORK gear_work = (GMS_GMK_GEAR_WORK)obsObjectWork;
        if (eve_rec.id == 182 && eve_rec.byte_param[1] != 0)
        {
            obsObjectWork.disp_flag |= 32U;
            obsObjectWork.move_flag |= 8448U;
            obsObjectWork.flag |= 2U;
            return obsObjectWork;
        }
        obsObjectWork.ppIn = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearInFunc);
        mtTaskChangeTcbDestructor(obsObjectWork.tcb, new GSF_TASK_PROCEDURE(gmGmkGearDest));
        gear_work.h_snd_gear = GsSoundAllocSeHandle();
        GmSoundStopSE(gear_work.h_snd_gear);
        GmSoundPlaySE("Gear", gear_work.h_snd_gear);
        if (eve_rec.id == 181)
        {
            float num = 0.2f;
            ObjObjectCopyAction3dNNModel(obsObjectWork, gm_gmk_gear_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            obsObjectWork.pos.z = -131072;
            obsObjectWork.ppLast = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearLastFunc);
            float val = num + 0.2f * MTM_MATH_CLIP(eve_rec.left, 0, 3);
            gear_work.h_snd_gear.au_player.SetAisac("Speed", val);
        }
        else if (eve_rec.id == 182)
        {
            ObjObjectCopyAction3dNNModel(obsObjectWork, gm_gmk_gear_move_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            obsObjectWork.pos.z = -131072;
            ObjCopyAction3dNNModel(gm_gmk_gear_opt_obj_3d_list[0], gear_work.obj_3d_gear_opt);
            ObjCopyAction3dNNModel(gm_gmk_gear_opt_obj_3d_list[1], gear_work.obj_3d_gear_opt_ashiba);
            obsObjectWork.ppLast = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearMoveLastFunc);
        }
        else
        {
            ObjObjectCopyAction3dNNModel(obsObjectWork, gm_gmk_gear_sw_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            obsObjectWork.pos.z = -131072;
            ObjCopyAction3dNNModel(gm_gmk_gear_opt_obj_3d_list[0], gear_work.obj_3d_gear_opt);
            ObjCopyAction3dNNModel(gm_gmk_gear_opt_obj_3d_list[1], gear_work.obj_3d_gear_opt_ashiba);
            obsObjectWork.ppLast = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearMoveLastFunc);
        }
        OBS_COLLISION_WORK colWork = gear_work.gmk_work.ene_com.col_work;
        colWork.obj_col.obj = (OBS_OBJECT_WORK)gear_work;
        colWork.obj_col.width = 176;
        colWork.obj_col.height = 176;
        colWork.obj_col.ofst_x = -88;
        colWork.obj_col.ofst_y = -88;
        colWork.obj_col.flag |= 402653216U;
        obsObjectWork.move_flag &= 4294967167U;
        obsObjectWork.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        gmsEnemy3DWork.obj_3d.use_light_flag &= 4294967294U;
        gmsEnemy3DWork.obj_3d.use_light_flag |= 2U;
        gear_work.obj_3d_gear_opt_ashiba.use_light_flag &= 4294967294U;
        gear_work.obj_3d_gear_opt_ashiba.use_light_flag |= 2U;
        if (g_gs_main_sys_info.stage_id == 14)
        {
            gear_work.obj_3d_gear_opt.use_light_flag &= 4294967294U;
            gear_work.obj_3d_gear_opt.use_light_flag |= 2U;
        }
        if (eve_rec.id == 181)
        {
            obsObjectWork.move_flag |= 8192U;
            obsObjectWork.move_flag |= 256U;
            gear_work.dir_speed = (float)(32.0 + MTM_MATH_CLIP(eve_rec.left, 0, 3) * 32.0);
            if ((eve_rec.flag & 1) != 0)
                gear_work.dir_speed = -gear_work.dir_speed;
            int num = (int)(65536.0 / MTM_MATH_ABS(gear_work.dir_speed));
            gear_work.dir_temp = g_gm_main_system.sync_time % num * gear_work.dir_speed;
            gear_work.dir_temp += MTM_MATH_CLIP(eve_rec.top, 0, 3) * 2048;
            float a = gear_work.dir_temp / 65536f;
            if (MTM_MATH_ABS(a) >= 1.0)
                gear_work.dir_temp -= a * 65536f;
            obsObjectWork.dir.z = gear_work.dir_speed <= 0.0 ? (ushort)((uint)(nnRoundOff(gear_work.dir_temp) + 1023.0) / 1024U * 1024U) : (ushort)((uint)nnRoundOff(gear_work.dir_temp) / 1024U * 1024U);
            gear_work.prev_dir = obsObjectWork.dir.z;
            gmGmkGearChangeCol(gear_work);
            gmGmkGearFwInit(obsObjectWork);
        }
        else if (eve_rec.id == 182)
        {
            obsObjectWork.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearMoveSwDraw);
            byte[] numArray1;
            if (gm_gmk_gear_add_data[16] is AmbChunk)
            {
                AmbChunk ambChunk = (AmbChunk)gm_gmk_gear_add_data[16];
                numArray1 = new byte[ambChunk.length];
                Buffer.BlockCopy(ambChunk.array, ambChunk.offset, numArray1, 0, ambChunk.length);
                gm_gmk_gear_add_data[16] = numArray1;
            }
            else
                numArray1 = (byte[])gm_gmk_gear_add_data[16];
            colWork.obj_col.diff_data = numArray1;
            byte[] numArray2;
            if (gm_gmk_gear_add_data[17] is AmbChunk)
            {
                AmbChunk ambChunk = (AmbChunk)gm_gmk_gear_add_data[17];
                numArray2 = new byte[ambChunk.length];
                Buffer.BlockCopy(ambChunk.array, ambChunk.offset, numArray2, 0, ambChunk.length);
                gm_gmk_gear_add_data[17] = numArray2;
            }
            else
                numArray2 = (byte[])gm_gmk_gear_add_data[17];
            colWork.obj_col.dir_data = numArray2;
            obsObjectWork.flag &= 4294967294U;
            obsObjectWork.disp_flag |= 16777472U;
            obsObjectWork.move_flag &= 4294967039U;
            obsObjectWork.move_flag |= 704U;
            if ((eve_rec.flag & 8) != 0)
                obsObjectWork.user_flag |= 4U;
            OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[2];
            pRec1.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkMoveGearBodyDefFunc);
            pRec1.ppHit = null;
            ObjRectWorkSet(pRec1, -16, -72, 16, -48);
            OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
            pRec2.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkMoveGearDefFunc);
            pRec2.ppHit = null;
            ObjRectAtkSet(pRec2, 0, 0);
            ObjRectDefSet(pRec2, 65527, 0);
            ObjRectWorkSet(pRec2, -80, -80, 80, 80);
            pRec2.flag |= 32U;
            ObjObjectFieldRectSet(obsObjectWork, -8, -8, 8, 8);
            if (type == 0)
            {
                obsObjectWork.move_flag |= 1U;
                obsObjectWork.prev_pos.Assign(obsObjectWork.pos);
                if ((eve_rec.flag & 1) != 0)
                {
                    obsObjectWork.dir.z = 16384;
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
                g_obj.ppCollision(obsObjectWork);
                obsObjectWork.move_flag |= 1U;
                vecFx32.Assign(obsObjectWork.pos);
                obsObjectWork.prev_pos.Assign(obsObjectWork.pos);
                obsObjectWork.move.x = obsObjectWork.move.y = 0;
                obsObjectWork.spd.x = obsObjectWork.spd.y = 0;
                if ((eve_rec.flag & 1) != 0)
                {
                    if ((eve_rec.flag & 2) != 0)
                    {
                        obsObjectWork.prev_pos.y -= 32768;
                        obsObjectWork.move.y = 32768;
                        obsObjectWork.spd.y = 32768;
                        obsObjectWork.disp_flag |= 1U;
                    }
                    else
                    {
                        obsObjectWork.prev_pos.y += 32768;
                        obsObjectWork.move.y = short.MinValue;
                        obsObjectWork.spd.y = short.MinValue;
                    }
                }
                else if ((eve_rec.flag & 2) != 0)
                {
                    obsObjectWork.prev_pos.x -= 32768;
                    obsObjectWork.move.x = 32768;
                    obsObjectWork.spd.x = 32768;
                    obsObjectWork.disp_flag |= 1U;
                }
                else
                {
                    obsObjectWork.prev_pos.x += 32768;
                    obsObjectWork.move.x = short.MinValue;
                    obsObjectWork.spd.x = short.MinValue;
                }
                g_obj.ppCollision(obsObjectWork);
                if (((int)obsObjectWork.move_flag & 8) != 0)
                {
                    if ((eve_rec.flag & 1) != 0)
                        obsObjectWork.prev_pos.y = vecFx32.y;
                    else
                        obsObjectWork.prev_pos.x = vecFx32.x;
                }
                else
                    obsObjectWork.pos.Assign(vecFx32);
                gear_work.move_end_x = (eve_rec.flag & 2) == 0 ? (gear_work.move_end_y = int.MaxValue) : (gear_work.move_end_y = 0);
                obsObjectWork.prev_pos.Assign(obsObjectWork.pos);
                obsObjectWork.move.x = obsObjectWork.move.y = 0;
                obsObjectWork.spd.x = obsObjectWork.spd.y = 0;
                obsObjectWork.disp_flag &= 4294967294U;
                obsObjectWork.move_flag |= 1U;
                gmGmkMoveGearFwInit(obsObjectWork);
            }
            else
            {
                nnMakeRotateZMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, 2730);
                gmGmkMoveGearEndInit(obsObjectWork);
            }
        }
        else
        {
            obsObjectWork.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearMoveSwDraw);
            byte[] numArray1;
            if (gm_gmk_gear_add_data[16] is AmbChunk)
            {
                AmbChunk ambChunk = (AmbChunk)gm_gmk_gear_add_data[16];
                numArray1 = new byte[ambChunk.length];
                Buffer.BlockCopy(ambChunk.array, ambChunk.offset, numArray1, 0, ambChunk.length);
                gm_gmk_gear_add_data[16] = numArray1;
            }
            else
                numArray1 = (byte[])gm_gmk_gear_add_data[16];
            colWork.obj_col.diff_data = numArray1;
            byte[] numArray2;
            if (gm_gmk_gear_add_data[17] is AmbChunk)
            {
                AmbChunk ambChunk = (AmbChunk)gm_gmk_gear_add_data[17];
                numArray2 = new byte[ambChunk.length];
                Buffer.BlockCopy(ambChunk.array, ambChunk.offset, numArray2, 0, ambChunk.length);
                gm_gmk_gear_add_data[17] = numArray2;
            }
            else
                numArray2 = (byte[])gm_gmk_gear_add_data[17];
            colWork.obj_col.dir_data = numArray2;
            OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[1];
            ObjRectGroupSet(pRec, 0, 2);
            pRec.ppDef = null;
            pRec.ppHit = new OBS_RECT_WORK_Delegate1(gmGmkGearSwitchAtkHitFunc);
            ObjRectAtkSet(pRec, 8, 1);
            ObjRectDefSet(pRec, ushort.MaxValue, 0);
            ObjRectWorkSet(pRec, -92, -92, 92, 92);
            pRec.flag |= 1024U;
            obsObjectWork.disp_flag |= 16777472U;
            obsObjectWork.move_flag |= 8448U;
            gear_work.open_rot_dist = 65536;
            if (eve_rec.height > 0)
                gear_work.close_rot_spd = 65536 / (eve_rec.height * 30);
            gmGmkGearSwFwInit(obsObjectWork);
        }
        return obsObjectWork;
    }

    private static OBS_OBJECT_WORK GmGmkGearMoveEndInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK rideWork = GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_GEAR_WORK(), "GMK_GEAR_END");
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)rideWork;
        if (eve_rec.byte_param[1] != 0)
        {
            OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(182, pos_x, pos_y, 0, 0, 0, 0, 0, 1);
            rideWork.parent_obj = obsObjectWork;
            rideWork.flag |= 18U;
        }
        else
        {
            rideWork.ppIn = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearInFunc);
            OBS_RECT_WORK pRec = gmsEnemyComWork.rect_work[1];
            ObjRectGroupSet(pRec, 0, 2);
            pRec.ppDef = null;
            pRec.ppHit = new OBS_RECT_WORK_Delegate1(gmGmkMoveGearEndAtkHitFunc);
            ObjRectAtkSet(pRec, 8, 1);
            ObjRectDefSet(pRec, ushort.MaxValue, 0);
            ObjRectWorkSet(pRec, -32, -32, 32, 32);
            pRec.flag |= 1024U;
            gmGmkMoveGearEndSwitchFwInit(rideWork);
        }
        rideWork.move_flag |= 8448U;
        rideWork.disp_flag |= 32U;
        return rideWork;
    }

    public static void GmGmkGearSetLight()
    {
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.x = -0.35f;
        nnsVector.y = 2.25f;
        nnsVector.z = -0.9f;
        nnNormalizeVector(nnsVector, nnsVector);
        float intensity = g_gs_main_sys_info.stage_id != 14 ? 1f : 0.9f;
        ObjDrawSetParallelLight(NNE_LIGHT_1, ref col, intensity, nnsVector);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    public static void GmGmkGearBuild()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(887);
        for (int index = 0; index < 19; ++index)
            gm_gmk_gear_add_data[index] = (AmbChunk)amBindGet(gimmickData, index);
        gm_gmk_gear_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(888), GmGameDatGetGimmickData(889), 0U);
        gm_gmk_gear_move_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(890), GmGameDatGetGimmickData(889), 0U);
        gm_gmk_gear_sw_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(890), GmGameDatGetGimmickData(889), 0U, readTXBfile(gm_gmk_gear_add_data[18]));
        gm_gmk_gear_opt_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(891), GmGameDatGetGimmickData(889), 0U);
    }

    public static void GmGmkGearFlush()
    {
        AMS_AMB_HEADER gimmickData1 = GmGameDatGetGimmickData(888);
        GmGameDBuildRegFlushModel(gm_gmk_gear_obj_3d_list, gimmickData1.file_num);
        AMS_AMB_HEADER gimmickData2 = GmGameDatGetGimmickData(890);
        GmGameDBuildRegFlushModel(gm_gmk_gear_move_obj_3d_list, gimmickData2.file_num);
        GmGameDBuildRegFlushModel(gm_gmk_gear_sw_obj_3d_list, gimmickData2.file_num);
        AMS_AMB_HEADER gimmickData3 = GmGameDatGetGimmickData(891);
        GmGameDBuildRegFlushModel(gm_gmk_gear_opt_obj_3d_list, gimmickData3.file_num);
        for (int index = 0; index < gm_gmk_gear_add_data.Length; ++index)
            gm_gmk_gear_add_data[index] = null;
    }

    private static void gmGmkGearInFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        GmEnemyDefaultInFunc(obj_work);
        if (gmsGmkGearWork.gear_end_obj != null && ((int)gmsGmkGearWork.gear_end_obj.flag & 12) != 0)
            gmsGmkGearWork.gear_end_obj = null;
        if (gmsGmkGearWork.move_gear_obj != null && ((int)gmsGmkGearWork.move_gear_obj.flag & 12) != 0)
            gmsGmkGearWork.move_gear_obj = null;
        if (gmsGmkGearWork.sw_gear_obj == null || ((int)gmsGmkGearWork.sw_gear_obj.flag & 12) == 0)
            return;
        gmsGmkGearWork.sw_gear_obj = null;
    }

    private static void gmGmkGearDest(MTS_TASK_TCB tcb)
    {
        GMS_GMK_GEAR_WORK tcbWork = (GMS_GMK_GEAR_WORK)mtTaskGetTcbWork(tcb);
        if (tcbWork.h_snd_gear != null)
        {
            GmSoundStopSE(tcbWork.h_snd_gear);
            GsSoundFreeSeHandle(tcbWork.h_snd_gear);
            tcbWork.h_snd_gear = null;
        }
        GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkGearFwInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearFwMain);
    }

    private static void gmGmkGearFwMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gear_work = (GMS_GMK_GEAR_WORK)obj_work;
        gear_work.dir_temp += gear_work.dir_speed;
        if (gear_work.dir_temp < -65536.0)
            gear_work.dir_temp += 65536f;
        else if (gear_work.dir_temp > 65536.0)
            gear_work.dir_temp -= 65536f;
        gear_work.prev_dir = obj_work.dir.z;
        obj_work.dir.z = gear_work.dir_speed <= 0.0 ? (ushort)(((int)nnRoundOff(gear_work.dir_temp) + 1023) / 1024 * 1024) : (ushort)((int)nnRoundOff(gear_work.dir_temp) / 1024 * 1024);
        gear_work.col_type = (uint)(obj_work.dir.z / 1024 % 8);
        gmGmkGearChangeCol(gear_work);
        if (gear_work.prev_dir == obj_work.dir.z || obj_work.col_work.obj_col.rider_obj == null || obj_work.col_work.obj_col.rider_obj.obj_type != 1)
            return;
        OBS_OBJECT_WORK riderObj = obj_work.col_work.obj_col.rider_obj;
        float num1 = FXM_FX32_TO_FLOAT(riderObj.pos.x - obj_work.pos.x);
        float num2 = -FXM_FX32_TO_FLOAT(riderObj.pos.y - obj_work.pos.y);
        int ang = obj_work.dir.z - gear_work.prev_dir;
        float num3 = (float)(nnCos(ang) * (double)num1 + nnSin(ang) * (double)num2);
        float num4 = (float)(-nnSin(ang) * (double)num1 + nnCos(ang) * (double)num2);
        riderObj.prev_pos.x = riderObj.pos.x;
        riderObj.prev_pos.y = riderObj.pos.y;
        riderObj.pos.x += FXM_FLOAT_TO_FX32(num3 - num1);
        riderObj.pos.y += -FXM_FLOAT_TO_FX32(num4 - num2);
        riderObj.move.x = riderObj.pos.x - riderObj.prev_pos.x;
        riderObj.move.y = riderObj.pos.y - riderObj.prev_pos.y;
    }

    private static void gmGmkGearChangeCol(GMS_GMK_GEAR_WORK gear_work)
    {
        byte[] numArray;
        if (gm_gmk_gear_add_data[(int)(gear_work.col_type * 2U)] is AmbChunk)
        {
            AmbChunk ambChunk = (AmbChunk)gm_gmk_gear_add_data[(int)(gear_work.col_type * 2U)];
            numArray = new byte[ambChunk.length];
            Buffer.BlockCopy(ambChunk.array, ambChunk.offset, numArray, 0, ambChunk.length);
            gm_gmk_gear_add_data[(int)(gear_work.col_type * 2U)] = numArray;
        }
        else
            numArray = (byte[])gm_gmk_gear_add_data[(int)(gear_work.col_type * 2U)];
        gear_work.gmk_work.ene_com.col_work.obj_col.diff_data = numArray;
    }
    private static void gmGmkGearSwFwInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        gmsGmkGearWork.open_rot_dist = 65536;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearSwFwMain);
        gmsGmkGearWork.move_gear_obj = null;
    }

    private static void gmGmkGearSwFwMain(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkGearSwRotExtWaitInit(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.ppFunc == new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearSwFwMain))
            GmSoundPlaySE("Gear2", null);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearSwRotExtWaitMain);
    }

    private static void gmGmkGearSwRotExtWaitMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.move_gear_obj == null)
        {
            gmGmkGearSwFwInit(obj_work);
        }
        else
        {
            GMS_GMK_GEAR_WORK moveGearObj = (GMS_GMK_GEAR_WORK)gmsGmkGearWork.move_gear_obj;
            int num1 = (ushort)((gmsGmkGearWork.move_draw_dir - 5461) / 5461 * 2);
            int num2 = (ushort)((moveGearObj.move_draw_dir - 2730) / 5461 * 2);
            if (num1 == num2)
                return;
            gmsGmkGearWork.move_draw_dir = (ushort)-(moveGearObj.move_draw_dir - (2730 - num2 * 5461) / 2);
            gmGmkGearSwRotExtInit(obj_work);
        }
    }

    private static void gmGmkGearSwRotExtInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGearSwRotExtMain);
    }

    private static void gmGmkGearSwRotExtMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.move_gear_obj == null)
        {
            obj_work.flag &= 4294967279U;
            gmGmkGearSwFwInit(obj_work);
        }
        else
        {
            GMS_GMK_GEAR_WORK moveGearObj = (GMS_GMK_GEAR_WORK)gmsGmkGearWork.move_gear_obj;
            int num = moveGearObj.move_draw_dir_spd;
            gmsGmkGearWork.move_draw_dir -= (ushort)num;
            gmsGmkGearWork.move_draw_dir_ofst = (short)-moveGearObj.move_draw_dir_ofst;
            nnMakeRotateZMatrix(obj_work.obj_3d.user_obj_mtx_r, gmsGmkGearWork.move_draw_dir * 2 + gmsGmkGearWork.move_draw_dir_ofst);
            if (gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.rider_obj != null && gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.rider_obj.obj_type == 1)
                gmGmkGearSetRotFlow(obj_work, (GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.rider_obj, -num * 2);
            else if (gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.toucher_obj != null && gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.toucher_obj.obj_type == 1)
                gmGmkGearSetRotFlow(obj_work, (GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.col_work.obj_col.toucher_obj, -num * 2);
            if ((moveGearObj.gmk_work.ene_com.eve_rec.flag & 1) != 0)
            {
                if ((moveGearObj.gmk_work.ene_com.eve_rec.flag & 2) == 0)
                    num = -num;
            }
            else if ((moveGearObj.gmk_work.ene_com.eve_rec.flag & 2) != 0)
                num = -num;
            gmsGmkGearWork.open_rot_dist += num;
            int numer = MTM_MATH_CLIP(gmsGmkGearWork.open_rot_dist, 0, 65536);
            if (num <= 0)
                GmGmkSwitchSetOnGearSwitch(gmsGmkGearWork.gmk_work.ene_com.eve_rec.width, FX_Div(numer, 65536));
            else
                GmGmkSwitchSetOffGearSwitch(gmsGmkGearWork.gmk_work.ene_com.eve_rec.width, FX_Div(numer, 65536));
            if (gmsGmkGearWork.open_rot_dist <= 0)
            {
                if (gmsGmkGearWork.gmk_work.ene_com.eve_rec.height == 0)
                {
                    gmsGmkGearWork.gmk_work.ene_com.eve_rec.byte_param[1] = 1;
                    obj_work.ppFunc = null;
                    gmsGmkGearWork.move_gear_obj = null;
                }
                else
                    obj_work.flag |= 16U;
            }
            else
            {
                if (gmsGmkGearWork.open_rot_dist < 65536)
                    return;
                obj_work.flag &= 4294967279U;
                gmGmkGearSwFwInit(obj_work);
            }
        }
    }

    private static void gmGmkGearSwitchAtkHitFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        if (match_rect.parent_obj.obj_type != 3)
        {
            ObjRectFuncNoHit(mine_rect, match_rect);
        }
        else
        {
            GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)match_rect.parent_obj;
            if (parentObj1.eve_rec.id != 182)
                ObjRectFuncNoHit(mine_rect, match_rect);
            else if (parentObj1.target_obj == null)
            {
                ObjRectFuncNoHit(mine_rect, match_rect);
            }
            else
            {
                GMS_ENEMY_COM_WORK parentObj2 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
                if (parentObj1.eve_rec.width != parentObj2.eve_rec.width)
                    return;
                if (!gmGmkMoveGearCheckSwitchMove(match_rect.parent_obj))
                {
                    ObjRectFuncNoHit(mine_rect, match_rect);
                }
                else
                {
                    ((GMS_GMK_GEAR_WORK)mine_rect.parent_obj).move_gear_obj = match_rect.parent_obj;
                    gmGmkMoveGearSwitchExeInit(match_rect.parent_obj, parentObj2.eve_rec.left, (short)-parentObj2.eve_rec.top);
                    gmGmkGearSwRotExtWaitInit(mine_rect.parent_obj);
                }
            }
        }
    }

    private static void gmGmkGearSetRotFlow(
      OBS_OBJECT_WORK gear_obj,
      GMS_PLAYER_WORK ply_work,
      int move_dir)
    {
        int a1 = ply_work.obj_work.pos.x - gear_obj.pos.x;
        int a2 = ply_work.obj_work.pos.y - gear_obj.pos.y;
        float num1 = FXM_FX32_TO_FLOAT(a1);
        float num2 = FXM_FX32_TO_FLOAT(a2);
        int num3 = nnArcTan2(-num2, num1);
        float num4 = nnSqrt((float)(num1 * (double)num1 + num2 * (double)num2));
        int ang = num3 + move_dir;
        int fx32 = FXM_FLOAT_TO_FX32(nnCos(ang) * num4);
        int num5 = -FXM_FLOAT_TO_FX32(nnSin(ang) * num4);
        int a3 = FX_Mul(fx32 - a1, 5120);
        if (MTM_MATH_ABS(a3) < 4096)
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
            if (move_dir > 0 && ply_work.obj_work.spd_m < short.MinValue && ply_work.obj_work.pos.y > gear_obj.pos.y || move_dir < 0 && ply_work.obj_work.spd_m > 32768 && ply_work.obj_work.pos.y < gear_obj.pos.y)
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

    private static void gmGmkGearMoveSwDraw(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        ObjDrawActionSummary(obj_work);
        VecFx32 vecFx32;
        vecFx32.x = obj_work.pos.x;
        vecFx32.y = obj_work.pos.y;
        vecFx32.z = obj_work.pos.z + 786432;
        gmsGmkGearWork.obj_3d_gear_opt.user_obj_mtx_r.Assign(obj_work.obj_3d.user_obj_mtx_r);
        ObjDrawAction3DNN(gmsGmkGearWork.obj_3d_gear_opt, new VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref obj_work.disp_flag);
        vecFx32.z = obj_work.pos.z + 524288;
        gmsGmkGearWork.obj_3d_gear_opt_ashiba.user_obj_mtx_r.Assign(obj_work.obj_3d.user_obj_mtx_r);
        ObjDrawAction3DNN(gmsGmkGearWork.obj_3d_gear_opt_ashiba, new VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref obj_work.disp_flag);
    }

    private static void gmGmkGearLastFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if (((int)obj_work.flag & 12) != 0 || gmsGmkGearWork.h_snd_gear == null)
            return;
        OBS_CAMERA obsCamera = ObjCameraGet(g_obj.glb_camera_id);
        float num1 = FXM_FX32_TO_FLOAT(gmsGmkGearWork.gmk_work.ene_com.obj_work.pos.x) - obsCamera.disp_pos.x;
        float num2 = FXM_FX32_TO_FLOAT(gmsGmkGearWork.gmk_work.ene_com.obj_work.pos.y) - -obsCamera.disp_pos.y;
        float num3;
        if (num1 < 400.0 && num2 < 400.0)
        {
            float num4 = (float)(num1 * (double)num1 + num2 * (double)num2);
            if (num4 <= 10000.0)
                num3 = 1f;
            else if (num4 <= 160000.0)
            {
                num3 = (float)((160000.0 - num4) / 90000.0);
                if (num3 > 1.0)
                    num3 = 1f;
                else if (num3 < 0.0)
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