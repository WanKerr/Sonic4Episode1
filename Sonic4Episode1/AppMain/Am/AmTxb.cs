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
    public static AppMain.TXB_HEADER readTXBfile(object data)
    {
        AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)data;
        return AppMain.readTXBfile(ambChunk.array, ambChunk.offset);
    }

    public static AppMain.TXB_HEADER readTXBfile(byte[] data, int offset)
    {
        return AppMain.readTXBfile(data, offset, (string)null);
    }

    public static AppMain.TXB_HEADER readTXBfile(byte[] data, int offset, string sPath)
    {
        AppMain.TXB_HEADER txbHeader = (AppMain.TXB_HEADER)null;
        if (data != null)
        {
            using (MemoryStream memoryStream = new MemoryStream(data, offset, data.Length - offset))
            {
                using (BinaryReader br = new BinaryReader((Stream)memoryStream))
                    txbHeader = AppMain.readTXBfile(br, sPath);
            }
        }
        return txbHeader;
    }

    public static AppMain.TXB_HEADER readTXBfile(BinaryReader br)
    {
        return AppMain.readTXBfile(br, (string)null);
    }

    public static AppMain.TXB_HEADER readTXBfile(BinaryReader br, string sPath)
    {
        AppMain.TXB_HEADER txbHeader = new AppMain.TXB_HEADER();
        txbHeader.file_id = br.ReadBytes(4);
        txbHeader.texfilelist_offset = AppMain._amConvertEndian(br.ReadInt32());
        txbHeader.pad = br.ReadBytes(8);
        br.BaseStream.Position = (long)txbHeader.texfilelist_offset;
        txbHeader.texfilelist.nTex = AppMain._amConvertEndian(br.ReadInt32());
        txbHeader.texfilelist.tex_list_offset = AppMain._amConvertEndian(br.ReadInt32());
        if (txbHeader.texfilelist.tex_list_offset != 0)
        {
            br.BaseStream.Position = (long)txbHeader.texfilelist.tex_list_offset;
            txbHeader.texfilelist.pTexFileList = (AppMain.NNS_TEXFILE[])new AppMain.TXB_TEXFILE[txbHeader.texfilelist.nTex];
            for (int index = 0; index < txbHeader.texfilelist.nTex; ++index)
            {
                txbHeader.texfilelist.pTexFileList[index] = (AppMain.NNS_TEXFILE)new AppMain.TXB_TEXFILE();
                txbHeader.texfilelist.pTexFileList[index].fType = AppMain._amConvertEndian(br.ReadUInt32());
                ((AppMain.TXB_TEXFILE)txbHeader.texfilelist.pTexFileList[index]).name_offset = AppMain._amConvertEndian(br.ReadInt32());
                txbHeader.texfilelist.pTexFileList[index].MinFilter = AppMain._amConvertEndian(br.ReadUInt16());
                txbHeader.texfilelist.pTexFileList[index].MagFilter = AppMain._amConvertEndian(br.ReadUInt16());
                txbHeader.texfilelist.pTexFileList[index].GlobalIndex = AppMain._amConvertEndian(br.ReadUInt32());
                txbHeader.texfilelist.pTexFileList[index].Bank = AppMain._amConvertEndian(br.ReadUInt32());
            }
            for (int index = 0; index < txbHeader.texfilelist.nTex; ++index)
            {
                br.BaseStream.Position = (long)((AppMain.TXB_TEXFILE)txbHeader.texfilelist.pTexFileList[index]).name_offset;
                txbHeader.texfilelist.pTexFileList[index].Filename = AppMain.readChars(br);
                string str = txbHeader.texfilelist.pTexFileList[index].Filename.Replace(".PVR", ".PNG");
                txbHeader.texfilelist.pTexFileList[index].Filename = str;
            }
        }
        return txbHeader;
    }

    private static AppMain.NNS_TEXFILELIST amTxbGetTexFileList(AppMain.TXB_HEADER txb)
    {
        return (AppMain.NNS_TEXFILELIST)txb.texfilelist;
    }

    private static uint amTxbGetCount(AppMain.TXB_HEADER txb)
    {
        return txb == null ? 0U : (uint)txb.texfilelist.nTex;
    }

    private ushort amTxbGetMagFilter(AppMain.TXB_HEADER txb, uint tex_no)
    {
        AppMain.mppAssertNotImpl();
        return tex_no >= AppMain.amTxbGetCount(txb) ? AppMain.g_txb_mag_filter[0] : txb.texfilelist.pTexFileList[(int)tex_no].MagFilter;
    }

    private ushort amTxbGetMinFilter(AppMain.TXB_HEADER txb, uint tex_no)
    {
        AppMain.mppAssertNotImpl();
        return tex_no >= AppMain.amTxbGetCount(txb) ? AppMain.g_txb_min_filter[0] : txb.texfilelist.pTexFileList[(int)tex_no].MinFilter;
    }

    private string amTxbGetName(AppMain.TXB_HEADER txb, uint tex_no)
    {
        AppMain.mppAssertNotImpl();
        if (tex_no >= AppMain.amTxbGetCount(txb))
            tex_no = 0U;
        return txb.texfilelist.pTexFileList[(int)tex_no].Filename;
    }

    private static int amTxbConv(AppMain.TXB_HEADER header)
    {
        return 0;
    }

    private static int amTxbConv(byte[] stream)
    {
        AppMain.mppAssertNotImpl();
        return 1;
    }
}