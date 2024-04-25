public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkLoopInit(
     GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkLoopLoadObjNoModel(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        gmGmkLoopInit(objWork);
        return objWork;
    }

    private static GMS_ENEMY_3D_WORK gmGmkLoopLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_LOOP");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static void gmGmkLoopInit(OBS_OBJECT_WORK obj_work)
    {
        gmGmkLoopSetRect(obj_work);
        obj_work.move_flag |= 8448U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkLoopMainFunc);
    }

    private static void gmGmkLoopSetRect(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        short cLeft = (short)(-gmsEnemy3DWork.ene_com.eve_rec.width * 64 / 2);
        short cRight = (short)(gmsEnemy3DWork.ene_com.eve_rec.width * 64 / 2);
        short cTop = (short)(-gmsEnemy3DWork.ene_com.eve_rec.height * 64 / 2);
        short cBottom = (short)(gmsEnemy3DWork.ene_com.eve_rec.height * 64 / 2);
        ObjRectWorkZSet(pRec, cLeft, cTop, -500, cRight, cBottom, 500);
        pRec.flag |= 1024U;
        ObjRectDefSet(pRec, 65534, 0);
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkLoopDefFunc);
    }

    private static void gmGmkLoopDefFunc(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        UNREFERENCED_PARAMETER(target_rect);
        own_rect.parent_obj.user_flag |= 1U;
    }

    private static void gmGmkLoopMainFunc(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.user_flag & 1) == 0)
            return;
        gmGmkLoopExecute(obj_work);
        obj_work.user_flag &= 4294967294U;
    }

    private static void gmGmkLoopExecute(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        int loop_x = gmsEnemy3DWork.ene_com.eve_rec.left * 64 * 4096;
        int loop_y = gmsEnemy3DWork.ene_com.eve_rec.top * 64 * 4096;
        gmGmkLoopExecuteObj(loop_x, loop_y, 1);
        gmGmkLoopExecuteObj(loop_x, loop_y, 2);
        gmGmkLoopExecuteEffect(loop_x, loop_y);
        gmGmkLoopExecuteRing(loop_x, loop_y);
        gmGmkLoopExecuteCamera(loop_x, loop_y);
        GmEveMgrCreateEventLcd(0U);
    }

    private static void gmGmkLoopExecuteObj(int loop_x, int loop_y, int obj_type)
    {
        for (OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, (ushort)obj_type); obj_work != null; obj_work = ObjObjectSearchRegistObject(obj_work, (ushort)obj_type))
        {
            obj_work.pos.x += loop_x;
            obj_work.pos.y += loop_y;
        }
    }

    private static void gmGmkLoopExecuteEffect(int loop_x, int loop_y)
    {
        for (OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, 5); obj_work != null; obj_work = ObjObjectSearchRegistObject(obj_work, 5))
        {
            obj_work.pos.x += loop_x;
            obj_work.pos.y += loop_y;
            if (obj_work.obj_3des != null)
                GmEffect3DESSetDuplicateDraw((GMS_EFFECT_3DES_WORK)obj_work, FX_FX32_TO_F32(loop_x), FX_FX32_TO_F32(loop_y), 0.0f);
        }
    }

    private static void gmGmkLoopExecuteRing(int loop_x, int loop_y)
    {
        for (GMS_RING_WORK gmsRingWork = GmRingGetWork().damage_ring_list_start; gmsRingWork != null; gmsRingWork = gmsRingWork.post_ring)
        {
            gmsRingWork.pos.x += loop_x;
            gmsRingWork.pos.y += loop_y;
        }
    }

    private static void gmGmkLoopExecuteCamera(int loop_x, int loop_y)
    {
        OBS_CAMERA obj_camera = ObjCameraGet(0);
        GmCameraPosSet(FX_F32_TO_FX32(obj_camera.pos.x) + loop_x, -FX_F32_TO_FX32(obj_camera.pos.y) + loop_y, FX_F32_TO_FX32(obj_camera.pos.z));
        ObjObjectCameraSet(FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - OBD_LCD_Y / 2), FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - OBD_LCD_Y / 2));
        GmCameraSetClipCamera(obj_camera);
    }

}