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
    private int nnGetMaterialIndex(AppMain.NNS_NODENAMELIST pMaterialNameList, string MaterialName)
    {
        return this.nnGetNodeIndex(pMaterialNameList, MaterialName);
    }

    private string nnGetMaterialName(AppMain.NNS_NODENAMELIST pMaterialNameList, int MaterialIndex)
    {
        return this.nnGetNodeName(pMaterialNameList, MaterialIndex);
    }

    private static void nnSetPrimitive3DMaterial(
      ref AppMain.NNS_RGBA diffuse,
      ref AppMain.SNNS_RGB ambient,
      float specular)
    {
        AppMain.nndrawprim3d.nnsDiffuse[0] = diffuse.r;
        AppMain.nndrawprim3d.nnsDiffuse[1] = diffuse.g;
        AppMain.nndrawprim3d.nnsDiffuse[2] = diffuse.b;
        AppMain.nndrawprim3d.nnsDiffuse[3] = diffuse.a;
        AppMain.nndrawprim3d.nnsAmbient[0] = ambient.r;
        AppMain.nndrawprim3d.nnsAmbient[1] = ambient.g;
        AppMain.nndrawprim3d.nnsAmbient[2] = ambient.b;
        AppMain.nndrawprim3d.nnsAmbient[3] = 1f;
        AppMain.nndrawprim3d.nnsSpecular[0] = specular;
        AppMain.nndrawprim3d.nnsSpecular[1] = specular;
        AppMain.nndrawprim3d.nnsSpecular[2] = specular;
        AppMain.nndrawprim3d.nnsSpecular[3] = 1f;
    }

    private static void nnSetPrimitive3DMaterial(
      ref AppMain.NNS_RGBA diffuse,
      ref AppMain.NNS_RGB ambient,
      float specular)
    {
        AppMain.nndrawprim3d.nnsDiffuse[0] = diffuse.r;
        AppMain.nndrawprim3d.nnsDiffuse[1] = diffuse.g;
        AppMain.nndrawprim3d.nnsDiffuse[2] = diffuse.b;
        AppMain.nndrawprim3d.nnsDiffuse[3] = diffuse.a;
        AppMain.nndrawprim3d.nnsAmbient[0] = ambient.r;
        AppMain.nndrawprim3d.nnsAmbient[1] = ambient.g;
        AppMain.nndrawprim3d.nnsAmbient[2] = ambient.b;
        AppMain.nndrawprim3d.nnsAmbient[3] = 1f;
        AppMain.nndrawprim3d.nnsSpecular[0] = specular;
        AppMain.nndrawprim3d.nnsSpecular[1] = specular;
        AppMain.nndrawprim3d.nnsSpecular[2] = specular;
        AppMain.nndrawprim3d.nnsSpecular[3] = 1f;
    }

    private static void nnSetPrimitive3DMaterialGL(
      ref AppMain.NNS_RGBA diffuse,
      ref AppMain.NNS_RGBA ambient,
      ref AppMain.NNS_RGBA specular,
      float shininess,
      ref AppMain.NNS_RGBA emission)
    {
        AppMain.nndrawprim3d.nnsDiffuse[0] = diffuse.r;
        AppMain.nndrawprim3d.nnsDiffuse[1] = diffuse.g;
        AppMain.nndrawprim3d.nnsDiffuse[2] = diffuse.b;
        AppMain.nndrawprim3d.nnsDiffuse[3] = diffuse.a;
        AppMain.nndrawprim3d.nnsAmbient[0] = ambient.r;
        AppMain.nndrawprim3d.nnsAmbient[1] = ambient.g;
        AppMain.nndrawprim3d.nnsAmbient[2] = ambient.b;
        AppMain.nndrawprim3d.nnsAmbient[3] = ambient.a;
        AppMain.nndrawprim3d.nnsSpecular[0] = specular.r;
        AppMain.nndrawprim3d.nnsSpecular[1] = specular.g;
        AppMain.nndrawprim3d.nnsSpecular[2] = specular.b;
        AppMain.nndrawprim3d.nnsSpecular[3] = specular.a;
        AppMain.nndrawprim3d.nnsShininess = shininess;
        AppMain.nndrawprim3d.nnsEmission[0] = emission.r;
        AppMain.nndrawprim3d.nnsEmission[1] = emission.g;
        AppMain.nndrawprim3d.nnsEmission[2] = emission.b;
        AppMain.nndrawprim3d.nnsEmission[3] = emission.a;
    }

    private static void nnSetPrimitive3DMatrix(ref AppMain.SNNS_MATRIX mtx)
    {
        AppMain.nnCopyMatrix(AppMain.nndrawprim3d.nnsPrim3DMatrix, ref mtx);
    }

    private static void nnSetPrimitive3DMatrix(AppMain.NNS_MATRIX mtx)
    {
        AppMain.nnCopyMatrix(AppMain.nndrawprim3d.nnsPrim3DMatrix, mtx);
    }

    private static void nnChangePrimitive3DMatrix(AppMain.NNS_MATRIX mtx)
    {
        OpenGL.glMatrixMode(5888U);
        Matrix matrix = (Matrix)mtx;
        OpenGL.glLoadMatrixf(ref matrix);
        if (AppMain.nngDrawPrimTexCoord != 1)
            return;
        AppMain.nnPutEnvironmentTextureMatrix(mtx);
    }

    private static void nnSetPrimitive3DAlphaFuncGL(uint func, float _ref)
    {
        AppMain.nndrawprim3d.nnsAlphaFunc = func;
        AppMain.nndrawprim3d.nnsAlphaFuncRef = _ref;
    }

    private static void nnSetPrimitive3DDepthFuncGL(uint func)
    {
        AppMain.nndrawprim3d.nnsDepthFunc = func;
    }

    private static void nnSetPrimitive3DDepthMaskGL(bool flag)
    {
        AppMain.nndrawprim3d.nnsDepthMask = flag;
    }

    private static void nnBeginDrawPrimitive3DCore(int fmt, int blend, int light)
    {
        OpenGL.glShadeModel(7425U);
        OpenGL.glLightModelf(2898U, 0.0f);
        OpenGL.glEnable(3008U);
        OpenGL.glAlphaFunc(AppMain.nndrawprim3d.nnsAlphaFunc, AppMain.nndrawprim3d.nnsAlphaFuncRef);
        OpenGL.glEnable(2929U);
        OpenGL.glDepthFunc(AppMain.nndrawprim3d.nnsDepthFunc);
        OpenGL.glDepthMask(AppMain.nndrawprim3d.nnsDepthMask);
        OpenGL.glColorMask((byte)1, (byte)1, (byte)1, (byte)1);
        OpenGL.glBindBuffer(34962U, 0U);
        OpenGL.glBindBuffer(34963U, 0U);
        OpenGL.glDisableClientState(34477U);
        OpenGL.glDisableClientState(34884U);
        if (blend == 1)
        {
            OpenGL.glEnable(3042U);
            switch (AppMain.nngDrawPrimBlend)
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
        AppMain.nnPutFogSwitchGL(AppMain.nngFogSwitch);
        switch (light)
        {
            case 0:
                OpenGL.glDisable(2896U);
                OpenGL.glMaterialfv(1032U, 4609U, (OpenGL.glArray4f)AppMain.nngColorWhite);
                OpenGL.glColor4fv(AppMain.nndrawprim3d.nnsDiffuse);
                break;
            case 1:
                OpenGL.glEnable(2896U);
                OpenGL.glMaterialfv(1032U, 4608U, AppMain.nndrawprim3d.nnsAmbient);
                OpenGL.glMaterialfv(1032U, 4609U, AppMain.nndrawprim3d.nnsDiffuse);
                OpenGL.glMaterialfv(1032U, 4610U, (OpenGL.glArray4f)AppMain.nngColorBlack);
                OpenGL.glMaterialf(1032U, 5633U, 0.0f);
                OpenGL.glMaterialfv(1032U, 5632U, (OpenGL.glArray4f)AppMain.nngColorBlack);
                OpenGL.glColor4fv((OpenGL.glArray4f)AppMain.nngColorWhite);
                break;
            case 2:
                OpenGL.glEnable(2896U);
                OpenGL.glMaterialfv(1032U, 4608U, AppMain.nndrawprim3d.nnsAmbient);
                OpenGL.glMaterialfv(1032U, 4609U, AppMain.nndrawprim3d.nnsDiffuse);
                OpenGL.glMaterialfv(1032U, 4610U, AppMain.nndrawprim3d.nnsSpecular);
                OpenGL.glMaterialf(1032U, 5633U, AppMain.nndrawprim3d.nnsShininess);
                OpenGL.glMaterialfv(1032U, 5632U, AppMain.nndrawprim3d.nnsEmission);
                OpenGL.glColor4fv((OpenGL.glArray4f)AppMain.nngColorWhite);
                break;
        }
        AppMain.nndrawprim3d.nnsFormat = fmt;
        switch (fmt)
        {
            case 1:
                OpenGL.glEnableClientState(32884U);
                OpenGL.glEnableClientState(32885U);
                OpenGL.glDisableClientState(32886U);
                if (AppMain.nngDrawPrimTexCoord == 1)
                {
                    OpenGL.glClientActiveTexture(33984U);
                    OpenGL.glEnableClientState(32888U);
                    AppMain.nnPutPrimitiveTexParameter();
                    AppMain.nnSetTexCoordSrc(0, 3);
                    AppMain.nnSetTexCoordSrc(1, 0);
                    AppMain.nnSetNormalFormatType(5126U);
                    AppMain.nnPutEnvironmentTextureMatrix(AppMain.nndrawprim3d.nnsPrim3DMatrix);
                    break;
                }
                AppMain.nnPutPrimitiveNoTexture();
                break;
            case 2:
                OpenGL.glEnableClientState(32884U);
                OpenGL.glDisableClientState(32885U);
                OpenGL.glEnableClientState(32886U);
                OpenGL.glClientActiveTexture(33984U);
                OpenGL.glEnableClientState(32888U);
                AppMain.nnPutPrimitiveNoTexture();
                break;
            case 3:
                OpenGL.glEnableClientState(32884U);
                OpenGL.glEnableClientState(32885U);
                OpenGL.glDisableClientState(32886U);
                OpenGL.glClientActiveTexture(33984U);
                OpenGL.glEnableClientState(32888U);
                AppMain.nnPutPrimitiveTexParameter();
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
                AppMain.nnPutPrimitiveTexParameter();
                OpenGL.glMatrixMode(5890U);
                OpenGL.glLoadIdentity();
                OpenGL.glTranslatef(0.0f, 1f, 0.0f);
                OpenGL.glScalef(1f, -1f, 1f);
                break;
            default:
                OpenGL.glEnableClientState(32884U);
                OpenGL.glDisableClientState(32885U);
                OpenGL.glDisableClientState(32886U);
                AppMain.nnPutPrimitiveNoTexture();
                break;
        }
        OpenGL.glMatrixMode(5888U);
        Matrix nnsPrim3Dmatrix = (Matrix)AppMain.nndrawprim3d.nnsPrim3DMatrix;
        OpenGL.glLoadMatrixf(ref nnsPrim3Dmatrix);
    }

    private static void nnDrawPrimitive3DCore(uint mode, object vtx, int count)
    {
        switch (AppMain.nndrawprim3d.nnsFormat)
        {
            case 0:
                AppMain.mppOpenGLFeatureNotImplAssert();
                break;
            case 1:
                AppMain.mppOpenGLFeatureNotImplAssert();
                break;
            case 2:
                AppMain.NNS_PRIM3D_PC[] data = (AppMain.NNS_PRIM3D_PC[])vtx;
                int count1 = 0;
                AppMain.RGBA_U8[] cbuf = AppMain._nnDrawPrimitive3DCore.cbuf;
                AppMain._nnDrawPrimitive3DCore.colorData.Init(cbuf, 0);
                OpenGL.glColorPointer(4, 5121U, 0, (OpenGL.GLVertexData)AppMain._nnDrawPrimitive3DCore.colorData);
                int index1;
                for (index1 = 0; index1 < count; ++index1)
                {
                    uint col = data[index1].Col;
                    if (count1 >= 6)
                    {
                        OpenGL.glVertexPointer(3, 5126U, 0, (OpenGL.GLVertexData)new AppMain.NNS_PRIM3D_PC_VertexData(data, index1 - count1));
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
                AppMain._nnDrawPrimitive3DCore.vertexDataPC.Init(data, index1 - count1);
                OpenGL.glVertexPointer(3, 5126U, 0, (OpenGL.GLVertexData)AppMain._nnDrawPrimitive3DCore.vertexDataPC);
                OpenGL.glDrawArrays(mode, 0, count1);
                break;
            case 3:
                AppMain.mppOpenGLFeatureNotImplAssert();
                break;
            case 4:
                AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = (AppMain.NNS_PRIM3D_PCT_ARRAY)vtx;
                AppMain.NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
                int offset = nnsPriM3DPctArray.offset;
                int num = count;
                switch (mode)
                {
                    case 3:
                        AppMain.mppAssertNotImpl();
                        break;
                    case 5:
                        num = (num - 2) * 3;
                        break;
                }
                if (AppMain._nnDrawPrimitive3DCore.prim_d == null || AppMain._nnDrawPrimitive3DCore.prim_d.Length < num)
                {
                    AppMain._nnDrawPrimitive3DCore.prim_d = new AppMain.NNS_PRIM3D_PCT[num * 2];
                    AppMain._nnDrawPrimitive3DCore.prim_c = new AppMain.RGBA_U8[num * 2];
                }
                AppMain.NNS_PRIM3D_PCT[] primD = AppMain._nnDrawPrimitive3DCore.prim_d;
                AppMain.RGBA_U8[] primC = AppMain._nnDrawPrimitive3DCore.prim_c;
                int count2 = 0;
                AppMain._nnDrawPrimitive3DCore.colorData.Init(primC, 0);
                OpenGL.glColorPointer(4, 5121U, 0, (OpenGL.GLVertexData)AppMain._nnDrawPrimitive3DCore.colorData);
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
                AppMain._nnDrawPrimitive3DCore.vertexData.Init(primD, 0);
                AppMain._nnDrawPrimitive3DCore.texCoordData.Init(primD, 0);
                OpenGL.glVertexPointer(3, 5126U, 0, (OpenGL.GLVertexData)AppMain._nnDrawPrimitive3DCore.vertexData);
                OpenGL.glTexCoordPointer(2, 5126U, 0, (OpenGL.GLVertexData)AppMain._nnDrawPrimitive3DCore.texCoordData);
                OpenGL.glDrawArrays(mode, 0, count2);
                break;
        }
    }

    private static void nnEndDrawPrimitive3DCore()
    {
    }

    private static void nnBeginDrawPrimitive3D(int fmt, int blend, int light, int cull)
    {
        AppMain.nnBeginDrawPrimitive3DCore(fmt, blend, light);
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
        AppMain.nnDrawPrimitive3DCore(mode, vtx, count);
    }

    private static void nnEndDrawPrimitive3D()
    {
        AppMain.nnEndDrawPrimitive3DCore();
    }

    private static void nnBeginDrawPrimitiveLine3D(ref AppMain.NNS_RGBA col, int blend)
    {
        AppMain.nnBeginDrawPrimitive3DCore(0, blend, 0);
        OpenGL.glDisable(2896U);
        OpenGL.glColor4fv((OpenGL.glArray4f)col);
    }

    private static void nnDrawPrimitiveLine3D(AppMain.NNE_PRIM_LINE type, object vtx, int count)
    {
        uint mode;
        switch (type)
        {
            case AppMain.NNE_PRIM_LINE.NNE_PRIM_LINE_LIST:
                mode = 1U;
                break;
            default:
                mode = 3U;
                break;
        }
        AppMain.nnDrawPrimitive3DCore(mode, vtx, count);
    }

    private static void nnEndDrawPrimitiveLine3D()
    {
        AppMain.nnEndDrawPrimitive3DCore();
    }

    private static void nnDrawPrimitivePoint3D(object vtx, int count)
    {
        AppMain.nnDrawPrimitive3DCore(0U, vtx, count);
    }

    private static void nnEndDrawPrimitivePoint3D()
    {
        AppMain.nnEndDrawPrimitive3DCore();
    }
}