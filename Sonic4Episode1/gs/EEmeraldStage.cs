// Decompiled with JetBrains decompiler
// Type: gs.backup.EEmeraldStage
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System.Runtime.InteropServices;

namespace gs.backup
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct EEmeraldStage
    {
        public enum Type
        {
            Null,
            Zone1Act1,
            Zone1Act2,
            Zone1Act3,
            Zone2Act1,
            Zone2Act2,
            Zone2Act3,
            Zone3Act1,
            Zone3Act2,
            Zone3Act3,
            Zone4Act1,
            Zone4Act2,
            Zone4Act3,
            Max,
            None,
        }
    }
}
