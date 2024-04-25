using System.Collections.Generic;

public partial class AppMain
{
    public class MatrixPool
    {
        private List<NNS_MATRIX> cache_;

        public MatrixPool()
        {
            this.cache_ = new List<NNS_MATRIX>();
        }

        public NNS_MATRIX Alloc()
        {
            NNS_MATRIX nnsMatrix;
            if (this.cache_.Count > 0)
            {
                int index = this.cache_.Count - 1;
                nnsMatrix = this.cache_[index];
                this.cache_.RemoveAt(index);
            }
            else
                nnsMatrix = new NNS_MATRIX();
            return nnsMatrix;
        }

        public void Release(NNS_MATRIX obj)
        {
            this.cache_.Add(obj);
        }
    }
}
