partial class AppMain
{
    private void nnSetNodeUserMotionCallback(NNS_NODEUSRMOT_CALLBACK_FUNC func)
    {
        nngNodeUserMotionCallbackFunc = func;
    }

    private NNS_NODEUSRMOT_CALLBACK_FUNC nnGetNodeUserMotionCallback()
    {
        return nngNodeUserMotionCallbackFunc;
    }

    private void nnSetNodeUserMotionTriggerTime(float t)
    {
        nngNodeUserMotionTriggerTime = t;
    }

    private static void nnSetUpNodeStatusList(uint[] nodestatlist, int num, uint flag)
    {
        for (int index = 0; index < num; ++index)
            nodestatlist[index] = flag;
    }

    private static void nnCalcNodeStatusListMatrixPaletteNode(int nodeIdx)
    {
        mppAssertNotImpl();
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_NODE nnsNode;
        do
        {
            nnsNode = nnnodestatuslist.nnsNodeList[nodeIdx];
            int iMatrix = nnsNode.iMatrix;
            if (iMatrix != -1)
            {
                NNS_MATRIX nnsMatrix3 = nnnodestatuslist.nnsMtxPal[iMatrix];
                if (((int)nnsNode.fType & 8) != 0)
                    nnCopyMatrix(nnsMatrix1, nnsMatrix3);
                if (((int)nnsNode.fType & 128) != 0)
                {
                    nnInvertOrthoMatrix(nnsMatrix2, nnsNode.InvInitMtx);
                    nnMultiplyMatrix(nnsMatrix1, nnsMatrix3, nnsMatrix2);
                }
                else
                {
                    nnInvertMatrix(nnsMatrix2, nnsNode.InvInitMtx);
                    nnMultiplyMatrix(nnsMatrix1, nnsMatrix3, nnsMatrix2);
                }
                nnCalcClipSetNodeStatus(nnnodestatuslist.nnsNodeStatList, nnnodestatuslist.nnsNodeList, nodeIdx, nnsMatrix1, 1f, nnnodestatuslist.nnsNSFlag);
            }
            if (nnsNode.iChild != -1)
                nnCalcNodeStatusListMatrixPaletteNode(nnsNode.iChild);
            nodeIdx = nnsNode.iSibling;
        }
        while (nnsNode.iSibling != -1);
    }

    private static void nnCalcNodeStatusListMatrixPalette(
      uint[] nodestatlist,
      NNS_MATRIX[] mtxpal,
      NNS_OBJECT obj,
      uint flag)
    {
        nnnodestatuslist.nnsMtxPal = mtxpal;
        nnnodestatuslist.nnsNodeStatList = nodestatlist;
        nnnodestatuslist.nnsNodeList = obj.pNodeList;
        nnnodestatuslist.nnsNSFlag = flag;
        nnCalcNodeStatusListMatrixPaletteNode(0);
    }

    public static bool NNM_NODE_IS_SIIK(uint _nodetype)
    {
        return ((int)_nodetype & 235134976) != 0 && ((int)_nodetype & 134217728) == 0;
    }

    public static bool NNM_NODE_IS_XSIIK(uint _nodetype)
    {
        return ((int)_nodetype & 134217728) != 0;
    }
}