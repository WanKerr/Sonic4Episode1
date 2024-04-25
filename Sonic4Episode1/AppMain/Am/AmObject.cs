using System.IO;

public partial class AppMain
{
    public static void amObjectSetup(
      out NNS_OBJECT _object,
      out NNS_TEXFILELIST texfilelist,
      object _buf)
    {
        _object = null;
        texfilelist = null;
        AmbChunk ambChunk = (AmbChunk)_buf;
        using (MemoryStream memoryStream = new MemoryStream(ambChunk.array, ambChunk.offset, ambChunk.array.Length - ambChunk.offset))
        {
            BinaryReader reader = new BinaryReader(memoryStream);
            var bincnkFileheader = NNS_BINCNK_FILEHEADER.Read(reader);
            long ofsData;
            reader.BaseStream.Seek(ofsData = bincnkFileheader.OfsData, SeekOrigin.Begin);
            var bincnkDataheader = NNS_BINCNK_DATAHEADER.Read(reader);
            long data0Pos = ofsData;
            reader.BaseStream.Seek(bincnkFileheader.OfsNOF0, SeekOrigin.Begin);
            var nof0Header = NNS_BINCNK_NOF0HEADER.Read(reader);
            int nChunk = bincnkFileheader.nChunk;
            while (nChunk > 0)
            {
                switch (bincnkDataheader.Id)
                {
                    case 1112492366:
                        reader.BaseStream.Seek(data0Pos + bincnkDataheader.OfsMainData, SeekOrigin.Begin);
                        _object = NNS_OBJECT.Read(reader, data0Pos);
                        break;
                    case 1145980238:
                        return;
                    case 1280592206:
                        reader.BaseStream.Seek(data0Pos + bincnkDataheader.OfsMainData, SeekOrigin.Begin);
                        texfilelist = NNS_TEXFILELIST.Read(reader, data0Pos);
                        break;
                }
                --nChunk;
                reader.BaseStream.Seek(ofsData += 8 + bincnkDataheader.OfsNextId, SeekOrigin.Begin);
                bincnkDataheader = NNS_BINCNK_DATAHEADER.Read(reader);
            }
        }
    }

    public static int amObjectLoad(
      out NNS_OBJECT _object,
      NNS_OBJECT obj_file,
      uint drawflag)
    {
        AMS_PARAM_VERTEX_BUFFER_OBJECT vertexBufferObject = new AMS_PARAM_VERTEX_BUFFER_OBJECT();
        _object = new NNS_OBJECT();
        vertexBufferObject.obj = _object;
        vertexBufferObject.srcobj = obj_file;
        vertexBufferObject.bindflag = 0U;
        vertexBufferObject.drawflag = drawflag;
        return amDrawRegistCommand(3, vertexBufferObject);
    }

    public static int amObjectLoad(
      out NNS_OBJECT _object,
      out NNS_TEXLIST texlist,
      out object texlistbuf,
      object buf,
      uint drawflag,
      string filepath,
      AMS_AMB_HEADER amb)
    {
        NNS_OBJECT _object1;
        NNS_TEXFILELIST texfilelist;
        amObjectSetup(out _object1, out texfilelist, buf);
        int nTex = texfilelist.nTex;
        texlistbuf = null;
        nnSetUpTexlist(out texlist, nTex, ref texlistbuf);
        int num = amObjectLoad(out _object, _object1, drawflag);
        if (filepath != null || amb != null)
            num = amTextureLoad(texlist, texfilelist, filepath, amb);
        return num;
    }

    private static int amObjectLoad(
      out NNS_OBJECT _object,
      NNS_TEXFILELIST txbfilelist,
      out NNS_TEXLIST texlist,
      out object texlistbuf,
      object buf,
      uint drawflag,
      string filepath,
      AMS_AMB_HEADER amb)
    {
        NNS_OBJECT _object1;
        amObjectSetup(out _object1, out NNS_TEXFILELIST _, buf);
        int nTex = txbfilelist.nTex;
        texlistbuf = null;
        nnSetUpTexlist(out texlist, nTex, ref texlistbuf);
        int num = amObjectLoad(out _object, _object1, drawflag);
        if (filepath != null || amb != null)
            num = amTextureLoad(texlist, txbfilelist, filepath, amb);
        return num;
    }

    private int amObjectLoadShader(ref NNS_OBJECT _object, uint drawflag)
    {
        mppAssertNotImpl();
        return 0;
    }

    private int amObjectLoadShader(ref NNS_OBJECT _object, int num, uint drawflag)
    {
        mppAssertNotImpl();
        return 0;
    }

    public static int amObjectRelease(NNS_OBJECT _object)
    {
        return amDrawRegistCommand(4, new AMS_PARAM_DELETE_VERTEX_OBJECT()
        {
            obj = _object
        });
    }

    private static int amObjectRelease(NNS_OBJECT _object, NNS_TEXLIST texlist)
    {
        amObjectRelease(_object);
        return amTextureRelease(texlist);
    }

}