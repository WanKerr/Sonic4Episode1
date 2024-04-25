public partial class AppMain
{

    private static void GsFontInit()
    {
    }

    private static void GsFontExit()
    {
    }

    private static void GsFontBuild()
    {
        GsFontBuild(true);
    }

    private static void GsFontBuild(bool use_mem2)
    {
        g_gs_font_builded = true;
    }

    private static bool GsFontIsBuilding()
    {
        return false;
    }

    private static bool GsFontIsBuilded()
    {
        return g_gs_font_builded;
    }

    private static void GsFontRelease()
    {
        g_gs_font_builded = false;
    }
}