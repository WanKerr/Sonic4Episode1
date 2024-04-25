public partial class AppMain
{
    public static OBS_OBJECT_WORK GmGmkBubbleManagerInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkBubbleLoadObjNoModel(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        gmGmkBubbleManagerInit(objWork);
        gmGmkBubbleSetUserWorkIntervalNormal(objWork, (ushort)((uint)eve_rec.left * 60U));
        return objWork;
    }

    public static GMS_ENEMY_3D_WORK gmGmkBubbleLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_BUBBLE");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    public static ushort gmGmkBubbleGameSystemGetWaterLevel()
    {
        return g_gm_main_system.water_level;
    }

    public static void gmGmkBubbleManagerInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 8448U;
        gmGmkBubbleSetUserTimerCounter(obj_work, 0U);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBubbleManagerMainWait);
        GmEfctZoneEsCreate(obj_work, 2, 4).efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBubbleManagerEffectMain);
    }

    public static void gmGmkBubbleManagerMainWait(OBS_OBJECT_WORK obj_work)
    {
        if (gmGmkBubbleGameSystemGetWaterLevel() * 4096 > obj_work.pos.y)
            return;
        if ((uint)((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.top * 60U < gmGmkBubbleGetUserTimerCounter(obj_work))
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBubbleManagerMain);
        gmGmkBubbleAddUserTimerCounter(obj_work, 1);
    }

    public static void gmGmkBubbleManagerMain(OBS_OBJECT_WORK obj_work)
    {
        if (gmGmkBubbleGameSystemGetWaterLevel() * 4096 > obj_work.pos.y)
            return;
        uint num = gmGmkBubbleGetUserWorkIntervalNormal(obj_work);
        if (num == 0U)
            num = 60U;
        if (gmGmkBubbleGetUserTimerCounter(obj_work) % num == 0U)
            gmGmkBubbleInit(GmEfctZoneEsCreate(obj_work, 2, 1));
        gmGmkBubbleAddUserTimerCounter(obj_work, 1);
    }

    public static void gmGmkBubbleManagerEffectMain(OBS_OBJECT_WORK obj_work)
    {
        if (gmGmkBubbleGameSystemGetWaterLevel() * 4096 > obj_work.pos.y)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
    }

    public static void gmGmkBubbleInit(GMS_EFFECT_3DES_WORK effect_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)effect_work;
        obsObjectWork.flag &= 4294967293U;
        obsObjectWork.move_flag &= 4294958847U;
        obsObjectWork.move_flag |= 4608U;
        obsObjectWork.spd.y = (int)-GMD_GMK_BUBBLE_SPEED_Y;
        obsObjectWork.pos.z = 1048576;
        OBS_RECT_WORK[] rectWork = effect_work.efct_com.rect_work;
        GmEffectRectInit(effect_work.efct_com, gm_gmk_bubble_table_atk, gm_gmk_bubble_table_def, 1, 1);
        ObjRectWorkSet(rectWork[0], -8, 7, 8, 8);
        rectWork[0].flag |= 1028U;
        rectWork[0].ppDef = new OBS_RECT_WORK_Delegate1(gmGmkBubbleDefFunc);
        rectWork[1].flag |= 3072U;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBubbleMainMoveLeft);
    }

    public static void gmGmkBubbleDefFunc(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj1 = own_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != 1)
            return;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)parentObj2;
        GmPlySeqInitBreathing(ply_work);
        GmPlayerBreathingSet(ply_work);
        parentObj1.flag |= 4U;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctZoneEsCreate(null, 2, 3);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj1.pos.x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj1.pos.y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = parentObj1.pos.z;
        GMM_PAD_VIB_SMALL();
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBubbleMainHit);
    }

    public static void gmGmkBubbleMainMoveLeft(OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.x = gmGmkBubbleAddUserWorkSpeedX(obj_work, (int)-GMD_GMK_BUBBLE_SPEED_X_ADD);
        if (obj_work.spd.x < -GMD_GMK_BUBBLE_SPEED_X_MAX)
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBubbleMainMoveRight);
        if (gmGmkBubbleGameSystemGetWaterLevel() * 4096 <= obj_work.pos.y)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBubbleMainEnd);
    }

    public static void gmGmkBubbleMainMoveRight(OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.x = gmGmkBubbleAddUserWorkSpeedX(obj_work, (int)GMD_GMK_BUBBLE_SPEED_X_ADD);
        if (obj_work.spd.x > GMD_GMK_BUBBLE_SPEED_X_MAX)
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBubbleMainMoveLeft);
        if (gmGmkBubbleGameSystemGetWaterLevel() * 4096 <= obj_work.pos.y)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBubbleMainEnd);
    }

    public static void gmGmkBubbleMainHit(OBS_OBJECT_WORK obj_work)
    {
        ++obj_work.user_timer;
        int num1 = GMD_GMK_BUBBLE_FRAME_HIT_DELETE - obj_work.user_timer;
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        if (num1 < GMD_GMK_BUBBLE_HIT_EFFECT_NUM)
            GmPlyEfctCreateBubble(ply_work);
        if (num1 > 0)
        {
            OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)ply_work;
            int num2 = obsObjectWork.pos.x - obj_work.pos.x;
            int num3 = obsObjectWork.pos.y - GMD_GMK_BUBBLE_OFFSET_Y * 4096 - obj_work.pos.y;
            int num4 = ((int)obsObjectWork.disp_flag & 1) == 0 ? num2 + GMD_GMK_BUBBLE_OFFSET_X * 4096 : num2 - GMD_GMK_BUBBLE_OFFSET_X * 4096;
            obj_work.spd.x = num4 / num1;
            obj_work.spd.y = num3 / num1;
        }
        else
        {
            obj_work.user_timer = 0;
            obj_work.flag |= 4U;
        }
    }

    public static void gmGmkBubbleMainEnd(OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag |= 4U;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctZoneEsCreate(null, 2, 2);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = obj_work.pos.x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = obj_work.pos.y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = obj_work.pos.z;
    }

    public static void gmGmkBubbleSetUserWorkIntervalNormal(
      OBS_OBJECT_WORK obj_work,
      ushort interval)
    {
        obj_work.user_work |= (uint)interval << 16;
    }

    public static ushort gmGmkBubbleGetUserWorkIntervalNormal(OBS_OBJECT_WORK obj_work)
    {
        return (ushort)(obj_work.user_work >> 16);
    }

    public static void gmGmkBubbleSetUserTimerCounter(OBS_OBJECT_WORK obj_work, uint count)
    {
        obj_work.user_timer = (int)count;
    }

    public static void gmGmkBubbleAddUserTimerCounter(OBS_OBJECT_WORK obj_work, int count)
    {
        obj_work.user_timer += count;
    }

    public static uint gmGmkBubbleGetUserTimerCounter(OBS_OBJECT_WORK obj_work)
    {
        return (uint)obj_work.user_timer;
    }

    public static int gmGmkBubbleAddUserWorkSpeedX(OBS_OBJECT_WORK obj_work, int speed)
    {
        obj_work.user_work += (uint)speed;
        return (int)obj_work.user_work;
    }

}