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
    public class NNS_OBJECT
    {
        public readonly AppMain.NNS_VECTOR Center = new AppMain.NNS_VECTOR();
        public float Radius;
        public int nMaterial;
        public AppMain.NNS_MATERIALPTR[] pMatPtrList;
        public int nVtxList;
        public AppMain.NNS_VTXLISTPTR[] pVtxListPtrList;
        public int nPrimList;
        public AppMain.NNS_PRIMLISTPTR[] pPrimListPtrList;
        public int nNode;
        public int MaxNodeDepth;
        public AppMain.NNS_NODE[] pNodeList;
        public int nMtxPal;
        public int nSubobj;
        public AppMain.NNS_SUBOBJ[] pSubobjList;
        public int nTex;
        public uint fType;
        public int Version;
        public float BoundingBoxX;
        public float BoundingBoxY;
        public float BoundingBoxZ;

        public NNS_OBJECT()
        {
        }

        public NNS_OBJECT(AppMain.NNS_OBJECT nnsObject)
        {
            this.Center.Assign(nnsObject.Center);
            this.Radius = nnsObject.Radius;
            this.nMaterial = nnsObject.nMaterial;
            this.pMatPtrList = nnsObject.pMatPtrList;
            this.nVtxList = nnsObject.nVtxList;
            this.pVtxListPtrList = nnsObject.pVtxListPtrList;
            this.nPrimList = nnsObject.nPrimList;
            this.pPrimListPtrList = nnsObject.pPrimListPtrList;
            this.nNode = nnsObject.nNode;
            this.MaxNodeDepth = nnsObject.MaxNodeDepth;
            this.pNodeList = nnsObject.pNodeList;
            this.nMtxPal = nnsObject.nMtxPal;
            this.nSubobj = nnsObject.nSubobj;
            this.pSubobjList = nnsObject.pSubobjList;
            this.nTex = nnsObject.nTex;
            this.fType = nnsObject.fType;
            this.Version = nnsObject.Version;
            this.BoundingBoxX = nnsObject.BoundingBoxX;
            this.BoundingBoxY = nnsObject.BoundingBoxY;
            this.BoundingBoxZ = nnsObject.BoundingBoxZ;
        }

        public AppMain.NNS_OBJECT Assign(AppMain.NNS_OBJECT nnsObject)
        {
            if (this != nnsObject)
            {
                this.Center.Assign(nnsObject.Center);
                this.Radius = nnsObject.Radius;
                this.nMaterial = nnsObject.nMaterial;
                this.pMatPtrList = nnsObject.pMatPtrList;
                this.nVtxList = nnsObject.nVtxList;
                this.pVtxListPtrList = nnsObject.pVtxListPtrList;
                this.nPrimList = nnsObject.nPrimList;
                this.pPrimListPtrList = nnsObject.pPrimListPtrList;
                this.nNode = nnsObject.nNode;
                this.MaxNodeDepth = nnsObject.MaxNodeDepth;
                this.pNodeList = nnsObject.pNodeList;
                this.nMtxPal = nnsObject.nMtxPal;
                this.nSubobj = nnsObject.nSubobj;
                this.pSubobjList = nnsObject.pSubobjList;
                this.nTex = nnsObject.nTex;
                this.fType = nnsObject.fType;
                this.Version = nnsObject.Version;
                this.BoundingBoxX = nnsObject.BoundingBoxX;
                this.BoundingBoxY = nnsObject.BoundingBoxY;
                this.BoundingBoxZ = nnsObject.BoundingBoxZ;
            }
            return this;
        }

        public static AppMain.NNS_OBJECT Read(BinaryReader reader, long data0Pos)
        {
            AppMain.NNS_OBJECT nnsObject = new AppMain.NNS_OBJECT();
            nnsObject.Center.x = reader.ReadSingle();
            nnsObject.Center.y = reader.ReadSingle();
            nnsObject.Center.z = reader.ReadSingle();
            nnsObject.Radius = reader.ReadSingle();
            nnsObject.nMaterial = reader.ReadInt32();
            uint num1 = reader.ReadUInt32();
            if (num1 != 0U)
            {
                bool flag = false;
                nnsObject.pMatPtrList = new AppMain.NNS_MATERIALPTR[nnsObject.nMaterial];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num1, SeekOrigin.Begin);
                for (int index = 0; index < nnsObject.nMaterial; ++index)
                {
                    bool transparentMaterial;
                    nnsObject.pMatPtrList[index] = AppMain.NNS_MATERIALPTR.Read(reader, data0Pos, out transparentMaterial);
                    flag |= transparentMaterial;
                }
                if (flag)
                {
                    for (int index = 0; index < nnsObject.nMaterial; ++index)
                        ((AppMain.NNS_MATERIAL_GLES11_DESC)nnsObject.pMatPtrList[index].pMaterial).fFlag |= 1U;
                }
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsObject.nVtxList = reader.ReadInt32();
            uint num2 = reader.ReadUInt32();
            if (num2 != 0U)
            {
                nnsObject.pVtxListPtrList = new AppMain.NNS_VTXLISTPTR[nnsObject.nVtxList];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num2, SeekOrigin.Begin);
                for (int index = 0; index < nnsObject.nVtxList; ++index)
                    nnsObject.pVtxListPtrList[index] = AppMain.NNS_VTXLISTPTR.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsObject.nPrimList = reader.ReadInt32();
            uint num3 = reader.ReadUInt32();
            if (num3 != 0U)
            {
                nnsObject.pPrimListPtrList = new AppMain.NNS_PRIMLISTPTR[nnsObject.nPrimList];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num3, SeekOrigin.Begin);
                for (int index = 0; index < nnsObject.nPrimList; ++index)
                    nnsObject.pPrimListPtrList[index] = AppMain.NNS_PRIMLISTPTR.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsObject.nNode = reader.ReadInt32();
            nnsObject.MaxNodeDepth = reader.ReadInt32();
            uint num4 = reader.ReadUInt32();
            if (num4 != 0U)
            {
                nnsObject.pNodeList = new AppMain.NNS_NODE[nnsObject.nNode];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num4, SeekOrigin.Begin);
                for (int index = 0; index < nnsObject.nNode; ++index)
                    nnsObject.pNodeList[index] = AppMain.NNS_NODE.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsObject.nMtxPal = reader.ReadInt32();
            nnsObject.nSubobj = reader.ReadInt32();
            uint num5 = reader.ReadUInt32();
            if (num5 != 0U)
            {
                nnsObject.pSubobjList = new AppMain.NNS_SUBOBJ[nnsObject.nSubobj];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num5, SeekOrigin.Begin);
                for (int index = 0; index < nnsObject.nSubobj; ++index)
                    nnsObject.pSubobjList[index] = AppMain.NNS_SUBOBJ.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsObject.nTex = reader.ReadInt32();
            nnsObject.fType = reader.ReadUInt32();
            nnsObject.Version = reader.ReadInt32();
            nnsObject.BoundingBoxX = reader.ReadSingle();
            nnsObject.BoundingBoxY = reader.ReadSingle();
            nnsObject.BoundingBoxZ = reader.ReadSingle();
            return nnsObject;
        }
    }
}
