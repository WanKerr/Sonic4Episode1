// Decompiled with JetBrains decompiler
// Type: accel.CArray3`1
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;

namespace accel
{
    public struct CArray3<T> where T : struct
    {
        private T v0;
        private T v1;
        private T v2;

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

        public T z
        {
            get => this.v2;
            set => this.v2 = value;
        }

        public int size()
        {
            return 3;
        }

        public static CArray3<T> initializer()
        {
            return initializer(default(T));
        }

        public static CArray3<T> initializer(T value)
        {
            CArray3<T> carray3;
            carray3.v0 = value;
            carray3.v1 = value;
            carray3.v2 = value;
            return carray3;
        }

        public static CArray3<T> initializer(T v0, T v1, T v2)
        {
            CArray3<T> carray3;
            carray3.v0 = v0;
            carray3.v1 = v1;
            carray3.v2 = v2;
            return carray3;
        }

        internal bool equals(CArray3<T> a2)
        {
            return this.v0.Equals(a2.v0) && this.v1.Equals(a2.v1) && this.v2.Equals(a2.v2);
        }
    }
}
