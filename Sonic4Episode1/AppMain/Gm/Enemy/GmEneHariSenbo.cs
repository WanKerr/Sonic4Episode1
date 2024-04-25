public partial class AppMain
{
    private static OBS_OBJECT_WORK GmEneHariSenboInit(
          GMS_EVE_RECORD_EVENT eve_rec,
          int pos_x,
          int pos_y,
          byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_HARI_WORK(), "ENE_HARI");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        if (eve_rec.id == 0)
            ObjObjectCopyAction3dNNModel(work, gm_ene_harisenbo_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        else
            ObjObjectCopyAction3dNNModel(work, gm_ene_harisenbo_r_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(660), null, 0, null);
        ObjDrawObjectSetToon(work);
        ObjDrawObjectActionSet(work, 0);
        work.pos.z = 655360;
        gmsEnemy3DWork.obj_3d.mtn_cb_func = new mtn_cb_func_delegate(gmEneHariMotionCallback);
        gmsEnemy3DWork.obj_3d.mtn_cb_param = work;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -12, -12, 12, 12);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        ObjRectWorkSet(pRec2, -22, -22, 22, 22);
        pRec2.flag |= 4U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -22, -22, 22, 22);
        pRec3.flag &= 4294967291U;
        ObjObjectFieldRectSet(work, -4, -8, 4, 0);
        work.move_flag |= 8448U;
        if ((eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        if (eve_rec.id == 0)
        {
            gmEneHarisenboFwInit(work);
            work.flag |= 1073741824U;
        }
        else
        {
            work.user_work = (uint)(eve_rec.width * 30 * 4096);
            if (work.user_work == 0U)
                work.user_work = 1228800U;
            work.user_flag = (uint)(eve_rec.height * 30 * 4096);
            if (work.user_flag == 0U)
                work.user_flag = 1228800U;
            gmEneHarisenboRedAtkWaitInit(work);
        }
        gmEneHariCreateJetEfct((GMS_ENE_HARI_WORK)work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static void GmEneHariSenboBuild()
    {
        gm_ene_harisenbo_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(658)), readAMBFile(GmGameDatGetEnemyData(659)), 0U);
        AMS_AMB_HEADER header = readAMBFile(GmGameDatGetEnemyData(659));
        AmbChunk ambChunk = amBindGet(header, header.file_num - 1, out header.dir);
        gm_ene_harisenbo_r_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(658)), readAMBFile(GmGameDatGetEnemyData(659)), 0U, readTXBfile(ambChunk.array, ambChunk.offset));
    }

    public static void GmEneHariSenboFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(658));
        GmGameDBuildRegFlushModel(gm_ene_harisenbo_obj_3d_list, amsAmbHeader.file_num);
        GmGameDBuildRegFlushModel(gm_ene_harisenbo_r_obj_3d_list, amsAmbHeader.file_num);
    }

    public static void gmEneHarisenboRedAtkWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjDrawObjectActionSet3DNNBlend(obj_work, 0);
        obj_work.disp_flag |= 4U;
        obj_work.user_timer = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneHarisenboRedAtkWaitMain);
    }

    private static void gmEneHarisenboRedAtkWaitMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountUp(obj_work.user_timer);
        if ((uint)obj_work.user_timer < obj_work.user_work)
            return;
        gmEneHarisenboRedAtkInit(obj_work);
    }

    private static void gmEneHarisenboRedAtkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_ENE_HARI_WORK gmsEneHariWork = (GMS_ENE_HARI_WORK)obj_work;
        ObjDrawObjectActionSet3DNNBlend(obj_work, 2);
        obj_work.disp_flag |= 4U;
        obj_work.user_timer = 245760;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneHarisenboRedAtkMain);
    }

    private static void gmEneHarisenboRedAtkMain(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_3d.act_id[0] == 2)
        {
            obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
            if (obj_work.user_timer != 0)
                return;
            ((GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[0].def_power = 2;
            ObjDrawObjectActionSet3DNNBlend(obj_work, 3);
        }
        else if (obj_work.obj_3d.act_id[0] == 3)
        {
            if (((int)obj_work.disp_flag & 8) == 0)
                return;
            OBS_RECT_WORK pRec = ((GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1];
            ObjRectWorkSet(pRec, -24, -24, 24, 24);
            pRec.flag |= 4U;
            ObjDrawObjectActionSet3DNNBlend(obj_work, 1);
            obj_work.disp_flag |= 4U;
            obj_work.user_timer = 0;
        }
        else
        {
            obj_work.user_timer = ObjTimeCountUp(obj_work.user_timer);
            if ((uint)obj_work.user_timer < obj_work.user_flag)
                return;
            OBS_RECT_WORK pRec = ((GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1];
            ObjRectWorkSet(pRec, -12, -12, 12, 12);
            pRec.flag |= 4U;
            ((GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[0].def_power = 0;
            gmEneHarisenboRedAtkWaitInit(obj_work);
        }
    }

    private static void gmEneHarisenboFwInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjDrawObjectActionSet(obj_work, 0);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneHarisenboFwMain);
    }

    private static void gmEneHarisenboFwMain(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmEneHariMotionCallback(
      AMS_MOTION motion,
      NNS_OBJECT _object,
      object param)
    {
        NNS_MATRIX motionCallbackNodeMtx = gmEneHariMotionCallback_node_mtx;
        NNS_MATRIX motionCallbackBaseMtx = gmEneHariMotionCallback_base_mtx;
        GMS_ENE_HARI_WORK gmsEneHariWork = (GMS_ENE_HARI_WORK)(OBS_OBJECT_WORK)param;
        nnMakeUnitMatrix(motionCallbackBaseMtx);
        nnMultiplyMatrix(motionCallbackBaseMtx, motionCallbackBaseMtx, amMatrixGetCurrent());
        nnCalcNodeMatrixTRSList(motionCallbackNodeMtx, _object, 7, motion.data, motionCallbackBaseMtx);
        gmsEneHariWork.jet_mtx.Assign(motionCallbackNodeMtx);
    }

    private static void gmEneHariCreateJetEfct(GMS_ENE_HARI_WORK hari_work)
    {
        if (hari_work.efct_jet != null)
            return;
        hari_work.efct_jet = GmEfctEneEsCreate((OBS_OBJECT_WORK)hari_work, 12);
        hari_work.efct_jet.efct_com.obj_work.flag |= 524304U;
        hari_work.efct_jet.efct_com.obj_work.user_work_OBJECT = hari_work.jet_mtx;
        hari_work.efct_jet.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneHariJetEfctMain);
    }

    private static void gmEneHariJetEfctMain(OBS_OBJECT_WORK obj_work)
    {
        NNS_MATRIX userWorkObject = (NNS_MATRIX)obj_work.user_work_OBJECT;
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        if (obj_work.parent_obj == null)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            nnsVector.x = userWorkObject.M03 - FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.x);
            nnsVector.y = -userWorkObject.M13 - FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.y);
            nnsVector.z = userWorkObject.M23 - FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.z);
            if (((int)obj_work.parent_obj.disp_flag & 1) != 0)
            {
                nnsVector.x = -nnsVector.x;
                nnsVector.z = -nnsVector.z;
            }
            nnsVector.y += 5f;
            GmComEfctSetDispOffsetF((GMS_EFFECT_3DES_WORK)obj_work, nnsVector.x, nnsVector.y, nnsVector.z);
            GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
            GlobalPool<NNS_VECTOR>.Release(nnsVector);
        }
    }

}