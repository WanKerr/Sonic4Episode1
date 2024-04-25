public partial class AppMain
{
    private static readonly int[] gm_efct_zone_texamb_index_tbl;
    private static readonly GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone01_create_param_tbl;
    private static readonly GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone02_create_param_tbl;
    private static readonly GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone03_create_param_tbl;
    private static readonly GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone04_create_param_tbl;
    private static readonly GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone_final_create_param_tbl;
    private static readonly GMS_EFCT_ZONE_CREATE_PARAM[] gm_efct_zone_ss_create_param_tbl;
    public static readonly GMS_EFCT_ZONE_CREATE_INFO[] gm_efct_zone_create_info;
    private static int gm_efct_zone_proc_state;
    private static int gm_efct_zone_tex_reg_id;
    private static int[] gm_efct_zone_model_reg_id_list;
    private static int gm_efct_zone_model_reg_num;
    private static int[] gm_efct_zone_mdl_tex_reg_id_list;
    private static int gm_efct_zone_target_zone_no;

    private static void GmEfctZoneBuildDataInit(int zone_no)
    {
        gm_efct_zone_proc_state = 1;
        gm_efct_zone_target_zone_no = zone_no;
    }

    private static void GmEfctZoneBuildDataLoopInit()
    {
        object texlist_buf = null;
        GMS_EFCT_ZONE_CREATE_INFO efctZoneCreateInfo = gm_efct_zone_create_info[gm_efct_zone_target_zone_no];
        int index1 = 0;
        AMS_AMB_HEADER amb = readAMBFile(ObjDataGetInc(ObjDataGet(6)));
        gm_efct_zone_model_reg_num = 1;
        if (gm_efct_zone_model_reg_num > 0)
        {
            gm_efct_zone_model_reg_id_list = new int[gm_efct_zone_model_reg_num];
            gm_efct_zone_mdl_tex_reg_id_list = new int[gm_efct_zone_model_reg_num];
            for (int index2 = 0; index2 < gm_efct_zone_model_reg_num; ++index2)
            {
                gm_efct_zone_model_reg_id_list[index2] = -1;
                gm_efct_zone_mdl_tex_reg_id_list[index2] = -1;
            }
        }
        OBS_DATA_WORK data_work1 = ObjDataGet(509);
        ObjDataLoadAmbIndex(data_work1, gm_efct_zone_texamb_index_tbl[gm_efct_zone_target_zone_no], amb);
        gm_efct_zone_tex_reg_id = ObjAction3dESTextureLoadToDwork(ObjDataGet(510), readAMBFile(data_work1.pData), ref texlist_buf);
        for (int index2 = 0; index2 < efctZoneCreateInfo.num; ++index2)
        {
            int modelIdx = efctZoneCreateInfo.zone_create_param[index2].create_param.model_idx;
            int modelDworkNo = efctZoneCreateInfo.zone_create_param[index2].model_dwork_no;
            int objectDworkNo = gmEfctZoneGetObjectDworkNo(modelDworkNo);
            int mdlAmbtexIdx = efctZoneCreateInfo.zone_create_param[index2].mdl_ambtex_idx;
            int mdlAmbtexDworkNo = gmEfctZoneGetMdlAmbtexDworkNo(modelDworkNo);
            int mdlTexlistDworkNo = gmEfctZoneGetMdlTexlistDworkNo(modelDworkNo);
            if (modelIdx != -1)
            {
                OBS_DATA_WORK data_work2 = ObjDataGet(mdlAmbtexDworkNo);
                ObjDataLoadAmbIndex(data_work2, mdlAmbtexIdx, amb);
                gm_efct_zone_mdl_tex_reg_id_list[index1] = ObjAction3dESTextureLoadToDwork(ObjDataGet(mdlTexlistDworkNo), readAMBFile(data_work2.pData), ref texlist_buf);
                ObjDataLoadAmbIndex(ObjDataGet(modelDworkNo), modelIdx, amb);
                gm_efct_zone_model_reg_id_list[index1] = ObjAction3dESModelLoadToDwork(ObjDataGet(objectDworkNo), (AmbChunk)ObjDataGet(modelDworkNo).pData, 0U);
                ++index1;
            }
        }
    }

    private static bool GmEfctZoneBuildDataLoop()
    {
        bool flag = true;
        if (gm_efct_zone_target_zone_no == -1)
            return flag;
        switch (gm_efct_zone_proc_state)
        {
            case 0:
                return true;
            case 1:
                if (GsMainSysGetDisplayListRegistNum() < 208)
                {
                    GmEfctZoneBuildDataLoopInit();
                    gm_efct_zone_proc_state = 2;
                }
                return false;
            default:
                if (gm_efct_zone_tex_reg_id != -1)
                {
                    if (amDrawIsRegistComplete(gm_efct_zone_tex_reg_id))
                        gm_efct_zone_tex_reg_id = -1;
                    else
                        flag = false;
                }
                for (int index = 0; index < gm_efct_zone_model_reg_num; ++index)
                {
                    if (gm_efct_zone_mdl_tex_reg_id_list[index] != -1)
                    {
                        if (amDrawIsRegistComplete(gm_efct_zone_mdl_tex_reg_id_list[index]))
                            gm_efct_zone_mdl_tex_reg_id_list[index] = -1;
                        else
                            flag = false;
                    }
                    if (gm_efct_zone_model_reg_id_list[index] != -1)
                    {
                        if (amDrawIsRegistComplete(gm_efct_zone_model_reg_id_list[index]))
                            gm_efct_zone_model_reg_id_list[index] = -1;
                        else
                            flag = false;
                    }
                }
                if (flag)
                {
                    gm_efct_zone_target_zone_no = -1;
                    gm_efct_zone_proc_state = 0;
                }
                return flag;
        }
    }

    private static void GmEfctZoneFlushDataInit(int zone_no)
    {
        gm_efct_zone_proc_state = 1;
        gm_efct_zone_target_zone_no = zone_no;
    }

    private static void GmEfctZoneFlushDataLoopInit()
    {
        GMS_EFCT_ZONE_CREATE_INFO efctZoneCreateInfo = gm_efct_zone_create_info[gm_efct_zone_target_zone_no];
        int index1 = 0;
        for (int index2 = 0; index2 < efctZoneCreateInfo.num; ++index2)
        {
            GMS_EFCT_ZONE_CREATE_PARAM efctZoneCreateParam = efctZoneCreateInfo.zone_create_param[index2];
            int modelDworkNo = efctZoneCreateParam.model_dwork_no;
            int objectDworkNo = gmEfctZoneGetObjectDworkNo(modelDworkNo);
            if (efctZoneCreateParam.create_param.model_idx != -1)
            {
                gm_efct_zone_model_reg_id_list[index1] = ObjAction3dESModelReleaseDwork(ObjDataGet(objectDworkNo));
                ObjDataRelease(ObjDataGet(modelDworkNo));
                gm_efct_zone_mdl_tex_reg_id_list[index1] = ObjAction3dESTextureReleaseDwork(ObjDataGet(gmEfctZoneGetMdlTexlistDworkNo(modelDworkNo)));
                ObjDataRelease(ObjDataGet(gmEfctZoneGetMdlAmbtexDworkNo(modelDworkNo)));
                ++index1;
            }
        }
        gm_efct_zone_tex_reg_id = ObjAction3dESTextureReleaseDwork(ObjDataGet(510));
        ObjDataRelease(ObjDataGet(509));
        ObjDataRelease(ObjDataGet(6));
    }

    private static bool GmEfctZoneFlushDataLoop()
    {
        bool flag = true;
        int index1 = 0;
        switch (gm_efct_zone_proc_state)
        {
            case 0:
                return true;
            case 1:
                if (GsMainSysGetDisplayListRegistNum() < 248)
                {
                    GmEfctZoneFlushDataLoopInit();
                    gm_efct_zone_proc_state = 2;
                }
                return false;
            default:
                GMS_EFCT_ZONE_CREATE_INFO efctZoneCreateInfo = gm_efct_zone_create_info[gm_efct_zone_target_zone_no];
                if (gm_efct_zone_model_reg_num != 0)
                {
                    for (int index2 = 0; index2 < efctZoneCreateInfo.num; ++index2)
                    {
                        GMS_EFCT_ZONE_CREATE_PARAM efctZoneCreateParam = efctZoneCreateInfo.zone_create_param[index2];
                        int modelDworkNo = efctZoneCreateParam.model_dwork_no;
                        int objectDworkNo = gmEfctZoneGetObjectDworkNo(modelDworkNo);
                        if (efctZoneCreateParam.create_param.model_idx != -1)
                        {
                            if (gm_efct_zone_model_reg_id_list[index1] != -1)
                            {
                                if (ObjAction3dESModelReleaseDworkCheck(ObjDataGet(objectDworkNo), gm_efct_zone_model_reg_id_list[index1]))
                                    gm_efct_zone_model_reg_id_list[index1] = -1;
                                else
                                    flag = false;
                            }
                            if (gm_efct_zone_mdl_tex_reg_id_list[index1] != -1)
                            {
                                if (ObjAction3dESTextureReleaseDworkCheck(ObjDataGet(gmEfctZoneGetMdlTexlistDworkNo(modelDworkNo)), gm_efct_zone_mdl_tex_reg_id_list[index1]))
                                    gm_efct_zone_mdl_tex_reg_id_list[index1] = -1;
                                else
                                    flag = false;
                            }
                            ++index1;
                        }
                    }
                }
                if (gm_efct_zone_tex_reg_id != -1)
                {
                    if (ObjAction3dESTextureReleaseDworkCheck(ObjDataGet(510), gm_efct_zone_tex_reg_id))
                        gm_efct_zone_tex_reg_id = -1;
                    else
                        flag = false;
                }
                if (flag)
                {
                    if (gm_efct_zone_mdl_tex_reg_id_list != null)
                        gm_efct_zone_mdl_tex_reg_id_list = null;
                    if (gm_efct_zone_model_reg_id_list != null)
                    {
                        gm_efct_zone_model_reg_id_list = null;
                        gm_efct_zone_model_reg_num = 0;
                    }
                    gm_efct_zone_target_zone_no = -1;
                    gm_efct_zone_proc_state = 0;
                }
                return flag;
        }
    }

    private static GMS_EFFECT_3DES_WORK GmEfctZoneEsCreate(
      OBS_OBJECT_WORK parent_obj,
      int zone_no,
      int efct_zone_idx)
    {
        GMS_EFCT_ZONE_CREATE_PARAM efctZoneCreateParam = gm_efct_zone_create_info[zone_no].zone_create_param[efct_zone_idx];
        OBS_DATA_WORK model_dwork;
        OBS_DATA_WORK object_dwork;
        OBS_DATA_WORK ambtex_dwork;
        OBS_DATA_WORK texlist_dwork;
        if (efctZoneCreateParam.create_param.model_idx != -1)
        {
            int modelDworkNo = efctZoneCreateParam.model_dwork_no;
            int objectDworkNo = gmEfctZoneGetObjectDworkNo(modelDworkNo);
            model_dwork = ObjDataGet(modelDworkNo);
            object_dwork = ObjDataGet(objectDworkNo);
            ambtex_dwork = ObjDataGet(gmEfctZoneGetMdlAmbtexDworkNo(modelDworkNo));
            texlist_dwork = ObjDataGet(gmEfctZoneGetMdlTexlistDworkNo(modelDworkNo));
        }
        else
        {
            model_dwork = null;
            object_dwork = null;
            ambtex_dwork = ObjDataGet(509);
            texlist_dwork = ObjDataGet(510);
        }
        return GmEffect3dESCreateByParam(efctZoneCreateParam.create_param, parent_obj, ObjDataGet(6).pData, ObjDataGet(gmEfctZoneGetAmeDworkNo(zone_no, efctZoneCreateParam.create_param.ame_idx)), ambtex_dwork, texlist_dwork, model_dwork, object_dwork);
    }

    private static int gmEfctZoneGetAmeDworkNo(int zone_no, int ame_amb_idx)
    {
        int num = 511;
        for (int index = 0; index < zone_no; ++index)
            num += gm_efct_zone_texamb_index_tbl[index];
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