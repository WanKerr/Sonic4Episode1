public partial class AppMain
{
    public static void gmEneExit(MTS_TASK_TCB tcb)
    {
        GmEneUtilExitNodeMatrix(((GMS_ENE_KANI_WORK)mtTaskGetTcbWork(tcb)).node_work);
        GmEnemyDefaultExit(tcb);
    }

    public static void GmEneUtilExitNodeMatrix(GMS_ENE_NODE_MATRIX node_work)
    {
        if (node_work._id[0] != 'S' || node_work._id[1] != 'N' || (node_work._id[2] != 'M' || node_work._id[3] != ' ') || (node_work._id[4] != 'S' || node_work._id[5] != 'Y' || node_work._id[6] != 'S'))
            return;
        GmBsCmnClearBossMotionCBSystem(node_work.obj_work);
        GmBsCmnDeleteSNMWork(node_work.snm_work);
        node_work._id[0] = char.MinValue;
    }

    public static NNS_MATRIX GmEneUtilGetNodeMatrix(
      GMS_ENE_NODE_MATRIX node_work,
      int node_id)
    {
        if (node_work.work[node_id] < 0)
            node_work.work[node_id] = GmBsCmnRegisterSNMNode(node_work.snm_work, node_id);
        return GmBsCmnGetSNMMtx(node_work.snm_work, node_work.work[node_id]);
    }

    public static void GmEneUtilInitNodeMatrix(
      GMS_ENE_NODE_MATRIX node_work,
      OBS_OBJECT_WORK obj_work,
      int max_node)
    {
        node_work.initCount = max_node;
        node_work.useCount = 0;
        GmBsCmnInitBossMotionCBSystem(obj_work, node_work.mtn_mgr);
        GmBsCmnCreateSNMWork(node_work.snm_work, obj_work.obj_3d._object, (ushort)max_node);
        GmBsCmnAppendBossMotionCallback(node_work.mtn_mgr, node_work.snm_work.bmcb_link);
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
      OBS_OBJECT_WORK obj_work,
      NNS_MATRIX w_mtx)
    {
        NNS_MATRIX userObjMtxR = obj_work.obj_3d.user_obj_mtx_r;
        obj_work.pos.x = FX_F32_TO_FX32(w_mtx.M03);
        obj_work.pos.y = -FX_F32_TO_FX32(w_mtx.M13);
        obj_work.pos.z = FX_F32_TO_FX32(w_mtx.M23);
        obj_work.disp_flag |= 16777216U;
        AkMathNormalizeMtx(userObjMtxR, w_mtx);
    }

}