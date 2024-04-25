// Decompiled with JetBrains decompiler
// Type: mpp.OpenGL
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace mpp
{
    public sealed class OpenGL
    {
        private static int drawArraysCallsCount = 0;
        private static DrawArraysCallContext[] drawArraysCalls = new DrawArraysCallContext[256];
        private static VertexPosTexColNorm[] pendingVertices = new VertexPosTexColNorm[4096];
        private static int pendingVerticesCount = 0;
        private static VertexPosTexColNorm[] drawArraysVertices = new VertexPosTexColNorm[16324];
        private static int verticesToDraw_ = 0;
        private static PrimitiveType prevPrimType = PrimitiveType.TriangleList;
        public static int drawVertexBuffer_Count = 0;
        private static VertexPosTexColNorm[] vtxBuffer = new VertexPosTexColNorm[4096];
        private static Vector4 posShift = new Vector4(-0.5f, -0.5f, 0.0f, 0.0f);
        private static GLVertexData[] texCoordData = new GLVertexData[2];
        public static int drawPrimitives_Count = 0;
        private static bool m_viewportSet = false;
        private static bool m_matrixPaletteOESActive = false;
        private static uint m_currentPaletteMatrixOES = 0;
        private static Color m_color = new Color(1f, 1f, 1f, 0.0f);
        private static Color m_clearColor = new Color();
        private static float m_clearDepth = 1f;
        private static bool m_perPixelLighting = true;
        private static bool m_alphaTestEnabled = false;
        private static CompareFunction m_alphaFunction = CompareFunction.Always;
        private static int m_alphaRef = 0;
        private static bool m_blendEnabled = false;
        private static Dictionary<long, BlendState> m_blendStates = new Dictionary<long, BlendState>();
        private static BlendState m_blendState = new BlendState()
        {
            ColorBlendFunction = BlendFunction.Add,
            AlphaBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.One,
            AlphaSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.Zero,
            AlphaDestinationBlend = Blend.Zero,
            ColorWriteChannels = ColorWriteChannels.All
        };
        private static bool m_vertexColorEnabled = false;
        private static bool m_colorLogicEnabled = false;
        private static bool m_colorMaterialEnabled = false;
        private static bool m_cullFaceEnabled = false;
        private static uint m_frontFace = 2305;
        private static uint m_cullFace = 1029;
        private static RasterizerState m_rasterizerState = new RasterizerState()
        {
            CullMode = CullMode.None,
            ScissorTestEnable = true,
            MultiSampleAntiAlias = false
        };
        private static RasterizerState m_rasterizerStateCW = new RasterizerState()
        {
            CullMode = CullMode.CullClockwiseFace,
            ScissorTestEnable = true,
            MultiSampleAntiAlias = false
        };
        private static RasterizerState m_rasterizerStateCCW = new RasterizerState()
        {
            CullMode = CullMode.CullClockwiseFace,
            ScissorTestEnable = true,
            MultiSampleAntiAlias = false
        };
        private static RasterizerState m_rasterizerStateCN = new RasterizerState()
        {
            CullMode = CullMode.None,
            ScissorTestEnable = true,
            MultiSampleAntiAlias = false
        };
        private static bool m_depthTestEnabled = false;
        private static Dictionary<int, DepthStencilState> m_depthStencils = new Dictionary<int, DepthStencilState>();
        private static DepthStencilState m_depthStencilState = new DepthStencilState()
        {
            DepthBufferFunction = CompareFunction.Less,
            DepthBufferWriteEnable = true
        };
        private static bool m_fogEnabled = false;
        private static Vector3 m_fogColor = new Vector3(0.0f, 0.0f, 0.0f);
        private static ushort m_fogMode = 2048;
        private static float m_fogStart = 0.5f;
        private static float m_fogEnd = 1f;
        private static bool m_lightingEnabled = false;
        private static LightUnit[] m_lightUnit = new LightUnit[3]
        {
            new LightUnit(),
            new LightUnit(),
            new LightUnit()
        };
        private static Vector3 m_ambientColor = new Vector3(0.2f, 0.2f, 0.2f);
        private static Vector3 m_diffuseColor = new Vector3(0.8f, 0.8f, 0.8f);
        private static float m_diffuseColorAlpha = 1f;
        private static Vector3 m_emissiveColor = new Vector3(0.0f, 0.0f, 0.0f);
        private static Vector3 m_specularColor = new Vector3(0.0f, 0.0f, 0.0f);
        private static float m_specularPower = 0.0f;
        private static bool m_matrixPalleteOESEnabled = false;
        private static uint m_activeTextureUnit = 0;
        private static uint m_clientActiveTextureUnit = 0;
        private static Dictionary<int, SamplerState> m_samplerStates = new Dictionary<int, SamplerState>();
        private static TextureUnit[] m_textureUnits = new TextureUnit[2]
        {
            new TextureUnit(),
            new TextureUnit()
        };
        private static uint[] m_usedTexIDs = new uint[8];
        private static Dictionary<uint, Texture2D> m_textures = new Dictionary<uint, Texture2D>(8);
        private static Texture2D m_curTexture = null;
        private static uint[] m_usedBufferIDs = new uint[8];
        public static Dictionary<uint, BufferItem> m_buffers = new Dictionary<uint, BufferItem>(8);
        public static uint m_boundArrayBuffer = 0;
        private static uint m_boundElementArrayBuffer = 0;
        private static bool m_normalArrayEnabled = false;
        private static uint m_normalArrayDataType = 0;
        private static int m_normalArrayStride = 0;
        private static GLVertexData m_normalArray = null;
        private static bool m_colorArrayEnabled = false;
        private static int m_colorArraySize = 0;
        private static uint m_colorArrayDataType = 0;
        private static int m_colorArrayStride = 0;
        private static GLVertexData m_colorArray = null;
        private static bool m_vertexArrayEnabled = false;
        private static int m_vertexArraySize = 0;
        private static uint m_vertexArrayDataType = 0;
        private static int m_vertexArrayStride = 0;
        private static GLVertexData m_vertexArray = null;
        private static int m_weightArraySize = 0;
        private static uint m_weightArrayDataType = 0;
        private static int m_weightArrayStride = 0;
        private static GLVertexData m_weightArray = null;
        private static bool m_matrixIndexArrayEnabled = false;
        private static int m_matrixIndexArraySize = 0;
        private static uint m_matrixIndexArrayDataType = 0;
        private static int m_matrixIndexArrayStride = 0;
        private static GLVertexData m_matrixIndexArray = null;
        private static bool m_secondaryColorArrayEnabled = false;
        private static readonly Matrix PROJECTION_CORRECTION = 
            Matrix.CreateRotationZ(1.570796f) * Matrix.CreateScale(1f, 1.12f, 0.5f) * Matrix.CreateTranslation(0.0f, 0.0f, 0.5f);
        private static VertexPosTexColNorm[][] m_vertices = new VertexPosTexColNorm[2][]
        {
            new VertexPosTexColNorm[32768],
            new VertexPosTexColNorm[2048]
        };
        private static short[] m_indices = null;
        private static Stack<Matrix> m_modelViewMatrixStack = new Stack<Matrix>();
        private static GraphicsDevice m_graphicsDevice;
        private static BasicEffect m_basicEffect;
        private static AlphaTestEffect m_alphaTestEffect;
        private static SkinnedEffect m_skinnedEffect;
        private static DualTextureEffect m_dualTextureEffect;
        private static Viewport m_viewport;
        private static Matrix m_invScreen;
        private static Texture2D m_fakeTexture;
        private static Stack<Matrix> m_projectionMatrixStack;
        private static Stack<Matrix> m_activeMatrixStack;
        private static Matrix[] m_matrixPalleteOES;
        private static bool m_weightArrayEnabled;

        public static void glActiveTexture(uint texture)
        {
            switch (texture)
            {
                case 33984:
                case 33985:
                    m_activeTextureUnit = texture - 33984U;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void glAlphaFunc(uint func, float _ref)
        {
            switch (func)
            {
                case 512:
                    m_alphaFunction = CompareFunction.Never;
                    break;
                case 513:
                    m_alphaFunction = CompareFunction.Less;
                    break;
                case 514:
                    m_alphaFunction = CompareFunction.Equal;
                    break;
                case 515:
                    m_alphaFunction = CompareFunction.LessEqual;
                    break;
                case 516:
                    m_alphaFunction = CompareFunction.Greater;
                    break;
                case 517:
                    m_alphaFunction = CompareFunction.NotEqual;
                    break;
                case 518:
                    m_alphaFunction = CompareFunction.GreaterEqual;
                    break;
                case 519:
                    m_alphaFunction = CompareFunction.Always;
                    break;
                default:
                    throw new ArgumentException();
            }
            _ref = Clamp(_ref, 0.0f, 1f);
            m_alphaRef = (int)(byte.MaxValue * (double)_ref);
        }

        public static void glArrayElement(int i)
        {
            throw new NotImplementedException();
        }

        public static void glBegin(uint mode)
        {
            throw new NotImplementedException();
        }

        public static void glBindBuffer(uint target, uint buffer)
        {
            switch (target)
            {
                case 34962:
                    m_boundArrayBuffer = buffer;
                    break;
                case 34963:
                    m_boundElementArrayBuffer = buffer;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public static void glBindTexture(uint target, uint texture)
        {
            m_textureUnits[(int)m_activeTextureUnit].BoundTexture = texture;
        }

        public static void glBlendEquation(uint mode)
        {
            BlendFunction ColorFunc;
            BlendFunction AlphaFunc;
            switch (mode)
            {
                case 32774:
                    ColorFunc = BlendFunction.Add;
                    AlphaFunc = BlendFunction.Add;
                    break;
                case 32779:
                    ColorFunc = BlendFunction.ReverseSubtract;
                    AlphaFunc = BlendFunction.ReverseSubtract;
                    break;
                default:
                    throw new NotImplementedException();
            }
            long key = CalcBlendStateId(m_blendState.ColorSourceBlend, m_blendState.AlphaSourceBlend, m_blendState.ColorDestinationBlend, m_blendState.AlphaDestinationBlend, m_blendState.ColorWriteChannels, ColorFunc, AlphaFunc);
            BlendState blendState;
            if (!m_blendStates.TryGetValue(key, out blendState))
            {
                blendState = m_blendState.clone();
                blendState.ColorBlendFunction = ColorFunc;
                blendState.AlphaBlendFunction = AlphaFunc;
                m_blendStates[key] = blendState;
            }
            m_blendState = blendState;
        }

        public static void glBlendFunc(uint sfactor, uint dfactor)
        {
            Blend colorSrcBlend;
            Blend alphaSrcBlend;
            switch (sfactor)
            {
                case 1:
                    colorSrcBlend = Blend.One;
                    alphaSrcBlend = Blend.One;
                    break;
                case 770:
                    colorSrcBlend = Blend.SourceAlpha;
                    alphaSrcBlend = Blend.SourceAlpha;
                    break;
                default:
                    throw new NotImplementedException();
            }
            Blend colorDstBlend;
            Blend alphaDstBlend;
            switch (dfactor)
            {
                case 1:
                    colorDstBlend = Blend.One;
                    alphaDstBlend = Blend.One;
                    break;
                case 771:
                    colorDstBlend = Blend.InverseSourceAlpha;
                    alphaDstBlend = Blend.InverseSourceAlpha;
                    break;
                default:
                    throw new NotImplementedException();
            }
            long key = CalcBlendStateId(colorSrcBlend, alphaSrcBlend, colorDstBlend, alphaDstBlend, m_blendState.ColorWriteChannels, m_blendState.ColorBlendFunction, m_blendState.AlphaBlendFunction);
            BlendState blendState;
            if (!m_blendStates.TryGetValue(key, out blendState))
            {
                blendState = m_blendState.clone();
                blendState.ColorSourceBlend = colorSrcBlend;
                blendState.AlphaSourceBlend = alphaSrcBlend;
                blendState.ColorDestinationBlend = colorDstBlend;
                blendState.AlphaDestinationBlend = alphaDstBlend;
                m_blendStates[key] = blendState;
            }
            m_blendState = blendState;
        }

        public static void glBufferVertexData(GLVertexData data)
        {
            VertexBufferDesc vertexBufferDesc = new VertexBufferDesc();
            vertexBufferDesc.Components = (GLVertexElementType[])data.DataComponents.Clone();
            Vertex[] vertexArray = new Vertex[data.VertexCount];
            for (int index = 0; index < data.VertexCount; ++index)
            {
                vertexArray[index].Color = Color.White;
                vertexArray[index].Normal = Vector3.Backward;
            }
            data.ExtractTo(vertexArray, data.VertexCount);
            VertexBuffer vertexBuffer = new VertexBuffer(m_graphicsDevice, Vertex.VertexDeclaration, vertexArray.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertexArray);
            ((VertexBufferDesc)m_buffers[m_boundArrayBuffer].buffer)?.Buffer.Dispose();
            vertexBufferDesc.Buffer = vertexBuffer;
            m_buffers[m_boundArrayBuffer].buffer = vertexBufferDesc;
            m_buffers[m_boundArrayBuffer].rawData = vertexArray;
        }

        public static void glBufferIndexData(GLIndexBuffer data)
        {
            ushort[] numArray1 = new ushort[data.Size];
            data.ExtractTo(numArray1);
            IndexBuffer indexBuffer = new IndexBuffer(m_graphicsDevice, IndexElementSize.SixteenBits, data.Size, BufferUsage.None);
            indexBuffer.SetData(numArray1);
            ((GraphicsResource)m_buffers[m_boundElementArrayBuffer].buffer)?.Dispose();
            short[] numArray2 = new short[data.Size];
            Buffer.BlockCopy(numArray1, 0, numArray2, 0, data.Size * 2);
            m_buffers[m_boundElementArrayBuffer].buffer = indexBuffer;
            m_buffers[m_boundElementArrayBuffer].rawData = numArray2;
        }

        public static void glBufferData(uint target, int size, ByteBuffer data, uint usage)
        {
            throw new NotImplementedException();
        }

        public static void glClear(uint mask)
        {
            flushDrawArrays();
            ClearOptions options = 0;
            if (((int)mask & 16384) != 0)
                options |= ClearOptions.Target;
            if (((int)mask & 256) != 0)
                options |= ClearOptions.DepthBuffer;
            if (options == 0)
                throw new ArgumentException();
            m_graphicsDevice.Clear(options, m_clearColor, m_clearDepth, 0);
        }

        public static void glClearColor(float red, float green, float blue, float alpha)
        {
            m_clearColor.R = (byte)(byte.MaxValue * (double)Clamp(red, 0.0f, 1f));
            m_clearColor.G = (byte)(byte.MaxValue * (double)Clamp(green, 0.0f, 1f));
            m_clearColor.B = (byte)(byte.MaxValue * (double)Clamp(blue, 0.0f, 1f));
            m_clearColor.A = (byte)(byte.MaxValue * (double)Clamp(alpha, 0.0f, 1f));
        }

        public static void glClearDepth(double depth)
        {
            m_clearDepth = (float)depth;
        }

        public static void glClearDepthf(float depth)
        {
            m_clearDepth = Clamp(depth, 0.0f, 1f);
        }

        public static void glClientActiveTexture(uint texture)
        {
            switch (texture)
            {
                case 33984:
                case 33985:
                    m_clientActiveTextureUnit = texture - 33984U;
                    break;
            }
        }

        public static void glColor3f(float red, float green, float blue)
        {
            throw new NotImplementedException();
        }

        public static void glColor3fv(float[] v)
        {
            throw new NotImplementedException();
        }

        public static void glColor4f(float red, float green, float blue, float alpha)
        {
            m_color.R = (byte)(red * (double)byte.MaxValue);
            m_color.G = (byte)(green * (double)byte.MaxValue);
            m_color.B = (byte)(blue * (double)byte.MaxValue);
            m_color.A = (byte)(alpha * (double)byte.MaxValue);
        }

        public static void glColor4fv(glArray4f v)
        {
            m_color.R = (byte)(byte.MaxValue * (double)v.f0);
            m_color.G = (byte)(byte.MaxValue * (double)v.f1);
            m_color.B = (byte)(byte.MaxValue * (double)v.f2);
            m_color.A = (byte)(byte.MaxValue * (double)v.f3);
        }

        public static void glColor4fv(float[] v)
        {
            m_color.R = (byte)(byte.MaxValue * (double)v[0]);
            m_color.G = (byte)(byte.MaxValue * (double)v[1]);
            m_color.B = (byte)(byte.MaxValue * (double)v[2]);
            m_color.A = (byte)(byte.MaxValue * (double)v[3]);
        }

        public static void glColorMask(byte red, byte green, byte blue, byte alpha)
        {
            ColorWriteChannels channels = ColorWriteChannels.None;
            if (red != 0)
                channels |= ColorWriteChannels.Red;
            if (green != 0)
                channels |= ColorWriteChannels.Green;
            if (blue != 0)
                channels |= ColorWriteChannels.Blue;
            if (alpha != 0)
                channels |= ColorWriteChannels.Alpha;
            long key = CalcBlendStateId(m_blendState.ColorSourceBlend, m_blendState.AlphaSourceBlend, m_blendState.ColorDestinationBlend, m_blendState.AlphaDestinationBlend, channels, m_blendState.ColorBlendFunction, m_blendState.AlphaBlendFunction);
            BlendState blendState;
            if (!m_blendStates.TryGetValue(key, out blendState))
            {
                blendState = m_blendState.clone();
                blendState.ColorWriteChannels = channels;
                m_blendStates[key] = blendState;
            }
            m_blendState = blendState;
        }

        public static void glColorPointer(
          int size,
          uint type,
          int stride,
          GLVertexData pointer)
        {
            m_colorArraySize = size;
            m_colorArrayDataType = type;
            m_colorArrayStride = stride == 0 ? SizeOf(type) * size : stride;
            m_colorArray = pointer;
        }

        public static void glCullFace(uint mode)
        {
            switch (mode)
            {
                case 1028:
                    m_rasterizerState = 2305U == m_frontFace ? m_rasterizerStateCCW : m_rasterizerStateCW;
                    break;
                case 1029:
                    m_rasterizerState = 2305U == m_frontFace ? m_rasterizerStateCW : m_rasterizerStateCCW;
                    break;
                default:
                    throw new NotImplementedException();
            }
            m_cullFace = mode;
        }

        public static void glCurrentPaletteMatrixOES(uint matrixpaletteindex)
        {
            if (matrixpaletteindex >= 32U)
                throw new ArgumentException();
            m_currentPaletteMatrixOES = matrixpaletteindex;
        }

        public static void glDeleteBuffers(int n, uint[] buffers)
        {
            for (int index = 0; index < n; ++index)
            {
                uint buffer = buffers[index];
                BufferItem bufferItem;
                if (!m_buffers.TryGetValue(buffer, out bufferItem))
                    throw new ArgumentException();
                if (bufferItem.buffer != null)
                {
                    if (bufferItem.buffer is VertexBufferDesc)
                        ((VertexBufferDesc)bufferItem.buffer).Buffer.Dispose();
                    else
                        ((GraphicsResource)bufferItem.buffer).Dispose();
                }
                m_buffers.Remove(buffer);
                uint num1 = buffer - 1U;
                uint num2 = num1 / 32U;
                uint num3 = 1U << (int)(num1 & 31U);
                m_usedBufferIDs[(int)num2] &= ~num3;
            }
        }

        public static void glDeleteTextures(int n, uint[] textures)
        {
            for (int index = 0; index < n; ++index)
            {
                uint texture = textures[index];
                Texture2D texture2D;
                if (!m_textures.TryGetValue(texture, out texture2D))
                    throw new ArgumentException();
                texture2D?.Dispose();
                m_textures.Remove(texture);
                uint num1 = texture - 1U;
                uint num2 = num1 / 32U;
                uint num3 = 1U << (int)(num1 & 31U);
                m_usedTexIDs[(int)num2] &= ~num3;
            }
        }

        public static void glDepthFunc(uint func)
        {
            CompareFunction depthBufferFunction;
            switch (func)
            {
                case 512:
                    depthBufferFunction = CompareFunction.Never;
                    break;
                case 513:
                    depthBufferFunction = CompareFunction.Less;
                    break;
                case 514:
                    depthBufferFunction = CompareFunction.Equal;
                    break;
                case 515:
                    depthBufferFunction = CompareFunction.LessEqual;
                    break;
                case 516:
                    depthBufferFunction = CompareFunction.Greater;
                    break;
                case 517:
                    depthBufferFunction = CompareFunction.NotEqual;
                    break;
                case 518:
                    depthBufferFunction = CompareFunction.GreaterEqual;
                    break;
                case 519:
                    depthBufferFunction = CompareFunction.Always;
                    break;
                default:
                    throw new ArgumentException();
            }
            int key = CalcDepthStencilStateId(depthBufferFunction, m_depthStencilState.DepthBufferWriteEnable);
            DepthStencilState depthStencilState;
            if (!m_depthStencils.TryGetValue(key, out depthStencilState))
            {
                depthStencilState = m_depthStencilState.clone();
                depthStencilState.DepthBufferFunction = depthBufferFunction;
                m_depthStencils[key] = depthStencilState;
            }
            m_depthStencilState = depthStencilState;
        }

        public static void glDepthMask(byte flag)
        {
            glDepthMask(0 != flag);
        }

        public static void glDepthMask(bool flag)
        {
            if (m_depthStencilState.DepthBufferWriteEnable == flag)
                return;
            int key = CalcDepthStencilStateId(m_depthStencilState.DepthBufferFunction, flag);
            DepthStencilState depthStencilState;
            if (!m_depthStencils.TryGetValue(key, out depthStencilState))
            {
                depthStencilState = m_depthStencilState.clone();
                depthStencilState.DepthBufferWriteEnable = flag;
                m_depthStencils[key] = depthStencilState;
            }
            m_depthStencilState = depthStencilState;
        }

        public static void glDisable(uint cap)
        {
            switch (cap)
            {
                case 2884:
                    m_cullFaceEnabled = false;
                    break;
                case 2896:
                    m_lightingEnabled = false;
                    break;
                case 2903:
                    m_colorMaterialEnabled = false;
                    break;
                case 2912:
                    m_fogEnabled = false;
                    break;
                case 2929:
                    m_depthTestEnabled = false;
                    break;
                case 3008:
                    m_alphaTestEnabled = false;
                    break;
                case 3042:
                    m_blendEnabled = false;
                    break;
                case 3058:
                    m_colorLogicEnabled = false;
                    break;
                case 3553:
                    m_textureUnits[(int)m_activeTextureUnit].Enabled = false;
                    break;
                case 16384:
                    m_lightUnit[0].Enabled = false;
                    break;
                case 16385:
                    m_lightUnit[1].Enabled = false;
                    break;
                case 16386:
                    m_lightUnit[2].Enabled = false;
                    break;
                case 16387:
                    break;
                case 16388:
                    break;
                case 16389:
                    break;
                case 16390:
                    break;
                case 16391:
                    break;
                case 34880:
                    m_matrixPalleteOESEnabled = false;
                    m_matrixPaletteOESActive = false;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void glEnable(uint cap)
        {
            switch (cap)
            {
                case 2884:
                    m_cullFaceEnabled = true;
                    break;
                case 2896:
                    m_lightingEnabled = true;
                    break;
                case 2903:
                    m_colorMaterialEnabled = true;
                    break;
                case 2912:
                    m_fogEnabled = true;
                    break;
                case 2929:
                    m_depthTestEnabled = true;
                    break;
                case 3008:
                    m_alphaTestEnabled = true;
                    break;
                case 3042:
                    m_blendEnabled = true;
                    break;
                case 3058:
                    m_colorLogicEnabled = true;
                    break;
                case 3553:
                    m_textureUnits[(int)m_activeTextureUnit].Enabled = true;
                    break;
                case 16384:
                    m_lightUnit[0].Enabled = true;
                    break;
                case 16385:
                    m_lightUnit[1].Enabled = true;
                    break;
                case 16386:
                    m_lightUnit[2].Enabled = true;
                    break;
                case 16387:
                    break;
                case 16388:
                    break;
                case 16389:
                    break;
                case 16390:
                    break;
                case 16391:
                    break;
                case 34880:
                    m_matrixPalleteOESEnabled = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void glDisableClientState(uint cap)
        {
            switch (cap)
            {
                case 32884:
                    m_vertexArrayEnabled = false;
                    break;
                case 32885:
                    m_normalArrayEnabled = false;
                    break;
                case 32886:
                    m_colorArrayEnabled = false;
                    break;
                case 32888:
                    m_textureUnits[(int)m_clientActiveTextureUnit].TexCoordArrayEnabled = false;
                    break;
                case 33886:
                    m_secondaryColorArrayEnabled = false;
                    break;
                case 34477:
                    m_weightArrayEnabled = false;
                    break;
                case 34884:
                    m_matrixIndexArrayEnabled = false;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void glEnableClientState(uint cap)
        {
            switch (cap)
            {
                case 32884:
                    m_vertexArrayEnabled = true;
                    break;
                case 32885:
                    m_normalArrayEnabled = true;
                    break;
                case 32886:
                    m_colorArrayEnabled = true;
                    break;
                case 32888:
                    m_textureUnits[(int)m_clientActiveTextureUnit].TexCoordArrayEnabled = true;
                    break;
                case 33886:
                    m_secondaryColorArrayEnabled = true;
                    break;
                case 34477:
                    m_weightArrayEnabled = true;
                    break;
                case 34884:
                    m_matrixIndexArrayEnabled = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void storeDrawArraysContext()
        {
            drawArraysCalls[drawArraysCallsCount].isPending = true;
            drawArraysCalls[drawArraysCallsCount].view = Matrix.Identity;
            drawArraysCalls[drawArraysCallsCount].projection = m_projectionMatrixStack.Peek() * PROJECTION_CORRECTION;
            drawArraysCalls[drawArraysCallsCount].alphaTestEnabled = m_alphaTestEnabled;
            drawArraysCalls[drawArraysCallsCount].normalArrayEnabled = m_normalArrayEnabled;
            drawArraysCalls[drawArraysCallsCount].vertexColorEnabled = m_vertexColorEnabled;
            drawArraysCalls[drawArraysCallsCount].referenceAlpha = m_alphaRef;
            drawArraysCalls[drawArraysCallsCount].alphaFunction = m_alphaFunction;
            drawArraysCalls[drawArraysCallsCount].blendState = m_blendEnabled ? m_blendState : BlendState.Opaque;
            drawArraysCalls[drawArraysCallsCount].depthStencilState = m_depthTestEnabled ? m_depthStencilState : DepthStencilState.None;
            drawArraysCalls[drawArraysCallsCount].rasterizerState = m_cullFaceEnabled ? m_rasterizerState : m_rasterizerStateCN;
            drawArraysCalls[drawArraysCallsCount].samplerState = m_textureUnits[0].SamplerState;
            drawArraysCalls[drawArraysCallsCount].ambientColor = m_ambientColor;
            drawArraysCalls[drawArraysCallsCount].diffuseColor = m_diffuseColor;
            drawArraysCalls[drawArraysCallsCount].diffuseColorAlpha = m_diffuseColorAlpha;
            drawArraysCalls[drawArraysCallsCount].emissiveColor = m_emissiveColor;
            drawArraysCalls[drawArraysCallsCount].specularColor = m_specularColor;
            drawArraysCalls[drawArraysCallsCount].specularPower = m_specularPower;
            drawArraysCalls[drawArraysCallsCount].fogEnabled = m_fogEnabled;
            drawArraysCalls[drawArraysCallsCount].fogStart = m_fogStart;
            drawArraysCalls[drawArraysCallsCount].fogEnd = m_fogEnd;
            drawArraysCalls[drawArraysCallsCount].fogColor = m_fogColor;
            bool flag = m_textureUnits[0].TexCoordArrayEnabled && m_textureUnits[0].Enabled;
            drawArraysCalls[drawArraysCallsCount].textureEnabled = flag;
            drawArraysCalls[drawArraysCallsCount].texture = flag ? m_textures[m_textureUnits[0].BoundTexture] : null;
            drawArraysCalls[drawArraysCallsCount].DirLight0Enabled = m_lightUnit[0].Enabled;
            if (m_lightUnit[0].Enabled)
            {
                drawArraysCalls[drawArraysCallsCount].DirLight0DiffuseColor = m_lightUnit[0].DiffuseColor;
                drawArraysCalls[drawArraysCallsCount].DirLight0SpecularColor = m_lightUnit[0].SpecularColor;
                drawArraysCalls[drawArraysCallsCount].DirLight0Direction = m_lightUnit[0].Direction;
            }
            drawArraysCalls[drawArraysCallsCount].DirLight1Enabled = m_lightUnit[1].Enabled;
            if (m_lightUnit[1].Enabled)
            {
                drawArraysCalls[drawArraysCallsCount].DirLight1DiffuseColor = m_lightUnit[1].DiffuseColor;
                drawArraysCalls[drawArraysCallsCount].DirLight1SpecularColor = m_lightUnit[1].SpecularColor;
                drawArraysCalls[drawArraysCallsCount].DirLight1Direction = m_lightUnit[1].Direction;
            }
            drawArraysCalls[drawArraysCallsCount].DirLight2Enabled = m_lightUnit[2].Enabled;
            if (m_lightUnit[2].Enabled)
            {
                drawArraysCalls[drawArraysCallsCount].DirLight2DiffuseColor = m_lightUnit[2].DiffuseColor;
                drawArraysCalls[drawArraysCallsCount].DirLight2SpecularColor = m_lightUnit[2].SpecularColor;
                drawArraysCalls[drawArraysCallsCount].DirLight2Direction = m_lightUnit[2].Direction;
            }
            ++drawArraysCallsCount;
        }

        private static void flushDrawArrays()
        {
            int index1 = 0;
            int arraysCallsCount = drawArraysCallsCount;
        label_13:
            while (drawArraysCallsCount > 0)
            {
                int destinationIndex1 = 0;
                --drawArraysCallsCount;
                drawArraysCalls[index1].isPending = false;
                Array.Copy(pendingVertices, drawArraysCalls[index1].vertexArrayOffset, drawArraysVertices, destinationIndex1, drawArraysCalls[index1].vertexCount);
                int destinationIndex2 = destinationIndex1 + drawArraysCalls[index1].vertexCount;
                for (int index2 = index1 + 1; index2 < arraysCallsCount; ++index2)
                {
                    if (drawArraysCalls[index2].isPending)
                    {
                        if (drawArraysCalls[index1].isEqualTo(ref drawArraysCalls[index2]))
                        {
                            --drawArraysCallsCount;
                            drawArraysCalls[index2].isPending = false;
                            Array.Copy(pendingVertices, drawArraysCalls[index2].vertexArrayOffset, drawArraysVertices, destinationIndex2, drawArraysCalls[index2].vertexCount);
                            destinationIndex2 += drawArraysCalls[index2].vertexCount;
                        }
                        else if (drawArraysCalls[index1].blendState != drawArraysCalls[index2].blendState || drawArraysCalls[index1].depthStencilState != drawArraysCalls[index2].depthStencilState || drawArraysCalls[index2].blendState == BlendState.Opaque)
                            break;
                    }
                }
                drawPrimitives(drawArraysVertices, PrimitiveType.TriangleList, destinationIndex2 / 3, ref drawArraysCalls[index1]);
                for (; index1 < arraysCallsCount && !drawArraysCalls[index1].isPending; ++index1)
                    drawArraysCalls[index1].texture = null;
                while (true)
                {
                    if (arraysCallsCount > index1 && !drawArraysCalls[arraysCallsCount - 1].isPending)
                    {
                        drawArraysCalls[arraysCallsCount - 1].texture = null;
                        --arraysCallsCount;
                    }
                    else
                        goto label_13;
                }
            }
            pendingVerticesCount = 0;
            drawArraysCallsCount = 0;
        }

        public static void glDrawArrays(uint mode, int first, int count)
        {
            if (!m_vertexArrayEnabled)
                return;
            int num = count;
            PrimitiveType primitiveType;
            int primitiveCount;
            switch (mode)
            {
                case 4:
                    primitiveType = PrimitiveType.TriangleList;
                    primitiveCount = count / 3;
                    break;
                case 5:
                    primitiveType = PrimitiveType.TriangleStrip;
                    primitiveCount = count - 2;
                    num = (count - 2) * 3;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (primitiveType == PrimitiveType.TriangleStrip || count > 64)
            {
                flushDrawArrays();
                bool twoPasses;
                initVertices(count, out twoPasses);
                verticesToDraw_ = count;
                drawPrimitives(primitiveType, primitiveCount, false, twoPasses);
            }
            else
            {
                prevPrimType = primitiveType;
                if (drawArraysCallsCount >= drawArraysCalls.Length)
                    flushDrawArrays();
                if (pendingVertices.Length < pendingVerticesCount + num)
                    pendingVertices = new VertexPosTexColNorm[pendingVertices.Length * 2];
                _glDrawArrays_PrepareVertices(primitiveType, pendingVertices, pendingVerticesCount, count);
                storeDrawArraysContext();
                drawArraysCalls[drawArraysCallsCount - 1].vertexArrayOffset = pendingVerticesCount;
                drawArraysCalls[drawArraysCallsCount - 1].vertexCount = num;
                pendingVerticesCount += num;
            }
        }

        public static void glDrawElements(uint mode, int count, uint type, ushort[] indices)
        {
            if (!m_vertexArrayEnabled)
                return;
            if (5123U != type)
                throw new ArgumentException();
            PrimitiveType primitiveType;
            int primitiveCount;
            switch (mode)
            {
                case 4:
                    primitiveType = PrimitiveType.TriangleList;
                    primitiveCount = count / 3;
                    break;
                case 5:
                    primitiveType = PrimitiveType.TriangleStrip;
                    primitiveCount = count - 2;
                    break;
                default:
                    throw new NotImplementedException();
            }
            BufferItem buffer1 = m_buffers[m_boundArrayBuffer];
            VertexBufferDesc buffer2 = (VertexBufferDesc)buffer1.buffer;
            if (Array.IndexOf(buffer2.Components, GLVertexElementType.Normal) < 0)
                m_ambientColor = new Vector3(1f, 1f, 1f);
            VertexBuffer buffer3 = buffer2.Buffer;
            BufferItem buffer4 = m_buffers[m_boundElementArrayBuffer];
            bool useVB = true;
            if (m_textureUnits[0].TexCoordArrayEnabled && m_textureUnits[0].Enabled || buffer2.vertices == null)
            {
                Matrix matrix = m_textureUnits[0].MatrixStack.Peek();
                Matrix other = m_textureUnits[1].MatrixStack.Peek();
                bool flag = !buffer2.TextureMatrix.Equals(matrix);
                if (!buffer2.Texture2Matrix.Equals(other))
                {
                    int num = m_textureUnits[1].Enabled ? 1 : 0;
                }
                if (flag || buffer2.vertices == null)
                {
                    useVB = m_matrixPalleteOESEnabled || count > 32;
                    if (buffer2.vertices == null)
                    {
                        buffer2.vertices = new Vertex[buffer3.VertexCount];
                        useVB = true;
                    }
                    Array.Copy((Array)buffer1.rawData, buffer2.vertices, buffer3.VertexCount);
                    for (int index = 0; index < buffer2.vertices.Length; ++index)
                    {
                        if (flag)
                            Vector2.Transform(ref buffer2.vertices[index].TextureCoordinate, ref matrix, out buffer2.vertices[index].TextureCoordinate);
                    }
                    if (useVB)
                        buffer3.SetData(buffer2.vertices);
                    else
                        buffer2.needSetBufferData = true;
                    buffer2.TextureMatrix = matrix;
                    buffer2.Texture2Matrix = other;
                }
            }
            flushDrawArrays();
            if (!useVB)
            {
                int index = 0;
                int pendingVerticesCount = OpenGL.pendingVerticesCount;
                ushort[] rawData = (ushort[])m_buffers[m_boundElementArrayBuffer].rawData;
                for (; index < count; ++index)
                {
                    m_vertices[0][index].Position = buffer2.vertices[rawData[index]].Position;
                    m_vertices[0][index].Color = buffer2.vertices[rawData[index]].Color;
                    m_vertices[0][index].Normal = buffer2.vertices[rawData[index]].Normal;
                    m_vertices[0][index].TextureCoordinate = buffer2.vertices[rawData[index]].TextureCoordinate;
                }
                verticesToDraw_ = count;
                drawPrimitives(primitiveType, primitiveCount, false, false);
            }
            else
            {
                if (m_matrixPalleteOESEnabled && m_alphaTestEnabled && count <= 32)
                    useVB = false;
                if (useVB && buffer2.needSetBufferData)
                {
                    buffer3.SetData(buffer2.vertices);
                    buffer2.needSetBufferData = false;
                }
                drawVertexBuffer(primitiveType, primitiveCount, buffer1, buffer4, useVB);
            }
        }

        private static void setupEffect(SkinnedEffect effect)
        {
            effect.DiffuseColor = m_diffuseColor;
            effect.Alpha = m_diffuseColorAlpha;
            effect.EmissiveColor = m_emissiveColor;
            effect.SpecularColor = m_specularColor;
            effect.SpecularPower = m_specularPower;
            setupEffect<SkinnedEffect>(effect);
            effect.PreferPerPixelLighting = false;
            effect.Texture = m_textureUnits[0].Enabled ? m_textures[m_textureUnits[0].BoundTexture] : m_fakeTexture;
            effect.SetBoneTransforms(m_matrixPalleteOES);
            if (m_normalArrayEnabled)
                return;
            m_skinnedEffect.DirectionalLight0.Enabled = false;
        }

        private static void setupEffect(BasicEffect effect)
        {
            effect.VertexColorEnabled = m_vertexColorEnabled || m_colorArrayEnabled;
            effect.DiffuseColor = m_diffuseColor;
            effect.Alpha = m_diffuseColorAlpha;
            effect.EmissiveColor = m_emissiveColor;
            effect.SpecularColor = m_specularColor;
            effect.SpecularPower = m_specularPower;
            effect.PreferPerPixelLighting = false;
            effect.TextureEnabled = m_textureUnits[0].TexCoordArrayEnabled && m_textureUnits[0].Enabled;
            effect.Texture = effect.TextureEnabled ? m_textures[m_textureUnits[0].BoundTexture] : m_fakeTexture;
            if (!m_normalArrayEnabled)
                effect.LightingEnabled = false;
            setupEffect<BasicEffect>(effect);
        }

        private static void setupEffect(AlphaTestEffect effect)
        {
            effect.VertexColorEnabled = m_vertexColorEnabled || m_colorArrayEnabled;
            effect.DiffuseColor = m_diffuseColor;
            effect.Alpha = m_diffuseColorAlpha;
            bool flag = m_textureUnits[0].TexCoordArrayEnabled && m_textureUnits[0].Enabled;
            effect.Texture = flag ? m_textures[m_textureUnits[0].BoundTexture] : m_fakeTexture;
            effect.AlphaFunction = m_alphaFunction;
            effect.ReferenceAlpha = m_alphaRef;
            effect.View = m_modelViewMatrixStack.Peek();
            effect.Projection = m_projectionMatrixStack.Peek() * PROJECTION_CORRECTION;
            effect.FogEnabled = m_fogEnabled && 9729 == m_fogMode;
            effect.FogColor = m_fogColor;
            effect.FogStart = m_fogStart;
            effect.FogEnd = m_fogEnd;
        }

        private static void setupEffect(
          BasicEffect effect,
          ref DrawArraysCallContext callContext)
        {
            effect.VertexColorEnabled = callContext.vertexColorEnabled;
            effect.DiffuseColor = callContext.diffuseColor;
            effect.Alpha = callContext.diffuseColorAlpha;
            effect.EmissiveColor = callContext.emissiveColor;
            effect.SpecularColor = callContext.specularColor;
            effect.SpecularPower = callContext.specularPower;
            effect.PreferPerPixelLighting = false;
            effect.TextureEnabled = callContext.texture != null;
            effect.Texture = callContext.texture;
            if (!callContext.normalArrayEnabled)
                effect.LightingEnabled = false;
            setupEffect<BasicEffect>(effect, ref callContext);
        }

        private static void setupEffect(
          AlphaTestEffect effect,
          ref DrawArraysCallContext callContext)
        {
            effect.VertexColorEnabled = callContext.vertexColorEnabled;
            effect.DiffuseColor = callContext.diffuseColor;
            effect.Alpha = callContext.diffuseColorAlpha;
            effect.Texture = callContext.textureEnabled ? callContext.texture : m_fakeTexture;
            effect.AlphaFunction = callContext.alphaFunction;
            effect.ReferenceAlpha = callContext.referenceAlpha;
            effect.View = callContext.view;
            effect.Projection = callContext.projection;
            effect.FogEnabled = callContext.fogEnabled;
            effect.FogColor = callContext.fogColor;
            effect.FogStart = callContext.fogStart;
            effect.FogEnd = callContext.fogEnd;
        }

        private static void setupEffect<TEffect>(TEffect effect) where TEffect : Effect, IEffectMatrices, IEffectLights, IEffectFog
        {
            effect.View = m_modelViewMatrixStack.Peek();
            effect.Projection = m_projectionMatrixStack.Peek() * PROJECTION_CORRECTION;
            effect.FogEnabled = m_fogEnabled && 9729 == m_fogMode;
            effect.FogColor = m_fogColor;
            effect.FogStart = m_fogStart;
            effect.FogEnd = m_fogEnd;
            effect.AmbientLightColor = m_ambientColor;
            effect.LightingEnabled |= m_lightingEnabled;
            effect.DirectionalLight0.Enabled = m_lightUnit[0].Enabled;
            effect.DirectionalLight0.DiffuseColor = m_lightUnit[0].DiffuseColor;
            effect.DirectionalLight0.SpecularColor = m_lightUnit[0].SpecularColor;
            effect.DirectionalLight0.Direction = m_lightUnit[0].Direction;
            effect.DirectionalLight1.Enabled = m_lightUnit[1].Enabled;
            effect.DirectionalLight1.DiffuseColor = m_lightUnit[1].DiffuseColor;
            effect.DirectionalLight1.SpecularColor = m_lightUnit[1].SpecularColor;
            effect.DirectionalLight1.Direction = m_lightUnit[1].Direction;
            effect.DirectionalLight2.Enabled = m_lightUnit[2].Enabled;
            effect.DirectionalLight2.DiffuseColor = m_lightUnit[2].DiffuseColor;
            effect.DirectionalLight2.SpecularColor = m_lightUnit[2].SpecularColor;
            effect.DirectionalLight2.Direction = m_lightUnit[2].Direction;
        }

        private static void setupEffect<TEffect>(
          TEffect effect,
          ref DrawArraysCallContext callContext)
          where TEffect : Effect, IEffectMatrices, IEffectLights, IEffectFog
        {
            effect.Projection = callContext.projection;
            effect.View = callContext.view;
            effect.FogEnabled = callContext.fogEnabled;
            effect.FogColor = callContext.fogColor;
            effect.FogStart = callContext.fogStart;
            effect.FogEnd = callContext.fogEnd;
            effect.AmbientLightColor = callContext.ambientColor;
            effect.DirectionalLight0.Enabled = callContext.DirLight0Enabled;
            if (effect.DirectionalLight0.Enabled)
            {
                effect.DirectionalLight0.DiffuseColor = callContext.DirLight0DiffuseColor;
                effect.DirectionalLight0.SpecularColor = callContext.DirLight0SpecularColor;
                effect.DirectionalLight0.Direction = callContext.DirLight0Direction;
            }
            effect.DirectionalLight1.Enabled = callContext.DirLight1Enabled;
            if (effect.DirectionalLight1.Enabled)
            {
                effect.DirectionalLight1.DiffuseColor = callContext.DirLight1DiffuseColor;
                effect.DirectionalLight1.SpecularColor = callContext.DirLight1SpecularColor;
                effect.DirectionalLight1.Direction = callContext.DirLight1Direction;
            }
            effect.DirectionalLight2.Enabled = callContext.DirLight2Enabled;
            if (!effect.DirectionalLight2.Enabled)
                return;
            effect.DirectionalLight2.DiffuseColor = callContext.DirLight2DiffuseColor;
            effect.DirectionalLight2.SpecularColor = callContext.DirLight2SpecularColor;
            effect.DirectionalLight2.Direction = callContext.DirLight2Direction;
        }

        private static void skinVertex(
          Matrix[] bones,
          ref Vector3 position,
          ref Vector3 normal,
          ref Vector4 blendIndices,
          ref Vector4 blendWeights,
          out Vector3 outPosition,
          out Vector3 outNormal)
        {
            int x = (int)blendIndices.X;
            int y = (int)blendIndices.Y;
            int z = (int)blendIndices.Z;
            int w = (int)blendIndices.W;
            Matrix blended;
            blend4x3Matrix(ref bones[x], ref bones[y], ref bones[z], ref bones[w], ref blendWeights, out blended);
            Vector3.Transform(ref position, ref blended, out outPosition);
            Vector3.TransformNormal(ref normal, ref blended, out outNormal);
        }

        private static void blend4x3Matrix(
          ref Matrix m1,
          ref Matrix m2,
          ref Matrix m3,
          ref Matrix m4,
          ref Vector4 weights,
          out Matrix blended)
        {
            float x = weights.X;
            float y = weights.Y;
            float z = weights.Z;
            float w = weights.W;
            float num1 = (float)(m1.M11 * (double)x + m2.M11 * (double)y + m3.M11 * (double)z + m4.M11 * (double)w);
            float num2 = (float)(m1.M12 * (double)x + m2.M12 * (double)y + m3.M12 * (double)z + m4.M12 * (double)w);
            float num3 = (float)(m1.M13 * (double)x + m2.M13 * (double)y + m3.M13 * (double)z + m4.M13 * (double)w);
            float num4 = (float)(m1.M21 * (double)x + m2.M21 * (double)y + m3.M21 * (double)z + m4.M21 * (double)w);
            float num5 = (float)(m1.M22 * (double)x + m2.M22 * (double)y + m3.M22 * (double)z + m4.M22 * (double)w);
            float num6 = (float)(m1.M23 * (double)x + m2.M23 * (double)y + m3.M23 * (double)z + m4.M23 * (double)w);
            float num7 = (float)(m1.M31 * (double)x + m2.M31 * (double)y + m3.M31 * (double)z + m4.M31 * (double)w);
            float num8 = (float)(m1.M32 * (double)x + m2.M32 * (double)y + m3.M32 * (double)z + m4.M32 * (double)w);
            float num9 = (float)(m1.M33 * (double)x + m2.M33 * (double)y + m3.M33 * (double)z + m4.M33 * (double)w);
            float num10 = (float)(m1.M41 * (double)x + m2.M41 * (double)y + m3.M41 * (double)z + m4.M41 * (double)w);
            float num11 = (float)(m1.M42 * (double)x + m2.M42 * (double)y + m3.M42 * (double)z + m4.M42 * (double)w);
            float num12 = (float)(m1.M43 * (double)x + m2.M43 * (double)y + m3.M43 * (double)z + m4.M43 * (double)w);
            blended = new Matrix();
            blended.M11 = num1;
            blended.M12 = num2;
            blended.M13 = num3;
            blended.M14 = 0.0f;
            blended.M21 = num4;
            blended.M22 = num5;
            blended.M23 = num6;
            blended.M24 = 0.0f;
            blended.M31 = num7;
            blended.M32 = num8;
            blended.M33 = num9;
            blended.M34 = 0.0f;
            blended.M41 = num10;
            blended.M42 = num11;
            blended.M43 = num12;
            blended.M44 = 1f;
        }

        private static void drawVertexBuffer(
          PrimitiveType primitiveType,
          int primitiveCount,
          BufferItem vbi,
          BufferItem ibi,
          bool useVB)
        {
            VertexBufferDesc buffer1 = (VertexBufferDesc)vbi.buffer;
            if (m_colorLogicEnabled)
                throw new NotImplementedException();
            if (m_colorMaterialEnabled)
                throw new NotImplementedException();
            BlendState blendState = m_blendEnabled ? m_blendState : BlendState.Opaque;
            if (m_graphicsDevice.BlendState != blendState)
                m_graphicsDevice.BlendState = blendState;
            DepthStencilState depthStencilState = m_depthTestEnabled ? m_depthStencilState : DepthStencilState.None;
            if (m_graphicsDevice.DepthStencilState != depthStencilState)
                m_graphicsDevice.DepthStencilState = depthStencilState;
            RasterizerState rasterizerState = m_cullFaceEnabled ? m_rasterizerState : m_rasterizerStateCN;
            if (m_graphicsDevice.RasterizerState != rasterizerState)
                m_graphicsDevice.RasterizerState = rasterizerState;
            if (m_graphicsDevice.SamplerStates[0] != m_textureUnits[0].SamplerState)
                m_graphicsDevice.SamplerStates[0] = m_textureUnits[0].SamplerState;
            if (!m_viewportSet)
            {
                m_graphicsDevice.Viewport = m_viewport;
                m_viewportSet = true;
            }
            Effect effect = m_basicEffect;
            if (m_matrixPalleteOESEnabled)
            {
                if (useVB)
                {
                    setupEffect(m_skinnedEffect);
                    m_skinnedEffect.SpecularPower = m_specularPower / 128f;
                    m_skinnedEffect.PreferPerPixelLighting = false;
                    effect = m_skinnedEffect;
                }
                else
                {
                    Vertex[] rawData = (Vertex[])vbi.rawData;
                    if (buffer1.vertices == null)
                        buffer1.vertices = new Vertex[rawData.Length];
                    for (int index = 0; index < rawData.Length; ++index)
                    {
                        Vector4 vector4 = rawData[index].BlendIndices.ToVector4();
                        skinVertex(m_matrixPalleteOES, ref rawData[index].Position, ref rawData[index].Normal, ref vector4, ref rawData[index].BlendWeight, out buffer1.vertices[index].Position, out buffer1.vertices[index].Normal);
                        buffer1.vertices[index].Color = rawData[index].Color;
                    }
                }
            }
            if (!useVB || useVB && !m_matrixPalleteOESEnabled)
            {
                if (m_alphaTestEnabled)
                {
                    setupEffect(m_alphaTestEffect);
                    effect = m_alphaTestEffect;
                }
                else
                    setupEffect(m_basicEffect);
            }
            if (useVB)
            {
                VertexBuffer buffer2 = buffer1.Buffer;
                effect.GraphicsDevice.SetVertexBuffer(buffer2);
                if (ibi != null)
                {
                    IndexBuffer buffer3 = (IndexBuffer)ibi.buffer;
                    effect.GraphicsDevice.Indices = buffer3;
                }
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    if (ibi != null)
                    {
                        m_graphicsDevice.DrawIndexedPrimitives(primitiveType, 0, 0, buffer2.VertexCount, 0, primitiveCount);
                        m_graphicsDevice.Indices = null;
                    }
                    else
                        m_graphicsDevice.DrawPrimitives(primitiveType, 0, primitiveCount);
                }
                m_graphicsDevice.SetVertexBuffer(null);
            }
            else
            {
                Vertex[] vertices = buffer1.vertices;
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    if (ibi != null)
                    {
                        short[] rawData = (short[])ibi.rawData;
                        m_graphicsDevice.DrawUserIndexedPrimitives(primitiveType, vertices, 0, vertices.Length, rawData, 0, primitiveCount);
                    }
                    else
                        m_graphicsDevice.DrawUserPrimitives(primitiveType, vertices, 0, primitiveCount);
                }
            }
        }

        private static void _glDrawArrays_PrepareVertices(
          PrimitiveType primitiveType,
          VertexPosTexColNorm[] dstArray,
          int dstOffset,
          int count)
        {
            Matrix matrix1 = m_textureUnits[0].MatrixStack.Peek();
            m_textureUnits[1].MatrixStack.Peek();
            bool flag1 = matrix1.Equals(Matrix.Identity);
            Matrix matrix2 = m_invScreen * Matrix.Invert(m_projectionMatrixStack.Peek());
            GLVertexData vertexArray = m_vertexArray;
            GLVertexData colorArray = m_colorArray;
            GLVertexData normalArray = m_normalArray;
            texCoordData[0] = m_textureUnits[0].TexCoordArray;
            texCoordData[1] = m_textureUnits[1].TexCoordArray;
            Vector4 vector4 = Vector4.Transform(posShift, matrix2 * Matrix.Invert(m_modelViewMatrixStack.Peek()));
            VertexPosTexColNorm[] dst;
            int dstOffset1;
            if (primitiveType == PrimitiveType.TriangleStrip)
            {
                dst = vtxBuffer;
                dstOffset1 = 0;
            }
            else
            {
                dst = dstArray;
                dstOffset1 = dstOffset;
            }
            vertexArray.ExtractTo(dst, dstOffset1, count);
            if (m_normalArrayEnabled)
                normalArray.ExtractTo(dst, dstOffset1, count);
            if (m_colorArrayEnabled)
                colorArray.ExtractTo(dst, dstOffset1, count);
            if (m_textureUnits[0].Enabled && m_textureUnits[0].TexCoordArrayEnabled)
                m_textureUnits[0].TexCoordArray.ExtractTo(dst, dstOffset1, count);
            if (m_textureUnits[1].Enabled && m_textureUnits[1].TexCoordArrayEnabled)
                m_textureUnits[1].TexCoordArray.ExtractTo(dst, dstOffset1, count);
            Matrix matrix3 = m_modelViewMatrixStack.Peek();
            for (int index1 = 0; index1 < count; ++index1)
            {
                int index2 = dstOffset1 + index1;
                dst[index2].Position.X += vector4.X;
                Vector3.Transform(ref dst[index2].Position, ref matrix3, out dst[index2].Position);
                Vector3.TransformNormal(ref dst[index2].Normal, ref matrix3, out dst[index2].Normal);
                Vector2 result = Vector2.Zero;
                if (m_textureUnits[0].Enabled && m_textureUnits[0].TexCoordArrayEnabled && !flag1)
                    Vector2.Transform(ref dst[index2].TextureCoordinate, ref matrix1, out result);
                dst[index2].TextureCoordinate = result;
            }
            if (primitiveType != PrimitiveType.TriangleStrip)
                return;
            dstArray[dstOffset] = dst[0];
            dstArray[dstOffset + 1] = dst[1];
            dstArray[dstOffset + 2] = dst[2];
            int index3 = 3;
            int index4 = dstOffset + 3;
            bool flag2 = false;
            for (; index3 < count; ++index3)
            {
                int index1 = index3 - 1;
                int index2 = index3 - 2;
                if (flag2)
                {
                    int num = index1;
                    index1 = index2;
                    index2 = num;
                }
                dstArray[index4] = dst[index1];
                int index5 = index4 + 1;
                dstArray[index5] = dst[index2];
                int index6 = index5 + 1;
                dstArray[index6] = dst[index3];
                index4 = index6 + 1;
            }
        }

        private static void initVertices(int count, out bool twoPasses)
        {
            twoPasses = false;
            switch (0 | (m_textureUnits[0].Enabled ? 1 : 0) | (m_textureUnits[1].Enabled ? 2 : 0))
            {
                case 0:
                    twoPasses = false;
                    break;
                case 1:
                    twoPasses = false;
                    break;
                case 2:
                    throw new InvalidOperationException();
                case 3:
                    twoPasses = true;
                    break;
            }
            Matrix matrix1 = m_textureUnits[0].MatrixStack.Peek();
            m_textureUnits[0].MatrixStack.Peek();
            bool flag = matrix1.Equals(Matrix.Identity);
            Matrix matrix2 = m_invScreen * Matrix.Invert(m_projectionMatrixStack.Peek());
            GLVertexData vertexArray = m_vertexArray;
            GLVertexData colorArray = m_colorArray;
            GLVertexData normalArray = m_normalArray;
            texCoordData[0] = m_textureUnits[0].TexCoordArray;
            texCoordData[1] = m_textureUnits[1].TexCoordArray;
            if (m_vertices[0] == null || count > m_vertices[0].Length)
                m_vertices[0] = new VertexPosTexColNorm[count * 2];
            Vector4 vector4 = Vector4.Transform(posShift, matrix2 * Matrix.Invert(m_modelViewMatrixStack.Peek()));
            vertexArray.ExtractTo(m_vertices[0], 0, count);
            if (m_normalArrayEnabled)
                normalArray.ExtractTo(m_vertices[0], 0, count);
            if (m_colorArrayEnabled)
                colorArray.ExtractTo(m_vertices[0], 0, count);
            if (m_textureUnits[0].Enabled && m_textureUnits[0].TexCoordArrayEnabled)
                texCoordData[0].ExtractTo(m_vertices[0], 0, count);
            for (int index = 0; index < count; ++index)
            {
                m_vertices[0][index].Position.X += vector4.X;
                Vector2 result = Vector2.Zero;
                if (m_textureUnits[0].Enabled && m_textureUnits[0].TexCoordArrayEnabled && !flag)
                    Vector2.Transform(ref m_vertices[0][index].TextureCoordinate, ref matrix1, out result);
                m_vertices[0][index].TextureCoordinate = result;
            }
        }

        private static void drawPrimitives(
          PrimitiveType primitiveType,
          int primitiveCount,
          bool useIndices,
          bool twoPasses)
        {
            if (m_colorLogicEnabled)
                throw new NotImplementedException();
            if (m_colorMaterialEnabled)
                throw new NotImplementedException();
            BlendState blendState = m_blendEnabled ? m_blendState : BlendState.Opaque;
            if (m_graphicsDevice.BlendState != blendState)
                m_graphicsDevice.BlendState = blendState;
            DepthStencilState depthStencilState = m_depthTestEnabled ? m_depthStencilState : DepthStencilState.None;
            if (m_graphicsDevice.DepthStencilState != depthStencilState)
                m_graphicsDevice.DepthStencilState = depthStencilState;
            RasterizerState rasterizerState = m_cullFaceEnabled ? m_rasterizerState : m_rasterizerStateCN;
            if (m_graphicsDevice.RasterizerState != rasterizerState)
                m_graphicsDevice.RasterizerState = rasterizerState;
            if (m_graphicsDevice.SamplerStates[0] != m_textureUnits[0].SamplerState)
                m_graphicsDevice.SamplerStates[0] = m_textureUnits[0].SamplerState;
            if (!m_viewportSet)
            {
                m_graphicsDevice.Viewport = m_viewport;
                m_viewportSet = true;
            }
            Effect effect = m_basicEffect;
            if (m_alphaTestEnabled)
            {
                setupEffect(m_alphaTestEffect);
                effect = m_alphaTestEffect;
            }
            else
                setupEffect(m_basicEffect);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                if (useIndices)
                    m_graphicsDevice.DrawUserIndexedPrimitives(primitiveType, m_vertices[0], 0, verticesToDraw_, m_indices, 0, primitiveCount);
                else
                    m_graphicsDevice.DrawUserPrimitives(primitiveType, m_vertices[0], 0, primitiveCount);
            }
        }

        private static void drawPrimitives(
          VertexPosTexColNorm[] vertices,
          PrimitiveType primitiveType,
          int primitiveCount,
          ref DrawArraysCallContext callContext)
        {
            ++drawPrimitives_Count;
            if (callContext.blendState != m_graphicsDevice.BlendState)
                m_graphicsDevice.BlendState = callContext.blendState;
            if (callContext.depthStencilState != m_graphicsDevice.DepthStencilState)
                m_graphicsDevice.DepthStencilState = callContext.depthStencilState;
            if (callContext.rasterizerState != m_graphicsDevice.RasterizerState)
                m_graphicsDevice.RasterizerState = callContext.rasterizerState;
            if (callContext.samplerState != m_graphicsDevice.SamplerStates[0])
                m_graphicsDevice.SamplerStates[0] = callContext.samplerState;
            if (!m_viewportSet)
            {
                m_graphicsDevice.Viewport = m_viewport;
                m_viewportSet = true;
            }
            Effect effect = m_basicEffect;
            if (callContext.alphaTestEnabled)
            {
                setupEffect(m_alphaTestEffect, ref callContext);
                effect = m_alphaTestEffect;
            }
            else
                setupEffect(m_basicEffect, ref callContext);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                m_graphicsDevice.DrawUserPrimitives(primitiveType, vertices, 0, primitiveCount);
            }
        }

        public static void glEnd()
        {
            throw new NotImplementedException();
        }

        public static void glFlush()
        {
            flushDrawArrays();
        }

        public static void glFogf(uint pname, float param)
        {
            switch (pname)
            {
                case 2914:
                    break;
                case 2915:
                    m_fogStart = param;
                    break;
                case 2916:
                    m_fogEnd = param;
                    break;
                case 2917:
                    m_fogMode = (ushort)param;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void glFogfv(uint pname, float[] param)
        {
            if (pname != 2918U)
                throw new NotImplementedException();
            m_fogColor.X = param[0];
            m_fogColor.Y = param[1];
            m_fogColor.Z = param[2];
        }

        public static void glFrontFace(uint mode)
        {
            switch (mode)
            {
                case 2304:
                    m_rasterizerState = 1029U == m_cullFace ? m_rasterizerStateCCW : m_rasterizerStateCW;
                    break;
                case 2305:
                    m_rasterizerState = 1029U == m_cullFace ? m_rasterizerStateCW : m_rasterizerStateCCW;
                    break;
                default:
                    throw new ArgumentException();
            }
            m_frontFace = mode;
        }

        public static void glGenBuffer(out uint buffer)
        {
            uint num1 = 0;
            uint num2 = 0;
            while (uint.MaxValue == m_usedBufferIDs[(int)num2])
            {
                ++num2;
                if (num2 == m_usedBufferIDs.Length)
                    Array.Resize(ref m_usedBufferIDs, (int)num2 + 8);
            }
            uint num3 = 1;
            uint num4 = 0;
            while (num3 != 0U)
            {
                if (((int)m_usedBufferIDs[(int)num2] & (int)num3) == 0)
                {
                    m_usedBufferIDs[(int)num2] |= num3;
                    uint key = (uint)((int)num2 * 32 + (int)num4 + 1);
                    num1 = key;
                    m_buffers.Add(key, new BufferItem());
                    break;
                }
                num3 <<= 1;
                ++num4;
            }
            buffer = num1;
        }

        public static void glGenBuffers(int n, uint[] buffers)
        {
            for (int index = 0; index < n; ++index)
                glGenBuffer(out buffers[index]);
        }

        public static void glGenTexture(out uint texture)
        {
            uint num1 = 0;
            uint num2 = 0;
            while (uint.MaxValue == m_usedTexIDs[(int)num2])
            {
                ++num2;
                if (num2 == m_usedTexIDs.Length)
                    Array.Resize(ref m_usedTexIDs, (int)num2 + 8);
            }
            uint num3 = 1;
            uint num4 = 0;
            while (num3 != 0U)
            {
                if (((int)m_usedTexIDs[(int)num2] & (int)num3) == 0)
                {
                    m_usedTexIDs[(int)num2] |= num3;
                    uint key = (uint)((int)num2 * 32 + (int)num4 + 1);
                    num1 = key;
                    m_textures.Add(key, null);
                    break;
                }
                num3 <<= 1;
                ++num4;
            }
            texture = num1;
        }

        public static void glGenTextures(int n, uint[] textures)
        {
            for (int index = 0; index < n; ++index)
                glGenTexture(out textures[index]);
        }

        public static void glGetFloatv(uint pname, byte[] param)
        {
            throw new NotImplementedException();
        }

        public static void glGetInteger(uint pname, out int param)
        {
            if (pname != 34882U)
                throw new NotImplementedException();
            param = 32;
        }

        public static void glGetRenderbufferParameteriOES(uint target, uint pname, out int param)
        {
            if (target != 36161U)
                throw new NotImplementedException();
            switch (pname)
            {
                case 36162:
                    param = m_graphicsDevice.PresentationParameters.BackBufferWidth;
                    break;
                case 36163:
                    param = m_graphicsDevice.PresentationParameters.BackBufferHeight;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static string glGetString(uint name)
        {
            throw new NotImplementedException();
        }

        public static void glLightf(uint light, uint pname, float param)
        {
        }

        public static void glLightfv(uint light, uint pname, ref glArray4f param)
        {
            light -= 16384U;
            switch (pname)
            {
                case 4608:
                    break;
                case 4609:
                    m_lightUnit[(int)light].DiffuseColor.X = param.f0;
                    m_lightUnit[(int)light].DiffuseColor.Y = param.f1;
                    m_lightUnit[(int)light].DiffuseColor.Z = param.f2;
                    break;
                case 4610:
                    m_lightUnit[(int)light].SpecularColor.X = param.f0;
                    m_lightUnit[(int)light].SpecularColor.Y = param.f1;
                    m_lightUnit[(int)light].SpecularColor.Z = param.f2;
                    break;
                case 4611:
                    break;
                case 4612:
                    m_lightUnit[(int)light].Direction.X = param.f0;
                    m_lightUnit[(int)light].Direction.Y = param.f1;
                    m_lightUnit[(int)light].Direction.Z = param.f2;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void glLightModelf(uint pname, float param)
        {
            if (pname != 2898U)
                throw new NotImplementedException();
        }

        public static void glLightModelfv(uint pname, glArray4f param)
        {
            if (pname != 2899U)
                throw new NotImplementedException();
            m_ambientColor.X = param.f0;
            m_ambientColor.Y = param.f0;
            m_ambientColor.Z = param.f0;
        }

        public static void glLoadIdentity()
        {
            if (m_matrixPaletteOESActive)
            {
                m_matrixPalleteOES[(int)m_currentPaletteMatrixOES] = Matrix.Identity;
            }
            else
            {
                m_activeMatrixStack.Pop();
                m_activeMatrixStack.Push(Matrix.Identity);
            }
        }

        public static void glLoadMatrixf(ref Matrix matrix)
        {
            if (m_matrixPaletteOESActive)
            {
                m_matrixPalleteOES[(int)m_currentPaletteMatrixOES] = matrix;
            }
            else
            {
                m_activeMatrixStack.Pop();
                m_activeMatrixStack.Push(matrix);
            }
        }

        public static void glLoadPaletteFromModelViewMatrixOES()
        {
            throw new NotImplementedException();
        }

        public static void glLogicOp(uint opcode)
        {
            throw new NotImplementedException();
        }

        public static object glMapBuffer(uint target, uint access)
        {
            throw new NotImplementedException();
        }

        public static void glMaterialf(uint face, uint pname, float param)
        {
            if (1032U != face)
                throw new NotImplementedException();
            if (5633U != pname)
                throw new ArgumentException();
            m_specularPower = param;
        }

        public static void glMaterialfv(uint face, uint pname, float[] param)
        {
            glMaterialfv(face, pname, new glArray4f(param[0], param[1], param[2], param[3]));
        }

        public static void glMaterialfv(uint face, uint pname, glArray4f param)
        {
            if (1032U != face)
                throw new NotImplementedException();
            switch (pname)
            {
                case 4608:
                    m_ambientColor.X = param.f0;
                    m_ambientColor.Y = param.f1;
                    m_ambientColor.Z = param.f2;
                    break;
                case 4609:
                    m_diffuseColor.X = param.f0;
                    m_diffuseColor.Y = param.f1;
                    m_diffuseColor.Z = param.f2;
                    m_diffuseColorAlpha = param.f3;
                    break;
                case 4610:
                    m_specularColor.X = param.f0;
                    m_specularColor.Y = param.f1;
                    m_specularColor.Z = param.f2;
                    break;
                case 5632:
                    m_emissiveColor.X = param.f0;
                    m_emissiveColor.Y = param.f1;
                    m_emissiveColor.Z = param.f2;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void glMatrixIndexPointerOES(
          int size,
          uint type,
          int stride,
          GLVertexData data)
        {
            m_matrixIndexArraySize = size;
            m_matrixIndexArrayDataType = type;
            m_matrixIndexArrayStride = stride == 0 ? SizeOf(type) * size : stride;
            m_matrixIndexArray = data;
        }

        public static void glMatrixMode(uint mode)
        {
            m_matrixPaletteOESActive = false;
            switch (mode)
            {
                case 5888:
                    m_activeMatrixStack = m_modelViewMatrixStack;
                    break;
                case 5889:
                    m_activeMatrixStack = m_projectionMatrixStack;
                    break;
                case 5890:
                    m_activeMatrixStack = m_textureUnits[(int)m_activeTextureUnit].MatrixStack;
                    break;
                case 6144:
                    throw new NotImplementedException();
                case 34880:
                    m_matrixPaletteOESActive = m_matrixPalleteOESEnabled;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public static void glMultiTexCoord2fv(uint target, float[] v)
        {
            throw new NotImplementedException();
        }

        public static void glNormal3fv(float[] v)
        {
            throw new NotImplementedException();
        }

        public static void glNormalPointer(uint type, int stride, GLVertexData pointer)
        {
            m_normalArrayDataType = type;
            m_normalArrayStride = stride == 0 ? SizeOf(type) * 3 : stride;
            m_normalArray = pointer;
        }

        public static void glOrtho(
          float left,
          float right,
          float bottom,
          float top,
          float nearVal,
          float farVal)
        {
            Matrix orthographicOffCenter = Matrix.CreateOrthographicOffCenter(left, right, bottom, top, nearVal, farVal);
            if (m_matrixPaletteOESActive)
                m_matrixPalleteOES[(int)m_currentPaletteMatrixOES] = orthographicOffCenter * m_matrixPalleteOES[(int)m_currentPaletteMatrixOES];
            else
                m_activeMatrixStack.Push(orthographicOffCenter * m_activeMatrixStack.Pop());
        }

        public static void glPixelStorei(uint pname, int param)
        {
            throw new NotImplementedException();
        }

        public static void glPolygonMode(uint face, uint mode)
        {
            throw new NotImplementedException();
        }

        public static void glPopMatrix()
        {
            if (m_matrixPaletteOESActive)
                throw new ArgumentException();
            if (1 == m_activeMatrixStack.Count)
                throw new InvalidOperationException();
            m_activeMatrixStack.Pop();
        }

        public static void glPushMatrix()
        {
            if (m_matrixPaletteOESActive)
                throw new ArgumentException();
            m_activeMatrixStack.Push(m_activeMatrixStack.Peek());
        }

        public static void glScalef(float x, float y, float z)
        {
            Matrix scale = Matrix.CreateScale(x, y, z);
            if (m_matrixPaletteOESActive)
                m_matrixPalleteOES[(int)m_currentPaletteMatrixOES] = scale * m_matrixPalleteOES[(int)m_currentPaletteMatrixOES];
            else
                m_activeMatrixStack.Push(scale * m_activeMatrixStack.Pop());
        }

        public static void glShadeModel(uint mode)
        {
            switch (mode)
            {
                case 7424:
                    m_perPixelLighting = false;
                    break;
                case 7425:
                    m_perPixelLighting = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void glTexCoordPointer(
          int size,
          uint type,
          int stride,
          GLVertexData pointer)
        {
            TextureUnit textureUnit = m_textureUnits[(int)m_clientActiveTextureUnit];
            textureUnit.TexCoordArraySize = size;
            textureUnit.TexCoordArrayDataType = type;
            textureUnit.TexCoordArrayStride = stride;
            textureUnit.TexCoordArray = pointer;
        }

        public static void glTexEnvf(uint target, uint pname, float param)
        {
            throw new NotImplementedException();
        }

        public static void glTexEnvfv(uint target, uint pname, glArray4f v)
        {
            if (target != 8960U)
                throw new NotImplementedException();
            if (pname != 8705U)
                throw new NotImplementedException();
            m_color.R = (byte)(byte.MaxValue * (double)v.f0);
            m_color.G = (byte)(byte.MaxValue * (double)v.f1);
            m_color.B = (byte)(byte.MaxValue * (double)v.f2);
            m_color.A = (byte)(byte.MaxValue * (double)v.f3);
        }

        public static void glTexEnvi(uint target, uint pname, int param)
        {
            if (target != 8960U)
                throw new NotImplementedException();
            if (pname != 8704U)
                throw new NotImplementedException();
            switch (param)
            {
                case 7681:
                    m_vertexColorEnabled = false;
                    break;
                case 8448:
                    m_vertexColorEnabled = true;
                    break;
                case 8449:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void mppglTexImage2D(Texture2D texture)
        {
            m_textures[m_textureUnits[(int)m_activeTextureUnit].BoundTexture] = texture;
            m_textures[m_textureUnits[(int)m_activeTextureUnit].BoundTexture].Tag = true;
        }

        public static void mppglTexImage2D(string textureName)
        {
            using (Stream stream = TitleContainer.OpenStream(Path.Combine("Content", textureName + ".png")))
            {
                m_textures[m_textureUnits[(int)m_activeTextureUnit].BoundTexture] = Texture2D.FromStream(m_graphicsDevice, stream);
                m_textures[m_textureUnits[(int)m_activeTextureUnit].BoundTexture].Tag = true;
            }
        }

        public static void glTexParameteri(uint target, uint pname, int param)
        {
            if (target != 3553U)
                throw new NotImplementedException();
            TextureAddressMode AddrU = m_textureUnits[(int)m_activeTextureUnit].SamplerState.AddressU;
            TextureAddressMode AddrV = m_textureUnits[(int)m_activeTextureUnit].SamplerState.AddressV;
            switch (pname)
            {
                case 10242:
                    switch (param)
                    {
                        case 10496:
                        case 33071:
                            AddrU = TextureAddressMode.Clamp;
                            break;
                        case 10497:
                            AddrU = TextureAddressMode.Wrap;
                            break;
                        case 33648:
                            AddrU = TextureAddressMode.Mirror;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case 10243:
                    switch (param)
                    {
                        case 10496:
                        case 33071:
                            AddrV = TextureAddressMode.Clamp;
                            break;
                        case 10497:
                            AddrV = TextureAddressMode.Wrap;
                            break;
                        case 33648:
                            AddrV = TextureAddressMode.Mirror;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                default:
                    return;
            }
            int key = CalcSamplerStateId(AddrU, AddrV);
            SamplerState samplerState;
            if (!m_samplerStates.TryGetValue(key, out samplerState))
            {
                samplerState = m_textureUnits[(int)m_activeTextureUnit].SamplerState.clone();
                samplerState.AddressU = AddrU;
                samplerState.AddressV = AddrV;
                m_samplerStates[key] = samplerState;
            }
            m_textureUnits[(int)m_activeTextureUnit].SamplerState = samplerState;
        }

        public static void glTranslatef(float x, float y, float z)
        {
            Matrix translation = Matrix.CreateTranslation(x, y, z);
            if (m_matrixPaletteOESActive)
                m_matrixPalleteOES[(int)m_currentPaletteMatrixOES] = translation * m_matrixPalleteOES[(int)m_currentPaletteMatrixOES];
            else
                m_activeMatrixStack.Push(translation * m_activeMatrixStack.Pop());
        }

        public static byte glUnmapBuffer(uint target)
        {
            throw new NotImplementedException();
        }

        public static void glVertex3f(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public static void glVertex3fv(float[] v)
        {
            throw new NotImplementedException();
        }

        public static void glVertexAttrib3fvARB(uint index, float[] v)
        {
            throw new NotImplementedException();
        }

        public static void glVertexPointer(
          int size,
          uint type,
          int stride,
          GLVertexData pointer)
        {
            m_vertexArraySize = size;
            m_vertexArrayDataType = type;
            m_vertexArrayStride = stride == 0 ? SizeOf(type) * size : stride;
            m_vertexArray = pointer;
        }

        public static void glViewport(int x, int y, int width, int height)
        {
            m_viewport = new Viewport(x, y, width, height);
            calculateInvScreenMatrix();
        }

        public static void glWeightPointerOES(
          int size,
          uint type,
          int stride,
          GLVertexData pointer)
        {
            m_weightArraySize = size;
            m_weightArrayDataType = type;
            m_weightArrayStride = stride == 0 ? SizeOf(type) * size : stride;
            m_weightArray = pointer;
        }

        static OpenGL()
        {
            m_modelViewMatrixStack.Push(Matrix.Identity);
            m_projectionMatrixStack = new Stack<Matrix>();
            m_projectionMatrixStack.Push(Matrix.Identity);
            m_matrixPalleteOES = new Matrix[32];
            for (int index = 0; index < 32; ++index)
                m_matrixPalleteOES[index] = Matrix.Identity;
            m_activeMatrixStack = m_modelViewMatrixStack;
        }

        public static void init(Game game, GraphicsDevice graphicsDevice)
        {
            m_graphicsDevice = graphicsDevice;
            m_basicEffect = new BasicEffect(m_graphicsDevice);
            m_alphaTestEffect = new AlphaTestEffect(m_graphicsDevice);
            m_skinnedEffect = new SkinnedEffect(m_graphicsDevice);
            m_dualTextureEffect = new DualTextureEffect(m_graphicsDevice);
            m_fakeTexture = new Texture2D(m_graphicsDevice, 1, 1);
            m_fakeTexture.SetData(new Color[1]
            {
        new Color(1f, 1f, 1f, 1f)
            });
            calculateInvScreenMatrix();
            int index1 = CalcDepthStencilStateId(m_depthStencilState.DepthBufferFunction, m_depthStencilState.DepthBufferWriteEnable);
            m_depthStencils[index1] = m_depthStencilState;
            int index2 = CalcSamplerStateId(m_textureUnits[0].SamplerState.AddressU, m_textureUnits[0].SamplerState.AddressV);
            m_samplerStates[index2] = m_textureUnits[0].SamplerState;
            long index3 = CalcBlendStateId(m_blendState.ColorSourceBlend, m_blendState.AlphaSourceBlend, m_blendState.ColorDestinationBlend, m_blendState.AlphaDestinationBlend, m_blendState.ColorWriteChannels, m_blendState.ColorBlendFunction, m_blendState.AlphaBlendFunction);
            m_blendStates[index3] = m_blendState;
        }

        private static int CalcDepthStencilStateId(
          CompareFunction depthBufferFunction,
          bool depthBufferWriteEnable)
        {
            return (int)(depthBufferFunction + 1) * 10 + (depthBufferWriteEnable ? 1 : 0);
        }

        private static int CalcSamplerStateId(TextureAddressMode AddrU, TextureAddressMode AddrV)
        {
            return (int)((int)(AddrU + 1) * 10 + AddrV);
        }

        private static long CalcBlendStateId(
          Blend colorSrcBlend,
          Blend alphaSrcBlend,
          Blend colorDstBlend,
          Blend alphaDstBlend,
          ColorWriteChannels channels,
          BlendFunction ColorFunc,
          BlendFunction AlphaFunc)
        {
            return (long)((int)(colorSrcBlend + 1) * 32 * 32 * 32 * 32 * 32 + (int)(alphaSrcBlend + 1) * 32 * 32 * 32 * 32 + (int)(colorDstBlend + 1) * 32 * 32 * 32 + (int)(alphaDstBlend + 1) * 32 * 32 + (int)(channels + 1) * 32 + (int)(ColorFunc + 1) * 5 + AlphaFunc);
        }

        private static void calculateInvScreenMatrix()
        {
            m_invScreen = Matrix.Identity;
            int num1 = m_viewport.Width >> 1;
            int num2 = m_viewport.Height >> 1;
            m_invScreen.M11 = num1;
            m_invScreen.M22 = -num2;
            m_invScreen.M41 = m_viewport.X + num1;
            m_invScreen.M42 = m_viewport.Y + num2;
            m_invScreen = Matrix.Invert(m_invScreen);
        }

        public static int SizeOf(uint type)
        {
            switch (type)
            {
                case 5120:
                case 5121:
                    return 1;
                case 5122:
                case 5123:
                    return 2;
                case 5124:
                case 5125:
                case 5126:
                    return 4;
                default:
                    throw new NotImplementedException();
            }
        }

        private static float Clamp(float value, float lowerBound, float upperBound)
        {
            if (value < (double)lowerBound)
                return lowerBound;
            return value > (double)upperBound ? upperBound : value;
        }

        #region Constants

        private const int MinBufferSize = 32;
        private const int MAX_PALETTE_MATRICES_OES = 32;
        private const int TEXTURES_NAME_CAPACITY = 8;
        private const int TEXTURES_NAME_CAPACITY_INCREMENT = 8;
        private const int BUFFERS_NAME_CAPACITY = 8;
        private const int BUFFERS_NAME_CAPACITY_INCREMENT = 8;
        public const ushort GL_BLEND_EQUATION_RGB_OES = 32777;
        public const ushort GL_BLEND_EQUATION_ALPHA_OES = 34877;
        public const ushort GL_BLEND_DST_RGB_OES = 32968;
        public const ushort GL_BLEND_SRC_RGB_OES = 32969;
        public const ushort GL_BLEND_DST_ALPHA_OES = 32970;
        public const ushort GL_BLEND_SRC_ALPHA_OES = 32971;
        public const ushort GL_BLEND_EQUATION_OES = 32777;
        public const ushort GL_FUNC_ADD_OES = 32774;
        public const ushort GL_FUNC_SUBTRACT_OES = 32778;
        public const ushort GL_FUNC_REVERSE_SUBTRACT_OES = 32779;
        public const ushort GL_ETC1_RGB8_OES = 36196;
        public const ushort GL_TEXTURE_CROP_RECT_OES = 35741;
        public const ushort GL_FIXED_OES = 5132;
        public const ushort GL_NONE_OES = 0;
        public const ushort GL_FRAMEBUFFER_OES = 36160;
        public const ushort GL_RENDERBUFFER_OES = 36161;
        public const ushort GL_RGBA4_OES = 32854;
        public const ushort GL_RGB5_A1_OES = 32855;
        public const ushort GL_RGB565_OES = 36194;
        public const ushort GL_DEPTH_COMPONENT16_OES = 33189;
        public const ushort GL_RENDERBUFFER_WIDTH_OES = 36162;
        public const ushort GL_RENDERBUFFER_HEIGHT_OES = 36163;
        public const ushort GL_RENDERBUFFER_INTERNAL_FORMAT_OES = 36164;
        public const ushort GL_RENDERBUFFER_RED_SIZE_OES = 36176;
        public const ushort GL_RENDERBUFFER_GREEN_SIZE_OES = 36177;
        public const ushort GL_RENDERBUFFER_BLUE_SIZE_OES = 36178;
        public const ushort GL_RENDERBUFFER_ALPHA_SIZE_OES = 36179;
        public const ushort GL_RENDERBUFFER_DEPTH_SIZE_OES = 36180;
        public const ushort GL_RENDERBUFFER_STENCIL_SIZE_OES = 36181;
        public const ushort GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE_OES = 36048;
        public const ushort GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME_OES = 36049;
        public const ushort GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL_OES = 36050;
        public const ushort GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE_OES = 36051;
        public const ushort GL_COLOR_ATTACHMENT0_OES = 36064;
        public const ushort GL_DEPTH_ATTACHMENT_OES = 36096;
        public const ushort GL_STENCIL_ATTACHMENT_OES = 36128;
        public const ushort GL_FRAMEBUFFER_COMPLETE_OES = 36053;
        public const ushort GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT_OES = 36054;
        public const ushort GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT_OES = 36055;
        public const ushort GL_FRAMEBUFFER_INCOMPLETE_DIMENSIONS_OES = 36057;
        public const ushort GL_FRAMEBUFFER_INCOMPLETE_FORMATS_OES = 36058;
        public const ushort GL_FRAMEBUFFER_UNSUPPORTED_OES = 36061;
        public const ushort GL_FRAMEBUFFER_BINDING_OES = 36006;
        public const ushort GL_RENDERBUFFER_BINDING_OES = 36007;
        public const ushort GL_MAX_RENDERBUFFER_SIZE_OES = 34024;
        public const ushort GL_INVALID_FRAMEBUFFER_OPERATION_OES = 1286;
        public const ushort GL_MODELVIEW_MATRIX_FLOAT_AS_INT_BITS_OES = 35213;
        public const ushort GL_PROJECTION_MATRIX_FLOAT_AS_INT_BITS_OES = 35214;
        public const ushort GL_TEXTURE_MATRIX_FLOAT_AS_INT_BITS_OES = 35215;
        public const ushort GL_MAX_VERTEX_UNITS_OES = 34468;
        public const ushort GL_MAX_PALETTE_MATRICES_OES = 34882;
        public const ushort GL_MATRIX_PALETTE_OES = 34880;
        public const ushort GL_MATRIX_INDEX_ARRAY_OES = 34884;
        public const ushort GL_WEIGHT_ARRAY_OES = 34477;
        public const ushort GL_CURRENT_PALETTE_MATRIX_OES = 34883;
        public const ushort GL_MATRIX_INDEX_ARRAY_SIZE_OES = 34886;
        public const ushort GL_MATRIX_INDEX_ARRAY_TYPE_OES = 34887;
        public const ushort GL_MATRIX_INDEX_ARRAY_STRIDE_OES = 34888;
        public const ushort GL_MATRIX_INDEX_ARRAY_POINTER_OES = 34889;
        public const ushort GL_MATRIX_INDEX_ARRAY_BUFFER_BINDING_OES = 35742;
        public const ushort GL_WEIGHT_ARRAY_SIZE_OES = 34475;
        public const ushort GL_WEIGHT_ARRAY_TYPE_OES = 34473;
        public const ushort GL_WEIGHT_ARRAY_STRIDE_OES = 34474;
        public const ushort GL_WEIGHT_ARRAY_POINTER_OES = 34476;
        public const ushort GL_WEIGHT_ARRAY_BUFFER_BINDING_OES = 34974;
        public const ushort GL_INCR_WRAP_OES = 34055;
        public const ushort GL_DECR_WRAP_OES = 34056;
        public const ushort GL_NORMAL_MAP_OES = 34065;
        public const ushort GL_REFLECTION_MAP_OES = 34066;
        public const ushort GL_TEXTURE_CUBE_MAP_OES = 34067;
        public const ushort GL_TEXTURE_BINDING_CUBE_MAP_OES = 34068;
        public const ushort GL_TEXTURE_CUBE_MAP_POSITIVE_X_OES = 34069;
        public const ushort GL_TEXTURE_CUBE_MAP_NEGATIVE_X_OES = 34070;
        public const ushort GL_TEXTURE_CUBE_MAP_POSITIVE_Y_OES = 34071;
        public const ushort GL_TEXTURE_CUBE_MAP_NEGATIVE_Y_OES = 34072;
        public const ushort GL_TEXTURE_CUBE_MAP_POSITIVE_Z_OES = 34073;
        public const ushort GL_TEXTURE_CUBE_MAP_NEGATIVE_Z_OES = 34074;
        public const ushort GL_MAX_CUBE_MAP_TEXTURE_SIZE_OES = 34076;
        public const ushort GL_TEXTURE_GEN_MODE_OES = 9472;
        public const ushort GL_TEXTURE_GEN_STR_OES = 36192;
        public const ushort GL_MIRRORED_REPEAT_OES = 33648;
        public const ushort GL_DEPTH_COMPONENT24_OES = 33190;
        public const ushort GL_DEPTH_COMPONENT32_OES = 33191;
        public const ushort GL_WRITE_ONLY_OES = 35001;
        public const ushort GL_BUFFER_ACCESS_OES = 35003;
        public const ushort GL_BUFFER_MAPPED_OES = 35004;
        public const ushort GL_BUFFER_MAP_POINTER_OES = 35005;
        public const ushort GL_RGB8_OES = 32849;
        public const ushort GL_RGBA8_OES = 32856;
        public const ushort GL_STENCIL_INDEX1_OES = 36166;
        public const ushort GL_STENCIL_INDEX4_OES = 36167;
        public const ushort GL_STENCIL_INDEX8_OES = 36168;
        public const ushort GL_3DC_X_AMD = 34809;
        public const ushort GL_3DC_XY_AMD = 34810;
        public const ushort GL_ATC_RGB_AMD = 35986;
        public const ushort GL_ATC_RGBA_EXPLICIT_ALPHA_AMD = 35987;
        public const ushort GL_ATC_RGBA_INTERPOLATED_ALPHA_AMD = 34798;
        public const ushort GL_TEXTURE_MAX_ANISOTROPY_EXT = 34046;
        public const ushort GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT = 34047;
        public const ushort GL_POINT_SIZE_MIN = 33062;
        public const ushort GL_POINT_SIZE_MAX = 33063;
        public const ushort GL_POINT_FADE_THRESHOLD_SIZE = 33064;
        public const ushort GL_POINT_DISTANCE_ATTENUATION = 33065;
        public const ushort GL_GENERATE_MIPMAP_HINT = 33170;
        public const ushort GL_FIXED = 5132;
        public const ushort GL_GENERATE_MIPMAP = 33169;
        public const ushort GL_ARRAY_BUFFER = 34962;
        public const ushort GL_ELEMENT_ARRAY_BUFFER = 34963;
        public const ushort GL_ARRAY_BUFFER_BINDING = 34964;
        public const ushort GL_ELEMENT_ARRAY_BUFFER_BINDING = 34965;
        public const ushort GL_VERTEX_ARRAY_BUFFER_BINDING = 34966;
        public const ushort GL_NORMAL_ARRAY_BUFFER_BINDING = 34967;
        public const ushort GL_COLOR_ARRAY_BUFFER_BINDING = 34968;
        public const ushort GL_TEXTURE_COORD_ARRAY_BUFFER_BINDING = 34970;
        public const ushort GL_STATIC_DRAW = 35044;
        public const ushort GL_DYNAMIC_DRAW = 35048;
        public const ushort GL_BUFFER_SIZE = 34660;
        public const ushort GL_BUFFER_USAGE = 34661;
        public const ushort GL_SRC0_RGB = 34176;
        public const ushort GL_SRC1_RGB = 34177;
        public const ushort GL_SRC2_RGB = 34178;
        public const ushort GL_SRC0_ALPHA = 34184;
        public const ushort GL_SRC1_ALPHA = 34185;
        public const ushort GL_SRC2_ALPHA = 34186;
        public const ushort GL_MIRRORED_REPEAT = 33648;
        public const ushort GL_SECONDARY_COLOR_ARRAY = 33886;
        public const byte GL_FALSE = 0;
        public const byte GL_TRUE = 1;
        public const ushort GL_BYTE = 5120;
        public const ushort GL_UNSIGNED_BYTE = 5121;
        public const ushort GL_SHORT = 5122;
        public const ushort GL_UNSIGNED_SHORT = 5123;
        public const ushort GL_INT = 5124;
        public const ushort GL_UNSIGNED_INT = 5125;
        public const ushort GL_FLOAT = 5126;
        public const ushort GL_2_BYTES = 5127;
        public const ushort GL_3_BYTES = 5128;
        public const ushort GL_4_BYTES = 5129;
        public const ushort GL_DOUBLE = 5130;
        public const ushort GL_POINTS = 0;
        public const ushort GL_LINES = 1;
        public const ushort GL_LINE_LOOP = 2;
        public const ushort GL_LINE_STRIP = 3;
        public const ushort GL_TRIANGLES = 4;
        public const ushort GL_TRIANGLE_STRIP = 5;
        public const ushort GL_TRIANGLE_FAN = 6;
        public const ushort GL_QUADS = 7;
        public const ushort GL_QUAD_STRIP = 8;
        public const ushort GL_POLYGON = 9;
        public const ushort GL_VERTEX_ARRAY = 32884;
        public const ushort GL_NORMAL_ARRAY = 32885;
        public const ushort GL_COLOR_ARRAY = 32886;
        public const ushort GL_INDEX_ARRAY = 32887;
        public const ushort GL_TEXTURE_COORD_ARRAY = 32888;
        public const ushort GL_EDGE_FLAG_ARRAY = 32889;
        public const ushort GL_VERTEX_ARRAY_SIZE = 32890;
        public const ushort GL_VERTEX_ARRAY_TYPE = 32891;
        public const ushort GL_VERTEX_ARRAY_STRIDE = 32892;
        public const ushort GL_NORMAL_ARRAY_TYPE = 32894;
        public const ushort GL_NORMAL_ARRAY_STRIDE = 32895;
        public const ushort GL_COLOR_ARRAY_SIZE = 32897;
        public const ushort GL_COLOR_ARRAY_TYPE = 32898;
        public const ushort GL_COLOR_ARRAY_STRIDE = 32899;
        public const ushort GL_INDEX_ARRAY_TYPE = 32901;
        public const ushort GL_INDEX_ARRAY_STRIDE = 32902;
        public const ushort GL_TEXTURE_COORD_ARRAY_SIZE = 32904;
        public const ushort GL_TEXTURE_COORD_ARRAY_TYPE = 32905;
        public const ushort GL_TEXTURE_COORD_ARRAY_STRIDE = 32906;
        public const ushort GL_EDGE_FLAG_ARRAY_STRIDE = 32908;
        public const ushort GL_VERTEX_ARRAY_POINTER = 32910;
        public const ushort GL_NORMAL_ARRAY_POINTER = 32911;
        public const ushort GL_COLOR_ARRAY_POINTER = 32912;
        public const ushort GL_INDEX_ARRAY_POINTER = 32913;
        public const ushort GL_TEXTURE_COORD_ARRAY_POINTER = 32914;
        public const ushort GL_EDGE_FLAG_ARRAY_POINTER = 32915;
        public const ushort GL_V2F = 10784;
        public const ushort GL_V3F = 10785;
        public const ushort GL_C4UB_V2F = 10786;
        public const ushort GL_C4UB_V3F = 10787;
        public const ushort GL_C3F_V3F = 10788;
        public const ushort GL_N3F_V3F = 10789;
        public const ushort GL_C4F_N3F_V3F = 10790;
        public const ushort GL_T2F_V3F = 10791;
        public const ushort GL_T4F_V4F = 10792;
        public const ushort GL_T2F_C4UB_V3F = 10793;
        public const ushort GL_T2F_C3F_V3F = 10794;
        public const ushort GL_T2F_N3F_V3F = 10795;
        public const ushort GL_T2F_C4F_N3F_V3F = 10796;
        public const ushort GL_T4F_C4F_N3F_V4F = 10797;
        public const ushort GL_MATRIX_MODE = 2976;
        public const ushort GL_MODELVIEW = 5888;
        public const ushort GL_PROJECTION = 5889;
        public const ushort GL_TEXTURE = 5890;
        public const ushort GL_POINT_SMOOTH = 2832;
        public const ushort GL_POINT_SIZE = 2833;
        public const ushort GL_POINT_SIZE_GRANULARITY = 2835;
        public const ushort GL_POINT_SIZE_RANGE = 2834;
        public const ushort GL_LINE_SMOOTH = 2848;
        public const ushort GL_LINE_STIPPLE = 2852;
        public const ushort GL_LINE_STIPPLE_PATTERN = 2853;
        public const ushort GL_LINE_STIPPLE_REPEAT = 2854;
        public const ushort GL_LINE_WIDTH = 2849;
        public const ushort GL_LINE_WIDTH_GRANULARITY = 2851;
        public const ushort GL_LINE_WIDTH_RANGE = 2850;
        public const ushort GL_POINT = 6912;
        public const ushort GL_LINE = 6913;
        public const ushort GL_FILL = 6914;
        public const ushort GL_CW = 2304;
        public const ushort GL_CCW = 2305;
        public const ushort GL_FRONT = 1028;
        public const ushort GL_BACK = 1029;
        public const ushort GL_POLYGON_MODE = 2880;
        public const ushort GL_POLYGON_SMOOTH = 2881;
        public const ushort GL_POLYGON_STIPPLE = 2882;
        public const ushort GL_EDGE_FLAG = 2883;
        public const ushort GL_CULL_FACE = 2884;
        public const ushort GL_CULL_FACE_MODE = 2885;
        public const ushort GL_FRONT_FACE = 2886;
        public const ushort GL_POLYGON_OFFSET_FACTOR = 32824;
        public const ushort GL_POLYGON_OFFSET_UNITS = 10752;
        public const ushort GL_POLYGON_OFFSET_POINT = 10753;
        public const ushort GL_POLYGON_OFFSET_LINE = 10754;
        public const ushort GL_POLYGON_OFFSET_FILL = 32823;
        public const ushort GL_COMPILE = 4864;
        public const ushort GL_COMPILE_AND_EXECUTE = 4865;
        public const ushort GL_LIST_BASE = 2866;
        public const ushort GL_LIST_INDEX = 2867;
        public const ushort GL_LIST_MODE = 2864;
        public const ushort GL_NEVER = 512;
        public const ushort GL_LESS = 513;
        public const ushort GL_EQUAL = 514;
        public const ushort GL_LEQUAL = 515;
        public const ushort GL_GREATER = 516;
        public const ushort GL_NOTEQUAL = 517;
        public const ushort GL_GEQUAL = 518;
        public const ushort GL_ALWAYS = 519;
        public const ushort GL_DEPTH_TEST = 2929;
        public const ushort GL_DEPTH_BITS = 3414;
        public const ushort GL_DEPTH_CLEAR_VALUE = 2931;
        public const ushort GL_DEPTH_FUNC = 2932;
        public const ushort GL_DEPTH_RANGE = 2928;
        public const ushort GL_DEPTH_WRITEMASK = 2930;
        public const ushort GL_DEPTH_COMPONENT = 6402;
        public const ushort GL_LIGHTING = 2896;
        public const ushort GL_LIGHT0 = 16384;
        public const ushort GL_LIGHT1 = 16385;
        public const ushort GL_LIGHT2 = 16386;
        public const ushort GL_LIGHT3 = 16387;
        public const ushort GL_LIGHT4 = 16388;
        public const ushort GL_LIGHT5 = 16389;
        public const ushort GL_LIGHT6 = 16390;
        public const ushort GL_LIGHT7 = 16391;
        public const ushort GL_SPOT_EXPONENT = 4613;
        public const ushort GL_SPOT_CUTOFF = 4614;
        public const ushort GL_CONSTANT_ATTENUATION = 4615;
        public const ushort GL_LINEAR_ATTENUATION = 4616;
        public const ushort GL_QUADRATIC_ATTENUATION = 4617;
        public const ushort GL_AMBIENT = 4608;
        public const ushort GL_DIFFUSE = 4609;
        public const ushort GL_SPECULAR = 4610;
        public const ushort GL_SHININESS = 5633;
        public const ushort GL_EMISSION = 5632;
        public const ushort GL_POSITION = 4611;
        public const ushort GL_SPOT_DIRECTION = 4612;
        public const ushort GL_AMBIENT_AND_DIFFUSE = 5634;
        public const ushort GL_COLOR_INDEXES = 5635;
        public const ushort GL_LIGHT_MODEL_TWO_SIDE = 2898;
        public const ushort GL_LIGHT_MODEL_LOCAL_VIEWER = 2897;
        public const ushort GL_LIGHT_MODEL_AMBIENT = 2899;
        public const ushort GL_FRONT_AND_BACK = 1032;
        public const ushort GL_SHADE_MODEL = 2900;
        public const ushort GL_FLAT = 7424;
        public const ushort GL_SMOOTH = 7425;
        public const ushort GL_COLOR_MATERIAL = 2903;
        public const ushort GL_COLOR_MATERIAL_FACE = 2901;
        public const ushort GL_COLOR_MATERIAL_PARAMETER = 2902;
        public const ushort GL_NORMALIZE = 2977;
        public const ushort GL_CLIP_PLANE0 = 12288;
        public const ushort GL_CLIP_PLANE1 = 12289;
        public const ushort GL_CLIP_PLANE2 = 12290;
        public const ushort GL_CLIP_PLANE3 = 12291;
        public const ushort GL_CLIP_PLANE4 = 12292;
        public const ushort GL_CLIP_PLANE5 = 12293;
        public const ushort GL_ACCUM_RED_BITS = 3416;
        public const ushort GL_ACCUM_GREEN_BITS = 3417;
        public const ushort GL_ACCUM_BLUE_BITS = 3418;
        public const ushort GL_ACCUM_ALPHA_BITS = 3419;
        public const ushort GL_ACCUM_CLEAR_VALUE = 2944;
        public const ushort GL_ACCUM = 256;
        public const ushort GL_ADD = 260;
        public const ushort GL_LOAD = 257;
        public const ushort GL_MULT = 259;
        public const ushort GL_RETURN = 258;
        public const ushort GL_ALPHA_TEST = 3008;
        public const ushort GL_ALPHA_TEST_REF = 3010;
        public const ushort GL_ALPHA_TEST_FUNC = 3009;
        public const ushort GL_BLEND = 3042;
        public const ushort GL_BLEND_SRC = 3041;
        public const ushort GL_BLEND_DST = 3040;
        public const ushort GL_ZERO = 0;
        public const ushort GL_ONE = 1;
        public const ushort GL_SRC_COLOR = 768;
        public const ushort GL_ONE_MINUS_SRC_COLOR = 769;
        public const ushort GL_SRC_ALPHA = 770;
        public const ushort GL_ONE_MINUS_SRC_ALPHA = 771;
        public const ushort GL_DST_ALPHA = 772;
        public const ushort GL_ONE_MINUS_DST_ALPHA = 773;
        public const ushort GL_DST_COLOR = 774;
        public const ushort GL_ONE_MINUS_DST_COLOR = 775;
        public const ushort GL_SRC_ALPHA_SATURATE = 776;
        public const ushort GL_FEEDBACK = 7169;
        public const ushort GL_RENDER = 7168;
        public const ushort GL_SELECT = 7170;
        public const ushort GL_2D = 1536;
        public const ushort GL_3D = 1537;
        public const ushort GL_3D_COLOR = 1538;
        public const ushort GL_3D_COLOR_TEXTURE = 1539;
        public const ushort GL_4D_COLOR_TEXTURE = 1540;
        public const ushort GL_POINT_TOKEN = 1793;
        public const ushort GL_LINE_TOKEN = 1794;
        public const ushort GL_LINE_RESET_TOKEN = 1799;
        public const ushort GL_POLYGON_TOKEN = 1795;
        public const ushort GL_BITMAP_TOKEN = 1796;
        public const ushort GL_DRAW_PIXEL_TOKEN = 1797;
        public const ushort GL_COPY_PIXEL_TOKEN = 1798;
        public const ushort GL_PASS_THROUGH_TOKEN = 1792;
        public const ushort GL_FEEDBACK_BUFFER_POINTER = 3568;
        public const ushort GL_FEEDBACK_BUFFER_SIZE = 3569;
        public const ushort GL_FEEDBACK_BUFFER_TYPE = 3570;
        public const ushort GL_SELECTION_BUFFER_POINTER = 3571;
        public const ushort GL_SELECTION_BUFFER_SIZE = 3572;
        public const ushort GL_FOG = 2912;
        public const ushort GL_FOG_MODE = 2917;
        public const ushort GL_FOG_DENSITY = 2914;
        public const ushort GL_FOG_COLOR = 2918;
        public const ushort GL_FOG_INDEX = 2913;
        public const ushort GL_FOG_START = 2915;
        public const ushort GL_FOG_END = 2916;
        public const ushort GL_LINEAR = 9729;
        public const ushort GL_EXP = 2048;
        public const ushort GL_EXP2 = 2049;
        public const ushort GL_LOGIC_OP = 3057;
        public const ushort GL_INDEX_LOGIC_OP = 3057;
        public const ushort GL_COLOR_LOGIC_OP = 3058;
        public const ushort GL_LOGIC_OP_MODE = 3056;
        public const ushort GL_CLEAR = 5376;
        public const ushort GL_SET = 5391;
        public const ushort GL_COPY = 5379;
        public const ushort GL_COPY_INVERTED = 5388;
        public const ushort GL_NOOP = 5381;
        public const ushort GL_INVERT = 5386;
        public const ushort GL_AND = 5377;
        public const ushort GL_NAND = 5390;
        public const ushort GL_OR = 5383;
        public const ushort GL_NOR = 5384;
        public const ushort GL_XOR = 5382;
        public const ushort GL_EQUIV = 5385;
        public const ushort GL_AND_REVERSE = 5378;
        public const ushort GL_AND_INVERTED = 5380;
        public const ushort GL_OR_REVERSE = 5387;
        public const ushort GL_OR_INVERTED = 5389;
        public const ushort GL_STENCIL_BITS = 3415;
        public const ushort GL_STENCIL_TEST = 2960;
        public const ushort GL_STENCIL_CLEAR_VALUE = 2961;
        public const ushort GL_STENCIL_FUNC = 2962;
        public const ushort GL_STENCIL_VALUE_MASK = 2963;
        public const ushort GL_STENCIL_FAIL = 2964;
        public const ushort GL_STENCIL_PASS_DEPTH_FAIL = 2965;
        public const ushort GL_STENCIL_PASS_DEPTH_PASS = 2966;
        public const ushort GL_STENCIL_REF = 2967;
        public const ushort GL_STENCIL_WRITEMASK = 2968;
        public const ushort GL_STENCIL_INDEX = 6401;
        public const ushort GL_KEEP = 7680;
        public const ushort GL_REPLACE = 7681;
        public const ushort GL_INCR = 7682;
        public const ushort GL_DECR = 7683;
        public const ushort GL_NONE = 0;
        public const ushort GL_LEFT = 1030;
        public const ushort GL_RIGHT = 1031;
        public const ushort GL_FRONT_LEFT = 1024;
        public const ushort GL_FRONT_RIGHT = 1025;
        public const ushort GL_BACK_LEFT = 1026;
        public const ushort GL_BACK_RIGHT = 1027;
        public const ushort GL_AUX0 = 1033;
        public const ushort GL_AUX1 = 1034;
        public const ushort GL_AUX2 = 1035;
        public const ushort GL_AUX3 = 1036;
        public const ushort GL_COLOR_INDEX = 6400;
        public const ushort GL_RED = 6403;
        public const ushort GL_GREEN = 6404;
        public const ushort GL_BLUE = 6405;
        public const ushort GL_ALPHA = 6406;
        public const ushort GL_LUMINANCE = 6409;
        public const ushort GL_LUMINANCE_ALPHA = 6410;
        public const ushort GL_ALPHA_BITS = 3413;
        public const ushort GL_RED_BITS = 3410;
        public const ushort GL_GREEN_BITS = 3411;
        public const ushort GL_BLUE_BITS = 3412;
        public const ushort GL_INDEX_BITS = 3409;
        public const ushort GL_SUBPIXEL_BITS = 3408;
        public const ushort GL_AUX_BUFFERS = 3072;
        public const ushort GL_READ_BUFFER = 3074;
        public const ushort GL_DRAW_BUFFER = 3073;
        public const ushort GL_DOUBLEBUFFER = 3122;
        public const ushort GL_STEREO = 3123;
        public const ushort GL_BITMAP = 6656;
        public const ushort GL_COLOR = 6144;
        public const ushort GL_DEPTH = 6145;
        public const ushort GL_STENCIL = 6146;
        public const ushort GL_DITHER = 3024;
        public const ushort GL_RGB = 6407;
        public const ushort GL_RGBA = 6408;
        public const ushort GL_MAX_LIST_NESTING = 2865;
        public const ushort GL_MAX_EVAL_ORDER = 3376;
        public const ushort GL_MAX_LIGHTS = 3377;
        public const ushort GL_MAX_CLIP_PLANES = 3378;
        public const ushort GL_MAX_TEXTURE_SIZE = 3379;
        public const ushort GL_MAX_PIXEL_MAP_TABLE = 3380;
        public const ushort GL_MAX_ATTRIB_STACK_DEPTH = 3381;
        public const ushort GL_MAX_MODELVIEW_STACK_DEPTH = 3382;
        public const ushort GL_MAX_NAME_STACK_DEPTH = 3383;
        public const ushort GL_MAX_PROJECTION_STACK_DEPTH = 3384;
        public const ushort GL_MAX_TEXTURE_STACK_DEPTH = 3385;
        public const ushort GL_MAX_VIEWPORT_DIMS = 3386;
        public const ushort GL_MAX_CLIENT_ATTRIB_STACK_DEPTH = 3387;
        public const ushort GL_ATTRIB_STACK_DEPTH = 2992;
        public const ushort GL_CLIENT_ATTRIB_STACK_DEPTH = 2993;
        public const ushort GL_COLOR_CLEAR_VALUE = 3106;
        public const ushort GL_COLOR_WRITEMASK = 3107;
        public const ushort GL_CURRENT_INDEX = 2817;
        public const ushort GL_CURRENT_COLOR = 2816;
        public const ushort GL_CURRENT_NORMAL = 2818;
        public const ushort GL_CURRENT_RASTER_COLOR = 2820;
        public const ushort GL_CURRENT_RASTER_DISTANCE = 2825;
        public const ushort GL_CURRENT_RASTER_INDEX = 2821;
        public const ushort GL_CURRENT_RASTER_POSITION = 2823;
        public const ushort GL_CURRENT_RASTER_TEXTURE_COORDS = 2822;
        public const ushort GL_CURRENT_RASTER_POSITION_VALID = 2824;
        public const ushort GL_CURRENT_TEXTURE_COORDS = 2819;
        public const ushort GL_INDEX_CLEAR_VALUE = 3104;
        public const ushort GL_INDEX_MODE = 3120;
        public const ushort GL_INDEX_WRITEMASK = 3105;
        public const ushort GL_MODELVIEW_MATRIX = 2982;
        public const ushort GL_MODELVIEW_STACK_DEPTH = 2979;
        public const ushort GL_NAME_STACK_DEPTH = 3440;
        public const ushort GL_PROJECTION_MATRIX = 2983;
        public const ushort GL_PROJECTION_STACK_DEPTH = 2980;
        public const ushort GL_RENDER_MODE = 3136;
        public const ushort GL_RGBA_MODE = 3121;
        public const ushort GL_TEXTURE_MATRIX = 2984;
        public const ushort GL_TEXTURE_STACK_DEPTH = 2981;
        public const ushort GL_VIEWPORT = 2978;
        public const ushort GL_AUTO_NORMAL = 3456;
        public const ushort GL_MAP1_COLOR_4 = 3472;
        public const ushort GL_MAP1_INDEX = 3473;
        public const ushort GL_MAP1_NORMAL = 3474;
        public const ushort GL_MAP1_TEXTURE_COORD_1 = 3475;
        public const ushort GL_MAP1_TEXTURE_COORD_2 = 3476;
        public const ushort GL_MAP1_TEXTURE_COORD_3 = 3477;
        public const ushort GL_MAP1_TEXTURE_COORD_4 = 3478;
        public const ushort GL_MAP1_VERTEX_3 = 3479;
        public const ushort GL_MAP1_VERTEX_4 = 3480;
        public const ushort GL_MAP2_COLOR_4 = 3504;
        public const ushort GL_MAP2_INDEX = 3505;
        public const ushort GL_MAP2_NORMAL = 3506;
        public const ushort GL_MAP2_TEXTURE_COORD_1 = 3507;
        public const ushort GL_MAP2_TEXTURE_COORD_2 = 3508;
        public const ushort GL_MAP2_TEXTURE_COORD_3 = 3509;
        public const ushort GL_MAP2_TEXTURE_COORD_4 = 3510;
        public const ushort GL_MAP2_VERTEX_3 = 3511;
        public const ushort GL_MAP2_VERTEX_4 = 3512;
        public const ushort GL_MAP1_GRID_DOMAIN = 3536;
        public const ushort GL_MAP1_GRID_SEGMENTS = 3537;
        public const ushort GL_MAP2_GRID_DOMAIN = 3538;
        public const ushort GL_MAP2_GRID_SEGMENTS = 3539;
        public const ushort GL_COEFF = 2560;
        public const ushort GL_ORDER = 2561;
        public const ushort GL_DOMAIN = 2562;
        public const ushort GL_PERSPECTIVE_CORRECTION_HINT = 3152;
        public const ushort GL_POINT_SMOOTH_HINT = 3153;
        public const ushort GL_LINE_SMOOTH_HINT = 3154;
        public const ushort GL_POLYGON_SMOOTH_HINT = 3155;
        public const ushort GL_FOG_HINT = 3156;
        public const ushort GL_DONT_CARE = 4352;
        public const ushort GL_FASTEST = 4353;
        public const ushort GL_NICEST = 4354;
        public const ushort GL_SCISSOR_BOX = 3088;
        public const ushort GL_SCISSOR_TEST = 3089;
        public const ushort GL_MAP_COLOR = 3344;
        public const ushort GL_MAP_STENCIL = 3345;
        public const ushort GL_INDEX_SHIFT = 3346;
        public const ushort GL_INDEX_OFFSET = 3347;
        public const ushort GL_RED_SCALE = 3348;
        public const ushort GL_RED_BIAS = 3349;
        public const ushort GL_GREEN_SCALE = 3352;
        public const ushort GL_GREEN_BIAS = 3353;
        public const ushort GL_BLUE_SCALE = 3354;
        public const ushort GL_BLUE_BIAS = 3355;
        public const ushort GL_ALPHA_SCALE = 3356;
        public const ushort GL_ALPHA_BIAS = 3357;
        public const ushort GL_DEPTH_SCALE = 3358;
        public const ushort GL_DEPTH_BIAS = 3359;
        public const ushort GL_PIXEL_MAP_S_TO_S_SIZE = 3249;
        public const ushort GL_PIXEL_MAP_I_TO_I_SIZE = 3248;
        public const ushort GL_PIXEL_MAP_I_TO_R_SIZE = 3250;
        public const ushort GL_PIXEL_MAP_I_TO_G_SIZE = 3251;
        public const ushort GL_PIXEL_MAP_I_TO_B_SIZE = 3252;
        public const ushort GL_PIXEL_MAP_I_TO_A_SIZE = 3253;
        public const ushort GL_PIXEL_MAP_R_TO_R_SIZE = 3254;
        public const ushort GL_PIXEL_MAP_G_TO_G_SIZE = 3255;
        public const ushort GL_PIXEL_MAP_B_TO_B_SIZE = 3256;
        public const ushort GL_PIXEL_MAP_A_TO_A_SIZE = 3257;
        public const ushort GL_PIXEL_MAP_S_TO_S = 3185;
        public const ushort GL_PIXEL_MAP_I_TO_I = 3184;
        public const ushort GL_PIXEL_MAP_I_TO_R = 3186;
        public const ushort GL_PIXEL_MAP_I_TO_G = 3187;
        public const ushort GL_PIXEL_MAP_I_TO_B = 3188;
        public const ushort GL_PIXEL_MAP_I_TO_A = 3189;
        public const ushort GL_PIXEL_MAP_R_TO_R = 3190;
        public const ushort GL_PIXEL_MAP_G_TO_G = 3191;
        public const ushort GL_PIXEL_MAP_B_TO_B = 3192;
        public const ushort GL_PIXEL_MAP_A_TO_A = 3193;
        public const ushort GL_PACK_ALIGNMENT = 3333;
        public const ushort GL_PACK_LSB_FIRST = 3329;
        public const ushort GL_PACK_ROW_LENGTH = 3330;
        public const ushort GL_PACK_SKIP_PIXELS = 3332;
        public const ushort GL_PACK_SKIP_ROWS = 3331;
        public const ushort GL_PACK_SWAP_BYTES = 3328;
        public const ushort GL_UNPACK_ALIGNMENT = 3317;
        public const ushort GL_UNPACK_LSB_FIRST = 3313;
        public const ushort GL_UNPACK_ROW_LENGTH = 3314;
        public const ushort GL_UNPACK_SKIP_PIXELS = 3316;
        public const ushort GL_UNPACK_SKIP_ROWS = 3315;
        public const ushort GL_UNPACK_SWAP_BYTES = 3312;
        public const ushort GL_ZOOM_X = 3350;
        public const ushort GL_ZOOM_Y = 3351;
        public const ushort GL_TEXTURE_ENV = 8960;
        public const ushort GL_TEXTURE_ENV_MODE = 8704;
        public const ushort GL_TEXTURE_1D = 3552;
        public const ushort GL_TEXTURE_2D = 3553;
        public const ushort GL_TEXTURE_WRAP_S = 10242;
        public const ushort GL_TEXTURE_WRAP_T = 10243;
        public const ushort GL_TEXTURE_MAG_FILTER = 10240;
        public const ushort GL_TEXTURE_MIN_FILTER = 10241;
        public const ushort GL_TEXTURE_ENV_COLOR = 8705;
        public const ushort GL_TEXTURE_GEN_S = 3168;
        public const ushort GL_TEXTURE_GEN_T = 3169;
        public const ushort GL_TEXTURE_GEN_MODE = 9472;
        public const ushort GL_TEXTURE_BORDER_COLOR = 4100;
        public const ushort GL_TEXTURE_WIDTH = 4096;
        public const ushort GL_TEXTURE_HEIGHT = 4097;
        public const ushort GL_TEXTURE_BORDER = 4101;
        public const ushort GL_TEXTURE_COMPONENTS = 4099;
        public const ushort GL_TEXTURE_RED_SIZE = 32860;
        public const ushort GL_TEXTURE_GREEN_SIZE = 32861;
        public const ushort GL_TEXTURE_BLUE_SIZE = 32862;
        public const ushort GL_TEXTURE_ALPHA_SIZE = 32863;
        public const ushort GL_TEXTURE_LUMINANCE_SIZE = 32864;
        public const ushort GL_TEXTURE_INTENSITY_SIZE = 32865;
        public const ushort GL_NEAREST_MIPMAP_NEAREST = 9984;
        public const ushort GL_NEAREST_MIPMAP_LINEAR = 9986;
        public const ushort GL_LINEAR_MIPMAP_NEAREST = 9985;
        public const ushort GL_LINEAR_MIPMAP_LINEAR = 9987;
        public const ushort GL_OBJECT_LINEAR = 9217;
        public const ushort GL_OBJECT_PLANE = 9473;
        public const ushort GL_EYE_LINEAR = 9216;
        public const ushort GL_EYE_PLANE = 9474;
        public const ushort GL_SPHERE_MAP = 9218;
        public const ushort GL_DECAL = 8449;
        public const ushort GL_MODULATE = 8448;
        public const ushort GL_NEAREST = 9728;
        public const ushort GL_REPEAT = 10497;
        public const ushort GL_CLAMP = 10496;
        public const ushort GL_S = 8192;
        public const ushort GL_T = 8193;
        public const ushort GL_R = 8194;
        public const ushort GL_Q = 8195;
        public const ushort GL_TEXTURE_GEN_R = 3170;
        public const ushort GL_TEXTURE_GEN_Q = 3171;
        public const ushort GL_VENDOR = 7936;
        public const ushort GL_RENDERER = 7937;
        public const ushort GL_VERSION = 7938;
        public const ushort GL_EXTENSIONS = 7939;
        public const ushort GL_NO_ERROR = 0;
        public const ushort GL_INVALID_ENUM = 1280;
        public const ushort GL_INVALID_VALUE = 1281;
        public const ushort GL_INVALID_OPERATION = 1282;
        public const ushort GL_STACK_OVERFLOW = 1283;
        public const ushort GL_STACK_UNDERFLOW = 1284;
        public const ushort GL_OUT_OF_MEMORY = 1285;
        public const int GL_CURRENT_BIT = 1;
        public const int GL_POINT_BIT = 2;
        public const int GL_LINE_BIT = 4;
        public const int GL_POLYGON_BIT = 8;
        public const int GL_POLYGON_STIPPLE_BIT = 16;
        public const int GL_PIXEL_MODE_BIT = 32;
        public const int GL_LIGHTING_BIT = 64;
        public const int GL_FOG_BIT = 128;
        public const int GL_DEPTH_BUFFER_BIT = 256;
        public const int GL_ACCUM_BUFFER_BIT = 512;
        public const int GL_STENCIL_BUFFER_BIT = 1024;
        public const int GL_VIEWPORT_BIT = 2048;
        public const int GL_TRANSFORM_BIT = 4096;
        public const int GL_ENABLE_BIT = 8192;
        public const int GL_COLOR_BUFFER_BIT = 16384;
        public const int GL_HINT_BIT = 32768;
        public const int GL_EVAL_BIT = 65536;
        public const int GL_LIST_BIT = 131072;
        public const int GL_TEXTURE_BIT = 262144;
        public const int GL_SCISSOR_BIT = 524288;
        public const int GL_ALL_ATTRIB_BITS = 1048575;
        public const ushort GL_PROXY_TEXTURE_1D = 32867;
        public const ushort GL_PROXY_TEXTURE_2D = 32868;
        public const ushort GL_TEXTURE_PRIORITY = 32870;
        public const ushort GL_TEXTURE_RESIDENT = 32871;
        public const ushort GL_TEXTURE_BINDING_1D = 32872;
        public const ushort GL_TEXTURE_BINDING_2D = 32873;
        public const ushort GL_TEXTURE_INTERNAL_FORMAT = 4099;
        public const ushort GL_ALPHA4 = 32827;
        public const ushort GL_ALPHA8 = 32828;
        public const ushort GL_ALPHA12 = 32829;
        public const ushort GL_ALPHA16 = 32830;
        public const ushort GL_LUMINANCE4 = 32831;
        public const ushort GL_LUMINANCE8 = 32832;
        public const ushort GL_LUMINANCE12 = 32833;
        public const ushort GL_LUMINANCE16 = 32834;
        public const ushort GL_LUMINANCE4_ALPHA4 = 32835;
        public const ushort GL_LUMINANCE6_ALPHA2 = 32836;
        public const ushort GL_LUMINANCE8_ALPHA8 = 32837;
        public const ushort GL_LUMINANCE12_ALPHA4 = 32838;
        public const ushort GL_LUMINANCE12_ALPHA12 = 32839;
        public const ushort GL_LUMINANCE16_ALPHA16 = 32840;
        public const ushort GL_INTENSITY = 32841;
        public const ushort GL_INTENSITY4 = 32842;
        public const ushort GL_INTENSITY8 = 32843;
        public const ushort GL_INTENSITY12 = 32844;
        public const ushort GL_INTENSITY16 = 32845;
        public const ushort GL_R3_G3_B2 = 10768;
        public const ushort GL_RGB4 = 32847;
        public const ushort GL_RGB5 = 32848;
        public const ushort GL_RGB8 = 32849;
        public const ushort GL_RGB10 = 32850;
        public const ushort GL_RGB12 = 32851;
        public const ushort GL_RGB16 = 32852;
        public const ushort GL_RGBA2 = 32853;
        public const ushort GL_RGBA4 = 32854;
        public const ushort GL_RGB5_A1 = 32855;
        public const ushort GL_RGBA8 = 32856;
        public const ushort GL_RGB10_A2 = 32857;
        public const ushort GL_RGBA12 = 32858;
        public const ushort GL_RGBA16 = 32859;
        public const uint GL_CLIENT_PIXEL_STORE_BIT = 1;
        public const uint GL_CLIENT_VERTEX_ARRAY_BIT = 2;
        public const uint GL_ALL_CLIENT_ATTRIB_BITS = 4294967295;
        public const uint GL_CLIENT_ALL_ATTRIB_BITS = 4294967295;
        public const ushort GL_RESCALE_NORMAL = 32826;
        public const ushort GL_CLAMP_TO_EDGE = 33071;
        public const ushort GL_MAX_ELEMENTS_VERTICES = 33000;
        public const ushort GL_MAX_ELEMENTS_INDICES = 33001;
        public const ushort GL_BGR = 32992;
        public const ushort GL_BGRA = 32993;
        public const ushort GL_UNSIGNED_BYTE_3_3_2 = 32818;
        public const ushort GL_UNSIGNED_BYTE_2_3_3_REV = 33634;
        public const ushort GL_UNSIGNED_SHORT_5_6_5 = 33635;
        public const ushort GL_UNSIGNED_SHORT_5_6_5_REV = 33636;
        public const ushort GL_UNSIGNED_SHORT_4_4_4_4 = 32819;
        public const ushort GL_UNSIGNED_SHORT_4_4_4_4_REV = 33637;
        public const ushort GL_UNSIGNED_SHORT_5_5_5_1 = 32820;
        public const ushort GL_UNSIGNED_SHORT_1_5_5_5_REV = 33638;
        public const ushort GL_UNSIGNED_INT_8_8_8_8 = 32821;
        public const ushort GL_UNSIGNED_INT_8_8_8_8_REV = 33639;
        public const ushort GL_UNSIGNED_INT_10_10_10_2 = 32822;
        public const ushort GL_UNSIGNED_INT_2_10_10_10_REV = 33640;
        public const ushort GL_LIGHT_MODEL_COLOR_CONTROL = 33272;
        public const ushort GL_SINGLE_COLOR = 33273;
        public const ushort GL_SEPARATE_SPECULAR_COLOR = 33274;
        public const ushort GL_TEXTURE_MIN_LOD = 33082;
        public const ushort GL_TEXTURE_MAX_LOD = 33083;
        public const ushort GL_TEXTURE_BASE_LEVEL = 33084;
        public const ushort GL_TEXTURE_MAX_LEVEL = 33085;
        public const ushort GL_SMOOTH_POINT_SIZE_RANGE = 2834;
        public const ushort GL_SMOOTH_POINT_SIZE_GRANULARITY = 2835;
        public const ushort GL_SMOOTH_LINE_WIDTH_RANGE = 2850;
        public const ushort GL_SMOOTH_LINE_WIDTH_GRANULARITY = 2851;
        public const ushort GL_ALIASED_POINT_SIZE_RANGE = 33901;
        public const ushort GL_ALIASED_LINE_WIDTH_RANGE = 33902;
        public const ushort GL_PACK_SKIP_IMAGES = 32875;
        public const ushort GL_PACK_IMAGE_HEIGHT = 32876;
        public const ushort GL_UNPACK_SKIP_IMAGES = 32877;
        public const ushort GL_UNPACK_IMAGE_HEIGHT = 32878;
        public const ushort GL_TEXTURE_3D = 32879;
        public const ushort GL_PROXY_TEXTURE_3D = 32880;
        public const ushort GL_TEXTURE_DEPTH = 32881;
        public const ushort GL_TEXTURE_WRAP_R = 32882;
        public const ushort GL_MAX_3D_TEXTURE_SIZE = 32883;
        public const ushort GL_TEXTURE_BINDING_3D = 32874;
        public const ushort GL_CONSTANT_COLOR = 32769;
        public const ushort GL_ONE_MINUS_CONSTANT_COLOR = 32770;
        public const ushort GL_CONSTANT_ALPHA = 32771;
        public const ushort GL_ONE_MINUS_CONSTANT_ALPHA = 32772;
        public const ushort GL_COLOR_TABLE = 32976;
        public const ushort GL_POST_CONVOLUTION_COLOR_TABLE = 32977;
        public const ushort GL_POST_COLOR_MATRIX_COLOR_TABLE = 32978;
        public const ushort GL_PROXY_COLOR_TABLE = 32979;
        public const ushort GL_PROXY_POST_CONVOLUTION_COLOR_TABLE = 32980;
        public const ushort GL_PROXY_POST_COLOR_MATRIX_COLOR_TABLE = 32981;
        public const ushort GL_COLOR_TABLE_SCALE = 32982;
        public const ushort GL_COLOR_TABLE_BIAS = 32983;
        public const ushort GL_COLOR_TABLE_FORMAT = 32984;
        public const ushort GL_COLOR_TABLE_WIDTH = 32985;
        public const ushort GL_COLOR_TABLE_RED_SIZE = 32986;
        public const ushort GL_COLOR_TABLE_GREEN_SIZE = 32987;
        public const ushort GL_COLOR_TABLE_BLUE_SIZE = 32988;
        public const ushort GL_COLOR_TABLE_ALPHA_SIZE = 32989;
        public const ushort GL_COLOR_TABLE_LUMINANCE_SIZE = 32990;
        public const ushort GL_COLOR_TABLE_INTENSITY_SIZE = 32991;
        public const ushort GL_CONVOLUTION_1D = 32784;
        public const ushort GL_CONVOLUTION_2D = 32785;
        public const ushort GL_SEPARABLE_2D = 32786;
        public const ushort GL_CONVOLUTION_BORDER_MODE = 32787;
        public const ushort GL_CONVOLUTION_FILTER_SCALE = 32788;
        public const ushort GL_CONVOLUTION_FILTER_BIAS = 32789;
        public const ushort GL_REDUCE = 32790;
        public const ushort GL_CONVOLUTION_FORMAT = 32791;
        public const ushort GL_CONVOLUTION_WIDTH = 32792;
        public const ushort GL_CONVOLUTION_HEIGHT = 32793;
        public const ushort GL_MAX_CONVOLUTION_WIDTH = 32794;
        public const ushort GL_MAX_CONVOLUTION_HEIGHT = 32795;
        public const ushort GL_POST_CONVOLUTION_RED_SCALE = 32796;
        public const ushort GL_POST_CONVOLUTION_GREEN_SCALE = 32797;
        public const ushort GL_POST_CONVOLUTION_BLUE_SCALE = 32798;
        public const ushort GL_POST_CONVOLUTION_ALPHA_SCALE = 32799;
        public const ushort GL_POST_CONVOLUTION_RED_BIAS = 32800;
        public const ushort GL_POST_CONVOLUTION_GREEN_BIAS = 32801;
        public const ushort GL_POST_CONVOLUTION_BLUE_BIAS = 32802;
        public const ushort GL_POST_CONVOLUTION_ALPHA_BIAS = 32803;
        public const ushort GL_CONSTANT_BORDER = 33105;
        public const ushort GL_REPLICATE_BORDER = 33107;
        public const ushort GL_CONVOLUTION_BORDER_COLOR = 33108;
        public const ushort GL_COLOR_MATRIX = 32945;
        public const ushort GL_COLOR_MATRIX_STACK_DEPTH = 32946;
        public const ushort GL_MAX_COLOR_MATRIX_STACK_DEPTH = 32947;
        public const ushort GL_POST_COLOR_MATRIX_RED_SCALE = 32948;
        public const ushort GL_POST_COLOR_MATRIX_GREEN_SCALE = 32949;
        public const ushort GL_POST_COLOR_MATRIX_BLUE_SCALE = 32950;
        public const ushort GL_POST_COLOR_MATRIX_ALPHA_SCALE = 32951;
        public const ushort GL_POST_COLOR_MATRIX_RED_BIAS = 32952;
        public const ushort GL_POST_COLOR_MATRIX_GREEN_BIAS = 32953;
        public const ushort GL_POST_COLOR_MATRIX_BLUE_BIAS = 32954;
        public const ushort GL_POST_COLOR_MATRIX_ALPHA_BIAS = 32955;
        public const ushort GL_HISTOGRAM = 32804;
        public const ushort GL_PROXY_HISTOGRAM = 32805;
        public const ushort GL_HISTOGRAM_WIDTH = 32806;
        public const ushort GL_HISTOGRAM_FORMAT = 32807;
        public const ushort GL_HISTOGRAM_RED_SIZE = 32808;
        public const ushort GL_HISTOGRAM_GREEN_SIZE = 32809;
        public const ushort GL_HISTOGRAM_BLUE_SIZE = 32810;
        public const ushort GL_HISTOGRAM_ALPHA_SIZE = 32811;
        public const ushort GL_HISTOGRAM_LUMINANCE_SIZE = 32812;
        public const ushort GL_HISTOGRAM_SINK = 32813;
        public const ushort GL_MINMAX = 32814;
        public const ushort GL_MINMAX_FORMAT = 32815;
        public const ushort GL_MINMAX_SINK = 32816;
        public const ushort GL_TABLE_TOO_LARGE = 32817;
        public const ushort GL_BLEND_EQUATION = 32777;
        public const ushort GL_MIN = 32775;
        public const ushort GL_MAX = 32776;
        public const ushort GL_FUNC_ADD = 32774;
        public const ushort GL_FUNC_SUBTRACT = 32778;
        public const ushort GL_FUNC_REVERSE_SUBTRACT = 32779;
        public const ushort GL_BLEND_COLOR = 32773;
        public const ushort GL_TEXTURE0 = 33984;
        public const ushort GL_TEXTURE1 = 33985;
        public const ushort GL_TEXTURE2 = 33986;
        public const ushort GL_TEXTURE3 = 33987;
        public const ushort GL_TEXTURE4 = 33988;
        public const ushort GL_TEXTURE5 = 33989;
        public const ushort GL_TEXTURE6 = 33990;
        public const ushort GL_TEXTURE7 = 33991;
        public const ushort GL_TEXTURE8 = 33992;
        public const ushort GL_TEXTURE9 = 33993;
        public const ushort GL_TEXTURE10 = 33994;
        public const ushort GL_TEXTURE11 = 33995;
        public const ushort GL_TEXTURE12 = 33996;
        public const ushort GL_TEXTURE13 = 33997;
        public const ushort GL_TEXTURE14 = 33998;
        public const ushort GL_TEXTURE15 = 33999;
        public const ushort GL_TEXTURE16 = 34000;
        public const ushort GL_TEXTURE17 = 34001;
        public const ushort GL_TEXTURE18 = 34002;
        public const ushort GL_TEXTURE19 = 34003;
        public const ushort GL_TEXTURE20 = 34004;
        public const ushort GL_TEXTURE21 = 34005;
        public const ushort GL_TEXTURE22 = 34006;
        public const ushort GL_TEXTURE23 = 34007;
        public const ushort GL_TEXTURE24 = 34008;
        public const ushort GL_TEXTURE25 = 34009;
        public const ushort GL_TEXTURE26 = 34010;
        public const ushort GL_TEXTURE27 = 34011;
        public const ushort GL_TEXTURE28 = 34012;
        public const ushort GL_TEXTURE29 = 34013;
        public const ushort GL_TEXTURE30 = 34014;
        public const ushort GL_TEXTURE31 = 34015;
        public const ushort GL_ACTIVE_TEXTURE = 34016;
        public const ushort GL_CLIENT_ACTIVE_TEXTURE = 34017;
        public const ushort GL_MAX_TEXTURE_UNITS = 34018;
        public const ushort GL_NORMAL_MAP = 34065;
        public const ushort GL_REFLECTION_MAP = 34066;
        public const ushort GL_TEXTURE_CUBE_MAP = 34067;
        public const ushort GL_TEXTURE_BINDING_CUBE_MAP = 34068;
        public const ushort GL_TEXTURE_CUBE_MAP_POSITIVE_X = 34069;
        public const ushort GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 34070;
        public const ushort GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 34071;
        public const ushort GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 34072;
        public const ushort GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 34073;
        public const ushort GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 34074;
        public const ushort GL_PROXY_TEXTURE_CUBE_MAP = 34075;
        public const ushort GL_MAX_CUBE_MAP_TEXTURE_SIZE = 34076;
        public const ushort GL_COMPRESSED_ALPHA = 34025;
        public const ushort GL_COMPRESSED_LUMINANCE = 34026;
        public const ushort GL_COMPRESSED_LUMINANCE_ALPHA = 34027;
        public const ushort GL_COMPRESSED_INTENSITY = 34028;
        public const ushort GL_COMPRESSED_RGB = 34029;
        public const ushort GL_COMPRESSED_RGBA = 34030;
        public const ushort GL_TEXTURE_COMPRESSION_HINT = 34031;
        public const ushort GL_TEXTURE_COMPRESSED_IMAGE_SIZE = 34464;
        public const ushort GL_TEXTURE_COMPRESSED = 34465;
        public const ushort GL_NUM_COMPRESSED_TEXTURE_FORMATS = 34466;
        public const ushort GL_COMPRESSED_TEXTURE_FORMATS = 34467;
        public const ushort GL_MULTISAMPLE = 32925;
        public const ushort GL_SAMPLE_ALPHA_TO_COVERAGE = 32926;
        public const ushort GL_SAMPLE_ALPHA_TO_ONE = 32927;
        public const ushort GL_SAMPLE_COVERAGE = 32928;
        public const ushort GL_SAMPLE_BUFFERS = 32936;
        public const ushort GL_SAMPLES = 32937;
        public const ushort GL_SAMPLE_COVERAGE_VALUE = 32938;
        public const ushort GL_SAMPLE_COVERAGE_INVERT = 32939;
        public const uint GL_MULTISAMPLE_BIT = 536870912;
        public const ushort GL_TRANSPOSE_MODELVIEW_MATRIX = 34019;
        public const ushort GL_TRANSPOSE_PROJECTION_MATRIX = 34020;
        public const ushort GL_TRANSPOSE_TEXTURE_MATRIX = 34021;
        public const ushort GL_TRANSPOSE_COLOR_MATRIX = 34022;
        public const ushort GL_COMBINE = 34160;
        public const ushort GL_COMBINE_RGB = 34161;
        public const ushort GL_COMBINE_ALPHA = 34162;
        public const ushort GL_SOURCE0_RGB = 34176;
        public const ushort GL_SOURCE1_RGB = 34177;
        public const ushort GL_SOURCE2_RGB = 34178;
        public const ushort GL_SOURCE0_ALPHA = 34184;
        public const ushort GL_SOURCE1_ALPHA = 34185;
        public const ushort GL_SOURCE2_ALPHA = 34186;
        public const ushort GL_OPERAND0_RGB = 34192;
        public const ushort GL_OPERAND1_RGB = 34193;
        public const ushort GL_OPERAND2_RGB = 34194;
        public const ushort GL_OPERAND0_ALPHA = 34200;
        public const ushort GL_OPERAND1_ALPHA = 34201;
        public const ushort GL_OPERAND2_ALPHA = 34202;
        public const ushort GL_RGB_SCALE = 34163;
        public const ushort GL_ADD_SIGNED = 34164;
        public const ushort GL_INTERPOLATE = 34165;
        public const ushort GL_SUBTRACT = 34023;
        public const ushort GL_CONSTANT = 34166;
        public const ushort GL_PRIMARY_COLOR = 34167;
        public const ushort GL_PREVIOUS = 34168;
        public const ushort GL_DOT3_RGB = 34478;
        public const ushort GL_DOT3_RGBA = 34479;
        public const ushort GL_CLAMP_TO_BORDER = 33069;
        public const ushort GL_TEXTURE0_ARB = 33984;
        public const ushort GL_TEXTURE1_ARB = 33985;
        public const ushort GL_TEXTURE2_ARB = 33986;
        public const ushort GL_TEXTURE3_ARB = 33987;
        public const ushort GL_TEXTURE4_ARB = 33988;
        public const ushort GL_TEXTURE5_ARB = 33989;
        public const ushort GL_TEXTURE6_ARB = 33990;
        public const ushort GL_TEXTURE7_ARB = 33991;
        public const ushort GL_TEXTURE8_ARB = 33992;
        public const ushort GL_TEXTURE9_ARB = 33993;
        public const ushort GL_TEXTURE10_ARB = 33994;
        public const ushort GL_TEXTURE11_ARB = 33995;
        public const ushort GL_TEXTURE12_ARB = 33996;
        public const ushort GL_TEXTURE13_ARB = 33997;
        public const ushort GL_TEXTURE14_ARB = 33998;
        public const ushort GL_TEXTURE15_ARB = 33999;
        public const ushort GL_TEXTURE16_ARB = 34000;
        public const ushort GL_TEXTURE17_ARB = 34001;
        public const ushort GL_TEXTURE18_ARB = 34002;
        public const ushort GL_TEXTURE19_ARB = 34003;
        public const ushort GL_TEXTURE20_ARB = 34004;
        public const ushort GL_TEXTURE21_ARB = 34005;
        public const ushort GL_TEXTURE22_ARB = 34006;
        public const ushort GL_TEXTURE23_ARB = 34007;
        public const ushort GL_TEXTURE24_ARB = 34008;
        public const ushort GL_TEXTURE25_ARB = 34009;
        public const ushort GL_TEXTURE26_ARB = 34010;
        public const ushort GL_TEXTURE27_ARB = 34011;
        public const ushort GL_TEXTURE28_ARB = 34012;
        public const ushort GL_TEXTURE29_ARB = 34013;
        public const ushort GL_TEXTURE30_ARB = 34014;
        public const ushort GL_TEXTURE31_ARB = 34015;
        public const ushort GL_ACTIVE_TEXTURE_ARB = 34016;
        public const ushort GL_CLIENT_ACTIVE_TEXTURE_ARB = 34017;
        public const ushort GL_MAX_TEXTURE_UNITS_ARB = 34018;
        #endregion

        public struct glArray4f
        {
            public float f0;
            public float f1;
            public float f2;
            public float f3;

            public glArray4f(float F0, float F1, float F2, float F3)
            {
                this.f0 = F0;
                this.f1 = F1;
                this.f2 = F2;
                this.f3 = F3;
            }

        }

        public interface GLVertexData
        {
            GLVertexElementType[] DataComponents { get; }

            int VertexCount { get; }

            void ExtractTo(Vertex[] dst, int count);

            void ExtractTo(VertexPosTexColNorm[] dst, int dstOffset, int count);
        }

        public enum GLVertexElementType
        {
            Position,
            Normal,
            Color,
            TextureCoordinate0,
            TextureCoordinate1,
            BlendWeight,
            BlendIndex,
        }

        public struct Vertex : IVertexType
        {
            public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[7]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(20, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(24, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(36, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
                new VertexElement(52, VertexElementFormat.Byte4, VertexElementUsage.BlendIndices, 0),
                new VertexElement(56, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1)
            });
            public Vector3 Position;
            public Vector2 TextureCoordinate;
            public Color Color;
            public Vector3 Normal;
            public Vector4 BlendWeight;
            public Byte4 BlendIndices;
            public Vector2 TextureCoordinate2;

            VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
        }

        public struct VertexPosTexColNorm : IVertexType
        {
            public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[4]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(20, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(24, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
            });
            public Vector3 Position;
            public Vector2 TextureCoordinate;
            public Color Color;
            public Vector3 Normal;

            VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
        }

        public class BufferItem
        {
            public object rawData;
            public object buffer;
            public int rawDataSize;
        }

        public class VertexBufferDesc
        {
            public Matrix TextureMatrix = Matrix.Identity;
            public Matrix Texture2Matrix = Matrix.Identity;
            public GLVertexElementType[] Components;
            public VertexBuffer Buffer;
            public Vertex[] vertices;
            public bool needSetBufferData;
            public uint VertexColor;
        }

        public interface GLIndexBuffer
        {
            int Size { get; }

            void ExtractTo(ushort[] dst);
        }

        private struct DrawArraysCallContext
        {
            public bool isPending;
            public int vertexArrayOffset;
            public int vertexCount;
            public Texture2D texture;
            public BlendState blendState;
            public DepthStencilState depthStencilState;
            public RasterizerState rasterizerState;
            public SamplerState samplerState;
            public Vector3 ambientColor;
            public Vector3 diffuseColor;
            public float diffuseColorAlpha;
            public Vector3 emissiveColor;
            public Vector3 specularColor;
            public float specularPower;
            public bool textureEnabled;
            public bool normalArrayEnabled;
            public bool vertexColorEnabled;
            public bool alphaTestEnabled;
            public CompareFunction alphaFunction;
            public int referenceAlpha;
            public Matrix view;
            public Matrix projection;
            public bool fogEnabled;
            public Vector3 fogColor;
            public float fogStart;
            public float fogEnd;
            public bool DirLight0Enabled;
            public Vector3 DirLight0DiffuseColor;
            public Vector3 DirLight0Direction;
            public Vector3 DirLight0SpecularColor;
            public bool DirLight1Enabled;
            public Vector3 DirLight1DiffuseColor;
            public Vector3 DirLight1Direction;
            public Vector3 DirLight1SpecularColor;
            public bool DirLight2Enabled;
            public Vector3 DirLight2DiffuseColor;
            public Vector3 DirLight2Direction;
            public Vector3 DirLight2SpecularColor;

            public bool isEqualTo(ref DrawArraysCallContext other)
            {
                if (this.alphaTestEnabled != other.alphaTestEnabled || this.blendState != other.blendState || (this.depthStencilState != other.depthStencilState || this.rasterizerState != other.rasterizerState) || (this.samplerState != other.samplerState || this.vertexColorEnabled != other.vertexColorEnabled || (this.fogEnabled != other.fogEnabled || this.texture != other.texture)) || this.projection != other.projection || this.fogEnabled && (this.fogColor != other.fogColor || fogStart != (double)other.fogStart || fogEnd != (double)other.fogEnd))
                    return false;
                if (this.alphaTestEnabled)
                {
                    if (!this.normalArrayEnabled && (this.diffuseColor != other.diffuseColor || diffuseColorAlpha != (double)other.diffuseColorAlpha) || (this.alphaFunction != other.alphaFunction || this.referenceAlpha != other.referenceAlpha || this.referenceAlpha != other.referenceAlpha))
                        return false;
                }
                else if (!this.normalArrayEnabled && (this.diffuseColor != other.diffuseColor || diffuseColorAlpha != (double)other.diffuseColorAlpha || (this.ambientColor != other.ambientColor || this.specularColor != other.specularColor) || (this.emissiveColor != other.emissiveColor || this.DirLight0Enabled != other.DirLight0Enabled || (this.DirLight1Enabled != other.DirLight1Enabled || this.DirLight2Enabled != other.DirLight2Enabled)) || this.DirLight0Enabled && (this.DirLight0DiffuseColor != other.DirLight0DiffuseColor || this.DirLight0Direction != other.DirLight0Direction || this.DirLight0SpecularColor != other.DirLight0SpecularColor) || (this.DirLight1Enabled && (this.DirLight1DiffuseColor != other.DirLight1DiffuseColor || this.DirLight1Direction != other.DirLight1Direction || this.DirLight1SpecularColor != other.DirLight1SpecularColor) || this.DirLight2Enabled && (this.DirLight2DiffuseColor != other.DirLight2DiffuseColor || this.DirLight2Direction != other.DirLight2Direction || this.DirLight2SpecularColor != other.DirLight2SpecularColor))))
                    return false;
                return true;
            }
        }

        private class TextureUnit
        {
            public bool Enabled;
            public uint BoundTexture;
            public bool TexCoordArrayEnabled;
            public int TexCoordArraySize;
            public uint TexCoordArrayDataType;
            public int TexCoordArrayStride;
            public GLVertexData TexCoordArray;
            public SamplerState SamplerState;
            public Stack<Matrix> MatrixStack;

            public TextureUnit()
            {
                this.SamplerState = new SamplerState()
                {
                    AddressU = TextureAddressMode.Wrap,
                    AddressV = TextureAddressMode.Wrap,
                    AddressW = TextureAddressMode.Wrap
                };

                this.MatrixStack = new Stack<Matrix>();
                this.MatrixStack.Push(Matrix.Identity);
            }
        }

        private class LightUnit
        {
            public bool Enabled;
            public Vector3 DiffuseColor;
            public Vector3 Direction;
            public Vector3 SpecularColor;
        }
    }
}
