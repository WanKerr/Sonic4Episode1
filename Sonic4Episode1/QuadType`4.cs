// Decompiled with JetBrains decompiler
// Type: QuadType`4
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

public struct QuadType<I, E1, E2, E3>
{
  private object Value;

  public static implicit operator QuadType<I, E1, E2, E3>(I i)
  {
    return new QuadType<I, E1, E2, E3>()
    {
      Value = (object) i
    };
  }

  public static implicit operator QuadType<I, E1, E2, E3>(E1 e1)
  {
    return new QuadType<I, E1, E2, E3>()
    {
      Value = (object) e1
    };
  }

  public static implicit operator QuadType<I, E1, E2, E3>(E2 e2)
  {
    return new QuadType<I, E1, E2, E3>()
    {
      Value = (object) e2
    };
  }

  public static implicit operator QuadType<I, E1, E2, E3>(E3 e3)
  {
    return new QuadType<I, E1, E2, E3>()
    {
      Value = (object) e3
    };
  }

  public static implicit operator I(QuadType<I, E1, E2, E3> quadType)
  {
    return quadType.Value is I ? (I) quadType.Value : default (I);
  }

  public static explicit operator E1(QuadType<I, E1, E2, E3> quadType)
  {
    return quadType.Value is E1 ? (E1) quadType.Value : default (E1);
  }

  public static explicit operator E2(QuadType<I, E1, E2, E3> quadType)
  {
    return quadType.Value is E2 ? (E2) quadType.Value : default (E2);
  }

  public static explicit operator E3(QuadType<I, E1, E2, E3> quadType)
  {
    return quadType.Value is E3 ? (E3) quadType.Value : default (E3);
  }
}
