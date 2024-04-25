public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSeesaw0Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = gmGmkSeesawInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSeesawWork.tilt = 0;
        gmGmkSeesawStart(gmsGmkSeesawWork.gmk_work.ene_com.obj_work);
        return gmsGmkSeesawWork.gmk_work.ene_com.obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkSeesaw30Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = gmGmkSeesawInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSeesawWork.tilt = 4608;
        gmGmkSeesawStart(gmsGmkSeesawWork.gmk_work.ene_com.obj_work);
        return gmsGmkSeesawWork.gmk_work.ene_com.obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkSeesaw330Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = gmGmkSeesawInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSeesawWork.tilt = -4608;
        gmGmkSeesawStart(gmsGmkSeesawWork.gmk_work.ene_com.obj_work);
        return gmsGmkSeesawWork.gmk_work.ene_com.obj_work;
    }

    public static void GmGmkSeesawBuild()
    {
        gm_gmk_seesaw_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(876), GmGameDatGetGimmickData(877), 0U);
        for (int index = 0; index < 16; ++index)
            seesaw_alive[index] = 0;
        control_right = null;
    }

    public static void GmGmkSeesawFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(876);
        GmGameDBuildRegFlushModel(gm_gmk_seesaw_obj_3d_list, gimmickData.file_num);
    }

    private static void gmGmkSeesawExitTCB(MTS_TASK_TCB tcb)
    {
        GMS_GMK_SEESAW_WORK tcbWork = (GMS_GMK_SEESAW_WORK)mtTaskGetTcbWork(tcb);
        --seesaw_alive[tcbWork.seesaw_id];
        if (control_right == tcbWork)
        {
            control_right = null;
            lock_seesaw_id = 0;
        }
        GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkSeesawStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = (GMS_GMK_SEESAW_WORK)obj_work;
        gmsGmkSeesawWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec1 = gmsGmkSeesawWork.gmk_work.ene_com.rect_work[2];
        pRec1.ppDef = null;
        pRec1.ppHit = null;
        ObjRectAtkSet(pRec1, 0, 0);
        ObjRectDefSet(pRec1, 65534, 0);
        ObjRectWorkSet(pRec1, -48, -24, 48, 0);
        OBS_RECT_WORK pRec2 = gmsGmkSeesawWork.gmk_work.ene_com.rect_work[0];
        pRec2.flag |= 4U;
        pRec2.ppDef = null;
        pRec2.ppHit = null;
        ObjRectWorkSet(pRec2, -2, -2, 2, 2);
        obj_work.flag &= 4294967293U;
        gmsGmkSeesawWork.initial_tilt = gmsGmkSeesawWork.tilt;
        gmsGmkSeesawWork.tilt_d = 0;
        gmsGmkSeesawWork.tilt_se_timer = 0;
        if (seesaw_alive[gmsGmkSeesawWork.seesaw_id] == 0)
            seesaw_tilt[gmsGmkSeesawWork.seesaw_id] = gmsGmkSeesawWork.tilt;
        else
            gmsGmkSeesawWork.tilt = seesaw_tilt[gmsGmkSeesawWork.seesaw_id];
        obj_work.dir.z = (ushort)gmsGmkSeesawWork.tilt;
        ++seesaw_alive[gmsGmkSeesawWork.seesaw_id];
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkSeesawExitTCB));
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSeesawStay);
    }

    private static void gmGmkSeesawStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = (GMS_GMK_SEESAW_WORK)obj_work;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (lock_seesaw_id == gmsGmkSeesawWork.seesaw_id)
        {
            gmsGmkSeesawWork.tilt = seesaw_tilt[gmsGmkSeesawWork.seesaw_id];
            obj_work.dir.z = (ushort)gmsGmkSeesawWork.tilt;
            gmsGmkSeesawWork.tilt_timer = 60;
        }
        else if (gmsGmkSeesawWork.tilt != gmsGmkSeesawWork.initial_tilt)
        {
            if (gmsGmkSeesawWork.tilt_timer <= 0)
            {
                if (gmsGmkSeesawWork.tilt > gmsGmkSeesawWork.initial_tilt)
                {
                    gmsGmkSeesawWork.tilt -= 256;
                    if (gmsGmkSeesawWork.tilt < gmsGmkSeesawWork.initial_tilt)
                        gmsGmkSeesawWork.tilt = gmsGmkSeesawWork.initial_tilt;
                }
                else if (gmsGmkSeesawWork.tilt < gmsGmkSeesawWork.initial_tilt)
                {
                    gmsGmkSeesawWork.tilt += 256;
                    if (gmsGmkSeesawWork.tilt > gmsGmkSeesawWork.initial_tilt)
                        gmsGmkSeesawWork.tilt = gmsGmkSeesawWork.initial_tilt;
                }
            }
            else
                --gmsGmkSeesawWork.tilt_timer;
        }
        obj_work.dir.z = (ushort)gmsGmkSeesawWork.tilt;
        if (gmsGmkSeesawWork.tilt == 0)
            gmsGmkSeesawWork.tilt = 0;
        if ((gmsPlayerWork.player_flag & 1024) != 0 || (gmsPlayerWork.obj_work.flag & 2) != 0 || (gmsPlayerWork.obj_work.move_flag & 16) != 0 && gmsPlayerWork.obj_work.spd.y < 0)
            return;
        int num1 = mtMathSin((ushort)gmsGmkSeesawWork.tilt) * 29;
        int num2 = mtMathCos((ushort)gmsGmkSeesawWork.tilt) * 29;
        int num3 = mtMathCos((ushort)gmsGmkSeesawWork.tilt) * 112 / 2;
        int num4 = MTM_MATH_ABS(mtMathSin((ushort)gmsGmkSeesawWork.tilt) * 112 / 2);
        int num5 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        if (num5 < -(num3 + 131072 - num1) || num5 > num3 + 131072 + num1)
            return;
        int num6 = 0;
        int num7 = 0;
        long num8;
        long num9;
        if (gmsPlayerWork.obj_work.move.x == 0)
        {
            if (gmsPlayerWork.obj_work.move.y == 0)
                return;
            num8 = gmsPlayerWork.obj_work.pos.x;
            num9 = 0L;
        }
        else if (gmsPlayerWork.obj_work.move.y == 0)
        {
            num8 = 0L;
            num9 = gmsPlayerWork.obj_work.pos.y;
        }
        else
        {
            num8 = (gmsPlayerWork.obj_work.move.y << 12) / (long)gmsPlayerWork.obj_work.move.x;
            long num10 = num8 * gmsPlayerWork.obj_work.pos.x >> 12;
            num9 = gmsPlayerWork.obj_work.pos.y - num10;
        }
        long num11 = mtMathCos((ushort)gmsGmkSeesawWork.tilt);
        long num12 = ((long)mtMathSin((ushort)gmsGmkSeesawWork.tilt) << 12) / num11;
        long num13 = num12 * (obj_work.pos.x + num1) >> 12;
        long num14 = obj_work.pos.y - num2 - num13;
        if (num8 != 0L && num8 == num12)
        {
            num6 = gmsPlayerWork.obj_work.pos.x;
            num7 = (int)((num12 * num6 >> 12) + num14);
        }
        else if (num8 != 0L && num9 != 0L)
        {
            num6 = (int)((num14 - num9 << 12) / (num8 - num12));
            num7 = (int)((num12 * num6 >> 12) + num14);
        }
        else if (num8 == 0L)
        {
            num7 = (int)num9;
            num6 = num12 == 0L ? gmsPlayerWork.obj_work.pos.x : (int)((num7 - num14 << 12) / num12);
        }
        else if (num9 == 0L)
        {
            num6 = (int)num8;
            num7 = (int)((num12 * num8 >> 12) + num14);
        }
        if (obj_work.pos.x - (num3 - num1) > num6 || num6 > obj_work.pos.x + (num3 + num1) || (obj_work.pos.y + (num4 - num2) + 2 < num7 || num7 < obj_work.pos.y - (num4 + num2) - 2))
            return;
        int a1 = gmsPlayerWork.obj_work.pos.x - gmsPlayerWork.obj_work.move.x;
        int a2 = gmsPlayerWork.obj_work.pos.y - gmsPlayerWork.obj_work.move.y;
        int x = gmsPlayerWork.obj_work.pos.x;
        int y = gmsPlayerWork.obj_work.pos.y;
        if (a1 < x)
            MTM_MATH_SWAP(ref a1, ref x);
        if (a2 < y)
            MTM_MATH_SWAP(ref a2, ref y);
        int num15 = a2 + 1280;
        int num16 = y - 1280;
        if (x > num6 || num6 > a1 || (num15 < num7 || num7 < num16))
            return;
        gmsGmkSeesawWork.ply_work = gmsPlayerWork;
        gmsGmkSeesawWork.hold_x = num6 - num1;
        gmsGmkSeesawWork.hold_y = num7;
        if (control_right == null)
            gmGmkSeesaw_PlayerHold(obj_work);
        else
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSeesaw_PlayerHold);
    }

    private static void gmGmkSeesaw_PlayerHold(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = (GMS_GMK_SEESAW_WORK)obj_work;
        GMS_PLAYER_WORK plyWork = gmsGmkSeesawWork.ply_work;
        if (control_right != null)
        {
            if (control_right.gmk_work.ene_com.obj_work.pos.x >= obj_work.pos.x && plyWork.obj_work.spd.x >= 0 || control_right.gmk_work.ene_com.obj_work.pos.x <= obj_work.pos.x && plyWork.obj_work.spd.x <= 0)
            {
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSeesawStay);
                gmGmkSeesawStay(obj_work);
                return;
            }
            gmsGmkSeesawWork.player_speed = control_right.player_speed;
        }
        else
            gmsGmkSeesawWork.player_speed = plyWork.obj_work.move.x / 2;
        GmPlySeqGmkInitSeesaw(plyWork, gmsGmkSeesawWork.gmk_work.ene_com);
        long num = (gmsGmkSeesawWork.hold_x - obj_work.pos.x << 12) / (long)mtMathCos(gmsGmkSeesawWork.tilt);
        gmsGmkSeesawWork.player_distance = num;
        control_right = gmsGmkSeesawWork;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSeesaw_PlayerHold_100);
        gmGmkSeesaw_PlayerHold_100(obj_work);
    }

    private static void gmGmkSeesaw_PlayerHold_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = (GMS_GMK_SEESAW_WORK)obj_work;
        GMS_PLAYER_WORK plyWork = gmsGmkSeesawWork.ply_work;
        lock_seesaw_id = 0;
        if (plyWork.seq_state != GME_PLY_SEQ_STATE_GMK_SEESAW || control_right != gmsGmkSeesawWork || ((int)plyWork.obj_work.move_flag & 256) != 0)
        {
            gmsGmkSeesawWork.tilt_d = 0;
            gmsGmkSeesawWork.tilt_se_timer = 0;
            if (control_right == gmsGmkSeesawWork)
                control_right = null;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSeesawStay);
            gmGmkSeesawStay(obj_work);
        }
        else
        {
            lock_seesaw_id = gmsGmkSeesawWork.seesaw_id;
            gmsGmkSeesawWork.tilt_timer = 60;
            control_right = gmsGmkSeesawWork;
            int num1 = GmPlayerKeyGetGimmickRotZ(gmsGmkSeesawWork.ply_work);
            if (num1 > 256)
                num1 = 256;
            if (num1 < -256)
                num1 = -256;
            gmsGmkSeesawWork.tilt_d = (short)num1;
            if (gmsGmkSeesawWork.tilt_d != 0)
            {
                gmsGmkSeesawWork.tilt += gmsGmkSeesawWork.tilt_d;
                if (gmsGmkSeesawWork.tilt_se_timer == 0)
                {
                    GmSoundPlaySE("Seesaw");
                    gmsGmkSeesawWork.tilt_se_timer = 8;
                }
                --gmsGmkSeesawWork.tilt_se_timer;
                if (gmsGmkSeesawWork.tilt >= 4608)
                {
                    gmsGmkSeesawWork.tilt = 4608;
                    gmsGmkSeesawWork.tilt_d = 0;
                }
                if (gmsGmkSeesawWork.tilt <= -4608)
                {
                    gmsGmkSeesawWork.tilt = -4608;
                    gmsGmkSeesawWork.tilt_d = 0;
                }
            }
            obj_work.dir.z = (ushort)gmsGmkSeesawWork.tilt;
            seesaw_tilt[gmsGmkSeesawWork.seesaw_id] = (short)obj_work.dir.z;
            if (((int)plyWork.obj_work.move_flag & 4) != 0)
            {
                int num2 = mtMathSin((ushort)gmsGmkSeesawWork.tilt) * 29;
                int num3 = obj_work.pos.x + num2;
                long num4 = (plyWork.obj_work.pos.x - num3 << 12) / (long)mtMathCos(gmsGmkSeesawWork.tilt);
                gmsGmkSeesawWork.player_distance = num4;
                if (((int)plyWork.obj_work.disp_flag & 1) == 0 && gmsGmkSeesawWork.player_speed > 0 || ((int)plyWork.obj_work.disp_flag & 1) != 0 && gmsGmkSeesawWork.player_speed < 0)
                    gmsGmkSeesawWork.player_speed = 0;
            }
            long playerDistance = gmsGmkSeesawWork.player_distance;
            gmsGmkSeesawWork.player_distance += gmsGmkSeesawWork.player_speed;
            long num5 = gmsGmkSeesawWork.player_distance - playerDistance;
            gmsGmkSeesawWork.player_speed += 256 * mtMathSin((ushort)gmsGmkSeesawWork.tilt) >> 12;
            int num6 = mtMathSin((ushort)gmsGmkSeesawWork.tilt) * 29;
            int num7 = obj_work.pos.x + num6;
            int num8 = mtMathCos((ushort)gmsGmkSeesawWork.tilt) * 29;
            int num9 = obj_work.pos.y - num8;
            int num10 = (int)(gmsGmkSeesawWork.player_distance * mtMathCos((ushort)gmsGmkSeesawWork.tilt) >> 12);
            int num11 = num7 + num10;
            int num12 = (int)(gmsGmkSeesawWork.player_distance * mtMathSin((ushort)gmsGmkSeesawWork.tilt) >> 12);
            int num13 = num9 + num12;
            if (gmsGmkSeesawWork.player_speed < 0 && ((int)plyWork.obj_work.disp_flag & 1) == 0)
                plyWork.obj_work.disp_flag |= 1U;
            else if (gmsGmkSeesawWork.player_speed > 0 && ((int)plyWork.obj_work.disp_flag & 1) != 0)
                plyWork.obj_work.disp_flag &= 4294967294U;
            if (gmsGmkSeesawWork.player_distance <= 231424L && gmsGmkSeesawWork.player_distance >= -231424L)
            {
                plyWork.obj_work.spd.x = num11 - plyWork.obj_work.pos.x;
                plyWork.obj_work.spd.y = num13 - plyWork.obj_work.pos.y;
            }
            else
            {
                int num2 = (int)(num5 * mtMathCos((ushort)gmsGmkSeesawWork.tilt) >> 12);
                int spdy = (int)(num5 * mtMathSin((ushort)gmsGmkSeesawWork.tilt) >> 12);
                if (MTM_MATH_ABS(num2) < 256)
                    num2 = num2 >= 0 ? 1024 : -1024;
                GmPlySeqGmkInitSeesawEnd(plyWork, num2, spdy);
                gmsGmkSeesawWork.tilt_d = 0;
                control_right = null;
                lock_seesaw_id = 0;
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSeesawStay);
            }
        }
    }

    private static void gmGmkSeesaw_CreateParts(GMS_GMK_SEESAW_WORK pwork)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)pwork;
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_SEESAWPARTS_WORK(), null, 0, "Gmk_SeesawParts");
        GMS_GMK_SEESAWPARTS_WORK gmkSeesawpartsWork = (GMS_GMK_SEESAWPARTS_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_seesaw_obj_3d_list[1], gmkSeesawpartsWork.eff_work.obj_3d);
        work.parent_obj = obsObjectWork;
        work.flag &= 4294966271U;
        work.pos.x = obsObjectWork.pos.x;
        work.pos.y = obsObjectWork.pos.y;
        work.pos.z = obsObjectWork.pos.z + 4096;
        work.disp_flag |= 4194304U;
        work.disp_flag |= 256U;
        work.flag |= 2U;
        work.ppFunc = null;
    }

    private static GMS_GMK_SEESAW_WORK gmGmkSeesawInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        GMS_GMK_SEESAW_WORK work = (GMS_GMK_SEESAW_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_SEESAW_WORK(), "Gmk_Seesaw");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_seesaw_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -4096;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        work.seesaw_id = (ushort)eve_rec.left;
        gmGmkSeesaw_CreateParts(work);
        return work;
    }

}