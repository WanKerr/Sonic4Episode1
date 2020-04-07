// Decompiled with JetBrains decompiler
// Type: accel.CArray4`1
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;

namespace accel
{
  public struct CArray4<T> where T : struct
  {
    private T v0;
    private T v1;
    private T v2;
    private T v3;

    public T this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.v0;
          case 1:
            return this.v1;
          case 2:
            return this.v2;
          case 3:
            return this.v3;
          default:
            throw new IndexOutOfRangeException();
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.v0 = value;
            break;
          case 1:
            this.v1 = value;
            break;
          case 2:
            this.v2 = value;
            break;
          case 3:
            this.v3 = value;
            break;
          default:
            throw new IndexOutOfRangeException();
        }
      }
    }

    public T x
    {
      get
      {
        return this.v0;
      }
      set
      {
        this.v0 = value;
      }
    }

    public T y
    {
      get
      {
        return this.v1;
      }
      set
      {
        this.v1 = value;
      }
    }

    public T z
    {
      get
      {
        return this.v2;
      }
      set
      {
        this.v2 = value;
      }
    }

    public T w
    {
      get
      {
        return this.v3;
      }
      set
      {
        this.v3 = value;
      }
    }

    public T r
    {
      get
      {
        return this.v0;
      }
      set
      {
        this.v0 = value;
      }
    }

    public T g
    {
      get
      {
        return this.v1;
      }
      set
      {
        this.v1 = value;
      }
    }

    public T b
    {
      get
      {
        return this.v2;
      }
      set
      {
        this.v2 = value;
      }
    }

    public T a
    {
      get
      {
        return this.v3;
      }
      set
      {
        this.v3 = value;
      }
    }

    public T left
    {
      get
      {
        return this.v0;
      }
      set
      {
        this.v0 = value;
      }
    }

    public T top
    {
      get
      {
        return this.v1;
      }
      set
      {
        this.v1 = value;
      }
    }

    public T right
    {
      get
      {
        return this.v2;
      }
      set
      {
        this.v2 = value;
      }
    }

    public T bottom
    {
      get
      {
        return this.v3;
      }
      set
      {
        this.v3 = value;
      }
    }

    public int size()
    {
      return 4;
    }

    public static CArray4<T> initializer()
    {
      return CArray4<T>.initializer(default (T));
    }

    public static CArray4<T> initializer(T value)
    {
      CArray4<T> carray4;
      carray4.v0 = value;
      carray4.v1 = value;
      carray4.v2 = value;
      carray4.v3 = value;
      return carray4;
    }

    public static CArray4<T> initializer(T v0, T v1, T v2, T v3)
    {
      CArray4<T> carray4;
      carray4.v0 = v0;
      carray4.v1 = v1;
      carray4.v2 = v2;
      carray4.v3 = v3;
      return carray4;
    }

    public static CArray4<T> initializer(T v0, T v1, T v2)
    {
      return CArray4<T>.initializer(v0, v1, v2, default (T));
    }

    public static CArray4<T> initializer(T v0, T v1)
    {
      return CArray4<T>.initializer(v0, v1, default (T), default (T));
    }
  }
}
