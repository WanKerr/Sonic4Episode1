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
    public class NNS_DRAWCALLBACK_VAL
    {
        public int iMaterial;
        public int iPrevMaterial;
        public int iVtxList;
        public int iPrevVtxList;
        public int iNode;
        public int iMeshset;
        public int iSubobject;
        public AppMain.NNS_MATERIALPTR pMaterial;
        public AppMain.NNS_VTXLISTPTR pVtxListPtr;
        public AppMain.NNS_OBJECT pObject;
        public AppMain.NNS_MATRIX[] pMatrixPalette;
        public uint[] pNodeStatusList;
        public uint DrawSubobjType;
        public uint DrawFlag;
        public int bModified;
        public int bReDraw;
    }
}
