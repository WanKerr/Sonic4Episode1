public partial class AppMain
{
    public static void GmGmkSwWallBuild()
    {
        gm_gmk_sw_wall_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(934)), readAMBFile(GmGameDatGetGimmickData(935)), 0U);
    }

    public static void GmGmkSwWallFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(934));
        GmGameDBuildRegFlushModel(gm_gmk_sw_wall_obj_3d_list, amsAmbHeader.file_num);
    }

    public static OBS_OBJECT_WORK GmGmkSwWallInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_SWWALL_WORK(), "GMK_SW_WALL");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (GMS_GMK_SWWALL_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_sw_wall_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        if (256 <= eve_rec.id && eve_rec.id <= 259)
        {
            ObjCopyAction3dNNModel(gm_gmk_sw_wall_obj_3d_list[1], gmsGmkSwwallWork.obj_3d_opt[0]);
            ObjCopyAction3dNNModel(gm_gmk_sw_wall_obj_3d_list[2], gmsGmkSwwallWork.obj_3d_opt[1]);
            gmsGmkSwwallWork.obj_3d_opt[1].drawflag |= 32U;
            gmsEnemy3DWork.ene_com.enemy_flag |= 1U;
        }
        else
            gmsGmkSwwallWork.h_snd = GsSoundAllocSeHandle();
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmGmkSwWallDest));
        work.pos.z = -655360;
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSwWallDispFunc);
        gmsGmkSwwallWork.id = (uint)MTM_MATH_CLIP(eve_rec.left, 0, 64);
        ushort wall_type;
        ushort num;
        if (248 <= eve_rec.id && eve_rec.id <= 251)
        {
            gmsGmkSwwallWork.wall_size = 524288;
            wall_type = (ushort)(eve_rec.id - 248U);
            num = 32;
        }
        else if (252 <= eve_rec.id && eve_rec.id <= byte.MaxValue)
        {
            gmsGmkSwwallWork.wall_size = 1048576;
            wall_type = (ushort)(eve_rec.id - 252U);
            num = 32;
        }
        else
        {
            gmsGmkSwwallWork.wall_size = 393216;
            wall_type = (ushort)(eve_rec.id - 256U);
            num = 24;
            gmsGmkSwwallWork.gear_pos.Assign(work.pos);
            gmsGmkSwwallWork.gearbase_pos.Assign(work.pos);
            switch (eve_rec.id)
            {
                case 257:
                    gmsGmkSwwallWork.gear_pos.x -= -144179;
                    gmsGmkSwwallWork.gear_pos.y += 68812;
                    gmsGmkSwwallWork.gearbase_pos.x -= -196608;
                    gmsGmkSwwallWork.gearbase_pos.y += 49152;
                    gmsGmkSwwallWork.gear_base_dir = 2730;
                    gmsEnemy3DWork.ene_com.enemy_flag |= 4U;
                    break;
                case 258:
                    gmsGmkSwwallWork.gear_pos.x += 68812;
                    gmsGmkSwwallWork.gear_pos.y += -144179;
                    gmsGmkSwwallWork.gearbase_pos.x += 49152;
                    gmsGmkSwwallWork.gearbase_pos.y += -196608;
                    gmsGmkSwwallWork.gear_base_dir = 8192;
                    gmsEnemy3DWork.ene_com.enemy_flag |= 4U;
                    work.dir.z = 49152;
                    break;
                case 259:
                    gmsGmkSwwallWork.gear_pos.x += 68812;
                    gmsGmkSwwallWork.gear_pos.y -= -144179;
                    gmsGmkSwwallWork.gearbase_pos.x += 49152;
                    gmsGmkSwwallWork.gearbase_pos.y -= -196608;
                    gmsGmkSwwallWork.gear_base_dir = 2730;
                    work.dir.z = 49152;
                    break;
                default:
                    gmsGmkSwwallWork.gear_pos.x += -144179;
                    gmsGmkSwwallWork.gear_pos.y += 68812;
                    gmsGmkSwwallWork.gearbase_pos.x += -196608;
                    gmsGmkSwwallWork.gearbase_pos.y += 49152;
                    gmsGmkSwwallWork.gear_base_dir = 62806;
                    break;
            }
        }
        gmsGmkSwwallWork.wall_type = wall_type;
        if (GmGmkSwitchTypeIsGear(gmsGmkSwwallWork.id) && (GmGmkSwitchGetPer(gmsGmkSwwallWork.id) != 0 || !GmGmkSwitchIsOn(gmsGmkSwwallWork.id)) && (GmGmkSwitchGetPer(gmsGmkSwwallWork.id) != 4096 || GmGmkSwitchIsOn(gmsGmkSwwallWork.id)))
        {
            int v2 = GmGmkSwitchGetPer(gmsGmkSwwallWork.id);
            if (GmGmkSwitchIsOn(gmsGmkSwwallWork.id))
            {
                if ((eve_rec.flag & 1) != 0)
                    v2 = 4096 - v2;
            }
            else if ((eve_rec.flag & 1) == 0)
                v2 = 4096 - v2;
            gmsGmkSwwallWork.wall_draw_size = FX_Mul(gmsGmkSwwallWork.wall_size, v2);
        }
        else if (GmGmkSwitchIsOn(gmsGmkSwwallWork.id) && (eve_rec.flag & 1) != 0 || !GmGmkSwitchIsOn(gmsGmkSwwallWork.id) && (eve_rec.flag & 1) == 0)
            gmsGmkSwwallWork.wall_draw_size = gmsGmkSwwallWork.wall_size;
        OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work;
        colWork.obj_col.dir_data = gmsGmkSwwallWork.col_dir_buf;
        colWork.obj_col.diff_data = g_gm_default_col;
        colWork.obj_col.flag |= 402653216U;
        if (wall_type <= 1)
        {
            colWork.obj_col.height = num;
            colWork.obj_col.ofst_y = -16;
        }
        else
        {
            colWork.obj_col.width = num;
            colWork.obj_col.ofst_x = -16;
        }
        gmGmkSwWallSetCol(colWork, gmsGmkSwwallWork.wall_size, gmsGmkSwwallWork.wall_draw_size, wall_type);
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        if (gmsGmkSwwallWork.wall_draw_size == 0 || gmsGmkSwwallWork.wall_draw_size == gmsGmkSwwallWork.wall_size)
            gmGmkSwWallFwInit(work);
        else if (GmGmkSwitchIsOn(gmsGmkSwwallWork.id) && (eve_rec.flag & 1) != 0 || !GmGmkSwitchIsOn(gmsGmkSwwallWork.id) && (eve_rec.flag & 1) == 0)
            gmGmkSwWallCloseInit(work);
        else
            gmGmkSwWallOpenInit(work);
        return work;
    }

    public static void gmGmkSwWallDest(MTS_TASK_TCB tcb)
    {
        GMS_GMK_SWWALL_WORK tcbWork = (GMS_GMK_SWWALL_WORK)mtTaskGetTcbWork(tcb);
        if (((int)tcbWork.gmk_work.ene_com.enemy_flag & 2) != 0)
            ObjAction3dNNMotionRelease(tcbWork.obj_3d_opt[1]);
        if (tcbWork.h_snd != null)
        {
            GmSoundStopSE(tcbWork.h_snd);
            GsSoundFreeSeHandle(tcbWork.h_snd);
            tcbWork.h_snd = null;
        }
        GmEnemyDefaultExit(tcb);
    }

    public static void gmGmkSwWallFwInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (GMS_GMK_SWWALL_WORK)obj_work;
        obj_work.flag &= 4294967279U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSwWallFwMain);
        obj_work.col_work.obj_col.obj = obj_work;
        if (gmsGmkSwwallWork.h_snd == null)
            return;
        GmSoundStopSE(gmsGmkSwwallWork.h_snd);
    }

    public static void gmGmkSwWallFwMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SWWALL_WORK swwall_work = (GMS_GMK_SWWALL_WORK)obj_work;
        GMS_EVE_RECORD_EVENT eveRec = swwall_work.gmk_work.ene_com.eve_rec;
        if (swwall_work.wall_draw_size != 0)
        {
            if ((eveRec.flag & 1) == 0 && GmGmkSwitchIsOn(swwall_work.id) || (eveRec.flag & 1) != 0 && !GmGmkSwitchIsOn(swwall_work.id))
                gmGmkSwWallOpenInit(obj_work);
        }
        else if ((eveRec.flag & 1) != 0 && GmGmkSwitchIsOn(swwall_work.id) || (eveRec.flag & 1) == 0 && !GmGmkSwitchIsOn(swwall_work.id))
            gmGmkSwWallCloseInit(obj_work);
        gmGmkSwWallCheckColOff(swwall_work);
    }

    public static void gmGmkSwWallOpenInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (GMS_GMK_SWWALL_WORK)obj_work;
        gmsGmkSwwallWork.wall_spd = !GmGmkSwitchTypeIsGear(gmsGmkSwwallWork.id) ? 16384 : 0;
        obj_work.flag |= 16U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSwWallOpenMain);
        obj_work.col_work.obj_col.obj = obj_work;
        if (gmsGmkSwwallWork.h_snd == null)
            return;
        GmSoundStopSE(gmsGmkSwwallWork.h_snd);
        GmSoundPlaySE("Boss3_01", gmsGmkSwwallWork.h_snd);
    }

    public static void gmGmkSwWallOpenMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (GMS_GMK_SWWALL_WORK)obj_work;
        if ((gmsGmkSwwallWork.gmk_work.ene_com.eve_rec.flag & 1) != 0 && GmGmkSwitchIsOn(gmsGmkSwwallWork.id) || (gmsGmkSwwallWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 && !GmGmkSwitchIsOn(gmsGmkSwwallWork.id))
            gmGmkSwWallCloseInit(obj_work);
        if (gmsGmkSwwallWork.wall_spd != 0)
        {
            gmsGmkSwwallWork.wall_draw_size -= gmsGmkSwwallWork.wall_spd;
        }
        else
        {
            int v2 = GmGmkSwitchGetPer(gmsGmkSwwallWork.id);
            if ((gmsGmkSwwallWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
                v2 = 4096 - v2;
            gmsGmkSwwallWork.wall_draw_size = FX_Mul(gmsGmkSwwallWork.wall_size, v2);
        }
        if (gmsGmkSwwallWork.wall_draw_size <= 0)
        {
            gmsGmkSwwallWork.wall_draw_size = 0;
            gmGmkSwWallFwInit(obj_work);
        }
        gmsGmkSwwallWork.gear_dir = (ushort)(gmsGmkSwwallWork.wall_draw_size / 64 * 16);
        if (gmsGmkSwwallWork.gmk_work.ene_com.eve_rec.id == 257 || gmsGmkSwwallWork.gmk_work.ene_com.eve_rec.id == 258)
            gmsGmkSwwallWork.gear_dir = (ushort)-gmsGmkSwwallWork.gear_dir;
        gmGmkSwWallSetCol(gmsGmkSwwallWork.gmk_work.ene_com.col_work, gmsGmkSwwallWork.wall_size, gmsGmkSwwallWork.wall_draw_size, gmsGmkSwwallWork.wall_type);
    }

    public static void gmGmkSwWallCloseInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (GMS_GMK_SWWALL_WORK)obj_work;
        gmsGmkSwwallWork.wall_spd = !GmGmkSwitchTypeIsGear(gmsGmkSwwallWork.id) ? 16384 : 0;
        obj_work.flag |= 16U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSwWallCloseMain);
        obj_work.col_work.obj_col.obj = obj_work;
        if (gmsGmkSwwallWork.h_snd == null)
            return;
        GmSoundStopSE(gmsGmkSwwallWork.h_snd);
        GmSoundPlaySE("Boss3_01", gmsGmkSwwallWork.h_snd);
    }

    public static void gmGmkSwWallCloseMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SWWALL_WORK swwall_work = (GMS_GMK_SWWALL_WORK)obj_work;
        if ((swwall_work.gmk_work.ene_com.eve_rec.flag & 1) != 0 && !GmGmkSwitchIsOn(swwall_work.id) || (swwall_work.gmk_work.ene_com.eve_rec.flag & 1) == 0 && GmGmkSwitchIsOn(swwall_work.id))
            gmGmkSwWallOpenInit(obj_work);
        if (swwall_work.wall_spd != 0)
        {
            swwall_work.wall_draw_size += swwall_work.wall_spd;
        }
        else
        {
            int v2 = GmGmkSwitchGetPer(swwall_work.id);
            if ((swwall_work.gmk_work.ene_com.eve_rec.flag & 1) != 0)
                v2 = 4096 - v2;
            swwall_work.wall_draw_size = FX_Mul(swwall_work.wall_size, v2);
        }
        if (swwall_work.wall_draw_size >= swwall_work.wall_size)
        {
            swwall_work.wall_draw_size = swwall_work.wall_size;
            gmGmkSwWallFwInit(obj_work);
        }
        swwall_work.gear_dir = (ushort)(swwall_work.wall_draw_size / 64 * 16);
        if (swwall_work.gmk_work.ene_com.eve_rec.id == 257 || swwall_work.gmk_work.ene_com.eve_rec.id == 258)
            swwall_work.gear_dir = (ushort)-swwall_work.gear_dir;
        gmGmkSwWallSetCol(swwall_work.gmk_work.ene_com.col_work, swwall_work.wall_size, swwall_work.wall_draw_size, swwall_work.wall_type);
        gmGmkSwWallCheckColOff(swwall_work);
    }

    public static void gmGmkSwWallDispFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (GMS_GMK_SWWALL_WORK)obj_work;
        VecFx32 vecFx32 = new VecFx32();
        VecFx32 scale = new VecFx32();
        VecU16 vecU16 = new VecU16();
        int num1 = (gmsGmkSwwallWork.wall_draw_size + 131071) / 131072;
        OBS_COLLISION_WORK colWork = gmsGmkSwwallWork.gmk_work.ene_com.col_work;
        vecFx32.Assign(obj_work.pos);
        int num2;
        int num3 = num2 = 0;
        if (gmsGmkSwwallWork.wall_type <= 1)
        {
            if (gmsGmkSwwallWork.wall_type == 0)
            {
                vecFx32.x += colWork.obj_col.width + colWork.obj_col.ofst_x - 16 << 12;
                num3 = -131072;
            }
            else
            {
                vecFx32.x += colWork.obj_col.ofst_x + 16 << 12;
                num3 = 131072;
            }
        }
        else if (gmsGmkSwwallWork.wall_type == 2)
        {
            vecFx32.y += colWork.obj_col.height + colWork.obj_col.ofst_y - 16 << 12;
            num2 = -131072;
        }
        else
        {
            vecFx32.y += colWork.obj_col.ofst_y + 16 << 12;
            num2 = 131072;
        }
        uint dispFlag = obj_work.disp_flag;
        if (((int)gmsGmkSwwallWork.gmk_work.ene_com.enemy_flag & 2) != 0)
        {
            while (num1 > 0)
            {
                ObjDrawAction3DNN(obj_work.obj_3d, new VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref dispFlag);
                --num1;
                vecFx32.x += num3;
                vecFx32.y += num2;
            }
        }
        else
        {
            while (num1 > 0)
            {
                ObjDrawAction3DNN(obj_work.obj_3d, new VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref dispFlag);
                --num1;
                vecFx32.x += num3;
                vecFx32.y += num2;
            }
        }
        if (((int)gmsGmkSwwallWork.gmk_work.ene_com.enemy_flag & 1) == 0)
            return;
        vecU16.x = vecU16.y = 0;
        vecU16.z = (ushort)(gmsGmkSwwallWork.gear_dir + (uint)gmsGmkSwwallWork.gear_base_dir);
        ObjDrawAction3DNN(gmsGmkSwwallWork.obj_3d_opt[0], new VecFx32?(gmsGmkSwwallWork.gear_pos), new AppMain.VecU16?(vecU16), obj_work.scale, ref dispFlag);
        scale.x = scale.y = scale.z = 4096;
        if (((int)gmsGmkSwwallWork.gmk_work.ene_com.enemy_flag & 4) != 0)
            scale.x = -scale.x;
        ObjDrawAction3DNN(gmsGmkSwwallWork.obj_3d_opt[1], new VecFx32?(gmsGmkSwwallWork.gearbase_pos), new AppMain.VecU16?(obj_work.dir), scale, ref dispFlag);
    }

    public static void gmGmkSwWallSetCol(
      OBS_COLLISION_WORK col_work,
      int size,
      int draw_size,
      ushort wall_type)
    {
        if (wall_type <= 1)
        {
            col_work.obj_col.width = (ushort)((ulong)((draw_size >> 12) + 7) & 4294967288UL);
            col_work.obj_col.ofst_x = wall_type != 0 ? (short)((size >> 1) - draw_size >> 12) : (short)((-size >> 13) - (col_work.obj_col.width - (draw_size >> 12)));
            gmGmkSwWallSetColDir(col_work.obj_col.dir_data, col_work.obj_col.width + 7 >> 3, col_work.obj_col.height + 7 >> 3, false);
        }
        else
        {
            col_work.obj_col.height = (ushort)((ulong)((draw_size >> 12) + 7) & 4294967288UL);
            col_work.obj_col.ofst_y = wall_type != 2 ? (short)((size >> 1) - draw_size >> 12) : (short)((-size >> 13) - (col_work.obj_col.height - (draw_size >> 12)));
            gmGmkSwWallSetColDir(col_work.obj_col.dir_data, col_work.obj_col.width + 7 >> 3, col_work.obj_col.height + 7 >> 3, true);
        }
    }

    public static void gmGmkSwWallSetColDir(byte[] buf, int width, int height, bool wall)
    {
        if (g_gs_main_sys_info.stage_id != 9 || width <= 0 || height <= 0)
            return;
        int num1 = width >> 1;
        int num2 = height >> 1;
        if (wall)
        {
            ArrayPointer<byte> arrayPointer1 = buf;
            int num3 = 0;
            while (num3 < height)
            {
                ArrayPointer<byte> arrayPointer2 = arrayPointer1;
                int num4 = 0;
                while (num4 < num1)
                {
                    int num5 = arrayPointer2.SetPrimitive(192);
                    ++num4;
                    ++arrayPointer2;
                }
                while (num4 < width)
                {
                    int num5 = arrayPointer2.SetPrimitive(64);
                    ++num4;
                    ++arrayPointer2;
                }
                ++num3;
                arrayPointer1 += width;
            }
            arrayPointer1 = new ArrayPointer<byte>(buf, 1);
            int num6 = 1;
            while (num6 < width - 1)
            {
                int num4 = arrayPointer1.SetPrimitive(0);
                ++num6;
                ++arrayPointer1;
            }
            if (height <= 1)
                return;
            arrayPointer1 = new ArrayPointer<byte>(buf, (height - 1) * width + 1);
            int num7 = 1;
            while (num7 < width - 1)
            {
                int num4 = arrayPointer1.SetPrimitive(128);
                ++num7;
                ++arrayPointer1;
            }
        }
        else
        {
            ArrayPointer<byte> arrayPointer = buf;
            int num3;
            for (num3 = 0; num3 < num2; ++num3)
            {
                int num4 = 0;
                while (num4 < width)
                {
                    int num5 = arrayPointer.SetPrimitive(0);
                    ++num4;
                    ++arrayPointer;
                }
            }
            for (; num3 < height; ++num3)
            {
                int num4 = 0;
                while (num4 < width)
                {
                    int num5 = arrayPointer.SetPrimitive(128);
                    ++num4;
                    ++arrayPointer;
                }
            }
            arrayPointer = new ArrayPointer<byte>(buf, width);
            int num6 = 1;
            while (num6 < height - 1)
            {
                int num4 = arrayPointer.SetPrimitive(192);
                ++num6;
                arrayPointer += width;
            }
            if (width <= 1)
                return;
            arrayPointer = new ArrayPointer<byte>(buf, width + (width - 1));
            int num7 = 1;
            while (num7 < height - 1)
            {
                int num4 = arrayPointer.SetPrimitive(64);
                ++num7;
                arrayPointer += width;
            }
        }
    }

    public static void gmGmkSwWallCheckColOff(GMS_GMK_SWWALL_WORK swwall_work)
    {
        if (g_gs_main_sys_info.stage_id != 9)
            return;
        OBS_OBJECT_WORK obsObjectWork1 = (OBS_OBJECT_WORK)swwall_work;
        GMS_EVE_RECORD_EVENT eveRec = swwall_work.gmk_work.ene_com.eve_rec;
        if (eveRec.id != 248 && eveRec.id != 249 && (eveRec.id != 252 && eveRec.id != 253))
            return;
        OBS_OBJECT_WORK obsObjectWork2 = (OBS_OBJECT_WORK)g_gm_main_system.ply_work[0];
        int num1 = obsObjectWork2.pos.x >> 12;
        int num2 = obsObjectWork2.pos.y >> 12;
        int num3 = obsObjectWork1.pos.x >> 12;
        int num4 = obsObjectWork1.pos.y >> 12;
        int num5 = swwall_work.wall_size >> 12;
        if (num3 - (num5 >> 1) < num1 + obsObjectWork2.field_rect[2] && num3 + (num5 >> 1) > num1 + obsObjectWork2.field_rect[0] && (num4 + obsObjectWork1.col_work.obj_col.ofst_y + 8 <= num2 + obsObjectWork2.field_rect[3] && num4 + obsObjectWork1.col_work.obj_col.ofst_y + obsObjectWork1.col_work.obj_col.height > num2 + obsObjectWork2.field_rect[1]))
            obsObjectWork1.col_work.obj_col.obj = null;
        else
            obsObjectWork1.col_work.obj_col.obj = obsObjectWork1;
    }

}