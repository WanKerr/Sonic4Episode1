public partial class AppMain
{
    private static void gmGmkWaterSliderEffectDestFunc(MTS_TASK_TCB tcb)
    {
        if (g_gm_gmk_water_slider_se_handle != null)
        {
            GmSoundStopSE(g_gm_gmk_water_slider_se_handle);
            GsSoundFreeSeHandle(g_gm_gmk_water_slider_se_handle);
            g_gm_gmk_water_slider_se_handle = null;
        }
        GMM_PAD_VIB_STOP();
        GmEffectDefaultExit(tcb);
        g_gm_gmk_water_slider_effct_player = null;
    }

    private static OBS_OBJECT_WORK GmGmkWaterSliderCreateEffect()
    {
        if (g_gm_gmk_water_slider_effct_player == null)
        {
            g_gm_gmk_water_slider_effct_player = GmEfctZoneEsCreate(g_gm_main_system.ply_work[0].obj_work, 2, 23);
            OBS_OBJECT_WORK sliderEffctPlayer = (OBS_OBJECT_WORK)g_gm_gmk_water_slider_effct_player;
            sliderEffctPlayer.parent_ofst.z = 131072;
            sliderEffctPlayer.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkWaterSliderEffectMainFunc);
            mtTaskChangeTcbDestructor(sliderEffctPlayer.tcb, new GSF_TASK_PROCEDURE(gmGmkWaterSliderEffectDestFunc));
        }
        if (g_gm_gmk_water_slider_se_handle == null)
        {
            g_gm_gmk_water_slider_se_handle = GsSoundAllocSeHandle();
            GmSoundPlaySE("WaterSlider", g_gm_gmk_water_slider_se_handle);
        }
        GMM_PAD_VIB_SMALL_TIME(30f);
        return (OBS_OBJECT_WORK)g_gm_gmk_water_slider_effct_player;
    }

    private static void gmGmkWaterSliderEffectMainFunc(OBS_OBJECT_WORK obj_work)
    {
        if (g_gm_main_system.ply_work[0].seq_state != 36)
            GmGmkWaterSliderDeleteEffect();
        else
            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void GmGmkWaterSliderDeleteEffect()
    {
        if (g_gm_gmk_water_slider_effct_player == null)
            return;
        g_gm_gmk_water_slider_effct_player.efct_com.obj_work.flag |= 8U;
        g_gm_gmk_water_slider_effct_player = null;
    }

    private static OBS_OBJECT_WORK GmGmkWaterSliderInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        uint num;
        switch (eve_rec.id)
        {
            case 116:
                num = 1U;
                break;
            case 117:
                num = 2U;
                break;
            case 118:
                num = 3U;
                break;
            case 120:
                num = 5U;
                break;
            case 121:
                num = 6U;
                break;
            case 122:
                num = 7U;
                break;
            default:
                return null;
        }
        OBS_OBJECT_WORK objWork = gmGmkWaterSliderLoadObj(eve_rec, pos_x, pos_y, num).ene_com.obj_work;
        gmGmkWaterSliderInit(objWork, num);
        return objWork;
    }

    public static void GmGmkWaterSliderBuild()
    {
        g_gm_gmk_water_slider_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(832), GmGameDatGetGimmickData(833), 0U);
    }

    public static void GmGmkWaterSliderFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(832);
        GmGameDBuildRegFlushModel(g_gm_gmk_water_slider_obj_3d_list, gimmickData.file_num);
        g_gm_gmk_water_slider_obj_3d_list = null;
    }

    public static OBS_ACTION3D_NN_WORK[] GmGmkWaterSliderGetObj3DList()
    {
        return g_gm_gmk_water_slider_obj_3d_list;
    }

    private static uint gmGmkWaterSlidereGameSystemGetSyncTime()
    {
        return g_gm_main_system.sync_time;
    }

    private static GMS_ENEMY_3D_WORK gmGmkWaterSliderLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      uint type)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_WATER_SLIDER_WORK(), "GMK_WATER_SLIDER");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkWaterSliderLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      uint type)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkWaterSliderLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index1 = g_gm_gmk_water_slider_model_id_main[(int)type];
        ObjObjectCopyAction3dNNModel(objWork, g_gm_gmk_water_slider_obj_3d_list[index1], gmsEnemy3DWork.obj_3d);
        int index2 = g_gm_gmk_water_slider_material_id_main[(int)type];
        object pData1 = ObjDataGet(835).pData;
        ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, null, null, index2, (AMS_AMB_HEADER)pData1);
        GMS_GMK_WATER_SLIDER_WORK gmkWaterSliderWork = (GMS_GMK_WATER_SLIDER_WORK)objWork;
        int index3 = g_gm_gmk_water_slider_model_id_sub[(int)type];
        if (index3 != -1)
            ObjCopyAction3dNNModel(g_gm_gmk_water_slider_obj_3d_list[index3], gmkWaterSliderWork.obj_3d_parts);
        gmkWaterSliderWork.obj_3d_parts.drawflag |= 32U;
        int index4 = g_gm_gmk_water_slider_motion_id_sub[(int)type];
        if (index4 != -1)
        {
            object pData2 = ObjDataGet(834).pData;
            ObjAction3dNNMotionLoad(gmkWaterSliderWork.obj_3d_parts, 0, false, null, null, index4, (AMS_AMB_HEADER)pData2);
        }
        int index5 = g_gm_gmk_water_slider_material_id_sub[(int)type];
        if (index5 != -1)
            ObjAction3dNNMaterialMotionLoad(gmkWaterSliderWork.obj_3d_parts, 0, null, null, index5, (AMS_AMB_HEADER)pData1);
        objWork.disp_flag |= 268435456U;
        gmkWaterSliderWork.obj_3d_parts.command_state = 17U;
        return gmsEnemy3DWork;
    }

    private static bool gmGmkWaterSliderCheckHFlip(uint type)
    {
        switch (type)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return false;
            case 4:
            case 5:
            case 6:
            case 7:
                return true;
            default:
                return false;
        }
    }

    private static void gmGmkWaterSliderInit(OBS_OBJECT_WORK obj_work, uint slider_type)
    {
        gmGmkWaterSliderSetRect((GMS_ENEMY_3D_WORK)obj_work, slider_type);
        gmGmkWaterSliderSetUserWorkSlideType(obj_work, slider_type);
        obj_work.move_flag = 8448U;
        int speed = -61440;
        obj_work.dir.y = 49152;
        if (gmGmkWaterSliderCheckHFlip(slider_type))
        {
            obj_work.disp_flag |= 1U;
            speed = -speed;
        }
        gmGmkWaterSliderSetUserTimerSlideSpeed(obj_work, speed);
        obj_work.obj_3d.drawflag |= 32U;
        obj_work.pos.z = 131072;
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag |= 20U;
        obj_work.ppFunc = null;
        obj_work.ppMove = null;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkWaterSliderDrawFunc);
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkWaterSliderDestFunc));
    }

    private static void gmGmkWaterSliderSetRect(
      GMS_ENEMY_3D_WORK gimmick_work,
      uint slider_type)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        short num1 = 0;
        short num2 = 0;
        short num3 = 0;
        switch (slider_type)
        {
            case 1:
            case 5:
                num1 = -64;
                num3 = 32;
                break;
            case 2:
            case 6:
                num1 = -64;
                num3 = 64;
                break;
            case 3:
            case 7:
                num1 = -64;
                num3 = 128;
                break;
        }
        ObjRectWorkZSet(pRec, (short)(num1 - 8), -8, -500, (short)(num2 + 8), (short)(num3 + 8), 500);
        ObjRectDefSet(pRec, 65534, 0);
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkWaterSliderDefFunc);
    }

    private static void gmGmkWaterSliderDestFunc(MTS_TASK_TCB tcb)
    {
        ObjAction3dNNMotionRelease(((GMS_GMK_WATER_SLIDER_WORK)mtTaskGetTcbWork(tcb)).obj_3d_parts);
        GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkWaterSliderDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (obj3d.motion != null)
        {
            float startFrame = amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
            float num = amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - startFrame;
            float syncTime = gmGmkWaterSlidereGameSystemGetSyncTime();
            obj3d.mat_frame = syncTime % num;
        }
        ObjDrawActionSummary(obj_work);
        uint p_disp_flag = (obj_work.disp_flag | 4U) & 4294967279U;
        if (ObjObjectPauseCheck(0U) != 0U)
            p_disp_flag |= 4096U;
        GMS_GMK_WATER_SLIDER_WORK gmkWaterSliderWork = (GMS_GMK_WATER_SLIDER_WORK)obj_work;
        VecFx32 pos = obj_work.pos;
        pos.z += 131072;
        ObjDrawAction3DNN(gmkWaterSliderWork.obj_3d_parts, new VecFx32?(pos), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref p_disp_flag);
    }

    private static void gmGmkWaterSliderDefFunc(
      OBS_RECT_WORK gimmick_rect,
      OBS_RECT_WORK player_rect)
    {
        OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)parentObj1;
        OBS_OBJECT_WORK parentObj2 = player_rect.parent_obj;
        if (parentObj2.obj_type != 1)
            return;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)parentObj2;
        if (ply_work.seq_state == 36)
        {
            ply_work.gmk_obj = parentObj1;
        }
        else
        {
            if (((int)parentObj2.move_flag & 1) == 0)
                return;
            if (gmGmkWaterSliderCheckHFlip(gmGmkWaterSliderGetUserWorkSlideType(parentObj1)))
                parentObj2.disp_flag &= 4294967294U;
            else
                parentObj2.disp_flag |= 1U;
            parentObj2.spd_m = gmGmkWaterSliderGetUserTimerSlideSpeed(parentObj1);
            parentObj2.spd.x = 0;
            parentObj2.spd.y = 0;
            parentObj2.spd_add.x = 0;
            parentObj2.spd_add.y = 0;
            GmPlySeqInitWaterSlider(ply_work, gmsEnemy3DWork.ene_com);
            gmsEnemy3DWork.ene_com.target_obj = parentObj2;
            gimmick_rect.flag |= 1024U;
            parentObj1.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkWaterSliderMainActive);
            GmGmkWaterSliderCreateEffect();
            nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
            int ay = -6144;
            if (((int)parentObj2.disp_flag & 1) != 0)
                ay = -ay;
            nnRotateYMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, ay);
            ply_work.ex_obj_mtx_r.M13 = -2f;
            ply_work.gmk_flag |= 32768U;
        }
    }

    private static void gmGmkWaterSliderMainActive(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_PLAYER_WORK targetObj = (GMS_PLAYER_WORK)gmsEnemy3DWork.ene_com.target_obj;
        OBS_RECT_WORK obsRectWork = gmsEnemy3DWork.ene_com.rect_work[2];
        if (targetObj.seq_state == 36)
            return;
        gmsEnemy3DWork.ene_com.target_obj = null;
        obsRectWork.flag &= 4294966271U;
        obj_work.ppFunc = null;
    }

    private static void gmGmkWaterSliderSetUserWorkSlideType(
      OBS_OBJECT_WORK obj_work,
      uint type)
    {
        obj_work.user_work = type;
    }

    private static uint gmGmkWaterSliderGetUserWorkSlideType(OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_work;
    }

    private static void gmGmkWaterSliderSetUserTimerSlideSpeed(
      OBS_OBJECT_WORK obj_work,
      int speed)
    {
        obj_work.user_timer = speed;
    }

    private static int gmGmkWaterSliderGetUserTimerSlideSpeed(OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_timer;
    }

}