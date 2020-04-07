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
    private static object amBindGet(AppMain.AMS_AMB_HEADER header, int index)
    {
        string sPath;
        AppMain.AmbChunk buf = AppMain.amBindGet(header, index, out sPath);
        if (header.files[index].IndexOf(".amb", StringComparison.OrdinalIgnoreCase) == -1)
            return (object)buf;
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(buf);
        amsAmbHeader.dir = sPath;
        return (object)amsAmbHeader;
    }

    private static AppMain.AmbChunk amBindGet(
      AppMain.AMS_AMB_HEADER header,
      int index,
      out string sPath)
    {
        sPath = (string)null;
        AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)null;
        if (index < header.file_num)
            ambChunk = new AppMain.AmbChunk(header.data, header.offsets[index], header.lengths[index], header);
        else
            AppMain.mppAssertNotImpl();
        return ambChunk;
    }

    private static AppMain.AmbChunk amBindGet(AppMain.AMS_FS header, int index)
    {
        return AppMain.amBindGet(header, index, out string _);
    }

    private static AppMain.AmbChunk amBindGet(
      AppMain.AMS_FS header,
      int index,
      out string sPath)
    {
        sPath = (string)null;
        byte[] array = (byte[])null;
        int offset = -1;
        int length = 0;
        if (index < header.count)
        {
            array = header.data;
            offset = header.offsets[index];
            length = header.lengths[index];
        }
        return new AppMain.AmbChunk(array, offset, length, header.amb_header);
    }

    public static AppMain.AmbChunk amBindSearch(AppMain.AMS_AMB_HEADER header, string filename)
    {
        for (int index = 0; index < header.file_num; ++index)
        {
            if (header.files[index] == filename)
                return new AppMain.AmbChunk(header.data, header.offsets[index], header.lengths[index], header);
        }
        return (AppMain.AmbChunk)null;
    }

    private static AppMain.AmbChunk amBindSearchEx(
      AppMain.AMS_AMB_HEADER header,
      string exname)
    {
        AppMain.AmbChunk ambChunk = (AppMain.AmbChunk)null;
        for (int index = 0; index < header.file_num; ++index)
        {
            if (header.files[index].IndexOf(exname, 0, StringComparison.OrdinalIgnoreCase) != -1)
            {
                ambChunk = new AppMain.AmbChunk(header.data, header.offsets[index], header.lengths[index], header);
                break;
            }
        }
        return ambChunk;
    }

    private byte[] amBindSearchID(ref AppMain.AMS_AMB_HEADER header, string file_id, byte[] top)
    {
        AppMain.mppAssertNotImpl();
        return (byte[])null;
    }



}