public partial class AppMain
{

    private static void GmEneBukuBuild()
    {
        gm_ene_buku_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(696)), readAMBFile(GmGameDatGetEnemyData(697)), 0U);
    }

    private static void GmEneBukuFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(696));
        GmGameDBuildRegFlushModel(gm_ene_buku_obj_3d_list, amsAmbHeader.file_num);
    }

    private static OBS_OBJECT_WORK GmEneBukuInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_BUKU_WORK(), "ENE_BUKU");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_BUKU_WORK gmsEneBukuWork = (GMS_ENE_BUKU_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_buku_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(698), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -8, -8, 8, 8);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        ObjRectWorkSet(pRec2, -16, -16, 16, 16);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -19, -16, 19, 16);
        pRec3.flag &= 4294967291U;
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        if ((eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        gmsEneBukuWork.spd_dec = 102;
        gmsEneBukuWork.spd_dec_dist = 20480;
        gmEneBukuWalkInit(work);
        GmComEfctSetDispOffsetF(GmEfctEneEsCreate(work, 9), -24f, -5f, 0.0f);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmEneBukuWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 0, 1);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneBukuWalkMain);
        obj_work.move_flag &= 4294967291U;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -2048;
        else
            obj_work.spd.x = 2048;
    }

    private static void gmEneBukuWalkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_BUKU_WORK buku_work = (GMS_ENE_BUKU_WORK)obj_work;
        if (gmEneBukuSetWalkSpeed(buku_work) != 0)
            gmEneBukuFlipInit(obj_work);
        if (buku_work.timer > 0)
            --buku_work.timer;
        else
            buku_work.timer = 216000 + mtMathRand() % 30;
    }

    private static void gmEneBukuFwMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        gmEneBukuFlipInit(obj_work);
    }

    private static void gmEneBukuFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSet3DNNBlendDependHFlip(obj_work, 2, 3);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneBukuFlipMain);
    }

    private static void gmEneBukuFlipMain(OBS_OBJECT_WORK obj_work)
    {
        gmEneBukuSetWalkSpeed((GMS_ENE_BUKU_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        gmEneBukuWalkInit(obj_work);
    }

    private static int gmEneBukuSetWalkSpeed(GMS_ENE_BUKU_WORK buku_work)
    {
        int num = 0;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)buku_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 3 && obsObjectWork.obj_3d.frame[0] >= 20.0)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, buku_work.spd_dec, 2048);
            else if (obsObjectWork.pos.x <= (int)obsObjectWork.user_work + buku_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, buku_work.spd_dec);
                num = 1;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > (int)obsObjectWork.user_work)
                {
                    obsObjectWork.spd.x = (int)obsObjectWork.user_work - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -buku_work.spd_dec)
                        obsObjectWork.spd.x = -buku_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -2048)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -buku_work.spd_dec, 2048);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 2 && obsObjectWork.obj_3d.frame[0] >= 20.0)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -buku_work.spd_dec, 2048);
        else if (obsObjectWork.pos.x >= (int)obsObjectWork.user_flag - buku_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, buku_work.spd_dec);
            num = 1;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < (int)obsObjectWork.user_flag)
            {
                obsObjectWork.spd.x = (int)obsObjectWork.user_flag - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > buku_work.spd_dec)
                    obsObjectWork.spd.x = buku_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < 2048)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, buku_work.spd_dec, 2048);
        return num;
    }
}