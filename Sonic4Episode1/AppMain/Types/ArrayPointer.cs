using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using accel;
using dbg;
using er;
using er.web;
using gs;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using mpp;
using setting;

public partial class AppMain
{
    public struct ArrayPointer<T>
    {
        public static readonly AppMain.ArrayPointer<T> NULL = new AppMain.ArrayPointer<T>();
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

        public static AppMain.ArrayPointer<T> operator ++(AppMain.ArrayPointer<T> _pointer)
        {
            ++_pointer.offset;
            return _pointer;
        }

        public static AppMain.ArrayPointer<T> operator --(AppMain.ArrayPointer<T> _pointer)
        {
            --_pointer.offset;
            return _pointer;
        }

        public static AppMain.ArrayPointer<T> operator +(
          AppMain.ArrayPointer<T> _pointer,
          int _offset)
        {
            _pointer.offset += _offset;
            return _pointer;
        }

        public static AppMain.ArrayPointer<T> operator -(
          AppMain.ArrayPointer<T> _pointer,
          int _offset)
        {
            _pointer.offset -= _offset;
            return _pointer;
        }

        public static bool operator <(
          AppMain.ArrayPointer<T> _pointer1,
          AppMain.ArrayPointer<T> _pointer2)
        {
            if (_pointer1.array != _pointer2.array)
                throw new InvalidOperationException();
            return _pointer1.offset < _pointer2.offset;
        }

        public static bool operator >(
          AppMain.ArrayPointer<T> _pointer1,
          AppMain.ArrayPointer<T> _pointer2)
        {
            if (_pointer1.array != _pointer2.array)
                throw new InvalidOperationException();
            return _pointer1.offset > _pointer2.offset;
        }

        public static bool operator ==(
          AppMain.ArrayPointer<T> _pointer1,
          AppMain.ArrayPointer<T> _pointer2)
        {
            return _pointer1.array == _pointer2.array && _pointer1.offset == _pointer2.offset;
        }

        public static bool operator !=(
          AppMain.ArrayPointer<T> _pointer1,
          AppMain.ArrayPointer<T> _pointer2)
        {
            return _pointer1.array != _pointer2.array || _pointer1.offset != _pointer2.offset;
        }

        public static T operator ~(AppMain.ArrayPointer<T> _pointer)
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

        public AppMain.ArrayPointer<T> Clone()
        {
            return new AppMain.ArrayPointer<T>(this.array, this.offset);
        }

        public static implicit operator AppMain.ArrayPointer<T>(T[] _array)
        {
            return new AppMain.ArrayPointer<T>(_array);
        }

        public static implicit operator T(AppMain.ArrayPointer<T> _pointer)
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
            return obj is AppMain.ArrayPointer<T> arrayPointer && arrayPointer.array == this.array && arrayPointer.offset == this.offset;
        }
    }
}
