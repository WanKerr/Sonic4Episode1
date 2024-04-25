using Microsoft.Xna.Framework;
using mpp;

public partial class AppMain
{
    private int nnGetMaterialIndex(NNS_NODENAMELIST pMaterialNameList, string MaterialName)
    {
        return this.nnGetNodeIndex(pMaterialNameList, MaterialName);
    }

    private string nnGetMaterialName(NNS_NODENAMELIST pMaterialNameList, int MaterialIndex)
    {
        return this.nnGetNodeName(pMaterialNameList, MaterialIndex);
    }

    private static void nnSetPrimitive3DMaterial(
      ref NNS_RGBA diffuse,
      ref SNNS_RGB ambient,
      float specular)
    {
        nndrawprim3d.nnsDiffuse[0] = diffuse.r;
        nndrawprim3d.nnsDiffuse[1] = diffuse.g;
        nndrawprim3d.nnsDiffuse[2] = diffuse.b;
        nndrawprim3d.nnsDiffuse[3] = diffuse.a;
        nndrawprim3d.nnsAmbient[0] = ambient.r;
        nndrawprim3d.nnsAmbient[1] = ambient.g;
        nndrawprim3d.nnsAmbient[2] = ambient.b;
        nndrawprim3d.nnsAmbient[3] = 1f;
        nndrawprim3d.nnsSpecular[0] = specular;
        nndrawprim3d.nnsSpecular[1] = specular;
        nndrawprim3d.nnsSpecular[2] = specular;
        nndrawprim3d.nnsSpecular[3] = 1f;
    }

    private static void nnSetPrimitive3DMaterial(
      ref NNS_RGBA diffuse,
      ref NNS_RGB ambient,
      float specular)
    {
        nndrawprim3d.nnsDiffuse[0] = diffuse.r;
        nndrawprim3d.nnsDiffuse[1] = diffuse.g;
        nndrawprim3d.nnsDiffuse[2] = diffuse.b;
        nndrawprim3d.nnsDiffuse[3] = diffuse.a;
        nndrawprim3d.nnsAmbient[0] = ambient.r;
        nndrawprim3d.nnsAmbient[1] = ambient.g;
        nndrawprim3d.nnsAmbient[2] = ambient.b;
        nndrawprim3d.nnsAmbient[3] = 1f;
        nndrawprim3d.nnsSpecular[0] = specular;
        nndrawprim3d.nnsSpecular[1] = specular;
        nndrawprim3d.nnsSpecular[2] = specular;
        nndrawprim3d.nnsSpecular[3] = 1f;
    }

    private static void nnSetPrimitive3DMaterialGL(
      ref NNS_RGBA diffuse,
      ref NNS_RGBA ambient,
      ref NNS_RGBA specular,
      float shininess,
      ref NNS_RGBA emission)
    {
        nndrawprim3d.nnsDiffuse[0] = diffuse.r;
        nndrawprim3d.nnsDiffuse[1] = diffuse.g;
        nndrawprim3d.nnsDiffuse[2] = diffuse.b;
        nndrawprim3d.nnsDiffuse[3] = diffuse.a;
        nndrawprim3d.nnsAmbient[0] = ambient.r;
        nndrawprim3d.nnsAmbient[1] = ambient.g;
        nndrawprim3d.nnsAmbient[2] = ambient.b;
        nndrawprim3d.nnsAmbient[3] = ambient.a;
        nndrawprim3d.nnsSpecular[0] = specular.r;
        nndrawprim3d.nnsSpecular[1] = specular.g;
        nndrawprim3d.nnsSpecular[2] = specular.b;
        nndrawprim3d.nnsSpecular[3] = specular.a;
        nndrawprim3d.nnsShininess = shininess;
        nndrawprim3d.nnsEmission[0] = emission.r;
        nndrawprim3d.nnsEmission[1] = emission.g;
        nndrawprim3d.nnsEmission[2] = emission.b;
        nndrawprim3d.nnsEmission[3] = emission.a;
    }

    private static void nnSetPrimitive3DMatrix(ref SNNS_MATRIX mtx)
    {
        nnCopyMatrix(nndrawprim3d.nnsPrim3DMatrix, ref mtx);
    }

    private static void nnSetPrimitive3DMatrix(NNS_MATRIX mtx)
    {
        nnCopyMatrix(nndrawprim3d.nnsPrim3DMatrix, mtx);
    }

