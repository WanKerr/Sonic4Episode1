using System.Globalization;

public partial class AppMain
{
    private static void GsEnvInit()
    {
        g_gs_env_region = GsEnvGetRegionIphone();
        g_gs_env_language = GsEnvGetLanguageIphone();
        switch (g_gs_env_language)
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

    private static GSE_REGION GsEnvGetRegion()
    {
        return g_gs_env_region;
    }

    private static bool GsEnvIsRegionAsia()
    {
        return g_gs_env_is_asia;
    }

    public static int GsEnvGetLanguage()
    {
        return g_gs_env_language;
    }

    private static GSE_DECIDE_KEY GeEnvGetDecideKey()
    {
        return g_gs_env_decide_key;
    }

    private static char GsEnvDebugGetDecideKeyChar()
    {
        return 'A';
    }

    private static GSE_REGION GsEnvGetRegionIphone()
    {
        var cregionTableArray = new CRegionTable[59]
        {
            new CRegionTable("JP", GSE_REGION.GSD_REGION_JP),
            new CRegionTable("US", GSE_REGION.GSD_REGION_US),
            new CRegionTable("CA", GSE_REGION.GSD_REGION_US),
            new CRegionTable("PM", GSE_REGION.GSD_REGION_US),
            new CRegionTable("FR", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("IT", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("DE", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("ES", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("AL", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("AD", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("AZ", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("AT", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("AM", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("BE", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("BA", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("BG", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("BY", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("HR", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("CZ", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("DK", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("EE", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("FO", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("FI", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("AX", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("GE", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("GI", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("GR", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("GL", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("VA", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("HU", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("IS", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("IE", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("LV", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("LI", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("LT", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("LU", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("MC", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("MD", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("ME", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("NL", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("NO", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("PL", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("PT", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("RO", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("SM", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("RS", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("SK", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("SI", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("SJ", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("SE", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("CH", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("UA", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("MK", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("GB", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("GG", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("JE", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("IM", GSE_REGION.GSD_REGION_EU),
            new CRegionTable("BR", GSE_REGION.GSD_REGION_US),
            new CRegionTable("RU", GSE_REGION.GSD_REGION_EU)
        };
        string name = CultureInfo.CurrentCulture.Name;
        for (int index = 0; index < cregionTableArray.Length; ++index)
        {
            if (name.IndexOf(cregionTableArray[index].country) != -1)
                return cregionTableArray[index].region;
        }
        return GSE_REGION.GSD_REGION_US;
    }

    private static int GsEnvGetLanguageIphone()
    {
        var clanguageTableArray = new CLanguageTable[16]
        {
            new CLanguageTable("ja-JP", 0),
            new CLanguageTable("en-US", 1),
            new CLanguageTable("fr-FR", 2),
            new CLanguageTable("it-IT", 3),
            new CLanguageTable("de-DE", 4),
            new CLanguageTable("es-ES", 5),
            new CLanguageTable("fi-FI", 6),
            new CLanguageTable("pt-BR", 7),
            new CLanguageTable("ru-RU", 8),
            new CLanguageTable("zh-CN", 9),
            new CLanguageTable("zh-Hans", 9),
            new CLanguageTable("zh-HK", 10),
            new CLanguageTable("zh-MO", 10),
            new CLanguageTable("zh-SG", 10),
            new CLanguageTable("zh-TW", 10),
            new CLanguageTable("zh-Hant", 10)
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