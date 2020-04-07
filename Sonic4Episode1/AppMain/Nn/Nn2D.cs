using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static void nnSetPrimitive2DAlphaFuncGL(uint func, float _ref)
    {
        AppMain.nndrawprim2d.nnsAlphaFunc = func;
        AppMain.nndrawprim2d.nnsAlphaFuncRef = _ref;
    }

    private static void nnSetPrimitive2DDepthFuncGL(uint func)
    {
        AppMain.nndrawprim2d.nnsDepthFunc = func;
    }

    private static void nnSetPrimitive2DDepthMaskGL(bool flag)
    {
        AppMain.nndrawprim2d.nnsDepthMask = flag;
    }

    private static void nnConvert2DTo3D(AppMain.NNS_VECTOR p3D, float x_2D, float y_2D, float z_3D)
    {
        float num1 = (float)(((double)x_2D - (double)AppMain.nngScreen.cx) * 2.0) / AppMain.nngScreen.w;
        float num2 = (float)(((double)AppMain.nngScreen.cy - (double)y_2D) * 2.0) / AppMain.nngScreen.h;
        float num3 = AppMain.nngProjectionMatrix.M32 * z_3D + AppMain.nngProjectionMatrix.M33;
        float num4 = num1 * num3;
        float num5 = num2 * num3;
        float num6 = (num4 - AppMain.nngProjectionMatrix.M02 * z_3D - AppMain.nngProjectionMatrix.M03) / AppMain.nngProjectionMatrix.M00;
        float num7 = (num5 - AppMain.nngProjectionMatrix.M12 * z_3D - AppMain.nngProjectionMatrix.M13) / AppMain.nngProjectionMatrix.M11;
        p3D.x = num6;
        p3D.y = num7;
        p3D.z = z_3D;
    }

    private static void nnConvert2DTo3D(ref Vector3 p3D, float x_2D, float y_2D, float z_3D)
    {
        float num1 = (float)(((double)x_2D - (double)AppMain.nngScreen.cx) * 2.0) / AppMain.nngScreen.w;
        float num2 = (float)(((double)AppMain.nngScreen.cy - (double)y_2D) * 2.0) / AppMain.nngScreen.h;
        float num3 = AppMain.nngProjectionMatrix.M32 * z_3D + AppMain.nngProjectionMatrix.M33;
        float num4 = num1 * num3;
        float num5 = num2 * num3;
        float num6 = (num4 - AppMain.nngProjectionMatrix.M02 * z_3D - AppMain.nngProjectionMatrix.M03) / AppMain.nngProjectionMatrix.M00;
        float num7 = (num5 - AppMain.nngProjectionMatrix.M12 * z_3D - AppMain.nngProjectionMatrix.M13) / AppMain.nngProjectionMatrix.M11;
        p3D.X = num6;
        p3D.Y = num7;
        p3D.Z = z_3D;
    }

    private static void nnBeginDrawPrimitive2DCore(int fmt, int blend)
    {
        OpenGL.glShadeModel(7424U);
        OpenGL.glDisable(2884U);
        OpenGL.glLightModelf(2898U, 0.0f);
        OpenGL.glDisable(2896U);
        OpenGL.glEnable(3008U);
        OpenGL.glAlphaFunc(AppMain.nndrawprim2d.nnsAlphaFunc, AppMain.nndrawprim2d.nnsAlphaFuncRef);
        OpenGL.glEnable(2929U);
        OpenGL.glDepthFunc(AppMain.nndrawprim2d.nnsDepthFunc);
        OpenGL.glDepthMask(AppMain.nndrawprim2d.nnsDepthMask);
        OpenGL.glColorMask((byte)1, (byte)1, (byte)1, (byte)1);
        OpenGL.glBindBuffer(34962U, 0U);
        OpenGL.glBindBuffer(34963U, 0U);
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
        OpenGL.glMaterialfv(1032U, 4609U, (OpenGL.glArray4f)AppMain.nngColorWhite);
        AppMain.nndrawprim2d.nnsFormat = fmt;
        OpenGL.glDisableClientState(32885U);
        OpenGL.glDisableClientState(34477U);
        OpenGL.glDisableClientState(34884U);
        switch (fmt)
        {
            case 1:
                AppMain.nnPutPrimitiveNoTexture();
                OpenGL.glEnableClientState(32884U);
                OpenGL.glEnableClientState(32886U);
                break;
            case 2:
                if (AppMain.nngDrawPrimTexture != 0)
                {
                    AppMain.nnPutPrimitiveTexParameter();
                    OpenGL.glMatrixMode(5890U);
                    OpenGL.glLoadIdentity();
                    OpenGL.glTranslatef(0.0f, 1f, 0.0f);
                    OpenGL.glScalef(1f, -1f, 1f);
                    OpenGL.glClientActiveTexture(33984U);
                    OpenGL.glEnableClientState(32888U);
                }
                else
                    AppMain.nnPutPrimitiveNoTexture();
                OpenGL.glEnableClientState(32884U);
                OpenGL.glEnableClientState(32886U);
                break;
            default:
                AppMain.nnPutPrimitiveNoTexture();
                OpenGL.glEnableClientState(32884U);
                OpenGL.glDisableClientState(32886U);
                break;
        }
        OpenGL.glMatrixMode(5888U);
        OpenGL.glLoadIdentity();
    }

    private static void nnDrawPrimitive2DCore(uint mode, object vtx, int count, float pri)
    {
        if ((double)pri > -(double)AppMain.nngClip2d.n_clip || -(double)AppMain.nngClip2d.f_clip > (double)pri)
            return;
        switch (AppMain.nndrawprim2d.nnsFormat)
        {
            case 0:
                AppMain.mppAssertNotImpl();
                break;
            case 1:
                AppMain.NNS_PRIM2D_PC[] nnsPriM2DPcArray = (AppMain.NNS_PRIM2D_PC[])vtx;
                Vector3[] vbuf = AppMain._nnDrawPrimitive2DCore.vbuf;
                AppMain.RGBA_U8[] cbuf = AppMain._nnDrawPrimitive2DCore.cbuf;
                int count1 = 0;
                OpenGL.glVertexPointer(3, 5126U, 0, (OpenGL.GLVertexData)new AppMain.Vector3_VertexData(vbuf));
                OpenGL.glColorPointer(4, 5121U, 0, (OpenGL.GLVertexData)new AppMain.RGBA_U8_ColorData(cbuf, 0));
                for (int index = 0; index < count; ++index)
                {
                    AppMain.NNS_VECTOR2D pos = nnsPriM2DPcArray[index].Pos;
                    uint col = nnsPriM2DPcArray[index].Col;
                    AppMain.nnConvert2DTo3D(ref vbuf[count1], pos.x, pos.y, pri);
                    cbuf[count1].r = (byte)(col >> 24);
                    cbuf[count1].g = (byte)(col >> 16);
                    cbuf[count1].b = (byte)(col >> 8);
                    cbuf[count1].a = (byte)col;
                    ++count1;
                    if (count1 >= 6)
                    {
                        OpenGL.glDrawArrays(mode, 0, count1);
                        switch (mode)
                        {
                            case 3:
                                vbuf[0] = vbuf[5];
                                cbuf[0] = cbuf[5];
                                count1 = 1;
                                continue;
                            case 5:
                                vbuf[0] = vbuf[4];
                                vbuf[1] = vbuf[5];
                                cbuf[0] = cbuf[4];
                                cbuf[1] = cbuf[5];
                                count1 = 2;
                                continue;
                            default:
                                count1 = 0;
                                continue;
                        }
                    }
                }
                if (count1 <= 0)
                    break;
                OpenGL.glDrawArrays(mode, 0, count1);
                break;
            case 2:
                AppMain.mppAssertNotImpl();
                break;
        }
    }

    private static void nnEndDrawPrimitive2DCore()
    {
    }

    private static void nnBeginDrawPrimitive2D(int fmt, int blend)
    {
        AppMain.nnBeginDrawPrimitive2DCore(fmt, blend);
    }

    private static void nnDrawPrimitive2D(int type, object vtx, int count, float pri)
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
        AppMain.nnDrawPrimitive2DCore(mode, vtx, count, pri);
    }

    private static void nnEndDrawPrimitive2D()
    {
        AppMain.nnEndDrawPrimitive2DCore();
    }

    private static void nnBeginDrawPrimitiveLine2D(ref AppMain.NNS_RGBA col, int blend)
    {
        AppMain.nnBeginDrawPrimitive2DCore(0, blend);
        OpenGL.glColor4fv((OpenGL.glArray4f)col);
    }

    private static void nnDrawPrimitiveLine2D(
      AppMain.NNE_PRIM_LINE type,
      object vtx,
      int count,
      float pri)
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
        AppMain.nnDrawPrimitive2DCore(mode, vtx, count, pri);
    }

    private static void nnEndDrawPrimitiveLine2D()
    {
        AppMain.nnEndDrawPrimitive2DCore();
    }

    private static void nnDrawPrimitivePoint2D(object vtx, int count, float pri)
    {
        AppMain.nnDrawPrimitive2DCore(0U, vtx, count, pri);
    }

    private static void nnEndDrawPrimitivePoint2D()
    {
        AppMain.nnEndDrawPrimitive2DCore();
    }
}