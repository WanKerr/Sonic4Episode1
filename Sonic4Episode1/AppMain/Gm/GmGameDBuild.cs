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
    private static void GmGameDBuildModelBuildInit()
    {
        for (int index = 0; index < AppMain.gm_obj_build_model_work_buf.Length; ++index)
            AppMain.gm_obj_build_model_work_buf[index].Clear();
        AppMain.gm_obj_build_model_work_reg_num = 0;
    }

    private static AppMain.OBS_ACTION3D_NN_WORK[] GmGameDBuildRegBuildModel(
      AppMain.AMS_AMB_HEADER mdl_amb,
      AppMain.AMS_AMB_HEADER tex_amb,
      uint draw_flag)
    {
        return AppMain.GmGameDBuildRegBuildModel(mdl_amb, tex_amb, draw_flag, (AppMain.TXB_HEADER)null);
    }

    private static AppMain.OBS_ACTION3D_NN_WORK[] GmGameDBuildRegBuildModel(
      AppMain.AMS_AMB_HEADER mdl_amb,
      AppMain.AMS_AMB_HEADER tex_amb,
      uint draw_flag,
      AppMain.TXB_HEADER txb)
    {
        AppMain.GMS_GDBUILD_BUILD_MDL_WORK gdbuildBuildMdlWork = AppMain.gm_obj_build_model_work_buf[AppMain.gm_obj_build_model_work_reg_num];
        ++AppMain.gm_obj_build_model_work_reg_num;
        gdbuildBuildMdlWork.num = mdl_amb.file_num;
        gdbuildBuildMdlWork.obj_3d_list = AppMain.New<AppMain.OBS_ACTION3D_NN_WORK>(gdbuildBuildMdlWork.num);
        gdbuildBuildMdlWork.mdl_amb = mdl_amb;
        gdbuildBuildMdlWork.tex_amb = tex_amb;
        gdbuildBuildMdlWork.draw_flag = draw_flag;
        gdbuildBuildMdlWork.txb = txb;
        gdbuildBuildMdlWork.build_state = AppMain.GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_REG_WAIT;
        return gdbuildBuildMdlWork.obj_3d_list;
    }

    private static bool GmGameDBuildCheckBuildModel()
    {
        AppMain.ArrayPointer<AppMain.GMS_GDBUILD_BUILD_MDL_WORK> arrayPointer1 = (AppMain.ArrayPointer<AppMain.GMS_GDBUILD_BUILD_MDL_WORK>)(AppMain.GMS_GDBUILD_BUILD_MDL_WORK[])null;
        AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK> arrayPointer2 = (AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>)(AppMain.OBS_ACTION3D_NN_WORK[])null;
        bool flag = true;
        if (AppMain.gm_obj_build_model_work_reg_num != 0)
        {
            int _offset = AppMain.gm_obj_build_model_work_reg_num - 1;
            arrayPointer1 = new AppMain.ArrayPointer<AppMain.GMS_GDBUILD_BUILD_MDL_WORK>(AppMain.gm_obj_build_model_work_buf, _offset);
            while (_offset >= 0)
            {
                if (((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).build_state == (AppMain.GME_GAME_DBUILD_MDL_STATE)AppMain.GME_GAME_DBUILD_MDL_STATE_REG_WAIT)
                {
                    int regNum = ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num;
                    arrayPointer2 = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).obj_3d_list, regNum);
                    if (((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).txb == null)
                    {
                        while (AppMain.GsMainSysGetDisplayListRegistNum() <= 188 && regNum < ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).num)
                        {
                            AppMain.ObjAction3dNNModelLoad((AppMain.OBS_ACTION3D_NN_WORK)~arrayPointer2, (AppMain.OBS_DATA_WORK)null, (string)null, regNum, ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).mdl_amb, (string)null, ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).tex_amb, ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).draw_flag);
                            ++regNum;
                            ++arrayPointer2;
                        }
                        if (regNum == ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num)
                            return false;
                        ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num = regNum;
                        if (((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num == ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).num)
                            ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).build_state = AppMain.GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_BUILD_WAIT;
                    }
                    else
                    {
                        while (AppMain.GsMainSysGetDisplayListRegistNum() <= 188 && regNum < ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).num)
                        {
                            AppMain.ObjAction3dNNModelLoadTxb((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer2, (AppMain.OBS_DATA_WORK)null, (string)null, regNum, ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).mdl_amb, (string)null, ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).tex_amb, ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).draw_flag, ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).txb);
                            ++regNum;
                            ++arrayPointer2;
                        }
                        if (regNum == ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num)
                            return false;
                        ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num = regNum;
                        if (((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num == ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).num)
                            ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).build_state = AppMain.GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_BUILD_WAIT;
                    }
                    flag = false;
                }
                else if (((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).build_state == (AppMain.GME_GAME_DBUILD_MDL_STATE)AppMain.GME_GAME_DBUILD_MDL_STATE_BUILD_WAIT)
                {
                    arrayPointer2 = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).obj_3d_list);
                    int num = 0;
                    while (num < ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).num)
                    {
                        if (!AppMain.ObjAction3dNNModelLoadCheck((AppMain.OBS_ACTION3D_NN_WORK)~arrayPointer2))
                        {
                            flag = false;
                            break;
                        }
                        ++num;
                        ++arrayPointer2;
                    }
                    if (num >= ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).num && _offset == AppMain.gm_obj_build_model_work_reg_num - 1)
                        --AppMain.gm_obj_build_model_work_reg_num;
                }
                --_offset;
                --arrayPointer1;
            }
        }
        return flag;
    }

    private static void GmGameDBuildModelFlushInit()
    {
        AppMain.ClearArray<AppMain.GMS_GDBUILD_BUILD_MDL_WORK>(AppMain.gm_obj_build_model_work_buf);
        AppMain.gm_obj_build_model_work_reg_num = 0;
    }

    private static void GmGameDBuildRegFlushModel(AppMain.OBS_ACTION3D_NN_WORK[] obj_3d_list, int num)
    {
        AppMain.GMS_GDBUILD_BUILD_MDL_WORK gdbuildBuildMdlWork = AppMain.gm_obj_build_model_work_buf[AppMain.gm_obj_build_model_work_reg_num];
        ++AppMain.gm_obj_build_model_work_reg_num;
        gdbuildBuildMdlWork.num = num;
        gdbuildBuildMdlWork.obj_3d_list = obj_3d_list;
        gdbuildBuildMdlWork.build_state = AppMain.GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_REG_FLUSH_WAIT;
    }

    private static bool GmGameDBuildCheckFlushModel()
    {
        bool flag = true;
        if (AppMain.gm_obj_build_model_work_reg_num != 0)
        {
            int _offset = AppMain.gm_obj_build_model_work_reg_num - 1;
            AppMain.ArrayPointer<AppMain.GMS_GDBUILD_BUILD_MDL_WORK> arrayPointer1 = new AppMain.ArrayPointer<AppMain.GMS_GDBUILD_BUILD_MDL_WORK>(AppMain.gm_obj_build_model_work_buf, _offset);
            while (_offset >= 0)
            {
                AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK> arrayPointer2;
                if (((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).build_state == AppMain.GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_REG_FLUSH_WAIT)
                {
                    int regNum = ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num;
                    arrayPointer2 = new AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>(((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).obj_3d_list, regNum);
                    while (AppMain.GsMainSysGetDisplayListRegistNum() <= 188 && regNum < ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).num)
                    {
                        AppMain.ObjAction3dNNModelRelease((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer2);
                        ++regNum;
                        ++arrayPointer2;
                    }
                    if (regNum == ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num)
                        return false;
                    ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num = regNum;
                    if (((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).reg_num == ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).num)
                        ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).build_state = AppMain.GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_FLUSH_WAIT;
                    flag = false;
                }
                else if (((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).build_state == AppMain.GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_FLUSH_WAIT)
                {
                    int num = 0;
                    arrayPointer2 = (AppMain.ArrayPointer<AppMain.OBS_ACTION3D_NN_WORK>)((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).obj_3d_list;
                    while (num < ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).num)
                    {
                        if (!AppMain.ObjAction3dNNModelReleaseCheck((AppMain.OBS_ACTION3D_NN_WORK)arrayPointer2))
                        {
                            flag = false;
                            break;
                        }
                        ++num;
                        ++arrayPointer2;
                    }
                    if (num >= ((AppMain.GMS_GDBUILD_BUILD_MDL_WORK)~arrayPointer1).num && _offset == AppMain.gm_obj_build_model_work_reg_num - 1)
                        --AppMain.gm_obj_build_model_work_reg_num;
                }
                --_offset;
                --arrayPointer1;
            }
        }
        return flag;
    }

}