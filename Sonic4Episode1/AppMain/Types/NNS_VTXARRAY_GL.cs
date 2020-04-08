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
    public class NNS_VTXARRAY_GL
    {
        public uint Type;
        public int Size;
        public uint DataType;
        public int Stride;
        public ByteBuffer Pointer;
        public OpenGL.GLVertexData Data;

        public NNS_VTXARRAY_GL()
        {
        }

        public NNS_VTXARRAY_GL(AppMain.NNS_VTXARRAY_GL array)
        {
            this.Type = array.Type;
            this.Size = array.Size;
            this.DataType = array.DataType;
            this.Stride = array.Stride;
            this.Pointer = array.Pointer;
            this.Data = array.Data;
        }

        public static AppMain.NNS_VTXARRAY_GL Read(
          BinaryReader reader,
          ByteBuffer vertexBuffer,
          uint vertexBufferOffset,
          int nVertex)
        {
            AppMain.NNS_VTXARRAY_GL nnsVtxarrayGl = new AppMain.NNS_VTXARRAY_GL();
            nnsVtxarrayGl.Type = reader.ReadUInt32();
            nnsVtxarrayGl.Size = reader.ReadInt32();
            nnsVtxarrayGl.DataType = reader.ReadUInt32();
            nnsVtxarrayGl.Stride = reader.ReadInt32();
            uint num = reader.ReadUInt32();
            if (num != 0U)
                nnsVtxarrayGl.Pointer = vertexBuffer + ((int)num - (int)vertexBufferOffset);
            return nnsVtxarrayGl;
        }
    }
}
