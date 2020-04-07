using System;
using System.Collections.Generic;
using System.Text;

partial class AppMain
{
    private void nnSetNodeUserMotionCallback(AppMain.NNS_NODEUSRMOT_CALLBACK_FUNC func)
    {
        AppMain.nngNodeUserMotionCallbackFunc = func;
    }

    private AppMain.NNS_NODEUSRMOT_CALLBACK_FUNC nnGetNodeUserMotionCallback()
    {
        return AppMain.nngNodeUserMotionCallbackFunc;
    }

    private void nnSetNodeUserMotionTriggerTime(float t)
    {
        AppMain.nngNodeUserMotionTriggerTime = t;
    }

    private static void nnSetUpNodeStatusList(uint[] nodestatlist, int num, uint flag)
    {
        for (int index = 0; index < num; ++index)
            nodestatlist[index] = flag;
    }

    private static void nnCalcNodeStatusListMatrixPaletteNode(int nodeIdx)
    {
        AppMain.mppAssertNotImpl();
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_NODE nnsNode;
        do
        {
            nnsNode = AppMain.nnnodestatuslist.nnsNodeList[nodeIdx];
            int iMatrix = (int)nnsNode.iMatrix;
            if (iMatrix != -1)
            {
                AppMain.NNS_MATRIX nnsMatrix3 = AppMain.nnnodestatuslist.nnsMtxPal[iMatrix];
                if (((int)nnsNode.fType & 8) != 0)
                    AppMain.nnCopyMatrix(nnsMatrix1, nnsMatrix3);
                if (((int)nnsNode.fType & 128) != 0)
                {
                    AppMain.nnInvertOrthoMatrix(nnsMatrix2, nnsNode.InvInitMtx);
                    AppMain.nnMultiplyMatrix(nnsMatrix1, nnsMatrix3, nnsMatrix2);
                }
                else
                {
                    AppMain.nnInvertMatrix(nnsMatrix2, nnsNode.InvInitMtx);
                    AppMain.nnMultiplyMatrix(nnsMatrix1, nnsMatrix3, nnsMatrix2);
                }
                AppMain.nnCalcClipSetNodeStatus(AppMain.nnnodestatuslist.nnsNodeStatList, AppMain.nnnodestatuslist.nnsNodeList, nodeIdx, nnsMatrix1, 1f, AppMain.nnnodestatuslist.nnsNSFlag);
            }
            if (nnsNode.iChild != (short)-1)
                AppMain.nnCalcNodeStatusListMatrixPaletteNode((int)nnsNode.iChild);
            nodeIdx = (int)nnsNode.iSibling;
        }
        while (nnsNode.iSibling != (short)-1);
    }

    private static void nnCalcNodeStatusListMatrixPalette(
      uint[] nodestatlist,
      AppMain.NNS_MATRIX[] mtxpal,
      AppMain.NNS_OBJECT obj,
      uint flag)
    {
        AppMain.nnnodestatuslist.nnsMtxPal = mtxpal;
        AppMain.nnnodestatuslist.nnsNodeStatList = nodestatlist;
        AppMain.nnnodestatuslist.nnsNodeList = obj.pNodeList;
        AppMain.nnnodestatuslist.nnsNSFlag = flag;
        AppMain.nnCalcNodeStatusListMatrixPaletteNode(0);
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