// Decompiled with JetBrains decompiler
// Type: DoubleType`2
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

public struct DoubleType<I, E>
{
    private object Value;

    public static implicit operator DoubleType<I, E>(I i)
    {
        return new DoubleType<I, E>() { Value = i };
    }

    public static implicit operator DoubleType<I, E>(E e)
    {
        return new DoubleType<I, E>() { Value = e };
    }

    public static implicit operator I(DoubleType<I, E> doubleType)
    {
        return doubleType.Value is I ? (I)doubleType.Value : default(I);
    }

    public static explicit operator E(DoubleType<I, E> doubleType)
    {
        return doubleType.Value is I ? default(E) : (E)doubleType.Value;
    }

    public static bool operator ==(DoubleType<I, E> doubleType1, DoubleType<I, E> doubleType2)
    {
        return doubleType1.Equals(doubleType2);
    }

    public static bool operator !=(DoubleType<I, E> doubleType1, DoubleType<I, E> doubleType2)
    {
        return !doubleType1.Equals(doubleType2);
    }

    public override int GetHashCode()
    {
        return this.Value != null ? this.Value.GetHashCode() : 0;
    }

    public override bool Equals(object obj)
    {
        return obj is DoubleType<I, E> doubleType && this.Value.Equals(doubleType.Value);
    }
}
