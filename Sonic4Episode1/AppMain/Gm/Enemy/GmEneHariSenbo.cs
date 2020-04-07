using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static AppMain.OBS_OBJECT_WORK GmEneHariSenboInit(
          AppMain.GMS_EVE_RECORD_EVENT eve_rec,
          int pos_x,
          int pos_y,
          byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_HARI_WORK()), "ENE_HARI");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        if (eve_rec.id == (ushort)0)
            AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_harisenbo_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        else
            AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_harisenbo_r_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(660), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        AppMain.ObjDrawObjectActionSet(work, 0);
        work.pos.z = 655360;
        gmsEnemy3DWork.obj_3d.mtn_cb_func = new AppMain.mtn_cb_func_delegate(AppMain.gmEneHariMotionCallback);
        gmsEnemy3DWork.obj_3d.mtn_cb_param = (object)work;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-12, (short)-12, (short)12, (short)12);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        AppMain.ObjRectWorkSet(pRec2, (short)-22, (short)-22, (short)22, (short)22);
        pRec2.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-22, (short)-22, (short)22, (short)22);
        pRec3.flag &= 4294967291U;
        AppMain.ObjObjectFieldRectSet(work, (short)-4, (short)-8, (short)4, (short)0);
        work.move_flag |= 8448U;
        if (((int)eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        if (eve_rec.id == (ushort)0)
        {
            AppMain.gmEneHarisenboFwInit(work);
            work.flag |= 1073741824U;
        }
        else
        {
            work.user_work = (uint)((int)eve_rec.width * 30 * 4096);
            if (work.user_work == 0U)
                work.user_work = 1228800U;
            work.user_flag = (uint)((int)eve_rec.height * 30 * 4096);
            if (work.user_flag == 0U)
                work.user_flag = 1228800U;
            AppMain.gmEneHarisenboRedAtkWaitInit(work);
        }
        AppMain.gmEneHariCreateJetEfct((AppMain.GMS_ENE_HARI_WORK)work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static void GmEneHariSenboBuild()
    {
        AppMain.gm_ene_harisenbo_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(658)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(659)), 0U);
        AppMain.AMS_AMB_HEADER header = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(659));
        AppMain.AmbChunk ambChunk = AppMain.amBindGet(header, header.file_num - 1, out header.dir);
        AppMain.gm_ene_harisenbo_r_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(658)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(659)), 0U, AppMain.readTXBfile(ambChunk.array, ambChunk.offset));
    }

    public static void GmEneHariSenboFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(658));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_harisenbo_obj_3d_list, amsAmbHeader.file_num);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_harisenbo_r_obj_3d_list, amsAmbHeader.file_num);
    }

    public static void gmEneHarisenboRedAtkWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 0);
        obj_work.disp_flag |= 4U;
        obj_work.user_timer = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneHarisenboRedAtkWaitMain);
    }

    private static void gmEneHarisenboRedAtkWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountUp(obj_work.user_timer);
        if ((uint)obj_work.user_timer < obj_work.user_work)
            return;
        AppMain.gmEneHarisenboRedAtkInit(obj_work);
    }

    private static void gmEneHarisenboRedAtkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_ENE_HARI_WORK gmsEneHariWork = (AppMain.GMS_ENE_HARI_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 2);
        obj_work.disp_flag |= 4U;
        obj_work.user_timer = 245760;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneHarisenboRedAtkMain);
    }

    private static void gmEneHarisenboRedAtkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_3d.act_id[0] == 2)
        {
            obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
            if (obj_work.user_timer != 0)
                return;
            ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[0].def_power = (short)2;
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 3);
        }
        else if (obj_work.obj_3d.act_id[0] == 3)
        {
            if (((int)obj_work.disp_flag & 8) == 0)
                return;
            AppMain.OBS_RECT_WORK pRec = ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1];
            AppMain.ObjRectWorkSet(pRec, (short)-24, (short)-24, (short)24, (short)24);
            pRec.flag |= 4U;
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 1);
            obj_work.disp_flag |= 4U;
            obj_work.user_timer = 0;
        }
        else
        {
            obj_work.user_timer = AppMain.ObjTimeCountUp(obj_work.user_timer);
            if ((uint)obj_work.user_timer < obj_work.user_flag)
                return;
            AppMain.OBS_RECT_WORK pRec = ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1];
            AppMain.ObjRectWorkSet(pRec, (short)-12, (short)-12, (short)12, (short)12);
            pRec.flag |= 4U;
            ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[0].def_power = (short)0;
            AppMain.gmEneHarisenboRedAtkWaitInit(obj_work);
        }
    }

    private static void gmEneHarisenboFwInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet(obj_work, 0);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneHarisenboFwMain);
    }

    private static void gmEneHarisenboFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmEneHariMotionCallback(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_OBJECT _object,
      object param)
    {
        AppMain.NNS_MATRIX motionCallbackNodeMtx = AppMain.gmEneHariMotionCallback_node_mtx;
        AppMain.NNS_MATRIX motionCallbackBaseMtx = AppMain.gmEneHariMotionCallback_base_mtx;
        AppMain.GMS_ENE_HARI_WORK gmsEneHariWork = (AppMain.GMS_ENE_HARI_WORK)(AppMain.OBS_OBJECT_WORK)param;
        AppMain.nnMakeUnitMatrix(motionCallbackBaseMtx);
        AppMain.nnMultiplyMatrix(motionCallbackBaseMtx, motionCallbackBaseMtx, AppMain.amMatrixGetCurrent());
        AppMain.nnCalcNodeMatrixTRSList(motionCallbackNodeMtx, _object, 7, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, motionCallbackBaseMtx);
        gmsEneHariWork.jet_mtx.Assign(motionCallbackNodeMtx);
    }

    private static void gmEneHariCreateJetEfct(AppMain.GMS_ENE_HARI_WORK hari_work)
    {
        if (hari_work.efct_jet != null)
            return;
        hari_work.efct_jet = AppMain.GmEfctEneEsCreate((AppMain.OBS_OBJECT_WORK)hari_work, 12);
        hari_work.efct_jet.efct_com.obj_work.flag |= 524304U;
        hari_work.efct_jet.efct_com.obj_work.user_work_OBJECT = (object)hari_work.jet_mtx;
        hari_work.efct_jet.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneHariJetEfctMain);
    }

    private static void gmEneHariJetEfctMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.NNS_MATRIX userWorkObject = (AppMain.NNS_MATRIX)obj_work.user_work_OBJECT;
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        if (obj_work.parent_obj == null)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            nnsVector.x = userWorkObject.M03 - AppMain.FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.x);
            nnsVector.y = -userWorkObject.M13 - AppMain.FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.y);
            nnsVector.z = userWorkObject.M23 - AppMain.FXM_FX32_TO_FLOAT(obj_work.parent_obj.pos.z);
            if (((int)obj_work.parent_obj.disp_flag & 1) != 0)
            {
                nnsVector.x = -nnsVector.x;
                nnsVector.z = -nnsVector.z;
            }
            nnsVector.y += 5f;
            AppMain.GmComEfctSetDispOffsetF((AppMain.GMS_EFFECT_3DES_WORK)obj_work, nnsVector.x, nnsVector.y, nnsVector.z);
            AppMain.GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        }
    }

}