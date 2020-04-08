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
    public class NNS_MATERIAL_STDSHADER_TEXMAP_DESC
    {
        public AppMain.NNS_RGBA? pBorderColor = new AppMain.NNS_RGBA?();
        public uint fType;
        public int iTexIdx;
        public int TexCoord;
        public float Blend;
        public AppMain.NNS_TEXCOORD Offset;
        public AppMain.NNS_TEXCOORD Scale;
        public int WrapS;
        public int WrapT;
        public AppMain.NNS_TEXTURE_FILTERMODE pFilterMode;
        public AppMain.NNS_TEXTURE_LOD_PARAM pLODParam;
        public object pTexInfo;
        public uint Reserved1;
        public uint Reserved0;

        public void Assign(AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC pPtr)
        {
            AppMain.mppAssertNotImpl();
        }

        public NNS_MATERIAL_STDSHADER_TEXMAP_DESC()
        {
        }

        public NNS_MATERIAL_STDSHADER_TEXMAP_DESC(AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC desc)
        {
            this.fType = desc.fType;
            this.iTexIdx = desc.iTexIdx;
            this.TexCoord = desc.TexCoord;
            this.Blend = desc.Blend;
            this.Offset = desc.Offset;
            this.Scale = desc.Scale;
            this.WrapS = desc.WrapS;
            this.WrapT = desc.WrapT;
            this.pBorderColor = desc.pBorderColor;
            this.pFilterMode = desc.pFilterMode;
            this.pLODParam = desc.pLODParam;
            this.pTexInfo = desc.pTexInfo;
            this.Reserved1 = desc.Reserved1;
            this.Reserved0 = desc.Reserved0;
        }
    }
}
