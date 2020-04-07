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
    private void nnDrawMultiObjectInitialPoseBaseMatrixList(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtxlist,
      uint[] nodestatlistptrlist,
      uint subobjtype,
      uint flag,
      int num)
    {
        AppMain.mppAssertNotImpl();
    }
    private void nnSetBoneColor(
      ref AppMain.NNS_RGBA pDiff,
      AppMain.NNS_RGB pAmb,
      ref AppMain.NNS_RGBA pWire)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawOneBoneData(float bonelength, AppMain.NNS_MATRIX mtx, uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnSetEffectorColor(
      ref AppMain.NNS_RGBA pXcol,
      ref AppMain.NNS_RGBA pYcol,
      ref AppMain.NNS_RGBA pZcol)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawEffector(AppMain.NNS_VECTOR p, AppMain.NNS_MATRIX mtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawSIIKBone(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MATRIX mtxlist,
      uint flag)
    {
        AppMain.NNS_RGBA nnsRgba1 = new AppMain.NNS_RGBA(1f, 1f, 1f, 0.5f);
        AppMain.NNS_RGBA nnsRgba2 = new AppMain.NNS_RGBA(1f, 0.0f, 0.0f, 0.5f);
        AppMain.NNS_RGBA nnsRgba3 = new AppMain.NNS_RGBA(0.0f, 1f, 0.0f, 0.5f);
        AppMain.NNS_RGBA nnsRgba4 = new AppMain.NNS_RGBA(0.0f, 0.0f, 1f, 0.5f);
        AppMain.NNS_RGBA nnsRgba5 = new AppMain.NNS_RGBA(1f, 1f, 0.0f, 0.5f);
        AppMain.NNS_RGBA nnsRgba6 = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_RGB nnsRgb = new AppMain.NNS_RGB(0.2f, 0.2f, 0.2f);
        AppMain.NNS_RGBA nnsRgba7 = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.mppAssertNotImpl();
    }

    private void nnMakeNodeTreeMatrix(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR vec,
      AppMain.NNS_VECTOR trans)
    {
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawNodeTree(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MATRIX mtxlist,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawAxis(AppMain.NNS_VECTOR p, float length, AppMain.NNS_MATRIX mtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private uint nnCalcGridBufferSize(int Xnum, int Znum)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private void nnInitGrid(AppMain.NNS_VECTOR pBuf, int Xnum, int Znum)
    {
        this.nngGridPos = pBuf;
        this.nngGridXnum = Xnum;
        this.nngGridZnum = Znum;
    }

    private void nnDrawGrid(AppMain.NNS_VECTOR p, float length, AppMain.NNS_MATRIX mtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawGridPlane(
      int Xnum,
      int Znum,
      float length,
      AppMain.NNS_MATRIX mtx,
      ref AppMain.NNS_RGBA pcolor)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnSetTexCoordSrc(int slot, int src)
    {
        AppMain.nnsTexCoordSrc[slot] = src;
    }

    private static int nnGetTexCoordSrc(int slot)
    {
        return AppMain.nnsTexCoordSrc[slot];
    }

    private static void nnSetNormalFormatType(uint ftype)
    {
        AppMain.nnsNormalFormatType = ftype;
    }

    private uint nnGetNormalFormatType()
    {
        return AppMain.nnsNormalFormatType;
    }

    private static void nnPutEnvironmentTextureMatrix(AppMain.NNS_MATRIX pEnvMtx)
    {
        if (AppMain.nnsTexCoordSrc[0] != 3 && AppMain.nnsTexCoordSrc[1] != 3)
            return;
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMakeTranslateMatrix(nnsMatrix1, 0.5f, 0.5f, 0.0f);
        AppMain.nnScaleMatrix(nnsMatrix1, nnsMatrix1, 0.5f, -0.5f, 0.0f);
        if (pEnvMtx != null)
        {
            AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.nnCopyMatrix(nnsMatrix2, pEnvMtx);
            nnsMatrix2.M03 = 0.0f;
            nnsMatrix2.M13 = 0.0f;
            AppMain.nnMultiplyMatrix(nnsMatrix1, nnsMatrix1, nnsMatrix2);
        }
        if (AppMain.nnsNormalFormatType == 5122U)
        {
            AppMain.nnScaleMatrix(nnsMatrix1, nnsMatrix1, 3.051804E-05f, 3.051804E-05f, 3.051804E-05f);
            AppMain.nnTranslateMatrix(nnsMatrix1, nnsMatrix1, 0.5f, 0.5f, 0.5f);
        }
        OpenGL.glMatrixMode(5890U);
        for (int _slot = 0; _slot < 2; ++_slot)
        {
            if (AppMain.nnsTexCoordSrc[_slot] == 3)
            {
                OpenGL.glActiveTexture(AppMain.NNM_GL_TEXTURE(_slot));
                Matrix matrix = (Matrix)nnsMatrix1;
                OpenGL.glLoadMatrixf(ref matrix);
            }
        }
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix1);
    }

    private static void nnDrawObjectVertexList(AppMain.NNS_VTXLISTPTR vlistptr, uint flag)
    {
        AppMain.nnDrawObjectVertexList(vlistptr, flag, 0U);
    }

    private static void nnMPPVerifyAlternativeLightingSettings()
    {
        OpenGL.BufferItem buffer1 = OpenGL.m_buffers[OpenGL.m_boundArrayBuffer];
        OpenGL.VertexBufferDesc buffer2 = (OpenGL.VertexBufferDesc)buffer1.buffer;
        VertexBuffer buffer3 = buffer2.Buffer;
        uint lightColorAbgr = AppMain.GmMainGetLightColorABGR();
        if ((int)buffer2.VertexColor == (int)lightColorAbgr)
            return;
        if (buffer2.vertices == null)
            buffer2.vertices = new OpenGL.Vertex[buffer3.VertexCount];
        OpenGL.Vertex[] rawData = (OpenGL.Vertex[])buffer1.rawData;
        Array.Copy((Array)rawData, (Array)buffer2.vertices, buffer3.VertexCount);
        for (int index = 0; index < buffer2.vertices.Length; ++index)
        {
            rawData[index].Color.PackedValue = lightColorAbgr;
            Vector2.Transform(ref buffer2.vertices[index].TextureCoordinate, ref buffer2.TextureMatrix, out buffer2.vertices[index].TextureCoordinate);
        }
        buffer3.SetData<OpenGL.Vertex>(rawData);
        buffer2.VertexColor = lightColorAbgr;
    }

    private static void nnDrawObjectVertexList(
      AppMain.NNS_VTXLISTPTR vlistptr,
      uint flag,
      uint alternativeLighting)
    {
        AppMain.NNS_VTXLIST_GL_DESC pVtxList = (AppMain.NNS_VTXLIST_GL_DESC)vlistptr.pVtxList;
        uint type1 = pVtxList.Type;
        int nArray = pVtxList.nArray;
        OpenGL.glBindBuffer(34962U, pVtxList.BufferName);
        if (alternativeLighting != 0U)
        {
            OpenGL.glArray4f glArray4f1 = new OpenGL.glArray4f(0.0f, 0.0f, -1f, 0.0f);
            OpenGL.glArray4f glArray4f2 = ((int)alternativeLighting & 32768) == 0 ? AppMain.GmMainGetLightColorArray4f() : AppMain.BreakWall_1_3_Color;
            OpenGL.glLightfv(16384U, 4612U, ref glArray4f1);
            OpenGL.glLightfv(16384U, 4609U, ref glArray4f2);
            AppMain.nnMPPVerifyAlternativeLightingSettings();
        }
        if (((int)type1 & 1) != 0)
            OpenGL.glEnableClientState(32884U);
        else
            OpenGL.glDisableClientState(32884U);
        if (((int)type1 & 8) != 0)
            OpenGL.glEnableClientState(32885U);
        else
            OpenGL.glDisableClientState(32885U);
        if (((int)flag & 28672) != 0)
            OpenGL.glDisableClientState(32886U);
        else if (((int)type1 & 16) != 0)
            OpenGL.glEnableClientState(32886U);
        else
            OpenGL.glDisableClientState(32886U);
        OpenGL.glClientActiveTexture(33984U);
        if (AppMain.nnGetTexCoordSrc(0) != 0)
            OpenGL.glEnableClientState(32888U);
        else
            OpenGL.glDisableClientState(32888U);
        OpenGL.glClientActiveTexture(33985U);
        if (AppMain.nnGetTexCoordSrc(1) != 0)
            OpenGL.glEnableClientState(32888U);
        else
            OpenGL.glDisableClientState(32888U);
        if (((int)type1 & 2) != 0)
            OpenGL.glEnableClientState(34477U);
        else
            OpenGL.glDisableClientState(34477U);
        if (((int)type1 & 4) != 0)
            OpenGL.glEnableClientState(34884U);
        else
            OpenGL.glDisableClientState(34884U);
        for (int index = 0; index < nArray; ++index)
        {
            AppMain.NNS_VTXARRAY_GL p = pVtxList.pArray[index];
            uint type2 = p.Type;
            if (type2 <= 8U)
            {
                switch ((int)type2 - 1)
                {
                    case 0:
                        OpenGL.glVertexPointer(p.Size, p.DataType, p.Stride, p.Data);
                        if (AppMain.nnGetTexCoordSrc(0) == 4)
                        {
                            OpenGL.glClientActiveTexture(33984U);
                            OpenGL.glEnableClientState(32888U);
                            OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                        }
                        if (AppMain.nnGetTexCoordSrc(1) == 4)
                        {
                            OpenGL.glClientActiveTexture(33985U);
                            OpenGL.glEnableClientState(32888U);
                            OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                            continue;
                        }
                        continue;
                    case 1:
                        OpenGL.glWeightPointerOES(p.Size, p.DataType, p.Stride, p.Data);
                        continue;
                    case 2:
                        continue;
                    case 3:
                        OpenGL.glMatrixIndexPointerOES(p.Size, p.DataType, p.Stride, p.Data);
                        continue;
                    default:
                        if (type2 == 8U)
                        {
                            OpenGL.glNormalPointer(p.DataType, p.Stride, p.Data);
                            AppMain.nnSetNormalFormatType(p.DataType);
                            if (AppMain.nnGetTexCoordSrc(0) == 3)
                            {
                                OpenGL.glClientActiveTexture(33984U);
                                OpenGL.glEnableClientState(32888U);
                                OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                            }
                            if (AppMain.nnGetTexCoordSrc(1) == 3)
                            {
                                OpenGL.glClientActiveTexture(33985U);
                                OpenGL.glEnableClientState(32888U);
                                OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                                continue;
                            }
                            continue;
                        }
                        continue;
                }
            }
            else if (type2 != 16U)
            {
                if (type2 != 256U)
                {
                    if (type2 == 512U)
                    {
                        if (AppMain.nnGetTexCoordSrc(0) == 2)
                        {
                            OpenGL.glClientActiveTexture(33984U);
                            OpenGL.glEnableClientState(32888U);
                            OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                        }
                        if (AppMain.nnGetTexCoordSrc(1) == 2)
                        {
                            OpenGL.glClientActiveTexture(33985U);
                            OpenGL.glEnableClientState(32888U);
                            OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                        }
                    }
                }
                else
                {
                    if (AppMain.nnGetTexCoordSrc(0) == 1)
                    {
                        OpenGL.glClientActiveTexture(33984U);
                        OpenGL.glEnableClientState(32888U);
                        OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                    }
                    if (AppMain.nnGetTexCoordSrc(1) == 1)
                    {
                        OpenGL.glClientActiveTexture(33985U);
                        OpenGL.glEnableClientState(32888U);
                        OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                    }
                }
            }
            else if (((int)flag & 28672) == 0)
                OpenGL.glColorPointer(p.Size, p.DataType, p.Stride, p.Data);
        }
    }

    private static void nnDrawObjectPrimitiveList(AppMain.NNS_PRIMLISTPTR plistptr, uint flag)
    {
        AppMain.NNS_PRIMLIST_GL_DESC pPrimList = (AppMain.NNS_PRIMLIST_GL_DESC)plistptr.pPrimList;
        OpenGL.glBindBuffer(34963U, pPrimList.BufferName);
        switch (flag & 28672U)
        {
            case 4096:
                if (((int)plistptr.fType & 2) != 0)
                    break;
                for (int index1 = 0; index1 < pPrimList.nPrim; ++index1)
                {
                    UShortBuffer pIndex = pPrimList.pIndices[index1];
                    ushort[] numArray1 = new ushort[128];
                    int length = numArray1.Length;
                    OpenGL.glDrawElements(3U, pPrimList.pCounts[index1], pPrimList.DataType, (ushort[])null);
                    for (int index2 = 0; index2 <= 1; ++index2)
                    {
                        int count = 0;
                        for (int index3 = index2; index3 < pPrimList.pCounts[index1]; index3 += 2)
                        {
                            numArray1[count++] = pIndex[index3];
                            if (count == length)
                            {
                                OpenGL.glDrawElements(3U, count, 5123U, (ushort[])null);
                                int num1 = 0;
                                ushort[] numArray2 = numArray1;
                                int index4 = num1;
                                count = index4 + 1;
                                int num2 = (int)pIndex[index3];
                                numArray2[index4] = (ushort)num2;
                            }
                        }
                        if (count >= 2)
                            OpenGL.glDrawElements(3U, count, 5123U, (ushort[])null);
                    }
                }
                break;
            case 12288:
                int nPrim1 = pPrimList.nPrim;
                float[] v = new float[3];
                for (int iStrip = 0; iStrip < nPrim1; ++iStrip)
                {
                    AppMain.nnPutColorStrip(iStrip, AppMain.nngDrawCallBackVal.iMeshset, AppMain.nngDrawCallBackVal.iSubobject);
                    v[0] = (float)AppMain.random.Next(0, (int)short.MaxValue) / (float)short.MaxValue;
                    v[1] = (float)AppMain.random.Next(0, (int)short.MaxValue) / (float)short.MaxValue;
                    v[2] = (float)AppMain.random.Next(0, (int)short.MaxValue) / (float)short.MaxValue;
                    OpenGL.glColor3fv(v);
                    OpenGL.glDrawElements(pPrimList.Mode, pPrimList.pCounts[iStrip], pPrimList.DataType, (ushort[])null);
                }
                break;
            default:
                int nPrim2 = pPrimList.nPrim;
                for (int index = 0; index < nPrim2; ++index)
                    OpenGL.glDrawElements(pPrimList.Mode, pPrimList.pCounts[index], pPrimList.DataType, (ushort[])null);
                break;
        }
    }

    private static void nnDrawObject(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX[] mtxpal,
      uint[] nodestatlist,
      uint subobjtype,
      uint flag,
      uint alternativeLighting)
    {
        int num1 = -1;
        if (((int)flag & 1) != 0 || obj.nSubobj <= 0)
            return;
        int num2 = 0;
        int num3 = 0;
        AppMain.nngDrawCallBackVal.pObject = obj;
        AppMain.nngDrawCallBackVal.pMatrixPalette = mtxpal;
        AppMain.nngDrawCallBackVal.pNodeStatusList = nodestatlist;
        AppMain.nngDrawCallBackVal.DrawSubobjType = subobjtype;
        AppMain.nngDrawCallBackVal.DrawFlag = flag;
        AppMain.nngDrawCallBackVal.iPrevMaterial = -1;
        AppMain.nngDrawCallBackVal.iPrevVtxList = -1;
        if (subobjtype == 2147483648U)
            subobjtype = 775U;
        OpenGL.glShadeModel(7425U);
        for (int index1 = 0; index1 < obj.nSubobj; ++index1)
        {
            AppMain.NNS_SUBOBJ pSubobj = obj.pSubobjList[index1];
            if (((int)pSubobj.fType & (int)subobjtype & 7) != 0 && ((int)pSubobj.fType & (int)subobjtype & 768) != 0)
            {
                AppMain.nngDrawCallBackVal.iSubobject = index1;
                if (((int)pSubobj.fType & 512) != 0)
                {
                    OpenGL.glMatrixMode(5888U);
                    OpenGL.glLoadIdentity();
                }
                for (int index2 = 0; index2 < pSubobj.nMeshset; ++index2)
                {
                    AppMain.NNS_MESHSET pMeshset = pSubobj.pMeshsetList[index2];
                    AppMain.NNS_VTXLISTPTR pVtxListPtr = obj.pVtxListPtrList[pMeshset.iVtxList];
                    AppMain.NNS_PRIMLISTPTR pPrimListPtr = obj.pPrimListPtrList[pMeshset.iPrimList];
                    if (nodestatlist == null || ((int)nodestatlist[pMeshset.iNode] & 1) == 0)
                    {
                        AppMain.nngDrawCallBackVal.iMeshset = index2;
                        AppMain.nngDrawCallBackVal.iNode = pMeshset.iNode;
                        AppMain.nngDrawCallBackVal.iMaterial = pMeshset.iMaterial;
                        AppMain.nngDrawCallBackVal.pMaterial = obj.pMatPtrList[pMeshset.iMaterial];
                        AppMain.nngDrawCallBackVal.iVtxList = pMeshset.iVtxList;
                        AppMain.nngDrawCallBackVal.pVtxListPtr = pVtxListPtr;
                        AppMain.nngDrawCallBackVal.bModified = 0;
                        AppMain.nngDrawCallBackVal.bReDraw = 0;
                        OpenGL.glMatrixMode(5888U);
                        if (((int)pSubobj.fType & 256) != 0)
                        {
                            Matrix matrix = (Matrix)mtxpal[pMeshset.iMatrix];
                            OpenGL.glLoadMatrixf(ref matrix);
                        }
                        else
                            OpenGL.glLoadIdentity();
                        while (AppMain.nnPutMaterial(AppMain.nngDrawCallBackVal) != 0)
                        {
                            AppMain.nngDrawCallBackVal.iPrevMaterial = pMeshset.iMaterial;
                            AppMain.nngDrawCallBackVal.iPrevVtxList = pMeshset.iVtxList;
                            if (((int)pVtxListPtr.fType & 1) != 0)
                            {
                                if (((int)flag & 768) != 0)
                                {
                                    AppMain.nnDrawObjectNormal(pVtxListPtr, pPrimListPtr, mtxpal, flag);
                                }
                                else
                                {
                                    if (((int)pSubobj.fType & 512) != 0)
                                    {
                                        AppMain.NNS_VTXLIST_GL_DESC pVtxList = (AppMain.NNS_VTXLIST_GL_DESC)pVtxListPtr.pVtxList;
                                        int nMatrix = pVtxList.nMatrix;
                                        OpenGL.glEnable(34880U);
                                        OpenGL.glMatrixMode(34880U);
                                        for (uint matrixpaletteindex = 0; (long)matrixpaletteindex < (long)nMatrix; ++matrixpaletteindex)
                                        {
                                            OpenGL.glCurrentPaletteMatrixOES(matrixpaletteindex);
                                            Matrix matrix = (Matrix)mtxpal[(int)pVtxList.pMatrixIndices[(int)matrixpaletteindex]];
                                            OpenGL.glLoadMatrixf(ref matrix);
                                        }
                                    }
                                    else
                                        OpenGL.glDisable(34880U);
                                    if (num1 != pMeshset.iVtxList || num2 != AppMain.nnGetTexCoordSrc(0) || num3 != AppMain.nnGetTexCoordSrc(1))
                                    {
                                        AppMain.nnDrawObjectVertexList(pVtxListPtr, flag, alternativeLighting);
                                        num1 = pMeshset.iVtxList;
                                        num2 = AppMain.nnGetTexCoordSrc(0);
                                        num3 = AppMain.nnGetTexCoordSrc(1);
                                        if (pMeshset.iMatrix != -1)
                                            AppMain.nnPutEnvironmentTextureMatrix(mtxpal[pMeshset.iMatrix]);
                                        else
                                            AppMain.nnPutEnvironmentTextureMatrix(mtxpal[0]);
                                    }
                                    AppMain.nnDrawObjectPrimitiveList(pPrimListPtr, flag);
                                }
                            }
                            else
                            {
                                int num4 = (int)pVtxListPtr.fType & 16711680;
                            }
                            if (AppMain.nngDrawCallBackVal.bReDraw == 0)
                                break;
                        }
                    }
                }
            }
        }
        OpenGL.glDisable(34880U);
    }

    private uint nnEstimateCameraBufferSize(uint type)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    public static void nnSetClipPlane()
    {
        if (AppMain.nngProjectionType != 1)
        {
            float num1 = 1f / AppMain.nngProjectionMatrix.M00;
            float num2 = 1f / AppMain.nngProjectionMatrix.M11;
            float num3 = num2 * (AppMain.nngProjectionMatrix.M12 - AppMain.nngClip3d.y0 / AppMain.nngScreen.cy);
            float num4 = AppMain.nnSqrt((float)((double)num3 * (double)num3 + 1.0));
            float num5 = num3 / num4;
            float num6 = 1f / num4;
            AppMain.nngClipPlane.Top.ny = num6;
            AppMain.nngClipPlane.Top.nz = num5;
            float num7 = num2 * (AppMain.nngProjectionMatrix.M12 - AppMain.nngClip3d.y1 / AppMain.nngScreen.cy);
            float num8 = AppMain.nnSqrt((float)((double)num7 * (double)num7 + 1.0));
            float num9 = num7 / num8;
            float num10 = 1f / num8;
            AppMain.nngClipPlane.Bottom.ny = -num10;
            AppMain.nngClipPlane.Bottom.nz = -num9;
            float num11 = num1 * (AppMain.nngProjectionMatrix.M02 + AppMain.nngClip3d.x1 / AppMain.nngScreen.cx);
            float num12 = AppMain.nnSqrt((float)((double)num11 * (double)num11 + 1.0));
            float num13 = num11 / num12;
            float num14 = 1f / num12;
            AppMain.nngClipPlane.Right.nx = num14;
            AppMain.nngClipPlane.Right.nz = num13;
            float num15 = num1 * (AppMain.nngProjectionMatrix.M02 + AppMain.nngClip3d.x0 / AppMain.nngScreen.cx);
            float num16 = AppMain.nnSqrt((float)((double)num15 * (double)num15 + 1.0));
            float num17 = num15 / num16;
            float num18 = 1f / num16;
            AppMain.nngClipPlane.Left.nx = -num18;
            AppMain.nngClipPlane.Left.nz = -num17;
        }
        else
        {
            float num1 = (float)(2.0 / (double)AppMain.nngProjectionMatrix.M11 / 2.0);
            float num2 = (float)-((double)AppMain.nngProjectionMatrix.M13 / (double)AppMain.nngProjectionMatrix.M11);
            AppMain.nngClipPlane.Top.mul = num1 * (AppMain.nngClip3d.y0 / -AppMain.nngScreen.cy);
            AppMain.nngClipPlane.Top.ofs = num2;
            AppMain.nngClipPlane.Bottom.mul = (float)(-(double)num1 * ((double)AppMain.nngClip3d.y1 / (double)AppMain.nngScreen.cy));
            AppMain.nngClipPlane.Bottom.ofs = num2;
            float num3 = (float)(2.0 / (double)AppMain.nngProjectionMatrix.M00 / 2.0);
            float num4 = (float)-((double)AppMain.nngProjectionMatrix.M03 / (double)AppMain.nngProjectionMatrix.M00);
            AppMain.nngClipPlane.Right.mul = num3 * (AppMain.nngClip3d.x0 / -AppMain.nngScreen.cx);
            AppMain.nngClipPlane.Right.ofs = num4;
            AppMain.nngClipPlane.Left.mul = (float)(-(double)num3 * ((double)AppMain.nngClip3d.x1 / (double)AppMain.nngScreen.cx));
            AppMain.nngClipPlane.Left.ofs = num4;
        }
    }

    public void nnSetClipScreenCoordinates(AppMain.NNS_VECTOR2D pos)
    {
        AppMain.mppAssertNotImpl();
    }

    public void nnSetClipZ(float znear, float zfar)
    {
        AppMain.mppAssertNotImpl();
    }

    private int nnGetNodeIndex(AppMain.NNS_NODENAMELIST pNodeNameList, string NodeName)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private string nnGetNodeName(AppMain.NNS_NODENAMELIST pNodeNameList, int NodeIndex)
    {
        AppMain.mppAssertNotImpl();
        return (string)null;
    }


    private void nnSetNormalLength(float len)
    {
        AppMain.nngNormalLength = len;
    }

    private void nnSetNormalColor(float r, float g, float b, float a)
    {
        AppMain.nngNormalColor.r = r;
        AppMain.nngNormalColor.g = g;
        AppMain.nngNormalColor.b = b;
        AppMain.nngNormalColor.a = a;
    }

    private void nnSetWireColor(float r, float g, float b, float a)
    {
        AppMain.nngWireColor.r = r;
        AppMain.nngWireColor.g = g;
        AppMain.nngWireColor.b = b;
        AppMain.nngWireColor.a = a;
    }

    private static void nnPutWireColor()
    {
        OpenGL.glColor4fv((OpenGL.glArray4f)AppMain.nngWireColor);
    }

    private static void nnDrawObjectNormal(
      AppMain.NNS_VTXLISTPTR vlistptr,
      AppMain.NNS_PRIMLISTPTR plistptr,
      AppMain.NNS_MATRIX[] mtxpal,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    public static uint NNM_CHUNK_ID(char a, char b, char c, char d)
    {
        return (uint)(((int)d & (int)byte.MaxValue) << 24 | ((int)c & (int)byte.MaxValue) << 16 | ((int)b & (int)byte.MaxValue) << 8 | (int)a & (int)byte.MaxValue);
    }

    private static void nnInitPreviousMaterialValueGL()
    {
        AppMain.nnmaterialcore.nngPreMatFlag = uint.MaxValue;
        AppMain.nnmaterialcore.nngpPreMatColor = (object)null;
        AppMain.nnmaterialcore.nngpPreMatLogic = (object)null;
    }

    private static void nnPutMaterialFlagGL(AppMain.NNS_DRAWCALLBACK_VAL val, uint fMatFlag)
    {
        switch (val.DrawFlag & 96U)
        {
            case 32:
                OpenGL.glDisable(2884U);
                break;
            case 64:
                OpenGL.glEnable(2884U);
                OpenGL.glCullFace(1028U);
                break;
            case 96:
                OpenGL.glEnable(2884U);
                OpenGL.glCullFace(1029U);
                break;
            default:
                if (((int)fMatFlag & 9) != 0)
                {
                    OpenGL.glDisable(2884U);
                    break;
                }
                OpenGL.glEnable(2884U);
                OpenGL.glCullFace(1029U);
                break;
        }
        if (((int)fMatFlag & 24) != 0)
            OpenGL.glLightModelf(2898U, 1f);
        else
            OpenGL.glLightModelf(2898U, 0.0f);
        if (((int)val.DrawFlag & 128) != 0 || ((int)fMatFlag & 2) != 0)
            OpenGL.glDisable(2896U);
        else
            OpenGL.glEnable(2896U);
        AppMain.nnPutFogSwitchGL(AppMain.nngFogSwitch && 0 == ((int)fMatFlag & 4));
        OpenGL.glDepthMask(((int)fMatFlag & 256) == 0 ? (byte)1 : (byte)0);
        if (((int)val.DrawFlag & int.MinValue) != 0)
            OpenGL.glColorMask((byte)0, (byte)0, (byte)0, (byte)0);
        else
            OpenGL.glColorMask(((int)fMatFlag & 512) == 0 ? (byte)1 : (byte)0, ((int)fMatFlag & 1024) == 0 ? (byte)1 : (byte)0, ((int)fMatFlag & 2048) == 0 ? (byte)1 : (byte)0, ((int)fMatFlag & 4096) == 0 ? (byte)1 : (byte)0);
        AppMain.nnmaterialcore.nngPreMatFlag = fMatFlag;
    }

    private static void nnPutMaterialColorGL(
      uint face,
      AppMain.NNS_DRAWCALLBACK_VAL val,
      AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor)
    {
        if (((int)pColor.fFlag & 1) != 0)
            OpenGL.glEnable(2903U);
        else
            OpenGL.glDisable(2903U);
        if (((int)val.DrawFlag & 334498816) == 0)
        {
            OpenGL.glMaterialfv(face, 4608U, (OpenGL.glArray4f)pColor.Ambient);
            OpenGL.glMaterialfv(face, 4609U, (OpenGL.glArray4f)pColor.Diffuse);
            OpenGL.glColor4fv((OpenGL.glArray4f)pColor.Diffuse);
            if (((int)pColor.fFlag & 2) != 0)
            {
                OpenGL.glArray4f glArray4f;
                glArray4f.f0 = pColor.Specular.r * pColor.SpecularIntensity;
                glArray4f.f1 = pColor.Specular.g * pColor.SpecularIntensity;
                glArray4f.f2 = pColor.Specular.b * pColor.SpecularIntensity;
                glArray4f.f3 = pColor.Specular.a;
                OpenGL.glMaterialfv(face, 4610U, glArray4f);
            }
            else
                OpenGL.glMaterialfv(face, 4610U, (OpenGL.glArray4f)pColor.Specular);
        }
        else
        {
            if (((int)val.DrawFlag & 2097152) != 0)
            {
                switch (AppMain.nngMatCtrlAmbient.mode)
                {
                    case 1:
                        OpenGL.glArray4f glArray4f1;
                        glArray4f1.f0 = AppMain.nngMatCtrlAmbient.col.r;
                        glArray4f1.f1 = AppMain.nngMatCtrlAmbient.col.g;
                        glArray4f1.f2 = AppMain.nngMatCtrlAmbient.col.b;
                        glArray4f1.f3 = 1f;
                        OpenGL.glMaterialfv(face, 4608U, glArray4f1);
                        break;
                    case 2:
                        OpenGL.glArray4f glArray4f2;
                        glArray4f2.f0 = pColor.Ambient.r + AppMain.nngMatCtrlAmbient.col.r;
                        glArray4f2.f1 = pColor.Ambient.g + AppMain.nngMatCtrlAmbient.col.g;
                        glArray4f2.f2 = pColor.Ambient.b + AppMain.nngMatCtrlAmbient.col.b;
                        glArray4f2.f3 = pColor.Ambient.a;
                        OpenGL.glMaterialfv(face, 4608U, glArray4f2);
                        break;
                    case 3:
                        OpenGL.glArray4f glArray4f3;
                        glArray4f3.f0 = pColor.Ambient.r * AppMain.nngMatCtrlAmbient.col.r;
                        glArray4f3.f1 = pColor.Ambient.g * AppMain.nngMatCtrlAmbient.col.g;
                        glArray4f3.f2 = pColor.Ambient.b * AppMain.nngMatCtrlAmbient.col.b;
                        glArray4f3.f3 = pColor.Ambient.a;
                        OpenGL.glMaterialfv(face, 4608U, glArray4f3);
                        break;
                    default:
                        OpenGL.glMaterialfv(face, 4608U, (OpenGL.glArray4f)pColor.Ambient);
                        break;
                }
            }
            else
                OpenGL.glMaterialfv(face, 4608U, (OpenGL.glArray4f)pColor.Ambient);
            if (((int)val.DrawFlag & 9437184) != 0)
            {
                OpenGL.glArray4f v;
                if (((int)val.DrawFlag & 1048576) != 0)
                {
                    switch (AppMain.nngMatCtrlDiffuse.mode)
                    {
                        case 1:
                            v.f0 = AppMain.nngMatCtrlDiffuse.col.r;
                            v.f1 = AppMain.nngMatCtrlDiffuse.col.g;
                            v.f2 = AppMain.nngMatCtrlDiffuse.col.b;
                            break;
                        case 2:
                            v.f0 = pColor.Diffuse.r + AppMain.nngMatCtrlDiffuse.col.r;
                            v.f1 = pColor.Diffuse.g + AppMain.nngMatCtrlDiffuse.col.g;
                            v.f2 = pColor.Diffuse.b + AppMain.nngMatCtrlDiffuse.col.b;
                            break;
                        case 3:
                            v.f0 = pColor.Diffuse.r * AppMain.nngMatCtrlDiffuse.col.r;
                            v.f1 = pColor.Diffuse.g * AppMain.nngMatCtrlDiffuse.col.g;
                            v.f2 = pColor.Diffuse.b * AppMain.nngMatCtrlDiffuse.col.b;
                            break;
                        default:
                            v.f0 = pColor.Diffuse.r;
                            v.f1 = pColor.Diffuse.g;
                            v.f2 = pColor.Diffuse.b;
                            break;
                    }
                }
                else
                {
                    v.f0 = pColor.Diffuse.r;
                    v.f1 = pColor.Diffuse.g;
                    v.f2 = pColor.Diffuse.b;
                }
                if (((int)val.DrawFlag & 8388608) != 0)
                {
                    switch (AppMain.nngMatCtrlAlpha.mode)
                    {
                        case 1:
                            v.f3 = AppMain.nngMatCtrlAlpha.alpha;
                            break;
                        case 2:
                            v.f3 = pColor.Diffuse.a + AppMain.nngMatCtrlAlpha.alpha;
                            break;
                        case 3:
                            v.f3 = pColor.Diffuse.a * AppMain.nngMatCtrlAlpha.alpha;
                            break;
                        default:
                            v.f3 = pColor.Diffuse.a;
                            break;
                    }
                }
                else
                    v.f3 = pColor.Diffuse.a;
                OpenGL.glMaterialfv(face, 4609U, v);
                OpenGL.glColor4fv(v);
            }
            else
            {
                OpenGL.glMaterialfv(face, 4609U, (OpenGL.glArray4f)pColor.Diffuse);
                OpenGL.glColor4fv((OpenGL.glArray4f)pColor.Diffuse);
            }
            if (((int)val.DrawFlag & 1024) != 0)
            {
                OpenGL.glMaterialfv(face, 4610U, (OpenGL.glArray4f)AppMain.nngColorBlack);
            }
            else
            {
                OpenGL.glArray4f glArray4f4;
                if (((int)pColor.fFlag & 2) != 0)
                {
                    glArray4f4.f0 = pColor.Specular.r * pColor.SpecularIntensity;
                    glArray4f4.f1 = pColor.Specular.g * pColor.SpecularIntensity;
                    glArray4f4.f2 = pColor.Specular.b * pColor.SpecularIntensity;
                    glArray4f4.f3 = pColor.Specular.a;
                }
                else
                {
                    glArray4f4.f0 = pColor.Specular.r;
                    glArray4f4.f1 = pColor.Specular.g;
                    glArray4f4.f2 = pColor.Specular.b;
                    glArray4f4.f3 = pColor.Specular.a;
                }
                if (((int)val.DrawFlag & 4194304) != 0)
                {
                    switch (AppMain.nngMatCtrlSpecular.mode)
                    {
                        case 1:
                            glArray4f4.f0 = AppMain.nngMatCtrlSpecular.col.r;
                            glArray4f4.f1 = AppMain.nngMatCtrlSpecular.col.g;
                            glArray4f4.f2 = AppMain.nngMatCtrlSpecular.col.b;
                            glArray4f4.f3 = 1f;
                            break;
                        case 2:
                            glArray4f4.f0 += AppMain.nngMatCtrlSpecular.col.r;
                            glArray4f4.f1 += AppMain.nngMatCtrlSpecular.col.g;
                            glArray4f4.f2 += AppMain.nngMatCtrlSpecular.col.b;
                            break;
                        case 3:
                            glArray4f4.f0 *= AppMain.nngMatCtrlSpecular.col.r;
                            glArray4f4.f1 *= AppMain.nngMatCtrlSpecular.col.g;
                            glArray4f4.f2 *= AppMain.nngMatCtrlSpecular.col.b;
                            break;
                    }
                }
                OpenGL.glMaterialfv(face, 4610U, glArray4f4);
            }
        }
        float num = (double)pColor.Shininess <= (double)AppMain.nngGLExtensions.max_shininess ? pColor.Shininess : AppMain.nngGLExtensions.max_shininess;
        OpenGL.glMaterialf(face, 5633U, num);
        OpenGL.glMaterialfv(face, 5632U, (OpenGL.glArray4f)pColor.Emission);
        AppMain.nnmaterialcore.nngpPreMatColor = (object)pColor;
    }

    private static void nnPutMaterialLogicGL(
      AppMain.NNS_DRAWCALLBACK_VAL val,
      AppMain.NNS_MATERIAL_LOGIC pLogic)
    {
        uint fFlag = pLogic.fFlag;
        if (((int)val.DrawFlag & 33554432) != 0)
        {
            OpenGL.glEnable(3042U);
            switch (AppMain.nngMatCtrlBlendMode.blendmode)
            {
                case 0:
                    OpenGL.glBlendFunc(770U, 771U);
                    OpenGL.glBlendEquation(32774U);
                    break;
                case 1:
                    OpenGL.glBlendFunc(770U, 1U);
                    OpenGL.glBlendEquation(32774U);
                    break;
                case 2:
                    OpenGL.glBlendFunc(770U, 1U);
                    OpenGL.glBlendEquation(32779U);
                    break;
            }
        }
        else if (((int)fFlag & 1) != 0)
        {
            OpenGL.glEnable(3042U);
            if (((int)fFlag & 2) != 0)
                OpenGL.glBlendFunc((uint)pLogic.SrcFactorRGB, (uint)pLogic.DstFactorRGB);
            else
                OpenGL.glBlendFunc((uint)pLogic.SrcFactorRGB, (uint)pLogic.DstFactorRGB);
            OpenGL.glBlendEquation((uint)pLogic.BlendOp);
        }
        else
            OpenGL.glDisable(3042U);
        if (((int)fFlag & 4) != 0)
        {
            OpenGL.glEnable(3058U);
            OpenGL.glLogicOp((uint)pLogic.LogicOp);
        }
        else
            OpenGL.glDisable(3058U);
        if (((int)val.DrawFlag & 1) == 0 && ((int)fFlag & 8) != 0)
        {
            OpenGL.glEnable(3008U);
            OpenGL.glAlphaFunc((uint)pLogic.AlphaFunc, pLogic.AlphaRef);
        }
        else
            OpenGL.glDisable(3008U);
        if (((int)fFlag & 16) != 0)
        {
            OpenGL.glEnable(2929U);
            OpenGL.glDepthFunc((uint)pLogic.DepthFunc);
        }
        else
            OpenGL.glDisable(2929U);
        AppMain.nnmaterialcore.nngpPreMatLogic = (object)pLogic;
    }

    private static void nnPutMaterialLogicGLES11(
      AppMain.NNS_DRAWCALLBACK_VAL val,
      AppMain.NNS_MATERIAL_GLES11_LOGIC pLogic)
    {
        uint fFlag = pLogic.fFlag;
        if (((int)val.DrawFlag & 33554432) != 0)
        {
            OpenGL.glEnable(3042U);
            switch (AppMain.nngMatCtrlBlendMode.blendmode)
            {
                case 0:
                    OpenGL.glBlendFunc(770U, 771U);
                    OpenGL.glBlendEquation(32774U);
                    break;
                case 1:
                    OpenGL.glBlendFunc(770U, 1U);
                    OpenGL.glBlendEquation(32774U);
                    break;
                case 2:
                    OpenGL.glBlendFunc(770U, 1U);
                    OpenGL.glBlendEquation(32779U);
                    break;
            }
        }
        else if (((int)fFlag & 1) != 0)
        {
            OpenGL.glEnable(3042U);
            OpenGL.glBlendFunc((uint)pLogic.SrcFactor, (uint)pLogic.DstFactor);
            OpenGL.glBlendEquation((uint)pLogic.BlendOp);
        }
        else
            OpenGL.glDisable(3042U);
        if (((int)fFlag & 4) != 0)
        {
            OpenGL.glEnable(3058U);
            OpenGL.glLogicOp((uint)pLogic.LogicOp);
        }
        else
            OpenGL.glDisable(3058U);
        if (((int)val.DrawFlag & 1) == 0 && ((int)fFlag & 8) != 0)
        {
            OpenGL.glEnable(3008U);
            OpenGL.glAlphaFunc((uint)pLogic.AlphaFunc, pLogic.AlphaRef);
        }
        else
            OpenGL.glDisable(3008U);
        if (((int)fFlag & 16) != 0)
        {
            OpenGL.glEnable(2929U);
            OpenGL.glDepthFunc((uint)pLogic.DepthFunc);
        }
        else
            OpenGL.glDisable(2929U);
        AppMain.nnmaterialcore.nngpPreMatLogic = (object)pLogic;
    }

    private void nnPutMaterialTextureShadowMap(
      int slot,
      AppMain.NNS_DRAWCALLBACK_VAL val,
      AppMain.NNE_SHADOWMAP idx)
    {
    }

    private uint nnGetTextureMask(uint flag)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private void nnPutMaterialTextureOneGL(
      int slot,
      AppMain.NNS_DRAWCALLBACK_VAL val,
      AppMain.NNS_MATERIAL_TEXMAP_DESC pTex)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnPutMaterialTextureOneGLES11(
      int slot,
      AppMain.NNS_DRAWCALLBACK_VAL val,
      ref AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC pTex)
    {
        uint fType = pTex.fType;
        uint num = fType & 3841U;
        if (pTex.pTexInfo != null)
            AppMain.nnSetTexInfo(slot, (AppMain.NNS_TEXINFO)pTex.pTexInfo);
        else
            AppMain.nnSetTextureNum(slot, pTex.iTexIdx);
        int src;
        switch (num)
        {
            case 1:
                if (((int)val.DrawFlag & 16777216) != 0)
                {
                    switch (AppMain.nngMatCtrlEnvTexMtx.texcoordsrc)
                    {
                        case 0:
                            src = 4;
                            break;
                        default:
                            src = 3;
                            break;
                    }
                }
                else
                {
                    src = 3;
                    break;
                }
                break;
            case 512:
                src = 2;
                break;
            default:
                src = 1;
                break;
        }
        AppMain.nnSetTexCoordSrc(slot, src);
        if (src != 3)
        {
            OpenGL.glMatrixMode(5890U);
            OpenGL.glLoadIdentity();
            OpenGL.glTranslatef(0.0f, 1f, 0.0f);
            OpenGL.glScalef(1f, -1f, 1f);
            if (((int)val.DrawFlag & 268435456) != 0)
            {
                switch (AppMain.nngMatCtrlTexOffset[slot].mode)
                {
                    case 1:
                        OpenGL.glTranslatef(AppMain.nngMatCtrlTexOffset[slot].offset.u, AppMain.nngMatCtrlTexOffset[slot].offset.v, 0.0f);
                        break;
                    case 2:
                        OpenGL.glTranslatef(pTex.Offset.u + AppMain.nngMatCtrlTexOffset[slot].offset.u, pTex.Offset.v + AppMain.nngMatCtrlTexOffset[slot].offset.v, 0.0f);
                        break;
                    case 3:
                        OpenGL.glTranslatef(pTex.Offset.u * AppMain.nngMatCtrlTexOffset[slot].offset.u, pTex.Offset.v * AppMain.nngMatCtrlTexOffset[slot].offset.v, 0.0f);
                        break;
                    default:
                        if (((int)fType & 1073741824) == 0)
                        {
                            OpenGL.glTranslatef(pTex.Offset.u, pTex.Offset.v, 0.0f);
                            break;
                        }
                        break;
                }
            }
            else if (((int)fType & 1073741824) == 0)
                OpenGL.glTranslatef(pTex.Offset.u, pTex.Offset.v, 0.0f);
            if (((int)fType & 65536) != 0)
                OpenGL.glScalef(pTex.Scale.u, pTex.Scale.v, 1f);
        }
        OpenGL.glTexEnvi(8960U, 8704U, pTex.EnvMode);
        if (pTex.pCombine != null)
        {
            AppMain.NNS_TEXTURE_GLES11_COMBINE pCombine = pTex.pCombine;
            OpenGL.glTexEnvi(8960U, 34161U, (int)pCombine.CombineRGB);
            OpenGL.glTexEnvi(8960U, 34176U, (int)pCombine.Source0RGB);
            OpenGL.glTexEnvi(8960U, 34192U, (int)pCombine.Operand0RGB);
            OpenGL.glTexEnvi(8960U, 34177U, (int)pCombine.Source1RGB);
            OpenGL.glTexEnvi(8960U, 34193U, (int)pCombine.Operand1RGB);
            OpenGL.glTexEnvi(8960U, 34178U, (int)pCombine.Source2RGB);
            OpenGL.glTexEnvi(8960U, 34194U, (int)pCombine.Operand2RGB);
            OpenGL.glTexEnvi(8960U, 34162U, (int)pCombine.CombineAlpha);
            OpenGL.glTexEnvi(8960U, 34184U, (int)pCombine.Source0Alpha);
            OpenGL.glTexEnvi(8960U, 34200U, (int)pCombine.Operand0Alpha);
            OpenGL.glTexEnvi(8960U, 34185U, (int)pCombine.Source1Alpha);
            OpenGL.glTexEnvi(8960U, 34201U, (int)pCombine.Operand1Alpha);
            OpenGL.glTexEnvi(8960U, 34186U, (int)pCombine.Source2Alpha);
            OpenGL.glTexEnvi(8960U, 34202U, (int)pCombine.Operand2Alpha);
            OpenGL.glTexEnvfv(8960U, 8705U, (OpenGL.glArray4f)pCombine.EnvColor);
        }
        OpenGL.glTexParameteri(3553U, 10242U, pTex.WrapS);
        OpenGL.glTexParameteri(3553U, 10243U, pTex.WrapT);
        if (pTex.pFilterMode == null)
            return;
        AppMain.NNS_TEXTURE_FILTERMODE pFilterMode = pTex.pFilterMode;
        OpenGL.glTexParameteri(3553U, 10240U, (int)pFilterMode.MagFilter);
        OpenGL.glTexParameteri(3553U, 10241U, (int)pFilterMode.MinFilter);
    }

    private static void nnPutMaterialTexturesGL(
      AppMain.NNS_DRAWCALLBACK_VAL val,
      AppMain.NNS_MATERIAL_TEXMAP_DESC[] texdesc,
      int num)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnPutMaterialTexturesGLES11(
      AppMain.NNS_DRAWCALLBACK_VAL val,
      AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[] texdesc,
      int num)
    {
        int maxTextureUnits = AppMain.nngGLExtensions.max_texture_units;
        int slot = 0;
        for (int index = 0; index < num; ++index)
        {
            AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC pTex = texdesc[index];
            AppMain.nnPutMaterialTextureOneGLES11(slot, val, ref pTex);
            ++slot;
            if (slot >= maxTextureUnits)
                return;
        }
        for (; slot < maxTextureUnits; ++slot)
        {
            AppMain.nnSetTextureNum(slot, -1);
            AppMain.nnSetTexCoordSrc(slot, 0);
        }
    }

    private void nnPutMaterialStdShaderTextureOneGL(
      int slot,
      AppMain.NNS_DRAWCALLBACK_VAL val,
      AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC pTex)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnPutMaterialStdShaderTexturesGL(
      AppMain.NNS_DRAWCALLBACK_VAL val,
      AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC texdesc,
      int num)
    {
        AppMain.mppAssertNotImpl();
    }

    private static int nnPutMaterialCore(AppMain.NNS_DRAWCALLBACK_VAL val)
    {
        AppMain.NNS_MATERIALPTR pMaterial1 = val.pMaterial;
        if (((int)val.DrawFlag & 29440) != 0)
        {
            AppMain.nnPutDisableTexturesGL();
            if (((int)val.DrawFlag & 768) != 0)
            {
                AppMain.nnPutFixedMaterialGL();
                return 1;
            }
            if (((int)val.DrawFlag & 28672) != 0)
            {
                AppMain.nnPutFixedMaterialGL();
                switch (val.DrawFlag & 96U)
                {
                    case 32:
                        OpenGL.glDisable(2884U);
                        break;
                    case 64:
                        OpenGL.glEnable(2884U);
                        OpenGL.glCullFace(1028U);
                        break;
                    case 96:
                        OpenGL.glEnable(2884U);
                        OpenGL.glCullFace(1029U);
                        break;
                    default:
                        if (((int)((AppMain.NNS_MATERIAL_DESC)pMaterial1.pMaterial).fFlag & 1) != 0)
                        {
                            OpenGL.glDisable(2884U);
                            break;
                        }
                        OpenGL.glEnable(2884U);
                        OpenGL.glCullFace(1029U);
                        break;
                }
                switch (val.DrawFlag & 28672U)
                {
                    case 4096:
                        AppMain.nnPutWireColor();
                        break;
                    case 8192:
                        if (((int)pMaterial1.fType & 8) != 0)
                        {
                            AppMain.nnPutColorNTexture(((AppMain.NNS_MATERIAL_GLES11_DESC)pMaterial1.pMaterial).nTex);
                            break;
                        }
                        AppMain.nnPutColorNTexture(0);
                        break;
                    case 16384:
                        AppMain.nnPutColorMeshset(val.iMeshset, val.iSubobject);
                        break;
                    case 20480:
                        AppMain.nnPutColorMaterial(val.iMaterial);
                        break;
                    case 24576:
                        AppMain.nnPutColorNWeight(val.pVtxListPtr);
                        break;
                }
                return 1;
            }
        }
        if (val.iPrevMaterial == -1 || val.bModified != 0)
            AppMain.nnInitPreviousMaterialValueGL();
        else if (val.iMaterial == val.iPrevMaterial)
            return 1;
        if (((int)pMaterial1.fType & 8) != 0)
        {
            AppMain.NNS_MATERIAL_GLES11_DESC pMaterial2 = (AppMain.NNS_MATERIAL_GLES11_DESC)pMaterial1.pMaterial;
            uint fFlag = pMaterial2.fFlag;
            if ((int)fFlag != (int)AppMain.nnmaterialcore.nngPreMatFlag)
                AppMain.nnPutMaterialFlagGL(val, fFlag);
            if (pMaterial2.pColor != AppMain.nnmaterialcore.nngpPreMatColor)
                AppMain.nnPutMaterialColorGL(1032U, val, pMaterial2.pColor);
            if (pMaterial2.pLogic != AppMain.nnmaterialcore.nngpPreMatLogic)
                AppMain.nnPutMaterialLogicGLES11(val, pMaterial2.pLogic);
            if (((int)val.DrawFlag & 2048) == 0)
                AppMain.nnPutMaterialTexturesGLES11(val, pMaterial2.pTexDesc, pMaterial2.nTex);
            else
                AppMain.nnPutDisableTexturesGL();
        }
        else if (((int)pMaterial1.fType & 1) != 0)
        {
            AppMain.NNS_MATERIAL_DESC pMaterial2 = (AppMain.NNS_MATERIAL_DESC)pMaterial1.pMaterial;
            uint fFlag = pMaterial2.fFlag;
            if ((int)fFlag != (int)AppMain.nnmaterialcore.nngPreMatFlag)
                AppMain.nnPutMaterialFlagGL(val, fFlag);
            if (pMaterial2.pColor != AppMain.nnmaterialcore.nngpPreMatColor)
                AppMain.nnPutMaterialColorGL(1032U, val, (AppMain.NNS_MATERIAL_STDSHADER_COLOR)pMaterial2.pColor);
            if (pMaterial2.pLogic != AppMain.nnmaterialcore.nngpPreMatLogic)
                AppMain.nnPutMaterialLogicGL(val, pMaterial2.pLogic);
            if (((int)val.DrawFlag & 2048) == 0)
                AppMain.nnPutMaterialTexturesGL(val, pMaterial2.pTexDesc, pMaterial2.nTex);
            else
                AppMain.nnPutDisableTexturesGL();
        }
        return 1;
    }

    private void nnInitCircumsphere()
    {
        AppMain.ArrayPointer<AppMain.NNS_VECTOR> _pointer1 = new AppMain.ArrayPointer<AppMain.NNS_VECTOR>(this.nngCircumPoint);
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float s1;
        float c1;
        float s2;
        float c2;
        for (int index1 = 0; index1 < 2; ++index1)
        {
            AppMain.nnSinCos(AppMain.NNM_DEGtoA32((int)((double)index1 * 360.0 / 4.0)), out s1, out c1);
            AppMain.nnSinCos(0, out s2, out c2);
            for (int index2 = 0; index2 < 20; ++index2)
            {
                ((AppMain.NNS_VECTOR)~_pointer1).x = c2 * c1 + num1;
                ((AppMain.NNS_VECTOR)~_pointer1).y = s2 + num2;
                ((AppMain.NNS_VECTOR)~_pointer1).z = c2 * s1 + num3;
                AppMain.ArrayPointer<AppMain.NNS_VECTOR> _pointer2 = ++(_pointer1);
                AppMain.nnSinCos(AppMain.NNM_DEGtoA32((int)((double)(index2 + 1) * 360.0 / 20.0)), out s2, out c2);
                ((AppMain.NNS_VECTOR)~_pointer2).x = c2 * c1 + num1;
                ((AppMain.NNS_VECTOR)~_pointer2).y = s2 + num2;
                ((AppMain.NNS_VECTOR)~_pointer2).z = c2 * s1 + num3;
                _pointer1 = ++(_pointer2);
            }
        }
        for (int index1 = 0; index1 < 1; ++index1)
        {
            AppMain.nnSinCos(AppMain.NNM_DEGtoA32(180), out s1, out c1);
            AppMain.nnSinCos(0, out s2, out c2);
            for (int index2 = 0; index2 < 20; ++index2)
            {
                ((AppMain.NNS_VECTOR)~_pointer1).x = c1 * c2 + num1;
                ((AppMain.NNS_VECTOR)~_pointer1).y = s1 + num2;
                ((AppMain.NNS_VECTOR)~_pointer1).z = c1 * s2 + num3;
                AppMain.ArrayPointer<AppMain.NNS_VECTOR> _pointer2 = ++(_pointer1);
                AppMain.nnSinCos(AppMain.NNM_DEGtoA32((int)((double)(index2 + 1) * 360.0 / 20.0)), out s2, out c2);
                ((AppMain.NNS_VECTOR)~_pointer2).x = c1 * c2 + num1;
                ((AppMain.NNS_VECTOR)~_pointer2).y = s1 + num2;
                ((AppMain.NNS_VECTOR)~_pointer2).z = c1 * s2 + num3;
                _pointer1 = ++(_pointer2);
            }
        }
    }

    private AppMain.NNE_CIRCUM_COL nnEstCircumColNum(uint clipstat)
    {
        AppMain.mppAssertNotImpl();
        return AppMain.NNE_CIRCUM_COL.NNE_CIRCUM_COL_NONE;
    }

    private void nnSetCircumsphereColor(
      uint dstflag,
      AppMain.NNE_CIRCUM_COL colnum,
      ref AppMain.NNS_RGBA col)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawCircumsphereCore(
      AppMain.NNS_VECTOR center,
      float radius,
      AppMain.NNS_MATRIX mtx,
      ref AppMain.NNS_RGBA col,
      int trans)
    {
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawClipBoxCore(
      AppMain.NNS_VECTOR center,
      float sx,
      float sy,
      float sz,
      AppMain.NNS_MATRIX mtx,
      ref AppMain.NNS_RGBA col,
      int trans)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawCircumsphereNode(int nodeIdx, uint hideflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawCircumsphere(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MATRIXSTACK mstk,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawCircumsphereMotionNode(int nodeIdx, uint hideflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawCircumsphereMotion(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mot,
      float frame,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MATRIXSTACK mstk,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawCircumsphereTRSListNode(int nodeIdx, uint hideflag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawCircumsphereTRSList(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_TRS trslist,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MATRIXSTACK mstk,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawClipSphere(AppMain.NNS_VECTOR center, float radius, AppMain.NNS_MATRIX mtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawClipBox(
      AppMain.NNS_VECTOR center,
      float sx,
      float sy,
      float sz,
      AppMain.NNS_MATRIX mtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawClipBound(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MATRIXSTACK mstk,
      uint flag)
    {
        this.nnDrawCircumsphere(obj, basemtx, mstk, flag);
    }

    private void nnDrawClipBoundMotion(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mot,
      float frame,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MATRIXSTACK mstk,
      uint flag)
    {
        this.nnDrawCircumsphereMotion(obj, mot, frame, basemtx, mstk, flag);
    }

    private void nnDrawClipBoundTRSList(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_TRS trslist,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MATRIXSTACK mstk,
      uint flag)
    {
        this.nnDrawCircumsphereTRSList(obj, trslist, basemtx, mstk, flag);
    }

    private void nnSetClipBoundColor(
      uint dstflag,
      AppMain.NNE_CIRCUM_COL colnum,
      ref AppMain.NNS_RGBA col)
    {
        this.nnSetCircumsphereColor(dstflag, colnum, ref col);
    }

    private void nnCalcTRS(AppMain.NNS_TRS trs, AppMain.NNS_OBJECT obj, int nodeidx)
    {
        uint fType = obj.pNodeList[nodeidx].fType;
        trs.Translation.x = obj.pNodeList[nodeidx].Translation.x;
        trs.Translation.y = obj.pNodeList[nodeidx].Translation.y;
        trs.Translation.z = obj.pNodeList[nodeidx].Translation.z;
        if (((int)fType & 2) != 0)
        {
            trs.Rotation.x = 0.0f;
            trs.Rotation.y = 0.0f;
            trs.Rotation.z = 0.0f;
            trs.Rotation.w = 1f;
        }
        else
        {
            int x = obj.pNodeList[nodeidx].Rotation.x;
            int ry;
            int rz;
            if (((int)fType & 114688) != 0)
            {
                ry = 0;
                rz = 0;
            }
            else
            {
                ry = obj.pNodeList[nodeidx].Rotation.y;
                rz = obj.pNodeList[nodeidx].Rotation.z;
            }
            switch (fType & 3840U)
            {
                case 256:
                    AppMain.nnMakeRotateXZYQuaternion(out trs.Rotation, x, ry, rz);
                    break;
                case 1024:
                    AppMain.nnMakeRotateZXYQuaternion(out trs.Rotation, x, ry, rz);
                    break;
                default:
                    AppMain.nnMakeRotateXYZQuaternion(out trs.Rotation, x, ry, rz);
                    break;
            }
        }
        trs.Scaling.x = obj.pNodeList[nodeidx].Scaling.x;
        trs.Scaling.y = obj.pNodeList[nodeidx].Scaling.y;
        trs.Scaling.z = obj.pNodeList[nodeidx].Scaling.z;
    }

    public static void nnCalcTRSList(AppMain.NNS_TRS[] trslist, int offset, AppMain.NNS_OBJECT obj)
    {
        for (int index1 = 0; index1 < obj.nNode; ++index1)
        {
            uint fType = obj.pNodeList[index1].fType;
            uint num = fType & 3840U;
            int index2 = index1 + offset;
            trslist[index2].Translation.x = obj.pNodeList[index1].Translation.x;
            trslist[index2].Translation.y = obj.pNodeList[index1].Translation.y;
            trslist[index2].Translation.z = obj.pNodeList[index1].Translation.z;
            if (((int)fType & 2) != 0)
            {
                trslist[index2].Rotation.x = 0.0f;
                trslist[index2].Rotation.y = 0.0f;
                trslist[index2].Rotation.z = 0.0f;
                trslist[index2].Rotation.w = 1f;
            }
            else
            {
                int x = obj.pNodeList[index1].Rotation.x;
                int ry;
                int rz;
                if (((int)fType & 114688) != 0)
                {
                    ry = 0;
                    rz = 0;
                }
                else
                {
                    ry = obj.pNodeList[index1].Rotation.y;
                    rz = obj.pNodeList[index1].Rotation.z;
                }
                switch (num)
                {
                    case 256:
                        AppMain.nnMakeRotateXZYQuaternion(out trslist[index2].Rotation, x, ry, rz);
                        break;
                    case 1024:
                        AppMain.nnMakeRotateZXYQuaternion(out trslist[index2].Rotation, x, ry, rz);
                        break;
                    default:
                        AppMain.nnMakeRotateXYZQuaternion(out trslist[index2].Rotation, x, ry, rz);
                        break;
                }
            }
            trslist[index2].Scaling.x = obj.pNodeList[index1].Scaling.x;
            trslist[index2].Scaling.y = obj.pNodeList[index1].Scaling.y;
            trslist[index2].Scaling.z = obj.pNodeList[index1].Scaling.z;
        }
    }

    private static int nnCalcNodeMotionTRSCore(
      out int tflag,
      out int rflag,
      out int sflag,
      AppMain.NNS_VECTOR tv,
      AppMain.NNS_VECTOR sv,
      ref AppMain.NNS_QUATERNION rq,
      ref AppMain.NNS_QUATERNION invrq,
      bool need_invrq,
      AppMain.NNS_NODE pNode,
      int NodeIdx,
      AppMain.NNS_MOTION pMot,
      int SubMotIdx,
      float frame)
    {
        AppMain.NNS_ROTATE_A32 rv = new AppMain.NNS_ROTATE_A32();
        uint fType = pNode.fType;
        uint rtype = fType & 3840U;
        tv.Assign(pNode.Translation);
        sv.Assign(pNode.Scaling);
        if (((int)fType & 2) != 0)
        {
            rv.x = rv.y = rv.z = 0;
            rq.x = 0.0f;
            rq.y = 0.0f;
            rq.z = 0.0f;
            rq.w = 1f;
            if (need_invrq)
            {
                invrq.x = 0.0f;
                invrq.y = 0.0f;
                invrq.z = 0.0f;
                invrq.w = 1f;
            }
        }
        else
        {
            rv.x = pNode.Rotation.x;
            if (((int)fType & 114688) != 0)
            {
                rv.y = 0;
                rv.z = 0;
            }
            else
            {
                rv.y = pNode.Rotation.y;
                rv.z = pNode.Rotation.z;
            }
            switch (rtype)
            {
                case 256:
                    AppMain.nnMakeRotateXZYQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
                case 1024:
                    AppMain.nnMakeRotateZXYQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
                default:
                    AppMain.nnMakeRotateXYZQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
            }
            if (need_invrq)
                AppMain.nnInvertQuaternion(ref invrq, ref rq);
        }
        tflag = 0;
        rflag = 0;
        sflag = 0;
        for (int SubMotIdx1 = SubMotIdx; SubMotIdx1 < pMot.nSubmotion; ++SubMotIdx1)
        {
            AppMain.NNS_SUBMOTION submot = pMot.pSubmotion[SubMotIdx1];
            if (NodeIdx < submot.Id)
            {
                SubMotIdx = SubMotIdx1;
                break;
            }
            float dstframe;
            if (submot.Id == NodeIdx && submot.fType != 0U && ((double)submot.StartFrame <= (double)frame && (double)frame <= (double)submot.EndFrame) && AppMain.nnCalcMotionFrame(out dstframe, submot.fIPType, submot.StartKeyFrame, submot.EndKeyFrame, frame) != 0)
            {
                if (((int)submot.fType & 30720) != 0)
                    rflag |= AppMain.nnCalcMotionRotate(submot, dstframe, ref rv, rq, rtype);
                else if (((int)submot.fType & 1792) != 0)
                    tflag |= AppMain.nnCalcMotionTranslate(submot, dstframe, tv);
                else if (((int)submot.fType & 229376) != 0)
                    sflag |= AppMain.nnCalcMotionScale(submot, dstframe, sv);
                else if (((int)submot.fType & 786432) != 0)
                    AppMain.nnCallbackMotionUserData(AppMain.nncalctrsmotion.nnsObj, pMot, SubMotIdx1, NodeIdx, dstframe, frame);
            }
        }
        if (rflag == 1)
        {
            switch (rtype)
            {
                case 256:
                    AppMain.nnMakeRotateXZYQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
                case 1024:
                    AppMain.nnMakeRotateZXYQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
                default:
                    AppMain.nnMakeRotateXYZQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
            }
        }
        return SubMotIdx;
    }

    private void nnCalcTRSMotion(
      AppMain.NNS_TRS trs,
      AppMain.NNS_OBJECT obj,
      int nodeidx,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.NNS_QUATERNION rq = new AppMain.NNS_QUATERNION();
        if (((int)mot.fType & 1) == 0)
            return;
        AppMain.nncalctrsmotion.nnsObj = obj;
        if (AppMain.nnCalcMotionFrame(out frame, mot.fType, mot.StartFrame, mot.EndFrame, frame) == 0)
        {
            this.nnCalcTRS(trs, obj, nodeidx);
        }
        else
        {
            AppMain.NNS_NODE pNode = obj.pNodeList[nodeidx];
            AppMain.NNS_VECTOR nnCalcTrsMotionTv = AppMain.nnCalcTRSMotion_tv;
            nnCalcTrsMotionTv.Clear();
            AppMain.NNS_VECTOR nnCalcTrsMotionSv = AppMain.nnCalcTRSMotion_sv;
            nnCalcTrsMotionSv.Clear();
            AppMain.NNS_QUATERNION invrq = new AppMain.NNS_QUATERNION();
            AppMain.nnCalcNodeMotionTRSCore(out int _, out int _, out int _, nnCalcTrsMotionTv, nnCalcTrsMotionSv, ref rq, ref invrq, false, pNode, nodeidx, mot, 0, frame);
            trs.Translation.x = nnCalcTrsMotionTv.x;
            trs.Translation.y = nnCalcTrsMotionTv.y;
            trs.Translation.z = nnCalcTrsMotionTv.z;
            trs.Rotation = rq;
            trs.Scaling.x = nnCalcTrsMotionSv.x;
            trs.Scaling.y = nnCalcTrsMotionSv.y;
            trs.Scaling.z = nnCalcTrsMotionSv.z;
        }
    }

    public static void nnCalcTRSListMotion(
      AppMain.NNS_TRS[] trslist,
      int offset,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.NNS_QUATERNION rq = new AppMain.NNS_QUATERNION();
        if (((int)mot.fType & 1) == 0)
            return;
        if (AppMain.nnCalcMotionFrame(out frame, mot.fType, mot.StartFrame, mot.EndFrame, frame) == 0)
        {
            AppMain.nnCalcTRSList(trslist, offset, obj);
        }
        else
        {
            AppMain.nncalctrsmotion.nnsObj = obj;
            AppMain.NNS_QUATERNION invrq = new AppMain.NNS_QUATERNION();
            AppMain.NNS_VECTOR nnCalcTrsMotionTv = AppMain.nnCalcTRSMotion_tv;
            nnCalcTrsMotionTv.Clear();
            AppMain.NNS_VECTOR nnCalcTrsMotionSv = AppMain.nnCalcTRSMotion_sv;
            nnCalcTrsMotionSv.Clear();
            int SubMotIdx = 0;
            for (int NodeIdx = 0; NodeIdx < obj.nNode; ++NodeIdx)
            {
                AppMain.NNS_NODE pNode = obj.pNodeList[NodeIdx];
                SubMotIdx = AppMain.nnCalcNodeMotionTRSCore(out int _, out int _, out int _, nnCalcTrsMotionTv, nnCalcTrsMotionSv, ref rq, ref invrq, false, pNode, NodeIdx, mot, SubMotIdx, frame);
                int index = NodeIdx + offset;
                trslist[index].Translation.x = nnCalcTrsMotionTv.x;
                trslist[index].Translation.y = nnCalcTrsMotionTv.y;
                trslist[index].Translation.z = nnCalcTrsMotionTv.z;
                trslist[index].Rotation = rq;
                trslist[index].Scaling.x = nnCalcTrsMotionSv.x;
                trslist[index].Scaling.y = nnCalcTrsMotionSv.y;
                trslist[index].Scaling.z = nnCalcTrsMotionSv.z;
            }
        }
    }

    private static void nnCalcMatrixPaletteTRSListNode(int nodeIdx)
    {
        AppMain.NNS_NODE nnsNode;
        do
        {
            nnsNode = AppMain.nncalctrsmotion.nnsNodeList[nodeIdx];
            AppMain.NNS_TRS nnsTrs = AppMain.nncalctrsmotion.nnsTrsList[nodeIdx];
            if (((int)nnsNode.fType & 134217728) != 0)
            {
                if (((int)nnsNode.fType & 100663296) != 0)
                {
                    AppMain.nnPushMatrix(AppMain.nncalctrsmotion.nnsMstk, (AppMain.NNS_MATRIX)null);
                    AppMain.NNS_MATRIX currentMatrix = AppMain.nnGetCurrentMatrix(AppMain.nncalctrsmotion.nnsMstk);
                    AppMain.nnTranslateMatrix(currentMatrix, currentMatrix, nnsTrs.Translation.x, nnsTrs.Translation.y, nnsTrs.Translation.z);
                    if (((int)nnsNode.fType & 4096) != 0)
                        AppMain.nnCopyMatrix33(currentMatrix, AppMain.nncalctrsmotion.nnsBaseMtx);
                    else if (((int)nnsNode.fType & 1835008) != 0)
                    {
                        if (((int)nnsNode.fType & 262144) != 0)
                            AppMain.nnNormalizeColumn0(currentMatrix);
                        if (((int)nnsNode.fType & 524288) != 0)
                            AppMain.nnNormalizeColumn1(currentMatrix);
                        if (((int)nnsNode.fType & 1048576) != 0)
                            AppMain.nnNormalizeColumn2(currentMatrix);
                    }
                    AppMain.nnQuaternionMatrix(currentMatrix, currentMatrix, ref nnsTrs.Rotation);
                    AppMain.nnScaleMatrix(currentMatrix, currentMatrix, nnsTrs.Scaling.x, nnsTrs.Scaling.y, nnsTrs.Scaling.z);
                    if (((int)nnsNode.fType & 33554432) != 0)
                        AppMain.nnCalcMatrixPaletteTRSListNode1BoneXSIIK(nodeIdx);
                    else if (((int)nnsNode.fType & 67108864) != 0)
                        AppMain.nnCalcMatrixPaletteTRSListNode2BoneXSIIK(nodeIdx);
                    AppMain.nnPopMatrix(AppMain.nncalctrsmotion.nnsMstk);
                    nodeIdx = (int)nnsNode.iSibling;
                    goto label_40;
                }
            }
            else
            {
                if (((int)nnsNode.fType & 16384) != 0)
                {
                    AppMain.nnPushMatrix(AppMain.nncalctrsmotion.nnsMstk, (AppMain.NNS_MATRIX)null);
                    AppMain.nnCalcMatrixPaletteTRSListNode1BoneSIIK(nodeIdx);
                    AppMain.nnPopMatrix(AppMain.nncalctrsmotion.nnsMstk);
                    break;
                }
                if (((int)nnsNode.fType & 32768) != 0)
                {
                    AppMain.nnPushMatrix(AppMain.nncalctrsmotion.nnsMstk, (AppMain.NNS_MATRIX)null);
                    AppMain.nnCalcMatrixPaletteTRSListNode2BoneSIIK(nodeIdx);
                    AppMain.nnPopMatrix(AppMain.nncalctrsmotion.nnsMstk);
                    break;
                }
            }
            AppMain.nnPushMatrix(AppMain.nncalctrsmotion.nnsMstk, (AppMain.NNS_MATRIX)null);
            AppMain.NNS_MATRIX currentMatrix1 = AppMain.nnGetCurrentMatrix(AppMain.nncalctrsmotion.nnsMstk);
            AppMain.nnTranslateMatrix(currentMatrix1, currentMatrix1, nnsTrs.Translation.x, nnsTrs.Translation.y, nnsTrs.Translation.z);
            if (((int)nnsNode.fType & 4096) != 0)
                AppMain.nnCopyMatrix33(currentMatrix1, AppMain.nncalctrsmotion.nnsBaseMtx);
            else if (((int)nnsNode.fType & 1835008) != 0)
            {
                if (((int)nnsNode.fType & 262144) != 0)
                    AppMain.nnNormalizeColumn0(currentMatrix1);
                if (((int)nnsNode.fType & 524288) != 0)
                    AppMain.nnNormalizeColumn1(currentMatrix1);
                if (((int)nnsNode.fType & 1048576) != 0)
                    AppMain.nnNormalizeColumn2(currentMatrix1);
            }
            AppMain.nnQuaternionMatrix(currentMatrix1, currentMatrix1, ref nnsTrs.Rotation);
            AppMain.nnScaleMatrix(currentMatrix1, currentMatrix1, nnsTrs.Scaling.x, nnsTrs.Scaling.y, nnsTrs.Scaling.z);
            if (nnsNode.iMatrix != (short)-1)
            {
                if (((int)nnsNode.fType & 8) != 0)
                    AppMain.nnCopyMatrix(AppMain.nncalctrsmotion.nnsMtxPal[(int)nnsNode.iMatrix], currentMatrix1);
                else
                    AppMain.nnMultiplyMatrix(AppMain.nncalctrsmotion.nnsMtxPal[(int)nnsNode.iMatrix], currentMatrix1, nnsNode.InvInitMtx);
            }
            if (AppMain.nncalctrsmotion.nnsNodeStatList != null)
            {
                if (nodeIdx == 0 && AppMain.nncalctrsmotion.nnsNSFlag != 0U)
                    AppMain.nncalctrsmotion.nnsRootScale = AppMain.nnEstimateMatrixScaling(currentMatrix1);
                AppMain.nnCalcClipSetNodeStatus(AppMain.nncalctrsmotion.nnsNodeStatList, AppMain.nncalctrsmotion.nnsNodeList, nodeIdx, currentMatrix1, AppMain.nncalctrsmotion.nnsRootScale, AppMain.nncalctrsmotion.nnsNSFlag);
            }
            if (nnsNode.iChild != (short)-1)
                AppMain.nnCalcMatrixPaletteTRSListNode((int)nnsNode.iChild);
            AppMain.nnPopMatrix(AppMain.nncalctrsmotion.nnsMstk);
            nodeIdx = (int)nnsNode.iSibling;
        label_40:;
        }
        while (nnsNode.iSibling != (short)-1);
    }

    private static void nnCalcMatrixPaletteTRSList(
      AppMain.NNS_MATRIX[] mtxpal,
      uint[] nodestatlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_TRS[] trslist,
      ref AppMain.SNNS_MATRIX basemtx,
      AppMain.NNS_MATRIXSTACK mstk,
      uint flag)
    {
        AppMain.nncalctrsmotion.nnsBaseMtx.Assign(ref basemtx);
        AppMain.nnSetCurrentMatrix(mstk, AppMain.nncalctrsmotion.nnsBaseMtx);
        AppMain.nncalctrsmotion.nnsMtxPal = mtxpal;
        AppMain.nncalctrsmotion.nnsNodeStatList = nodestatlist;
        AppMain.nncalctrsmotion.nnsNSFlag = flag;
        AppMain.nncalctrsmotion.nnsTrsList = trslist;
        AppMain.nncalctrsmotion.nnsNodeList = obj.pNodeList;
        AppMain.nncalctrsmotion.nnsMstk = mstk;
        AppMain.nnCalcMatrixPaletteTRSListNode(0);
    }

    private static void nnCalcMatrixPaletteTRSList(
      AppMain.NNS_MATRIX[] mtxpal,
      uint[] nodestatlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_TRS[] trslist,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MATRIXSTACK mstk,
      uint flag)
    {
        if (basemtx != null)
            AppMain.nncalctrsmotion.nnsBaseMtx.Assign(basemtx);
        else
            AppMain.nncalctrsmotion.nnsBaseMtx.Assign(AppMain.nngUnitMatrix);
        AppMain.nnSetCurrentMatrix(mstk, AppMain.nncalctrsmotion.nnsBaseMtx);
        AppMain.nncalctrsmotion.nnsMtxPal = mtxpal;
        AppMain.nncalctrsmotion.nnsNodeStatList = nodestatlist;
        AppMain.nncalctrsmotion.nnsNSFlag = flag;
        AppMain.nncalctrsmotion.nnsTrsList = trslist;
        AppMain.nncalctrsmotion.nnsNodeList = obj.pNodeList;
        AppMain.nncalctrsmotion.nnsMstk = mstk;
        AppMain.nnCalcMatrixPaletteTRSListNode(0);
    }

    public static void nnLinkMotion(
      AppMain.ArrayPointer<AppMain.NNS_TRS> dstpose,
      AppMain.ArrayPointer<AppMain.NNS_TRS> pose0,
      AppMain.ArrayPointer<AppMain.NNS_TRS> pose1,
      int nnode,
      float ratio)
    {
        for (int index = 0; index < nnode; ++index)
        {
            AppMain.NNS_TRS nnsTrs1 = dstpose.array[dstpose.offset + index];
            AppMain.NNS_TRS nnsTrs2 = pose0.array[pose0.offset + index];
            AppMain.NNS_TRS nnsTrs3 = pose1.array[pose1.offset + index];
            nnsTrs1.Translation.x = nnsTrs2.Translation.x + (nnsTrs3.Translation.x - nnsTrs2.Translation.x) * ratio;
            nnsTrs1.Translation.y = nnsTrs2.Translation.y + (nnsTrs3.Translation.y - nnsTrs2.Translation.y) * ratio;
            nnsTrs1.Translation.z = nnsTrs2.Translation.z + (nnsTrs3.Translation.z - nnsTrs2.Translation.z) * ratio;
            nnsTrs1.Scaling.x = nnsTrs2.Scaling.x + (nnsTrs3.Scaling.x - nnsTrs2.Scaling.x) * ratio;
            nnsTrs1.Scaling.y = nnsTrs2.Scaling.y + (nnsTrs3.Scaling.y - nnsTrs2.Scaling.y) * ratio;
            nnsTrs1.Scaling.z = nnsTrs2.Scaling.z + (nnsTrs3.Scaling.z - nnsTrs2.Scaling.z) * ratio;
            AppMain.nnSlerpQuaternion(out nnsTrs1.Rotation, ref nnsTrs2.Rotation, ref nnsTrs3.Rotation, ratio);
        }
    }

    private void nnBlendMotion(
      AppMain.ArrayPointer<AppMain.NNS_TRS> _dstpose,
      AppMain.ArrayPointer<AppMain.NNS_TRS> _srcpose,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mot,
      float frame,
      AppMain.NNE_MOTIONBLEND blendmode)
    {
        if (((int)mot.fType & 1) == 0)
            return;
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        AppMain.ArrayPointer<AppMain.NNS_TRS> arrayPointer1 = _dstpose.Clone();
        AppMain.ArrayPointer<AppMain.NNS_TRS> arrayPointer2 = _srcpose.Clone();
        if (AppMain.nnCalcMotionFrame(out frame, mot.fType, mot.StartFrame, mot.EndFrame, frame) == 0)
        {
            if ((AppMain.NNS_TRS)~arrayPointer1 == (AppMain.NNS_TRS)~arrayPointer2)
                return;
            for (int index = 0; index < obj.nNode; ++index)
                arrayPointer1[index].Assign((AppMain.NNS_TRS)arrayPointer2);
        }
        else
        {
            int SubMotIdx = 0;
            AppMain.ArrayPointer<AppMain.NNS_NODE> pNodeList = (AppMain.ArrayPointer<AppMain.NNS_NODE>)obj.pNodeList;
            AppMain.NNS_VECTOR nnCalcTrsMotionTv = AppMain.nnCalcTRSMotion_tv;
            nnCalcTrsMotionTv.Clear();
            AppMain.NNS_VECTOR nnCalcTrsMotionSv = AppMain.nnCalcTRSMotion_sv;
            nnCalcTrsMotionSv.Clear();
            for (int NodeIdx = 0; NodeIdx < obj.nNode; ++NodeIdx)
            {
                int tflag;
                int rflag;
                int sflag;
                SubMotIdx = AppMain.nnCalcNodeMotionTRSCore(out tflag, out rflag, out sflag, nnCalcTrsMotionTv, nnCalcTrsMotionSv, ref nnsQuaternion1, ref nnsQuaternion2, true, (AppMain.NNS_NODE)pNodeList, NodeIdx, mot, SubMotIdx, frame);
                switch (blendmode)
                {
                    case AppMain.NNE_MOTIONBLEND.NNE_MOTIONBLEND_REPLACE_ALL:
                        if (tflag != 0 || rflag != 0 || sflag != 0)
                        {
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.x = nnCalcTrsMotionTv.x;
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.y = nnCalcTrsMotionTv.y;
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.z = nnCalcTrsMotionTv.z;
                            ((AppMain.NNS_TRS)~arrayPointer1).Rotation = nnsQuaternion1;
                            ((AppMain.NNS_TRS)~arrayPointer1).Scaling.x = nnCalcTrsMotionSv.x;
                            ((AppMain.NNS_TRS)~arrayPointer1).Scaling.y = nnCalcTrsMotionSv.y;
                            ((AppMain.NNS_TRS)~arrayPointer1).Scaling.z = nnCalcTrsMotionSv.z;
                            break;
                        }
                    ((AppMain.NNS_TRS)~arrayPointer1).Assign((AppMain.NNS_TRS)~arrayPointer2);
                        break;
                    case AppMain.NNE_MOTIONBLEND.NNE_MOTIONBLEND_ADD_TRANSLATION:
                        if (tflag != 0 || rflag != 0 || sflag != 0)
                        {
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.x = ((AppMain.NNS_TRS)~arrayPointer2).Translation.x + nnCalcTrsMotionTv.x - ((AppMain.NNS_NODE)~pNodeList).Translation.x;
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.y = ((AppMain.NNS_TRS)~arrayPointer2).Translation.y + nnCalcTrsMotionTv.y - ((AppMain.NNS_NODE)~pNodeList).Translation.y;
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.z = ((AppMain.NNS_TRS)~arrayPointer2).Translation.z + nnCalcTrsMotionTv.z - ((AppMain.NNS_NODE)~pNodeList).Translation.z;
                            ((AppMain.NNS_TRS)~arrayPointer1).Rotation = nnsQuaternion1;
                            ((AppMain.NNS_TRS)~arrayPointer1).Scaling.x = nnCalcTrsMotionSv.x;
                            ((AppMain.NNS_TRS)~arrayPointer1).Scaling.y = nnCalcTrsMotionSv.y;
                            ((AppMain.NNS_TRS)~arrayPointer1).Scaling.z = nnCalcTrsMotionSv.z;
                            break;
                        }
                    ((AppMain.NNS_TRS)~arrayPointer1).Assign((AppMain.NNS_TRS)~arrayPointer2);
                        break;
                    case AppMain.NNE_MOTIONBLEND.NNE_MOTIONBLEND_ADD_ALL:
                        if (tflag != 0)
                        {
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.x = ((AppMain.NNS_TRS)~arrayPointer2).Translation.x + nnCalcTrsMotionTv.x - ((AppMain.NNS_NODE)~pNodeList).Translation.x;
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.y = ((AppMain.NNS_TRS)~arrayPointer2).Translation.y + nnCalcTrsMotionTv.y - ((AppMain.NNS_NODE)~pNodeList).Translation.y;
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.z = ((AppMain.NNS_TRS)~arrayPointer2).Translation.z + nnCalcTrsMotionTv.z - ((AppMain.NNS_NODE)~pNodeList).Translation.z;
                        }
                        else
                        {
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.x = ((AppMain.NNS_TRS)~arrayPointer2).Translation.x;
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.y = ((AppMain.NNS_TRS)~arrayPointer2).Translation.y;
                            ((AppMain.NNS_TRS)~arrayPointer1).Translation.z = ((AppMain.NNS_TRS)~arrayPointer2).Translation.z;
                        }
                        if (rflag != 0)
                        {
                            AppMain.nnMultiplyQuaternion(ref ((AppMain.NNS_TRS)~arrayPointer1).Rotation, ref ((AppMain.NNS_TRS)~arrayPointer2).Rotation, ref nnsQuaternion2);
                            AppMain.nnMultiplyQuaternion(ref ((AppMain.NNS_TRS)~arrayPointer1).Rotation, ref ((AppMain.NNS_TRS)~arrayPointer1).Rotation, ref nnsQuaternion1);
                        }
                        else
                            ((AppMain.NNS_TRS)~arrayPointer1).Rotation = ((AppMain.NNS_TRS)~arrayPointer2).Rotation;
                        if (sflag != 0)
                        {
                            ((AppMain.NNS_TRS)~arrayPointer1).Scaling.x = ((AppMain.NNS_TRS)~arrayPointer2).Scaling.x * nnCalcTrsMotionSv.x / ((AppMain.NNS_NODE)~pNodeList).Scaling.x;
                            ((AppMain.NNS_TRS)~arrayPointer1).Scaling.y = ((AppMain.NNS_TRS)~arrayPointer2).Scaling.y * nnCalcTrsMotionSv.y / ((AppMain.NNS_NODE)~pNodeList).Scaling.y;
                            ((AppMain.NNS_TRS)~arrayPointer1).Scaling.z = ((AppMain.NNS_TRS)~arrayPointer2).Scaling.z * nnCalcTrsMotionSv.z / ((AppMain.NNS_NODE)~pNodeList).Scaling.z;
                            break;
                        }
                    ((AppMain.NNS_TRS)~arrayPointer1).Scaling.x = ((AppMain.NNS_TRS)~arrayPointer2).Scaling.x;
                        ((AppMain.NNS_TRS)~arrayPointer1).Scaling.y = ((AppMain.NNS_TRS)~arrayPointer2).Scaling.y;
                        ((AppMain.NNS_TRS)~arrayPointer1).Scaling.z = ((AppMain.NNS_TRS)~arrayPointer2).Scaling.z;
                        break;
                }
                ++pNodeList;
                ++arrayPointer1;
                ++arrayPointer2;
            }
        }
    }

    private void nnBlendMotionNode(
      AppMain.NNS_TRS dsttrs,
      AppMain.NNS_TRS srctrs,
      AppMain.NNS_OBJECT obj,
      int inode,
      AppMain.NNS_MOTION mot,
      float frame,
      AppMain.NNE_MOTIONBLEND blendmode)
    {
        if (((int)mot.fType & 1) == 0)
            return;
        if (AppMain.nnCalcMotionFrame(out frame, mot.fType, mot.StartFrame, mot.EndFrame, frame) == 0)
        {
            if (dsttrs == srctrs)
                return;
            dsttrs.Assign(srctrs);
        }
        else
        {
            AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
            AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
            AppMain.NNS_VECTOR nnCalcTrsMotionTv = AppMain.nnCalcTRSMotion_tv;
            nnCalcTrsMotionTv.Clear();
            AppMain.NNS_VECTOR nnCalcTrsMotionSv = AppMain.nnCalcTRSMotion_sv;
            nnCalcTrsMotionSv.Clear();
            AppMain.NNS_NODE pNode = obj.pNodeList[inode];
            int tflag;
            int rflag;
            int sflag;
            AppMain.nnCalcNodeMotionTRSCore(out tflag, out rflag, out sflag, nnCalcTrsMotionTv, nnCalcTrsMotionSv, ref nnsQuaternion1, ref nnsQuaternion2, true, pNode, inode, mot, 0, frame);
            switch (blendmode)
            {
                case AppMain.NNE_MOTIONBLEND.NNE_MOTIONBLEND_REPLACE_ALL:
                    if (tflag != 0 || rflag != 0 || sflag != 0)
                    {
                        dsttrs.Translation.x = nnCalcTrsMotionTv.x;
                        dsttrs.Translation.y = nnCalcTrsMotionTv.y;
                        dsttrs.Translation.z = nnCalcTrsMotionTv.z;
                        dsttrs.Rotation = nnsQuaternion1;
                        dsttrs.Scaling.x = nnCalcTrsMotionSv.x;
                        dsttrs.Scaling.y = nnCalcTrsMotionSv.y;
                        dsttrs.Scaling.z = nnCalcTrsMotionSv.z;
                        break;
                    }
                    dsttrs.Assign(srctrs);
                    break;
                case AppMain.NNE_MOTIONBLEND.NNE_MOTIONBLEND_ADD_TRANSLATION:
                    if (tflag != 0 || rflag != 0 || sflag != 0)
                    {
                        dsttrs.Translation.x = srctrs.Translation.x + nnCalcTrsMotionTv.x - pNode.Translation.x;
                        dsttrs.Translation.y = srctrs.Translation.y + nnCalcTrsMotionTv.y - pNode.Translation.y;
                        dsttrs.Translation.z = srctrs.Translation.z + nnCalcTrsMotionTv.z - pNode.Translation.z;
                        dsttrs.Rotation = nnsQuaternion1;
                        dsttrs.Scaling.x = nnCalcTrsMotionSv.x;
                        dsttrs.Scaling.y = nnCalcTrsMotionSv.y;
                        dsttrs.Scaling.z = nnCalcTrsMotionSv.z;
                        break;
                    }
                    dsttrs.Assign(srctrs);
                    break;
                case AppMain.NNE_MOTIONBLEND.NNE_MOTIONBLEND_ADD_ALL:
                    if (tflag != 0)
                    {
                        dsttrs.Translation.x = srctrs.Translation.x + nnCalcTrsMotionTv.x - pNode.Translation.x;
                        dsttrs.Translation.y = srctrs.Translation.y + nnCalcTrsMotionTv.y - pNode.Translation.y;
                        dsttrs.Translation.z = srctrs.Translation.z + nnCalcTrsMotionTv.z - pNode.Translation.z;
                    }
                    else
                    {
                        dsttrs.Translation.x = srctrs.Translation.x;
                        dsttrs.Translation.y = srctrs.Translation.y;
                        dsttrs.Translation.z = srctrs.Translation.z;
                    }
                    if (rflag != 0)
                    {
                        AppMain.nnMultiplyQuaternion(ref dsttrs.Rotation, ref srctrs.Rotation, ref nnsQuaternion2);
                        AppMain.nnMultiplyQuaternion(ref dsttrs.Rotation, ref dsttrs.Rotation, ref nnsQuaternion1);
                    }
                    else
                        dsttrs.Rotation = srctrs.Rotation;
                    if (sflag != 0)
                    {
                        dsttrs.Scaling.x = srctrs.Scaling.x * nnCalcTrsMotionSv.x / pNode.Scaling.x;
                        dsttrs.Scaling.y = srctrs.Scaling.y * nnCalcTrsMotionSv.y / pNode.Scaling.y;
                        dsttrs.Scaling.z = srctrs.Scaling.z * nnCalcTrsMotionSv.z / pNode.Scaling.z;
                        break;
                    }
                    dsttrs.Scaling.x = srctrs.Scaling.x;
                    dsttrs.Scaling.y = srctrs.Scaling.y;
                    dsttrs.Scaling.z = srctrs.Scaling.z;
                    break;
            }
        }
    }

    private void nnCalcMatrixTRSList1BoneSIIK(
      AppMain.NNS_MATRIX jnt1mtx,
      AppMain.NNS_MATRIX effmtx,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_TRS[] trslist,
      AppMain.NNS_MATRIX basemtx,
      int jnt1idx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixTRSList2BoneSIIK(
      AppMain.NNS_MATRIX jnt1mtx,
      AppMain.NNS_MATRIX jnt2mtx,
      AppMain.NNS_MATRIX effmtx,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_TRS[] trslist,
      AppMain.NNS_MATRIX basemtx,
      int jnt1idx)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnCalcMatrixPaletteTRSListNode1BoneSIIK(int jnt1nodeIdx)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnCalcMatrixPaletteTRSListNode2BoneSIIK(int jnt1nodeIdx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixPaletteLinkMotionNode(int nodeIdx)
    {
        AppMain.NNS_VECTOR tv1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR sv1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR tv2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR sv2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_TRS nnsTrs = new AppMain.NNS_TRS();
        AppMain.NNS_NODE nnsNode;
        do
        {
            nnsNode = AppMain.nncalctrsmotion.nnsNodeList[nodeIdx];
            AppMain.NNS_QUATERNION invrq = new AppMain.NNS_QUATERNION();
            int tflag1;
            int rflag1;
            int sflag1;
            AppMain.nncalctrsmotion.nnsSubMotIdx0 = AppMain.nnCalcNodeMotionTRSCore(out tflag1, out rflag1, out sflag1, tv1, sv1, ref nnsQuaternion1, ref invrq, false, nnsNode, nodeIdx, AppMain.nncalctrsmotion.nnsMot0, AppMain.nncalctrsmotion.nnsSubMotIdx0, AppMain.nncalctrsmotion.nnsFrame0);
            int tflag2;
            int rflag2;
            int sflag2;
            AppMain.nncalctrsmotion.nnsSubMotIdx1 = AppMain.nnCalcNodeMotionTRSCore(out tflag2, out rflag2, out sflag2, tv2, sv2, ref nnsQuaternion2, ref invrq, false, nnsNode, nodeIdx, AppMain.nncalctrsmotion.nnsMot1, AppMain.nncalctrsmotion.nnsSubMotIdx1, AppMain.nncalctrsmotion.nnsFrame1);
            float num = 1f - AppMain.nncalctrsmotion.nnsRatio;
            AppMain.nnPushMatrix(AppMain.nncalctrsmotion.nnsMstk, (AppMain.NNS_MATRIX)null);
            AppMain.NNS_MATRIX currentMatrix = AppMain.nnGetCurrentMatrix(AppMain.nncalctrsmotion.nnsMstk);
            if ((tflag1 | tflag2) != 0)
            {
                nnsTrs.Translation.x = (float)((double)tv1.x * (double)num + (double)tv2.x * (double)AppMain.nncalctrsmotion.nnsRatio);
                nnsTrs.Translation.y = (float)((double)tv1.y * (double)num + (double)tv2.y * (double)AppMain.nncalctrsmotion.nnsRatio);
                nnsTrs.Translation.z = (float)((double)tv1.z * (double)num + (double)tv2.z * (double)AppMain.nncalctrsmotion.nnsRatio);
                AppMain.nnTranslateMatrixFast(currentMatrix, nnsTrs.Translation.x, nnsTrs.Translation.y, nnsTrs.Translation.z);
            }
            else
                AppMain.nnTranslateMatrixFast(currentMatrix, tv1.x, tv1.y, tv1.z);
            if (((int)nnsNode.fType & 4096) != 0)
                AppMain.nnCopyMatrix33(currentMatrix, AppMain.nncalctrsmotion.nnsBaseMtx);
            else if (((int)nnsNode.fType & 1835008) != 0)
            {
                if (((int)nnsNode.fType & 262144) != 0)
                    AppMain.nnNormalizeColumn0(currentMatrix);
                if (((int)nnsNode.fType & 524288) != 0)
                    AppMain.nnNormalizeColumn1(currentMatrix);
                if (((int)nnsNode.fType & 1048576) != 0)
                    AppMain.nnNormalizeColumn2(currentMatrix);
            }
            if ((rflag1 | rflag2) != 0)
                AppMain.nnSlerpQuaternion(out nnsTrs.Rotation, ref nnsQuaternion1, ref nnsQuaternion2, AppMain.nncalctrsmotion.nnsRatio);
            else
                nnsTrs.Rotation = nnsQuaternion1;
            AppMain.nnQuaternionMatrix(currentMatrix, currentMatrix, ref nnsTrs.Rotation);
            if ((sflag1 | sflag2) != 0)
            {
                nnsTrs.Scaling.x = (float)((double)sv1.x * (double)num + (double)sv2.x * (double)AppMain.nncalctrsmotion.nnsRatio);
                nnsTrs.Scaling.y = (float)((double)sv1.y * (double)num + (double)sv2.y * (double)AppMain.nncalctrsmotion.nnsRatio);
                nnsTrs.Scaling.z = (float)((double)sv1.z * (double)num + (double)sv2.z * (double)AppMain.nncalctrsmotion.nnsRatio);
                AppMain.nnScaleMatrix(currentMatrix, currentMatrix, nnsTrs.Scaling.x, nnsTrs.Scaling.y, nnsTrs.Scaling.z);
            }
            else
                AppMain.nnScaleMatrixFast(currentMatrix, sv1.x, sv1.y, sv1.z);
            if (nnsNode.iMatrix != (short)-1)
            {
                if (((int)nnsNode.fType & 8) != 0)
                    AppMain.nnCopyMatrix(AppMain.nncalctrsmotion.nnsMtxPal[(int)nnsNode.iMatrix], currentMatrix);
                else
                    AppMain.nnMultiplyMatrix(AppMain.nncalctrsmotion.nnsMtxPal[(int)nnsNode.iMatrix], currentMatrix, nnsNode.InvInitMtx);
            }
            if (AppMain.nncalctrsmotion.nnsNodeStatList != null)
            {
                if (nodeIdx == 0 && AppMain.nncalctrsmotion.nnsNSFlag != 0U)
                    AppMain.nncalctrsmotion.nnsRootScale = AppMain.nnEstimateMatrixScaling(currentMatrix);
                AppMain.nnCalcClipSetNodeStatus(AppMain.nncalctrsmotion.nnsNodeStatList, AppMain.nncalctrsmotion.nnsNodeList, nodeIdx, currentMatrix, AppMain.nncalctrsmotion.nnsRootScale, AppMain.nncalctrsmotion.nnsNSFlag);
            }
            if (nnsNode.iChild != (short)-1)
                this.nnCalcMatrixPaletteLinkMotionNode((int)nnsNode.iChild);
            AppMain.nnPopMatrix(AppMain.nncalctrsmotion.nnsMstk);
            nodeIdx = (int)nnsNode.iSibling;
        }
        while (nnsNode.iSibling != (short)-1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(tv1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(sv1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(tv2);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(sv2);
    }

    private void nnCalcMatrixTRSList1BoneXSIIK(
      AppMain.NNS_MATRIX[] mtxlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_TRS[] trslist,
      AppMain.NNS_MATRIX basemtx,
      int rootidx)
    {
        int index1 = -1;
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        int index2 = -1;
        AppMain.NNS_VECTORFAST dst = new AppMain.NNS_VECTORFAST();
        AppMain.NNS_NODE[] pNodeList = obj.pNodeList;
        AppMain.NNS_NODE nnsNode1 = pNodeList[rootidx];
        AppMain.NNS_MATRIX src = mtxlist[rootidx];
        AppMain.NNS_NODE nnsNode2;
        for (int index3 = (int)nnsNode1.iChild; index3 != -1; index3 = (int)nnsNode2.iSibling)
        {
            nnsNode2 = pNodeList[index3];
            if (((int)nnsNode2.fType & 16384) != 0)
                index1 = index3;
            if (((int)nnsNode2.fType & 8192) != 0)
                index2 = index3;
        }
        AppMain.NNM_ASSERT(index1 != -1, "XSIIK 1Bone Joint1 not Found");
        AppMain.NNM_ASSERT(index2 != -1, "XSIIK 1Bone Effector not Found");
        AppMain.NNS_MATRIX nnsMatrix2 = nnsMatrix1;
        AppMain.NNS_NODE nnsNode3 = pNodeList[index1];
        AppMain.NNS_TRS nnsTrs1 = trslist[index1];
        AppMain.NNS_MATRIX nnsMatrix3 = mtxlist[index1];
        AppMain.NNS_NODE nnsNode4 = pNodeList[index2];
        AppMain.NNS_TRS nnsTrs2 = trslist[index2];
        AppMain.NNS_MATRIX nnsMatrix4 = mtxlist[index2];
        float siikBoneLength = nnsNode3.SIIKBoneLength;
        AppMain.nnMakeQuaternionMatrix(nnsMatrix2, ref nnsTrs1.Rotation);
        AppMain.nnScaleMatrix(nnsMatrix2, nnsMatrix2, nnsTrs1.Scaling.x, 1f, 1f);
        AppMain.nnMakeUnitMatrix(nnsMatrix4);
        AppMain.nnTransformVectorFast(out dst, basemtx, nnsTrs2.Translation);
        AppMain.nnCopyVectorFastMatrixTranslation(nnsMatrix4, ref dst);
        AppMain.nnCopyMatrix(nnsMatrix3, src);
        AppMain.nnCalc1BoneSIIK(nnsMatrix3, nnsMatrix2, nnsMatrix4, siikBoneLength);
        if (((int)nnsNode4.fType & 4096) == 0)
            AppMain.nnCopyMatrix33(nnsMatrix4, src);
        AppMain.nnQuaternionMatrix(nnsMatrix4, nnsMatrix4, ref nnsTrs2.Rotation);
    }

    private void nnCalcMatrixTRSList2BoneXSIIK(
      AppMain.NNS_MATRIX[] mtxlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_TRS[] trslist,
      AppMain.NNS_MATRIX basemtx,
      int rootidx)
    {
        int index1 = -1;
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        int index2 = -1;
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        int index3 = -1;
        AppMain.NNS_VECTORFAST dst = new AppMain.NNS_VECTORFAST();
        AppMain.NNS_NODE[] pNodeList = obj.pNodeList;
        AppMain.NNS_NODE nnsNode1 = pNodeList[rootidx];
        AppMain.NNS_MATRIX src = mtxlist[rootidx];
        AppMain.NNS_NODE nnsNode2;
        for (int index4 = (int)nnsNode1.iChild; index4 != -1; index4 = (int)nnsNode2.iSibling)
        {
            nnsNode2 = pNodeList[index4];
            if (((int)nnsNode2.fType & 32768) != 0)
            {
                index1 = index4;
                index2 = (int)nnsNode2.iChild;
                AppMain.NNM_ASSERT(pNodeList[index2].fType & 65536U, "XSIIK 2Bone Joint2 not Found");
            }
            if (((int)nnsNode2.fType & 8192) != 0)
                index3 = index4;
        }
        AppMain.NNM_ASSERT(index1 != -1, "XSIIK 2Bone Joint1 not Found");
        AppMain.NNM_ASSERT(index3 != -1, "XSIIK 2Bone Effector not Found");
        AppMain.NNS_NODE nnsNode3 = pNodeList[index1];
        AppMain.NNS_TRS nnsTrs1 = trslist[index1];
        AppMain.NNS_MATRIX nnsMatrix3 = mtxlist[index1];
        AppMain.NNS_MATRIX nnsMatrix4 = nnsMatrix1;
        AppMain.NNS_NODE nnsNode4 = pNodeList[index2];
        AppMain.NNS_TRS nnsTrs2 = trslist[index2];
        AppMain.NNS_MATRIX jnt2mtx = mtxlist[index2];
        AppMain.NNS_MATRIX nnsMatrix5 = nnsMatrix2;
        AppMain.NNS_NODE nnsNode5 = pNodeList[index3];
        AppMain.NNS_TRS nnsTrs3 = trslist[index3];
        AppMain.NNS_MATRIX nnsMatrix6 = mtxlist[index3];
        AppMain.nnMakeQuaternionMatrix(nnsMatrix4, ref nnsTrs1.Rotation);
        AppMain.nnScaleMatrix(nnsMatrix4, nnsMatrix4, nnsTrs1.Scaling.x, 1f, 1f);
        AppMain.nnMakeQuaternionMatrix(nnsMatrix5, ref nnsTrs2.Rotation);
        AppMain.nnScaleMatrix(nnsMatrix5, nnsMatrix5, nnsTrs2.Scaling.x, 1f, 1f);
        float siikBoneLength1 = nnsNode3.SIIKBoneLength;
        float siikBoneLength2 = nnsNode4.SIIKBoneLength;
        AppMain.nnMakeUnitMatrix(nnsMatrix6);
        AppMain.nnTransformVectorFast(out dst, basemtx, nnsTrs3.Translation);
        AppMain.nnCopyVectorFastMatrixTranslation(nnsMatrix6, ref dst);
        int zpref = ((int)nnsNode4.fType & 131072) == 0 ? 0 : 1;
        AppMain.nnCopyMatrix(nnsMatrix3, src);
        AppMain.nnCalc2BoneSIIK(nnsMatrix3, nnsMatrix4, jnt2mtx, nnsMatrix5, nnsMatrix6, siikBoneLength1, siikBoneLength2, zpref);
        if (((int)nnsNode5.fType & 4096) == 0)
            AppMain.nnCopyMatrix33(nnsMatrix6, src);
        AppMain.nnQuaternionMatrix(nnsMatrix6, nnsMatrix6, ref nnsTrs3.Rotation);
    }

    private static void nnCalcMatrixPaletteTRSListNode1BoneXSIIK(int rootidx)
    {
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        int nodeIdx1 = -1;
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix3 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        int nodeIdx2 = -1;
        AppMain.NNS_MATRIX nnsMatrix4 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix5 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_VECTORFAST dst = new AppMain.NNS_VECTORFAST();
        AppMain.NNS_NODE nnsNode1 = AppMain.nncalctrsmotion.nnsNodeList[rootidx];
        AppMain.NNS_MATRIX nnsMatrix6 = nnsMatrix1;
        AppMain.nnCopyMatrix(nnsMatrix6, AppMain.nnGetCurrentMatrix(AppMain.nncalctrsmotion.nnsMstk));
        AppMain.NNS_NODE nnsNode2;
        for (int index = (int)nnsNode1.iChild; index != -1; index = (int)nnsNode2.iSibling)
        {
            nnsNode2 = AppMain.nncalctrsmotion.nnsNodeList[index];
            if (((int)nnsNode2.fType & 16384) != 0)
                nodeIdx1 = index;
            if (((int)nnsNode2.fType & 8192) != 0)
                nodeIdx2 = index;
        }
        AppMain.NNM_ASSERT(nodeIdx1 != -1, "XSIIK 1Bone Joint1 not Found");
        AppMain.NNM_ASSERT(nodeIdx2 != -1, "XSIIK 1Bone Effector not Found");
        AppMain.NNS_NODE nnsNode3 = AppMain.nncalctrsmotion.nnsNodeList[nodeIdx1];
        AppMain.NNS_TRS nnsTrs1 = AppMain.nncalctrsmotion.nnsTrsList[nodeIdx1];
        AppMain.NNS_MATRIX nnsMatrix7 = nnsMatrix2;
        AppMain.NNS_MATRIX nnsMatrix8 = nnsMatrix3;
        AppMain.NNS_NODE nnsNode4 = AppMain.nncalctrsmotion.nnsNodeList[nodeIdx2];
        AppMain.NNS_MATRIX nnsMatrix9 = nnsMatrix4;
        AppMain.NNS_TRS nnsTrs2 = AppMain.nncalctrsmotion.nnsTrsList[nodeIdx2];
        float siikBoneLength = nnsNode3.SIIKBoneLength;
        AppMain.nnCopyMatrix(nnsMatrix7, nnsMatrix6);
        AppMain.nnMakeQuaternionMatrix(nnsMatrix8, ref nnsTrs1.Rotation);
        AppMain.nnScaleMatrix(nnsMatrix8, nnsMatrix8, nnsTrs1.Scaling.x, 1f, 1f);
        AppMain.nnMakeQuaternionMatrix(nnsMatrix9, ref nnsTrs2.Rotation);
        AppMain.nnScaleMatrix(nnsMatrix9, nnsMatrix9, nnsTrs2.Scaling.x, nnsTrs2.Scaling.y, nnsTrs2.Scaling.z);
        AppMain.nnMakeUnitMatrix(nnsMatrix5);
        AppMain.nnCopyMatrix33(nnsMatrix5, nnsMatrix9);
        AppMain.nnTransformVectorFast(out dst, AppMain.nncalctrsmotion.nnsBaseMtx, nnsTrs2.Translation);
        AppMain.nnCopyVectorFastMatrixTranslation(nnsMatrix9, ref dst);
        AppMain.nnCalc1BoneSIIK(nnsMatrix7, nnsMatrix8, nnsMatrix9, siikBoneLength);
        if (((int)nnsNode4.fType & 4096) == 0)
            AppMain.nnCopyMatrix33(nnsMatrix9, nnsMatrix6);
        AppMain.nnMultiplyMatrix(nnsMatrix9, nnsMatrix9, nnsMatrix5);
        if (nnsNode1.iMatrix != (short)-1)
            AppMain.nnMultiplyMatrix(AppMain.nncalctrsmotion.nnsMtxPal[(int)nnsNode1.iMatrix], nnsMatrix6, nnsNode1.InvInitMtx);
        if (nnsNode3.iMatrix != (short)-1)
            AppMain.nnMultiplyMatrix(AppMain.nncalctrsmotion.nnsMtxPal[(int)nnsNode3.iMatrix], nnsMatrix7, nnsNode3.InvInitMtx);
        if (nnsNode4.iMatrix != (short)-1)
            AppMain.nnMultiplyMatrix(AppMain.nncalctrsmotion.nnsMtxPal[(int)nnsNode4.iMatrix], nnsMatrix9, nnsNode4.InvInitMtx);
        if (AppMain.nncalctrsmotion.nnsNodeStatList != null)
        {
            AppMain.nnCalcClipSetNodeStatus(AppMain.nncalctrsmotion.nnsNodeStatList, AppMain.nncalctrsmotion.nnsNodeList, rootidx, nnsMatrix6, AppMain.nncalctrsmotion.nnsRootScale, AppMain.nncalctrsmotion.nnsNSFlag);
            AppMain.nnCalcClipSetNodeStatus(AppMain.nncalctrsmotion.nnsNodeStatList, AppMain.nncalctrsmotion.nnsNodeList, nodeIdx1, nnsMatrix7, AppMain.nncalctrsmotion.nnsRootScale, AppMain.nncalctrsmotion.nnsNSFlag);
            AppMain.nnCalcClipSetNodeStatus(AppMain.nncalctrsmotion.nnsNodeStatList, AppMain.nncalctrsmotion.nnsNodeList, nodeIdx2, nnsMatrix9, AppMain.nncalctrsmotion.nnsRootScale, AppMain.nncalctrsmotion.nnsNSFlag);
        }
        if (nnsNode4.iChild != (short)-1)
        {
            AppMain.nnPushMatrix(AppMain.nncalctrsmotion.nnsMstk, nnsMatrix9);
            AppMain.nnCalcMatrixPaletteTRSListNode((int)nnsNode4.iChild);
            AppMain.nnPopMatrix(AppMain.nncalctrsmotion.nnsMstk);
        }
        if (nnsNode4.iSibling != (short)-1)
        {
            AppMain.nnPushMatrix(AppMain.nncalctrsmotion.nnsMstk, nnsMatrix7);
            AppMain.nnCalcMatrixPaletteTRSListNode((int)nnsNode4.iSibling);
            AppMain.nnPopMatrix(AppMain.nncalctrsmotion.nnsMstk);
        }
        if (nnsNode3.iChild != (short)-1)
        {
            AppMain.nnPushMatrix(AppMain.nncalctrsmotion.nnsMstk, nnsMatrix7);
            AppMain.nnCalcMatrixPaletteTRSListNode((int)nnsNode3.iChild);
            AppMain.nnPopMatrix(AppMain.nncalctrsmotion.nnsMstk);
        }
        if (nnsNode3.iSibling == (short)-1)
            return;
        AppMain.nnCalcMatrixPaletteTRSListNode((int)nnsNode3.iSibling);
    }

    private static void nnCalcMatrixPaletteTRSListNode2BoneXSIIK(int rootidx)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnBindBufferVertexDescGL(AppMain.NNS_VTXLIST_GL_DESC pVtxDesc, uint flag)
    {
        OpenGL.glGenBuffer(out pVtxDesc.BufferName);
        OpenGL.glBindBuffer(34962U, pVtxDesc.BufferName);
        if (((int)flag & 2) != 0 && ((int)pVtxDesc.Type & 65536) == 0)
        {
            AppMain.NNS_VTXARRAY_GL p1 = pVtxDesc.pArray[0];
            int num1 = p1.Stride * pVtxDesc.nVertex;
            ByteBuffer pointer = p1.Pointer;
            ByteBuffer byteBuffer = p1.Pointer + num1;
            for (int index = 1; index < pVtxDesc.nArray; ++index)
            {
                AppMain.NNS_VTXARRAY_GL p2 = pVtxDesc.pArray[index];
                int num2 = p2.Stride * pVtxDesc.nVertex;
                if (pointer > p2.Pointer)
                    pointer = p2.Pointer;
                if (byteBuffer < p2.Pointer + num2)
                    byteBuffer = p2.Pointer + num2;
            }
            pVtxDesc.pVertexBuffer = pointer;
            pVtxDesc.VertexBufferSize = byteBuffer - pointer;
            OpenGL.glBufferVertexData((OpenGL.GLVertexData)new AppMain.GLVertexBuffer_(pVtxDesc));
            for (int index = 0; index < pVtxDesc.nArray; ++index)
            {
                AppMain.NNS_VTXARRAY_GL p2 = pVtxDesc.pArray[index];
            }
        }
        else
        {
            OpenGL.glBufferVertexData((OpenGL.GLVertexData)new AppMain.GLVertexBuffer_(pVtxDesc));
            for (int index = 0; index < pVtxDesc.nArray; ++index)
            {
                AppMain.NNS_VTXARRAY_GL p = pVtxDesc.pArray[index];
                p.Pointer.Offset = p.Pointer - pVtxDesc.pVertexBuffer;
            }
        }
    }

    private static void nnBindBufferPrimitiveDescGL(AppMain.NNS_PRIMLIST_GL_DESC pPrimDesc, uint flag)
    {
        OpenGL.glGenBuffer(out pPrimDesc.BufferName);
        OpenGL.glBindBuffer(34963U, pPrimDesc.BufferName);
        OpenGL.glBufferIndexData((OpenGL.GLIndexBuffer)new GLIndexBuffer_ByteBuffer(pPrimDesc.pIndexBuffer, pPrimDesc.IndexBufferSize));
        for (int index = 0; index < pPrimDesc.nPrim; ++index)
            pPrimDesc.pIndices[index].Offset = (ByteBuffer)pPrimDesc.pIndices[index] - pPrimDesc.pIndexBuffer;
    }

    private static uint nnBindBufferVertexListGL(
      AppMain.NNS_VTXLISTPTR[] dstvlist,
      AppMain.NNS_VTXLISTPTR[] srcvlist,
      int nVtxList,
      uint flag)
    {
        for (int index1 = 0; index1 < nVtxList; ++index1)
        {
            if (((int)srcvlist[index1].fType & 1) != 0)
            {
                AppMain.NNS_VTXLIST_GL_DESC pVtxList = (AppMain.NNS_VTXLIST_GL_DESC)srcvlist[index1].pVtxList;
                AppMain.NNS_VTXLIST_GL_DESC pVtxDesc = (AppMain.NNS_VTXLIST_GL_DESC)null;
                if (dstvlist != null)
                {
                    dstvlist[index1].fType = srcvlist[0].fType;
                    dstvlist[index1].pVtxList = (object)(pVtxDesc = new AppMain.NNS_VTXLIST_GL_DESC());
                    pVtxDesc.Assign(pVtxList);
                }
                if (dstvlist != null)
                {
                    pVtxDesc.pArray = new AppMain.NNS_VTXARRAY_GL[pVtxList.nArray];
                    for (int index2 = 0; index2 < pVtxDesc.nArray; ++index2)
                        pVtxDesc.pArray[index2] = new AppMain.NNS_VTXARRAY_GL(pVtxList.pArray[index2]);
                }
                if (((int)flag & 1) != 0 && ((int)pVtxList.Type & 6) != 0)
                {
                    if (((int)flag & 2) != 0 && ((int)pVtxList.Type & 65536) == 0)
                    {
                        if (dstvlist != null)
                        {
                            byte[] data = new byte[pVtxList.VertexBufferSize];
                            Array.Copy((Array)pVtxList.pVertexBuffer.Data, (Array)data, pVtxList.VertexBufferSize);
                            pVtxDesc.pVertexBuffer = ByteBuffer.Wrap(data);
                            for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
                                pVtxDesc.pArray[index2].Pointer = pVtxDesc.pVertexBuffer + pVtxList.pArray[index2].Pointer.Offset;
                            pVtxDesc.VertexBufferSize = pVtxList.VertexBufferSize;
                        }
                        else
                        {
                            int num = 0;
                            while (num < pVtxList.nArray)
                                ++num;
                        }
                    }
                    else if (dstvlist != null)
                    {
                        byte[] data = new byte[pVtxList.VertexBufferSize];
                        Array.Copy((Array)pVtxList.pVertexBuffer.Data, (Array)data, pVtxList.VertexBufferSize);
                        pVtxDesc.pVertexBuffer = ByteBuffer.Wrap(data);
                        for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
                            pVtxDesc.pArray[index2].Pointer = pVtxDesc.pVertexBuffer + pVtxList.pArray[index2].Pointer.Offset;
                    }
                }
                else if (dstvlist != null)
                {
                    AppMain.nnBindBufferVertexDescGL(pVtxDesc, flag);
                    dstvlist[index1].fType |= 16U;
                }
                if (pVtxList.nMatrix > 0 && dstvlist != null)
                {
                    pVtxDesc.pMatrixIndices = new ushort[pVtxList.nMatrix];
                    Array.Copy((Array)pVtxList.pMatrixIndices, (Array)pVtxDesc.pMatrixIndices, pVtxList.nMatrix);
                }
            }
            else
                AppMain.NNM_ASSERT(0, "Unknown vertex foramt.\n");
        }
        return 0;
    }

    private static uint nnBindBufferPrimitiveListGL(
      AppMain.NNS_PRIMLISTPTR[] dstplist,
      AppMain.NNS_PRIMLISTPTR[] srcplist,
      int nPrimList,
      uint flag)
    {
        for (int index = 0; index < nPrimList; ++index)
        {
            if (dstplist != null)
                dstplist[index].fType = srcplist[0].fType | 2U;
            if (((int)srcplist[index].fType & 1) != 0)
            {
                AppMain.NNS_PRIMLIST_GL_DESC pPrimList = (AppMain.NNS_PRIMLIST_GL_DESC)srcplist[index].pPrimList;
                AppMain.NNS_PRIMLIST_GL_DESC pPrimDesc = (AppMain.NNS_PRIMLIST_GL_DESC)null;
                if (dstplist != null)
                {
                    dstplist[index].pPrimList = (object)(pPrimDesc = new AppMain.NNS_PRIMLIST_GL_DESC());
                    pPrimDesc.Assign(pPrimList);
                }
                if (dstplist != null)
                {
                    pPrimDesc.pCounts = new int[pPrimList.nPrim];
                    Array.Copy((Array)pPrimList.pCounts, (Array)pPrimDesc.pCounts, pPrimList.nPrim);
                }
                if (dstplist != null)
                {
                    pPrimDesc.pIndices = new UShortBuffer[pPrimList.nPrim];
                    Array.Copy((Array)pPrimList.pIndices, (Array)pPrimDesc.pIndices, pPrimList.nPrim);
                    AppMain.nnBindBufferPrimitiveDescGL(pPrimDesc, flag);
                }
            }
            else
                AppMain.NNM_ASSERT(0, "Unknown primitive foramt.\n");
        }
        return 0;
    }

    private static uint nnBindBufferObjectGL(
      AppMain.NNS_OBJECT dstobj,
      AppMain.NNS_OBJECT srcobj,
      uint flag)
    {
        if (((int)srcobj.fType & 65536) != 0)
        {
            AppMain.NNM_ASSERT(0, "You can not bind-buffer the common-vertex-format object.\n");
            return AppMain.nnCopyObject(dstobj, srcobj, 0U);
        }
        if (((int)srcobj.fType & 64) != 0 && ((int)srcobj.fType & 128) == 0)
            flag |= 2U;
        if (dstobj != null)
        {
            dstobj.Assign(srcobj);
            dstobj.fType |= 16777344U;
        }
        if (dstobj != null)
        {
            dstobj.pMatPtrList = AppMain.New<AppMain.NNS_MATERIALPTR>(srcobj.nMaterial);
            int num = (int)AppMain.nnCopyMaterialList(dstobj.pMatPtrList, srcobj.pMatPtrList, srcobj.nMaterial, 0U);
        }
        else
        {
            int num1 = (int)AppMain.nnCopyMaterialList((AppMain.NNS_MATERIALPTR[])null, srcobj.pMatPtrList, srcobj.nMaterial, 0U);
        }
        if (dstobj != null)
        {
            dstobj.pVtxListPtrList = AppMain.New<AppMain.NNS_VTXLISTPTR>(srcobj.nVtxList);
            int num2 = (int)AppMain.nnBindBufferVertexListGL(dstobj.pVtxListPtrList, srcobj.pVtxListPtrList, srcobj.nVtxList, flag);
        }
        else
        {
            int num3 = (int)AppMain.nnBindBufferVertexListGL((AppMain.NNS_VTXLISTPTR[])null, srcobj.pVtxListPtrList, srcobj.nVtxList, flag);
        }
        if (dstobj != null)
        {
            dstobj.pPrimListPtrList = AppMain.New<AppMain.NNS_PRIMLISTPTR>(srcobj.nPrimList);
            int num2 = (int)AppMain.nnBindBufferPrimitiveListGL(dstobj.pPrimListPtrList, srcobj.pPrimListPtrList, srcobj.nPrimList, flag);
        }
        else
        {
            int num4 = (int)AppMain.nnBindBufferPrimitiveListGL((AppMain.NNS_PRIMLISTPTR[])null, srcobj.pPrimListPtrList, srcobj.nPrimList, flag);
        }
        if (dstobj != null)
        {
            dstobj.pNodeList = new AppMain.NNS_NODE[srcobj.nNode];
            for (int index = 0; index < srcobj.nNode; ++index)
                dstobj.pNodeList[index] = new AppMain.NNS_NODE(srcobj.pNodeList[index]);
        }
        if (dstobj != null)
        {
            dstobj.pSubobjList = AppMain.New<AppMain.NNS_SUBOBJ>(srcobj.nSubobj);
            int num2 = (int)AppMain.nnCopySubobjList(dstobj.pSubobjList, srcobj.pSubobjList, srcobj.nSubobj, flag);
        }
        else
        {
            int num5 = (int)AppMain.nnCopySubobjList((AppMain.NNS_SUBOBJ[])null, srcobj.pSubobjList, srcobj.nSubobj, flag);
        }
        return 0;
    }

    private void nnBindBufferObjectDirectGL(AppMain.NNS_OBJECT obj, uint flag)
    {
        if (((int)obj.fType & 65536) != 0)
        {
            AppMain.NNM_ASSERT(0, "You can not bind-buffer the common-vertex-format object.\n");
        }
        else
        {
            if (((int)obj.fType & 64) != 0 && ((int)obj.fType & 128) == 0)
                flag |= 2U;
            for (int index = 0; index < obj.nVtxList; ++index)
            {
                AppMain.NNS_VTXLISTPTR pVtxListPtr = obj.pVtxListPtrList[index];
                if (((int)pVtxListPtr.fType & 1) != 0)
                {
                    AppMain.nnBindBufferVertexDescGL((AppMain.NNS_VTXLIST_GL_DESC)pVtxListPtr.pVtxList, flag);
                    pVtxListPtr.fType |= 16U;
                }
                else
                    AppMain.NNM_ASSERT(0, "Unknown vertex foramt.\n");
            }
            for (int index = 0; index < obj.nPrimList; ++index)
            {
                AppMain.NNS_PRIMLISTPTR pPrimListPtr = obj.pPrimListPtrList[index];
                if (((int)pPrimListPtr.fType & 1) != 0)
                {
                    AppMain.nnBindBufferPrimitiveDescGL((AppMain.NNS_PRIMLIST_GL_DESC)pPrimListPtr.pPrimList, flag);
                    pPrimListPtr.fType |= 2U;
                }
                else
                    AppMain.NNM_ASSERT(0, "Unknown primitive foramt.\n");
            }
            obj.fType |= 16777216U;
        }
    }

    private static void nnDeleteBufferObjectGL(AppMain.NNS_OBJECT obj)
    {
        for (int index = 0; index < obj.nVtxList; ++index)
        {
            AppMain.NNS_VTXLISTPTR pVtxListPtr = obj.pVtxListPtrList[index];
            if (((int)pVtxListPtr.fType & 16) != 0)
                OpenGL.glDeleteBuffers(1, new uint[1]
                {
          ((AppMain.NNS_VTXLIST_GL_DESC) pVtxListPtr.pVtxList).BufferName
                });
        }
        for (int index = 0; index < obj.nPrimList; ++index)
        {
            AppMain.NNS_PRIMLISTPTR pPrimListPtr = obj.pPrimListPtrList[index];
            if (((int)pPrimListPtr.fType & 2) != 0)
                OpenGL.glDeleteBuffers(1, new uint[1]
                {
          ((AppMain.NNS_PRIMLIST_GL_DESC) pPrimListPtr.pPrimList).BufferName
                });
        }
    }

    public static void nnSetUpMatrixStack(ref AppMain.NNS_MATRIXSTACK mstk, uint size)
    {
        mstk = new AppMain.NNS_MATRIXSTACK(size);
        AppMain.NNS_MATRIX identity = AppMain.NNS_MATRIX.CreateIdentity();
        mstk.push(identity);
    }

    public static void nnClearMatrixStack(AppMain.NNS_MATRIXSTACK mstk)
    {
        mstk.clear();
        AppMain.NNS_MATRIX identity = AppMain.NNS_MATRIX.CreateIdentity();
        mstk.push(identity);
    }

    public static AppMain.NNS_MATRIX nnGetCurrentMatrix(AppMain.NNS_MATRIXSTACK mstk)
    {
        return mstk.get();
    }

    public static void nnSetCurrentMatrix(AppMain.NNS_MATRIXSTACK mstk, AppMain.NNS_MATRIX mtx)
    {
        mstk.set(mtx);
    }

    public static void nnPushMatrix(AppMain.NNS_MATRIXSTACK mstk, ref AppMain.SNNS_MATRIX mtx)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.nnmatrixstack_mtx_pool.Alloc();
        AppMain.nnCopyMatrix(nnsMatrix, ref mtx);
        mstk.push(nnsMatrix);
    }

    public static void nnPushMatrix(AppMain.NNS_MATRIXSTACK mstk, AppMain.NNS_MATRIX mtx)
    {
        AppMain.NNS_MATRIX matrix = AppMain.nnmatrixstack_mtx_pool.Alloc();
        if (mtx == null)
            matrix.Assign(mstk.get());
        else
            matrix.Assign(mtx);
        mstk.push(matrix);
    }

    public static void nnPushMatrix(AppMain.NNS_MATRIXSTACK mstk)
    {
        AppMain.nnPushMatrix(mstk, (AppMain.NNS_MATRIX)null);
    }

    public static void nnPopMatrix(AppMain.NNS_MATRIXSTACK mstk)
    {
        AppMain.nnmatrixstack_mtx_pool.Release(mstk.pop());
    }

    private void nnInitLight()
    {
        AppMain.nnlight.nngLight.AmbientColor.r = 0.2f;
        AppMain.nnlight.nngLight.AmbientColor.g = 0.2f;
        AppMain.nnlight.nngLight.AmbientColor.b = 0.2f;
        AppMain.nnlight.nngLight.AmbientColor.a = 1f;
        for (int index = 0; index < AppMain.NNE_LIGHT_MAX; ++index)
        {
            AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[index];
            nnsGlLightData.bEnable = 0;
            nnsGlLightData.fType = 1U;
            nnsGlLightData.Intensity = 1f;
            nnsGlLightData.Ambient.r = 0.0f;
            nnsGlLightData.Ambient.g = 0.0f;
            nnsGlLightData.Ambient.b = 0.0f;
            nnsGlLightData.Ambient.a = 1f;
            nnsGlLightData.Diffuse.r = 1f;
            nnsGlLightData.Diffuse.g = 1f;
            nnsGlLightData.Diffuse.b = 1f;
            nnsGlLightData.Diffuse.a = 1f;
            nnsGlLightData.Specular.r = 1f;
            nnsGlLightData.Specular.g = 1f;
            nnsGlLightData.Specular.b = 1f;
            nnsGlLightData.Specular.a = 1f;
            nnsGlLightData.Direction.x = 0.0f;
            nnsGlLightData.Direction.y = 0.0f;
            nnsGlLightData.Direction.z = -1f;
            nnsGlLightData.Position.x = 0.0f;
            nnsGlLightData.Position.y = 0.0f;
            nnsGlLightData.Position.z = 0.0f;
            nnsGlLightData.Position.w = 1f;
            nnsGlLightData.Target.x = 0.0f;
            nnsGlLightData.Target.y = 0.0f;
            nnsGlLightData.Target.z = 0.0f;
            nnsGlLightData.RotType = 0;
            nnsGlLightData.Rotation.x = 0;
            nnsGlLightData.Rotation.y = 0;
            nnsGlLightData.Rotation.z = 0;
            nnsGlLightData.InnerAngle = 16384;
            nnsGlLightData.OuterAngle = 16384;
            nnsGlLightData.InnerRange = 1E+12f;
            nnsGlLightData.OuterRange = 1E+12f;
            nnsGlLightData.FallOffStart = 1E+12f;
            nnsGlLightData.FallOffEnd = 1E+12f;
            nnsGlLightData.ConstantAttenuation = 1f;
            nnsGlLightData.LinearAttenuation = 0.0f;
            nnsGlLightData.QuadraticAttenuation = 0.0f;
        }
    }

    private static void nnSetLightSwitch(int no, int on_off)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.nnlight.nngLight.LightData[no].bEnable = on_off;
    }

    private static void nnSetLightType(int no, uint type)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.nnlight.nngLight.LightData[no].fType = type;
    }

    private static void nnSetLightAmbientGL(int no, float r, float g, float b)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.Ambient.r = r;
        nnsGlLightData.Ambient.g = g;
        nnsGlLightData.Ambient.b = b;
    }

    private static void nnSetLightDiffuseGL(int no, float r, float g, float b)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.Diffuse.r = r;
        nnsGlLightData.Diffuse.g = g;
        nnsGlLightData.Diffuse.b = b;
    }

    private static void nnSetLightSpecularGL(int no, float r, float g, float b)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.Specular.r = r;
        nnsGlLightData.Specular.g = g;
        nnsGlLightData.Specular.b = b;
    }

    private static void nnSetLightColor(int no, float r, float g, float b)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.nnSetLightDiffuseGL(no, r, g, b);
        AppMain.nnSetLightSpecularGL(no, r, g, b);
    }

    private static void nnSetLightAlpha(int no, float a)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.nnlight.nngLight.LightData[no].Diffuse.a = a;
    }

    private static void nnSetLightDirection(int no, float x, float y, float z)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.Direction.x = x;
        nnsGlLightData.Direction.y = y;
        nnsGlLightData.Direction.z = z;
    }

    private static void nnSetLightPosition(int no, float x, float y, float z)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.Position.x = x;
        nnsGlLightData.Position.y = y;
        nnsGlLightData.Position.z = z;
        nnsGlLightData.Position.w = 1f;
    }

    private static void nnSetLightTarget(int no, float x, float y, float z)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.Target.x = x;
        nnsGlLightData.Target.y = y;
        nnsGlLightData.Target.z = z;
    }

    private static void nnSetLightRotation(int no, int rottype, int rotx, int roty, int rotz)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.RotType = rottype;
        nnsGlLightData.Rotation.x = rotx;
        nnsGlLightData.Rotation.y = roty;
        nnsGlLightData.Rotation.z = rotz;
        float s1;
        float c1;
        AppMain.nnSinCos(rotx, out s1, out c1);
        float s2;
        float c2;
        AppMain.nnSinCos(roty, out s2, out c2);
        float s3;
        float c3;
        AppMain.nnSinCos(rotz, out s3, out c3);
        switch (rottype)
        {
            case 1:
                nnsGlLightData.Direction.x = (float)(-(double)c2 * (double)s3 * (double)s1 - (double)s2 * (double)c1);
                nnsGlLightData.Direction.y = c3 * s1;
                nnsGlLightData.Direction.z = (float)((double)s2 * (double)s3 * (double)s1 - (double)c2 * (double)c1);
                break;
            case 4:
                nnsGlLightData.Direction.x = -s2 * c1;
                nnsGlLightData.Direction.y = s1;
                nnsGlLightData.Direction.z = -c2 * c1;
                break;
            default:
                nnsGlLightData.Direction.x = (float)(-(double)c3 * (double)s2 * (double)c1 - (double)s3 * (double)s1);
                nnsGlLightData.Direction.y = (float)(-(double)s3 * (double)s2 * (double)c1 + (double)c3 * (double)s1);
                nnsGlLightData.Direction.z = -c2 * c1;
                break;
        }
    }

    public static void nnSetAmbientColor(float r, float g, float b)
    {
        AppMain.nnlight.nngLight.AmbientColor.r = r;
        AppMain.nnlight.nngLight.AmbientColor.g = g;
        AppMain.nnlight.nngLight.AmbientColor.b = b;
    }

    public static void nnSetLightIntensity(int no, float intensity)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.nnlight.nngLight.LightData[no].Intensity = intensity;
    }

    public static void nnSetLightAngle(int no, int innerangle, int outerangle)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.InnerAngle = innerangle;
        nnsGlLightData.OuterAngle = outerangle;
        nnsGlLightData.SpotExponent = 0.0f;
        nnsGlLightData.SpotCutoff = AppMain.NNM_A32toDEG(outerangle);
    }

    public static void nnSetLightSpotEffectGL(int no, float exp, float cutoff)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.SpotExponent = exp;
        nnsGlLightData.SpotCutoff = cutoff;
    }

    public static void nnSetLightRange(int no, float innerrange, float outerrange)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.InnerRange = innerrange;
        nnsGlLightData.OuterRange = outerrange;
    }

    public static void nnSetLightFallOff(int no, float falloffstart, float falloffend)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.FallOffStart = falloffstart;
        nnsGlLightData.FallOffEnd = falloffend;
        nnsGlLightData.ConstantAttenuation = 1f;
        nnsGlLightData.LinearAttenuation = 0.0f;
        if ((double)falloffstart > 9.99999996004197E-13)
            nnsGlLightData.QuadraticAttenuation = (float)(1.0 / ((double)falloffstart * (double)falloffstart));
        else
            nnsGlLightData.QuadraticAttenuation = 1E+12f;
    }

    public static void nnSetLightAttenuationGL(int no, float cnst, float lin, float quad)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        nnsGlLightData.ConstantAttenuation = cnst;
        nnsGlLightData.LinearAttenuation = lin;
        nnsGlLightData.QuadraticAttenuation = quad;
    }

    public static void nnSetLightMatrix(AppMain.NNS_MATRIX mtx)
    {
        if (mtx != null)
            AppMain.nnCopyMatrix(AppMain.nnlight.nngLightMtx, mtx);
        else
            AppMain.nnMakeUnitMatrix(AppMain.nnlight.nngLightMtx);
    }

    public static void nnSetUpParallelLight(
      AppMain.NNS_LIGHT_PARALLEL light,
      ref AppMain.NNS_RGBA color,
      float inten,
      AppMain.NNS_VECTOR dir)
    {
        light.Color = color;
        light.Intensity = inten;
        light.Direction.Assign(dir);
    }

    public static void nnSetUpPointLight(
      AppMain.NNS_LIGHT_POINT light,
      ref AppMain.NNS_RGBA color,
      float inten,
      AppMain.NNS_VECTOR pos,
      float falloffstart,
      float falloffend)
    {
        light.Color = color;
        light.Intensity = inten;
        light.Position.Assign(pos);
        light.FallOffStart = falloffstart;
        light.FallOffEnd = falloffend;
    }

    public static void nnSetUpTargetSpotLight(
      AppMain.NNS_LIGHT_TARGET_SPOT light,
      ref AppMain.NNS_RGBA color,
      float inten,
      AppMain.NNS_VECTOR pos,
      AppMain.NNS_VECTOR target,
      int innerangle,
      int outerangle,
      float falloffstart,
      float falloffend)
    {
        light.Color = color;
        light.Intensity = inten;
        light.Position.Assign(pos);
        light.Target.Assign(target);
        light.InnerAngle = innerangle;
        light.OuterAngle = outerangle;
        light.FallOffStart = falloffstart;
        light.FallOffEnd = falloffend;
    }

    public static void nnSetUpRotationSpotLight(
      AppMain.NNS_LIGHT_ROTATION_SPOT light,
      ref AppMain.NNS_RGBA color,
      float inten,
      AppMain.NNS_VECTOR pos,
      int rottype,
      AppMain.NNS_ROTATE_A32 rotation,
      int innerangle,
      int outerangle,
      float falloffstart,
      float falloffend)
    {
        light.Color = color;
        light.Intensity = inten;
        light.Position.Assign(pos);
        light.RotType = rottype;
        light.Rotation = rotation;
        light.InnerAngle = innerangle;
        light.OuterAngle = outerangle;
        light.FallOffStart = falloffstart;
        light.FallOffEnd = falloffend;
    }

    public static void nnSetUpTargetDirectionalLight(
      AppMain.NNS_LIGHT_TARGET_DIRECTIONAL light,
      ref AppMain.NNS_RGBA color,
      float inten,
      AppMain.NNS_VECTOR pos,
      AppMain.NNS_VECTOR target,
      float innerrange,
      float outerrange,
      float falloffstart,
      float falloffend)
    {
        light.Color = color;
        light.Intensity = inten;
        light.Position.Assign(pos);
        light.Target.Assign(target);
        light.InnerRange = innerrange;
        light.OuterRange = outerrange;
        light.FallOffStart = falloffstart;
        light.FallOffEnd = falloffend;
    }

    public static void nnSetUpRotationDirectionalLight(
      AppMain.NNS_LIGHT_ROTATION_DIRECTIONAL light,
      ref AppMain.NNS_RGBA color,
      float inten,
      AppMain.NNS_VECTOR pos,
      int rottype,
      AppMain.NNS_ROTATE_A32 rotation,
      float innerrange,
      float outerrange,
      float falloffstart,
      float falloffend)
    {
        light.Color = color;
        light.Intensity = inten;
        light.Position.Assign(pos);
        light.RotType = rottype;
        light.Rotation = rotation;
        light.InnerRange = innerrange;
        light.OuterRange = outerrange;
        light.FallOffStart = falloffstart;
        light.FallOffEnd = falloffend;
    }

    public static void nnSetUpStandardLightGL(
      AppMain.NNS_LIGHT_STANDARD_GL light,
      ref AppMain.NNS_RGBA ambient,
      ref AppMain.NNS_RGBA diffuse,
      ref AppMain.NNS_RGBA specular,
      AppMain.NNS_VECTOR4D position,
      AppMain.NNS_VECTOR direction,
      float expornent,
      float cutoff,
      float cnstattn,
      float linattn,
      float quadattn)
    {
        light.Ambient = ambient;
        light.Diffuse = diffuse;
        light.Specular = specular;
        light.Position.Assign(position);
        light.SpotDirection.Assign(direction);
        light.SpotExponent = expornent;
        light.SpotCutoff = cutoff;
        light.ConstantAttenuation = cnstattn;
        light.LinearAttenuation = linattn;
        light.QuadraticAttenuation = quadattn;
    }

    private static void nnSetLight(int no, object light, uint type)
    {
        if (no >= AppMain.NNE_LIGHT_MAX)
            return;
        AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[no];
        AppMain.nnSetLightType(no, type);
        uint num = type & 65599U;
        if (num <= 8U)
        {
            switch ((int)num - 1)
            {
                case 0:
                    AppMain.NNS_LIGHT_PARALLEL nnsLightParallel = (AppMain.NNS_LIGHT_PARALLEL)(AppMain.NNS_LIGHT_TARGET_DIRECTIONAL)light;
                    AppMain.nnSetLightColor(no, nnsLightParallel.Color.r, nnsLightParallel.Color.g, nnsLightParallel.Color.b);
                    AppMain.nnSetLightIntensity(no, nnsLightParallel.Intensity);
                    AppMain.nnSetLightDirection(no, nnsLightParallel.Direction.x, nnsLightParallel.Direction.y, nnsLightParallel.Direction.z);
                    AppMain.nnSetLightAmbientGL(no, 0.0f, 0.0f, 0.0f);
                    break;
                case 1:
                    AppMain.NNS_LIGHT_POINT nnsLightPoint = (AppMain.NNS_LIGHT_POINT)(AppMain.NNS_LIGHT_TARGET_DIRECTIONAL)light;
                    AppMain.nnSetLightColor(no, nnsLightPoint.Color.r, nnsLightPoint.Color.g, nnsLightPoint.Color.b);
                    AppMain.nnSetLightIntensity(no, nnsLightPoint.Intensity);
                    AppMain.nnSetLightPosition(no, nnsLightPoint.Position.x, nnsLightPoint.Position.y, nnsLightPoint.Position.z);
                    AppMain.nnSetLightFallOff(no, nnsLightPoint.FallOffStart, nnsLightPoint.FallOffEnd);
                    AppMain.nnSetLightAmbientGL(no, 0.0f, 0.0f, 0.0f);
                    break;
                case 2:
                    break;
                case 3:
                    AppMain.NNS_LIGHT_TARGET_SPOT nnsLightTargetSpot = (AppMain.NNS_LIGHT_TARGET_SPOT)(AppMain.NNS_LIGHT_TARGET_DIRECTIONAL)light;
                    AppMain.nnSetLightColor(no, nnsLightTargetSpot.Color.r, nnsLightTargetSpot.Color.g, nnsLightTargetSpot.Color.b);
                    AppMain.nnSetLightIntensity(no, nnsLightTargetSpot.Intensity);
                    AppMain.nnSetLightPosition(no, nnsLightTargetSpot.Position.x, nnsLightTargetSpot.Position.y, nnsLightTargetSpot.Position.z);
                    AppMain.nnSetLightTarget(no, nnsLightTargetSpot.Target.x, nnsLightTargetSpot.Target.y, nnsLightTargetSpot.Target.z);
                    AppMain.nnSetLightAngle(no, nnsLightTargetSpot.InnerAngle, nnsLightTargetSpot.OuterAngle);
                    AppMain.nnSetLightFallOff(no, nnsLightTargetSpot.FallOffStart, nnsLightTargetSpot.FallOffEnd);
                    AppMain.nnSetLightAmbientGL(no, 0.0f, 0.0f, 0.0f);
                    break;
                default:
                    if (num != 8U)
                        break;
                    AppMain.NNS_LIGHT_ROTATION_SPOT lightRotationSpot = (AppMain.NNS_LIGHT_ROTATION_SPOT)(AppMain.NNS_LIGHT_TARGET_DIRECTIONAL)light;
                    AppMain.nnSetLightColor(no, lightRotationSpot.Color.r, lightRotationSpot.Color.g, lightRotationSpot.Color.b);
                    AppMain.nnSetLightIntensity(no, lightRotationSpot.Intensity);
                    AppMain.nnSetLightPosition(no, lightRotationSpot.Position.x, lightRotationSpot.Position.y, lightRotationSpot.Position.z);
                    AppMain.nnSetLightRotation(no, lightRotationSpot.RotType, lightRotationSpot.Rotation.x, lightRotationSpot.Rotation.y, lightRotationSpot.Rotation.z);
                    AppMain.nnSetLightAngle(no, lightRotationSpot.InnerAngle, lightRotationSpot.OuterAngle);
                    AppMain.nnSetLightFallOff(no, lightRotationSpot.FallOffStart, lightRotationSpot.FallOffEnd);
                    AppMain.nnSetLightAmbientGL(no, 0.0f, 0.0f, 0.0f);
                    break;
            }
        }
        else if (num != 16U)
        {
            if (num != 32U)
            {
                if (num != 65536U)
                    return;
                AppMain.mppAssertNotImpl();
            }
            else
                AppMain.mppAssertNotImpl();
        }
        else
            AppMain.mppAssertNotImpl();
    }

    private static void nnPutLightSettings()
    {
        OpenGL.glMatrixMode(5888U);
        Matrix nngLightMtx = (Matrix)AppMain.nnlight.nngLightMtx;
        OpenGL.glLoadMatrixf(ref nngLightMtx);
        OpenGL.glArray4f glArray4f1;
        glArray4f1.f0 = AppMain.nnlight.nngLight.AmbientColor.r;
        glArray4f1.f1 = AppMain.nnlight.nngLight.AmbientColor.g;
        glArray4f1.f2 = AppMain.nnlight.nngLight.AmbientColor.b;
        glArray4f1.f3 = AppMain.nnlight.nngLight.AmbientColor.a;
        OpenGL.glLightModelfv(2899U, glArray4f1);
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int index1 = 0;
        for (; index1 < AppMain.NNE_LIGHT_MAX; ++index1)
        {
            AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[index1];
            if (nnsGlLightData.bEnable != 0)
            {
                uint fType = nnsGlLightData.fType;
                if (fType <= 8U)
                {
                    switch ((int)fType - 1)
                    {
                        case 0:
                            break;
                        case 1:
                            if (num2 < 4)
                            {
                                ++num2;
                                continue;
                            }
                            nnsGlLightData.bEnable = 0;
                            continue;
                        case 2:
                            continue;
                        case 3:
                            if (num3 < 4)
                            {
                                ++num3;
                                continue;
                            }
                            nnsGlLightData.bEnable = 0;
                            continue;
                        default:
                            if (fType == 8U)
                                goto case 3;
                            else
                                continue;
                    }
                }
                else if (fType != 16U && fType != 32U)
                    continue;
                if (num1 < 4)
                    ++num1;
                else
                    nnsGlLightData.bEnable = 0;
            }
        }
        AppMain.nnlight.nngNumParallelLight = num1;
        AppMain.nnlight.nngNumPointLight = num2;
        AppMain.nnlight.nngNumSpotLight = num3;
        int _idx1 = -1;
        int index2 = -1;
        int index3 = -1;
        int num4 = -1;
        int index4 = 0;
        int _idx2 = 0;
        for (; index4 < AppMain.NNE_LIGHT_MAX; ++index4)
        {
            AppMain.NNS_GL_LIGHT_DATA nnsGlLightData = AppMain.nnlight.nngLight.LightData[index4];
            if (1 == nnsGlLightData.bEnable)
            {
                uint fType = nnsGlLightData.fType;
                uint num5;
                if (fType <= 8U)
                {
                    switch ((int)fType - 1)
                    {
                        case 0:
                            break;
                        case 1:
                            ++index2;
                            num5 = AppMain.NNM_GL_LIGHT(num1 + index2);
                            goto label_27;
                        case 2:
                            goto label_26;
                        case 3:
                            ++index3;
                            num5 = AppMain.NNM_GL_LIGHT(num1 + num2 + index3);
                            goto label_27;
                        default:
                            if (fType == 8U)
                                goto case 3;
                            else
                                goto label_26;
                    }
                }
                else if (fType != 16U && fType != 32U)
                    goto label_26;
                ++_idx1;
                num5 = AppMain.NNM_GL_LIGHT(_idx1);
                goto label_27;
            label_26:
                ++num4;
                num5 = AppMain.NNM_GL_LIGHT(num1 + num2 + num3 + num4);
            label_27:
                OpenGL.glEnable(num5);
                if ((double)nnsGlLightData.Intensity == 1.0)
                {
                    OpenGL.glArray4f glArray4f2;
                    glArray4f2.f0 = nnsGlLightData.Ambient.r;
                    glArray4f2.f1 = nnsGlLightData.Ambient.g;
                    glArray4f2.f2 = nnsGlLightData.Ambient.b;
                    glArray4f2.f3 = nnsGlLightData.Ambient.a;
                    OpenGL.glLightfv(num5, 4608U, ref glArray4f2);
                    glArray4f2.f0 = nnsGlLightData.Diffuse.r;
                    glArray4f2.f1 = nnsGlLightData.Diffuse.g;
                    glArray4f2.f2 = nnsGlLightData.Diffuse.b;
                    glArray4f2.f3 = nnsGlLightData.Diffuse.a;
                    OpenGL.glLightfv(num5, 4609U, ref glArray4f2);
                    glArray4f2.f0 = nnsGlLightData.Specular.r;
                    glArray4f2.f1 = nnsGlLightData.Specular.g;
                    glArray4f2.f2 = nnsGlLightData.Specular.b;
                    glArray4f2.f3 = nnsGlLightData.Specular.a;
                    OpenGL.glLightfv(num5, 4610U, ref glArray4f2);
                }
                else
                {
                    float intensity = nnsGlLightData.Intensity;
                    OpenGL.glArray4f glArray4f2;
                    glArray4f2.f0 = nnsGlLightData.Ambient.r * intensity;
                    glArray4f2.f1 = nnsGlLightData.Ambient.g * intensity;
                    glArray4f2.f2 = nnsGlLightData.Ambient.b * intensity;
                    glArray4f2.f3 = nnsGlLightData.Ambient.a;
                    OpenGL.glLightfv(num5, 4608U, ref glArray4f2);
                    glArray4f2.f0 = nnsGlLightData.Diffuse.r * intensity;
                    glArray4f2.f1 = nnsGlLightData.Diffuse.g * intensity;
                    glArray4f2.f2 = nnsGlLightData.Diffuse.b * intensity;
                    glArray4f2.f3 = nnsGlLightData.Diffuse.a;
                    OpenGL.glLightfv(num5, 4609U, ref glArray4f2);
                    glArray4f2.f0 = nnsGlLightData.Specular.r * intensity;
                    glArray4f2.f1 = nnsGlLightData.Specular.g * intensity;
                    glArray4f2.f2 = nnsGlLightData.Specular.b * intensity;
                    glArray4f2.f3 = nnsGlLightData.Specular.a;
                    OpenGL.glLightfv(num5, 4610U, ref glArray4f2);
                }
                switch (nnsGlLightData.fType)
                {
                    case 1:
                    case 32:
                        OpenGL.glArray4f glArray4f3;
                        glArray4f3.f0 = -nnsGlLightData.Direction.x;
                        glArray4f3.f1 = -nnsGlLightData.Direction.y;
                        glArray4f3.f2 = -nnsGlLightData.Direction.z;
                        glArray4f3.f3 = 0.0f;
                        OpenGL.glLightfv(num5, 4611U, ref glArray4f3);
                        break;
                    case 16:
                        OpenGL.glArray4f glArray4f4;
                        glArray4f4.f0 = nnsGlLightData.Position.x - nnsGlLightData.Target.x;
                        glArray4f4.f1 = nnsGlLightData.Position.y - nnsGlLightData.Target.y;
                        glArray4f4.f2 = nnsGlLightData.Position.z - nnsGlLightData.Target.z;
                        glArray4f4.f3 = 0.0f;
                        AppMain.nnNormalizeVector(ref glArray4f4, ref glArray4f4);
                        OpenGL.glLightfv(num5, 4611U, ref glArray4f4);
                        break;
                    default:
                        OpenGL.glArray4f glArray4f5;
                        glArray4f5.f0 = nnsGlLightData.Position.x;
                        glArray4f5.f1 = nnsGlLightData.Position.y;
                        glArray4f5.f2 = nnsGlLightData.Position.z;
                        glArray4f5.f3 = nnsGlLightData.Position.w;
                        OpenGL.glLightfv(num5, 4611U, ref glArray4f5);
                        break;
                }
                if (nnsGlLightData.fType == 4U)
                {
                    OpenGL.glArray4f glArray4f2;
                    glArray4f2.f0 = nnsGlLightData.Target.x - nnsGlLightData.Position.x;
                    glArray4f2.f1 = nnsGlLightData.Target.y - nnsGlLightData.Position.y;
                    glArray4f2.f2 = nnsGlLightData.Target.z - nnsGlLightData.Position.z;
                    glArray4f2.f3 = 0.0f;
                    AppMain.nnNormalizeVector(ref glArray4f2, ref glArray4f2);
                    OpenGL.glLightfv(num5, 4612U, ref glArray4f2);
                }
                else
                {
                    OpenGL.glArray4f glArray4f2;
                    glArray4f2.f0 = nnsGlLightData.Direction.x;
                    glArray4f2.f1 = nnsGlLightData.Direction.y;
                    glArray4f2.f2 = nnsGlLightData.Direction.z;
                    glArray4f2.f3 = 0.0f;
                    OpenGL.glLightfv(num5, 4612U, ref glArray4f2);
                }
                switch (nnsGlLightData.fType)
                {
                    case 4:
                    case 8:
                    case 65536:
                        OpenGL.glLightf(num5, 4613U, nnsGlLightData.SpotExponent);
                        OpenGL.glLightf(num5, 4614U, nnsGlLightData.SpotCutoff);
                        break;
                    default:
                        OpenGL.glLightf(num5, 4613U, 0.0f);
                        OpenGL.glLightf(num5, 4614U, 180f);
                        break;
                }
                switch (nnsGlLightData.fType)
                {
                    case 2:
                    case 4:
                    case 8:
                    case 65536:
                        OpenGL.glLightf(num5, 4615U, nnsGlLightData.ConstantAttenuation);
                        OpenGL.glLightf(num5, 4616U, nnsGlLightData.LinearAttenuation);
                        OpenGL.glLightf(num5, 4617U, nnsGlLightData.QuadraticAttenuation);
                        break;
                    default:
                        OpenGL.glLightf(num5, 4615U, 1f);
                        OpenGL.glLightf(num5, 4616U, 0.0f);
                        OpenGL.glLightf(num5, 4617U, 0.0f);
                        break;
                }
                switch (nnsGlLightData.fType)
                {
                    case 2:
                        AppMain.nnlight.nngPointLightFallOffEnd[index2] = nnsGlLightData.FallOffEnd;
                        float num6 = nnsGlLightData.FallOffEnd - nnsGlLightData.FallOffStart;
                        float num7 = (double)num6 > 9.99999996004197E-13 ? 1f / num6 : 1E+12f;
                        AppMain.nnlight.nngPointLightFallOffScale[index2] = num7;
                        break;
                    case 4:
                    case 8:
                        AppMain.nnlight.nngSpotLightFallOffEnd[index3] = nnsGlLightData.FallOffEnd;
                        float num8 = nnsGlLightData.FallOffEnd - nnsGlLightData.FallOffStart;
                        float num9 = (double)num8 > 9.99999996004197E-13 ? 1f / num8 : 1E+12f;
                        AppMain.nnlight.nngSpotLightFallOffScale[index3] = num9;
                        float num10 = AppMain.nnCos(nnsGlLightData.OuterAngle) - AppMain.nnCos(nnsGlLightData.InnerAngle);
                        float num11 = (double)num10 < 9.99999996004197E-13 ? 1f / num10 : -1E+12f;
                        AppMain.nnlight.nngSpotLightAngleScale[index3] = num11;
                        break;
                }
                ++_idx2;
            }
        }
        for (; _idx2 < AppMain.NNE_LIGHT_MAX; ++_idx2)
            OpenGL.glDisable(AppMain.NNM_GL_LIGHT(_idx2));
    }

    private uint nnEstimateLightBufferSize(uint type)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private void nnCalcMotionCameraScalar(AppMain.NNS_SUBMOTION submot, float frame, ref float val)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMotionCameraAngle(AppMain.NNS_SUBMOTION submot, float frame, ref int ang)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMotionCameraXYZ(
      AppMain.NNS_SUBMOTION submot,
      float frame,
      AppMain.NNS_VECTOR xyz)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcCameraMotionCore(
      AppMain.NNS_CAMERAPTR dstptr,
      AppMain.NNS_CAMERAPTR camptr,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcCameraMotion(
      AppMain.NNS_CAMERAPTR dstptr,
      AppMain.NNS_CAMERAPTR camptr,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.mppAssertNotImpl();
    }

    public static void nnSetUpTexlist(out AppMain.NNS_TEXLIST texlist, int num, ref object buf)
    {
        buf = (object)(texlist = new AppMain.NNS_TEXLIST());
        texlist.nTex = num;
        texlist.pTexInfoList = new AppMain.NNS_TEXINFO[num];
        for (int index = 0; index < num; ++index)
            texlist.pTexInfoList[index] = new AppMain.NNS_TEXINFO();
    }

    public static uint nnEstimateTexlistSize(int num)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private static int nnSetTextureList(AppMain.NNS_TEXLIST pTexList)
    {
        AppMain.nngCurrentTextureList = pTexList;
        return 1;
    }

    public static int nnGetTextureList(out AppMain.NNS_TEXLIST pTexList)
    {
        pTexList = (AppMain.NNS_TEXLIST)null;
        AppMain.mppAssertNotImpl();
        return 1;
    }

    private static int nnSetTexInfo(int slot, AppMain.NNS_TEXINFO pTexInfo)
    {
        if (slot >= AppMain.nngGLExtensions.max_texture_units)
            return 1;
        OpenGL.glActiveTexture(AppMain.NNM_GL_TEXTURE(slot));
        if (pTexInfo != null)
            OpenGL.glBindTexture(3553U, pTexInfo.TexName);
        if (pTexInfo != null)
            OpenGL.glEnable(3553U);
        else
            OpenGL.glDisable(3553U);
        return 1;
    }

    public static int nnSetTexture(int slot, AppMain.NNS_TEXLIST pTexList, int num)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private static int nnSetTextureNum(int slot, int num)
    {
        if (num < 0)
            return AppMain.nnSetTexInfo(slot, (AppMain.NNS_TEXINFO)null);
        AppMain.NNS_TEXINFO[] pTexInfoList = AppMain.nngCurrentTextureList.pTexInfoList;
        return AppMain.nnSetTexInfo(slot, pTexInfoList[num]);
    }

    private void nnConfigureSystemGL(AppMain.NNS_CONFIG_GL config)
    {
        if (AppMain.nnsystem_init != 1)
        {
            AppMain.nnsystem_init = 1;
            this.nnInitCircumsphere();
        }
        AppMain.nngScreen.ax = 1f;
        AppMain.nngScreen.ay = 1f;
        AppMain.nngScreen.aspect = 1f;
        AppMain.nngScreen.dist = 500f;
        AppMain.nngScreen.xad = AppMain.nngScreen.ax * AppMain.nngScreen.dist;
        AppMain.nngScreen.yad = (float)-((double)AppMain.nngScreen.ay * (double)AppMain.nngScreen.dist);
        AppMain.nngScreen.ooxad = 1f / AppMain.nngScreen.xad;
        AppMain.nngScreen.ooyad = 1f / AppMain.nngScreen.yad;
        AppMain.nngScreen.w = (float)config.WindowWidth;
        AppMain.nngScreen.h = (float)config.WindowHeight;
        AppMain.nngScreen.cx = AppMain.nngScreen.w * 0.5f;
        AppMain.nngScreen.cy = AppMain.nngScreen.h * 0.5f;
        AppMain.nngClip2d.x0 = 0.0f;
        AppMain.nngClip2d.y0 = 0.0f;
        AppMain.nngClip2d.x1 = AppMain.nngScreen.w - 1f;
        AppMain.nngClip2d.y1 = AppMain.nngScreen.h - 1f;
        AppMain.nngClip2d.n_clip = 1f;
        AppMain.nngClip2d.f_clip = 10000f;
        AppMain.nngClip3d.x0 = AppMain.nngClip2d.x0 - AppMain.nngScreen.cx;
        AppMain.nngClip3d.y0 = AppMain.nngClip2d.y0 - AppMain.nngScreen.cy;
        AppMain.nngClip3d.x1 = AppMain.nngClip2d.x1 - AppMain.nngScreen.cx;
        AppMain.nngClip3d.y1 = AppMain.nngClip2d.y1 - AppMain.nngScreen.cy;
        AppMain.nngClip3d.n_clip = 1f;
        AppMain.nngClip3d.f_clip = 10000f;
    }

    public static void nnSetProjection(AppMain.NNS_MATRIX mtx, int type)
    {
        AppMain.nngProjectionMatrix.Assign(mtx);
        AppMain.nngProjectionType = type;
        AppMain.nnSetClipPlane();
        AppMain.nnLoadProjectionMatrixGL(mtx);
    }

    public static void nnLoadProjectionMatrixGL(AppMain.NNS_MATRIX mtx)
    {
        float m20 = mtx.M20;
        float m21 = mtx.M21;
        float m22 = mtx.M22;
        float m23 = mtx.M23;
        OpenGL.glMatrixMode(5889U);
        Matrix matrix = (Matrix)mtx;
        OpenGL.glLoadMatrixf(ref matrix);
        mtx.M20 = m20;
        mtx.M21 = m21;
        mtx.M22 = m22;
        mtx.M23 = m23;
    }

    private void nnCalcMultiplyMatrices(
      AppMain.ArrayPointer<AppMain.NNS_MATRIX> dstlist,
      AppMain.NNS_MATRIX src,
      AppMain.ArrayPointer<AppMain.NNS_MATRIX> srclist,
      int num)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixPaletteMultiplyMatrix(
      AppMain.ArrayPointer<AppMain.NNS_MATRIX> dstpal,
      AppMain.NNS_MATRIX src,
      AppMain.ArrayPointer<AppMain.NNS_MATRIX> srcpal,
      int num)
    {
        this.nnCalcMultiplyMatrices(dstpal, src, srcpal, num);
    }

    private uint nnCalcShaderManageBufferSizeGL(int num)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private void nnSetUpShaderConfigBasicGL(AppMain.NNS_SHADER_CONFIG config)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnConfigureShaderGL(AppMain.NNS_SHADER_CONFIG config, object managebuffer, int num)
    {
        AppMain.mppAssertNotImpl();
    }

    private AppMain.NNS_SHADER_NAME nnGetShaderNameGL(AppMain.NNS_SHADER_PROFILE profile)
    {
        AppMain.NNS_SHADER_NAME nnsShaderName = new AppMain.NNS_SHADER_NAME();
        AppMain.mppAssertNotImpl();
        return nnsShaderName;
    }

    private void nnGetShaderProfileGL(
      AppMain.NNS_SHADER_PROFILE profile,
      AppMain.NNS_SHADER_NAME Name)
    {
        AppMain.mppAssertNotImpl();
    }

    private static int nnCompareShaderName(AppMain.NNS_SHADER_NAME lhs, AppMain.NNS_SHADER_NAME rhs)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private int nnRegistShaderNameGL(AppMain.NNS_SHADER_NAME Name)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private int nnRegistShaderProfileGL(AppMain.NNS_SHADER_PROFILE profile)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private static int nnGetTexCoord(uint fType)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private static int nnTexCoordIndex(int texcoord)
    {
        AppMain.mppAssertNotImpl();
        return texcoord;
    }

    private void nnInitShaderProfileGL(AppMain.NNS_SHADER_PROFILE profile)
    {
        AppMain.mppAssertNotImpl();
    }

    private int nnSetupShaderProfile(
      AppMain.NNS_SHADER_PROFILE profile,
      AppMain.NNS_MATERIALPTR pMat,
      AppMain.NNS_VTXLISTPTR pVtxListPtr,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private int nnRegistObjectShaderProfilesGL(AppMain.NNS_OBJECT obj, uint flag)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private int nnGetCurrentShaderProfileNumberGL()
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private void nnGetShaderProfileOneGL(AppMain.NNS_SHADER_PROFILE profile, int idx)
    {
        AppMain.mppAssertNotImpl();
    }

    private AppMain.NNS_SHADER_NAME nnGetShaderNameListGL()
    {
        AppMain.mppAssertNotImpl();
        return new AppMain.NNS_SHADER_NAME();
    }

    private void nnClearShaderProfilesGL()
    {
        AppMain.mppAssertNotImpl();
    }

    private uint nnCalcBuildShaderWorkBufferSizeGL(uint vtxshadersize, uint fragshadersize)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private static int nnCompareShaderManager(object elem1, object elem2)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private int nnGetUnbuildShaderProfileNumberGL()
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private int nnGetUnbuildShaderProfileOneGL(AppMain.NNS_SHADER_PROFILE profile)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private void nnRegistCompiledShaderProfileGL(
      AppMain.NNS_COMPILED_SHADER_PROFILE compiledShader,
      AppMain.NNS_SHADER_PROFILE profile)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnBindVertexAttributeGL(uint program)
    {
        AppMain.mppAssertNotImpl();
    }

    private uint nnGetErrorVertexShaderObjectGL()
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private uint nnGetErrorFragmentShaderObjectGL()
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private uint nnGetErrorShaderProgramObjectGL()
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private void nnReleaseShaderGL()
    {
        AppMain.mppAssertNotImpl();
    }

    private static AppMain.NNS_SHADER_MANAGER nnSearchShaderManager(
      AppMain.NNS_SHADER_NAME name)
    {
        AppMain.mppAssertNotImpl();
        return (AppMain.NNS_SHADER_MANAGER)null;
    }

    private void nnPutColorShader(AppMain.NNS_DRAWCALLBACK_VAL val)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnBindFixedShaderGL()
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnBindPrintShaderGL()
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnRegistPrimitive2DShaderGL(int bTexture)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnRegistPrimitive3DShaderGL(int bLighting, int bTexture, int texcoord)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnBindPrimitive2DShaderGL(int bTexture)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnBindPrimitive3DShaderGL(int bLighting, int bTexture, int texcoord)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnRegistDefaultShader()
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnSetUserUniformGL(int idx, float x, float y, float z, float w)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnPutUserUniformGL(AppMain.NNS_DRAWCALLBACK_VAL val)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnSetFogSwitch(bool on_off)
    {
        AppMain.nngFogSwitch = on_off;
    }

    private bool nnGetFogSwitch()
    {
        AppMain.mppAssertNotImpl();
        return AppMain.nngFogSwitch;
    }

    private static void nnSetFogColor(float r, float g, float b)
    {
        AppMain.nnSetFogColor_col[0] = r;
        AppMain.nnSetFogColor_col[1] = g;
        AppMain.nnSetFogColor_col[2] = b;
        AppMain.nnSetFogColor_col[3] = 1f;
        OpenGL.glFogfv(2918U, AppMain.nnSetFogColor_col);
    }

    private static void nnSetFogRange(float fnear, float ffar)
    {
        OpenGL.glFogf(2917U, 9729f);
        AppMain.nngFogStart = fnear;
        AppMain.nngFogEnd = ffar;
    }

    private void nnSetFogLinearGL(float fnear, float ffar)
    {
        AppMain.mppAssertNotImpl();
        OpenGL.glFogf(2917U, 9729f);
        AppMain.nngFogStart = fnear;
        AppMain.nngFogEnd = ffar;
    }

    private void nnSetFogExpGL(float density)
    {
        AppMain.mppAssertNotImpl();
        OpenGL.glFogf(2917U, 2048f);
        AppMain.nngFogDensity = density;
    }

    private void nnSetFogExp2GL(float density)
    {
        AppMain.mppAssertNotImpl();
        OpenGL.glFogf(2917U, 2049f);
        AppMain.nngFogDensity = density;
    }

    private void nnSetFogDensityGL(float density)
    {
        AppMain.mppAssertNotImpl();
        AppMain.nngFogDensity = density;
    }

    private static void nnPutFogSwitchGL(bool on_off)
    {
        if (on_off)
        {
            OpenGL.glEnable(2912U);
            OpenGL.glFogf(2915U, AppMain.nngFogStart);
            OpenGL.glFogf(2916U, AppMain.nngFogEnd);
            OpenGL.glFogf(2914U, AppMain.nngFogDensity);
        }
        else
        {
            OpenGL.glDisable(2912U);
            OpenGL.glFogf(2915U, AppMain.nngClip3d.f_clip);
            OpenGL.glFogf(2916U, AppMain.nngClip3d.f_clip + 1f);
            OpenGL.glFogf(2914U, 0.0f);
        }
    }

    private void nnDrawMultiObjectInitialPose(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX[] basemtxptrlist,
      uint[] nodestatlistptrlist,
      uint subobjtype,
      uint flag,
      int num)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnSetMaterialControlDiffuse(int mode, float r, float g, float b)
    {
        AppMain.nngMatCtrlDiffuse.mode = mode;
        AppMain.nngMatCtrlDiffuse.col.r = r;
        AppMain.nngMatCtrlDiffuse.col.g = g;
        AppMain.nngMatCtrlDiffuse.col.b = b;
    }

    private static void nnSetMaterialControlAmbient(int mode, float r, float g, float b)
    {
        AppMain.nngMatCtrlAmbient.mode = mode;
        AppMain.nngMatCtrlAmbient.col.r = r;
        AppMain.nngMatCtrlAmbient.col.g = g;
        AppMain.nngMatCtrlAmbient.col.b = b;
    }

    private void nnSetMaterialControlSpecularGL(int mode, float r, float g, float b)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnSetMaterialControlAlpha(int mode, float alpha)
    {
        AppMain.nngMatCtrlAlpha.mode = mode;
        AppMain.nngMatCtrlAlpha.alpha = alpha;
    }

    private static void nnSetMaterialControlEnvTexMatrix(int texsrc, AppMain.NNS_MATRIX texmtx)
    {
        AppMain.nngMatCtrlEnvTexMtx.texcoordsrc = texsrc;
        AppMain.nnCopyMatrix(AppMain.nngMatCtrlEnvTexMtx.texmtx, texmtx);
    }

    private static void nnSetMaterialControlBlendMode(int blendmode)
    {
        AppMain.nngMatCtrlBlendMode.blendmode = blendmode;
    }

    private static void nnSetMaterialControlTextureOffset(int slot, int mode, float u, float v)
    {
        AppMain.nngMatCtrlTexOffset[slot].mode = mode;
        AppMain.nngMatCtrlTexOffset[slot].offset.u = u;
        AppMain.nngMatCtrlTexOffset[slot].offset.v = v;
    }

    private static void nnSetPrimitiveBlend(int blend)
    {
        AppMain.nngDrawPrimBlend = blend;
    }

    private static void nnSetPrimitiveTexNum(AppMain.NNS_TEXLIST texlist, int num)
    {
        if (texlist != null && num >= 0 && num < texlist.nTex)
        {
            AppMain.nngDrawPrimTexName = texlist.pTexInfoList[num].TexName;
            AppMain.nngDrawPrimTexture = 1;
        }
        else
            AppMain.nngDrawPrimTexture = 0;
    }

    private static void nnSetPrimitiveTexState(int blend, int coord, int uwrap, int vwrap)
    {
        switch (blend)
        {
            case 1:
                AppMain.nngDrawPrimTexBlend = 7681;
                break;
            default:
                AppMain.nngDrawPrimTexBlend = 8448;
                break;
        }
        AppMain.nngDrawPrimTexCoord = coord;
        switch (uwrap)
        {
            case 1:
                AppMain.nngDrawPrimTexSWarp = 33071;
                break;
            default:
                AppMain.nngDrawPrimTexSWarp = 10497;
                break;
        }
        switch (vwrap)
        {
            case 1:
                AppMain.nngDrawPrimTexTWarp = 33071;
                break;
            default:
                AppMain.nngDrawPrimTexTWarp = 10497;
                break;
        }
    }

    private static void nnPutPrimitiveTexParameter()
    {
        OpenGL.glClientActiveTexture(33985U);
        OpenGL.glDisableClientState(32888U);
        OpenGL.glClientActiveTexture(33986U);
        OpenGL.glDisableClientState(32888U);
        OpenGL.glClientActiveTexture(33987U);
        OpenGL.glDisableClientState(32888U);
        OpenGL.glActiveTexture(33985U);
        OpenGL.glDisable(3553U);
        OpenGL.glActiveTexture(33984U);
        OpenGL.glEnable(3553U);
        OpenGL.glBindTexture(3553U, AppMain.nngDrawPrimTexName);
        OpenGL.glTexEnvi(8960U, 8704U, AppMain.nngDrawPrimTexBlend);
        OpenGL.glTexParameteri(3553U, 10242U, AppMain.nngDrawPrimTexSWarp);
        OpenGL.glTexParameteri(3553U, 10243U, AppMain.nngDrawPrimTexTWarp);
    }

    private static void nnPutPrimitiveNoTexture()
    {
        OpenGL.glClientActiveTexture(33984U);
        OpenGL.glDisableClientState(32888U);
        OpenGL.glClientActiveTexture(33985U);
        OpenGL.glDisableClientState(32888U);
        OpenGL.glActiveTexture(33984U);
        OpenGL.glDisable(3553U);
        OpenGL.glActiveTexture(33985U);
        OpenGL.glDisable(3553U);
    }

}