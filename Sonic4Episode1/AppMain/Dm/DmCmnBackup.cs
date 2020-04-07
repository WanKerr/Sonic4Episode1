using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static void DmCmnBackupLoad()
    {
        var instance = SSave.CreateInstance();
        var save = XmlStorage.Load();
        if (save != null)
        {
            instance.SetSave(save);
         }
    }

    private static bool DmCmnBackupIsLoadFinished()
    {
        //AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        //if (!AppMain.AoStorageLoadIsFinished())
        //    return false;
        //mainSysInfo.cmp_backup.setData(mainSysInfo.backup.getData());
        return true;
    }

    private static bool DmCmnBackupIsLoadSuccessed()
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        bool flag;
        if (XmlStorage.SaveSuccess())
        {
            mainSysInfo.is_save_run = 1U;
            flag = true;
        }
        else
            flag = false;
        return flag;
    }

    private static void DmCmnBackupSave(bool is_first, bool is_new, bool is_del)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        var save = SSave.CreateInstance().GetSave();
        if (save == null)
        {
            SSave.CreateInstance().SetSave((save = new gs.Sonic4Save()));
        }

        if (is_first)
        {
            XmlStorage.Save(save, true, false);
            //mainSysInfo.cmp_backup.setData(mainSysInfo.backup.getData());
        }
        else if (is_new)
        {
            XmlStorage.Save(save, false, true);
            //mainSysInfo.cmp_backup.setData(mainSysInfo.backup.getData());
        }
        else
        {
            if (mainSysInfo.is_save_run == 0U || !AppMain.dmCmnBackupIsCmpSaveData())
                return;
            XmlStorage.Save(save, false, false);
            // mainSysInfo.cmp_backup.setData(mainSysInfo.backup.getData());
        }
    }

    private static bool DmCmnBackupIsSaveFinished()
    {
        return true;
    }

    private static bool DmCmnBackupIsSaveSuccessed()
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        bool flag;
        if (XmlStorage.SaveSuccess())
        {
            mainSysInfo.is_save_run = 1U;
            flag = true;
        }
        else if (XmlStorage.GetLastError() == 2)
        {
            mainSysInfo.is_save_run = 0U;
            flag = true;
        }
        else
            flag = false;
        return flag;
    }

    private static bool dmCmnBackupIsCmpSaveData()
    {
        return !AppMain.dmCmnBackupMathCompare();
    }

    private static bool dmCmnBackupMathCompare()
    {
        return !SSave.CreateInstance().GetDirty();
    }
}