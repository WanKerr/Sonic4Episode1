using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mpp;


public partial class AppMain
{
    private void nnDrawMultiObjectInitialPoseBaseMatrixList(
        NNS_OBJECT obj,
        NNS_MATRIX basemtxlist,
        uint[] nodestatlistptrlist,
        uint subobjtype,
        uint flag,
        int num)
    {
        mppAssertNotImpl();
    }

    private void nnSetBoneColor(
        ref NNS_RGBA pDiff,
        NNS_RGB pAmb,
        ref NNS_RGBA pWire)
    {
        mppAssertNotImpl();
    }

    private void nnDrawOneBoneData(float bonelength, NNS_MATRIX mtx, uint flag)
    {
        mppAssertNotImpl();
    }

    private void nnSetEffectorColor(
        ref NNS_RGBA pXcol,
        ref NNS_RGBA pYcol,
        ref NNS_RGBA pZcol)
    {
        mppAssertNotImpl();
    }

    private void nnDrawEffector(NNS_VECTOR p, NNS_MATRIX mtx)
    {
        mppAssertNotImpl();
    }

    private void nnDrawSIIKBone(
        NNS_OBJECT obj,
        NNS_MATRIX basemtx,
        NNS_MATRIX mtxlist,
        uint flag)
    {
        NNS_RGBA nnsRgba1 = new NNS_RGBA(1f, 1f, 1f, 0.5f);
        NNS_RGBA nnsRgba2 = new NNS_RGBA(1f, 0.0f, 0.0f, 0.5f);
        NNS_RGBA nnsRgba3 = new NNS_RGBA(0.0f, 1f, 0.0f, 0.5f);
        NNS_RGBA nnsRgba4 = new NNS_RGBA(0.0f, 0.0f, 1f, 0.5f);
        NNS_RGBA nnsRgba5 = new NNS_RGBA(1f, 1f, 0.0f, 0.5f);
        NNS_RGBA nnsRgba6 = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_RGB nnsRgb = new NNS_RGB(0.2f, 0.2f, 0.2f);
        NNS_RGBA nnsRgba7 = new NNS_RGBA(1f, 1f, 1f, 1f);
        mppAssertNotImpl();
    }

    private void nnMakeNodeTreeMatrix(
        NNS_MATRIX mtx,
        NNS_VECTOR vec,
        NNS_VECTOR trans)
    {
        GlobalPool<NNS_VECTOR>.Alloc();
        GlobalPool<NNS_VECTOR>.Alloc();
        GlobalPool<NNS_VECTOR>.Alloc();
        mppAssertNotImpl();
    }

    private void nnDrawNodeTree(
        NNS_OBJECT obj,
        NNS_MATRIX basemtx,
        NNS_MATRIX mtxlist,
        uint flag)
    {
        mppAssertNotImpl();
    }

    private void nnDrawAxis(NNS_VECTOR p, float length, NNS_MATRIX mtx)
    {
        mppAssertNotImpl();
    }

    private uint nnCalcGridBufferSize(int Xnum, int Znum)
    {
        mppAssertNotImpl();
        return 0;
    }

    private void nnInitGrid(NNS_VECTOR pBuf, int Xnum, int Znum)
    {
        this.nngGridPos = pBuf;
        this.nngGridXnum = Xnum;
        this.nngGridZnum = Znum;
    }

    private void nnDrawGrid(NNS_VECTOR p, float length, NNS_MATRIX mtx)
    {
        mppAssertNotImpl();
    }

    private void nnDrawGridPlane(
        int Xnum,
        int Znum,
        float length,
        NNS_MATRIX mtx,
        ref NNS_RGBA pcolor)
    {
        mppAssertNotImpl();
    }

    private static void nnSetTexCoordSrc(int slot, int src)
    {
        nnsTexCoordSrc[slot] = src;
    }

    private static int nnGetTexCoordSrc(int slot)
    {
        return nnsTexCoordSrc[slot];
    }

    private static void nnSetNormalFormatType(uint ftype)
    {
        nnsNormalFormatType = ftype;
    }

    private uint nnGetNormalFormatType()
    {
        return nnsNormalFormatType;
    }

    private static void nnPutEnvironmentTextureMatrix(NNS_MATRIX pEnvMtx)
    {
        if (nnsTexCoordSrc[0] != 3 && nnsTexCoordSrc[1] != 3)
            return;
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        nnMakeTranslateMatrix(nnsMatrix1, 0.5f, 0.5f, 0.0f);
        nnScaleMatrix(nnsMatrix1, nnsMatrix1, 0.5f, -0.5f, 0.0f);
        if (pEnvMtx != null)
        {
            NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
            nnCopyMatrix(nnsMatrix2, pEnvMtx);
            nnsMatrix2.M03 = 0.0f;
            nnsMatrix2.M13 = 0.0f;
            nnMultiplyMatrix(nnsMatrix1, nnsMatrix1, nnsMatrix2);
        }

        if (nnsNormalFormatType == 5122U)
        {
            nnScaleMatrix(nnsMatrix1, nnsMatrix1, 3.051804E-05f, 3.051804E-05f, 3.051804E-05f);
            nnTranslateMatrix(nnsMatrix1, nnsMatrix1, 0.5f, 0.5f, 0.5f);
        }

        OpenGL.glMatrixMode(5890U);
        for (int _slot = 0; _slot < 2; ++_slot)
        {
            if (nnsTexCoordSrc[_slot] == 3)
            {
                OpenGL.glActiveTexture(NNM_GL_TEXTURE(_slot));
                Matrix matrix = (Matrix)nnsMatrix1;
                OpenGL.glLoadMatrixf(ref matrix);
            }
        }

        GlobalPool<NNS_MATRIX>.Release(nnsMatrix1);
    }

    private static void nnDrawObjectVertexList(NNS_VTXLISTPTR vlistptr, uint flag)
    {
        nnDrawObjectVertexList(vlistptr, flag, 0U);
    }

    private static void nnMPPVerifyAlternativeLightingSettings()
    {
        OpenGL.BufferItem buffer1 = OpenGL.m_buffers[OpenGL.m_boundArrayBuffer];
        OpenGL.VertexBufferDesc buffer2 = (OpenGL.VertexBufferDesc)buffer1.buffer;
        VertexBuffer buffer3 = buffer2.Buffer;
        uint lightColorAbgr = GmMainGetLightColorABGR();
        if ((int)buffer2.VertexColor == (int)lightColorAbgr)
            return;
        if (buffer2.vertices == null)
            buffer2.vertices = new OpenGL.Vertex[buffer3.VertexCount];
        OpenGL.Vertex[] rawData = (OpenGL.Vertex[])buffer1.rawData;
        Array.Copy(rawData, buffer2.vertices, buffer3.VertexCount);
        for (int index = 0; index < buffer2.vertices.Length; ++index)
        {
            rawData[index].Color.PackedValue = lightColorAbgr;
            Vector2.Transform(ref buffer2.vertices[index].TextureCoordinate, ref buffer2.TextureMatrix,
                out buffer2.vertices[index].TextureCoordinate);
        }

        buffer3.SetData(rawData);
        buffer2.VertexColor = lightColorAbgr;
    }

