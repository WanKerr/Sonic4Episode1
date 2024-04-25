using System.Collections.Generic;

public partial class AppMain
{
    public class ArrayPool<T> where T : new()
    {
        private List<T[]> freeArrays_ = new List<T[]>();
        private List<T[]> usedArrays_ = new List<T[]>();

        public T[] AllocArray(int size)
        {
            int index1 = -1;
            int num = this.freeArrays_.Count > 0 ? this.freeArrays_[0].Length : -1;
            for (int index2 = 0; index2 < this.freeArrays_.Count; ++index2)
            {
                int length = this.freeArrays_[index2].Length;
                if (length >= size && length <= num)
                    index1 = index2;
            }
            T[] objArray;
            if (index1 >= 0)
            {
                objArray = this.freeArrays_[index1];
                this.freeArrays_.RemoveAt(index1);
            }
            else
                objArray = new T[size];
            this.usedArrays_.Add(objArray);
            return objArray;
        }

        public void ReleaseArray(T[] array)
        {
            this.freeArrays_.Add(array);
            this.usedArrays_.Remove(array);
        }

        public void ReleaseUsedArrays()
        {
            this.freeArrays_.AddRange(usedArrays_);
            this.usedArrays_.Clear();
        }

        public void Clear()
        {
            this.freeArrays_.Clear();
            this.usedArrays_.Clear();
        }
    }
}
