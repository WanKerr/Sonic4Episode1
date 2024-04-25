public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkPistonUpInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obsObjectWork = gmGmkPistonInit(eve_rec, pos_x, pos_y, type);
        GMS_GMK_PISTON_WORK gmsGmkPistonWork = (GMS_GMK_PISTON_WORK)obsObjectWork;
        gmsGmkPistonWork.obj_type = 0U;
        gmsGmkPistonWork.piston_vect = 32768;
        obsObjectWork.dir.z = 0;
        gmsGmkPistonWork.gmk_work.ene_com.col_work.obj_col.flag |= 32U;
        if (eve_rec.top < 0)
            gmsGmkPistonWork.timer_set_move = -eve_rec.top * 2 << 12;
        else if (eve_rec.top > 0)
            gmsGmkPistonWork.timer_set_move = eve_rec.top * 2 << 12;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonStart);
        return obsObjectWork;
    }

    private static OBS_OBJECT_WORK GmGmkPistonDownInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obsObjectWork = gmGmkPistonInit(eve_rec, pos_x, pos_y, type);
        GMS_GMK_PISTON_WORK gmsGmkPistonWork = (GMS_GMK_PISTON_WORK)obsObjectWork;
        gmsGmkPistonWork.obj_type = 1U;
        gmsGmkPistonWork.piston_vect = 0;
        obsObjectWork.dir.z = 32768;
        gmsGmkPistonWork.gmk_work.ene_com.col_work.obj_col.flag |= 32U;
        if (eve_rec.top > 0)
            gmsGmkPistonWork.timer_set_move = eve_rec.top * 2 << 12;
        else if (eve_rec.top < 0)
            gmsGmkPistonWork.timer_set_move = -eve_rec.top * 2 << 12;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonStart);
        return obsObjectWork;
    }

    public static void GmGmkPistonBuild()
    {
        gm_gmk_piston_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(842), GmGameDatGetGimmickData(843), 0U);
    }

    public static void GmGmkPistonFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(842);
        GmGameDBuildRegFlushModel(gm_gmk_piston_obj_3d_list, gimmickData.file_num);
    }

    private static uint gmGmkPistonSyncTimeGet(GMS_GMK_PISTON_WORK pwork)
    {
        return g_gm_main_system.sync_time % ((uint)((pwork.timer_set_move + (pwork.stroke_spd - 1)) / pwork.stroke_spd) * 2U + (uint)pwork.timer_set_wait_upper + (uint)pwork.timer_set_wait_lower);
    }

    private static void gmGmkPistonStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK pwork = (GMS_GMK_PISTON_WORK)obj_work;
        pwork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        pwork.gmk_work.ene_com.col_work.obj_col.width = (ushort)GmkPistonData.tbl_gm_gmk_piston_col_rect[(int)pwork.obj_type][0];
        pwork.gmk_work.ene_com.col_work.obj_col.height = (ushort)GmkPistonData.tbl_gm_gmk_piston_col_rect[(int)pwork.obj_type][1];
        pwork.gmk_work.ene_com.col_work.obj_col.ofst_x = GmkPistonData.tbl_gm_gmk_piston_col_rect[(int)pwork.obj_type][2];
        pwork.gmk_work.ene_com.col_work.obj_col.ofst_y = GmkPistonData.tbl_gm_gmk_piston_col_rect[(int)pwork.obj_type][3];
        pwork.gmk_work.ene_com.col_work.obj_col.dir = 0;
        pwork.gmk_work.ene_com.col_work.obj_col.diff_data = g_gm_default_col;
        pwork.gmk_work.ene_com.col_work.obj_col.flag |= 134217760U;
        pwork.gmk_work.ene_com.rect_work[2].flag &= 4294967291U;
        gmGmkPistonRod_Create(obj_work);
        uint num1 = (uint)((pwork.timer_set_move + (pwork.stroke_spd - 1)) / pwork.stroke_spd);
        uint num2 = gmGmkPistonSyncTimeGet(pwork);
        if (num2 <= (uint)pwork.timer_set_wait_lower)
        {
            pwork.timer_dec = (int)(pwork.timer_set_wait_lower - (num2 - 1U));
            gmGmkPistonStay(obj_work);
        }
        else
        {
            uint num3 = num2 - (uint)pwork.timer_set_wait_lower;
            int num4 = pwork.piston_vect == 0 ? pwork.stroke_spd : -pwork.stroke_spd;
            if (num3 < num1)
            {
                pwork.timer_dec = pwork.timer_set_move;
                for (; num3 > 1U; --num3)
                {
                    pwork.timer_dec -= pwork.stroke_spd;
                    obj_work.pos.y += num4;
                }
                gmGmkPistonStroke(obj_work);
            }
            else
            {
                uint num5 = num3 - num1;
                obj_work.pos.y += pwork.piston_vect == 0 ? pwork.timer_set_move : -pwork.timer_set_move;
                if (num5 <= (uint)pwork.timer_set_wait_upper)
                {
                    pwork.timer_dec = (int)(pwork.timer_set_wait_upper - (num5 - 1U));
                    gmGmkPistonTopDeadWait(obj_work);
                }
                else
                {
                    uint num6 = num5 - (uint)pwork.timer_set_wait_upper;
                    pwork.timer_dec = pwork.timer_set_move;
                    for (; num6 > 1U; --num6)
                    {
                        pwork.timer_dec -= pwork.stroke_spd;
                        obj_work.pos.y -= num4;
                    }
                    gmGmkPistonShrink(obj_work);
                }
            }
        }
    }

    private static void gmGmkPistonStay(OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.y = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonStay_100);
        gmGmkPistonStay_100(obj_work);
    }

    private static void gmGmkPistonStay_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK gmsGmkPistonWork = (GMS_GMK_PISTON_WORK)obj_work;
        --gmsGmkPistonWork.timer_dec;
        if (gmsGmkPistonWork.timer_dec > 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonStay_200);
    }

    private static void gmGmkPistonStay_200(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK gmsGmkPistonWork = (GMS_GMK_PISTON_WORK)obj_work;
        gmsGmkPistonWork.timer_dec = gmsGmkPistonWork.timer_set_move;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonStroke);
        gmGmkPistonStroke(obj_work);
    }

    private static void gmGmkPistonStroke(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK gmsGmkPistonWork = (GMS_GMK_PISTON_WORK)obj_work;
        obj_work.spd.y = gmsGmkPistonWork.piston_vect == 0 ? gmsGmkPistonWork.stroke_spd : -gmsGmkPistonWork.stroke_spd;
        obj_work.pos.y += obj_work.spd.y;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonStroke_100);
        gmGmkPistonStroke_100(obj_work);
        GmSoundPlaySE("Piston1");
    }

    private static void gmGmkPistonStroke_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK gmsGmkPistonWork = (GMS_GMK_PISTON_WORK)obj_work;
        gmsGmkPistonWork.timer_dec -= gmsGmkPistonWork.stroke_spd;
        if (gmsGmkPistonWork.timer_dec > 0)
            return;
        obj_work.spd.y = 0;
        if (gmsGmkPistonWork.timer_dec < 0)
        {
            obj_work.spd.y = gmsGmkPistonWork.timer_dec;
            if (gmsGmkPistonWork.piston_vect == 32768)
                obj_work.spd.y = -obj_work.spd.y;
        }
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonStroke_200);
    }

    private static void gmGmkPistonStroke_200(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK pwork = (GMS_GMK_PISTON_WORK)obj_work;
        uint num1 = (uint)((pwork.timer_set_move + (pwork.stroke_spd - 1)) / pwork.stroke_spd);
        uint num2 = gmGmkPistonSyncTimeGet(pwork) - (uint)pwork.timer_set_wait_lower - num1;
        pwork.timer_dec = num2 > (uint)pwork.timer_set_wait_upper ? 0 : (int)(pwork.timer_set_wait_upper - (num2 - 1U));
        gmGmkPistonTopDeadWait(obj_work);
    }

    private static void gmGmkPistonTopDeadWait(OBS_OBJECT_WORK obj_work)
    {
        if (!((GMS_GMK_PISTON_WORK)obj_work).efct_di)
        {
            OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(null, 48);
            obsObjectWork.pos.x = obj_work.pos.x;
            obsObjectWork.pos.y = obj_work.pos.y;
            obsObjectWork.pos.z = obj_work.pos.z + 65536;
            obsObjectWork.dir.z = obj_work.dir.z;
            GmSoundPlaySE("Piston2");
        }
        obj_work.spd.y = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonTopDeadWait_100);
        gmGmkPistonTopDeadWait_100(obj_work);
    }

    private static void gmGmkPistonTopDeadWait_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK gmsGmkPistonWork = (GMS_GMK_PISTON_WORK)obj_work;
        --gmsGmkPistonWork.timer_dec;
        if (gmsGmkPistonWork.timer_dec > 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonTopDeadWait_200);
    }

    private static void gmGmkPistonTopDeadWait_200(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK gmsGmkPistonWork = (GMS_GMK_PISTON_WORK)obj_work;
        gmsGmkPistonWork.timer_dec = gmsGmkPistonWork.timer_set_move;
        gmGmkPistonShrink(obj_work);
    }

    private static void gmGmkPistonShrink(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK gmsGmkPistonWork = (GMS_GMK_PISTON_WORK)obj_work;
        obj_work.spd.y = gmsGmkPistonWork.piston_vect == 0 ? -gmsGmkPistonWork.stroke_spd : gmsGmkPistonWork.stroke_spd;
        obj_work.pos.y += obj_work.spd.y;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonShrink_100);
        gmGmkPistonShrink_100(obj_work);
        GmSoundPlaySE("Piston1");
    }

    private static void gmGmkPistonShrink_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK gmsGmkPistonWork = (GMS_GMK_PISTON_WORK)obj_work;
        gmsGmkPistonWork.timer_dec -= gmsGmkPistonWork.stroke_spd;
        if (gmsGmkPistonWork.timer_dec > 0)
            return;
        obj_work.spd.y = 0;
        if (gmsGmkPistonWork.timer_dec < 0)
        {
            obj_work.spd.y = gmsGmkPistonWork.timer_dec;
            if (gmsGmkPistonWork.piston_vect != 32768)
                obj_work.spd.y = -obj_work.spd.y;
        }
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonShrink_200);
    }

    private static void gmGmkPistonShrink_200(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTON_WORK pwork = (GMS_GMK_PISTON_WORK)obj_work;
        uint num = gmGmkPistonSyncTimeGet(pwork);
        pwork.timer_dec = num > (uint)pwork.timer_set_wait_lower ? 0 : (int)(pwork.timer_set_wait_lower - (num - 1U));
        gmGmkPistonStay(obj_work);
    }

    private static void gmGmkPistonRodStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PISTONROD_WORK gmkPistonrodWork = (GMS_GMK_PISTONROD_WORK)obj_work;
        int num = MTM_MATH_ABS(obj_work.parent_obj.pos.y - gmkPistonrodWork.fulcrum) / 8;
        obj_work.scale.y = num;
    }

    private static void gmGmkPistonRod_Create(OBS_OBJECT_WORK parent_obj)
    {
        GMS_GMK_PISTONROD_WORK work = (GMS_GMK_PISTONROD_WORK)GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_PISTONROD_WORK(), null, 0, "Gmk_PistonRod");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_piston_obj_3d_list[1], work.eff_work.obj_3d);
        obj_work.parent_obj = parent_obj;
        obj_work.parent_ofst.x = 0;
        obj_work.parent_ofst.y = 65536;
        obj_work.parent_ofst.z = -524288;
        obj_work.dir.z = (ushort)(parent_obj.dir.z ^ 32768U);
        if (obj_work.dir.z == 0)
            obj_work.parent_ofst.y = -obj_work.parent_ofst.y;
        obj_work.flag |= 1024U;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        obj_work.disp_flag &= 4294967039U;
        obj_work.flag |= 2U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPistonRodStay);
        work.fulcrum = parent_obj.pos.y + obj_work.parent_ofst.y;
    }

    private static OBS_OBJECT_WORK gmGmkPistonInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_PISTON_WORK work = (GMS_GMK_PISTON_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_PISTON_WORK(), "Gmk_PistonRod");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_piston_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -131072;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        obj_work.disp_flag &= 4294967039U;
        obj_work.flag |= 2U;
        work.stroke_spd = 16384;
        work.timer_set_move = 524288;
        if ((eve_rec.flag & 31) != 0)
        {
            int num = (eve_rec.flag & 31) > 16 ? -(eve_rec.flag & 15) << 10 : (eve_rec.flag & 31) << 10;
            work.stroke_spd += num;
        }
        work.efct_di = (eve_rec.flag & 128) != 0;
        work.timer_set_wait_upper = eve_rec.left * 2;
        work.timer_set_wait_lower = eve_rec.height * 2;
        return obj_work;
    }

}