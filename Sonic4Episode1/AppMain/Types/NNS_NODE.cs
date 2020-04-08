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
    public class NNS_NODE
    {
        public readonly AppMain.NNS_VECTOR Translation = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR Scaling = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_MATRIX InvInitMtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public readonly AppMain.NNS_VECTOR Center = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public uint fType;
        public short iMatrix;
        public short iParent;
        public short iChild;
        public short iSibling;
        public AppMain.NNS_ROTATE_A32 Rotation;
        public float Radius;
        public uint User;
        public float SIIKBoneLength;
        public float BoundingBoxY;
        public float BoundingBoxZ;

        public float BoundingBoxX
        {
            get
            {
                return this.SIIKBoneLength;
            }
            set
            {
                this.SIIKBoneLength = value;
            }
        }

        public NNS_NODE()
        {
        }

        public NNS_NODE(AppMain.NNS_NODE node)
        {
            this.fType = node.fType;
            this.iMatrix = node.iMatrix;
            this.iParent = node.iParent;
            this.iChild = node.iChild;
            this.iSibling = node.iSibling;
            this.Translation.Assign(node.Translation);
            this.Rotation = node.Rotation;
            this.Scaling.Assign(node.Scaling);
            this.InvInitMtx.Assign(node.InvInitMtx);
            this.Center.Assign(node.Center);
            this.Radius = node.Radius;
            this.User = node.User;
            this.SIIKBoneLength = node.SIIKBoneLength;
            this.BoundingBoxY = node.BoundingBoxY;
            this.BoundingBoxZ = node.BoundingBoxZ;
        }

        public AppMain.NNS_NODE Assign(AppMain.NNS_NODE node)
        {
            if (this != node)
            {
                this.fType = node.fType;
                this.iMatrix = node.iMatrix;
                this.iParent = node.iParent;
                this.iChild = node.iChild;
                this.iSibling = node.iSibling;
                this.Translation.Assign(node.Translation);
                this.Rotation = node.Rotation;
                this.Scaling.Assign(node.Scaling);
                this.InvInitMtx.Assign(node.InvInitMtx);
                this.Center.Assign(node.Center);
                this.Radius = node.Radius;
                this.User = node.User;
                this.SIIKBoneLength = node.SIIKBoneLength;
                this.BoundingBoxY = node.BoundingBoxY;
                this.BoundingBoxZ = node.BoundingBoxZ;
            }
            return this;
        }

        public static AppMain.NNS_NODE Read(BinaryReader reader, long data0Pos)
        {
            return new AppMain.NNS_NODE()
            {
                fType = reader.ReadUInt32(),
                iMatrix = reader.ReadInt16(),
                iParent = reader.ReadInt16(),
                iChild = reader.ReadInt16(),
                iSibling = reader.ReadInt16(),
                Translation = {
          x = reader.ReadSingle(),
          y = reader.ReadSingle(),
          z = reader.ReadSingle()
        },
                Rotation = {
          x = reader.ReadInt32(),
          y = reader.ReadInt32(),
          z = reader.ReadInt32()
        },
                Scaling = {
          x = reader.ReadSingle(),
          y = reader.ReadSingle(),
          z = reader.ReadSingle()
        },
                InvInitMtx = {
          M00 = reader.ReadSingle(),
          M10 = reader.ReadSingle(),
          M20 = reader.ReadSingle(),
          M30 = reader.ReadSingle(),
          M01 = reader.ReadSingle(),
          M11 = reader.ReadSingle(),
          M21 = reader.ReadSingle(),
          M31 = reader.ReadSingle(),
          M02 = reader.ReadSingle(),
          M12 = reader.ReadSingle(),
          M22 = reader.ReadSingle(),
          M32 = reader.ReadSingle(),
          M03 = reader.ReadSingle(),
          M13 = reader.ReadSingle(),
          M23 = reader.ReadSingle(),
          M33 = reader.ReadSingle()
        },
                Center = {
          x = reader.ReadSingle(),
          y = reader.ReadSingle(),
          z = reader.ReadSingle()
        },
                Radius = reader.ReadSingle(),
                User = reader.ReadUInt32(),
                BoundingBoxX = reader.ReadSingle(),
                BoundingBoxY = reader.ReadSingle(),
                BoundingBoxZ = reader.ReadSingle()
            };
        }
    }
}
