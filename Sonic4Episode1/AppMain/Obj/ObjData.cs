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

    private static object ObjDataLoadAmbIndex(
      AppMain.OBS_DATA_WORK data_work,
      int index,
      AppMain.AMS_AMB_HEADER amb)
    {
        object obj = (object)null;
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
                        data_work.pData = (object)AppMain.readAMBFile(AppMain.amBindGet(amb, index, out sPath));
                        ((AppMain.AMS_AMB_HEADER)data_work.pData).dir = sPath;
                    }
                    else
                    {
                        string sPath;
                        data_work.pData = amb.files[index].IndexOf(".ame", StringComparison.InvariantCultureIgnoreCase) == -1 ? (amb.files[index].IndexOf(".ama", StringComparison.InvariantCultureIgnoreCase) == -1 ? (object)AppMain.amBindGet(amb, index, out sPath) : (object)AppMain.readAMAFile((object)AppMain.amBindGet(amb, index, out sPath))) : (object)AppMain.readAMEfile(AppMain.amBindGet(amb, index, out sPath));
                    }
                    amb.buf[index] = data_work.pData;
                    data_work.num = (ushort)32768;
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
                obj = (object)AppMain.readAMBFile(AppMain.amBindGet(amb, index, out sPath));
                ((AppMain.AMS_AMB_HEADER)obj).dir = sPath;
            }
            else
            {
                string sPath;
                obj = amb.files[index].IndexOf(".ame", StringComparison.InvariantCultureIgnoreCase) == -1 ? (amb.files[index].IndexOf(".ama", StringComparison.InvariantCultureIgnoreCase) == -1 ? (object)AppMain.amBindGet(amb, index, out sPath) : (object)AppMain.readAMAFile((object)AppMain.amBindGet(amb, index, out sPath))) : (object)AppMain.readAMEfile(AppMain.amBindGet(amb, index, out sPath));
            }
            amb.buf[index] = obj;
        }
        return obj;
    }

    private static object ObjDataSet(AppMain.OBS_DATA_WORK pWork, object pData)
    {
        pWork.pData = pData;
        ++pWork.num;
        return pWork.pData;
    }

    private static object ObjDataGetInc(AppMain.OBS_DATA_WORK pWork)
    {
        if (pWork.pData != null)
            ++pWork.num;
        return pWork.pData;
    }

    private static byte[] ObjDataLoad(
      AppMain.OBS_DATA_WORK data_work,
      string filename,
      object archive)
    {
        byte[] buf1 = (byte[])null;
        AppMain.sFile = filename;
        if (data_work != null)
        {
            if (data_work.pData == null)
            {
                if (archive != null)
                {
                    AppMain.AmbChunk ambChunk = AppMain.amBindSearch((AppMain.AMS_AMB_HEADER)archive, AppMain.sFile);
                    byte[] numArray = new byte[ambChunk.length];
                    Buffer.BlockCopy((Array)ambChunk.array, ambChunk.offset, (Array)numArray, 0, ambChunk.length);
                    data_work.pData = (object)numArray;
                    data_work.num = (ushort)32768;
                    ++data_work.num;
                }
                else
                {
                    byte[] buf2;
                    AppMain.amFsRead(AppMain.sFile, out buf2);
                    data_work.pData = (object)buf2;
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
            AppMain.AmbChunk ambChunk = AppMain.amBindSearch((AppMain.AMS_AMB_HEADER)archive, AppMain.sFile);
            buf1 = new byte[ambChunk.length];
            Buffer.BlockCopy((Array)ambChunk.array, ambChunk.offset, (Array)buf1, 0, ambChunk.length);
        }
        else
            AppMain.amFsRead(AppMain.sFile, out buf1);
        return buf1;
    }

    private static void ObjDataRelease(AppMain.OBS_DATA_WORK pWork)
    {
        if (pWork.num == (ushort)0 || pWork.pData == null)
            return;
        --pWork.num;
        if (pWork.num == (ushort)0)
            pWork.pData = (object)null;
        if (pWork.num != (ushort)32768)
            return;
        pWork.pData = (object)null;
        pWork.num = (ushort)0;
    }
}