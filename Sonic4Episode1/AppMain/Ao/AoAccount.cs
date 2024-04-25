public partial class AppMain
{
    private static void AoAccountInit()
    {
        g_ao_account_current_id = -1;
    }

    private static void AoAccountDebugInit()
    {
    }

    private static void AoAccountExit()
    {
    }

    private static void AoAccountClearCurrentId()
    {
        g_ao_account_current_id = -1;
    }

    private static void AoAccountSetCurrentIdStart(uint id)
    {
        g_ao_account_current_id = (int)id;
    }

    private static bool AoAccountSetCurrentIdIsFinished()
    {
        return true;
    }

    public static int AoAccountGetCurrentId()
    {
        return g_ao_account_current_id;
    }

    private static bool AoAccountIsCurrentSignin()
    {
        return AoAccountGetCurrentId() >= 0;
    }

    private static bool AoAccountIsCurrentOnline()
    {
        return XBOXLive.signinStatus == XBOXLive.SigninStatus.LIVE;
    }

    public static bool AoAccountIsCurrentEnable()
    {
        return (uint)g_ao_account_current_id < 4U;
    }


}