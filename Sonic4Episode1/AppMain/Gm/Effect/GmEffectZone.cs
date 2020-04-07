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
    private static readonly int[] gm_efct_zone_texamb_index_tbl;
    private static readonly AppMain.GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone01_create_param_tbl;
    private static readonly AppMain.GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone02_create_param_tbl;
    private static readonly AppMain.GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone03_create_param_tbl;
    private static readonly AppMain.GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone04_create_param_tbl;
    private static readonly AppMain.GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone_final_create_param_tbl;
    private static readonly AppMain.GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone_ss_create_param_tbl;
    public static readonly AppMain.GMS_EFCT_ZONE_CREATE_INFO[] gm_efct_zone_create_info;
    private static int gm_efct_zone_proc_state;
    private static int gm_efct_zone_tex_reg_id;
    private static int[] gm_efct_zone_model_reg_id_list;
    private static int gm_efct_zone_model_reg_num;
    private static int[] gm_efct_zone_mdl_tex_reg_id_list;
    private static int gm_efct_zone_target_zone_no;

    private static void GmEfctZoneBuildDataInit(int zone_no)
    {
        AppMain.gm_efct_zone_proc_state = 1;
        AppMain.gm_efct_zone_target_zone_no = zone_no;
    }

    private static void GmEfctZoneBuildDataLoopInit()
    {
        object texlist_buf = (object)null;
        AppMain.GMS_EFCT_ZONE_CREATE_INFO efctZoneCreateInfo = AppMain.gm_efct_zone_create_info[AppMain.gm_efct_zone_target_zone_no];
        int index1 = 0;
        AppMain.AMS_AMB_HEADER amb = AppMain.readAMBFile(AppMain.ObjDataGetInc(AppMain.ObjDataGet(6)));
        AppMain.gm_efct_zone_model_reg_num = 1;
        if (AppMain.gm_efct_zone_model_reg_num > 0)
        {
            AppMain.gm_efct_zone_model_reg_id_list = new int[AppMain.gm_efct_zone_model_reg_num];
            AppMain.gm_efct_zone_mdl_tex_reg_id_list = new int[AppMain.gm_efct_zone_model_reg_num];
            for (int index2 = 0; index2 < AppMain.gm_efct_zone_model_reg_num; ++index2)
            {
                AppMain.gm_efct_zone_model_reg_id_list[index2] = -1;
                AppMain.gm_efct_zone_mdl_tex_reg_id_list[index2] = -1;
            }
        }
        AppMain.OBS_DATA_WORK data_work1 = AppMain.ObjDataGet(509);
        AppMain.ObjDataLoadAmbIndex(data_work1, AppMain.gm_efct_zone_texamb_index_tbl[AppMain.gm_efct_zone_target_zone_no], amb);
        AppMain.gm_efct_zone_tex_reg_id = AppMain.ObjAction3dESTextureLoadToDwork(AppMain.ObjDataGet(510), AppMain.readAMBFile(data_work1.pData), ref texlist_buf);
        for (int index2 = 0; index2 < efctZoneCreateInfo.num; ++index2)
        {
            int modelIdx = efctZoneCreateInfo.zone_create_param[index2].create_param.model_idx;
            int modelDworkNo = efctZoneCreateInfo.zone_create_param[index2].model_dwork_no;
            int objectDworkNo = AppMain.gmEfctZoneGetObjectDworkNo(modelDworkNo);
            int mdlAmbtexIdx = efctZoneCreateInfo.zone_create_param[index2].mdl_ambtex_idx;
            int mdlAmbtexDworkNo = AppMain.gmEfctZoneGetMdlAmbtexDworkNo(modelDworkNo);
            int mdlTexlistDworkNo = AppMain.gmEfctZoneGetMdlTexlistDworkNo(modelDworkNo);
            if (modelIdx != -1)
            {
                AppMain.OBS_DATA_WORK data_work2 = AppMain.ObjDataGet(mdlAmbtexDworkNo);
                AppMain.ObjDataLoadAmbIndex(data_work2, mdlAmbtexIdx, amb);
                AppMain.gm_efct_zone_mdl_tex_reg_id_list[index1] = AppMain.ObjAction3dESTextureLoadToDwork(AppMain.ObjDataGet(mdlTexlistDworkNo), AppMain.readAMBFile(data_work2.pData), ref texlist_buf);
                AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(modelDworkNo), modelIdx, amb);
                AppMain.gm_efct_zone_model_reg_id_list[index1] = AppMain.ObjAction3dESModelLoadToDwork(AppMain.ObjDataGet(objectDworkNo), (object)(AppMain.AmbChunk)AppMain.ObjDataGet(modelDworkNo).pData, 0U);
                ++index1;
            }
        }
    }

    private static bool GmEfctZoneBuildDataLoop()
    {
        bool flag = true;
        if (AppMain.gm_efct_zone_target_zone_no == -1)
            return flag;
        switch (AppMain.gm_efct_zone_proc_state)
        {
            case 0:
                return true;
            case 1:
                if (AppMain.GsMainSysGetDisplayListRegistNum() < 208)
                {
                    AppMain.GmEfctZoneBuildDataLoopInit();
                    AppMain.gm_efct_zone_proc_state = 2;
                }
                return false;
            default:
                if (AppMain.gm_efct_zone_tex_reg_id != -1)
                {
                    if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_zone_tex_reg_id))
                        AppMain.gm_efct_zone_tex_reg_id = -1;
                    else
                        flag = false;
                }
                for (int index = 0; index < AppMain.gm_efct_zone_model_reg_num; ++index)
                {
                    if (AppMain.gm_efct_zone_mdl_tex_reg_id_list[index] != -1)
                    {
                        if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_zone_mdl_tex_reg_id_list[index]))
                            AppMain.gm_efct_zone_mdl_tex_reg_id_list[index] = -1;
                        else
                            flag = false;
                    }
                    if (AppMain.gm_efct_zone_model_reg_id_list[index] != -1)
                    {
                        if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_zone_model_reg_id_list[index]))
                            AppMain.gm_efct_zone_model_reg_id_list[index] = -1;
                        else
                            flag = false;
                    }
                }
                if (flag)
                {
                    AppMain.gm_efct_zone_target_zone_no = -1;
                    AppMain.gm_efct_zone_proc_state = 0;
                }
                return flag;
        }
    }

    private static void GmEfctZoneFlushDataInit(int zone_no)
    {
        AppMain.gm_efct_zone_proc_state = 1;
        AppMain.gm_efct_zone_target_zone_no = zone_no;
    }

    private static void GmEfctZoneFlushDataLoopInit()
    {
        AppMain.GMS_EFCT_ZONE_CREATE_INFO efctZoneCreateInfo = AppMain.gm_efct_zone_create_info[AppMain.gm_efct_zone_target_zone_no];
        int index1 = 0;
        for (int index2 = 0; index2 < efctZoneCreateInfo.num; ++index2)
        {
            AppMain.GMS_EFCT_ZONE_CREATE_PARAM efctZoneCreateParam = efctZoneCreateInfo.zone_create_param[index2];
            int modelDworkNo = efctZoneCreateParam.model_dwork_no;
            int objectDworkNo = AppMain.gmEfctZoneGetObjectDworkNo(modelDworkNo);
            if (efctZoneCreateParam.create_param.model_idx != -1)
            {
                AppMain.gm_efct_zone_model_reg_id_list[index1] = AppMain.ObjAction3dESModelReleaseDwork(AppMain.ObjDataGet(objectDworkNo));
                AppMain.ObjDataRelease(AppMain.ObjDataGet(modelDworkNo));
                AppMain.gm_efct_zone_mdl_tex_reg_id_list[index1] = AppMain.ObjAction3dESTextureReleaseDwork(AppMain.ObjDataGet(AppMain.gmEfctZoneGetMdlTexlistDworkNo(modelDworkNo)));
                AppMain.ObjDataRelease(AppMain.ObjDataGet(AppMain.gmEfctZoneGetMdlAmbtexDworkNo(modelDworkNo)));
                ++index1;
            }
        }
        AppMain.gm_efct_zone_tex_reg_id = AppMain.ObjAction3dESTextureReleaseDwork(AppMain.ObjDataGet(510));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(509));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(6));
    }

    private static bool GmEfctZoneFlushDataLoop()
    {
        bool flag = true;
        int index1 = 0;
        switch (AppMain.gm_efct_zone_proc_state)
        {
            case 0:
                return true;
            case 1:
                if (AppMain.GsMainSysGetDisplayListRegistNum() < 248)
                {
                    AppMain.GmEfctZoneFlushDataLoopInit();
                    AppMain.gm_efct_zone_proc_state = 2;
                }
                return false;
            default:
                AppMain.GMS_EFCT_ZONE_CREATE_INFO efctZoneCreateInfo = AppMain.gm_efct_zone_create_info[AppMain.gm_efct_zone_target_zone_no];
                if (AppMain.gm_efct_zone_model_reg_num != 0)
                {
                    for (int index2 = 0; index2 < efctZoneCreateInfo.num; ++index2)
                    {
                        AppMain.GMS_EFCT_ZONE_CREATE_PARAM efctZoneCreateParam = efctZoneCreateInfo.zone_create_param[index2];
                        int modelDworkNo = efctZoneCreateParam.model_dwork_no;
                        int objectDworkNo = AppMain.gmEfctZoneGetObjectDworkNo(modelDworkNo);
                        if (efctZoneCreateParam.create_param.model_idx != -1)
                        {
                            if (AppMain.gm_efct_zone_model_reg_id_list[index1] != -1)
                            {
                                if (AppMain.ObjAction3dESModelReleaseDworkCheck(AppMain.ObjDataGet(objectDworkNo), AppMain.gm_efct_zone_model_reg_id_list[index1]))
                                    AppMain.gm_efct_zone_model_reg_id_list[index1] = -1;
                                else
                                    flag = false;
                            }
                            if (AppMain.gm_efct_zone_mdl_tex_reg_id_list[index1] != -1)
                            {
                                if (AppMain.ObjAction3dESTextureReleaseDworkCheck(AppMain.ObjDataGet(AppMain.gmEfctZoneGetMdlTexlistDworkNo(modelDworkNo)), AppMain.gm_efct_zone_mdl_tex_reg_id_list[index1]))
                                    AppMain.gm_efct_zone_mdl_tex_reg_id_list[index1] = -1;
                                else
                                    flag = false;
                            }
                            ++index1;
                        }
                    }
                }
                if (AppMain.gm_efct_zone_tex_reg_id != -1)
                {
                    if (AppMain.ObjAction3dESTextureReleaseDworkCheck(AppMain.ObjDataGet(510), AppMain.gm_efct_zone_tex_reg_id))
                        AppMain.gm_efct_zone_tex_reg_id = -1;
                    else
                        flag = false;
                }
                if (flag)
                {
                    if (AppMain.gm_efct_zone_mdl_tex_reg_id_list != null)
                        AppMain.gm_efct_zone_mdl_tex_reg_id_list = (int[])null;
                    if (AppMain.gm_efct_zone_model_reg_id_list != null)
                    {
                        AppMain.gm_efct_zone_model_reg_id_list = (int[])null;
                        AppMain.gm_efct_zone_model_reg_num = 0;
                    }
                    AppMain.gm_efct_zone_target_zone_no = -1;
                    AppMain.gm_efct_zone_proc_state = 0;
                }
                return flag;
        }
    }

    private static AppMain.GMS_EFFECT_3DES_WORK GmEfctZoneEsCreate(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int zone_no,
      int efct_zone_idx)
    {
        AppMain.GMS_EFCT_ZONE_CREATE_PARAM efctZoneCreateParam = AppMain.gm_efct_zone_create_info[zone_no].zone_create_param[efct_zone_idx];
        AppMain.OBS_DATA_WORK model_dwork;
        AppMain.OBS_DATA_WORK object_dwork;
        AppMain.OBS_DATA_WORK ambtex_dwork;
        AppMain.OBS_DATA_WORK texlist_dwork;
        if (efctZoneCreateParam.create_param.model_idx != -1)
        {
            int modelDworkNo = efctZoneCreateParam.model_dwork_no;
            int objectDworkNo = AppMain.gmEfctZoneGetObjectDworkNo(modelDworkNo);
            model_dwork = AppMain.ObjDataGet(modelDworkNo);
            object_dwork = AppMain.ObjDataGet(objectDworkNo);
            ambtex_dwork = AppMain.ObjDataGet(AppMain.gmEfctZoneGetMdlAmbtexDworkNo(modelDworkNo));
            texlist_dwork = AppMain.ObjDataGet(AppMain.gmEfctZoneGetMdlTexlistDworkNo(modelDworkNo));
        }
        else
        {
            model_dwork = (AppMain.OBS_DATA_WORK)null;
            object_dwork = (AppMain.OBS_DATA_WORK)null;
            ambtex_dwork = AppMain.ObjDataGet(509);
            texlist_dwork = AppMain.ObjDataGet(510);
        }
        return AppMain.GmEffect3dESCreateByParam(efctZoneCreateParam.create_param, parent_obj, AppMain.ObjDataGet(6).pData, AppMain.ObjDataGet(AppMain.gmEfctZoneGetAmeDworkNo(zone_no, efctZoneCreateParam.create_param.ame_idx)), ambtex_dwork, texlist_dwork, model_dwork, object_dwork);
    }

    private static int gmEfctZoneGetAmeDworkNo(int zone_no, int ame_amb_idx)
    {
        int num = 511;
        for (int index = 0; index < zone_no; ++index)
            num += AppMain.gm_efct_zone_texamb_index_tbl[index];
        return num + ame_amb_idx;
    }

    private static int gmEfctZoneGetObjectDworkNo(int model_dwork_no)
    {
        return 584 + (model_dwork_no - 582);
    }

    private static int gmEfctZoneGetMdlAmbtexDworkNo(int model_dwork_no)
    {
        return 586 + (model_dwork_no - 586);
    }

    private static int gmEfctZoneGetMdlTexlistDworkNo(int model_dwork_no)
    {
        return 588 + (model_dwork_no - 588);
    }
}