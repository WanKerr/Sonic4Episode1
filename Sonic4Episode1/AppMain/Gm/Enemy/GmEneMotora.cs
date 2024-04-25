using System;

public partial class AppMain
{
    private static OBS_OBJECT_WORK GmEneMotoraInit(
         GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_MOTORA_WORK(), "ENE_MOTORA");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_MOTORA_WORK gmsEneMotoraWork = (GMS_ENE_MOTORA_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_motora_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(663), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -11, -24, 11, 0);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        ObjRectWorkSet(pRec2, -19, -32, 19, 0);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -19, -32, 19, 0);
        pRec3.flag &= 4294967291U;
        ObjObjectFieldRectSet(work, -4, -8, 4, -2);
        work.move_flag |= 128U;
        if ((eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        gmsEneMotoraWork.spd_dec = 102;
        gmsEneMotoraWork.spd_dec_dist = 20480;
        gmEneMotoraWalkInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static void GmEneMotoraBuild()
    {
        gm_ene_motora_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(661)), readAMBFile(GmGameDatGetEnemyData(662)), 0U);
    }

    public static void GmEneMotoraFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(661));
        GmGameDBuildRegFlushModel(gm_ene_motora_obj_3d_list, amsAmbHeader.file_num);
    }

    public static void gmEneMotoraWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 1, 2);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMotoraWalkMain);
        obj_work.move_flag &= 4294967291U;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -2048;
        else
            obj_work.spd.x = 2048;
    }

    private static void gmEneMotoraWalkMain(OBS_OBJECT_WORK obj_work)
    {
        if (!gmEneMotoraSetWalkSpeed((GMS_ENE_MOTORA_WORK)obj_work))
            return;
        gmEneMotoraFlipInit(obj_work);
    }

    private static bool gmEneMotoraSetWalkSpeed(GMS_ENE_MOTORA_WORK motora_work)
    {
        bool flag = false;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)motora_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 4 && obsObjectWork.obj_3d.frame[0] >= 20.0)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, motora_work.spd_dec, 2048);
            else if (obsObjectWork.pos.x <= Convert.ToInt32(obsObjectWork.user_work) + motora_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, motora_work.spd_dec);
                flag = true;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > Convert.ToInt32(obsObjectWork.user_work))
                {
                    obsObjectWork.spd.x = Convert.ToInt32(obsObjectWork.user_work) - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -motora_work.spd_dec)
                        obsObjectWork.spd.x = -motora_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -2048)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -motora_work.spd_dec, 2048);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 3 && obsObjectWork.obj_3d.frame[0] >= 20.0)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -motora_work.spd_dec, 2048);
        else if (obsObjectWork.pos.x >= Convert.ToInt32(obsObjectWork.user_flag) - motora_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, motora_work.spd_dec);
            flag = true;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < Convert.ToInt32(obsObjectWork.user_flag))
            {
                obsObjectWork.spd.x = Convert.ToInt32(obsObjectWork.user_flag) - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > motora_work.spd_dec)
                    obsObjectWork.spd.x = motora_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < 2048)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, motora_work.spd_dec, 2048);
        return flag;
    }

    private static void gmEneMotoraFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSet3DNNBlendDependHFlip(obj_work, 3, 4);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMotoraFlipMain);
    }

    private static void gmEneMotoraFlipMain(OBS_OBJECT_WORK obj_work)
    {
        gmEneMotoraSetWalkSpeed((GMS_ENE_MOTORA_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        gmEneMotoraWalkInit(obj_work);
    }
}