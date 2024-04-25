public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSpearUInit(
         GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        GMS_GMK_SPEAR_WORK pwork = (GMS_GMK_SPEAR_WORK)gmGmkSpearInit(eve_rec, pos_x, pos_y, type);
        pwork.obj_type = 0U;
        pwork.vect = 49152;
        pwork.gmk_work.ene_com.obj_work.dir.z = 0;
        if (eve_rec.left > 0)
            pwork.timer_set_wait_upper = eve_rec.left;
        if (eve_rec.width > 0)
            pwork.timer_set_wait_lower = eve_rec.width;
        if (eve_rec.top < 0)
            pwork.timer_set_move = -(eve_rec.top << 12);
        gmGmkSpear_CreateParts(pwork);
        pwork.gmk_work.ene_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearStart);
        return pwork.gmk_work.ene_com.obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkSpearDInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SPEAR_WORK pwork = (GMS_GMK_SPEAR_WORK)gmGmkSpearInit(eve_rec, pos_x, pos_y, type);
        pwork.obj_type = 1U;
        pwork.gmk_work.ene_com.obj_work.dir.z = 32768;
        pwork.vect = 16384;
        if (eve_rec.left > 0)
            pwork.timer_set_wait_upper = eve_rec.left;
        if (eve_rec.width > 0)
            pwork.timer_set_wait_lower = eve_rec.width;
        if (eve_rec.top > 0)
            pwork.timer_set_move = eve_rec.top << 12;
        gmGmkSpear_CreateParts(pwork);
        pwork.gmk_work.ene_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearStart);
        return pwork.gmk_work.ene_com.obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkSpearLInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SPEAR_WORK pwork = (GMS_GMK_SPEAR_WORK)gmGmkSpearInit(eve_rec, pos_x, pos_y, type);
        pwork.obj_type = 2U;
        pwork.gmk_work.ene_com.obj_work.dir.z = 49152;
        pwork.vect = 32768;
        if (eve_rec.top > 0)
            pwork.timer_set_wait_upper = eve_rec.top;
        if (eve_rec.width > 0)
            pwork.timer_set_wait_lower = eve_rec.width;
        if (eve_rec.left < 0)
            pwork.timer_set_move = -(eve_rec.left << 12);
        gmGmkSpear_CreateParts(pwork);
        pwork.gmk_work.ene_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearStart);
        return pwork.gmk_work.ene_com.obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkSpearRInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SPEAR_WORK pwork = (GMS_GMK_SPEAR_WORK)gmGmkSpearInit(eve_rec, pos_x, pos_y, type);
        pwork.obj_type = 3U;
        pwork.gmk_work.ene_com.obj_work.dir.z = 16384;
        pwork.vect = 0;
        if (eve_rec.top > 0)
            pwork.timer_set_wait_upper = eve_rec.top;
        if (eve_rec.width > 0)
            pwork.timer_set_wait_lower = eve_rec.width;
        if (eve_rec.left > 0)
            pwork.timer_set_move = eve_rec.left << 12;
        gmGmkSpear_CreateParts(pwork);
        pwork.gmk_work.ene_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearStart);
        return pwork.gmk_work.ene_com.obj_work;
    }

    public static void GmGmkSpearBuild()
    {
        gm_gmk_spear_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(856)), readAMBFile(GmGameDatGetGimmickData(857)), 0U);
    }

    public static void GmGmkSpearFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(856));
        GmGameDBuildRegFlushModel(gm_gmk_spear_obj_3d_list, amsAmbHeader.file_num);
    }

    private static uint gmGmkSpearSyncTimeGet(GMS_GMK_SPEAR_WORK pwork)
    {
        return g_gm_main_system.sync_time % ((uint)((pwork.timer_set_move + (pwork.stroke_spd - 1)) / pwork.stroke_spd) * 2U + (uint)pwork.timer_set_wait_upper + (uint)pwork.timer_set_wait_lower);
    }

    private static void gmGmkSpearStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (GMS_GMK_SPEAR_WORK)obj_work;
        gmsGmkSpearWork.gmk_work.ene_com.rect_work[2].flag &= 4294967291U;
        gmsGmkSpearWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsGmkSpearWork.gmk_work.ene_com.rect_work[1].flag |= 4U;
        OBS_RECT_WORK pRec = gmsGmkSpearWork.gmk_work.ene_com.rect_work[1];
        ObjRectWorkZSet(pRec, tbl_gm_gmk_spear_rect[(int)gmsGmkSpearWork.obj_type][0], tbl_gm_gmk_spear_rect[(int)gmsGmkSpearWork.obj_type][1], -500, tbl_gm_gmk_spear_rect[(int)gmsGmkSpearWork.obj_type][2], tbl_gm_gmk_spear_rect[(int)gmsGmkSpearWork.obj_type][3], 500);
        pRec.flag |= 4U;
        pRec.flag |= 1024U;
        obj_work.flag &= 4294967293U;
        uint syncTime = g_gm_main_system.sync_time;
        uint num1 = (uint)((gmsGmkSpearWork.timer_set_move + (gmsGmkSpearWork.stroke_spd - 1)) / gmsGmkSpearWork.stroke_spd);
        uint num2 = num1 * 2U + (uint)gmsGmkSpearWork.timer_set_wait_upper + (uint)gmsGmkSpearWork.timer_set_wait_lower;
        if (syncTime <= (uint)gmsGmkSpearWork.timer_dec)
        {
            gmsGmkSpearWork.timer_dec -= (int)syncTime - 1;
            gmsGmkSpearWork.timer_dec += gmsGmkSpearWork.timer_set_wait_lower;
            gmGmkSpearStay(obj_work);
        }
        else
        {
            uint num3 = (syncTime - (uint)gmsGmkSpearWork.timer_dec) % num2;
            if (num3 <= (uint)gmsGmkSpearWork.timer_set_wait_lower)
            {
                gmsGmkSpearWork.timer_dec = (int)(gmsGmkSpearWork.timer_set_wait_lower - (num3 - 1U));
                gmGmkSpearStay(obj_work);
            }
            else
            {
                uint num4 = num3 - (uint)gmsGmkSpearWork.timer_set_wait_lower;
                int num5 = mtMathCos(gmsGmkSpearWork.vect);
                int num6 = mtMathSin(gmsGmkSpearWork.vect);
                int num7 = num5 * gmsGmkSpearWork.stroke_spd >> 12;
                int num8 = num6 * gmsGmkSpearWork.stroke_spd >> 12;
                if (num4 < num1)
                {
                    gmsGmkSpearWork.timer_dec = gmsGmkSpearWork.timer_set_move;
                    for (; num4 > 1U; --num4)
                    {
                        gmsGmkSpearWork.timer_dec -= gmsGmkSpearWork.stroke_spd;
                        obj_work.pos.x += num7;
                        obj_work.pos.y += num8;
                    }
                    obj_work.spd.x = num7;
                    obj_work.spd.y = num8;
                    gmGmkSpearStroke(obj_work);
                }
                else
                {
                    uint num9 = num4 - num1;
                    int num10 = mtMathCos(gmsGmkSpearWork.vect) * gmsGmkSpearWork.timer_set_move >> 12;
                    int num11 = mtMathSin(gmsGmkSpearWork.vect) * gmsGmkSpearWork.timer_set_move >> 12;
                    obj_work.pos.x += num10;
                    obj_work.pos.y += num11;
                    if (num9 <= (uint)gmsGmkSpearWork.timer_set_wait_upper)
                    {
                        gmsGmkSpearWork.timer_dec = (int)(gmsGmkSpearWork.timer_set_wait_upper - (num9 - 1U));
                        gmGmkSpearWait(obj_work);
                    }
                    else
                    {
                        uint num12 = num9 - (uint)gmsGmkSpearWork.timer_set_wait_upper;
                        gmsGmkSpearWork.timer_dec = gmsGmkSpearWork.timer_set_move;
                        for (; num12 > 1U; --num12)
                        {
                            gmsGmkSpearWork.timer_dec -= gmsGmkSpearWork.stroke_spd;
                            obj_work.pos.x -= num7;
                            obj_work.pos.y -= num8;
                        }
                        obj_work.spd.x = -num7;
                        obj_work.spd.y = -num8;
                        gmGmkSpearShrink(obj_work);
                    }
                }
            }
        }
    }

    private static void gmGmkSpearStay(OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearStay_100);
        gmGmkSpearStay_100(obj_work);
    }

    private static void gmGmkSpearStay_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (GMS_GMK_SPEAR_WORK)obj_work;
        --gmsGmkSpearWork.timer_dec;
        if (gmsGmkSpearWork.timer_dec > 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearStay_200);
    }

    private static void gmGmkSpearStay_200(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (GMS_GMK_SPEAR_WORK)obj_work;
        obj_work.spd.x = mtMathCos(gmsGmkSpearWork.vect);
        obj_work.spd.y = mtMathSin(gmsGmkSpearWork.vect);
        obj_work.spd.x = obj_work.spd.x * gmsGmkSpearWork.stroke_spd >> 12;
        obj_work.spd.y = obj_work.spd.y * gmsGmkSpearWork.stroke_spd >> 12;
        gmsGmkSpearWork.timer_dec = gmsGmkSpearWork.timer_set_move;
        GmSoundPlaySE("Spear");
        gmGmkSpearStroke(obj_work);
    }

    private static void gmGmkSpearStroke(OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x += obj_work.spd.x;
        obj_work.pos.y += obj_work.spd.y;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearStroke_100);
        gmGmkSpearStroke_100(obj_work);
    }

    private static void gmGmkSpearStroke_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (GMS_GMK_SPEAR_WORK)obj_work;
        gmsGmkSpearWork.timer_dec -= gmsGmkSpearWork.stroke_spd;
        if (gmsGmkSpearWork.timer_dec > 0)
            return;
        if (gmsGmkSpearWork.timer_dec < 0)
        {
            obj_work.spd.x = mtMathCos(gmsGmkSpearWork.vect);
            obj_work.spd.y = mtMathSin(gmsGmkSpearWork.vect);
            obj_work.spd.x = obj_work.spd.x * gmsGmkSpearWork.timer_dec >> 12;
            obj_work.spd.y = obj_work.spd.y * gmsGmkSpearWork.timer_dec >> 12;
        }
        else
        {
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearStroke_200);
    }

    private static void gmGmkSpearStroke_200(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPEAR_WORK pwork = (GMS_GMK_SPEAR_WORK)obj_work;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        uint num1 = (uint)((pwork.timer_set_move + (pwork.stroke_spd - 1)) / pwork.stroke_spd);
        uint num2 = gmGmkSpearSyncTimeGet(pwork) - (uint)pwork.timer_set_wait_lower - num1;
        pwork.timer_dec = num2 > (uint)pwork.timer_set_wait_upper ? 0 : (int)(pwork.timer_set_wait_upper - (num2 - 1U));
        gmGmkSpearWait(obj_work);
    }

    private static void gmGmkSpearWait(OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearWait_100);
        gmGmkSpearWait_100(obj_work);
    }

    private static void gmGmkSpearWait_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (GMS_GMK_SPEAR_WORK)obj_work;
        --gmsGmkSpearWork.timer_dec;
        if (gmsGmkSpearWork.timer_dec > 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearWait_200);
    }

    private static void gmGmkSpearWait_200(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (GMS_GMK_SPEAR_WORK)obj_work;
        obj_work.spd.x = -(mtMathCos(gmsGmkSpearWork.vect) * gmsGmkSpearWork.stroke_spd) >> 12;
        obj_work.spd.y = -(mtMathSin(gmsGmkSpearWork.vect) * gmsGmkSpearWork.stroke_spd) >> 12;
        gmsGmkSpearWork.timer_dec = gmsGmkSpearWork.timer_set_move;
        gmGmkSpearShrink(obj_work);
    }

    private static void gmGmkSpearShrink(OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x += obj_work.spd.x;
        obj_work.pos.y += obj_work.spd.y;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearShrink_100);
        gmGmkSpearShrink_100(obj_work);
    }

    private static void gmGmkSpearShrink_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (GMS_GMK_SPEAR_WORK)obj_work;
        gmsGmkSpearWork.timer_dec -= gmsGmkSpearWork.stroke_spd;
        if (gmsGmkSpearWork.timer_dec > 0)
            return;
        if (gmsGmkSpearWork.timer_dec < 0)
        {
            obj_work.spd.x = mtMathCos(gmsGmkSpearWork.vect);
            obj_work.spd.y = mtMathSin(gmsGmkSpearWork.vect);
            obj_work.spd.x = -(obj_work.spd.x * gmsGmkSpearWork.timer_dec >> 12);
            obj_work.spd.y = -(obj_work.spd.y * gmsGmkSpearWork.timer_dec >> 12);
        }
        else
        {
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearShrink_200);
    }

    private static void gmGmkSpearShrink_200(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPEAR_WORK pwork = (GMS_GMK_SPEAR_WORK)obj_work;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        uint num = gmGmkSpearSyncTimeGet(pwork);
        pwork.timer_dec = num > (uint)pwork.timer_set_wait_lower ? 0 : (int)(pwork.timer_set_wait_lower - (num - 1U));
        gmGmkSpearStay(obj_work);
    }

    private static void gmGmkSpearRod(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SPEARPARTS_WORK gmkSpearpartsWork = (GMS_GMK_SPEARPARTS_WORK)obj_work;
        switch (gmkSpearpartsWork.connect_type)
        {
            case 0:
                gmkSpearpartsWork.connect = gmkSpearpartsWork.parent_connect.pos.y;
                break;
            case 1:
                gmkSpearpartsWork.connect = gmkSpearpartsWork.parent_connect.pos.y;
                break;
            case 2:
                gmkSpearpartsWork.connect = gmkSpearpartsWork.parent_connect.pos.x;
                break;
            case 3:
                gmkSpearpartsWork.connect = gmkSpearpartsWork.parent_connect.pos.x;
                break;
        }
        int num = MTM_MATH_ABS(gmkSpearpartsWork.connect - gmkSpearpartsWork.fulcrum) / 5;
        obj_work.scale.y = num;
    }

    private static void gmGmkSpear_CreateParts(GMS_GMK_SPEAR_WORK pwork)
    {
        OBS_OBJECT_WORK objWork = pwork.gmk_work.ene_com.obj_work;
        OBS_OBJECT_WORK work1 = GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_SPEARPARTS_WORK(), null, 0, "Gmk_SpearBase");
        GMS_GMK_SPEARPARTS_WORK gmkSpearpartsWork1 = (GMS_GMK_SPEARPARTS_WORK)work1;
        ObjObjectCopyAction3dNNModel(work1, gm_gmk_spear_obj_3d_list[2], gmkSpearpartsWork1.eff_work.obj_3d);
        work1.parent_obj = objWork;
        work1.pos.x = objWork.pos.x;
        work1.pos.y = objWork.pos.y;
        work1.pos.z = objWork.pos.z;
        switch (pwork.obj_type)
        {
            case 0:
                work1.pos.y += 16384;
                break;
            case 1:
                work1.pos.y -= 16384;
                break;
            case 2:
                work1.pos.x += 16384;
                break;
            case 3:
                work1.pos.x -= 16384;
                break;
        }
        work1.dir.z = objWork.dir.z;
        work1.flag &= 4294966271U;
        work1.move_flag |= 256U;
        work1.disp_flag &= 4294967039U;
        work1.flag |= 2U;
        work1.ppFunc = null;
        OBS_OBJECT_WORK work2 = GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_SPEARPARTS_WORK(), null, 0, "Gmk_SpearRod");
        GMS_GMK_SPEARPARTS_WORK gmkSpearpartsWork2 = (GMS_GMK_SPEARPARTS_WORK)work2;
        ObjObjectCopyAction3dNNModel(work2, gm_gmk_spear_obj_3d_list[1], gmkSpearpartsWork2.eff_work.obj_3d);
        work2.parent_obj = objWork;
        work2.parent_ofst.x = 0;
        work2.parent_ofst.y = 0;
        work2.parent_ofst.z = -1;
        work2.dir.z = objWork.dir.z;
        work2.flag |= 1024U;
        work2.move_flag |= 256U;
        work2.disp_flag &= 4294967039U;
        work2.flag |= 2U;
        switch (pwork.obj_type)
        {
            case 0:
                gmkSpearpartsWork2.connect = objWork.pos.y;
                break;
            case 1:
                gmkSpearpartsWork2.connect = objWork.pos.y;
                break;
            case 2:
                gmkSpearpartsWork2.connect = objWork.pos.x;
                break;
            case 3:
                gmkSpearpartsWork2.connect = objWork.pos.x;
                break;
        }
        gmkSpearpartsWork2.connect_type = pwork.obj_type;
        gmkSpearpartsWork2.parent_connect = objWork;
        gmkSpearpartsWork2.obj_type = pwork.obj_type;
        gmkSpearpartsWork2.fulcrum = gmkSpearpartsWork2.connect;
        work2.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpearRod);
    }

    private static OBS_OBJECT_WORK gmGmkSpearInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SPEAR_WORK work = (GMS_GMK_SPEAR_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_SPEAR_WORK(), "Gmk_Spear");
        OBS_OBJECT_WORK objWork = work.gmk_work.ene_com.obj_work;
        GMS_ENEMY_3D_WORK gmkWork = work.gmk_work;
        ObjObjectCopyAction3dNNModel(objWork, gm_gmk_spear_obj_3d_list[0], gmkWork.obj_3d);
        objWork.pos.z = 0;
        objWork.move_flag |= 256U;
        work.timer_set_move = 196608;
        work.stroke_spd = 32768;
        work.timer_set_wait_upper = 120;
        work.timer_set_wait_lower = 120;
        work.timer_dec = eve_rec.height;
        if ((eve_rec.flag & 31) != 0)
        {
            int num;
            if ((eve_rec.flag & 16) == 0)
            {
                num = (eve_rec.flag & 15) << 10;
            }
            else
            {
                num = -(eve_rec.flag & 15) << 10;
                if (num == 0)
                    num = -16384;
            }
            work.stroke_spd += num;
        }
        return objWork;
    }


}