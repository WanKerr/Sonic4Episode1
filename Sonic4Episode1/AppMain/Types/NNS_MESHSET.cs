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
    public class NNS_MESHSET
    {
        public readonly AppMain.NNS_VECTOR Center = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public float Radius;
        public int iNode;
        public int iMatrix;
        public int iMaterial;
        public int iVtxList;
        public int iPrimList;
        public uint Reserved2;
        public uint Reserved1;
        public uint Reserved0;

        public NNS_MESHSET()
        {
        }

        public NNS_MESHSET(AppMain.NNS_MESHSET meshSet)
        {
            this.Center.Assign(meshSet.Center);
            this.Radius = meshSet.Radius;
            this.iNode = meshSet.iNode;
            this.iMatrix = meshSet.iMatrix;
            this.iMaterial = meshSet.iMaterial;
            this.iVtxList = meshSet.iVtxList;
            this.iPrimList = meshSet.iPrimList;
            this.Reserved2 = meshSet.Reserved2;
            this.Reserved1 = meshSet.Reserved1;
            this.Reserved0 = meshSet.Reserved0;
        }

        public AppMain.NNS_MESHSET Assign(AppMain.NNS_MESHSET meshSet)
        {
            if (this != meshSet)
            {
                this.Center.Assign(meshSet.Center);
                this.Radius = meshSet.Radius;
                this.iNode = meshSet.iNode;
                this.iMatrix = meshSet.iMatrix;
                this.iMaterial = meshSet.iMaterial;
                this.iVtxList = meshSet.iVtxList;
                this.iPrimList = meshSet.iPrimList;
                this.Reserved2 = meshSet.Reserved2;
                this.Reserved1 = meshSet.Reserved1;
                this.Reserved0 = meshSet.Reserved0;
            }
            return this;
        }

        public static AppMain.NNS_MESHSET Read(BinaryReader reader)
        {
            return new AppMain.NNS_MESHSET()
            {
                Center = {
          x = reader.ReadSingle(),
          y = reader.ReadSingle(),
          z = reader.ReadSingle()
        },
                Radius = reader.ReadSingle(),
                iNode = reader.ReadInt32(),
                iMatrix = reader.ReadInt32(),
                iMaterial = reader.ReadInt32(),
                iVtxList = reader.ReadInt32(),
                iPrimList = reader.ReadInt32(),
                Reserved2 = reader.ReadUInt32(),
                Reserved1 = reader.ReadUInt32(),
                Reserved0 = reader.ReadUInt32()
            };
        }
    }
}
