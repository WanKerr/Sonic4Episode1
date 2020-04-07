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
    private static void GsEnvInit()
    {
        AppMain.g_gs_env_region = AppMain.GsEnvGetRegionIphone();
        AppMain.g_gs_env_language = AppMain.GsEnvGetLanguageIphone();
        switch (AppMain.g_gs_env_language)
        {
            case 0:
                Sonic4ep1.Strings.Culture = new CultureInfo("ja-JP");
                break;
            case 1:
                Sonic4ep1.Strings.Culture = new CultureInfo("en-US");
                break;
            case 2:
                Sonic4ep1.Strings.Culture = new CultureInfo("fr-FR");
                break;
            case 3:
                Sonic4ep1.Strings.Culture = new CultureInfo("it-IT");
                break;
            case 4:
                Sonic4ep1.Strings.Culture = new CultureInfo("de-DE");
                break;
            case 5:
                Sonic4ep1.Strings.Culture = new CultureInfo("es-ES");
                break;
            case 6:
                Sonic4ep1.Strings.Culture = new CultureInfo("fi-FI");
                break;
            case 7:
                Sonic4ep1.Strings.Culture = new CultureInfo("pt-BR");
                break;
            case 8:
                Sonic4ep1.Strings.Culture = new CultureInfo("ru-RU");
                break;
            case 9:
                Sonic4ep1.Strings.Culture = new CultureInfo("zh-CN");
                break;
            case 10:
                Sonic4ep1.Strings.Culture = new CultureInfo("zh-TW");
                break;
            default:
                Sonic4ep1.Strings.Culture = new CultureInfo("en");
                break;
        }
    }

    private static AppMain.GSE_REGION GsEnvGetRegion()
    {
        return AppMain.g_gs_env_region;
    }

    private static bool GsEnvIsRegionAsia()
    {
        return AppMain.g_gs_env_is_asia;
    }

    public static int GsEnvGetLanguage()
    {
        return AppMain.g_gs_env_language;
    }

    private static AppMain.GSE_DECIDE_KEY GeEnvGetDecideKey()
    {
        return AppMain.g_gs_env_decide_key;
    }

    private static char GsEnvDebugGetDecideKeyChar()
    {
        return 'A';
    }

    private static AppMain.GSE_REGION GsEnvGetRegionIphone()
    {
        AppMain.CRegionTable[] cregionTableArray = new AppMain.CRegionTable[59]
        {
      new AppMain.CRegionTable("JP", AppMain.GSE_REGION.GSD_REGION_JP),
      new AppMain.CRegionTable("US", AppMain.GSE_REGION.GSD_REGION_US),
      new AppMain.CRegionTable("CA", AppMain.GSE_REGION.GSD_REGION_US),
      new AppMain.CRegionTable("PM", AppMain.GSE_REGION.GSD_REGION_US),
      new AppMain.CRegionTable("FR", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("IT", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("DE", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("ES", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("AL", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("AD", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("AZ", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("AT", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("AM", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("BE", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("BA", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("BG", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("BY", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("HR", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("CZ", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("DK", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("EE", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("FO", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("FI", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("AX", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("GE", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("GI", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("GR", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("GL", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("VA", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("HU", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("IS", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("IE", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("LV", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("LI", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("LT", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("LU", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("MC", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("MD", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("ME", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("NL", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("NO", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("PL", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("PT", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("RO", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("SM", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("RS", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("SK", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("SI", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("SJ", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("SE", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("CH", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("UA", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("MK", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("GB", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("GG", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("JE", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("IM", AppMain.GSE_REGION.GSD_REGION_EU),
      new AppMain.CRegionTable("BR", AppMain.GSE_REGION.GSD_REGION_US),
      new AppMain.CRegionTable("RU", AppMain.GSE_REGION.GSD_REGION_EU)
        };
        string name = CultureInfo.CurrentCulture.Name;
        for (int index = 0; index < cregionTableArray.Length; ++index)
        {
            if (name.IndexOf(cregionTableArray[index].country) != -1)
                return cregionTableArray[index].region;
        }
        return AppMain.GSE_REGION.GSD_REGION_US;
    }

    private static int GsEnvGetLanguageIphone()
    {
        AppMain.CLanguageTable[] clanguageTableArray = new AppMain.CLanguageTable[16]
        {
      new AppMain.CLanguageTable("ja-JP", 0),
      new AppMain.CLanguageTable("en-US", 1),
      new AppMain.CLanguageTable("fr-FR", 2),
      new AppMain.CLanguageTable("it-IT", 3),
      new AppMain.CLanguageTable("de-DE", 4),
      new AppMain.CLanguageTable("es-ES", 5),
      new AppMain.CLanguageTable("fi-FI", 6),
      new AppMain.CLanguageTable("pt-BR", 7),
      new AppMain.CLanguageTable("ru-RU", 8),
      new AppMain.CLanguageTable("zh-CN", 9),
      new AppMain.CLanguageTable("zh-Hans", 9),
      new AppMain.CLanguageTable("zh-HK", 10),
      new AppMain.CLanguageTable("zh-MO", 10),
      new AppMain.CLanguageTable("zh-SG", 10),
      new AppMain.CLanguageTable("zh-TW", 10),
      new AppMain.CLanguageTable("zh-Hant", 10)
        };
        string name = CultureInfo.CurrentCulture.Name;
        for (int index = 0; index < clanguageTableArray.Length; ++index)
        {
            if (name.IndexOf(clanguageTableArray[index].lang) != -1)
                return clanguageTableArray[index].code;
        }
        return 1;
    }

}