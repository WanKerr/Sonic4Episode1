public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkBeltConveyorInit(
     GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        GMS_GMK_BELTC_WORK work = (GMS_GMK_BELTC_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_BELTC_WORK(), "Gmk_BeltConveyor");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_beltconv_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -69632;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBeltConveyor_ppOut);
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        obj_work.flag |= 2U;
        work.width = (short)(eve_rec.width * 2);
        if (eve_rec.left < 0)
        {
            work.vect = 32768;
            work.roller = -work.width * 4096;
        }
        else
        {
            work.vect = 0;
            work.roller = work.width * 4096;
        }
        work.speed = 8192;
        int num = (eve_rec.flag & 15) >= 15 ? -2048 : (eve_rec.flag & 15) << 11;
        work.speed += num;
        work.rolldir = 0;
        if (work.vect == 32768)
            work.speed = -work.speed;
        work.diradd = 65536 * work.speed / 6 / 16;
        gmGmkBeltConveyorStart(obj_work);
        return obj_work;
    }

    public static void GmGmkBeltConveyorBuild()
    {
        gm_gmk_beltconv_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(844), GmGameDatGetGimmickData(845), 0U);
        gm_gmk_beltconv_obj_tvx_list = GmGameDatGetGimmickData(846);
    }

    public static void GmGmkBeltConveyorFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(844);
        GmGameDBuildRegFlushModel(gm_gmk_beltconv_obj_3d_list, gimmickData.file_num);
        gm_gmk_beltconv_obj_3d_list = null;
        gm_gmk_beltconv_obj_tvx_list = null;
    }

    private static void gmGmkBeltConveyor_ppOut(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BELTC_WORK gmsGmkBeltcWork = (GMS_GMK_BELTC_WORK)obj_work;
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        if (_tvx_roller == null)
        {
            _tvx_roller = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_beltconv_obj_tvx_list, 0));
            _tvx_axis = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_beltconv_obj_tvx_list, 1));
            _tvx_belt_up = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_beltconv_obj_tvx_list, 2));
            _tvx_belt_down = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_beltconv_obj_tvx_list, 3));
        }
        TVX_FILE tvxRoller = _tvx_roller;
        TVX_FILE tvxAxis = _tvx_axis;
        TVX_FILE tvxBeltUp = _tvx_belt_up;
        TVX_FILE tvxBeltDown = _tvx_belt_down;
        NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        short rotate_z = (short)-obj_work.dir.z;
        VecFx32 pos = obj_work.pos;
        GmTvxSetModel(tvxRoller, texlist, ref pos, ref obj_work.scale, GMD_TVX_DISP_ROTATE, rotate_z);
        pos.z += 4096;
        GmTvxSetModel(tvxAxis, texlist, ref pos, ref obj_work.scale, 0U, 0);
        pos.x += gmsGmkBeltcWork.roller;
        GmTvxSetModel(tvxAxis, texlist, ref pos, ref obj_work.scale, 0U, 0);
        pos.z -= 4096;
        GmTvxSetModel(tvxRoller, texlist, ref pos, ref obj_work.scale, GMD_TVX_DISP_ROTATE, rotate_z);
        pos.x -= gmsGmkBeltcWork.roller;
        int num = gmsGmkBeltcWork.vect == 0 ? 262144 : -262144;
        int roller = gmsGmkBeltcWork.roller;
        GMS_TVX_EX_WORK ex_work = new GMS_TVX_EX_WORK();
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.v = 0.0f;
        ex_work.color = 0U;
        pos.y += -65536;
        pos.z = -73728;
        if (gmsGmkBeltcWork.vect == 32768)
            pos.x += num;
        for (; roller != 0; roller -= num)
        {
            ex_work.coord.u = gmsGmkBeltcWork.tex_u;
            GmTvxSetModelEx(tvxBeltUp, texlist, ref pos, ref obj_work.scale, 0U, 0, ref ex_work);
            pos.y -= -131072;
            ex_work.coord.u = -gmsGmkBeltcWork.tex_u;
            GmTvxSetModelEx(tvxBeltDown, texlist, ref pos, ref obj_work.scale, 0U, 0, ref ex_work);
            pos.y += -131072;
            pos.x += num;
        }
    }

    private static void gmGmkBeltConveyorStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BELTC_WORK gmsGmkBeltcWork = (GMS_GMK_BELTC_WORK)obj_work;
        OBS_OBJECT_WORK objWork = g_gm_main_system.ply_work[0].obj_work;
        bool flag = false;
        if (objWork.ride_obj == obj_work)
        {
            objWork.flow.x = gmsGmkBeltcWork.speed;
            flag = true;
        }
        if (gmsGmkBeltcWork.last_under && !flag && (g_gm_main_system.ply_work[0].seq_state == 1 && ((int)objWork.move_flag & 1) == 0) && (gmsGmkBeltcWork.speed > 0 && objWork.spd_m < 0 && objWork.pos.x > obj_work.pos.x || gmsGmkBeltcWork.speed < 0 && objWork.spd_m > 0 && objWork.pos.x < obj_work.pos.x))
            objWork.spd_m = gmsGmkBeltcWork.speed;
        gmsGmkBeltcWork.last_under = flag;
        gmsGmkBeltcWork.rolldir += gmsGmkBeltcWork.diradd;
        obj_work.dir.z = (ushort)(gmsGmkBeltcWork.rolldir >> 12);
        gmsGmkBeltcWork.tex_u -= (gmsGmkBeltcWork.speed >> 12) / 128f;
        while (gmsGmkBeltcWork.tex_u >= 0.125)
            gmsGmkBeltcWork.tex_u -= 0.125f;
        while (gmsGmkBeltcWork.tex_u <= -0.125)
            gmsGmkBeltcWork.tex_u += 0.125f;
    }

    private static void gmGmkBeltConveyorStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BELTC_WORK gmsGmkBeltcWork = (GMS_GMK_BELTC_WORK)obj_work;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.diff_data = g_gm_default_col;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.flag |= 134217728U;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)gmsGmkBeltcWork.width;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.ofst_x = gmsGmkBeltcWork.vect == 0 ? (short)0 : (short)-gmsGmkBeltcWork.width;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.height = 8;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.ofst_y = -16;
        if ((((GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 16) != 0)
        {
            gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.width += 16;
            gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.ofst_x -= 16;
        }
        if ((((GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 32) != 0)
            gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.width += 16;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.dir = 0;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.flag |= 32U;
        gmsGmkBeltcWork.gmk_work.ene_com.col_work.obj_col.attr = 1;
        gmsGmkBeltcWork.last_under = false;
        gmsGmkBeltcWork.tex_u = 0.0f;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBeltConveyorStay);
    }
}