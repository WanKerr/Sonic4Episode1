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
    public class MatrixPool
    {
        private List<AppMain.NNS_MATRIX> cache_;

        public MatrixPool()
        {
            this.cache_ = new List<AppMain.NNS_MATRIX>();
        }

        public AppMain.NNS_MATRIX Alloc()
        {
            AppMain.NNS_MATRIX nnsMatrix;
            if (this.cache_.Count > 0)
            {
                int index = this.cache_.Count - 1;
                nnsMatrix = this.cache_[index];
                this.cache_.RemoveAt(index);
            }
            else
                nnsMatrix = new AppMain.NNS_MATRIX();
            return nnsMatrix;
        }

        public void Release(AppMain.NNS_MATRIX obj)
        {
            this.cache_.Add(obj);
        }
    }
}
