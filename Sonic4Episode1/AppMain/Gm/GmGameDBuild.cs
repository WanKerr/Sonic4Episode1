public partial class AppMain
{
    private static void GmGameDBuildModelBuildInit()
    {
        for (int index = 0; index < gm_obj_build_model_work_buf.Length; ++index)
            gm_obj_build_model_work_buf[index].Clear();
        gm_obj_build_model_work_reg_num = 0;
    }

    private static OBS_ACTION3D_NN_WORK[] GmGameDBuildRegBuildModel(
      AMS_AMB_HEADER mdl_amb,
      AMS_AMB_HEADER tex_amb,
      uint draw_flag)
    {
        return GmGameDBuildRegBuildModel(mdl_amb, tex_amb, draw_flag, null);
    }

    private static OBS_ACTION3D_NN_WORK[] GmGameDBuildRegBuildModel(
      AMS_AMB_HEADER mdl_amb,
      AMS_AMB_HEADER tex_amb,
      uint draw_flag,
      TXB_HEADER txb)
    {
        GMS_GDBUILD_BUILD_MDL_WORK gdbuildBuildMdlWork = gm_obj_build_model_work_buf[gm_obj_build_model_work_reg_num];
        ++gm_obj_build_model_work_reg_num;
        gdbuildBuildMdlWork.num = mdl_amb.file_num;
        gdbuildBuildMdlWork.obj_3d_list = New<OBS_ACTION3D_NN_WORK>(gdbuildBuildMdlWork.num);
        gdbuildBuildMdlWork.mdl_amb = mdl_amb;
        gdbuildBuildMdlWork.tex_amb = tex_amb;
        gdbuildBuildMdlWork.draw_flag = draw_flag;
        gdbuildBuildMdlWork.txb = txb;
        gdbuildBuildMdlWork.build_state = GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_REG_WAIT;
        return gdbuildBuildMdlWork.obj_3d_list;
    }

    private static bool GmGameDBuildCheckBuildModel()
    {
        ArrayPointer<GMS_GDBUILD_BUILD_MDL_WORK> arrayPointer1 = null;
        ArrayPointer<OBS_ACTION3D_NN_WORK> arrayPointer2 = null;
        bool flag = true;
        if (gm_obj_build_model_work_reg_num != 0)
        {
            int _offset = gm_obj_build_model_work_reg_num - 1;
            arrayPointer1 = new ArrayPointer<GMS_GDBUILD_BUILD_MDL_WORK>(gm_obj_build_model_work_buf, _offset);
            while (_offset >= 0)
            {
                if ((~arrayPointer1).build_state == (GME_GAME_DBUILD_MDL_STATE)GME_GAME_DBUILD_MDL_STATE_REG_WAIT)
                {
                    int regNum = (~arrayPointer1).reg_num;
                    arrayPointer2 = new ArrayPointer<OBS_ACTION3D_NN_WORK>((~arrayPointer1).obj_3d_list, regNum);
                    if ((~arrayPointer1).txb == null)
                    {
                        while (GsMainSysGetDisplayListRegistNum() <= 188 && regNum < (~arrayPointer1).num)
                        {
                            ObjAction3dNNModelLoad(~arrayPointer2, null, null, regNum, (~arrayPointer1).mdl_amb, null, (~arrayPointer1).tex_amb, (~arrayPointer1).draw_flag);
                            ++regNum;
                            ++arrayPointer2;
                        }
                        if (regNum == (~arrayPointer1).reg_num)
                            return false;
                        (~arrayPointer1).reg_num = regNum;
                        if ((~arrayPointer1).reg_num == (~arrayPointer1).num)
                            (~arrayPointer1).build_state = GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_BUILD_WAIT;
                    }
                    else
                    {
                        while (GsMainSysGetDisplayListRegistNum() <= 188 && regNum < (~arrayPointer1).num)
                        {
                            ObjAction3dNNModelLoadTxb(arrayPointer2, null, null, regNum, (~arrayPointer1).mdl_amb, null, (~arrayPointer1).tex_amb, (~arrayPointer1).draw_flag, (~arrayPointer1).txb);
                            ++regNum;
                            ++arrayPointer2;
                        }
                        if (regNum == (~arrayPointer1).reg_num)
                            return false;
                        (~arrayPointer1).reg_num = regNum;
                        if ((~arrayPointer1).reg_num == (~arrayPointer1).num)
                            (~arrayPointer1).build_state = GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_BUILD_WAIT;
                    }
                    flag = false;
                }
                else if ((~arrayPointer1).build_state == (GME_GAME_DBUILD_MDL_STATE)GME_GAME_DBUILD_MDL_STATE_BUILD_WAIT)
                {
                    arrayPointer2 = new ArrayPointer<OBS_ACTION3D_NN_WORK>((~arrayPointer1).obj_3d_list);
                    int num = 0;
                    while (num < (~arrayPointer1).num)
                    {
                        if (!ObjAction3dNNModelLoadCheck(~arrayPointer2))
                        {
                            flag = false;
                            break;
                        }
                        ++num;
                        ++arrayPointer2;
                    }
                    if (num >= (~arrayPointer1).num && _offset == gm_obj_build_model_work_reg_num - 1)
                        --gm_obj_build_model_work_reg_num;
                }
                --_offset;
                --arrayPointer1;
            }
        }
        return flag;
    }

    private static void GmGameDBuildModelFlushInit()
    {
        ClearArray(gm_obj_build_model_work_buf);
        gm_obj_build_model_work_reg_num = 0;
    }

    private static void GmGameDBuildRegFlushModel(OBS_ACTION3D_NN_WORK[] obj_3d_list, int num)
    {
        GMS_GDBUILD_BUILD_MDL_WORK gdbuildBuildMdlWork = gm_obj_build_model_work_buf[gm_obj_build_model_work_reg_num];
        ++gm_obj_build_model_work_reg_num;
        gdbuildBuildMdlWork.num = num;
        gdbuildBuildMdlWork.obj_3d_list = obj_3d_list;
        gdbuildBuildMdlWork.build_state = GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_REG_FLUSH_WAIT;
    }

    private static bool GmGameDBuildCheckFlushModel()
    {
        bool flag = true;
        if (gm_obj_build_model_work_reg_num != 0)
        {
            int _offset = gm_obj_build_model_work_reg_num - 1;
            ArrayPointer<GMS_GDBUILD_BUILD_MDL_WORK> arrayPointer1 = new ArrayPointer<GMS_GDBUILD_BUILD_MDL_WORK>(gm_obj_build_model_work_buf, _offset);
            while (_offset >= 0)
            {
                ArrayPointer<OBS_ACTION3D_NN_WORK> arrayPointer2;
                if ((~arrayPointer1).build_state == GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_REG_FLUSH_WAIT)
                {
                    int regNum = (~arrayPointer1).reg_num;
                    arrayPointer2 = new ArrayPointer<OBS_ACTION3D_NN_WORK>((~arrayPointer1).obj_3d_list, regNum);
                    while (GsMainSysGetDisplayListRegistNum() <= 188 && regNum < (~arrayPointer1).num)
                    {
                        ObjAction3dNNModelRelease(arrayPointer2);
                        ++regNum;
                        ++arrayPointer2;
                    }
                    if (regNum == (~arrayPointer1).reg_num)
                        return false;
                    (~arrayPointer1).reg_num = regNum;
                    if ((~arrayPointer1).reg_num == (~arrayPointer1).num)
                        (~arrayPointer1).build_state = GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_FLUSH_WAIT;
                    flag = false;
                }
                else if ((~arrayPointer1).build_state == GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_FLUSH_WAIT)
                {
                    int num = 0;
                    arrayPointer2 = (~arrayPointer1).obj_3d_list;
                    while (num < (~arrayPointer1).num)
                    {
                        if (!ObjAction3dNNModelReleaseCheck(arrayPointer2))
                        {
                            flag = false;
                            break;
                        }
                        ++num;
                        ++arrayPointer2;
                    }
                    if (num >= (~arrayPointer1).num && _offset == gm_obj_build_model_work_reg_num - 1)
                        --gm_obj_build_model_work_reg_num;
                }
                --_offset;
                --arrayPointer1;
            }
        }
        return flag;
    }

}