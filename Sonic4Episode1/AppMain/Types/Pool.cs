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
    public class Pool<T> where T : class, new()
    {
        private T[] freeObjects_ = new T[1024];
        private T[] usedObjects_ = new T[1024];
        private int freeObjectsCount_;
        private int usedObjectsCount_;

        public T Alloc()
        {
            T obj;
            if (this.freeObjectsCount_ > 0)
            {
                int index = this.freeObjectsCount_ - 1;
                obj = this.freeObjects_[index];
                this.freeObjects_[index] = default(T);
                --this.freeObjectsCount_;
            }
            else
                obj = new T();
            if (this.usedObjects_.Length == this.usedObjectsCount_)
            {
                T[] objArray = new T[this.usedObjects_.Length * 2];
                Array.Copy((Array)this.usedObjects_, (Array)objArray, this.usedObjects_.Length);
                this.usedObjects_ = objArray;
            }
            this.usedObjects_[this.usedObjectsCount_] = obj;
            ++this.usedObjectsCount_;
            return obj;
        }

        public void Release(T obj)
        {
            if (this.freeObjects_.Length == this.freeObjectsCount_)
            {
                T[] objArray = new T[this.freeObjects_.Length * 2];
                Array.Copy((Array)this.freeObjects_, (Array)objArray, this.freeObjects_.Length);
                this.freeObjects_ = objArray;
            }
            this.freeObjects_[this.freeObjectsCount_] = obj;
            ++this.freeObjectsCount_;
            int num = Array.IndexOf<T>(this.usedObjects_, obj);
            if (num < 0)
                return;
            for (int index = num + 1; index < this.usedObjectsCount_; ++index)
                this.usedObjects_[index - 1] = this.usedObjects_[index];
            --this.usedObjectsCount_;
        }

        public void ReleaseUsedObjects()
        {
            for (int index = 0; index < this.usedObjectsCount_; ++index)
                this.freeObjects_[this.freeObjectsCount_ + index] = this.usedObjects_[index];
            this.freeObjectsCount_ += this.usedObjectsCount_;
            for (int index = 0; index < this.usedObjectsCount_; ++index)
                this.usedObjects_[index] = default(T);
            this.usedObjectsCount_ = 0;
        }

        public void Clear()
        {
            for (int index = 0; index < this.freeObjectsCount_; ++index)
                this.freeObjects_[index] = default(T);
            this.freeObjectsCount_ = 0;
            for (int index = 0; index < this.usedObjectsCount_; ++index)
                this.usedObjects_[index] = default(T);
            this.usedObjectsCount_ = 0;
        }
    }
}
