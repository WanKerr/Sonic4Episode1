// Decompiled with JetBrains decompiler
// Type: TripleType`3
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

public struct TripleType<I, E1, E2>
{
    private object Value;

    public static implicit operator TripleType<I, E1, E2>(I i)
    {
        return new TripleType<I, E1, E2>()
        {
            Value = i
        };
    }

    public static implicit operator TripleType<I, E1, E2>(E1 e1)
    {
        return new TripleType<I, E1, E2>()
        {
            Value = e1
        };
    }

    public static implicit operator TripleType<I, E1, E2>(E2 e2)
    {
        return new TripleType<I, E1, E2>()
        {
            Value = e2
        };
    }

    public static implicit operator I(TripleType<I, E1, E2> tripleType)
    {
        return tripleType.Value is I ? (I)tripleType.Value : default(I);
    }

    public static explicit operator E1(TripleType<I, E1, E2> tripleType)
    {
        return tripleType.Value is E1 ? (E1)tripleType.Value : default(E1);
    }

    public static explicit operator E2(TripleType<I, E1, E2> tripleType)
    {
        return tripleType.Value is E2 ? (E2)tripleType.Value : default(E2);
    }
}
