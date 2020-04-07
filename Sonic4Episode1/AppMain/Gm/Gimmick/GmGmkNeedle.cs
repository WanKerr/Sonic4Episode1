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
    private static AppMain.OBS_OBJECT_WORK GmGmkNeedleInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_NEEDLE_WORK()), "GMK_NEEDLE_MAIN");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        gmsGmkNeedleWork.needle_type = AppMain.GmGmkNeedleGetType(eve_rec.id);
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_needle_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleDrawFunc);
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)AppMain.gm_gmk_col_rect_tbl[(int)gmsGmkNeedleWork.needle_type][0];
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)AppMain.gm_gmk_col_rect_tbl[(int)gmsGmkNeedleWork.needle_type][1];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)AppMain.gm_gmk_col_rect_tbl[(int)gmsGmkNeedleWork.needle_type][2];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)AppMain.gm_gmk_col_rect_tbl[(int)gmsGmkNeedleWork.needle_type][3];
        gmsEnemy3DWork.ene_com.col_work.obj_col.dir = (ushort)(16384U * (uint)gmsGmkNeedleWork.needle_type);
        work.pos.z = -4096;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkZSet(pRec, (short)AppMain.gm_gmk_atk_rect_tbl[(int)gmsGmkNeedleWork.needle_type][0], (short)AppMain.gm_gmk_atk_rect_tbl[(int)gmsGmkNeedleWork.needle_type][1], (short)-500, (short)AppMain.gm_gmk_atk_rect_tbl[(int)gmsGmkNeedleWork.needle_type][2], (short)AppMain.gm_gmk_atk_rect_tbl[(int)gmsGmkNeedleWork.needle_type][3], (short)500);
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)9)
        {
            if (gmsGmkNeedleWork.needle_type == (ushort)1)
                pRec.rect.left -= (short)16;
            else if (gmsGmkNeedleWork.needle_type == (ushort)3)
                pRec.rect.right += (short)16;
        }
        pRec.flag |= 4U;
        pRec.flag |= 1024U;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
        {
            AppMain.ObjDrawObjectActionSet(work, 0);
            work.obj_3d.use_light_flag &= 4294967294U;
            work.obj_3d.use_light_flag |= 4U;
        }
        work.move_flag |= 8449U;
        work.disp_flag |= 4194304U;
        gmsGmkNeedleWork.state = 0U;
        AppMain.gmGmkNeedleFwInit(work);
        work.flag |= 1073741824U;
        gmsGmkNeedleWork.color = uint.MaxValue;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
            gmsGmkNeedleWork.color = 4288717055U;
        else if (AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
            gmsGmkNeedleWork.color = 2694881535U;
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkActNeedleInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_NEEDLE_WORK()), "GMK_NEEDLE_ACT_MAIN");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        gmsGmkNeedleWork.needle_type = AppMain.GmGmkNeedleGetType(eve_rec.id);
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_needle_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleDrawFunc);
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)AppMain.gm_gmk_col_rect_tbl[(int)gmsGmkNeedleWork.needle_type][0];
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)AppMain.gm_gmk_col_rect_tbl[(int)gmsGmkNeedleWork.needle_type][1];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)AppMain.gm_gmk_col_rect_tbl[(int)gmsGmkNeedleWork.needle_type][2];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)AppMain.gm_gmk_col_rect_tbl[(int)gmsGmkNeedleWork.needle_type][3];
        gmsEnemy3DWork.ene_com.col_work.obj_col.dir = (ushort)(32768 * ((int)gmsGmkNeedleWork.needle_type - 4));
        work.pos.z = -4096;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkZSet(pRec, (short)AppMain.gm_gmk_atk_rect_tbl[(int)gmsGmkNeedleWork.needle_type][0], (short)AppMain.gm_gmk_atk_rect_tbl[(int)gmsGmkNeedleWork.needle_type][1], (short)-500, (short)AppMain.gm_gmk_atk_rect_tbl[(int)gmsGmkNeedleWork.needle_type][2], (short)AppMain.gm_gmk_atk_rect_tbl[(int)gmsGmkNeedleWork.needle_type][3], (short)500);
        pRec.flag |= 4U;
        pRec.flag |= 1024U;
        work.move_flag |= 8449U;
        work.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        work.scale.y = 4096;
        AppMain.amFlagOn(ref work.flag, 2U);
        AppMain.amFlagOff(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
        gmsGmkNeedleWork.state = 0U;
        gmsGmkNeedleWork.is_first_disp = (ushort)1;
        gmsGmkNeedleWork.timer = -30;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkActNeedleFwMain);
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
        {
            AppMain.ObjDrawObjectActionSet(work, 0);
            work.obj_3d.use_light_flag &= 4294967294U;
            work.obj_3d.use_light_flag |= 4U;
        }
        gmsGmkNeedleWork.color = uint.MaxValue;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
            gmsGmkNeedleWork.color = 4288717055U;
        else if (AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
            gmsGmkNeedleWork.color = 2694881535U;
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBackNeedleInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return (AppMain.OBS_OBJECT_WORK)null;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkStandNeedleInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return (AppMain.OBS_OBJECT_WORK)null;
    }

    public static void GmGmkNeedleBuild()
    {
        AppMain.gm_gmk_needle_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(822), AppMain.GmGameDatGetGimmickData(823), 0U);
        AppMain.gm_gmk_needle_obj_tvx_list = AppMain.GmGameDatGetGimmickData(824);
        AppMain.tvx_needle = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_needle_obj_tvx_list, 0));
        AppMain.tvx_stand = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_needle_obj_tvx_list, 1));
    }

    public static void GmGmkNeedleFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(822));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_needle_obj_3d_list, amsAmbHeader.file_num);
        AppMain.gm_gmk_needle_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        AppMain.gm_gmk_needle_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
    }

    private static void gmGmkNeedleFwInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)(AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleFwMain);
    }

    private static void gmGmkNeedleFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        AppMain.OBS_RECT_WORK obsRectWork = gmsEnemy3DWork.ene_com.rect_work[1];
        if (gmsGmkNeedleWork.needle_type != (ushort)0)
            return;
        AppMain.OBS_OBJECT_WORK riderObj = gmsEnemy3DWork.ene_com.col_work.obj_col.rider_obj;
        if (riderObj != null && riderObj.ride_obj == (AppMain.OBS_OBJECT_WORK)gmsEnemy3DWork)
        {
            if (riderObj.obj_type != (ushort)1)
                return;
            obsRectWork.flag |= 4U;
        }
        else
            obsRectWork.flag &= 4294967291U;
    }

    private static void gmGmkActNeedleFwInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (((AppMain.GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork).state == 0U)
            AppMain.amFlagOff(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
        else
            AppMain.amFlagOn(ref obj_work.flag, 2U);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkActNeedleFwMain);
    }

    private static void gmGmkActNeedleFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        AppMain.OBS_RECT_WORK obsRectWork = gmsEnemy3DWork.ene_com.rect_work[1];
        if (gmsGmkNeedleWork.needle_type == (ushort)4)
        {
            AppMain.OBS_OBJECT_WORK riderObj = gmsEnemy3DWork.ene_com.col_work.obj_col.rider_obj;
            if (riderObj != null && riderObj.ride_obj == (AppMain.OBS_OBJECT_WORK)gmsEnemy3DWork)
            {
                if (riderObj.obj_type == (ushort)1)
                    AppMain.amFlagOff(ref obj_work.flag, 2U);
            }
            else
                AppMain.amFlagOn(ref obj_work.flag, 2U);
        }
        if (gmsGmkNeedleWork.timer >= 60)
        {
            gmsGmkNeedleWork.timer = 0;
            AppMain.gmGmkActNeedleScalingInit(obj_work);
        }
        else
            ++gmsGmkNeedleWork.timer;
        if (((int)gmsGmkNeedleWork.scale_flag & 1) == 0)
            return;
        AppMain.gmGmkActNeedleSetScaleColRect(obj_work);
    }

    private static void gmGmkActNeedleScalingInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        if (gmsGmkNeedleWork.state == 1U)
        {
            AppMain.amFlagOn(ref obj_work.flag, 2U);
            AppMain.amFlagOn(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
            gmsGmkNeedleWork.scale_flag |= 1U;
            gmsGmkNeedleWork.scale_flag |= 4U;
        }
        else if (gmsGmkNeedleWork.is_first_disp == (ushort)0)
        {
            obj_work.scale.y = 256;
            gmsGmkNeedleWork.scale_flag |= 1U;
            gmsGmkNeedleWork.scale_flag |= 2U;
        }
        else
        {
            gmsGmkNeedleWork.scale_flag |= 1U;
            gmsGmkNeedleWork.scale_flag |= 4U;
        }
        if (gmsGmkNeedleWork.is_first_disp != (ushort)0)
        {
            AppMain.amFlagOn(ref obj_work.flag, 2U);
            AppMain.amFlagOn(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
            gmsGmkNeedleWork.state = 1U;
            gmsGmkNeedleWork.is_first_disp = (ushort)0;
        }
        AppMain.amFlagOff(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
        if (gmsGmkNeedleWork.needle_type == (ushort)4)
        {
            AppMain.OBS_OBJECT_WORK riderObj = gmsEnemy3DWork.ene_com.col_work.obj_col.rider_obj;
            if (riderObj != null && riderObj.ride_obj == (AppMain.OBS_OBJECT_WORK)gmsEnemy3DWork)
            {
                if (riderObj.obj_type == (ushort)1)
                    AppMain.amFlagOff(ref obj_work.flag, 2U);
            }
            else
                AppMain.amFlagOn(ref obj_work.flag, 2U);
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkActNeedleScalingMain);
    }

    private static void gmGmkActNeedleScalingMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        if (gmsGmkNeedleWork.timer >= 6)
        {
            gmsGmkNeedleWork.timer = 0;
            AppMain.gmGmkActNeedleRectWaitInit(obj_work);
        }
        else
        {
            AppMain.amFlagOn(ref obj_work.flag, 2U);
            if (gmsGmkNeedleWork.state == 0U)
                obj_work.scale.y += 682;
            else
                obj_work.scale.y -= 682;
            if (obj_work.scale.y > 4096)
                obj_work.scale.y = 4096;
            else if (obj_work.scale.y <= 0)
                obj_work.scale.y = 0;
            AppMain.amFlagOff(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
            obj_work.scale.y = AppMain.MTM_MATH_CLIP(obj_work.scale.y, 0, 4096);
            if (((int)gmsGmkNeedleWork.scale_flag & 1) != 0)
                AppMain.gmGmkActNeedleSetScaleColRect(obj_work);
            if (gmsGmkNeedleWork.needle_type == (ushort)4)
            {
                AppMain.OBS_OBJECT_WORK riderObj = gmsEnemy3DWork.ene_com.col_work.obj_col.rider_obj;
                if (riderObj != null && riderObj.ride_obj == (AppMain.OBS_OBJECT_WORK)gmsEnemy3DWork)
                {
                    if (riderObj.obj_type == (ushort)1)
                        AppMain.amFlagOff(ref obj_work.flag, 2U);
                }
                else
                    AppMain.amFlagOn(ref obj_work.flag, 2U);
            }
            ++gmsGmkNeedleWork.timer;
        }
    }

    private static void gmGmkActNeedleRectWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)(AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkActNeedleRectWaitMain);
    }

    private static void gmGmkActNeedleRectWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        AppMain.OBS_RECT_WORK obsRectWork = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.amFlagOff(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
        AppMain.gmGmkActNeedleFwInit(obj_work);
        gmsGmkNeedleWork.state ^= 1U;
        if (gmsGmkNeedleWork.state == 1U)
            AppMain.GmSoundPlaySE("Spine");
        if (((int)gmsGmkNeedleWork.scale_flag & 1) == 0)
            return;
        AppMain.gmGmkActNeedleSetScaleColRect(obj_work);
    }

    private static ushort GmGmkNeedleGetType(ushort type)
    {
        ushort num1;
        if (type < (ushort)97)
        {
            ushort num2 = (ushort)((uint)type - 91U);
            num1 = (ushort)AppMain.gm_gmk_ndl_type_tbl[(int)num2];
        }
        else
        {
            ushort num2 = (ushort)(4U + (uint)(ushort)((uint)type - 97U));
            num1 = (ushort)AppMain.gm_gmk_ndl_type_tbl[(int)num2];
        }
        return num1;
    }

    private static void gmGmkActNeedleSetScaleColRect(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        ++gmsGmkNeedleWork.scale_timer;
        if (((int)gmsGmkNeedleWork.scale_flag & 4) != 0)
        {
            int num = (int)gmsEnemy3DWork.ene_com.col_work.obj_col.height - 3;
            if (num < 0)
            {
                num = 0;
                gmsGmkNeedleWork.scale_timer = 0;
                gmsGmkNeedleWork.scale_flag &= 4294967294U;
                gmsGmkNeedleWork.scale_flag &= 4294967291U;
            }
            gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)num;
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)(-1 * (int)gmsEnemy3DWork.ene_com.col_work.obj_col.height);
        }
        else
        {
            if (((int)gmsGmkNeedleWork.scale_flag & 2) == 0)
                return;
            int num = (int)gmsEnemy3DWork.ene_com.col_work.obj_col.height + 4;
            if (num > 32)
            {
                num = 32;
                gmsGmkNeedleWork.scale_timer = 0;
                gmsGmkNeedleWork.scale_flag &= 4294967294U;
                gmsGmkNeedleWork.scale_flag &= 4294967293U;
            }
            gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)num;
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)(-1 * (int)gmsEnemy3DWork.ene_com.col_work.obj_col.height);
        }
    }

    private static void gmGmkNeedleDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (AppMain.GMS_GMK_NEEDLE_WORK)(AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.VecFx32 pos = new AppMain.VecFx32();
        AppMain.VecFx32 scale = new AppMain.VecFx32(4096, 4096, 4096);
        AppMain.NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        AppMain.GMS_TVX_EX_WORK ex_work = new AppMain.GMS_TVX_EX_WORK();
        short rotate_z = (short)(-49152 * (int)gmsGmkNeedleWork.needle_type);
        if (gmsGmkNeedleWork.needle_type == (ushort)5)
            rotate_z = short.MinValue;
        obj_work.dir.z = (ushort)-rotate_z;
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.u = 0.0f;
        ex_work.coord.v = 0.0f;
        ex_work.color = gmsGmkNeedleWork.color;
        for (int index = 0; index < 5; ++index)
        {
            pos.z = obj_work.pos.z;
            if (index >= 3)
                pos.z -= 8192;
            switch (gmsGmkNeedleWork.needle_type)
            {
                case 0:
                case 4:
                    pos.x = obj_work.pos.x + AppMain.gm_gmk_disp_ofst_tbl_u[index][0];
                    pos.y = obj_work.pos.y + AppMain.gm_gmk_disp_ofst_tbl_u[index][1];
                    break;
                case 1:
                case 5:
                    pos.x = obj_work.pos.x + AppMain.gm_gmk_disp_ofst_tbl_l[index][0];
                    pos.y = obj_work.pos.y + AppMain.gm_gmk_disp_ofst_tbl_l[index][1];
                    break;
                case 2:
                    pos.x = obj_work.pos.x + AppMain.gm_gmk_disp_ofst_tbl_d[index][0];
                    pos.y = obj_work.pos.y + AppMain.gm_gmk_disp_ofst_tbl_d[index][1];
                    break;
                case 3:
                    pos.x = obj_work.pos.x + AppMain.gm_gmk_disp_ofst_tbl_r[index][0];
                    pos.y = obj_work.pos.y + AppMain.gm_gmk_disp_ofst_tbl_r[index][1];
                    break;
            }
            AppMain.GmTvxSetModelEx(AppMain.tvx_needle, texlist, ref pos, ref obj_work.scale, AppMain.GMD_TVX_DISP_ROTATE | AppMain.GMD_TVX_DISP_SCALE | AppMain.GMD_TVX_DISP_LIGHT_DISABLE, rotate_z, ref ex_work);
            AppMain.GmTvxSetModel(AppMain.tvx_stand, texlist, ref pos, ref scale, AppMain.GMD_TVX_DISP_ROTATE | AppMain.GMD_TVX_DISP_SCALE, rotate_z);
        }
    }

    public static void GmGmkNeedleSetLight()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
        {
            nnsVector.x = -0.1f;
            nnsVector.y = -0.09f;
            nnsVector.z = -0.93f;
        }
        else
        {
            nnsVector.x = -1f;
            nnsVector.y = -1f;
            nnsVector.z = -1f;
        }
        col.r = 1f;
        col.g = 1f;
        col.b = 1f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        float intensity = AppMain.g_gs_main_sys_info.stage_id != (ushort)14 ? 1f : 0.8f;
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_2, ref col, intensity, nnsVector);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

}