// Decompiled with JetBrains decompiler
// Type: accel.CCircularBuffer`1
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

namespace accel
{
  public class CCircularBuffer<T> where T : new()
  {
    private readonly T[] m_data;
    private int m_begin;
    private int m_size;
    private int static_size;

    public CCircularBuffer(int size)
    {
      this.static_size = size;
      this.m_data = new T[size];
      if (!typeof (T).IsValueType)
      {
        for (int index = 0; index < size; ++index)
          this.m_data[index] = new T();
      }
      this.m_begin = 0;
      this.m_size = 0;
    }

    public T this[int index]
    {
      get
      {
        return this.getAt(index);
      }
      set
      {
        this.setAt(index, value);
      }
    }

    public T getAt(int index)
    {
      return this.m_data[this.indexAt(index)];
    }

    public void setAt(int index, T value)
    {
      this.m_data[this.indexAt(index)] = value;
    }

    public T front
    {
      get
      {
        return this.getAt(0);
      }
      set
      {
        this.setAt(0, value);
      }
    }

    public T back
    {
      get
      {
        return this.getAt(this.m_size - 1);
      }
      set
      {
        this.setAt(this.m_size - 1, value);
      }
    }

    public void push_back(T value)
    {
      if (this.m_size < this.static_size)
      {
        ++this.m_size;
      }
      else
      {
        ++this.m_begin;
        if (this.static_size <= this.m_begin)
          this.m_begin = 0;
      }
      this.back = value;
    }

    public void push_front()
    {
      this.push_front(new T());
    }

    public void push_front(T value)
    {
      if (this.m_size < this.static_size)
        ++this.m_size;
      if (0 < this.m_begin)
        --this.m_begin;
      else
        this.m_begin = this.static_size - 1;
      this.front = value;
    }

    public void pop_back()
    {
      if (0 >= this.m_size)
        return;
      --this.m_size;
    }

    public void pop_front()
    {
      if (0 >= this.m_size)
        return;
      --this.m_size;
      ++this.m_begin;
      if (this.static_size > this.m_begin)
        return;
      this.m_begin = 0;
    }

    public int size()
    {
      return this.m_size;
    }

    public int max_size()
    {
      return this.static_size;
    }

    public void clear()
    {
      this.m_size = 0;
    }

    public bool empty()
    {
      return this.m_size == 0;
    }

    public bool full()
    {
      return this.static_size == this.m_size;
    }

    private int indexAt(int index)
    {
      if (this.static_size <= index)
        index %= this.static_size;
      index += this.m_begin;
      if (this.static_size <= index)
        index -= this.static_size;
      return index;
    }
  }
}
