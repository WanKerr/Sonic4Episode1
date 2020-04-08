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
    public class NNS_MATERIAL_GLES11_DESC
    {
        public uint fFlag;
        public uint User;
        public AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor;
        public AppMain.NNS_MATERIAL_GLES11_LOGIC pLogic;
        public int nTex;
        public AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[] pTexDesc;

        public static AppMain.NNS_MATERIAL_GLES11_DESC Read(
          BinaryReader reader,
          long data0Pos,
          out bool transparentMaterial)
        {
            transparentMaterial = false;
            AppMain.NNS_MATERIAL_GLES11_DESC materialGleS11Desc = new AppMain.NNS_MATERIAL_GLES11_DESC();
            materialGleS11Desc.fFlag = reader.ReadUInt32();
            materialGleS11Desc.User = reader.ReadUInt32();
            uint num1 = reader.ReadUInt32();
            if (num1 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num1, SeekOrigin.Begin);
                materialGleS11Desc.pColor = AppMain.NNS_MATERIAL_STDSHADER_COLOR.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
                transparentMaterial = 1.0 != (double)materialGleS11Desc.pColor.Diffuse.a;
            }
            uint num2 = reader.ReadUInt32();
            if (num2 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num2, SeekOrigin.Begin);
                materialGleS11Desc.pLogic = AppMain.NNS_MATERIAL_GLES11_LOGIC.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            materialGleS11Desc.nTex = reader.ReadInt32();
            uint num3 = reader.ReadUInt32();
            if (num3 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num3, SeekOrigin.Begin);
                materialGleS11Desc.pTexDesc = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[materialGleS11Desc.nTex];
                for (int index = 0; index < materialGleS11Desc.nTex; ++index)
                    materialGleS11Desc.pTexDesc[index] = AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return materialGleS11Desc;
        }

        public NNS_MATERIAL_GLES11_DESC()
        {
        }

        public NNS_MATERIAL_GLES11_DESC(AppMain.NNS_MATERIAL_GLES11_DESC desc)
        {
            this.fFlag = desc.fFlag;
            this.User = desc.User;
            this.pColor = desc.pColor;
            this.pLogic = desc.pLogic;
            this.nTex = desc.nTex;
            this.pTexDesc = desc.pTexDesc;
        }

        public AppMain.NNS_MATERIAL_GLES11_DESC Assign(AppMain.NNS_MATERIAL_GLES11_DESC desc)
        {
            this.fFlag = desc.fFlag;
            this.User = desc.User;
            this.pColor = desc.pColor;
            this.pLogic = desc.pLogic;
            this.nTex = desc.nTex;
            this.pTexDesc = desc.pTexDesc;
            return this;
        }
    }
}
