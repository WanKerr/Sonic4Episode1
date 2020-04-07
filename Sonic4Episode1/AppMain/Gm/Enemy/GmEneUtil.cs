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
    public static void gmEneExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GmEneUtilExitNodeMatrix(((AppMain.GMS_ENE_KANI_WORK)AppMain.mtTaskGetTcbWork(tcb)).node_work);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    public static void GmEneUtilExitNodeMatrix(AppMain.GMS_ENE_NODE_MATRIX node_work)
    {
        if (node_work._id[0] != 'S' || node_work._id[1] != 'N' || (node_work._id[2] != 'M' || node_work._id[3] != ' ') || (node_work._id[4] != 'S' || node_work._id[5] != 'Y' || node_work._id[6] != 'S'))
            return;
        AppMain.GmBsCmnClearBossMotionCBSystem(node_work.obj_work);
        AppMain.GmBsCmnDeleteSNMWork(node_work.snm_work);
        node_work._id[0] = char.MinValue;
    }

    public static AppMain.NNS_MATRIX GmEneUtilGetNodeMatrix(
      AppMain.GMS_ENE_NODE_MATRIX node_work,
      int node_id)
    {
        if (node_work.work[node_id] < 0)
            node_work.work[node_id] = AppMain.GmBsCmnRegisterSNMNode(node_work.snm_work, node_id);
        return AppMain.GmBsCmnGetSNMMtx(node_work.snm_work, node_work.work[node_id]);
    }

    public static void GmEneUtilInitNodeMatrix(
      AppMain.GMS_ENE_NODE_MATRIX node_work,
      AppMain.OBS_OBJECT_WORK obj_work,
      int max_node)
    {
        node_work.initCount = max_node;
        node_work.useCount = 0;
        AppMain.GmBsCmnInitBossMotionCBSystem(obj_work, node_work.mtn_mgr);
        AppMain.GmBsCmnCreateSNMWork(node_work.snm_work, obj_work.obj_3d._object, (ushort)max_node);
        AppMain.GmBsCmnAppendBossMotionCallback(node_work.mtn_mgr, node_work.snm_work.bmcb_link);
        node_work.obj_work = obj_work;
        for (int index = 0; index < 32; ++index)
            node_work.work[index] = -1;
        node_work._id[0] = 'S';
        node_work._id[1] = 'N';
        node_work._id[2] = 'M';
        node_work._id[3] = ' ';
        node_work._id[4] = 'S';
        node_work._id[5] = 'Y';
        node_work._id[6] = 'S';
    }

    public static void GmEneUtilSetMatrixNN(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.NNS_MATRIX w_mtx)
    {
        AppMain.NNS_MATRIX userObjMtxR = obj_work.obj_3d.user_obj_mtx_r;
        obj_work.pos.x = AppMain.FX_F32_TO_FX32(w_mtx.M03);
        obj_work.pos.y = -AppMain.FX_F32_TO_FX32(w_mtx.M13);
        obj_work.pos.z = AppMain.FX_F32_TO_FX32(w_mtx.M23);
        obj_work.disp_flag |= 16777216U;
        AppMain.AkMathNormalizeMtx(userObjMtxR, w_mtx);
    }

}