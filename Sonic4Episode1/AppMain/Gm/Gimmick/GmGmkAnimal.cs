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
    public static void GmGmkAnimalBuild()
    {
        AppMain.gm_gmk_animal_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(870), AppMain.GmGameDatGetGimmickData(871), 0U);
    }

    public static void GmGmkAnimalFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(870));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_animal_obj_3d_list, amsAmbHeader.file_num);
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkAnimalInit(
      AppMain.OBS_OBJECT_WORK parent_work,
      int ofs_x,
      int ofs_y,
      int ofs_z,
      byte type,
      byte vec,
      ushort timer)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), parent_work, (ushort)0, "GMK_ANIMAL");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (AppMain.GMS_EFFECT_3DNN_WORK)work;
        work.view_out_ofst = (short)64;
        work.pos.x += ofs_x;
        work.pos.y += ofs_y;
        work.pos.z = ofs_z - 131072;
        type = type == (byte)0 ? (byte)((uint)AppMain.mtMathRand() & 1U) : (byte)((int)type - 1 & 1);
        int index = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        work.user_work = AppMain.g_gm_gmk_animal_type_id[index][(int)type];
        work.user_flag = (uint)vec;
        work.user_timer = (int)timer;
        AppMain.gmGmkAnimalObjSet(work, gmsEffect3DnnWork.obj_3d);
        work.move_flag |= 16128U;
        work.move_flag &= 4294967167U;
        work.flag |= 512U;
        work.flag |= 2U;
        work.flag &= 4294967279U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkAnimalWait);
        return work;
    }

    private static void gmGmkAnimalObjSet(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION3D_NN_WORK dest_obj_3d)
    {
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_animal_obj_3d_list[(int)AppMain.g_gm_gmk_animal_obj_id[(int)obj_work.user_work][0]], dest_obj_3d);
        AppMain.ObjObjectFieldRectSet(obj_work, (short)-2, (short)-8, (short)2, (short)0);
        obj_work.disp_flag |= 4259840U;
    }

    private static void gmGmkAnimalWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_animal_obj_3d_list[(int)AppMain.g_gm_gmk_animal_obj_id[(int)obj_work.user_work][1]], obj_work.obj_3d);
            obj_work.move_flag &= 4294952703U;
            obj_work.move_flag |= 1680U;
            obj_work.spd.y = AppMain.g_gm_gmk_animal_speed_param[(int)obj_work.user_work].jump;
            obj_work.spd_fall = AppMain.g_gm_gmk_animal_speed_param[(int)obj_work.user_work].gravity;
            obj_work.pos.z = 131072;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkAnimalJump);
        }
    }

    private static void gmGmkAnimalJump(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.move_flag & 1) == 0)
            return;
        if (obj_work.user_flag != 0U)
        {
            obj_work.spd.x = AppMain.g_gm_gmk_animal_speed_param[(int)obj_work.user_work].spd_x;
            AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_animal_obj_3d_list[(int)AppMain.g_gm_gmk_animal_obj_id[(int)obj_work.user_work][2]], obj_work.obj_3d);
            obj_work.dir.y = (ushort)45056;
        }
        else
        {
            obj_work.spd.x = -AppMain.g_gm_gmk_animal_speed_param[(int)obj_work.user_work].spd_x;
            AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_animal_obj_3d_list[(int)AppMain.g_gm_gmk_animal_obj_id[(int)obj_work.user_work][3]], obj_work.obj_3d);
            obj_work.dir.y = (ushort)45056;
        }
        obj_work.spd.y = AppMain.g_gm_gmk_animal_speed_param[(int)obj_work.user_work].jump;
        obj_work.move_flag |= 16U;
        obj_work.disp_flag &= 4290772735U;
    }

    private static void gmGmkEndingAnimalMove(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.move_flag & 1) == 0)
            return;
        if (((int)((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 24) == 0 || AppMain.GmEndingAnimalForwardChk())
        {
            obj_work.spd.x = 0;
            AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_animal_obj_3d_list[(int)AppMain.g_gm_gmk_animal_obj_id[(int)obj_work.user_work][1]], obj_work.obj_3d);
            obj_work.dir.y = (ushort)45056;
        }
        else
        {
            obj_work.user_flag = (uint)((int)obj_work.user_flag + 1 & 3);
            if (((int)obj_work.user_flag & 2) != 0)
            {
                obj_work.spd.x = AppMain.g_gm_gmk_animal_speed_param[(int)obj_work.user_work].spd_x;
                AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
                AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_animal_obj_3d_list[(int)AppMain.g_gm_gmk_animal_obj_id[(int)obj_work.user_work][2]], obj_work.obj_3d);
                obj_work.dir.y = (ushort)45056;
            }
            else
            {
                obj_work.spd.x = -AppMain.g_gm_gmk_animal_speed_param[(int)obj_work.user_work].spd_x;
                AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
                AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_animal_obj_3d_list[(int)AppMain.g_gm_gmk_animal_obj_id[(int)obj_work.user_work][3]], obj_work.obj_3d);
                obj_work.dir.y = (ushort)45056;
            }
        }
        obj_work.spd.y = AppMain.g_gm_gmk_animal_speed_param[(int)obj_work.user_work].jump;
        obj_work.move_flag |= 16U;
        obj_work.disp_flag &= 4290772735U;
    }

}