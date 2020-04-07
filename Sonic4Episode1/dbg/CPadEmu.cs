// Decompiled with JetBrains decompiler
// Type: dbg.CPadEmu
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

namespace dbg
{
  internal class CPadEmu
  {
    private static CPadEmu instance;

    public static CPadEmu CreateInstance()
    {
      if (CPadEmu.instance == null)
        CPadEmu.instance = new CPadEmu();
      return CPadEmu.instance;
    }

    public void Create(CPadEmu.EMode mode)
    {
    }

    public enum EMode
    {
      Tap,
      Drag,
      Game,
      Dummy,
      Max,
      None,
    }
  }
}
