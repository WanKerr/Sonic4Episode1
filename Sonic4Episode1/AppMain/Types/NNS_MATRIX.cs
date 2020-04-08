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
    public class NNS_MATRIX : AppMain.IClearable
    {
        public float M00;
        public float M01;
        public float M02;
        public float M03;
        public float M10;
        public float M11;
        public float M12;
        public float M13;
        public float M20;
        public float M21;
        public float M22;
        public float M23;
        public float M30;
        public float M31;
        public float M32;
        public float M33;

        public float M(int row, int col)
        {
            switch (row)
            {
                case 0:
                    switch (col)
                    {
                        case 0:
                            return this.M00;
                        case 1:
                            return this.M01;
                        case 2:
                            return this.M02;
                        default:
                            return this.M03;
                    }
                case 1:
                    switch (col)
                    {
                        case 0:
                            return this.M10;
                        case 1:
                            return this.M11;
                        case 2:
                            return this.M12;
                        default:
                            return this.M13;
                    }
                case 2:
                    switch (col)
                    {
                        case 0:
                            return this.M20;
                        case 1:
                            return this.M21;
                        case 2:
                            return this.M22;
                        default:
                            return this.M23;
                    }
                default:
                    switch (col)
                    {
                        case 0:
                            return this.M30;
                        case 1:
                            return this.M31;
                        case 2:
                            return this.M32;
                        default:
                            return this.M33;
                    }
            }
        }

        public void SetM(int row, int col, float value)
        {
            switch (row)
            {
                case 0:
                    switch (col)
                    {
                        case 0:
                            this.M00 = value;
                            return;
                        case 1:
                            this.M01 = value;
                            return;
                        case 2:
                            this.M02 = value;
                            return;
                        default:
                            this.M03 = value;
                            return;
                    }
                case 1:
                    switch (col)
                    {
                        case 0:
                            this.M10 = value;
                            return;
                        case 1:
                            this.M11 = value;
                            return;
                        case 2:
                            this.M12 = value;
                            return;
                        default:
                            this.M13 = value;
                            return;
                    }
                case 2:
                    switch (col)
                    {
                        case 0:
                            this.M20 = value;
                            return;
                        case 1:
                            this.M21 = value;
                            return;
                        case 2:
                            this.M22 = value;
                            return;
                        default:
                            this.M23 = value;
                            return;
                    }
                default:
                    switch (col)
                    {
                        case 0:
                            this.M30 = value;
                            return;
                        case 1:
                            this.M31 = value;
                            return;
                        case 2:
                            this.M32 = value;
                            return;
                        default:
                            this.M33 = value;
                            return;
                    }
            }
        }

        public void Clear()
        {
            this.M00 = this.M01 = this.M02 = this.M03 = this.M10 = this.M11 = this.M12 = this.M13 = this.M20 = this.M21 = this.M22 = this.M23 = this.M30 = this.M31 = this.M32 = this.M33 = 0.0f;
        }

        public static AppMain.NNS_MATRIX CreateIdentity()
        {
            AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            nnsMatrix.M00 = 1f;
            nnsMatrix.M01 = 0.0f;
            nnsMatrix.M02 = 0.0f;
            nnsMatrix.M03 = 0.0f;
            nnsMatrix.M10 = 0.0f;
            nnsMatrix.M11 = 1f;
            nnsMatrix.M12 = 0.0f;
            nnsMatrix.M13 = 0.0f;
            nnsMatrix.M20 = 0.0f;
            nnsMatrix.M21 = 0.0f;
            nnsMatrix.M22 = 1f;
            nnsMatrix.M23 = 0.0f;
            nnsMatrix.M30 = 0.0f;
            nnsMatrix.M31 = 0.0f;
            nnsMatrix.M32 = 0.0f;
            nnsMatrix.M33 = 1f;
            return nnsMatrix;
        }

        public NNS_MATRIX()
        {
        }

        public NNS_MATRIX(AppMain.NNS_MATRIX matrix)
        {
            this.Assign(matrix);
        }

        public AppMain.NNS_MATRIX Assign(ref AppMain.SNNS_MATRIX matrix)
        {
            this.M00 = matrix.M00;
            this.M01 = matrix.M01;
            this.M02 = matrix.M02;
            this.M03 = matrix.M03;
            this.M10 = matrix.M10;
            this.M11 = matrix.M11;
            this.M12 = matrix.M12;
            this.M13 = matrix.M13;
            this.M20 = matrix.M20;
            this.M21 = matrix.M21;
            this.M22 = matrix.M22;
            this.M23 = matrix.M23;
            this.M30 = matrix.M30;
            this.M31 = matrix.M31;
            this.M32 = matrix.M32;
            this.M33 = matrix.M33;
            return this;
        }

        public AppMain.NNS_MATRIX Assign(AppMain.NNS_MATRIX matrix)
        {
            this.M00 = matrix.M00;
            this.M01 = matrix.M01;
            this.M02 = matrix.M02;
            this.M03 = matrix.M03;
            this.M10 = matrix.M10;
            this.M11 = matrix.M11;
            this.M12 = matrix.M12;
            this.M13 = matrix.M13;
            this.M20 = matrix.M20;
            this.M21 = matrix.M21;
            this.M22 = matrix.M22;
            this.M23 = matrix.M23;
            this.M30 = matrix.M30;
            this.M31 = matrix.M31;
            this.M32 = matrix.M32;
            this.M33 = matrix.M33;
            return this;
        }

        public static explicit operator Matrix(AppMain.NNS_MATRIX m)
        {
            return new Matrix(m.M00, m.M10, m.M20, m.M30, m.M01, m.M11, m.M21, m.M31, m.M02, m.M12, m.M22, m.M32, m.M03, m.M13, m.M23, m.M33);
        }
    }
}