    private static void nnDrawObjectVertexList(
        NNS_VTXLISTPTR vlistptr,
        uint flag,
        uint alternativeLighting)
    {
        NNS_VTXLIST_GL_DESC pVtxList = (NNS_VTXLIST_GL_DESC)vlistptr.pVtxList;
        uint type1 = pVtxList.Type;
        int nArray = pVtxList.nArray;
        OpenGL.glBindBuffer(34962U, pVtxList.BufferName);
        if (alternativeLighting != 0U)
        {
            OpenGL.glArray4f glArray4f1 = new OpenGL.glArray4f(0.0f, 0.0f, -1f, 0.0f);
            OpenGL.glArray4f glArray4f2 = ((int)alternativeLighting & 32768) == 0
                ? GmMainGetLightColorArray4f()
                : BreakWall_1_3_Color;
            OpenGL.glLightfv(16384U, 4612U, ref glArray4f1);
            OpenGL.glLightfv(16384U, 4609U, ref glArray4f2);
            nnMPPVerifyAlternativeLightingSettings();
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
        if (nnGetTexCoordSrc(0) != 0)
            OpenGL.glEnableClientState(32888U);
        else
            OpenGL.glDisableClientState(32888U);
        OpenGL.glClientActiveTexture(33985U);
        if (nnGetTexCoordSrc(1) != 0)
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
            NNS_VTXARRAY_GL p = pVtxList.pArray[index];
            uint type2 = p.Type;
            if (type2 <= 8U)
            {
                switch ((int)type2 - 1)
                {
                    case 0:
                        OpenGL.glVertexPointer(p.Size, p.DataType, p.Stride, p.Data);
                        if (nnGetTexCoordSrc(0) == 4)
                        {
                            OpenGL.glClientActiveTexture(33984U);
                            OpenGL.glEnableClientState(32888U);
                            OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                        }

                        if (nnGetTexCoordSrc(1) == 4)
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
                            nnSetNormalFormatType(p.DataType);
                            if (nnGetTexCoordSrc(0) == 3)
                            {
                                OpenGL.glClientActiveTexture(33984U);
                                OpenGL.glEnableClientState(32888U);
                                OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                            }

                            if (nnGetTexCoordSrc(1) == 3)
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
                        if (nnGetTexCoordSrc(0) == 2)
                        {
                            OpenGL.glClientActiveTexture(33984U);
                            OpenGL.glEnableClientState(32888U);
                            OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                        }

                        if (nnGetTexCoordSrc(1) == 2)
                        {
                            OpenGL.glClientActiveTexture(33985U);
                            OpenGL.glEnableClientState(32888U);
                            OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                        }
                    }
                }
                else
                {
                    if (nnGetTexCoordSrc(0) == 1)
                    {
                        OpenGL.glClientActiveTexture(33984U);
                        OpenGL.glEnableClientState(32888U);
                        OpenGL.glTexCoordPointer(p.Size, p.DataType, p.Stride, p.Data);
                    }

                    if (nnGetTexCoordSrc(1) == 1)
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

    private static void nnDrawObjectPrimitiveList(NNS_PRIMLISTPTR plistptr, uint flag)
    {
        NNS_PRIMLIST_GL_DESC pPrimList = (NNS_PRIMLIST_GL_DESC)plistptr.pPrimList;
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
                    OpenGL.glDrawElements(3U, pPrimList.pCounts[index1], pPrimList.DataType, null);
                    for (int index2 = 0; index2 <= 1; ++index2)
                    {
                        int count = 0;
                        for (int index3 = index2; index3 < pPrimList.pCounts[index1]; index3 += 2)
                        {
                            numArray1[count++] = pIndex[index3];
                            if (count == length)
                            {
                                OpenGL.glDrawElements(3U, count, 5123U, null);
                                int num1 = 0;
                                int index4 = num1;
                                count = index4 + 1;
                                int num2 = pIndex[index3];
                                numArray1[index4] = (ushort)num2;
                            }
                        }

                        if (count >= 2)
                            OpenGL.glDrawElements(3U, count, 5123U, null);
                    }
                }

                break;
            case 12288:
                int nPrim1 = pPrimList.nPrim;
                float[] v = new float[3];
                for (int iStrip = 0; iStrip < nPrim1; ++iStrip)
                {
                    nnPutColorStrip(iStrip, nngDrawCallBackVal.iMeshset, nngDrawCallBackVal.iSubobject);
                    v[0] = random.Next(0, short.MaxValue) / (float)short.MaxValue;
                    v[1] = random.Next(0, short.MaxValue) / (float)short.MaxValue;
                    v[2] = random.Next(0, short.MaxValue) / (float)short.MaxValue;
                    OpenGL.glColor3fv(v);
                    OpenGL.glDrawElements(pPrimList.Mode, pPrimList.pCounts[iStrip], pPrimList.DataType, null);
                }

                break;
            default:
                int nPrim2 = pPrimList.nPrim;
                for (int index = 0; index < nPrim2; ++index)
                    OpenGL.glDrawElements(pPrimList.Mode, pPrimList.pCounts[index], pPrimList.DataType, null);
                break;
        }
    }

    private static void nnDrawObject(
        NNS_OBJECT obj,
        NNS_MATRIX[] mtxpal,
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
        nngDrawCallBackVal.pObject = obj;
        nngDrawCallBackVal.pMatrixPalette = mtxpal;
        nngDrawCallBackVal.pNodeStatusList = nodestatlist;
        nngDrawCallBackVal.DrawSubobjType = subobjtype;
        nngDrawCallBackVal.DrawFlag = flag;
        nngDrawCallBackVal.iPrevMaterial = -1;
        nngDrawCallBackVal.iPrevVtxList = -1;
        if (subobjtype == 2147483648U)
            subobjtype = 775U;
        OpenGL.glShadeModel(7425U);
        for (int index1 = 0; index1 < obj.nSubobj; ++index1)
        {
            NNS_SUBOBJ pSubobj = obj.pSubobjList[index1];
            if (((int)pSubobj.fType & (int)subobjtype & 7) != 0 &&
                ((int)pSubobj.fType & (int)subobjtype & 768) != 0)
            {
                nngDrawCallBackVal.iSubobject = index1;
                if (((int)pSubobj.fType & 512) != 0)
                {
                    OpenGL.glMatrixMode(5888U);
                    OpenGL.glLoadIdentity();
                }

                for (int index2 = 0; index2 < pSubobj.nMeshset; ++index2)
                {
                    NNS_MESHSET pMeshset = pSubobj.pMeshsetList[index2];
                    NNS_VTXLISTPTR pVtxListPtr = obj.pVtxListPtrList[pMeshset.iVtxList];
                    NNS_PRIMLISTPTR pPrimListPtr = obj.pPrimListPtrList[pMeshset.iPrimList];
                    if (nodestatlist == null || ((int)nodestatlist[pMeshset.iNode] & 1) == 0)
                    {
                        nngDrawCallBackVal.iMeshset = index2;
                        nngDrawCallBackVal.iNode = pMeshset.iNode;
                        nngDrawCallBackVal.iMaterial = pMeshset.iMaterial;
                        nngDrawCallBackVal.pMaterial = obj.pMatPtrList[pMeshset.iMaterial];
                        nngDrawCallBackVal.iVtxList = pMeshset.iVtxList;
                        nngDrawCallBackVal.pVtxListPtr = pVtxListPtr;
                        nngDrawCallBackVal.bModified = 0;
                        nngDrawCallBackVal.bReDraw = 0;
                        OpenGL.glMatrixMode(5888U);
                        if (((int)pSubobj.fType & 256) != 0)
                        {
                            Matrix matrix = (Matrix)mtxpal[pMeshset.iMatrix];
                            OpenGL.glLoadMatrixf(ref matrix);
                        }
                        else
                            OpenGL.glLoadIdentity();

                        while (nnPutMaterial(nngDrawCallBackVal) != 0)
                        {
                            nngDrawCallBackVal.iPrevMaterial = pMeshset.iMaterial;
                            nngDrawCallBackVal.iPrevVtxList = pMeshset.iVtxList;
                            if (((int)pVtxListPtr.fType & 1) != 0)
                            {
                                if (((int)flag & 768) != 0)
                                {
                                    nnDrawObjectNormal(pVtxListPtr, pPrimListPtr, mtxpal, flag);
                                }
                                else
                                {
                                    if (((int)pSubobj.fType & 512) != 0)
                                    {
                                        NNS_VTXLIST_GL_DESC pVtxList = (NNS_VTXLIST_GL_DESC)pVtxListPtr.pVtxList;
                                        int nMatrix = pVtxList.nMatrix;
                                        OpenGL.glEnable(34880U);
                                        OpenGL.glMatrixMode(34880U);
                                        for (uint matrixpaletteindex = 0;
                                             matrixpaletteindex < nMatrix;
                                             ++matrixpaletteindex)
                                        {
                                            OpenGL.glCurrentPaletteMatrixOES(matrixpaletteindex);
                                            Matrix matrix =
                                                (Matrix)mtxpal[pVtxList.pMatrixIndices[(int)matrixpaletteindex]];
                                            OpenGL.glLoadMatrixf(ref matrix);
                                        }
                                    }
                                    else
                                        OpenGL.glDisable(34880U);

                                    if (num1 != pMeshset.iVtxList || num2 != nnGetTexCoordSrc(0) ||
                                        num3 != nnGetTexCoordSrc(1))
                                    {
                                        nnDrawObjectVertexList(pVtxListPtr, flag, alternativeLighting);
                                        num1 = pMeshset.iVtxList;
                                        num2 = nnGetTexCoordSrc(0);
                                        num3 = nnGetTexCoordSrc(1);
                                        if (pMeshset.iMatrix != -1)
                                            nnPutEnvironmentTextureMatrix(mtxpal[pMeshset.iMatrix]);
                                        else
                                            nnPutEnvironmentTextureMatrix(mtxpal[0]);
                                    }

                                    nnDrawObjectPrimitiveList(pPrimListPtr, flag);
                                }
                            }
                            else
                            {
                                int num4 = (int)pVtxListPtr.fType & 16711680;
                            }

                            if (nngDrawCallBackVal.bReDraw == 0)
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
        mppAssertNotImpl();
        return 0;
    }

    public static void nnSetClipPlane()
    {
        if (nngProjectionType != 1)
        {
            float num1 = 1f / nngProjectionMatrix.M00;
            float num2 = 1f / nngProjectionMatrix.M11;
            float num3 = num2 * (nngProjectionMatrix.M12 - nngClip3d.y0 / nngScreen.cy);
            float num4 = nnSqrt((float)(num3 * (double)num3 + 1.0));
            float num5 = num3 / num4;
            float num6 = 1f / num4;
            nngClipPlane.Top.ny = num6;
            nngClipPlane.Top.nz = num5;
            float num7 = num2 * (nngProjectionMatrix.M12 - nngClip3d.y1 / nngScreen.cy);
            float num8 = nnSqrt((float)(num7 * (double)num7 + 1.0));
            float num9 = num7 / num8;
            float num10 = 1f / num8;
            nngClipPlane.Bottom.ny = -num10;
            nngClipPlane.Bottom.nz = -num9;
            float num11 = num1 * (nngProjectionMatrix.M02 + nngClip3d.x1 / nngScreen.cx);
            float num12 = nnSqrt((float)(num11 * (double)num11 + 1.0));
            float num13 = num11 / num12;
            float num14 = 1f / num12;
            nngClipPlane.Right.nx = num14;
            nngClipPlane.Right.nz = num13;
            float num15 = num1 * (nngProjectionMatrix.M02 + nngClip3d.x0 / nngScreen.cx);
            float num16 = nnSqrt((float)(num15 * (double)num15 + 1.0));
            float num17 = num15 / num16;
            float num18 = 1f / num16;
            nngClipPlane.Left.nx = -num18;
            nngClipPlane.Left.nz = -num17;
        }
        else
        {
            float num1 = (float)(2.0 / nngProjectionMatrix.M11 / 2.0);
            float num2 = (float)-(nngProjectionMatrix.M13 / (double)nngProjectionMatrix.M11);
            nngClipPlane.Top.mul = num1 * (nngClip3d.y0 / -nngScreen.cy);
            nngClipPlane.Top.ofs = num2;
            nngClipPlane.Bottom.mul = (float)(-num1 * (nngClip3d.y1 / (double)nngScreen.cy));
            nngClipPlane.Bottom.ofs = num2;
            float num3 = (float)(2.0 / nngProjectionMatrix.M00 / 2.0);
            float num4 = (float)-(nngProjectionMatrix.M03 / (double)nngProjectionMatrix.M00);
            nngClipPlane.Right.mul = num3 * (nngClip3d.x0 / -nngScreen.cx);
            nngClipPlane.Right.ofs = num4;
            nngClipPlane.Left.mul = (float)(-num3 * (nngClip3d.x1 / (double)nngScreen.cx));
            nngClipPlane.Left.ofs = num4;
        }
    }

    public void nnSetClipScreenCoordinates(NNS_VECTOR2D pos)
    {
        mppAssertNotImpl();
    }

    public void nnSetClipZ(float znear, float zfar)
    {
        mppAssertNotImpl();
    }

    private int nnGetNodeIndex(NNS_NODENAMELIST pNodeNameList, string NodeName)
    {
        mppAssertNotImpl();
        return 0;
    }

    private string nnGetNodeName(NNS_NODENAMELIST pNodeNameList, int NodeIndex)
    {
        mppAssertNotImpl();
        return null;
    }


    private void nnSetNormalLength(float len)
    {
        nngNormalLength = len;
    }

    private void nnSetNormalColor(float r, float g, float b, float a)
    {
        nngNormalColor.r = r;
        nngNormalColor.g = g;
        nngNormalColor.b = b;
        nngNormalColor.a = a;
    }

    private void nnSetWireColor(float r, float g, float b, float a)
    {
        nngWireColor.r = r;
        nngWireColor.g = g;
        nngWireColor.b = b;
        nngWireColor.a = a;
    }

    private static void nnPutWireColor()
    {
        OpenGL.glColor4fv((OpenGL.glArray4f)nngWireColor);
    }

    private static void nnDrawObjectNormal(
        NNS_VTXLISTPTR vlistptr,
        NNS_PRIMLISTPTR plistptr,
        NNS_MATRIX[] mtxpal,
        uint flag)
    {
        mppAssertNotImpl();
    }

    public static uint NNM_CHUNK_ID(char a, char b, char c, char d)
    {
        return (uint)((d & byte.MaxValue) << 24 | (c & byte.MaxValue) << 16 | (b & byte.MaxValue) << 8 |
                       a & byte.MaxValue);
    }

    private static void nnInitPreviousMaterialValueGL()
    {
        nnmaterialcore.nngPreMatFlag = uint.MaxValue;
        nnmaterialcore.nngpPreMatColor = null;
        nnmaterialcore.nngpPreMatLogic = null;
    }

    private static void nnPutMaterialFlagGL(NNS_DRAWCALLBACK_VAL val, uint fMatFlag)
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
        nnPutFogSwitchGL(nngFogSwitch && 0 == ((int)fMatFlag & 4));
        OpenGL.glDepthMask(((int)fMatFlag & 256) == 0 ? (byte)1 : (byte)0);
        if (((int)val.DrawFlag & int.MinValue) != 0)
            OpenGL.glColorMask(0, 0, 0, 0);
        else
            OpenGL.glColorMask(((int)fMatFlag & 512) == 0 ? (byte)1 : (byte)0,
                ((int)fMatFlag & 1024) == 0 ? (byte)1 : (byte)0, ((int)fMatFlag & 2048) == 0 ? (byte)1 : (byte)0,
                ((int)fMatFlag & 4096) == 0 ? (byte)1 : (byte)0);
        nnmaterialcore.nngPreMatFlag = fMatFlag;
    }

    private static void nnPutMaterialColorGL(
        uint face,
        NNS_DRAWCALLBACK_VAL val,
        NNS_MATERIAL_STDSHADER_COLOR pColor)
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
                switch (nngMatCtrlAmbient.mode)
                {
                    case 1:
                        OpenGL.glArray4f glArray4f1;
                        glArray4f1.f0 = nngMatCtrlAmbient.col.r;
                        glArray4f1.f1 = nngMatCtrlAmbient.col.g;
                        glArray4f1.f2 = nngMatCtrlAmbient.col.b;
                        glArray4f1.f3 = 1f;
                        OpenGL.glMaterialfv(face, 4608U, glArray4f1);
                        break;
                    case 2:
                        OpenGL.glArray4f glArray4f2;
                        glArray4f2.f0 = pColor.Ambient.r + nngMatCtrlAmbient.col.r;
                        glArray4f2.f1 = pColor.Ambient.g + nngMatCtrlAmbient.col.g;
                        glArray4f2.f2 = pColor.Ambient.b + nngMatCtrlAmbient.col.b;
                        glArray4f2.f3 = pColor.Ambient.a;
                        OpenGL.glMaterialfv(face, 4608U, glArray4f2);
                        break;
                    case 3:
                        OpenGL.glArray4f glArray4f3;
                        glArray4f3.f0 = pColor.Ambient.r * nngMatCtrlAmbient.col.r;
                        glArray4f3.f1 = pColor.Ambient.g * nngMatCtrlAmbient.col.g;
                        glArray4f3.f2 = pColor.Ambient.b * nngMatCtrlAmbient.col.b;
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
                    switch (nngMatCtrlDiffuse.mode)
                    {
                        case 1:
                            v.f0 = nngMatCtrlDiffuse.col.r;
                            v.f1 = nngMatCtrlDiffuse.col.g;
                            v.f2 = nngMatCtrlDiffuse.col.b;
                            break;
                        case 2:
                            v.f0 = pColor.Diffuse.r + nngMatCtrlDiffuse.col.r;
                            v.f1 = pColor.Diffuse.g + nngMatCtrlDiffuse.col.g;
                            v.f2 = pColor.Diffuse.b + nngMatCtrlDiffuse.col.b;
                            break;
                        case 3:
                            v.f0 = pColor.Diffuse.r * nngMatCtrlDiffuse.col.r;
                            v.f1 = pColor.Diffuse.g * nngMatCtrlDiffuse.col.g;
                            v.f2 = pColor.Diffuse.b * nngMatCtrlDiffuse.col.b;
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
                    switch (nngMatCtrlAlpha.mode)
                    {
                        case 1:
                            v.f3 = nngMatCtrlAlpha.alpha;
                            break;
                        case 2:
                            v.f3 = pColor.Diffuse.a + nngMatCtrlAlpha.alpha;
                            break;
                        case 3:
                            v.f3 = pColor.Diffuse.a * nngMatCtrlAlpha.alpha;
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
                OpenGL.glMaterialfv(face, 4610U, (OpenGL.glArray4f)nngColorBlack);
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
                    switch (nngMatCtrlSpecular.mode)
                    {
                        case 1:
                            glArray4f4.f0 = nngMatCtrlSpecular.col.r;
                            glArray4f4.f1 = nngMatCtrlSpecular.col.g;
                            glArray4f4.f2 = nngMatCtrlSpecular.col.b;
                            glArray4f4.f3 = 1f;
                            break;
                        case 2:
                            glArray4f4.f0 += nngMatCtrlSpecular.col.r;
                            glArray4f4.f1 += nngMatCtrlSpecular.col.g;
                            glArray4f4.f2 += nngMatCtrlSpecular.col.b;
                            break;
                        case 3:
                            glArray4f4.f0 *= nngMatCtrlSpecular.col.r;
                            glArray4f4.f1 *= nngMatCtrlSpecular.col.g;
                            glArray4f4.f2 *= nngMatCtrlSpecular.col.b;
                            break;
                    }
                }

                OpenGL.glMaterialfv(face, 4610U, glArray4f4);
            }
        }

        float num = pColor.Shininess <= (double)nngGLExtensions.max_shininess
            ? pColor.Shininess
            : nngGLExtensions.max_shininess;
        OpenGL.glMaterialf(face, 5633U, num);
        OpenGL.glMaterialfv(face, 5632U, (OpenGL.glArray4f)pColor.Emission);
        nnmaterialcore.nngpPreMatColor = pColor;
    }

    private static void nnPutMaterialLogicGL(
        NNS_DRAWCALLBACK_VAL val,
        NNS_MATERIAL_LOGIC pLogic)
    {
        uint fFlag = pLogic.fFlag;
        if (((int)val.DrawFlag & 33554432) != 0)
        {
            OpenGL.glEnable(3042U);
            switch (nngMatCtrlBlendMode.blendmode)
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
                OpenGL.glBlendFunc(pLogic.SrcFactorRGB, pLogic.DstFactorRGB);
            else
                OpenGL.glBlendFunc(pLogic.SrcFactorRGB, pLogic.DstFactorRGB);
            OpenGL.glBlendEquation(pLogic.BlendOp);
        }
        else
            OpenGL.glDisable(3042U);

        if (((int)fFlag & 4) != 0)
        {
            OpenGL.glEnable(3058U);
            OpenGL.glLogicOp(pLogic.LogicOp);
        }
        else
            OpenGL.glDisable(3058U);

        if (((int)val.DrawFlag & 1) == 0 && ((int)fFlag & 8) != 0)
        {
            OpenGL.glEnable(3008U);
            OpenGL.glAlphaFunc(pLogic.AlphaFunc, pLogic.AlphaRef);
        }
        else
            OpenGL.glDisable(3008U);

        if (((int)fFlag & 16) != 0)
        {
            OpenGL.glEnable(2929U);
            OpenGL.glDepthFunc(pLogic.DepthFunc);
        }
        else
            OpenGL.glDisable(2929U);

        nnmaterialcore.nngpPreMatLogic = pLogic;
    }

    private static void nnPutMaterialLogicGLES11(
        NNS_DRAWCALLBACK_VAL val,
        NNS_MATERIAL_GLES11_LOGIC pLogic)
    {
        uint fFlag = pLogic.fFlag;
        if (((int)val.DrawFlag & 33554432) != 0)
        {
            OpenGL.glEnable(3042U);
            switch (nngMatCtrlBlendMode.blendmode)
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
            OpenGL.glBlendFunc(pLogic.SrcFactor, pLogic.DstFactor);
            OpenGL.glBlendEquation(pLogic.BlendOp);
        }
        else
            OpenGL.glDisable(3042U);

        if (((int)fFlag & 4) != 0)
        {
            OpenGL.glEnable(3058U);
            OpenGL.glLogicOp(pLogic.LogicOp);
        }
        else
            OpenGL.glDisable(3058U);

        if (((int)val.DrawFlag & 1) == 0 && ((int)fFlag & 8) != 0)
        {
            OpenGL.glEnable(3008U);
            OpenGL.glAlphaFunc(pLogic.AlphaFunc, pLogic.AlphaRef);
        }
        else
            OpenGL.glDisable(3008U);

        if (((int)fFlag & 16) != 0)
        {
            OpenGL.glEnable(2929U);
            OpenGL.glDepthFunc(pLogic.DepthFunc);
        }
        else
            OpenGL.glDisable(2929U);

        nnmaterialcore.nngpPreMatLogic = pLogic;
    }

    private void nnPutMaterialTextureShadowMap(
        int slot,
        NNS_DRAWCALLBACK_VAL val,
        NNE_SHADOWMAP idx)
    {
    }

    private uint nnGetTextureMask(uint flag)
    {
        mppAssertNotImpl();
        return 0;
    }

    private void nnPutMaterialTextureOneGL(
        int slot,
        NNS_DRAWCALLBACK_VAL val,
        NNS_MATERIAL_TEXMAP_DESC pTex)
    {
        mppAssertNotImpl();
    }

    private static void nnPutMaterialTextureOneGLES11(
        int slot,
        NNS_DRAWCALLBACK_VAL val,
        ref NNS_MATERIAL_GLES11_TEXMAP_DESC pTex)
    {
        uint fType = pTex.fType;
        uint num = fType & 3841U;
        if (pTex.pTexInfo != null)
            nnSetTexInfo(slot, (NNS_TEXINFO)pTex.pTexInfo);
        else
            nnSetTextureNum(slot, pTex.iTexIdx);
        int src;
        switch (num)
        {
            case 1:
                if (((int)val.DrawFlag & 16777216) != 0)
                {
                    switch (nngMatCtrlEnvTexMtx.texcoordsrc)
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

        nnSetTexCoordSrc(slot, src);
        if (src != 3)
        {
            OpenGL.glMatrixMode(5890U);
            OpenGL.glLoadIdentity();
            OpenGL.glTranslatef(0.0f, 1f, 0.0f);
            OpenGL.glScalef(1f, -1f, 1f);
            if (((int)val.DrawFlag & 268435456) != 0)
            {
                switch (nngMatCtrlTexOffset[slot].mode)
                {
                    case 1:
                        OpenGL.glTranslatef(nngMatCtrlTexOffset[slot].offset.u, nngMatCtrlTexOffset[slot].offset.v,
                            0.0f);
                        break;
                    case 2:
                        OpenGL.glTranslatef(pTex.Offset.u + nngMatCtrlTexOffset[slot].offset.u,
                            pTex.Offset.v + nngMatCtrlTexOffset[slot].offset.v, 0.0f);
                        break;
                    case 3:
                        OpenGL.glTranslatef(pTex.Offset.u * nngMatCtrlTexOffset[slot].offset.u,
                            pTex.Offset.v * nngMatCtrlTexOffset[slot].offset.v, 0.0f);
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
            NNS_TEXTURE_GLES11_COMBINE pCombine = pTex.pCombine;
            OpenGL.glTexEnvi(8960U, 34161U, pCombine.CombineRGB);
            OpenGL.glTexEnvi(8960U, 34176U, pCombine.Source0RGB);
            OpenGL.glTexEnvi(8960U, 34192U, pCombine.Operand0RGB);
            OpenGL.glTexEnvi(8960U, 34177U, pCombine.Source1RGB);
            OpenGL.glTexEnvi(8960U, 34193U, pCombine.Operand1RGB);
            OpenGL.glTexEnvi(8960U, 34178U, pCombine.Source2RGB);
            OpenGL.glTexEnvi(8960U, 34194U, pCombine.Operand2RGB);
            OpenGL.glTexEnvi(8960U, 34162U, pCombine.CombineAlpha);
            OpenGL.glTexEnvi(8960U, 34184U, pCombine.Source0Alpha);
            OpenGL.glTexEnvi(8960U, 34200U, pCombine.Operand0Alpha);
            OpenGL.glTexEnvi(8960U, 34185U, pCombine.Source1Alpha);
            OpenGL.glTexEnvi(8960U, 34201U, pCombine.Operand1Alpha);
            OpenGL.glTexEnvi(8960U, 34186U, pCombine.Source2Alpha);
            OpenGL.glTexEnvi(8960U, 34202U, pCombine.Operand2Alpha);
            OpenGL.glTexEnvfv(8960U, 8705U, (OpenGL.glArray4f)pCombine.EnvColor);
        }

        OpenGL.glTexParameteri(3553U, 10242U, pTex.WrapS);
        OpenGL.glTexParameteri(3553U, 10243U, pTex.WrapT);
        if (pTex.pFilterMode == null)
            return;
        NNS_TEXTURE_FILTERMODE pFilterMode = pTex.pFilterMode;
        OpenGL.glTexParameteri(3553U, 10240U, pFilterMode.MagFilter);
        OpenGL.glTexParameteri(3553U, 10241U, pFilterMode.MinFilter);
    }

    private static void nnPutMaterialTexturesGL(
        NNS_DRAWCALLBACK_VAL val,
        NNS_MATERIAL_TEXMAP_DESC[] texdesc,
        int num)
    {
        mppAssertNotImpl();
    }

    private static void nnPutMaterialTexturesGLES11(
        NNS_DRAWCALLBACK_VAL val,
        NNS_MATERIAL_GLES11_TEXMAP_DESC[] texdesc,
        int num)
    {
        int maxTextureUnits = nngGLExtensions.max_texture_units;
        int slot = 0;
        for (int index = 0; index < num; ++index)
        {
            NNS_MATERIAL_GLES11_TEXMAP_DESC pTex = texdesc[index];
            nnPutMaterialTextureOneGLES11(slot, val, ref pTex);
            ++slot;
            if (slot >= maxTextureUnits)
                return;
        }

        for (; slot < maxTextureUnits; ++slot)
        {
            nnSetTextureNum(slot, -1);
            nnSetTexCoordSrc(slot, 0);
        }
    }

    private void nnPutMaterialStdShaderTextureOneGL(
        int slot,
        NNS_DRAWCALLBACK_VAL val,
        NNS_MATERIAL_STDSHADER_TEXMAP_DESC pTex)
    {
        mppAssertNotImpl();
    }

    private void nnPutMaterialStdShaderTexturesGL(
        NNS_DRAWCALLBACK_VAL val,
        NNS_MATERIAL_STDSHADER_TEXMAP_DESC texdesc,
        int num)
    {
        mppAssertNotImpl();
    }

    private static int nnPutMaterialCore(NNS_DRAWCALLBACK_VAL val)
    {
        NNS_MATERIALPTR pMaterial1 = val.pMaterial;
        if (((int)val.DrawFlag & 29440) != 0)
        {
            nnPutDisableTexturesGL();
            if (((int)val.DrawFlag & 768) != 0)
            {
                nnPutFixedMaterialGL();
                return 1;
            }

            if (((int)val.DrawFlag & 28672) != 0)
            {
                nnPutFixedMaterialGL();
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
                        if (((int)((NNS_MATERIAL_DESC)pMaterial1.pMaterial).fFlag & 1) != 0)
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
                        nnPutWireColor();
                        break;
                    case 8192:
                        if (((int)pMaterial1.fType & 8) != 0)
                        {
                            nnPutColorNTexture(((NNS_MATERIAL_GLES11_DESC)pMaterial1.pMaterial).nTex);
                            break;
                        }

                        nnPutColorNTexture(0);
                        break;
                    case 16384:
                        nnPutColorMeshset(val.iMeshset, val.iSubobject);
                        break;
                    case 20480:
                        nnPutColorMaterial(val.iMaterial);
                        break;
                    case 24576:
                        nnPutColorNWeight(val.pVtxListPtr);
                        break;
                }

                return 1;
            }
        }

        if (val.iPrevMaterial == -1 || val.bModified != 0)
            nnInitPreviousMaterialValueGL();
        else if (val.iMaterial == val.iPrevMaterial)
            return 1;
        if (((int)pMaterial1.fType & 8) != 0)
        {
            NNS_MATERIAL_GLES11_DESC pMaterial2 = (NNS_MATERIAL_GLES11_DESC)pMaterial1.pMaterial;
            uint fFlag = pMaterial2.fFlag;
            if ((int)fFlag != (int)nnmaterialcore.nngPreMatFlag)
                nnPutMaterialFlagGL(val, fFlag);
            if (pMaterial2.pColor != nnmaterialcore.nngpPreMatColor)
                nnPutMaterialColorGL(1032U, val, pMaterial2.pColor);
            if (pMaterial2.pLogic != nnmaterialcore.nngpPreMatLogic)
                nnPutMaterialLogicGLES11(val, pMaterial2.pLogic);
            if (((int)val.DrawFlag & 2048) == 0)
                nnPutMaterialTexturesGLES11(val, pMaterial2.pTexDesc, pMaterial2.nTex);
            else
                nnPutDisableTexturesGL();
        }
        else if (((int)pMaterial1.fType & 1) != 0)
        {
            NNS_MATERIAL_DESC pMaterial2 = (NNS_MATERIAL_DESC)pMaterial1.pMaterial;
            uint fFlag = pMaterial2.fFlag;
            if ((int)fFlag != (int)nnmaterialcore.nngPreMatFlag)
                nnPutMaterialFlagGL(val, fFlag);
            if (pMaterial2.pColor != nnmaterialcore.nngpPreMatColor)
                nnPutMaterialColorGL(1032U, val, (NNS_MATERIAL_STDSHADER_COLOR)pMaterial2.pColor);
            if (pMaterial2.pLogic != nnmaterialcore.nngpPreMatLogic)
                nnPutMaterialLogicGL(val, pMaterial2.pLogic);
            if (((int)val.DrawFlag & 2048) == 0)
                nnPutMaterialTexturesGL(val, pMaterial2.pTexDesc, pMaterial2.nTex);
            else
                nnPutDisableTexturesGL();
        }

        return 1;
    }

    private void nnInitCircumsphere()
    {
        ArrayPointer<NNS_VECTOR> _pointer1 = new ArrayPointer<NNS_VECTOR>(this.nngCircumPoint);
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float s1;
        float c1;
        float s2;
        float c2;
        for (int index1 = 0; index1 < 2; ++index1)
        {
            nnSinCos(NNM_DEGtoA32((int)(index1 * 360.0 / 4.0)), out s1, out c1);
            nnSinCos(0, out s2, out c2);
            for (int index2 = 0; index2 < 20; ++index2)
            {
                (~_pointer1).x = c2 * c1 + num1;
                (~_pointer1).y = s2 + num2;
                (~_pointer1).z = c2 * s1 + num3;
                ArrayPointer<NNS_VECTOR> _pointer2 = ++(_pointer1);
                nnSinCos(NNM_DEGtoA32((int)((index2 + 1) * 360.0 / 20.0)), out s2, out c2);
                (~_pointer2).x = c2 * c1 + num1;
                (~_pointer2).y = s2 + num2;
                (~_pointer2).z = c2 * s1 + num3;
                _pointer1 = ++(_pointer2);
            }
        }

        for (int index1 = 0; index1 < 1; ++index1)
        {
            nnSinCos(NNM_DEGtoA32(180), out s1, out c1);
            nnSinCos(0, out s2, out c2);
            for (int index2 = 0; index2 < 20; ++index2)
            {
                (~_pointer1).x = c1 * c2 + num1;
                (~_pointer1).y = s1 + num2;
                (~_pointer1).z = c1 * s2 + num3;
                ArrayPointer<NNS_VECTOR> _pointer2 = ++(_pointer1);
                nnSinCos(NNM_DEGtoA32((int)((index2 + 1) * 360.0 / 20.0)), out s2, out c2);
                (~_pointer2).x = c1 * c2 + num1;
                (~_pointer2).y = s1 + num2;
                (~_pointer2).z = c1 * s2 + num3;
                _pointer1 = ++(_pointer2);
            }
        }
    }

    private NNE_CIRCUM_COL nnEstCircumColNum(uint clipstat)
    {
        mppAssertNotImpl();
        return NNE_CIRCUM_COL.NNE_CIRCUM_COL_NONE;
    }

    private void nnSetCircumsphereColor(
        uint dstflag,
        NNE_CIRCUM_COL colnum,
        ref NNS_RGBA col)
    {
        mppAssertNotImpl();
    }

    private void nnDrawCircumsphereCore(
        NNS_VECTOR center,
        float radius,
        NNS_MATRIX mtx,
        ref NNS_RGBA col,
        int trans)
    {
        GlobalPool<NNS_MATRIX>.Alloc();
        GlobalPool<NNS_VECTOR>.Alloc();
        mppAssertNotImpl();
    }

    private void nnDrawClipBoxCore(
        NNS_VECTOR center,
        float sx,
        float sy,
        float sz,
        NNS_MATRIX mtx,
        ref NNS_RGBA col,
        int trans)
    {
        mppAssertNotImpl();
    }

    private void nnDrawCircumsphereNode(int nodeIdx, uint hideflag)
    {
        mppAssertNotImpl();
    }

    private void nnDrawCircumsphere(
        NNS_OBJECT obj,
        NNS_MATRIX basemtx,
        NNS_MATRIXSTACK mstk,
        uint flag)
    {
        mppAssertNotImpl();
    }

    private void nnDrawCircumsphereMotionNode(int nodeIdx, uint hideflag)
    {
        mppAssertNotImpl();
    }

    private void nnDrawCircumsphereMotion(
        NNS_OBJECT obj,
        NNS_MOTION mot,
        float frame,
        NNS_MATRIX basemtx,
        NNS_MATRIXSTACK mstk,
        uint flag)
    {
        mppAssertNotImpl();
    }

    private void nnDrawCircumsphereTRSListNode(int nodeIdx, uint hideflag)
    {
        mppAssertNotImpl();
    }

    private void nnDrawCircumsphereTRSList(
        NNS_OBJECT obj,
        NNS_TRS trslist,
        NNS_MATRIX basemtx,
        NNS_MATRIXSTACK mstk,
        uint flag)
    {
        mppAssertNotImpl();
    }

    private void nnDrawClipSphere(NNS_VECTOR center, float radius, NNS_MATRIX mtx)
    {
        mppAssertNotImpl();
    }

    private void nnDrawClipBox(
        NNS_VECTOR center,
        float sx,
        float sy,
        float sz,
        NNS_MATRIX mtx)
    {
        mppAssertNotImpl();
    }

    private void nnDrawClipBound(
        NNS_OBJECT obj,
        NNS_MATRIX basemtx,
        NNS_MATRIXSTACK mstk,
        uint flag)
    {
        this.nnDrawCircumsphere(obj, basemtx, mstk, flag);
    }

    private void nnDrawClipBoundMotion(
        NNS_OBJECT obj,
        NNS_MOTION mot,
        float frame,
        NNS_MATRIX basemtx,
        NNS_MATRIXSTACK mstk,
        uint flag)
    {
        this.nnDrawCircumsphereMotion(obj, mot, frame, basemtx, mstk, flag);
    }

    private void nnDrawClipBoundTRSList(
        NNS_OBJECT obj,
        NNS_TRS trslist,
        NNS_MATRIX basemtx,
        NNS_MATRIXSTACK mstk,
        uint flag)
    {
        this.nnDrawCircumsphereTRSList(obj, trslist, basemtx, mstk, flag);
    }

    private void nnSetClipBoundColor(
        uint dstflag,
        NNE_CIRCUM_COL colnum,
        ref NNS_RGBA col)
    {
        this.nnSetCircumsphereColor(dstflag, colnum, ref col);
    }

    private void nnCalcTRS(NNS_TRS trs, NNS_OBJECT obj, int nodeidx)
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
                    nnMakeRotateXZYQuaternion(out trs.Rotation, x, ry, rz);
                    break;
                case 1024:
                    nnMakeRotateZXYQuaternion(out trs.Rotation, x, ry, rz);
                    break;
                default:
                    nnMakeRotateXYZQuaternion(out trs.Rotation, x, ry, rz);
                    break;
            }
        }

        trs.Scaling.x = obj.pNodeList[nodeidx].Scaling.x;
        trs.Scaling.y = obj.pNodeList[nodeidx].Scaling.y;
        trs.Scaling.z = obj.pNodeList[nodeidx].Scaling.z;
    }

    public static void nnCalcTRSList(NNS_TRS[] trslist, int offset, NNS_OBJECT obj)
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
                        nnMakeRotateXZYQuaternion(out trslist[index2].Rotation, x, ry, rz);
                        break;
                    case 1024:
                        nnMakeRotateZXYQuaternion(out trslist[index2].Rotation, x, ry, rz);
                        break;
                    default:
                        nnMakeRotateXYZQuaternion(out trslist[index2].Rotation, x, ry, rz);
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
        NNS_VECTOR tv,
        NNS_VECTOR sv,
        ref NNS_QUATERNION rq,
        ref NNS_QUATERNION invrq,
        bool need_invrq,
        NNS_NODE pNode,
        int NodeIdx,
        NNS_MOTION pMot,
        int SubMotIdx,
        float frame)
    {
        NNS_ROTATE_A32 rv = new NNS_ROTATE_A32();
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
                    nnMakeRotateXZYQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
                case 1024:
                    nnMakeRotateZXYQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
                default:
                    nnMakeRotateXYZQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
            }

            if (need_invrq)
                nnInvertQuaternion(ref invrq, ref rq);
        }

        tflag = 0;
        rflag = 0;
        sflag = 0;
        for (int SubMotIdx1 = SubMotIdx; SubMotIdx1 < pMot.nSubmotion; ++SubMotIdx1)
        {
            NNS_SUBMOTION submot = pMot.pSubmotion[SubMotIdx1];
            if (NodeIdx < submot.Id)
            {
                SubMotIdx = SubMotIdx1;
                break;
            }

            float dstframe;
            if (submot.Id == NodeIdx && submot.fType != 0U &&
                (submot.StartFrame <= (double)frame && frame <= (double)submot.EndFrame) &&
                nnCalcMotionFrame(out dstframe, submot.fIPType, submot.StartKeyFrame, submot.EndKeyFrame, frame) != 0)
            {
                if (((int)submot.fType & 30720) != 0)
                    rflag |= nnCalcMotionRotate(submot, dstframe, ref rv, rq, rtype);
                else if (((int)submot.fType & 1792) != 0)
                    tflag |= nnCalcMotionTranslate(submot, dstframe, tv);
                else if (((int)submot.fType & 229376) != 0)
                    sflag |= nnCalcMotionScale(submot, dstframe, sv);
                else if (((int)submot.fType & 786432) != 0)
                    nnCallbackMotionUserData(nncalctrsmotion.nnsObj, pMot, SubMotIdx1, NodeIdx, dstframe, frame);
            }
        }

        if (rflag == 1)
        {
            switch (rtype)
            {
                case 256:
                    nnMakeRotateXZYQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
                case 1024:
                    nnMakeRotateZXYQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
                default:
                    nnMakeRotateXYZQuaternion(out rq, rv.x, rv.y, rv.z);
                    break;
            }
        }

        return SubMotIdx;
    }

    private void nnCalcTRSMotion(
        NNS_TRS trs,
        NNS_OBJECT obj,
        int nodeidx,
        NNS_MOTION mot,
        float frame)
    {
        NNS_QUATERNION rq = new NNS_QUATERNION();
        if (((int)mot.fType & 1) == 0)
            return;
        nncalctrsmotion.nnsObj = obj;
        if (nnCalcMotionFrame(out frame, mot.fType, mot.StartFrame, mot.EndFrame, frame) == 0)
        {
            this.nnCalcTRS(trs, obj, nodeidx);
        }
        else
        {
            NNS_NODE pNode = obj.pNodeList[nodeidx];
            NNS_VECTOR nnCalcTrsMotionTv = nnCalcTRSMotion_tv;
            nnCalcTrsMotionTv.Clear();
            NNS_VECTOR nnCalcTrsMotionSv = nnCalcTRSMotion_sv;
            nnCalcTrsMotionSv.Clear();
            NNS_QUATERNION invrq = new NNS_QUATERNION();
            nnCalcNodeMotionTRSCore(out int _, out int _, out int _, nnCalcTrsMotionTv, nnCalcTrsMotionSv, ref rq,
                ref invrq, false, pNode, nodeidx, mot, 0, frame);
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
        NNS_TRS[] trslist,
        int offset,
        NNS_OBJECT obj,
        NNS_MOTION mot,
        float frame)
    {
        NNS_QUATERNION rq = new NNS_QUATERNION();
        if (((int)mot.fType & 1) == 0)
            return;
        if (nnCalcMotionFrame(out frame, mot.fType, mot.StartFrame, mot.EndFrame, frame) == 0)
        {
            nnCalcTRSList(trslist, offset, obj);
        }
        else
        {
            nncalctrsmotion.nnsObj = obj;
            NNS_QUATERNION invrq = new NNS_QUATERNION();
            NNS_VECTOR nnCalcTrsMotionTv = nnCalcTRSMotion_tv;
            nnCalcTrsMotionTv.Clear();
            NNS_VECTOR nnCalcTrsMotionSv = nnCalcTRSMotion_sv;
            nnCalcTrsMotionSv.Clear();
            int SubMotIdx = 0;
            for (int NodeIdx = 0; NodeIdx < obj.nNode; ++NodeIdx)
            {
                NNS_NODE pNode = obj.pNodeList[NodeIdx];
                SubMotIdx = nnCalcNodeMotionTRSCore(out int _, out int _, out int _, nnCalcTrsMotionTv,
                    nnCalcTrsMotionSv, ref rq, ref invrq, false, pNode, NodeIdx, mot, SubMotIdx, frame);
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
        NNS_NODE nnsNode;
        do
        {
            nnsNode = nncalctrsmotion.nnsNodeList[nodeIdx];
            NNS_TRS nnsTrs = nncalctrsmotion.nnsTrsList[nodeIdx];
            if (((int)nnsNode.fType & 134217728) != 0)
            {
                if (((int)nnsNode.fType & 100663296) != 0)
                {
                    nnPushMatrix(nncalctrsmotion.nnsMstk, null);
                    NNS_MATRIX currentMatrix = nnGetCurrentMatrix(nncalctrsmotion.nnsMstk);
                    nnTranslateMatrix(currentMatrix, currentMatrix, nnsTrs.Translation.x, nnsTrs.Translation.y,
                        nnsTrs.Translation.z);
                    if (((int)nnsNode.fType & 4096) != 0)
                        nnCopyMatrix33(currentMatrix, nncalctrsmotion.nnsBaseMtx);
                    else if (((int)nnsNode.fType & 1835008) != 0)
                    {
                        if (((int)nnsNode.fType & 262144) != 0)
                            nnNormalizeColumn0(currentMatrix);
                        if (((int)nnsNode.fType & 524288) != 0)
                            nnNormalizeColumn1(currentMatrix);
                        if (((int)nnsNode.fType & 1048576) != 0)
                            nnNormalizeColumn2(currentMatrix);
                    }

                    nnQuaternionMatrix(currentMatrix, currentMatrix, ref nnsTrs.Rotation);
                    nnScaleMatrix(currentMatrix, currentMatrix, nnsTrs.Scaling.x, nnsTrs.Scaling.y, nnsTrs.Scaling.z);
                    if (((int)nnsNode.fType & 33554432) != 0)
                        nnCalcMatrixPaletteTRSListNode1BoneXSIIK(nodeIdx);
                    else if (((int)nnsNode.fType & 67108864) != 0)
                        nnCalcMatrixPaletteTRSListNode2BoneXSIIK(nodeIdx);
                    nnPopMatrix(nncalctrsmotion.nnsMstk);
                    nodeIdx = nnsNode.iSibling;
                    goto label_40;
                }
            }
            else
            {
                if (((int)nnsNode.fType & 16384) != 0)
                {
                    nnPushMatrix(nncalctrsmotion.nnsMstk, null);
                    nnCalcMatrixPaletteTRSListNode1BoneSIIK(nodeIdx);
                    nnPopMatrix(nncalctrsmotion.nnsMstk);
                    break;
                }

                if (((int)nnsNode.fType & 32768) != 0)
                {
                    nnPushMatrix(nncalctrsmotion.nnsMstk, null);
                    nnCalcMatrixPaletteTRSListNode2BoneSIIK(nodeIdx);
                    nnPopMatrix(nncalctrsmotion.nnsMstk);
                    break;
                }
            }

            nnPushMatrix(nncalctrsmotion.nnsMstk, null);
            NNS_MATRIX currentMatrix1 = nnGetCurrentMatrix(nncalctrsmotion.nnsMstk);
            nnTranslateMatrix(currentMatrix1, currentMatrix1, nnsTrs.Translation.x, nnsTrs.Translation.y,
                nnsTrs.Translation.z);
            if (((int)nnsNode.fType & 4096) != 0)
                nnCopyMatrix33(currentMatrix1, nncalctrsmotion.nnsBaseMtx);
            else if (((int)nnsNode.fType & 1835008) != 0)
            {
                if (((int)nnsNode.fType & 262144) != 0)
                    nnNormalizeColumn0(currentMatrix1);
                if (((int)nnsNode.fType & 524288) != 0)
                    nnNormalizeColumn1(currentMatrix1);
                if (((int)nnsNode.fType & 1048576) != 0)
                    nnNormalizeColumn2(currentMatrix1);
            }

            nnQuaternionMatrix(currentMatrix1, currentMatrix1, ref nnsTrs.Rotation);
            nnScaleMatrix(currentMatrix1, currentMatrix1, nnsTrs.Scaling.x, nnsTrs.Scaling.y, nnsTrs.Scaling.z);
            if (nnsNode.iMatrix != -1)
            {
                if (((int)nnsNode.fType & 8) != 0)
                    nnCopyMatrix(nncalctrsmotion.nnsMtxPal[nnsNode.iMatrix], currentMatrix1);
                else
                    nnMultiplyMatrix(nncalctrsmotion.nnsMtxPal[nnsNode.iMatrix], currentMatrix1, nnsNode.InvInitMtx);
            }

            if (nncalctrsmotion.nnsNodeStatList != null)
            {
                if (nodeIdx == 0 && nncalctrsmotion.nnsNSFlag != 0U)
                    nncalctrsmotion.nnsRootScale = nnEstimateMatrixScaling(currentMatrix1);
                nnCalcClipSetNodeStatus(nncalctrsmotion.nnsNodeStatList, nncalctrsmotion.nnsNodeList, nodeIdx,
                    currentMatrix1, nncalctrsmotion.nnsRootScale, nncalctrsmotion.nnsNSFlag);
            }

            if (nnsNode.iChild != -1)
                nnCalcMatrixPaletteTRSListNode(nnsNode.iChild);
            nnPopMatrix(nncalctrsmotion.nnsMstk);
            nodeIdx = nnsNode.iSibling;
        label_40:;
        } while (nnsNode.iSibling != -1);
    }

    private static void nnCalcMatrixPaletteTRSList(
        NNS_MATRIX[] mtxpal,
        uint[] nodestatlist,
        NNS_OBJECT obj,
        NNS_TRS[] trslist,
        ref SNNS_MATRIX basemtx,
        NNS_MATRIXSTACK mstk,
        uint flag)
    {
        nncalctrsmotion.nnsBaseMtx.Assign(ref basemtx);
        nnSetCurrentMatrix(mstk, nncalctrsmotion.nnsBaseMtx);
        nncalctrsmotion.nnsMtxPal = mtxpal;
        nncalctrsmotion.nnsNodeStatList = nodestatlist;
        nncalctrsmotion.nnsNSFlag = flag;
        nncalctrsmotion.nnsTrsList = trslist;
        nncalctrsmotion.nnsNodeList = obj.pNodeList;
        nncalctrsmotion.nnsMstk = mstk;
        nnCalcMatrixPaletteTRSListNode(0);
    }

    private static void nnCalcMatrixPaletteTRSList(
        NNS_MATRIX[] mtxpal,
        uint[] nodestatlist,
        NNS_OBJECT obj,
        NNS_TRS[] trslist,
        NNS_MATRIX basemtx,
        NNS_MATRIXSTACK mstk,
        uint flag)
    {
        if (basemtx != null)
            nncalctrsmotion.nnsBaseMtx.Assign(basemtx);
        else
            nncalctrsmotion.nnsBaseMtx.Assign(nngUnitMatrix);
        nnSetCurrentMatrix(mstk, nncalctrsmotion.nnsBaseMtx);
        nncalctrsmotion.nnsMtxPal = mtxpal;
        nncalctrsmotion.nnsNodeStatList = nodestatlist;
        nncalctrsmotion.nnsNSFlag = flag;
        nncalctrsmotion.nnsTrsList = trslist;
        nncalctrsmotion.nnsNodeList = obj.pNodeList;
        nncalctrsmotion.nnsMstk = mstk;
        nnCalcMatrixPaletteTRSListNode(0);
    }

    public static void nnLinkMotion(
        ArrayPointer<NNS_TRS> dstpose,
        ArrayPointer<NNS_TRS> pose0,
        ArrayPointer<NNS_TRS> pose1,
        int nnode,
        float ratio)
    {
        for (int index = 0; index < nnode; ++index)
        {
            NNS_TRS nnsTrs1 = dstpose.array[dstpose.offset + index];
            NNS_TRS nnsTrs2 = pose0.array[pose0.offset + index];
            NNS_TRS nnsTrs3 = pose1.array[pose1.offset + index];
            nnsTrs1.Translation.x = nnsTrs2.Translation.x + (nnsTrs3.Translation.x - nnsTrs2.Translation.x) * ratio;
            nnsTrs1.Translation.y = nnsTrs2.Translation.y + (nnsTrs3.Translation.y - nnsTrs2.Translation.y) * ratio;
            nnsTrs1.Translation.z = nnsTrs2.Translation.z + (nnsTrs3.Translation.z - nnsTrs2.Translation.z) * ratio;
            nnsTrs1.Scaling.x = nnsTrs2.Scaling.x + (nnsTrs3.Scaling.x - nnsTrs2.Scaling.x) * ratio;
            nnsTrs1.Scaling.y = nnsTrs2.Scaling.y + (nnsTrs3.Scaling.y - nnsTrs2.Scaling.y) * ratio;
            nnsTrs1.Scaling.z = nnsTrs2.Scaling.z + (nnsTrs3.Scaling.z - nnsTrs2.Scaling.z) * ratio;
            nnSlerpQuaternion(out nnsTrs1.Rotation, ref nnsTrs2.Rotation, ref nnsTrs3.Rotation, ratio);
        }
    }

    private void nnBlendMotion(
        ArrayPointer<NNS_TRS> _dstpose,
        ArrayPointer<NNS_TRS> _srcpose,
        NNS_OBJECT obj,
        NNS_MOTION mot,
        float frame,
        NNE_MOTIONBLEND blendmode)
    {
        if (((int)mot.fType & 1) == 0)
            return;
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        ArrayPointer<NNS_TRS> arrayPointer1 = _dstpose.Clone();
        ArrayPointer<NNS_TRS> arrayPointer2 = _srcpose.Clone();
        if (nnCalcMotionFrame(out frame, mot.fType, mot.StartFrame, mot.EndFrame, frame) == 0)
        {
            if (~arrayPointer1 == ~arrayPointer2)
                return;
            for (int index = 0; index < obj.nNode; ++index)
                arrayPointer1[index].Assign(arrayPointer2);
        }
        else
        {
            int SubMotIdx = 0;
            ArrayPointer<NNS_NODE> pNodeList = obj.pNodeList;
            NNS_VECTOR nnCalcTrsMotionTv = nnCalcTRSMotion_tv;
            nnCalcTrsMotionTv.Clear();
            NNS_VECTOR nnCalcTrsMotionSv = nnCalcTRSMotion_sv;
            nnCalcTrsMotionSv.Clear();
            for (int NodeIdx = 0; NodeIdx < obj.nNode; ++NodeIdx)
            {
                int tflag;
                int rflag;
                int sflag;
                SubMotIdx = nnCalcNodeMotionTRSCore(out tflag, out rflag, out sflag, nnCalcTrsMotionTv,
                    nnCalcTrsMotionSv, ref nnsQuaternion1, ref nnsQuaternion2, true, pNodeList, NodeIdx, mot, SubMotIdx,
                    frame);
                switch (blendmode)
                {
                    case NNE_MOTIONBLEND.NNE_MOTIONBLEND_REPLACE_ALL:
                        if (tflag != 0 || rflag != 0 || sflag != 0)
                        {
                            (~arrayPointer1).Translation.x = nnCalcTrsMotionTv.x;
                            (~arrayPointer1).Translation.y = nnCalcTrsMotionTv.y;
                            (~arrayPointer1).Translation.z = nnCalcTrsMotionTv.z;
                            (~arrayPointer1).Rotation = nnsQuaternion1;
                            (~arrayPointer1).Scaling.x = nnCalcTrsMotionSv.x;
                            (~arrayPointer1).Scaling.y = nnCalcTrsMotionSv.y;
                            (~arrayPointer1).Scaling.z = nnCalcTrsMotionSv.z;
                            break;
                        }

                        (~arrayPointer1).Assign(~arrayPointer2);
                        break;
                    case NNE_MOTIONBLEND.NNE_MOTIONBLEND_ADD_TRANSLATION:
                        if (tflag != 0 || rflag != 0 || sflag != 0)
                        {
                            (~arrayPointer1).Translation.x = (~arrayPointer2).Translation.x + nnCalcTrsMotionTv.x -
                                                             (~pNodeList).Translation.x;
                            (~arrayPointer1).Translation.y = (~arrayPointer2).Translation.y + nnCalcTrsMotionTv.y -
                                                             (~pNodeList).Translation.y;
                            (~arrayPointer1).Translation.z = (~arrayPointer2).Translation.z + nnCalcTrsMotionTv.z -
                                                             (~pNodeList).Translation.z;
                            (~arrayPointer1).Rotation = nnsQuaternion1;
                            (~arrayPointer1).Scaling.x = nnCalcTrsMotionSv.x;
                            (~arrayPointer1).Scaling.y = nnCalcTrsMotionSv.y;
                            (~arrayPointer1).Scaling.z = nnCalcTrsMotionSv.z;
                            break;
                        }

                        (~arrayPointer1).Assign(~arrayPointer2);
                        break;
                    case NNE_MOTIONBLEND.NNE_MOTIONBLEND_ADD_ALL:
                        if (tflag != 0)
                        {
                            (~arrayPointer1).Translation.x = (~arrayPointer2).Translation.x + nnCalcTrsMotionTv.x -
                                                             (~pNodeList).Translation.x;
                            (~arrayPointer1).Translation.y = (~arrayPointer2).Translation.y + nnCalcTrsMotionTv.y -
                                                             (~pNodeList).Translation.y;
                            (~arrayPointer1).Translation.z = (~arrayPointer2).Translation.z + nnCalcTrsMotionTv.z -
                                                             (~pNodeList).Translation.z;
                        }
                        else
                        {
                            (~arrayPointer1).Translation.x = (~arrayPointer2).Translation.x;
                            (~arrayPointer1).Translation.y = (~arrayPointer2).Translation.y;
                            (~arrayPointer1).Translation.z = (~arrayPointer2).Translation.z;
                        }

                        if (rflag != 0)
                        {
                            nnMultiplyQuaternion(ref (~arrayPointer1).Rotation, ref (~arrayPointer2).Rotation,
                                ref nnsQuaternion2);
                            nnMultiplyQuaternion(ref (~arrayPointer1).Rotation, ref (~arrayPointer1).Rotation,
                                ref nnsQuaternion1);
                        }
                        else
                            (~arrayPointer1).Rotation = (~arrayPointer2).Rotation;

                        if (sflag != 0)
                        {
                            (~arrayPointer1).Scaling.x = (~arrayPointer2).Scaling.x * nnCalcTrsMotionSv.x /
                                                         (~pNodeList).Scaling.x;
                            (~arrayPointer1).Scaling.y = (~arrayPointer2).Scaling.y * nnCalcTrsMotionSv.y /
                                                         (~pNodeList).Scaling.y;
                            (~arrayPointer1).Scaling.z = (~arrayPointer2).Scaling.z * nnCalcTrsMotionSv.z /
                                                         (~pNodeList).Scaling.z;
                            break;
                        }

                        (~arrayPointer1).Scaling.x = (~arrayPointer2).Scaling.x;
                        (~arrayPointer1).Scaling.y = (~arrayPointer2).Scaling.y;
                        (~arrayPointer1).Scaling.z = (~arrayPointer2).Scaling.z;
                        break;
                }

                ++pNodeList;
                ++arrayPointer1;
                ++arrayPointer2;
            }
        }
    }

    private void nnBlendMotionNode(
        NNS_TRS dsttrs,
        NNS_TRS srctrs,
        NNS_OBJECT obj,
        int inode,
        NNS_MOTION mot,
        float frame,
        NNE_MOTIONBLEND blendmode)
    {
        if (((int)mot.fType & 1) == 0)
            return;
        if (nnCalcMotionFrame(out frame, mot.fType, mot.StartFrame, mot.EndFrame, frame) == 0)
        {
            if (dsttrs == srctrs)
                return;
            dsttrs.Assign(srctrs);
        }
        else
        {
            NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
            NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
            NNS_VECTOR nnCalcTrsMotionTv = nnCalcTRSMotion_tv;
            nnCalcTrsMotionTv.Clear();
            NNS_VECTOR nnCalcTrsMotionSv = nnCalcTRSMotion_sv;
            nnCalcTrsMotionSv.Clear();
            NNS_NODE pNode = obj.pNodeList[inode];
            int tflag;
            int rflag;
            int sflag;
            nnCalcNodeMotionTRSCore(out tflag, out rflag, out sflag, nnCalcTrsMotionTv, nnCalcTrsMotionSv,
                ref nnsQuaternion1, ref nnsQuaternion2, true, pNode, inode, mot, 0, frame);
            switch (blendmode)
            {
                case NNE_MOTIONBLEND.NNE_MOTIONBLEND_REPLACE_ALL:
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
                case NNE_MOTIONBLEND.NNE_MOTIONBLEND_ADD_TRANSLATION:
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
                case NNE_MOTIONBLEND.NNE_MOTIONBLEND_ADD_ALL:
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
                        nnMultiplyQuaternion(ref dsttrs.Rotation, ref srctrs.Rotation, ref nnsQuaternion2);
                        nnMultiplyQuaternion(ref dsttrs.Rotation, ref dsttrs.Rotation, ref nnsQuaternion1);
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
        NNS_MATRIX jnt1mtx,
        NNS_MATRIX effmtx,
        NNS_OBJECT obj,
        NNS_TRS[] trslist,
        NNS_MATRIX basemtx,
        int jnt1idx)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixTRSList2BoneSIIK(
        NNS_MATRIX jnt1mtx,
        NNS_MATRIX jnt2mtx,
        NNS_MATRIX effmtx,
        NNS_OBJECT obj,
        NNS_TRS[] trslist,
        NNS_MATRIX basemtx,
        int jnt1idx)
    {
        mppAssertNotImpl();
    }

    private static void nnCalcMatrixPaletteTRSListNode1BoneSIIK(int jnt1nodeIdx)
    {
        mppAssertNotImpl();
    }

    private static void nnCalcMatrixPaletteTRSListNode2BoneSIIK(int jnt1nodeIdx)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixPaletteLinkMotionNode(int nodeIdx)
    {
        NNS_VECTOR tv1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR sv1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR tv2 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR sv2 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        NNS_TRS nnsTrs = new NNS_TRS();
        NNS_NODE nnsNode;
        do
        {
            nnsNode = nncalctrsmotion.nnsNodeList[nodeIdx];
            NNS_QUATERNION invrq = new NNS_QUATERNION();
            int tflag1;
            int rflag1;
            int sflag1;
            nncalctrsmotion.nnsSubMotIdx0 = nnCalcNodeMotionTRSCore(out tflag1, out rflag1, out sflag1, tv1, sv1,
                ref nnsQuaternion1, ref invrq, false, nnsNode, nodeIdx, nncalctrsmotion.nnsMot0,
                nncalctrsmotion.nnsSubMotIdx0, nncalctrsmotion.nnsFrame0);
            int tflag2;
            int rflag2;
            int sflag2;
            nncalctrsmotion.nnsSubMotIdx1 = nnCalcNodeMotionTRSCore(out tflag2, out rflag2, out sflag2, tv2, sv2,
                ref nnsQuaternion2, ref invrq, false, nnsNode, nodeIdx, nncalctrsmotion.nnsMot1,
                nncalctrsmotion.nnsSubMotIdx1, nncalctrsmotion.nnsFrame1);
            float num = 1f - nncalctrsmotion.nnsRatio;
            nnPushMatrix(nncalctrsmotion.nnsMstk, null);
            NNS_MATRIX currentMatrix = nnGetCurrentMatrix(nncalctrsmotion.nnsMstk);
            if ((tflag1 | tflag2) != 0)
            {
                nnsTrs.Translation.x = (float)(tv1.x * (double)num + tv2.x * (double)nncalctrsmotion.nnsRatio);
                nnsTrs.Translation.y = (float)(tv1.y * (double)num + tv2.y * (double)nncalctrsmotion.nnsRatio);
                nnsTrs.Translation.z = (float)(tv1.z * (double)num + tv2.z * (double)nncalctrsmotion.nnsRatio);
                nnTranslateMatrixFast(currentMatrix, nnsTrs.Translation.x, nnsTrs.Translation.y, nnsTrs.Translation.z);
            }
            else
                nnTranslateMatrixFast(currentMatrix, tv1.x, tv1.y, tv1.z);

            if (((int)nnsNode.fType & 4096) != 0)
                nnCopyMatrix33(currentMatrix, nncalctrsmotion.nnsBaseMtx);
            else if (((int)nnsNode.fType & 1835008) != 0)
            {
                if (((int)nnsNode.fType & 262144) != 0)
                    nnNormalizeColumn0(currentMatrix);
                if (((int)nnsNode.fType & 524288) != 0)
                    nnNormalizeColumn1(currentMatrix);
                if (((int)nnsNode.fType & 1048576) != 0)
                    nnNormalizeColumn2(currentMatrix);
            }

            if ((rflag1 | rflag2) != 0)
                nnSlerpQuaternion(out nnsTrs.Rotation, ref nnsQuaternion1, ref nnsQuaternion2,
                    nncalctrsmotion.nnsRatio);
            else
                nnsTrs.Rotation = nnsQuaternion1;
            nnQuaternionMatrix(currentMatrix, currentMatrix, ref nnsTrs.Rotation);
            if ((sflag1 | sflag2) != 0)
            {
                nnsTrs.Scaling.x = (float)(sv1.x * (double)num + sv2.x * (double)nncalctrsmotion.nnsRatio);
                nnsTrs.Scaling.y = (float)(sv1.y * (double)num + sv2.y * (double)nncalctrsmotion.nnsRatio);
                nnsTrs.Scaling.z = (float)(sv1.z * (double)num + sv2.z * (double)nncalctrsmotion.nnsRatio);
                nnScaleMatrix(currentMatrix, currentMatrix, nnsTrs.Scaling.x, nnsTrs.Scaling.y, nnsTrs.Scaling.z);
            }
            else
                nnScaleMatrixFast(currentMatrix, sv1.x, sv1.y, sv1.z);

            if (nnsNode.iMatrix != -1)
            {
                if (((int)nnsNode.fType & 8) != 0)
                    nnCopyMatrix(nncalctrsmotion.nnsMtxPal[nnsNode.iMatrix], currentMatrix);
                else
                    nnMultiplyMatrix(nncalctrsmotion.nnsMtxPal[nnsNode.iMatrix], currentMatrix, nnsNode.InvInitMtx);
            }

            if (nncalctrsmotion.nnsNodeStatList != null)
            {
                if (nodeIdx == 0 && nncalctrsmotion.nnsNSFlag != 0U)
                    nncalctrsmotion.nnsRootScale = nnEstimateMatrixScaling(currentMatrix);
                nnCalcClipSetNodeStatus(nncalctrsmotion.nnsNodeStatList, nncalctrsmotion.nnsNodeList, nodeIdx,
                    currentMatrix, nncalctrsmotion.nnsRootScale, nncalctrsmotion.nnsNSFlag);
            }

            if (nnsNode.iChild != -1)
                this.nnCalcMatrixPaletteLinkMotionNode(nnsNode.iChild);
            nnPopMatrix(nncalctrsmotion.nnsMstk);
            nodeIdx = nnsNode.iSibling;
        } while (nnsNode.iSibling != -1);

        GlobalPool<NNS_VECTOR>.Release(tv1);
        GlobalPool<NNS_VECTOR>.Release(sv1);
        GlobalPool<NNS_VECTOR>.Release(tv2);
        GlobalPool<NNS_VECTOR>.Release(sv2);
    }

    private void nnCalcMatrixTRSList1BoneXSIIK(
        NNS_MATRIX[] mtxlist,
        NNS_OBJECT obj,
        NNS_TRS[] trslist,
        NNS_MATRIX basemtx,
        int rootidx)
    {
        int index1 = -1;
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        int index2 = -1;
        NNS_VECTORFAST dst = new NNS_VECTORFAST();
        NNS_NODE[] pNodeList = obj.pNodeList;
        NNS_NODE nnsNode1 = pNodeList[rootidx];
        NNS_MATRIX src = mtxlist[rootidx];
        NNS_NODE nnsNode2;
        for (int index3 = nnsNode1.iChild; index3 != -1; index3 = nnsNode2.iSibling)
        {
            nnsNode2 = pNodeList[index3];
            if (((int)nnsNode2.fType & 16384) != 0)
                index1 = index3;
            if (((int)nnsNode2.fType & 8192) != 0)
                index2 = index3;
        }

        NNM_ASSERT(index1 != -1, "XSIIK 1Bone Joint1 not Found");
        NNM_ASSERT(index2 != -1, "XSIIK 1Bone Effector not Found");
        NNS_MATRIX nnsMatrix2 = nnsMatrix1;
        NNS_NODE nnsNode3 = pNodeList[index1];
        NNS_TRS nnsTrs1 = trslist[index1];
        NNS_MATRIX nnsMatrix3 = mtxlist[index1];
        NNS_NODE nnsNode4 = pNodeList[index2];
        NNS_TRS nnsTrs2 = trslist[index2];
        NNS_MATRIX nnsMatrix4 = mtxlist[index2];
        float siikBoneLength = nnsNode3.SIIKBoneLength;
        nnMakeQuaternionMatrix(nnsMatrix2, ref nnsTrs1.Rotation);
        nnScaleMatrix(nnsMatrix2, nnsMatrix2, nnsTrs1.Scaling.x, 1f, 1f);
        nnMakeUnitMatrix(nnsMatrix4);
        nnTransformVectorFast(out dst, basemtx, nnsTrs2.Translation);
        nnCopyVectorFastMatrixTranslation(nnsMatrix4, ref dst);
        nnCopyMatrix(nnsMatrix3, src);
        nnCalc1BoneSIIK(nnsMatrix3, nnsMatrix2, nnsMatrix4, siikBoneLength);
        if (((int)nnsNode4.fType & 4096) == 0)
            nnCopyMatrix33(nnsMatrix4, src);
        nnQuaternionMatrix(nnsMatrix4, nnsMatrix4, ref nnsTrs2.Rotation);
    }

    private void nnCalcMatrixTRSList2BoneXSIIK(
        NNS_MATRIX[] mtxlist,
        NNS_OBJECT obj,
        NNS_TRS[] trslist,
        NNS_MATRIX basemtx,
        int rootidx)
    {
        int index1 = -1;
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        int index2 = -1;
        NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
        int index3 = -1;
        NNS_VECTORFAST dst = new NNS_VECTORFAST();
        NNS_NODE[] pNodeList = obj.pNodeList;
        NNS_NODE nnsNode1 = pNodeList[rootidx];
        NNS_MATRIX src = mtxlist[rootidx];
        NNS_NODE nnsNode2;
        for (int index4 = nnsNode1.iChild; index4 != -1; index4 = nnsNode2.iSibling)
        {
            nnsNode2 = pNodeList[index4];
            if (((int)nnsNode2.fType & 32768) != 0)
            {
                index1 = index4;
                index2 = nnsNode2.iChild;
                NNM_ASSERT(pNodeList[index2].fType & 65536U, "XSIIK 2Bone Joint2 not Found");
            }

            if (((int)nnsNode2.fType & 8192) != 0)
                index3 = index4;
        }

        NNM_ASSERT(index1 != -1, "XSIIK 2Bone Joint1 not Found");
        NNM_ASSERT(index3 != -1, "XSIIK 2Bone Effector not Found");
        NNS_NODE nnsNode3 = pNodeList[index1];
        NNS_TRS nnsTrs1 = trslist[index1];
        NNS_MATRIX nnsMatrix3 = mtxlist[index1];
        NNS_MATRIX nnsMatrix4 = nnsMatrix1;
        NNS_NODE nnsNode4 = pNodeList[index2];
        NNS_TRS nnsTrs2 = trslist[index2];
        NNS_MATRIX jnt2mtx = mtxlist[index2];
        NNS_MATRIX nnsMatrix5 = nnsMatrix2;
        NNS_NODE nnsNode5 = pNodeList[index3];
        NNS_TRS nnsTrs3 = trslist[index3];
        NNS_MATRIX nnsMatrix6 = mtxlist[index3];
        nnMakeQuaternionMatrix(nnsMatrix4, ref nnsTrs1.Rotation);
        nnScaleMatrix(nnsMatrix4, nnsMatrix4, nnsTrs1.Scaling.x, 1f, 1f);
        nnMakeQuaternionMatrix(nnsMatrix5, ref nnsTrs2.Rotation);
        nnScaleMatrix(nnsMatrix5, nnsMatrix5, nnsTrs2.Scaling.x, 1f, 1f);
        float siikBoneLength1 = nnsNode3.SIIKBoneLength;
        float siikBoneLength2 = nnsNode4.SIIKBoneLength;
        nnMakeUnitMatrix(nnsMatrix6);
        nnTransformVectorFast(out dst, basemtx, nnsTrs3.Translation);
        nnCopyVectorFastMatrixTranslation(nnsMatrix6, ref dst);
        int zpref = ((int)nnsNode4.fType & 131072) == 0 ? 0 : 1;
        nnCopyMatrix(nnsMatrix3, src);
        nnCalc2BoneSIIK(nnsMatrix3, nnsMatrix4, jnt2mtx, nnsMatrix5, nnsMatrix6, siikBoneLength1, siikBoneLength2,
            zpref);
        if (((int)nnsNode5.fType & 4096) == 0)
            nnCopyMatrix33(nnsMatrix6, src);
        nnQuaternionMatrix(nnsMatrix6, nnsMatrix6, ref nnsTrs3.Rotation);
    }

    private static void nnCalcMatrixPaletteTRSListNode1BoneXSIIK(int rootidx)
    {
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        int nodeIdx1 = -1;
        NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX nnsMatrix3 = GlobalPool<NNS_MATRIX>.Alloc();
        int nodeIdx2 = -1;
        NNS_MATRIX nnsMatrix4 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX nnsMatrix5 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_VECTORFAST dst = new NNS_VECTORFAST();
        NNS_NODE nnsNode1 = nncalctrsmotion.nnsNodeList[rootidx];
        NNS_MATRIX nnsMatrix6 = nnsMatrix1;
        nnCopyMatrix(nnsMatrix6, nnGetCurrentMatrix(nncalctrsmotion.nnsMstk));
        NNS_NODE nnsNode2;
        for (int index = nnsNode1.iChild; index != -1; index = nnsNode2.iSibling)
        {
            nnsNode2 = nncalctrsmotion.nnsNodeList[index];
            if (((int)nnsNode2.fType & 16384) != 0)
                nodeIdx1 = index;
            if (((int)nnsNode2.fType & 8192) != 0)
                nodeIdx2 = index;
        }

        NNM_ASSERT(nodeIdx1 != -1, "XSIIK 1Bone Joint1 not Found");
        NNM_ASSERT(nodeIdx2 != -1, "XSIIK 1Bone Effector not Found");
        NNS_NODE nnsNode3 = nncalctrsmotion.nnsNodeList[nodeIdx1];
        NNS_TRS nnsTrs1 = nncalctrsmotion.nnsTrsList[nodeIdx1];
        NNS_MATRIX nnsMatrix7 = nnsMatrix2;
        NNS_MATRIX nnsMatrix8 = nnsMatrix3;
        NNS_NODE nnsNode4 = nncalctrsmotion.nnsNodeList[nodeIdx2];
        NNS_MATRIX nnsMatrix9 = nnsMatrix4;
        NNS_TRS nnsTrs2 = nncalctrsmotion.nnsTrsList[nodeIdx2];
        float siikBoneLength = nnsNode3.SIIKBoneLength;
        nnCopyMatrix(nnsMatrix7, nnsMatrix6);
        nnMakeQuaternionMatrix(nnsMatrix8, ref nnsTrs1.Rotation);
        nnScaleMatrix(nnsMatrix8, nnsMatrix8, nnsTrs1.Scaling.x, 1f, 1f);
        nnMakeQuaternionMatrix(nnsMatrix9, ref nnsTrs2.Rotation);
        nnScaleMatrix(nnsMatrix9, nnsMatrix9, nnsTrs2.Scaling.x, nnsTrs2.Scaling.y, nnsTrs2.Scaling.z);
        nnMakeUnitMatrix(nnsMatrix5);
        nnCopyMatrix33(nnsMatrix5, nnsMatrix9);
        nnTransformVectorFast(out dst, nncalctrsmotion.nnsBaseMtx, nnsTrs2.Translation);
        nnCopyVectorFastMatrixTranslation(nnsMatrix9, ref dst);
        nnCalc1BoneSIIK(nnsMatrix7, nnsMatrix8, nnsMatrix9, siikBoneLength);
        if (((int)nnsNode4.fType & 4096) == 0)
            nnCopyMatrix33(nnsMatrix9, nnsMatrix6);
        nnMultiplyMatrix(nnsMatrix9, nnsMatrix9, nnsMatrix5);
        if (nnsNode1.iMatrix != -1)
            nnMultiplyMatrix(nncalctrsmotion.nnsMtxPal[nnsNode1.iMatrix], nnsMatrix6, nnsNode1.InvInitMtx);
        if (nnsNode3.iMatrix != -1)
            nnMultiplyMatrix(nncalctrsmotion.nnsMtxPal[nnsNode3.iMatrix], nnsMatrix7, nnsNode3.InvInitMtx);
        if (nnsNode4.iMatrix != -1)
            nnMultiplyMatrix(nncalctrsmotion.nnsMtxPal[nnsNode4.iMatrix], nnsMatrix9, nnsNode4.InvInitMtx);
        if (nncalctrsmotion.nnsNodeStatList != null)
        {
            nnCalcClipSetNodeStatus(nncalctrsmotion.nnsNodeStatList, nncalctrsmotion.nnsNodeList, rootidx, nnsMatrix6,
                nncalctrsmotion.nnsRootScale, nncalctrsmotion.nnsNSFlag);
            nnCalcClipSetNodeStatus(nncalctrsmotion.nnsNodeStatList, nncalctrsmotion.nnsNodeList, nodeIdx1, nnsMatrix7,
                nncalctrsmotion.nnsRootScale, nncalctrsmotion.nnsNSFlag);
            nnCalcClipSetNodeStatus(nncalctrsmotion.nnsNodeStatList, nncalctrsmotion.nnsNodeList, nodeIdx2, nnsMatrix9,
                nncalctrsmotion.nnsRootScale, nncalctrsmotion.nnsNSFlag);
        }

        if (nnsNode4.iChild != -1)
        {
            nnPushMatrix(nncalctrsmotion.nnsMstk, nnsMatrix9);
            nnCalcMatrixPaletteTRSListNode(nnsNode4.iChild);
            nnPopMatrix(nncalctrsmotion.nnsMstk);
        }

        if (nnsNode4.iSibling != -1)
        {
            nnPushMatrix(nncalctrsmotion.nnsMstk, nnsMatrix7);
            nnCalcMatrixPaletteTRSListNode(nnsNode4.iSibling);
            nnPopMatrix(nncalctrsmotion.nnsMstk);
        }

        if (nnsNode3.iChild != -1)
        {
            nnPushMatrix(nncalctrsmotion.nnsMstk, nnsMatrix7);
            nnCalcMatrixPaletteTRSListNode(nnsNode3.iChild);
            nnPopMatrix(nncalctrsmotion.nnsMstk);
        }

        if (nnsNode3.iSibling == -1)
            return;
        nnCalcMatrixPaletteTRSListNode(nnsNode3.iSibling);
    }

    private static void nnCalcMatrixPaletteTRSListNode2BoneXSIIK(int rootidx)
    {
        mppAssertNotImpl();
    }

    private static void nnBindBufferVertexDescGL(NNS_VTXLIST_GL_DESC pVtxDesc, uint flag)
    {
        OpenGL.glGenBuffer(out pVtxDesc.BufferName);
        OpenGL.glBindBuffer(34962U, pVtxDesc.BufferName);
        if (((int)flag & 2) != 0 && ((int)pVtxDesc.Type & 65536) == 0)
        {
            NNS_VTXARRAY_GL p1 = pVtxDesc.pArray[0];
            int num1 = p1.Stride * pVtxDesc.nVertex;
            ByteBuffer pointer = p1.Pointer;
            ByteBuffer byteBuffer = p1.Pointer + num1;
            for (int index = 1; index < pVtxDesc.nArray; ++index)
            {
                NNS_VTXARRAY_GL p2 = pVtxDesc.pArray[index];
                int num2 = p2.Stride * pVtxDesc.nVertex;
                if (pointer > p2.Pointer)
                    pointer = p2.Pointer;
                if (byteBuffer < p2.Pointer + num2)
                    byteBuffer = p2.Pointer + num2;
            }

            pVtxDesc.pVertexBuffer = pointer;
            pVtxDesc.VertexBufferSize = byteBuffer - pointer;
            OpenGL.glBufferVertexData(new GLVertexBuffer_(pVtxDesc));
            for (int index = 0; index < pVtxDesc.nArray; ++index)
            {
                NNS_VTXARRAY_GL p2 = pVtxDesc.pArray[index];
            }
        }
        else
        {
            OpenGL.glBufferVertexData(new GLVertexBuffer_(pVtxDesc));
            for (int index = 0; index < pVtxDesc.nArray; ++index)
            {
                NNS_VTXARRAY_GL p = pVtxDesc.pArray[index];
                p.Pointer.Offset = p.Pointer - pVtxDesc.pVertexBuffer;
            }
        }
    }

    private static void nnBindBufferPrimitiveDescGL(NNS_PRIMLIST_GL_DESC pPrimDesc, uint flag)
    {
        OpenGL.glGenBuffer(out pPrimDesc.BufferName);
        OpenGL.glBindBuffer(34963U, pPrimDesc.BufferName);
        OpenGL.glBufferIndexData(new GLIndexBuffer_ByteBuffer(pPrimDesc.pIndexBuffer, pPrimDesc.IndexBufferSize));
        for (int index = 0; index < pPrimDesc.nPrim; ++index)
            pPrimDesc.pIndices[index].Offset = pPrimDesc.pIndices[index] - pPrimDesc.pIndexBuffer;
    }

    private static uint nnBindBufferVertexListGL(
        NNS_VTXLISTPTR[] dstvlist,
        NNS_VTXLISTPTR[] srcvlist,
        int nVtxList,
        uint flag)
    {
        for (int index1 = 0; index1 < nVtxList; ++index1)
        {
            if (((int)srcvlist[index1].fType & 1) != 0)
            {
                NNS_VTXLIST_GL_DESC pVtxList = (NNS_VTXLIST_GL_DESC)srcvlist[index1].pVtxList;
                NNS_VTXLIST_GL_DESC pVtxDesc = null;
                if (dstvlist != null)
                {
                    dstvlist[index1].fType = srcvlist[0].fType;
                    dstvlist[index1].pVtxList = pVtxDesc = new NNS_VTXLIST_GL_DESC();
                    pVtxDesc.Assign(pVtxList);
                }

                if (dstvlist != null)
                {
                    pVtxDesc.pArray = new NNS_VTXARRAY_GL[pVtxList.nArray];
                    for (int index2 = 0; index2 < pVtxDesc.nArray; ++index2)
                        pVtxDesc.pArray[index2] = new NNS_VTXARRAY_GL(pVtxList.pArray[index2]);
                }

                if (((int)flag & 1) != 0 && ((int)pVtxList.Type & 6) != 0)
                {
                    if (((int)flag & 2) != 0 && ((int)pVtxList.Type & 65536) == 0)
                    {
                        if (dstvlist != null)
                        {
                            byte[] data = new byte[pVtxList.VertexBufferSize];
                            Array.Copy(pVtxList.pVertexBuffer.Data, data, pVtxList.VertexBufferSize);
                            pVtxDesc.pVertexBuffer = ByteBuffer.Wrap(data);
                            for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
                                pVtxDesc.pArray[index2].Pointer =
                                    pVtxDesc.pVertexBuffer + pVtxList.pArray[index2].Pointer.Offset;
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
                        Array.Copy(pVtxList.pVertexBuffer.Data, data, pVtxList.VertexBufferSize);
                        pVtxDesc.pVertexBuffer = ByteBuffer.Wrap(data);
                        for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
                            pVtxDesc.pArray[index2].Pointer =
                                pVtxDesc.pVertexBuffer + pVtxList.pArray[index2].Pointer.Offset;
                    }
                }
                else if (dstvlist != null)
                {
                    nnBindBufferVertexDescGL(pVtxDesc, flag);
                    dstvlist[index1].fType |= 16U;
                }

                if (pVtxList.nMatrix > 0 && dstvlist != null)
                {
                    pVtxDesc.pMatrixIndices = new ushort[pVtxList.nMatrix];
                    Array.Copy(pVtxList.pMatrixIndices, pVtxDesc.pMatrixIndices, pVtxList.nMatrix);
                }
            }
            else
                NNM_ASSERT(0, "Unknown vertex foramt.\n");
        }

        return 0;
    }

    private static uint nnBindBufferPrimitiveListGL(
        NNS_PRIMLISTPTR[] dstplist,
        NNS_PRIMLISTPTR[] srcplist,
        int nPrimList,
        uint flag)
    {
        for (int index = 0; index < nPrimList; ++index)
        {
            if (dstplist != null)
                dstplist[index].fType = srcplist[0].fType | 2U;
            if (((int)srcplist[index].fType & 1) != 0)
            {
                NNS_PRIMLIST_GL_DESC pPrimList = (NNS_PRIMLIST_GL_DESC)srcplist[index].pPrimList;
                NNS_PRIMLIST_GL_DESC pPrimDesc = null;
                if (dstplist != null)
                {
                    dstplist[index].pPrimList = pPrimDesc = new NNS_PRIMLIST_GL_DESC();
                    pPrimDesc.Assign(pPrimList);
                }

                if (dstplist != null)
                {
                    pPrimDesc.pCounts = new int[pPrimList.nPrim];
                    Array.Copy(pPrimList.pCounts, pPrimDesc.pCounts, pPrimList.nPrim);
                }

                if (dstplist != null)
                {
                    pPrimDesc.pIndices = new UShortBuffer[pPrimList.nPrim];
                    Array.Copy(pPrimList.pIndices, pPrimDesc.pIndices, pPrimList.nPrim);
                    nnBindBufferPrimitiveDescGL(pPrimDesc, flag);
                }
            }
            else
                NNM_ASSERT(0, "Unknown primitive foramt.\n");
        }

        return 0;
    }

    private static uint nnBindBufferObjectGL(
        NNS_OBJECT dstobj,
        NNS_OBJECT srcobj,
        uint flag)
    {
        if (((int)srcobj.fType & 65536) != 0)
        {
            NNM_ASSERT(0, "You can not bind-buffer the common-vertex-format object.\n");
            return nnCopyObject(dstobj, srcobj, 0U);
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
            dstobj.pMatPtrList = New<NNS_MATERIALPTR>(srcobj.nMaterial);
            int num = (int)nnCopyMaterialList(dstobj.pMatPtrList, srcobj.pMatPtrList, srcobj.nMaterial, 0U);
        }
        else
        {
            int num1 = (int)nnCopyMaterialList(null, srcobj.pMatPtrList, srcobj.nMaterial, 0U);
        }

        if (dstobj != null)
        {
            dstobj.pVtxListPtrList = New<NNS_VTXLISTPTR>(srcobj.nVtxList);
            int num2 = (int)nnBindBufferVertexListGL(dstobj.pVtxListPtrList, srcobj.pVtxListPtrList, srcobj.nVtxList,
                flag);
        }
        else
        {
            int num3 = (int)nnBindBufferVertexListGL(null, srcobj.pVtxListPtrList, srcobj.nVtxList, flag);
        }

        if (dstobj != null)
        {
            dstobj.pPrimListPtrList = New<NNS_PRIMLISTPTR>(srcobj.nPrimList);
            int num2 = (int)nnBindBufferPrimitiveListGL(dstobj.pPrimListPtrList, srcobj.pPrimListPtrList,
                srcobj.nPrimList, flag);
        }
        else
        {
            int num4 = (int)nnBindBufferPrimitiveListGL(null, srcobj.pPrimListPtrList, srcobj.nPrimList, flag);
        }

        if (dstobj != null)
        {
            dstobj.pNodeList = new NNS_NODE[srcobj.nNode];
            for (int index = 0; index < srcobj.nNode; ++index)
                dstobj.pNodeList[index] = new NNS_NODE(srcobj.pNodeList[index]);
        }

        if (dstobj != null)
        {
            dstobj.pSubobjList = New<NNS_SUBOBJ>(srcobj.nSubobj);
            int num2 = (int)nnCopySubobjList(dstobj.pSubobjList, srcobj.pSubobjList, srcobj.nSubobj, flag);
        }
        else
        {
            int num5 = (int)nnCopySubobjList(null, srcobj.pSubobjList, srcobj.nSubobj, flag);
        }

        return 0;
    }

    private void nnBindBufferObjectDirectGL(NNS_OBJECT obj, uint flag)
    {
        if (((int)obj.fType & 65536) != 0)
        {
            NNM_ASSERT(0, "You can not bind-buffer the common-vertex-format object.\n");
        }
        else
        {
            if (((int)obj.fType & 64) != 0 && ((int)obj.fType & 128) == 0)
                flag |= 2U;
            for (int index = 0; index < obj.nVtxList; ++index)
            {
                NNS_VTXLISTPTR pVtxListPtr = obj.pVtxListPtrList[index];
                if (((int)pVtxListPtr.fType & 1) != 0)
                {
                    nnBindBufferVertexDescGL((NNS_VTXLIST_GL_DESC)pVtxListPtr.pVtxList, flag);
                    pVtxListPtr.fType |= 16U;
                }
                else
                    NNM_ASSERT(0, "Unknown vertex foramt.\n");
            }

            for (int index = 0; index < obj.nPrimList; ++index)
            {
                NNS_PRIMLISTPTR pPrimListPtr = obj.pPrimListPtrList[index];
                if (((int)pPrimListPtr.fType & 1) != 0)
                {
                    nnBindBufferPrimitiveDescGL((NNS_PRIMLIST_GL_DESC)pPrimListPtr.pPrimList, flag);
                    pPrimListPtr.fType |= 2U;
                }
                else
                    NNM_ASSERT(0, "Unknown primitive foramt.\n");
            }

            obj.fType |= 16777216U;
        }
    }

    private static void nnDeleteBufferObjectGL(NNS_OBJECT obj)
    {
        for (int index = 0; index < obj.nVtxList; ++index)
        {
            NNS_VTXLISTPTR pVtxListPtr = obj.pVtxListPtrList[index];
            if (((int)pVtxListPtr.fType & 16) != 0)
                OpenGL.glDeleteBuffers(1, new uint[1]
                {
                    ((NNS_VTXLIST_GL_DESC) pVtxListPtr.pVtxList).BufferName
                });
        }

        for (int index = 0; index < obj.nPrimList; ++index)
        {
            NNS_PRIMLISTPTR pPrimListPtr = obj.pPrimListPtrList[index];
            if (((int)pPrimListPtr.fType & 2) != 0)
                OpenGL.glDeleteBuffers(1, new uint[1]
                {
                    ((NNS_PRIMLIST_GL_DESC) pPrimListPtr.pPrimList).BufferName
                });
        }
    }

    public static void nnSetUpMatrixStack(ref NNS_MATRIXSTACK mstk, uint size)
    {
        mstk = new NNS_MATRIXSTACK(size);
        NNS_MATRIX identity = NNS_MATRIX.CreateIdentity();
        mstk.push(identity);
    }

    public static void nnClearMatrixStack(NNS_MATRIXSTACK mstk)
    {
        mstk.clear();
        NNS_MATRIX identity = NNS_MATRIX.CreateIdentity();
        mstk.push(identity);
    }

    public static NNS_MATRIX nnGetCurrentMatrix(NNS_MATRIXSTACK mstk)
    {
        return mstk.get();
    }

    public static void nnSetCurrentMatrix(NNS_MATRIXSTACK mstk, NNS_MATRIX mtx)
    {
        mstk.set(mtx);
    }

    public static void nnPushMatrix(NNS_MATRIXSTACK mstk, ref SNNS_MATRIX mtx)
    {
        NNS_MATRIX nnsMatrix = nnmatrixstack_mtx_pool.Alloc();
        nnCopyMatrix(nnsMatrix, ref mtx);
        mstk.push(nnsMatrix);
    }

    public static void nnPushMatrix(NNS_MATRIXSTACK mstk, NNS_MATRIX mtx)
    {
        NNS_MATRIX matrix = nnmatrixstack_mtx_pool.Alloc();
        if (mtx == null)
            matrix.Assign(mstk.get());
        else
            matrix.Assign(mtx);
        mstk.push(matrix);
    }

    public static void nnPushMatrix(NNS_MATRIXSTACK mstk)
    {
        nnPushMatrix(mstk, null);
    }

    public static void nnPopMatrix(NNS_MATRIXSTACK mstk)
    {
        nnmatrixstack_mtx_pool.Release(mstk.pop());
    }

    private void nnInitLight()
    {
        nnlight.nngLight.AmbientColor.r = 0.2f;
        nnlight.nngLight.AmbientColor.g = 0.2f;
        nnlight.nngLight.AmbientColor.b = 0.2f;
        nnlight.nngLight.AmbientColor.a = 1f;
        for (int index = 0; index < NNE_LIGHT_MAX; ++index)
        {
            NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[index];
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
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA lightData = nnlight.nngLight.LightData[no];
        lightData.bEnable = on_off;
    }

    private static void nnSetLightType(int no, uint type)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA lightData = nnlight.nngLight.LightData[no];
        lightData.fType = type;
    }

    private static void nnSetLightAmbientGL(int no, float r, float g, float b)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.Ambient.r = r;
        nnsGlLightData.Ambient.g = g;
        nnsGlLightData.Ambient.b = b;
    }

    private static void nnSetLightDiffuseGL(int no, float r, float g, float b)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.Diffuse.r = r;
        nnsGlLightData.Diffuse.g = g;
        nnsGlLightData.Diffuse.b = b;
    }

    private static void nnSetLightSpecularGL(int no, float r, float g, float b)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.Specular.r = r;
        nnsGlLightData.Specular.g = g;
        nnsGlLightData.Specular.b = b;
    }

    private static void nnSetLightColor(int no, float r, float g, float b)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        nnSetLightDiffuseGL(no, r, g, b);
        nnSetLightSpecularGL(no, r, g, b);
    }

    private static void nnSetLightAlpha(int no, float a)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        nnlight.nngLight.LightData[no].Diffuse.a = a;
    }

    private static void nnSetLightDirection(int no, float x, float y, float z)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.Direction.x = x;
        nnsGlLightData.Direction.y = y;
        nnsGlLightData.Direction.z = z;
    }

    private static void nnSetLightPosition(int no, float x, float y, float z)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.Position.x = x;
        nnsGlLightData.Position.y = y;
        nnsGlLightData.Position.z = z;
        nnsGlLightData.Position.w = 1f;
    }

