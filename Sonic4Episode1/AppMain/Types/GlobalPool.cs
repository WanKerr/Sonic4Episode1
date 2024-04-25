using System;
using System.Collections.Generic;
#if NET7_0_OR_GREATER
using System.Diagnostics.Metrics;
#endif

public class GlobalPool<T> where T : class, IClearable, new()
{
    private static List<T> cache_ = new List<T>();
    internal static Func<T> factory;

    public static T Alloc()
    {
        T obj;
        if (cache_.Count > 0)
        {
            int index = cache_.Count - 1;
            obj = cache_[index];
            obj.Clear();
            cache_.RemoveAt(index);
        }
        else
            obj = factory != null ? factory() : new T();

        return obj;
    }

    public static void Release(T obj)
    {
        cache_.Add(obj);
    }
}