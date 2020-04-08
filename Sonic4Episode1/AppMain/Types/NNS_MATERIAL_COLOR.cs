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
    public class NNS_MATERIAL_COLOR
    {
        public uint fFlag;
        public AppMain.NNS_RGBA Ambient;
        public AppMain.NNS_RGBA Diffuse;
        public AppMain.NNS_RGBA Specular;
        public AppMain.NNS_RGBA Emission;
        public float Shininess;
        public uint VtxColMaterial;

        public NNS_MATERIAL_COLOR()
        {
        }

        public NNS_MATERIAL_COLOR(AppMain.NNS_MATERIAL_COLOR matColor)
        {
            this.fFlag = matColor.fFlag;
            this.Ambient = matColor.Ambient;
            this.Diffuse = matColor.Diffuse;
            this.Specular = matColor.Specular;
            this.Emission = matColor.Emission;
            this.Shininess = matColor.Shininess;
            this.VtxColMaterial = matColor.VtxColMaterial;
        }

        public AppMain.NNS_MATERIAL_COLOR Assign(AppMain.NNS_MATERIAL_COLOR matColor)
        {
            this.fFlag = matColor.fFlag;
            this.Ambient = matColor.Ambient;
            this.Diffuse = matColor.Diffuse;
            this.Specular = matColor.Specular;
            this.Emission = matColor.Emission;
            this.Shininess = matColor.Shininess;
            this.VtxColMaterial = matColor.VtxColMaterial;
            return this;
        }

        public static explicit operator AppMain.NNS_MATERIAL_STDSHADER_COLOR(
          AppMain.NNS_MATERIAL_COLOR color)
        {
            return new AppMain.NNS_MATERIAL_STDSHADER_COLOR()
            {
                Ambient = color.Ambient,
                Diffuse = color.Diffuse,
                Emission = color.Emission,
                fFlag = color.fFlag,
                Shininess = color.Shininess,
                Specular = color.Specular,
                SpecularIntensity = MppBitConverter.UInt32ToSingle(color.VtxColMaterial)
            };
        }
    }
}
