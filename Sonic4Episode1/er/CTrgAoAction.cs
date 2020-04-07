// Decompiled with JetBrains decompiler
// Type: er.CTrgAoAction
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System.Runtime.InteropServices;

namespace er
{
  public class CTrgAoAction : CTrgBase<CTrgState>
  {
    private bool[] m_flag = new bool[1];
    private AppMain.AOS_ACTION m_act;

    public CTrgAoAction()
    {
      this.Constructor();
    }

    ~CTrgAoAction()
    {
      this.Destructor();
    }

    public void Constructor()
    {
    }

    public void Destructor()
    {
    }

    public bool Create(AppMain.AOS_ACTION act)
    {
      this.Release();
      this.m_act = act;
      return this.create();
    }

    public virtual void Release()
    {
      if (!this.m_flag[0])
        return;
      this.m_flag[0] = false;
    }

    public override bool IsValid()
    {
      return this.m_flag[0];
    }

    protected bool create()
    {
      bool flag;
      if (this.m_act == null)
      {
        flag = false;
      }
      else
      {
        flag = true;
        this.ResetState();
        this.SetRepeatInterval();
        this.SetDoubleClickTime();
        this.SetMoveThreshold();
        this.m_flag[0] = true;
      }
      return flag;
    }

    protected override bool hitTest(IntPair pos, uint index)
    {
      bool flag = false;
      if (this.m_flag[0])
      {
        uint hitNum = AppMain.AoActGetHitNum(this.m_act);
        AppMain.AOS_ACT_HIT[] aosActHitArray = AppMain.New<AppMain.AOS_ACT_HIT>((int) hitNum);
        int hitTbl = (int) AppMain.AoActGetHitTbl((AppMain.ArrayPointer<AppMain.AOS_ACT_HIT>) aosActHitArray, hitNum, this.m_act);
        for (int index1 = 0; (long) index1 < (long) hitNum; ++index1)
        {
          if (AppMain.AoActHitTestCorReverse(aosActHitArray[index1], (float) pos.first, (float) pos.second))
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct BFlag
    {
      public const int Setup = 0;
      public const int Max = 1;
      public const int None = 2;
    }
  }
}
