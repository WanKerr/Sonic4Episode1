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
    public class GLVertexBuffer_ : OpenGL.GLVertexData
    {
        private OpenGL.GLVertexElementType[] elementTypes_;
        private AppMain.NNS_VTXARRAY_GL posArray_;
        private AppMain.NNS_VTXARRAY_GL normArray_;
        private AppMain.NNS_VTXARRAY_GL colArray_;
        private AppMain.NNS_VTXARRAY_GL texCoord0Array_;
        private AppMain.NNS_VTXARRAY_GL texCoord1Array_;
        private AppMain.NNS_VTXARRAY_GL blWeightArray_;
        private AppMain.NNS_VTXARRAY_GL blIndexArray_;
        private AppMain.NNS_VTXLIST_GL_DESC pVtxDesc_;

        public GLVertexBuffer_(AppMain.NNS_VTXLIST_GL_DESC pVtxDesc)
        {
            this.pVtxDesc_ = pVtxDesc;
            this.elementTypes_ = new OpenGL.GLVertexElementType[this.pVtxDesc_.nArray];
            for (int index = 0; index < this.pVtxDesc_.nArray; ++index)
            {
                uint type = this.pVtxDesc_.pArray[index].Type;
                if (type <= 8U)
                {
                    switch ((int)type - 1)
                    {
                        case 0:
                            this.posArray_ = this.pVtxDesc_.pArray[index];
                            this.elementTypes_[index] = OpenGL.GLVertexElementType.Position;
                            continue;
                        case 1:
                            this.blWeightArray_ = this.pVtxDesc_.pArray[index];
                            this.elementTypes_[index] = OpenGL.GLVertexElementType.BlendWeight;
                            continue;
                        case 2:
                            continue;
                        case 3:
                            this.blIndexArray_ = this.pVtxDesc_.pArray[index];
                            this.elementTypes_[index] = OpenGL.GLVertexElementType.BlendIndex;
                            continue;
                        default:
                            if (type == 8U)
                            {
                                this.normArray_ = this.pVtxDesc_.pArray[index];
                                this.elementTypes_[index] = OpenGL.GLVertexElementType.Normal;
                                continue;
                            }
                            continue;
                    }
                }
                else if (type != 16U)
                {
                    if (type != 256U)
                    {
                        if (type == 512U)
                        {
                            this.texCoord1Array_ = this.pVtxDesc_.pArray[index];
                            this.elementTypes_[index] = OpenGL.GLVertexElementType.TextureCoordinate1;
                        }
                    }
                    else
                    {
                        this.texCoord0Array_ = this.pVtxDesc_.pArray[index];
                        this.elementTypes_[index] = OpenGL.GLVertexElementType.TextureCoordinate0;
                    }
                }
                else
                {
                    this.colArray_ = this.pVtxDesc_.pArray[index];
                    this.elementTypes_[index] = OpenGL.GLVertexElementType.Color;
                }
            }
        }

        public OpenGL.GLVertexElementType[] DataComponents
        {
            get
            {
                return this.elementTypes_;
            }
        }

        public int VertexCount
        {
            get
            {
                return this.pVtxDesc_.nVertex;
            }
        }

        public void ExtractTo(OpenGL.Vertex[] dst, int count)
        {
            int getOffset1 = 0;
            int getOffset2 = 0;
            int index1 = 0;
            int getOffset3 = 0;
            int getOffset4 = 0;
            int getOffset5 = 0;
            int index2 = 0;
            for (int index3 = 0; index3 < count; ++index3)
            {
                float x1 = this.posArray_.Pointer.GetFloat(getOffset1);
                float y1 = this.posArray_.Pointer.GetFloat(getOffset1 + 4);
                float z1 = this.posArray_.Pointer.GetFloat(getOffset1 + 8);
                dst[index3].Position = new Vector3(x1, y1, z1);
                getOffset1 += this.posArray_.Stride;
                if (this.normArray_ != null)
                {
                    float x2 = this.normArray_.Pointer.GetFloat(getOffset2);
                    float y2 = this.normArray_.Pointer.GetFloat(getOffset2 + 4);
                    float z2 = this.normArray_.Pointer.GetFloat(getOffset2 + 8);
                    dst[index3].Normal = new Vector3(x2, y2, z2);
                    getOffset2 += this.normArray_.Stride;
                }
                if (this.colArray_ != null)
                {
                    byte num1 = this.colArray_.Pointer[index1];
                    byte num2 = this.colArray_.Pointer[index1 + 1];
                    byte num3 = this.colArray_.Pointer[index1 + 2];
                    byte num4 = this.colArray_.Pointer[index1 + 3];
                    dst[index3].Color = new Color((int)num1, (int)num2, (int)num3, (int)num4);
                    index1 += this.colArray_.Stride;
                }
                else
                    dst[index3].Color = Color.White;
                if (this.texCoord0Array_ != null)
                {
                    float x2 = this.texCoord0Array_.Pointer.GetFloat(getOffset3);
                    float y2 = this.texCoord0Array_.Pointer.GetFloat(getOffset3 + 4);
                    dst[index3].TextureCoordinate = new Vector2(x2, y2);
                    getOffset3 += this.texCoord0Array_.Stride;
                }
                if (this.texCoord1Array_ != null)
                {
                    float x2 = this.texCoord1Array_.Pointer.GetFloat(getOffset4);
                    float y2 = this.texCoord1Array_.Pointer.GetFloat(getOffset4 + 4);
                    dst[index3].TextureCoordinate2 = new Vector2(x2, y2);
                    getOffset4 += this.texCoord1Array_.Stride;
                }
                else
                    dst[index3].TextureCoordinate2 = Vector2.Zero;
                if (this.blWeightArray_ != null)
                {
                    float x2 = this.blWeightArray_.Pointer.GetFloat(getOffset5);
                    float y2 = this.blWeightArray_.Size > 1 ? this.blWeightArray_.Pointer.GetFloat(getOffset5 + 4) : 0.0f;
                    float z2 = this.blWeightArray_.Size > 2 ? this.blWeightArray_.Pointer.GetFloat(getOffset5 + 8) : 0.0f;
                    float w = this.blWeightArray_.Size > 3 ? this.blWeightArray_.Pointer.GetFloat(getOffset5 + 12) : 0.0f;
                    dst[index3].BlendWeight = new Vector4(x2, y2, z2, w);
                    getOffset5 += this.blWeightArray_.Stride;
                }
                else
                    dst[index3].BlendWeight = Vector4.UnitX;
                if (this.blIndexArray_ != null)
                {
                    byte num1 = this.blIndexArray_.Pointer[index2];
                    byte num2 = this.blIndexArray_.Size > 1 ? this.blIndexArray_.Pointer[index2 + 1] : (byte)0;
                    byte num3 = this.blIndexArray_.Size > 2 ? this.blIndexArray_.Pointer[index2 + 2] : (byte)0;
                    byte num4 = this.blIndexArray_.Size > 3 ? this.blIndexArray_.Pointer[index2 + 3] : (byte)0;
                    dst[index3].BlendIndices = new Byte4((float)num1, (float)num2, (float)num3, (float)num4);
                    index2 += this.blIndexArray_.Stride;
                }
                else
                    dst[index3].BlendIndices = new Byte4();
            }
        }

        public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
