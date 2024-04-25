public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSsRingGateInit(
         GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        OBS_OBJECT_WORK work1 = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_SS_RINGGATE");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work1;
        work1.user_work = GmSplStageRingGateNumGet((ushort)eve_rec.left);
        work1.user_flag = eve_rec.flag & 1U;
        work1.user_timer = 20;
        if ((ushort)work1.user_work > g_gm_main_system.ply_work[0].ring_num)
        {
            ObjObjectCopyAction3dNNModel(work1, gm_gmk_ss_ringgate_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            uint num = g_gm_main_system.sync_time % 128U;
            gmsEnemy3DWork.obj_3d.mat_frame = num;
            work1.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsRingGateDrawFunc);
            work1.user_flag = (uint)((int)work1.user_flag & 1 | ((int)num & sbyte.MaxValue) << 8);
        }
        else
            ObjObjectCopyAction3dNNModel(work1, gm_gmk_ss_ringgate_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        work1.pos.z = -131072;
        work1.move_flag |= 8448U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work1.disp_flag |= 4194304U;
        work1.obj_3d.use_light_flag &= 4294967294U;
        work1.obj_3d.use_light_flag |= 2U;
        if ((ushort)work1.user_work > g_gm_main_system.ply_work[0].ring_num)
        {
            OBS_OBJECT_WORK work2 = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DNN_WORK(), work1, 0, "GATERING");
            GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork1 = (GMS_EFFECT_3DNN_WORK)work2;
            ObjObjectCopyAction3dNNModel(work2, gm_gmk_ss_ringgate_obj_3d_list[12], gmsEffect3DnnWork1.obj_3d);
            work2.pos.z = -65536;
            work2.move_flag |= 8448U;
            work2.disp_flag &= 4294967039U;
            work2.user_work = 0U;
            work2.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsRingGateNumMain);
            work2.dir.y = 49152;
            OBS_OBJECT_WORK work3 = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DNN_WORK(), work1, 0, "GATENUM10");
            GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork2 = (GMS_EFFECT_3DNN_WORK)work3;
            work3.user_timer = (int)(work1.user_work / 10U);
            ObjObjectCopyAction3dNNModel(work3, gm_gmk_ss_ringgate_obj_3d_list[2 + work3.user_timer], gmsEffect3DnnWork2.obj_3d);
            work3.pos.z = -65536;
            work3.move_flag |= 8448U;
            work3.disp_flag &= 4294967039U;
            work3.user_work = 1U;
            work3.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsRingGateNumMain);
            work3.dir.y = 49152;
            OBS_OBJECT_WORK work4 = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DNN_WORK(), work1, 0, "GATENUM1");
            GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork3 = (GMS_EFFECT_3DNN_WORK)work4;
            work4.user_timer = (int)(work1.user_work % 10U);
            ObjObjectCopyAction3dNNModel(work4, gm_gmk_ss_ringgate_obj_3d_list[2 + work4.user_timer], gmsEffect3DnnWork3.obj_3d);
            work4.pos.z = -65536;
            work4.move_flag |= 8448U;
            work4.disp_flag &= 4294967039U;
            work4.user_work = 2U;
            work4.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsRingGateNumMain);
            work4.dir.y = 49152;
        }
        work1.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsRingGateMain);
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkSsRingGateDefFunc);
        ObjRectDefSet(pRec, 65534, 0);
        if ((eve_rec.flag & 1) != 0)
        {
            ObjRectWorkSet(pRec, -20, -52, 20, 52);
            work1.dir.z = 16384;
        }
        else
            ObjRectWorkSet(pRec, -52, -20, 52, 20);
        pRec.flag |= 1024U;
        OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work1;
        colWork.obj_col.diff_data = g_gm_default_col;
        if ((eve_rec.flag & 1) != 0)
        {
            colWork.obj_col.width = 24;
            colWork.obj_col.height = 96;
        }
        else
        {
            colWork.obj_col.width = 96;
            colWork.obj_col.height = 24;
        }
        colWork.obj_col.ofst_x = (short)-(colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-(colWork.obj_col.height / 2);
        colWork.obj_col.attr = 2;
        colWork.obj_col.flag |= 134217760U;
        if ((ushort)work1.user_work <= g_gm_main_system.ply_work[0].ring_num)
        {
            gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
            gmsEnemy3DWork.ene_com.col_work.obj_col.obj = null;
            work1.ppFunc = null;
            work1.ppOut = new MPP_VOID_OBS_OBJECT_WORK(ObjDrawActionSummary);
        }
        return work1;
    }

    public static void GmGmkSsRingGateBuild()
    {
        gm_gmk_ss_ringgate_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(917), GmGameDatGetGimmickData(918), 0U);
        gm_gmk_ss_ringgate_obj_tvx_list = GmGameDatGetGimmickData(920);
    }

    public static void GmGmkSsRingGateFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(917);
        GmGameDBuildRegFlushModel(gm_gmk_ss_ringgate_obj_3d_list, gimmickData.file_num);
        gm_gmk_ss_ringgate_obj_tvx_list = null;
    }

    private static void gmGmkSsRingGateMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        uint num = (obj_work.user_flag >> 8 & (uint)sbyte.MaxValue) + 1U;
        obj_work.user_flag = (uint)((int)obj_work.user_flag & 1 | ((int)num & sbyte.MaxValue) << 8);
        if ((ushort)obj_work.user_work > gmsPlayerWork.ring_num)
            return;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = null;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsRingGateVanish);
        obj_work.disp_flag |= 134217728U;
        obj_work.obj_3d.drawflag |= 8388608U;
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        obj_work.user_timer = 20;
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctZoneEsCreate(obj_work, 5, 9);
        GmEffect3DESSetDispOffset(efct_3des, 0.0f, 0.0f, 8f);
        efct_3des.efct_com.obj_work.dir.z = obj_work.dir.z;
        efct_3des.efct_com.obj_work.flag |= 512U;
    }

    private static void gmGmkSsRingGateVanish(OBS_OBJECT_WORK obj_work)
    {
        --obj_work.user_timer;
        if (obj_work.user_timer == 8)
        {
            obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(ObjDrawActionSummary);
            ObjObjectAction3dNNModelReleaseCopy(obj_work);
            ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_ss_ringgate_obj_3d_list[1], ((GMS_ENEMY_3D_WORK)obj_work).obj_3d);
        }
        if (obj_work.user_timer > 4.0)
        {
            obj_work.obj_3d.draw_state.alpha.alpha = obj_work.user_timer / 20f;
        }
        else
        {
            obj_work.obj_3d.draw_state.alpha.alpha = 0.2f;
            obj_work.ppFunc = null;
        }
    }

    private static void gmGmkSsRingGateDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != 1 || parentObj2.gmk_obj == (OBS_OBJECT_WORK)parentObj1))
            return;
        int v1_1 = FX_Mul(parentObj2.obj_work.spd.x, 5120);
        int num1 = FX_Mul(parentObj2.obj_work.spd.y, 5120);
        short objectRotation = (short)GmMainGetObjectRotation();
        if (((int)parentObj1.obj_work.user_flag & 1) != 0)
        {
            if (objectRotation > 0)
                objectRotation -= 16384;
            else
                objectRotation += 16384;
        }
        short num2 = (short)(objectRotation * 2);
        int v1_2 = -num1;
        int num3 = FX_Mul(v1_1, mtMathCos(num2)) + FX_Mul(v1_2, mtMathSin(num2));
        int num4 = FX_Mul(v1_2, mtMathCos(num2)) - FX_Mul(v1_1, mtMathSin(num2));
        parentObj2.obj_work.spd.x = num3;
        parentObj2.obj_work.spd.y = num4;
        parentObj2.obj_work.spd_m = FX_Mul(-parentObj2.obj_work.spd_m, 5120);
        GMM_PAD_VIB_MID_TIME(60f);
        parentObj2.player_flag &= 4294967280U;
        parentObj2.player_flag |= 1U;
    }

    private static void gmGmkSsRingGateNumMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (GMS_EFFECT_3DNN_WORK)obj_work;
        if (parentObj.user_timer < 8)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            int num = MTM_MATH_CLIP((int)(parentObj.user_work - g_gm_main_system.ply_work[0].ring_num), 0, 99);
            if (obj_work.user_work == 1U && num < 10)
            {
                obj_work.disp_flag |= 32U;
            }
            else
            {
                bool flag = false;
                switch (obj_work.user_work)
                {
                    case 1:
                        if (obj_work.user_timer != num / 10)
                        {
                            obj_work.user_timer = num / 10;
                            flag = true;
                            break;
                        }
                        break;
                    case 2:
                        if (obj_work.user_timer != num % 10)
                        {
                            obj_work.user_timer = num % 10;
                            flag = true;
                            break;
                        }
                        break;
                }
                if (flag)
                {
                    ObjAction3dNNMotionRelease(obj_work.obj_3d);
                    ObjObjectAction3dNNModelReleaseCopy(obj_work);
                    ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_ss_ringgate_obj_3d_list[2 + obj_work.user_timer], gmsEffect3DnnWork.obj_3d);
                }
                obj_work.dir.z = GmMainGetObjectRotation();
                obj_work.disp_flag |= parentObj.disp_flag & 134217728U;
                obj_work.obj_3d.drawflag |= parentObj.obj_3d.drawflag & 8388608U;
                obj_work.obj_3d.draw_state.alpha.alpha = parentObj.obj_3d.draw_state.alpha.alpha;
                int x = parentObj.pos.x;
                int y = parentObj.pos.y;
                switch (obj_work.user_work)
                {
                    case 0:
                        x += FX_Mul(-36864, mtMathCos(obj_work.dir.z));
                        y += FX_Mul(-36864, mtMathSin(obj_work.dir.z));
                        break;
                    case 2:
                        x += FX_Mul(36864, mtMathCos(obj_work.dir.z));
                        y += FX_Mul(36864, mtMathSin(obj_work.dir.z));
                        break;
                }
                obj_work.pos.x = x;
                obj_work.pos.y = y;
            }
        }
    }

    private static void gmGmkSsRingGateDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        TVX_FILE model_tvx;
        if (gm_gmk_ss_ringgate_obj_tvx_list.buf[0] == null)
        {
            model_tvx = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_ss_ringgate_obj_tvx_list, 0));
            gm_gmk_ss_ringgate_obj_tvx_list.buf[0] = model_tvx;
        }
        else
            model_tvx = (TVX_FILE)gm_gmk_ss_ringgate_obj_tvx_list.buf[0];
        NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        uint dispLightDisable = GMD_TVX_DISP_LIGHT_DISABLE;
        uint num1 = 0;
        if (obj_work.dir.z != 0)
        {
            dispLightDisable |= GMD_TVX_DISP_ROTATE;
            num1 = obj_work.dir.z;
        }
        GMS_TVX_EX_WORK ex_work = new GMS_TVX_EX_WORK();
        uint num2 = obj_work.user_flag >> 13 & 3U;
        ex_work.u_wrap = 0;
        ex_work.v_wrap = 0;
        ex_work.coord.u = -0.25f * num2;
        ex_work.coord.v = 0.0f;
        ex_work.color = uint.MaxValue;
        GmTvxSetModelEx(model_tvx, texlist, ref obj_work.pos, ref obj_work.scale, dispLightDisable, (short)num1, ref ex_work);
    }
}