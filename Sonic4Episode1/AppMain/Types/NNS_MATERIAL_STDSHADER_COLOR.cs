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
    public class NNS_MATERIAL_STDSHADER_COLOR
    {
        public uint fFlag;
        public AppMain.NNS_RGBA Ambient;
        public AppMain.NNS_RGBA Diffuse;
        public AppMain.NNS_RGBA Specular;
        public AppMain.NNS_RGBA Emission;
        public float Shininess;
        public float SpecularIntensity;

        public static AppMain.NNS_MATERIAL_STDSHADER_COLOR Read(BinaryReader reader)
        {
            return new AppMain.NNS_MATERIAL_STDSHADER_COLOR()
            {
                fFlag = reader.ReadUInt32(),
                Ambient = {
          r = reader.ReadSingle(),
          g = reader.ReadSingle(),
          b = reader.ReadSingle(),
          a = reader.ReadSingle()
        },
                Diffuse = {
          r = reader.ReadSingle(),
          g = reader.ReadSingle(),
          b = reader.ReadSingle(),
          a = reader.ReadSingle()
        },
                Specular = {
          r = reader.ReadSingle(),
          g = reader.ReadSingle(),
          b = reader.ReadSingle(),
          a = reader.ReadSingle()
        },
                Emission = {
          r = reader.ReadSingle(),
          g = reader.ReadSingle(),
          b = reader.ReadSingle(),
          a = reader.ReadSingle()
        },
                Shininess = reader.ReadSingle(),
                SpecularIntensity = reader.ReadSingle()
            };
        }

        public NNS_MATERIAL_STDSHADER_COLOR()
        {
        }

        public NNS_MATERIAL_STDSHADER_COLOR(AppMain.NNS_MATERIAL_STDSHADER_COLOR matColor)
        {
            this.fFlag = matColor.fFlag;
            this.Ambient.r = matColor.Ambient.r;
            this.Ambient.g = matColor.Ambient.g;
            this.Ambient.b = matColor.Ambient.b;
            this.Ambient.a = matColor.Ambient.a;
            this.Diffuse.r = matColor.Diffuse.r;
            this.Diffuse.g = matColor.Diffuse.g;
            this.Diffuse.b = matColor.Diffuse.b;
            this.Diffuse.a = matColor.Diffuse.a;
            this.Specular.r = matColor.Specular.r;
            this.Specular.g = matColor.Specular.g;
            this.Specular.b = matColor.Specular.b;
            this.Specular.a = matColor.Specular.a;
            this.Emission.r = matColor.Emission.r;
            this.Emission.g = matColor.Emission.g;
            this.Emission.b = matColor.Emission.b;
            this.Emission.a = matColor.Emission.a;
            this.Shininess = matColor.Shininess;
            this.SpecularIntensity = matColor.SpecularIntensity;
        }

        public AppMain.NNS_MATERIAL_STDSHADER_COLOR Assign(
          AppMain.NNS_MATERIAL_STDSHADER_COLOR matColor)
        {
            this.fFlag = matColor.fFlag;
            this.Ambient.r = matColor.Ambient.r;
            this.Ambient.g = matColor.Ambient.g;
            this.Ambient.b = matColor.Ambient.b;
            this.Ambient.a = matColor.Ambient.a;
            this.Diffuse.r = matColor.Diffuse.r;
            this.Diffuse.g = matColor.Diffuse.g;
            this.Diffuse.b = matColor.Diffuse.b;
            this.Diffuse.a = matColor.Diffuse.a;
            this.Specular.r = matColor.Specular.r;
            this.Specular.g = matColor.Specular.g;
            this.Specular.b = matColor.Specular.b;
            this.Specular.a = matColor.Specular.a;
            this.Emission.r = matColor.Emission.r;
            this.Emission.g = matColor.Emission.g;
            this.Emission.b = matColor.Emission.b;
            this.Emission.a = matColor.Emission.a;
            this.Shininess = matColor.Shininess;
            this.SpecularIntensity = matColor.SpecularIntensity;
            return this;
        }
    }
}
