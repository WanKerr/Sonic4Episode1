public partial class AppMain
{
    public static void GmPauseMenuLoadStart()
    {
        CMain_PauseMenu instance = CMain_PauseMenu.CreateInstance();
        instance.Create();
        instance.LoadFile();
    }

    public static bool GmPauseMenuLoadIsFinished()
    {
        return CMain_PauseMenu.CreateInstance().IsLoadFile();
    }

    public static void GmPauseMenuBuildStart()
    {
        CMain_PauseMenu.CreateInstance().CreateTexture();
    }

    public static bool GmPauseMenuBuildIsFinished()
    {
        return CMain_PauseMenu.CreateInstance().IsCreatedTexture();
    }

    public static void GmPauseMenuFlushStart()
    {
        CMain_PauseMenu.CreateInstance().ReleaseTexture();
    }

    public static bool GmPauseMenuFlushIsFinished()
    {
        return CMain_PauseMenu.CreateInstance().IsReleasedTexture();
    }

    public static void GmPauseMenuRelease()
    {
        CMain_PauseMenu.CreateInstance().Release();
    }

    public static void GmPauseMenuStart(uint prio)
    {
        CMain_PauseMenu.CreateInstance().Start(prio);
    }

    public static void GmPauseMenuCancel()
    {
        CMain_PauseMenu.CreateInstance().Cancel();
    }

    public static bool GmPauseMenuIsFinished()
    {
        return CMain_PauseMenu.CreateInstance().IsPlay();
    }

    public static int GmPauseMenuGetResult()
    {
        return CMain_PauseMenu.CreateInstance().GetResult();
    }

}