    private static void nnChangePrimitive3DMatrix(NNS_MATRIX mtx)
    {
        OpenGL.glMatrixMode(5888U);
        Matrix matrix = (Matrix)mtx;
        OpenGL.glLoadMatrixf(ref matrix);
        if (nngDrawPrimTexCoord != 1)
            return;
        nnPutEnvironmentTextureMatrix(mtx);
    }

    private static void nnSetPrimitive3DAlphaFuncGL(uint func, float _ref)
    {
        nndrawprim3d.nnsAlphaFunc = func;
        nndrawprim3d.nnsAlphaFuncRef = _ref;
    }

    private static void nnSetPrimitive3DDepthFuncGL(uint func)
    {
        nndrawprim3d.nnsDepthFunc = func;
    }

    private static void nnSetPrimitive3DDepthMaskGL(bool flag)
    {
        nndrawprim3d.nnsDepthMask = flag;
    }

    private static void nnBeginDrawPrimitive3DCore(int fmt, int blend, int light)
    {
        OpenGL.glShadeModel(7425U);
        OpenGL.glLightModelf(2898U, 0.0f);
        OpenGL.glEnable(3008U);
        OpenGL.glAlphaFunc(nndrawprim3d.nnsAlphaFunc, nndrawprim3d.nnsAlphaFuncRef);
        OpenGL.glEnable(2929U);
        OpenGL.glDepthFunc(nndrawprim3d.nnsDepthFunc);
        OpenGL.glDepthMask(nndrawprim3d.nnsDepthMask);
        OpenGL.glColorMask(1, 1, 1, 1);
        OpenGL.glBindBuffer(34962U, 0U);
        OpenGL.glBindBuffer(34963U, 0U);
        OpenGL.glDisableClientState(34477U);
        OpenGL.glDisableClientState(34884U);
        if (blend == 1)
        {
            OpenGL.glEnable(3042U);
            switch (nngDrawPrimBlend)
            {
                case 0:
                    OpenGL.glBlendFunc(770U, 1U);
                    OpenGL.glBlendEquation(32774U);
                    break;
                default:
                    OpenGL.glBlendFunc(770U, 771U);
                    OpenGL.glBlendEquation(32774U);
                    break;
            }
        }
        else
            OpenGL.glDisable(3042U);
        nnPutFogSwitchGL(nngFogSwitch);
        switch (light)
        {
            case 0:
                OpenGL.glDisable(2896U);
                OpenGL.glMaterialfv(1032U, 4609U, (OpenGL.glArray4f)nngColorWhite);
                OpenGL.glColor4fv(nndrawprim3d.nnsDiffuse);
                break;
            case 1:
                OpenGL.glEnable(2896U);
                OpenGL.glMaterialfv(1032U, 4608U, nndrawprim3d.nnsAmbient);
                OpenGL.glMaterialfv(1032U, 4609U, nndrawprim3d.nnsDiffuse);
                OpenGL.glMaterialfv(1032U, 4610U, (OpenGL.glArray4f)nngColorBlack);
                OpenGL.glMaterialf(1032U, 5633U, 0.0f);
                OpenGL.glMaterialfv(1032U, 5632U, (OpenGL.glArray4f)nngColorBlack);
                OpenGL.glColor4fv((OpenGL.glArray4f)nngColorWhite);
                break;
            case 2:
                OpenGL.glEnable(2896U);
                OpenGL.glMaterialfv(1032U, 4608U, nndrawprim3d.nnsAmbient);
                OpenGL.glMaterialfv(1032U, 4609U, nndrawprim3d.nnsDiffuse);
                OpenGL.glMaterialfv(1032U, 4610U, nndrawprim3d.nnsSpecular);
                OpenGL.glMaterialf(1032U, 5633U, nndrawprim3d.nnsShininess);
                OpenGL.glMaterialfv(1032U, 5632U, nndrawprim3d.nnsEmission);
                OpenGL.glColor4fv((OpenGL.glArray4f)nngColorWhite);
                break;
        }
        nndrawprim3d.nnsFormat = fmt;
        switch (fmt)
        {
            case 1:
                OpenGL.glEnableClientState(32884U);
                OpenGL.glEnableClientState(32885U);
                OpenGL.glDisableClientState(32886U);
                if (nngDrawPrimTexCoord == 1)
                {
                    OpenGL.glClientActiveTexture(33984U);
                    OpenGL.glEnableClientState(32888U);
                    nnPutPrimitiveTexParameter();
                    nnSetTexCoordSrc(0, 3);
                    nnSetTexCoordSrc(1, 0);
                    nnSetNormalFormatType(5126U);
                    nnPutEnvironmentTextureMatrix(nndrawprim3d.nnsPrim3DMatrix);
                    break;
                }
                nnPutPrimitiveNoTexture();
                break;
            case 2:
                OpenGL.glEnableClientState(32884U);
                OpenGL.glDisableClientState(32885U);
                OpenGL.glEnableClientState(32886U);
                OpenGL.glClientActiveTexture(33984U);
                OpenGL.glEnableClientState(32888U);
                nnPutPrimitiveNoTexture();
                break;
            case 3:
                OpenGL.glEnableClientState(32884U);
                OpenGL.glEnableClientState(32885U);
                OpenGL.glDisableClientState(32886U);
                OpenGL.glClientActiveTexture(33984U);
                OpenGL.glEnableClientState(32888U);
                nnPutPrimitiveTexParameter();
                OpenGL.glMatrixMode(5890U);
                OpenGL.glLoadIdentity();
                OpenGL.glTranslatef(0.0f, 1f, 0.0f);
                OpenGL.glScalef(1f, -1f, 1f);
                break;
            case 4:
                OpenGL.glEnableClientState(32884U);
                OpenGL.glDisableClientState(32885U);
                OpenGL.glEnableClientState(32886U);
                OpenGL.glClientActiveTexture(33984U);
                OpenGL.glEnableClientState(32888U);
                nnPutPrimitiveTexParameter();
                OpenGL.glMatrixMode(5890U);
                OpenGL.glLoadIdentity();
                OpenGL.glTranslatef(0.0f, 1f, 0.0f);
                OpenGL.glScalef(1f, -1f, 1f);
                break;
            default:
                OpenGL.glEnableClientState(32884U);
                OpenGL.glDisableClientState(32885U);
                OpenGL.glDisableClientState(32886U);
                nnPutPrimitiveNoTexture();
                break;
        }
        OpenGL.glMatrixMode(5888U);
        Matrix nnsPrim3Dmatrix = (Matrix)nndrawprim3d.nnsPrim3DMatrix;
        OpenGL.glLoadMatrixf(ref nnsPrim3Dmatrix);
    }

