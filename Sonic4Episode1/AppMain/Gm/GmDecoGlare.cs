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
    public static void GmDecoGlareSetData(AppMain.AMS_AMB_HEADER amb_header)
    {
        AppMain.pIF.Clear();
        string sPath;
        AppMain.pIF.amb_header = AppMain.readAMBFile(AppMain.amBindGet(amb_header, 1, out sPath));
        AppMain.pIF.amb_header.dir = sPath;
        AppMain.TXB_HEADER txb = AppMain.readTXBfile(AppMain.amBindGet(AppMain.pIF.amb_header, 0));
        AppMain.pIF.tex_buf = AppMain.amTxbGetTexFileList(txb);
        AppMain.mppAssertNotImpl();
        AppMain.nnSetUpTexlist(out AppMain.pIF.texlist, AppMain.pIF.tex_buf.nTex, ref AppMain.pIF.texlistbuf);
        AppMain.pIF.regId = AppMain.amTextureLoad(AppMain.pIF.texlist, AppMain.pIF.tex_buf, (string)null, AppMain.pIF.amb_header);
        AppMain.pIF.drawFlag = 1;
        AppMain.pIF.texId = 0;
    }

    public static void GmDecoGlareDraw(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int userWork = (int)obj_work.user_work;
        if (userWork < 14)
            return;
        AppMain.GMS_DECOGLARE_PARAM gmsDecoglareParam;
        switch (userWork)
        {
            case 14:
            case 109:
                gmsDecoglareParam = AppMain._gm_decoGlare_param[0];
                break;
            case 15:
                gmsDecoglareParam = AppMain._gm_decoGlare_param[1];
                break;
            case 16:
            case 110:
                gmsDecoglareParam = AppMain._gm_decoGlare_param[2];
                break;
            case 54:
                gmsDecoglareParam = AppMain._gm_decoGlare_param[3];
                break;
            default:
                return;
        }
        if (!AppMain.amDrawIsRegistComplete(AppMain.pIF.regId) || AppMain.pIF.drawFlag != 1 || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32(obj_work.pos);
        float num1 = (float)(vecFx32.x >> 12);
        float num2 = -(float)(vecFx32.y >> 12);
        float Z = 256f;
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.aTest = (short)0;
        setParam.zMask = (short)0;
        setParam.zTest = (short)1;
        setParam.ablend = 1;
        switch (gmsDecoglareParam.ablend)
        {
            case 0:
                AppMain.amDrawGetPrimBlendParam(0, setParam);
                break;
            case 1:
                AppMain.amDrawGetPrimBlendParam(1, setParam);
                break;
        }
        AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(6);
        AppMain.NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
        int offset = nnsPriM3DPctArray.offset;
        float num3 = gmsDecoglareParam.size * 0.5f;
        float num4 = gmsDecoglareParam.size * 0.5f;
        float num5 = num1 + 10f;
        float num6 = num2 - 10f;
        buffer[offset].Pos.Assign(num5 - num3, num6 + num4, Z);
        buffer[offset + 1].Pos.Assign(num5 + num3, num6 + num4, Z);
        buffer[offset + 2].Pos.Assign(num5 - num3, num6 - num4, Z);
        buffer[offset + 5].Pos.Assign(num5 + num3, num6 - num4, Z);
        buffer[offset].Col = gmsDecoglareParam.color;
        buffer[offset + 1].Col = gmsDecoglareParam.color;
        buffer[offset + 2].Col = gmsDecoglareParam.color;
        buffer[offset + 5].Col = gmsDecoglareParam.color;
        buffer[offset].Tex.u = 0.0f;
        buffer[offset].Tex.v = 0.0f;
        buffer[offset + 1].Tex.u = 1f;
        buffer[offset + 1].Tex.v = 0.0f;
        buffer[offset + 2].Tex.u = 0.0f;
        buffer[offset + 2].Tex.v = 1f;
        buffer[offset + 5].Tex.u = 1f;
        buffer[offset + 5].Tex.v = 1f;
        buffer[offset + 3] = buffer[offset + 1];
        buffer[offset + 4] = buffer[offset + 2];
        setParam.format3D = 4;
        setParam.type = 0;
        setParam.vtxPCT3D = nnsPriM3DPctArray;
        setParam.texlist = AppMain.pIF.texlist;
        setParam.texId = AppMain.pIF.texId;
        setParam.count = 6;
        setParam.sortZ = gmsDecoglareParam.sort_z;
        AppMain.amDrawPrimitive3D(4U, setParam);
        AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    public static void GmDecoGlareDataRelease()
    {
        AppMain.pIF.drawFlag = 2;
        if (AppMain.pIF.texlist != null)
        {
            AppMain.pIF.regId = AppMain.amTextureRelease(AppMain.pIF.texlist);
            AppMain.pIF.texlist = (AppMain.NNS_TEXLIST)null;
        }
        if (AppMain.pIF.texlistbuf == null)
            return;
        AppMain.pIF.texlistbuf = (object)null;
    }

    public static AppMain.GMDECO_GLARE_INTERFACE GmDecoGlareGetGlobal()
    {
        return AppMain.pIF;
    }
}