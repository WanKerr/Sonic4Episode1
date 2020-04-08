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
    public class NNS_PRIM3D_PCT_TexCoordData : OpenGL.GLVertexData
    {
        protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1]
        {
      OpenGL.GLVertexElementType.TextureCoordinate0
        };
        private AppMain.NNS_PRIM3D_PCT[] data_;
        private int startIndex_;

        public NNS_PRIM3D_PCT_TexCoordData()
        {
        }

        public NNS_PRIM3D_PCT_TexCoordData(AppMain.NNS_PRIM3D_PCT[] data, int startIndex)
        {
            this.data_ = data;
            this.startIndex_ = startIndex;
        }

        public void Init(AppMain.NNS_PRIM3D_PCT[] data, int startIndex)
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
                dst[index].TextureCoordinate.X = this.data_[this.startIndex_ + index].Tex.u;
                dst[index].TextureCoordinate.Y = this.data_[this.startIndex_ + index].Tex.v;
            }
        }

        public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                dst[index + dstOffset].TextureCoordinate.X = this.data_[this.startIndex_ + index].Tex.u;
                dst[index + dstOffset].TextureCoordinate.Y = this.data_[this.startIndex_ + index].Tex.v;
            }
        }
    }
}
