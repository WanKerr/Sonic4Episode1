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
    public class ArrayPoolFast<T> where T : new()
    {
        private List<T[]> freeArrays_ = new List<T[]>();
        private List<T[]> usedArrays_ = new List<T[]>();

        public T[] AllocArray(int size)
        {
            int index1 = -1;
            for (int index2 = 0; index2 < this.freeArrays_.Count; ++index2)
            {
                if (size <= this.freeArrays_[index2].Length)
                {
                    index1 = index2;
                    break;
                }
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
            this.freeArrays_.AddRange((IEnumerable<T[]>)this.usedArrays_);
            this.usedArrays_.Clear();
        }

        public void Clear()
        {
            this.freeArrays_.Clear();
            this.usedArrays_.Clear();
        }

        public int GetFreeCount()
        {
            return this.freeArrays_.Count;
        }
    }
}
