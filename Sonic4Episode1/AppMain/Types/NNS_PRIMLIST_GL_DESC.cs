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
    public class NNS_PRIMLIST_GL_DESC
    {
        public uint Mode;
        public int[] pCounts;
        public uint DataType;
        public UShortBuffer[] pIndices;
        public int nPrim;
        public int IndexBufferSize;
        public ByteBuffer pIndexBuffer;
        public uint BufferName;

        public static AppMain.NNS_PRIMLIST_GL_DESC Read(BinaryReader reader, long data0Pos)
        {
            AppMain.NNS_PRIMLIST_GL_DESC nnsPrimlistGlDesc = new AppMain.NNS_PRIMLIST_GL_DESC();
            nnsPrimlistGlDesc.Mode = reader.ReadUInt32();
            uint num1 = reader.ReadUInt32();
            long position1 = reader.BaseStream.Position;
            reader.BaseStream.Seek(data0Pos + (long)num1, SeekOrigin.Begin);
            nnsPrimlistGlDesc.pCounts = new int[1];
            nnsPrimlistGlDesc.pCounts[0] = reader.ReadInt32();
            reader.BaseStream.Seek(position1, SeekOrigin.Begin);
            nnsPrimlistGlDesc.DataType = reader.ReadUInt32();
            uint num2 = reader.ReadUInt32();
            nnsPrimlistGlDesc.nPrim = reader.ReadInt32();
            nnsPrimlistGlDesc.IndexBufferSize = reader.ReadInt32();
            uint num3 = reader.ReadUInt32();
            long position2 = reader.BaseStream.Position;
            reader.BaseStream.Seek(data0Pos + (long)num3, SeekOrigin.Begin);
            byte[] numArray = new byte[nnsPrimlistGlDesc.IndexBufferSize];
            reader.Read(numArray, 0, nnsPrimlistGlDesc.IndexBufferSize);
            nnsPrimlistGlDesc.pIndexBuffer = ByteBuffer.Wrap(numArray);
            reader.BaseStream.Seek(position2, SeekOrigin.Begin);
            nnsPrimlistGlDesc.pIndices = new UShortBuffer[nnsPrimlistGlDesc.nPrim];
            long position3 = reader.BaseStream.Position;
            reader.BaseStream.Seek(data0Pos + (long)num2, SeekOrigin.Begin);
            for (int index = 0; index < nnsPrimlistGlDesc.nPrim; ++index)
            {
                uint num4 = reader.ReadUInt32();
                nnsPrimlistGlDesc.pIndices[index] = (nnsPrimlistGlDesc.pIndexBuffer + ((int)num4 - (int)num3)).AsUShortBuffer();
            }
            reader.BaseStream.Seek(position3, SeekOrigin.Begin);
            nnsPrimlistGlDesc.BufferName = reader.ReadUInt32();
            return nnsPrimlistGlDesc;
        }

        public AppMain.NNS_PRIMLIST_GL_DESC Assign(AppMain.NNS_PRIMLIST_GL_DESC desc)
        {
            this.Mode = desc.Mode;
            this.pCounts = desc.pCounts;
            this.DataType = desc.DataType;
            this.pIndices = desc.pIndices;
            this.nPrim = desc.nPrim;
            this.IndexBufferSize = desc.IndexBufferSize;
            this.pIndexBuffer = desc.pIndexBuffer;
            this.BufferName = desc.BufferName;
            return this;
        }
    }
}