    private static void nnSetLightTarget(int no, float x, float y, float z)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.Target.x = x;
        nnsGlLightData.Target.y = y;
        nnsGlLightData.Target.z = z;
    }

    private static void nnSetLightRotation(int no, int rottype, int rotx, int roty, int rotz)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.RotType = rottype;
        nnsGlLightData.Rotation.x = rotx;
        nnsGlLightData.Rotation.y = roty;
        nnsGlLightData.Rotation.z = rotz;
        float s1;
        float c1;
        nnSinCos(rotx, out s1, out c1);
        float s2;
        float c2;
        nnSinCos(roty, out s2, out c2);
        float s3;
        float c3;
        nnSinCos(rotz, out s3, out c3);
        switch (rottype)
        {
            case 1:
                nnsGlLightData.Direction.x = (float)(-c2 * (double)s3 * s1 - s2 * (double)c1);
                nnsGlLightData.Direction.y = c3 * s1;
                nnsGlLightData.Direction.z = (float)(s2 * (double)s3 * s1 - c2 * (double)c1);
                break;
            case 4:
                nnsGlLightData.Direction.x = -s2 * c1;
                nnsGlLightData.Direction.y = s1;
                nnsGlLightData.Direction.z = -c2 * c1;
                break;
            default:
                nnsGlLightData.Direction.x = (float)(-c3 * (double)s2 * c1 - s3 * (double)s1);
                nnsGlLightData.Direction.y = (float)(-s3 * (double)s2 * c1 + c3 * (double)s1);
                nnsGlLightData.Direction.z = -c2 * c1;
                break;
        }
    }

    public static void nnSetAmbientColor(float r, float g, float b)
    {
        nnlight.nngLight.AmbientColor.r = r;
        nnlight.nngLight.AmbientColor.g = g;
        nnlight.nngLight.AmbientColor.b = b;
    }

    public static void nnSetLightIntensity(int no, float intensity)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        nnlight.nngLight.LightData[no].Intensity = intensity;
    }

    public static void nnSetLightAngle(int no, int innerangle, int outerangle)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.InnerAngle = innerangle;
        nnsGlLightData.OuterAngle = outerangle;
        nnsGlLightData.SpotExponent = 0.0f;
        nnsGlLightData.SpotCutoff = NNM_A32toDEG(outerangle);
    }

    public static void nnSetLightSpotEffectGL(int no, float exp, float cutoff)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.SpotExponent = exp;
        nnsGlLightData.SpotCutoff = cutoff;
    }

    public static void nnSetLightRange(int no, float innerrange, float outerrange)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.InnerRange = innerrange;
        nnsGlLightData.OuterRange = outerrange;
    }

    public static void nnSetLightFallOff(int no, float falloffstart, float falloffend)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.FallOffStart = falloffstart;
        nnsGlLightData.FallOffEnd = falloffend;
        nnsGlLightData.ConstantAttenuation = 1f;
        nnsGlLightData.LinearAttenuation = 0.0f;
        if (falloffstart > 9.99999996004197E-13)
            nnsGlLightData.QuadraticAttenuation = (float)(1.0 / (falloffstart * (double)falloffstart));
        else
            nnsGlLightData.QuadraticAttenuation = 1E+12f;
    }

    public static void nnSetLightAttenuationGL(int no, float cnst, float lin, float quad)
    {
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnsGlLightData.ConstantAttenuation = cnst;
        nnsGlLightData.LinearAttenuation = lin;
        nnsGlLightData.QuadraticAttenuation = quad;
    }

    public static void nnSetLightMatrix(NNS_MATRIX mtx)
    {
        if (mtx != null)
            nnCopyMatrix(nnlight.nngLightMtx, mtx);
        else
            nnMakeUnitMatrix(nnlight.nngLightMtx);
    }

    public static void nnSetUpParallelLight(
        NNS_LIGHT_PARALLEL light,
        ref NNS_RGBA color,
        float inten,
        NNS_VECTOR dir)
    {
        light.Color = color;
        light.Intensity = inten;
        light.Direction.Assign(dir);
    }

    public static void nnSetUpPointLight(
        NNS_LIGHT_POINT light,
        ref NNS_RGBA color,
        float inten,
        NNS_VECTOR pos,
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
        NNS_LIGHT_TARGET_SPOT light,
        ref NNS_RGBA color,
        float inten,
        NNS_VECTOR pos,
        NNS_VECTOR target,
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
        NNS_LIGHT_ROTATION_SPOT light,
        ref NNS_RGBA color,
        float inten,
        NNS_VECTOR pos,
        int rottype,
        NNS_ROTATE_A32 rotation,
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
        NNS_LIGHT_TARGET_DIRECTIONAL light,
        ref NNS_RGBA color,
        float inten,
        NNS_VECTOR pos,
        NNS_VECTOR target,
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
        NNS_LIGHT_ROTATION_DIRECTIONAL light,
        ref NNS_RGBA color,
        float inten,
        NNS_VECTOR pos,
        int rottype,
        NNS_ROTATE_A32 rotation,
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
        NNS_LIGHT_STANDARD_GL light,
        ref NNS_RGBA ambient,
        ref NNS_RGBA diffuse,
        ref NNS_RGBA specular,
        NNS_VECTOR4D position,
        NNS_VECTOR direction,
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
        if (no >= NNE_LIGHT_MAX)
            return;
        NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[no];
        nnSetLightType(no, type);
        uint num = type & 65599U;
        if (num <= 8U)
        {
            switch (num - 1)
            {
                case 0:
                    NNS_LIGHT_PARALLEL nnsLightParallel = (NNS_LIGHT_PARALLEL)(NNS_LIGHT_TARGET_DIRECTIONAL)light;
                    nnSetLightColor(no, nnsLightParallel.Color.r, nnsLightParallel.Color.g, nnsLightParallel.Color.b);
                    nnSetLightIntensity(no, nnsLightParallel.Intensity);
                    nnSetLightDirection(no, nnsLightParallel.Direction.x, nnsLightParallel.Direction.y,
                        nnsLightParallel.Direction.z);
                    nnSetLightAmbientGL(no, 0.0f, 0.0f, 0.0f);
                    break;
                case 1:
                    NNS_LIGHT_POINT nnsLightPoint = (NNS_LIGHT_POINT)(NNS_LIGHT_TARGET_DIRECTIONAL)light;
                    nnSetLightColor(no, nnsLightPoint.Color.r, nnsLightPoint.Color.g, nnsLightPoint.Color.b);
                    nnSetLightIntensity(no, nnsLightPoint.Intensity);
                    nnSetLightPosition(no, nnsLightPoint.Position.x, nnsLightPoint.Position.y,
                        nnsLightPoint.Position.z);
                    nnSetLightFallOff(no, nnsLightPoint.FallOffStart, nnsLightPoint.FallOffEnd);
                    nnSetLightAmbientGL(no, 0.0f, 0.0f, 0.0f);
                    break;
                case 2:
                    break;
                case 3:
                    NNS_LIGHT_TARGET_SPOT nnsLightTargetSpot =
                        (NNS_LIGHT_TARGET_SPOT)(NNS_LIGHT_TARGET_DIRECTIONAL)light;
                    nnSetLightColor(no, nnsLightTargetSpot.Color.r, nnsLightTargetSpot.Color.g,
                        nnsLightTargetSpot.Color.b);
                    nnSetLightIntensity(no, nnsLightTargetSpot.Intensity);
                    nnSetLightPosition(no, nnsLightTargetSpot.Position.x, nnsLightTargetSpot.Position.y,
                        nnsLightTargetSpot.Position.z);
                    nnSetLightTarget(no, nnsLightTargetSpot.Target.x, nnsLightTargetSpot.Target.y,
                        nnsLightTargetSpot.Target.z);
                    nnSetLightAngle(no, nnsLightTargetSpot.InnerAngle, nnsLightTargetSpot.OuterAngle);
                    nnSetLightFallOff(no, nnsLightTargetSpot.FallOffStart, nnsLightTargetSpot.FallOffEnd);
                    nnSetLightAmbientGL(no, 0.0f, 0.0f, 0.0f);
                    break;
                case 8:
                    NNS_LIGHT_ROTATION_SPOT lightRotationSpot =
                        (NNS_LIGHT_ROTATION_SPOT)(NNS_LIGHT_TARGET_DIRECTIONAL)light;
                    nnSetLightColor(no, lightRotationSpot.Color.r, lightRotationSpot.Color.g,
                        lightRotationSpot.Color.b);
                    nnSetLightIntensity(no, lightRotationSpot.Intensity);
                    nnSetLightPosition(no, lightRotationSpot.Position.x, lightRotationSpot.Position.y,
                        lightRotationSpot.Position.z);
                    nnSetLightRotation(no, lightRotationSpot.RotType, lightRotationSpot.Rotation.x,
                        lightRotationSpot.Rotation.y, lightRotationSpot.Rotation.z);
                    nnSetLightAngle(no, lightRotationSpot.InnerAngle, lightRotationSpot.OuterAngle);
                    nnSetLightFallOff(no, lightRotationSpot.FallOffStart, lightRotationSpot.FallOffEnd);
                    nnSetLightAmbientGL(no, 0.0f, 0.0f, 0.0f);
                    break;
            }
        }
        else if (num != 16U)
        {
            if (num != 32U)
            {
                if (num != 65536U)
                    return;
                mppAssertNotImpl();
            }
            else
                mppAssertNotImpl();
        }
        else
            mppAssertNotImpl();
    }

    private static void nnPutLightSettings()
    {
        OpenGL.glMatrixMode(5888U);
        Matrix nngLightMtx = (Matrix)nnlight.nngLightMtx;
        OpenGL.glLoadMatrixf(ref nngLightMtx);
        OpenGL.glArray4f glArray4f1;
        glArray4f1.f0 = nnlight.nngLight.AmbientColor.r;
        glArray4f1.f1 = nnlight.nngLight.AmbientColor.g;
        glArray4f1.f2 = nnlight.nngLight.AmbientColor.b;
        glArray4f1.f3 = nnlight.nngLight.AmbientColor.a;
        OpenGL.glLightModelfv(2899U, glArray4f1);
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int index1 = 0;
        for (; index1 < NNE_LIGHT_MAX; ++index1)
        {
            NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[index1];
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

        nnlight.nngNumParallelLight = num1;
        nnlight.nngNumPointLight = num2;
        nnlight.nngNumSpotLight = num3;
        int _idx1 = -1;
        int index2 = -1;
        int index3 = -1;
        int num4 = -1;
        int index4 = 0;
        int _idx2 = 0;
        for (; index4 < NNE_LIGHT_MAX; ++index4)
        {
            NNS_GL_LIGHT_DATA nnsGlLightData = nnlight.nngLight.LightData[index4];
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
                            num5 = NNM_GL_LIGHT(num1 + index2);
                            goto label_27;
                        case 2:
                            goto label_26;
                        case 3:
                            ++index3;
                            num5 = NNM_GL_LIGHT(num1 + num2 + index3);
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
                num5 = NNM_GL_LIGHT(_idx1);
                goto label_27;
            label_26:
                ++num4;
                num5 = NNM_GL_LIGHT(num1 + num2 + num3 + num4);
            label_27:
                OpenGL.glEnable(num5);
                if (nnsGlLightData.Intensity == 1.0)
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
                        nnNormalizeVector(ref glArray4f4, ref glArray4f4);
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
                    nnNormalizeVector(ref glArray4f2, ref glArray4f2);
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
                        nnlight.nngPointLightFallOffEnd[index2] = nnsGlLightData.FallOffEnd;
                        float num6 = nnsGlLightData.FallOffEnd - nnsGlLightData.FallOffStart;
                        float num7 = num6 > 9.99999996004197E-13 ? 1f / num6 : 1E+12f;
                        nnlight.nngPointLightFallOffScale[index2] = num7;
                        break;
                    case 4:
                    case 8:
                        nnlight.nngSpotLightFallOffEnd[index3] = nnsGlLightData.FallOffEnd;
                        float num8 = nnsGlLightData.FallOffEnd - nnsGlLightData.FallOffStart;
                        float num9 = num8 > 9.99999996004197E-13 ? 1f / num8 : 1E+12f;
                        nnlight.nngSpotLightFallOffScale[index3] = num9;
                        float num10 = nnCos(nnsGlLightData.OuterAngle) - nnCos(nnsGlLightData.InnerAngle);
                        float num11 = num10 < 9.99999996004197E-13 ? 1f / num10 : -1E+12f;
                        nnlight.nngSpotLightAngleScale[index3] = num11;
                        break;
                }

                ++_idx2;
            }
        }

        for (; _idx2 < NNE_LIGHT_MAX; ++_idx2)
            OpenGL.glDisable(NNM_GL_LIGHT(_idx2));
    }

    private uint nnEstimateLightBufferSize(uint type)
    {
        mppAssertNotImpl();
        return 0;
    }

    private void nnCalcMotionCameraScalar(NNS_SUBMOTION submot, float frame, ref float val)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMotionCameraAngle(NNS_SUBMOTION submot, float frame, ref int ang)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMotionCameraXYZ(
        NNS_SUBMOTION submot,
        float frame,
        NNS_VECTOR xyz)
    {
        mppAssertNotImpl();
    }

    private void nnCalcCameraMotionCore(
        NNS_CAMERAPTR dstptr,
        NNS_CAMERAPTR camptr,
        NNS_MOTION mot,
        float frame)
    {
        mppAssertNotImpl();
    }

    private void nnCalcCameraMotion(
        NNS_CAMERAPTR dstptr,
        NNS_CAMERAPTR camptr,
        NNS_MOTION mot,
        float frame)
    {
        mppAssertNotImpl();
    }

    public static void nnSetUpTexlist(out NNS_TEXLIST texlist, int num, ref object buf)
    {
        buf = texlist = new NNS_TEXLIST();
        texlist.nTex = num;
        texlist.pTexInfoList = new NNS_TEXINFO[num];
        for (int index = 0; index < num; ++index)
            texlist.pTexInfoList[index] = new NNS_TEXINFO();
    }

    public static uint nnEstimateTexlistSize(int num)
    {
        mppAssertNotImpl();
        return (uint)num * 0x10 + 8; 
    }

    private static int nnSetTextureList(NNS_TEXLIST pTexList)
    {
        nngCurrentTextureList = pTexList;
        return 1;
    }

    public static int nnGetTextureList(out NNS_TEXLIST pTexList)
    {
        pTexList = null;
        mppAssertNotImpl();
        return 1;
    }

    private static int nnSetTexInfo(int slot, NNS_TEXINFO pTexInfo)
    {
        if (slot >= nngGLExtensions.max_texture_units)
            return 1;
        OpenGL.glActiveTexture(NNM_GL_TEXTURE(slot));
        if (pTexInfo != null)
            OpenGL.glBindTexture(3553U, pTexInfo.TexName);
        if (pTexInfo != null)
            OpenGL.glEnable(3553U);
        else
            OpenGL.glDisable(3553U);
        return 1;
    }

    public static int nnSetTexture(int slot, NNS_TEXLIST pTexList, int num)
    {
        mppAssertNotImpl();
        return 0;
    }

    private static int nnSetTextureNum(int slot, int num)
    {
        if (num < 0)
            return nnSetTexInfo(slot, null);
        NNS_TEXINFO[] pTexInfoList = nngCurrentTextureList.pTexInfoList;
        return nnSetTexInfo(slot, pTexInfoList[num]);
    }

    private void nnConfigureSystemGL(NNS_CONFIG_GL config)
    {
        if (nnsystem_init != 1)
        {
            nnsystem_init = 1;
            this.nnInitCircumsphere();
            this.nnInitLight();
        }

        nngScreen.ax = 1f;
        nngScreen.ay = 1f;
        nngScreen.aspect = 1f;
        nngScreen.dist = 500f;
        nngScreen.xad = nngScreen.ax * nngScreen.dist;
        nngScreen.yad = (float)-(nngScreen.ay * (double)nngScreen.dist);
        nngScreen.ooxad = 1f / nngScreen.xad;
        nngScreen.ooyad = 1f / nngScreen.yad;
        nngScreen.w = config.WindowWidth;
        nngScreen.h = config.WindowHeight;
        nngScreen.cx = nngScreen.w * 0.5f;
        nngScreen.cy = nngScreen.h * 0.5f;
        nngClip2d.x0 = 0.0f;
        nngClip2d.y0 = 0.0f;
        nngClip2d.x1 = nngScreen.w - 1f;
        nngClip2d.y1 = nngScreen.h - 1f;
        nngClip2d.n_clip = 1f;
        nngClip2d.f_clip = 10000f;
        nngClip3d.x0 = nngClip2d.x0 - nngScreen.cx;
        nngClip3d.y0 = nngClip2d.y0 - nngScreen.cy;
        nngClip3d.x1 = nngClip2d.x1 - nngScreen.cx;
        nngClip3d.y1 = nngClip2d.y1 - nngScreen.cy;
        nngClip3d.n_clip = 1f;
        nngClip3d.f_clip = 10000f;
    }

    public static void nnSetProjection(NNS_MATRIX mtx, int type)
    {
        nngProjectionMatrix.Assign(mtx);
        nngProjectionType = type;
        nnSetClipPlane();
        nnLoadProjectionMatrixGL(mtx);
    }

    public static void nnLoadProjectionMatrixGL(NNS_MATRIX mtx)
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
        ArrayPointer<NNS_MATRIX> dstlist,
        NNS_MATRIX src,
        ArrayPointer<NNS_MATRIX> srclist,
        int num)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixPaletteMultiplyMatrix(
        ArrayPointer<NNS_MATRIX> dstpal,
        NNS_MATRIX src,
        ArrayPointer<NNS_MATRIX> srcpal,
        int num)
    {
        this.nnCalcMultiplyMatrices(dstpal, src, srcpal, num);
    }

    private uint nnCalcShaderManageBufferSizeGL(int num)
    {
        mppAssertNotImpl();
        return 0;
    }

    private void nnSetUpShaderConfigBasicGL(NNS_SHADER_CONFIG config)
    {
        mppAssertNotImpl();
    }

    private void nnConfigureShaderGL(NNS_SHADER_CONFIG config, object managebuffer, int num)
    {
        mppAssertNotImpl();
    }

    private NNS_SHADER_NAME nnGetShaderNameGL(NNS_SHADER_PROFILE profile)
    {
        NNS_SHADER_NAME nnsShaderName = new NNS_SHADER_NAME();
        mppAssertNotImpl();
        return nnsShaderName;
    }

    private void nnGetShaderProfileGL(
        NNS_SHADER_PROFILE profile,
        NNS_SHADER_NAME Name)
    {
        mppAssertNotImpl();
    }

    private static int nnCompareShaderName(NNS_SHADER_NAME lhs, NNS_SHADER_NAME rhs)
    {
        mppAssertNotImpl();
        return 0;
    }

    private int nnRegistShaderNameGL(NNS_SHADER_NAME Name)
    {
        mppAssertNotImpl();
        return 0;
    }

    private int nnRegistShaderProfileGL(NNS_SHADER_PROFILE profile)
    {
        mppAssertNotImpl();
        return 0;
    }

    private static int nnGetTexCoord(uint fType)
    {
        mppAssertNotImpl();
        return 0;
    }

    private static int nnTexCoordIndex(int texcoord)
    {
        mppAssertNotImpl();
        return texcoord;
    }

    private void nnInitShaderProfileGL(NNS_SHADER_PROFILE profile)
    {
        mppAssertNotImpl();
    }

    private int nnSetupShaderProfile(
        NNS_SHADER_PROFILE profile,
        NNS_MATERIALPTR pMat,
        NNS_VTXLISTPTR pVtxListPtr,
        uint flag)
    {
        mppAssertNotImpl();
        return 0;
    }

    private int nnRegistObjectShaderProfilesGL(NNS_OBJECT obj, uint flag)
    {
        mppAssertNotImpl();
        return 0;
    }

    private int nnGetCurrentShaderProfileNumberGL()
    {
        mppAssertNotImpl();
        return 0;
    }

    private void nnGetShaderProfileOneGL(NNS_SHADER_PROFILE profile, int idx)
    {
        mppAssertNotImpl();
    }

    private NNS_SHADER_NAME nnGetShaderNameListGL()
    {
        mppAssertNotImpl();
        return new NNS_SHADER_NAME();
    }

    private void nnClearShaderProfilesGL()
    {
        mppAssertNotImpl();
    }

    private uint nnCalcBuildShaderWorkBufferSizeGL(uint vtxshadersize, uint fragshadersize)
    {
        mppAssertNotImpl();
        return 0;
    }

    private static int nnCompareShaderManager(object elem1, object elem2)
    {
        mppAssertNotImpl();
        return 0;
    }

    private int nnGetUnbuildShaderProfileNumberGL()
    {
        mppAssertNotImpl();
        return 0;
    }

    private int nnGetUnbuildShaderProfileOneGL(NNS_SHADER_PROFILE profile)
    {
        mppAssertNotImpl();
        return 0;
    }

    private void nnRegistCompiledShaderProfileGL(
        NNS_COMPILED_SHADER_PROFILE compiledShader,
        NNS_SHADER_PROFILE profile)
    {
        mppAssertNotImpl();
    }

    private void nnBindVertexAttributeGL(uint program)
    {
        mppAssertNotImpl();
    }

    private uint nnGetErrorVertexShaderObjectGL()
    {
        mppAssertNotImpl();
        return 0;
    }

    private uint nnGetErrorFragmentShaderObjectGL()
    {
        mppAssertNotImpl();
        return 0;
    }

    private uint nnGetErrorShaderProgramObjectGL()
    {
        mppAssertNotImpl();
        return 0;
    }

    private void nnReleaseShaderGL()
    {
        mppAssertNotImpl();
    }

    private static NNS_SHADER_MANAGER nnSearchShaderManager(
        NNS_SHADER_NAME name)
    {
        mppAssertNotImpl();
        return null;
    }

    private void nnPutColorShader(NNS_DRAWCALLBACK_VAL val)
    {
        mppAssertNotImpl();
    }

    private static void nnBindFixedShaderGL()
    {
        mppAssertNotImpl();
    }

    private void nnBindPrintShaderGL()
    {
        mppAssertNotImpl();
    }

    private void nnRegistPrimitive2DShaderGL(int bTexture)
    {
        mppAssertNotImpl();
    }

    private void nnRegistPrimitive3DShaderGL(int bLighting, int bTexture, int texcoord)
    {
        mppAssertNotImpl();
    }

    private static void nnBindPrimitive2DShaderGL(int bTexture)
    {
        mppAssertNotImpl();
    }

    private void nnBindPrimitive3DShaderGL(int bLighting, int bTexture, int texcoord)
    {
        mppAssertNotImpl();
    }

    private static void nnRegistDefaultShader()
    {
        mppAssertNotImpl();
    }

    private void nnSetUserUniformGL(int idx, float x, float y, float z, float w)
    {
        mppAssertNotImpl();
    }

    private void nnPutUserUniformGL(NNS_DRAWCALLBACK_VAL val)
    {
        mppAssertNotImpl();
    }

    private static void nnSetFogSwitch(bool on_off)
    {
        nngFogSwitch = on_off;
    }

    private bool nnGetFogSwitch()
    {
        mppAssertNotImpl();
        return nngFogSwitch;
    }

    private static void nnSetFogColor(float r, float g, float b)
    {
        nnSetFogColor_col[0] = r;
        nnSetFogColor_col[1] = g;
        nnSetFogColor_col[2] = b;
        nnSetFogColor_col[3] = 1f;
        OpenGL.glFogfv(2918U, nnSetFogColor_col);
    }

    private static void nnSetFogRange(float fnear, float ffar)
    {
        OpenGL.glFogf(2917U, 9729f);
        nngFogStart = fnear;
        nngFogEnd = ffar;
    }

    private void nnSetFogLinearGL(float fnear, float ffar)
    {
        mppAssertNotImpl();
        OpenGL.glFogf(2917U, 9729f);
        nngFogStart = fnear;
        nngFogEnd = ffar;
    }

    private void nnSetFogExpGL(float density)
    {
        mppAssertNotImpl();
        OpenGL.glFogf(2917U, 2048f);
        nngFogDensity = density;
    }

    private void nnSetFogExp2GL(float density)
    {
        mppAssertNotImpl();
        OpenGL.glFogf(2917U, 2049f);
        nngFogDensity = density;
    }

    private void nnSetFogDensityGL(float density)
    {
        mppAssertNotImpl();
        nngFogDensity = density;
    }

    private static void nnPutFogSwitchGL(bool on_off)
    {
        if (on_off)
        {
            OpenGL.glEnable(2912U);
            OpenGL.glFogf(2915U, nngFogStart);
            OpenGL.glFogf(2916U, nngFogEnd);
            OpenGL.glFogf(2914U, nngFogDensity);
        }
        else
        {
            OpenGL.glDisable(2912U);
            OpenGL.glFogf(2915U, nngClip3d.f_clip);
            OpenGL.glFogf(2916U, nngClip3d.f_clip + 1f);
            OpenGL.glFogf(2914U, 0.0f);
        }
    }

    private void nnDrawMultiObjectInitialPose(
        NNS_OBJECT obj,
        NNS_MATRIX[] basemtxptrlist,
        uint[] nodestatlistptrlist,
        uint subobjtype,
        uint flag,
        int num)
    {
        mppAssertNotImpl();
    }

    private static void nnSetMaterialControlDiffuse(int mode, float r, float g, float b)
    {
        nngMatCtrlDiffuse.mode = mode;
        nngMatCtrlDiffuse.col.r = r;
        nngMatCtrlDiffuse.col.g = g;
        nngMatCtrlDiffuse.col.b = b;
    }

    private static void nnSetMaterialControlAmbient(int mode, float r, float g, float b)
    {
        nngMatCtrlAmbient.mode = mode;
        nngMatCtrlAmbient.col.r = r;
        nngMatCtrlAmbient.col.g = g;
        nngMatCtrlAmbient.col.b = b;
    }

    private void nnSetMaterialControlSpecularGL(int mode, float r, float g, float b)
    {
        mppAssertNotImpl();
    }

    private static void nnSetMaterialControlAlpha(int mode, float alpha)
    {
        nngMatCtrlAlpha.mode = mode;
        nngMatCtrlAlpha.alpha = alpha;
    }

    private static void nnSetMaterialControlEnvTexMatrix(int texsrc, NNS_MATRIX texmtx)
    {
        nngMatCtrlEnvTexMtx.texcoordsrc = texsrc;
        nnCopyMatrix(nngMatCtrlEnvTexMtx.texmtx, texmtx);
    }

    private static void nnSetMaterialControlBlendMode(int blendmode)
    {
        nngMatCtrlBlendMode.blendmode = blendmode;
    }

    private static void nnSetMaterialControlTextureOffset(int slot, int mode, float u, float v)
    {
        nngMatCtrlTexOffset[slot].mode = mode;
        nngMatCtrlTexOffset[slot].offset.u = u;
        nngMatCtrlTexOffset[slot].offset.v = v;
    }

    private static void nnSetPrimitiveBlend(int blend)
    {
        nngDrawPrimBlend = blend;
    }

    private static void nnSetPrimitiveTexNum(NNS_TEXLIST texlist, int num)
    {
        if (texlist != null && num >= 0 && num < texlist.nTex)
        {
            nngDrawPrimTexName = texlist.pTexInfoList[num].TexName;
            nngDrawPrimTexture = 1;
        }
        else
            nngDrawPrimTexture = 0;
    }

    private static void nnSetPrimitiveTexState(int blend, int coord, int uwrap, int vwrap)
    {
        switch (blend)
        {
            case 1:
                nngDrawPrimTexBlend = 7681;
                break;
            default:
                nngDrawPrimTexBlend = 8448;
                break;
        }

        nngDrawPrimTexCoord = coord;
        switch (uwrap)
        {
            case 1:
                nngDrawPrimTexSWarp = 33071;
                break;
            default:
                nngDrawPrimTexSWarp = 10497;
                break;
        }

        switch (vwrap)
        {
            case 1:
                nngDrawPrimTexTWarp = 33071;
                break;
            default:
                nngDrawPrimTexTWarp = 10497;
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
        OpenGL.glBindTexture(3553U, nngDrawPrimTexName);
        OpenGL.glTexEnvi(8960U, 8704U, nngDrawPrimTexBlend);
        OpenGL.glTexParameteri(3553U, 10242U, nngDrawPrimTexSWarp);
        OpenGL.glTexParameteri(3553U, 10243U, nngDrawPrimTexTWarp);
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



    private static void nnPutFixedMaterialGL()
    {
        OpenGL.glDisable(2884U);
        OpenGL.glLightModelf(2898U, 0.0f);
        OpenGL.glDisable(2896U);
        nnPutFogSwitchGL(false);
        OpenGL.glDepthMask(1);
        OpenGL.glColorMask(1, 1, 1, 1);
        OpenGL.glDisable(2903U);
        OpenGL.glEnable(3042U);
        OpenGL.glBlendFunc(770U, 771U);
        OpenGL.glBlendEquation(32774U);
        OpenGL.glDisable(3058U);
        OpenGL.glDisable(3008U);
        OpenGL.glEnable(2929U);
        OpenGL.glDepthFunc(515U);
        OpenGL.glMaterialfv(1032U, 4609U, (OpenGL.glArray4f)nngColorWhite);
    }

    private static void nnPutDisableTexturesGL()
    {
        OpenGL.glActiveTexture(33984U);
        OpenGL.glDisable(3553U);
        OpenGL.glActiveTexture(33985U);
        OpenGL.glDisable(3553U);
        OpenGL.glClientActiveTexture(33984U);
        OpenGL.glDisableClientState(32888U);
        OpenGL.glClientActiveTexture(33985U);
        OpenGL.glDisableClientState(32888U);
    }

    private static void nnSetDivColor(float r, float g, float b, float a)
    {
        OpenGL.glColor4f(r, g, b, a);
    }

    private static void nnSetDivColorRandom(int i)
    {
        Random random = new Random(i * 15485863);
        OpenGL.glColor3f(random.Next() / (float)short.MaxValue, random.Next() / (float)short.MaxValue, random.Next() / (float)short.MaxValue);
    }

    private static void nnSetDivColorRandomA(int nSeed, uint[] seeds)
    {
        int num1;
        uint num2 = (uint)(num1 = 0);
        uint num3 = (uint)num1;
        uint num4 = (uint)num1;
        int num5 = 0;
        for (int index = 0; index < nSeed; ++index)
        {
            int Seed = num5 ^ (int)seeds[index] * 15485863;
            Random random = new Random(Seed);
            num5 = Seed ^ random.Next() ^ random.Next() << 10 ^ random.Next() << 20;
            num4 ^= (uint)random.Next();
            num3 ^= (uint)random.Next();
            num2 ^= (uint)random.Next();
        }
        OpenGL.glColor3f(num4 / (float)short.MaxValue, num3 / (float)short.MaxValue, num2 / (float)short.MaxValue);
    }

    private static void nnPutColorStrip(int iStrip, int iMeshset, int iSubobj)
    {
        nnSetDivColorRandom(iStrip * 10007 + iMeshset * 7 + iSubobj);
    }

    private static void nnPutColorMeshset(int iMeshset, int iSubobj)
    {
        nnSetDivColorRandom(iMeshset * 7 + iSubobj);
    }

    private static void nnPutColorMaterial(int iMaterial)
    {
        nnSetDivColorRandom(iMaterial);
    }

    private static void nnPutColorNWeight(NNS_VTXLISTPTR vlistptr)
    {
        float[][] numArray = new float[5][]
        {
      new float[3]{ 0.0f, 0.0f, 1f },
      new float[3]{ 0.0f, 1f, 0.0f },
      new float[3]{ 1f, 1f, 0.0f },
      new float[3]{ 1f, 0.0f, 1f },
      new float[3]{ 1f, 0.0f, 0.0f }
        };
        if (((int)vlistptr.fType & 1) != 0)
        {
            NNS_VTXLIST_GL_DESC pVtxList = (NNS_VTXLIST_GL_DESC)vlistptr.pVtxList;
            int index1 = 0;
            for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
            {
                NNS_VTXARRAY_GL p = pVtxList.pArray[index2];
                if (p.Type == 4U)
                    index1 = p.Size;
            }
            OpenGL.glColor3fv(numArray[index1]);
        }
        else
        {
            if (((int)vlistptr.fType & 16711680) == 0)
                return;
            switch (((NNS_VTXLIST_COMMON_DESC)vlistptr.pVtxList).List0.fType & 15872U)
            {
                case 0:
                    OpenGL.glColor3fv(numArray[0]);
                    break;
                case 512:
                    OpenGL.glColor3fv(numArray[1]);
                    break;
                case 1024:
                    OpenGL.glColor3fv(numArray[2]);
                    break;
                case 2048:
                    OpenGL.glColor3fv(numArray[3]);
                    break;
                case 4096:
                    OpenGL.glColor3fv(numArray[4]);
                    break;
            }
        }
    }

    private static void nnPutColorNTexture(int nTexture)
    {
        float[][] numArray = new float[8][]
        {
      new float[3],
      new float[3]{ 0.0f, 0.0f, 1f },
      new float[3]{ 0.0f, 1f, 1f },
      new float[3]{ 0.0f, 1f, 0.0f },
      new float[3]{ 1f, 1f, 0.0f },
      new float[3]{ 1f, 0.0f, 0.0f },
      new float[3]{ 1f, 0.0f, 1f },
      new float[3]{ 1f, 1f, 1f }
        };
        if (nTexture >= numArray.Length)
            nTexture = numArray.Length - 1;
        OpenGL.glColor3fv(numArray[nTexture]);
    }
}