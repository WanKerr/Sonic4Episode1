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
    public class NNS_MATERIALPTR
    {
        public uint fType;
        public object pMaterial;

        public NNS_MATERIALPTR()
        {
        }

        public NNS_MATERIALPTR(AppMain.NNS_MATERIALPTR materialPtr)
        {
            this.fType = materialPtr.fType;
            this.pMaterial = materialPtr.pMaterial;
        }

        public AppMain.NNS_MATERIALPTR Assign(AppMain.NNS_MATERIALPTR materialPtr)
        {
            this.fType = materialPtr.fType;
            this.pMaterial = materialPtr.pMaterial;
            return this;
        }

        public static AppMain.NNS_MATERIALPTR Read(
          BinaryReader reader,
          long data0Pos,
          out bool transparentMaterial)
        {
            transparentMaterial = false;
            AppMain.NNS_MATERIALPTR nnsMaterialptr = new AppMain.NNS_MATERIALPTR();
            nnsMaterialptr.fType = reader.ReadUInt32();
            uint num = reader.ReadUInt32();
            if (num != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num, SeekOrigin.Begin);
                nnsMaterialptr.pMaterial = (object)AppMain.NNS_MATERIAL_GLES11_DESC.Read(reader, data0Pos, out transparentMaterial);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return nnsMaterialptr;
        }
    }
}
