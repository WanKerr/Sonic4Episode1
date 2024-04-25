using System;
using System.Collections.Generic;

public partial class AppMain
{
    public class ArrayPoolMultisize<T> where T : new()
    {
        private List<int> arrayElementCapacity_ = new List<int>();
        private List<ArrayPoolFast<T>> Arrays_ = new List<ArrayPoolFast<T>>();

        public T[] AllocArray(int size)
        {
            return this.Arrays_[this._GetArrayPoolIDBestForCapacity(size)].AllocArray(size);
        }

        public void ReleaseArray(T[] array)
        {
            this.Arrays_[this._GetArrayPoolIDBestForCapacity(array.Length)].ReleaseArray(array);
        }

        public void ReleaseUsedArrays()
        {
            for (int index = 0; index < this.Arrays_.Count; ++index)
                this.Arrays_[index].ReleaseUsedArrays();
        }

        public void Clear()
        {
            for (int index = 0; index < this.Arrays_.Count; ++index)
                this.Arrays_[index].Clear();
        }

        public void AddCacheWithCapacity(int iCapacity, int iAmount)
        {
            if (this.arrayElementCapacity_.Count > 0 && this.arrayElementCapacity_[this.arrayElementCapacity_.Count - 1] > iCapacity)
                throw new NotSupportedException();
            this.arrayElementCapacity_.Add(iCapacity);
            ArrayPoolFast<T> arrayPoolFast = new ArrayPoolFast<T>();
            for (int index = 0; index < iAmount; ++index)
                arrayPoolFast.AllocArray(iCapacity);
            arrayPoolFast.ReleaseUsedArrays();
            this.Arrays_.Add(arrayPoolFast);
        }

        private int _GetArrayPoolIDBestForCapacity(int iCapacity)
        {
            for (int index = 0; index < this.arrayElementCapacity_.Count; ++index)
            {
                if (iCapacity <= this.arrayElementCapacity_[index] && this.Arrays_[index].GetFreeCount() > 0)
                    return index;
            }
            if (iCapacity <= this.arrayElementCapacity_[this.arrayElementCapacity_.Count - 1])
                return this.arrayElementCapacity_.Count - 1;
            this.AddCacheWithCapacity(iCapacity, 1);
            return this.arrayElementCapacity_.Count - 1;
        }
    }
}
