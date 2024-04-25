using Microsoft.Xna.Framework;
using mpp;

public partial class AppMain
{
    private static void nnSetPrimitive2DAlphaFuncGL(uint func, float _ref)
    {
        nndrawprim2d.nnsAlphaFunc = func;
        nndrawprim2d.nnsAlphaFuncRef = _ref;
    }

    private static void nnSetPrimitive2DDepthFuncGL(uint func)
    {
        nndrawprim2d.nnsDepthFunc = func;
    }

    private static void nnSetPrimitive2DDepthMaskGL(bool flag)
    {
        nndrawprim2d.nnsDepthMask = flag;
    }

    private static void nnConvert2DTo3D(NNS_VECTOR p3D, float x_2D, float y_2D, float z_3D)
    {
        float num1 = (float)((x_2D - (double)nngScreen.cx) * 2.0) / nngScreen.w;
        float num2 = (float)((nngScreen.cy - (double)y_2D) * 2.0) / nngScreen.h;
        float num3 = nngProjectionMatrix.M32 * z_3D + nngProjectionMatrix.M33;
        float num4 = num1 * num3;
        float num5 = num2 * num3;
        float num6 = (num4 - nngProjectionMatrix.M02 * z_3D - nngProjectionMatrix.M03) / nngProjectionMatrix.M00;
        float num7 = (num5 - nngProjectionMatrix.M12 * z_3D - nngProjectionMatrix.M13) / nngProjectionMatrix.M11;
        p3D.x = num6;
        p3D.y = num7;
        p3D.z = z_3D;
    }

    private static void nnConvert2DTo3D(ref Vector3 p3D, float x_2D, float y_2D, float z_3D)
    {
        float num1 = (float)((x_2D - (double)nngScreen.cx) * 2.0) / nngScreen.w;
        float num2 = (float)((nngScreen.cy - (double)y_2D) * 2.0) / nngScreen.h;
        float num3 = nngProjectionMatrix.M32 * z_3D + nngProjectionMatrix.M33;
        float num4 = num1 * num3;
        float num5 = num2 * num3;
        float num6 = (num4 - nngProjectionMatrix.M02 * z_3D - nngProjectionMatrix.M03) / nngProjectionMatrix.M00;
        float num7 = (num5 - nngProjectionMatrix.M12 * z_3D - nngProjectionMatrix.M13) / nngProjectionMatrix.M11;
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
        OpenGL.glAlphaFunc(nndrawprim2d.nnsAlphaFunc, nndrawprim2d.nnsAlphaFuncRef);
        OpenGL.glEnable(2929U);
        OpenGL.glDepthFunc(nndrawprim2d.nnsDepthFunc);
        OpenGL.glDepthMask(nndrawprim2d.nnsDepthMask);
        OpenGL.glColorMask(1, 1, 1, 1);
        OpenGL.glBindBuffer(34962U, 0U);
        OpenGL.glBindBuffer(34963U, 0U);
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
        OpenGL.glMaterialfv(1032U, 4609U, (OpenGL.glArray4f)nngColorWhite);
        nndrawprim2d.nnsFormat = fmt;
        OpenGL.glDisableClientState(32885U);
        OpenGL.glDisableClientState(34477U);
        OpenGL.glDisableClientState(34884U);
        switch (fmt)
        {
            case 1:
                nnPutPrimitiveNoTexture();
                OpenGL.glEnableClientState(32884U);
                OpenGL.glEnableClientState(32886U);
                break;
            case 2:
                if (nngDrawPrimTexture != 0)
                {
                    nnPutPrimitiveTexParameter();
                    OpenGL.glMatrixMode(5890U);
                    OpenGL.glLoadIdentity();
                    OpenGL.glTranslatef(0.0f, 1f, 0.0f);
                    OpenGL.glScalef(1f, -1f, 1f);
                    OpenGL.glClientActiveTexture(33984U);
                    OpenGL.glEnableClientState(32888U);
                }
                else
                    nnPutPrimitiveNoTexture();
                OpenGL.glEnableClientState(32884U);
                OpenGL.glEnableClientState(32886U);
                break;
            default:
                nnPutPrimitiveNoTexture();
                OpenGL.glEnableClientState(32884U);
                OpenGL.glDisableClientState(32886U);
                break;
        }
        OpenGL.glMatrixMode(5888U);
        OpenGL.glLoadIdentity();
    }

    private static void nnDrawPrimitive2DCore(uint mode, object vtx, int count, float pri)
    {
        if (pri > -(double)nngClip2d.n_clip || -nngClip2d.f_clip > (double)pri)
            return;
        switch (nndrawprim2d.nnsFormat)
        {
            case 0:
                mppAssertNotImpl();
                break;
            case 1:
                NNS_PRIM2D_PC[] nnsPriM2DPcArray = (NNS_PRIM2D_PC[])vtx;
                Vector3[] vbuf = _nnDrawPrimitive2DCore.vbuf;
                RGBA_U8[] cbuf = _nnDrawPrimitive2DCore.cbuf;
                int count1 = 0;
                OpenGL.glVertexPointer(3, 5126U, 0, new Vector3_VertexData(vbuf));
                OpenGL.glColorPointer(4, 5121U, 0, new RGBA_U8_ColorData(cbuf, 0));
                for (int index = 0; index < count; ++index)
                {
                    NNS_VECTOR2D pos = nnsPriM2DPcArray[index].Pos;
                    uint col = nnsPriM2DPcArray[index].Col;
                    nnConvert2DTo3D(ref vbuf[count1], pos.x, pos.y, pri);
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
                mppAssertNotImpl();
                break;
        }
    }

    private static void nnEndDrawPrimitive2DCore()
    {
    }

    private static void nnBeginDrawPrimitive2D(int fmt, int blend)
    {
        nnBeginDrawPrimitive2DCore(fmt, blend);
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
        nnDrawPrimitive2DCore(mode, vtx, count, pri);
    }

    private static void nnEndDrawPrimitive2D()
    {
        nnEndDrawPrimitive2DCore();
    }

    private static void nnBeginDrawPrimitiveLine2D(ref NNS_RGBA col, int blend)
    {
        nnBeginDrawPrimitive2DCore(0, blend);
        OpenGL.glColor4fv((OpenGL.glArray4f)col);
    }

    private static void nnDrawPrimitiveLine2D(
      NNE_PRIM_LINE type,
      object vtx,
      int count,
      float pri)
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
        nnDrawPrimitive2DCore(mode, vtx, count, pri);
    }

    private static void nnEndDrawPrimitiveLine2D()
    {
        nnEndDrawPrimitive2DCore();
    }

    private static void nnDrawPrimitivePoint2D(object vtx, int count, float pri)
    {
        nnDrawPrimitive2DCore(0U, vtx, count, pri);
    }

    private static void nnEndDrawPrimitivePoint2D()
    {
        nnEndDrawPrimitive2DCore();
    }
}