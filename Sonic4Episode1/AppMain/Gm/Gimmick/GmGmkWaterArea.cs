public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkWaterAreaInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort water_level = (ushort)((uint)eve_rec.left * 100U + (uint)eve_rec.top);
        ushort time = 0;
        ushort flag = eve_rec.flag;
        for (ushort index = 0; 10 > index; ++index)
        {
            if ((flag & 1) != 0)
                time += (ushort)(index + 1U);
            flag >>= 1;
        }
        if (gmGmkWaterAreaGetType(eve_rec) == 0U)
        {
            if (gmGmkWaterAreaCheckRestart(pos_x, pos_y))
                GmWaterSurfaceRequestChangeWaterLevel(water_level, (ushort)(time * 60U), false);
            eve_rec.pos_x = byte.MaxValue;
            return null;
        }
        GMS_ENEMY_3D_WORK gimmick_work = gmGmkWaterAreaLoadObj(eve_rec, pos_x, pos_y, type);
        gmGmkWaterAreaInit(gimmick_work, water_level, time);
        return (OBS_OBJECT_WORK)gimmick_work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkWaterAreaLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_WATER_AREA");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static void gmGmkWaterAreaInit(
      GMS_ENEMY_3D_WORK gimmick_work,
      ushort water_level,
      ushort time)
    {
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)gimmick_work;
        GMS_EVE_RECORD_EVENT eveRec = gimmick_work.ene_com.eve_rec;
        uint type = gmGmkWaterAreaGetType(eveRec);
        byte width = eveRec.width;
        byte height = eveRec.height;
        gmGmkWaterAreaSetRect(gimmick_work, width, height, type);
        gimmick_work.ene_com.target_obj = g_gm_main_system.ply_work[0].obj_work;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 32U;
        gmGmkWaterAreaUserWorkSetLevel(obj_work, water_level);
        gmGmkWaterAreaUserWorkSetTime(obj_work, time);
    }

    private static bool gmGmkWaterAreaCheckRestart(int pos_x, int pos_y)
    {
        int num = MTM_MATH_ABS(g_gm_main_system.resume_pos_x - pos_x);
        return MTM_MATH_ABS(g_gm_main_system.resume_pos_y - pos_y) <= 524288 && num <= 524288;
    }

    private static uint gmGmkWaterAreaGetType(GMS_EVE_RECORD_EVENT eve_rec)
    {
        uint num = uint.MaxValue;
        switch (eve_rec.id)
        {
            case 102:
                num = 1U;
                break;
            case 103:
                num = 2U;
                break;
            case 104:
                num = 3U;
                break;
            case 105:
                num = 4U;
                break;
            case 106:
                num = 0U;
                break;
        }
        return num;
    }

    private static void gmGmkWaterAreaRequestChangeWatarLevel(OBS_OBJECT_WORK obj_work)
    {
        GmWaterSurfaceRequestChangeWaterLevel(gmGmkWaterAreaUserWorkGetLevel(obj_work), (ushort)(gmGmkWaterAreaUserWorkGetTime(obj_work) * 60U), false);
    }

    private static void gmGmkWaterAreaSetRect(
      GMS_ENEMY_3D_WORK gimmick_work,
      byte width,
      byte height,
      uint type)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        if (width < 34)
            width = 34;
        if (height < 34)
            height = 34;
        ObjRectWorkZSet(pRec, (short)(-width / 2), (short)(-height / 2), -500, (short)(width / 2), (short)(height / 2), 500);
        ObjRectAtkSet(pRec, 0, 0);
        pRec.ppHit = null;
        ObjRectDefSet(pRec, 0, 0);
        switch (type)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                pRec.flag |= 1024U;
                pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkWaterAreaDefFuncDelay);
                break;
        }
    }

    private static bool gmGmkWaterAreaCheckDir(
      OBS_OBJECT_WORK gimmick_obj_work,
      OBS_OBJECT_WORK player_obj_work,
      uint type)
    {
        bool flag = false;
        switch (type)
        {
            case 0:
                flag = true;
                break;
            case 1:
                if (player_obj_work.pos.x < gimmick_obj_work.pos.x)
                {
                    flag = true;
                    break;
                }
                break;
            case 2:
                if (gimmick_obj_work.pos.x < player_obj_work.pos.x)
                {
                    flag = true;
                    break;
                }
                break;
            case 3:
                if (player_obj_work.pos.y < gimmick_obj_work.pos.y)
                {
                    flag = true;
                    break;
                }
                break;
            case 4:
                if (gimmick_obj_work.pos.y < player_obj_work.pos.y)
                {
                    flag = true;
                    break;
                }
                break;
        }
        return flag;
    }

    private static void gmGmkWaterAreaDefFuncDelay(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj1 = own_rect.parent_obj;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)parentObj1;
        OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (!gmGmkWaterAreaModeCheckWait(parentObj1))
            return;
        uint type = gmGmkWaterAreaGetType(gmsEnemy3DWork.ene_com.eve_rec);
        if (!gmGmkWaterAreaCheckDir(parentObj1, parentObj2, type))
            return;
        gmGmkWaterAreaModeChangeLady(parentObj1);
    }

    private static bool gmGmkWaterAreaModeCheckWait(OBS_OBJECT_WORK obj_work)
    {
        return obj_work.ppFunc == null;
    }

    private static void gmGmkWaterAreaModeChangeWait(OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag &= 4294967279U;
        obj_work.ppFunc = null;
    }

    private static void gmGmkWaterAreaModeChangeLady(OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag &= 4294967279U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkWaterAreaMainLady);
    }

    private static void gmGmkWaterAreaModeChangeActive(OBS_OBJECT_WORK obj_work)
    {
        gmGmkWaterAreaRequestChangeWatarLevel(obj_work);
        obj_work.flag |= 16U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkWaterAreaMainActive);
        gmGmkWaterAreaUserTimerSetCounter(obj_work, 0);
        gmGmkWaterAreaRequestChangeWatarLevel(obj_work);
        GmWaterSurfaceSetFlagDraw(true);
    }

    private static void gmGmkWaterAreaMainLady(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (((int)gmsEnemy3DWork.ene_com.rect_work[2].flag & 131072) != 0)
            return;
        uint type = gmGmkWaterAreaGetType(gmsEnemy3DWork.ene_com.eve_rec);
        OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        if (gmGmkWaterAreaCheckDir(obj_work, targetObj, type))
            gmGmkWaterAreaModeChangeWait(obj_work);
        else
            gmGmkWaterAreaModeChangeActive(obj_work);
    }

    private static void gmGmkWaterAreaMainActive(OBS_OBJECT_WORK obj_work)
    {
        int time = gmGmkWaterAreaUserWorkGetTime(obj_work);
        int counter = gmGmkWaterAreaUserTimerGetCounter(obj_work);
        gmGmkWaterAreaUserTimerAddCounter(obj_work, 1);
        if (counter < time * 60)
            return;
        gmGmkWaterAreaModeChangeWait(obj_work);
    }

    private static void gmGmkWaterAreaUserWorkSetLevel(OBS_OBJECT_WORK obj_work, ushort level)
    {
        obj_work.user_work |= (uint)level << 16;
    }

    private static ushort gmGmkWaterAreaUserWorkGetLevel(OBS_OBJECT_WORK obj_work)
    {
        return (ushort)(obj_work.user_work >> 16);
    }

    private static void gmGmkWaterAreaUserWorkSetTime(OBS_OBJECT_WORK obj_work, ushort time)
    {
        obj_work.user_work &= 4294901760U;
        obj_work.user_work |= time;
    }

    private static ushort gmGmkWaterAreaUserWorkGetTime(OBS_OBJECT_WORK obj_work)
    {
        return (ushort)obj_work.user_work;
    }

    private static void gmGmkWaterAreaUserTimerSetCounter(OBS_OBJECT_WORK obj_work, int time)
    {
        obj_work.user_timer = time;
    }

    private static void gmGmkWaterAreaUserTimerAddCounter(OBS_OBJECT_WORK obj_work, int time)
    {
        obj_work.user_timer += time;
    }

    private static int gmGmkWaterAreaUserTimerGetCounter(OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_timer;
    }

}