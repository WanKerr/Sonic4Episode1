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
    public class AMS_PARAM_DRAW_PRIMITIVE : AppMain.IClearable
    {
        public AppMain.NNS_MATRIX mtx;
        public int type;
        public AppMain.NNS_PRIM3D_PCT_ARRAY vtxPCT3D;
        public AppMain.NNS_PRIM3D_PC[] vtxPC3D;
        public AppMain.NNS_PRIM2D_PCT[] vtxPCT2D;
        public AppMain.NNS_PRIM2D_PC[] vtxPC2D;
        private int formatXD;
        public int count;
        public AppMain.NNS_TEXLIST texlist;
        public int texId;
        public int ablend;
        public float sortZ;
        public int bldSrc;
        public int bldDst;
        public int bldMode;
        public short aTest;
        public short zMask;
        public short zTest;
        public short noSort;
        public int uwrap;
        public int vwrap;

        public int format3D
        {
            get
            {
                return this.formatXD;
            }
            set
            {
                this.formatXD = value;
            }
        }

        public int format2D
        {
            get
            {
                return this.formatXD;
            }
            set
            {
                this.formatXD = value;
            }
        }

        public float zOffset
        {
            get
            {
                return this.sortZ;
            }
            set
            {
                this.sortZ = value;
            }
        }

        public void Assign(AppMain.AMS_PARAM_DRAW_PRIMITIVE other)
        {
            this.mtx = other.mtx;
            this.type = other.type;
            this.vtxPCT3D = other.vtxPCT3D;
            this.vtxPC3D = other.vtxPC3D;
            this.vtxPCT2D = other.vtxPCT2D;
            this.vtxPC2D = other.vtxPC2D;
            this.formatXD = other.formatXD;
            this.count = other.count;
            this.texlist = other.texlist;
            this.texId = other.texId;
            this.ablend = other.ablend;
            this.sortZ = other.sortZ;
            this.bldSrc = other.bldSrc;
            this.bldDst = other.bldDst;
            this.bldMode = other.bldMode;
            this.aTest = other.aTest;
            this.zMask = other.zMask;
            this.zTest = other.zTest;
            this.noSort = other.noSort;
            this.uwrap = other.vwrap;
            this.vwrap = other.vwrap;
        }

        public void Clear()
        {
            this.mtx = (AppMain.NNS_MATRIX)null;
            this.type = 0;
            this.vtxPCT3D = (AppMain.NNS_PRIM3D_PCT_ARRAY)null;
            this.vtxPC3D = (AppMain.NNS_PRIM3D_PC[])null;
            this.vtxPCT2D = (AppMain.NNS_PRIM2D_PCT[])null;
            this.vtxPC2D = (AppMain.NNS_PRIM2D_PC[])null;
            this.formatXD = 0;
            this.count = 0;
            this.texlist = (AppMain.NNS_TEXLIST)null;
            this.texId = 0;
            this.ablend = 0;
            this.sortZ = 0.0f;
            this.bldSrc = 0;
            this.bldDst = 0;
            this.bldMode = 0;
            this.aTest = (short)0;
            this.zMask = (short)0;
            this.zTest = (short)0;
            this.noSort = (short)0;
            this.uwrap = 0;
            this.vwrap = 0;
        }
    }
}
