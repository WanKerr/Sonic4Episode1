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
    public class ArrayPoolMultisize<T> where T : new()
    {
        private List<int> arrayElementCapacity_ = new List<int>();
        private List<AppMain.ArrayPoolFast<T>> Arrays_ = new List<AppMain.ArrayPoolFast<T>>();

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
            AppMain.ArrayPoolFast<T> arrayPoolFast = new AppMain.ArrayPoolFast<T>();
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
