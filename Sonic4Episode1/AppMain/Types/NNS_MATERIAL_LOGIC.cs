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
    public class NNS_MATERIAL_LOGIC
    {
        public uint fFlag;
        public ushort SrcFactorRGB;
        public ushort DstFactorRGB;
        public ushort SrcFactorA;
        public ushort DstFactorA;
        public AppMain.NNS_RGBA BlendColor;
        public ushort BlendOp;
        public ushort LogicOp;
        public ushort AlphaFunc;
        public ushort DepthFunc;
        public float AlphaRef;

        public AppMain.NNS_MATERIAL_LOGIC Assign(AppMain.NNS_MATERIAL_LOGIC matLogic)
        {
            this.fFlag = matLogic.fFlag;
            this.SrcFactorRGB = matLogic.SrcFactorRGB;
            this.DstFactorRGB = matLogic.DstFactorRGB;
            this.SrcFactorA = matLogic.SrcFactorA;
            this.DstFactorA = matLogic.DstFactorA;
            this.BlendColor = matLogic.BlendColor;
            this.BlendOp = matLogic.BlendOp;
            this.LogicOp = matLogic.LogicOp;
            this.AlphaFunc = matLogic.AlphaFunc;
            this.DepthFunc = matLogic.DepthFunc;
            this.AlphaRef = matLogic.AlphaRef;
            return this;
        }
    }
}
