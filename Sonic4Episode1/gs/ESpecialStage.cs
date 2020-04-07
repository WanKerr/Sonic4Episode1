// Decompiled with JetBrains decompiler
// Type: gs.backup.ESpecialStage
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System.Runtime.InteropServices;

namespace gs.backup
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct ESpecialStage
    {
        public enum Type
        {
            Stage1,
            Stage2,
            Stage3,
            Stage4,
            Stage5,
            Stage6,
            Stage7,
            Max,
            None,
        }
    }
}
