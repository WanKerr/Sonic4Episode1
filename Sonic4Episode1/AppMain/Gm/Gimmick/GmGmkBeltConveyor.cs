using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    private static AppMain.OBS_OBJECT_WORK GmGmkBeltConveyorInit(
     AppMain.GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        AppMain.GMS_GMK_BELTC_WORK work = (AppMain.GMS_GMK_BELTC_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BELTC_WORK()), "Gmk_BeltConveyor");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_beltconv_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -69632;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBeltConveyor_ppOut);
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        obj_work.flag |= 2U;
        work.width = (short)((int)eve_rec.width * 2);
        if (eve_rec.left < (sbyte)0)
        {
            work.vect = (ushort)32768;
            work.roller = (int)-work.width * 4096;
        }
        else
        {
            work.vect = (ushort)0;
            work.roller = (int)work.width * 4096;
        }
        work.speed = 8192;
        int num = ((int)eve_rec.flag & 15) >= 15 ? -2048 : ((int)eve_rec.flag & 15) << 11;
        work.speed += num;
        work.rolldir = 0;
        if (work.vect == (ushort)32768)
            work.speed = -work.speed;
        work.diradd = 65536 * work.speed / 6 / 16;
        AppMain.gmGmkBeltConveyorStart(obj_work);
        return obj_work;
    }

    public static void GmGmkBeltConveyorBuild()
    {
        AppMain.gm_gmk_beltconv_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(844), AppMain.GmGameDatGetGimmickData(845), 0U);
        AppMain.gm_gmk_beltconv_obj_tvx_list = AppMain.GmGameDatGetGimmickData(846);
    }

    public static void GmGmkBeltConveyorFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(844);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_beltconv_obj_3d_list, gimmickData.file_num);
        AppMain.gm_gmk_beltconv_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        AppMain.gm_gmk_beltconv_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
    }

    private static void gmGmkBeltConveyor_ppOut(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BELTC_WORK gmsGmkBeltcWork = (AppMain.GMS_GMK_BELTC_WORK)obj_work;
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        if (AppMain._tvx_roller == null)
        {
            AppMain._tvx_roller = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_beltconv_obj_tvx_list, 0));
            AppMain._tvx_axis = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_beltconv_obj_tvx_list, 1));
            AppMain._tvx_belt_up = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_beltconv_obj_tvx_list, 2));
            AppMain._tvx_belt_down = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_beltconv_obj_tvx_list, 3));
        }
        AppMain.TVX_FILE tvxRoller = AppMain._tvx_roller;
        AppMain.TVX_FILE tvxAxis = AppMain._tvx_axis;
        AppMain.TVX_FILE tvxBeltUp = AppMain._tvx_belt_up;
        AppMain.TVX_FILE tvxBeltDown = AppMain._tvx_belt_down;
        AppMain.NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        short rotate_z = (short)-obj_work.dir.z;
        AppMain.VecFx32 pos = obj_work.pos;
        AppMain.GmTvxSetModel(tvxRoller, texlist, ref pos, ref obj_work.scale, AppMain.GMD_TVX_DISP_ROTATE, rotate_z);
        pos.z += 4096;
        AppMain.GmTvxSetModel(tvxAxis, texlist, ref pos, ref obj_work.scale, 0U, (short)0);
        pos.x += gmsGmkBeltcWork.roller;
        AppMain.GmTvxSetModel(tvxAxis, texlist, ref pos, ref obj_work.scale, 0U, (short)0);
        pos.z -= 4096;
        AppMain.GmTvxSetModel(tvxRoller, texlist, ref pos, ref obj_work.scale, AppMain.GMD_TVX_DISP_ROTATE, rotate_z);
        pos.x -= gmsGmkBeltcWork.roller;
        int num = gmsGmkBeltcWork.vect == (ushort)0 ? 262144 : -262144;
        int roller = gmsGmkBeltcWork.roller;
        AppMain.GMS_TVX_EX_WORK ex_work = new AppMain.GMS_TVX_EX_WORK();
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.v = 0.0f;
        ex_work.color = 0U;
        pos.y += -65536;
        pos.z = -73728;
        if (gmsGmkBeltcWork.vect == (ushort)32768)
            pos.x += num;
        for (; roller != 0; roller -= num)
        {
            ex_work.coord.u = gmsGmkBeltcWork.tex_u;
            AppMain.GmTvxSetModelEx(tvxBeltUp, texlist, ref pos, ref obj_work.scale, 0U, (short)0, ref ex_work);
            pos.y -= -131072;
            ex_work.coord.u = -gmsGmkBeltcWork.tex_u;
            AppMain.GmTvxSetModelEx(tvxBeltDown, texlist, ref pos, ref obj_work.scale, 0U, (short)0, ref ex_work);
            pos.y += -131072;
            pos.x += num;
        }
    }

    private static void gmGmkBeltConveyorStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BELTC_WORK gmsGmkBeltcWork = (AppMain.GMS_GMK_BELTC_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK objWork = AppMain.g_gm_main_system.ply_work[0].obj_work;
        bool flag = false;
        if (objWork.ride_obj == obj_work)
        {
            objWork.flow.x = gmsGmkBeltcWork.speed;
            flag = true;
        }
        if (gmsGmkBeltcWork.last_under && !flag && (AppMain.g_gm_main_system.ply_work[0].seq_state == 1 && ((int)objWork.move_flag & 1) == 0) && (gmsGmkBeltcWork.speed > 0 && objWork.spd_m < 0 && objWork.pos.x > obj_work.pos.x || gmsGmkBeltcWork.speed < 0 && objWork.spd_m > 0 && objWork.pos.x < obj_work.pos.x))
            objWork.spd_m = gmsGmkBeltcWork.speed;
        gmsGmkBeltcWork.last_under = flag;
        gmsGmkBeltcWork.rolldir += gmsGmkBeltcWork.diradd;
        obj_work.dir.z = (ushort)(gmsGmkBeltcWork.rolldir >> 12);
        gmsGmkBeltcWork.tex_u -= (float)(gmsGmkBeltcWork.speed >> 12) / 128f;
        while ((double)gmsGmkBeltcWork.tex_u >= 0.125)
            gmsGmkBeltcWork.tex_u -= 0.125f;
        while ((double)gmsGmkBeltcWork.tex_u <= -0.125)
            gmsGmkBeltcWork.tex_u += 0.125f;
    }

    private static void gmGmkBeltConveyorStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BELTC_WORK gmsGmkBeltcWork = (AppMain.GMS_GMK_BELTC_WORK)obj_work;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.diff_data = AppMain.g_gm_default_col;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.flag |= 134217728U;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)gmsGmkBeltcWork.width;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.ofst_x = gmsGmkBeltcWork.vect == (ushort)0 ? (short)0 : (short)-gmsGmkBeltcWork.width;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)8;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.ofst_y = (short)-16;
        if (((int)((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 16) != 0)
        {
            gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.width += (ushort)16;
            gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.ofst_x -= (short)16;
        }
        if (((int)((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 32) != 0)
            gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.width += (ushort)16;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.dir = (ushort)0;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.flag |= 32U;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.attr = (ushort)1;
        gmsGmkBeltcWork.last_under = false;
        gmsGmkBeltcWork.tex_u = 0.0f;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBeltConveyorStay);
    }
}