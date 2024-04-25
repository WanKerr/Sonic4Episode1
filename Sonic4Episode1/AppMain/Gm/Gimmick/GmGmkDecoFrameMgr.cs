public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkDecoFrameMgrInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        if (eve_rec.byte_param[1] != 0)
        {
            eve_rec.pos_x = byte.MaxValue;
            return null;
        }
        if ((eve_rec.flag & 1) != 0)
        {
            int index = 0;
            if (eve_rec.id == 293)
                index = 1;
            GmDecoSetFrameMotion(0, index);
            eve_rec.pos_x = byte.MaxValue;
            eve_rec.byte_param[1] = 1;
            return null;
        }
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)gmGmkDecoFrameMgrLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        gmGmkDecoFrameMgrInit(obj_work);
        return obj_work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkDecoFrameMgrLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_DECO_FRAME_MGR");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static void gmGmkDecoFrameMgrInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.disp_flag |= 32U;
        obj_work.move_flag |= 8448U;
        obj_work.flag |= 16U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDecoFrameMgrMainFunc);
        obj_work.ppOut = null;
        obj_work.ppMove = null;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.user_timer = gmsEnemy3DWork.ene_com.eve_rec.byte_param[1] * 2;
    }

    private static void gmGmkDecoFrameMgrMainFunc(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer >= 510)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            ++obj_work.user_timer;
            int index = 0;
            GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
            if (gmsEnemy3DWork.ene_com.eve_rec.id == 293)
                index = 1;
            GmDecoSetFrameMotion(obj_work.user_timer, index);
            gmsEnemy3DWork.ene_com.eve_rec.byte_param[1] = (byte)(obj_work.user_timer / 2);
        }
    }

}