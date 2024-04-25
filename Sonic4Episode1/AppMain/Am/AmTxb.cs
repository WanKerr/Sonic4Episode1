using System.IO;

public partial class AppMain
{
    public static TXB_HEADER readTXBfile(object data)
    {
        AmbChunk ambChunk = (AmbChunk)data;
        return readTXBfile(ambChunk.array, ambChunk.offset);
    }

    public static TXB_HEADER readTXBfile(byte[] data, int offset)
    {
        return readTXBfile(data, offset, null);
    }

    public static TXB_HEADER readTXBfile(byte[] data, int offset, string sPath)
    {
        TXB_HEADER txbHeader = null;
        if (data != null)
        {
            using (MemoryStream memoryStream = new MemoryStream(data, offset, data.Length - offset))
            {
                using (BinaryReader br = new BinaryReader(memoryStream))
                    txbHeader = readTXBfile(br, sPath);
            }
        }
        return txbHeader;
    }

    public static TXB_HEADER readTXBfile(BinaryReader br)
    {
        return readTXBfile(br, null);
    }

    public static TXB_HEADER readTXBfile(BinaryReader br, string sPath)
    {
        TXB_HEADER txbHeader = new TXB_HEADER();
        txbHeader.file_id = br.ReadBytes(4);
        txbHeader.texfilelist_offset = _amConvertEndian(br.ReadInt32());
        txbHeader.pad = br.ReadBytes(8);
        br.BaseStream.Position = txbHeader.texfilelist_offset;
        txbHeader.texfilelist.nTex = _amConvertEndian(br.ReadInt32());
        txbHeader.texfilelist.tex_list_offset = _amConvertEndian(br.ReadInt32());
        if (txbHeader.texfilelist.tex_list_offset != 0)
        {
            br.BaseStream.Position = txbHeader.texfilelist.tex_list_offset;
            txbHeader.texfilelist.pTexFileList = (new TXB_TEXFILE[txbHeader.texfilelist.nTex]);
            for (int index = 0; index < txbHeader.texfilelist.nTex; ++index)
            {
                txbHeader.texfilelist.pTexFileList[index] = new TXB_TEXFILE();
                txbHeader.texfilelist.pTexFileList[index].fType = _amConvertEndian(br.ReadUInt32());
                ((TXB_TEXFILE)txbHeader.texfilelist.pTexFileList[index]).name_offset = _amConvertEndian(br.ReadInt32());
                txbHeader.texfilelist.pTexFileList[index].MinFilter = _amConvertEndian(br.ReadUInt16());
                txbHeader.texfilelist.pTexFileList[index].MagFilter = _amConvertEndian(br.ReadUInt16());
                txbHeader.texfilelist.pTexFileList[index].GlobalIndex = _amConvertEndian(br.ReadUInt32());
                txbHeader.texfilelist.pTexFileList[index].Bank = _amConvertEndian(br.ReadUInt32());
            }
            for (int index = 0; index < txbHeader.texfilelist.nTex; ++index)
            {
                br.BaseStream.Position = ((TXB_TEXFILE)txbHeader.texfilelist.pTexFileList[index]).name_offset;
                txbHeader.texfilelist.pTexFileList[index].Filename = readChars(br);
                string str = txbHeader.texfilelist.pTexFileList[index].Filename.Replace(".PVR", ".PNG");
                txbHeader.texfilelist.pTexFileList[index].Filename = str;
            }
        }
        return txbHeader;
    }

    private static NNS_TEXFILELIST amTxbGetTexFileList(TXB_HEADER txb)
    {
        return txb.texfilelist;
    }

    private static uint amTxbGetCount(TXB_HEADER txb)
    {
        return txb == null ? 0U : (uint)txb.texfilelist.nTex;
    }

    private ushort amTxbGetMagFilter(TXB_HEADER txb, uint tex_no)
    {
        mppAssertNotImpl();
        return tex_no >= amTxbGetCount(txb) ? g_txb_mag_filter[0] : txb.texfilelist.pTexFileList[(int)tex_no].MagFilter;
    }

    private ushort amTxbGetMinFilter(TXB_HEADER txb, uint tex_no)
    {
        mppAssertNotImpl();
        return tex_no >= amTxbGetCount(txb) ? g_txb_min_filter[0] : txb.texfilelist.pTexFileList[(int)tex_no].MinFilter;
    }

    private string amTxbGetName(TXB_HEADER txb, uint tex_no)
    {
        mppAssertNotImpl();
        if (tex_no >= amTxbGetCount(txb))
            tex_no = 0U;
        return txb.texfilelist.pTexFileList[(int)tex_no].Filename;
    }

    private static int amTxbConv(TXB_HEADER header)
    {
        return 0;
    }

    private static int amTxbConv(byte[] stream)
    {
        mppAssertNotImpl();
        return 1;
    }
}