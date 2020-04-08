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
    public class NNS_MATRIXSTACK
    {
        private readonly AppMain.NNS_MATRIX identity = AppMain.NNS_MATRIX.CreateIdentity();
        private List<AppMain.NNS_MATRIX> stack;
        private AppMain.NNS_MATRIX invert;

        public NNS_MATRIXSTACK(uint uiSize)
        {
            this.stack = new List<AppMain.NNS_MATRIX>((int)uiSize);
        }

        public NNS_MATRIXSTACK()
        {
            this.stack = new List<AppMain.NNS_MATRIX>(16);
        }

        public void push(AppMain.NNS_MATRIX matrix)
        {
            this.invert = (AppMain.NNS_MATRIX)null;
            this.stack.Add(matrix);
        }

        public AppMain.NNS_MATRIX pop()
        {
            this.invert = (AppMain.NNS_MATRIX)null;
            int index = this.stack.Count - 1;
            AppMain.NNS_MATRIX nnsMatrix = this.stack[index];
            this.stack.RemoveAt(index);
            return nnsMatrix;
        }

        public AppMain.NNS_MATRIX get()
        {
            return this.stack.Count == 0 ? this.identity : this.stack[this.stack.Count - 1];
        }

        public void set(AppMain.NNS_MATRIX mtx)
        {
            if (this.stack.Count > 0)
                this.stack[this.stack.Count - 1] = mtx;
            else
                this.push(mtx);
        }

        public void clear()
        {
            this.stack.Clear();
        }

        public AppMain.NNS_MATRIX getInvert()
        {
            if (this.invert == null)
            {
                AppMain.NNS_MATRIX src = this.get();
                this.invert = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
                AppMain.nnInvertMatrix(this.invert, src);
            }
            return this.invert;
        }
    }
}
