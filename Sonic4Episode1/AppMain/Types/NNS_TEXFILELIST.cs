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
    public class NNS_TEXFILELIST
    {
        public int nTex;
        public AppMain.NNS_TEXFILE[] pTexFileList;

        public static AppMain.NNS_TEXFILELIST Read(BinaryReader reader, long data0Pos)
        {
            AppMain.NNS_TEXFILELIST nnsTexfilelist = new AppMain.NNS_TEXFILELIST()
            {
                nTex = reader.ReadInt32()
            };
            nnsTexfilelist.pTexFileList = new AppMain.NNS_TEXFILE[nnsTexfilelist.nTex];
            uint num = reader.ReadUInt32();
            reader.BaseStream.Seek(data0Pos + (long)num, SeekOrigin.Begin);
            for (int index = 0; index < nnsTexfilelist.nTex; ++index)
                nnsTexfilelist.pTexFileList[index] = AppMain.NNS_TEXFILE.Read(reader, data0Pos);
            return nnsTexfilelist;
        }
    }
}
