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
    public static void GmEfctBossCmnBuildDataInit()
    {
        object texlist_buf = (object)null;
        int index1 = 0;
        AppMain.AMS_AMB_HEADER amb = AppMain.readAMBFile(AppMain.ObjDataGetInc(AppMain.ObjDataGet(15)));
        AppMain.gm_efct_boss_cmn_model_reg_num = 6;
        if (AppMain.gm_efct_boss_cmn_model_reg_num > 0)
        {
            AppMain.gm_efct_boss_cmn_model_reg_id_list = new int[AppMain.gm_efct_boss_cmn_model_reg_num];
            AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list = new int[AppMain.gm_efct_boss_cmn_model_reg_num];
            for (int index2 = 0; index2 < AppMain.gm_efct_boss_cmn_model_reg_num; ++index2)
            {
                AppMain.gm_efct_boss_cmn_model_reg_id_list[index2] = -1;
                AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list[index2] = -1;
            }
        }
        AppMain.OBS_DATA_WORK data_work1 = AppMain.ObjDataGet(621);
        AppMain.ObjDataLoadAmbIndex(data_work1, 6, amb);
        AppMain.gm_efct_boss_cmn_tex_reg_id = AppMain.ObjAction3dESTextureLoadToDwork(AppMain.ObjDataGet(622), AppMain.readAMBFile(data_work1.pData), ref texlist_buf);
        for (int ame_idx = 0; ame_idx < 6; ++ame_idx)
        {
            AppMain.GMS_EFCT_BOSS_CMN_CREATE_PARAM bossCmnCreateParam = AppMain.gm_efct_boss_cmn_create_param_tbl[ame_idx];
            int modelIdx = bossCmnCreateParam.create_param.model_idx;
            int modelDworkNo = AppMain.gmEfctBossCmnGetModelDworkNo(ame_idx);
            int objectDworkNo = AppMain.gmEfctBossCmnGetObjectDworkNo(ame_idx);
            int mdlAmbtexIdx = bossCmnCreateParam.mdl_ambtex_idx;
            int mdlAmbtexDworkNo = AppMain.gmEfctBossCmnGetMdlAmbtexDworkNo(ame_idx);
            int mdlTexlistDworkNo = AppMain.gmEfctBossCmnGetMdlTexlistDworkNo(ame_idx);
            if (modelIdx != -1)
            {
                AppMain.OBS_DATA_WORK data_work2 = AppMain.ObjDataGet(mdlAmbtexDworkNo);
                AppMain.ObjDataLoadAmbIndex(data_work2, mdlAmbtexIdx, amb);
                AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list[index1] = AppMain.ObjAction3dESTextureLoadToDwork(AppMain.ObjDataGet(mdlTexlistDworkNo), AppMain.readAMBFile(data_work2.pData), ref texlist_buf);
                AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(modelDworkNo), modelIdx, amb);
                AppMain.gm_efct_boss_cmn_model_reg_id_list[index1] = AppMain.ObjAction3dESModelLoadToDwork(AppMain.ObjDataGet(objectDworkNo), (object)(AppMain.AmbChunk)AppMain.ObjDataGet(modelDworkNo).pData, 0U);
                ++index1;
            }
        }
    }

    public static bool GmEfctBossCmnBuildDataLoop()
    {
        bool flag = true;
        if (AppMain.gm_efct_boss_cmn_tex_reg_id != -1)
        {
            if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_boss_cmn_tex_reg_id))
                AppMain.gm_efct_boss_cmn_tex_reg_id = -1;
            else
                flag = false;
        }
        for (int index = 0; index < AppMain.gm_efct_boss_cmn_model_reg_num; ++index)
        {
            if (AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list[index] != -1)
            {
                if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list[index]))
                    AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list[index] = -1;
                else
                    flag = false;
            }
            if (AppMain.gm_efct_boss_cmn_model_reg_id_list[index] != -1)
            {
                if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_boss_cmn_model_reg_id_list[index]))
                    AppMain.gm_efct_boss_cmn_model_reg_id_list[index] = -1;
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
            AppMain.GMS_EFCT_BOSS_CMN_CREATE_PARAM bossCmnCreateParam = AppMain.gm_efct_boss_cmn_create_param_tbl[ame_idx];
            int modelDworkNo = AppMain.gmEfctBossCmnGetModelDworkNo(ame_idx);
            int objectDworkNo = AppMain.gmEfctBossCmnGetObjectDworkNo(ame_idx);
            if (bossCmnCreateParam.create_param.model_idx != -1)
            {
                AppMain.gm_efct_boss_cmn_model_reg_id_list[index] = AppMain.ObjAction3dESModelReleaseDwork(AppMain.ObjDataGet(objectDworkNo));
                AppMain.ObjDataRelease(AppMain.ObjDataGet(modelDworkNo));
                AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list[index] = AppMain.ObjAction3dESTextureReleaseDwork(AppMain.ObjDataGet(AppMain.gmEfctBossCmnGetMdlTexlistDworkNo(ame_idx)));
                AppMain.ObjDataRelease(AppMain.ObjDataGet(AppMain.gmEfctBossCmnGetMdlAmbtexDworkNo(ame_idx)));
                ++index;
            }
        }
        AppMain.gm_efct_boss_cmn_tex_reg_id = AppMain.ObjAction3dESTextureReleaseDwork(AppMain.ObjDataGet(622));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(621));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(15));
    }

    public static bool GmEfctBossCmnFlushDataLoop()
    {
        bool flag = true;
        int index = 0;
        if (AppMain.gm_efct_boss_cmn_model_reg_num != 0)
        {
            for (int ame_idx = 0; ame_idx < 6; ++ame_idx)
            {
                if (AppMain.gm_efct_boss_cmn_create_param_tbl[ame_idx].create_param.model_idx != -1)
                {
                    if (AppMain.gm_efct_boss_cmn_model_reg_id_list[index] != -1)
                    {
                        if (AppMain.ObjAction3dESModelReleaseDworkCheck(AppMain.ObjDataGet(AppMain.gmEfctBossCmnGetObjectDworkNo(ame_idx)), AppMain.gm_efct_boss_cmn_model_reg_id_list[index]))
                            AppMain.gm_efct_boss_cmn_model_reg_id_list[index] = -1;
                        else
                            flag = false;
                    }
                    if (AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list[index] != -1)
                    {
                        if (AppMain.ObjAction3dESTextureReleaseDworkCheck(AppMain.ObjDataGet(AppMain.gmEfctBossCmnGetMdlTexlistDworkNo(ame_idx)), AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list[index]))
                            AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list[index] = -1;
                        else
                            flag = false;
                    }
                    ++index;
                }
            }
        }
        if (AppMain.gm_efct_boss_cmn_tex_reg_id != -1)
        {
            if (AppMain.ObjAction3dESTextureReleaseDworkCheck(AppMain.ObjDataGet(622), AppMain.gm_efct_boss_cmn_tex_reg_id))
                AppMain.gm_efct_boss_cmn_tex_reg_id = -1;
            else
                flag = false;
        }
        if (flag)
        {
            if (AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list != null)
                AppMain.gm_efct_boss_cmn_mdl_tex_reg_id_list = (int[])null;
            if (AppMain.gm_efct_boss_cmn_model_reg_id_list != null)
            {
                AppMain.gm_efct_boss_cmn_model_reg_id_list = (int[])null;
                AppMain.gm_efct_boss_cmn_model_reg_num = 0;
            }
        }
        return flag;
    }

    public static void GmEfctBossBuildSingleDataInit()
    {
        AppMain.gm_efct_boss_single_reg_num = 0;
    }

    public static void GmEfctBossBuildSingleDataReg(
      int tex_index,
      AppMain.OBS_DATA_WORK ambtex_dwork,
      AppMain.OBS_DATA_WORK texlist_dwork,
      int model_index,
      AppMain.OBS_DATA_WORK model_dwork,
      AppMain.OBS_DATA_WORK object_dwork,
      AppMain.AMS_AMB_HEADER arc)
    {
        object texlist_buf = (object)null;
        AppMain.GMS_EFCT_BOSS_SINGLE_BUILD_WORK efctBossSingleBuild = AppMain.gm_efct_boss_single_build_list[AppMain.gm_efct_boss_single_reg_num];
        ++AppMain.gm_efct_boss_single_reg_num;
        AppMain.ObjDataLoadAmbIndex(ambtex_dwork, tex_index, arc);
        efctBossSingleBuild.tex_reg_id = AppMain.ObjAction3dESTextureLoadToDwork(texlist_dwork, AppMain.readAMBFile(ambtex_dwork.pData), ref texlist_buf);
        efctBossSingleBuild.ambtex_dwork = ambtex_dwork;
        efctBossSingleBuild.texlist_dwork = texlist_dwork;
        if (model_dwork != null)
        {
            AppMain.ObjDataLoadAmbIndex(model_dwork, model_index, arc);
            efctBossSingleBuild.model_reg_id = AppMain.ObjAction3dESModelLoadToDwork(object_dwork, (object)(AppMain.AmbChunk)model_dwork.pData, 0U);
            efctBossSingleBuild.model_dwork = model_dwork;
            efctBossSingleBuild.object_dwork = object_dwork;
        }
        else
        {
            efctBossSingleBuild.model_reg_id = -1;
            efctBossSingleBuild.model_dwork = (AppMain.OBS_DATA_WORK)null;
            efctBossSingleBuild.object_dwork = (AppMain.OBS_DATA_WORK)null;
        }
    }

    public static bool GmEfctBossBuildSingleDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < AppMain.gm_efct_boss_single_reg_num; ++index)
        {
            AppMain.GMS_EFCT_BOSS_SINGLE_BUILD_WORK efctBossSingleBuild = AppMain.gm_efct_boss_single_build_list[index];
            if (efctBossSingleBuild.tex_reg_id != -1)
            {
                if (AppMain.amDrawIsRegistComplete(efctBossSingleBuild.tex_reg_id))
                    efctBossSingleBuild.tex_reg_id = -1;
                else
                    flag = false;
            }
            if (efctBossSingleBuild.model_reg_id != -1)
            {
                if (AppMain.amDrawIsRegistComplete(efctBossSingleBuild.model_reg_id))
                    efctBossSingleBuild.model_reg_id = -1;
                else
                    flag = false;
            }
        }
        return flag;
    }

    public static void GmEfctBossFlushSingleDataInit()
    {
        for (int index = 0; index < AppMain.gm_efct_boss_single_reg_num; ++index)
        {
            AppMain.GMS_EFCT_BOSS_SINGLE_BUILD_WORK efctBossSingleBuild = AppMain.gm_efct_boss_single_build_list[index];
            if (efctBossSingleBuild.object_dwork != null)
            {
                efctBossSingleBuild.model_reg_id = AppMain.ObjAction3dESModelReleaseDwork(efctBossSingleBuild.object_dwork);
                AppMain.ObjDataRelease(efctBossSingleBuild.model_dwork);
                efctBossSingleBuild.model_dwork = (AppMain.OBS_DATA_WORK)null;
            }
            efctBossSingleBuild.tex_reg_id = AppMain.ObjAction3dESTextureReleaseDwork(efctBossSingleBuild.texlist_dwork);
            AppMain.ObjDataRelease(efctBossSingleBuild.ambtex_dwork);
            efctBossSingleBuild.ambtex_dwork = (AppMain.OBS_DATA_WORK)null;
        }
    }

    public static bool GmEfctBossFlushSingleDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < AppMain.gm_efct_boss_single_reg_num; ++index)
        {
            AppMain.GMS_EFCT_BOSS_SINGLE_BUILD_WORK efctBossSingleBuild = AppMain.gm_efct_boss_single_build_list[index];
            if (efctBossSingleBuild.model_reg_id != -1)
            {
                if (AppMain.ObjAction3dESModelReleaseDworkCheck(efctBossSingleBuild.object_dwork, efctBossSingleBuild.model_reg_id))
                {
                    efctBossSingleBuild.model_reg_id = -1;
                    efctBossSingleBuild.object_dwork = (AppMain.OBS_DATA_WORK)null;
                }
                else
                    flag = false;
            }
            if (efctBossSingleBuild.tex_reg_id != -1)
            {
                if (AppMain.ObjAction3dESTextureReleaseDworkCheck(efctBossSingleBuild.texlist_dwork, efctBossSingleBuild.tex_reg_id))
                {
                    efctBossSingleBuild.tex_reg_id = -1;
                    efctBossSingleBuild.texlist_dwork = (AppMain.OBS_DATA_WORK)null;
                }
                else
                    flag = false;
            }
        }
        if (flag)
            AppMain.gm_efct_boss_single_reg_num = 0;
        return flag;
    }

    public static AppMain.GMS_EFFECT_3DES_WORK GmEfctBossCmnEsCreate(
      AppMain.OBS_OBJECT_WORK parent_obj,
      uint efct_bscmn_idx)
    {
        AppMain.GMS_EFCT_BOSS_CMN_CREATE_PARAM bossCmnCreateParam = AppMain.gm_efct_boss_cmn_create_param_tbl[(int)efct_bscmn_idx];
        AppMain.OBS_DATA_WORK model_dwork;
        AppMain.OBS_DATA_WORK object_dwork;
        AppMain.OBS_DATA_WORK ambtex_dwork;
        AppMain.OBS_DATA_WORK texlist_dwork;
        if (bossCmnCreateParam.create_param.model_idx != -1)
        {
            int ameIdx = bossCmnCreateParam.create_param.ame_idx;
            model_dwork = AppMain.ObjDataGet(AppMain.gmEfctBossCmnGetModelDworkNo(ameIdx));
            object_dwork = AppMain.ObjDataGet(AppMain.gmEfctBossCmnGetObjectDworkNo(ameIdx));
            ambtex_dwork = AppMain.ObjDataGet(AppMain.gmEfctBossCmnGetMdlAmbtexDworkNo(ameIdx));
            texlist_dwork = AppMain.ObjDataGet(AppMain.gmEfctBossCmnGetMdlTexlistDworkNo(ameIdx));
        }
        else
        {
            model_dwork = (AppMain.OBS_DATA_WORK)null;
            object_dwork = (AppMain.OBS_DATA_WORK)null;
            ambtex_dwork = AppMain.ObjDataGet(621);
            texlist_dwork = AppMain.ObjDataGet(622);
        }
        return AppMain.GmEffect3dESCreateByParam(bossCmnCreateParam.create_param, parent_obj, AppMain.ObjDataGet(15).pData, AppMain.ObjDataGet(AppMain.gmEfctBossCmnGetAmeDworkNo(bossCmnCreateParam.create_param.ame_idx)), ambtex_dwork, texlist_dwork, model_dwork, object_dwork);
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