using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    public static uint GMM_EFCT_ENE_STAGE_FLAG(int stage_no)
    {
        return (uint)(1 << stage_no);
    }

    public static int GMM_EFCT_ENE_ARC_DW_NO(AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.arc_dwork_no;
    }

    public static int GMM_EFCT_ENE_AMBTEX_DW_NO(AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ambtex_dwork_no;
    }

    public static int GMM_EFCT_ENE_TEXLIST_DW_NO(AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ambtex_dwork_no + 1;
    }

    public static int GMM_EFCT_ENE_AME_DW_NO(AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ame_dwork_no;
    }

    public static int GMM_EFCT_ENE_MODEL_DW_NO(AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ame_dwork_no + 1;
    }

    public static int GMM_EFCT_ENE_OBJECT_DW_NO(AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ame_dwork_no + 2;
    }

    public static int GMM_EFCT_ENE_MDL_AMBTEX_DW_NO(AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ame_dwork_no + 3;
    }

    public static int GMM_EFCT_ENE_MDL_TEXLIST_DW_NO(AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ame_dwork_no + 4;
    }

    public static void GmEfctEneBuildDataInit(int zone_no)
    {
        object texlist_buf = (object)null;
        for (int index = 0; index < 13; ++index)
        {
            AppMain.gm_efct_ene_tex_reg_id_list[index] = -1;
            AppMain.gm_efct_ene_model_reg_id_list[index] = -1;
        }
        AppMain.gm_efct_ene_target_zone_no = zone_no;
        for (int index1 = 0; index1 < 13; ++index1)
        {
            AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param = AppMain.gm_efct_ene_create_param_tbl[index1];
            int arcDworkNo = p_cr_param.arc_dwork_no;
            if (((int)p_cr_param.stage_flag & (int)AppMain.GMM_EFCT_ENE_STAGE_FLAG(zone_no)) != 0)
            {
                AppMain.AMS_AMB_HEADER amb = AppMain.readAMBFile(AppMain.ObjDataGetInc(AppMain.ObjDataGet(arcDworkNo)));
                int modelIdx = p_cr_param.create_param.model_idx;
                int index2 = AppMain.GMM_EFCT_ENE_MODEL_DW_NO(p_cr_param);
                int index3 = AppMain.GMM_EFCT_ENE_OBJECT_DW_NO(p_cr_param);
                AppMain.OBS_DATA_WORK data_work;
                AppMain.OBS_DATA_WORK texlist_dwork;
                if (modelIdx != -1)
                {
                    AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(index2), modelIdx, amb);
                    AppMain.gm_efct_ene_model_reg_id_list[index1] = AppMain.ObjAction3dESModelLoadToDwork(AppMain.ObjDataGet(index3), (object)(AppMain.AmbChunk)AppMain.ObjDataGet(index2).pData, 0U);
                    data_work = AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_MDL_AMBTEX_DW_NO(p_cr_param));
                    texlist_dwork = AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_MDL_TEXLIST_DW_NO(p_cr_param));
                }
                else
                {
                    data_work = AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_AMBTEX_DW_NO(p_cr_param));
                    texlist_dwork = AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_TEXLIST_DW_NO(p_cr_param));
                }
                AppMain.ObjDataLoadAmbIndex(data_work, p_cr_param.ambtex_idx, amb);
                AppMain.gm_efct_ene_tex_reg_id_list[index1] = AppMain.ObjAction3dESTextureLoadToDwork(texlist_dwork, AppMain.readAMBFile(data_work.pData), ref texlist_buf);
            }
        }
    }

    public static bool GmEfctEneBuildDataLoop()
    {
        bool flag = true;
        if (AppMain.gm_efct_ene_target_zone_no == -1)
            return flag;
        for (int index = 0; index < 13; ++index)
        {
            if (((int)AppMain.gm_efct_ene_create_param_tbl[index].stage_flag & (int)AppMain.GMM_EFCT_ENE_STAGE_FLAG(AppMain.gm_efct_ene_target_zone_no)) != 0)
            {
                if (AppMain.gm_efct_ene_tex_reg_id_list[index] != -1)
                {
                    if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_ene_tex_reg_id_list[index]))
                        AppMain.gm_efct_ene_tex_reg_id_list[index] = -1;
                    else
                        flag = false;
                }
                if (AppMain.gm_efct_ene_model_reg_id_list[index] != -1)
                {
                    if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_ene_model_reg_id_list[index]))
                        AppMain.gm_efct_ene_model_reg_id_list[index] = -1;
                    else
                        flag = false;
                }
            }
        }
        if (flag)
            AppMain.gm_efct_ene_target_zone_no = -1;
        return flag;
    }

    public static void GmEfctEneFlushDataInit(int zone_no)
    {
        AppMain.gm_efct_ene_target_zone_no = zone_no;
        for (int index = 0; index < 13; ++index)
        {
            AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param = AppMain.gm_efct_ene_create_param_tbl[index];
            if (((int)p_cr_param.stage_flag & (int)AppMain.GMM_EFCT_ENE_STAGE_FLAG(zone_no)) != 0)
            {
                AppMain.OBS_DATA_WORK pWork;
                AppMain.OBS_DATA_WORK texlist_dwork;
                if (p_cr_param.create_param.model_idx != -1)
                {
                    AppMain.gm_efct_ene_model_reg_id_list[index] = AppMain.ObjAction3dESModelReleaseDwork(AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_OBJECT_DW_NO(p_cr_param)));
                    AppMain.ObjDataRelease(AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_MODEL_DW_NO(p_cr_param)));
                    pWork = AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_MDL_AMBTEX_DW_NO(p_cr_param));
                    texlist_dwork = AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_MDL_TEXLIST_DW_NO(p_cr_param));
                }
                else
                {
                    pWork = AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_AMBTEX_DW_NO(p_cr_param));
                    texlist_dwork = AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_TEXLIST_DW_NO(p_cr_param));
                }
                AppMain.gm_efct_ene_tex_reg_id_list[index] = AppMain.ObjAction3dESTextureReleaseDwork(texlist_dwork);
                AppMain.ObjDataRelease(pWork);
                AppMain.ObjDataRelease(AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_ARC_DW_NO(p_cr_param)));
            }
        }
    }

    public static bool GmEfctEneFlushDataLoop()
    {
        bool flag = true;
        if (AppMain.gm_efct_ene_target_zone_no == -1)
            return flag;
        for (int index = 0; index < 13; ++index)
        {
            AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param = AppMain.gm_efct_ene_create_param_tbl[index];
            if (((int)p_cr_param.stage_flag & (int)AppMain.GMM_EFCT_ENE_STAGE_FLAG(AppMain.gm_efct_ene_target_zone_no)) != 0)
            {
                AppMain.OBS_DATA_WORK texlist_dwork = p_cr_param.create_param.model_idx == -1 ? AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_TEXLIST_DW_NO(p_cr_param)) : AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_MDL_TEXLIST_DW_NO(p_cr_param));
                if (AppMain.gm_efct_ene_model_reg_id_list[index] != -1)
                {
                    if (AppMain.ObjAction3dESModelReleaseDworkCheck(AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_OBJECT_DW_NO(p_cr_param)), AppMain.gm_efct_ene_model_reg_id_list[index]))
                        AppMain.gm_efct_ene_model_reg_id_list[index] = -1;
                    else
                        flag = false;
                }
                if (AppMain.gm_efct_ene_tex_reg_id_list[index] != -1)
                {
                    if (AppMain.ObjAction3dESTextureReleaseDworkCheck(texlist_dwork, AppMain.gm_efct_ene_tex_reg_id_list[index]))
                        AppMain.gm_efct_ene_tex_reg_id_list[index] = -1;
                    else
                        flag = false;
                }
            }
        }
        if (flag)
            AppMain.gm_efct_ene_target_zone_no = -1;
        return flag;
    }

    public static AppMain.GMS_EFFECT_3DES_WORK GmEfctEneEsCreate(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int efct_ene_idx)
    {
        AppMain.GMS_EFCT_ENE_CREATE_PARAM p_cr_param = AppMain.gm_efct_ene_create_param_tbl[efct_ene_idx];
        AppMain.OBS_DATA_WORK model_dwork;
        AppMain.OBS_DATA_WORK object_dwork;
        if (p_cr_param.create_param.model_idx != -1)
        {
            model_dwork = AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_MODEL_DW_NO(p_cr_param));
            object_dwork = AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_OBJECT_DW_NO(p_cr_param));
        }
        else
        {
            model_dwork = (AppMain.OBS_DATA_WORK)null;
            object_dwork = (AppMain.OBS_DATA_WORK)null;
        }
        return AppMain.GmEffect3dESCreateByParam(p_cr_param.create_param, parent_obj, AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_ARC_DW_NO(p_cr_param)).pData, AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_AME_DW_NO(p_cr_param)), AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_AMBTEX_DW_NO(p_cr_param)), AppMain.ObjDataGet(AppMain.GMM_EFCT_ENE_TEXLIST_DW_NO(p_cr_param)), model_dwork, object_dwork);
    }

}