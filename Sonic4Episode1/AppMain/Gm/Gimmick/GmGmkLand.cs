public partial class AppMain
{
    public static void GmGmkLandBuild()
    {
        int index = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        gm_gmk_land_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(gm_gmk_land_obj_data[index][0]), GmGameDatGetGimmickData(gm_gmk_land_obj_data[index][1]), 0U);
        if (index != 2)
            return;
        gm_gmk_land_3_obj_tvx_list = GmGameDatGetGimmickData(810);
    }

    public static void GmGmkLandFlush()
    {
        int index = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(gm_gmk_land_obj_data[index][0]);
        GmGameDBuildRegFlushModel(gm_gmk_land_obj_3d_list, gimmickData.file_num);
        gm_gmk_land_3_obj_tvx_list = null;
    }

    public static OBS_OBJECT_WORK GmGmkLandInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int index1 = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK rideWork = GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_LAND");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)rideWork;
        int index2;
        ushort num;
        if (eve_rec.id == 82)
        {
            index2 = 1;
            num = 1;
        }
        else if (eve_rec.id == 83)
        {
            index2 = 2;
            num = 2;
        }
        else
        {
            index2 = 0;
            num = 0;
        }
        int index3 = gm_gmk_land_mdl_data[index1][num];
        ObjObjectCopyAction3dNNModel(rideWork, gm_gmk_land_obj_3d_list[index3], gmsEnemy3DWork.obj_3d);
        switch (index1)
        {
            case 1:
                int id = index3;
                int index4 = index3;
                ObjObjectAction3dNNMotionLoad(rideWork, 0, false, ObjDataGet(805), null, 0, null);
                ObjDrawObjectActionSet(rideWork, id);
                ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, null, null, index4, (AMS_AMB_HEADER)ObjDataGet(806).pData);
                ObjDrawObjectActionSet3DNNMaterial(rideWork, 0);
                rideWork.disp_flag |= 4U;
                break;
            case 4:
                int index5 = index3;
                ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, null, null, index5, (AMS_AMB_HEADER)ObjDataGet(815).pData);
                ObjDrawObjectActionSet3DNNMaterial(rideWork, 0);
                rideWork.disp_flag |= 16U;
                ((NNS_MOTION_KEY_Class5[])rideWork.obj_3d.motion.mmtn[0].pSubmotion[0].pKeyList)[0].Value.y = 1f;
                break;
            default:
                if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
                {
                    gmsEnemy3DWork.obj_3d.use_light_flag &= 4294967294U;
                    gmsEnemy3DWork.obj_3d.use_light_flag |= 2U;
                    gmsEnemy3DWork.obj_3d.use_light_flag |= 65536U;
                    break;
                }
                break;
        }
        if (index1 == 2)
            rideWork.ppOut = index3 != 0 ? new MPP_VOID_OBS_OBJECT_WORK(gmGmkLand3TvxRDrawFunc) : new MPP_VOID_OBS_OBJECT_WORK(gmGmkLand3TvxDrawFunc);
        rideWork.pos.z = -131072;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = rideWork;
        gmsEnemy3DWork.ene_com.col_work.obj_col.diff_data = g_gm_default_col;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        if ((eve_rec.flag & 128) == 0 && eve_rec.id != 83)
            gmsEnemy3DWork.ene_com.col_work.obj_col.attr = 1;
        switch (gm_gmk_land_col_type_tbl[index2])
        {
            case 1:
                gmsEnemy3DWork.ene_com.col_work.obj_col.width = 80;
                gmsEnemy3DWork.ene_com.col_work.obj_col.height = 24;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)(-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = -17;
                if ((gmsEnemy3DWork.ene_com.col_work.obj_col.attr & 1) != 0)
                {
                    gmsEnemy3DWork.ene_com.col_work.obj_col.height = 8;
                    ++gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y;
                    break;
                }
                break;
            case 2:
                if (index1 != 2)
                {
                    gmsEnemy3DWork.ene_com.col_work.obj_col.width = 64;
                    gmsEnemy3DWork.ene_com.col_work.obj_col.height = 64;
                    gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)(-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
                    gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = -31;
                }
                else
                {
                    gmsEnemy3DWork.ene_com.col_work.obj_col.width = 24;
                    gmsEnemy3DWork.ene_com.col_work.obj_col.height = 32;
                    gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)(-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
                    gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = -15;
                }
                rideWork.field_rect[0] = gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x;
                rideWork.field_rect[1] = gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y;
                rideWork.field_rect[2] = (short)(gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x + gmsEnemy3DWork.ene_com.col_work.obj_col.width);
                rideWork.field_rect[3] = (short)(gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y + gmsEnemy3DWork.ene_com.col_work.obj_col.height);
                break;
            default:
                gmsEnemy3DWork.ene_com.col_work.obj_col.width = 48;
                gmsEnemy3DWork.ene_com.col_work.obj_col.height = 24;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)(-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = -17;
                if ((gmsEnemy3DWork.ene_com.col_work.obj_col.attr & 1) != 0)
                {
                    gmsEnemy3DWork.ene_com.col_work.obj_col.height = 8;
                    ++gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y;
                    break;
                }
                break;
        }
        rideWork.move_flag |= 8448U;
        rideWork.disp_flag |= 4194304U;
        rideWork.flag |= 2U;
        gmGmkLandMoveInit(rideWork);
        return rideWork;
    }

    public static OBS_OBJECT_WORK GmGmkZ3LandPulleyInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK rideWork = GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_LAND_PULLEY");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)rideWork;
        ObjObjectCopyAction3dNNModel(rideWork, gm_gmk_land_obj_3d_list[2], gmsEnemy3DWork.obj_3d);
        rideWork.pos.z = -163840;
        rideWork.move_flag |= 8448U;
        rideWork.disp_flag |= 4194304U;
        rideWork.flag |= 2U;
        rideWork.user_work = (uint)(short)(eve_rec.left << 8) / 10U;
        rideWork.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkLand3TvxPulleyDrawFunc);
        return rideWork;
    }

    public static OBS_OBJECT_WORK GmGmkZ3LandRopeInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK rideWork = GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_LAND_ROPE");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)rideWork;
        ObjObjectCopyAction3dNNModel(rideWork, gm_gmk_land_obj_3d_list[3], gmsEnemy3DWork.obj_3d);
        rideWork.pos.z = -196608;
        rideWork.move_flag |= 8448U;
        rideWork.disp_flag |= 4194304U;
        rideWork.flag |= 2U;
        if (eve_rec.id == 275)
            rideWork.dir.z = 49152;
        if ((eve_rec.flag & 1) != 0)
            rideWork.dir.z += 32768;
        if (eve_rec.left != 0)
            gmsEnemy3DWork.obj_3d.mat_speed = eve_rec.left / 10f;
        rideWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkLand3TvxRopeMain);
        float num1 = 120f;
        float num2 = g_gm_main_system.sync_time / (num1 / gmsEnemy3DWork.obj_3d.mat_speed);
        gmsEnemy3DWork.obj_3d.mat_frame = (num2 - (int)num2) * num1;
        rideWork.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkLand3TvxRopeDrawFunc);
        return rideWork;
    }

    public static void gmGmkLandMoveInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.prev_pos.x = (obj_work.pos.x >> 12) + gmsEnemy3DWork.ene_com.eve_rec.left + (gmsEnemy3DWork.ene_com.eve_rec.width >> 1);
        obj_work.prev_pos.y = (obj_work.pos.y >> 12) + gmsEnemy3DWork.ene_com.eve_rec.top + (gmsEnemy3DWork.ene_com.eve_rec.height >> 1);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkLandMain);
        if ((gmsEnemy3DWork.ene_com.eve_rec.width | gmsEnemy3DWork.ene_com.eve_rec.height) == 0)
            return;
        if (gmsEnemy3DWork.ene_com.eve_rec.id != 128)
        {
            int num1;
            int num2;
            int num3;
            if (gmsEnemy3DWork.ene_com.eve_rec.height < gmsEnemy3DWork.ene_com.eve_rec.width)
            {
                num1 = gmsEnemy3DWork.ene_com.eve_rec.width >> 1;
                num2 = obj_work.pos.x >> 12;
                num3 = obj_work.prev_pos.x;
            }
            else
            {
                num1 = gmsEnemy3DWork.ene_com.eve_rec.height >> 1;
                num2 = obj_work.pos.y >> 12;
                num3 = obj_work.prev_pos.y;
            }
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 4) == 0)
            {
                ushort num4 = 768;
                while (num4 > 256 && num3 + (num1 * mtMathSin((ushort)((uint)num4 << 6)) >> 12) <= num2)
                    num4 -= 4;
                obj_work.user_timer = num4;
            }
            else
            {
                obj_work.user_timer = 0;
                obj_work.user_flag = 0U;
            }
            short num5 = (short)((gmsEnemy3DWork.ene_com.eve_rec.flag & 48) >> 4 << 8);
            obj_work.user_timer -= num5;
            obj_work.user_timer &= 16383;
        }
        else
        {
            short num1 = (short)(gmsEnemy3DWork.ene_com.eve_rec.left * 2);
            short num2 = (short)(gmsEnemy3DWork.ene_com.eve_rec.top * 2);
            short num3 = (short)(gmsEnemy3DWork.ene_com.eve_rec.width * 2);
            short num4 = (short)(gmsEnemy3DWork.ene_com.eve_rec.height * 2);
            int num5 = num3 * 2 + num4 * 2;
            obj_work.user_timer = num2 != 0 ? (num1 != 0 ? (num1 + num3 != 0 ? (num5 - num4 - MTM_MATH_ABS(num1)) * 4096 / num5 : (num3 + MTM_MATH_ABS(num2)) * 4096 / num5) : (num5 - MTM_MATH_ABS(num2)) * 4096 / num5) : MTM_MATH_ABS(num1) * 4096 / num5;
            obj_work.view_out_ofst += 256;
        }
    }

    public static void gmGmkLandMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        OBS_COLLISION_OBJ objCol = obj_work.col_work.obj_col;
        if (obj_work.user_work < 30U)
            gmGmkLandMove(obj_work);
        if (objCol.rider_obj != null && objCol.rider_obj.ride_obj == obj_work)
        {
            gmsEnemy3DWork.ene_com.enemy_flag |= 1U;
            obj_work.ofst.y = ((int)obj_work.disp_flag & 2) == 0 ? 4096 : -4096;
        }
        if (((int)gmsEnemy3DWork.ene_com.enemy_flag & 1) == 0)
            return;
        if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 64) != 0)
        {
            ++obj_work.user_work;
            if (obj_work.user_work == 30U)
            {
                obj_work.move_flag &= 4294959103U;
                obj_work.move_flag |= 128U;
                obj_work.prev_pos.x = obj_work.pos.x;
                obj_work.prev_pos.y = obj_work.pos.y;
                obj_work.spd_fall_max = 30720;
                if (gmsEnemy3DWork.ene_com.eve_rec.id == 83)
                {
                    obj_work.move_flag &= 4294967039U;
                    obj_work.move_flag |= 1024U;
                    obj_work.ppFunc = _gmGmkLandColFall;
                }
                else
                    obj_work.ppFunc = null;
            }
        }
        if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 4) == 0)
            return;
        obj_work.user_flag |= 65536U;
    }

    public static void gmGmkLandMove(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        byte num1 = gm_gmk_land_spd_tbl[(byte)(gmsEnemy3DWork.ene_com.eve_rec.flag & 3U)];
        ushort userTimer = (ushort)obj_work.user_timer;
        int num2;
        int num3;
        if (gmsEnemy3DWork.ene_com.eve_rec.id != 128)
        {
            int x = obj_work.prev_pos.x;
            int y = obj_work.prev_pos.y;
            short num4 = (short)(gmsEnemy3DWork.ene_com.eve_rec.width >> 1);
            short num5 = (short)(gmsEnemy3DWork.ene_com.eve_rec.height >> 1);
            ushort num6;
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 4) == 0)
                num6 = (ushort)((int)g_gm_main_system.sync_time * num1 + userTimer & 1023);
            else if (obj_work.user_flag == 0U)
            {
                num6 = (ushort)(obj_work.user_timer & 1023);
            }
            else
            {
                num6 = (ushort)(num1 * ((int)obj_work.user_flag & 1023) + userTimer & 1023);
                obj_work.user_flag = (uint)((int)obj_work.user_flag & 65536 | (int)obj_work.user_flag + 1 & 1023);
            }
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 8) != 0)
            {
                num2 = (x << 12) + num4 * mtMathSin((ushort)((num6 << 6) + 32768));
                num3 = (y << 12) + num5 * mtMathSin((ushort)((uint)num6 << 6));
            }
            else
            {
                num2 = (x << 12) + num4 * mtMathSin((ushort)((uint)num6 << 6));
                num3 = (y << 12) + num5 * mtMathSin((ushort)((uint)num6 << 6));
            }
        }
        else
        {
            short num4 = (short)(gmsEnemy3DWork.ene_com.eve_rec.left * 2);
            short num5 = (short)(gmsEnemy3DWork.ene_com.eve_rec.top * 2);
            short num6 = (short)(gmsEnemy3DWork.ene_com.eve_rec.width * 2);
            short num7 = (short)(gmsEnemy3DWork.ene_com.eve_rec.height * 2);
            int num8 = (num6 * 2 + num7 * 2) * (num1 == 0 ? (ushort)((g_gm_main_system.sync_time + (userTimer >> 2) & 1023L) << 2) : (int)g_gm_main_system.sync_time * num1 + userTimer & 4095) / 4096;
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 8) == 0)
            {
                if (num8 <= num6)
                {
                    num2 = gmsEnemy3DWork.ene_com.born_pos_x + (num4 + (short)num8 << 12);
                    num3 = gmsEnemy3DWork.ene_com.born_pos_y + (num5 << 12);
                }
                else if (num8 <= num6 + num7)
                {
                    num2 = gmsEnemy3DWork.ene_com.born_pos_x + (num4 + num6 << 12);
                    num3 = gmsEnemy3DWork.ene_com.born_pos_y + (num5 + (short)num8 - num6 << 12);
                }
                else if (num8 <= num6 * 2 + num7)
                {
                    num2 = gmsEnemy3DWork.ene_com.born_pos_x + (num4 + num6 - ((short)num8 - num6 - num7) << 12);
                    num3 = gmsEnemy3DWork.ene_com.born_pos_y + (num5 + num7 << 12);
                }
                else
                {
                    num2 = gmsEnemy3DWork.ene_com.born_pos_x + (num4 << 12);
                    num3 = gmsEnemy3DWork.ene_com.born_pos_y + (num5 + (num7 - ((short)num8 - num6 * 2 - num7)) << 12);
                }
            }
            else if (num8 <= num6)
            {
                num2 = gmsEnemy3DWork.ene_com.born_pos_x + (num4 + num6 - (short)num8 << 12);
                num3 = gmsEnemy3DWork.ene_com.born_pos_y + (num5 << 12);
            }
            else if (num8 <= num6 + num7)
            {
                num2 = gmsEnemy3DWork.ene_com.born_pos_x + (num4 << 12);
                num3 = gmsEnemy3DWork.ene_com.born_pos_y + (num5 + (short)num8 - num6 << 12);
            }
            else if (num8 <= num6 * 2 + num7)
            {
                num2 = gmsEnemy3DWork.ene_com.born_pos_x + (num4 + ((short)num8 - num6 - num7) << 12);
                num3 = gmsEnemy3DWork.ene_com.born_pos_y + (num5 + num7 << 12);
            }
            else
            {
                num2 = gmsEnemy3DWork.ene_com.born_pos_x + (num4 + num6 << 12);
                num3 = gmsEnemy3DWork.ene_com.born_pos_y + (num5 + (num7 - ((short)num8 - num6 * 2 - num7)) << 12);
            }
        }
        obj_work.move.x = num2 - obj_work.pos.x;
        obj_work.move.y = num3 - obj_work.pos.y;
        obj_work.pos.x = num2;
        obj_work.pos.y = num3;
    }

    public static void gmGmkLandColFall(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.move_flag & 1) == 0)
            return;
        obj_work.move_flag |= 256U;
        obj_work.ppFunc = null;
    }

    public static void gmGmkZ3LandPulleyMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.dir.z += (ushort)obj_work.user_work;
    }

    public static void gmGmkLand3TvxRopeMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        obj3d.mat_frame += obj3d.mat_speed;
        if (obj3d.mat_frame < 120.0)
            return;
        obj3d.mat_frame -= 120f;
    }

    public static void gmGmkLand3TvxDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        NNS_TEXCOORD uv = new NNS_TEXCOORD(0.0f, 0.0f);
        gmGmkLand3TvxDrawFuncEx(0U, obj_work.obj_3d.texlist, ref obj_work.pos, ref obj_work.scale, GMD_TVX_DISP_LIGHT_DISABLE, 0, ref uv);
    }

    public static void gmGmkLand3TvxRDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        NNS_TEXCOORD uv = new NNS_TEXCOORD(0.0f, 0.0f);
        gmGmkLand3TvxDrawFuncEx(1U, obj_work.obj_3d.texlist, ref obj_work.pos, ref obj_work.scale, GMD_TVX_DISP_LIGHT_DISABLE, 0, ref uv);
    }

    public static void gmGmkLand3TvxPulleyDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        NNS_TEXCOORD uv = new NNS_TEXCOORD(0.0f, 0.0f);
        gmGmkLand3TvxDrawFuncEx(2U, obj_work.obj_3d.texlist, ref obj_work.pos, ref obj_work.scale, GMD_TVX_DISP_LIGHT_DISABLE | GMD_TVX_DISP_ROTATE, (short)-obj_work.dir.z, ref uv);
    }

    public static void gmGmkLand3TvxRopeDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        var coord = new NNS_TEXCOORD(0.0f, 0.0f)
        {
            v = (float)(-0.25 * obj_work.obj_3d.mat_frame / 120.0)
        };
        gmGmkLand3TvxDrawFuncEx(3U, obj_work.obj_3d.texlist, ref obj_work.pos, ref obj_work.scale, GMD_TVX_DISP_LIGHT_DISABLE | GMD_TVX_DISP_ROTATE, (short)-obj_work.dir.z, ref coord);
    }

    public static void gmGmkLand3TvxDrawFuncEx(
      uint tvx_index,
      NNS_TEXLIST texlist,
      ref VecFx32 pos,
      ref VecFx32 scale,
      uint disp_flag,
      short dir_z,
      ref NNS_TEXCOORD uv)
    {
        int index = (int)tvx_index;
        TVX_FILE model_tvx;
        if (gm_gmk_land_3_obj_tvx_list.buf[index] == null)
        {
            model_tvx = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_land_3_obj_tvx_list, index));
            gm_gmk_land_3_obj_tvx_list.buf[index] = model_tvx;
        }
        else
            model_tvx = (TVX_FILE)gm_gmk_land_3_obj_tvx_list.buf[index];

        var work = new GMS_TVX_EX_WORK()
        {
            u_wrap = 1,
            v_wrap = 1,
            coord = {
        u = uv.u,
        v = uv.v
      },
            color = uint.MaxValue
        };
        GmTvxSetModelEx(model_tvx, texlist, ref pos, ref scale, disp_flag, dir_z, ref work);
    }


}