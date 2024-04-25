public partial class AppMain
{
    private static void GmEneGabuBuild()
    {
        gm_ene_gabu_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(664)), readAMBFile(GmGameDatGetEnemyData(665)), 0U);
    }

    private static void GmEneGabuFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(664));
        GmGameDBuildRegFlushModel(gm_ene_gabu_obj_3d_list, amsAmbHeader.file_num);
    }

    private static OBS_OBJECT_WORK GmEneGabuInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "ENE_GABU");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_gabu_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(666), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -11, -16, 11, 16);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        ObjRectWorkSet(pRec2, -19, -24, 19, 24);
        pRec2.flag |= 4U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -19, -24, 19, 24);
        pRec3.flag &= 4294967291U;
        work.move_flag |= 384U;
        work.view_out_ofst_plus[1] = -256;
        int num1 = -eve_rec.top << 13;
        if (num1 <= 0)
            num1 = 393216;
        int num2 = eve_rec.flag & 7;
        work.user_timer = num2 > 3 ? -24576 - -1024 * (num2 - 3) : -1024 * num2 - 24576;
        int denom = FX_Div(num1 * 2, -work.user_timer);
        work.spd_fall = FX_Div(-work.user_timer, denom);
        work.user_work = (uint)work.pos.y;
        ObjDrawObjectActionSet(work, 1);
        work.disp_flag |= 4U;
        gmEneGabuJumpInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmEneGabuJumpInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (obj_work.obj_3d.act_id[0] != 1)
        {
            ObjDrawObjectActionSet3DNNBlend(obj_work, 1);
            obj_work.disp_flag |= 4U;
        }
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGabuJumpMain);
        obj_work.spd.y = obj_work.user_timer;
    }

    private static void gmEneGabuJumpMain(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_3d.act_id[0] != 2 && obj_work.spd.y < 0 && -obj_work.spd.y / obj_work.spd_fall <= 20)
            ObjDrawObjectActionSet(obj_work, 2);
        if (obj_work.obj_3d.act_id[0] == 2 && ((int)obj_work.disp_flag & 8) != 0)
        {
            ObjDrawObjectActionSet3DNNBlend(obj_work, 0);
            obj_work.disp_flag |= 4U;
        }
        if (obj_work.pos.y < (int)obj_work.user_work)
            return;
        if ((((GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 128) != 0)
            gmEneGabuJumpWaitInit(obj_work);
        else
            gmEneGabuJumpInit(obj_work);
    }

    private static void gmEneGabuJumpWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjDrawObjectActionSet3DNNBlend(obj_work, 1);
        obj_work.disp_flag |= 4U;
        obj_work.user_flag = 61440U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGabuJumpWaitMain);
        obj_work.spd.y = 0;
        obj_work.move_flag &= 4294967167U;
    }

    private static void gmEneGabuJumpWaitMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_flag = (uint)ObjTimeCountDown((int)obj_work.user_flag);
        if (obj_work.user_flag != 0U)
            return;
        obj_work.move_flag |= 128U;
        gmEneGabuJumpInit(obj_work);
    }


}