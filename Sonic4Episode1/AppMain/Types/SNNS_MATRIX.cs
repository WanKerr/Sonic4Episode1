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
    public struct SNNS_MATRIX
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

        public void Assign(AppMain.NNS_MATRIX matrix)
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
        }

        public void Assign(ref AppMain.SNNS_MATRIX matrix)
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
        }
    }
}
