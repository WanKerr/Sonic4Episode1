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
    private static void GmEfctCmnBuildDataInit()
    {
        AppMain.gm_efct_cmn_proc_state = 1U;
    }

    private static bool GmEfctCmnBuildDataLoopInitPart()
    {
        int num1 = 97 / AppMain.StageCount;
        int num2 = num1 * AppMain.stage;
        int num3 = num2 + num1;
        if (AppMain.stage == AppMain.StageCount - 1)
            num3 = 97;
        for (int ame_idx = num2; ame_idx < num3; ++ame_idx)
        {
            AppMain.GMS_EFCT_CMN_CREATE_PARAM efctCmnCreateParam = AppMain.gm_efct_cmn_create_param_tbl[ame_idx];
            int modelIdx = efctCmnCreateParam.create_param.model_idx;
            int modelDworkNo = AppMain.gmEfctCmnGetModelDworkNo(ame_idx);
            int objectDworkNo = AppMain.gmEfctCmnGetObjectDworkNo(ame_idx);
            int mdlAmbtexIdx = efctCmnCreateParam.mdl_ambtex_idx;
            int mdlAmbtexDworkNo = AppMain.gmEfctCmnGetMdlAmbtexDworkNo(ame_idx);
            int mdlTexlistDworkNo = AppMain.gmEfctCmnGetMdlTexlistDworkNo(ame_idx);
            if (modelIdx != -1)
            {
                AppMain.ambtex_dwork = AppMain.ObjDataGet(mdlAmbtexDworkNo);
                AppMain.ObjDataLoadAmbIndex(AppMain.ambtex_dwork, mdlAmbtexIdx, AppMain.eff_cmn_arc);
                AppMain.gm_efct_cmn_mdl_tex_reg_id_list[AppMain.model_reg_cnt] = AppMain.ObjAction3dESTextureLoadToDwork(AppMain.ObjDataGet(mdlTexlistDworkNo), AppMain.readAMBFile(AppMain.ambtex_dwork.pData), ref AppMain.texlistbuf);
                AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(modelDworkNo), modelIdx, AppMain.eff_cmn_arc);
                AppMain.gm_efct_cmn_model_reg_id_list[AppMain.model_reg_cnt] = AppMain.ObjAction3dESModelLoadToDwork(AppMain.ObjDataGet(objectDworkNo), (object)(AppMain.AmbChunk)AppMain.ObjDataGet(modelDworkNo).pData, 0U);
                ++AppMain.model_reg_cnt;
            }
        }
        if (AppMain.stage == AppMain.StageCount - 1)
        {
            AppMain.eff_cmn_arc = (AppMain.AMS_AMB_HEADER)null;
            AppMain.ambtex_dwork = (AppMain.OBS_DATA_WORK)null;
            AppMain.texlistbuf = (object)null;
            AppMain.model_reg_cnt = 0;
            AppMain.stage = 0;
            return true;
        }
        ++AppMain.stage;
        return false;
    }

    private static void GmEfctCmnBuildDataLoopInit()
    {
        AppMain.eff_cmn_arc = AppMain.readAMBFile(AppMain.ObjDataGetInc(AppMain.ObjDataGet(5)));
        AppMain.gm_efct_cmn_model_reg_num = 97;
        if (AppMain.gm_efct_cmn_model_reg_num > 0)
        {
            AppMain.gm_efct_cmn_model_reg_id_list = new int[AppMain.gm_efct_cmn_model_reg_num];
            AppMain.gm_efct_cmn_mdl_tex_reg_id_list = new int[AppMain.gm_efct_cmn_model_reg_num];
            for (int index = 0; index < AppMain.gm_efct_cmn_model_reg_num; ++index)
                AppMain.gm_efct_cmn_model_reg_id_list[index] = AppMain.gm_efct_cmn_mdl_tex_reg_id_list[index] = -1;
        }
        AppMain.ambtex_dwork = AppMain.ObjDataGet(17);
        AppMain.ObjDataLoadAmbIndex(AppMain.ambtex_dwork, 97, AppMain.eff_cmn_arc);
        AppMain.gm_efct_cmn_tex_reg_id = AppMain.ObjAction3dESTextureLoadToDwork(AppMain.ObjDataGet(18), AppMain.readAMBFile(AppMain.ambtex_dwork.pData), ref AppMain.texlistbuf);
        AppMain.GmEfctCmnBuildDataLoopInitPartWorking = true;
    }

    private static bool GmEfctCmnBuildDataLoop()
    {
        bool flag = true;
        switch (AppMain.gm_efct_cmn_proc_state)
        {
            case 0:
                return true;
            case 1:
                if (AppMain.GsMainSysGetDisplayListRegistNum() < 192)
                {
                    if (!AppMain.GmEfctCmnBuildDataLoopInitPartWorking)
                        AppMain.GmEfctCmnBuildDataLoopInit();
                    else if (AppMain.GmEfctCmnBuildDataLoopInitPart())
                    {
                        AppMain.gm_efct_cmn_proc_state = 2U;
                        AppMain.GmEfctCmnBuildDataLoopInitPartWorking = false;
                    }
                }
                return false;
            default:
                if (AppMain.gm_efct_cmn_tex_reg_id != -1)
                {
                    if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_cmn_tex_reg_id))
                        AppMain.gm_efct_cmn_tex_reg_id = -1;
                    else
                        flag = false;
                }
                for (int index = 0; index < AppMain.gm_efct_cmn_model_reg_num; ++index)
                {
                    if (AppMain.gm_efct_cmn_mdl_tex_reg_id_list[index] != -1)
                    {
                        if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_cmn_mdl_tex_reg_id_list[index]))
                            AppMain.gm_efct_cmn_mdl_tex_reg_id_list[index] = -1;
                        else
                            flag = false;
                    }
                    if (AppMain.gm_efct_cmn_model_reg_id_list[index] != -1)
                    {
                        if (AppMain.amDrawIsRegistComplete(AppMain.gm_efct_cmn_model_reg_id_list[index]))
                            AppMain.gm_efct_cmn_model_reg_id_list[index] = -1;
                        else
                            flag = false;
                    }
                }
                if (flag)
                    AppMain.gm_efct_cmn_proc_state = 0U;
                return flag;
        }
    }

    private static void GmEfctCmnFlushDataInit()
    {
        AppMain.gm_efct_cmn_proc_state = 1U;
    }

    private static void GmEfctCmnFlushDataLoopInit()
    {
        int index = 0;
        for (int ame_idx = 0; ame_idx < 97; ++ame_idx)
        {
            AppMain.GMS_EFCT_CMN_CREATE_PARAM efctCmnCreateParam = AppMain.gm_efct_cmn_create_param_tbl[ame_idx];
            int modelDworkNo = AppMain.gmEfctCmnGetModelDworkNo(ame_idx);
            int objectDworkNo = AppMain.gmEfctCmnGetObjectDworkNo(ame_idx);
            if (efctCmnCreateParam.create_param.model_idx != -1)
            {
                AppMain.gm_efct_cmn_model_reg_id_list[index] = AppMain.ObjAction3dESModelReleaseDwork(AppMain.ObjDataGet(objectDworkNo));
                AppMain.ObjDataRelease(AppMain.ObjDataGet(modelDworkNo));
                AppMain.gm_efct_cmn_mdl_tex_reg_id_list[index] = AppMain.ObjAction3dESTextureReleaseDwork(AppMain.ObjDataGet(AppMain.gmEfctCmnGetMdlTexlistDworkNo(ame_idx)));
                AppMain.ObjDataRelease(AppMain.ObjDataGet(AppMain.gmEfctCmnGetMdlAmbtexDworkNo(ame_idx)));
                ++index;
            }
        }
        AppMain.gm_efct_cmn_tex_reg_id = AppMain.ObjAction3dESTextureReleaseDwork(AppMain.ObjDataGet(18));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(17));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(5));
    }

    private static bool GmEfctCmnFlushDataLoop()
    {
        bool flag = true;
        int index = 0;
        switch (AppMain.gm_efct_cmn_proc_state)
        {
            case 0:
                return true;
            case 1:
                if (AppMain.GsMainSysGetDisplayListRegistNum() < 240)
                {
                    AppMain.GmEfctCmnFlushDataLoopInit();
                    AppMain.gm_efct_cmn_proc_state = 2U;
                }
                return false;
            default:
                if (AppMain.gm_efct_cmn_model_reg_num != 0)
                {
                    for (int ame_idx = 0; ame_idx < 97; ++ame_idx)
                    {
                        if (AppMain.gm_efct_cmn_create_param_tbl[ame_idx].create_param.model_idx != -1)
                        {
                            if (AppMain.gm_efct_cmn_model_reg_id_list[index] != -1)
                            {
                                if (AppMain.ObjAction3dESModelReleaseDworkCheck(AppMain.ObjDataGet(AppMain.gmEfctCmnGetObjectDworkNo(ame_idx)), AppMain.gm_efct_cmn_model_reg_id_list[index]))
                                    AppMain.gm_efct_cmn_model_reg_id_list[index] = -1;
                                else
                                    flag = false;
                            }
                            if (AppMain.gm_efct_cmn_mdl_tex_reg_id_list[index] != -1)
                            {
                                if (AppMain.ObjAction3dESTextureReleaseDworkCheck(AppMain.ObjDataGet(AppMain.gmEfctCmnGetMdlTexlistDworkNo(ame_idx)), AppMain.gm_efct_cmn_mdl_tex_reg_id_list[index]))
                                    AppMain.gm_efct_cmn_mdl_tex_reg_id_list[index] = -1;
                                else
                                    flag = false;
                            }
                            ++index;
                        }
                    }
                }
                if (AppMain.gm_efct_cmn_tex_reg_id != -1)
                {
                    if (AppMain.ObjAction3dESTextureReleaseDworkCheck(AppMain.ObjDataGet(18), AppMain.gm_efct_cmn_tex_reg_id))
                        AppMain.gm_efct_cmn_tex_reg_id = -1;
                    else
                        flag = false;
                }
                if (flag)
                {
                    if (AppMain.gm_efct_cmn_mdl_tex_reg_id_list != null)
                        AppMain.gm_efct_cmn_mdl_tex_reg_id_list = (int[])null;
                    if (AppMain.gm_efct_cmn_model_reg_id_list != null)
                    {
                        AppMain.gm_efct_cmn_model_reg_id_list = (int[])null;
                        AppMain.gm_efct_cmn_model_reg_num = 0;
                    }
                    AppMain.gm_efct_cmn_proc_state = 0U;
                }
                return flag;
        }
    }

    private static AppMain.GMS_EFFECT_3DES_WORK GmEfctCmnEsCreate(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int efct_cmn_idx)
    {
        AppMain.GMS_EFCT_CMN_CREATE_PARAM efctCmnCreateParam = AppMain.gm_efct_cmn_create_param_tbl[efct_cmn_idx];
        AppMain.OBS_DATA_WORK model_dwork;
        AppMain.OBS_DATA_WORK object_dwork;
        AppMain.OBS_DATA_WORK ambtex_dwork;
        AppMain.OBS_DATA_WORK texlist_dwork;
        if (efctCmnCreateParam.create_param.model_idx != -1)
        {
            int ameIdx = efctCmnCreateParam.create_param.ame_idx;
            model_dwork = AppMain.ObjDataGet(AppMain.gmEfctCmnGetModelDworkNo(ameIdx));
            object_dwork = AppMain.ObjDataGet(AppMain.gmEfctCmnGetObjectDworkNo(ameIdx));
            ambtex_dwork = AppMain.ObjDataGet(AppMain.gmEfctCmnGetMdlAmbtexDworkNo(ameIdx));
            texlist_dwork = AppMain.ObjDataGet(AppMain.gmEfctCmnGetMdlTexlistDworkNo(ameIdx));
        }
        else
        {
            model_dwork = (AppMain.OBS_DATA_WORK)null;
            object_dwork = (AppMain.OBS_DATA_WORK)null;
            ambtex_dwork = AppMain.ObjDataGet(17);
            texlist_dwork = AppMain.ObjDataGet(18);
        }
        return AppMain.GmEffect3dESCreateByParam(efctCmnCreateParam.create_param, parent_obj, AppMain.ObjDataGet(5).pData, AppMain.ObjDataGet(AppMain.gmEfctCmnGetAmeDworkNo(efctCmnCreateParam.create_param.ame_idx)), ambtex_dwork, texlist_dwork, model_dwork, object_dwork);
    }

    private static void GmEfctCmnUpdateInvincibleMainPart(AppMain.GMS_EFFECT_3DES_WORK efct_3des)
    {
        efct_3des.efct_com.obj_work.dir.z += (ushort)1820;
    }

    private static void GmEfctCmnUpdateInvincibleSubPart(
      AppMain.GMS_EFFECT_3DES_WORK efct_3des,
      AppMain.OBS_OBJECT_WORK ply_obj)
    {
        AppMain.OBS_OBJECT_WORK objWork = efct_3des.efct_com.obj_work;
        objWork.dir.z += (ushort)1820;
        objWork.spd.x = AppMain.FX_Mul(ply_obj.pos.x - objWork.pos.x, 204);
        objWork.spd.y = AppMain.FX_Mul(ply_obj.pos.y - objWork.pos.y, 204);
        if (objWork.spd.x > 0 && ply_obj.move.x > 0 || objWork.spd.x < 0 && ply_obj.move.x < 0)
            objWork.spd.x += AppMain.FX_Mul(ply_obj.move.x, 1024);
        if ((objWork.spd.y <= 0 || ply_obj.move.y <= 0) && (objWork.spd.y >= 0 || ply_obj.move.y >= 0))
            return;
        objWork.spd.y += AppMain.FX_Mul(ply_obj.move.y, 1024);
    }

    private static int gmEfctCmnGetAmeDworkNo(int ame_idx)
    {
        return 19 + ame_idx;
    }

    private static int gmEfctCmnGetModelDworkNo(int ame_idx)
    {
        return 117 + ame_idx;
    }

    private static int gmEfctCmnGetObjectDworkNo(int ame_idx)
    {
        return 215 + ame_idx;
    }

    private static int gmEfctCmnGetMdlAmbtexDworkNo(int ame_idx)
    {
        return 313 + ame_idx;
    }

    private static int gmEfctCmnGetMdlTexlistDworkNo(int ame_idx)
    {
        return 411 + ame_idx;
    }

    private static void GmComEfctCreateRing(int pos_x, int pos_y)
    {
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 50);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = pos_x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = pos_y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 655360;
    }

    private static void GmComEfctCreateEneDeadSmoke(
      AppMain.OBS_OBJECT_WORK obj_work,
      int ofst_x,
      int ofst_y)
    {
        AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 10);
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GMM_MAIN_GET_ZONE_TYPE() != 2 ? AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 10) : (!AppMain.GmMainIsWaterLevel() || obj_work.pos.y + ofst_y - 196608 >> 12 <= (int)AppMain.g_gm_main_system.water_level ? AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 10) : AppMain.GmEfctZoneEsCreate(obj_work, 2, 0));
        efct_work.efct_com.obj_work.parent_obj = (AppMain.OBS_OBJECT_WORK)null;
        efct_work.efct_com.obj_work.pos.Assign(obj_work.pos);
        AppMain.GmComEfctSetDispOffset(efct_work, ofst_x, ofst_y, 655360);
    }

    private static void GmComEfctCreateHitPlayer(
      AppMain.OBS_OBJECT_WORK obj_work,
      int ofst_x,
      int ofst_y)
    {
    }

    private static void GmComEfctCreateHitEnemy(
      AppMain.OBS_OBJECT_WORK obj_work,
      int ofst_x,
      int ofst_y)
    {
        AppMain.GmComEfctSetDispOffset(AppMain.GmEfctCmnEsCreate(obj_work, 37), ofst_x, ofst_y, 655360);
    }

    private static void GmComEfctCreateSpring(
      AppMain.OBS_OBJECT_WORK obj_work,
      int ofst_x,
      int ofst_y)
    {
        AppMain.GmComEfctCreateSpring(obj_work, ofst_x, ofst_y, 0);
    }

    private static void GmComEfctCreateSpring(
      AppMain.OBS_OBJECT_WORK obj_work,
      int ofst_x,
      int ofst_y,
      int ofst_z)
    {
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)9)
            return;
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(obj_work, 77);
        AppMain.GmComEfctSetDispOffset(efct_work, ofst_x, ofst_y, 65536 + ofst_z);
        efct_work.obj_3des.ecb.drawObjState = 0U;
    }

    private static void GmComEfctSetDispOffset(
      AppMain.GMS_EFFECT_3DES_WORK efct_work,
      int ofst_x,
      int ofst_y,
      int ofst_z)
    {
        if (((int)efct_work.efct_com.obj_work.disp_flag & 4194304) != 0)
            AppMain.GmEffect3DESSetDispOffset(efct_work, AppMain.FXM_FX32_TO_FLOAT(ofst_x), AppMain.FXM_FX32_TO_FLOAT(-ofst_y), AppMain.FXM_FX32_TO_FLOAT(ofst_z));
        else
            AppMain.GmEffect3DESSetDispOffset(efct_work, AppMain.FXM_FX32_TO_FLOAT(-ofst_z), AppMain.FXM_FX32_TO_FLOAT(-ofst_y), AppMain.FXM_FX32_TO_FLOAT(ofst_x));
    }

    private static void GmComEfctSetDispOffsetF(
      AppMain.GMS_EFFECT_3DES_WORK efct_work,
      float ofst_x,
      float ofst_y,
      float ofst_z)
    {
        if (((int)efct_work.efct_com.obj_work.disp_flag & 4194304) != 0)
            AppMain.GmEffect3DESSetDispOffset(efct_work, ofst_x, -ofst_y, ofst_z);
        else
            AppMain.GmEffect3DESSetDispOffset(efct_work, -ofst_z, -ofst_y, ofst_x);
    }

    private static void GmComEfctAddDispOffset(
      AppMain.GMS_EFFECT_3DES_WORK efct_work,
      int ofst_x,
      int ofst_y,
      int ofst_z)
    {
        if (((int)efct_work.efct_com.obj_work.disp_flag & 4194304) != 0)
            AppMain.GmEffect3DESAddDispOffset(efct_work, AppMain.FXM_FX32_TO_FLOAT(ofst_x), AppMain.FXM_FX32_TO_FLOAT(-ofst_y), AppMain.FXM_FX32_TO_FLOAT(ofst_z));
        else
            AppMain.GmEffect3DESAddDispOffset(efct_work, AppMain.FXM_FX32_TO_FLOAT(-ofst_z), AppMain.FXM_FX32_TO_FLOAT(-ofst_y), AppMain.FXM_FX32_TO_FLOAT(ofst_x));
    }

    private static void GmComEfctAddDispOffsetF(
      AppMain.GMS_EFFECT_3DES_WORK efct_work,
      float ofst_x,
      float ofst_y,
      float ofst_z)
    {
        if (((int)efct_work.efct_com.obj_work.disp_flag & 4194304) != 0)
            AppMain.GmEffect3DESAddDispOffset(efct_work, ofst_x, -ofst_y, ofst_z);
        else
            AppMain.GmEffect3DESAddDispOffset(efct_work, -ofst_z, -ofst_y, ofst_x);
    }

    private static void GmComEfctSetDispRotation(
      AppMain.GMS_EFFECT_3DES_WORK efct_work,
      ushort dir_x,
      ushort dir_y,
      ushort dir_z)
    {
        if (((int)efct_work.efct_com.obj_work.disp_flag & 4194304) != 0)
            AppMain.GmEffect3DESSetDispRotation(efct_work, (short)dir_x, (short)dir_y, (short)dir_z);
        else
            AppMain.GmEffect3DESSetDispRotation(efct_work, (short)-dir_z, (short)-dir_y, (short)dir_x);
    }

    private static void GmComEfctSetDispRotationS(
      AppMain.GMS_EFFECT_3DES_WORK efct_work,
      short dir_x,
      short dir_y,
      short dir_z)
    {
        if (((int)efct_work.efct_com.obj_work.disp_flag & 4194304) != 0)
            AppMain.GmEffect3DESSetDispRotation(efct_work, dir_x, dir_y, dir_z);
        else
            AppMain.GmEffect3DESSetDispRotation(efct_work, (short)-dir_z, (short)-dir_y, dir_x);
    }

    private static void GmComEfctAddDispRotation(
      AppMain.GMS_EFFECT_3DES_WORK efct_work,
      ushort dir_x,
      ushort dir_y,
      ushort dir_z)
    {
        if (((int)efct_work.efct_com.obj_work.disp_flag & 4194304) != 0)
            AppMain.GmEffect3DESAddDispRotation(efct_work, (short)dir_x, (short)dir_y, (short)dir_z);
        else
            AppMain.GmEffect3DESAddDispRotation(efct_work, (short)-dir_z, (short)-dir_y, (short)dir_x);
    }

    private static void GmComEfctAddDispRotationS(
      AppMain.GMS_EFFECT_3DES_WORK efct_work,
      short dir_x,
      short dir_y,
      short dir_z)
    {
        if (((int)efct_work.efct_com.obj_work.disp_flag & 4194304) != 0)
            AppMain.GmEffect3DESAddDispRotation(efct_work, dir_x, dir_y, dir_z);
        else
            AppMain.GmEffect3DESAddDispRotation(efct_work, (short)-dir_z, (short)-dir_y, dir_x);
    }
}