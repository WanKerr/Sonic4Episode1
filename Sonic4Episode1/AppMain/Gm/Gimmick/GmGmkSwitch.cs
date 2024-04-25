public partial class AppMain
{
    private static void GmGmkSwitchBuildTypeZone3()
    {
        gm_gmk_switch_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(931), GmGameDatGetGimmickData(932), 0U);
        ClearArray(gm_gmk_switch_state);
    }

    private static void GmGmkSwitchBuildTypeZone4()
    {
        gm_gmk_switch_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(931), GmGameDatGetGimmickData(932), 0U);
        ClearArray(gm_gmk_switch_state);
    }

    private static void GmGmkSwitchReBuild()
    {
        ClearArray(gm_gmk_switch_state);
    }

    private static void GmGmkSwitchFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(931);
        GmGameDBuildRegFlushModel(gm_gmk_switch_obj_3d_list, gimmickData.file_num);
    }

    private static OBS_OBJECT_WORK GmGmkSwitchInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_SW_WORK(), "GMK_SWITCH");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_GMK_SW_WORK gmsGmkSwWork = (GMS_GMK_SW_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_switch_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        if (GMM_MAIN_GET_ZONE_TYPE() == 2)
        {
            ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, ObjDataGet(933), null, 0, null, 1, 1);
            ObjDrawAction3dActionSet3DNNMaterial(gmsEnemy3DWork.obj_3d, 0);
            work.disp_flag |= 4U;
        }
        ObjCopyAction3dNNModel(gm_gmk_switch_obj_3d_list[0], gmsGmkSwWork.obj_3d_base);
        work.pos.z = -262144;
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSwDispFunc);
        OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work;
        colWork.obj_col.width = 32;
        colWork.obj_col.height = 24;
        colWork.obj_col.ofst_x = -16;
        colWork.obj_col.ofst_y = -14;
        if (g_gs_main_sys_info.stage_id == 9)
        {
            colWork.obj_col.obj = null;
            OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
            pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkSwitchDefFunc);
            pRec.ppHit = null;
            ObjRectAtkSet(pRec, 0, 0);
            ObjRectDefSet(pRec, 65534, 0);
            ObjRectWorkSet(pRec, -16, -20, 16, -4);
            pRec.flag |= 132U;
        }
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        gmsGmkSwWork.id = (uint)MTM_MATH_CLIP(eve_rec.left, 0, 64);
        gmsGmkSwWork.time = eve_rec.width * 60 * 4096 + eve_rec.top * 4096;
        if (gmsGmkSwWork.time != 0 && gmsGmkSwWork.time < 12288)
            gmsGmkSwWork.time = 12288;
        if (gm_gmk_switch_state[(int)gmsGmkSwWork.id].sw)
        {
            gmsGmkSwWork.top_pos_y = -10;
            gmGmkSwOnInit(work, false);
        }
        else
        {
            gmsGmkSwWork.top_pos_y = -14;
            gmGmkSwOffInit(work);
        }
        gmGmkSwSetCol(gmsGmkSwWork.gmk_work.ene_com.col_work, gmsGmkSwWork.top_pos_y);
        return work;
    }

    private static bool GmGmkSwitchIsOn(uint sw_id)
    {
        return gm_gmk_switch_state[(int)sw_id].sw;
    }

    private static bool GmGmkSwitchTypeIsGear(uint sw_id)
    {
        return gm_gmk_switch_state[(int)sw_id].gear;
    }

    private static void GmGmkSwitchSetOnGearSwitch(uint sw_id, int per)
    {
        gm_gmk_switch_state[(int)sw_id].sw = true;
        gm_gmk_switch_state[(int)sw_id].gear = true;
        gm_gmk_switch_state[(int)sw_id].per = per;
    }

    private static void GmGmkSwitchSetOffGearSwitch(uint sw_id, int per)
    {
        gm_gmk_switch_state[(int)sw_id].sw = false;
        gm_gmk_switch_state[(int)sw_id].gear = true;
        gm_gmk_switch_state[(int)sw_id].per = per;
    }

    private static int GmGmkSwitchGetPer(uint sw_id)
    {
        return gm_gmk_switch_state[(int)sw_id].per;
    }

    private static void gmGmkSwOffInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SW_WORK gmsGmkSwWork = (GMS_GMK_SW_WORK)obj_work;
        gm_gmk_switch_state[(int)gmsGmkSwWork.id].sw = false;
        gm_gmk_switch_state[(int)gmsGmkSwWork.id].time = 0;
        obj_work.flag &= 4294967279U;
        if (gmsGmkSwWork.top_pos_y < -14)
            gmsGmkSwWork.top_pos_y = -14;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSwOffMain);
    }

    private static void gmGmkSwOffMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SW_WORK gmsGmkSwWork = (GMS_GMK_SW_WORK)obj_work;
        if (gmsGmkSwWork.top_pos_y > -14)
        {
            gmsGmkSwWork.top_pos_y += -2;
            if (gmsGmkSwWork.top_pos_y < -14)
                gmsGmkSwWork.top_pos_y = -14;
            gmGmkSwSetCol(gmsGmkSwWork.gmk_work.ene_com.col_work, gmsGmkSwWork.top_pos_y);
        }
        if (gmsGmkSwWork.gmk_work.ene_com.col_work.obj_col.rider_obj != null && gmsGmkSwWork.gmk_work.ene_com.col_work.obj_col.rider_obj.obj_type == 1 || ((int)gmsGmkSwWork.gmk_work.ene_com.enemy_flag & 1) != 0)
            gmGmkSwOnInit(obj_work, true);
        gmsGmkSwWork.gmk_work.ene_com.enemy_flag &= 4294967294U;
    }

    private static void gmGmkSwOnInit(OBS_OBJECT_WORK obj_work, bool now_on)
    {
        GMS_GMK_SW_WORK gmsGmkSwWork = (GMS_GMK_SW_WORK)obj_work;
        gm_gmk_switch_state[(int)gmsGmkSwWork.id].sw = true;
        gm_gmk_switch_state[(int)gmsGmkSwWork.id].time = gmsGmkSwWork.time;
        if (gmsGmkSwWork.time != 0)
            obj_work.flag |= 16U;
        if (gmsGmkSwWork.top_pos_y > -10)
            gmsGmkSwWork.top_pos_y = -10;
        if (now_on)
        {
            GmSoundPlaySE("Switch");
            GMM_PAD_VIB_SMALL();
            GmComEfctCreateSpring(obj_work, 0, short.MinValue, -obj_work.pos.z);
        }
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSwOnMain);
    }

    private static void gmGmkSwOnMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SW_WORK gmsGmkSwWork = (GMS_GMK_SW_WORK)obj_work;
        if (gmsGmkSwWork.top_pos_y < -10)
        {
            gmsGmkSwWork.top_pos_y += 2;
            if (gmsGmkSwWork.top_pos_y > -10)
                gmsGmkSwWork.top_pos_y = -10;
            gmGmkSwSetCol(gmsGmkSwWork.gmk_work.ene_com.col_work, gmsGmkSwWork.top_pos_y);
        }
        gmsGmkSwWork.gmk_work.ene_com.enemy_flag &= 4294967294U;
        if (gmsGmkSwWork.gmk_work.ene_com.col_work.obj_col.rider_obj != null && gmsGmkSwWork.gmk_work.ene_com.col_work.obj_col.rider_obj.obj_type == 1 || ((int)gmsGmkSwWork.gmk_work.ene_com.enemy_flag & 1) != 0)
            gm_gmk_switch_state[(int)gmsGmkSwWork.id].time = gmsGmkSwWork.time;
        else if (gm_gmk_switch_state[(int)gmsGmkSwWork.id].time != 0)
        {
            gm_gmk_switch_state[(int)gmsGmkSwWork.id].time = ObjTimeCountDown(gm_gmk_switch_state[(int)gmsGmkSwWork.id].time);
            if (gm_gmk_switch_state[(int)gmsGmkSwWork.id].time == 0)
                gmGmkSwOffInit(obj_work);
        }
        gmsGmkSwWork.gmk_work.ene_com.enemy_flag &= 4294967294U;
    }

    private static void gmGmkSwitchDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != 1)
            return;
        parentObj1.enemy_flag |= 1U;
    }

    private static void gmGmkSwDispFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SW_WORK gmsGmkSwWork = (GMS_GMK_SW_WORK)obj_work;
        VecFx32 vecFx32 = new VecFx32();
        uint dispFlag = obj_work.disp_flag;
        vecFx32.Assign(obj_work.pos);
        vecFx32.y += gmsGmkSwWork.top_pos_y << 12;
        ObjDrawAction3DNN(obj_work.obj_3d, new VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref obj_work.disp_flag);
        ObjDrawAction3DNN(gmsGmkSwWork.obj_3d_base, new VecFx32?(obj_work.pos), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref dispFlag);
    }

    private static void gmGmkSwSetCol(OBS_COLLISION_WORK col_work, int top_pos_y)
    {
        if (g_gs_main_sys_info.stage_id == 9)
            return;
        col_work.obj_col.ofst_y = (short)top_pos_y;
    }

}