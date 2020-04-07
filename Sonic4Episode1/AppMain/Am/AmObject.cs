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
    public static void amObjectSetup(
      out AppMain.NNS_OBJECT _object,
      out AppMain.NNS_TEXFILELIST texfilelist,
      object _buf)
    {
        _object = (AppMain.NNS_OBJECT)null;
        texfilelist = (AppMain.NNS_TEXFILELIST)null;
        AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)_buf;
        using (MemoryStream memoryStream = new MemoryStream(ambChunk.array, ambChunk.offset, ambChunk.array.Length - ambChunk.offset))
        {
            BinaryReader reader = new BinaryReader((Stream)memoryStream);
            AppMain.NNS_BINCNK_FILEHEADER bincnkFileheader = AppMain.NNS_BINCNK_FILEHEADER.Read(reader);
            long ofsData;
            reader.BaseStream.Seek(ofsData = (long)bincnkFileheader.OfsData, SeekOrigin.Begin);
            AppMain.NNS_BINCNK_DATAHEADER bincnkDataheader = AppMain.NNS_BINCNK_DATAHEADER.Read(reader);
            long data0Pos = ofsData;
            reader.BaseStream.Seek((long)bincnkFileheader.OfsNOF0, SeekOrigin.Begin);
            AppMain.NNS_BINCNK_NOF0HEADER.Read(reader);
            int nChunk = bincnkFileheader.nChunk;
            while (nChunk > 0)
            {
                switch (bincnkDataheader.Id)
                {
                    case 1112492366:
                        reader.BaseStream.Seek(data0Pos + (long)bincnkDataheader.OfsMainData, SeekOrigin.Begin);
                        _object = AppMain.NNS_OBJECT.Read(reader, data0Pos);
                        break;
                    case 1145980238:
                        return;
                    case 1280592206:
                        reader.BaseStream.Seek(data0Pos + (long)bincnkDataheader.OfsMainData, SeekOrigin.Begin);
                        texfilelist = AppMain.NNS_TEXFILELIST.Read(reader, data0Pos);
                        break;
                }
                --nChunk;
                reader.BaseStream.Seek(ofsData += (long)(8 + bincnkDataheader.OfsNextId), SeekOrigin.Begin);
                bincnkDataheader = AppMain.NNS_BINCNK_DATAHEADER.Read(reader);
            }
        }
    }

    public static int amObjectLoad(
      out AppMain.NNS_OBJECT _object,
      AppMain.NNS_OBJECT obj_file,
      uint drawflag)
    {
        AppMain.AMS_PARAM_VERTEX_BUFFER_OBJECT vertexBufferObject = new AppMain.AMS_PARAM_VERTEX_BUFFER_OBJECT();
        _object = new AppMain.NNS_OBJECT();
        vertexBufferObject.obj = _object;
        vertexBufferObject.srcobj = obj_file;
        vertexBufferObject.bindflag = 0U;
        vertexBufferObject.drawflag = drawflag;
        return AppMain.amDrawRegistCommand(3, (object)vertexBufferObject);
    }

    public static int amObjectLoad(
      out AppMain.NNS_OBJECT _object,
      out AppMain.NNS_TEXLIST texlist,
      out object texlistbuf,
      object buf,
      uint drawflag,
      string filepath,
      AppMain.AMS_AMB_HEADER amb)
    {
        AppMain.NNS_OBJECT _object1;
        AppMain.NNS_TEXFILELIST texfilelist;
        AppMain.amObjectSetup(out _object1, out texfilelist, buf);
        int nTex = texfilelist.nTex;
        texlistbuf = (object)null;
        AppMain.nnSetUpTexlist(out texlist, nTex, ref texlistbuf);
        int num = AppMain.amObjectLoad(out _object, _object1, drawflag);
        if (filepath != null || amb != null)
            num = AppMain.amTextureLoad(texlist, texfilelist, filepath, amb);
        return num;
    }

    private static int amObjectLoad(
      out AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXFILELIST txbfilelist,
      out AppMain.NNS_TEXLIST texlist,
      out object texlistbuf,
      object buf,
      uint drawflag,
      string filepath,
      AppMain.AMS_AMB_HEADER amb)
    {
        AppMain.NNS_OBJECT _object1;
        AppMain.amObjectSetup(out _object1, out AppMain.NNS_TEXFILELIST _, buf);
        int nTex = txbfilelist.nTex;
        texlistbuf = (object)null;
        AppMain.nnSetUpTexlist(out texlist, nTex, ref texlistbuf);
        int num = AppMain.amObjectLoad(out _object, _object1, drawflag);
        if (filepath != null || amb != null)
            num = AppMain.amTextureLoad(texlist, txbfilelist, filepath, amb);
        return num;
    }

    private int amObjectLoadShader(ref AppMain.NNS_OBJECT _object, uint drawflag)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private int amObjectLoadShader(ref AppMain.NNS_OBJECT _object, int num, uint drawflag)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    public static int amObjectRelease(AppMain.NNS_OBJECT _object)
    {
        return AppMain.amDrawRegistCommand(4, (object)new AppMain.AMS_PARAM_DELETE_VERTEX_OBJECT()
        {
            obj = _object
        });
    }

    private static int amObjectRelease(AppMain.NNS_OBJECT _object, AppMain.NNS_TEXLIST texlist)
    {
        AppMain.amObjectRelease(_object);
        return AppMain.amTextureRelease(texlist);
    }

}