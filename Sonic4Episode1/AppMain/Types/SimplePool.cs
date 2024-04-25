using System.Collections.Generic;

public class SimplePool<T> where T : class, new()
{
    private List<T> cache_;

    public SimplePool()
    {
        this.cache_ = new List<T>();
    }

    public T Alloc()
    {
        T obj;
        if (this.cache_.Count > 0)
        {
            int index = this.cache_.Count - 1;
            obj = this.cache_[index];
            this.cache_.RemoveAt(index);
        }
        else
            obj = new T();
        return obj;
    }

    public void Release(T obj)
    {
        this.cache_.Add(obj);
    }
}
