public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkCannonInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_CANNON_WORK work = (GMS_GMK_CANNON_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_CANNON_WORK(), "Gmk_Cannon");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_cannon_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = 131072;
        obj_work.pos.y -= 18432;
        obj_work.dir.y = 32768;
        obj_work.move_flag |= 256U;
        work.cannon_power = eve_rec.width == 0 ? 61440 : eve_rec.width;
        gmGmkCannon_CreateParts(work);
        gmGmkCannonStart(obj_work);
        return obj_work;
    }

    private static void gmGmkCannonFieldColOn(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_CANNON_WORK gmsGmkCannonWork = (GMS_GMK_CANNON_WORK)obj_work;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.diff_data = g_gm_default_col;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.width = 24;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.height = 56;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.ofst_x = -12;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.ofst_y = -30;
        gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.flag |= 134217824U;
    }

    private static void gmGmkCannonFieldColOff(OBS_OBJECT_WORK obj_work)
    {
        ((GMS_GMK_CANNON_WORK)obj_work).gmk_work.ene_com.col_work.obj_col.obj = null;
    }

    private static void gmGmkCannon_CannonTurn(GMS_GMK_CANNON_WORK pwork)
    {
        if (pwork.angle_set > pwork.angle_now)
        {
            pwork.angle_now += 342;
            if (pwork.angle_now <= pwork.angle_set)
                return;
            pwork.angle_now = pwork.angle_set;
        }
        else
        {
            pwork.angle_now -= 342;
            if (pwork.angle_now >= pwork.angle_set)
                return;
            pwork.angle_now = pwork.angle_set;
        }
    }

    private static void gmGmkCannonStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_CANNON_WORK gmsGmkCannonWork = (GMS_GMK_CANNON_WORK)obj_work;
        gmGmkCannonFieldColOn(obj_work);
        gmsGmkCannonWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsGmkCannonWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsGmkCannonWork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkCannonHit);
        pRec.ppHit = null;
        ObjRectAtkSet(pRec, 0, 0);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectWorkSet(pRec, -12, -38, 12, -6);
        obj_work.flag &= 4294967293U;
        gmsGmkCannonWork.ply_work = null;
        gmsGmkCannonWork.angle_set = 0;
        gmsGmkCannonWork.angle_now = 0;
        gmsGmkCannonWork.gmk_work.ene_com.enemy_flag &= 4294934527U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCannonStay);
    }

    private static void gmGmkCannonStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_CANNON_WORK gmsGmkCannonWork = (GMS_GMK_CANNON_WORK)obj_work;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (gmsGmkCannonWork.gmk_work.ene_com.col_work.obj_col.obj != null)
        {
            if (gmsPlayerWork.act_state == 31)
                gmGmkCannonFieldColOff(obj_work);
        }
        else if (gmsPlayerWork.act_state != 31)
            gmGmkCannonFieldColOn(obj_work);
        if (gmsGmkCannonWork.ply_work == null)
            return;
        gmsGmkCannonWork.gmk_work.ene_com.rect_work[2].flag &= 4294967291U;
        if (gmsPlayerWork.seq_state != GME_PLY_SEQ_STATE_GMK_CANNON)
        {
            gmGmkCannonStart(obj_work);
        }
        else
        {
            if (obj_work.pos.y > gmsGmkCannonWork.ply_work.obj_work.pos.y)
                return;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCannonReady);
        }
    }

    private static short gmGmkCannon_GetAngle(ushort key)
    {
        if ((key & 8) != 0)
            return 2730;
        return (key & 4) != 0 ? (short)-2730 : (short)0;
    }

    private static void gmGmkCannonReady(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_CANNON_WORK pwork = (GMS_GMK_CANNON_WORK)obj_work;
        short angleSet = pwork.angle_set;
        short angleNow = pwork.angle_now;
        if (pwork.angle_set == pwork.angle_now)
        {
            if (((int)g_gs_main_sys_info.game_flag & 1) == 0)
            {
                int num1 = (int)(_am_iphone_accel_data.sensor.x * 16384.0) * 3;
                if (num1 > 32768)
                    num1 = 32768;
                else if (num1 < short.MinValue)
                    num1 = short.MinValue;
                int num2 = num1 / 2;
                int num3 = num2 < pwork.angle_now + 2730 ? (pwork.angle_now != 13650 || num2 < 16380 ? (num2 > pwork.angle_now - 2730 ? (pwork.angle_now != -13650 || num2 > -16380 ? pwork.angle_now : -16380) : pwork.angle_now - 2730) : 16380) : pwork.angle_now + 2730;
                pwork.angle_set = (short)num3;
            }
            else
            {
                pwork.angle_set += gmGmkCannon_GetAngle(pwork.ply_work.key_on);
                if (pwork.angle_set > 16380 && (ushort)pwork.angle_set < 49156)
                    pwork.angle_set = 16380;
                if (pwork.angle_set < -16380 && (ushort)pwork.angle_set > 16380)
                    pwork.angle_set = -16380;
            }
            if (angleSet != pwork.angle_set)
                GmSoundPlaySE("Cannon1");
        }
        if (pwork.angle_set != pwork.angle_now)
        {
            gmGmkCannon_CannonTurn(pwork);
            obj_work.dir.z = (ushort)pwork.angle_now;
        }
        if (pwork.angle_set != pwork.angle_now || angleNow != pwork.angle_now || !GmPlayerKeyCheckJumpKeyPush(pwork.ply_work))
            return;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(pwork.ply_work.obj_work, 20);
        gmsEffect3DesWork.efct_com.obj_work.dir.z = obj_work.dir.z;
        gmsEffect3DesWork.efct_com.obj_work.pos.x += mtMathSin(obj_work.dir.z) * 32;
        gmsEffect3DesWork.efct_com.obj_work.pos.y -= mtMathCos(obj_work.dir.z) * 32;
        GmPlySeqInitCannonShoot(pwork.ply_work, mtMathCos(obj_work.dir.z - 16384) * pwork.cannon_power, mtMathSin(obj_work.dir.z - 16384) * pwork.cannon_power);
        gmGmkCannonFieldColOff(obj_work);
        pwork.gmk_work.ene_com.enemy_flag |= 32768U;
        pwork.shoot_after = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCannonShoot);
        gmGmkCannonShoot(obj_work);
        GmSoundPlaySE("Cannon2");
        GMM_PAD_VIB_SMALL();
    }

    private static void gmGmkCannonShoot(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_CANNON_WORK gmsGmkCannonWork = (GMS_GMK_CANNON_WORK)obj_work;
        ++gmsGmkCannonWork.shoot_after;
        if (gmsGmkCannonWork.shoot_after == 16)
        {
            gmGmkCannonFieldColOn(obj_work);
            if (gmsGmkCannonWork.angle_now == 0)
            {
                gmsGmkCannonWork.ply_work = null;
                gmGmkCannonStart(obj_work);
                return;
            }
        }
        if (gmsGmkCannonWork.shoot_after <= 32)
            return;
        gmsGmkCannonWork.ply_work = null;
        gmsGmkCannonWork.angle_set = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCannonShootEnd);
    }

    private static void gmGmkCannonShootEnd(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_CANNON_WORK pwork = (GMS_GMK_CANNON_WORK)obj_work;
        gmGmkCannon_CannonTurn(pwork);
        obj_work.dir.z = (ushort)pwork.angle_now;
        if (pwork.angle_now != pwork.angle_set)
            return;
        gmGmkCannonStart(obj_work);
    }

    private static void gmGmkCannonHit(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        GMS_GMK_CANNON_WORK gmsGmkCannonWork = (GMS_GMK_CANNON_WORK)parentObj1;
        gmsGmkCannonWork.hitpass = false;
        if (parentObj2 == g_gm_main_system.ply_work[0])
        {
            if (gmsGmkCannonWork.ply_work != parentObj2)
            {
                short num1 = (short)((parentObj1.pos.y >> 12) + mine_rect.rect.top);
                if (parentObj2.obj_work.pos.y >> 12 < num1 && parentObj2.obj_work.move.y >= 0 || parentObj2.act_state == 31)
                {
                    short num2 = 1;
                    short num3 = (short)(MTM_MATH_ABS(mine_rect.rect.left - match_rect.rect.left) + MTM_MATH_ABS(mine_rect.rect.right - match_rect.rect.right));
                    short num4 = (short)MTM_MATH_ABS(parentObj2.obj_work.move.x >> 12);
                    if (num4 != 0)
                        num2 = (short)(num4 / num3 + 1);
                    if (parentObj2.obj_work.move.x < 0)
                        num3 = (short)-num3;
                    short num5 = (short)((parentObj1.pos.x >> 12) + mine_rect.rect.left - match_rect.rect.left);
                    short num6 = (short)((parentObj1.pos.x >> 12) + mine_rect.rect.right - match_rect.rect.right);
                    short num7 = (short)(parentObj2.obj_work.pos.x >> 12);
                    for (; num2 != 0; --num2)
                    {
                        if (num7 >= num5 && num7 <= num6)
                        {
                            gmsGmkCannonWork.ply_work = parentObj2;
                            GmPlySeqInitCannon(parentObj2, (GMS_ENEMY_COM_WORK)parentObj1);
                            GmSoundPlaySE("Cannon3");
                            break;
                        }
                        num7 += num3;
                    }
                }
            }
            gmsGmkCannonWork.hitpass = true;
        }
        mine_rect.flag &= 4294573823U;
    }

    private static void gmGmkCannon_CreateParts(GMS_GMK_CANNON_WORK pwork)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)pwork;
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_CANNONPARTS_WORK(), null, 0, "Gmk_CannonBase");
        GMS_GMK_CANNONPARTS_WORK gmkCannonpartsWork = (GMS_GMK_CANNONPARTS_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_cannon_obj_3d_list[1], gmkCannonpartsWork.eff_work.obj_3d);
        work.parent_obj = obsObjectWork;
        work.flag &= 4294966271U;
        work.pos.x = obsObjectWork.pos.x;
        work.pos.y = obsObjectWork.pos.y + 122880;
        work.pos.z = obsObjectWork.pos.z + 122880;
        work.dir.y = obsObjectWork.dir.y;
        work.move_flag |= 256U;
        work.disp_flag &= 4294967039U;
        work.flag |= 2U;
        work.ppFunc = null;
    }

    private static void GmGmkCannonBuild()
    {
        gm_gmk_cannon_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(858), GmGameDatGetGimmickData(859), 0U);
    }

    private static void GmGmkCannonFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(858);
        GmGameDBuildRegFlushModel(gm_gmk_cannon_obj_3d_list, gimmickData.file_num);
    }

}