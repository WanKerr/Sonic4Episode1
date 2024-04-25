// Decompiled with JetBrains decompiler
// Type: accel.CArray2`1
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;

namespace accel
{
    public struct CArray2<T> where T : struct
    {
        private T v0;
        private T v1;

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
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public T x
        {
            get => this.v0;
            set => this.v0 = value;
        }

        public T y
        {
            get => this.v1;
            set => this.v1 = value;
        }

        public T u
        {
            get => this.v0;
            set => this.v0 = value;
        }

        public T v
        {
            get => this.v1;
            set => this.v1 = value;
        }

        public T s
        {
            get => this.v0;
            set => this.v0 = value;
        }

        public T t
        {
            get => this.v1;
            set => this.v1 = value;
        }

        public T width
        {
            get => this.v0;
            set => this.v0 = value;
        }

        public T height
        {
            get => this.v1;
            set => this.v1 = value;
        }

        public int size()
        {
            return 2;
        }

        public static CArray2<T> initializer()
        {
            return initializer(default(T));
        }

        public static CArray2<T> initializer(T value)
        {
            CArray2<T> carray2;
            carray2.v0 = value;
            carray2.v1 = value;
            return carray2;
        }

        public static CArray2<T> initializer(T v0, T v1)
        {
            CArray2<T> carray2;
            carray2.v0 = v0;
            carray2.v1 = v1;
            return carray2;
        }

        public bool equals(CArray2<T> a2)
        {
            return this.v0.Equals(a2.v0) && this.v1.Equals(a2.v1);
        }
    }
}