    private static void nnDrawPrimitive3DCore(uint mode, object vtx, int count)
    {
        switch (nndrawprim3d.nnsFormat)
        {
            case 0:
                mppOpenGLFeatureNotImplAssert();
                break;
            case 1:
                mppOpenGLFeatureNotImplAssert();
                break;
            case 2:
                NNS_PRIM3D_PC[] data = (NNS_PRIM3D_PC[])vtx;
                int count1 = 0;
                RGBA_U8[] cbuf = _nnDrawPrimitive3DCore.cbuf;
                _nnDrawPrimitive3DCore.colorData.Init(cbuf, 0);
                OpenGL.glColorPointer(4, 5121U, 0, _nnDrawPrimitive3DCore.colorData);
                int index1;
                for (index1 = 0; index1 < count; ++index1)
                {
                    uint col = data[index1].Col;
                    if (count1 >= 6)
                    {
                        OpenGL.glVertexPointer(3, 5126U, 0, new NNS_PRIM3D_PC_VertexData(data, index1 - count1));
                        OpenGL.glDrawArrays(mode, 0, count1);
                        switch (mode)
                        {
                            case 3:
                                cbuf[0] = cbuf[5];
                                count1 = 1;
                                break;
                            case 5:
                                cbuf[0] = cbuf[4];
                                cbuf[1] = cbuf[5];
                                count1 = 2;
                                break;
                            default:
                                count1 = 0;
                                break;
                        }
                    }
                    cbuf[count1].r = (byte)(col >> 24);
                    cbuf[count1].g = (byte)(col >> 16);
                    cbuf[count1].b = (byte)(col >> 8);
                    cbuf[count1].a = (byte)col;
                    ++count1;
                }
                if (count1 <= 0)
                    break;
                _nnDrawPrimitive3DCore.vertexDataPC.Init(data, index1 - count1);
                OpenGL.glVertexPointer(3, 5126U, 0, _nnDrawPrimitive3DCore.vertexDataPC);
                OpenGL.glDrawArrays(mode, 0, count1);
                break;
            case 3:
                mppOpenGLFeatureNotImplAssert();
                break;
            case 4:
                NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = (NNS_PRIM3D_PCT_ARRAY)vtx;
                NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
                int offset = nnsPriM3DPctArray.offset;
                int num = count;
                switch (mode)
                {
                    case 3:
                        mppAssertNotImpl();
                        break;
                    case 5:
                        num = (num - 2) * 3;
                        break;
                }
                if (_nnDrawPrimitive3DCore.prim_d == null || _nnDrawPrimitive3DCore.prim_d.Length < num)
                {
                    _nnDrawPrimitive3DCore.prim_d = new NNS_PRIM3D_PCT[num * 2];
                    _nnDrawPrimitive3DCore.prim_c = new RGBA_U8[num * 2];
                }
                NNS_PRIM3D_PCT[] primD = _nnDrawPrimitive3DCore.prim_d;
                RGBA_U8[] primC = _nnDrawPrimitive3DCore.prim_c;
                int count2 = 0;
                _nnDrawPrimitive3DCore.colorData.Init(primC, 0);
                OpenGL.glColorPointer(4, 5121U, 0, _nnDrawPrimitive3DCore.colorData);
                OpenGL.glClientActiveTexture(33984U);
                for (int index2 = 0; index2 < count; ++index2)
                {
                    uint col = buffer[offset + index2].Col;
                    if (count2 >= 6)
                    {
                        switch (mode)
                        {
                            case 3:
                                primC[count2] = primC[count2 - 1];
                                primD[count2] = primD[count2 - 1];
                                ++count2;
                                break;
                            case 5:
                                primC[count2] = primC[count2 - 2];
                                primC[count2 + 1] = primC[count2 - 1];
                                primD[count2] = primD[count2 - 2];
                                primD[count2 + 1] = primD[count2 - 1];
                                count2 += 2;
                                break;
                        }
                    }
                    primC[count2].r = (byte)(col >> 24);
                    primC[count2].g = (byte)(col >> 16);
                    primC[count2].b = (byte)(col >> 8);
                    primC[count2].a = (byte)col;
                    primD[count2] = buffer[offset + index2];
                    ++count2;
                }
                _nnDrawPrimitive3DCore.vertexData.Init(primD, 0);
                _nnDrawPrimitive3DCore.texCoordData.Init(primD, 0);
                OpenGL.glVertexPointer(3, 5126U, 0, _nnDrawPrimitive3DCore.vertexData);
                OpenGL.glTexCoordPointer(2, 5126U, 0, _nnDrawPrimitive3DCore.texCoordData);
                OpenGL.glDrawArrays(mode, 0, count2);
                break;
        }
    }

