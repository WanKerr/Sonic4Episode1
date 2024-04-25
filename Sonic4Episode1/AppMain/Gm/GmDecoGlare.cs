public partial class AppMain
{
    public static void GmDecoGlareSetData(AMS_AMB_HEADER amb_header)
    {
        pIF.Clear();
        string sPath;
        pIF.amb_header = readAMBFile(amBindGet(amb_header, 1, out sPath));
        pIF.amb_header.dir = sPath;
        TXB_HEADER txb = readTXBfile(amBindGet(pIF.amb_header, 0));
        pIF.tex_buf = amTxbGetTexFileList(txb);
        mppAssertNotImpl();
        nnSetUpTexlist(out pIF.texlist, pIF.tex_buf.nTex, ref pIF.texlistbuf);
        pIF.regId = amTextureLoad(pIF.texlist, pIF.tex_buf, null, pIF.amb_header);
        pIF.drawFlag = 1;
        pIF.texId = 0;
    }

    public static void GmDecoGlareDraw(OBS_OBJECT_WORK obj_work)
    {
        int userWork = (int)obj_work.user_work;
        if (userWork < 14)
            return;
        GMS_DECOGLARE_PARAM gmsDecoglareParam;
        switch (userWork)
        {
            case 14:
            case 109:
                gmsDecoglareParam = _gm_decoGlare_param[0];
                break;
            case 15:
                gmsDecoglareParam = _gm_decoGlare_param[1];
                break;
            case 16:
            case 110:
                gmsDecoglareParam = _gm_decoGlare_param[2];
                break;
            case 54:
                gmsDecoglareParam = _gm_decoGlare_param[3];
                break;
            default:
                return;
        }
        if (!amDrawIsRegistComplete(pIF.regId) || pIF.drawFlag != 1 || ((int)obj_work.disp_flag & 32) != 0)
            return;
        VecFx32 vecFx32 = new VecFx32(obj_work.pos);
        float num1 = vecFx32.x >> 12;
        float num2 = -(vecFx32.y >> 12);
        float Z = 256f;
        AMS_PARAM_DRAW_PRIMITIVE setParam = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.aTest = 0;
        setParam.zMask = 0;
        setParam.zTest = 1;
        setParam.ablend = 1;
        switch (gmsDecoglareParam.ablend)
        {
            case 0:
                amDrawGetPrimBlendParam(0, setParam);
                break;
            case 1:
                amDrawGetPrimBlendParam(1, setParam);
                break;
        }
        NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = amDrawAlloc_NNS_PRIM3D_PCT(6);
        NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
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
        setParam.texlist = pIF.texlist;
        setParam.texId = pIF.texId;
        setParam.count = 6;
        setParam.sortZ = gmsDecoglareParam.sort_z;
        amDrawPrimitive3D(4U, setParam);
        GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    public static void GmDecoGlareDataRelease()
    {
        pIF.drawFlag = 2;
        if (pIF.texlist != null)
        {
            pIF.regId = amTextureRelease(pIF.texlist);
            pIF.texlist = null;
        }
        if (pIF.texlistbuf == null)
            return;
        pIF.texlistbuf = null;
    }

    public static GMDECO_GLARE_INTERFACE GmDecoGlareGetGlobal()
    {
        return pIF;
    }
}