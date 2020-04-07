using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

public partial class AppMain
{

    private static byte[][] g_gm_player_motion_right_tbl
    {
        get
        {
            if (AppMain._g_gm_player_motion_right_tbl == null)
                AppMain._g_gm_player_motion_right_tbl = new byte[7][]
                {
          AppMain.gm_player_motion_list_son_right,
          AppMain.gm_player_motion_list_sson_right,
          AppMain.gm_player_motion_list_son_right,
          AppMain.gm_player_motion_list_pn_son_right,
          AppMain.gm_player_motion_list_pn_sson_right,
          AppMain.gm_player_motion_list_tr_son_right,
          AppMain.gm_player_motion_list_tr_son_right
                };
            return AppMain._g_gm_player_motion_right_tbl;
        }
    }

    private static byte[][] g_gm_player_motion_left_tbl
    {
        get
        {
            if (AppMain._g_gm_player_motion_left_tbl == null)
                AppMain._g_gm_player_motion_left_tbl = new byte[7][]
                {
          AppMain.gm_player_motion_list_son_left,
          AppMain.gm_player_motion_list_sson_left,
          AppMain.gm_player_motion_list_son_left,
          AppMain.gm_player_motion_list_pn_son_left,
          AppMain.gm_player_motion_list_pn_sson_left,
          AppMain.gm_player_motion_list_tr_son_left,
          AppMain.gm_player_motion_list_tr_son_left
                };
            return AppMain._g_gm_player_motion_left_tbl;
        }
    }

    private static byte[][] g_gm_player_model_tbl
    {
        get
        {
            if (AppMain._g_gm_player_model_tbl == null)
                AppMain._g_gm_player_model_tbl = new byte[7][]
                {
          AppMain.gm_player_model_list_son,
          AppMain.gm_player_model_list_son,
          AppMain.gm_player_model_list_son,
          AppMain.gm_player_model_list_pn_son,
          AppMain.gm_player_model_list_pn_son,
          AppMain.gm_player_model_list_tr_son,
          AppMain.gm_player_model_list_tr_son
                };
            return AppMain._g_gm_player_model_tbl;
        }
    }

