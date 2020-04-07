// Decompiled with JetBrains decompiler
// Type: ao.CProc`1
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

namespace ao
{
  public class CProc<T>
  {
    private CProc<T>.TypeProcedure m_proc;
    private uint m_counter;
    private uint m_counter_next;

    public CProc()
    {
      this.SetOwnProcNone();
      this.m_counter = 0U;
    }

    public void SetOwnProcNone()
    {
      this.SetOwnProc((CProc<T>.TypeProcedure) null);
    }

    public void SetProcNone(uint no)
    {
      this.SetProc(no, (CProc<T>.TypeProcedure) null);
    }

    public void SetOwnProc()
    {
      this.SetOwnProc((CProc<T>.TypeProcedure) null);
    }

    public void SetProc(uint no)
    {
      this.SetProc(no, (CProc<T>.TypeProcedure) null);
    }

    public void SetOwnProc(CProc<T>.TypeProcedure proc)
    {
      this.m_proc = proc;
      this.m_counter_next = 0U;
    }

    public void SetProc(uint no, CProc<T>.TypeProcedure proc)
    {
      this.SetOwnProc(proc);
    }

    public CProc<T>.TypeProcedure GetOwnProc()
    {
      return this.m_proc;
    }

    public CProc<T>.TypeProcedure GetProc(uint no)
    {
      return this.GetOwnProc();
    }

    public bool IsOwnProcNone()
    {
      return this.m_proc == null;
    }

    public bool IsProcNone(uint no)
    {
      return this.IsOwnProcNone();
    }

    public bool IsOwnProc(CProc<T>.TypeProcedure proc)
    {
      return proc == this.m_proc;
    }

    public bool IsProc(uint no, CProc<T>.TypeProcedure proc)
    {
      return this.IsOwnProc(proc);
    }

    public uint GetCount()
    {
      return this.m_counter;
    }

    public void ResetCount()
    {
      this.m_counter_next = 0U;
    }

    public void operator_brackets()
    {
      this.m_counter = this.m_counter_next;
      if (this.m_counter_next < uint.MaxValue)
        ++this.m_counter_next;
      if (this.m_proc == null)
        return;
      this.m_proc();
    }

    public void operator_brackets(uint no)
    {
      this.operator_brackets();
    }

    public void Call()
    {
      this.operator_brackets();
    }

    public void Call(uint no)
    {
      this.operator_brackets(no);
    }

    public delegate void TypeProcedure();
  }
}
