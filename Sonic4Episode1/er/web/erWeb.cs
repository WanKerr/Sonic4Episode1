﻿// Decompiled with JetBrains decompiler
// Type: er.web.erWeb
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

namespace er.web
{
    public class erWeb
    {
        public static void StartWeb(string url)
        {
#if WINDOWS_UAP
            Windows.System.Launcher.LaunchUriAsync(new System.Uri(url));
#elif WINDOWSPHONE7_5 || WASM
            (new Microsoft.Phone.Tasks.WebBrowserTask() { Uri = new System.Uri(url) }).Show();
#else
            System.Diagnostics.Process.Start(url);
#endif
        }
    }
}
