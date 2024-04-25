public partial class AppMain
{
    public static void AoTexBuild(AOS_TEXTURE tex, AMS_AMB_HEADER amb)
    {
        if (tex == null || amb == null)
            return;
        aoTexInitTex(tex);
        tex.amb = amb;
        AmbChunk ambChunk = amBindSearchEx(amb, ".txb");
        tex.txb = readTXBfile(ambChunk.array, ambChunk.offset, amb.dir);
        TXB_HEADER txb = tex.txb;
    }

    public static void AoTexLoad(AOS_TEXTURE tex)
    {
        if (tex == null || tex.txb == null || (tex.amb == null || tex.reg_id >= 0))
            return;
        uint count = amTxbGetCount(tex.txb);
        nnSetUpTexlist(out tex.texlist, (int)count, ref tex.texlist_buf);
        NNS_TEXFILELIST texFileList = amTxbGetTexFileList(tex.txb);
        tex.reg_id = amTextureLoad(tex.texlist, texFileList, null, tex.amb);
    }

    public static bool AoTexIsLoaded(AOS_TEXTURE tex)
    {
        if (tex == null || tex.texlist == null)
            return false;
        if (tex.reg_id >= 0 && amDrawIsRegistComplete(tex.reg_id))
            tex.reg_id = -1;
        return tex.reg_id < 0;
    }

    public static NNS_TEXLIST AoTexGetTexList(AOS_TEXTURE tex)
    {
        return tex == null || tex.texlist == null || tex.reg_id >= 0 ? null : tex.texlist;
    }

    public TXB_HEADER AoTexGetTxb(ref AOS_TEXTURE tex)
    {
        return tex == null || tex.texlist == null || tex.reg_id >= 0 ? null : tex.txb;
    }

    private static void AoTexRelease(AOS_TEXTURE tex)
    {
        if (!AoTexIsLoaded(tex))
            return;
        tex.reg_id = amTextureRelease(tex.texlist);
        tex.texlist = null;
    }

    private static bool AoTexIsReleased(AOS_TEXTURE tex)
    {
        if (tex == null || tex.reg_id < 0)
            return true;
        if (!amDrawIsRegistComplete(tex.reg_id))
            return false;
        aoTexInitTex(tex);
        return true;
    }

    private static void aoTexInitTex(AOS_TEXTURE tex)
    {
        tex.texlist = null;
        tex.reg_id = -1;
        tex.amb = null;
        tex.txb = null;
    }


}