    private static byte[][] g_gm_player_mtn_blend_setting_tbl
    {
        get
        {
            if (AppMain._g_gm_player_mtn_blend_setting_tbl == null)
                AppMain._g_gm_player_mtn_blend_setting_tbl = new byte[7][]
                {
          AppMain.gm_player_mtn_blend_setting_son,
          AppMain.gm_player_mtn_blend_setting_son,
          AppMain.gm_player_mtn_blend_setting_son,
          AppMain.gm_player_mtn_blend_setting_pn_son,
          AppMain.gm_player_mtn_blend_setting_pn_son,
          AppMain.gm_player_mtn_blend_setting_tr_son,
          AppMain.gm_player_mtn_blend_setting_tr_son
                };
            return AppMain._g_gm_player_mtn_blend_setting_tbl;
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
        AppMain.AMS_AMB_HEADER archive1 = AppMain.readAMBFile((AppMain.AMS_FS)AppMain.g_gm_player_data_work[0][0].pData);
        AppMain.AMS_AMB_HEADER amb_tex1 = AppMain.readAMBFile((AppMain.AMS_FS)AppMain.g_gm_player_data_work[0][1].pData);
        AppMain.g_gm_ply_son_obj_3d_list = AppMain.New<AppMain.OBS_ACTION3D_NN_WORK>(archive1.file_num);
        AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK> arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(AppMain.g_gm_ply_son_obj_3d_list);
        int index1 = 0;
        while (index1 < archive1.file_num)
        {
            AppMain.ObjAction3dNNModelLoad((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer, (AppMain.OBS_DATA_WORK)null, (string)null, index1, archive1, (string)null, amb_tex1, 0U);
            ++index1;
            ++arrayPointer;
        }
        AppMain.AMS_AMB_HEADER archive2 = AppMain.readAMBFile((AppMain.AMS_FS)AppMain.g_gm_player_data_work[0][2].pData);
        AppMain.AMS_AMB_HEADER amb_tex2 = AppMain.readAMBFile((AppMain.AMS_FS)AppMain.g_gm_player_data_work[0][3].pData);
        AppMain.g_gm_ply_sson_obj_3d_list = AppMain.New<AppMain.OBS_ACTION3D_NN_WORK>(archive2.file_num);
        arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(AppMain.g_gm_ply_sson_obj_3d_list);
        int index2 = 0;
        while (index2 < archive2.file_num)
        {
            AppMain.ObjAction3dNNModelLoad((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer, (AppMain.OBS_DATA_WORK)null, (string)null, index2, archive2, (string)null, amb_tex2, 0U);
            ++index2;
            ++arrayPointer;
        }
        AppMain.gm_ply_obj_3d_list_tbl = new AppMain.OBS_ACTION3D_NN_WORK[7][][]
        {
      new AppMain.OBS_ACTION3D_NN_WORK[2][]
      {
        AppMain.g_gm_ply_son_obj_3d_list,
        AppMain.g_gm_ply_sson_obj_3d_list
      },
      new AppMain.OBS_ACTION3D_NN_WORK[2][]
      {
        AppMain.g_gm_ply_son_obj_3d_list,
        AppMain.g_gm_ply_sson_obj_3d_list
      },
      new AppMain.OBS_ACTION3D_NN_WORK[2][]
      {
        AppMain.g_gm_ply_son_obj_3d_list,
        AppMain.g_gm_ply_sson_obj_3d_list
      },
      new AppMain.OBS_ACTION3D_NN_WORK[2][]
      {
        AppMain.g_gm_ply_son_obj_3d_list,
        AppMain.g_gm_ply_sson_obj_3d_list
      },
      new AppMain.OBS_ACTION3D_NN_WORK[2][]
      {
        AppMain.g_gm_ply_son_obj_3d_list,
        AppMain.g_gm_ply_sson_obj_3d_list
      },
      new AppMain.OBS_ACTION3D_NN_WORK[2][]
      {
        AppMain.g_gm_ply_son_obj_3d_list,
        AppMain.g_gm_ply_sson_obj_3d_list
      },
      new AppMain.OBS_ACTION3D_NN_WORK[2][]
      {
        AppMain.g_gm_ply_son_obj_3d_list,
        AppMain.g_gm_ply_sson_obj_3d_list
      }
        };
    }

    private static void GmPlayerFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader1 = AppMain.readAMBFile((AppMain.AMS_FS)AppMain.g_gm_player_data_work[0][0].pData);
        AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK> arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(AppMain.g_gm_ply_son_obj_3d_list);
        int num1 = 0;
        while (num1 < amsAmbHeader1.file_num)
        {
            AppMain.ObjAction3dNNModelRelease((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer);
            ++num1;
            ++arrayPointer;
        }
        AppMain.AMS_AMB_HEADER amsAmbHeader2 = AppMain.readAMBFile((AppMain.AMS_FS)AppMain.g_gm_player_data_work[0][2].pData);
        arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(AppMain.g_gm_ply_sson_obj_3d_list);
        int num2 = 0;
        while (num2 < amsAmbHeader2.file_num)
        {
            AppMain.ObjAction3dNNModelRelease((AppMain.OBS_ACTION3D_NN_WORK)~arrayPointer);
            ++num2;
            ++arrayPointer;
        }
    }

    private static bool GmPlayerBuildCheck()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader1 = AppMain.readAMBFile((AppMain.AMS_FS)AppMain.g_gm_player_data_work[0][0].pData);
        AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK> arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(AppMain.g_gm_ply_son_obj_3d_list);
        int num1 = 0;
        while (num1 < amsAmbHeader1.file_num)
        {
            if (!AppMain.ObjAction3dNNModelLoadCheck((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer))
                return false;
            ++num1;
            ++arrayPointer;
        }
        AppMain.AMS_AMB_HEADER amsAmbHeader2 = AppMain.readAMBFile((AppMain.AMS_FS)AppMain.g_gm_player_data_work[0][2].pData);
        arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(AppMain.g_gm_ply_sson_obj_3d_list);
        int num2 = 0;
        while (num2 < amsAmbHeader2.file_num)
        {
            if (!AppMain.ObjAction3dNNModelLoadCheck((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer))
                return false;
            ++num2;
            ++arrayPointer;
        }
        return true;
    }

    private static bool GmPlayerFlushCheck()
    {
        AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK> arrayPointer;
        if (AppMain.g_gm_ply_son_obj_3d_list != null)
        {
            AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((AppMain.AMS_FS)AppMain.g_gm_player_data_work[0][0].pData);
            arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(AppMain.g_gm_ply_son_obj_3d_list);
            int num = 0;
            while (num < amsAmbHeader.file_num)
            {
                if (!AppMain.ObjAction3dNNModelReleaseCheck((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer))
                    return false;
                ++num;
                ++arrayPointer;
            }
            AppMain.g_gm_ply_son_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        }
        if (AppMain.g_gm_ply_sson_obj_3d_list != null)
        {
            AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((AppMain.AMS_FS)AppMain.g_gm_player_data_work[0][2].pData);
            arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(AppMain.g_gm_ply_sson_obj_3d_list);
            int num = 0;
            while (num < amsAmbHeader.file_num)
            {
                if (!AppMain.ObjAction3dNNModelReleaseCheck((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer))
                    return false;
                ++num;
                ++arrayPointer;
            }
            AppMain.g_gm_ply_sson_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        }
        return true;
    }

    private static void GmPlayerRelease()
    {
        for (int index1 = 0; index1 < 1; ++index1)
        {
            for (int index2 = 0; index2 < 5; ++index2)
                AppMain.ObjDataRelease(AppMain.g_gm_player_data_work[index1][index2]);
        }
    }

    private static AppMain.GMS_PLAYER_WORK GmPlayerInit(
      int char_id,
      ushort ctrl_id,
      ushort player_id,
      ushort camera_id)
    {
        ushort[] numArray1 = new ushort[3]
        {
      (ushort) 0,
      (ushort) 6,
      (ushort) 1
        };
        ushort[] numArray2 = new ushort[3]
        {
      (ushort) 65533,
      ushort.MaxValue,
      (ushort) 65534
        };
        if (char_id >= 7)
            char_id = 0;
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)4352, (byte)1, (byte)0, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_PLAYER_WORK()), "PLAYER OBJ");
        AppMain.mtTaskChangeTcbDestructor(obsObjectWork.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmPlayerExit));
        obsObjectWork.ppUserRelease = new AppMain.OBS_OBJECT_WORK_Delegate4(AppMain.gmPlayerObjRelease);
        obsObjectWork.ppUserReleaseWait = new AppMain.OBS_OBJECT_WORK_Delegate4(AppMain.gmPlayerObjReleaseWait);
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)obsObjectWork;
        ply_work.player_id = (byte)player_id;
        ply_work.camera_no = (byte)camera_id;
        ply_work.ctrl_id = (byte)ctrl_id;
        ply_work.char_id = (byte)char_id;
        ply_work.act_state = -1;
        ply_work.prev_act_state = -1;
        AppMain.nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
        AppMain.GmPlayerInitModel(ply_work);
        ply_work.key_map[0] = (ushort)1;
        ply_work.key_map[1] = (ushort)2;
        ply_work.key_map[2] = (ushort)4;
        ply_work.key_map[3] = (ushort)8;
        ply_work.key_map[4] = (ushort)32;
        ply_work.key_map[5] = (ushort)128;
        ply_work.key_map[6] = (ushort)64;
        ply_work.key_map[7] = (ushort)16;
        obsObjectWork.obj_type = (ushort)1;
        obsObjectWork.flag |= 16U;
        obsObjectWork.flag |= 1U;
        obsObjectWork.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlayerDispFunc);
        obsObjectWork.ppIn = AppMain.GSM_MAIN_STAGE_IS_SPSTAGE() ? new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlayerSplStgInFunc) : new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlayerDefaultInFunc);
        obsObjectWork.ppLast = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlayerDefaultLastFunc);
        obsObjectWork.ppMove = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmPlySeqMoveFunc);
        obsObjectWork.disp_flag |= 2048U;
        AppMain.GmPlySeqSetSeqState(ply_work);
        AppMain.GmPlayerStateInit(ply_work);
        AppMain.ObjObjectFieldRectSet(obsObjectWork, (short)-6, (short)-12, (short)6, (short)13);
        AppMain.ObjObjectGetRectBuf(obsObjectWork, (AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>)ply_work.rect_work, (ushort)3);
        for (int index = 0; index < 3; ++index)
        {
            AppMain.ObjRectGroupSet(ply_work.rect_work[index], (byte)0, (byte)2);
            AppMain.ObjRectAtkSet(ply_work.rect_work[index], numArray1[index], (short)1);
            AppMain.ObjRectDefSet(ply_work.rect_work[index], numArray2[index], (short)0);
            ply_work.rect_work[index].parent_obj = obsObjectWork;
            ply_work.rect_work[index].flag &= 4294967291U;
            ply_work.rect_work[index].rect.back = (short)-16;
            ply_work.rect_work[index].rect.front = (short)16;
        }
        ply_work.rect_work[0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmPlayerDefFunc);
        ply_work.rect_work[1].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmPlayerAtkFunc);
        ply_work.rect_work[0].flag |= 128U;
        ply_work.rect_work[1].flag |= 32U;
        ply_work.rect_work[2].flag |= 224U;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            AppMain.ObjObjectFieldRectSet(obsObjectWork, (short)-7, (short)-8, (short)7, (short)10);
            AppMain.ObjRectWorkZSet(ply_work.rect_work[2], (short)-11, (short)-11, (short)-500, (short)11, (short)11, (short)500);
            AppMain.ObjRectWorkZSet(ply_work.rect_work[0], (short)-12, (short)-12, (short)-500, (short)12, (short)12, (short)500);
            AppMain.ObjRectWorkZSet(ply_work.rect_work[1], (short)-13, (short)-13, (short)-500, (short)13, (short)13, (short)500);
        }
        else
        {
            AppMain.ObjRectWorkZSet(ply_work.rect_work[2], (short)-8, (short)-19, (short)-500, (short)8, (short)13, (short)500);
            AppMain.ObjRectWorkZSet(ply_work.rect_work[0], (short)-8, (short)-19, (short)-500, (short)8, (short)13, (short)500);
            AppMain.ObjRectWorkZSet(ply_work.rect_work[1], (short)-16, (short)-19, (short)-500, (short)16, (short)13, (short)500);
        }
        ply_work.rect_work[1].flag &= 4294967291U;
        AppMain.ObjRectWorkZSet(AppMain.gm_ply_touch_rect[0], (short)-16, (short)-51, (short)-500, (short)64, (short)37, (short)500);
        AppMain.ObjRectWorkZSet(AppMain.gm_ply_touch_rect[1], (short)-64, (short)-51, (short)-500, (short)-16, (short)37, (short)500);
        ply_work.calc_accel.x = 0.0f;
        ply_work.calc_accel.y = 0.0f;
        ply_work.calc_accel.z = 0.0f;
        ply_work.control_type = 2;
        ply_work.jump_rect = AppMain.gm_player_push_jump_key_rect[2];
        ply_work.ssonic_rect = AppMain.gm_player_push_ssonic_key_rect[2];
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 1) != 0)
        {
            ply_work.control_type = 0;
            ply_work.jump_rect = AppMain.gm_player_push_jump_key_rect[0];
            ply_work.ssonic_rect = AppMain.gm_player_push_ssonic_key_rect[0];
        }
        ply_work.accel_counter = 0;
        ply_work.dir_vec_add = 0;
        ply_work.spin_se_timer = (short)0;
        ply_work.spin_back_se_timer = (short)0;
        ply_work.safe_timer = 0;
        ply_work.safe_jump_timer = 0;
        ply_work.safe_spin_timer = 0;
        if (ply_work.player_id == (byte)0)
        {
            AppMain.gm_pos_x = ply_work.obj_work.pos.x >> 12;
            AppMain.gm_pos_y = ply_work.obj_work.pos.y >> 12;
            AppMain.gm_pos_z = ply_work.obj_work.pos.z >> 12;
        }
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlayerMain);
        AppMain.GmPlySeqChangeFw(ply_work);
        if (!SaveState.resumePlayer_2(ply_work))
        {
            ply_work.obj_work.pos.x = AppMain.g_gm_main_system.resume_pos_x;
            ply_work.obj_work.pos.y = AppMain.g_gm_main_system.resume_pos_y;
        }
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)5)
            AppMain.GmPlayerSetPinballSonic(ply_work);
        else if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
            AppMain.GmPlayerSetSplStgSonic(ply_work);
        return ply_work;
    }

    private static void GmPlayerResetInit(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag &= 4290772991U;
        AppMain.g_obj.flag &= 4294966271U;
        AppMain.g_obj.scroll[0] = AppMain.g_obj.scroll[1] = 0;
        ply_work.player_flag &= 4290766847U;
        ply_work.gmk_flag &= 3556759567U;
        ply_work.graind_id = (byte)0;
        ply_work.graind_prev_ride = (byte)0;
        ply_work.gmk_flag &= 4294967291U;
        ply_work.spd_pool = (short)0;
        ply_work.obj_work.scale.x = ply_work.obj_work.scale.y = ply_work.obj_work.scale.z = 4096;
        AppMain.GmPlayerStateInit(ply_work);
        AppMain.GmPlayerStateGimmickInit(ply_work);
    }

    private static void GmPlayerInitModel(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK> arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(ply_work.obj_3d_work);
        for (int index1 = 0; index1 < 2; ++index1)
        {
            AppMain.OBS_ACTION3D_NN_WORK[] obsActioN3DNnWorkArray = AppMain.gm_ply_obj_3d_list_tbl[(int)ply_work.char_id][index1];
            int index2 = 0;
            while (index2 < 4)
            {
                AppMain.ObjCopyAction3dNNModel(obsActioN3DNnWorkArray[index2], (AppMain.OBS_ACTION3D_NN_WORK)arrayPointer);
                ((AppMain.OBS_ACTION3D_NN_WORK)~arrayPointer).blend_spd = 0.25f;
                AppMain.ObjDrawSetToon((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer);
                ((AppMain.OBS_ACTION3D_NN_WORK)~arrayPointer).use_light_flag &= 4294967294U;
                ((AppMain.OBS_ACTION3D_NN_WORK)~arrayPointer).use_light_flag |= 64U;
                ++index2;
                ++arrayPointer;
            }
        }
        objWork.obj_3d = ply_work.obj_3d_work[0];
        objWork.flag |= 536870912U;
        AppMain.ObjObjectAction3dNNMotionLoad(objWork, 0, true, AppMain.g_gm_player_data_work[(int)ply_work.player_id][4], (string)null, 0, (AppMain.AMS_AMB_HEADER)null, 136, 16);
        objWork.disp_flag |= 16777728U;
        for (int index = 1; index < 8; ++index)
            ply_work.obj_3d_work[index].motion = ply_work.obj_3d_work[0].motion;
        AppMain.GmPlayerSetModel(ply_work, 0);
    }

    private static void GmPlayerSetModel(AppMain.GMS_PLAYER_WORK ply_work, int model_set)
    {
        ply_work.obj_3d[0] = ply_work.obj_3d_work[model_set * 4];
        ply_work.obj_3d[1] = ply_work.obj_3d_work[model_set * 4 + 1];
        ply_work.obj_3d[2] = ply_work.obj_3d_work[model_set * 4 + 2];
        ply_work.obj_3d[3] = ply_work.obj_3d_work[model_set * 4 + 3];
        int index = ply_work.act_state;
        if (index == -1)
            index = 0;
        ply_work.obj_work.obj_3d = ply_work.obj_3d[(int)AppMain.g_gm_player_model_tbl[(int)ply_work.char_id][index]];
    }

    private static void GmPlayerStateInit(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.seq_init_tbl = AppMain.g_gm_ply_seq_init_tbl_list[(int)ply_work.char_id];
        AppMain.GmPlayerSpdParameterSet(ply_work);
        ply_work.obj_work.dir.x = (ushort)0;
        ply_work.obj_work.dir.y = (ushort)0;
        ply_work.obj_work.dir.z = (ushort)0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd.z = 0;
        if (((int)ply_work.gmk_flag & 256) == 0)
            ply_work.obj_work.pos.z = 0;
        ply_work.obj_work.ride_obj = (AppMain.OBS_OBJECT_WORK)null;
        if (((int)ply_work.gmk_flag & 1536) == 0)
        {
            ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
            ply_work.gmk_camera_ofst_x = (short)0;
            ply_work.gmk_camera_ofst_y = (short)0;
            ply_work.gmk_camera_gmk_center_ofst_x = (short)0;
            ply_work.gmk_camera_gmk_center_ofst_y = (short)0;
            ply_work.gmk_flag &= 4227858183U;
        }
        ply_work.gmk_work0 = 0;
        ply_work.gmk_work1 = 0;
        ply_work.gmk_work2 = 0;
        ply_work.gmk_work3 = 0;
        ply_work.spd_work_max = 0;
        ply_work.camera_jump_pos_y = 0;
        if (((int)ply_work.graind_id & 128) != 0)
            ply_work.obj_work.flag |= 1U;
        ply_work.gmk_flag &= 3720345596U;
        ply_work.player_flag &= 4294367568U;
        ply_work.obj_work.flag &= 4294967293U;
        ply_work.obj_work.flag |= 16U;
        ply_work.obj_work.disp_flag &= 4294966991U;
        ply_work.obj_work.move_flag &= 3623816416U;
        ply_work.obj_work.move_flag |= 1076756672U;
        if (((int)ply_work.player_flag & 262144) != 0)
            ply_work.obj_work.move_flag &= 4294705151U;
        else if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            ply_work.obj_work.move_flag &= 4294705151U;
            ply_work.obj_work.move_flag |= 536875008U;
        }
        ply_work.no_jump_move_timer = 0;
        if (ply_work.obj_work.obj_3d == null)
            return;
        ply_work.obj_work.obj_3d.blend_spd = 0.25f;
    }

    private static void GmPlayerStateGimmickInit(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
        ply_work.gmk_camera_ofst_x = (short)0;
        ply_work.gmk_camera_ofst_y = (short)0;
        ply_work.gmk_camera_gmk_center_ofst_x = (short)0;
        ply_work.gmk_camera_gmk_center_ofst_y = (short)0;
        ply_work.gmk_work0 = 0;
        ply_work.gmk_work1 = 0;
        ply_work.gmk_work2 = 0;
        ply_work.gmk_work3 = 0;
        ply_work.obj_work.dir.x = (ushort)0;
        ply_work.obj_work.dir.y = (ushort)0;
        ply_work.score_combo_cnt = 0U;
        AppMain.GmPlayerCameraOffsetSet(ply_work, (short)0, (short)0);
        AppMain.GmCameraAllowReset();
        if (((int)ply_work.graind_id & 128) != 0)
        {
            ply_work.obj_work.flag |= 1U;
            ply_work.graind_id = (byte)0;
        }
        ply_work.player_flag &= 4294434128U;
        ply_work.gmk_flag &= 4227612431U;
        ply_work.gmk_flag2 &= 4294967097U;
        ply_work.obj_work.disp_flag &= 4294967007U;
        ply_work.obj_work.move_flag &= 3623816447U;
        ply_work.obj_work.move_flag |= 1076232384U;
        ply_work.no_jump_move_timer = 0;
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            ply_work.obj_work.move_flag &= 4294705151U;
        }
        else
        {
            if (!AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
                return;
            ply_work.obj_work.move_flag &= 4294705151U;
            ply_work.obj_work.move_flag |= 536875008U;
        }
    }

    private static void GmPlayerSpdParameterSet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.spd_add = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_add;
        ply_work.spd_max = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_max;
        ply_work.spd1 = (int)((double)ply_work.spd_max * 0.15);
        ply_work.spd2 = (int)((double)ply_work.spd_max * 0.3);
        ply_work.spd3 = (int)((double)ply_work.spd_max * 0.5);
        ply_work.spd4 = (int)((double)ply_work.spd_max * 0.7);
        ply_work.spd5 = (int)((double)ply_work.spd_max * 0.9);
        ply_work.spd_dec = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_dec;
        ply_work.spd_spin = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_spin;
        ply_work.spd_add_spin = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_add_spin;
        ply_work.spd_max_spin = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_max_spin;
        ply_work.spd_dec_spin = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_dec_spin;
        ply_work.spd_max_add_slope = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_max_add_slope;
        ply_work.spd_jump = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_jump;
        ply_work.time_air = (int)AppMain.g_gm_player_parameter[(int)ply_work.char_id].time_air << 12;
        ply_work.time_damage = (int)AppMain.g_gm_player_parameter[(int)ply_work.char_id].time_damage << 12;
        ply_work.fall_timer = (int)AppMain.g_gm_player_parameter[(int)ply_work.char_id].fall_wait_time << 12;
        ply_work.spd_jump_add = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_jump_add;
        ply_work.spd_jump_max = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_jump_max;
        ply_work.spd_jump_dec = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_jump_dec;
        ply_work.spd_add_spin_pinball = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_add_spin_pinball;
        ply_work.spd_max_spin_pinball = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_max_spin_pinball;
        ply_work.spd_dec_spin_pinball = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_dec_spin_pinball;
        ply_work.spd_max_add_slope_spin_pinball = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_max_add_slope_spin_pinball;
        ply_work.obj_work.dir_slope = AppMain.GSM_MAIN_STAGE_IS_SPSTAGE() ? (ushort)1 : (((int)ply_work.player_flag & 262144) == 0 ? (ushort)192 : (ushort)512);
        ply_work.obj_work.spd_slope = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_slope;
        ply_work.obj_work.spd_slope_max = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_slope_max;
        ply_work.obj_work.spd_fall = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_fall;
        ply_work.obj_work.spd_fall_max = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_fall_max;
        ply_work.obj_work.push_max = AppMain.g_gm_player_parameter[(int)ply_work.char_id].push_max;
        if (((int)ply_work.player_flag & 67108864) != 0)
        {
            AppMain.GMD_PLAYER_WATERJUMP_SET(ref ply_work.spd_jump);
            AppMain.GMD_PLAYER_WATER_SET(ref ply_work.obj_work.spd_fall);
        }
        if (ply_work.hi_speed_timer == 0)
            return;
        ply_work.spd_add <<= 1;
        if (ply_work.spd_add > 61440)
            ply_work.spd_add = 61440;
        ply_work.spd_max <<= 1;
        if (ply_work.spd_max > 61440)
            ply_work.spd_max = 61440;
        ply_work.spd_dec <<= 1;
        ply_work.spd_spin <<= 1;
        ply_work.spd_add_spin <<= 1;
        ply_work.spd_max_spin <<= 1;
        if (ply_work.spd_max_spin > 61440)
            ply_work.spd_max_spin = 61440;
        ply_work.spd_dec_spin <<= 1;
        ply_work.spd_max_add_slope <<= 1;
        ply_work.spd_jump_add <<= 1;
        ply_work.spd_jump_max <<= 1;
        if (ply_work.spd_jump_max > 61440)
            ply_work.spd_jump_max = 61440;
        ply_work.spd_jump_dec <<= 1;
    }

    private static void GmPlayerSpdParameterSetWater(AppMain.GMS_PLAYER_WORK ply_work, bool water)
    {
        ply_work.spd_jump = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_jump;
        ply_work.obj_work.spd_fall = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_fall;
        if (!water)
            return;
        AppMain.GMD_PLAYER_WATERJUMP_SET(ref ply_work.spd_jump);
        AppMain.GMD_PLAYER_WATER_SET(ref ply_work.obj_work.spd_fall);
    }

    private static void GmPlayerSetAtk(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.rect_work[1].flag |= 4U;
        AppMain.ObjRectHitAgain(ply_work.rect_work[1]);
    }

    private static void GmPlayerSetDefInvincible(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.rect_work[0].def_power = (short)3;
    }

    private static void GmPlayerSetDefNormal(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.rect_work[0].def_power = (short)0;
    }

    private static void GmPlayerBreathingSet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.water_timer = 0;
    }

    private static void GmPlayerSetMarkerPoint(
      AppMain.GMS_PLAYER_WORK ply_work,
      int pos_x,
      int pos_y)
    {
        AppMain.g_gm_main_system.time_save = AppMain.g_gm_main_system.game_time;
        AppMain.g_gm_main_system.resume_pos_x = pos_x;
        AppMain.g_gm_main_system.resume_pos_y = pos_y - ((int)ply_work.obj_work.field_rect[3] << 12);
    }

    private static void GmPlayerSetSuperSonic(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerStateInit(ply_work);
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            ply_work.obj_work.pos.z = (int)short.MinValue;
            ply_work.gmk_flag |= 536870912U;
        }
        ply_work.char_id = ((int)ply_work.player_flag & 131072) == 0 ? (((int)ply_work.player_flag & 262144) == 0 ? (byte)1 : (byte)6) : (byte)4;
        ply_work.player_flag |= 16384U;
        AppMain.GmPlayerSetModel(ply_work, 1);
        AppMain.GmPlySeqSetSeqState(ply_work);
        AppMain.GmPlayerSpdParameterSet(ply_work);
        ply_work.obj_work.move_flag |= 16U;
        ply_work.obj_work.move_flag &= 4294967152U;
        ply_work.obj_work.flag |= 2U;
        AppMain.GmPlyEfctCreateSuperAuraDeco(ply_work);
        AppMain.GmPlyEfctCreateSuperAuraBase(ply_work);
        ply_work.super_sonic_ring_timer = int.MaxValue;
        ply_work.light_rate = 0.0f;
        ply_work.light_anm_flag = 0;
        AppMain.g_gm_main_system.game_flag |= 524288U;
        AppMain.GmSoundPlaySE("Transform");
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)28)
            return;
        AppMain.GmSoundPlayJingleInvincible();
    }

    private static void GmPlayerSetEndSuperSonic(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.char_id = ((int)ply_work.player_flag & 131072) == 0 ? (((int)ply_work.player_flag & 262144) == 0 ? (byte)0 : (byte)5) : (byte)3;
        ply_work.player_flag &= 4294950911U;
        AppMain.GmPlayerSetModel(ply_work, 0);
        AppMain.GmPlySeqSetSeqState(ply_work);
        AppMain.GmPlayerSpdParameterSet(ply_work);
        AppMain.GmPlyEfctCreateSuperEnd(ply_work);
        AppMain.GmPlayerSetDefLight();
        AppMain.GmPlayerSetDefRimParam(ply_work);
    }

    private static void GmPlayerSetDefLight()
    {
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_6, ref AppMain.g_gm_main_system.ply_light_col, 1f, AppMain.g_gm_main_system.ply_light_vec);
    }

    private static void GmPlayerSetSplStgSonic(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.move_flag |= 139520U;
        ply_work.obj_work.move_flag &= 4294705151U;
        AppMain.GmPlySeqSetSeqState(ply_work);
        AppMain.GmPlayerSpdParameterSet(ply_work);
        AppMain.GmPlayerActionChange(ply_work, 39);
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlayerSetPinballSonic(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.char_id = ((int)ply_work.player_flag & 16384) == 0 ? (byte)3 : (byte)4;
        ply_work.player_flag |= 131072U;
        ply_work.obj_work.move_flag &= 4294705151U;
        AppMain.GmPlySeqSetSeqState(ply_work);
        AppMain.GmPlayerSpdParameterSet(ply_work);
        AppMain.GmPlayerActionChange(ply_work, 39);
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlayerSetEndPinballSonic(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.char_id = ((int)ply_work.player_flag & 16384) == 0 ? (byte)0 : (byte)1;
        ply_work.player_flag &= 4294836223U;
        ply_work.obj_work.move_flag |= 262144U;
        AppMain.GmPlySeqSetSeqState(ply_work);
        AppMain.GmPlayerSpdParameterSet(ply_work);
    }

    private static void GmPlayerSetTruckRide(
      AppMain.GMS_PLAYER_WORK ply_work,
      AppMain.OBS_OBJECT_WORK truck_obj,
      short field_left,
      short field_top,
      short field_right,
      short field_bottom)
    {
        bool flag = false;
        AppMain.OBS_OBJECT_WORK pObj = (AppMain.OBS_OBJECT_WORK)ply_work;
        ply_work.char_id = ((int)ply_work.player_flag & 16384) == 0 ? (byte)5 : (byte)6;
        ply_work.player_flag |= 262144U;
        ply_work.obj_work.move_flag &= 4294705151U;
        ply_work.gmk_flag2 &= 4294967294U;
        ply_work.truck_obj = truck_obj;
        ply_work.obj_work.ppRec = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlayerRectTruckFunc);
        ply_work.obj_work.ppCol = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlayerTruckCollisionFunc);
        AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK> arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(ply_work.obj_3d_work);
        int num = 0;
        while (num < 8)
        {
            ((AppMain.OBS_ACTION3D_NN_WORK)~arrayPointer).mtn_cb_func = new AppMain.mtn_cb_func_delegate(AppMain.gmGmkPlayerMotionCallbackTruck);
            ((AppMain.OBS_ACTION3D_NN_WORK)~arrayPointer).mtn_cb_param = (object)ply_work;
            ++num;
            ++arrayPointer;
        }
        AppMain.nnMakeUnitMatrix(ply_work.truck_mtx_ply_mtn_pos);
        AppMain.GmPlySeqSetSeqState(ply_work);
        AppMain.GmPlayerSpdParameterSet(ply_work);
        AppMain.ObjObjectFieldRectSet(pObj, field_left, field_top, field_right, field_bottom);
        pObj.field_ajst_w_db_f = (sbyte)3;
        pObj.field_ajst_w_db_b = (sbyte)4;
        pObj.field_ajst_w_dl_f = (sbyte)3;
        pObj.field_ajst_w_dl_b = (sbyte)4;
        pObj.field_ajst_w_dt_f = (sbyte)3;
        pObj.field_ajst_w_dt_b = (sbyte)4;
        pObj.field_ajst_w_dr_f = (sbyte)3;
        pObj.field_ajst_w_dr_b = (sbyte)4;
        pObj.field_ajst_h_db_r = (sbyte)3;
        pObj.field_ajst_h_db_l = (sbyte)3;
        pObj.field_ajst_h_dl_r = (sbyte)3;
        pObj.field_ajst_h_dl_l = (sbyte)3;
        pObj.field_ajst_h_dt_r = (sbyte)3;
        pObj.field_ajst_h_dt_l = (sbyte)3;
        pObj.field_ajst_h_dr_r = (sbyte)3;
        pObj.field_ajst_h_dr_l = (sbyte)3;
        AppMain.ObjRectWorkSet(ply_work.rect_work[2], (short)-8, (short)((int)field_bottom - 32), (short)8, field_bottom);
        AppMain.ObjRectWorkSet(ply_work.rect_work[0], (short)-8, (short)((int)field_bottom - 48), (short)8, (short)((int)field_bottom - 16));
        AppMain.ObjRectWorkSet(ply_work.rect_work[1], (short)-16, (short)((int)field_bottom - 48), (short)16, (short)((int)field_bottom - 16));
        ply_work.rect_work[1].flag &= 4294967291U;
        AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id).user_func = new AppMain.OBJF_CAMERA_USER_FUNC(AppMain.GmCameraTruckFunc);
        if (((int)ply_work.obj_work.disp_flag & 1) != 0)
            AppMain.GmPlayerSetReverse(ply_work);
        if (ply_work.seq_state == 39)
            flag = true;
        AppMain.GmPlySeqChangeFw(ply_work);
        if (!flag)
            return;
        AppMain.GmPlySeqInitDemoFw(ply_work);
    }

    private static void GmPlayerSetEndTruckRide(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)ply_work;
        ply_work.char_id = ((int)ply_work.player_flag & 16384) == 0 ? (byte)0 : (byte)1;
        ply_work.player_flag &= 4294705151U;
        ply_work.obj_work.move_flag |= 262144U;
        ply_work.obj_work.ppRec = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        ply_work.obj_work.ppCol = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK> arrayPointer = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(ply_work.obj_3d_work);
        int num = 0;
        while (num < 8)
        {
            ((AppMain.OBS_ACTION3D_NN_WORK)~arrayPointer).mtn_cb_func = (AppMain.mtn_cb_func_delegate)null;
            ((AppMain.OBS_ACTION3D_NN_WORK)~arrayPointer).mtn_cb_param = (object)null;
            ++num;
            ++arrayPointer;
        }
        AppMain.GmPlySeqSetSeqState(ply_work);
        AppMain.GmPlayerSpdParameterSet(ply_work);
        AppMain.ObjObjectFieldRectSet(ply_work.obj_work, (short)-6, (short)-12, (short)6, (short)13);
        obsObjectWork.field_ajst_w_db_f = (sbyte)2;
        obsObjectWork.field_ajst_w_db_b = (sbyte)4;
        obsObjectWork.field_ajst_w_dl_f = (sbyte)2;
        obsObjectWork.field_ajst_w_dl_b = (sbyte)4;
        obsObjectWork.field_ajst_w_dt_f = (sbyte)2;
        obsObjectWork.field_ajst_w_dt_b = (sbyte)4;
        obsObjectWork.field_ajst_w_dr_f = (sbyte)2;
        obsObjectWork.field_ajst_w_dr_b = (sbyte)4;
        obsObjectWork.field_ajst_h_db_r = (sbyte)1;
        obsObjectWork.field_ajst_h_db_l = (sbyte)1;
        obsObjectWork.field_ajst_h_dl_r = (sbyte)1;
        obsObjectWork.field_ajst_h_dl_l = (sbyte)1;
        obsObjectWork.field_ajst_h_dt_r = (sbyte)1;
        obsObjectWork.field_ajst_h_dt_l = (sbyte)1;
        obsObjectWork.field_ajst_h_dr_r = (sbyte)2;
        obsObjectWork.field_ajst_h_dr_l = (sbyte)2;
        AppMain.ObjRectWorkZSet(ply_work.rect_work[2], (short)-8, (short)-19, (short)-500, (short)8, (short)13, (short)500);
        AppMain.ObjRectWorkZSet(ply_work.rect_work[0], (short)-8, (short)-19, (short)-500, (short)8, (short)13, (short)500);
        AppMain.ObjRectWorkZSet(ply_work.rect_work[1], (short)-16, (short)-19, (short)-500, (short)16, (short)13, (short)500);
        ply_work.rect_work[1].flag &= 4294967291U;
        ply_work.obj_work.dir_fall = (ushort)0;
        AppMain.g_gm_main_system.pseudofall_dir = (ushort)0;
        AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id).user_func = new AppMain.OBJF_CAMERA_USER_FUNC(AppMain.GmCameraFunc);
    }

    private static void GmPlayerSetGoalState(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 131072) != 0)
            AppMain.GmPlayerSetEndPinballSonic(ply_work);
        if (((int)ply_work.player_flag & 16384) != 0)
            AppMain.gmPlayerSuperSonicToSonic(ply_work);
        AppMain.GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
        ply_work.genocide_timer = 0;
        if (((int)ply_work.player_flag & 262144) == 0)
            return;
        AppMain.ObjRectWorkSet(ply_work.rect_work[2], (short)0, (short)-37, (short)16, (short)-5);
    }

    private static void GmPlayerSetAutoRun(
      AppMain.GMS_PLAYER_WORK ply_work,
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

    private static void GmPlayerRingGet(AppMain.GMS_PLAYER_WORK ply_work, short add_ring)
    {
        short ringNum = ply_work.ring_num;
        ply_work.ring_num += add_ring;
        ply_work.ring_num = (short)AppMain.MTM_MATH_CLIP((int)ply_work.ring_num, 0, 999);
        ply_work.ring_stage_num += add_ring;
        ply_work.ring_stage_num = (short)AppMain.MTM_MATH_CLIP((int)ply_work.ring_stage_num, 0, 9999);
        AppMain.GmRingGetSE();
        if (AppMain.g_gs_main_sys_info.game_mode == 1)
            return;
        if (!AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            if (((int)ply_work.player_flag & 16384) != 0 || AppMain.g_gs_main_sys_info.game_mode == 1)
                return;
            for (short index = 100; index <= (short)900; index += (short)100)
            {
                if ((int)ringNum < (int)index && (int)ply_work.ring_num >= (int)index)
                {
                    AppMain.GmPlayerStockGet(ply_work, (short)1);
                    AppMain.GmSoundPlayJingle1UP(true);
                }
            }
        }
        else
        {
            if (ringNum >= (short)50 || ply_work.ring_num < (short)50)
                return;
            AppMain.GmPlayerStockGet(ply_work, (short)1);
            AppMain.GmSoundPlayJingle1UP(true);
        }
    }

    private static void GmPlayerRingDec(AppMain.GMS_PLAYER_WORK ply_work, short dec_ring)
    {
        if (AppMain.GMM_MAIN_USE_SUPER_SONIC())
            return;
        ply_work.ring_num -= dec_ring;
        ply_work.ring_num = (short)AppMain.MTM_MATH_CLIP((int)ply_work.ring_num, 0, 999);
        ply_work.ring_stage_num -= dec_ring;
        ply_work.ring_stage_num = (short)AppMain.MTM_MATH_CLIP((int)ply_work.ring_stage_num, 0, 9999);
    }

    private static void GmPlayerStockGet(AppMain.GMS_PLAYER_WORK ply_work, short add_stock)
    {
        if (AppMain.g_gs_main_sys_info.game_mode == 1 && ((ushort)21 > AppMain.g_gs_main_sys_info.stage_id || AppMain.g_gs_main_sys_info.stage_id > (ushort)27))
            return;
        AppMain.g_gm_main_system.player_rest_num[(int)ply_work.player_id] += (uint)add_stock;
        AppMain.g_gm_main_system.player_rest_num[(int)ply_work.player_id] = AppMain.MTM_MATH_CLIP(AppMain.g_gm_main_system.player_rest_num[(int)ply_work.player_id], 0U, 1000U);
        AppMain.HgTrophyTryAcquisition(5);
    }

    private static void GmPlayerAddScore(
      AppMain.GMS_PLAYER_WORK ply_work,
      int score,
      int pos_x,
      int pos_y)
    {
        ply_work.score += (uint)score;
        AppMain.GmScoreCreateScore(score, pos_x, pos_y, 4096, 0);
    }

    private static void GmPlayerAddScoreNoDisp(AppMain.GMS_PLAYER_WORK ply_work, int score)
    {
        ply_work.score += (uint)score;
    }

    private static void GmPlayerComboScore(AppMain.GMS_PLAYER_WORK ply_work, int pos_x, int pos_y)
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
        uint num = ply_work.score_combo_cnt != 0U ? (ply_work.score_combo_cnt - 1U < 5U ? ply_work.score_combo_cnt - 1U : 4U) : 0U;
        int score = AppMain.gm_ply_score_combo_tbl[(int)num];
        ply_work.score += (uint)score;
        AppMain.GmScoreCreateScore(score, pos_x, pos_y, AppMain.gm_ply_score_combo_scale_tbl[(int)num], AppMain.gm_ply_score_combo_vib_level_tbl[(int)num]);
    }

    private static void GmPlayerItemHiSpeedSet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.hi_speed_timer = 3686400;
        AppMain.GmPlayerSpdParameterSet(ply_work);
        AppMain.GmSoundChangeSpeedupBGM();
    }

    private static void GmPlayerItemInvincibleSet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmSoundPlayJingleInvincible();
        if (ply_work.genocide_timer == 0)
            AppMain.GmPlyEfctCreateInvincible(ply_work);
        ply_work.genocide_timer = 4091904;
    }

    private static void GmPlayerItemRing10Set(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerRingGet(ply_work, (short)10);
    }

    private static void GmPlayerItemBarrierSet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 268435456) == 0)
        {
            AppMain.GmPlyEfctCreateBarrier(ply_work);
            AppMain.GmSoundPlaySE("Barrier");
        }
        ply_work.player_flag |= 268435456U;
    }

    private static void GmPlayerItem1UPSet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerStockGet(ply_work, (short)1);
        AppMain.GmSoundPlayJingle1UP(true);
    }

    private static void GmPlayerActionChange(AppMain.GMS_PLAYER_WORK ply_work, int act_state)
    {
        ply_work.prev_act_state = ply_work.act_state;
        ply_work.act_state = act_state;
        ply_work.obj_work.obj_3d = ply_work.obj_3d[(int)AppMain.g_gm_player_model_tbl[(int)ply_work.char_id][act_state]];
        ply_work.obj_work.obj_3d.motion._object = ply_work.obj_work.obj_3d._object;
        int id = ((int)ply_work.obj_work.disp_flag & 1) == 0 ? (int)AppMain.g_gm_player_motion_right_tbl[(int)ply_work.char_id][act_state] : (int)AppMain.g_gm_player_motion_left_tbl[(int)ply_work.char_id][act_state];
        if (ply_work.prev_act_state != -1 && (int)AppMain.g_gm_player_model_tbl[(int)ply_work.char_id][act_state] == (int)AppMain.g_gm_player_model_tbl[(int)ply_work.char_id][ply_work.prev_act_state] && (AppMain.g_gm_player_mtn_blend_setting_tbl[(int)ply_work.char_id][ply_work.prev_act_state] != (byte)0 && AppMain.g_gm_player_mtn_blend_setting_tbl[(int)ply_work.char_id][act_state] != (byte)0))
        {
            AppMain.ObjDrawObjectActionSet3DNNBlend(ply_work.obj_work, id);
            if (act_state == 26 || act_state == 27)
                ply_work.obj_work.obj_3d.blend_spd = 0.125f;
            else if (19 <= ply_work.prev_act_state && ply_work.prev_act_state < 22 && (act_state == 40 || act_state == 42))
                ply_work.obj_work.obj_3d.blend_spd = 0.125f;
            else if (ply_work.prev_act_state == 0 && act_state == 19)
                ply_work.obj_work.obj_3d.blend_spd = 0.125f;
            else if (ply_work.prev_seq_state == 20)
                ply_work.obj_work.obj_3d.blend_spd = 0.08333334f;
            else
                ply_work.obj_work.obj_3d.blend_spd = 0.25f;
        }
        else
            AppMain.ObjDrawObjectActionSet3DNN(ply_work.obj_work, id, 0);
    }

    private static void GmPlayerSaveResetAction(
      AppMain.GMS_PLAYER_WORK ply_work,
      AppMain.GMS_PLAYER_RESET_ACT_WORK reset_act_work)
    {
        reset_act_work.frame[0] = ply_work.obj_work.obj_3d.frame[0];
        reset_act_work.frame[1] = ply_work.obj_work.obj_3d.frame[1];
        reset_act_work.blend_spd = ply_work.obj_work.obj_3d.blend_spd;
        reset_act_work.marge = ply_work.obj_work.obj_3d.marge;
        reset_act_work.obj_3d_flag = ply_work.obj_work.obj_3d.flag;
    }

    private static void GmPlayerResetAction(
      AppMain.GMS_PLAYER_WORK ply_work,
      AppMain.GMS_PLAYER_RESET_ACT_WORK reset_act_work)
    {
        int[] numArray1 = AppMain.New<int>(2);
        float[] numArray2 = new float[2];
        numArray1[0] = ply_work.act_state;
        numArray1[1] = ply_work.prev_act_state;
        uint dispFlag = ply_work.obj_work.disp_flag;
        AppMain.GmPlayerActionChange(ply_work, numArray1[1]);
        AppMain.GmPlayerActionChange(ply_work, numArray1[0]);
        ply_work.obj_work.obj_3d.frame[0] = reset_act_work.frame[0];
        ply_work.obj_work.obj_3d.frame[1] = reset_act_work.frame[1];
        ply_work.obj_work.obj_3d.blend_spd = reset_act_work.blend_spd;
        ply_work.obj_work.obj_3d.marge = reset_act_work.marge;
        ply_work.obj_work.obj_3d.flag &= 4294967294U;
        ply_work.obj_work.obj_3d.flag |= reset_act_work.obj_3d_flag & 1U;
        ply_work.obj_work.disp_flag |= dispFlag & 12U;
        for (int index = 0; index < 2; ++index)
        {
            numArray2[index] = AppMain.amMotionGetEndFrame(ply_work.obj_work.obj_3d.motion, ply_work.obj_work.obj_3d.act_id[index]) - AppMain.amMotionGetStartFrame(ply_work.obj_work.obj_3d.motion, ply_work.obj_work.obj_3d.act_id[index]);
            if ((double)ply_work.obj_work.obj_3d.frame[index] >= (double)numArray2[index])
                ply_work.obj_work.obj_3d.frame[index] = 0.0f;
        }
    }

    private static void GmPlayerWalkActionSet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int num = AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
        bool flag = false;
        short z = (short)ply_work.obj_work.dir.z;
        if (((int)ply_work.obj_work.disp_flag & 1) == 0 && z > (short)4096 || ((int)ply_work.obj_work.disp_flag & 1) != 0 && z < (short)-4096 && ((int)ply_work.gmk_flag & 131072) == 0)
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
            if (((int)ply_work.player_flag & 512) == 0)
                AppMain.GmPlyEfctCreateRunDust(ply_work);
        }
        else if (num < ply_work.spd3)
        {
            act_state = 21;
            if (((int)ply_work.player_flag & 512) == 0)
                AppMain.GmPlyEfctCreateDash1Dust(ply_work);
        }
        else if (num < ply_work.spd4 && (flag || ply_work.maxdash_timer != 0))
        {
            act_state = 22;
            AppMain.GmPlyEfctCreateRollDash(ply_work);
            if (((int)ply_work.player_flag & 512) == 0)
                AppMain.GmPlyEfctCreateDash2Dust(ply_work);
            AppMain.GmPlyEfctCreateDash2Impact(ply_work);
            AppMain.GmPlyEfctCreateSuperAuraDash(ply_work);
        }
        else
        {
            act_state = 21;
            if (((int)ply_work.player_flag & 512) == 0)
                AppMain.GmPlyEfctCreateDash1Dust(ply_work);
        }
        AppMain.GmPlayerActionChange(ply_work, act_state);
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlayerWalkActionCheck(AppMain.GMS_PLAYER_WORK ply_work)
    {
        bool flag = false;
        short z = (short)ply_work.obj_work.dir.z;
        int num = AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
        if ((((int)ply_work.obj_work.disp_flag & 1) == 0 && z > (short)4096 || ((int)ply_work.obj_work.disp_flag & 1) != 0 && z < (short)-4096) && ((int)ply_work.gmk_flag & 131072) == 0)
        {
            flag = true;
            ply_work.maxdash_timer = 122880;
        }
        if (ply_work.act_state < 19 || ply_work.act_state > 22)
            AppMain.GmPlayerActionChange(ply_work, 19);
        if (((int)ply_work.obj_work.disp_flag & 8) != 0)
        {
            if (ply_work.act_state == 19)
            {
                if (num >= ply_work.spd2)
                {
                    AppMain.GmPlayerActionChange(ply_work, 20);
                    if (((int)ply_work.player_flag & 512) == 0)
                        AppMain.GmPlyEfctCreateRunDust(ply_work);
                }
            }
            else if (ply_work.act_state == 20)
            {
                if (num >= ply_work.spd3)
                {
                    AppMain.GmPlayerActionChange(ply_work, 21);
                    if (((int)ply_work.player_flag & 512) == 0)
                        AppMain.GmPlyEfctCreateDash1Dust(ply_work);
                }
                else if (num < ply_work.spd2)
                    AppMain.GmPlayerActionChange(ply_work, 19);
            }
            else if (ply_work.act_state == 21)
            {
                if (num >= ply_work.spd_max && (flag || ply_work.maxdash_timer != 0))
                {
                    AppMain.GmPlayerActionChange(ply_work, 22);
                    AppMain.GmPlyEfctCreateRollDash(ply_work);
                    if (((int)ply_work.player_flag & 512) == 0)
                        AppMain.GmPlyEfctCreateDash2Dust(ply_work);
                    AppMain.GmPlyEfctCreateDash2Impact(ply_work);
                }
                else if (num < ply_work.spd3)
                {
                    AppMain.GmPlayerActionChange(ply_work, 20);
                    if (((int)ply_work.player_flag & 512) == 0)
                        AppMain.GmPlyEfctCreateRunDust(ply_work);
                }
            }
            else if (ply_work.act_state == 22 && (num < ply_work.spd_max || !flag && ply_work.maxdash_timer == 0))
            {
                AppMain.GmPlayerActionChange(ply_work, 21);
                if (((int)ply_work.player_flag & 512) == 0)
                    AppMain.GmPlyEfctCreateDash1Dust(ply_work);
            }
        }
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlayerAnimeSpeedSetWalk(AppMain.GMS_PLAYER_WORK ply_work, int spd_set)
    {
        int a = AppMain.MTM_MATH_ABS((spd_set >> 3) + (spd_set >> 2));
        if (a <= 4096)
            a = 4096;
        if (a >= 32768)
            a = 32768;
        if (ply_work.act_state == 22)
            a = 4096;
        else if ((ply_work.act_state == 26 || ply_work.act_state == 27) && (((int)ply_work.obj_work.obj_3d.flag & 1) != 0 && a > 4096))
            a = 4096;
        if (ply_work.obj_work.obj_3d == null)
            return;
        ply_work.obj_work.obj_3d.speed[0] = AppMain.FXM_FX32_TO_FLOAT(a);
        ply_work.obj_work.obj_3d.speed[1] = AppMain.FXM_FX32_TO_FLOAT(a);
    }

    private static void GmPlayerSpdSet(AppMain.GMS_PLAYER_WORK ply_work, int spd_x, int spd_y)
    {
        ply_work.no_spddown_timer = 524288;
        if (spd_x < 0)
            ply_work.obj_work.disp_flag |= 1U;
        else
            ply_work.obj_work.disp_flag &= 4294967294U;
        if (((int)ply_work.obj_work.move_flag & 16) != 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd.x > spd_x || ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd.x < spd_x)
                ply_work.obj_work.spd.x = spd_x;
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.y) >= AppMain.MTM_MATH_ABS(spd_y))
                return;
            ply_work.obj_work.spd.y = spd_y;
        }
        else
        {
            switch (((int)ply_work.obj_work.dir.z + 8192 & 49152) >> 6)
            {
                case 0:
                case 2:
                    if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd_m > spd_x || ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd_m < spd_x)
                        ply_work.obj_work.spd_m = spd_x;
                    if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.y) >= AppMain.MTM_MATH_ABS(spd_y))
                        break;
                    ply_work.obj_work.spd.y = spd_y;
                    if (ply_work.obj_work.spd.y >= 0)
                        break;
                    ply_work.obj_work.move_flag |= 16U;
                    break;
                case 1:
                    if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd_m > spd_y || ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd_m < spd_y)
                        ply_work.obj_work.spd_m = spd_y;
                    if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) >= AppMain.MTM_MATH_ABS(spd_x))
                        break;
                    ply_work.obj_work.spd.x = spd_x;
                    break;
                case 3:
                    if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd_m > -spd_y || ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd_m < -spd_y)
                        ply_work.obj_work.spd_m = -spd_y;
                    if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) >= AppMain.MTM_MATH_ABS(spd_x))
                        break;
                    ply_work.obj_work.spd.x = spd_x;
                    break;
            }
        }
    }

    private static void GmPlayerSetReverse(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag &= 2147483375U;
        ply_work.pgm_turn_dir = (ushort)0;
        ply_work.pgm_turn_spd = (ushort)0;
        ply_work.obj_work.disp_flag ^= 1U;
        if ((int)AppMain.g_gm_player_motion_left_tbl[(int)ply_work.char_id][ply_work.act_state] == (int)AppMain.g_gm_player_motion_right_tbl[(int)ply_work.char_id][ply_work.act_state])
            return;
        float[] numArray = new float[2];
        uint num = ply_work.obj_work.disp_flag & 12U;
        numArray[0] = ply_work.obj_work.obj_3d.frame[0];
        AppMain.GmPlayerActionChange(ply_work, ply_work.act_state);
        ply_work.obj_work.obj_3d.frame[0] = numArray[0];
        ply_work.obj_work.obj_3d.marge = 0.0f;
        ply_work.obj_work.obj_3d.flag &= 4294967294U;
        ply_work.obj_work.disp_flag |= num;
    }

    private static void GmPlayerSetReverseOnlyState(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag &= 2147483375U;
        ply_work.pgm_turn_dir = (ushort)0;
        ply_work.pgm_turn_spd = (ushort)0;
        ply_work.obj_work.disp_flag ^= 1U;
    }

    private static bool GmPlayerKeyCheckWalkLeft(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            if (((int)ply_work.key_on & 4) != 0 || ply_work.key_rot_z < 0)
                return true;
        }
        else if (((int)AppMain.g_gs_main_sys_info.game_flag & 1) != 0)
        {
            if (((int)ply_work.key_on & 4) != 0)
                return true;
        }
        else if (((int)ply_work.key_on & 4) != 0 || ply_work.key_walk_rot_z < 0)
            return true;
        return false;
    }

    private static bool GmPlayerKeyCheckWalkRight(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            if (((int)ply_work.key_on & 8) != 0 || ply_work.key_rot_z > 0)
                return true;
        }
        else if (((int)AppMain.g_gs_main_sys_info.game_flag & 1) != 0)
        {
            if (((int)ply_work.key_on & 8) != 0)
                return true;
        }
        else if (((int)ply_work.key_on & 8) != 0 || ply_work.key_walk_rot_z > 0)
            return true;
        return false;
    }

    private static bool GmPlayerKeyCheckJumpKeyOn(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ((int)ply_work.key_on & 160) != 0;
    }

    private static bool GmPlayerKeyCheckJumpKeyPush(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ((int)ply_work.key_push & 160) != 0;
    }

    private static int GmPlayerKeyGetGimmickRotZ(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ((int)AppMain.g_gs_main_sys_info.game_flag & 1) == 0 ? ply_work.key_rot_z : ply_work.key_walk_rot_z;
    }

    private static bool GmPlayerKeyCheckTransformKeyPush(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ((int)ply_work.key_push & 80) != 0;
    }

    private static void GmPlayerSetLight(AppMain.NNS_VECTOR light_vec, ref AppMain.NNS_RGBA light_col)
    {
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.nnNormalizeVector(nnsVector, light_vec);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_6, ref light_col, 1f, nnsVector);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    private static void GmPlayerSetDefRimParam(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private void GmPlayerSetRimParam(AppMain.GMS_PLAYER_WORK ply_work, AppMain.NNS_RGB toon_rim_param)
    {
    }

    private static bool GmPlayerCheckGimmickEnable(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ply_work.gmk_obj != null && ply_work.gmk_obj.obj_type == (ushort)3 && ((AppMain.GMS_ENEMY_COM_WORK)ply_work.gmk_obj).target_obj == (AppMain.OBS_OBJECT_WORK)ply_work;
    }

    private static bool GmPlayerIsTransformSuperSonic(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ((int)AppMain.g_gm_main_system.game_flag & 1048576) == 0 && ((int)ply_work.player_flag & 1049600) == 0 && (!AppMain.GSM_MAIN_STAGE_IS_SPSTAGE() && ((int)AppMain.g_gs_main_sys_info.game_flag & 32) != 0) && (ply_work.ring_num >= (short)50 && ((int)ply_work.player_flag & 16384) == 0);
    }

    private static void GmPlayerCameraOffsetSet(
      AppMain.GMS_PLAYER_WORK ply_work,
      short ofs_x,
      short ofs_y)
    {
        ply_work.gmk_camera_center_ofst_x = ofs_x;
        ply_work.gmk_camera_center_ofst_y = ofs_y;
    }

    private static bool GmPlayerIsStateWait(AppMain.GMS_PLAYER_WORK ply_work)
    {
        bool flag = false;
        if (ply_work.act_state >= 2 && ply_work.act_state <= 7)
            flag = true;
        return flag;
    }

    private static bool gmPlayerObjRelease(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d = (AppMain.OBS_ACTION3D_NN_WORK)null;
        return true;
    }

    private static bool gmPlayerObjReleaseWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = (AppMain.GMS_PLAYER_WORK)obj_work;
        AppMain.ObjAction3dNNMotionRelease(gmsPlayerWork.obj_3d_work[0]);
        for (int index = 1; index < 8; ++index)
            gmsPlayerWork.obj_3d_work[index].motion = (AppMain.AMS_MOTION)null;
        return false;
    }

    private static void gmPlayerExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_PLAYER_WORK tcbWork = (AppMain.GMS_PLAYER_WORK)AppMain.mtTaskGetTcbWork(tcb);
        AppMain.g_gm_main_system.ply_work[(int)tcbWork.player_id] = (AppMain.GMS_PLAYER_WORK)null;
        AppMain.ObjObjectExit(tcb);
    }

    private static void gmPlayerMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)obj_work;
        if (ply_work.spin_se_timer > (short)0)
            --ply_work.spin_se_timer;
        if (ply_work.spin_back_se_timer > (short)0)
            --ply_work.spin_back_se_timer;
        AppMain.GmPlySeqMain(ply_work);
    }

    private static void gmPlayerDispFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        ushort num1 = 0;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = (AppMain.GMS_PLAYER_WORK)obj_work;
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx_r);
        if (((int)gmsPlayerWork.gmk_flag & 32768) != 0)
            AppMain.nnMultiplyMatrix(obj3d.user_obj_mtx_r, obj3d.user_obj_mtx_r, gmsPlayerWork.ex_obj_mtx_r);
        float num2 = 0.0f;
        float num3 = -15f;
        if (((int)gmsPlayerWork.player_flag & 131072) != 0 && (26 > gmsPlayerWork.act_state || gmsPlayerWork.act_state > 30) || AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            num3 = -21f;
        else if (((int)gmsPlayerWork.player_flag & 262144) != 0 && ((int)gmsPlayerWork.gmk_flag2 & 64) == 0)
        {
            num2 = 0.0f;
            num3 = 0.0f;
        }
        AppMain.nnTranslateMatrix(obj3d.user_obj_mtx_r, obj3d.user_obj_mtx_r, 0.0f, num3 / AppMain.FXM_FX32_TO_FLOAT(AppMain.g_obj.draw_scale.y), num2 / AppMain.FXM_FX32_TO_FLOAT(AppMain.g_obj.draw_scale.x));
        if (((int)gmsPlayerWork.player_flag & -2147483376) != 0)
        {
            num1 = gmsPlayerWork.obj_work.dir.y;
            gmsPlayerWork.obj_work.dir.y += gmsPlayerWork.pgm_turn_dir;
        }
        AppMain.ObjDrawActionSummary(obj_work);
        if (((int)gmsPlayerWork.player_flag & -2147483376) == 0)
            return;
        gmsPlayerWork.obj_work.dir.y = num1;
    }

    private static void gmPlayerDefaultInFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)obj_work;
        AppMain.gmPlayerKeyGet(ply_work);
        AppMain.gmPlayerWaterCheck(ply_work);
        AppMain.gmPlayerTimeOverCheck(ply_work);
        AppMain.gmPlayerFallDownCheck(ply_work);
        AppMain.gmPlayerPressureCheck(ply_work);
        AppMain.gmPlayerGetHomingTarget(ply_work);
        AppMain.gmPlayerSuperSonicCheck(ply_work);
        AppMain.gmPlayerPushSet(ply_work);
        AppMain.gmPlayerEarthTouch(ply_work);
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            ply_work.truck_prev_dir = ply_work.obj_work.dir.z;
            ply_work.truck_prev_dir_fall = ply_work.obj_work.dir_fall;
        }
        if (ply_work.gmk_obj != null && ((int)ply_work.gmk_obj.flag & 4) != 0)
            ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
        if (ply_work.invincible_timer != 0)
        {
            ply_work.invincible_timer = AppMain.ObjTimeCountDown(ply_work.invincible_timer);
            if ((ply_work.invincible_timer & 16384) != 0)
                ply_work.obj_work.disp_flag |= 32U;
            else
                ply_work.obj_work.disp_flag &= 4294967263U;
            if (ply_work.invincible_timer == 0)
            {
                ply_work.obj_work.disp_flag &= 4294967263U;
                AppMain.GmPlayerSetDefNormal(ply_work);
            }
        }
        if (ply_work.disapprove_item_catch_timer != 0)
            ply_work.disapprove_item_catch_timer = AppMain.ObjTimeCountDown(ply_work.disapprove_item_catch_timer);
        if (ply_work.genocide_timer != 0)
            ply_work.genocide_timer = AppMain.ObjTimeCountDown(ply_work.genocide_timer);
        if (ply_work.genocide_timer != 0 || ((int)ply_work.player_flag & 16384) != 0)
        {
            ply_work.water_timer = 0;
            AppMain.OBS_RECT_WORK obsRectWork = ply_work.rect_work[2];
            obsRectWork.hit_flag |= (ushort)2;
            obsRectWork.hit_power = (short)3;
            ply_work.rect_work[0].def_flag = ushort.MaxValue;
        }
        else if (((int)ply_work.rect_work[2].hit_flag & 2) != 0)
        {
            ply_work.obj_work.disp_flag &= 4294967263U;
            AppMain.OBS_RECT_WORK obsRectWork = ply_work.rect_work[2];
            ushort num = 65533;
            obsRectWork.hit_flag &= num;
            obsRectWork.hit_power = (short)1;
            ply_work.rect_work[0].def_flag = (ushort)65533;
            AppMain.GmSoundStopJingleInvincible();
        }
        if (ply_work.hi_speed_timer != 0)
        {
            ply_work.hi_speed_timer = AppMain.ObjTimeCountDown(ply_work.hi_speed_timer);
            if (ply_work.hi_speed_timer == 0)
                AppMain.GmPlayerSpdParameterSet(ply_work);
        }
        if (ply_work.homing_timer != 0)
            ply_work.homing_timer = AppMain.ObjTimeCountDown(ply_work.homing_timer);
        if (((int)ply_work.player_flag & 262144) == 0)
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
        if (ply_work.jump_pseudofall_eve_id_set == (ushort)0)
            ply_work.jump_pseudofall_eve_id_cur = ply_work.jump_pseudofall_eve_id_wait;
        ply_work.jump_pseudofall_eve_id_set = (ushort)0;
        ply_work.jump_pseudofall_eve_id_wait = (ushort)0;
        if ((((int)ply_work.gmk_flag2 & 256) == 0 || ((int)ply_work.obj_work.move_flag & 1) != 0) && ply_work.seq_state != 39)
        {
            int num1 = -ply_work.key_rot_z;
            int num2;
            if (((int)AppMain.g_gs_main_sys_info.game_flag & 512) != 0)
            {
                num2 = num1 * 38 / 10;
                if (AppMain.fall_rot_buf_gmPlayerDefaultInFunc > 0 && num2 >= 0 || AppMain.fall_rot_buf_gmPlayerDefaultInFunc < 0 && num2 <= 0)
                    num2 += AppMain.fall_rot_buf_gmPlayerDefaultInFunc;
                AppMain.fall_rot_buf_gmPlayerDefaultInFunc = 0;
                if (num2 > 5120)
                {
                    AppMain.fall_rot_buf_gmPlayerDefaultInFunc += num2 - 5120;
                    if (AppMain.fall_rot_buf_gmPlayerDefaultInFunc > 49152)
                        AppMain.fall_rot_buf_gmPlayerDefaultInFunc = 49152;
                    num2 = 5120;
                }
                else if (num2 < -5120)
                {
                    AppMain.fall_rot_buf_gmPlayerDefaultInFunc += num2 + 5120;
                    if (AppMain.fall_rot_buf_gmPlayerDefaultInFunc < -49152)
                        AppMain.fall_rot_buf_gmPlayerDefaultInFunc = -49152;
                    num2 = -5120;
                }
            }
            else
                num2 = num1 / 24;
            ushort num3 = (ushort)((uint)ply_work.obj_work.dir.z + ((uint)ply_work.obj_work.dir_fall - (uint)AppMain.g_gm_main_system.pseudofall_dir));
            int num4 = 60075;
            int num5 = 5460;
            if ((int)num3 > num5 && (int)num3 < num4)
            {
                if (num3 > (ushort)32768 && num2 > 0)
                    num2 = 0;
                else if (num3 <= (ushort)32768 && num2 <= 0)
                    num2 = 0;
            }
            if (((int)ply_work.gmk_flag & 262144) != 0 && ((int)ply_work.gmk_flag & 1073741824) == 0)
                num2 = 0;
            if (((int)AppMain.g_gm_main_system.game_flag & 16384) != 0 || ((int)ply_work.player_flag & 1048576) != 0)
                num2 = (short)num3 >= (short)0 ? ((short)num3 <= (short)0 ? 0 : (int)(short)num3) : (int)(short)num3;
            int num6 = num2;
            if (num6 > 32768)
                num6 -= 65536;
            else if (num6 < (int)short.MinValue)
                num6 += 65536;
            int a = num6;
            if (AppMain.MTM_MATH_ABS(a) > 5120)
                a = a < 0 ? -5120 : 5120;
            ply_work.ply_pseudofall_dir += a;
            AppMain.g_gm_main_system.pseudofall_dir = (ushort)ply_work.ply_pseudofall_dir;
        }
        ply_work.prev_dir_fall = obj_work.dir_fall;
        obj_work.dir_fall = ((int)obj_work.move_flag & 1) == 0 ? (ushort)((int)ply_work.jump_pseudofall_dir + 8192 & 49152) : (ushort)((int)AppMain.g_gm_main_system.pseudofall_dir + 8192 & 49152);
        if ((int)ply_work.prev_dir_fall != (int)obj_work.dir_fall)
            ply_work.obj_work.dir.z -= (ushort)((uint)obj_work.dir_fall - (uint)ply_work.prev_dir_fall);
        int num7 = 60075;
        int num8 = 5460;
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
        {
            ushort num1 = (ushort)((uint)ply_work.obj_work.dir.z + ((uint)ply_work.obj_work.dir_fall - (uint)AppMain.g_gm_main_system.pseudofall_dir));
            if ((int)num1 > num8 && (int)num1 < num7)
            {
                if (num1 > (ushort)32768)
                    ply_work.ply_pseudofall_dir -= num7 - (int)num1;
                else if (num1 <= (ushort)32768)
                    ply_work.ply_pseudofall_dir += (int)num1 - num8;
            }
        }
        ushort num9 = (ushort)((uint)ply_work.obj_work.dir.z + ((uint)ply_work.obj_work.dir_fall - (uint)AppMain.g_gm_main_system.pseudofall_dir));
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
        {
            if (((int)ply_work.gmk_flag2 & 24) == 0 && (ushort)27392 < num9 && num9 < (ushort)38144)
            {
                if ((int)ply_work.truck_prev_dir == (int)obj_work.dir.z)
                {
                    ply_work.obj_work.spd_m = 0;
                    if (((int)ply_work.gmk_flag & 262144) == 0)
                    {
                        ply_work.gmk_flag |= 262144U;
                        AppMain.GmPlySeqGmkInitTruckDanger(ply_work, ply_work.truck_obj);
                        AppMain.GmPlayerSpdParameterSet(ply_work);
                    }
                }
            }
            else if (((int)ply_work.gmk_flag & 262144) == 0)
                ply_work.truck_stick_prev_dir = num9;
        }
        if (((int)ply_work.gmk_flag & 1074003968) == 1074003968 && (((int)ply_work.obj_work.move_flag & 1) == 0 || (ushort)27392 >= num9 || num9 >= (ushort)38144))
        {
            ply_work.player_flag |= 1U;
            AppMain.GmPlayerSpdParameterSet(ply_work);
        }
        if (((int)ply_work.gmk_flag & int.MinValue) == 0)
            return;
        ply_work.gmk_flag &= 1073479679U;
        ply_work.obj_work.vib_timer = 0;
        AppMain.GmPlayerSetDefNormal(ply_work);
        AppMain.GmPlayerSpdParameterSet(ply_work);
    }

    private static void gmPlayerSplStgInFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)obj_work;
        AppMain.gmPlayerKeyGet(ply_work);
        AppMain.gmPlayerTimeOverCheck(ply_work);
        AppMain.gmPlayerEarthTouch(ply_work);
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
        {
            AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
            AppMain.g_gm_main_system.pseudofall_dir = (ushort)-obsCamera.roll;
            ply_work.prev_dir_fall2 = ply_work.prev_dir_fall;
            ply_work.prev_dir_fall = obj_work.dir_fall;
            obj_work.dir_fall = (ushort)((int)AppMain.g_gm_main_system.pseudofall_dir + 8192 & 49152);
            ply_work.jump_pseudofall_dir = AppMain.g_gm_main_system.pseudofall_dir;
        }
        if (ply_work.gmk_obj == null || ((int)ply_work.gmk_obj.flag & 4) == 0)
            return;
        ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmPlayerRectTruckFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)obj_work;
        if ((((int)obj_work.move_flag & 1) != 0 && AppMain.MTM_MATH_ABS(obj_work.spd_m) > 256 || ((int)obj_work.move_flag & 1) == 0) && (((int)ply_work.player_flag & 1024) == 0 && ply_work.seq_state != 22))
            AppMain.GmPlayerSetAtk(ply_work);
        else
            ply_work.rect_work[1].flag &= 4294967291U;
    }

    private static void gmPlayerDefaultLastFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)obj_work;
        AppMain.gmPlayerCameraOffset(ply_work);
        if (ply_work.gmk_obj == null || ((int)ply_work.gmk_flag & 8) != 0)
            obj_work.move_flag &= 4294950911U;
        if (((int)ply_work.player_flag & 32768) == 0 || obj_work.pos.x >> 12 <= AppMain.g_gm_main_system.map_fcol.left + 128)
            return;
        obj_work.move_flag |= 16384U;
    }

    private static void gmPlayerTruckCollisionFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        bool flag = false;
        uint num1 = 0;
        int v1 = 0;
        AppMain.VecFx32 vecFx32_1 = new AppMain.VecFx32();
        AppMain.VecFx32 vecFx32_2 = new AppMain.VecFx32();
        AppMain.VecFx32 vecFx32_3 = new AppMain.VecFx32();
        ushort num2 = 0;
        ushort num3 = 0;
        if (((int)obj_work.move_flag & 1) != 0 && ((int)obj_work.move_flag & 16) == 0 && AppMain.MTM_MATH_ABS(obj_work.spd_m) >= 16384)
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
        if (AppMain.g_obj.ppCollision != null)
            AppMain.g_obj.ppCollision(obj_work);
        if (flag && ((int)obj_work.move_flag & 1) != 0)
        {
            ushort num4 = (ushort)((uint)obj_work.dir.z - (uint)num2);
            if (v1 > 0)
            {
                if ((ushort)4096 > num4 || num4 > (ushort)16384)
                    return;
            }
            else if ((ushort)61440 < num4 || num4 < (ushort)32768)
                return;
            obj_work.move_flag = num1;
            obj_work.spd_m = AppMain.FX_Mul(v1, 4096);
            obj_work.spd.Assign(vecFx32_1);
            obj_work.pos.Assign(vecFx32_2);
            obj_work.move.Assign(vecFx32_3);
            obj_work.dir.z = num2;
            obj_work.dir_fall = num3;
            obj_work.move_flag = num1 & 4294967294U;
            ((AppMain.GMS_PLAYER_WORK)obj_work).gmk_flag2 |= 1U;
        }
        if (((int)obj_work.move_flag & 4194305) != 4194305)
            return;
        if ((int)obj_work.dir_fall != (int)((AppMain.GMS_PLAYER_WORK)obj_work).truck_prev_dir_fall)
        {
            ((AppMain.GMS_PLAYER_WORK)obj_work).truck_prev_dir = (ushort)((uint)((AppMain.GMS_PLAYER_WORK)obj_work).truck_prev_dir + (uint)((AppMain.GMS_PLAYER_WORK)obj_work).truck_prev_dir_fall - (uint)obj_work.dir_fall);
            ((AppMain.GMS_PLAYER_WORK)obj_work).truck_prev_dir_fall = obj_work.dir_fall;
        }
        if (AppMain.MTM_MATH_ABS((int)((AppMain.GMS_PLAYER_WORK)obj_work).truck_prev_dir - (int)obj_work.dir.z) <= 1024)
            return;
        obj_work.dir.z = AppMain.ObjRoopMove16(((AppMain.GMS_PLAYER_WORK)obj_work).truck_prev_dir, obj_work.dir.z, (short)1024);
    }

    private static void gmPlayerAtkFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
    }

    private static void gmPlayerDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        if (gs.backup.SSave.CreateInstance().GetDebug().GodMode)
            return;

        AppMain.GMS_PLAYER_WORK parentObj1 = (AppMain.GMS_PLAYER_WORK)mine_rect.parent_obj;
        AppMain.HgTrophyIncPlayerDamageCount(parentObj1);
        if (((int)parentObj1.obj_work.move_flag & 32768) != 0)
        {
            int x = parentObj1.obj_work.spd.x;
        }
        else
        {
            int spdM = parentObj1.obj_work.spd_m;
        }
        if (match_rect.parent_obj.obj_type == (ushort)3)
        {
            AppMain.GMS_ENEMY_COM_WORK parentObj2 = (AppMain.GMS_ENEMY_COM_WORK)match_rect.parent_obj;
            if ((ushort)91 <= parentObj2.eve_rec.id && parentObj2.eve_rec.id <= (ushort)94 || (parentObj2.eve_rec.id == (ushort)97 || parentObj2.eve_rec.id == (ushort)98))
                AppMain.GmSoundPlaySE("Damage2");
        }
        if (((int)parentObj1.player_flag & 268435456) == 0 && parentObj1.ring_num == (short)0)
        {
            AppMain.GmPlySeqChangeDeath(parentObj1);
        }
        else
        {
            int gmkFlag = (int)parentObj1.gmk_flag;
            AppMain.GmPlayerStateInit(parentObj1);
            if (((int)parentObj1.player_flag & 268435456) == 0)
            {
                if (parentObj1.ring_num != (short)0)
                    AppMain.GmSoundPlaySE("Ring2");
                AppMain.GmRingDamageSet(parentObj1);
                AppMain.GmComEfctCreateHitEnemy(parentObj1.obj_work, ((int)mine_rect.rect.left + (int)mine_rect.rect.right) * 4096 / 2, ((int)mine_rect.rect.top + (int)mine_rect.rect.bottom) * 4096 / 2);
            }
            if (((int)parentObj1.player_flag & 805306368) != 0)
                AppMain.GmSoundPlaySE("Damage1");
            parentObj1.player_flag &= 3489660927U;
            parentObj1.invincible_timer = parentObj1.time_damage;
            AppMain.GmPlySeqChangeDamage(parentObj1);
        }
    }

    private static void gmPlayerPushSet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.seq_state == 18 && ply_work.act_state == 18)
            ply_work.obj_work.move_flag |= 16777216U;
        else
            ply_work.obj_work.move_flag &= 4278190079U;
    }

    private static void gmPlayerEarthTouch(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.gmk_flag & 1) == 0)
            return;
        if (((int)ply_work.obj_work.move_flag & 15) == 0)
            return;
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
            AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        else if (((int)ply_work.obj_work.move_flag & 2) != 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                AppMain.GmPlySeqLandingSet(ply_work, (ushort)24576);
            else
                AppMain.GmPlySeqLandingSet(ply_work, (ushort)40960);
            AppMain.OBS_COL_CHK_DATA pData = AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Alloc();
            pData.pos_x = ply_work.obj_work.pos.x >> 12;
            pData.pos_y = (ply_work.obj_work.pos.y >> 12) + (int)ply_work.obj_work.field_rect[1] - 4;
            pData.flag = (ushort)(ply_work.obj_work.flag & 1U);
            pData.vec = (ushort)3;
            ushort[] numArray = new ushort[1];
            pData.dir = numArray;
            pData.attr = (uint[])null;
            ushort z = ply_work.obj_work.dir.z;
            numArray[0] = z;
            AppMain.ObjDiffCollisionFast(pData);
            ushort num = numArray[0];
            ply_work.obj_work.dir.z = num;
            AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Release(pData);
        }
        else if (((int)ply_work.obj_work.move_flag & 4) != 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                AppMain.GmPlySeqLandingSet(ply_work, (ushort)16384);
            else
                AppMain.GmPlySeqLandingSet(ply_work, (ushort)49152);
        }
        else if (((int)ply_work.obj_work.move_flag & 8) != 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                AppMain.GmPlySeqLandingSet(ply_work, (ushort)49152);
            else
                AppMain.GmPlySeqLandingSet(ply_work, (ushort)16384);
        }
        if (((int)ply_work.gmk_flag & 2048) != 0)
        {
            if (ply_work.obj_work.dir.z < (ushort)32768)
            {
                ply_work.obj_work.disp_flag |= 1U;
                ply_work.obj_work.spd_m = -AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
            }
            else
            {
                ply_work.obj_work.disp_flag &= 4294967294U;
                ply_work.obj_work.spd_m = AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
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
        AppMain.GmPlySeqChangeFw(ply_work);
    }

    private static void gmPlayerWaterCheck(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (AppMain.GmMainIsWaterLevel())
        {
            if ((ply_work.obj_work.pos.y >> 12) - -10 >= (int)AppMain.g_gm_main_system.water_level)
            {
                bool flag = false;
                if (((int)ply_work.player_flag & 67108864) == 0)
                {
                    if (((int)AppMain.g_gm_main_system.game_flag & 8192) == 0)
                    {
                        AppMain.GmPlyEfctCreateSpray(ply_work);
                        AppMain.GmPlyEfctCreateBubble(ply_work);
                        AppMain.GmSoundPlaySE("Spray");
                    }
                    AppMain.GmPlayerSpdParameterSetWater(ply_work, true);
                }
                ply_work.player_flag |= 67108864U;
                if (((int)ply_work.player_flag & 16778240) == 0)
                {
                    if ((ply_work.obj_work.pos.y >> 12) - 4 >= (int)AppMain.g_gm_main_system.water_level)
                    {
                        ply_work.water_timer = AppMain.ObjTimeCountUp(ply_work.water_timer);
                    }
                    else
                    {
                        ply_work.water_timer = 0;
                        AppMain.GmPlyEfctCreateRunSpray(ply_work);
                        flag = true;
                    }
                }
                if (((int)ply_work.player_flag & 16778240) != 0)
                    return;
                if ((ply_work.water_timer >> 12) % 50 == 0 && ply_work.water_timer < ply_work.time_air)
                    AppMain.GmPlyEfctCreateBubble(ply_work);
                if (!flag && (ply_work.water_timer >> 12) % 300 == 0)
                {
                    if (((int)ply_work.gmk_flag & 524288) == 0)
                        AppMain.GmSoundPlaySE("Attention");
                    ply_work.gmk_flag |= 524288U;
                }
                else
                    ply_work.gmk_flag &= 4294443007U;
                int num = ply_work.time_air - ply_work.water_timer;
                if (num >= 245760 && num - 245760 <= 2457600 && (num - 245760 >> 12) % 120 == 0)
                {
                    uint no = AppMain.MTM_MATH_CLIP((uint)((num - 245760 >> 12) / 120), 0U, 5U);
                    AppMain.GmPlyEfctWaterCount(ply_work, no);
                }
                if (num == 2826240)
                    AppMain.GmSoundPlayJingleObore();
                else if (num > 2826240)
                    AppMain.GmSoundStopJingleObore();
                if (ply_work.water_timer <= ply_work.time_air)
                    return;
                AppMain.GmPlySeqChangeDeath(ply_work);
                ply_work.obj_work.spd.y = 0;
                AppMain.GmPlyEfctWaterDeath(ply_work);
            }
            else
            {
                if (((int)ply_work.player_flag & 67108864) != 0)
                {
                    if (((int)AppMain.g_gm_main_system.game_flag & 8192) == 0)
                    {
                        AppMain.GmPlyEfctCreateSpray(ply_work);
                        AppMain.GmSoundPlaySE("Spray");
                    }
                    AppMain.GmSoundStopJingleObore();
                    AppMain.GmPlayerSpdParameterSetWater(ply_work, false);
                }
                ply_work.player_flag &= 4227858431U;
                ply_work.water_timer = 0;
                ply_work.obj_work.spd_fall = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_fall;
                ply_work.obj_work.spd_fall_max = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_fall_max;
            }
        }
        else
        {
            if (((int)ply_work.player_flag & 67108864) != 0)
            {
                ply_work.obj_work.spd_fall = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_fall;
                ply_work.obj_work.spd_fall_max = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_fall_max;
            }
            ply_work.player_flag &= 4227858431U;
            ply_work.water_timer = 0;
            ply_work.gmk_flag &= 4294443007U;
        }
    }

    private static void gmPlayerTimeOverCheck(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)AppMain.g_gm_main_system.game_flag & 1327676) != 0 || ((int)ply_work.player_flag & 65536) != 0)
            return;
        if (!AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            if (((int)ply_work.player_flag & 1024) != 0 || AppMain.g_gm_main_system.game_time < 35999U)
                return;
            AppMain.GmPlySeqChangeDeath(ply_work);
            AppMain.g_gm_main_system.game_flag |= 512U;
            AppMain.g_gm_main_system.time_save = 0U;
            AppMain.g_gs_main_sys_info.game_flag |= 8U;
        }
        else
        {
            if ((int)AppMain.g_gm_main_system.game_time > 0)
                return;
            AppMain.g_gm_main_system.game_flag |= 262144U;
            AppMain.g_gm_main_system.time_save = 0U;
            AppMain.g_gs_main_sys_info.game_flag |= 8U;
            ply_work.obj_work.move_flag |= 8448U;
        }
    }

    private static void gmPlayerFallDownCheck(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 1024) != 0 || AppMain.g_gm_main_system.map_size[1] - 16 << 12 > ply_work.obj_work.pos.y)
            return;
        AppMain.GmPlySeqChangeDeath(ply_work);
    }

    private static void gmPlayerPressureCheck(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 2048) != 0 && ply_work.obj_work.touch_obj == null && (((int)ply_work.obj_work.move_flag & 4) != 0 && ((int)ply_work.obj_work.move_flag & 8) != 0))
            AppMain.GmPlySeqChangeDeath(ply_work);
        if (ply_work.obj_work.touch_obj == null || ((int)ply_work.player_flag & 4096) != 0 || ply_work.obj_work.touch_obj.obj_type == (ushort)3 && (ply_work.obj_work.touch_obj.obj_type != (ushort)3 || ((int)((AppMain.GMS_ENEMY_COM_WORK)ply_work.obj_work.touch_obj).enemy_flag & 16384) != 0) || (ply_work.obj_work.ride_obj == null && ((int)ply_work.obj_work.move_flag & 1) != 0 && (((int)ply_work.obj_work.move_flag & 2) != 0 && ply_work.obj_work.touch_obj.move.y <= 0) || (((int)ply_work.obj_work.move_flag & 1) == 0 || ((int)ply_work.obj_work.move_flag & 2) == 0) && (((int)ply_work.obj_work.move_flag & 4) == 0 || ((int)ply_work.obj_work.move_flag & 8) == 0)))
            return;
        AppMain.GmPlySeqChangeDeath(ply_work);
    }

    private static void gmPlayerSuperSonicCheck(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 16384) == 0)
            return;
        ply_work.super_sonic_ring_timer = AppMain.ObjTimeCountDown(ply_work.super_sonic_ring_timer);
        if (ply_work.super_sonic_ring_timer != 0)
            return;
        --ply_work.ring_num;
        if (ply_work.ring_num <= (short)0)
        {
            ply_work.ring_num = (short)0;
            AppMain.gmPlayerSuperSonicToSonic(ply_work);
        }
        else
            ply_work.super_sonic_ring_timer = 245760;
    }

    private static void gmPlayerSuperSonicToSonic(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GMS_PLAYER_RESET_ACT_WORK reset_act_work = new AppMain.GMS_PLAYER_RESET_ACT_WORK();
        AppMain.GmPlayerSaveResetAction(ply_work, reset_act_work);
        AppMain.GmPlayerSetEndSuperSonic(ply_work);
        if ((((int)ply_work.obj_work.move_flag & 1) == 0 || ((int)ply_work.obj_work.move_flag & 16) != 0) && ply_work.act_state == 21 || ply_work.act_state == 22)
        {
            AppMain.GmPlayerActionChange(ply_work, 42);
            ply_work.obj_work.disp_flag |= 4U;
        }
        else
            AppMain.GmPlayerResetAction(ply_work, reset_act_work);
    }

    private static void gmPlayerGetHomingTarget(AppMain.GMS_PLAYER_WORK ply_work)
    {
        float num1 = AppMain.FXM_FX32_TO_FLOAT(786432) * AppMain.FXM_FX32_TO_FLOAT(786432);
        float num2 = 1.5f;
        if (ply_work.homing_boost_timer != 0)
        {
            num2 = 1f;
            ply_work.homing_boost_timer = AppMain.ObjTimeCountDown(ply_work.homing_boost_timer);
        }
        if (ply_work.enemy_obj != null && ((int)ply_work.enemy_obj.flag & 4) != 0)
            ply_work.enemy_obj = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_RECT_WORK obsRectWork1 = ply_work.rect_work[2];
        float num3 = AppMain.FXM_FX32_TO_FLOAT(ply_work.obj_work.pos.x);
        float num4 = AppMain.FXM_FX32_TO_FLOAT(ply_work.obj_work.pos.y + ((int)obsRectWork1.rect.top + (int)obsRectWork1.rect.bottom >> 1));
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
            AppMain.MTM_MATH_SWAP<int>(ref a, ref b);
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, ushort.MaxValue);
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        for (; obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
        {
            if (((int)obj_work.disp_flag & 32) == 0)
            {
                AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork;
                if (obj_work.obj_type == (ushort)3)
                {
                    gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
                    if (((int)gmsEnemyComWork.enemy_flag & 32768) != 0 || ((ushort)63 > gmsEnemyComWork.eve_rec.id || gmsEnemyComWork.eve_rec.id > (ushort)67 || gmsEnemyComWork.eve_rec.byte_param[1] != (byte)0) && (((ushort)70 > gmsEnemyComWork.eve_rec.id || gmsEnemyComWork.eve_rec.id > (ushort)79) && ((ushort)100 > gmsEnemyComWork.eve_rec.id || gmsEnemyComWork.eve_rec.id > (ushort)101)) && ((ushort)130 != gmsEnemyComWork.eve_rec.id && ((ushort)112 > gmsEnemyComWork.eve_rec.id || gmsEnemyComWork.eve_rec.id > (ushort)114) && ((ushort)163 != gmsEnemyComWork.eve_rec.id && (ushort)86 != gmsEnemyComWork.eve_rec.id && ((ushort)161 != gmsEnemyComWork.eve_rec.id && (ushort)247 != gmsEnemyComWork.eve_rec.id))))
                        continue;
                }
                else if (obj_work.obj_type == (ushort)2)
                {
                    gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
                    if (((int)gmsEnemyComWork.enemy_flag & 32768) != 0)
                        continue;
                }
                else
                    continue;
                AppMain.OBS_RECT_WORK obsRectWork2 = gmsEnemyComWork.rect_work[2];
                float num5 = AppMain.FXM_FX32_TO_FLOAT(obj_work.pos.x);
                float num6 = AppMain.FXM_FX32_TO_FLOAT(obj_work.pos.y + ((int)obsRectWork2.rect.top + (int)obsRectWork2.rect.bottom >> 1));
                float num7 = num5 - num3;
                float num8 = num6 - num4;
                int num9 = AppMain.nnArcTan2((double)num8, (double)num7);
                if (num9 >= a && num9 <= b)
                {
                    float num10 = num8 * num2;
                    float num11 = (float)((double)num7 * (double)num7 + (double)num10 * (double)num10);
                    if ((double)num11 < (double)num1)
                    {
                        num1 = num11;
                        obsObjectWork = obj_work;
                    }
                }
            }
        }
        ply_work.enemy_obj = obsObjectWork;
        if (ply_work.cursol_enemy_obj == ply_work.enemy_obj && AppMain.GmPlySeqCheckAcceptHoming(ply_work))
            return;
        ply_work.cursol_enemy_obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmPlayerKeyGet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.no_key_timer != 0 || ((int)ply_work.player_flag & 4194304) != 0)
        {
            ply_work.no_key_timer = AppMain.ObjTimeCountDown(ply_work.no_key_timer);
            ply_work.key_on = (ushort)0;
            ply_work.key_push = (ushort)0;
            ply_work.key_repeat = (ushort)0;
            ply_work.key_release = (ushort)0;
            ply_work.key_rot_z = 0;
            ply_work.key_walk_rot_z = 0;
        }
        else
        {
            if (((int)ply_work.player_flag & 4194304) == 0 && ply_work.player_id == (byte)0)
            {
                int rotZ = AppMain.gmPlayerKeyGetRotZ(ply_work);
                ply_work.prev_key_rot_z = ply_work.key_rot_z;
                ply_work.key_rot_z = rotZ;
                ushort num1 = AppMain.gmPlayerRemapKey(ply_work, (ushort)0, rotZ);
                ushort num2 = (ushort)((uint)ply_work.key_on ^ (uint)num1);
                ply_work.key_push = (ushort)((uint)num2 & (uint)num1);
                ply_work.key_release = (ushort)((uint)num2 & (uint)~num1);
                ply_work.key_on = num1;
                ply_work.key_repeat = (ushort)0;
                for (int index = 0; index < 8; ++index)
                {
                    if (((int)ply_work.key_on & (int)AppMain.gm_key_map_key_list[index]) == 0)
                        ply_work.key_repeat_timer[index] = 30;
                    else if (--ply_work.key_repeat_timer[index] == 0)
                    {
                        ply_work.key_repeat |= (ushort)((uint)ply_work.key_on & (uint)(ushort)AppMain.gm_key_map_key_list[index]);
                        ply_work.key_repeat_timer[index] = 5;
                    }
                }
                ply_work.key_walk_rot_z = 0;
                if (((int)AppMain.g_gs_main_sys_info.game_flag & 1) != 0)
                {
                    if (((int)ply_work.key_on & 8) != 0)
                        ply_work.key_walk_rot_z = (int)short.MaxValue;
                    else if (((int)ply_work.key_on & 4) != 0)
                        ply_work.key_walk_rot_z = -32767;
                }
                else
                    ply_work.key_walk_rot_z = rotZ;
            }
            if (!AppMain.GMM_MAIN_STAGE_IS_ENDING())
                return;
            AppMain.GmEndingPlyKeyCustom(ply_work);
        }
    }

    private static ushort gmPlayerRemapKey(
      AppMain.GMS_PLAYER_WORK ply_work,
      ushort key,
      int key_rot_z)
    {
        ushort num1 = (ushort)((uint)key & 4294967056U);
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE() || AppMain.g_gs_main_sys_info.stage_id == (ushort)9)
        {
            if (((int)AppMain.g_gs_main_sys_info.game_flag & 512) != 0)
            {
                int focusTpIndex = AppMain.CPadPolarHandle.CreateInstance().GetFocusTpIndex();
                if (AppMain.gmPlayerIsInputDPadJumpKey(ply_work, focusTpIndex))
                    num1 |= ply_work.key_map[4];
                if (AppMain.gmPlayerIsInputDPadSSonicKey(ply_work, focusTpIndex))
                    num1 |= ply_work.key_map[6];
            }
            else
                num1 |= AppMain.gmPlayerRemapKeyIPhoneZone32SS(ply_work);
        }
        else if (((int)AppMain.g_gs_main_sys_info.game_flag & 1) != 0)
        {
            CPadVirtualPad cpadVirtualPad = CPadVirtualPad.CreateInstance();
            int num2 = cpadVirtualPad.IsValid() ? cpadVirtualPad.GetValue() : (int)AoPad.AoPadMDirect();

            if ((8 & (int)num2) != 0)
                num1 |= ply_work.key_map[3];
            else if ((4 & (int)num2) != 0)
                num1 |= ply_work.key_map[2];
            else if ((1 & (int)num2) != 0)
                num1 |= ply_work.key_map[0];
            else if ((2 & (int)num2) != 0)
                num1 |= ply_work.key_map[1];
            if (AppMain.gmPlayerIsInputDPadJumpKey(ply_work, -1))
                num1 |= ply_work.key_map[4];
            if (AppMain.gmPlayerIsInputDPadSSonicKey(ply_work, -1))
                num1 |= ply_work.key_map[6];
        }
        else
        {
            if (key_rot_z > 1024)
                num1 |= (ushort)8;
            else if (key_rot_z < -1024)
                num1 |= (ushort)4;
            num1 |= AppMain.gmPlayerRemapKeyIPhone(ply_work);
        }
        if ((32 & (int)key) != 0)
            num1 |= ply_work.key_map[4];
        if ((128 & (int)key) != 0)
            num1 |= ply_work.key_map[5];
        if ((64 & (int)key) != 0)
            num1 |= ply_work.key_map[6];
        if ((16 & (int)key) != 0)
            num1 |= ply_work.key_map[7];
        return num1;
    }

    private static int gmPlayerKeyGetRotZ(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.is_nudge = false;
        int stageId = (int)AppMain.g_gs_main_sys_info.stage_id;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            AppMain.NNS_VECTOR core = AppMain._am_iphone_accel_data.core;
            AppMain.NNS_VECTOR calcAccel = ply_work.calc_accel;
            calcAccel.x = (float)((double)core.x * 0.100000001490116 + (double)calcAccel.x * 0.899999976158142);
            calcAccel.y = (float)((double)core.y * 0.100000001490116 + (double)calcAccel.y * 0.899999976158142);
            calcAccel.y = (float)((double)core.y * 0.100000001490116 + (double)calcAccel.y * 0.899999976158142);
            float num1 = core.x - calcAccel.x;
            float num2 = core.y - calcAccel.y;
            float num3 = core.z - calcAccel.z;
            if ((double)AppMain.nnSqrt((float)((double)num1 * (double)num1 + (double)num2 * (double)num2 + (double)num3 * (double)num3)) >= 2.0)
                ply_work.is_nudge = true;
        }
        int num4;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE() && ((int)AppMain.g_gs_main_sys_info.game_flag & 512) != 0)
            num4 = AppMain.g_gm_main_system.polar_diff;
        else if (stageId == 9 && ((int)AppMain.g_gs_main_sys_info.game_flag & 512) != 0)
        {
            num4 = AppMain.g_gm_main_system.polar_diff;
        }
        else
        {
            int num1 = (int)((double)AppMain._am_iphone_accel_data.sensor.x * 16384.0);
            if (num1 < 2048 && num1 > -2048)
            {
                num4 = 0;
            }
            else
            {
                num4 = num1 * 3;
                if (num4 > 32768)
                    num4 = 32768;
                else if (num4 < (int)short.MinValue)
                    num4 = (int)short.MinValue;
            }
        }
        return num4;
    }

    private static ushort gmPlayerRemapKeyIPhone(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ushort num1 = 0;
        if (AppMain.GmMainKeyCheckPauseKeyPush() != -1)
            return num1;
        int seqState = ply_work.seq_state;
        if (ply_work.safe_timer > 0)
        {
            --ply_work.safe_timer;
            if (AppMain.amTpIsTouchPull(0))
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
            num1 |= (ushort)32;
        }
        else if (ply_work.safe_spin_timer != 0)
        {
            uint num2 = ply_work.obj_work.disp_flag & 1U;
            if (AppMain.amTpIsTouchPull(0))
                ply_work.safe_spin_timer = 0;
            switch (ply_work.safe_spin_timer)
            {
                case 1:
                    num1 |= (ushort)2;
                    if (!AppMain.amTpIsTouchPull(0) && (seqState == 6 || seqState == 7 || seqState == 8))
                    {
                        num1 |= (ushort)32;
                        break;
                    }
                    break;
                case 2:
                    if (seqState != 2)
                        ply_work.safe_spin_timer = 1;
                    num1 |= (ushort)2;
                    break;
                case 3:
                    ushort num3;
                    if (AppMain.ObjTouchCheck(ply_work.obj_work, AppMain.gm_ply_touch_rect[1]) != (ushort)0)
                    {
                        num3 = num2 == 0U ? (ushort)4 : (ushort)8;
                        ply_work.safe_spin_timer = 2;
                    }
                    else
                    {
                        num3 = (ushort)2;
                        ply_work.safe_spin_timer = 1;
                    }
                    num1 |= num3;
                    break;
            }
        }
        else
        {
            bool flag1 = AppMain.amTpIsTouchOn(0);
            bool flag2 = AppMain.amTpIsTouchPush(0);
            if (AppMain.GmPlayerIsTransformSuperSonic(ply_work) && flag1 && AppMain.GMM_PLAYER_IS_TOUCH_SUPER_SONIC_REGION((int)AppMain._am_tp_touch[0].on[0], (int)AppMain._am_tp_touch[0].on[1]))
                num1 |= (ushort)80;
            if (((int)num1 & 80) == 0)
            {
                ushort num2 = 0;
                uint num3 = ply_work.obj_work.disp_flag & 1U;
                ushort num4 = AppMain.ObjTouchCheck(ply_work.obj_work, AppMain.gm_ply_touch_rect[0]);
                if (num4 == (ushort)0 && AppMain.ObjTouchCheck(ply_work.obj_work, AppMain.gm_ply_touch_rect[1]) != (ushort)0)
                {
                    num4 = (ushort)1;
                    num2 = num3 == 0U ? (ushort)4 : (ushort)8;
                }
                if (num4 != (ushort)0)
                {
                    if (AppMain._am_tp_touch[0].on[0] < (ushort)80 || AppMain._am_tp_touch[0].on[0] > (ushort)400)
                    {
                        ply_work.safe_timer = 25;
                    }
                    else
                    {
                        switch (seqState)
                        {
                            case 0:
                            case 9:
                                if (num2 != (ushort)0)
                                {
                                    num1 |= num2;
                                    break;
                                }
                                if (flag2 || flag1)
                                {
                                    num1 |= (ushort)2;
                                    break;
                                }
                                break;
                            case 1:
                                if (flag2 || flag1)
                                {
                                    ply_work.spin_state = 3;
                                    num1 |= (ushort)2;
                                    break;
                                }
                                break;
                            case 2:
                                if (flag2 | flag1)
                                {
                                    num1 |= (ushort)2;
                                    break;
                                }
                                break;
                            case 6:
                            case 7:
                            case 8:
                                if (flag2 || flag1)
                                {
                                    num1 |= (ushort)34;
                                    break;
                                }
                                break;
                            case 11:
                            case 12:
                                if (flag2 | flag1)
                                {
                                    num1 |= (ushort)2;
                                    break;
                                }
                                break;
                            default:
                                if (ply_work.spin_state != 3)
                                {
                                    if (flag2 || flag1)
                                    {
                                        ply_work.spin_state = 0;
                                        num1 |= (ushort)32;
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
                        num1 |= (ushort)32;
                }
            }
        }
        return num1;
    }

    private static ushort gmPlayerRemapKeyIPhoneZone32SS(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ushort num = 0;
        if (AppMain.GmMainKeyCheckPauseKeyPush() != -1)
            return num;
        for (int index = 0; index < 4; ++index)
        {
            bool flag1 = AppMain.amTpIsTouchOn(index);
            bool flag2 = AppMain.amTpIsTouchPush(index);
            if (AppMain.GmPlayerIsTransformSuperSonic(ply_work) && flag1 && AppMain.GMM_PLAYER_IS_TOUCH_SUPER_SONIC_REGION((int)AppMain._am_tp_touch[index].on[0], (int)AppMain._am_tp_touch[index].on[1]))
                num |= (ushort)80;
            if (((int)num & 80) == 0 && (flag2 || flag1))
                num |= (ushort)32;
            if (num != (ushort)0)
                break;
        }
        return num;
    }

    private static bool gmPlayerIsInputDPadJumpKey(AppMain.GMS_PLAYER_WORK ply_work, int ignore_key)
    {
        if ((AoPad.AoPadMDirect() & ControllerConsts.JUMP_BUTTON) != 0)
            return true;

        if (ply_work.control_type == 2)
            return false;
        if (ply_work.jump_rect == null)
        {
            AppMain.mppAssertNotImpl();
            return false;
        }
        bool flag = false;
        for (int index = 0; index < 4; ++index)
        {
            if ((index != ignore_key || AppMain.amTpIsTouchPush(index)) && AppMain.amTpIsTouchOn(index))
            {
                short num1 = (short)AppMain._am_tp_touch[index].on[0];
                short num2 = (short)AppMain._am_tp_touch[index].on[1];
                if ((int)ply_work.jump_rect[0] <= (int)num1 && (int)num1 <= (int)ply_work.jump_rect[2] && ((int)ply_work.jump_rect[1] <= (int)num2 && (int)num2 <= (int)ply_work.jump_rect[3]))
                {
                    flag = true;
                    break;
                }
            }
        }
        return flag;
    }

    private static bool gmPlayerIsInputDPadSSonicKey(AppMain.GMS_PLAYER_WORK ply_work, int ignore_key)
    {
        if ((AoPad.AoPadMDirect() & ControllerConsts.SUPER_SONIC) != 0)
            return true;

        if (ply_work.control_type == 2)
            return false;
        bool flag = false;
        for (int index = 0; index < 4; ++index)
        {
            if ((index != ignore_key || AppMain.amTpIsTouchPush(index)) && AppMain.amTpIsTouchOn(index))
            {
                short num1 = (short)AppMain._am_tp_touch[index].on[0];
                short num2 = (short)AppMain._am_tp_touch[index].on[1];
                if ((int)ply_work.ssonic_rect[0] <= (int)num1 && (int)num1 <= (int)ply_work.ssonic_rect[2] && ((int)ply_work.ssonic_rect[1] <= (int)num2 && (int)num2 <= (int)ply_work.ssonic_rect[3]))
                {
                    flag = true;
                    break;
                }
            }
        }
        return flag;
    }

    private static void gmPlayerCameraOffset(AppMain.GMS_PLAYER_WORK ply_work)
    {
        byte num1 = 4;
        if (ply_work.player_id != (byte)0 || AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            return;
        if (ply_work.gmk_obj == null)
        {
            ply_work.gmk_flag &= 4227858431U;
            ply_work.gmk_camera_gmk_center_ofst_x = (short)0;
            ply_work.gmk_camera_gmk_center_ofst_y = (short)0;
        }
        short num2 = 0;
        short num3 = 0;
        if (((int)ply_work.player_flag & 8192) != 0)
        {
            ply_work.camera_ofst_x -= ply_work.camera_ofst_x >> 2;
            ply_work.camera_ofst_y -= ply_work.camera_ofst_y >> 2;
        }
        else if (((int)ply_work.gmk_flag & 67108864) != 0)
        {
            ply_work.camera_ofst_x += ply_work.camera_ofst_tag_x - ply_work.camera_ofst_x + ((int)ply_work.gmk_camera_center_ofst_x + (int)ply_work.gmk_camera_gmk_center_ofst_x << 12) >> (int)num1;
            ply_work.camera_ofst_y += ply_work.camera_ofst_tag_y - ply_work.camera_ofst_y + ((int)ply_work.gmk_camera_center_ofst_y + (int)ply_work.gmk_camera_gmk_center_ofst_y << 12) >> (int)num1;
        }
        else
        {
            ply_work.camera_ofst_x += ply_work.camera_ofst_tag_x - ply_work.camera_ofst_x + ((int)ply_work.gmk_camera_center_ofst_x << 12) >> (int)num1;
            ply_work.camera_ofst_y += ply_work.camera_ofst_tag_y - ply_work.camera_ofst_y + ((int)ply_work.gmk_camera_center_ofst_y << 12) >> (int)num1;
        }
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        obsCamera.ofst.x = AppMain.FXM_FX32_TO_FLOAT(((int)num2 << 12) + ply_work.camera_ofst_x);
        obsCamera.ofst.y = AppMain.FXM_FX32_TO_FLOAT(((int)num3 << 12) + ply_work.camera_ofst_y);
    }

    public static void gmGmkPlayerMotionCallbackTruck(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_OBJECT _object,
      object param)
    {
        AppMain.NNS_MATRIX mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = (AppMain.GMS_PLAYER_WORK)param;
        AppMain.nnMakeUnitMatrix(nnsMatrix);
        AppMain.nnMultiplyMatrix(nnsMatrix, nnsMatrix, AppMain.amMatrixGetCurrent());
        AppMain.nnCalcNodeMatrixTRSList(mtx, _object, AppMain.GMD_PLAYER_NODE_ID_TRUCK_CENTER, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, nnsMatrix);
        mtx.Assign(gmsPlayerWork.truck_mtx_ply_mtn_pos);
    }


}