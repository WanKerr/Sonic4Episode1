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
    public static void AoTexBuild(AppMain.AOS_TEXTURE tex, AppMain.AMS_AMB_HEADER amb)
    {
        if (tex == null || amb == null)
            return;
        AppMain.aoTexInitTex(tex);
        tex.amb = amb;
        AppMain.AmbChunk ambChunk = AppMain.amBindSearchEx(amb, ".txb");
        tex.txb = AppMain.readTXBfile(ambChunk.array, ambChunk.offset, amb.dir);
        AppMain.TXB_HEADER txb = tex.txb;
    }

    public static void AoTexLoad(AppMain.AOS_TEXTURE tex)
    {
        if (tex == null || tex.txb == null || (tex.amb == null || tex.reg_id >= 0))
            return;
        uint count = AppMain.amTxbGetCount(tex.txb);
        AppMain.nnSetUpTexlist(out tex.texlist, (int)count, ref tex.texlist_buf);
        AppMain.NNS_TEXFILELIST texFileList = AppMain.amTxbGetTexFileList(tex.txb);
        tex.reg_id = AppMain.amTextureLoad(tex.texlist, texFileList, (string)null, tex.amb);
    }

    public static bool AoTexIsLoaded(AppMain.AOS_TEXTURE tex)
    {
        if (tex == null || tex.texlist == null)
            return false;
        if (tex.reg_id >= 0 && AppMain.amDrawIsRegistComplete(tex.reg_id))
            tex.reg_id = -1;
        return tex.reg_id < 0;
    }

    public static AppMain.NNS_TEXLIST AoTexGetTexList(AppMain.AOS_TEXTURE tex)
    {
        return tex == null || tex.texlist == null || tex.reg_id >= 0 ? (AppMain.NNS_TEXLIST)null : tex.texlist;
    }

    public AppMain.TXB_HEADER AoTexGetTxb(ref AppMain.AOS_TEXTURE tex)
    {
        return tex == null || tex.texlist == null || tex.reg_id >= 0 ? (AppMain.TXB_HEADER)null : tex.txb;
    }

    private static void AoTexRelease(AppMain.AOS_TEXTURE tex)
    {
        if (!AppMain.AoTexIsLoaded(tex))
            return;
        tex.reg_id = AppMain.amTextureRelease(tex.texlist);
        tex.texlist = (AppMain.NNS_TEXLIST)null;
    }

    private static bool AoTexIsReleased(AppMain.AOS_TEXTURE tex)
    {
        if (tex == null || tex.reg_id < 0)
            return true;
        if (!AppMain.amDrawIsRegistComplete(tex.reg_id))
            return false;
        AppMain.aoTexInitTex(tex);
        return true;
    }

    private static void aoTexInitTex(AppMain.AOS_TEXTURE tex)
    {
        tex.texlist = (AppMain.NNS_TEXLIST)null;
        tex.reg_id = -1;
        tex.amb = (AppMain.AMS_AMB_HEADER)null;
        tex.txb = (AppMain.TXB_HEADER)null;
    }


}