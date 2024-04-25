using System;

public partial class AppMain
{
    public struct ArrayPointer<T>
    {
        public static readonly ArrayPointer<T> NULL = new ArrayPointer<T>();
        public T[] array;
        public int offset;

        public ArrayPointer(T[] _array)
          : this(_array, 0)
        {
        }

        public ArrayPointer(T[] _array, int _offset)
        {
            this.array = _array;
            this.offset = _offset;
        }

        public static ArrayPointer<T> operator ++(ArrayPointer<T> _pointer)
        {
            ++_pointer.offset;
            return _pointer;
        }

        public static ArrayPointer<T> operator --(ArrayPointer<T> _pointer)
        {
            --_pointer.offset;
            return _pointer;
        }

        public static ArrayPointer<T> operator +(
          ArrayPointer<T> _pointer,
          int _offset)
        {
            _pointer.offset += _offset;
            return _pointer;
        }

        public static ArrayPointer<T> operator -(
          ArrayPointer<T> _pointer,
          int _offset)
        {
            _pointer.offset -= _offset;
            return _pointer;
        }

        public static bool operator <(
          ArrayPointer<T> _pointer1,
          ArrayPointer<T> _pointer2)
        {
            if (_pointer1.array != _pointer2.array)
                throw new InvalidOperationException();
            return _pointer1.offset < _pointer2.offset;
        }

        public static bool operator >(
          ArrayPointer<T> _pointer1,
          ArrayPointer<T> _pointer2)
        {
            if (_pointer1.array != _pointer2.array)
                throw new InvalidOperationException();
            return _pointer1.offset > _pointer2.offset;
        }

        public static bool operator ==(
          ArrayPointer<T> _pointer1,
          ArrayPointer<T> _pointer2)
        {
            return _pointer1.array == _pointer2.array && _pointer1.offset == _pointer2.offset;
        }

        public static bool operator !=(
          ArrayPointer<T> _pointer1,
          ArrayPointer<T> _pointer2)
        {
            return _pointer1.array != _pointer2.array || _pointer1.offset != _pointer2.offset;
        }

        public static T operator ~(ArrayPointer<T> _pointer)
        {
            return _pointer.array[_pointer.offset];
        }

        public T this[int _index]
        {
            get
            {
                return this.array[this.offset + _index];
            }
        }

        public T[] GetInternalArray()
        {
            return this.array;
        }

        public ArrayPointer<T> Clone()
        {
            return new ArrayPointer<T>(this.array, this.offset);
        }

        public static implicit operator ArrayPointer<T>(T[] _array)
        {
            return new ArrayPointer<T>(_array);
        }

        public static implicit operator T(ArrayPointer<T> _pointer)
        {
            return _pointer.array[_pointer.offset];
        }

        public T SetPrimitive(T value)
        {
            this.array[this.offset] = value;
            return value;
        }

        public T SetPrimitive(int index, T value)
        {
            this.array[this.offset + index] = value;
            return value;
        }

        public override int GetHashCode()
        {
            return this.array.GetHashCode() ^ this.offset.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is ArrayPointer<T> arrayPointer && arrayPointer.array == this.array && arrayPointer.offset == this.offset;
        }
    }
}
