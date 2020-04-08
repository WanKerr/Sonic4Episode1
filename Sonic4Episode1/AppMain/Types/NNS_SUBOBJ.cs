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
    public class NNS_SUBOBJ
    {
        public uint fType;
        public int nMeshset;
        public AppMain.NNS_MESHSET[] pMeshsetList;
        public int nTex;
        public int[] pTexNumList;

        public NNS_SUBOBJ()
        {
        }

        public NNS_SUBOBJ(AppMain.NNS_SUBOBJ subObj)
        {
            this.fType = subObj.fType;
            this.nMeshset = subObj.nMeshset;
            this.pMeshsetList = subObj.pMeshsetList;
            this.nTex = subObj.nTex;
            this.pTexNumList = subObj.pTexNumList;
        }

        public AppMain.NNS_SUBOBJ Assign(AppMain.NNS_SUBOBJ subObj)
        {
            if (this != subObj)
            {
                this.fType = subObj.fType;
                this.nMeshset = subObj.nMeshset;
                this.pMeshsetList = subObj.pMeshsetList;
                this.nTex = subObj.nTex;
                this.pTexNumList = subObj.pTexNumList;
            }
            return this;
        }

        public static AppMain.NNS_SUBOBJ Read(BinaryReader reader, long data0Pos)
        {
            AppMain.NNS_SUBOBJ nnsSubobj = new AppMain.NNS_SUBOBJ();
            nnsSubobj.fType = reader.ReadUInt32();
            nnsSubobj.nMeshset = reader.ReadInt32();
            uint num1 = reader.ReadUInt32();
            if (num1 != 0U)
            {
                nnsSubobj.pMeshsetList = new AppMain.NNS_MESHSET[nnsSubobj.nMeshset];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num1, SeekOrigin.Begin);
                for (int index = 0; index < nnsSubobj.nMeshset; ++index)
                    nnsSubobj.pMeshsetList[index] = AppMain.NNS_MESHSET.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsSubobj.nTex = reader.ReadInt32();
            uint num2 = reader.ReadUInt32();
            if (num2 != 0U)
            {
                nnsSubobj.pTexNumList = new int[nnsSubobj.nTex];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num2, SeekOrigin.Begin);
                for (int index = 0; index < nnsSubobj.nTex; ++index)
                    nnsSubobj.pTexNumList[index] = reader.ReadInt32();
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return nnsSubobj;
        }
    }
}
