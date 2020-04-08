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
    public struct NNS_MATERIAL_GLES11_TEXMAP_DESC
    {
        public uint fType;
        public int iTexIdx;
        public int EnvMode;
        public AppMain.NNS_TEXTURE_GLES11_COMBINE pCombine;
        public AppMain.NNS_TEXCOORD Offset;
        public AppMain.NNS_TEXCOORD Scale;
        public int WrapS;
        public int WrapT;
        public AppMain.NNS_TEXTURE_FILTERMODE pFilterMode;
        public float LODBias;
        public object pTexInfo;

        public void Assign(ref AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC pPtr)
        {
            this.fType = pPtr.fType;
            this.iTexIdx = pPtr.iTexIdx;
            this.EnvMode = pPtr.EnvMode;
            this.pCombine = pPtr.pCombine;
            this.Offset = pPtr.Offset;
            this.Scale = pPtr.Scale;
            this.WrapS = pPtr.WrapS;
            this.WrapT = pPtr.WrapT;
            this.pFilterMode = pPtr.pFilterMode;
            this.LODBias = pPtr.LODBias;
            this.pTexInfo = pPtr.pTexInfo;
        }

        public NNS_MATERIAL_GLES11_TEXMAP_DESC(ref AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC desc)
        {
            this.fType = desc.fType;
            this.iTexIdx = desc.iTexIdx;
            this.EnvMode = desc.EnvMode;
            this.pCombine = desc.pCombine;
            this.Offset = desc.Offset;
            this.Scale = desc.Scale;
            this.WrapS = desc.WrapS;
            this.WrapT = desc.WrapT;
            this.pFilterMode = desc.pFilterMode;
            this.LODBias = desc.LODBias;
            this.pTexInfo = desc.pTexInfo;
        }

        public static AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC Read(
          BinaryReader reader,
          long data0Pos)
        {
            AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC gleS11TexmapDesc = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC();
            gleS11TexmapDesc.fType = reader.ReadUInt32();
            gleS11TexmapDesc.iTexIdx = reader.ReadInt32();
            gleS11TexmapDesc.EnvMode = reader.ReadInt32();
            uint num1 = reader.ReadUInt32();
            if (num1 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num1, SeekOrigin.Begin);
                gleS11TexmapDesc.pCombine = AppMain.NNS_TEXTURE_GLES11_COMBINE.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            gleS11TexmapDesc.Offset.u = reader.ReadSingle();
            gleS11TexmapDesc.Offset.v = reader.ReadSingle();
            gleS11TexmapDesc.Scale.u = reader.ReadSingle();
            gleS11TexmapDesc.Scale.v = reader.ReadSingle();
            gleS11TexmapDesc.WrapS = reader.ReadInt32();
            gleS11TexmapDesc.WrapT = reader.ReadInt32();
            uint num2 = reader.ReadUInt32();
            if (num2 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num2, SeekOrigin.Begin);
                gleS11TexmapDesc.pFilterMode = AppMain.NNS_TEXTURE_FILTERMODE.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            gleS11TexmapDesc.LODBias = reader.ReadSingle();
            uint num3 = reader.ReadUInt32();
            if (num3 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num3, SeekOrigin.Begin);
                gleS11TexmapDesc.pTexInfo = (object)AppMain.NNS_TEXINFO.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return gleS11TexmapDesc;
        }
    }
}
