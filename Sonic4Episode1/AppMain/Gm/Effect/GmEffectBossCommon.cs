public partial class AppMain
{
    public static void GmEfctBossCmnBuildDataInit()
    {
        object texlist_buf = null;
        int index1 = 0;
        AMS_AMB_HEADER amb = readAMBFile(ObjDataGetInc(ObjDataGet(15)));
        gm_efct_boss_cmn_model_reg_num = 6;
        if (gm_efct_boss_cmn_model_reg_num > 0)
        {
            gm_efct_boss_cmn_model_reg_id_list = new int[gm_efct_boss_cmn_model_reg_num];
            gm_efct_boss_cmn_mdl_tex_reg_id_list = new int[gm_efct_boss_cmn_model_reg_num];
            for (int index2 = 0; index2 < gm_efct_boss_cmn_model_reg_num; ++index2)
            {
                gm_efct_boss_cmn_model_reg_id_list[index2] = -1;
                gm_efct_boss_cmn_mdl_tex_reg_id_list[index2] = -1;
            }
        }
        OBS_DATA_WORK data_work1 = ObjDataGet(621);
        ObjDataLoadAmbIndex(data_work1, 6, amb);
        gm_efct_boss_cmn_tex_reg_id = ObjAction3dESTextureLoadToDwork(ObjDataGet(622), readAMBFile(data_work1.pData), ref texlist_buf);
        for (int ame_idx = 0; ame_idx < 6; ++ame_idx)
        {
            GMS_EFCT_BOSS_CMN_CREATE_PARAM bossCmnCreateParam = gm_efct_boss_cmn_create_param_tbl[ame_idx];
            int modelIdx = bossCmnCreateParam.create_param.model_idx;
            int modelDworkNo = gmEfctBossCmnGetModelDworkNo(ame_idx);
            int objectDworkNo = gmEfctBossCmnGetObjectDworkNo(ame_idx);
            int mdlAmbtexIdx = bossCmnCreateParam.mdl_ambtex_idx;
            int mdlAmbtexDworkNo = gmEfctBossCmnGetMdlAmbtexDworkNo(ame_idx);
            int mdlTexlistDworkNo = gmEfctBossCmnGetMdlTexlistDworkNo(ame_idx);
            if (modelIdx != -1)
            {
                OBS_DATA_WORK data_work2 = ObjDataGet(mdlAmbtexDworkNo);
                ObjDataLoadAmbIndex(data_work2, mdlAmbtexIdx, amb);
                gm_efct_boss_cmn_mdl_tex_reg_id_list[index1] = ObjAction3dESTextureLoadToDwork(ObjDataGet(mdlTexlistDworkNo), readAMBFile(data_work2.pData), ref texlist_buf);
                ObjDataLoadAmbIndex(ObjDataGet(modelDworkNo), modelIdx, amb);
                gm_efct_boss_cmn_model_reg_id_list[index1] = ObjAction3dESModelLoadToDwork(ObjDataGet(objectDworkNo), (AmbChunk)ObjDataGet(modelDworkNo).pData, 0U);
                ++index1;
            }
        }
    }

    public static bool GmEfctBossCmnBuildDataLoop()
    {
        bool flag = true;
        if (gm_efct_boss_cmn_tex_reg_id != -1)
        {
            if (amDrawIsRegistComplete(gm_efct_boss_cmn_tex_reg_id))
                gm_efct_boss_cmn_tex_reg_id = -1;
            else
                flag = false;
        }
        for (int index = 0; index < gm_efct_boss_cmn_model_reg_num; ++index)
        {
            if (gm_efct_boss_cmn_mdl_tex_reg_id_list[index] != -1)
            {
                if (amDrawIsRegistComplete(gm_efct_boss_cmn_mdl_tex_reg_id_list[index]))
                    gm_efct_boss_cmn_mdl_tex_reg_id_list[index] = -1;
                else
                    flag = false;
            }
            if (gm_efct_boss_cmn_model_reg_id_list[index] != -1)
            {
                if (amDrawIsRegistComplete(gm_efct_boss_cmn_model_reg_id_list[index]))
                    gm_efct_boss_cmn_model_reg_id_list[index] = -1;
                else
                    flag = false;
            }
        }
        return flag;
    }

    public static void GmEfctBossCmnFlushDataInit()
    {
        int index = 0;
        for (int ame_idx = 0; ame_idx < 6; ++ame_idx)
        {
            GMS_EFCT_BOSS_CMN_CREATE_PARAM bossCmnCreateParam = gm_efct_boss_cmn_create_param_tbl[ame_idx];
            int modelDworkNo = gmEfctBossCmnGetModelDworkNo(ame_idx);
            int objectDworkNo = gmEfctBossCmnGetObjectDworkNo(ame_idx);
            if (bossCmnCreateParam.create_param.model_idx != -1)
            {
                gm_efct_boss_cmn_model_reg_id_list[index] = ObjAction3dESModelReleaseDwork(ObjDataGet(objectDworkNo));
                ObjDataRelease(ObjDataGet(modelDworkNo));
                gm_efct_boss_cmn_mdl_tex_reg_id_list[index] = ObjAction3dESTextureReleaseDwork(ObjDataGet(gmEfctBossCmnGetMdlTexlistDworkNo(ame_idx)));
                ObjDataRelease(ObjDataGet(gmEfctBossCmnGetMdlAmbtexDworkNo(ame_idx)));
                ++index;
            }
        }
        gm_efct_boss_cmn_tex_reg_id = ObjAction3dESTextureReleaseDwork(ObjDataGet(622));
        ObjDataRelease(ObjDataGet(621));
        ObjDataRelease(ObjDataGet(15));
    }

    public static bool GmEfctBossCmnFlushDataLoop()
    {
        bool flag = true;
        int index = 0;
        if (gm_efct_boss_cmn_model_reg_num != 0)
        {
            for (int ame_idx = 0; ame_idx < 6; ++ame_idx)
            {
                if (gm_efct_boss_cmn_create_param_tbl[ame_idx].create_param.model_idx != -1)
                {
                    if (gm_efct_boss_cmn_model_reg_id_list[index] != -1)
                    {
                        if (ObjAction3dESModelReleaseDworkCheck(ObjDataGet(gmEfctBossCmnGetObjectDworkNo(ame_idx)), gm_efct_boss_cmn_model_reg_id_list[index]))
                            gm_efct_boss_cmn_model_reg_id_list[index] = -1;
                        else
                            flag = false;
                    }
                    if (gm_efct_boss_cmn_mdl_tex_reg_id_list[index] != -1)
                    {
                        if (ObjAction3dESTextureReleaseDworkCheck(ObjDataGet(gmEfctBossCmnGetMdlTexlistDworkNo(ame_idx)), gm_efct_boss_cmn_mdl_tex_reg_id_list[index]))
                            gm_efct_boss_cmn_mdl_tex_reg_id_list[index] = -1;
                        else
                            flag = false;
                    }
                    ++index;
                }
            }
        }
        if (gm_efct_boss_cmn_tex_reg_id != -1)
        {
            if (ObjAction3dESTextureReleaseDworkCheck(ObjDataGet(622), gm_efct_boss_cmn_tex_reg_id))
                gm_efct_boss_cmn_tex_reg_id = -1;
            else
                flag = false;
        }
        if (flag)
        {
            if (gm_efct_boss_cmn_mdl_tex_reg_id_list != null)
                gm_efct_boss_cmn_mdl_tex_reg_id_list = null;
            if (gm_efct_boss_cmn_model_reg_id_list != null)
            {
                gm_efct_boss_cmn_model_reg_id_list = null;
                gm_efct_boss_cmn_model_reg_num = 0;
            }
        }
        return flag;
    }

    public static void GmEfctBossBuildSingleDataInit()
    {
        gm_efct_boss_single_reg_num = 0;
    }

    public static void GmEfctBossBuildSingleDataReg(
      int tex_index,
      OBS_DATA_WORK ambtex_dwork,
      OBS_DATA_WORK texlist_dwork,
      int model_index,
      OBS_DATA_WORK model_dwork,
      OBS_DATA_WORK object_dwork,
      AMS_AMB_HEADER arc)
    {
        object texlist_buf = null;
        GMS_EFCT_BOSS_SINGLE_BUILD_WORK efctBossSingleBuild = gm_efct_boss_single_build_list[gm_efct_boss_single_reg_num];
        ++gm_efct_boss_single_reg_num;
        ObjDataLoadAmbIndex(ambtex_dwork, tex_index, arc);
        efctBossSingleBuild.tex_reg_id = ObjAction3dESTextureLoadToDwork(texlist_dwork, readAMBFile(ambtex_dwork.pData), ref texlist_buf);
        efctBossSingleBuild.ambtex_dwork = ambtex_dwork;
        efctBossSingleBuild.texlist_dwork = texlist_dwork;
        if (model_dwork != null)
        {
            ObjDataLoadAmbIndex(model_dwork, model_index, arc);
            efctBossSingleBuild.model_reg_id = ObjAction3dESModelLoadToDwork(object_dwork, (AmbChunk)model_dwork.pData, 0U);
            efctBossSingleBuild.model_dwork = model_dwork;
            efctBossSingleBuild.object_dwork = object_dwork;
        }
        else
        {
            efctBossSingleBuild.model_reg_id = -1;
            efctBossSingleBuild.model_dwork = null;
            efctBossSingleBuild.object_dwork = null;
        }
    }

    public static bool GmEfctBossBuildSingleDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < gm_efct_boss_single_reg_num; ++index)
        {
            GMS_EFCT_BOSS_SINGLE_BUILD_WORK efctBossSingleBuild = gm_efct_boss_single_build_list[index];
            if (efctBossSingleBuild.tex_reg_id != -1)
            {
                if (amDrawIsRegistComplete(efctBossSingleBuild.tex_reg_id))
                    efctBossSingleBuild.tex_reg_id = -1;
                else
                    flag = false;
            }
            if (efctBossSingleBuild.model_reg_id != -1)
            {
                if (amDrawIsRegistComplete(efctBossSingleBuild.model_reg_id))
                    efctBossSingleBuild.model_reg_id = -1;
                else
                    flag = false;
            }
        }
        return flag;
    }

    public static void GmEfctBossFlushSingleDataInit()
    {
        for (int index = 0; index < gm_efct_boss_single_reg_num; ++index)
        {
            GMS_EFCT_BOSS_SINGLE_BUILD_WORK efctBossSingleBuild = gm_efct_boss_single_build_list[index];
            if (efctBossSingleBuild.object_dwork != null)
            {
                efctBossSingleBuild.model_reg_id = ObjAction3dESModelReleaseDwork(efctBossSingleBuild.object_dwork);
                ObjDataRelease(efctBossSingleBuild.model_dwork);
                efctBossSingleBuild.model_dwork = null;
            }
            efctBossSingleBuild.tex_reg_id = ObjAction3dESTextureReleaseDwork(efctBossSingleBuild.texlist_dwork);
            ObjDataRelease(efctBossSingleBuild.ambtex_dwork);
            efctBossSingleBuild.ambtex_dwork = null;
        }
    }

    public static bool GmEfctBossFlushSingleDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < gm_efct_boss_single_reg_num; ++index)
        {
            GMS_EFCT_BOSS_SINGLE_BUILD_WORK efctBossSingleBuild = gm_efct_boss_single_build_list[index];
            if (efctBossSingleBuild.model_reg_id != -1)
            {
                if (ObjAction3dESModelReleaseDworkCheck(efctBossSingleBuild.object_dwork, efctBossSingleBuild.model_reg_id))
                {
                    efctBossSingleBuild.model_reg_id = -1;
                    efctBossSingleBuild.object_dwork = null;
                }
                else
                    flag = false;
            }
            if (efctBossSingleBuild.tex_reg_id != -1)
            {
                if (ObjAction3dESTextureReleaseDworkCheck(efctBossSingleBuild.texlist_dwork, efctBossSingleBuild.tex_reg_id))
                {
                    efctBossSingleBuild.tex_reg_id = -1;
                    efctBossSingleBuild.texlist_dwork = null;
                }
                else
                    flag = false;
            }
        }
        if (flag)
            gm_efct_boss_single_reg_num = 0;
        return flag;
    }

    public static GMS_EFFECT_3DES_WORK GmEfctBossCmnEsCreate(
      OBS_OBJECT_WORK parent_obj,
      uint efct_bscmn_idx)
    {
        GMS_EFCT_BOSS_CMN_CREATE_PARAM bossCmnCreateParam = gm_efct_boss_cmn_create_param_tbl[(int)efct_bscmn_idx];
        OBS_DATA_WORK model_dwork;
        OBS_DATA_WORK object_dwork;
        OBS_DATA_WORK ambtex_dwork;
        OBS_DATA_WORK texlist_dwork;
        if (bossCmnCreateParam.create_param.model_idx != -1)
        {
            int ameIdx = bossCmnCreateParam.create_param.ame_idx;
            model_dwork = ObjDataGet(gmEfctBossCmnGetModelDworkNo(ameIdx));
            object_dwork = ObjDataGet(gmEfctBossCmnGetObjectDworkNo(ameIdx));
            ambtex_dwork = ObjDataGet(gmEfctBossCmnGetMdlAmbtexDworkNo(ameIdx));
            texlist_dwork = ObjDataGet(gmEfctBossCmnGetMdlTexlistDworkNo(ameIdx));
        }
        else
        {
            model_dwork = null;
            object_dwork = null;
            ambtex_dwork = ObjDataGet(621);
            texlist_dwork = ObjDataGet(622);
        }
        return GmEffect3dESCreateByParam(bossCmnCreateParam.create_param, parent_obj, ObjDataGet(15).pData, ObjDataGet(gmEfctBossCmnGetAmeDworkNo(bossCmnCreateParam.create_param.ame_idx)), ambtex_dwork, texlist_dwork, model_dwork, object_dwork);
    }

    public static int gmEfctBossCmnGetAmeDworkNo(int ame_idx)
    {
        return 623 + ame_idx;
    }

    public static int gmEfctBossCmnGetModelDworkNo(int ame_idx)
    {
        return 630 + ame_idx;
    }

    public static int gmEfctBossCmnGetObjectDworkNo(int ame_idx)
    {
        return 637 + ame_idx;
    }

    public static int gmEfctBossCmnGetMdlAmbtexDworkNo(int ame_idx)
    {
        return 644 + ame_idx;
    }

    public static int gmEfctBossCmnGetMdlTexlistDworkNo(int ame_idx)
    {
        return 651 + ame_idx;
    }

}