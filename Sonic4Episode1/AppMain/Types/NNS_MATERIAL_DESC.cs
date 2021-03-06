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
    public class NNS_MATERIAL_DESC
    {
        public uint fFlag;
        public uint User;
        public AppMain.NNS_MATERIAL_COLOR pColor;
        public AppMain.NNS_MATERIAL_COLOR pBackColor;
        public AppMain.NNS_MATERIAL_LOGIC pLogic;
        public int nTex;
        public AppMain.NNS_MATERIAL_TEXMAP_DESC[] pTexDesc;

        public NNS_MATERIAL_DESC()
        {
        }

        public NNS_MATERIAL_DESC(AppMain.NNS_MATERIAL_DESC desc)
        {
            this.fFlag = desc.fFlag;
            this.User = desc.User;
            this.pColor = desc.pColor;
            this.pBackColor = desc.pBackColor;
            this.pLogic = desc.pLogic;
            this.nTex = desc.nTex;
            this.pTexDesc = desc.pTexDesc;
        }

        public AppMain.NNS_MATERIAL_DESC Assign(AppMain.NNS_MATERIAL_DESC desc)
        {
            this.fFlag = desc.fFlag;
            this.User = desc.User;
            this.pColor = desc.pColor;
            this.pBackColor = desc.pBackColor;
            this.pLogic = desc.pLogic;
            this.nTex = desc.nTex;
            this.pTexDesc = desc.pTexDesc;
            return this;
        }
    }
}