    private static void nnEndDrawPrimitive3DCore()
    {
    }

    private static void nnBeginDrawPrimitive3D(int fmt, int blend, int light, int cull)
    {
        nnBeginDrawPrimitive3DCore(fmt, blend, light);
        switch (cull)
        {
            case 0:
                OpenGL.glDisable(2884U);
                break;
            case 1:
                OpenGL.glEnable(2884U);
                OpenGL.glCullFace(1029U);
                break;
            case 2:
                OpenGL.glEnable(2884U);
                OpenGL.glCullFace(1028U);
                break;
        }
    }

    private static void nnDrawPrimitive3D(int type, object vtx, int count)
    {
        uint mode;
        switch (type)
        {
            case 0:
                mode = 4U;
                break;
            default:
                mode = 5U;
                break;
        }
        nnDrawPrimitive3DCore(mode, vtx, count);
    }

    private static void nnEndDrawPrimitive3D()
    {
        nnEndDrawPrimitive3DCore();
    }

    private static void nnBeginDrawPrimitiveLine3D(ref NNS_RGBA col, int blend)
    {
        nnBeginDrawPrimitive3DCore(0, blend, 0);
        OpenGL.glDisable(2896U);
        OpenGL.glColor4fv((OpenGL.glArray4f)col);
    }

    private static void nnDrawPrimitiveLine3D(NNE_PRIM_LINE type, object vtx, int count)
    {
        uint mode;
        switch (type)
        {
            case NNE_PRIM_LINE.NNE_PRIM_LINE_LIST:
                mode = 1U;
                break;
            default:
                mode = 3U;
                break;
        }
        nnDrawPrimitive3DCore(mode, vtx, count);
    }

    private static void nnEndDrawPrimitiveLine3D()
    {
        nnEndDrawPrimitive3DCore();
    }

    private static void nnDrawPrimitivePoint3D(object vtx, int count)
    {
        nnDrawPrimitive3DCore(0U, vtx, count);
    }

    private static void nnEndDrawPrimitivePoint3D()
    {
        nnEndDrawPrimitive3DCore();
    }
}