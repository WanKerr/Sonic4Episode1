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
    public class NNS_MATERIAL_GLES11_LOGIC
    {
        public uint fFlag;
        public ushort SrcFactor;
        public ushort DstFactor;
        public ushort BlendOp;
        public ushort LogicOp;
        public ushort AlphaFunc;
        public ushort DepthFunc;
        public float AlphaRef;

        public static AppMain.NNS_MATERIAL_GLES11_LOGIC Read(BinaryReader reader)
        {
            return new AppMain.NNS_MATERIAL_GLES11_LOGIC()
            {
                fFlag = reader.ReadUInt32(),
                SrcFactor = reader.ReadUInt16(),
                DstFactor = reader.ReadUInt16(),
                BlendOp = reader.ReadUInt16(),
                LogicOp = reader.ReadUInt16(),
                AlphaFunc = reader.ReadUInt16(),
                DepthFunc = reader.ReadUInt16(),
                AlphaRef = reader.ReadSingle()
            };
        }

        public AppMain.NNS_MATERIAL_GLES11_LOGIC Assign(AppMain.NNS_MATERIAL_GLES11_LOGIC logic)
        {
            this.fFlag = logic.fFlag;
            this.SrcFactor = logic.SrcFactor;
            this.DstFactor = logic.DstFactor;
            this.BlendOp = logic.BlendOp;
            this.LogicOp = logic.LogicOp;
            this.AlphaFunc = logic.AlphaFunc;
            this.DepthFunc = logic.DepthFunc;
            this.AlphaRef = logic.AlphaRef;
            return this;
        }
    }
}
