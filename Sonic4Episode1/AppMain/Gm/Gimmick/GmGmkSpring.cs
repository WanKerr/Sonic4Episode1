public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSpringInit(
        GMS_EVE_RECORD_EVENT eve_rec,
        int pos_x,
        int pos_y,
        byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_SPRING");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        int index1 = eve_rec.id > 79 ? eve_rec.id - 70 + 1 - 21 : eve_rec.id - 70;
        if (eve_rec.id == 71 || eve_rec.id == 73 || (eve_rec.id == 75 || eve_rec.id == 77))
        {
            if (eve_rec.id == 71 || eve_rec.id == 75)
                ObjObjectCopyAction3dNNModel(work, gm_gmk_spring_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
            else
                ObjObjectCopyAction3dNNModel(work, gm_gmk_spring_obj_3d_list[2], gmsEnemy3DWork.obj_3d);
            ObjObjectAction3dNNMotionLoad(work, 0, false, ObjDataGet(793), null, 0, null);
            work.user_timer = 2;
            work.user_work = 3U;
        }
        else
        {
            ObjObjectCopyAction3dNNModel(work, gm_gmk_spring_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            ObjObjectAction3dNNMotionLoad(work, 0, false, ObjDataGet(793), null, 0, null);
            work.user_timer = 0;
            work.user_work = 1U;
        }
        work.pos.z = eve_rec.id == 78 || eve_rec.id == 79 ? -655360 : -131072;
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[0];
        pRec1.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkSpringDefFunc);
        pRec1.ppHit = null;
        ObjRectAtkSet(pRec1, 0, 0);
        ObjRectDefSet(pRec1, 65534, 0);
        ObjRectWorkSet(pRec1, gm_gmk_spring_rect[index1][0], gm_gmk_spring_rect[index1][1], gm_gmk_spring_rect[index1][2], gm_gmk_spring_rect[index1][3]);
        pRec1.flag |= 1024U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec2, gm_gmk_spring_rect[index1][0], gm_gmk_spring_rect[index1][1], gm_gmk_spring_rect[index1][2], gm_gmk_spring_rect[index1][3]);
        pRec2.flag &= 4294967291U;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.dir.z = gm_gmk_spring_dir[index1];
        if ((eve_rec.flag & 16) != 0)
        {
            if (eve_rec.width >= 64)
                eve_rec.width = 0;
            if (!GmGmkSwitchIsOn(eve_rec.width))
                gmGmkSpringSwitchOffInit(work);
            else
                gmGmkSpringFwInit(work);
        }
        else
            gmGmkSpringFwInit(work);
        if (GsGetMainSysInfo().stage_id == 14)
        {
            work.obj_3d.use_light_flag = 0U;
            work.obj_3d.use_light_flag |= 64U;
        }
        else
        {
            int nMaterial = work.obj_3d._object.nMaterial;
            if (nMaterial == 1)
            {
                ((NNS_MATERIAL_GLES11_DESC)work.obj_3d._object.pMatPtrList[0].pMaterial).fFlag |= 3U;
            }
            else
            {
                NNS_MATERIAL_GLES11_DESC[] pMaterial = (NNS_MATERIAL_GLES11_DESC[])work.obj_3d._object.pMatPtrList[0].pMaterial;
                for (int index2 = 0; index2 < nMaterial; ++index2)
                    pMaterial[index2].fFlag |= 3U;
            }
        }
        work.flag |= 1073741824U;
        return work;
    }

    public static void GmGmkSpringBuild()
    {
        gm_gmk_spring_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(791), GmGameDatGetGimmickData(792), 0U);
    }

    public static void GmGmkSpringFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(791));
        GmGameDBuildRegFlushModel(gm_gmk_spring_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkSpringFwInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjDrawObjectActionSet(obj_work, obj_work.user_timer);
        obj_work.ppFunc = (gmsEnemy3DWork.ene_com.eve_rec.flag & 16) == 0 ? new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpringFwMain) : new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpringSwitchOnMain);
        if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 5) == 0)
            return;
        obj_work.disp_flag |= 32U;
    }

    private static void gmGmkSpringFwMain(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkSpringActInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjDrawObjectActionSet(obj_work, (int)obj_work.user_work);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpringActMain);
        if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 4) != 0)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
    }

    private static void gmGmkSpringActMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294573823U;
        gmGmkSpringFwInit(obj_work);
    }

    private static void gmGmkSpringDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        int fall_dir = -1;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != 1)
            return;
        gmGmkSpringActInit((OBS_OBJECT_WORK)parentObj1);
        int a = MTM_MATH_CLIP(parentObj1.eve_rec.left, 0, 7);
        if (parentObj1.eve_rec.id == 76 || parentObj1.eve_rec.id == 72)
            a = MTM_MATH_CLIP(a, 0, 5);
        int spd_x = 30720 + 6144 * a;
        int spd_y = -spd_x;
        if (parentObj1.eve_rec.id == 74 || parentObj1.eve_rec.id == 70)
            spd_x = 0;
        else if (parentObj1.eve_rec.id == 72 || parentObj1.eve_rec.id == 76)
        {
            spd_y = 0;
        }
        else
        {
            spd_x = spd_x * 181 >> 8;
            spd_y = spd_y * 181 >> 8;
        }
        if (73 <= parentObj1.eve_rec.id && parentObj1.eve_rec.id <= 75)
            spd_y = -spd_y;
        if (75 <= parentObj1.eve_rec.id && parentObj1.eve_rec.id <= 77 || (parentObj1.eve_rec.id == 79 || parentObj1.eve_rec.id == 101))
            spd_x = -spd_x;
        if ((parentObj1.eve_rec.id == 76 || parentObj1.eve_rec.id == 72) && (parentObj1.eve_rec.flag & 2) == 0)
            parentObj2.obj_work.pos.y += 8192;
        if (1 <= parentObj1.eve_rec.height && parentObj1.eve_rec.height <= 4)
            fall_dir = (parentObj1.eve_rec.height - 1) * 16384;
        GmPlySeqInitSpringJump(parentObj2, spd_x, spd_y, (parentObj1.eve_rec.flag & 8) != 0, parentObj1.eve_rec.top >= 0 ? parentObj1.eve_rec.top * 4096 : 0, fall_dir, (parentObj1.eve_rec.flag & 32) != 0);
        GmComEfctCreateSpring(parentObj1.obj_work, (mine_rect.rect.left + mine_rect.rect.right) * 4096 / 2, (mine_rect.rect.top + mine_rect.rect.bottom) * 4096 / 2);
        if ((parentObj1.eve_rec.flag & 64) == 0 || ((int)g_gs_main_sys_info.game_flag & 512) == 0)
            return;
        parentObj2.gmk_flag2 |= 512U;
    }

    private static void gmGmkSpringSwitchOffInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjDrawObjectActionSet(obj_work, obj_work.user_timer);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSpringSwitchOffMain);
        obj_work.disp_flag |= 32U;
        obj_work.flag |= 2U;
    }

    private static void gmGmkSpringSwitchOffMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (!GmGmkSwitchIsOn(gmsEnemy3DWork.ene_com.eve_rec.width))
            return;
        obj_work.disp_flag &= 4294967263U;
        obj_work.flag &= 4294967293U;
        GmComEfctCreateSpring(obj_work, (gmsEnemy3DWork.ene_com.rect_work[2].rect.left + gmsEnemy3DWork.ene_com.rect_work[2].rect.right) * 4096 / 2, (gmsEnemy3DWork.ene_com.rect_work[2].rect.top + gmsEnemy3DWork.ene_com.rect_work[2].rect.bottom) * 4096 / 2);
        gmGmkSpringFwInit(obj_work);
    }

    private static void gmGmkSpringSwitchOnMain(OBS_OBJECT_WORK obj_work)
    {
        if (GmGmkSwitchIsOn(((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.width))
            return;
        gmGmkSpringSwitchOffInit(obj_work);
    }


}