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
    public class NNS_VTXLISTPTR
    {
        public uint fType;
        public object pVtxList;

        public NNS_VTXLISTPTR()
        {
        }

        public NNS_VTXLISTPTR(AppMain.NNS_VTXLISTPTR vtxListPtr)
        {
            this.fType = vtxListPtr.fType;
            this.pVtxList = vtxListPtr.pVtxList;
        }

        public AppMain.NNS_VTXLISTPTR Assign(AppMain.NNS_VTXLISTPTR vtxListPtr)
        {
            this.fType = vtxListPtr.fType;
            this.pVtxList = vtxListPtr.pVtxList;
            return this;
        }

        public static AppMain.NNS_VTXLISTPTR Read(BinaryReader reader, long data0Pos)
        {
            AppMain.NNS_VTXLISTPTR nnsVtxlistptr = new AppMain.NNS_VTXLISTPTR();
            nnsVtxlistptr.fType = reader.ReadUInt32();
            uint num = reader.ReadUInt32();
            if (num != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num, SeekOrigin.Begin);
                nnsVtxlistptr.pVtxList = (object)AppMain.NNS_VTXLIST_GL_DESC.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return nnsVtxlistptr;
        }
    }
}
