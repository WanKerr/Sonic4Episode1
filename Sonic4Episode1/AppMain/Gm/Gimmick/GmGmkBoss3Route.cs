public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkBoss3RouteInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkBoss3RouteLoadObjNoModel(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        gmGmkBoss3RouteInit(objWork);
        return objWork;
    }

    private static GMS_ENEMY_3D_WORK gmGmkBoss3RouteLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_BOSS3_ROUTE");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static void gmGmkBoss3RouteInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 8448U;
        obj_work.flag |= 16U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss3RouteMainFunc);
        obj_work.ppOut = null;
        obj_work.ppMove = null;
    }

    private static bool gmGmkBoss3RouteCheckHit(
      OBS_OBJECT_WORK target_obj_work,
      OBS_OBJECT_WORK gimmick_obj_work)
    {
        int num1 = target_obj_work.pos.x - gimmick_obj_work.pos.x;
        int num2 = target_obj_work.pos.y - gimmick_obj_work.pos.y;
        return MTM_MATH_ABS(num1) <= 262144 && MTM_MATH_ABS(num2) <= 262144 && FX_Mul(num1, num1) + FX_Mul(num2, num2) <= FX_Mul(262144, 262144);
    }

    private static bool gmGmkBoss3RouteSetMoveParam(
      OBS_OBJECT_WORK target_obj_work,
      OBS_OBJECT_WORK gimmick_obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)gimmick_obj_work;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        float num1 = gmsEnemy3DWork.ene_com.eve_rec.width / 10f;
        if (gmsPlayerWork.obj_work.pos.y >= target_obj_work.pos.y && (gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0 && ObjViewOutCheck(target_obj_work.pos.x, target_obj_work.pos.y, 96, 0, 0, 0, 0) != 0)
        {
            target_obj_work.spd.x = 0;
            target_obj_work.spd.y = 0;
            return false;
        }
        int x1 = gimmick_obj_work.pos.x + gmsEnemy3DWork.ene_com.eve_rec.left * 64 * 4096;
        int x2 = gimmick_obj_work.pos.y + gmsEnemy3DWork.ene_com.eve_rec.top * 64 * 4096;
        float f32_1 = FX_FX32_TO_F32(x1);
        float f32_2 = FX_FX32_TO_F32(x2);
        float f32_3 = FX_FX32_TO_F32(target_obj_work.pos.x);
        float f32_4 = FX_FX32_TO_F32(target_obj_work.pos.y);
        float x3 = f32_1 - f32_3;
        float y = f32_2 - f32_4;
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        amVectorSet(nnsVector, x3, y, 0.0f);
        float num2 = 1f / nnLengthVector(nnsVector);
        float x4 = x3 * num2 * num1;
        float x5 = y * num2 * num1;
        target_obj_work.spd.x = FX_F32_TO_FX32(x4);
        target_obj_work.spd.y = FX_F32_TO_FX32(x5);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
        return true;
    }

    private static void gmGmkBoss3RouteMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        for (OBS_OBJECT_WORK obsObjectWork = ObjObjectSearchRegistObject(null, 2); obsObjectWork != null; obsObjectWork = ObjObjectSearchRegistObject(obsObjectWork, 2))
        {
            if (((GMS_ENEMY_COM_WORK)obsObjectWork).eve_rec.id == 319)
            {
                if (!gmGmkBoss3RouteCheckHit(obsObjectWork, obj_work))
                    break;
                if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 2) != 0)
                {
                    obsObjectWork.spd.x = 0;
                    obsObjectWork.spd.y = 0;
                    obsObjectWork.user_flag = 1U;
                    obj_work.flag |= 4U;
                    obj_work.ppFunc = null;
                    gmsEnemy3DWork.ene_com.enemy_flag |= 65536U;
                    break;
                }
                if (!gmGmkBoss3RouteSetMoveParam(obsObjectWork, obj_work))
                    break;
                obj_work.flag |= 4U;
                obj_work.ppFunc = null;
                gmsEnemy3DWork.ene_com.enemy_flag |= 65536U;
                break;
            }
        }
    }

}