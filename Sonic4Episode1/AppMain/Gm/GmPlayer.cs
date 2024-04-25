using System;

public partial class AppMain
{
    private static byte[][] g_gm_player_motion_right_tbl
    {
        get
        {
            if (_g_gm_player_motion_right_tbl == null)
                _g_gm_player_motion_right_tbl = new byte[7][]
                {
                    gm_player_motion_list_son_right,
                    gm_player_motion_list_sson_right,
                    gm_player_motion_list_son_right,
                    gm_player_motion_list_pn_son_right,
                    gm_player_motion_list_pn_sson_right,
                    gm_player_motion_list_tr_son_right,
                    gm_player_motion_list_tr_son_right
                };
            return _g_gm_player_motion_right_tbl;
        }
    }

    private static byte[][] g_gm_player_motion_left_tbl
    {
        get
        {
            if (_g_gm_player_motion_left_tbl == null)
                _g_gm_player_motion_left_tbl = new byte[7][]
                {
                    gm_player_motion_list_son_left,
                    gm_player_motion_list_sson_left,
                    gm_player_motion_list_son_left,
                    gm_player_motion_list_pn_son_left,
                    gm_player_motion_list_pn_sson_left,
                    gm_player_motion_list_tr_son_left,
                    gm_player_motion_list_tr_son_left
                };
            return _g_gm_player_motion_left_tbl;
        }
    }

    private static byte[][] g_gm_player_model_tbl
    {
        get
        {
            if (_g_gm_player_model_tbl == null)
                _g_gm_player_model_tbl = new byte[7][]
                {
                    gm_player_model_list_son,
                    gm_player_model_list_son,
                    gm_player_model_list_son,
                    gm_player_model_list_pn_son,
                    gm_player_model_list_pn_son,
                    gm_player_model_list_tr_son,
                    gm_player_model_list_tr_son
                };
            return _g_gm_player_model_tbl;
        }
    }

    private static byte[][] g_gm_player_mtn_blend_setting_tbl
    {
        get
        {
            if (_g_gm_player_mtn_blend_setting_tbl == null)
                _g_gm_player_mtn_blend_setting_tbl = new byte[7][]
                {
                    gm_player_mtn_blend_setting_son,
                    gm_player_mtn_blend_setting_son,
                    gm_player_mtn_blend_setting_son,
                    gm_player_mtn_blend_setting_pn_son,
                    gm_player_mtn_blend_setting_pn_son,
                    gm_player_mtn_blend_setting_tr_son,
                    gm_player_mtn_blend_setting_tr_son
                };
            return _g_gm_player_mtn_blend_setting_tbl;
        }
    }

    public static bool GMM_PLAYER_IS_TOUCH_SUPER_SONIC_REGION(int x, int y)
    {
        return x > 390 && x < 475 && y > 5 && y < 85;
    }

    public static void GMD_PLAYER_WATER_SET(ref int fSpd)
    {
        fSpd >>= 1;
    }

    public static void GMD_PLAYER_WATERJUMP_SET(ref int fSpd)
    {
        fSpd = (fSpd >> 1) + (fSpd >> 2);
    }

    public static int GMD_PLAYER_WATER_GET(int fSpd)
    {
        return fSpd >> 1;
    }

    public static int GMD_PLAYER_WATERJUMP_GET(int fSpd)
    {
        return (fSpd >> 1) + (fSpd >> 2);
    }

    private static void GmPlayerBuild()
    {
        AMS_AMB_HEADER archive1 = readAMBFile((AMS_FS)g_gm_player_data_work[0][0].pData);
        AMS_AMB_HEADER amb_tex1 = readAMBFile((AMS_FS)g_gm_player_data_work[0][1].pData);
        g_gm_ply_son_obj_3d_list = New<OBS_ACTION3D_NN_WORK>(archive1.file_num);
        ArrayPointer<OBS_ACTION3D_NN_WORK> arrayPointer =
            new ArrayPointer<OBS_ACTION3D_NN_WORK>(g_gm_ply_son_obj_3d_list);
        int index1 = 0;
        while (index1 < archive1.file_num)
        {
            ObjAction3dNNModelLoad(arrayPointer, null, null, index1, archive1, null, amb_tex1, 0U);
            ++index1;
            ++arrayPointer;
        }

        AMS_AMB_HEADER archive2 = readAMBFile((AMS_FS)g_gm_player_data_work[0][2].pData);
        AMS_AMB_HEADER amb_tex2 = readAMBFile((AMS_FS)g_gm_player_data_work[0][3].pData);
        g_gm_ply_sson_obj_3d_list = New<OBS_ACTION3D_NN_WORK>(archive2.file_num);
        arrayPointer = new ArrayPointer<OBS_ACTION3D_NN_WORK>(g_gm_ply_sson_obj_3d_list);
        int index2 = 0;
        while (index2 < archive2.file_num)
        {
            ObjAction3dNNModelLoad(arrayPointer, null, null, index2, archive2, null, amb_tex2, 0U);
            ++index2;
            ++arrayPointer;
        }

        gm_ply_obj_3d_list_tbl = new OBS_ACTION3D_NN_WORK[7][][]
        {
            new OBS_ACTION3D_NN_WORK[2][]
            {
                g_gm_ply_son_obj_3d_list,
                g_gm_ply_sson_obj_3d_list
            },
            new OBS_ACTION3D_NN_WORK[2][]
            {
                g_gm_ply_son_obj_3d_list,
                g_gm_ply_sson_obj_3d_list
            },
            new OBS_ACTION3D_NN_WORK[2][]
            {
                g_gm_ply_son_obj_3d_list,
                g_gm_ply_sson_obj_3d_list
            },
            new OBS_ACTION3D_NN_WORK[2][]
            {
                g_gm_ply_son_obj_3d_list,
                g_gm_ply_sson_obj_3d_list
            },
            new OBS_ACTION3D_NN_WORK[2][]
            {
                g_gm_ply_son_obj_3d_list,
                g_gm_ply_sson_obj_3d_list
            },
            new OBS_ACTION3D_NN_WORK[2][]
            {
                g_gm_ply_son_obj_3d_list,
                g_gm_ply_sson_obj_3d_list
            },
            new OBS_ACTION3D_NN_WORK[2][]
            {
                g_gm_ply_son_obj_3d_list,
                g_gm_ply_sson_obj_3d_list
            }
        };
    }

    private static void GmPlayerFlush()
    {
        AMS_AMB_HEADER amsAmbHeader1 = readAMBFile((AMS_FS)g_gm_player_data_work[0][0].pData);
        ArrayPointer<OBS_ACTION3D_NN_WORK> arrayPointer =
            new ArrayPointer<OBS_ACTION3D_NN_WORK>(g_gm_ply_son_obj_3d_list);
        int num1 = 0;
        while (num1 < amsAmbHeader1.file_num)
        {
            ObjAction3dNNModelRelease(arrayPointer);
            ++num1;
            ++arrayPointer;
        }

        AMS_AMB_HEADER amsAmbHeader2 = readAMBFile((AMS_FS)g_gm_player_data_work[0][2].pData);
        arrayPointer = new ArrayPointer<OBS_ACTION3D_NN_WORK>(g_gm_ply_sson_obj_3d_list);
        int num2 = 0;
        while (num2 < amsAmbHeader2.file_num)
        {
            ObjAction3dNNModelRelease(~arrayPointer);
            ++num2;
            ++arrayPointer;
        }
    }

    private static bool GmPlayerBuildCheck()
    {
        AMS_AMB_HEADER amsAmbHeader1 = readAMBFile((AMS_FS)g_gm_player_data_work[0][0].pData);
        ArrayPointer<OBS_ACTION3D_NN_WORK> arrayPointer =
            new ArrayPointer<OBS_ACTION3D_NN_WORK>(g_gm_ply_son_obj_3d_list);
        int num1 = 0;
        while (num1 < amsAmbHeader1.file_num)
        {
            if (!ObjAction3dNNModelLoadCheck(arrayPointer))
                return false;
            ++num1;
            ++arrayPointer;
        }

        AMS_AMB_HEADER amsAmbHeader2 = readAMBFile((AMS_FS)g_gm_player_data_work[0][2].pData);
        arrayPointer = new ArrayPointer<OBS_ACTION3D_NN_WORK>(g_gm_ply_sson_obj_3d_list);
        int num2 = 0;
        while (num2 < amsAmbHeader2.file_num)
        {
            if (!ObjAction3dNNModelLoadCheck(arrayPointer))
                return false;
            ++num2;
            ++arrayPointer;
        }

        return true;
    }

    private static bool GmPlayerFlushCheck()
    {
        ArrayPointer<OBS_ACTION3D_NN_WORK> arrayPointer;
        if (g_gm_ply_son_obj_3d_list != null)
        {
            AMS_AMB_HEADER amsAmbHeader = readAMBFile((AMS_FS)g_gm_player_data_work[0][0].pData);
            arrayPointer = new ArrayPointer<OBS_ACTION3D_NN_WORK>(g_gm_ply_son_obj_3d_list);
            int num = 0;
            while (num < amsAmbHeader.file_num)
            {
                if (!ObjAction3dNNModelReleaseCheck(arrayPointer))
                    return false;
                ++num;
                ++arrayPointer;
            }

            g_gm_ply_son_obj_3d_list = null;
        }

        if (g_gm_ply_sson_obj_3d_list != null)
        {
            AMS_AMB_HEADER amsAmbHeader = readAMBFile((AMS_FS)g_gm_player_data_work[0][2].pData);
            arrayPointer = new ArrayPointer<OBS_ACTION3D_NN_WORK>(g_gm_ply_sson_obj_3d_list);
            int num = 0;
            while (num < amsAmbHeader.file_num)
            {
                if (!ObjAction3dNNModelReleaseCheck(arrayPointer))
                    return false;
                ++num;
                ++arrayPointer;
            }

            g_gm_ply_sson_obj_3d_list = null;
        }

        return true;
    }

    private static void GmPlayerRelease()
    {
        for (int index1 = 0; index1 < 1; ++index1)
        {
            for (int index2 = 0; index2 < 5; ++index2)
                ObjDataRelease(g_gm_player_data_work[index1][index2]);
        }
    }

