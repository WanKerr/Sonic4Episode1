using System;

public partial class AppMain
{
    private static object amBindGet(AMS_AMB_HEADER header, int index)
    {
        string sPath;
        AmbChunk buf = amBindGet(header, index, out sPath);
        if (header.files[index].IndexOf(".amb", StringComparison.OrdinalIgnoreCase) == -1)
            return buf;
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(buf);
        amsAmbHeader.dir = sPath;
        return amsAmbHeader;
    }

    private static AmbChunk amBindGet(
      AMS_AMB_HEADER header,
      int index,
      out string sPath)
    {
        sPath = null;
        AmbChunk ambChunk = null;
        if (index < header.file_num)
            ambChunk = new AmbChunk(header.data, header.offsets[index], header.lengths[index], header);
        else
            mppAssertNotImpl();
        return ambChunk;
    }

    private static AmbChunk amBindGet(AMS_FS header, int index)
    {
        return amBindGet(header, index, out string _);
    }

    private static AmbChunk amBindGet(
      AMS_FS header,
      int index,
      out string sPath)
    {
        sPath = null;
        byte[] array = null;
        int offset = -1;
        int length = 0;
        if (index < header.count)
        {
            array = header.data;
            offset = header.offsets[index];
            length = header.lengths[index];
        }
        return new AmbChunk(array, offset, length, header.amb_header);
    }

    public static AmbChunk amBindSearch(AMS_AMB_HEADER header, string filename)
    {
        for (int index = 0; index < header.file_num; ++index)
        {
            if (header.files[index] == filename)
                return new AmbChunk(header.data, header.offsets[index], header.lengths[index], header);
        }
        return null;
    }

    private static AmbChunk amBindSearchEx(
      AMS_AMB_HEADER header,
      string exname)
    {
        AmbChunk ambChunk = null;
        for (int index = 0; index < header.file_num; ++index)
        {
            if (header.files[index].IndexOf(exname, 0, StringComparison.OrdinalIgnoreCase) != -1)
            {
                ambChunk = new AmbChunk(header.data, header.offsets[index], header.lengths[index], header);
                break;
            }
        }
        return ambChunk;
    }

    private byte[] amBindSearchID(ref AMS_AMB_HEADER header, string file_id, byte[] top)
    {
        mppAssertNotImpl();
        return null;
    }



}