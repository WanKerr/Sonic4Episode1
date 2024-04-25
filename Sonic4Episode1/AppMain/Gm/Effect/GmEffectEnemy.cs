public partial class AppMain
{
    public static uint GMM_EFCT_ENE_STAGE_FLAG(int stage_no)
    {
        return (uint)(1 << stage_no);
    }

    public static int GMM_EFCT_ENE_ARC_DW_NO(GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.arc_dwork_no;
    }

    public static int GMM_EFCT_ENE_AMBTEX_DW_NO(GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ambtex_dwork_no;
    }

    public static int GMM_EFCT_ENE_TEXLIST_DW_NO(GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ambtex_dwork_no + 1;
    }

    public static int GMM_EFCT_ENE_AME_DW_NO(GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ame_dwork_no;
    }

    public static int GMM_EFCT_ENE_MODEL_DW_NO(GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ame_dwork_no + 1;
    }

    public static int GMM_EFCT_ENE_OBJECT_DW_NO(GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ame_dwork_no + 2;
    }

    public static int GMM_EFCT_ENE_MDL_AMBTEX_DW_NO(GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ame_dwork_no + 3;
    }

    public static int GMM_EFCT_ENE_MDL_TEXLIST_DW_NO(GMS_EFCT_ENE_CREATE_PARAM p_cr_param)
    {
        return p_cr_param.ame_dwork_no + 4;
    }

    public static void GmEfctEneBuildDataInit(int zone_no)
    {
        object texlist_buf = null;
        for (int index = 0; index < 13; ++index)
        {
            gm_efct_ene_tex_reg_id_list[index] = -1;
            gm_efct_ene_model_reg_id_list[index] = -1;
        }
        gm_efct_ene_target_zone_no = zone_no;
        for (int index1 = 0; index1 < 13; ++index1)
        {
            GMS_EFCT_ENE_CREATE_PARAM p_cr_param = gm_efct_ene_create_param_tbl[index1];
            int arcDworkNo = p_cr_param.arc_dwork_no;
            if (((int)p_cr_param.stage_flag & (int)GMM_EFCT_ENE_STAGE_FLAG(zone_no)) != 0)
            {
                AMS_AMB_HEADER amb = readAMBFile(ObjDataGetInc(ObjDataGet(arcDworkNo)));
                int modelIdx = p_cr_param.create_param.model_idx;
                int index2 = GMM_EFCT_ENE_MODEL_DW_NO(p_cr_param);
                int index3 = GMM_EFCT_ENE_OBJECT_DW_NO(p_cr_param);
                OBS_DATA_WORK data_work;
                OBS_DATA_WORK texlist_dwork;
                if (modelIdx != -1)
                {
                    ObjDataLoadAmbIndex(ObjDataGet(index2), modelIdx, amb);
                    gm_efct_ene_model_reg_id_list[index1] = ObjAction3dESModelLoadToDwork(ObjDataGet(index3), (AmbChunk)ObjDataGet(index2).pData, 0U);
                    data_work = ObjDataGet(GMM_EFCT_ENE_MDL_AMBTEX_DW_NO(p_cr_param));
                    texlist_dwork = ObjDataGet(GMM_EFCT_ENE_MDL_TEXLIST_DW_NO(p_cr_param));
                }
                else
                {
                    data_work = ObjDataGet(GMM_EFCT_ENE_AMBTEX_DW_NO(p_cr_param));
                    texlist_dwork = ObjDataGet(GMM_EFCT_ENE_TEXLIST_DW_NO(p_cr_param));
                }
                ObjDataLoadAmbIndex(data_work, p_cr_param.ambtex_idx, amb);
                gm_efct_ene_tex_reg_id_list[index1] = ObjAction3dESTextureLoadToDwork(texlist_dwork, readAMBFile(data_work.pData), ref texlist_buf);
            }
        }
    }

    public static bool GmEfctEneBuildDataLoop()
    {
        bool flag = true;
        if (gm_efct_ene_target_zone_no == -1)
            return flag;
        for (int index = 0; index < 13; ++index)
        {
            if (((int)gm_efct_ene_create_param_tbl[index].stage_flag & (int)GMM_EFCT_ENE_STAGE_FLAG(gm_efct_ene_target_zone_no)) != 0)
            {
                if (gm_efct_ene_tex_reg_id_list[index] != -1)
                {
                    if (amDrawIsRegistComplete(gm_efct_ene_tex_reg_id_list[index]))
                        gm_efct_ene_tex_reg_id_list[index] = -1;
                    else
                        flag = false;
                }
                if (gm_efct_ene_model_reg_id_list[index] != -1)
                {
                    if (amDrawIsRegistComplete(gm_efct_ene_model_reg_id_list[index]))
                        gm_efct_ene_model_reg_id_list[index] = -1;
                    else
                        flag = false;
                }
            }
        }
        if (flag)
            gm_efct_ene_target_zone_no = -1;
        return flag;
    }

    public static void GmEfctEneFlushDataInit(int zone_no)
    {
        gm_efct_ene_target_zone_no = zone_no;
        for (int index = 0; index < 13; ++index)
        {
            GMS_EFCT_ENE_CREATE_PARAM p_cr_param = gm_efct_ene_create_param_tbl[index];
            if (((int)p_cr_param.stage_flag & (int)GMM_EFCT_ENE_STAGE_FLAG(zone_no)) != 0)
            {
                OBS_DATA_WORK pWork;
                OBS_DATA_WORK texlist_dwork;
                if (p_cr_param.create_param.model_idx != -1)
                {
                    gm_efct_ene_model_reg_id_list[index] = ObjAction3dESModelReleaseDwork(ObjDataGet(GMM_EFCT_ENE_OBJECT_DW_NO(p_cr_param)));
                    ObjDataRelease(ObjDataGet(GMM_EFCT_ENE_MODEL_DW_NO(p_cr_param)));
                    pWork = ObjDataGet(GMM_EFCT_ENE_MDL_AMBTEX_DW_NO(p_cr_param));
                    texlist_dwork = ObjDataGet(GMM_EFCT_ENE_MDL_TEXLIST_DW_NO(p_cr_param));
                }
                else
                {
                    pWork = ObjDataGet(GMM_EFCT_ENE_AMBTEX_DW_NO(p_cr_param));
                    texlist_dwork = ObjDataGet(GMM_EFCT_ENE_TEXLIST_DW_NO(p_cr_param));
                }
                gm_efct_ene_tex_reg_id_list[index] = ObjAction3dESTextureReleaseDwork(texlist_dwork);
                ObjDataRelease(pWork);
                ObjDataRelease(ObjDataGet(GMM_EFCT_ENE_ARC_DW_NO(p_cr_param)));
            }
        }
    }

    public static bool GmEfctEneFlushDataLoop()
    {
        bool flag = true;
        if (gm_efct_ene_target_zone_no == -1)
            return flag;
        for (int index = 0; index < 13; ++index)
        {
            GMS_EFCT_ENE_CREATE_PARAM p_cr_param = gm_efct_ene_create_param_tbl[index];
            if (((int)p_cr_param.stage_flag & (int)GMM_EFCT_ENE_STAGE_FLAG(gm_efct_ene_target_zone_no)) != 0)
            {
                OBS_DATA_WORK texlist_dwork = p_cr_param.create_param.model_idx == -1 ? ObjDataGet(GMM_EFCT_ENE_TEXLIST_DW_NO(p_cr_param)) : ObjDataGet(GMM_EFCT_ENE_MDL_TEXLIST_DW_NO(p_cr_param));
                if (gm_efct_ene_model_reg_id_list[index] != -1)
                {
                    if (ObjAction3dESModelReleaseDworkCheck(ObjDataGet(GMM_EFCT_ENE_OBJECT_DW_NO(p_cr_param)), gm_efct_ene_model_reg_id_list[index]))
                        gm_efct_ene_model_reg_id_list[index] = -1;
                    else
                        flag = false;
                }
                if (gm_efct_ene_tex_reg_id_list[index] != -1)
                {
                    if (ObjAction3dESTextureReleaseDworkCheck(texlist_dwork, gm_efct_ene_tex_reg_id_list[index]))
                        gm_efct_ene_tex_reg_id_list[index] = -1;
                    else
                        flag = false;
                }
            }
        }
        if (flag)
            gm_efct_ene_target_zone_no = -1;
        return flag;
    }

    public static GMS_EFFECT_3DES_WORK GmEfctEneEsCreate(
      OBS_OBJECT_WORK parent_obj,
      int efct_ene_idx)
    {
        GMS_EFCT_ENE_CREATE_PARAM p_cr_param = gm_efct_ene_create_param_tbl[efct_ene_idx];
        OBS_DATA_WORK model_dwork;
        OBS_DATA_WORK object_dwork;
        if (p_cr_param.create_param.model_idx != -1)
        {
            model_dwork = ObjDataGet(GMM_EFCT_ENE_MODEL_DW_NO(p_cr_param));
            object_dwork = ObjDataGet(GMM_EFCT_ENE_OBJECT_DW_NO(p_cr_param));
        }
        else
        {
            model_dwork = null;
            object_dwork = null;
        }
        return GmEffect3dESCreateByParam(p_cr_param.create_param, parent_obj, ObjDataGet(GMM_EFCT_ENE_ARC_DW_NO(p_cr_param)).pData, ObjDataGet(GMM_EFCT_ENE_AME_DW_NO(p_cr_param)), ObjDataGet(GMM_EFCT_ENE_AMBTEX_DW_NO(p_cr_param)), ObjDataGet(GMM_EFCT_ENE_TEXLIST_DW_NO(p_cr_param)), model_dwork, object_dwork);
    }

}