    private static GMS_PLAYER_WORK GmPlayerInit(
        int char_id,
        ushort ctrl_id,
        ushort player_id,
        ushort camera_id)
    {
        ushort[] numArray1 = new ushort[3]
        {
            0,
            6,
            1
        };
        ushort[] numArray2 = new ushort[3]
        {
            65533,
            ushort.MaxValue,
            65534
        };
        if (char_id >= 7)
            char_id = 0;
        OBS_OBJECT_WORK obsObjectWork =
            OBM_OBJECT_TASK_DETAIL_INIT(4352, 1, 0, 0, () => new GMS_PLAYER_WORK(), "PLAYER OBJ");
        mtTaskChangeTcbDestructor(obsObjectWork.tcb, new GSF_TASK_PROCEDURE(gmPlayerExit));
        obsObjectWork.ppUserRelease = new OBS_OBJECT_WORK_Delegate4(gmPlayerObjRelease);
        obsObjectWork.ppUserReleaseWait = new OBS_OBJECT_WORK_Delegate4(gmPlayerObjReleaseWait);
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)obsObjectWork;
        ply_work.player_id = (byte)player_id;
        ply_work.camera_no = (byte)camera_id;
        ply_work.ctrl_id = (byte)ctrl_id;
        ply_work.char_id = (byte)char_id;
        ply_work.act_state = -1;
        ply_work.prev_act_state = -1;
        nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
        GmPlayerInitModel(ply_work);
        ply_work.key_map[0] = 1;
        ply_work.key_map[1] = 2;
        ply_work.key_map[2] = 4;
        ply_work.key_map[3] = 8;
        ply_work.key_map[4] = 32;
        ply_work.key_map[5] = 128;
        ply_work.key_map[6] = 64;
        ply_work.key_map[7] = 16;
        obsObjectWork.obj_type = 1;
        obsObjectWork.flag |= 16U;
        obsObjectWork.flag |= 1U;
        obsObjectWork.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmPlayerDispFunc);
        obsObjectWork.ppIn = GSM_MAIN_STAGE_IS_SPSTAGE()
            ? new MPP_VOID_OBS_OBJECT_WORK(gmPlayerSplStgInFunc)
            : new MPP_VOID_OBS_OBJECT_WORK(gmPlayerDefaultInFunc);
        obsObjectWork.ppLast = new MPP_VOID_OBS_OBJECT_WORK(gmPlayerDefaultLastFunc);
        obsObjectWork.ppMove = new MPP_VOID_OBS_OBJECT_WORK(GmPlySeqMoveFunc);
        obsObjectWork.disp_flag |= 2048U;
        GmPlySeqSetSeqState(ply_work);
        GmPlayerStateInit(ply_work);
        ObjObjectFieldRectSet(obsObjectWork, -6, -12, 6, 13);
        ObjObjectGetRectBuf(obsObjectWork, ply_work.rect_work, 3);
        for (int index = 0; index < 3; ++index)
        {
            ObjRectGroupSet(ply_work.rect_work[index], 0, 2);
            ObjRectAtkSet(ply_work.rect_work[index], numArray1[index], 1);
            ObjRectDefSet(ply_work.rect_work[index], numArray2[index], 0);
            ply_work.rect_work[index].parent_obj = obsObjectWork;
            ply_work.rect_work[index].flag &= 4294967291U;
            ply_work.rect_work[index].rect.back = -16;
            ply_work.rect_work[index].rect.front = 16;
        }

        ply_work.rect_work[0].ppDef = new OBS_RECT_WORK_Delegate1(gmPlayerDefFunc);
        ply_work.rect_work[1].ppHit = new OBS_RECT_WORK_Delegate1(gmPlayerAtkFunc);
        ply_work.rect_work[0].flag |= 128U;
        ply_work.rect_work[1].flag |= 32U;
        ply_work.rect_work[2].flag |= 224U;
        if (GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            ObjObjectFieldRectSet(obsObjectWork, -7, -8, 7, 10);
            ObjRectWorkZSet(ply_work.rect_work[2], -11, -11, -500, 11, 11, 500);
            ObjRectWorkZSet(ply_work.rect_work[0], -12, -12, -500, 12, 12, 500);
            ObjRectWorkZSet(ply_work.rect_work[1], -13, -13, -500, 13, 13, 500);
        }
        else
        {
            ObjRectWorkZSet(ply_work.rect_work[2], -8, -19, -500, 8, 13, 500);
            ObjRectWorkZSet(ply_work.rect_work[0], -8, -19, -500, 8, 13, 500);
            ObjRectWorkZSet(ply_work.rect_work[1], -16, -19, -500, 16, 13, 500);
        }

        ply_work.rect_work[1].flag &= 4294967291U;
        ObjRectWorkZSet(gm_ply_touch_rect[0], -16, -51, -500, 64, 37, 500);
        ObjRectWorkZSet(gm_ply_touch_rect[1], -64, -51, -500, -16, 37, 500);
        ply_work.calc_accel.x = 0.0f;
        ply_work.calc_accel.y = 0.0f;
        ply_work.calc_accel.z = 0.0f;
        ply_work.control_type = 2;
        ply_work.jump_rect = gm_player_push_jump_key_rect[2];
        ply_work.ssonic_rect = gm_player_push_ssonic_key_rect[2];
        if (((int)g_gs_main_sys_info.game_flag & 1) != 0)
        {
            ply_work.control_type = 0;
            ply_work.jump_rect = gm_player_push_jump_key_rect[0];
            ply_work.ssonic_rect = gm_player_push_ssonic_key_rect[0];
        }

        ply_work.accel_counter = 0;
        ply_work.dir_vec_add = 0;
        ply_work.spin_se_timer = 0;
        ply_work.spin_back_se_timer = 0;
        ply_work.safe_timer = 0;
        ply_work.safe_jump_timer = 0;
        ply_work.safe_spin_timer = 0;
        if (ply_work.player_id == 0)
        {
            gm_pos_x = ply_work.obj_work.pos.x >> 12;
            gm_pos_y = ply_work.obj_work.pos.y >> 12;
            gm_pos_z = ply_work.obj_work.pos.z >> 12;
        }

        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlayerMain);
        GmPlySeqChangeFw(ply_work);
        if (!SaveState.resumePlayer_2(ply_work))
        {
            ply_work.obj_work.pos.x = g_gm_main_system.resume_pos_x;
            ply_work.obj_work.pos.y = g_gm_main_system.resume_pos_y;
        }

        if (g_gs_main_sys_info.stage_id == 5)
            GmPlayerSetPinballSonic(ply_work);
        else if (GSM_MAIN_STAGE_IS_SPSTAGE())
            GmPlayerSetSplStgSonic(ply_work);
        return ply_work;
    }

    private static void GmPlayerResetInit(GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag &= 4290772991U;
        g_obj.flag &= 4294966271U;
        g_obj.scroll[0] = g_obj.scroll[1] = 0;
        ply_work.player_flag &= 4290766847U;
        ply_work.gmk_flag &= 3556759567U;
        ply_work.graind_id = 0;
        ply_work.graind_prev_ride = 0;
        ply_work.gmk_flag &= 4294967291U;
        ply_work.spd_pool = 0;
        ply_work.obj_work.scale.x = ply_work.obj_work.scale.y = ply_work.obj_work.scale.z = 4096;
        GmPlayerStateInit(ply_work);
        GmPlayerStateGimmickInit(ply_work);
    }

    private static void GmPlayerInitModel(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        ArrayPointer<OBS_ACTION3D_NN_WORK> arrayPointer = new ArrayPointer<OBS_ACTION3D_NN_WORK>(ply_work.obj_3d_work);
        for (int index1 = 0; index1 < 2; ++index1)
        {
            OBS_ACTION3D_NN_WORK[] obsActioN3DNnWorkArray = gm_ply_obj_3d_list_tbl[ply_work.char_id][index1];
            int index2 = 0;
            while (index2 < 4)
            {
                ObjCopyAction3dNNModel(obsActioN3DNnWorkArray[index2], arrayPointer);
                (~arrayPointer).blend_spd = 0.25f;
                ObjDrawSetToon(arrayPointer);
                (~arrayPointer).use_light_flag &= 4294967294U;
                (~arrayPointer).use_light_flag |= 64U;
                ++index2;
                ++arrayPointer;
            }
        }

        objWork.obj_3d = ply_work.obj_3d_work[0];
        objWork.flag |= 536870912U;
        ObjObjectAction3dNNMotionLoad(objWork, 0, true, g_gm_player_data_work[ply_work.player_id][4], null, 0, null,
            136, 16);
        objWork.disp_flag |= 16777728U;
        for (int index = 1; index < 8; ++index)
            ply_work.obj_3d_work[index].motion = ply_work.obj_3d_work[0].motion;
        GmPlayerSetModel(ply_work, 0);
    }

    private static void GmPlayerSetModel(GMS_PLAYER_WORK ply_work, int model_set)
    {
        ply_work.obj_3d[0] = ply_work.obj_3d_work[model_set * 4];
        ply_work.obj_3d[1] = ply_work.obj_3d_work[model_set * 4 + 1];
        ply_work.obj_3d[2] = ply_work.obj_3d_work[model_set * 4 + 2];
        ply_work.obj_3d[3] = ply_work.obj_3d_work[model_set * 4 + 3];
        int index = ply_work.act_state;
        if (index == -1)
            index = 0;
        ply_work.obj_work.obj_3d = ply_work.obj_3d[g_gm_player_model_tbl[ply_work.char_id][index]];
    }

    private static void GmPlayerStateInit(GMS_PLAYER_WORK ply_work)
    {
        ply_work.seq_init_tbl = g_gm_ply_seq_init_tbl_list[ply_work.char_id];
        GmPlayerSpdParameterSet(ply_work);
        ply_work.obj_work.dir.x = 0;
        ply_work.obj_work.dir.y = 0;
        ply_work.obj_work.dir.z = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd.z = 0;
        if (((int)ply_work.gmk_flag & 256) == 0)
            ply_work.obj_work.pos.z = 0;
        ply_work.obj_work.ride_obj = null;
        if (((int)ply_work.gmk_flag & 1536) == 0)
        {
            ply_work.gmk_obj = null;
            ply_work.gmk_camera_ofst_x = 0;
            ply_work.gmk_camera_ofst_y = 0;
            ply_work.gmk_camera_gmk_center_ofst_x = 0;
            ply_work.gmk_camera_gmk_center_ofst_y = 0;
            ply_work.gmk_flag &= 4227858183U;
        }

        ply_work.gmk_work0 = 0;
        ply_work.gmk_work1 = 0;
        ply_work.gmk_work2 = 0;
        ply_work.gmk_work3 = 0;
        ply_work.spd_work_max = 0;
        ply_work.camera_jump_pos_y = 0;
        if ((ply_work.graind_id & 128) != 0)
            ply_work.obj_work.flag |= 1U;
        ply_work.gmk_flag &= 3720345596U;
        ply_work.player_flag &= 4294367568U;
        ply_work.obj_work.flag &= 4294967293U;
        ply_work.obj_work.flag |= 16U;
        ply_work.obj_work.disp_flag &= 4294966991U;
        ply_work.obj_work.move_flag &= 3623816416U;
        ply_work.obj_work.move_flag |= 1076756672U;
        if ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) != 0)
            ply_work.obj_work.move_flag &= 0b11111111111110111111111111111111U;
        else if (GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            ply_work.obj_work.move_flag &= 4294705151U;
            ply_work.obj_work.move_flag |= 536875008U;
        }

        ply_work.no_jump_move_timer = 0;
        if (ply_work.obj_work.obj_3d == null)
            return;
        ply_work.obj_work.obj_3d.blend_spd = 0.25f;
    }

    private static void GmPlayerStateGimmickInit(GMS_PLAYER_WORK ply_work)
    {
        ply_work.gmk_obj = null;
        ply_work.gmk_camera_ofst_x = 0;
        ply_work.gmk_camera_ofst_y = 0;
        ply_work.gmk_camera_gmk_center_ofst_x = 0;
        ply_work.gmk_camera_gmk_center_ofst_y = 0;
        ply_work.gmk_work0 = 0;
        ply_work.gmk_work1 = 0;
        ply_work.gmk_work2 = 0;
        ply_work.gmk_work3 = 0;
        ply_work.obj_work.dir.x = 0;
        ply_work.obj_work.dir.y = 0;
        ply_work.score_combo_cnt = 0U;
        GmPlayerCameraOffsetSet(ply_work, 0, 0);
        GmCameraAllowReset();
        if ((ply_work.graind_id & 128) != 0)
        {
            ply_work.obj_work.flag |= 1U;
            ply_work.graind_id = 0;
        }

        ply_work.player_flag &= 4294434128U;
        ply_work.gmk_flag &= 4227612431U;
        ply_work.gmk_flag2 &= 4294967097U;
        ply_work.obj_work.disp_flag &= 4294967007U;
        ply_work.obj_work.move_flag &= 3623816447U;
        ply_work.obj_work.move_flag |= 1076232384U;
        ply_work.no_jump_move_timer = 0;
        if ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) != 0)
        {
            ply_work.obj_work.move_flag &= 4294705151U;
        }
        else
        {
            if (!GSM_MAIN_STAGE_IS_SPSTAGE())
                return;
            ply_work.obj_work.move_flag &= 4294705151U;
            ply_work.obj_work.move_flag |= 536875008U;
        }
    }

    private static void GmPlayerSpdParameterSet(GMS_PLAYER_WORK ply_work)
    {
        GmPlayer.SpdParameterSet(ply_work);
    }

    private static void GmPlayerSpdParameterSetWater(GMS_PLAYER_WORK ply_work, bool water)
    {
        ply_work.spd_jump = g_gm_player_parameter[ply_work.char_id].spd_jump;
        ply_work.obj_work.spd_fall = g_gm_player_parameter[ply_work.char_id].spd_fall;
        if (!water)
            return;
        GMD_PLAYER_WATERJUMP_SET(ref ply_work.spd_jump);
        GMD_PLAYER_WATER_SET(ref ply_work.obj_work.spd_fall);
    }

    private static void GmPlayerSetAtk(GMS_PLAYER_WORK ply_work)
    {
        ply_work.rect_work[1].flag |= 4U;
        ObjRectHitAgain(ply_work.rect_work[1]);
    }

    private static void GmPlayerSetDefInvincible(GMS_PLAYER_WORK ply_work)
    {
        ply_work.rect_work[0].def_power = 3;
    }

    private static void GmPlayerSetDefNormal(GMS_PLAYER_WORK ply_work)
    {
        ply_work.rect_work[0].def_power = 0;
    }

    private static void GmPlayerBreathingSet(GMS_PLAYER_WORK ply_work)
    {
        ply_work.water_timer = 0;
    }

    private static void GmPlayerSetMarkerPoint(
        GMS_PLAYER_WORK ply_work,
        int pos_x,
        int pos_y)
    {
        g_gm_main_system.time_save = g_gm_main_system.game_time;
        g_gm_main_system.resume_pos_x = pos_x;
        g_gm_main_system.resume_pos_y = pos_y - (ply_work.obj_work.field_rect[3] << 12);
    }

    private static void GmPlayerSetSuperSonic(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerStateInit(ply_work);
        if ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) != 0)
        {
            ply_work.obj_work.pos.z = short.MinValue;
            ply_work.gmk_flag |= 536870912U;
        }

        ply_work.char_id = (ply_work.player_flag & GMD_PLF_PINBALL_SONIC) == 0
            ? ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) == 0 ? (byte)1 : (byte)6)
            : (byte)4;
        ply_work.player_flag |= GMD_PLF_SUPER_SONIC;
        GmPlayerSetModel(ply_work, 1);
        GmPlySeqSetSeqState(ply_work);
        GmPlayerSpdParameterSet(ply_work);
        ply_work.obj_work.move_flag |= 16U;
        ply_work.obj_work.move_flag &= 4294967152U;
        ply_work.obj_work.flag |= 2U;
        GmPlyEfctCreateSuperAuraDeco(ply_work);
        GmPlyEfctCreateSuperAuraBase(ply_work);
        ply_work.super_sonic_ring_timer = int.MaxValue;
        ply_work.light_rate = 0.0f;
        ply_work.light_anm_flag = 0;
        g_gm_main_system.game_flag |= 524288U;
        GmSoundPlaySE("Transform");
        if (g_gs_main_sys_info.stage_id == 28)
            return;
        GmSoundPlayJingleInvincible();
    }

    private static void GmPlayerSetEndSuperSonic(GMS_PLAYER_WORK ply_work)
    {
        ply_work.char_id = (ply_work.player_flag & GMD_PLF_PINBALL_SONIC) == 0
            ? ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) == 0 ? (byte)0 : (byte)5)
            : (byte)3;
        ply_work.player_flag &= 4294950911U;
        GmPlayerSetModel(ply_work, 0);
        GmPlySeqSetSeqState(ply_work);
        GmPlayerSpdParameterSet(ply_work);
        GmPlyEfctCreateSuperEnd(ply_work);
        GmPlayerSetDefLight();
        GmPlayerSetDefRimParam(ply_work);
    }

    private static void GmPlayerSetDefLight()
    {
        ObjDrawSetParallelLight(NNE_LIGHT_6, ref g_gm_main_system.ply_light_col, 1f, g_gm_main_system.ply_light_vec);
    }

    private static void GmPlayerSetSplStgSonic(GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.move_flag |= 139520U;
        ply_work.obj_work.move_flag &= 4294705151U;
        GmPlySeqSetSeqState(ply_work);
        GmPlayerSpdParameterSet(ply_work);
        GmPlayerActionChange(ply_work, 39);
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlayerSetPinballSonic(GMS_PLAYER_WORK ply_work)
    {
        ply_work.char_id = (ply_work.player_flag & GMD_PLF_SUPER_SONIC) == 0 ? (byte)3 : (byte)4;
        ply_work.player_flag |= GMD_PLF_PINBALL_SONIC;
        ply_work.obj_work.move_flag &= 4294705151U;
        GmPlySeqSetSeqState(ply_work);
        GmPlayerSpdParameterSet(ply_work);
        GmPlayerActionChange(ply_work, 39);
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlayerSetEndPinballSonic(GMS_PLAYER_WORK ply_work)
    {
        ply_work.char_id = (ply_work.player_flag & GMD_PLF_SUPER_SONIC) == 0 ? (byte)0 : (byte)1;
        ply_work.player_flag &= 4294836223U;
        ply_work.obj_work.move_flag |= GMD_PLF_TRUCK_RIDE;
        GmPlySeqSetSeqState(ply_work);
        GmPlayerSpdParameterSet(ply_work);
    }

    private static void GmPlayerSetTruckRide(
        GMS_PLAYER_WORK ply_work,
        OBS_OBJECT_WORK truck_obj,
        short field_left,
        short field_top,
        short field_right,
        short field_bottom)
    {
        bool flag = false;
        OBS_OBJECT_WORK pObj = (OBS_OBJECT_WORK)ply_work;
        ply_work.char_id = (ply_work.player_flag & GMD_PLF_SUPER_SONIC) == 0 ? (byte)5 : (byte)6;
        ply_work.player_flag |= GMD_PLF_TRUCK_RIDE;
        ply_work.obj_work.move_flag &= 4294705151U;
        ply_work.gmk_flag2 &= 4294967294U;
        ply_work.truck_obj = truck_obj;
        ply_work.obj_work.ppRec = new MPP_VOID_OBS_OBJECT_WORK(gmPlayerRectTruckFunc);
        ply_work.obj_work.ppCol = new MPP_VOID_OBS_OBJECT_WORK(gmPlayerTruckCollisionFunc);
        ArrayPointer<OBS_ACTION3D_NN_WORK> arrayPointer = new ArrayPointer<OBS_ACTION3D_NN_WORK>(ply_work.obj_3d_work);
        int num = 0;
        while (num < 8)
        {
            (~arrayPointer).mtn_cb_func = new mtn_cb_func_delegate(gmGmkPlayerMotionCallbackTruck);
            (~arrayPointer).mtn_cb_param = ply_work;
            ++num;
            ++arrayPointer;
        }

        nnMakeUnitMatrix(ply_work.truck_mtx_ply_mtn_pos);
        GmPlySeqSetSeqState(ply_work);
        GmPlayerSpdParameterSet(ply_work);
        ObjObjectFieldRectSet(pObj, field_left, field_top, field_right, field_bottom);
        pObj.field_ajst_w_db_f = 3;
        pObj.field_ajst_w_db_b = 4;
        pObj.field_ajst_w_dl_f = 3;
        pObj.field_ajst_w_dl_b = 4;
        pObj.field_ajst_w_dt_f = 3;
        pObj.field_ajst_w_dt_b = 4;
        pObj.field_ajst_w_dr_f = 3;
        pObj.field_ajst_w_dr_b = 4;
        pObj.field_ajst_h_db_r = 3;
        pObj.field_ajst_h_db_l = 3;
        pObj.field_ajst_h_dl_r = 3;
        pObj.field_ajst_h_dl_l = 3;
        pObj.field_ajst_h_dt_r = 3;
        pObj.field_ajst_h_dt_l = 3;
        pObj.field_ajst_h_dr_r = 3;
        pObj.field_ajst_h_dr_l = 3;
        ObjRectWorkSet(ply_work.rect_work[2], -8, (short)(field_bottom - 32), 8, field_bottom);
        ObjRectWorkSet(ply_work.rect_work[0], -8, (short)(field_bottom - 48), 8, (short)(field_bottom - 16));
        ObjRectWorkSet(ply_work.rect_work[1], -16, (short)(field_bottom - 48), 16, (short)(field_bottom - 16));
        ply_work.rect_work[1].flag &= 4294967291U;
        ObjCameraGet(g_obj.glb_camera_id).user_func = new OBJF_CAMERA_USER_FUNC(GmCameraTruckFunc);
        if (((int)ply_work.obj_work.disp_flag & 1) != 0)
            GmPlayerSetReverse(ply_work);
        if (ply_work.seq_state == GME_PLY_SEQ_STATE_GMK_DEMO_FW)
            flag = true;
        GmPlySeqChangeFw(ply_work);
        if (!flag)
            return;
        GmPlySeqInitDemoFw(ply_work);
    }

    private static void GmPlayerSetEndTruckRide(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)ply_work;
        ply_work.char_id = (ply_work.player_flag & 16384) == 0 ? (byte)0 : (byte)1;
        ply_work.player_flag &= 4294705151U;
        ply_work.obj_work.move_flag |= 262144U;
        ply_work.obj_work.ppRec = null;
        ply_work.obj_work.ppCol = null;
        ArrayPointer<OBS_ACTION3D_NN_WORK> arrayPointer = new ArrayPointer<OBS_ACTION3D_NN_WORK>(ply_work.obj_3d_work);
        int num = 0;
        while (num < 8)
        {
            (~arrayPointer).mtn_cb_func = null;
            (~arrayPointer).mtn_cb_param = null;
            ++num;
            ++arrayPointer;
        }

        GmPlySeqSetSeqState(ply_work);
        GmPlayerSpdParameterSet(ply_work);
        ObjObjectFieldRectSet(ply_work.obj_work, -6, -12, 6, 13);
        obsObjectWork.field_ajst_w_db_f = 2;
        obsObjectWork.field_ajst_w_db_b = 4;
        obsObjectWork.field_ajst_w_dl_f = 2;
        obsObjectWork.field_ajst_w_dl_b = 4;
        obsObjectWork.field_ajst_w_dt_f = 2;
        obsObjectWork.field_ajst_w_dt_b = 4;
        obsObjectWork.field_ajst_w_dr_f = 2;
        obsObjectWork.field_ajst_w_dr_b = 4;
        obsObjectWork.field_ajst_h_db_r = 1;
        obsObjectWork.field_ajst_h_db_l = 1;
        obsObjectWork.field_ajst_h_dl_r = 1;
        obsObjectWork.field_ajst_h_dl_l = 1;
        obsObjectWork.field_ajst_h_dt_r = 1;
        obsObjectWork.field_ajst_h_dt_l = 1;
        obsObjectWork.field_ajst_h_dr_r = 2;
        obsObjectWork.field_ajst_h_dr_l = 2;
        ObjRectWorkZSet(ply_work.rect_work[2], -8, -19, -500, 8, 13, 500);
        ObjRectWorkZSet(ply_work.rect_work[0], -8, -19, -500, 8, 13, 500);
        ObjRectWorkZSet(ply_work.rect_work[1], -16, -19, -500, 16, 13, 500);
        ply_work.rect_work[1].flag &= 4294967291U;
        ply_work.obj_work.dir_fall = 0;
        g_gm_main_system.pseudofall_dir = 0;
        ObjCameraGet(g_obj.glb_camera_id).user_func = new OBJF_CAMERA_USER_FUNC(GmCameraFunc);
    }

    private static void GmPlayerSetGoalState(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 131072) != 0)
            GmPlayerSetEndPinballSonic(ply_work);
        if ((ply_work.player_flag & 16384) != 0)
            gmPlayerSuperSonicToSonic(ply_work);
        GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
        ply_work.genocide_timer = 0;
        if ((ply_work.player_flag & 262144) == 0)
            return;
        ObjRectWorkSet(ply_work.rect_work[2], 0, -37, 16, -5);
    }

    private static void GmPlayerSetAutoRun(
        GMS_PLAYER_WORK ply_work,
        int scroll_spd_x,
        bool enable)
    {
        if (enable)
        {
            ply_work.player_flag |= 32768U;
            ply_work.scroll_spd_x = scroll_spd_x;
        }
        else
            ply_work.player_flag &= 4294934527U;
    }

    private static void GmPlayerRingGet(GMS_PLAYER_WORK ply_work, short add_ring)
    {
        short ringNum = ply_work.ring_num;
        ply_work.ring_num += add_ring;
        ply_work.ring_num = (short)MTM_MATH_CLIP(ply_work.ring_num, 0, 999);
        ply_work.ring_stage_num += add_ring;
        ply_work.ring_stage_num = (short)MTM_MATH_CLIP(ply_work.ring_stage_num, 0, 9999);
        GmRingGetSE();
        if (g_gs_main_sys_info.game_mode == 1)
            return;
        if (!GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            if ((ply_work.player_flag & 16384) != 0 || g_gs_main_sys_info.game_mode == 1)
                return;
            for (short index = 100; index <= 900; index += 100)
            {
                if (ringNum < index && ply_work.ring_num >= index)
                {
                    GmPlayerStockGet(ply_work, 1);
                    GmSoundPlayJingle1UP(true);
                }
            }
        }
        else
        {
            if (ringNum >= 50 || ply_work.ring_num < 50)
                return;
            GmPlayerStockGet(ply_work, 1);
            GmSoundPlayJingle1UP(true);
        }
    }

    private static void GmPlayerRingDec(GMS_PLAYER_WORK ply_work, short dec_ring)
    {
        if (GMM_MAIN_USE_SUPER_SONIC())
            return;
        ply_work.ring_num -= dec_ring;
        ply_work.ring_num = (short)MTM_MATH_CLIP(ply_work.ring_num, 0, 999);
        ply_work.ring_stage_num -= dec_ring;
        ply_work.ring_stage_num = (short)MTM_MATH_CLIP(ply_work.ring_stage_num, 0, 9999);
    }

    private static void GmPlayerStockGet(GMS_PLAYER_WORK ply_work, short add_stock)
    {
        if (g_gs_main_sys_info.game_mode == 1 && (21 > g_gs_main_sys_info.stage_id || g_gs_main_sys_info.stage_id > 27))
            return;
        g_gm_main_system.player_rest_num[ply_work.player_id] += (uint)add_stock;
        g_gm_main_system.player_rest_num[ply_work.player_id] =
            MTM_MATH_CLIP(g_gm_main_system.player_rest_num[ply_work.player_id], 0U, 1000U);
        HgTrophyTryAcquisition(5);
    }

    private static void GmPlayerAddScore(
        GMS_PLAYER_WORK ply_work,
        int score,
        int pos_x,
        int pos_y)
    {
        ply_work.score += (uint)score;
        GmScoreCreateScore(score, pos_x, pos_y, 4096, 0);
    }

    private static void GmPlayerAddScoreNoDisp(GMS_PLAYER_WORK ply_work, int score)
    {
        ply_work.score += (uint)score;
    }

    private static void GmPlayerComboScore(GMS_PLAYER_WORK ply_work, int pos_x, int pos_y)
    {
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
        {
            ply_work.score_combo_cnt = 0U;
        }
        else
        {
            ++ply_work.score_combo_cnt;
            if (ply_work.score_combo_cnt > 9999U)
                ply_work.score_combo_cnt = 9999U;
        }

        uint num = ply_work.score_combo_cnt != 0U
            ? (ply_work.score_combo_cnt - 1U < 5U ? ply_work.score_combo_cnt - 1U : 4U)
            : 0U;
        int score = gm_ply_score_combo_tbl[(int)num];
        ply_work.score += (uint)score;
        GmScoreCreateScore(score, pos_x, pos_y, gm_ply_score_combo_scale_tbl[(int)num],
            gm_ply_score_combo_vib_level_tbl[(int)num]);
    }

    private static void GmPlayerItemHiSpeedSet(GMS_PLAYER_WORK ply_work)
    {
        ply_work.hi_speed_timer = 3686400;
        GmPlayerSpdParameterSet(ply_work);
        GmSoundChangeSpeedupBGM();
    }

    private static void GmPlayerItemInvincibleSet(GMS_PLAYER_WORK ply_work)
    {
        GmSoundPlayJingleInvincible();
        if (ply_work.genocide_timer == 0)
            GmPlyEfctCreateInvincible(ply_work);
        ply_work.genocide_timer = 4091904;
    }

    private static void GmPlayerItemRing10Set(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerRingGet(ply_work, 10);
    }

    private static void GmPlayerItemBarrierSet(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 268435456) == 0)
        {
            GmPlyEfctCreateBarrier(ply_work);
            GmSoundPlaySE("Barrier");
        }

        ply_work.player_flag |= 268435456U;
    }

    private static void GmPlayerItem1UPSet(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerStockGet(ply_work, 1);
        GmSoundPlayJingle1UP(true);
    }

    private static void GmPlayerActionChange(GMS_PLAYER_WORK ply_work, int act_state)
    {
        ply_work.prev_act_state = ply_work.act_state;
        ply_work.act_state = act_state;
        ply_work.obj_work.obj_3d = ply_work.obj_3d[g_gm_player_model_tbl[ply_work.char_id][act_state]];
        ply_work.obj_work.obj_3d.motion._object = ply_work.obj_work.obj_3d._object;
        int id = ((int)ply_work.obj_work.disp_flag & 1) == 0
            ? g_gm_player_motion_right_tbl[ply_work.char_id][act_state]
            : g_gm_player_motion_left_tbl[ply_work.char_id][act_state];
        if (ply_work.prev_act_state != -1 &&
            g_gm_player_model_tbl[ply_work.char_id][act_state] ==
            g_gm_player_model_tbl[ply_work.char_id][ply_work.prev_act_state] &&
            (g_gm_player_mtn_blend_setting_tbl[ply_work.char_id][ply_work.prev_act_state] != 0 &&
             g_gm_player_mtn_blend_setting_tbl[ply_work.char_id][act_state] != 0))
        {
            ObjDrawObjectActionSet3DNNBlend(ply_work.obj_work, id);
            if (act_state == 26 || act_state == 27)
                ply_work.obj_work.obj_3d.blend_spd = 0.125f;
            else if (19 <= ply_work.prev_act_state && ply_work.prev_act_state < 22 &&
                     (act_state == 40 || act_state == 42))
                ply_work.obj_work.obj_3d.blend_spd = 0.125f;
            else if (ply_work.prev_act_state == 0 && act_state == 19)
                ply_work.obj_work.obj_3d.blend_spd = 0.125f;
            else if (ply_work.prev_seq_state == 20)
                ply_work.obj_work.obj_3d.blend_spd = 0.08333334f;
            else
                ply_work.obj_work.obj_3d.blend_spd = 0.25f;
        }
        else
            ObjDrawObjectActionSet3DNN(ply_work.obj_work, id, 0);
    }

    private static void GmPlayerSaveResetAction(
        GMS_PLAYER_WORK ply_work,
        GMS_PLAYER_RESET_ACT_WORK reset_act_work)
    {
        reset_act_work.frame[0] = ply_work.obj_work.obj_3d.frame[0];
        reset_act_work.frame[1] = ply_work.obj_work.obj_3d.frame[1];
        reset_act_work.blend_spd = ply_work.obj_work.obj_3d.blend_spd;
        reset_act_work.marge = ply_work.obj_work.obj_3d.marge;
        reset_act_work.obj_3d_flag = ply_work.obj_work.obj_3d.flag;
    }

    private static void GmPlayerResetAction(
        GMS_PLAYER_WORK ply_work,
        GMS_PLAYER_RESET_ACT_WORK reset_act_work)
    {
        int[] numArray1 = New<int>(2);
        float[] numArray2 = new float[2];
        numArray1[0] = ply_work.act_state;
        numArray1[1] = ply_work.prev_act_state;
        uint dispFlag = ply_work.obj_work.disp_flag;
        GmPlayerActionChange(ply_work, numArray1[1]);
        GmPlayerActionChange(ply_work, numArray1[0]);
        ply_work.obj_work.obj_3d.frame[0] = reset_act_work.frame[0];
        ply_work.obj_work.obj_3d.frame[1] = reset_act_work.frame[1];
        ply_work.obj_work.obj_3d.blend_spd = reset_act_work.blend_spd;
        ply_work.obj_work.obj_3d.marge = reset_act_work.marge;
        ply_work.obj_work.obj_3d.flag &= 4294967294U;
        ply_work.obj_work.obj_3d.flag |= reset_act_work.obj_3d_flag & 1U;
        ply_work.obj_work.disp_flag |= dispFlag & 12U;
        for (int index = 0; index < 2; ++index)
        {
            numArray2[index] =
                amMotionGetEndFrame(ply_work.obj_work.obj_3d.motion, ply_work.obj_work.obj_3d.act_id[index]) -
                amMotionGetStartFrame(ply_work.obj_work.obj_3d.motion, ply_work.obj_work.obj_3d.act_id[index]);
            if (ply_work.obj_work.obj_3d.frame[index] >= (double)numArray2[index])
                ply_work.obj_work.obj_3d.frame[index] = 0.0f;
        }
    }

    private static void GmPlayerWalkActionSet(GMS_PLAYER_WORK ply_work)
    {
        int num = MTM_MATH_ABS(ply_work.obj_work.spd_m);
        bool flag = false;
        short z = (short)ply_work.obj_work.dir.z;
        if (((int)ply_work.obj_work.disp_flag & 1) == 0 && z > 4096 || ((int)ply_work.obj_work.disp_flag & 1) != 0 &&
            z < -4096 && ((int)ply_work.gmk_flag & 131072) == 0)
        {
            flag = true;
            ply_work.maxdash_timer = 122880;
        }

        int act_state;
        if (num < ply_work.spd1)
            act_state = 19;
        else if (num < ply_work.spd2)
        {
            act_state = 20;
            if ((ply_work.player_flag & 512) == 0)
                GmPlyEfctCreateRunDust(ply_work);
        }
        else if (num < ply_work.spd3)
        {
            act_state = 21;
            if ((ply_work.player_flag & 512) == 0)
                GmPlyEfctCreateDash1Dust(ply_work);
        }
        else if (num < ply_work.spd4 && (flag || ply_work.maxdash_timer != 0))
        {
            act_state = 22;
            GmPlyEfctCreateRollDash(ply_work);
            if ((ply_work.player_flag & 512) == 0)
                GmPlyEfctCreateDash2Dust(ply_work);
            GmPlyEfctCreateDash2Impact(ply_work);
            GmPlyEfctCreateSuperAuraDash(ply_work);
        }
        else
        {
            act_state = 21;
            if ((ply_work.player_flag & 512) == 0)
                GmPlyEfctCreateDash1Dust(ply_work);
        }

        GmPlayerActionChange(ply_work, act_state);
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlayerWalkActionCheck(GMS_PLAYER_WORK ply_work)
    {
        bool flag = false;
        short z = (short)ply_work.obj_work.dir.z;
        int num = MTM_MATH_ABS(ply_work.obj_work.spd_m);
        if ((((int)ply_work.obj_work.disp_flag & 1) == 0 && z > 4096 ||
             ((int)ply_work.obj_work.disp_flag & 1) != 0 && z < -4096) && ((int)ply_work.gmk_flag & 131072) == 0)
        {
            flag = true;
            ply_work.maxdash_timer = 122880;
        }

        if (ply_work.act_state < 19 || ply_work.act_state > 22)
            GmPlayerActionChange(ply_work, 19);
        if (((int)ply_work.obj_work.disp_flag & 8) != 0)
        {
            if (ply_work.act_state == 19)
            {
                if (num >= ply_work.spd2)
                {
                    GmPlayerActionChange(ply_work, 20);
                    if ((ply_work.player_flag & 512) == 0)
                        GmPlyEfctCreateRunDust(ply_work);
                }
            }
            else if (ply_work.act_state == 20)
            {
                if (num >= ply_work.spd3)
                {
                    GmPlayerActionChange(ply_work, 21);
                    if ((ply_work.player_flag & 512) == 0)
                        GmPlyEfctCreateDash1Dust(ply_work);
                }
                else if (num < ply_work.spd2)
                    GmPlayerActionChange(ply_work, 19);
            }
            else if (ply_work.act_state == 21)
            {
                if (num >= ply_work.spd_max && (flag || ply_work.maxdash_timer != 0))
                {
                    GmPlayerActionChange(ply_work, 22);
                    GmPlyEfctCreateRollDash(ply_work);
                    if ((ply_work.player_flag & 512) == 0)
                        GmPlyEfctCreateDash2Dust(ply_work);
                    GmPlyEfctCreateDash2Impact(ply_work);
                }
                else if (num < ply_work.spd3)
                {
                    GmPlayerActionChange(ply_work, 20);
                    if ((ply_work.player_flag & 512) == 0)
                        GmPlyEfctCreateRunDust(ply_work);
                }
            }
            else if (ply_work.act_state == 22 && (num < ply_work.spd_max || !flag && ply_work.maxdash_timer == 0))
            {
                GmPlayerActionChange(ply_work, 21);
                if ((ply_work.player_flag & 512) == 0)
                    GmPlyEfctCreateDash1Dust(ply_work);
            }
        }

        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlayerAnimeSpeedSetWalk(GMS_PLAYER_WORK ply_work, int spd_set)
    {
        int a = MTM_MATH_ABS((spd_set >> 3) + (spd_set >> 2));
        if (a <= 4096)
            a = 4096;
        if (a >= 32768)
            a = 32768;
        if (ply_work.act_state == 22)
            a = 4096;
        else if ((ply_work.act_state == 26 || ply_work.act_state == 27) &&
                 (((int)ply_work.obj_work.obj_3d.flag & 1) != 0 && a > 4096))
            a = 4096;
        if (ply_work.obj_work.obj_3d == null)
            return;
        ply_work.obj_work.obj_3d.speed[0] = FXM_FX32_TO_FLOAT(a);
        ply_work.obj_work.obj_3d.speed[1] = FXM_FX32_TO_FLOAT(a);
    }

    private static void GmPlayerSpdSet(GMS_PLAYER_WORK ply_work, int spd_x, int spd_y)
    {
        ply_work.no_spddown_timer = 524288;
        if (spd_x < 0)
            ply_work.obj_work.disp_flag |= 1U;
        else
            ply_work.obj_work.disp_flag &= 4294967294U;
        if (((int)ply_work.obj_work.move_flag & 16) != 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd.x > spd_x ||
                ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd.x < spd_x)
                ply_work.obj_work.spd.x = spd_x;
            if (MTM_MATH_ABS(ply_work.obj_work.spd.y) >= MTM_MATH_ABS(spd_y))
                return;
            ply_work.obj_work.spd.y = spd_y;
        }
        else
        {
            switch ((ply_work.obj_work.dir.z + 8192 & 49152) >> 6)
            {
                case 0:
                case 2:
                    if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd_m > spd_x ||
                        ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd_m < spd_x)
                        ply_work.obj_work.spd_m = spd_x;
                    if (MTM_MATH_ABS(ply_work.obj_work.spd.y) >= MTM_MATH_ABS(spd_y))
                        break;
                    ply_work.obj_work.spd.y = spd_y;
                    if (ply_work.obj_work.spd.y >= 0)
                        break;
                    ply_work.obj_work.move_flag |= 16U;
                    break;
                case 1:
                    if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd_m > spd_y ||
                        ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd_m < spd_y)
                        ply_work.obj_work.spd_m = spd_y;
                    if (MTM_MATH_ABS(ply_work.obj_work.spd.x) >= MTM_MATH_ABS(spd_x))
                        break;
                    ply_work.obj_work.spd.x = spd_x;
                    break;
                case 3:
                    if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd_m > -spd_y ||
                        ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd_m < -spd_y)
                        ply_work.obj_work.spd_m = -spd_y;
                    if (MTM_MATH_ABS(ply_work.obj_work.spd.x) >= MTM_MATH_ABS(spd_x))
                        break;
                    ply_work.obj_work.spd.x = spd_x;
                    break;
            }
        }
    }

    private static void GmPlayerSetReverse(GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag &= 2147483375U;
        ply_work.pgm_turn_dir = 0;
        ply_work.pgm_turn_spd = 0;
        ply_work.obj_work.disp_flag ^= 1U;
        if (g_gm_player_motion_left_tbl[ply_work.char_id][ply_work.act_state] ==
            g_gm_player_motion_right_tbl[ply_work.char_id][ply_work.act_state])
            return;
        float[] numArray = new float[2];
        uint num = ply_work.obj_work.disp_flag & 12U;
        numArray[0] = ply_work.obj_work.obj_3d.frame[0];
        GmPlayerActionChange(ply_work, ply_work.act_state);
        ply_work.obj_work.obj_3d.frame[0] = numArray[0];
        ply_work.obj_work.obj_3d.marge = 0.0f;
        ply_work.obj_work.obj_3d.flag &= 4294967294U;
        ply_work.obj_work.disp_flag |= num;
    }

    private static void GmPlayerSetReverseOnlyState(GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag &= 2147483375U;
        ply_work.pgm_turn_dir = 0;
        ply_work.pgm_turn_spd = 0;
        ply_work.obj_work.disp_flag ^= 1U;
    }

    private static bool GmPlayerKeyCheckWalkLeft(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 262144) != 0)
        {
            if ((ply_work.key_on & 4) != 0 || ply_work.key_rot_z < 0)
                return true;
        }
        else if (((int)g_gs_main_sys_info.game_flag & 1) != 0)
        {
            if ((ply_work.key_on & 4) != 0)
                return true;
        }
        else if ((ply_work.key_on & 4) != 0 || ply_work.key_walk_rot_z < 0)
            return true;

        return false;
    }

    private static bool GmPlayerKeyCheckWalkRight(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 262144) != 0)
        {
            if ((ply_work.key_on & 8) != 0 || ply_work.key_rot_z > 0)
                return true;
        }
        else if (((int)g_gs_main_sys_info.game_flag & 1) != 0)
        {
            if ((ply_work.key_on & 8) != 0)
                return true;
        }
        else if ((ply_work.key_on & 8) != 0 || ply_work.key_walk_rot_z > 0)
            return true;

        return false;
    }

    private static bool GmPlayerKeyCheckJumpKeyOn(GMS_PLAYER_WORK ply_work)
    {
        return (ply_work.key_on & 160) != 0;
    }

    private static bool GmPlayerKeyCheckJumpKeyPush(GMS_PLAYER_WORK ply_work)
    {
        return (ply_work.key_push & 160) != 0;
    }

    private static int GmPlayerKeyGetGimmickRotZ(GMS_PLAYER_WORK ply_work)
    {
        return ((int)g_gs_main_sys_info.game_flag & 1) == 0 ? ply_work.key_rot_z : ply_work.key_walk_rot_z;
    }

    private static bool GmPlayerKeyCheckTransformKeyPush(GMS_PLAYER_WORK ply_work)
    {
        return (ply_work.key_push & 80) != 0;
    }

    private static void GmPlayerSetLight(NNS_VECTOR light_vec, ref NNS_RGBA light_col)
    {
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnNormalizeVector(nnsVector, light_vec);
        ObjDrawSetParallelLight(NNE_LIGHT_6, ref light_col, 1f, nnsVector);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    private static void GmPlayerSetDefRimParam(GMS_PLAYER_WORK ply_work)
    {
    }

    private void GmPlayerSetRimParam(GMS_PLAYER_WORK ply_work, NNS_RGB toon_rim_param)
    {
    }

    private static bool GmPlayerCheckGimmickEnable(GMS_PLAYER_WORK ply_work)
    {
        return ply_work.gmk_obj != null && ply_work.gmk_obj.obj_type == 3 &&
               ((GMS_ENEMY_COM_WORK)ply_work.gmk_obj).target_obj == (OBS_OBJECT_WORK)ply_work;
    }

    private static bool GmPlayerIsTransformSuperSonic(GMS_PLAYER_WORK ply_work)
    {
        return ((int)g_gm_main_system.game_flag & 1048576) == 0 && (ply_work.player_flag & 1049600) == 0 &&
               (!GSM_MAIN_STAGE_IS_SPSTAGE() && ((int)g_gs_main_sys_info.game_flag & 32) != 0) &&
               (ply_work.ring_num >= 50 && (ply_work.player_flag & 16384) == 0);
    }

    private static void GmPlayerCameraOffsetSet(
        GMS_PLAYER_WORK ply_work,
        short ofs_x,
        short ofs_y)
    {
        ply_work.gmk_camera_center_ofst_x = ofs_x;
        ply_work.gmk_camera_center_ofst_y = ofs_y;
    }

    private static bool GmPlayerIsStateWait(GMS_PLAYER_WORK ply_work)
    {
        bool flag = false;
        if (ply_work.act_state >= 2 && ply_work.act_state <= 7)
            flag = true;
        return flag;
    }

    private static bool gmPlayerObjRelease(OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d = null;
        return true;
    }

    private static bool gmPlayerObjReleaseWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = (GMS_PLAYER_WORK)obj_work;
        ObjAction3dNNMotionRelease(gmsPlayerWork.obj_3d_work[0]);
        for (int index = 1; index < 8; ++index)
            gmsPlayerWork.obj_3d_work[index].motion = null;
        return false;
    }

    private static void gmPlayerExit(MTS_TASK_TCB tcb)
    {
        GMS_PLAYER_WORK tcbWork = (GMS_PLAYER_WORK)mtTaskGetTcbWork(tcb);
        g_gm_main_system.ply_work[tcbWork.player_id] = null;
        ObjObjectExit(tcb);
    }

    private static void gmPlayerMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)obj_work;
        if (ply_work.spin_se_timer > 0)
            --ply_work.spin_se_timer;
        if (ply_work.spin_back_se_timer > 0)
            --ply_work.spin_back_se_timer;
        GmPlySeqMain(ply_work);
    }

    private static void gmPlayerDispFunc(OBS_OBJECT_WORK obj_work)
    {
        ushort num1 = 0;
        GMS_PLAYER_WORK gmsPlayerWork = (GMS_PLAYER_WORK)obj_work;
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        nnMakeUnitMatrix(obj3d.user_obj_mtx_r);
        if (((int)gmsPlayerWork.gmk_flag & 32768) != 0)
            nnMultiplyMatrix(obj3d.user_obj_mtx_r, obj3d.user_obj_mtx_r, gmsPlayerWork.ex_obj_mtx_r);
        float num2 = 0.0f;
        float num3 = -15f;
        if (((int)gmsPlayerWork.player_flag & 131072) != 0 &&
            (26 > gmsPlayerWork.act_state || gmsPlayerWork.act_state > 30) || GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            num3 = -21f;
        else if (((int)gmsPlayerWork.player_flag & 262144) != 0 && ((int)gmsPlayerWork.gmk_flag2 & 64) == 0)
        {
            num2 = 0.0f;
            num3 = 0.0f;
        }

        nnTranslateMatrix(obj3d.user_obj_mtx_r, obj3d.user_obj_mtx_r, 0.0f,
            num3 / FXM_FX32_TO_FLOAT(g_obj.draw_scale.y), num2 / FXM_FX32_TO_FLOAT(g_obj.draw_scale.x));
        if (((int)gmsPlayerWork.player_flag & -2147483376) != 0)
        {
            num1 = gmsPlayerWork.obj_work.dir.y;
            gmsPlayerWork.obj_work.dir.y += gmsPlayerWork.pgm_turn_dir;
        }

        ObjDrawActionSummary(obj_work);
        if (((int)gmsPlayerWork.player_flag & -2147483376) == 0)
            return;
        gmsPlayerWork.obj_work.dir.y = num1;
    }

    private static void gmPlayerDefaultInFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)obj_work;
        gmPlayerKeyGet(ply_work);
        gmPlayerWaterCheck(ply_work);
        gmPlayerTimeOverCheck(ply_work);
        gmPlayerFallDownCheck(ply_work);
        gmPlayerPressureCheck(ply_work);
        gmPlayerGetHomingTarget(ply_work);
        gmPlayerSuperSonicCheck(ply_work);
        gmPlayerPushSet(ply_work);
        gmPlayerEarthTouch(ply_work);
        if ((ply_work.player_flag & 262144) != 0)
        {
            ply_work.truck_prev_dir = ply_work.obj_work.dir.z;
            ply_work.truck_prev_dir_fall = ply_work.obj_work.dir_fall;
        }

        if (ply_work.gmk_obj != null && ((int)ply_work.gmk_obj.flag & 4) != 0)
            ply_work.gmk_obj = null;
        if (ply_work.invincible_timer != 0)
        {
            ply_work.invincible_timer = ObjTimeCountDown(ply_work.invincible_timer);
            if ((ply_work.invincible_timer & 16384) != 0)
                ply_work.obj_work.disp_flag |= 32U;
            else
                ply_work.obj_work.disp_flag &= 4294967263U;
            if (ply_work.invincible_timer == 0)
            {
                ply_work.obj_work.disp_flag &= 4294967263U;
                GmPlayerSetDefNormal(ply_work);
            }
        }

        if (ply_work.disapprove_item_catch_timer != 0)
            ply_work.disapprove_item_catch_timer = ObjTimeCountDown(ply_work.disapprove_item_catch_timer);
        if (ply_work.genocide_timer != 0)
            ply_work.genocide_timer = ObjTimeCountDown(ply_work.genocide_timer);
        if (ply_work.genocide_timer != 0 || (ply_work.player_flag & 16384) != 0)
        {
            ply_work.water_timer = 0;
            OBS_RECT_WORK obsRectWork = ply_work.rect_work[2];
            obsRectWork.hit_flag |= 2;
            obsRectWork.hit_power = 3;
            ply_work.rect_work[0].def_flag = ushort.MaxValue;
        }
        else if ((ply_work.rect_work[2].hit_flag & 2) != 0)
        {
            ply_work.obj_work.disp_flag &= 4294967263U;
            OBS_RECT_WORK obsRectWork = ply_work.rect_work[2];
            ushort num = 65533;
            obsRectWork.hit_flag &= num;
            obsRectWork.hit_power = 1;
            ply_work.rect_work[0].def_flag = 65533;
            GmSoundStopJingleInvincible();
        }

        if (ply_work.hi_speed_timer != 0)
        {
            ply_work.hi_speed_timer = ObjTimeCountDown(ply_work.hi_speed_timer);
            if (ply_work.hi_speed_timer == 0)
                GmPlayerSpdParameterSet(ply_work);
        }

        if (ply_work.homing_timer != 0)
            ply_work.homing_timer = ObjTimeCountDown(ply_work.homing_timer);
        if ((ply_work.player_flag & 262144) == 0)
            return;
        ply_work.obj_work.sys_flag &= 4294967055U;
        if (((int)ply_work.obj_work.sys_flag & 1) != 0)
            ply_work.obj_work.sys_flag |= 16U;
        if (((int)ply_work.obj_work.sys_flag & 2) != 0)
            ply_work.obj_work.sys_flag |= 32U;
        if (((int)ply_work.obj_work.sys_flag & 4) != 0)
            ply_work.obj_work.sys_flag |= 64U;
        if (((int)ply_work.obj_work.sys_flag & 8) != 0)
            ply_work.obj_work.sys_flag |= 128U;
        ply_work.obj_work.sys_flag &= 4294967280U;
        ply_work.gmk_flag2 &= 4294967279U;
        if (((int)ply_work.gmk_flag2 & 8) != 0)
            ply_work.gmk_flag2 |= 16U;
        ply_work.gmk_flag2 &= 4294967287U;
        if (ply_work.jump_pseudofall_eve_id_set == 0)
            ply_work.jump_pseudofall_eve_id_cur = ply_work.jump_pseudofall_eve_id_wait;
        ply_work.jump_pseudofall_eve_id_set = 0;
        ply_work.jump_pseudofall_eve_id_wait = 0;
        if ((((int)ply_work.gmk_flag2 & 256) == 0 || ((int)ply_work.obj_work.move_flag & 1) != 0) &&
            ply_work.seq_state != GME_PLY_SEQ_STATE_GMK_DEMO_FW)
        {
            int num1 = -ply_work.key_rot_z;
            int num2;
            if (((int)g_gs_main_sys_info.game_flag & 512) != 0)
            {
                num2 = num1 * 38 / 10;
                if (fall_rot_buf_gmPlayerDefaultInFunc > 0 && num2 >= 0 ||
                    fall_rot_buf_gmPlayerDefaultInFunc < 0 && num2 <= 0)
                    num2 += fall_rot_buf_gmPlayerDefaultInFunc;
                fall_rot_buf_gmPlayerDefaultInFunc = 0;
                if (num2 > 5120)
                {
                    fall_rot_buf_gmPlayerDefaultInFunc += num2 - 5120;
                    if (fall_rot_buf_gmPlayerDefaultInFunc > 49152)
                        fall_rot_buf_gmPlayerDefaultInFunc = 49152;
                    num2 = 5120;
                }
                else if (num2 < -5120)
                {
                    fall_rot_buf_gmPlayerDefaultInFunc += num2 + 5120;
                    if (fall_rot_buf_gmPlayerDefaultInFunc < -49152)
                        fall_rot_buf_gmPlayerDefaultInFunc = -49152;
                    num2 = -5120;
                }
            }
            else
                num2 = num1 / 24;

            ushort num3 = (ushort)(ply_work.obj_work.dir.z +
                                    (ply_work.obj_work.dir_fall - (uint)g_gm_main_system.pseudofall_dir));
            int num4 = 60075;
            int num5 = 5460;
            if (num3 > num5 && num3 < num4)
            {
                if (num3 > 32768 && num2 > 0)
                    num2 = 0;
                else if (num3 <= 32768 && num2 <= 0)
                    num2 = 0;
            }

            if (((int)ply_work.gmk_flag & 262144) != 0 && ((int)ply_work.gmk_flag & 1073741824) == 0)
                num2 = 0;
            if (((int)g_gm_main_system.game_flag & 16384) != 0 || (ply_work.player_flag & 1048576) != 0)
                num2 = (short)num3 >= 0 ? ((short)num3 <= 0 ? 0 : num3) : num3;
            int num6 = num2;
            if (num6 > 32768)
                num6 -= 65536;
            else if (num6 < short.MinValue)
                num6 += 65536;
            int a = num6;
            if (MTM_MATH_ABS(a) > 5120)
                a = a < 0 ? -5120 : 5120;
            ply_work.ply_pseudofall_dir += a;
            g_gm_main_system.pseudofall_dir = (ushort)ply_work.ply_pseudofall_dir;
        }

        ply_work.prev_dir_fall = obj_work.dir_fall;
        obj_work.dir_fall = ((int)obj_work.move_flag & 1) == 0
            ? (ushort)(ply_work.jump_pseudofall_dir + 8192 & 49152)
            : (ushort)(g_gm_main_system.pseudofall_dir + 8192 & 49152);
        if (ply_work.prev_dir_fall != obj_work.dir_fall)
            ply_work.obj_work.dir.z -= (ushort)(obj_work.dir_fall - (uint)ply_work.prev_dir_fall);
        int num7 = 60075;
        int num8 = 5460;
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
        {
            ushort num1 = (ushort)(ply_work.obj_work.dir.z +
                                    (ply_work.obj_work.dir_fall - (uint)g_gm_main_system.pseudofall_dir));
            if (num1 > num8 && num1 < num7)
            {
                if (num1 > 32768)
                    ply_work.ply_pseudofall_dir -= num7 - num1;
                else if (num1 <= 32768)
                    ply_work.ply_pseudofall_dir += num1 - num8;
            }
        }

        ushort num9 = (ushort)(ply_work.obj_work.dir.z +
                                (ply_work.obj_work.dir_fall - (uint)g_gm_main_system.pseudofall_dir));
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
        {
            if (((int)ply_work.gmk_flag2 & 24) == 0 && 27392 < num9 && num9 < 38144)
            {
                if (ply_work.truck_prev_dir == obj_work.dir.z)
                {
                    ply_work.obj_work.spd_m = 0;
                    if (((int)ply_work.gmk_flag & 262144) == 0)
                    {
                        ply_work.gmk_flag |= 262144U;
                        GmPlySeqGmkInitTruckDanger(ply_work, ply_work.truck_obj);
                        GmPlayerSpdParameterSet(ply_work);
                    }
                }
            }
            else if (((int)ply_work.gmk_flag & 262144) == 0)
                ply_work.truck_stick_prev_dir = num9;
        }

        if (((int)ply_work.gmk_flag & 1074003968) == 1074003968 &&
            (((int)ply_work.obj_work.move_flag & 1) == 0 || 27392 >= num9 || num9 >= 38144))
        {
            ply_work.player_flag |= 1U;
            GmPlayerSpdParameterSet(ply_work);
        }

        if (((int)ply_work.gmk_flag & int.MinValue) == 0)
            return;
        ply_work.gmk_flag &= 1073479679U;
        ply_work.obj_work.vib_timer = 0;
        GmPlayerSetDefNormal(ply_work);
        GmPlayerSpdParameterSet(ply_work);
    }

    private static void gmPlayerSplStgInFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)obj_work;
        gmPlayerKeyGet(ply_work);
        gmPlayerTimeOverCheck(ply_work);
        gmPlayerEarthTouch(ply_work);
        if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
        {
            OBS_CAMERA obsCamera = ObjCameraGet(g_obj.glb_camera_id);
            g_gm_main_system.pseudofall_dir = (ushort)-obsCamera.roll;
            ply_work.prev_dir_fall2 = ply_work.prev_dir_fall;
            ply_work.prev_dir_fall = obj_work.dir_fall;
            obj_work.dir_fall = (ushort)(g_gm_main_system.pseudofall_dir + 8192 & 49152);
            ply_work.jump_pseudofall_dir = g_gm_main_system.pseudofall_dir;
        }

        if (ply_work.gmk_obj == null || ((int)ply_work.gmk_obj.flag & 4) == 0)
            return;
        ply_work.gmk_obj = null;
    }

    private static void gmPlayerRectTruckFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)obj_work;
        if ((((int)obj_work.move_flag & 1) != 0 && MTM_MATH_ABS(obj_work.spd_m) > 256 ||
             ((int)obj_work.move_flag & 1) == 0) &&
            ((ply_work.player_flag & GMD_PLF_DIE) == 0 && ply_work.seq_state != GME_PLY_SEQ_STATE_DAMAGE))
            GmPlayerSetAtk(ply_work);
        else
            ply_work.rect_work[1].flag &= 4294967291U;
    }

    private static void gmPlayerDefaultLastFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)obj_work;
        gmPlayerCameraOffset(ply_work);
        if (ply_work.gmk_obj == null || ((int)ply_work.gmk_flag & 8) != 0)
            obj_work.move_flag &= 4294950911U;
        if ((ply_work.player_flag & 32768) == 0 || obj_work.pos.x >> 12 <= g_gm_main_system.map_fcol.left + 128)
            return;
        obj_work.move_flag |= 16384U;
    }

    private static void gmPlayerTruckCollisionFunc(OBS_OBJECT_WORK obj_work)
    {
        bool flag = false;
        uint num1 = 0;
        int v1 = 0;
        VecFx32 vecFx32_1 = new VecFx32();
        VecFx32 vecFx32_2 = new VecFx32();
        VecFx32 vecFx32_3 = new VecFx32();
        ushort num2 = 0;
        ushort num3 = 0;
        if (((int)obj_work.move_flag & 1) != 0 && ((int)obj_work.move_flag & 16) == 0 &&
            MTM_MATH_ABS(obj_work.spd_m) >= 16384)
        {
            flag = true;
            num1 = obj_work.move_flag;
            v1 = obj_work.spd_m;
            vecFx32_1.Assign(obj_work.spd);
            vecFx32_2.Assign(obj_work.pos);
            vecFx32_3.Assign(obj_work.move);
            num2 = obj_work.dir.z;
            num3 = obj_work.dir_fall;
        }

        if (g_obj.ppCollision != null)
            g_obj.ppCollision(obj_work);
        if (flag && ((int)obj_work.move_flag & 1) != 0)
        {
            ushort num4 = (ushort)(obj_work.dir.z - (uint)num2);
            if (v1 > 0)
            {
                if (4096 > num4 || num4 > 16384)
                    return;
            }
            else if (61440 < num4 || num4 < 32768)
                return;

            obj_work.move_flag = num1;
            obj_work.spd_m = FX_Mul(v1, 4096);
            obj_work.spd.Assign(vecFx32_1);
            obj_work.pos.Assign(vecFx32_2);
            obj_work.move.Assign(vecFx32_3);
            obj_work.dir.z = num2;
            obj_work.dir_fall = num3;
            obj_work.move_flag = num1 & 4294967294U;
            ((GMS_PLAYER_WORK)obj_work).gmk_flag2 |= 1U;
        }

        if (((int)obj_work.move_flag & 4194305) != 4194305)
            return;
        if (obj_work.dir_fall != ((GMS_PLAYER_WORK)obj_work).truck_prev_dir_fall)
        {
            ((GMS_PLAYER_WORK)obj_work).truck_prev_dir = (ushort)(((GMS_PLAYER_WORK)obj_work).truck_prev_dir +
                (uint)((GMS_PLAYER_WORK)obj_work).truck_prev_dir_fall - obj_work.dir_fall);
            ((GMS_PLAYER_WORK)obj_work).truck_prev_dir_fall = obj_work.dir_fall;
        }

        if (MTM_MATH_ABS(((GMS_PLAYER_WORK)obj_work).truck_prev_dir - obj_work.dir.z) <= 1024)
            return;
        obj_work.dir.z = ObjRoopMove16(((GMS_PLAYER_WORK)obj_work).truck_prev_dir, obj_work.dir.z, 1024);
    }

    private static void gmPlayerAtkFunc(
        OBS_RECT_WORK mine_rect,
        OBS_RECT_WORK match_rect)
    {
    }

    private static void gmPlayerDefFunc(
        OBS_RECT_WORK mine_rect,
        OBS_RECT_WORK match_rect)
    {
        if (gs.backup.SSave.CreateInstance().GetDebug().GodMode)
            return;

        GMS_PLAYER_WORK parentObj1 = (GMS_PLAYER_WORK)mine_rect.parent_obj;
        HgTrophyIncPlayerDamageCount(parentObj1);
        if (((int)parentObj1.obj_work.move_flag & 32768) != 0)
        {
            int x = parentObj1.obj_work.spd.x;
        }
        else
        {
            int spdM = parentObj1.obj_work.spd_m;
        }

        if (match_rect.parent_obj.obj_type == 3)
        {
            GMS_ENEMY_COM_WORK parentObj2 = (GMS_ENEMY_COM_WORK)match_rect.parent_obj;
            if (91 <= parentObj2.eve_rec.id && parentObj2.eve_rec.id <= 94 ||
                (parentObj2.eve_rec.id == 97 || parentObj2.eve_rec.id == 98))
                GmSoundPlaySE("Damage2");
        }

        if (((int)parentObj1.player_flag & 268435456) == 0 && parentObj1.ring_num == 0)
        {
            GmPlySeqChangeDeath(parentObj1);
        }
        else
        {
            int gmkFlag = (int)parentObj1.gmk_flag;
            GmPlayerStateInit(parentObj1);
            if (((int)parentObj1.player_flag & 268435456) == 0)
            {
                if (parentObj1.ring_num != 0)
                    GmSoundPlaySE("Ring2");
                GmRingDamageSet(parentObj1);
                GmComEfctCreateHitEnemy(parentObj1.obj_work, (mine_rect.rect.left + mine_rect.rect.right) * 4096 / 2,
                    (mine_rect.rect.top + mine_rect.rect.bottom) * 4096 / 2);
            }

            if (((int)parentObj1.player_flag & 805306368) != 0)
                GmSoundPlaySE("Damage1");
            parentObj1.player_flag &= 3489660927U;
            parentObj1.invincible_timer = parentObj1.time_damage;
            GmPlySeqChangeDamage(parentObj1);
        }
    }

    private static void gmPlayerPushSet(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.seq_state == GME_PLY_SEQ_STATE_WALLPUSH && ply_work.act_state == 18)
            ply_work.obj_work.move_flag |= 16777216U;
        else
            ply_work.obj_work.move_flag &= 4278190079U;
    }

    private static void gmPlayerEarthTouch(GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.gmk_flag & 1) == 0)
            return;
        if (((int)ply_work.obj_work.move_flag & 15) == 0)
            return;
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
            GmPlySeqLandingSet(ply_work, 0);
        else if (((int)ply_work.obj_work.move_flag & 2) != 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                GmPlySeqLandingSet(ply_work, 24576);
            else
                GmPlySeqLandingSet(ply_work, 40960);
            OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
            pData.pos_x = ply_work.obj_work.pos.x >> 12;
            pData.pos_y = (ply_work.obj_work.pos.y >> 12) + ply_work.obj_work.field_rect[1] - 4;
            pData.flag = (ushort)(ply_work.obj_work.flag & 1U);
            pData.vec = 3;
            ushort[] numArray = new ushort[1];
            pData.dir = numArray;
            pData.attr = null;
            ushort z = ply_work.obj_work.dir.z;
            numArray[0] = z;
            ObjDiffCollisionFast(pData);
            ushort num = numArray[0];
            ply_work.obj_work.dir.z = num;
            GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
        }
        else if (((int)ply_work.obj_work.move_flag & 4) != 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                GmPlySeqLandingSet(ply_work, 16384);
            else
                GmPlySeqLandingSet(ply_work, 49152);
        }
        else if (((int)ply_work.obj_work.move_flag & 8) != 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                GmPlySeqLandingSet(ply_work, 49152);
            else
                GmPlySeqLandingSet(ply_work, 16384);
        }

        if (((int)ply_work.gmk_flag & 2048) != 0)
        {
            if (ply_work.obj_work.dir.z < 32768)
            {
                ply_work.obj_work.disp_flag |= 1U;
                ply_work.obj_work.spd_m = -MTM_MATH_ABS(ply_work.obj_work.spd_m);
            }
            else
            {
                ply_work.obj_work.disp_flag &= 4294967294U;
                ply_work.obj_work.spd_m = MTM_MATH_ABS(ply_work.obj_work.spd_m);
            }
        }
        else
        {
            if (((int)ply_work.gmk_flag & 33554432) != 0)
                ply_work.obj_work.disp_flag ^= 1U;
            if (((int)ply_work.gmk_flag & 2) != 0)
            {
                ply_work.obj_work.disp_flag ^= 1U;
                ply_work.obj_work.spd_m = -ply_work.obj_work.spd_m;
            }
        }

        ply_work.obj_work.move_flag |= 1U;
        ply_work.gmk_flag &= 4261410812U;
        GmPlySeqChangeFw(ply_work);
    }

    private static void gmPlayerWaterCheck(GMS_PLAYER_WORK ply_work)
    {
        if (GmMainIsWaterLevel())
        {
            if ((ply_work.obj_work.pos.y >> 12) - -10 >= g_gm_main_system.water_level)
            {
                bool flag = false;
                if ((ply_work.player_flag & 67108864) == 0)
                {
                    if (((int)g_gm_main_system.game_flag & 8192) == 0)
                    {
                        GmPlyEfctCreateSpray(ply_work);
                        GmPlyEfctCreateBubble(ply_work);
                        GmSoundPlaySE("Spray");
                    }

                    GmPlayerSpdParameterSetWater(ply_work, true);
                }

                ply_work.player_flag |= 67108864U;
                if ((ply_work.player_flag & 16778240) == 0)
                {
                    if ((ply_work.obj_work.pos.y >> 12) - 4 >= g_gm_main_system.water_level)
                    {
                        ply_work.water_timer = ObjTimeCountUp(ply_work.water_timer);
                    }
                    else
                    {
                        ply_work.water_timer = 0;
                        GmPlyEfctCreateRunSpray(ply_work);
                        flag = true;
                    }
                }

                if ((ply_work.player_flag & 16778240) != 0)
                    return;
                if ((ply_work.water_timer >> 12) % 50 == 0 && ply_work.water_timer < ply_work.time_air)
                    GmPlyEfctCreateBubble(ply_work);
                if (!flag && (ply_work.water_timer >> 12) % 300 == 0)
                {
                    if (((int)ply_work.gmk_flag & 524288) == 0)
                        GmSoundPlaySE("Attention");
                    ply_work.gmk_flag |= 524288U;
                }
                else
                    ply_work.gmk_flag &= 4294443007U;

                int num = ply_work.time_air - ply_work.water_timer;
                if (num >= 245760 && num - 245760 <= 2457600 && (num - 245760 >> 12) % 120 == 0)
                {
                    uint no = MTM_MATH_CLIP((uint)((num - 245760 >> 12) / 120), 0U, 5U);
                    GmPlyEfctWaterCount(ply_work, no);
                }

                if (num == 2826240)
                    GmSoundPlayJingleObore();
                else if (num > 2826240)
                    GmSoundStopJingleObore();
                if (ply_work.water_timer <= ply_work.time_air)
                    return;
                GmPlySeqChangeDeath(ply_work);
                ply_work.obj_work.spd.y = 0;
                GmPlyEfctWaterDeath(ply_work);
            }
            else
            {
                if ((ply_work.player_flag & 67108864) != 0)
                {
                    if (((int)g_gm_main_system.game_flag & 8192) == 0)
                    {
                        GmPlyEfctCreateSpray(ply_work);
                        GmSoundPlaySE("Spray");
                    }

                    GmSoundStopJingleObore();
                    GmPlayerSpdParameterSetWater(ply_work, false);
                }

                ply_work.player_flag &= 4227858431U;
                ply_work.water_timer = 0;
                ply_work.obj_work.spd_fall = g_gm_player_parameter[ply_work.char_id].spd_fall;
                ply_work.obj_work.spd_fall_max = g_gm_player_parameter[ply_work.char_id].spd_fall_max;
            }
        }
        else
        {
            if ((ply_work.player_flag & 67108864) != 0)
            {
                ply_work.obj_work.spd_fall = g_gm_player_parameter[ply_work.char_id].spd_fall;
                ply_work.obj_work.spd_fall_max = g_gm_player_parameter[ply_work.char_id].spd_fall_max;
            }

            ply_work.player_flag &= 4227858431U;
            ply_work.water_timer = 0;
            ply_work.gmk_flag &= 4294443007U;
        }
    }

    private static void gmPlayerTimeOverCheck(GMS_PLAYER_WORK ply_work)
    {
        if (((int)g_gm_main_system.game_flag & 1327676) != 0 || (ply_work.player_flag & 65536) != 0)
            return;
        if (!GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            if ((ply_work.player_flag & 1024) != 0 || g_gm_main_system.game_time < 35999U)
                return;
            GmPlySeqChangeDeath(ply_work);
            g_gm_main_system.game_flag |= 512U;
            g_gm_main_system.time_save = 0U;
            g_gs_main_sys_info.game_flag |= 8U;
        }
        else
        {
            if ((int)g_gm_main_system.game_time > 0)
                return;
            g_gm_main_system.game_flag |= 262144U;
            g_gm_main_system.time_save = 0U;
            g_gs_main_sys_info.game_flag |= 8U;
            ply_work.obj_work.move_flag |= 8448U;
        }
    }

    private static void gmPlayerFallDownCheck(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 1024) != 0 ||
            g_gm_main_system.map_size[1] - 16 << 12 > ply_work.obj_work.pos.y)
            return;
        GmPlySeqChangeDeath(ply_work);
    }

    private static void gmPlayerPressureCheck(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 2048) != 0 && ply_work.obj_work.touch_obj == null &&
            (((int)ply_work.obj_work.move_flag & 4) != 0 && ((int)ply_work.obj_work.move_flag & 8) != 0))
            GmPlySeqChangeDeath(ply_work);
        if (ply_work.obj_work.touch_obj == null || (ply_work.player_flag & 4096) != 0 ||
            ply_work.obj_work.touch_obj.obj_type == 3 && (ply_work.obj_work.touch_obj.obj_type != 3 ||
                                                          ((int)((GMS_ENEMY_COM_WORK)ply_work.obj_work.touch_obj)
                                                              .enemy_flag & 16384) != 0) ||
            (ply_work.obj_work.ride_obj == null && ((int)ply_work.obj_work.move_flag & 1) != 0 &&
             (((int)ply_work.obj_work.move_flag & 2) != 0 && ply_work.obj_work.touch_obj.move.y <= 0) ||
             (((int)ply_work.obj_work.move_flag & 1) == 0 || ((int)ply_work.obj_work.move_flag & 2) == 0) &&
             (((int)ply_work.obj_work.move_flag & 4) == 0 || ((int)ply_work.obj_work.move_flag & 8) == 0)))
            return;
        GmPlySeqChangeDeath(ply_work);
    }

    private static void gmPlayerSuperSonicCheck(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 16384) == 0)
            return;
        ply_work.super_sonic_ring_timer = ObjTimeCountDown(ply_work.super_sonic_ring_timer);
        if (ply_work.super_sonic_ring_timer != 0)
            return;
        --ply_work.ring_num;
        if (ply_work.ring_num <= 0)
        {
            ply_work.ring_num = 0;
            gmPlayerSuperSonicToSonic(ply_work);
        }
        else
            ply_work.super_sonic_ring_timer = 245760;
    }

    private static void gmPlayerSuperSonicToSonic(GMS_PLAYER_WORK ply_work)
    {
        GMS_PLAYER_RESET_ACT_WORK reset_act_work = new GMS_PLAYER_RESET_ACT_WORK();
        GmPlayerSaveResetAction(ply_work, reset_act_work);
        GmPlayerSetEndSuperSonic(ply_work);
        if ((((int)ply_work.obj_work.move_flag & 1) == 0 || ((int)ply_work.obj_work.move_flag & 16) != 0) &&
            ply_work.act_state == 21 || ply_work.act_state == 22)
        {
            GmPlayerActionChange(ply_work, 42);
            ply_work.obj_work.disp_flag |= 4U;
        }
        else
            GmPlayerResetAction(ply_work, reset_act_work);
    }

    private static void gmPlayerGetHomingTarget(GMS_PLAYER_WORK ply_work)
    {
        float num1 = FXM_FX32_TO_FLOAT(786432) * FXM_FX32_TO_FLOAT(786432);
        float num2 = 1.5f;
        if (ply_work.homing_boost_timer != 0)
        {
            num2 = 1f;
            ply_work.homing_boost_timer = ObjTimeCountDown(ply_work.homing_boost_timer);
        }

        if (ply_work.enemy_obj != null && ((int)ply_work.enemy_obj.flag & 4) != 0)
            ply_work.enemy_obj = null;
        OBS_RECT_WORK obsRectWork1 = ply_work.rect_work[2];
        float num3 = FXM_FX32_TO_FLOAT(ply_work.obj_work.pos.x);
        float num4 =
            FXM_FX32_TO_FLOAT(ply_work.obj_work.pos.y + (obsRectWork1.rect.top + obsRectWork1.rect.bottom >> 1));
        int a;
        int b;
        if (((int)ply_work.obj_work.disp_flag & 1) != 0)
        {
            a = 32768;
            b = 18205;
        }
        else
        {
            a = 0;
            b = 14563;
        }

        if (b < a)
            MTM_MATH_SWAP(ref a, ref b);
        OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, ushort.MaxValue);
        OBS_OBJECT_WORK obsObjectWork = null;
        for (; obj_work != null; obj_work = ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
        {
            if (((int)obj_work.disp_flag & 32) == 0)
            {
                GMS_ENEMY_COM_WORK gmsEnemyComWork;
                if (obj_work.obj_type == 3)
                {
                    gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
                    if (((int)gmsEnemyComWork.enemy_flag & 32768) != 0 ||
                        (63 > gmsEnemyComWork.eve_rec.id || gmsEnemyComWork.eve_rec.id > 67 ||
                         gmsEnemyComWork.eve_rec.byte_param[1] != 0) &&
                        ((70 > gmsEnemyComWork.eve_rec.id || gmsEnemyComWork.eve_rec.id > 79) &&
                         (100 > gmsEnemyComWork.eve_rec.id || gmsEnemyComWork.eve_rec.id > 101)) &&
                        (130 != gmsEnemyComWork.eve_rec.id &&
                         (112 > gmsEnemyComWork.eve_rec.id || gmsEnemyComWork.eve_rec.id > 114) &&
                         (163 != gmsEnemyComWork.eve_rec.id && 86 != gmsEnemyComWork.eve_rec.id &&
                          (161 != gmsEnemyComWork.eve_rec.id && 247 != gmsEnemyComWork.eve_rec.id))))
                        continue;
                }
                else if (obj_work.obj_type == 2)
                {
                    gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
                    if (((int)gmsEnemyComWork.enemy_flag & 32768) != 0)
                        continue;
                }
                else
                    continue;

                OBS_RECT_WORK obsRectWork2 = gmsEnemyComWork.rect_work[2];
                float num5 = FXM_FX32_TO_FLOAT(obj_work.pos.x);
                float num6 = FXM_FX32_TO_FLOAT(obj_work.pos.y + (obsRectWork2.rect.top + obsRectWork2.rect.bottom >> 1));
                float num7 = num5 - num3;
                float num8 = num6 - num4;
                int num9 = nnArcTan2(num8, num7);
                if (num9 >= a && num9 <= b)
                {
                    float num10 = num8 * num2;
                    float num11 = (float)(num7 * (double)num7 + num10 * (double)num10);
                    if (num11 < (double)num1)
                    {
                        num1 = num11;
                        obsObjectWork = obj_work;
                    }
                }
            }
        }

        ply_work.enemy_obj = obsObjectWork;
        if (ply_work.cursol_enemy_obj == ply_work.enemy_obj && GmPlySeqCheckAcceptHoming(ply_work))
            return;
        ply_work.cursol_enemy_obj = null;
    }

    private static void gmPlayerKeyGet(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.no_key_timer != 0 || (ply_work.player_flag & 4194304) != 0)
        {
            ply_work.no_key_timer = ObjTimeCountDown(ply_work.no_key_timer);
            ply_work.key_on = 0;
            ply_work.key_push = 0;
            ply_work.key_repeat = 0;
            ply_work.key_release = 0;
            ply_work.key_rot_z = 0;
            ply_work.key_walk_rot_z = 0;
        }
        else
        {
            if ((ply_work.player_flag & 4194304) == 0 && ply_work.player_id == 0)
            {
                int rotZ = gmPlayerKeyGetRotZ(ply_work);
                ply_work.prev_key_rot_z = ply_work.key_rot_z;
                ply_work.key_rot_z = rotZ;
                ushort num1 = gmPlayerRemapKey(ply_work, 0, rotZ);
                ushort num2 = (ushort)(ply_work.key_on ^ (uint)num1);
                ply_work.key_push = (ushort)(num2 & (uint)num1);
                ply_work.key_release = (ushort)(num2 & (uint)~num1);
                ply_work.key_on = num1;
                ply_work.key_repeat = 0;
                for (int index = 0; index < 8; ++index)
                {
                    if ((ply_work.key_on & (int)gm_key_map_key_list[index]) == 0)
                        ply_work.key_repeat_timer[index] = 30;
                    else if (--ply_work.key_repeat_timer[index] == 0)
                    {
                        ply_work.key_repeat |= (ushort)(ply_work.key_on & (uint)(ushort)gm_key_map_key_list[index]);
                        ply_work.key_repeat_timer[index] = 5;
                    }
                }

                ply_work.key_walk_rot_z = 0;
                if (((int)g_gs_main_sys_info.game_flag & 1) != 0)
                {
                    if ((ply_work.key_on & 8) != 0)
                        ply_work.key_walk_rot_z = short.MaxValue;
                    else if ((ply_work.key_on & 4) != 0)
                        ply_work.key_walk_rot_z = -32767;
                }
                else
                    ply_work.key_walk_rot_z = rotZ;
            }

            if (!GMM_MAIN_STAGE_IS_ENDING())
                return;
            GmEndingPlyKeyCustom(ply_work);
        }
    }

    private static ushort gmPlayerRemapKey(
        GMS_PLAYER_WORK ply_work,
        ushort key,
        int key_rot_z)
    {
        ushort num1 = (ushort)(key & 4294967056U);
        if (GSM_MAIN_STAGE_IS_SPSTAGE() || g_gs_main_sys_info.stage_id == 9)
        {
            if (((int)g_gs_main_sys_info.game_flag & 512) != 0)
            {
                int focusTpIndex = CPadPolarHandle.CreateInstance().GetFocusTpIndex();
                if (gmPlayerIsInputDPadJumpKey(ply_work, focusTpIndex))
                    num1 |= ply_work.key_map[4];
                if (gmPlayerIsInputDPadSSonicKey(ply_work, focusTpIndex))
                    num1 |= ply_work.key_map[6];
            }
            else
                num1 |= gmPlayerRemapKeyIPhoneZone32SS(ply_work);
        }
        else if (((int)g_gs_main_sys_info.game_flag & 1) != 0)
        {
            CPadVirtualPad cpadVirtualPad = CPadVirtualPad.CreateInstance();
            int num2 = cpadVirtualPad.IsValid() ? cpadVirtualPad.GetValue() : (int)AoPad.AoPadMDirect();

            if ((8 & num2) != 0)
                num1 |= ply_work.key_map[3];
            else if ((4 & num2) != 0)
                num1 |= ply_work.key_map[2];
            else if ((1 & num2) != 0)
                num1 |= ply_work.key_map[0];
            else if ((2 & num2) != 0)
                num1 |= ply_work.key_map[1];
            if (gmPlayerIsInputDPadJumpKey(ply_work, -1))
                num1 |= ply_work.key_map[4];
            if (gmPlayerIsInputDPadSSonicKey(ply_work, -1))
                num1 |= ply_work.key_map[6];
        }
        else
        {
            if (key_rot_z > 1024)
                num1 |= 8;
            else if (key_rot_z < -1024)
                num1 |= 4;
            num1 |= gmPlayerRemapKeyIPhone(ply_work);
        }

        if ((32 & key) != 0)
            num1 |= ply_work.key_map[4];
        if ((128 & key) != 0)
            num1 |= ply_work.key_map[5];
        if ((64 & key) != 0)
            num1 |= ply_work.key_map[6];
        if ((16 & key) != 0)
            num1 |= ply_work.key_map[7];
        return num1;
    }

    private static int gmPlayerKeyGetRotZ(GMS_PLAYER_WORK ply_work)
    {
        ply_work.is_nudge = false;
        int stageId = g_gs_main_sys_info.stage_id;
        if (GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            NNS_VECTOR core = _am_iphone_accel_data.core;
            NNS_VECTOR calcAccel = ply_work.calc_accel;
            calcAccel.x = (float)(core.x * 0.100000001490116 + calcAccel.x * 0.899999976158142);
            calcAccel.y = (float)(core.y * 0.100000001490116 + calcAccel.y * 0.899999976158142);
            calcAccel.y = (float)(core.y * 0.100000001490116 + calcAccel.y * 0.899999976158142);
            float num1 = core.x - calcAccel.x;
            float num2 = core.y - calcAccel.y;
            float num3 = core.z - calcAccel.z;
            if (nnSqrt((float)(num1 * (double)num1 + num2 * (double)num2 + num3 * (double)num3)) >= 2.0)
                ply_work.is_nudge = true;
        }

        var analog = AoPad.AoPadAnalogLX() / 256;
        if (analog > 2 || analog < -2)
            return analog;

        if ((AoPad.AoPadMDirect() & ControllerConsts.LEFT) != 0)
        {
            return -128;
        }

        if ((AoPad.AoPadMDirect() & ControllerConsts.RIGHT) != 0)
        {
            return 128;
        }

        int retVal;
        if ((GSM_MAIN_STAGE_IS_SPSTAGE() || stageId == 9) && ((int)g_gs_main_sys_info.game_flag & 512) != 0)
        {
            return g_gm_main_system.polar_diff;
        }

        int accelRaw = (int)(_am_iphone_accel_data.sensor.x * 16384.0);
        if (accelRaw < 2048 && accelRaw > -2048)
        {
            retVal = 0;
        }
        else
        {
            retVal = Math.Min(Math.Max(accelRaw * 3, short.MinValue), short.MaxValue);
        }

        return retVal;
    }

    private static ushort gmPlayerRemapKeyIPhone(GMS_PLAYER_WORK ply_work)
    {
        ushort num1 = 0;
        if (GmMainKeyCheckPauseKeyPush() != -1)
            return num1;
        int seqState = ply_work.seq_state;
        if (ply_work.safe_timer > 0)
        {
            --ply_work.safe_timer;
            if (amTpIsTouchPull(0))
            {
                ply_work.safe_timer = 0;
                ply_work.safe_jump_timer = 10;
            }
            else if (ply_work.safe_timer == 0)
                ply_work.safe_spin_timer = 3;
        }
        else if (ply_work.safe_jump_timer > 0)
        {
            --ply_work.safe_jump_timer;
            num1 |= 32;
        }
        else if (ply_work.safe_spin_timer != 0)
        {
            uint num2 = ply_work.obj_work.disp_flag & 1U;
            if (amTpIsTouchPull(0))
                ply_work.safe_spin_timer = 0;
            switch (ply_work.safe_spin_timer)
            {
                case 1:
                    num1 |= 2;
                    if (!amTpIsTouchPull(0) && (seqState == 6 || seqState == 7 || seqState == 8))
                    {
                        num1 |= 32;
                        break;
                    }

                    break;
                case 2:
                    if (seqState != 2)
                        ply_work.safe_spin_timer = 1;
                    num1 |= 2;
                    break;
                case 3:
                    ushort num3;
                    if (ObjTouchCheck(ply_work.obj_work, gm_ply_touch_rect[1]) != 0)
                    {
                        num3 = num2 == 0U ? (ushort)4 : (ushort)8;
                        ply_work.safe_spin_timer = 2;
                    }
                    else
                    {
                        num3 = 2;
                        ply_work.safe_spin_timer = 1;
                    }

                    num1 |= num3;
                    break;
            }
        }
        else
        {
            bool flag1 = amTpIsTouchOn(0);
            bool flag2 = amTpIsTouchPush(0);
            if (GmPlayerIsTransformSuperSonic(ply_work) && flag1 &&
                GMM_PLAYER_IS_TOUCH_SUPER_SONIC_REGION(_am_tp_touch[0].on[0], _am_tp_touch[0].on[1]))
                num1 |= 80;
            if ((num1 & 80) == 0)
            {
                ushort num2 = 0;
                uint num3 = ply_work.obj_work.disp_flag & 1U;
                ushort num4 = ObjTouchCheck(ply_work.obj_work, gm_ply_touch_rect[0]);
                if (num4 == 0 && ObjTouchCheck(ply_work.obj_work, gm_ply_touch_rect[1]) != 0)
                {
                    num4 = 1;
                    num2 = num3 == 0U ? (ushort)4 : (ushort)8;
                }

                if (num4 != 0)
                {
                    if (_am_tp_touch[0].on[0] < 80 || _am_tp_touch[0].on[0] > 400)
                    {
                        ply_work.safe_timer = 25;
                    }
                    else
                    {
                        switch (seqState)
                        {
                            case 0:
                            case 9:
                                if (num2 != 0)
                                {
                                    num1 |= num2;
                                    break;
                                }

                                if (flag2 || flag1)
                                {
                                    num1 |= 2;
                                    break;
                                }

                                break;
                            case 1:
                                if (flag2 || flag1)
                                {
                                    ply_work.spin_state = 3;
                                    num1 |= 2;
                                    break;
                                }

                                break;
                            case 2:
                                if (flag2 | flag1)
                                {
                                    num1 |= 2;
                                    break;
                                }

                                break;
                            case 6:
                            case 7:
                            case 8:
                                if (flag2 || flag1)
                                {
                                    num1 |= 34;
                                    break;
                                }

                                break;
                            case 11:
                            case 12:
                                if (flag2 | flag1)
                                {
                                    num1 |= 2;
                                    break;
                                }

                                break;
                            default:
                                if (ply_work.spin_state != 3)
                                {
                                    if (flag2 || flag1)
                                    {
                                        ply_work.spin_state = 0;
                                        num1 |= 32;
                                        break;
                                    }

                                    break;
                                }

                                goto case 1;
                        }
                    }
                }
                else
                {
                    ply_work.spin_state = 0;
                    if (flag2 || flag1)
                        num1 |= 32;
                }
            }
        }

        return num1;
    }

    private static ushort gmPlayerRemapKeyIPhoneZone32SS(GMS_PLAYER_WORK ply_work)
    {
        ushort num = 0;
        if (GmMainKeyCheckPauseKeyPush() != -1)
            return num;
        for (int index = 0; index < 4; ++index)
        {
            bool flag1 = amTpIsTouchOn(index);
            bool flag2 = amTpIsTouchPush(index);
            if (GmPlayerIsTransformSuperSonic(ply_work) && flag1 &&
                GMM_PLAYER_IS_TOUCH_SUPER_SONIC_REGION(_am_tp_touch[index].on[0], _am_tp_touch[index].on[1]))
                num |= 80;
            if ((num & 80) == 0 && (flag2 || flag1))
                num |= 32;
            if (num != 0)
                break;
        }

        return num;
    }

    private static bool gmPlayerIsInputDPadJumpKey(GMS_PLAYER_WORK ply_work, int ignore_key)
    {
        if ((AoPad.AoPadMDirect() & ControllerConsts.JUMP) != 0)
            return true;

        if (ply_work.control_type == 2)
            return false;
        if (ply_work.jump_rect == null)
        {
            mppAssertNotImpl();
            return false;
        }

        bool flag = false;
        for (int index = 0; index < 4; ++index)
        {
            if ((index != ignore_key || amTpIsTouchPush(index)) && amTpIsTouchOn(index))
            {
                short num1 = (short)_am_tp_touch[index].on[0];
                short num2 = (short)_am_tp_touch[index].on[1];
                if (ply_work.jump_rect[0] <= num1 && num1 <= ply_work.jump_rect[2] &&
                    (ply_work.jump_rect[1] <= num2 && num2 <= ply_work.jump_rect[3]))
                {
                    flag = true;
                    break;
                }
            }
        }

        return flag;
    }

    private static bool gmPlayerIsInputDPadSSonicKey(GMS_PLAYER_WORK ply_work, int ignore_key)
    {
        if ((AoPad.AoPadMDirect() & ControllerConsts.SUPER_SONIC) != 0)
            return true;

        if (ply_work.control_type == 2)
            return false;
        bool flag = false;
        for (int index = 0; index < 4; ++index)
        {
            if ((index != ignore_key || amTpIsTouchPush(index)) && amTpIsTouchOn(index))
            {
                short num1 = (short)_am_tp_touch[index].on[0];
                short num2 = (short)_am_tp_touch[index].on[1];
                if (ply_work.ssonic_rect[0] <= num1 && num1 <= ply_work.ssonic_rect[2] &&
                    (ply_work.ssonic_rect[1] <= num2 && num2 <= ply_work.ssonic_rect[3]))
                {
                    flag = true;
                    break;
                }
            }
        }

        return flag;
    }

    private static void gmPlayerCameraOffset(GMS_PLAYER_WORK ply_work)
    {
        byte num1 = 4;
        if (ply_work.player_id != 0 || GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            return;
        if (ply_work.gmk_obj == null)
        {
            ply_work.gmk_flag &= 4227858431U;
            ply_work.gmk_camera_gmk_center_ofst_x = 0;
            ply_work.gmk_camera_gmk_center_ofst_y = 0;
        }

        short num2 = 0;
        short num3 = 0;
        if ((ply_work.player_flag & 8192) != 0)
        {
            ply_work.camera_ofst_x -= ply_work.camera_ofst_x >> 2;
            ply_work.camera_ofst_y -= ply_work.camera_ofst_y >> 2;
        }
        else if (((int)ply_work.gmk_flag & 67108864) != 0)
        {
            ply_work.camera_ofst_x += ply_work.camera_ofst_tag_x - ply_work.camera_ofst_x +
                (ply_work.gmk_camera_center_ofst_x + ply_work.gmk_camera_gmk_center_ofst_x << 12) >> num1;
            ply_work.camera_ofst_y += ply_work.camera_ofst_tag_y - ply_work.camera_ofst_y +
                (ply_work.gmk_camera_center_ofst_y + ply_work.gmk_camera_gmk_center_ofst_y << 12) >> num1;
        }
        else
        {
            ply_work.camera_ofst_x += ply_work.camera_ofst_tag_x - ply_work.camera_ofst_x +
                (ply_work.gmk_camera_center_ofst_x << 12) >> num1;
            ply_work.camera_ofst_y += ply_work.camera_ofst_tag_y - ply_work.camera_ofst_y +
                (ply_work.gmk_camera_center_ofst_y << 12) >> num1;
        }

        OBS_CAMERA obsCamera = ObjCameraGet(0);
        obsCamera.ofst.x = FXM_FX32_TO_FLOAT((num2 << 12) + ply_work.camera_ofst_x);
        obsCamera.ofst.y = FXM_FX32_TO_FLOAT((num3 << 12) + ply_work.camera_ofst_y);
    }

    public static void gmGmkPlayerMotionCallbackTruck(
        AMS_MOTION motion,
        NNS_OBJECT _object,
        object param)
    {
        NNS_MATRIX mtx = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        GMS_PLAYER_WORK gmsPlayerWork = (GMS_PLAYER_WORK)param;
        nnMakeUnitMatrix(nnsMatrix);
        nnMultiplyMatrix(nnsMatrix, nnsMatrix, amMatrixGetCurrent());
        nnCalcNodeMatrixTRSList(mtx, _object, GMD_PLAYER_NODE_ID_TRUCK_CENTER, motion.data, nnsMatrix);
        mtx.Assign(gmsPlayerWork.truck_mtx_ply_mtn_pos);
    }
}