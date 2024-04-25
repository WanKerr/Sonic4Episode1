using System;

public partial class AppMain
{

    private static object ObjDataLoadAmbIndex(
      OBS_DATA_WORK data_work,
      int index,
      AMS_AMB_HEADER amb)
    {
        object obj = null;
        if (data_work != null)
        {
            if (data_work.pData == null)
            {
                if (amb != null)
                {
                    if (amb.buf[index] != null)
                        data_work.pData = amb.buf[index];
                    else if (amb.files[index].IndexOf(".amb", StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        string sPath;
                        data_work.pData = readAMBFile(amBindGet(amb, index, out sPath));
                        ((AMS_AMB_HEADER)data_work.pData).dir = sPath;
                    }
                    else
                    {
                        string sPath;
                        data_work.pData = amb.files[index].IndexOf(".ame", StringComparison.InvariantCultureIgnoreCase) == -1 ? (amb.files[index].IndexOf(".ama", StringComparison.InvariantCultureIgnoreCase) == -1 ? amBindGet(amb, index, out sPath) : (object)readAMAFile(amBindGet(amb, index, out sPath))) : readAMEfile(amBindGet(amb, index, out sPath));
                    }
                    amb.buf[index] = data_work.pData;
                    data_work.num = 32768;
                    ++data_work.num;
                }
            }
            else
                ++data_work.num;
            return data_work.pData;
        }
        if (amb != null)
        {
            if (amb.buf[index] != null)
                obj = amb.buf[index];
            else if (amb.files[index].IndexOf(".amb", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                string sPath;
                obj = readAMBFile(amBindGet(amb, index, out sPath));
                ((AMS_AMB_HEADER)obj).dir = sPath;
            }
            else
            {
                string sPath;
                obj = amb.files[index].IndexOf(".ame", StringComparison.InvariantCultureIgnoreCase) == -1 ? (amb.files[index].IndexOf(".ama", StringComparison.InvariantCultureIgnoreCase) == -1 ? amBindGet(amb, index, out sPath) : (object)readAMAFile(amBindGet(amb, index, out sPath))) : readAMEfile(amBindGet(amb, index, out sPath));
            }
            amb.buf[index] = obj;
        }
        return obj;
    }

    private static object ObjDataSet(OBS_DATA_WORK pWork, object pData)
    {
        pWork.pData = pData;
        ++pWork.num;
        return pWork.pData;
    }

    private static object ObjDataGetInc(OBS_DATA_WORK pWork)
    {
        if (pWork.pData != null)
            ++pWork.num;
        return pWork.pData;
    }

    private static byte[] ObjDataLoad(
      OBS_DATA_WORK data_work,
      string filename,
      object archive)
    {
        byte[] buf1 = null;
        sFile = filename;
        if (data_work != null)
        {
            if (data_work.pData == null)
            {
                if (archive != null)
                {
                    AmbChunk ambChunk = amBindSearch((AMS_AMB_HEADER)archive, sFile);
                    byte[] numArray = new byte[ambChunk.length];
                    Buffer.BlockCopy(ambChunk.array, ambChunk.offset, numArray, 0, ambChunk.length);
                    data_work.pData = numArray;
                    data_work.num = 32768;
                    ++data_work.num;
                }
                else
                {
                    byte[] buf2;
                    amFsRead(sFile, out buf2);
                    data_work.pData = buf2;
                    if (data_work.pData != null)
                        ++data_work.num;
                }
            }
            else
                ++data_work.num;
            return (byte[])data_work.pData;
        }
        if (archive != null)
        {
            AmbChunk ambChunk = amBindSearch((AMS_AMB_HEADER)archive, sFile);
            buf1 = new byte[ambChunk.length];
            Buffer.BlockCopy(ambChunk.array, ambChunk.offset, buf1, 0, ambChunk.length);
        }
        else
            amFsRead(sFile, out buf1);
        return buf1;
    }

    private static void ObjDataRelease(OBS_DATA_WORK pWork)
    {
        if (pWork.num == 0 || pWork.pData == null)
            return;
        --pWork.num;
        if (pWork.num == 0)
            pWork.pData = null;
        if (pWork.num != 32768)
            return;
        pWork.pData = null;
        pWork.num = 0;
    }
}