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
    public class RGBA_U8_ColorData : OpenGL.GLVertexData
    {
        protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1]
        {
      OpenGL.GLVertexElementType.Color
        };
        private AppMain.RGBA_U8[] data_;
        private int startIndex_;

        public RGBA_U8_ColorData()
        {
        }

        public RGBA_U8_ColorData(AppMain.RGBA_U8[] data, int startIndex)
        {
            this.data_ = data;
            this.startIndex_ = startIndex;
        }

        public void Init(AppMain.RGBA_U8[] data, int startIndex)
        {
            this.data_ = data;
            this.startIndex_ = startIndex;
        }

        public OpenGL.GLVertexElementType[] DataComponents
        {
            get
            {
                return this.compType_;
            }
        }

        public int VertexCount
        {
            get
            {
                return this.data_.Length;
            }
        }

        public void ExtractTo(OpenGL.Vertex[] dst, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                dst[index].Color.A = this.data_[this.startIndex_ + index].a;
                dst[index].Color.R = this.data_[this.startIndex_ + index].r;
                dst[index].Color.G = this.data_[this.startIndex_ + index].g;
                dst[index].Color.B = this.data_[this.startIndex_ + index].b;
            }
        }

        public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
        {
            for (int index1 = 0; index1 < count; ++index1)
            {
                int index2 = index1 + dstOffset;
                int index3 = this.startIndex_ + index1;
                dst[index2].Color = new Color((int)this.data_[index3].r, (int)this.data_[index3].g, (int)this.data_[index3].b, (int)this.data_[index3].a);
            }
        }
    }
}
