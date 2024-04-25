// Decompiled with JetBrains decompiler
// Type: XBOXLive
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

//using Microsoft.Phone.Tasks;

public abstract class XBOXLive
{
    protected static bool TrialModeCached = false;
    public static SigninStatus signinStatus = SigninStatus.None;
    public static bool displayTitleUpdateMessage = false;
    public static bool allowShowUpdate = false;
    public static int updateExceptCount = 1;
    //protected static GamerServicesComponent gameService;
    public static XBOXLive instanceXBOX;

    public static bool isTrial(bool update)
    {
        return TrialModeCached;
    }

    public static bool isTrial()
    {
        return TrialModeCached;
    }

    public static void showGuide()
    {
        if (!isTrial())
            return;
        //Guide.ShowMarketplace(PlayerIndex.One);
    }

    //public static void HandleGameUpdateRequired(GameUpdateRequiredException e)
    //{
    //  try
    //  {
    //    if (XBOXLive.gameService.Enabled)
    //      XBOXLive.displayTitleUpdateMessage = true;
    //    XBOXLive.signinStatus = XBOXLive.SigninStatus.UpdateNeeded;
    //    XBOXLive.gameService.Enabled = false;
    //  }
    //  catch (Exception ex)
    //  {
    //  }
    //}

    public abstract void _initTextDialog(
      out string dlgYes,
      out string dlgNo,
      out string dlgCaption,
      out string dlgText);

    public static void showUpdateMB()
    {
        if (!displayTitleUpdateMessage || !allowShowUpdate)
            return;
        displayTitleUpdateMessage = false;
        AppMain.g_ao_sys_global.is_show_ui = true;
        string dlgYes = "";
        string dlgNo = "";
        string dlgCaption = "";
        string dlgText = "";
        if (instanceXBOX != null)
            instanceXBOX._initTextDialog(out dlgYes, out dlgNo, out dlgCaption, out dlgText);

        return;
    }

    public static void tryUpdate()
    {
        if (updateExceptCount < 0)
            updateExceptCount = 0;
        if (updateExceptCount == 0) { }
        //throw new GameUpdateRequiredException("Text Exception");
    }

    public enum SigninStatus
    {
        None,
        SigningIn,
        Local,
        LIVE,
        Error,
        UpdateNeeded,
    }
}
